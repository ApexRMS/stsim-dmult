'*********************************************************************************************
' Dynamic Multipliers: A SyncroSim module of dynamic multipliers for ST-Sim
'
' Copyright © 2007-2017 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'*********************************************************************************************

Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports SyncroSim.Core
Imports SyncroSim.STSim
Imports SyncroSim.StochasticTime

<ObfuscationAttribute(Exclude:=True, ApplyToMembers:=False)>
Class DynMultTransformer
    Inherits Transformer

    Private m_IsEnabled As Boolean
    Private m_frequency As Integer
    Private m_scriptName As String
    Private m_StateAttrId As Integer
    Private m_DynMultRasters As New Collection
    Private m_STSimTransformer As STSimTransformer
    Private m_HandlersAdded As Boolean

    Protected ReadOnly Property STSimTransformer As STSimTransformer
        Get
            Return Me.m_STSimTransformer
        End Get
    End Property

    Public Property Enabled() As Boolean
        Get
            Return m_IsEnabled
        End Get
        Set(value As Boolean)
            m_IsEnabled = value
        End Set
    End Property

    Public Property Frequency() As Integer
        Get
            Return m_frequency
        End Get
        Set(ByVal value As Integer)
            m_frequency = value
        End Set
    End Property

    Public Property ScriptName() As String
        Get
            Return m_scriptName
        End Get
        Set(ByVal value As String)
            m_scriptName = value
        End Set
    End Property

    Public Property StateAttrId() As Integer
        Get
            Return m_StateAttrId
        End Get
        Set(ByVal value As Integer)
            m_StateAttrId = value
        End Set
    End Property

    Public Property DynMultRasters() As Collection
        Get
            Return m_DynMultRasters
        End Get
        Set(ByVal value As Collection)
            m_DynMultRasters = value
        End Set
    End Property

    Public Overrides Sub Transform()

        Me.m_STSimTransformer = Me.GetSTSimTransformer()
        Me.InitializeProperties()

        If Not Me.Enabled() Then
            Exit Sub
        End If

        If Not Me.IsSpatialRun() Then
            Exit Sub
        End If

        If Not CheckDynHabSuitSettings() Then
            ClearDHSMultEnabled()
            Return
        End If

        AddHandler Me.STSimTransformer.ExternalMultipliersRequested, AddressOf Me.OnSTSimExternalMultipliersRequested
        AddHandler Me.STSimTransformer.ApplyingProbabilisticTransitionsRaster, AddressOf OnBeforeApplyProbabilisticTransitionsRaster

        Me.m_HandlersAdded = True

    End Sub

    ''' <summary>
    ''' Overrides Dispose
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub Dispose(disposing As Boolean)

        If (disposing And Not Me.IsDisposed()) Then

            If (Me.m_HandlersAdded) Then

                ' DEVNOTE: We have to check here as we can trust STSimTransformer being initialized in OnTransform
                If Me.Frequency > Me.STSimTransformer.MaximumTimestep Then
                    Me.RecordStatus(StatusType.Information, "Dynamic Habitat Suitablity Multipliers Timestep value greater than Maximum Timestep.")
                End If

                RemoveHandler Me.STSimTransformer.ExternalMultipliersRequested, AddressOf OnSTSimExternalMultipliersRequested
                RemoveHandler Me.STSimTransformer.ApplyingProbabilisticTransitionsRaster, AddressOf OnBeforeApplyProbabilisticTransitionsRaster

            End If

        End If

        MyBase.Dispose(disposing)

    End Sub

    ''' <summary>
    ''' Event handler for External Multipliers Requested.  Applies a multiplier for the specified cell and transition group
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OnSTSimExternalMultipliersRequested(sender As Object, e As ExternalMultipliersEventArgs)

        'Debug.Print("DHS.OnSTSimExternalMultipliersRequested:" & e.Timestep & ", CellId:" & e.CellId)

        If Me.DynMultRasters.Contains(e.TransitionGroupId.ToString(CultureInfo.InvariantCulture)) Then
            Dim rast As StochasticTimeRaster = CType(Me.DynMultRasters.Item(e.TransitionGroupId.ToString(CultureInfo.InvariantCulture)), StochasticTimeRaster)

            Dim val As Double = rast.DblCells(e.CellId)
            If Math.Abs(val - rast.NoDataValue) < Double.Epsilon Then
                val = 1.0
            End If
            e.ApplyMultiplier(val)
        Else
            ' We could get here if the script didnt properly produce the dhsm tifs for import.
            e.ApplyMultiplier(1.0)
        End If

    End Sub

    Private Sub OnBeforeApplyProbabilisticTransitionsRaster(sender As Object, e As ApplyProbabilisticTransitionsRasterEventArgs)

        Dim sMsg As String

        Debug.Print("DHS.OnBeforeApplyProbabilisticTransitionsRaster: Iteration {0}/Timestep {1}", e.Iteration, e.Timestep)

        ' Check Timestep
        If Me.IsOutputTimestep(e.Timestep, Me.Frequency, True) Then

            Dim ds As DataSheet = Me.Scenario.GetDataSheet(DATAFEED_DHSM_NAME)
            Dim spatialOutputPath As String = RasterFiles.GetOutputFolder(ds, True)

            ' We'll use a DHSM subdirectory under spatial output for all our processing - basically a temp
            ' directory that will leave some processing clues, and is multiprocessing friendly becuase of its location
            Dim dhmsOutputPath As String = Path.Combine(Path.GetDirectoryName(spatialOutputPath), DHSM_FOLDER_NAME)
            If Not IO.Directory.Exists(dhmsOutputPath) Then
                IO.Directory.CreateDirectory(dhmsOutputPath)
            End If

            ' Create the Transition Group CSV file that will be used by the external script.
            Dim tgFilename = Path.Combine(dhmsOutputPath, DHSM_TRANSITION_GROUP_CSV_FILENAME)
            If Not IO.File.Exists(tgFilename) Then
                Me.ExportTGtoCSV(tgFilename)
            End If


            ' App is any command line exeutable file type: py, R, bat, shell
            Dim app As String = Me.ScriptName

            ' Figure out the name of the State Attr File that may have been generated using normal Spatial Output mechansim. Note
            ' that its "last" timesteps values, so use timestep -1 when figuring out its name
            Dim saFilename As String
            If e.Timestep = Me.STSimTransformer.MinimumTimestep Then
                saFilename = CalcStateAttrOutputRasterFileName(0, 0, StateAttrId.ToString(CultureInfo.InvariantCulture))
            Else
                saFilename = CalcStateAttrOutputRasterFileName(e.Iteration, e.Timestep - 1, StateAttrId.ToString(CultureInfo.InvariantCulture))
            End If
            saFilename = saFilename & ".tif"
            saFilename = Path.Combine(spatialOutputPath, saFilename)

            Dim dhsmSAFilename As String = Path.Combine(dhmsOutputPath, DHSM_SA_FILENAME)

            ' See if there already a SA file in the DHSM directory. It's R/O ( all raster as created thus), so we need to set R/W before we can overwite it
            If IO.File.Exists(dhsmSAFilename) Then
                File.SetAttributes(dhsmSAFilename, FileAttributes.Normal)
                File.Delete(dhsmSAFilename)
            End If

            ' Check that this SA raster file exists, a product of "normal"  SA spatial output setting
            If Not IO.File.Exists(saFilename) Then
                ' Didnt find, so lets go make it. Make sure its in the DHSM subdirectory
                If e.Timestep = Me.STSimTransformer.MinimumTimestep Then
                    CreateDHSMultRasterStateAttributeOutput(0, 0, Me.StateAttrId, dhsmSAFilename)
                Else
                    CreateDHSMultRasterStateAttributeOutput(e.Iteration, e.Timestep - 1, Me.StateAttrId, dhsmSAFilename)
                End If
            Else
                ' Copy the "standard" SA file into our DHSM subdirectory
                File.Copy(saFilename, dhsmSAFilename)
            End If


            ' Command line args - wrap in quotes to deal with spaces in path/filename
            Dim args As String = """" & Path.Combine(dhmsOutputPath, dhsmSAFilename) & """"

            Try
                ExternalScript.ProcessCmd(app, args)
                '                sMsg = String.Format(CultureInfo.InvariantCulture, "Executed external script file '{0}'.", app)
                '                Me.AddStatusRecord(StatusRecordType.Information, sMsg)

            Catch ex As System.ComponentModel.Win32Exception
                sMsg = String.Format(CultureInfo.InvariantCulture, "Could not find external script file '{0}'.", app)
                Me.RecordStatus(StatusType.Warning, sMsg)
                Exit Sub
            Catch ex As Exception
                sMsg = String.Format(CultureInfo.InvariantCulture, "Error executing external script file '{0}'.", app)
                Me.RecordStatus(StatusType.Warning, sMsg)
                Exit Sub
            End Try

            ' Now its time to load up the new Dynamic Habitat Suitablity Multiplication rasters created by the script
            Me.DynMultRasters.Clear()
            ' Loop thru transition groups
            For Each tg In Me.STSimTransformer.TransitionGroups
                Debug.Print("DHSM: Importing for TransitionGroup:{0}", tg.TransitionGroupId)
                Dim dhsmFilename As String = String.Format(CultureInfo.InvariantCulture, DHSM_DHSM_TG_FILENAME, tg.TransitionGroupId)
                dhsmFilename = Path.Combine(dhmsOutputPath, dhsmFilename)
                If IO.File.Exists(dhsmFilename) Then
                    Dim rastDhsm As New StochasticTimeRaster
                    RasterFiles.LoadRasterFile(dhsmFilename, rastDhsm, RasterDataType.DTDouble)

                    ' Check for rows and columns match
                    If rastDhsm.NumberCols <> Me.STSimTransformer.InputRasters.NumberColumns Or
                            rastDhsm.NumberRows <> Me.STSimTransformer.InputRasters.NumberRows Then
                        sMsg = String.Format(CultureInfo.InvariantCulture, "DHSM: The number of row and/or columns of the imported Dynamic Habitat Suitability Multiplier file '{0}' did not match that expected.", dhsmFilename)
                        Me.RecordStatus(StatusType.Warning, sMsg)
                    Else
                        DynMultRasters.Add(rastDhsm, tg.TransitionGroupId.ToString(CultureInfo.InvariantCulture))
                    End If
                Else
                    ' We dont neccessarily expect a Dynamic Habitat Suitablity Multiplication rasters per Transition Group, so don't get exited or log anything   
                End If
            Next

            If DynMultRasters.Count = 0 Then
                sMsg = String.Format(CultureInfo.InvariantCulture, "Did not find any Dynamic Habitat Suitability Multiplier raster files in '{0}'.", dhmsOutputPath)
                Me.RecordStatus(StatusType.Warning, sMsg)
            End If

        End If

    End Sub

    Public Function CheckDynHabSuitSettings() As Boolean

        If Me.Frequency = 0 Then
            Me.RecordStatus(StatusType.Warning, "Dynamic Habitat Suitablity Multipliers Timestep value not specified. Feature not performed.")
            Return False
        End If

        If Me.StateAttrId = 0 Then
            Me.RecordStatus(StatusType.Warning, "Dynamic Habitat Suitablity Multipliers State Attribute not specified. Feature not performed.")
            Return False
        End If

        If String.IsNullOrEmpty(ScriptName) Then
            Me.RecordStatus(StatusType.Warning, "Dynamic Habitat Suitablity Multipliers Script name not specified. Feature not performed.")
            Return False
        End If

        If Not IO.File.Exists(Me.ScriptName) Then
            Dim sMsg As String = String.Format(CultureInfo.InvariantCulture, "Dynamic Habitat Suitablity Multipliers Script '{0}' does not exist. Feature not performed.", Me.ScriptName)
            Me.RecordStatus(StatusType.Warning, sMsg)
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' Initialize the Class properties based on the Datasheet values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeProperties()

        Dim dr As DataRow = Me.Scenario.GetDataSheet(DATAFEED_DHSM_NAME).GetDataRow()

        If dr Is Nothing Then
            Exit Sub
        End If

        Me.Enabled = DataTableUtilities.GetDataBool(dr(DATASHEET_DHSM_ENABLED_COLUMN_NAME))
        Me.Frequency = DataTableUtilities.GetDataInt(dr(DATASHEET_DHSM_FREQUENCY_COLUMN_NAME))
        Me.ScriptName = DataTableUtilities.GetDataStr(dr(DATASHEET_DHSM_SCRIPT_COLUMN_NAME))
        Me.StateAttrId = DataTableUtilities.GetDataInt(dr(DATASHEET_DHSM_STATE_ATTRIBUTE_TYPE_COLUMN_NAME))

    End Sub

    Private Function CalcStateAttrOutputRasterFileName(iteration As Integer, timestep As Integer, attributeId As String) As String

        '        Name template = Itx-Tsy-sa-z.tif
        Dim fileName As String = String.Format(CultureInfo.InvariantCulture, "It{0}-Ts{1}-{2}-{3}",
                         iteration.ToString("0000", CultureInfo.InvariantCulture),
                         timestep.ToString("0000", CultureInfo.InvariantCulture),
                         SPATIAL_MAP_STATE_ATTRIBUTE_VARIABLE_PREFIX, attributeId)

        fileName = RasterFiles.SanitizeFileName(fileName)
        Return fileName

    End Function

End Class

'*********************************************************************************************
' Dynamic Multipliers: A SyncroSim module of dynamic multipliers for ST-Sim
'
' Copyright © 2007-2017 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'*********************************************************************************************

Imports System.IO
Imports SyncroSim.STSim
Imports SyncroSim.StochasticTime

Partial Class DynMultTransformer

    ''' <summary>
    ''' Determines whether or not the specified timestep is an Output timestep
    ''' </summary>
    ''' <param name="timestep">The timestep to test</param>
    ''' <param name="frequency">The frequency for timestep output</param>
    ''' <param name="shouldCreateOutput">Whether or not to create output</param>
    ''' <returns>
    ''' True if the timestep is the first timestep, the last timestep, or the timestep is within the frequency specified by the user.  
    ''' False will be returned if the user has not specified that this type of output should be generated or if the conditions for True are not met.
    ''' </returns>
    ''' <remarks>
    ''' The frequency of the timestep corresponds to the values that the user has specified for timestep output.  For example, someone
    ''' might specifiy that they only want data every 5 timesteps.  In this case, the frequency will be 5.
    ''' </remarks>
    Private Function IsOutputTimestep(ByVal timestep As Integer, ByVal frequency As Integer, ByVal shouldCreateOutput As Boolean) As Boolean

        If (shouldCreateOutput) Then

            If (timestep = Me.STSimTransformer.MinimumTimestep) Then
                Return True
            End If

            If (((timestep - Me.m_STSimTransformer.TimestepZero) Mod frequency) = 0) Then
                Return True
            End If

        End If

        Return False

    End Function

    ''' <summary>
    ''' Process Raster State Attribute Output
    ''' </summary>
    ''' <param name="iteration"></param>
    ''' <param name="timestep"></param>
    ''' <remarks></remarks>
    Private Function CreateDHSMultRasterStateAttributeOutput(ByVal iteration As Integer, ByVal timestep As Integer, stateAttrId As Integer, saFilename As String) As String

        Dim rastOutput As New StochasticTimeRaster
        ' Fetch the raster metadata from the InpRasters object
        Me.STSimTransformer.InputRasters.GetMetadata(rastOutput)
        rastOutput.InitDblCells()

        If Me.STSimTransformer.IsNoAgeAttributeType(stateAttrId) Then

            For Each c As Cell In Me.STSimTransformer.Cells

                Dim AttrValue As Nullable(Of Double) =
                    Me.STSimTransformer.GetAttributeValueNoAge(
                        stateAttrId, c.StratumId, c.SecondaryStratumId, c.StateClassId, iteration, timestep)

                ' If no value, then use NO_DATA (initialized above), otherwise AttrValue
                If AttrValue IsNot Nothing Then
                    rastOutput.DblCells(c.CellId) = CDbl(AttrValue)
                End If
            Next

        ElseIf Me.STSimTransformer.IsAgeAttributeType(stateAttrId) Then

            For Each c As Cell In Me.STSimTransformer.Cells

                Dim AttrValue As Nullable(Of Double) = Me.STSimTransformer.GetAttributeValueByAge(
                    stateAttrId, c.StratumId, c.SecondaryStratumId, c.StateClassId, iteration, timestep, c.Age)

                ' If no value, then use NO_DATA, otherwise AttrValue
                If AttrValue IsNot Nothing Then
                    rastOutput.DblCells(c.CellId) = CDbl(AttrValue)
                End If
            Next

        Else
            Debug.Assert(False, "Specified State Attributes not found in StateAttributeTypeIdsXXXAges")
            Return ""
        End If

        'DEVNOTE: Use Default NODATA_Value for all spatial output raster files
        rastOutput.NoDataValue = StochasticTimeRaster.DefaultNoDataValue

        RasterFiles.ProcessDoubleRasterToFile(rastOutput, saFilename, RasterCompression.GetGeoTiffCompressionType(Me.Library))

        Return saFilename

    End Function

    ''' <summary>
    ''' Export to a CSV file the list of Transition Groups. This will be used by an external script
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <remarks></remarks>
    Private Sub ExportTGtoCSV(ByVal fileName As String)

        Using sw As New StreamWriter(fileName, False)

            sw.Write("ID,")
            sw.Write("Name")
            sw.Write(vbCrLf)

            For Each tg In Me.STSimTransformer.TransitionGroups

                sw.Write(CSVFormatInteger(tg.TransitionGroupId))
                sw.Write(",")
                sw.Write(CSVFormatString(tg.DisplayName))
                sw.Write(vbCrLf)

            Next

        End Using

    End Sub

End Class

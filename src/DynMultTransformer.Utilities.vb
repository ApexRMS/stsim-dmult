'***********************************************************************************************
' stsim-dmult: SyncroSim Add-On Package (to stsim) for generating dynamic multipliers in ST-Sim.
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'***********************************************************************************************

Imports SyncroSim.Core
Imports SyncroSim.STSim
Imports System.Globalization

Partial Class DynMultTransformer

    Const CSV_INTEGER_FORMAT As String = "F0"
    Const CSV_DOUBLE_FORMAT As String = "F4"

    Public Shared Function CSVFormatInteger(value As Int32) As String
        Return value.ToString(CSV_INTEGER_FORMAT, CultureInfo.InvariantCulture)
    End Function

    Public Shared Function CSVFormatInteger(value As Int64) As String
        Return value.ToString(CSV_INTEGER_FORMAT, CultureInfo.InvariantCulture)
    End Function

    Public Shared Function CSVFormatString(value As String) As String
        Return InternalFormatStringCSV(value)
    End Function

    Private Shared Function InternalFormatStringCSV(value As String) As String

        Dim ContainsComma As Boolean = value.Contains(","c)
        Dim ContainsQuote As Boolean = value.Contains(""""c)

        If (Not ContainsComma And Not ContainsQuote) Then
            Return value
        End If

        If (ContainsQuote) Then

            Dim s As String = value.Replace("""", """""")
            Return String.Format(CultureInfo.InvariantCulture, """{0}""", s)

        Else
            Return String.Format(CultureInfo.InvariantCulture, """{0}""", value)
        End If

    End Function

    ''' <summary>
    ''' Gets the ST-Sim transformer
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSTSimTransformer() As STSimTransformer

        For Each t As Transformer In Me.Transformers

            If (t.Name = "stsim:runtime") Then
                Return CType(t, STSim.STSimTransformer)
            End If

        Next

        Throw New InvalidOperationException("ST-Sim Transformer not found.  Fatal error!")
        Return Nothing

    End Function

    ''' <summary>
    ''' Determines if Dynamic Habitat Suitablity Multipliers is enabled
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function IsDHSMultEnabled() As Boolean

        Dim dr As DataRow = Me.Scenario.GetDataSheet(DATAFEED_DHSM_NAME).GetDataRow()

        If dr Is Nothing Then
            Return False
        Else
            Return CBool(dr(DATASHEET_DHSM_ENABLED_COLUMN_NAME))
        End If

    End Function

    ''' <summary>
    ''' Sets whether or not this is a spatial model run
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function IsSpatialRun() As Boolean

        Dim drrc As DataRow = Me.Scenario.GetDataSheet("STSim_RunControl").GetDataRow()
        Return DataTableUtilities.GetDataBool(drrc("IsSpatial"))

    End Function

    ''' <summary>
    ''' Clear the Dynamic Habitat Suitablity Enable flag
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearDHSMultEnabled()

        Dim dr As DataRow = Me.Scenario.GetDataSheet(DATAFEED_DHSM_NAME).GetDataRow()

        If dr Is Nothing Then
            Return
        Else
            dr(DATASHEET_DHSM_ENABLED_COLUMN_NAME) = False
        End If

    End Sub

End Class

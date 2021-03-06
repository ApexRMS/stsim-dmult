﻿'***********************************************************************************************
' stsim-dmult: SyncroSim Add-On Package (to stsim) for generating dynamic multipliers in ST-Sim.
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'***********************************************************************************************

Imports System.Globalization

Module DataTableUtilities

    Public Function GetDataBool(value As Object) As Boolean
        If (Object.ReferenceEquals(value, DBNull.Value)) Then
            Return False
        Else
            Return Convert.ToBoolean(value, CultureInfo.InvariantCulture)
        End If
    End Function

    Public Function GetDataStr(value As Object) As String
        If (Object.ReferenceEquals(value, DBNull.Value)) Then
            Return Nothing
        Else
            Return Convert.ToString(value, CultureInfo.InvariantCulture)
        End If
    End Function

    Public Function GetDataInt(value As Object) As Integer
        If (Object.ReferenceEquals(value, DBNull.Value)) Then
            Return 0
        Else
            Return Convert.ToInt32(value, CultureInfo.InvariantCulture)
        End If
    End Function

End Module


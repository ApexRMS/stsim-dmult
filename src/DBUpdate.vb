'***********************************************************************************************
' stsim-dmult: SyncroSim Add-On Package (to stsim) for generating dynamic multipliers in ST-Sim.
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'***********************************************************************************************

Imports SyncroSim.Core

Class DBUpdate
    Inherits UpdateProvider

    Public Overrides Sub PerformUpdate(store As DataStore, currentSchemaVersion As Integer)

        'Start at 100 for 2.1.x
        If (currentSchemaVersion < 100) Then
            DMULT_0000100(store)
        End If

    End Sub

    ''' <summary>
    ''' DMULT_0000100
    ''' </summary>
    ''' <param name="store"></param>
    ''' <remarks>
    ''' This update renames the dynamic multiplier tables for the new namespace rules.
    ''' </remarks>
    Private Shared Sub DMULT_0000100(ByVal store As DataStore)
        UpdateProvider.RenameTablesWithPrefix(store, "DM_", "stsimdmult_")
    End Sub

End Class

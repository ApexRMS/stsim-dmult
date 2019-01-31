'*********************************************************************************************
' Dynamic Multipliers: A SyncroSim Package of dynamic multipliers for ST-Sim
'
' Copyright © 2007-2019 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'*********************************************************************************************

Imports System.Globalization

Module ExternalScript

    ''' <summary>
    ''' Process a O/S Command, such as a Windows Batch file, Linux shell scripts, or Python script. Block until the
    ''' script execution is completed.
    ''' </summary>
    ''' <param name="appName">The name of the command to run. The command should either include an appropriate path, or be accessible via the O/S path.</param>
    ''' <param name="args">Any arguments to apply to the command</param>
    ''' <remarks></remarks>
    Public Sub ProcessCmd(appName As String, args As String)

        Debug.Print("ProcessCmd:{0} {1}", appName, args)

        Dim oProcess As New Process()
        Dim oStartInfo As New ProcessStartInfo(appName, args)
        oStartInfo.CreateNoWindow = True
        oStartInfo.UseShellExecute = False
        oStartInfo.RedirectStandardError = True

        oProcess.StartInfo = oStartInfo
        'DEVNOTE: If the app (ie script) doesn't exist, or not in path, an exception will be thrown
        oProcess.Start()

        Dim oStreamReader As System.IO.StreamReader = oProcess.StandardError
        Dim sOutput As String = oStreamReader.ReadToEnd()
        oProcess.WaitForExit() ' wait indefinitely for the process to exit

        If oProcess.ExitCode <> 0 Then
            Dim sMsg = String.Format(CultureInfo.InvariantCulture, "Error executing external script '{0}'. Error Message: '{1}'", appName, sOutput)
            Throw New Exception(sMsg)
        End If

    End Sub

End Module

'*********************************************************************************************
' Dynamic Multipliers: A SyncroSim module of dynamic multipliers for ST-Sim
'
' Copyright © 2007-2017 Apex Resource Management Solution Ltd. (ApexRMS). All rights reserved.
'
'*********************************************************************************************

Imports SyncroSim.Core
Imports System.Reflection
Imports System.Globalization
Imports System.Windows.Forms

<ObfuscationAttribute(Exclude:=True, ApplyToMembers:=False)>
Class OptionsDataFeedView

    Private m_IsDisposed As Boolean

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If (disposing And Not Me.m_IsDisposed) Then

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

            Me.m_IsDisposed = True

        End If

        MyBase.Dispose(disposing)

    End Sub

    Public Overrides Sub EnableView(enable As Boolean)

        MyBase.EnableView(enable)

        If (enable) Then
            Me.EnableControls()
        End If

        Me.TextBoxScript.Enabled = False

    End Sub

    Public Overrides Sub LoadDataFeed(dataFeed As Core.DataFeed)

        MyBase.LoadDataFeed(dataFeed)

        Me.SetCheckBoxBinding(Me.CheckBoxEnable, DATASHEET_DHSM_ENABLED_COLUMN_NAME)
        Me.SetTextBoxBinding(Me.TextBoxTimesteps, DATASHEET_DHSM_FREQUENCY_COLUMN_NAME)
        Me.SetComboBoxBinding(Me.ComboBoxStateAttributeType, DATASHEET_DHSM_STATE_ATTRIBUTE_TYPE_COLUMN_NAME)
        Me.SetTextBoxBinding(Me.TextBoxScript, DATASHEET_DHSM_SCRIPT_COLUMN_NAME)

        If (ShouldEnableView()) Then
            Me.EnableControls()
        End If

        Me.MonitorDataSheet(
          "STSim_Terminology",
          AddressOf Me.OnTerminologyChanged,
          True)

    End Sub

    Private Sub OnTerminologyChanged(ByVal e As DataSheetMonitorEventArgs)

        Dim t As String = CStr(e.GetValue(
            "TimestepUnits", "timestep")).ToLower(CultureInfo.InvariantCulture)

        Me.LabelTimesteps.Text = t

    End Sub

    Protected Overrides Sub OnBoundCheckBoxChanged(checkBox As System.Windows.Forms.CheckBox, columnName As String)

        MyBase.OnBoundCheckBoxChanged(checkBox, columnName)
        Me.EnableControls()

    End Sub

    Private Sub EnableControls()

        Me.LabelUpdateEvery.Enabled = Me.CheckBoxEnable.Checked
        Me.TextBoxTimesteps.Enabled = Me.CheckBoxEnable.Checked
        Me.LabelTimesteps.Enabled = Me.CheckBoxEnable.Checked
        Me.LabelStateAttrType.Enabled = Me.CheckBoxEnable.Checked
        Me.ComboBoxStateAttributeType.Enabled = Me.CheckBoxEnable.Checked
        Me.TextBoxScript.Enabled = Me.CheckBoxEnable.Checked
        Me.ButtonChooseScript.Enabled = Me.CheckBoxEnable.Checked
        Me.ButtonClearScript.Enabled = Me.CheckBoxEnable.Checked
        Me.ButtonClearAll.Enabled = Me.CheckBoxEnable.Checked

    End Sub

    Private Sub ChooseScriptName()

        Dim dlg As New OpenFileDialog()

        dlg.Title = "Choose Script"
        'DEVTODO: TOM - Ask Leo what he wants to do about this filter. I would like to support Windows BAT, Powershell(?), and Linux Shell scripts
        dlg.Filter = "Script Name(*.*)|*.*"

        If (dlg.ShowDialog <> DialogResult.OK) Then
            Return
        End If

        Me.DataFeed.GetDataSheet(DATAFEED_DHSM_NAME).SetSingleRowData(
            DATASHEET_DHSM_SCRIPT_COLUMN_NAME, dlg.FileName)

    End Sub

    Private Sub ClearScriptName()

        Me.DataFeed.GetDataSheet(DATAFEED_DHSM_NAME).SetSingleRowData(
            DATASHEET_DHSM_SCRIPT_COLUMN_NAME, Nothing)

    End Sub

    Private Sub ButtonChooseScript_Click(sender As System.Object, e As System.EventArgs) Handles ButtonChooseScript.Click
        Me.ChooseScriptName()
    End Sub

    Private Sub ButtonClearScript_Click(sender As System.Object, e As System.EventArgs) Handles ButtonClearScript.Click
        Me.ClearScriptName()
    End Sub

    Private Sub ButtonClearAll_Click(sender As System.Object, e As System.EventArgs) Handles ButtonClearAll.Click

        Me.DataFeed.GetDataSheet(DATAFEED_DHSM_NAME).ClearData()
        Me.EnableControls()

    End Sub

End Class

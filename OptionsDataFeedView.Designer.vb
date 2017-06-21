<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsDataFeedView
    Inherits SyncroSim.Core.Forms.DataFeedView

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LabelUpdateEvery = New System.Windows.Forms.Label()
        Me.LabelStateAttrType = New System.Windows.Forms.Label()
        Me.TextBoxTimesteps = New System.Windows.Forms.TextBox()
        Me.LabelTimesteps = New System.Windows.Forms.Label()
        Me.ComboBoxStateAttributeType = New System.Windows.Forms.ComboBox()
        Me.LabelScriptName = New System.Windows.Forms.Label()
        Me.TextBoxScript = New System.Windows.Forms.TextBox()
        Me.ButtonChooseScript = New System.Windows.Forms.Button()
        Me.ButtonClearScript = New System.Windows.Forms.Button()
        Me.ButtonClearAll = New System.Windows.Forms.Button()
        Me.CheckBoxEnable = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'LabelUpdateEvery
        '
        Me.LabelUpdateEvery.AutoSize = True
        Me.LabelUpdateEvery.Location = New System.Drawing.Point(60, 64)
        Me.LabelUpdateEvery.Name = "LabelUpdateEvery"
        Me.LabelUpdateEvery.Size = New System.Drawing.Size(74, 13)
        Me.LabelUpdateEvery.TabIndex = 1
        Me.LabelUpdateEvery.Text = "Update every:"
        '
        'LabelStateAttrType
        '
        Me.LabelStateAttrType.AutoSize = True
        Me.LabelStateAttrType.Location = New System.Drawing.Point(35, 90)
        Me.LabelStateAttrType.Name = "LabelStateAttrType"
        Me.LabelStateAttrType.Size = New System.Drawing.Size(99, 13)
        Me.LabelStateAttrType.TabIndex = 4
        Me.LabelStateAttrType.Text = "State attribute type:"
        '
        'TextBoxTimesteps
        '
        Me.TextBoxTimesteps.Location = New System.Drawing.Point(140, 62)
        Me.TextBoxTimesteps.Name = "TextBoxTimesteps"
        Me.TextBoxTimesteps.Size = New System.Drawing.Size(55, 20)
        Me.TextBoxTimesteps.TabIndex = 2
        '
        'LabelTimesteps
        '
        Me.LabelTimesteps.AutoSize = True
        Me.LabelTimesteps.Location = New System.Drawing.Point(201, 65)
        Me.LabelTimesteps.Name = "LabelTimesteps"
        Me.LabelTimesteps.Size = New System.Drawing.Size(46, 13)
        Me.LabelTimesteps.TabIndex = 3
        Me.LabelTimesteps.Text = "timestep"
        '
        'ComboBoxStateAttributeType
        '
        Me.ComboBoxStateAttributeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxStateAttributeType.FormattingEnabled = True
        Me.ComboBoxStateAttributeType.Location = New System.Drawing.Point(140, 88)
        Me.ComboBoxStateAttributeType.Name = "ComboBoxStateAttributeType"
        Me.ComboBoxStateAttributeType.Size = New System.Drawing.Size(182, 21)
        Me.ComboBoxStateAttributeType.TabIndex = 5
        '
        'LabelScriptName
        '
        Me.LabelScriptName.AutoSize = True
        Me.LabelScriptName.Location = New System.Drawing.Point(97, 116)
        Me.LabelScriptName.Name = "LabelScriptName"
        Me.LabelScriptName.Size = New System.Drawing.Size(37, 13)
        Me.LabelScriptName.TabIndex = 6
        Me.LabelScriptName.Text = "Script:"
        '
        'TextBoxScript
        '
        Me.TextBoxScript.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxScript.Location = New System.Drawing.Point(140, 115)
        Me.TextBoxScript.Name = "TextBoxScript"
        Me.TextBoxScript.Size = New System.Drawing.Size(302, 20)
        Me.TextBoxScript.TabIndex = 7
        '
        'ButtonChooseScript
        '
        Me.ButtonChooseScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonChooseScript.Location = New System.Drawing.Point(449, 113)
        Me.ButtonChooseScript.Name = "ButtonChooseScript"
        Me.ButtonChooseScript.Size = New System.Drawing.Size(75, 23)
        Me.ButtonChooseScript.TabIndex = 8
        Me.ButtonChooseScript.Text = "Browse..."
        Me.ButtonChooseScript.UseVisualStyleBackColor = True
        '
        'ButtonClearScript
        '
        Me.ButtonClearScript.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClearScript.Location = New System.Drawing.Point(530, 113)
        Me.ButtonClearScript.Name = "ButtonClearScript"
        Me.ButtonClearScript.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClearScript.TabIndex = 9
        Me.ButtonClearScript.Text = "Clear"
        Me.ButtonClearScript.UseVisualStyleBackColor = True
        '
        'ButtonClearAll
        '
        Me.ButtonClearAll.Location = New System.Drawing.Point(140, 142)
        Me.ButtonClearAll.Name = "ButtonClearAll"
        Me.ButtonClearAll.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClearAll.TabIndex = 10
        Me.ButtonClearAll.Text = "Clear All"
        Me.ButtonClearAll.UseVisualStyleBackColor = True
        '
        'CheckBoxEnable
        '
        Me.CheckBoxEnable.AutoSize = True
        Me.CheckBoxEnable.Location = New System.Drawing.Point(16, 15)
        Me.CheckBoxEnable.Name = "CheckBoxEnable"
        Me.CheckBoxEnable.Size = New System.Drawing.Size(229, 17)
        Me.CheckBoxEnable.TabIndex = 0
        Me.CheckBoxEnable.Text = "Enable dynamic habitat suitability multipliers"
        Me.CheckBoxEnable.UseVisualStyleBackColor = True
        '
        'DHSMultipliersDataFeedView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CheckBoxEnable)
        Me.Controls.Add(Me.ButtonClearAll)
        Me.Controls.Add(Me.ButtonClearScript)
        Me.Controls.Add(Me.ButtonChooseScript)
        Me.Controls.Add(Me.TextBoxScript)
        Me.Controls.Add(Me.LabelScriptName)
        Me.Controls.Add(Me.ComboBoxStateAttributeType)
        Me.Controls.Add(Me.LabelTimesteps)
        Me.Controls.Add(Me.TextBoxTimesteps)
        Me.Controls.Add(Me.LabelStateAttrType)
        Me.Controls.Add(Me.LabelUpdateEvery)
        Me.Name = "DHSMultipliersDataFeedView"
        Me.Size = New System.Drawing.Size(629, 179)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelUpdateEvery As System.Windows.Forms.Label
    Friend WithEvents LabelStateAttrType As System.Windows.Forms.Label
    Friend WithEvents TextBoxTimesteps As System.Windows.Forms.TextBox
    Friend WithEvents LabelTimesteps As System.Windows.Forms.Label
    Friend WithEvents ComboBoxStateAttributeType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelScriptName As System.Windows.Forms.Label
    Friend WithEvents TextBoxScript As System.Windows.Forms.TextBox
    Friend WithEvents ButtonChooseScript As System.Windows.Forms.Button
    Friend WithEvents ButtonClearScript As System.Windows.Forms.Button
    Friend WithEvents ButtonClearAll As System.Windows.Forms.Button
    Friend WithEvents CheckBoxEnable As System.Windows.Forms.CheckBox

End Class

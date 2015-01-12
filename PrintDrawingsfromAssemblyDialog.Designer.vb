<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class printDrawingsfromAssemblyDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(printDrawingsfromAssemblyDialog))
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SelectAll = New System.Windows.Forms.CheckBox()
        Me.rbtnOpenFile = New System.Windows.Forms.RadioButton()
        Me.rbtnPrintFiles = New System.Windows.Forms.RadioButton()
        Me.SelectUnApproved = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(93, 298)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 0
        Me.btnAccept.Text = "O&K"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(180, 298)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'SelectAll
        '
        Me.SelectAll.AutoSize = True
        Me.SelectAll.Location = New System.Drawing.Point(17, 304)
        Me.SelectAll.Name = "SelectAll"
        Me.SelectAll.Size = New System.Drawing.Size(70, 17)
        Me.SelectAll.TabIndex = 2
        Me.SelectAll.Text = "Select All"
        Me.SelectAll.UseVisualStyleBackColor = True
        '
        'rbtnOpenFile
        '
        Me.rbtnOpenFile.AutoSize = True
        Me.rbtnOpenFile.Location = New System.Drawing.Point(17, 265)
        Me.rbtnOpenFile.Name = "rbtnOpenFile"
        Me.rbtnOpenFile.Size = New System.Drawing.Size(98, 17)
        Me.rbtnOpenFile.TabIndex = 3
        Me.rbtnOpenFile.Text = "Open Drawings"
        Me.rbtnOpenFile.UseVisualStyleBackColor = True
        '
        'rbtnPrintFiles
        '
        Me.rbtnPrintFiles.AutoSize = True
        Me.rbtnPrintFiles.Checked = True
        Me.rbtnPrintFiles.Location = New System.Drawing.Point(17, 242)
        Me.rbtnPrintFiles.Name = "rbtnPrintFiles"
        Me.rbtnPrintFiles.Size = New System.Drawing.Size(93, 17)
        Me.rbtnPrintFiles.TabIndex = 4
        Me.rbtnPrintFiles.TabStop = True
        Me.rbtnPrintFiles.Text = "Print Drawings"
        Me.rbtnPrintFiles.UseVisualStyleBackColor = True
        '
        'SelectUnApproved
        '
        Me.SelectUnApproved.AutoSize = True
        Me.SelectUnApproved.Location = New System.Drawing.Point(120, 265)
        Me.SelectUnApproved.Name = "SelectUnApproved"
        Me.SelectUnApproved.Size = New System.Drawing.Size(139, 17)
        Me.SelectUnApproved.TabIndex = 5
        Me.SelectUnApproved.Text = "Show Unapproved Only"
        Me.SelectUnApproved.UseVisualStyleBackColor = True
        '
        'printDrawingsfromAssemblyDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(268, 332)
        Me.Controls.Add(Me.SelectUnApproved)
        Me.Controls.Add(Me.rbtnPrintFiles)
        Me.Controls.Add(Me.rbtnOpenFile)
        Me.Controls.Add(Me.SelectAll)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "printDrawingsfromAssemblyDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Print Drawings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents SelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents rbtnOpenFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnPrintFiles As System.Windows.Forms.RadioButton
    Friend WithEvents SelectUnApproved As System.Windows.Forms.CheckBox
End Class

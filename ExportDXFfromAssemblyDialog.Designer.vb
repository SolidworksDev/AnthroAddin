﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportDXFfromAssemblyDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExportDXFfromAssemblyDialog))
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SelectAll = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(101, 297)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 0
        Me.btnAccept.Text = "OK"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = "Cancel Button"
        Me.btnCancel.AccessibleName = "Cancel"
        Me.btnCancel.Location = New System.Drawing.Point(182, 297)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'SelectAll
        '
        Me.SelectAll.AutoSize = True
        Me.SelectAll.Location = New System.Drawing.Point(12, 303)
        Me.SelectAll.Name = "SelectAll"
        Me.SelectAll.Size = New System.Drawing.Size(70, 17)
        Me.SelectAll.TabIndex = 3
        Me.SelectAll.Text = "Select All"
        Me.SelectAll.UseVisualStyleBackColor = True
        '
        'ExportDXFfromAssemblyDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(269, 332)
        Me.Controls.Add(Me.SelectAll)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ExportDXFfromAssemblyDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Export DXF"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents SelectAll As System.Windows.Forms.CheckBox
End Class
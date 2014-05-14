<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SlotFeatureDialog
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SlotFeatureDialog))
        Me.tblButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.tblSize = New System.Windows.Forms.TableLayoutPanel()
        Me.txtbHeigth = New System.Windows.Forms.TextBox()
        Me.txtHeigth = New System.Windows.Forms.Label()
        Me.txtWidth = New System.Windows.Forms.Label()
        Me.txtbWidth = New System.Windows.Forms.TextBox()
        Me.rbtnHorizontal = New System.Windows.Forms.RadioButton()
        Me.rbtnVertical = New System.Windows.Forms.RadioButton()
        Me.cbWorkAxis = New System.Windows.Forms.CheckBox()
        Me.tblButtons.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tblSize.SuspendLayout()
        Me.SuspendLayout()
        '
        'tblButtons
        '
        Me.tblButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tblButtons.ColumnCount = 2
        Me.tblButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblButtons.Controls.Add(Me.btnAccept, 0, 0)
        Me.tblButtons.Controls.Add(Me.btnCancel, 1, 0)
        Me.tblButtons.Location = New System.Drawing.Point(23, 201)
        Me.tblButtons.Name = "tblButtons"
        Me.tblButtons.RowCount = 1
        Me.tblButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblButtons.Size = New System.Drawing.Size(162, 29)
        Me.tblButtons.TabIndex = 0
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(3, 3)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 2
        Me.btnAccept.Text = "OK"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(84, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'tblSize
        '
        Me.tblSize.ColumnCount = 1
        Me.tblSize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblSize.Controls.Add(Me.txtbWidth, 0, 3)
        Me.tblSize.Controls.Add(Me.txtWidth, 0, 2)
        Me.tblSize.Controls.Add(Me.txtHeigth, 0, 0)
        Me.tblSize.Controls.Add(Me.txtbHeigth, 0, 1)
        Me.tblSize.Location = New System.Drawing.Point(12, 22)
        Me.tblSize.Name = "tblSize"
        Me.tblSize.RowCount = 2
        Me.tblSize.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblSize.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblSize.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblSize.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblSize.Size = New System.Drawing.Size(132, 89)
        Me.tblSize.TabIndex = 0
        '
        'txtbHeigth
        '
        Me.txtbHeigth.AcceptsTab = True
        Me.txtbHeigth.Location = New System.Drawing.Point(3, 18)
        Me.txtbHeigth.Name = "txtbHeigth"
        Me.txtbHeigth.Size = New System.Drawing.Size(105, 20)
        Me.txtbHeigth.TabIndex = 0
        Me.txtbHeigth.Text = ".125"
        '
        'txtHeigth
        '
        Me.txtHeigth.AutoSize = True
        Me.txtHeigth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeigth.Location = New System.Drawing.Point(3, 0)
        Me.txtHeigth.Name = "txtHeigth"
        Me.txtHeigth.Size = New System.Drawing.Size(43, 15)
        Me.txtHeigth.TabIndex = 3
        Me.txtHeigth.Text = "Heigth"
        '
        'txtWidth
        '
        Me.txtWidth.AutoSize = True
        Me.txtWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWidth.Location = New System.Drawing.Point(3, 41)
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(38, 15)
        Me.txtWidth.TabIndex = 2
        Me.txtWidth.Text = "Width"
        '
        'txtbWidth
        '
        Me.txtbWidth.AcceptsTab = True
        Me.txtbWidth.Location = New System.Drawing.Point(3, 59)
        Me.txtbWidth.Name = "txtbWidth"
        Me.txtbWidth.Size = New System.Drawing.Size(105, 20)
        Me.txtbWidth.TabIndex = 1
        Me.txtbWidth.Text = ".5"
        '
        'rbtnHorizontal
        '
        Me.rbtnHorizontal.AutoSize = True
        Me.rbtnHorizontal.Checked = True
        Me.rbtnHorizontal.Location = New System.Drawing.Point(12, 129)
        Me.rbtnHorizontal.Name = "rbtnHorizontal"
        Me.rbtnHorizontal.Size = New System.Drawing.Size(72, 17)
        Me.rbtnHorizontal.TabIndex = 4
        Me.rbtnHorizontal.TabStop = True
        Me.rbtnHorizontal.Text = "Horizontal"
        Me.rbtnHorizontal.UseVisualStyleBackColor = True
        '
        'rbtnVertical
        '
        Me.rbtnVertical.AutoSize = True
        Me.rbtnVertical.Location = New System.Drawing.Point(12, 161)
        Me.rbtnVertical.Name = "rbtnVertical"
        Me.rbtnVertical.Size = New System.Drawing.Size(60, 17)
        Me.rbtnVertical.TabIndex = 5
        Me.rbtnVertical.Text = "Vertical"
        Me.rbtnVertical.UseVisualStyleBackColor = True
        '
        'cbWorkAxis
        '
        Me.cbWorkAxis.AutoSize = True
        Me.cbWorkAxis.Location = New System.Drawing.Point(96, 161)
        Me.cbWorkAxis.Name = "cbWorkAxis"
        Me.cbWorkAxis.Size = New System.Drawing.Size(74, 17)
        Me.cbWorkAxis.TabIndex = 6
        Me.cbWorkAxis.TabStop = False
        Me.cbWorkAxis.Text = "Work Axis"
        Me.cbWorkAxis.UseVisualStyleBackColor = True
        '
        'SlotFeatureDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(206, 242)
        Me.Controls.Add(Me.cbWorkAxis)
        Me.Controls.Add(Me.tblButtons)
        Me.Controls.Add(Me.rbtnVertical)
        Me.Controls.Add(Me.rbtnHorizontal)
        Me.Controls.Add(Me.tblSize)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SlotFeatureDialog"
        Me.Text = "Slot"
        Me.tblButtons.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tblSize.ResumeLayout(False)
        Me.tblSize.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tblButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents cbWorkAxis As System.Windows.Forms.CheckBox
    Friend WithEvents rbtnVertical As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnHorizontal As System.Windows.Forms.RadioButton
    Friend WithEvents tblSize As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtbWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtWidth As System.Windows.Forms.Label
    Friend WithEvents txtHeigth As System.Windows.Forms.Label
    Friend WithEvents txtbHeigth As System.Windows.Forms.TextBox
End Class

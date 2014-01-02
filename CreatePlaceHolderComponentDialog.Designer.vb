<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CreatePlaceHolderComponentDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CreatePlaceHolderComponentDialog))
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.txtbxComponentPartNumber = New System.Windows.Forms.TextBox()
        Me.txtbxComponentDescription = New System.Windows.Forms.TextBox()
        Me.lblPartNumber = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.rbtnCheckin = New System.Windows.Forms.RadioButton()
        Me.lblMaterialGroup = New System.Windows.Forms.Label()
        Me.cbxMaterialGroup = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(3, 3)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 4
        Me.btnAccept.Text = "OK"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(87, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnAccept, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(114, 122)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(168, 34)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'txtbxComponentPartNumber
        '
        Me.txtbxComponentPartNumber.Location = New System.Drawing.Point(142, 59)
        Me.txtbxComponentPartNumber.Name = "txtbxComponentPartNumber"
        Me.txtbxComponentPartNumber.Size = New System.Drawing.Size(140, 20)
        Me.txtbxComponentPartNumber.TabIndex = 1
        '
        'txtbxComponentDescription
        '
        Me.txtbxComponentDescription.Location = New System.Drawing.Point(142, 86)
        Me.txtbxComponentDescription.Name = "txtbxComponentDescription"
        Me.txtbxComponentDescription.Size = New System.Drawing.Size(140, 20)
        Me.txtbxComponentDescription.TabIndex = 2
        '
        'lblPartNumber
        '
        Me.lblPartNumber.AutoSize = True
        Me.lblPartNumber.Location = New System.Drawing.Point(13, 66)
        Me.lblPartNumber.Name = "lblPartNumber"
        Me.lblPartNumber.Size = New System.Drawing.Size(123, 13)
        Me.lblPartNumber.TabIndex = 5
        Me.lblPartNumber.Text = "Component Part Number"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(16, 93)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(117, 13)
        Me.lblDescription.TabIndex = 6
        Me.lblDescription.Text = "Component Description"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'rbtnCheckin
        '
        Me.rbtnCheckin.AutoSize = True
        Me.rbtnCheckin.Location = New System.Drawing.Point(19, 134)
        Me.rbtnCheckin.Name = "rbtnCheckin"
        Me.rbtnCheckin.Size = New System.Drawing.Size(68, 17)
        Me.rbtnCheckin.TabIndex = 3
        Me.rbtnCheckin.TabStop = True
        Me.rbtnCheckin.Text = "Check In"
        Me.rbtnCheckin.UseVisualStyleBackColor = True
        '
        'lblMaterialGroup
        '
        Me.lblMaterialGroup.AutoSize = True
        Me.lblMaterialGroup.Location = New System.Drawing.Point(60, 32)
        Me.lblMaterialGroup.Name = "lblMaterialGroup"
        Me.lblMaterialGroup.Size = New System.Drawing.Size(76, 13)
        Me.lblMaterialGroup.TabIndex = 8
        Me.lblMaterialGroup.Text = "Material Group"
        '
        'cbxMaterialGroup
        '
        Me.cbxMaterialGroup.FormattingEnabled = True
        Me.cbxMaterialGroup.Items.AddRange(New Object() {"175 (Plastics)", "180 (Fabric)", "200 (Wire)", "375 (Tools)", "395 (Electrical Termination)", "400 (electrical)", "581 (Casegood Hardware)", "625 (Phantom-Assembly)"})
        Me.cbxMaterialGroup.Location = New System.Drawing.Point(142, 32)
        Me.cbxMaterialGroup.Name = "cbxMaterialGroup"
        Me.cbxMaterialGroup.Size = New System.Drawing.Size(140, 21)
        Me.cbxMaterialGroup.TabIndex = 0
        '
        'CreatePlaceHolderComponentDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(300, 168)
        Me.Controls.Add(Me.cbxMaterialGroup)
        Me.Controls.Add(Me.lblMaterialGroup)
        Me.Controls.Add(Me.rbtnCheckin)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblPartNumber)
        Me.Controls.Add(Me.txtbxComponentDescription)
        Me.Controls.Add(Me.txtbxComponentPartNumber)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CreatePlaceHolderComponentDialog"
        Me.Text = "Component Placeholder"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents txtbxComponentPartNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtbxComponentDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblPartNumber As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents rbtnCheckin As System.Windows.Forms.RadioButton
    Friend WithEvents cbxMaterialGroup As System.Windows.Forms.ComboBox
    Friend WithEvents lblMaterialGroup As System.Windows.Forms.Label
End Class

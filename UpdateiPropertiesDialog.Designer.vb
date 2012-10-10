<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateiPropertiesDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdateiPropertiesDialog))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAccept = New System.Windows.Forms.Button()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtBxApprovedBy = New System.Windows.Forms.TextBox()
        Me.txtBxRevision = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CommentLable = New System.Windows.Forms.Label()
        Me.txtComment = New System.Windows.Forms.TextBox()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.SelectAll = New System.Windows.Forms.CheckBox()
        Me.ShowUnApproved = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnCancel, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAccept, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(107, 370)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(162, 35)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.CausesValidation = False
        Me.btnCancel.Location = New System.Drawing.Point(84, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 25)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(3, 3)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 25)
        Me.btnAccept.TabIndex = 0
        Me.btnAccept.Text = "OK"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.35408!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.64592!))
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtBxApprovedBy, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.txtBxRevision, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.CommentLable, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.txtComment, 1, 2)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(257, 93)
        Me.TableLayoutPanel2.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Approved By:"
        '
        'txtBxApprovedBy
        '
        Me.txtBxApprovedBy.Location = New System.Drawing.Point(98, 3)
        Me.txtBxApprovedBy.Name = "txtBxApprovedBy"
        Me.txtBxApprovedBy.Size = New System.Drawing.Size(130, 20)
        Me.txtBxApprovedBy.TabIndex = 2
        Me.txtBxApprovedBy.Text = "jtm"
        '
        'txtBxRevision
        '
        Me.txtBxRevision.Location = New System.Drawing.Point(98, 33)
        Me.txtBxRevision.Name = "txtBxRevision"
        Me.txtBxRevision.Size = New System.Drawing.Size(130, 20)
        Me.txtBxRevision.TabIndex = 3
        Me.txtBxRevision.Text = "-"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Revision:"
        '
        'CommentLable
        '
        Me.CommentLable.AutoSize = True
        Me.CommentLable.Location = New System.Drawing.Point(3, 60)
        Me.CommentLable.Name = "CommentLable"
        Me.CommentLable.Size = New System.Drawing.Size(54, 13)
        Me.CommentLable.TabIndex = 4
        Me.CommentLable.Text = "Comment:"
        '
        'txtComment
        '
        Me.txtComment.Location = New System.Drawing.Point(98, 63)
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(130, 20)
        Me.txtComment.TabIndex = 5
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'SelectAll
        '
        Me.SelectAll.AutoSize = True
        Me.SelectAll.Location = New System.Drawing.Point(15, 380)
        Me.SelectAll.Name = "SelectAll"
        Me.SelectAll.Size = New System.Drawing.Size(69, 17)
        Me.SelectAll.TabIndex = 3
        Me.SelectAll.Text = "Select all"
        Me.SelectAll.UseVisualStyleBackColor = True
        '
        'ShowUnApproved
        '
        Me.ShowUnApproved.AutoSize = True
        Me.ShowUnApproved.Location = New System.Drawing.Point(15, 350)
        Me.ShowUnApproved.Name = "ShowUnApproved"
        Me.ShowUnApproved.Size = New System.Drawing.Size(139, 17)
        Me.ShowUnApproved.TabIndex = 4
        Me.ShowUnApproved.Text = "Show Unapproved Only"
        Me.ShowUnApproved.UseVisualStyleBackColor = True
        '
        'UpdateiPropertiesDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(284, 407)
        Me.Controls.Add(Me.ShowUnApproved)
        Me.Controls.Add(Me.SelectAll)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UpdateiPropertiesDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Updated iProperties"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBxApprovedBy As System.Windows.Forms.TextBox
    Friend WithEvents txtBxRevision As System.Windows.Forms.TextBox
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents SelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents CommentLable As System.Windows.Forms.Label
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents ShowUnApproved As System.Windows.Forms.CheckBox
End Class

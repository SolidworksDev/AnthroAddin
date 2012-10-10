Imports Inventor
Imports System.IO
Imports System.Security
Imports System.Windows.Forms
Imports AnthroAddIn.Security
Imports AnthroAddIn.DocumentSvc
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity


Public Class SelectDialog

    Private invApp As Inventor.Application
    Private drawingList As New ArrayList
    Private exportPath As String = "\\ANTHRO3\dwf\Standard\"
    Public bAcceptClicked As Boolean = False
    Public bCancelClicked As Boolean = False

    Public ReadOnly Property DrawingListFormControls() As Control.ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Function SelectDialog_AddListBox(ByVal inControls As Control.ControlCollection) As Boolean
        Try

            Dim LablePosition As System.Drawing.Point
            LablePosition.X = 12
            LablePosition.Y = 15

            Dim ListPosition As System.Drawing.Point
            ListPosition.X = 12
            ListPosition.Y = 40

            Dim newList As New CheckedListBox
            Dim newLable As New System.Windows.Forms.Label

            Dim strListName As String

            newList.CheckOnClick = True

            newLable.Name = "DocumentsListDialogLable"
            newLable.Text = "Select Drawings to Print"
            newLable.Width = 225
            newLable.Location = LablePosition

            newList.Text = "Select Drawings"
            newList.Name = "DocumentsListBox"
            newList.Width = 235
            newList.Height = 280
            newList.Location = ListPosition

            Dim invDocs As Documents = invApp.Documents

            Dim invDoc As Document

            For i = 1 To invDocs.Count
                invDoc = invDocs.Item(i)                
                If invDoc.DocumentType = Inventor.DocumentTypeEnum.kDrawingDocumentObject Then
                    strListName = invDoc.DisplayName
                    newList.Items.Add(invDoc.DisplayName)
                End If
            Next

            newList.Sorted = True

            inControls.Add(newLable)
            inControls.Add(newList)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Function

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim verifyForm As New VerifyForm
        Dim DrawingListcontrols As Control.ControlCollection = verifyForm.DrawingListFormControls
        Dim invDocs As Documents = invApp.Documents

        Me.Visible = False

        For Each aControl In Me.Controls
            If TypeName(aControl) = "CheckedListBox" Then
                If CType(aControl, CheckedListBox).CheckedItems.Count <> 0 Then
                    For Each anObject In CType(aControl, CheckedListBox).CheckedItems
                        If TypeOf anObject Is String Then
                            drawingList.Add(anObject.ToString)
                        End If
                    Next
                Else
                    MsgBox("You muse select at lease" & Chr(13) & "one part from the list", MsgBoxStyle.Critical, "Select a part")
                    Me.Visible = True
                    Exit Sub
                End If
            End If
        Next

       

        If rbtnExport.Checked Then
            verifyForm.VerifyDialog_AddListBox(drawingList, DrawingListcontrols, _
                                              "DrawingsListDialogLable",
                                              "Export the following drawings?",
                                              "DrawingsListBox",
                                              "Select Drawings")
            verifyForm.Text = "Verify Export DWFx"
            verifyForm.Icon = My.Resources.DWF_Viewer
        Else
            verifyForm.VerifyDialog_AddListBox(drawingList, DrawingListcontrols, _
                                              "DrawingsListDialogLable",
                                              "Print the following drawings?",
                                              "DrawingsListBox",
                                              "Select Drawings")
            verifyForm.Text = "Veriry Print Drawings"
            verifyForm.Icon = My.Resources.printer
        End If

        verifyForm.ShowDialog()

        If verifyForm.bAcceptClicked = True Then
            For i = 1 To invDocs.Count
                For j = 0 To drawingList.Count - 1
                    If invDocs.Item(i).DisplayName = drawingList.Item(j).ToString And invDocs.Item(i).DocumentType = DocumentTypeEnum.kDrawingDocumentObject Then
                        If rbtnExport.Checked = True Then
                            invDocs.Item(i).SaveAs(exportPath & drawingList(j).ToString & ".dwfx", True)
                        Else
                            invDocs.Item(i).PrintManager.SubmitPrint()
                        End If
                    End If
                Next
            Next
            Me.Close()
        Else
            Me.Visible = True
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp

    End Sub

    

    Private Sub rbtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnExport.Click

        For Each aControl In Me.Controls
            If TypeName(aControl) = "Label" Then
                If CType(aControl, System.Windows.Forms.Label).Name.ToString = "DocumentsListDialogLable" Then
                    CType(aControl, System.Windows.Forms.Label).Text = "Select Drawings to Export"
                    Exit For
                End If
            End If
        Next

        Me.Icon = My.Resources.DWF_Viewer

    End Sub

    Private Sub rbtnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnPrint.Click

        For Each aControl In Me.Controls
            If TypeName(aControl) = "Label" Then
                If CType(aControl, System.Windows.Forms.Label).Name.ToString = "DocumentsListDialogLable" Then
                    CType(aControl, System.Windows.Forms.Label).Text = "Select Drawings to Print"
                    Exit For
                End If
            End If
        Next

        Me.Icon = My.Resources.printer

    End Sub
End Class
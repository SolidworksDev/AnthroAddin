Imports Inventor
Imports System.IO
Imports System.Security
Imports System.Windows.Forms
Imports AnthroAddIn.Security
Imports AnthroAddIn.DocumentSvc
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity
Imports System.IO.Path

Public Class printDrawingsfromAssemblyDialog

    Public invApp As Inventor.Application
    Public drawingList As New DrawingDocuments
    Public serverLogin As New ServerLogin
    Private bAllChecked As Boolean = False
    Private bSelectUnapproved As Boolean = False
    Public Shared bAcceptClicked As Boolean = False
    Public Shared bCancelClicked As Boolean = False
    Private anObject As Object
    Private strDrawingFileNames As String = ""

    ' Create a property so the controls can be easily retrieved.
    Public ReadOnly Property DrawingsListFormControls() As Control.ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Function DrawingsDialog_AddListBox(ByVal inControls As Control.ControlCollection) As Boolean

        Try

            Dim LablePosition As System.Drawing.Point
            LablePosition.X = 10
            LablePosition.Y = 10

            Dim ListPosition As System.Drawing.Point
            ListPosition.X = 10
            ListPosition.Y = 40

            Dim newList As New CheckedListBox
            Dim newLable As New System.Windows.Forms.Label

            newList.CheckOnClick = True

            newLable.Name = "DrawingsListDialogLable"
            newLable.Text = "Select Drawings to Print"
            newLable.Width = 200
            newLable.Location = LablePosition

            newList.Text = "Select Drawings"
            newList.Name = "DrawingsListBox"
            newList.Width = 245
            newList.Height = 280
            newList.Location = ListPosition

            Dim invAsmDoc As AssemblyDocument
            invAsmDoc = invApp.ActiveDocument
            newList.Items.Add(invAsmDoc.DisplayName)
            If SelectUnApproved.Checked And IsApproved(invAsmDoc) Then
                newList.Items.Remove(invAsmDoc.DisplayName)
            End If

            Dim invRefDocs As DocumentsEnumerator
            invRefDocs = invAsmDoc.AllReferencedDocuments

            Dim invRefDoc As Document
            Dim DocName As String            

            For Each invRefDoc In invRefDocs
                DocName = invRefDoc.DisplayName
                If IsDoc(DocName) Then
                    If Not newList.Items.Contains(DocName) Then
                        newList.Items.Add(DocName)
                    End If
                    If SelectUnApproved.Checked And IsApproved(invRefDoc) Then
                        newList.Items.Remove(DocName)
                    End If
                End If
            Next

            Dim strFirstnewLisName As String
            Dim strSecondnewListName As String
            Dim inewListCount As Integer = newList.Items.Count - 1

            For i = 0 To newList.Items.Count - 1
                If i < inewListCount Then
                    strFirstnewLisName = ChangeExtension(newList.Items.Item(i).ToString, "")
                    strSecondnewListName = ChangeExtension(newList.Items.Item(i + 1).ToString, "")
                    If strFirstnewLisName = strSecondnewListName Then
                        newList.Items.Remove(newList.Items.Item(i + 1))
                        inewListCount = inewListCount - 1
                    End If
                Else
                    Exit For
                End If
            Next

            inControls.Add(newLable)
            newList.Sorted = True
            inControls.Add(newList)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            strDrawingFileNames = ""
            drawingList.DrawingName.Clear()
            drawingList.DrawingIndex.Clear()
            anObject = Nothing
            Me.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        serverLogin.LoginToVault(HOST)
        If serverLogin.LoggedIn = False Then
            strDrawingFileNames = ""
            drawingList.DrawingName.Clear()
            drawingList.DrawingIndex.Clear()
            anObject = Nothing
            Me.Dispose()
            Exit Sub
        End If

        Dim files() As DocumentSvc.File = {}
        Dim vaultService As New VaultServices

        Dim i As Integer
        Dim j As Integer


        Dim intUserResponse As Integer = 0

        Dim invDocs As Documents = invApp.Documents
        Dim drawingFiles() As String = {}
        Dim downloadFiles() As String = {}

        Dim verifyForm As New VerifyForm()
        Dim DrawingListcontrols As Control.ControlCollection = verifyForm.DrawingListFormControls

        Me.Cursor = Cursors.WaitCursor
        Me.Visible = False

        Try

            ' Get the checklistbox control and then set the size of the drawingFiles and downloadFiles
            ' array to that size
            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    ReDim drawingFiles(0 To CType(aControl, CheckedListBox).CheckedItems.Count - 1)
                    ReDim downloadFiles(0 To CType(aControl, CheckedListBox).CheckedItems.Count - 1)
                End If
            Next

            'Add the drawing documents that the user has selected to the drawingList array 
            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    If CType(aControl, CheckedListBox).CheckedItems.Count <> 0 Then
                        For Each Me.anObject In CType(aControl, CheckedListBox).CheckedItems
                            If TypeOf anObject Is String Then
                                If drawingList.NotInList(anObject.ToString) Then
                                    drawingList.SetDocName(anObject.ToString)
                                End If
                            End If
                        Next
                    Else
                        Me.Cursor = Cursors.Default
                        MsgBox("You muse select at least" & Chr(13) & "one drawing from the list", MsgBoxStyle.Critical, "Select a part")
                        Me.Visible = True
                        Exit Sub
                    End If
                End If
            Next
       
            Me.Visible = False

            'Iterate through the list of documents selected by the user and build the drawingList and downloadFiles list
            'The drawingList has a path approprate for the call to FildLatestFilesByPath
            'The downloadFile has a path approprate for writing the files to disk
            For i = 0 To drawingList.DrawingName.Count - 1
                For j = 1 To invDocs.Count
                    If Not invDocs.Item(j).DocumentType = DocumentTypeEnum.kDrawingDocumentObject And invDocs.Item(j).FullFileName = "" Then
                        MessageBox.Show("The file " & invDocs.Item(j).DisplayName & "has not been saved and will not be printed", "File Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit For
                    End If
                    If invDocs.Item(j).DisplayName = drawingList.DrawingName.Item(i).ToString And Not invDocs.Item(j).FullFileName = Nothing Then
                        drawingFiles(i) = ChangeExtension(invDocs.Item(j).FullFileName.Replace("C:\_Vault_Working_Folder", "$").Replace("\", "/"), ".idw")
                        downloadFiles(i) = ChangeExtension(invDocs.Item(j).FullFileName, ".idw")                        
                    End If
                Next j
            Next i

            For i = 0 To drawingList.DrawingName.Count - 1
                drawingList.DrawingName.Item(i) = ChangeExtension(drawingList.DrawingName.Item(i).ToString, ".idw")
            Next

            'Generate the list of drawings to present to the user that will be printed
            If rbtnPrintFiles.Checked = True Then
                verifyForm.VerifyDialog_AddListBox(drawingList.DrawingName, DrawingListcontrols, _
                                               "DrawingsListDialogLable",
                                               "Print the following drawings?",
                                               "DrawingsListBox",
                                               "Selected Drawings")
                verifyForm.Text = "Verify Drawings to Print"
                verifyForm.Icon = My.Resources.printer

            Else
                verifyForm.VerifyDialog_AddListBox(drawingList.DrawingName, DrawingListcontrols, _
                                               "DrawingsListDialogLable",
                                               "Open the following drawings?",
                                               "DrawingsListBox",
                                               "Selected Drawings")
                verifyForm.Text = "Verify Drawings to Open"
                verifyForm.Icon = My.Resources.FileFolder
            End If

            verifyForm.ShowDialog()

            If verifyForm.bAcceptClicked = True Then

                Me.Cursor = Cursors.WaitCursor

                'Make the call to Vault to get the potential files to download.
                'The list is potential because there is no garantee that there is a drawing
                'file for every document selected.
                files = serverLogin.docSvc.FindLatestFilesByPaths(drawingFiles)

                'Iterate through the list of files to down load
                'Error checking for the existence of files is handled in the Vaultservices class
                For i = 0 To files.Length - 1
                    vaultService.Execute(files(i), downloadFiles(i), serverLogin)
                Next

                'We are finished with the Vault services so log out of the Vault
                serverLogin.LogoutOfVault()

                Dim invDrawingDocs(downloadFiles.Length - 1) As DrawingDocument


                'Iterate through all the files downloaded and open them as visable in Inventor
                'This is needed because Inventor won't print a drawing file unless it is visable
                For i = 0 To downloadFiles.Length - 1
                    If System.IO.File.Exists(downloadFiles(i)) Then
                        invDrawingDocs(i) = invDocs.Open(downloadFiles(i))
                        Dim info As FileInfo = New FileInfo(downloadFiles(i))
                        info.IsReadOnly = False
                        invDrawingDocs(i).Dirty = False
                        info.IsReadOnly = True
                        If rbtnPrintFiles.Checked = True Then
                            invDrawingDocs(i).PrintManager.SubmitPrint()
                            invDrawingDocs(i).Dirty = False
                            invDrawingDocs(i).Close()
                        End If
                    End If
                Next

                Me.Cursor = Cursors.Default
                Me.Dispose()

            Else

                strDrawingFileNames = ""
                drawingList.DrawingName.Clear()
                drawingList.DrawingIndex.Clear()
                anObject = Nothing
                Me.Cursor = Cursors.Default
                Me.Visible = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAll.CheckedChanged

        Try

            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    If bAllChecked = True Then
                        For i = 0 To CType(aControl, CheckedListBox).Items.Count - 1
                            CType(aControl, CheckedListBox).SetItemChecked(i, False)
                        Next
                        bAllChecked = False
                    Else
                        For i = 0 To CType(aControl, CheckedListBox).Items.Count - 1
                            CType(aControl, CheckedListBox).SetItemChecked(i, True)
                        Next
                        bAllChecked = True
                    End If
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp

    End Sub

    Private Sub SelectUnApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectUnApproved.CheckedChanged

        Try

            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    CType(aControl, CheckedListBox).Dispose()
                    DrawingsDialog_AddListBox(Me.Controls)
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

End Class
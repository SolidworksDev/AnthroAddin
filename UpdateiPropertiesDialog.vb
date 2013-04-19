Imports Inventor
Imports System.IO
Imports AnthroAddIn.ServerLogin
Imports AnthroAddIn.DocumentSvc
Imports System.Windows.Forms
Imports System.Collections.Generic


Public Class UpdateiPropertiesDialog

    Private invApp As Inventor.Application
    Private serverLogin As New ServerLogin
    Private drawingList As New DrawingDocuments
    Private docsList As New List(Of iPropDocs)
    Private bAllChecked As Boolean = False
    Private bLeaveiPropDlg As Boolean = False
    Public verifyList As New ArrayList

    ' Create a property so the controls can be easily retrieved.
    Public ReadOnly Property DocumentListFormControls() As Control.ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Function UpdateiPropertiesDialog_AddListBox(ByVal inControls As Control.ControlCollection) As Boolean

        Try

            Dim LablePosition As System.Drawing.Point
            LablePosition.X = 12
            LablePosition.Y = 45

            Dim ListPosition As System.Drawing.Point
            ListPosition.X = 12
            ListPosition.Y = 70

            Dim newList As New CheckedListBox
            Dim newLable As New System.Windows.Forms.Label

            Dim strListName As String

            newList.CheckOnClick = True

            newLable.Name = "DocumentsListDialogLable"
            newLable.Text = "Select documents to update"
            newLable.Width = 250
            newLable.Location = LablePosition

            newList.Text = "Select Documents"
            newList.Name = "DocumentsListBox"
            newList.Width = 260
            newList.Height = 270
            newList.Location = ListPosition

            Dim invAsmDoc As AssemblyDocument
            invAsmDoc = invApp.ActiveDocument
            newList.Items.Add(invAsmDoc.DisplayName + ".iam")
            If ShowUnApproved.Checked And IsApproved(invAsmDoc) Then
                newList.Items.Remove(invAsmDoc.DisplayName + ".iam")
            End If

            Dim invRefDocs As DocumentsEnumerator
            invRefDocs = invAsmDoc.AllReferencedDocuments

            Dim invRefDoc As Document

            For i = 1 To invRefDocs.Count
                invRefDoc = invRefDocs.Item(i)
                If IsDoc(invRefDoc.DisplayName) Then
                    Select Case invRefDoc.DocumentType
                        Case Inventor.DocumentTypeEnum.kAssemblyDocumentObject
                            strListName = invRefDoc.DisplayName + ".iam"
                            newList.Items.Add(strListName)
                            If ShowUnApproved.Checked And IsApproved(invRefDoc) Then
                                newList.Items.Remove(strListName)
                            End If
                        Case Inventor.DocumentTypeEnum.kPartDocumentObject
                            strListName = invRefDoc.DisplayName + ".ipt"
                            newList.Items.Add(strListName)
                            If ShowUnApproved.Checked And IsApproved(invRefDoc) Then
                                newList.Items.Remove(strListName)
                            End If
                    End Select
                End If
            Next

            newList.Sorted = True

            Dim strFirstnewLisName As String
            Dim strSecondnewListName As String
            Dim inewListCount As Integer = newList.Items.Count - 1

            For i = 0 To newList.Items.Count - 1
                If i < inewListCount Then
                    strFirstnewLisName = newList.Items.Item(i).ToString.Remove(newList.Items.Item(i).ToString.Length - 4)
                    strSecondnewListName = newList.Items.Item(i + 1).ToString.Remove(newList.Items.Item(i + 1).ToString.Length - 4)
                    If strFirstnewLisName = strSecondnewListName Then
                        newList.Items.Remove(newList.Items.Item(i + 1))
                        inewListCount = inewListCount - 1
                    End If
                Else
                    Exit For
                End If
            Next

            inControls.Add(newLable)
            inControls.Add(newList)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Public Sub SetiProperty(ByRef invDoc As Document)

        ' Get the PropertySets object.
        Dim oPropSets As PropertySets

        ' Get the design tracking property set.
        Dim oPropSet As PropertySet

        ' Get the part number iProperty.
        Dim oPartAuthorityProp As Inventor.Property

        ' Get the revision number iProperty
        Dim oPartRevisionProp As Inventor.Property

        Dim x As Integer

        oPropSets = invDoc.PropertySets
        oPropSet = oPropSets.Item("Design Tracking Properties")
        oPartAuthorityProp = oPropSet.Item("Authority")
        oPartAuthorityProp.Value = Me.txtBxApprovedBy.Text
        oPropSet = oPropSets.Item("Inventor Summary Information")
        oPartRevisionProp = oPropSet.Item("Revision Number")

        ' If the Revision Value is a numeric value of is an empty string change it to "-"
        If Integer.TryParse(oPartRevisionProp.Value.ToString, x) Or oPartRevisionProp.Value.ToString = "" Then
            oPartRevisionProp.Value = "-"
        End If

    End Sub

    Public Sub MoveECOBlocks(ByVal invRefDoc As Document, ByVal invDrawingDoc As Document)

        Dim invPropSets As PropertySets
        Dim invPropSet As PropertySet
        Dim invPartRevisionProp As Inventor.Property
        Dim invDrawingSheet As Sheet = Nothing
        Dim invSketchSymbol As SketchedSymbol = Nothing
        Dim ecoPosition As Point2d = Nothing
        Dim x As Integer

        Try

            invPropSets = invRefDoc.PropertySets
            invPropSet = invPropSets.Item("Inventor Summary Information")
            invPartRevisionProp = invPropSet.Item("Revision Number")

            If Integer.TryParse(invPartRevisionProp.Value.ToString, x) Or invPartRevisionProp.Value.ToString = "" Then
                For k = 1 To invDrawingDoc.Sheets.Count
                    invDrawingSheet = invDrawingDoc.Sheets.Item(k)
                    For l = 1 To invDrawingSheet.SketchedSymbols.Count
                        invSketchSymbol = invDrawingSheet.SketchedSymbols.Item(l)
                        If invSketchSymbol.Name.Substring(0, 4) = "ECO " Then
                            ecoPosition = invSketchSymbol.Position
                            ecoPosition.X = ecoPosition.X + 6.0
                            invSketchSymbol.Position = ecoPosition
                        End If
                    Next l
                Next k
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub CheckinDoc(ByVal invDoc As Document)

        Dim bytes() As Byte
        Dim vaultFile() As DocumentSvc.File
        Dim fullPath As String = invDoc.FullDocumentName
        Dim fullDocName() As String = {invDoc.FullDocumentName}
        Dim oDate As Date
        Dim info As System.IO.FileInfo = New FileInfo(invDoc.FullDocumentName)

        Dim invRefDocs As DocumentsEnumerator = invDoc.ReferencedDocuments
        Dim strRefDocs(0 To invRefDocs.Count - 1) As String

        Try

            fullDocName = {invDoc.FullDocumentName.Replace("C:\_Vault_Working_Folder", "$").Replace("\", "/")}
            vaultFile = serverLogin.docSvc.FindLatestFilesByPaths(fullDocName)


            If invRefDocs.Count <> 0 Then

                For j = 0 To invRefDocs.Count - 1
                    strRefDocs(j) = invRefDocs.Item(j + 1).FullDocumentName.Replace("C:\_Vault_Working_Folder", "$").Replace("\", "/")
                Next

                Dim dependentRefFiles() As DocumentSvc.File = serverLogin.docSvc.FindLatestFilesByPaths(strRefDocs)
                Dim dependentRefFileIds(0 To dependentRefFiles.Length - 1) As Long
                Dim dependentRefSources(0 To dependentRefFiles.Length - 1) As String

                For j = 0 To dependentRefFiles.Length - 1
                    dependentRefFileIds(j) = dependentRefFiles(j).Id
                    dependentRefSources(j) = Nothing

                Next

                bytes = System.IO.File.ReadAllBytes(invDoc.FullDocumentName)
                oDate = System.IO.File.GetLastAccessTime(invDoc.FullDocumentName)
                serverLogin.docSvc.CheckinFile(vaultFile(0).MasterId, "Drawing Approval", False,
                                                   oDate, Nothing, Nothing, True,
                                                   vaultFile(0).Name, vaultFile(0).FileClass, vaultFile(0).Hidden, bytes)

                info.IsReadOnly = False
                info.CreationTime = vaultFile(0).CreateDate
                info.IsReadOnly = True

            Else

                bytes = System.IO.File.ReadAllBytes(invDoc.FullDocumentName)
                oDate = System.IO.File.GetLastAccessTime(invDoc.FullDocumentName)
                serverLogin.docSvc.CheckinFile(vaultFile(0).MasterId, "Drawing Approval", False,
                                                   oDate, Nothing, Nothing, True,
                                                   vaultFile(0).Name, vaultFile(0).FileClass, vaultFile(0).Hidden, bytes)

                info.IsReadOnly = False
                info.CreationTime = vaultFile(0).CreateDate
                info.IsReadOnly = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim invDocs As Documents = invApp.Documents                      
        Dim ecoPosition As Point2d = Nothing
        Dim drawingFiles() As DocumentSvc.File = {}
        Dim refFiles() As DocumentSvc.File = {}
        Dim strDrawingFiles() As String = {}
        Dim strDownloadFiles() As String = {}       
        Dim strRefFiles() As String = Nothing
        Dim strFolders() As String = {}
        Dim strLocalPath() As String = Nothing
        Dim vaultService As New VaultServices          
        Dim invCurrentRefDoc As Document
        Dim invCurrentDrawingDoc As Document       
        Dim bytes As Byte() = {}
        Dim machineName As String = System.Environment.MachineName
        Dim folders() As DocumentSvc.Folder
        Dim verifyForm As New VerifyForm
        Dim DrawingListcontrols As Control.ControlCollection = verifyForm.DrawingListFormControls
        Dim progress As New ProgressDialog

        serverLogin.LoginToVault(HOST)

        Try

            Me.Visible = False
            Me.Cursor = Cursors.WaitCursor

            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    If CType(aControl, CheckedListBox).CheckedItems.Count <> 0 Then
                        For Each anObject In CType(aControl, CheckedListBox).CheckedItems
                            If TypeOf anObject Is String Then
                                If drawingList.NotInList(anObject.ToString) Then
                                    Dim currentDoc As New iPropDocs
                                    currentDoc.SetRefFile(anObject.ToString)
                                    docsList.Add(currentDoc)
                                End If
                            End If
                        Next
                    Else
                        Me.Cursor = Cursors.Default
                        MsgBox("You muse select at least" & Chr(13) & "one document from the list", MsgBoxStyle.Critical, "Select a part")
                        Me.Visible = True
                        Exit Sub
                    End If
                End If
            Next

            For i = 0 To docsList.Count - 1
                For j = 1 To invDocs.Count
                    If docsList.Item(i).StripExt(docsList.Item(i).GetRefFile()) = invDocs.Item(j).DisplayName() Then
                        docsList.Item(i).SetRefIndex(j)
                        docsList.Item(i).SetFullRefName(invDocs.Item(j).FullDocumentName())
                        docsList.Item(i).SetFullDawingName(invDocs.Item(j).FullDocumentName())
                        docsList.Item(i).SetRefPath(invDocs.Item(j))
                        docsList.Item(i).SetDrawingFile(invDocs.Item(j).DisplayName())
                        docsList.Item(i).SetDrawingPath(invDocs.Item(j))
                        Exit For
                    End If
                Next j
            Next i

            ReDim strDrawingFiles(0 To docsList.Count - 1)
            ReDim strDownloadFiles(0 To docsList.Count - 1)
            ReDim strFolders(0 To docsList.Count - 1)
            ReDim strRefFiles(0 To docsList.Count - 1)
            ReDim strLocalPath(0 To docsList.Count - 1)            

            For i = 0 To docsList.Count - 1
                strDrawingFiles(i) = docsList.Item(i).GetDrawingVaultPathName()
                strDownloadFiles(i) = docsList.Item(i).GetDrawingPathName
                strRefFiles(i) = docsList.Item(i).GetRefVaultPathName()
                strLocalPath(i) = docsList.Item(i).GetRefPath()
                strFolders(i) = docsList.Item(i).GetRefVaultPath()
            Next

            For i = 0 To docsList.Count - 1
                verifyList.Add(docsList.Item(i).GetDrawingFile())
            Next

            verifyForm.VerifyDialog_AddListBox(verifyList, DrawingListcontrols, _
                                              "iPropListDialogLable",
                                              "Update the following component iProperties?",
                                              "iPropListBox",
                                              "Selected Conents")

            verifyForm.Text = "Verify iProperty Updates"
            verifyForm.Icon = My.Resources.iProperty
            verifyForm.ShowDialog(New WindowWrapper(invApp.MainFrameHWND))

            If verifyForm.bAcceptClicked = True Then

                Me.Cursor = Cursors.WaitCursor

                progress.ProgressBar1.Minimum = 0
                progress.ProgressBar1.Maximum = docsList.Count
                progress.ProgressBar1.Value = 0

                Dim FoldersString As String = strFolders(0).ToString + Chr(13)

                For i = 1 To strFolders.Length - 1
                    FoldersString = FoldersString + strFolders(i).ToString + Chr(13)
                Next                

                folders = serverLogin.docSvc.GetFoldersByPaths(strFolders)

                For i = 0 To folders.Length - 1
                    FoldersString = FoldersString + folders(i).FullName + Chr(13)
                Next

                'Get the latest version of the drawing files from the Vault server
                drawingFiles = serverLogin.docSvc.FindLatestFilesByPaths(strDrawingFiles)

                'Tag any drawings that do no exist by setting there name to Nothing
                For i = 0 To drawingFiles.Length - 1
                    If drawingFiles(i).Name = Nothing Then
                        docsList.Item(i).SetDrawingFile(Nothing)
                    End If
                Next

                'Download the latest version of the drawing files and write them to the local disk
                For i = 0 To drawingFiles.Length - 1
                    If drawingFiles(i).Name <> Nothing Then
                        vaultService.Execute(drawingFiles(i), strDownloadFiles(i), serverLogin)
                    End If
                Next

                progress.Show(New WindowWrapper(invApp.MainFrameHWND))

                'Open all the drawing files that have been downloaded to the local disk but don't show them
                For i = 0 To drawingFiles.Length - 1
                    If drawingFiles(i).Name <> Nothing Then
                        invDocs.Open(strDownloadFiles(i), False)
                    End If
                Next

                'Get the latest version of the Ref files from the Vault server
                refFiles = serverLogin.docSvc.FindLatestFilesByPaths(strRefFiles)

                'Check out the ref files
                For i = 0 To docsList.Count - 1

                    progress.ProgressBar1.Value = i + 1

                    If docsList(i).GetDrawingFile <> Nothing Then

                        If Not refFiles(i).CheckedOut Then
                            serverLogin.docSvc.CheckoutFile(folders(i).Id, refFiles(i).Id, CheckoutFileOptions.Master,
                                                            machineName, strLocalPath(i), "", False, False, bytes)
                        End If

                        If Not drawingFiles(i).CheckedOut Then
                            serverLogin.docSvc.CheckoutFile(folders(i).Id, drawingFiles(i).Id, CheckoutFileOptions.Master,
                                                            machineName, strLocalPath(i), "", False, False, bytes)
                        End If

                        invCurrentRefDoc = invDocs.ItemByName(docsList.Item(i).GetRefPathName())
                        invCurrentDrawingDoc = invDocs.ItemByName(docsList.Item(i).GetDrawingPathName())

                        MoveECOBlocks(invCurrentRefDoc, invCurrentDrawingDoc)
                        SetiProperty(invCurrentRefDoc)

                        invApp.SilentOperation = True
                        invCurrentDrawingDoc.Save2(True)
                        invApp.SilentOperation = False

                        CheckinDoc(invCurrentRefDoc)
                        CheckinDoc(invCurrentDrawingDoc)

                    End If
                Next

                Me.Cursor = Cursors.Default
                progress.Close()
                serverLogin.LogoutOfVault()
                Me.Close()

            Else
                Me.Cursor = Cursors.Default
                Me.Visible = True

            End If
                        
        Catch ex As Exception
            progress.Close()
            MessageBox.Show(ex.ToString)
        End Try


    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
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

    Private Sub txtBxApprovedBy_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBxApprovedBy.Validating

        Try

            If Not AllLetters(Me.txtBxApprovedBy.Text) Then
                ErrorProvider1.SetError(txtBxApprovedBy, "You must imput a valid apporved by initial")
                e.Cancel = True
            Else
                ErrorProvider1.SetError(txtBxApprovedBy, "")
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnCancel_MouseClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseHover
        bLeaveiPropDlg = True
    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp

    End Sub

    Private Sub ShowUnApproved_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowUnApproved.CheckedChanged

        Try

            For Each aControl In Me.Controls
                If TypeName(aControl) = "CheckedListBox" Then
                    CType(aControl, CheckedListBox).Dispose()
                    UpdateiPropertiesDialog_AddListBox(Me.Controls)
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

End Class

#Region "hWnd Wrapper Class"
' This class is used to wrap a Win32 hWnd as a .Net IWind32Window class.
' This is primarily used for parenting a dialog to the Inventor window.'
' For example:
' myForm.Show(New WindowWrapper(m_inventorApplication.MainFrameHWND))
Public Class WindowWrapper
    Implements System.Windows.Forms.IWin32Window

    Public Sub New(ByVal handle As IntPtr)
        _hwnd = handle
    End Sub
    Public ReadOnly Property Handle() As IntPtr Implements System.Windows.Forms.IWin32Window.Handle
        Get
            Return _hwnd
        End Get
    End Property
    Private _hwnd As IntPtr
End Class
#End Region
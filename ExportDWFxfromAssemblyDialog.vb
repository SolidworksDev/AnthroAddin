Imports Inventor
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.IO.File
Imports System.Security
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity
Imports System.IO.Path
Imports ACW = Autodesk.Connectivity.WebServices
Imports VDF = Autodesk.DataManagement.Client.Framework

Public Class ExportDWFxfromAssemblyDialog

    Public invApp As Inventor.Application
    Public drawingList As New ArrayList
    'Public drawingList As New DrawingDocuments
    Public serverLogin As New ServerLogin
    Private bAllChecked As Boolean = False
    Private exportPath As String = "\\ANTHRO3\dwf\Standard\"
    Private exportDXFPath As String = "\\ANTHRO3\DXF\"
    Private strFileName As String = ""
    Private bCurrentSettings As Boolean = False
    Public Shared bAcceptClicked As Boolean = False
    Public Shared bCancelClicked As Boolean = False
    Private anObject As Object
    Private strDrawingFileNames As String = ""
    Private drawingsList As New List(Of iPropDocs)

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
            newLable.Text = "Select Drawings to Export"
            newLable.Width = 200
            newLable.Location = LablePosition

            newList.Text = "Select Drawings"
            newList.Name = "DrawingsListBox"
            newList.Width = 215
            newList.Height = 280
            newList.Location = ListPosition

            Dim invAsmDoc As AssemblyDocument
            invAsmDoc = invApp.ActiveDocument
            newList.Items.Add(invAsmDoc.DisplayName)

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

        anObject = Nothing
        Me.Dispose()

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        serverLogin.LoginToVault(HOST)

        Dim files() As ACW.File = {}

        Dim vaultService As New VaultServices

        Dim aControl As Control
        Dim anObject As Object

        Dim i As Integer
        Dim j As Integer

        Dim intUserResponse As Integer = 0

        Dim invDocs As Documents = invApp.Documents
        Dim invDrawingDoc As DrawingDocument

        Dim drawingFiles() As String = {}
        Dim downloadFiles() As String = {}
        Dim strDrawingFiles() As String = {}
        Dim strDownloadFiles() As String = {}
        Dim strRefFiles() As String = {}
        Dim strFolders() As String = {}
        Dim strLocalPath() As String = {}
        Dim strDrawingFileName As String
        Dim bFileExists As Boolean
        Dim UnsavedDrawing As DrawingDocument
        Dim verifyForm As New VerifyForm
        Dim DrawingListcontrols As Control.ControlCollection = verifyForm.DrawingListFormControls

        Me.Cursor = Cursors.WaitCursor
        Me.Visible = False

        bCurrentSettings = invApp.SilentOperation

        Try

            invApp.SilentOperation = True

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
                        For Each anObject In CType(aControl, CheckedListBox).CheckedItems
                            If TypeOf anObject Is String Then
                                Dim currentDoc As New iPropDocs
                                currentDoc.SetRefFile(anObject.ToString)
                                drawingsList.Add(currentDoc)
                                Me.Cursor = Cursors.Default
                            End If
                        Next
                    Else
                        Me.Cursor = Cursors.Default
                        MsgBox("You muse select at lease" & Chr(13) & "one drawing from the list", MsgBoxStyle.Critical, "Select a part")
                        Me.Visible = True
                        Exit Sub
                    End If
                End If
            Next

            For i = 0 To drawingsList.Count - 1
                For j = 1 To invDocs.Count
                    If drawingsList.Item(i).GetRefFile() = invDocs.Item(j).DisplayName() Then
                        drawingsList.Item(i).SetRefIndex(j)
                        drawingsList.Item(i).SetFullRefName(invDocs.Item(j).FullDocumentName())
                        drawingsList.Item(i).SetFullDawingName(invDocs.Item(j).FullDocumentName())
                        drawingsList.Item(i).SetRefPath(invDocs.Item(j))
                        drawingsList.Item(i).SetDrawingFile(ChangeExtension(invDocs.Item(j).DisplayName(), "idw"))
                        drawingsList.Item(i).SetDrawingPath(invDocs.Item(j))
                        Exit For
                    End If
                Next j
            Next i

            ReDim strDrawingFiles(0 To drawingsList.Count - 1)
            ReDim strDownloadFiles(0 To drawingsList.Count - 1)
            ReDim strFolders(0 To drawingsList.Count - 1)
            ReDim strRefFiles(0 To drawingsList.Count - 1)
            ReDim strLocalPath(0 To drawingsList.Count - 1)

            For i = 0 To drawingsList.Count - 1
                strDrawingFiles(i) = drawingsList.Item(i).GetDrawingVaultPathName()
                strDownloadFiles(i) = drawingsList.Item(i).GetDrawingPathName
                strRefFiles(i) = drawingsList.Item(i).GetRefVaultPathName()
                strLocalPath(i) = drawingsList.Item(i).GetRefPath()
                strFolders(i) = drawingsList.Item(i).GetRefVaultPath()
            Next
           
            Me.Visible = False

            For i = 0 To drawingsList.Count - 1
                drawingList.Add(drawingsList.Item(i).GetDrawingFile())
            Next

            verifyForm.VerifyDialog_AddListBox(drawingList, DrawingListcontrols, _
                                               "DrawingsListDialogLable",
                                               "Export the following drawings?",
                                               "DrawingsListBox",
                                               "Select Drawings")

            verifyForm.Text = "Verify Export DWFx"
            verifyForm.Icon = My.Resources.DWF_Viewer
            verifyForm.ShowDialog()

            If verifyForm.bAcceptClicked = True Then

                'Make the call to Vault to get the potential files to download.
                'The list is potential because there is no garantee that there is a drawing
                'file for every document selected.
                files = serverLogin.connection.WebServiceManager.DocumentService.FindLatestFilesByPaths(strDrawingFiles)

                Dim fileIters As List(Of VDF.Vault.Currency.Entities.FileIteration) = New List(Of VDF.Vault.Currency.Entities.FileIteration)
                For Each vFile In files
                    fileIters.Add(New VDF.Vault.Currency.Entities.FileIteration(serverLogin.connection, vFile))
                Next

                'Iterate through the list of files to down load
                'Error checking for the existence of files is handled in the Vaultservices class
                'vaultService.DownloadDialog(fileIters.Item(0), serverLogin, New WindowWrapper(invApp.MainFrameHWND))
                vaultService.Execute(fileIters, strDownloadFiles, serverLogin)


                'We are finished with the Vault services so log out of the Vault
                serverLogin.LogoutOfVault()

                'Iterate through all the files downloaded and open them as visable in Inventor
                'This is needed because Inventor won't print a drawing file unless it is visable
                For i = 0 To drawingsList.Count - 1
                    strDrawingFileName = drawingsList.Item(i).GetDrawingPathName()
                    bFileExists = FileInUse(strDrawingFileName)
                    If (bFileExists = False) Then
                        'This code fixes a bug that occurs when the user has a drawing that is open and has not been saved when exporting a .dwfx
                        For j = 1 To invDocs.Count
                            If drawingsList.Item(i).GetRefFile() = invDocs.Item(j).DisplayName() _
                                And invDocs.Item(j).DocumentType = DocumentTypeEnum.kDrawingDocumentObject Then
                                UnsavedDrawing = invDocs.Item(j)
                                UnsavedDrawing.SaveAs(strDrawingFileName, False)
                                MessageBox.Show("File " & UnsavedDrawing.DisplayName & ".idw" & Chr(13) & _
                                        " has been saved. Please check the file into Valut", "File Saved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                        Next
                    End If
                    invDrawingDoc = invDocs.Open(strDrawingFileName)
                    invDrawingDoc.SaveAs(exportPath & ChangeExtension(invDrawingDoc.DisplayName(), "dwfx"), True)
                    'Added to export .dxf file to new location'
                    'invDrawingDoc.SaveAs(exportDXFPath & invDrawingDoc.DisplayName & ".dxf", True)
                    invDrawingDoc.Close()

                Next

                invApp.SilentOperation = bCurrentSettings
                Me.Cursor = Cursors.Default
                Me.Dispose()
            Else
                aControl = Nothing
                anObject = Nothing
                Me.Cursor = Cursors.Default
                Me.Visible = True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAll.CheckedChanged

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

    End Sub

    Public Sub New(ByVal ThisApplication As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApplication
    End Sub
    'Determine whether a file is already open or not
    Private Declare Function lOpen Lib "kernel32" Alias "_lopen" (ByVal lpPathName As String, ByVal iReadWrite As Long) As Long
    Private Declare Function lClose Lib "kernel32" Alias "_lclose" (ByVal hFile As Long) As Long
    Private Function IsFileAlreadyOpen(FileName As String) As Boolean
        Dim hFile As Long
        Dim lastErr As Long
        ' Initialize file handle and error variable.
        hFile = -1
        lastErr = 0
        ' Open for for read and exclusive sharing.
        hFile = lOpen(FileName, &H10)
        ' If we couldn't open the file, get the last error.
        If hFile = -1 Then
            lastErr = Err.LastDllError
        Else
            ' Make sure we close the file on success.
            lClose(hFile)
        End If
        ' Check for sharing violation error.
        IsFileAlreadyOpen = (hFile = -1) And (lastErr = 32)
    End Function
    
   Public Function FileInUse(ByVal sFile As String) As Boolean 
        Dim thisFileInUse As Boolean = False
        If System.IO.File.Exists(sFile) Then
            Try
                Using f As New IO.FileStream(sFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                    thisFileInUse = False
                End Using
            Catch
                thisFileInUse = True
            End Try
        End If
        Return thisFileInUse
    End Function

End Class




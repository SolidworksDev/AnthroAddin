Imports Inventor
Imports System.Windows.Forms
Imports System.IO.Path

Public Class ExportDXFfromAssemblyDialog

    Public invApp As Inventor.Application
    Public DocList As New DXFDocuments
    Public strDXFLocation As String = "\\svr12T\TRUMPF.NET\DXF\"
    'Public strDXFLocation As String = "C:\Users\tclift\Documents\YESMORSP\"
    Private bAllChecked As Boolean = False

    ' Create a property so the controls can be easily retrieved.
    Public ReadOnly Property PartsListFormControls() As Control.ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Function IsSheetMetal(ByVal strPartName As String) As Boolean

        If strPartName.Length <= 3 Then
            Return False
        End If

        Dim strFirstFourChr As String
        strFirstFourChr = strPartName.Substring(0, 4)
        Dim strFirstTwoChr As String
        strFirstTwoChr = strPartName.Substring(0, 2)

        If strFirstFourChr = "225-" Or strFirstTwoChr = "M-" Then
            IsSheetMetal = True
        Else
            IsSheetMetal = False
        End If

    End Function

    Public Function PartsDialog_AddListBox(ByVal inControls As Control.ControlCollection, _
                                           ByVal invDocs As Documents) As Boolean

        Dim LablePosition As System.Drawing.Point
        LablePosition.X = 10
        LablePosition.Y = 10

        Dim ListPosition As System.Drawing.Point
        ListPosition.X = 10
        ListPosition.Y = 35

        Dim newList As New CheckedListBox
        Dim newLable As New Label

        newList.CheckOnClick = True

        newLable.Name = "PartsListDialogLable"
        newLable.Text = "Select parts to exprot"
        newLable.Width = 200
        newLable.Location = LablePosition

        newList.Text = "Select Parts to Export"
        newList.Name = "PartsListBox"
        newList.Width = 215
        newList.Height = 280
        newList.Location = ListPosition

        Dim invAsmDoc As AssemblyDocument
        invAsmDoc = invApp.ActiveDocument        

        Dim invRefDocs As DocumentsEnumerator
        invRefDocs = invAsmDoc.AllReferencedDocuments

        Dim invRefDoc As Document

        For Each invRefDoc In invRefDocs
            If IsSheetMetal(invRefDoc.DisplayName) And _
                invRefDoc.DocumentType = DocumentTypeEnum.kPartDocumentObject Then
                newList.Items.Add(invRefDoc.DisplayName)
            End If
        Next

        inControls.Add(newLable)
        newList.Sorted = True
        inControls.Add(newList)

    End Function

    Private Sub ExportToServer(ByVal invDocs As Documents, ByVal DocList As DXFDocuments)

        Dim strFullPathWithName As String

        Dim i As Integer
        Dim invDoc As Inventor.Document

        ' Get the DataIO object.
        Dim invDataIO As DataIO
        ' Build the string that defines the format of the DXF file.
        Dim sOut As String
        sOut = "FLAT PATTERN DXF?AcadVersion=2000&InvisibleLayers=IV_TANGENT;IV_ARC_CENTERS;IV_ALTREP_FRONT;IV_ALTREP_BACK;IV_UNCOMSUMED_SKETCHES;IV_ROLL_TANGENT;IV_ROLL"
        'sOut = ""
        For i = 0 To DocList.DocIndex.Count - 1
            invDoc = invDocs(DocList.DocIndex.Item(i))
            strFullPathWithName = strDXFLocation + ChangeExtension(invDoc.DisplayName(), ".dxf")
            invDoc.SaveAs(strFullPathWithName, True)
            invDataIO = invDoc.ComponentDefinition.DataIO


            ' Create the DXF file.
            invDataIO.WriteDataToFile(sOut, strFullPathWithName)
        Next

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim aControl As Control
        Dim anObject As Object
        Dim i As Integer
        Dim j As Integer
        Dim intUserResponse As Integer = 0

        Dim invDocs As Documents = invApp.Documents
        Dim verifyForm As New VerifyForm
        Dim PartsListcontrols As Control.ControlCollection = verifyForm.DrawingListFormControls

        For Each aControl In Me.Controls
            If TypeName(aControl) = "CheckedListBox" Then
                If CType(aControl, CheckedListBox).CheckedItems.Count <> 0 Then
                    For Each anObject In CType(aControl, CheckedListBox).CheckedItems
                        If TypeOf anObject Is String Then
                            DocList.SetDocName(anObject.ToString)
                        End If
                    Next
                Else
                    MsgBox("You muse select at lease" & Chr(13) & "one part from the list", MsgBoxStyle.Critical, "Select a part")
                    Exit Sub
                End If
            End If
        Next


        Me.Visible = False

        For i = 0 To DocList.DocName.Count - 1
            For j = 1 To invDocs.Count
                If invDocs.Item(j).DisplayName = DocList.DocName.Item(i).ToString And _
                    invDocs.Item(j).DocumentType = DocumentTypeEnum.kPartDocumentObject Then
                    DocList.SetDocIndex(j)
                End If
            Next j
        Next i

        verifyForm.VerifyDialog_AddListBox(DocList.DocName, PartsListcontrols, _
                                              "DrawingsListDialogLable",
                                              "Export the following drawings?",
                                              "DrawingsListBox",
                                              "Select Drawings")

        verifyForm.Text = "Verify Export DXF"
        verifyForm.Icon = My.Resources.DXFIcon
        VerifyForm.ShowDialog()

        If verifyForm.bAcceptClicked = True Then
            ExportToServer(invDocs, DocList)
            Me.Close()
        Else
            Me.Visible = True
            DocList.DocName.Clear()
            DocList.DocIndex.Clear()
            aControl = Nothing
            anObject = Nothing
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DocList.DocName.Clear()
        DocList.DocIndex.Clear()
        Me.Close()
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

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp

    End Sub
End Class
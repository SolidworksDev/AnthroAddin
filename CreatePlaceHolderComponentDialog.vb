Imports Inventor
Imports System.Windows.Forms
Imports System.IO
Imports Microsoft

Public Class CreatePlaceHolderComponentDialog

    Private invApp As Inventor.Application
    Private serverLogin As New ServerLogin
    Private bLeavingPlaceHolderComponentDlg As Boolean = False
    Private strFolderName As String

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ErrorProvider1.Clear()
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Try
            serverLogin.LoginToVault(HOST)
            Dim vaultFile() As DocumentSvc.File
            Dim vaultFolder As DocumentSvc.Folder
            Dim newVaultFolder As DocumentSvc.Folder
            Dim strComponentPlaceholderName As String = VisualBasic.Left(cbxMaterialGroup.Text, 3) + "-" + txtbxComponentPartNumber.Text
            Dim strFullPartFileName() As String = {"$/Design/Autocad/Parts/" + cbxMaterialGroup.Text + "/" + strComponentPlaceholderName + "/" + strComponentPlaceholderName + ".ipt"}
            Dim strFullAssemblyFileName() As String = {"$/Design/Autocad/Parts/" + cbxMaterialGroup.Text + "/" + strComponentPlaceholderName + "/" + strComponentPlaceholderName + ".iam"}
            Dim strParentFolder As String = "$/Design/Autocad/Parts/" + cbxMaterialGroup.Text + "/"
            Dim vaultParentFolder As DocumentSvc.Folder

            strFolderName = strComponentPlaceholderName
            vaultFile = serverLogin.docSvc.FindLatestFilesByPaths(strFullPartFileName)

            If vaultFile(0).Id <> -1 Then
                MessageBox.Show("The file " + strComponentPlaceholderName + ".ipt" + " already exist" _
                                + Chr(13) + "Please select a part number that does not exist")
                Exit Sub
            End If

            vaultFile = serverLogin.docSvc.FindLatestFilesByPaths(strFullAssemblyFileName)

            If vaultFile(0).Id <> -1 Then
                MessageBox.Show("The file " + strComponentPlaceholderName + ".iam" + " already exist" _
                                + Chr(13) + "Please select a part number that does not exist")
                Exit Sub
            End If

            Dim invSheetMetalDoc As PartDocument = invApp.ActiveDocument
            Dim invSheetMetalCompDef As SheetMetalComponentDefinition = invSheetMetalDoc.ComponentDefinition
            Dim invTransGeom As TransientGeometry = invApp.TransientGeometry
            Dim invSketch As PlanarSketch = invSheetMetalCompDef.Sketches.Item(invSheetMetalCompDef.Sketches.Count)
            Dim invTextSketch As PlanarSketch = invSheetMetalCompDef.Sketches.Add(invSheetMetalDoc.ComponentDefinition.WorkPlanes(3))
            invSketch.Edit()
            Dim UpperLeft As Point2d = invTransGeom.CreatePoint2d(-1.27, 0.635)
            Dim LowerRight As Point2d = invTransGeom.CreatePoint2d(1.27, -0.635)
            Dim invRectangleLines As SketchEntitiesEnumerator = invSketch.SketchLines.AddAsTwoPointRectangle(UpperLeft, LowerRight)
            invSketch.ExitEdit()
            Dim invFaceProfile As Profile = invSketch.Profiles.AddForSolid()
            Dim invExtrusion As ExtrudeFeature
            invExtrusion = invSheetMetalCompDef.Features.ExtrudeFeatures.AddByDistanceExtent(invFaceProfile, 0.00254, PartFeatureExtentDirectionEnum.kNegativeExtentDirection, PartFeatureOperationEnum.kJoinOperation)
            invSketch.Shared = False
            invSketch.Visible = False            
            Dim invTextBox As Inventor.TextBox = invTextSketch.TextBoxes.AddFitted(invTransGeom.CreatePoint2d(-1.18, 0.158), strFolderName)
            invTextBox.FormattedText = "<StyleOverride FontSize='0.3'>" & strFolderName & "</StyleOverride>"
            SetDescriptioniProp(invSheetMetalDoc)
            Dim filePath As String = "C:\_Vault_Working_Folder\Design\Autocad\Parts\" + cbxMaterialGroup.Text + "\" + strComponentPlaceholderName
            Directory.CreateDirectory(filePath)
            Dim fileName As String = strComponentPlaceholderName + ".ipt"
            invSheetMetalDoc.SaveAs(filePath + "\" + fileName, False)

            If rbtnCheckin.Checked = True Then
                Dim bytes() As Byte = System.IO.File.ReadAllBytes(filePath + "\" + fileName)
                vaultParentFolder = serverLogin.docSvc.GetFolderByPath(strParentFolder)
                newVaultFolder = serverLogin.docSvc.AddFolder(strFolderName, vaultParentFolder.Id, False)
                vaultFolder = serverLogin.docSvc.GetFolderById(newVaultFolder.Id)
                serverLogin.docSvc.AddFile(vaultFolder.Id, fileName, "Initial check-in", System.IO.File.GetLastAccessTime(filePath), Nothing, Nothing, Nothing, Nothing, Nothing, DocumentSvc.FileClassification.None, False, bytes)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Me.Close()

    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp

    End Sub

    Protected Overrides Function ProcessKeyPreview(ByRef m As System.Windows.Forms.Message) As Boolean

        Try
            Const WM_KEYDOWN As Integer = &H100
            Dim keycode As System.Windows.Forms.Keys = CType(CInt(m.WParam), System.Windows.Forms.Keys) And Windows.Forms.Keys.KeyCode

            If (m.Msg = WM_KEYDOWN And keycode = Windows.Forms.Keys.Tab) Then
                Return Me.ProcessTabKey(True)
            End If
            Return MyBase.ProcessKeyPreview(m)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Private Sub txtbxComponentPartNumber_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbxComponentPartNumber.TextChanged

        Try
            txtbxComponentPartNumber.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtbxComponentPartNumber_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtbxComponentPartNumber.Validating

        If bLeavingPlaceHolderComponentDlg = True Then
            e.Cancel = False
            Return
        End If

        Try

            If String.IsNullOrEmpty(txtbxComponentPartNumber.Text) Then
                txtbxComponentPartNumber.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbxComponentPartNumber, "You must enter a Part Number")
                e.Cancel = True
                Exit Sub
            End If
            
            Dim iPartNumberLength As Integer = Microsoft.VisualBasic.Strings.Len(txtbxComponentPartNumber.Text)
            If iPartNumberLength <> 7 Then
                txtbxComponentPartNumber.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbxComponentPartNumber, "Invalid part number length")
                e.Cancel = True
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtbxComponentDescription_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbxComponentDescription.TextChanged

        Try
            txtbxComponentDescription.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtbxComponentDescription_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtbxComponentDescription.Validating

        If bLeavingPlaceHolderComponentDlg = True Then
            e.Cancel = False
            Return
        End If

        Try
            If String.IsNullOrEmpty(txtbxComponentDescription.Text) Then
                txtbxComponentDescription.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbxComponentDescription, "You must enter a Part Description")
                e.Cancel = True
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnCancel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnCancel.MouseDown
        bLeavingPlaceHolderComponentDlg = True
    End Sub

    Private Sub btnCancel_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseHover
        bLeavingPlaceHolderComponentDlg = True
    End Sub

    Private Sub btnCancel_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseLeave
        bLeavingPlaceHolderComponentDlg = False
    End Sub

    Private Sub SetDescriptioniProp(ByVal invDoc As Document)
        ' Get the PropertySets object.
        Dim oPropSets As PropertySets

        ' Get the design tracking property set.
        Dim oPropSet As PropertySet

        ' Get the part number iProperty.
        Dim oPartDescriptionProp As Inventor.Property

        oPropSets = invDoc.PropertySets
        oPropSet = oPropSets.Item("Design Tracking Properties")
        oPartDescriptionProp = oPropSet.Item("Description")
        oPartDescriptionProp.Value = Me.txtbxComponentDescription.Text

    End Sub

    Private Function ValidateStandardPart(ByVal MaterialGroup As String) As String

        Select Case MaterialGroup
            Case "100-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "101-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "200-"
                Return "200 (Wire)"
            Case "225-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "325-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "375-"
                Return "375 (tools)"
            Case "400-"
                Return "400 (electrical)"
            Case "425-"
                Return "425 (Inner Packaging)"
            Case "450-"
                Return "450 (OuterPacking Material"
            Case "500-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "525"
                Return "525 (Machined Stl)"
            Case "575-"
                Return "575 (Mechanisms)"
            Case "580-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "581-"
                Return "581 (Casegood Hardware)"
            Case "625-"
                Return "625 (Phantom-Assembly)"
            Case "725-"
                Return "725 (Labels)"
            Case "825-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "835-"
                MessageBox.Show("A place holder for the material group " + MaterialGroup + Chr(13) + "not be created using this tool")
                Return Nothing
            Case "999-"
                Return "999 (Customer Supplied Material)"
            Case Else
                Return Nothing
        End Select

    End Function

    Private Sub cbxMaterialGroup_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cbxMaterialGroup.Validating

        If bLeavingPlaceHolderComponentDlg = True Then
            e.Cancel = False
            Return
        End If

        If String.IsNullOrEmpty(cbxMaterialGroup.Text) Then            
            ErrorProvider1.SetError(cbxMaterialGroup, "You must select a Material Group")
            e.Cancel = True
            Exit Sub
        End If

    End Sub

End Class
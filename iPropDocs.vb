Imports Inventor
Imports System.IO.Path

Public Class iPropDocs

    Private strDrawingFile As String
    Private strRefFile As String
    Private strDrawingPath As String
    Private strRefPath As String
    Private strRefVaultPath As String
    Private strDrawingVaultPath As String
    Private strFullDrawingName As String
    Private strFullRefName As String
    Private iRefIndex As Integer

    Public Sub SetDrawingFile(ByVal Name As String)

        If Name = Nothing Then
            strDrawingFile = Nothing
        Else
            strDrawingFile = Name
        End If

    End Sub

    Public Sub SetFullDawingName(ByVal Name As String)
        strFullDrawingName = Name
    End Sub

    Public Function GetDrawingFile() As String
        Return strDrawingFile
    End Function

    Public Sub SetDrawingPath(ByVal Ref As Document)        
        strDrawingPath = GetDirectoryName(Ref.FullFileName) + "\"
        strDrawingVaultPath = strDrawingPath.Replace("C:\_Vault_Working_Folder", "$").Replace("\", "/")
    End Sub

    Public Function GetDrawingPath() As String
        Return strDrawingPath
    End Function

    Public Function GetDrawingVaultPathName() As String
        Return strDrawingVaultPath + strDrawingFile
    End Function

    Public Function GetDrawingPathName() As String
        Return strDrawingPath + strDrawingFile
    End Function

    Public Function GetDrawingVaultPath() As String
        Return strDrawingVaultPath
    End Function

    Public Sub SetRefFile(ByVal Name As String)
        strRefFile = Name
    End Sub

    Public Sub SetFullRefName(ByVal Name As String)
        strFullRefName = Name
    End Sub

    Public Function GetRefFile() As String
        Return strRefFile
    End Function

    Public Sub SetRefPath(ByVal Ref As Document)
        strRefPath = GetDirectoryName(Ref.FullFileName) + "\"
        strRefVaultPath = strRefPath.Replace("C:\_Vault_Working_Folder", "$").Replace("\", "/")
    End Sub

    Public Function GetRefPath() As String
        Return strRefPath
    End Function

    Public Function GetRefPathName() As String
        Return strRefPath + strRefFile
    End Function

    Public Function GetRefVaultPathName() As String
        Return strRefVaultPath + strRefFile
    End Function

    Public Function GetRefVaultPath() As String
        Return strRefVaultPath
    End Function

    Public Sub SetRefIndex(ByVal Index As Integer)
        iRefIndex = Index
    End Sub

    Public Function GetIndex() As Integer
        Return iRefIndex
    End Function

    Public Function GetFullDrawingName() As String
        Return strFullDrawingName
    End Function

    Public Function GetFullRefName() As String
        Return strFullRefName
    End Function

End Class

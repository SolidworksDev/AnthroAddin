Imports System.Windows.Forms
Imports System.Security
Imports AnthroAddIn.Security
Imports AnthroAddIn.DocumentSvc
Imports System.Security.Principal.WindowsIdentity

Public Class ServerLogin

    Public secSvc As SecurityService = New SecurityService()
    Public docSvc As DocumentService = New DocumentService()
    Public LoggedIn As Boolean = False
    Private Shared MAX_FILE_SIZE_BYTES As Integer = 49 * 1024 * 1024

    Public Sub LoginToVault(ByVal serverName As String)

        Dim strUserName As String = Environment.UserName
        If strUserName = "steve" Then
            strUserName = "LinderS"
        ElseIf strUserName = "jeffm" Then
            strUserName = "McCaffreyJ"
        ElseIf strUserName = "mike" Then
            strUserName = "MullenM"
        ElseIf strUserName = "jerryn" Then
            strUserName = "NethkenJ"
        ElseIf strUserName = "Mikedg" Then
            strUserName = "cliftt"
        ElseIf strUserName = "mikedg" Then
            strUserName = "cliftt"
        Else
            Dim length As Integer = strUserName.Length
            Dim strLastChar As String = strUserName.Remove(1)
            Dim strMiddelChars As String = strUserName.Remove(0, 1)
            Dim strFirstChar As String = strMiddelChars.Remove(1)
            strMiddelChars = strMiddelChars.Remove(0, 1)
            strUserName = strFirstChar.ToUpper + strMiddelChars + strLastChar.ToUpper
        End If
        
        Try

            secSvc.SecurityHeaderValue = New Global.AnthroAddIn.Security.SecurityHeader()
            secSvc.Url = "http://" + serverName + "/AutodeskDM/Services/SecurityService.asmx"
            Try
                secSvc.SignIn(strUserName, "1234", "Anthro_Vault")
            Catch ex As Exception
                'Throw New Exception("An error occured while logging into the Vault Server for user: " + strUserName)
                MessageBox.Show("Vault login failed for user: " + strUserName + Chr(13) + "Vault services may be down at this time" + Chr(13) + "Please contact the Vault Administrator")
                Exit Sub
            End Try

            docSvc.SecurityHeaderValue = New DocumentSvc.SecurityHeader()
            docSvc.SecurityHeaderValue.UserId = secSvc.SecurityHeaderValue.UserId
            docSvc.SecurityHeaderValue.Ticket = secSvc.SecurityHeaderValue.Ticket
            docSvc.Url = "http://" + serverName + "/AutodeskDM/Services/DocumentService.asmx"


        Catch ex As Exception
            MessageBox.Show("Vault login failed for user: " + strUserName + Chr(13) + "Vault services may be down at this time" + Chr(13) + "Please contact the Vault Administrator")
            Exit Sub
            'Throw New Exception("Vault services are unavailable at this time" + Chr(13) + "Please contact the Vault Administrator")
        End Try
        LoggedIn = True

    End Sub

    Public Sub LogoutOfVault()
        secSvc.SignOut()
        LoggedIn = False
    End Sub

End Class

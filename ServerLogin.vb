Imports System.Windows.Forms
Imports System.Security
Imports System.Security.Principal.WindowsIdentity
Imports ACW = Autodesk.Connectivity.WebServices
Imports VDF = Autodesk.DataManagement.Client.Framework

Public Class ServerLogin

    Public results As VDF.Vault.Results.LogInResult
    Public connection As VDF.Vault.Currency.Connections.Connection
    Public LoggedIn As Boolean = False    

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

            Try
                results = VDF.Vault.Library.ConnectionManager.LogIn("svr19", "Anthro_Vault", strUserName, "1234", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, Nothing)
            Catch ex As Exception
                'Throw New Exception("An error occured while logging into the Vault Server for user: " + strUserName)
                MessageBox.Show("Vault login failed for user: " + strUserName + Chr(13) + "Vault services may be down at this time" + Chr(13) + "Please contact the Vault Administrator")
                Exit Sub
            End Try

            connection = results.Connection

        Catch ex As Exception
            MessageBox.Show("Vault login failed for user: " + strUserName + Chr(13) + "Vault services may be down at this time" + Chr(13) + "Please contact the Vault Administrator")
            Exit Sub
            'Throw New Exception("Vault services are unavailable at this time" + Chr(13) + "Please contact the Vault Administrator")
        End Try
        LoggedIn = True

    End Sub

    Public Sub LogoutOfVault()
        VDF.Vault.Library.ConnectionManager.LogOut(connection)
        LoggedIn = False
    End Sub

End Class

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
        If strUserName = "PCN8CZ" Or strUserName = "pcn8cz" Then
            strUserName = "CarbajalP"
        ElseIf strUserName = "CBGH72" Or strUserName = "cbgh72" Then
            strUserName = "BartonC"
        ElseIf strUserName = "BK83F6" Or strUserName = "bk83f6" Then
            strUserName = "KnightB"
        ElseIf strUserName = "SAUQQ3" Or strUserName = "sauqq3" Then
            strUserName = "AldrichS"
        ElseIf strUserName = "MGXP3V" Or strUserName = "mgxp3v" Then
            strUserName = "GiansanteM"
        ElseIf strUserName = "TCS4FS" Or strUserName = "tcs4fs" Then
            strUserName = "cliftt"
        ElseIf strUserName = "REWJK3" Or strUserName = "rewjk3" Then
            strUserName = "ElliottR"
        ElseIf strUserName = "SL8LQZ" Or strUserName = "sl8lqz" Then
            strUserName = "LinderS"
        ElseIf strUserName = "BRHLEU" Or strUserName = "brhleu" Then
            strUserName = "Rurikb"
        ElseIf strUserName = "COA32B" Or strUserName = "coa32b" Then
            strUserName = "OcallaghanC"
        ElseIf strUserName = "JWJ7JC" Or strUserName = "jwj7jc" Then
            strUserName = "WarrenJ"
        Else
            MessageBox.Show("No Vault Login found for user: " + strUserName + " Please contact the Vault Administrator")
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

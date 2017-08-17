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
        If strUserName = "Craing.Barton" Or strUserName = "craig.barton" Then
            strUserName = "BartonC"
        ElseIf strUserName = "Brent.Knight" Or strUserName = "brent.knight" Then
            strUserName = "KnightB"
        ElseIf strUserName = "Michael.Giansante" Or strUserName = "michael.giansante" Then
            strUserName = "GiansanteM"
        ElseIf strUserName = "James.Clift" Or strUserName = "james.clift" Then
            strUserName = "cliftt"
        ElseIf strUserName = "Ronald.Elliott" Or strUserName = "ronald.elliott" Then
            strUserName = "ElliottR"
        ElseIf strUserName = "Steve.Linder" Or strUserName = "steve.linder" Then
            strUserName = "LinderS"
        ElseIf strUserName = "Brian.Rurik" Or strUserName = "brian.rurik" Then
            strUserName = "Rurikb"
        ElseIf strUserName = "Colin.OCallaghn" Or strUserName = "colin.ocallaghn" Then
            strUserName = "OcallaghanC"
        ElseIf strUserName = "Nick.McFaddin" Or strUserName = "nick.mcfaddin" Then
            strUserName = "mcfaddinn"
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

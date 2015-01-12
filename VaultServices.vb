Imports System.IO
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports VDF = Autodesk.DataManagement.Client.Framework




Public Class VaultServices

    Public Sub Execute(fileIters As ICollection(Of VDF.Vault.Currency.Entities.FileIteration), ByVal downloadFiles() As String, ByVal serverLogin As ServerLogin, Optional ByVal checkout As Boolean = False)

        Try
            For Each downloadFile As String In downloadFiles

                Dim filePath As String = downloadFile

                If downloadFile = "" Then
                    MessageBox.Show("No drawing found for" + Chr(13) + filePath)
                End If

                'determine if the file already exists
                If (System.IO.File.Exists(filePath)) Then
                    'we'll try to delete the file so we can get the latest copy
                    Try

                        FileSystem.SetAttr(filePath, FileAttribute.Normal)
                        System.IO.File.Delete(filePath)

                    Catch e As IOException
                        Throw New Exception("The file you are attempting to open already exists and can not be overwritten. This file may currently be open, try closing any application you are using to view this file and try opening the file again.")
                    End Try
                End If
            Next

            DownlodFiles(fileIters, serverLogin)
            If checkout Then
                CheckoutFiles(fileIters, serverLogin)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Public Sub DownlodFiles(fileIters As ICollection(Of VDF.Vault.Currency.Entities.FileIteration), ByVal serverLogin As ServerLogin)

        ' download individual
        Dim settings As New VDF.Vault.Settings.AcquireFilesSettings(serverLogin.connection)

        For Each fileIter As VDF.Vault.Currency.Entities.FileIteration In fileIters

            If fileIter.EntityName <> Nothing Then
                settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download)
            End If

        Next

        Dim results As VDF.Vault.Results.AcquireFilesResults = serverLogin.connection.FileManager.AcquireFiles(settings)

    End Sub

    Public Sub CheckoutFiles(fileIters As ICollection(Of VDF.Vault.Currency.Entities.FileIteration), ByVal serverLogin As ServerLogin)

        ' download individual files
        Dim settings As New VDF.Vault.Settings.AcquireFilesSettings(serverLogin.connection)

        For Each fileIter As VDF.Vault.Currency.Entities.FileIteration In fileIters

            settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Checkout)

        Next

        Dim results As VDF.Vault.Results.AcquireFilesResults = serverLogin.connection.FileManager.AcquireFiles(settings)

    End Sub

    Public Sub DownloadAssembly(topLevelAssembly As VDF.Vault.Currency.Entities.FileIteration, ByVal serverLogin As ServerLogin)

        Dim settings As New VDF.Vault.Settings.AcquireFilesSettings(serverLogin.connection)

        ' download the latest version of the assembly to working folders        
        settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeChildren = True
        settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseChildren = True
        settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeRelatedDocumentation = True
        settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption = _
               VDF.Vault.Currency.VersionGatheringOption.Latest
        settings.AddFileToAcquire(topLevelAssembly, _
           VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download)

        Dim results As VDF.Vault.Results.AcquireFilesResults = _
            serverLogin.connection.FileManager.AcquireFiles(settings)
    End Sub

    Public Sub DownloadDialog(fileIter As VDF.Vault.Currency.Entities.FileIteration, ByVal serverLogin As ServerLogin, parentWindowHandle As WindowWrapper)

        ' pop up the Get/Checkout dialog
        Dim settings As New VDF.Vault.Forms.Settings.InteractiveAcquireFileSettings(serverLogin.connection, parentWindowHandle.Handle, "Download files")
        settings.AddEntityToAcquire(fileIter)

        VDF.Vault.Forms.Library.AcquireFiles(settings)
    End Sub
End Class

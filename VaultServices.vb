Imports System.IO
Imports System.Windows.Forms
Imports AnthroAddIn.Security
Imports AnthroAddIn.DocumentSvc
Imports System.Collections.Generic


Public Class VaultServices

    Private Shared m_downloadedFiles As List(Of String) = New List(Of String)()

    Public Sub Execute(ByVal file As DocumentSvc.File, ByVal downloadFile As String, ByVal serverLogin As ServerLogin)

        Try
            Dim filePath As String = downloadFile

            'determine if the file already exists
            If (System.IO.File.Exists(filePath)) Then
                'we'll try to delete the file so we can get the latest copy
                Try

                    FileSystem.SetAttr(filePath, FileAttribute.Normal)
                    System.IO.File.Delete(filePath)

                    'remove the file from the collection of downloaded files that need to be removed when the application exits
                    If (m_downloadedFiles.Contains(filePath)) Then
                        m_downloadedFiles.Remove(filePath)
                    End If
                Catch e As IOException
                    Throw New Exception("The file you are attempting to open already exists and can not be overwritten. This file may currently be open, try closing any application you are using to view this file and try opening the file again.")
                End Try
            End If

            If (file.FileSize > MAX_FILE_SIZE_BYTES) Then
                MultiDownload(file, filePath, serverLogin)
            Else
                SimpleDownload(file, filePath, serverLogin)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub SimpleDownload(ByVal file As DocumentSvc.File, ByVal filePath As String, ByVal serverLogin As ServerLogin)

        Dim info As System.IO.FileInfo = New FileInfo(filePath)

        Try

            If file.Id = -1 Then
                MessageBox.Show("No drawing found for" + Chr(13) + filePath)
                Exit Sub
            End If

            ' stream the data to the client
            Dim fileData As Byte() = {}
            Dim currentFileName As String = ""
            currentFileName = serverLogin.docSvc.DownloadFile(file.Id, False, fileData)

            Using stream As FileStream = New FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite)
                'add the newly created file to the collection of files that need to be removed when the application exits
                m_downloadedFiles.Add(filePath)

                'write the downloaded file to a physical file on the users machine
                stream.Write(fileData, 0, fileData.Length)
                stream.Close()
            End Using

            info.IsReadOnly = False
            info.CreationTime = file.CreateDate            

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub MultiDownload(ByVal file As DocumentSvc.File, ByVal filePath As String, ByVal serverLogin As ServerLogin)

        Dim info As System.IO.FileInfo = New FileInfo(filePath)

        Try

            ' stream the data to the client
            Dim fileData As Byte() = {}
            Dim fileName As String = serverLogin.docSvc.DownloadFile(file.Id, False, fileData)

            Using stream As FileStream = New FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite)
                'add the newly created file to the collection of files that need to be removed when the application exits
                m_downloadedFiles.Add(filePath)

                Dim startByte As Long = 0
                Dim endByte As Long = MAX_FILE_SIZE_BYTES
                Dim buffer As Byte()

                While (startByte < file.FileSize)
                    endByte = startByte + MAX_FILE_SIZE_BYTES
                    If (endByte > file.FileSize) Then
                        endByte = file.FileSize
                    End If

                    buffer = serverLogin.docSvc.DownloadFilePart(file.Id, startByte, endByte, True)
                    stream.Write(buffer, 0, buffer.Length)
                    startByte += buffer.Length
                End While
                stream.Close()
            End Using

            info.IsReadOnly = False
            info.CreationTime = file.CreateDate

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
End Class

Imports Inventor
Imports System.IO
Imports System.Security
Imports System.Windows.Forms
Imports AnthroAddIn.Security
Imports AnthroAddIn.DocumentSvc
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity

Public Class VerifyForm

    Public bAcceptClicked As Boolean = False
    Public bCancelClicked As Boolean = False

    Public ReadOnly Property DrawingListFormControls() As Control.ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Function VerifyDialog_AddListBox(ByVal drawingList As ArrayList, _
                                            ByVal inControls As Control.ControlCollection, _
                                            ByVal LableName As String, _
                                            ByVal LableText As String, _
                                            ByVal ListName As String, _
                                            ByVal ListText As String) As Boolean
        Try

            Dim LablePosition As System.Drawing.Point
            LablePosition.X = 15
            LablePosition.Y = 10

            Dim ListPosition As System.Drawing.Point
            ListPosition.X = 15
            ListPosition.Y = 40

            Dim newList As New ListBox
            Dim newLable As New System.Windows.Forms.Label

            newLable.Name = LableName
            newLable.Text = LableText
            newLable.Width = 250
            newLable.Location = LablePosition

            newList.Name = ListName
            newList.Text = ListText
            newList.Width = 225
            newList.Height = 280
            newList.Location = ListPosition

            For i = 0 To drawingList.Count - 1

                If Not newList.Items.Contains(drawingList(i).ToString) Then
                    newList.Items.Add(drawingList(i).ToString)
                End If

            Next

            inControls.Add(newLable)
            inControls.Add(newList)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        bAcceptClicked = True
        bCancelClicked = False
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        bAcceptClicked = False
        bCancelClicked = True
        Me.Close()
    End Sub

End Class
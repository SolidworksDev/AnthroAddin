
Public Class DrawingDocuments

    Public DrawingIndex As New ArrayList
    Public DrawingName As New ArrayList


    Public Sub SetDocName(ByVal invDocName As String)
        DrawingName.Add(invDocName)
    End Sub

    Public Sub SetDocIndex(ByVal invDocIndex As Integer)
        DrawingIndex.Add(invDocIndex)
    End Sub

    Public Function NotInList(ByVal invDocName As String) As Boolean

        If DrawingName.Count = 0 Then
            Return True
        End If

        Dim i As Integer = 0
        Dim bNotInList As Boolean = True
        Dim strDocName As String = invDocName.Remove(invDocName.Length - 4)

        For Each dName In DrawingName
            If dName.ToString.Remove(dName.ToString.Length - 4) = strDocName Then
                bNotInList = False
                Exit For
            End If
        Next dName

        If bNotInList Then
            Return True
        Else
            Return False
        End If

    End Function

End Class


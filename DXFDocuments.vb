Public Class DXFDocuments
    Public DocIndex As New ArrayList
    Public DocName As New ArrayList

    Public Sub SetDocName(ByVal invDocName As String)
        DocName.Add(invDocName)
    End Sub

    Public Sub SetDocIndex(ByVal invDocIndex As Integer)
        DocIndex.Add(invDocIndex)
    End Sub

End Class

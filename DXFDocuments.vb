Public Class DXFDocuments
    
    Private m_PartName As String
    Private m_PartIndex As String

    Public Property PartName() As String
        Get
            Return m_PartName
        End Get

        Set(value As String)
            m_PartName = value
        End Set
    End Property

    Public Property PartIndex() As String
        Get
            Return m_PartIndex
        End Get

        Set(value As String)
            m_PartIndex = value
        End Set
    End Property

End Class

Imports System.Windows.Forms

Module PrintDrawings
    Public Sub PrintCurrentDrawingDocuments(ByVal ThisApplication As Inventor.Application)

        ' Declare a variable as Inventor's Application object type.
        Dim invApp As Inventor.Application

        ' Declare a variable for the Applications Documents
        Dim invDocs As Inventor.Documents

        ' Declare a variable to hold the Drawing Document
        Dim invDrawingDoc As Inventor.DrawingDocument

        Dim invDoc As Inventor.Document

        'Get the application
        invApp = ThisApplication

        ' Get the documents currently open in the application
        invDocs = invApp.Documents

        Dim bdrawingFound As Boolean
        bdrawingFound = False

        Dim strDrawingFileNames As String
        strDrawingFileNames = ""

        Dim intUserResponse As Integer

        Dim strPrinterName As String
        strPrinterName = ""

        Try
            For i = 1 To invDocs.Count
                invDoc = invDocs.Item(i)
                If invDoc.DocumentType = Inventor.DocumentTypeEnum.kDrawingDocumentObject Then
                    strDrawingFileNames = strDrawingFileNames & invDoc.DisplayName & ".idw" & Chr(13)
                    bdrawingFound = True
                End If
            Next

            'Check to see if bdrawingFound has been set to True
            'If not display a message to the user and exit the macro
            If bdrawingFound = False Then
                MsgBox("You must have at least one drawing opened" & Chr(13) & _
                       "Please open a drawing to print and re-run the command", _
                       vbOKOnly + vbExclamation, "Print Drawings")
                Exit Sub
            End If

            intUserResponse = MsgBox("The following files will be printed to:" & Chr(13) & "Default Printer" & _
                                    Chr(13) & Chr(13) & strDrawingFileNames & Chr(13) & "Print Drawings?" _
                                    , vbYesNo + vbQuestion, "Print Drawing files?")

            If intUserResponse = vbYes Then
                For i = 1 To invDocs.Count
                    If invDocs.Item(i).DocumentType = Inventor.DocumentTypeEnum.kDrawingDocumentObject Then
                        invDrawingDoc = invDocs.Item(i)
                        invDrawingDoc.PrintManager.SubmitPrint()
                    End If
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

End Module

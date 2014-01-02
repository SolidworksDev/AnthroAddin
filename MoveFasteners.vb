Imports Inventor
Imports System.Windows.Forms

Module MoveFasteners
    Public Sub MoveAllFasteners(ByVal ThisApplication As Inventor.Application)

        'This macro is intended to be used on assembly documents
        'If no assembly document is opened a Message Box is displayed
        'to indicate that an assembly document must be opened and the
        'macro exits
        'If an assembly document is open the macro checks to see if a
        '"Fasteners" folder exist.  If none exists one is created and all
        'nodes in the "Model" browser pane that start with "325", i.e. fasteners,
        'are moved to the "Fasteners" folder

        ' Declare a variable as Inventor's Application object type.
        Dim invApp As Inventor.Application
        invApp = ThisApplication

        'Declare a variable to hold the Assembly Document
        Dim invAssemblyDoc As AssemblyDocument

        'Declare a variable to hold the Documents
        Dim invDocs As Documents

        'Declare a variable to hold the Document
        Dim invDoc As Inventor.Document

        'Declare a variable to hold the Browser Pane
        Dim invBrowserModelPane As BrowserPane

        'Declare a variable to hold a Browser Folder
        Dim invFastenersFolder As BrowserFolder
        invFastenersFolder = Nothing

        'Declare a variable to hold the Browser Top Node
        Dim invBrowserTopNode As BrowserNode

        'Declare a variable to hold the Browser Node to move
        Dim invBrowserNodeToMove As BrowserNode

        'Declare a variable to hold the New Browser Node to be added
        'Dim invFastenersFolderBrowserNode As BrowserNode

        'Declare a variable to hold the Browser Nodes
        Dim invBrowserNodes As BrowserNodesEnumerator

        'Declare a variable to hold the count of the browser nodes
        'Dim invBrowserFolderCount As Integer

        'Declare a variable to hold the current node name
        Dim nodename As String

        Try
            ' Get the documents currently open in the application
            invDocs = invApp.Documents
            If invDocs.Count = 0 Then
                MsgBox("No documents are currently opened")
                Exit Sub
            End If

            invDoc = invApp.ActiveDocument

            'Check to see if we have the first Assembly Document in the Documents Object
            If invDoc.DocumentType = DocumentTypeEnum.kAssemblyDocumentObject Then

                'Get the current assembly document
                invAssemblyDoc = invDoc
                'Get the "Model" pane in the current assembly document
                invBrowserModelPane = invAssemblyDoc.BrowserPanes.Item("Model")
                'Get the TopNode of the current "Model" pane
                invBrowserTopNode = invBrowserModelPane.TopNode

                If invBrowserTopNode.BrowserFolders.Count < 1 Then
                    'No folders exist so we will create one
                    invFastenersFolder = invBrowserModelPane.AddBrowserFolder("Fasteners")
                End If

                For j = 1 To invBrowserTopNode.BrowserFolders.Count

                    'Check to see if the current folder in the "Fasteners" folder
                    If invBrowserTopNode.BrowserFolders.Item(j).Name.ToLower = "fasteners" Then

                        'Assign the current folder to invFastenersFolder so we can
                        'move all the fasteners to it
                        invFastenersFolder = invBrowserTopNode.BrowserFolders.Item(j)

                        'Get all the nodes in the "Model" pane
                        invBrowserNodes = invBrowserTopNode.BrowserNodes
                        For k = 1 To invBrowserNodes.Count

                            'Get the first three characters of the current node
                            nodename = Left$(invBrowserNodes.Item(k).BrowserNodeDefinition.Label, 3)

                            'If the first three character are "325" the node is a fastener
                            'and it will be moved to the new Fasteners folder
                            If nodename = "325" Then
                                'Get the node to move
                                invBrowserNodeToMove = invBrowserNodes.Item(k)
                                'Move the node to the "Fasteners" folder
                                invFastenersFolder.Add(invBrowserNodeToMove)
                            End If

                        Next

                    End If

                    'Get all the nodes in the "Model" pane
                    invBrowserNodes = invBrowserTopNode.BrowserNodes

                Next

            Else

                MsgBox("No assembly document is currently opened" & Chr(13) & _
                       "This macro is intended to be ran on assembly documents", vbOKOnly)
                Exit Sub

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

End Module

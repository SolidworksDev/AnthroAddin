Imports Inventor
Imports stdole.LoadPictureConstants
Imports System.Drawing
Imports Microsoft.Win32
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Namespace AnthroAddIn
    <ProgIdAttribute("AnthroAddIn.StandardAddInServer"), _
    GuidAttribute("f3971a52-8202-4a82-82a2-77d7cdb707f9")> _ 
    Public Class StandardAddInServer
        Implements Inventor.ApplicationAddInServer

        'GuidAttribute("6f4da88b-9508-4c99-bbf8-0d46d6d7140d")> _ f3971a52-8202-4a82-82a2-77d7cdb707f9
#Region "hWnd Wrapper Class"
        ' This class is used to wrap a Win32 hWnd as a .Net IWind32Window class.
        ' This is primarily used for parenting a dialog to the Inventor window.'
        ' For example:
        ' myForm.Show(New WindowWrapper(m_inventorApplication.MainFrameHWND))
        Public Class WindowWrapper
            Implements System.Windows.Forms.IWin32Window

            Public Sub New(ByVal handle As IntPtr)
                _hwnd = handle
            End Sub
            Public ReadOnly Property Handle() As IntPtr Implements System.Windows.Forms.IWin32Window.Handle
                Get
                    Return _hwnd
                End Get
            End Property
            Private _hwnd As IntPtr
        End Class
#End Region

#Region "Data Members"
        ' Inventor application object.
        Private m_inventorApplication As Inventor.Application
        Private m_currentPart As Inventor.PartDocument
        Private m_ClientID As String
        Private WithEvents m_applicationEvents As Inventor.ApplicationEvents       
        Private WithEvents m_userInterfaceEvents As Inventor.UserInterfaceEvents
        Private WithEvents m_sketchEvents As Inventor.SketchEvents
        Private WithEvents m_transactionEvents As Inventor.TransactionEvents        
       
        'buttons
        Private WithEvents m_printalldrawingsButtonDef As ButtonDefinition
        Private WithEvents m_moveFastenersToFolderButtonDef As ButtonDefinition
        Private WithEvents m_exportDXFButtonDef As ButtonDefinition
        Private WithEvents m_drawSlotButtonDef As ButtonDefinition
        Private WithEvents m_printDrawingsFromAssemblyButtonDef As ButtonDefinition
        Private WithEvents m_updateiPropertiesButtonDef As ButtonDefinition
        Private WithEvents m_exportDWFxButtonDef As ButtonDefinition
        Private WithEvents m_slotFeatureButtonDef As ButtonDefinition
        Private WithEvents m_drawLineArtButtonDef As ButtonDefinition
        Private WithEvents m_updateLineArtPrecisionButtonDef As ButtonDefinition
        Private WithEvents m_updateCustomerPrecisionButtonDef As ButtonDefinition
        Private WithEvents m_createComponentPlaceholderButtonDef As ButtonDefinition
        Private WithEvents m_showBendDirectionComponentButtonDef As ButtonDefinition
        Private WithEvents m_calculateTopAreaComponentButtonDef As ButtonDefinition
        Private WithEvents m_interactionEvents As InteractionEvents
        Private WithEvents m_drawRectangleComponentButtonDef As ButtonDefinition
        Private WithEvents m_selection As SelectEvents

        ' Parts List export dialog sizes
        Private iPartsDialogWidth As Integer = 250
        Private iPartsDialogHeight As Integer = 400
        ' Print Drawing dialog sizes
        Private iPrintDrawingsfromAssemblyDialogWidth As Integer = 270
        Private iPrintDrawingsfromAssemblyDialogHeight As Integer = 435
        ' export DWFx dialog sizes
        Private iExportDWFxfromAssemblyDialogWidth As Integer = 250
        Private iExportDWFxfromAssemblyDialogHeight As Integer = 400

        Private dBottomArea As Double = 0.0
        Private dTotalArea As Double = 0.0
        Private dGuleArea As Double = 0.0

#End Region

#Region "ApplicationAddInServer Members"

        Public Sub Activate(ByVal addInSiteObject As Inventor.ApplicationAddInSite, ByVal firstTime As Boolean) Implements Inventor.ApplicationAddInServer.Activate

            Try                
                ' This method is called by Inventor when it loads the AddIn.
                ' The AddInSiteObject provides access to the Inventor Application object.
                ' The FirstTime flag indicates if the AddIn is loaded for the first time.

                ' Initialize AddIn members.
                m_inventorApplication = addInSiteObject.Application

                'Get the ClassID for this add-in and save it in a
                'member variable to use wherever a ClientID is Needed.
                m_ClientID = AddInGuid(GetType(StandardAddInServer))
                m_applicationEvents = m_inventorApplication.ApplicationEvents                
                m_userInterfaceEvents = m_inventorApplication.UserInterfaceManager.UserInterfaceEvents
                m_sketchEvents = m_inventorApplication.SketchEvents
                m_transactionEvents = m_inventorApplication.TransactionManager.TransactionEvents
               
                'Create the button definition
                Dim controlDefs As ControlDefinitions
                controlDefs = m_inventorApplication.CommandManager.ControlDefinitions

                Dim UIManager As Inventor.UserInterfaceManager = m_inventorApplication.UserInterfaceManager

                Dim largeIconSize As Integer
                If UIManager.InterfaceStyle = InterfaceStyleEnum.kRibbonInterface Then
                    largeIconSize = 32
                Else
                    largeIconSize = 24
                End If

                Dim FolderPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.FileFolder, 16, 16))
                Dim PrinterPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.printer, 16, 16))
                Dim DXFPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.DXFIcon, 16, 16))
                Dim iPropertyPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.iProperty, 16, 16))
                Dim DWFxPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.DWF_Viewer, 16, 16))                
                Dim SlotFeaturePicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.SlotFeature, 16, 16))
                Dim DrawLineArtPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.edit, 16, 16))
                Dim DimensionPrecisionPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.fix, 16, 16))
                Dim ComponentPlaceholderPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.Component, 16, 16))
                Dim TopAreaPicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.TopArea, 16, 16))
                Dim DrawRectanglePicture As IPictureDisp = PictureDispConverter.ToIPictureDisp(New Icon(My.Resources.Draw, 16, 16))

                m_moveFastenersToFolderButtonDef = controlDefs.AddButtonDefinition("Move Fasteners", _
                                        "MoveFastenersToFolder",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Move all fasteners to folder", FolderPicture, FolderPicture)
                m_printalldrawingsButtonDef = controlDefs.AddButtonDefinition("Print", _
                                       "AnthroPringAllDrawings",
                                       CommandTypesEnum.kNonShapeEditCmdType,
                                       m_ClientID, _
                                       ,
                                       "Print all current drawings",
                                       PrinterPicture, PrinterPicture)

                m_exportDXFButtonDef = controlDefs.AddButtonDefinition("Export DXF", _
                                        "AnthroExportDXF",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Export Flat Pattern",
                                        DXFPicture, DXFPicture)

                m_printDrawingsFromAssemblyButtonDef = controlDefs.AddButtonDefinition("Print Drawings", _
                                        "AnthroPrintfromAssembly",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Print or Open Drawings from this Assembly",
                                        PrinterPicture, PrinterPicture)

                m_updateiPropertiesButtonDef = controlDefs.AddButtonDefinition("iProperties", _
                                        "UpdateiProperties",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Update Apprpved by and Rev iProperties",
                                        iPropertyPicture, iPropertyPicture)

                m_exportDWFxButtonDef = controlDefs.AddButtonDefinition("Export DWFx", _
                                       "ExportDWFx",
                                       CommandTypesEnum.kNonShapeEditCmdType,
                                       m_ClientID, _
                                       ,
                                       "Export DWFx files to server",
                                       DWFxPicture, DWFxPicture)

                m_slotFeatureButtonDef = controlDefs.AddButtonDefinition("Slot", _
                                        "SlotFeature",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Create Slot Feature",
                                        SlotFeaturePicture, SlotFeaturePicture)

                m_drawLineArtButtonDef = controlDefs.AddButtonDefinition("Draw Line Art",
                                        "DrawLineArt",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Draw Line Art",
                                        DrawLineArtPicture, DrawLineArtPicture)

                m_updateLineArtPrecisionButtonDef = controlDefs.AddButtonDefinition("Line Art Dim",
                                        "LineArtPrecision",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Change Dimensions for Line Art",
                                        DimensionPrecisionPicture, DimensionPrecisionPicture)
                m_updateCustomerPrecisionButtonDef = controlDefs.AddButtonDefinition("Customer Dim",
                                        "CustomerPrecision",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Change Dimensions for Customer Approval",
                                        DimensionPrecisionPicture, DimensionPrecisionPicture)
                m_createComponentPlaceholderButtonDef = controlDefs.AddButtonDefinition("Component",
                                        "ComponentPlaceholder",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Create a Component Placeholder",
                                       ComponentPlaceholderPicture, ComponentPlaceholderPicture)
                m_showBendDirectionComponentButtonDef = controlDefs.AddButtonDefinition("Show Bend Direction",
                                        "BendDirection",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Show the Bend Direction",
                                        DimensionPrecisionPicture, DimensionPrecisionPicture)
                m_calculateTopAreaComponentButtonDef = controlDefs.AddButtonDefinition("Top Area",
                                        "TopArea",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Calculate the glue area of the top",
                                        TopAreaPicture, TopAreaPicture)
                m_drawRectangleComponentButtonDef = controlDefs.AddButtonDefinition("Draw Rectangle",
                                        "DrawRectangle",
                                        CommandTypesEnum.kNonShapeEditCmdType,
                                        m_ClientID, _
                                        ,
                                        "Draw a rectangle constrained to the XY origin",
                                        DrawRectanglePicture, DrawRectanglePicture)

                If firstTime Then

                    Dim partRibbon As Inventor.Ribbon = UIManager.Ribbons.Item("Part")
                    Dim drawingRibbon As Inventor.Ribbon = UIManager.Ribbons.Item("Drawing")
                    Dim assemblyRibbon As Inventor.Ribbon = UIManager.Ribbons.Item("Assembly")

                    Dim partTab As Inventor.RibbonTab
                    partTab = partRibbon.RibbonTabs.Item("id_TabModel")

                    Dim drawingTab As Inventor.RibbonTab
                    drawingTab = drawingRibbon.RibbonTabs.Item("id_TabPlaceViews")

                    Dim assemblyTab As Inventor.RibbonTab
                    assemblyTab = assemblyRibbon.RibbonTabs.Item("id_TabAssemble")

                    Dim modifyTab As Inventor.RibbonTab
                    modifyTab = assemblyRibbon.RibbonTabs.Item("id_TabModel")

                    Dim sketchTab As Inventor.RibbonTab
                    sketchTab = partRibbon.RibbonTabs.Item("id_TabSketch")

                    Dim flatPatternTab As Inventor.RibbonTab
                    flatPatternTab = partRibbon.RibbonTabs.Item("id_TabFlatPattern")

                    Dim sheetmetalTab As Inventor.RibbonTab
                    sheetmetalTab = partRibbon.RibbonTabs.Item("id_TabSheetMetal")

                    Dim toolsTab As Inventor.RibbonTab
                    toolsTab = partRibbon.RibbonTabs.Item("id_TabTools")

                    Dim partPanel As Inventor.RibbonPanel
                    partPanel = partTab.RibbonPanels.Add("Anthro", "AnthroRibbonUITab", m_ClientID)

                    Dim drawingPanel As Inventor.RibbonPanel
                    drawingPanel = drawingTab.RibbonPanels.Add("Anthro", "AnthroDrawingRibbonUITab", m_ClientID)

                    Dim assemblyPanel As Inventor.RibbonPanel
                    assemblyPanel = assemblyTab.RibbonPanels.Add("Anthro", "AnthroAssemblyRibbonUITab", m_ClientID)

                    Dim sketchPanel As Inventor.RibbonPanel
                    sketchPanel = sketchTab.RibbonPanels.Item("id_PanelP_2DSketchDraw")

                    Dim sheetmetalPanelModify As Inventor.RibbonPanel
                    sheetmetalPanelModify = sheetmetalTab.RibbonPanels.Item("id_PanelP_SheetMetalModify")

                    Dim sheetmetalPanelCreate As Inventor.RibbonPanel
                    sheetmetalPanelCreate = sheetmetalTab.RibbonPanels.Item("id_PanelP_SheetMetalCreate")

                    Dim flatPatternPanel As Inventor.RibbonPanel
                    flatPatternPanel = flatPatternTab.RibbonPanels.Item("id_PanelP_FlatPatternPattern")

                    Dim assemblyModifyPanel As Inventor.RibbonPanel
                    assemblyModifyPanel = modifyTab.RibbonPanels.Item("id_PanelA_ModelModify")

                    Dim toolsMeasurePanel As Inventor.RibbonPanel
                    toolsMeasurePanel = toolsTab.RibbonPanels.Item("id_PanelP_ToolsMeasure")

                    assemblyPanel.CommandControls.AddButton(m_moveFastenersToFolderButtonDef)
                    assemblyPanel.CommandControls.AddButton(m_exportDXFButtonDef)
                    assemblyPanel.CommandControls.AddButton(m_printDrawingsFromAssemblyButtonDef)
                    assemblyPanel.CommandControls.AddButton(m_updateiPropertiesButtonDef)
                    assemblyPanel.CommandControls.AddButton(m_exportDWFxButtonDef)
                    assemblyPanel.CommandControls.AddButton(m_drawLineArtButtonDef)
                    drawingPanel.CommandControls.AddButton(m_printalldrawingsButtonDef)
                    drawingPanel.CommandControls.AddButton(m_updateLineArtPrecisionButtonDef)
                    drawingPanel.CommandControls.AddButton(m_updateCustomerPrecisionButtonDef)
                    sheetmetalPanelModify.CommandControls.AddButton(m_slotFeatureButtonDef)
                    flatPatternPanel.CommandControls.AddButton(m_showBendDirectionComponentButtonDef)
                    toolsMeasurePanel.CommandControls.AddButton(m_calculateTopAreaComponentButtonDef)
                    sketchPanel.CommandControls.AddButton(m_drawRectangleComponentButtonDef)
                    sheetmetalPanelCreate.CommandControls.AddButton(m_createComponentPlaceholderButtonDef)
                    m_createComponentPlaceholderButtonDef.Enabled = False

                End If
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Public Sub Deactivate() Implements Inventor.ApplicationAddInServer.Deactivate

            ' This method is called by Inventor when the AddIn is unloaded.
            ' The AddIn will be unloaded either manually by the user or
            ' when the Inventor session is terminated.

            ' Release objects.
            Marshal.ReleaseComObject(m_inventorApplication)
            m_inventorApplication = Nothing

            Marshal.ReleaseComObject(m_exportDXFButtonDef)
            m_exportDXFButtonDef = Nothing

            Marshal.ReleaseComObject(m_moveFastenersToFolderButtonDef)
            m_moveFastenersToFolderButtonDef = Nothing

            Marshal.ReleaseComObject(m_printalldrawingsButtonDef)
            m_printalldrawingsButtonDef = Nothing

            Marshal.ReleaseComObject(m_printDrawingsFromAssemblyButtonDef)
            m_printDrawingsFromAssemblyButtonDef = Nothing

            Marshal.ReleaseComObject(m_updateiPropertiesButtonDef)
            m_updateiPropertiesButtonDef = Nothing

            Marshal.ReleaseComObject(m_exportDWFxButtonDef)
            m_exportDWFxButtonDef = Nothing

            Marshal.ReleaseComObject(m_slotFeatureButtonDef)
            m_slotFeatureButtonDef = Nothing

            Marshal.ReleaseComObject(m_drawLineArtButtonDef)
            m_drawLineArtButtonDef = Nothing

            Marshal.ReleaseComObject(m_updateLineArtPrecisionButtonDef)
            m_updateLineArtPrecisionButtonDef = Nothing

            Marshal.ReleaseComObject(m_updateCustomerPrecisionButtonDef)
            m_updateCustomerPrecisionButtonDef = Nothing

            Marshal.ReleaseComObject(m_createComponentPlaceholderButtonDef)
            m_createComponentPlaceholderButtonDef = Nothing

            Marshal.ReleaseComObject(m_showBendDirectionComponentButtonDef)
            m_showBendDirectionComponentButtonDef = Nothing

            Marshal.ReleaseComObject(m_calculateTopAreaComponentButtonDef)
            m_calculateTopAreaComponentButtonDef = Nothing

            Marshal.ReleaseComObject(m_drawRectangleComponentButtonDef)
            m_drawRectangleComponentButtonDef = Nothing

            System.GC.WaitForPendingFinalizers()
            System.GC.Collect()

        End Sub

        Public ReadOnly Property Automation() As Object Implements Inventor.ApplicationAddInServer.Automation

            ' This property is provided to allow the AddIn to expose an API 
            ' of its own to other programs. Typically, this  would be done by
            ' implementing the AddIn's API interface in a class and returning 
            ' that class object through this property.

            Get
                Return Nothing
            End Get

        End Property

        Public Sub ExecuteCommand(ByVal commandID As Integer) Implements Inventor.ApplicationAddInServer.ExecuteCommand

            ' Note:this method is now obsolete, you should use the 
            ' ControlDefinition functionality for implementing commands.

        End Sub

#End Region

#Region "COM Registration"
        
        Public Shared ReadOnly Property AddInGuid(ByVal t As Type) As String
            Get
                Dim guid As String = ""
                Try
                    Dim customAttributes() As Object = t.GetCustomAttributes(GetType(GuidAttribute), False)
                    Dim guidAttribute As GuidAttribute = CType(customAttributes(0), GuidAttribute)
                    guid = "{" + guidAttribute.Value.ToString() + "}"
                Finally
                    AddInGuid = guid
                End Try
            End Get
        End Property

#End Region

#Region "Inventor Addin Members"

        Private Sub m_moveFastenersToFolderButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_moveFastenersToFolderButtonDef.OnExecute
            MoveAllFasteners(m_inventorApplication)
        End Sub

        Private Sub m_printalldrawingsButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_printalldrawingsButtonDef.OnExecute

            Dim selectDrawingsDialog As New SelectDialog(m_inventorApplication)
            Dim drawingsListcontrols As Control.ControlCollection = selectDrawingsDialog.DrawingListFormControls
            selectDrawingsDialog.SelectDialog_AddListBox(drawingsListcontrols)
            selectDrawingsDialog.ShowDialog()

            'PrintCurrentDrawingDocuments(m_inventorApplication)
        End Sub

        Private Sub m_exportDXFButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_exportDXFButtonDef.OnExecute

            Try

                Dim partsListDialog As New ExportDXFfromAssemblyDialog(m_inventorApplication)

                Dim partsListControls As Control.ControlCollection = partsListDialog.PartsListFormControls

                Dim invDocs As Documents = m_inventorApplication.Documents

                Dim AcceptPosition As System.Drawing.Point
                AcceptPosition.X = iPartsDialogWidth - 170
                AcceptPosition.Y = iPartsDialogHeight - 65

                Dim CancelPosition As System.Drawing.Point
                CancelPosition.X = iPartsDialogWidth - 90
                CancelPosition.Y = iPartsDialogHeight - 65

                Dim SelectAllPosition As System.Drawing.Point
                SelectAllPosition.X = 10
                SelectAllPosition.Y = iPartsDialogHeight - 65

                partsListDialog.PartsDialog_AddListBox(partsListControls, invDocs)

                partsListDialog.Width = iPartsDialogWidth
                partsListDialog.Height = iPartsDialogHeight
                partsListDialog.btnAccept.Location = AcceptPosition
                partsListDialog.btnCancel.Location = CancelPosition
                partsListDialog.SelectAll.Location = SelectAllPosition
                partsListDialog.Show(New WindowWrapper(m_inventorApplication.MainFrameHWND))

            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Private Sub m_exportDWFxButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_exportDWFxButtonDef.OnExecute

            Try
                Dim exportDWFxfromAssemblyDialog As New ExportDWFxfromAssemblyDialog(m_inventorApplication)

                Dim DrawingsListControls As Control.ControlCollection = exportDWFxfromAssemblyDialog.DrawingsListFormControls

                Dim AcceptPosition As System.Drawing.Point
                AcceptPosition.X = iExportDWFxfromAssemblyDialogWidth - 170
                AcceptPosition.Y = iExportDWFxfromAssemblyDialogHeight - 65

                Dim CancelPosition As System.Drawing.Point
                CancelPosition.X = iExportDWFxfromAssemblyDialogWidth - 90
                CancelPosition.Y = iExportDWFxfromAssemblyDialogHeight - 65

                Dim SelectAllPosition As System.Drawing.Point
                SelectAllPosition.X = 10
                SelectAllPosition.Y = iExportDWFxfromAssemblyDialogHeight - 65

                exportDWFxfromAssemblyDialog.DrawingsDialog_AddListBox(DrawingsListControls)
                exportDWFxfromAssemblyDialog.Height = iExportDWFxfromAssemblyDialogHeight
                exportDWFxfromAssemblyDialog.Width = iExportDWFxfromAssemblyDialogWidth
                exportDWFxfromAssemblyDialog.btnAccept.Location = AcceptPosition
                exportDWFxfromAssemblyDialog.btnCancel.Location = CancelPosition
                exportDWFxfromAssemblyDialog.SelectAll.Location = SelectAllPosition
                exportDWFxfromAssemblyDialog.ShowDialog(New WindowWrapper(m_inventorApplication.MainFrameHWND))
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Private Sub m_drawRectangleComponentButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_drawRectangleComponentButtonDef.OnExecute

            Try

                Dim drawRectangleDialog As New DrawRectangleDialog(m_inventorApplication)
                drawRectangleDialog.ShowDialog(New WindowWrapper(m_inventorApplication.MainFrameHWND))

            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        End Sub

        Private Sub m_printDrawingsFromAssemblyButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_printDrawingsFromAssemblyButtonDef.OnExecute

            Try

                Dim printDrawingsfromAssemblyDialog As New printDrawingsfromAssemblyDialog(m_inventorApplication)

                Dim DrawingsListControls As Control.ControlCollection = printDrawingsfromAssemblyDialog.DrawingsListFormControls

                Dim AcceptPosition As System.Drawing.Point
                AcceptPosition.X = iPrintDrawingsfromAssemblyDialogWidth - 170
                AcceptPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 65

                Dim CancelPosition As System.Drawing.Point
                CancelPosition.X = iPrintDrawingsfromAssemblyDialogWidth - 90
                CancelPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 65

                Dim SelectAllPosition As System.Drawing.Point
                SelectAllPosition.X = 10
                SelectAllPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 65

                Dim OpenRadioButtonPosition As System.Drawing.Point
                OpenRadioButtonPosition.X = 10
                OpenRadioButtonPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 90

                Dim PrintRadioButtionPosition As System.Drawing.Point
                PrintRadioButtionPosition.X = 10
                PrintRadioButtionPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 110

                Dim SelectUnauthroizedButtonPosition As System.Drawing.Point
                SelectUnauthroizedButtonPosition.X = 115
                SelectUnauthroizedButtonPosition.Y = iPrintDrawingsfromAssemblyDialogHeight - 90

                printDrawingsfromAssemblyDialog.DrawingsDialog_AddListBox(DrawingsListControls)
                printDrawingsfromAssemblyDialog.Height = iPrintDrawingsfromAssemblyDialogHeight
                printDrawingsfromAssemblyDialog.Width = iPrintDrawingsfromAssemblyDialogWidth
                printDrawingsfromAssemblyDialog.btnAccept.Location = AcceptPosition
                printDrawingsfromAssemblyDialog.btnCancel.Location = CancelPosition
                printDrawingsfromAssemblyDialog.SelectAll.Location = SelectAllPosition
                printDrawingsfromAssemblyDialog.rbtnOpenFile.Location = OpenRadioButtonPosition
                printDrawingsfromAssemblyDialog.rbtnPrintFiles.Location = PrintRadioButtionPosition
                printDrawingsfromAssemblyDialog.SelectUnApproved.Location = SelectUnauthroizedButtonPosition
                printDrawingsfromAssemblyDialog.ShowDialog(New WindowWrapper(m_inventorApplication.MainFrameHWND))

            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Private Sub m_updateiPropertiesButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_updateiPropertiesButtonDef.OnExecute

            Try

                Dim updateiPropertiesDialog As New UpdateiPropertiesDialog(m_inventorApplication)
                Dim DocsListcontrols As Control.ControlCollection = updateiPropertiesDialog.DocumentListFormControls
                updateiPropertiesDialog.UpdateiPropertiesDialog_AddListBox(DocsListcontrols)
                updateiPropertiesDialog.txtBxApprovedBy.Select()
                updateiPropertiesDialog.ShowDialog(New WindowWrapper(m_inventorApplication.MainFrameHWND))

            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Private Sub m_slotFeatureButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_slotFeatureButtonDef.OnExecute

            Try
                If bSlotFeatureRunning = False Then

                    Dim invPartDoc As Inventor.PartDocument = m_inventorApplication.ActiveEditDocument
                    Dim invSketch As Sketch
                    Dim bFoundVisableSketch As Boolean = False
                    Dim invPartCompDef As PartComponentDefinition = invPartDoc.ComponentDefinition
                    Dim iVisableSketchCount As Integer

                    For i = 1 To invPartCompDef.Sketches.Count
                        invSketch = invPartCompDef.Sketches.Item(i)
                        If invSketch.Visible = True Then
                            iVisableSketchCount += 1
                            If iVisableSketchCount > 1 Then
                                MsgBox("Multiple Visable Sketches Found" + Chr(13) + "Only One Sketch can be Visable" + _
                                        Chr(13) + "Please hide all but one Sketch")
                                Exit Sub
                            End If
                        End If
                    Next

                    For i = 1 To invPartCompDef.Sketches.Count
                        invSketch = invPartCompDef.Sketches.Item(i)
                        If invSketch.Visible = True Then
                            bFoundVisableSketch = True
                            Exit For
                        End If
                    Next

                    If bFoundVisableSketch = False Then
                        MsgBox("No Visable Sketch Found")
                        Exit Sub
                    End If

                    Dim slotFeatureDialog As New SlotFeatureDialog(m_inventorApplication)

                    slotFeatureDialog.AutoValidate = AutoValidate.EnablePreventFocusChange
                    slotFeatureDialog.Show(New WindowWrapper(m_inventorApplication.MainFrameHWND))
                    slotFeatureDialog.rbtnHorizontal.Focus()

                End If
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        End Sub

        Private Sub m_drawLineArtButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_drawLineArtButtonDef.OnExecute

            Dim drawLineArt As New DrawLineArt(m_inventorApplication)
            drawLineArt.DrawViews()

        End Sub

        Private Sub m_updateLineArtPrecisionButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_updateLineArtPrecisionButtonDef.OnExecute

            Dim invApp As Inventor.Application

            invApp = m_inventorApplication

            Dim updatePrecisionTransaction As Transaction

            updatePrecisionTransaction = invApp.TransactionManager.StartTransaction(invApp.ActiveEditDocument, "updatePrecision")

            Dim invDoc As DrawingDocument

            Dim invSheet As Sheet

            invDoc = invApp.ActiveDocument

            invSheet = invDoc.Sheets.Item(1)

            Dim oUOM As UnitsOfMeasure
            oUOM = invApp.ActiveDocument.UnitsOfMeasure

            Dim dDefaultDimensionValue As String

            Dim dDimensionValue As Double

            Dim dRoundedValue As Double

            Dim dWholeValue As Double

            Dim dInchValue As Double = 0

            Dim dCentimeterValue As Double

            For i = 1 To invSheet.DrawingDimensions.Count
                invSheet.DrawingDimensions.Item(i).Precision = 2
                dDefaultDimensionValue = invSheet.DrawingDimensions.Item(i).ModelValue
                dDimensionValue = oUOM.ConvertUnits(dDefaultDimensionValue, UnitsTypeEnum.kCentimeterLengthUnits, UnitsTypeEnum.kInchLengthUnits)
                dWholeValue = Fix(dDimensionValue)
                dRoundedValue = RoundToFractional(dDimensionValue, 4, 2)

                If dWholeValue = dRoundedValue Then
                    dInchValue = dWholeValue
                Else
                    dInchValue = dWholeValue + dRoundedValue
                End If

                If dInchValue < dDimensionValue Then
                    dInchValue = dInchValue + 0.25
                End If

                dCentimeterValue = oUOM.ConvertUnits(dInchValue, UnitsTypeEnum.kInchLengthUnits, UnitsTypeEnum.kCentimeterLengthUnits)
                invSheet.DrawingDimensions.Item(i).OverrideModelValue() = dCentimeterValue
                invSheet.DrawingDimensions.Item(i).Text.FormattedText() = """"
            Next

            updatePrecisionTransaction.End()

        End Sub

        Private Sub m_updateCustomerPrecisionButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_updateCustomerPrecisionButtonDef.OnExecute

            Try
                Dim invApp As Inventor.Application
                invApp = m_inventorApplication
                Dim updateCustomerPrecisionTransaction As Transaction
                updateCustomerPrecisionTransaction = invApp.TransactionManager.StartTransaction(invApp.ActiveEditDocument, "updateCustomerPrecision")
                Dim invDrawingDoc As DrawingDocument = invApp.ActiveDocument
                Dim invSheet As Sheet = invDrawingDoc.ActiveSheet
                Dim SketchSymbolDef As SketchedSymbolDefinition = invDrawingDoc.SketchedSymbolDefinitions.Item("Custom Approval")
                Dim invSketchSymbol As SketchedSymbol
                Dim invSketchPosition As Point2d

                invSketchPosition = invApp.TransientGeometry.CreatePoint2d(22.479, 10.388)
                invSketchSymbol = invSheet.SketchedSymbols.Add(SketchSymbolDef, invSketchPosition)
                invSketchSymbol.Static = True

                For i = 1 To invSheet.DrawingDimensions.Count
                    invSheet.DrawingDimensions.Item(i).Precision = 2
                Next

                updateCustomerPrecisionTransaction.End()
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        End Sub

        Private Sub m_createComponentPlaceHolderButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_createComponentPlaceholderButtonDef.OnExecute

            Dim invSheetMetalDoc As PartDocument = m_inventorApplication.ActiveDocument
            Dim strPartName As String = Left(invSheetMetalDoc.DisplayName, 4)

            If strPartName <> "Part" Then
                MessageBox.Show("You must start a new part before using this tool")
                Exit Sub
            End If

            Dim invSheetMetalCompDef As SheetMetalComponentDefinition = invSheetMetalDoc.ComponentDefinition
            If invSheetMetalCompDef.Sketches.Count > 1 Then
                MessageBox.Show("Multible sketches found" + Chr(13) + "To use this tool start a new part" + Chr(13) _
                                + "Exit out of sketch mode" + Chr(13) + "and re-run Create Electrical Component")
                Exit Sub
            End If

            Dim createComponentPlaceHolderDialog As New CreatePlaceHolderComponentDialog(m_inventorApplication)
            createComponentPlaceHolderDialog.cbxMaterialGroup.Focus()
            createComponentPlaceHolderDialog.Show(New WindowWrapper(m_inventorApplication.MainFrameHWND))

        End Sub

        Private Sub m_showBendDirectionComponentButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_showBendDirectionComponentButtonDef.OnExecute

            m_interactionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents
            m_interactionEvents.StatusBarText = "Press Esc to Cancel command."
            m_interactionEvents.Start()

            Dim i As Integer = 1
            ' Set a reference to the active document.
            Dim oPartDoc As PartDocument
            oPartDoc = m_inventorApplication.ActiveDocument

            Dim oSheetMetalCompDef As SheetMetalComponentDefinition
            oSheetMetalCompDef = oPartDoc.ComponentDefinition

            Dim oSheetMetalFeatures As SheetMetalFeatures
            oSheetMetalFeatures = oSheetMetalCompDef.Features

            Dim oSheetMetalFlangeFeature As FlangeFeatures
            oSheetMetalFlangeFeature = oSheetMetalFeatures.FlangeFeatures

            Dim oFlangeDef As FlangeDefinition
            oFlangeDef = oSheetMetalFlangeFeature.Item(1).Definition

            Dim oFlangeAngleParam As Inventor.Parameter
            oFlangeAngleParam = oFlangeDef.FlangeAngle

            Dim strBendAngle = oFlangeAngleParam.Expression

            ' Set a reference to the active flat pattern.
            Dim oFlatPattern As FlatPattern
            oFlatPattern = m_inventorApplication.ActiveEditObject

            Dim oAllBendEdges As EdgeCollection
            oAllBendEdges = m_inventorApplication.TransientObjects.CreateEdgeCollection

            Dim oTempEdge As Edge

            ' Highlight tangent edges in blue.
            Dim oTangentHS As HighlightSet
            oTangentHS = m_inventorApplication.ActiveDocument.CreateHighlightSet
            oTangentHS.Color = m_inventorApplication.TransientObjects.CreateColor(255, 0, 0)

            On Error Resume Next
            FlatPatternInformation()
            If Err.Number Then Err.Clear()

            ' Get all Bend UP edges on top face
            Dim oTopFaceBendUpEdges As Edges
            oTopFaceBendUpEdges = oFlatPattern.GetEdgesOfType(FlatPatternEdgeTypeEnum.kBendUpFlatPatternEdge, True)
            For Each oTempEdge In oTopFaceBendUpEdges
                AddText(i, "UP " + strBendAngle, (oTempEdge.StartVertex.Point.X + oTempEdge.StopVertex.Point.X) / 2, (oTempEdge.StartVertex.Point.Y + oTempEdge.StopVertex.Point.Y) / 2)
                i += 1
            Next

            ' Highlight tangent edges in blue.
            Dim oTangentHS2 As HighlightSet
            oTangentHS2 = m_inventorApplication.ActiveDocument.CreateHighlightSet
            oTangentHS2.Color = m_inventorApplication.TransientObjects.CreateColor(0, 0, 255)

            ' Get all Bend DOWN edges on top face
            Dim oTopFaceBendDownEdges As Edges
            oTopFaceBendDownEdges = oFlatPattern.GetEdgesOfType(FlatPatternEdgeTypeEnum.kBendDownFlatPatternEdge, True)
            For Each oTempEdge In oTopFaceBendDownEdges
                AddText(i, "DN " + strBendAngle, (oTempEdge.StartVertex.Point.X + oTempEdge.StopVertex.Point.X) / 2, (oTempEdge.StartVertex.Point.Y + oTempEdge.StopVertex.Point.Y) / 2)
                i += 1
            Next

            ' Get all Bend UP edges on top face
            oTopFaceBendUpEdges = oFlatPattern.GetEdgesOfType(FlatPatternEdgeTypeEnum.kBendUpFlatPatternEdge, True)
            For Each oTempEdge In oTopFaceBendUpEdges
                oAllBendEdges.Add(oTempEdge)
                oTangentHS.AddItem(oTempEdge)
            Next

            ' Get all Bend DOWN edges on top face
            oTopFaceBendDownEdges = oFlatPattern.GetEdgesOfType(FlatPatternEdgeTypeEnum.kBendDownFlatPatternEdge, True)
            For Each oTempEdge In oTopFaceBendDownEdges
                oAllBendEdges.Add(oTempEdge)
                oTangentHS2.AddItem(oTempEdge)
            Next
            m_inventorApplication.ActiveView.Update()

        End Sub

        Private Sub m_calculateTopAreaComponentButtonDef_OnExecute(ByVal Context As Inventor.NameValueMap) Handles m_calculateTopAreaComponentButtonDef.OnExecute

            m_interactionEvents = m_inventorApplication.CommandManager.CreateInteractionEvents
            m_interactionEvents.StatusBarText = "Select the bottom face."
            m_selection = m_interactionEvents.SelectEvents
            m_selection.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)
            m_interactionEvents.Start()

        End Sub

        Public Sub FlatPatternInformation()

            Dim oDoc As PartDocument
            oDoc = m_inventorApplication.ActiveDocument

            Dim SpaceX As Integer = 200
            Dim SpaceY As Integer = 10
            Dim strThickness As String
            Dim strMaterial As String
            Dim strBendRadius As String
            Dim oExtent As Inventor.Box
            Dim dLength As Double
            Dim dWidth As Double
            Dim sLength As String
            Dim sWidth As String
            Dim oFlatPattern As Inventor.FlatPattern
            oFlatPattern = oDoc.ComponentDefinition.FlatPattern
            strMaterial = oDoc.ComponentDefinition.Material.Name
            ' Get a reference to the parameter controlling the thickness.
            Dim oThicknessParam As Inventor.Parameter
            oThicknessParam = oDoc.ComponentDefinition.Thickness

            Dim oSheetMetalCompDef As SheetMetalComponentDefinition
            oSheetMetalCompDef = oDoc.ComponentDefinition

            Dim oBendRadiusParam As Inventor.Parameter
            oBendRadiusParam = oSheetMetalCompDef.BendRadius
            strBendRadius = oBendRadiusParam.Expression

            strThickness = oThicknessParam.Expression
            oExtent = oFlatPattern.Body.RangeBox

            ' Extract the width and length from the range.
            dLength = System.Math.Round(oExtent.MaxPoint.X, 2) - System.Math.Round(oExtent.MinPoint.X, 2)
            dWidth = System.Math.Round(oExtent.MaxPoint.Y, 2) - System.Math.Round(oExtent.MinPoint.Y, 2)

            Dim oUom As Inventor.UnitsOfMeasure
            oUom = oDoc.UnitsOfMeasure

            sWidth = oUom.GetStringFromValue(dWidth, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)
            sLength = oUom.GetStringFromValue(dLength, Inventor.UnitsTypeEnum.kDefaultDisplayLengthUnits)

            ' Set a reference to the component definition.
            Dim oCompDef As ComponentDefinition
            oCompDef = oDoc.ComponentDefinition

            ' Attempt to get the existing client graphics object.  If it exists
            ' delete it so the rest of the code can continue as if it never existed.
            Dim oClientGraphics As ClientGraphics

            ' Create a new ClientGraphics object.
            oClientGraphics = m_inventorApplication.ActiveEditObject.ClientGraphicsCollection.Add("Flat Information")

            ' Create a graphics node.
            Dim oNode As GraphicsNode
            oNode = oClientGraphics.AddNode(1)

            Dim oTG As TransientGeometry
            oTG = m_inventorApplication.TransientGeometry

            Dim oModelAnchorPoint As Inventor.Point
            If dLength > dWidth * 2 Then
                oModelAnchorPoint = oTG.CreatePoint(oExtent.MinPoint.X, oExtent.MinPoint.Y - 6, 0)
            Else
                oModelAnchorPoint = oTG.CreatePoint(oExtent.MaxPoint.X + 6, oExtent.MaxPoint.Y, 0)
            End If

            ' Create several text graphics objects, one for each font change.  The anchor of the
            ' TextGraphics object defines the position of each text element relative to each other.
            ' Because they're all drawn with pixel scaling behavior these coordinates are in
            ' pixel SpaceX.  They all use the same point as input for the SetTransformBehavior call.
            ' This point is in model SpaceX and defines their anchor within the model.

            ' Draw the first character which is the diameter symbol.
            Dim oTextGraphics(0 To 10) As TextGraphics
            oTextGraphics(1) = oNode.AddTextGraphics
            oTextGraphics(1).Text = "Thickness: "
            oTextGraphics(1).Anchor = oTG.CreatePoint(oModelAnchorPoint.X, oModelAnchorPoint.Y, 0)
            oTextGraphics(1).Font = "ARIAL"
            oTextGraphics(1).FontSize = 18
            oTextGraphics(1).Bold = True
            oTextGraphics(1).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            oTextGraphics(1).PutTextColor(255, 255, 0)
            oTextGraphics(1).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(1).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            oTextGraphics(2) = oNode.AddTextGraphics
            oTextGraphics(2).Text = strThickness
            oTextGraphics(2).Anchor = oTG.CreatePoint(oModelAnchorPoint.X + SpaceX, oModelAnchorPoint.Y, 0)
            oTextGraphics(2).Font = "ARIAL"
            oTextGraphics(2).FontSize = 18
            oTextGraphics(2).Bold = True
            oTextGraphics(2).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextRight
            oTextGraphics(2).PutTextColor(255, 255, 0)
            oTextGraphics(2).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(2).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            ' Draw the next section of the string relative to the first section.
            oTextGraphics(3) = oNode.AddTextGraphics
            oTextGraphics(3).Text = "Material: "
            oTextGraphics(3).Anchor = oTG.CreatePoint(oModelAnchorPoint.X, oModelAnchorPoint.Y - SpaceY * 2, 0)
            oTextGraphics(3).Font = "Arial"
            oTextGraphics(3).FontSize = 18
            oTextGraphics(3).Bold = True
            oTextGraphics(3).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            oTextGraphics(3).PutTextColor(255, 255, 0)
            oTextGraphics(3).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(3).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            oTextGraphics(4) = oNode.AddTextGraphics
            oTextGraphics(4).Text = strMaterial
            oTextGraphics(4).Anchor = oTG.CreatePoint(oModelAnchorPoint.X + SpaceX, oModelAnchorPoint.Y - SpaceY * 2, 0)
            oTextGraphics(4).Font = "ARIAL"
            oTextGraphics(4).FontSize = 18
            oTextGraphics(4).Bold = True
            oTextGraphics(4).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextRight
            oTextGraphics(4).PutTextColor(255, 255, 0)
            oTextGraphics(4).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(4).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            ' Draw the last set of text.s
            oTextGraphics(5) = oNode.AddTextGraphics
            oTextGraphics(5).Text = "Max X Dim: "
            oTextGraphics(5).Anchor = oTG.CreatePoint(oModelAnchorPoint.X, oModelAnchorPoint.Y - SpaceY * 4, 0)
            oTextGraphics(5).Font = "Arial"
            oTextGraphics(5).FontSize = 18
            oTextGraphics(5).Bold = True
            oTextGraphics(5).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            oTextGraphics(5).PutTextColor(255, 255, 0)
            oTextGraphics(5).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(5).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            oTextGraphics(6) = oNode.AddTextGraphics
            oTextGraphics(6).Text = sLength
            oTextGraphics(6).Anchor = oTG.CreatePoint(oModelAnchorPoint.X + SpaceX, oModelAnchorPoint.Y - SpaceY * 4, 0)
            oTextGraphics(6).Font = "ARIAL"
            oTextGraphics(6).FontSize = 18
            oTextGraphics(6).Bold = True
            oTextGraphics(6).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextRight
            oTextGraphics(6).PutTextColor(255, 255, 0)
            oTextGraphics(6).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(6).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            ' Draw the last set of text.s
            oTextGraphics(7) = oNode.AddTextGraphics
            oTextGraphics(7).Text = "Max Y Dim: "
            oTextGraphics(7).Anchor = oTG.CreatePoint(oModelAnchorPoint.X, oModelAnchorPoint.Y - SpaceY * 6, 0)
            oTextGraphics(7).Font = "Arial"
            oTextGraphics(7).FontSize = 18
            oTextGraphics(7).Bold = True
            oTextGraphics(7).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            oTextGraphics(7).PutTextColor(255, 255, 0)
            oTextGraphics(7).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(7).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            oTextGraphics(8) = oNode.AddTextGraphics
            oTextGraphics(8).Text = sWidth
            oTextGraphics(8).Anchor = oTG.CreatePoint(oModelAnchorPoint.X + SpaceX, oModelAnchorPoint.Y - SpaceY * 6, 0)
            oTextGraphics(8).Font = "ARIAL"
            oTextGraphics(8).FontSize = 18
            oTextGraphics(8).Bold = True
            oTextGraphics(8).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextRight
            oTextGraphics(8).PutTextColor(255, 255, 0)
            oTextGraphics(8).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(8).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            ' Draw the last set of text.s
            oTextGraphics(9) = oNode.AddTextGraphics
            oTextGraphics(9).Text = "Bend Radius: "
            oTextGraphics(9).Anchor = oTG.CreatePoint(oModelAnchorPoint.X, oModelAnchorPoint.Y - SpaceY * 8, 0)
            oTextGraphics(9).Font = "Arial"
            oTextGraphics(9).FontSize = 18
            oTextGraphics(9).Bold = True
            oTextGraphics(9).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            oTextGraphics(9).PutTextColor(255, 255, 0)
            oTextGraphics(9).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(9).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            oTextGraphics(10) = oNode.AddTextGraphics
            oTextGraphics(10).Text = strBendRadius
            oTextGraphics(10).Anchor = oTG.CreatePoint(oModelAnchorPoint.X + SpaceX, oModelAnchorPoint.Y - SpaceY * 8, 0)
            oTextGraphics(10).Font = "ARIAL"
            oTextGraphics(10).FontSize = 18
            oTextGraphics(10).Bold = True
            oTextGraphics(10).HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextRight
            oTextGraphics(10).PutTextColor(255, 255, 0)
            oTextGraphics(10).VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextUpper
            oTextGraphics(10).SetTransformBehavior(oModelAnchorPoint, DisplayTransformBehaviorEnum.kFrontFacingAndPixelScaling)

            ' Update the view to see the text.
            m_inventorApplication.ActiveView.Update()
            'Inv.ActiveView.Fit(True)
        End Sub

        Private Sub m_interactionEvents_OnTerminate() Handles m_interactionEvents.OnTerminate

            Dim oClientGraphics As ClientGraphics
            Dim oPartDoc As PartDocument = m_inventorApplication.ActiveDocument
            Dim oPartDocComponentDef As PartComponentDefinition = oPartDoc.ComponentDefinition

            For Each oClientGraphics In oPartDocComponentDef.ClientGraphicsCollection
                oClientGraphics.Delete()
            Next

            m_selection.ResetSelections()
            m_interactionEvents.Stop()
            m_selection = Nothing
            m_interactionEvents = Nothing
            m_inventorApplication.ActiveView.Update()

        End Sub

        Private Sub AddText(ByRef Instance As Integer, ByRef Direction As String, ByRef x As Integer, ByRef y As Integer, Optional ByVal LJustify As Boolean = False)

            Dim oClientGraphics As ClientGraphics
            oClientGraphics = m_inventorApplication.ActiveEditObject.ClientGraphicsCollection.Add("Bend" & Instance)
            ' Create a graphics node.
            Dim oNode As GraphicsNode
            oNode = oClientGraphics.AddNode(1)
            ' Create text graphics.
            Dim oTextGraphics As TextGraphics
            oTextGraphics = oNode.AddTextGraphics

            ' Set the properties of the text.
            oTextGraphics.Text = Direction
            oTextGraphics.Bold = False
            oTextGraphics.Font = "Arial"
            oTextGraphics.FontSize = 18
            oTextGraphics.Italic = False
            oTextGraphics.PutTextColor(255, 255, 0)
            oTextGraphics.VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextLower

            If LJustify = True Then
                ' Set the properties of the text.
                oTextGraphics.Anchor = m_inventorApplication.TransientGeometry.CreatePoint(x, y, 0)
                oTextGraphics.HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft
            Else
                ' Set the properties of the text.
                oTextGraphics.Anchor = m_inventorApplication.TransientGeometry.CreatePoint(x, y, 0)
                oTextGraphics.HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextCenter
            End If

        End Sub

        Private Sub m_selection_OnSelect(ByVal JustSelectedEntities As Inventor.ObjectsEnumerator, ByVal SelectionDevice As Inventor.SelectionDeviceEnum, ByVal ModelPosition As Inventor.Point, ByVal ViewPosition As Inventor.Point2d, ByVal View As Inventor.View) Handles m_selection.OnSelect

            m_interactionEvents.StatusBarText = "Select the Bottom"

            Dim oFace As Face = JustSelectedEntities.Item(1)
            Dim oPartDoc As PartDocument
            oPartDoc = m_inventorApplication.ActiveDocument

            Dim oMassProps As MassProperties
            oMassProps = oPartDoc.ComponentDefinition.MassProperties
            dTotalArea = Math.Round(oMassProps.Area / (2.54 * 2.54), 3)
            dBottomArea = Math.Round(oFace.Evaluator.Area / (2.54 * 2.54), 3)
            m_interactionEvents.StatusBarText = "Select the Top"
            dGuleArea = dTotalArea - dBottomArea
            MessageBox.Show("Glue Area:  " & dGuleArea & " in^2", "Glue Area", MessageBoxButtons.OK, MessageBoxIcon.Information)
            m_selection.ResetSelections()
            m_interactionEvents.Stop()
            m_selection = Nothing
            m_interactionEvents = Nothing
            m_inventorApplication.ActiveView.Update()

        End Sub

        Private Sub m_userInterfaceEvents_OnEnvironmentChange(ByVal Environment As Inventor.Environment, ByVal EnvironmentState As Inventor.EnvironmentStateEnum, ByVal BeforeOrAfter As Inventor.EventTimingEnum, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles m_userInterfaceEvents.OnEnvironmentChange

            If BeforeOrAfter = EventTimingEnum.kAfter And m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                If Environment.DisplayName = "2D Sketch" Then
                    If EnvironmentState = EnvironmentStateEnum.kTerminateEnvironmentState Then
                        Dim oActiveDoc As PartDocument = m_inventorApplication.ActiveDocument
                        Dim oPartCompDef As PartComponentDefinition = oActiveDoc.ComponentDefinition
                        If oPartCompDef.Sketches.Count = 1 And oPartCompDef.Features.Count = 0 Then
                            m_createComponentPlaceholderButtonDef.Enabled = True
                        Else
                            m_createComponentPlaceholderButtonDef.Enabled = False
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub m_sketchEvents_OnDelete(ByVal DocumentObject As Inventor._Document, ByVal Entity As Object, ByVal BeforeOrAfter As Inventor.EventTimingEnum, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles m_sketchEvents.OnDelete

            If BeforeOrAfter = EventTimingEnum.kAfter And m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                Dim oActiveDoc As PartDocument = DocumentObject
                Dim oPartCompDef As PartComponentDefinition = oActiveDoc.ComponentDefinition
                If oPartCompDef.Sketches.Count = 1 And oPartCompDef.Features.Count = 0 Then
                    m_createComponentPlaceholderButtonDef.Enabled = True
                Else
                    m_createComponentPlaceholderButtonDef.Enabled = False
                End If
            End If

        End Sub

        Private Sub m_sketchEvents_OnSketchChange(ByVal DocumentObject As Inventor._Document, ByVal Sketch As Inventor.Sketch, ByVal BeforeOrAfter As Inventor.EventTimingEnum, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles m_sketchEvents.OnSketchChange

            If BeforeOrAfter = EventTimingEnum.kAfter And m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                Dim oActiveDoc As PartDocument = DocumentObject
                Dim oPartCompDef As PartComponentDefinition = oActiveDoc.ComponentDefinition
                If oPartCompDef.Sketches.Count = 1 And oPartCompDef.Features.Count = 0 Then
                    m_createComponentPlaceholderButtonDef.Enabled = True
                Else
                    m_createComponentPlaceholderButtonDef.Enabled = False
                End If
            End If

        End Sub

        Private Sub m_transactionEvents_OnUndo(ByVal TransactionObject As Inventor.Transaction, ByVal Context As Inventor.NameValueMap, ByVal BeforeOrAfter As Inventor.EventTimingEnum, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles m_transactionEvents.OnUndo

            If BeforeOrAfter = EventTimingEnum.kAfter And m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                Dim oActiveDoc As PartDocument = m_inventorApplication.ActiveDocument
                Dim oPartCompDef As PartComponentDefinition = oActiveDoc.ComponentDefinition
                If oPartCompDef.Sketches.Count = 1 And oPartCompDef.Features.Count = 0 Then
                    m_createComponentPlaceholderButtonDef.Enabled = True
                Else
                    m_createComponentPlaceholderButtonDef.Enabled = False
                End If
            End If

        End Sub

        Private Sub m_applicationEvents_OnDocumentChange(ByVal DocumentObject As Inventor._Document, ByVal BeforeOrAfter As Inventor.EventTimingEnum, ByVal ReasonsForChange As Inventor.CommandTypesEnum, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles m_applicationEvents.OnDocumentChange

            If BeforeOrAfter = EventTimingEnum.kAfter And m_inventorApplication.ActiveDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                Dim oActiveDoc As PartDocument = m_inventorApplication.ActiveDocument
                Dim oPartCompDef As PartComponentDefinition = oActiveDoc.ComponentDefinition
                If oPartCompDef.Sketches.Count = 1 And oPartCompDef.Features.Count = 0 Then
                    m_createComponentPlaceholderButtonDef.Enabled = True
                Else
                    m_createComponentPlaceholderButtonDef.Enabled = False
                End If
            End If

        End Sub

#End Region

    End Class

       


End Namespace
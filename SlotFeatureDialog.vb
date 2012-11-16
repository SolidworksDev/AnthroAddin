Imports Inventor
Imports System.Windows.Forms
Imports System.Drawing
Imports stdole.LoadPictureConstants
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System.Collections.Generic



Public Class SlotFeatureDialog

    Public invApp As Inventor.Application
    Private invPartDoc As Inventor.PartDocument
    Private invPartCompDef As Inventor.ComponentDefinition
    Private oPoint As Inventor.Point
    Private sPoint As SketchPoint
    Private sketchPoint As Point2d
    Private oSketch As PlanarSketch
    Private invSketch As Sketch
    Private invCurvesNode As GraphicsNode
    Private oCutDefinition As CutDefinition
    Private oCutFeature As CutFeature
    Private oSheetMetalFeatures As SheetMetalFeatures
    Public Shared transientGeometry As TransientGeometry
    Private createSlotTransaction As Transaction
    Private bHorizontal As Boolean = True
    Private bVertical As Boolean = False
    Private oSlots As ObjectCollection
    Private invClientGraphics As ClientGraphics
    Private oWorkAxis As WorkAxis
    Private bFoundVisableSketch As Boolean = False
    Private clientGraphicNodes As New List(Of ClientGraphicsNodes)
    Private nodeCount As Integer = 1
    Private bFirstClick As Boolean = True

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try

            If Not invClientGraphics Is Nothing Then
                invClientGraphics.Delete()
            End If

            Me.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        Dim oProfile As Profile = Nothing
        Dim SlotFeaturePicture As IPictureDisp
        Dim invClientNodeResource As ClientNodeResource
        Dim invBrowserModelPane As BrowserPane = invPartDoc.BrowserPanes.Item("Model")
        Dim invBrowserTopNode As BrowserNode = invBrowserModelPane.TopNode
        Dim invBrowserNode As BrowserNode
        Dim invNativeBrowserNodeDef As NativeBrowserNodeDefinitionObject       

        Try
            iFeatureCount = GetSlotCount(invPartDoc)
            iFeatureCount += 1
            Dim strSlotName As String = "Slot" + iFeatureCount.ToString
           
            SlotFeaturePicture = AnthroAddIn.PictureDispConverter.ToIPictureDisp(My.Resources.BrowserSlotPNG)

            invClientNodeResource = invPartDoc.BrowserPanes.ClientNodeResources.Add(strSlotName, 1, Nothing)

            oSlots = invApp.TransientObjects.CreateObjectCollection

            For i = 1 To invSketch.SketchPoints.Count
                If invSketch.SketchPoints.Item(i).HoleCenter = True Then
                    sketchPoint = invSketch.SketchPoints.Item(i).Geometry
                    oSketch = invSketch
                    oPoint = oSketch.SketchToModelSpace(sketchPoint)
                    sPoint = invSketch.SketchPoints.Item(i)
                    DrawSlot(sketchPoint, i)
                    If cbWorkAxis.Checked Then
                        oWorkAxis = invPartCompDef.WorkAxes.AddByNormalToSurface(oSketch, sPoint)
                        oWorkAxis.AutoResize = True
                    End If
                End If
            Next

            oProfile = oSketch.Profiles.AddForSolid(False, oSlots)

            oSheetMetalFeatures = invPartCompDef.Features

            oCutDefinition = oSheetMetalFeatures.CutFeatures.CreateCutDefinition(oProfile)

            oCutDefinition.SetToNextExtent(PartFeatureExtentDirectionEnum.kNegativeExtentDirection)

            oCutFeature = oSheetMetalFeatures.CutFeatures.Add(oCutDefinition)
            oCutFeature.Name = strSlotName
            invClientNodeResource.Icon = SlotFeaturePicture

            Dim strNodeName As String
            
            For i = 1 To invBrowserTopNode.BrowserNodes.Count
                invBrowserNode = invBrowserTopNode.BrowserNodes.Item(i)
                For j = 1 To invBrowserNode.BrowserNodes.Count
                    strNodeName = invBrowserNode.BrowserNodes.Item(j).BrowserNodeDefinition.Label
                    If strNodeName.Contains("Slot") Then
                        invNativeBrowserNodeDef = invBrowserNode.BrowserNodes.Item(j).BrowserNodeDefinition
                        invNativeBrowserNodeDef.OverrideIcon() = invClientNodeResource
                    End If
                Next (j)
            Next (i)

            oSketch.Shared = False
            oSketch.Visible = False

            oSlots.Clear()
            invCurvesNode.Delete()
            invClientGraphics.Delete()
            invApp.ActiveView.Update()

            Me.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub DrawSlot(ByVal oPoint2d As Point2d, ByVal index As Integer)

        Dim oUOM As UnitsOfMeasure = invApp.ActiveDocument.UnitsOfMeasure

        'draw the sketch for the slot
        Dim lines(2) As SketchLine
        Dim arcs(2) As SketchArc
        Dim point As SketchPoint

        Dim dFirstPointX As Double
        Dim dFirstPointY As Double
        Dim dSecondPointX As Double
        Dim dSecondPointY As Double

        Try

            Dim dSlotHeight As Double = oUOM.GetValueFromExpression(txtbHeigth.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)
            Dim dSlotWidth As Double = oUOM.GetValueFromExpression(txtbWidth.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)

            Dim transientGeometry As TransientGeometry = invApp.TransientGeometry

            If rbtnHorizontal.Checked = True Then

                dFirstPointX = oPoint2d.X + (dSlotHeight / 2)
                dFirstPointY = oPoint2d.Y - (dSlotWidth / 2)
                dSecondPointX = oPoint2d.X + (dSlotHeight / 2)
                dSecondPointY = oPoint2d.Y + (dSlotWidth / 2)

                'draw the lines and arcs that make up the shape of the slot
                lines(1) = oSketch.SketchLines.AddByTwoPoints(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                                   transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(lines(1))

                dFirstPointX = oPoint2d.X + (dSlotHeight / 2)
                dFirstPointY = oPoint2d.Y
                point = oSketch.SketchPoints.Add(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), False)
                oSketch.GeometricConstraints.AddMidpoint(point, lines(1))
                oSketch.GeometricConstraints.AddHorizontalAlign(point, sPoint)

                dFirstPointX = oPoint2d.X
                dFirstPointY = oPoint2d.Y + (dSlotWidth / 2)
                dSecondPointX = oPoint2d.X - dSlotHeight / 2
                dSecondPointY = oPoint2d.Y + (dSlotWidth / 2)

                arcs(1) = oSketch.SketchArcs.AddByCenterStartEndPoint(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                                           lines(1).EndSketchPoint, _
                                                                           transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(arcs(1))

                dSecondPointX = oPoint2d.X - dSlotHeight / 2
                dSecondPointY = oPoint2d.Y - (dSlotWidth / 2)

                lines(2) = oSketch.SketchLines.AddByTwoPoints(arcs(1).EndSketchPoint, transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(lines(2))

                dFirstPointX = oPoint2d.X
                dFirstPointY = oPoint2d.Y - (dSlotWidth / 2)

                arcs(2) = oSketch.SketchArcs.AddByCenterStartEndPoint(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                                           lines(2).EndSketchPoint, lines(1).StartSketchPoint)

                oSlots.Add(arcs(2))

                oPoint2d.X = oPoint2d.X - (dSlotHeight / 2) - 0.75
                oSketch.DimensionConstraints.AddTwoPointDistance(lines(2).StartSketchPoint, lines(2).EndSketchPoint, Inventor.DimensionOrientationEnum.kAlignedDim, oPoint2d)

                oPoint2d.Y = oPoint2d.Y + ((dSlotWidth - dSlotHeight) / 2) + 0.75
                oSketch.DimensionConstraints.AddRadius(arcs(1), oPoint2d)

                oSketch.GeometricConstraints.AddVerticalAlign(sPoint, arcs(1).CenterSketchPoint)
                oSketch.GeometricConstraints.AddVerticalAlign(sPoint, arcs(2).CenterSketchPoint)
                oSketch.GeometricConstraints.AddTangent(lines(1), arcs(1))
                oSketch.GeometricConstraints.AddTangent(lines(2), arcs(1))
                oSketch.GeometricConstraints.AddTangent(lines(2), arcs(2))
                oSketch.GeometricConstraints.AddTangent(lines(1), arcs(2))
                oSketch.GeometricConstraints.AddParallel(lines(1), lines(2))

            End If

            If rbtnVertical.Checked = True Then

                dFirstPointX = oPoint2d.X + (dSlotWidth / 2)
                dFirstPointY = oPoint2d.Y + (dSlotHeight / 2)
                dSecondPointX = oPoint2d.X - (dSlotWidth / 2)
                dSecondPointY = oPoint2d.Y + (dSlotHeight / 2)

                lines(1) = oSketch.SketchLines.AddByTwoPoints(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                                   transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(lines(1))

                dFirstPointX = oPoint2d.X
                dFirstPointY = oPoint2d.Y + (dSlotHeight / 2)
                point = oSketch.SketchPoints.Add(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), False)
                oSketch.GeometricConstraints.AddMidpoint(point, lines(1))
                oSketch.GeometricConstraints.AddVerticalAlign(point, sPoint)

                dFirstPointX = oPoint2d.X - (dSlotWidth / 2)
                dFirstPointY = oPoint2d.Y
                dSecondPointX = oPoint2d.X - (dSlotWidth / 2)
                dSecondPointY = oPoint2d.Y - (dSlotHeight / 2)

                arcs(1) = oSketch.SketchArcs.AddByCenterStartEndPoint(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                                           lines(1).EndSketchPoint, _
                                                                           transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(arcs(1))

                dSecondPointX = oPoint2d.X + (dSlotWidth / 2)
                dSecondPointY = oPoint2d.Y - (dSlotHeight / 2)

                lines(2) = oSketch.SketchLines.AddByTwoPoints(arcs(1).EndSketchPoint, transientGeometry.CreatePoint2d(dSecondPointX, dSecondPointY))

                oSlots.Add(lines(2))

                dFirstPointX = oPoint2d.X + (dSlotWidth / 2)
                dFirstPointY = oPoint2d.Y

                arcs(2) = oSketch.SketchArcs.AddByCenterStartEndPoint(transientGeometry.CreatePoint2d(dFirstPointX, dFirstPointY), _
                                                              lines(2).EndSketchPoint, lines(1).StartSketchPoint)

                oSlots.Add(arcs(2))

                oPoint2d.Y = oPoint2d.Y + (dSlotHeight / 2) + 0.75
                oSketch.DimensionConstraints.AddTwoPointDistance(lines(1).StartSketchPoint, lines(1).EndSketchPoint, Inventor.DimensionOrientationEnum.kAlignedDim, oPoint2d)

                oPoint2d.X = oPoint2d.X - ((dSlotWidth - dSlotHeight) / 2) - 0.75
                oSketch.DimensionConstraints.AddRadius(arcs(1), oPoint2d)

                oSketch.GeometricConstraints.AddHorizontalAlign(sPoint, arcs(1).CenterSketchPoint)
                oSketch.GeometricConstraints.AddHorizontalAlign(sPoint, arcs(2).CenterSketchPoint)
                oSketch.GeometricConstraints.AddTangent(lines(1), arcs(1))
                oSketch.GeometricConstraints.AddTangent(lines(2), arcs(1))
                oSketch.GeometricConstraints.AddTangent(lines(2), arcs(2))
                oSketch.GeometricConstraints.AddTangent(lines(1), arcs(2))
                oSketch.GeometricConstraints.AddParallel(lines(1), lines(2))

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub DrawClientGraphics(ByVal oPoint2d As Point2d, ByVal oPoint As Inventor.Point, ByVal index As Integer)

        Dim oUOM As UnitsOfMeasure = invApp.ActiveDocument.UnitsOfMeasure

        Dim Coord2d(1) As Double
        Dim oStartPoint2d As Point2d
        Dim oEndPoint2d As Point2d
        Dim oCenter2d As Point2d
        Dim oStartPoint As Inventor.Point
        Dim oEndPoint As Inventor.Point
        Dim oCenter As Inventor.Point

        Try

            Dim invSheetMetalCompDef As SheetMetalComponentDefinition = invPartDoc.ComponentDefinition
            Dim invThickness As Double = invSheetMetalCompDef.Parameters("Thickness").Value / 2.54

            Dim dSlotHeight As Double = oUOM.GetValueFromExpression(txtbHeigth.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)
            Dim dSlotWidth As Double = oUOM.GetValueFromExpression(txtbWidth.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)
            invCurvesNode = invClientGraphics.AddNode(nodeCount)
            Dim newGraphicsNode As New ClientGraphicsNodes(oPoint, invCurvesNode.Id)
            clientGraphicNodes.Add(newGraphicsNode)

            invCurvesNode.RenderStyle = invPartDoc.RenderStyles.Item("Red")

            If rbtnHorizontal.Checked = True Then

                oPoint2d.GetPointData(Coord2d)
                Coord2d(1) = Coord2d(1) + dSlotWidth / 2 + dSlotHeight / 2
                oCenter2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))
                Coord2d(1) = Coord2d(1) - dSlotHeight / 2
                Coord2d(0) = Coord2d(0) + dSlotHeight / 2
                oEndPoint2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))
                Coord2d(0) = Coord2d(0) - dSlotHeight
                oStartPoint2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))
                oStartPoint = oSketch.SketchToModelSpace(oStartPoint2d)
                oEndPoint = oSketch.SketchToModelSpace(oEndPoint2d)
                oCenter = oSketch.SketchToModelSpace(oCenter2d)

                ' Offset the current points by the material thichness                

                ' Create a transient arc object
                Dim oArc1 As Arc3d
                oArc1 = transientGeometry.CreateArc3dByThreePoints(oStartPoint, oCenter, oEndPoint)                

                ' Create an arc graphics object within the node.
                Dim oArcGraphics1 As CurveGraphics
                oArcGraphics1 = invCurvesNode.AddCurveGraphics(oArc1)
                oArcGraphics1.BurnThrough = True

                oPoint2d.GetPointData(Coord2d)
                Coord2d(1) = Coord2d(1) - dSlotWidth / 2 - dSlotHeight / 2
                oCenter2d.PutPointData(Coord2d)
                Coord2d(1) = Coord2d(1) + dSlotHeight / 2
                Coord2d(0) = Coord2d(0) - dSlotHeight / 2
                oEndPoint2d.PutPointData(Coord2d)
                Coord2d(0) = Coord2d(0) + dSlotHeight
                oStartPoint2d.PutPointData(Coord2d)
                oStartPoint = oSketch.SketchToModelSpace(oStartPoint2d)
                oEndPoint = oSketch.SketchToModelSpace(oEndPoint2d)
                oCenter = oSketch.SketchToModelSpace(oCenter2d)

                Dim oArc2 As Arc3d
                oArc2 = transientGeometry.CreateArc3dByThreePoints(oStartPoint, oCenter, oEndPoint)

                Dim oArcGraphics2 As CurveGraphics
                oArcGraphics2 = invCurvesNode.AddCurveGraphics(oArc2)
                oArcGraphics2.BurnThrough = True

                ' Create a transient line segment object
                Dim oLineSegment1 As LineSegment
                oLineSegment1 = transientGeometry.CreateLineSegment(oArc1.StartPoint, oArc2.EndPoint)

                ' Create an line graphics object within the node.
                Dim oLineGraphics1 As CurveGraphics
                oLineGraphics1 = invCurvesNode.AddCurveGraphics(oLineSegment1)
                oLineGraphics1.BurnThrough = True

                ' Create a transient line segment object
                Dim oLineSegment2 As LineSegment
                oLineSegment2 = transientGeometry.CreateLineSegment(oArc2.StartPoint, oArc1.EndPoint)

                ' Create an line graphics object within the node.
                Dim oLineGraphics2 As CurveGraphics
                oLineGraphics2 = invCurvesNode.AddCurveGraphics(oLineSegment2)
                oLineGraphics2.BurnThrough = True

            End If

            If rbtnVertical.Checked = True Then

                oPoint2d.GetPointData(Coord2d)
                Coord2d(0) = Coord2d(0) + dSlotWidth / 2 + dSlotHeight / 2
                oCenter2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))
                Coord2d(0) = Coord2d(0) - dSlotHeight / 2
                Coord2d(1) = Coord2d(1) + dSlotHeight / 2
                oEndPoint2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))
                Coord2d(1) = Coord2d(1) - dSlotHeight
                oStartPoint2d = transientGeometry.CreatePoint2d(Coord2d(0), Coord2d(1))

                oStartPoint = oSketch.SketchToModelSpace(oStartPoint2d)
                oEndPoint = oSketch.SketchToModelSpace(oEndPoint2d)
                oCenter = oSketch.SketchToModelSpace(oCenter2d)

                ' Create a transient arc object
                Dim oArc1 As Arc3d
                oArc1 = transientGeometry.CreateArc3dByThreePoints(oStartPoint, oCenter, oEndPoint)


                ' Create an arc graphics object within the node.
                Dim oArcGraphics1 As CurveGraphics
                oArcGraphics1 = invCurvesNode.AddCurveGraphics(oArc1)
                oArcGraphics1.BurnThrough = True

                oPoint2d.GetPointData(Coord2d)
                Coord2d(0) = Coord2d(0) - dSlotWidth / 2 - dSlotHeight / 2
                oCenter2d.PutPointData(Coord2d)
                Coord2d(0) = Coord2d(0) + dSlotHeight / 2
                Coord2d(1) = Coord2d(1) - dSlotHeight / 2
                oEndPoint2d.PutPointData(Coord2d)
                Coord2d(1) = Coord2d(1) + dSlotHeight
                oStartPoint2d.PutPointData(Coord2d)

                oStartPoint = oSketch.SketchToModelSpace(oStartPoint2d)
                oEndPoint = oSketch.SketchToModelSpace(oEndPoint2d)
                oCenter = oSketch.SketchToModelSpace(oCenter2d)

                Dim oArc2 As Arc3d
                oArc2 = transientGeometry.CreateArc3dByThreePoints(oStartPoint, oCenter, oEndPoint)

                Dim oArcGraphics2 As CurveGraphics
                oArcGraphics2 = invCurvesNode.AddCurveGraphics(oArc2)
                oArcGraphics2.BurnThrough = True

                ' Create a transient line segment object
                Dim oLineSegment1 As LineSegment
                oLineSegment1 = transientGeometry.CreateLineSegment(oArc1.StartPoint, oArc2.EndPoint)

                ' Create an line graphics object within the node.
                Dim oLineGraphics1 As CurveGraphics
                oLineGraphics1 = invCurvesNode.AddCurveGraphics(oLineSegment1)
                oLineGraphics1.BurnThrough = True

                ' Create a transient line segment object
                Dim oLineSegment2 As LineSegment
                oLineSegment2 = transientGeometry.CreateLineSegment(oArc2.StartPoint, oArc1.EndPoint)

                ' Create an line graphics object within the node.
                Dim oLineGraphics2 As CurveGraphics
                oLineGraphics2 = invCurvesNode.AddCurveGraphics(oLineSegment2)
                oLineGraphics2.BurnThrough = True

            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub CopyClientGraphics(ByVal oPoint As Inventor.Point, ByVal OriginPoint As Inventor.Point)

        Dim translationVector As Vector = OriginPoint.VectorTo(oPoint)       
        Dim invMatrix As Matrix = invCurvesNode.Transformation
        invMatrix.SetTranslation(translationVector)
        Dim newNode As GraphicsNode
        nodeCount += 1
        newNode = invCurvesNode.Copy(invMatrix, nodeCount)
        invClientGraphics.AddNode(newNode.Id)
        newNode.Transformation = invMatrix
        Dim newGraphicsNode As New ClientGraphicsNodes(oPoint, newNode.Id)
        clientGraphicNodes.Add(newGraphicsNode)

    End Sub

    Private Sub RotateClientGraphics(ByVal oPoint As Inventor.Point, ByVal OriginPoint As Inventor.Point, ByVal currentGraphicsNode As GraphicsNode)

        Dim dPi As Double = Math.Atan(1) * 4
        Dim sketchPlane As Plane = oSketch.PlanarEntityGeometry
        Dim axisVector As Vector = sketchPlane.Normal.AsVector
        Dim translationVector As Vector = OriginPoint.VectorTo(oPoint)
        Dim invMatrix As Matrix = currentGraphicsNode.Transformation
        Dim invTempMatrix As Matrix = transientGeometry.CreateMatrix
        If bFirstClick = True Then
            invMatrix.SetTranslation(translationVector)            
        End If
        invTempMatrix.SetToRotation(dPi / 2, axisVector, oPoint)
        invMatrix.TransformBy(invTempMatrix)
        currentGraphicsNode.Transformation = invMatrix

    End Sub

    Private Sub SlotFeatureDialog_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        If Not createSlotTransaction Is Nothing Then
            createSlotTransaction.End()
        End If

    End Sub

    Protected Overrides Function ProcessKeyPreview(ByRef m As System.Windows.Forms.Message) As Boolean

        Try
            Const WM_KEYDOWN As Integer = &H100
            Dim keycode As System.Windows.Forms.Keys = CType(CInt(m.WParam), System.Windows.Forms.Keys) And Windows.Forms.Keys.KeyCode

            If (m.Msg = WM_KEYDOWN And keycode = Windows.Forms.Keys.Tab) Then
                Return Me.ProcessTabKey(True)
            End If
            Return MyBase.ProcessKeyPreview(m)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Function

    Private Sub rbtnHorizontal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnHorizontal.Click

        Try

            Dim strHLastError As String
            Dim strWLastError As String
            Dim bFirstPoint As Boolean = False
            strHLastError = ErrorProvider1.GetError(txtbHeigth)
            strWLastError = ErrorProvider1.GetError(txtbWidth)

            If Not strHLastError Is "" Then
                txtbHeigth.Focus()
                rbtnHorizontal.Checked = False
                Exit Sub
            End If

            If Not strWLastError Is "" Then
                txtbWidth.Focus()
                rbtnHorizontal.Checked = False
                Exit Sub
            End If

            Dim iVisableSketchCount As Integer = 0

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

            For i = 0 To clientGraphicNodes.Count - 1
                RotateClientGraphics(clientGraphicNodes(i).centerPoint, clientGraphicNodes(0).centerPoint, invClientGraphics.ItemById(clientGraphicNodes(i).id))
            Next

            bFirstClick = False

            invApp.ActiveView.Update()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub rbtnVertical_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnVertical.Click

        Try

            Dim strHLastError As String
            Dim strWLastError As String
            strHLastError = ErrorProvider1.GetError(txtbHeigth)
            strWLastError = ErrorProvider1.GetError(txtbWidth)

            If Not strHLastError Is "" Then
                txtbHeigth.Focus()
                rbtnVertical.Checked = False
                Exit Sub
            End If

            If Not strWLastError Is "" Then
                txtbWidth.Focus()
                rbtnVertical.Checked = False
                Exit Sub
            End If

            Dim iVisableSketchCount As Integer = 0

            For i = 1 To invPartCompDef.Sketches.Count
                invSketch = invPartCompDef.Sketches.Item(i)
                If invSketch.Visible = True Then
                    iVisableSketchCount += 1
                    If iVisableSketchCount > 1 Then
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

            For i = 0 To clientGraphicNodes.Count - 1
                RotateClientGraphics(clientGraphicNodes(i).centerPoint, clientGraphicNodes(0).centerPoint, invClientGraphics.ItemById(clientGraphicNodes(i).id))               
            Next

            bFirstClick = False

            invApp.ActiveView.Update()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub SlotFeatureDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Dim iVisableSketchCount As Integer = 0
        Dim bHoleCentersFound As Boolean = False
        Dim bFirstPoint As Boolean = False
        Dim OriginPoint As Inventor.Point = transientGeometry.CreatePoint(0, 0, 0)

        Try

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

            oSketch = invSketch

            For i = 1 To invSketch.SketchPoints.Count
                If invSketch.SketchPoints.Item(i).HoleCenter = True Then
                    If bFirstPoint = False Then
                        invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                        sketchPoint = invSketch.SketchPoints.Item(i).Geometry                        
                        oPoint = oSketch.SketchToModelSpace(sketchPoint)
                        sPoint = invSketch.SketchPoints.Item(i)
                        DrawClientGraphics(sPoint.Geometry, oPoint, i)
                        OriginPoint = oPoint
                        bFirstPoint = True
                        bHoleCentersFound = True
                    Else
                        invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                        sketchPoint = invSketch.SketchPoints.Item(i).Geometry                        
                        oPoint = oSketch.SketchToModelSpace(sketchPoint)
                        sPoint = invSketch.SketchPoints.Item(i)
                        CopyClientGraphics(oPoint, OriginPoint)
                    End If
                End If
            Next

            invApp.ActiveView.Update()

            If bHoleCentersFound = False Then
                MsgBox("No Hole Centers Found in Sketch")
                Exit Sub
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize the class variables.

        Try

            invApp = ThisApp
            createSlotTransaction = invApp.TransactionManager.StartTransaction(invApp.ActiveEditDocument, "CreateSlot")

            invPartDoc = invApp.ActiveEditDocument
            invPartCompDef = invPartDoc.ComponentDefinition

            If invClientGraphics Is Nothing Then
                invClientGraphics = invPartCompDef.ClientGraphicsCollection.Add("ClientGraphics")
            Else
                invClientGraphics.Delete()
                invClientGraphics = invPartCompDef.ClientGraphicsCollection.Add("ClientGraphics")
            End If

            transientGeometry = invApp.TransientGeometry

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        createSlotTransaction.End()
        MyBase.Finalize()
    End Sub

    Private Sub txtbHeigth_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbHeigth.Leave

        Try

            Dim bFirstPoint As Boolean = False
            Dim bHoleCentersFound As Boolean = False
            Dim OriginPoint As Inventor.Point = transientGeometry.CreatePoint(0, 0, 0)
            Dim strLastError As String
            strLastError = ErrorProvider1.GetError(txtbHeigth)

            If Not strLastError Is "" Then
                txtbHeigth.Focus()
                Exit Sub
            End If

            If bLeavingSlotFeatureDlg = False Then

                invClientGraphics.Delete()
                clientGraphicNodes.RemoveRange(0, clientGraphicNodes.Count)                
                invClientGraphics = invPartCompDef.ClientGraphicsCollection.Add("ClientGraphics")

                Dim iVisableSketchCount As Integer = 0

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

                oSketch = invSketch

                For i = 1 To invSketch.SketchPoints.Count
                    If invSketch.SketchPoints.Item(i).HoleCenter = True Then
                        If bFirstPoint = False Then
                            invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                            sketchPoint = invSketch.SketchPoints.Item(i).Geometry
                            oPoint = oSketch.SketchToModelSpace(sketchPoint)
                            sPoint = invSketch.SketchPoints.Item(i)
                            DrawClientGraphics(sPoint.Geometry, oPoint, i)
                            OriginPoint = oPoint
                            bFirstPoint = True
                            bHoleCentersFound = True
                        Else
                            invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                            sketchPoint = invSketch.SketchPoints.Item(i).Geometry
                            oPoint = oSketch.SketchToModelSpace(sketchPoint)
                            sPoint = invSketch.SketchPoints.Item(i)
                            CopyClientGraphics(oPoint, OriginPoint)
                        End If
                    End If
                Next

            End If

            invApp.ActiveView.Update()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtbWidth_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbWidth.Leave

        Try

            Dim bFirstPoint As Boolean = False
            Dim bHoleCentersFound As Boolean = False
            Dim OriginPoint As Inventor.Point = transientGeometry.CreatePoint(0, 0, 0)
            Dim strLastError As String
            strLastError = ErrorProvider1.GetError(txtbWidth)

            If Not strLastError Is "" Then
                txtbWidth.Focus()
                Exit Sub
            End If

            If bLeavingSlotFeatureDlg = False Then

                invClientGraphics.Delete()
                clientGraphicNodes.RemoveRange(0, clientGraphicNodes.Count)                
                invClientGraphics = invPartCompDef.ClientGraphicsCollection.Add("ClientGraphics")

                Dim iVisableSketchCount As Integer = 0

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

                For i = 1 To invSketch.SketchPoints.Count
                    If invSketch.SketchPoints.Item(i).HoleCenter = True Then
                        If bFirstPoint = False Then
                            invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                            sketchPoint = invSketch.SketchPoints.Item(i).Geometry
                            oPoint = oSketch.SketchToModelSpace(sketchPoint)
                            sPoint = invSketch.SketchPoints.Item(i)
                            DrawClientGraphics(sPoint.Geometry, oPoint, i)
                            OriginPoint = oPoint
                            bFirstPoint = True
                            bHoleCentersFound = True
                        Else
                            invPartDoc.SelectSet.Select(invSketch.SketchPoints.Item(i))
                            sketchPoint = invSketch.SketchPoints.Item(i).Geometry
                            oPoint = oSketch.SketchToModelSpace(sketchPoint)
                            sPoint = invSketch.SketchPoints.Item(i)
                            CopyClientGraphics(oPoint, OriginPoint)
                        End If
                    End If                    
                Next

            End If

            invApp.ActiveView.Update()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub txtbWidth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbWidth.TextChanged

        Try
            If String.IsNullOrEmpty(txtbWidth.Text) Then
                txtbWidth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbWidth, "Floating point value required")
                invApp.StatusBarText = "Input must be a floating point number"
            End If

            Dim X As Double = Double.Parse(txtbWidth.Text)
            Dim dSlotWidth As Double = Convert.ToDouble(txtbWidth.Text)

            If dSlotWidth = 0 Then
                txtbWidth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbWidth, "Width can not be 0!")
                invApp.StatusBarText = "Width can not be 0!"
                Exit Sub
            End If

            txtbWidth.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            ErrorProvider1.SetError(txtbWidth, "Floating point value required")
            invApp.StatusBarText = "Input must be a floating point number"
            txtbWidth.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Private Sub txtbWidth_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtbWidth.Validating

        If bLeavingSlotFeatureDlg = True Then
            e.Cancel = False
            bLeavingSlotFeatureDlg = False
            Return
        End If

        Try
            If String.IsNullOrEmpty(txtbWidth.Text) Then
                txtbWidth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbWidth, "Floating point value required")
                e.Cancel = True
            End If

            Dim X As Double = Double.Parse(txtbWidth.Text)
            Dim dSlotWidth As Double = Convert.ToDouble(txtbWidth.Text)

            If dSlotWidth = 0 Then
                txtbWidth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbWidth, "Width can not be 0!")
                invApp.StatusBarText = "Width can not be 0!"
                e.Cancel = True
                Exit Sub
            End If

            txtbWidth.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            ErrorProvider1.SetError(txtbWidth, "Floating point value required")
            invApp.StatusBarText = "Input must be a floating point number"
            txtbWidth.ForeColor = Drawing.Color.Red
            e.Cancel = True
        End Try

    End Sub

    Private Sub txtbHeigth_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtbHeigth.TextChanged

        Try
            If String.IsNullOrEmpty(txtbHeigth.Text) Then
                txtbHeigth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbHeigth, "Floating point value required")
            End If

            Dim X As Double = Double.Parse(txtbHeigth.Text)
            Dim dSlotHeigth As Double = Convert.ToDouble(txtbHeigth.Text)

            If dSlotHeigth = 0 Then
                txtbHeigth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbHeigth, "Width can not be 0!")
                invApp.StatusBarText = "Width can not be 0!"
                Exit Sub
            End If

            txtbHeigth.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            ErrorProvider1.SetError(txtbHeigth, "Floating point value required")
            invApp.StatusBarText = "Input must be a floating point number"
            txtbHeigth.ForeColor = Drawing.Color.Red
        End Try

    End Sub

    Private Sub txtbHeigth_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtbHeigth.Validating

        If bLeavingSlotFeatureDlg = True Then
            e.Cancel = False
            bLeavingSlotFeatureDlg = False
            Return
        End If

        Try
            If String.IsNullOrEmpty(txtbHeigth.Text) Then
                txtbHeigth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbHeigth, "Floating point value required")
                e.Cancel = True
            End If

            Dim X As Double = Double.Parse(txtbHeigth.Text)
            Dim dSlotHeigth As Double = Convert.ToDouble(txtbHeigth.Text)

            If dSlotHeigth = 0 Then
                txtbHeigth.ForeColor = Drawing.Color.Red
                ErrorProvider1.SetError(txtbHeigth, "Width can not be 0!")
                invApp.StatusBarText = "Width can not be 0!"
                e.Cancel = True
                Exit Sub
            End If

            txtbHeigth.ForeColor = Drawing.Color.Black
            ErrorProvider1.Clear()
        Catch ex As Exception
            ErrorProvider1.SetError(txtbHeigth, "Floating point value required")
            invApp.StatusBarText = "Input must be a floating point number"
            txtbHeigth.ForeColor = Drawing.Color.Red
            e.Cancel = True
        End Try

    End Sub

    Private Sub btnCancel_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnCancel.MouseClick
        bLeavingSlotFeatureDlg = True
    End Sub

    Private Sub btnCancel_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseHover
        bLeavingSlotFeatureDlg = True
    End Sub

    Private Sub btnCancel_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.MouseLeave
        bLeavingSlotFeatureDlg = False
    End Sub

    Private Function GetSlotCount(ByVal invPartDoc As PartDocument) As Integer

        Dim invBrowserModelPane As BrowserPane = invPartDoc.BrowserPanes.Item("Model")
        Dim invBrowserTopNode As BrowserNode = invBrowserModelPane.TopNode
        Dim invBrowserNode As BrowserNode
        Dim strNodeName As String
        Dim strSlotCount As String
        Dim iSlotCount As Integer = 0
        Dim iReturnSlotCount As Integer = 0

        For i = 1 To invBrowserTopNode.BrowserNodes.Count
            invBrowserNode = invBrowserTopNode.BrowserNodes.Item(i)
            For j = 1 To invBrowserNode.BrowserNodes.Count
                strNodeName = invBrowserNode.BrowserNodes.Item(j).BrowserNodeDefinition.Label
                If strNodeName.Contains("Slot") Then
                    strSlotCount = strNodeName.Remove(0, 4)
                    iSlotCount = Convert.ToInt16(strSlotCount)
                    If iSlotCount > iReturnSlotCount Then
                        iReturnSlotCount = iSlotCount
                    End If

                End If
            Next (j)
        Next (i)

        Return iReturnSlotCount

    End Function

    Friend Class ClientGraphicsNodes

        Public centerPoint As Inventor.Point = transientGeometry.CreatePoint(0, 0, 0)
        Public id As Integer

        Public Sub New(ByVal point As Inventor.Point, ByVal index As Integer)
            centerPoint = point
            id = index
        End Sub
    End Class

End Class


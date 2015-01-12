Imports Inventor
Imports System.IO
Imports System.Security
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity





Public Class DrawRectangleDialog

    Public invApp As Inventor.Application
    Private invPartDoc As Inventor.PartDocument
    Private invPartCompDef As Inventor.ComponentDefinition
    Private invSketch As Sketch
    Private invPlanarSketch As PlanarSketch
    Private createRecTransaction As Transaction
    Private transGeo As TransientGeometry

    Public Sub New(ByVal ThisApp As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        invApp = ThisApp
        createRecTransaction = invApp.TransactionManager.StartTransaction(invApp.ActiveEditDocument, "DrawRec")

    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        invPartDoc = invApp.ActiveEditDocument
        invPartCompDef = invPartDoc.ComponentDefinition
        invSketch = invApp.ActiveEditObject
        invPlanarSketch = invSketch
        transGeo = invApp.TransientGeometry

        Dim oUOM As UnitsOfMeasure = invApp.ActiveDocument.UnitsOfMeasure

        Dim dWidth1 As Double = oUOM.GetValueFromExpression("2", UnitsTypeEnum.kDefaultDisplayLengthUnits)

        Dim dWidth As Double = oUOM.GetValueFromExpression(txtbxWidth.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)
        Dim dHeight As Double = oUOM.GetValueFromExpression(txtbxHeight.Text, UnitsTypeEnum.kDefaultDisplayLengthUnits)
        Dim dUpperLeftX As Double = -(dWidth / 2)
        Dim dUpperLeftY As Double = (dHeight / 2)
        Dim lines(4) As SketchLine
        Dim invSPointY As SketchPoint
        Dim invSPointX As SketchPoint
        Dim oPoint2dY As Point2d = transGeo.CreatePoint2d(0, (dHeight / 2) + 0.75)
        Dim oPoint2dX As Point2d = transGeo.CreatePoint2d((dWidth / 2) + 0.75, 0)
        Dim oPoint2d As Point2d = transGeo.CreatePoint2d(0, 0)
        Dim oSPoint2d As SketchPoint = Nothing
        Dim transientGeometry As TransientGeometry = invApp.TransientGeometry

        For i = 1 To invPlanarSketch.SketchPoints.Count
            If invPlanarSketch.SketchPoints.Item(i).Geometry.X = 0 And invPlanarSketch.SketchPoints.Item(i).Geometry.Y = 0 Then
                oSPoint2d = invPlanarSketch.SketchPoints.Item(i)
            End If
        Next        

        lines(1) = invPlanarSketch.SketchLines.AddByTwoPoints(transientGeometry.CreatePoint2d(dUpperLeftX, dUpperLeftY), _
                                                              transientGeometry.CreatePoint2d(dUpperLeftX + dWidth, dUpperLeftY))
        lines(2) = invPlanarSketch.SketchLines.AddByTwoPoints(lines(1).EndSketchPoint, _
                                                              transientGeometry.CreatePoint2d(dUpperLeftX + dWidth, dUpperLeftY - dHeight))
        lines(3) = invPlanarSketch.SketchLines.AddByTwoPoints(lines(2).EndSketchPoint, _
                                                              transientGeometry.CreatePoint2d(dUpperLeftX, dUpperLeftY - dHeight))
        lines(4) = invPlanarSketch.SketchLines.AddByTwoPoints(lines(3).EndSketchPoint, lines(1).StartSketchPoint)

        invPlanarSketch.GeometricConstraints.AddParallel(lines(1), lines(3))
        invPlanarSketch.GeometricConstraints.AddParallel(lines(2), lines(4))
        invSPointY = invPlanarSketch.SketchPoints.Add(transGeo.CreatePoint2d(0, dHeight / 2), False)
        invSPointX = invPlanarSketch.SketchPoints.Add(transGeo.CreatePoint2d(dWidth / 2, 0), False)
        invPlanarSketch.GeometricConstraints.AddMidpoint(invSPointY, lines(1))
        invPlanarSketch.GeometricConstraints.AddVerticalAlign(invSPointY, oSPoint2d)
        invPlanarSketch.DimensionConstraints.AddTwoPointDistance(lines(1).StartSketchPoint, lines(1).EndSketchPoint, Inventor.DimensionOrientationEnum.kAlignedDim, oPoint2dY)
        invPlanarSketch.GeometricConstraints.AddMidpoint(invSPointX, lines(2))
        invPlanarSketch.GeometricConstraints.AddHorizontalAlign(invSPointX, oSPoint2d)
        invPlanarSketch.DimensionConstraints.AddTwoPointDistance(lines(2).StartSketchPoint, lines(2).EndSketchPoint, Inventor.DimensionOrientationEnum.kAlignedDim, oPoint2dX)
        invPlanarSketch.GeometricConstraints.AddHorizontal(lines(3))
        invPlanarSketch.GeometricConstraints.AddVertical(lines(2))
        createRecTransaction.End()

        Me.Close()

    End Sub
End Class
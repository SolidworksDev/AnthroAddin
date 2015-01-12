Imports Inventor
Imports System.IO
Imports System.Security
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Security.Principal.WindowsIdentity


Public Class DimensionFlat

    Private invApp As Inventor.Application
    
    Private yMin As Double
    Private yMax As Double
    Private xMin As Double
    Private xMax As Double

    Private yMinPoint As Point2d
    Private yMaxPoint As Point2d

    Private yMinCurve As DrawingCurve
    Private yMaxCurve As DrawingCurve

    Private xMinPoint As Point2d
    Private xMaxPoint As Point2d

    Private xMinCurve As DrawingCurve
    Private xMaxCurve As DrawingCurve

    'On Error GoTo Line Next
    Private ErrorCheckingSPY As Boolean
    Private ErrorCheckingEPY As Boolean
    Private ErrorCheckingSPX As Boolean
    Private ErrorCheckingEPX As Boolean

    Public Sub FlatPlusDims(invApp As Inventor.Application)

        ' Set a reference to the drawing document.
        ' This assumes a drawing document is active.

        Dim oPartDoc As PartDocument
        oPartDoc = invApp.ActiveDocument

        Dim oFlatPattern As FlatPattern
        oFlatPattern = oPartDoc.ComponentDefinition.FlatPattern

        If oFlatPattern Is Nothing Then
            Dim oSheetMetalCompDef As SheetMetalComponentDefinition
            oSheetMetalCompDef = oPartDoc.ComponentDefinition
            oSheetMetalCompDef.Unfold()
            oFlatPattern = oPartDoc.ComponentDefinition.Flatpattern
        End If

        Dim oExtents As Box
        oExtents = oFlatPattern.Body.RangeBox

        Dim dLength As Double
        dLength = oExtents.MaxPoint.X - oExtents.MinPoint.X
       
        Dim dScale As Double
        dScale = 18 / dLength

        Dim oDrawDoc As DrawingDocument
        oDrawDoc = invApp.Documents.Add(DocumentTypeEnum.kDrawingDocumentObject, "\\svr19\Design\InventorData\Templates\Standard.idw")

        'Set a reference to the active sheet.
        Dim oSheet As Sheet
        oSheet = oDrawDoc.ActiveSheet

        ' Create a new NameValueMap object
        Dim oBaseViewOptions As NameValueMap
        oBaseViewOptions = invApp.TransientObjects.CreateNameValueMap

        ' Set the options to use when creating the base view.
        oBaseViewOptions.Add("SheetMetalFoldedModel", False)        

        ' Create the placement point object.
        Dim oPoint As Point2d
        oPoint = invApp.TransientGeometry.CreatePoint2d(11.43, 10.795)

        ' Create a base view.
        Dim oBaseView As DrawingView
        oBaseView = oSheet.DrawingViews.AddBaseView(oPartDoc, oPoint, 1, _
        ViewOrientationTypeEnum.kDefaultViewOrientation, DrawingViewStyleEnum.kHiddenLineDrawingViewStyle,  , , oBaseViewOptions)

        Dim oNewView As DrawingView
        Dim TestCurve As DrawingCurve
        Dim TestSeg As DrawingCurveSegment
              
        For Each oNewView In oSheet.DrawingViews

            yMin = oNewView.Height
            yMax = 0

            xMin = oNewView.Width
            xMax = oNewView.Left

            For Each TestCurve In oNewView.DrawingCurves

                For Each TestSeg In TestCurve.Segments

                    If TestCurve.CurveType <> CurveTypeEnum.kCircularArcCurve Then

                        On Error GoTo ErrHandlerSPY
                        ErrorCheckingSPY = IsError(TestSeg.StartPoint)

                        On Error GoTo ErrHandlerEPY
                        ErrorCheckingEPY = IsError(TestSeg.EndPoint)

                        If ErrorCheckingSPY = False Then
                            If Not (TestSeg.StartPoint Is Nothing) Then

                                If TestSeg.StartPoint.Y <= yMin Then
                                    yMin = TestSeg.StartPoint.Y
                                    yMinPoint = TestSeg.StartPoint
                                    yMinCurve = TestSeg.Parent
                                End If

                                If TestSeg.StartPoint.Y >= yMax Then
                                    yMax = TestSeg.StartPoint.Y
                                    yMaxPoint = TestSeg.StartPoint
                                    yMaxCurve = TestSeg.Parent
                                End If

                                If TestSeg.StartPoint.X <= xMin Then
                                    xMin = TestSeg.StartPoint.X
                                    xMinPoint = TestSeg.StartPoint
                                    xMinCurve = TestSeg.Parent
                                End If

                                If TestSeg.StartPoint.X >= xMax Then
                                    xMax = TestSeg.StartPoint.X
                                    xMaxPoint = TestSeg.StartPoint
                                    xMaxCurve = TestSeg.Parent
                                End If

                            End If
                        End If

                        If ErrorCheckingEPY = False Then
                            If Not (TestSeg.EndPoint Is Nothing) Then
                                If TestSeg.EndPoint.Y <= yMin Then
                                    yMin = TestSeg.EndPoint.Y
                                    yMinPoint = TestSeg.EndPoint
                                    yMinCurve = TestSeg.Parent
                                End If

                                If TestSeg.EndPoint.Y >= yMax Then
                                    yMax = TestSeg.EndPoint.Y
                                    yMaxPoint = TestSeg.EndPoint
                                    yMaxCurve = TestSeg.Parent
                                End If

                                If TestSeg.EndPoint.X <= xMin Then
                                    xMin = TestSeg.EndPoint.X
                                    xMinPoint = TestSeg.EndPoint
                                    xMinCurve = TestSeg.Parent
                                End If

                                If TestSeg.EndPoint.X >= xMax Then
                                    xMax = TestSeg.EndPoint.X
                                    xMaxPoint = TestSeg.EndPoint
                                    xMaxCurve = TestSeg.Parent
                                End If

                            End If
                        End If

                    End If

                Next

                TestCurve.LineWeight = 0.0039370078740157
            Next

            Dim intentMinY As GeometryIntent
            intentMinY = oDrawDoc.ActiveSheet.CreateGeometryIntent(yMinCurve, yMinPoint)

            Dim intentMinX As GeometryIntent
            intentMinX = oDrawDoc.ActiveSheet.CreateGeometryIntent(xMinCurve, xMinPoint)

            Dim intentMaxY As GeometryIntent
            intentMaxY = oDrawDoc.ActiveSheet.CreateGeometryIntent(yMaxCurve, yMaxPoint)

            Dim intentMaxX As GeometryIntent
            intentMaxX = oDrawDoc.ActiveSheet.CreateGeometryIntent(xMaxCurve, xMaxPoint)

            Dim XPos As Point2d
            Dim YPos As Point2d

            YPos = oNewView.Position
            XPos = oNewView.Position

            YPos.X = YPos.X + (oNewView.Width / 2) + 4

            XPos.Y = XPos.Y + (oNewView.Height / 2) + 4


            Dim DimStyle As DimensionStyle
            DimStyle = oDrawDoc.StylesManager.DimensionStyles.Item("Flat_Dim_Style")

            Dim oLinDimY As LinearGeneralDimension
            oLinDimY = oDrawDoc.ActiveSheet.DrawingDimensions.GeneralDimensions.AddLinear(YPos, intentMinY, intentMaxY, DimensionTypeEnum.kVerticalDimensionType, True, DimStyle)

            Dim oLinDimX As LinearGeneralDimension
            oLinDimX = oDrawDoc.ActiveSheet.DrawingDimensions.GeneralDimensions.AddLinear(XPos, intentMinX, intentMaxX, DimensionTypeEnum.kHorizontalDimensionType, True, DimStyle)

            oNewView.Scale = dScale

        Next

ErrHandlerSPY:

        ErrorCheckingSPY = True

        Resume Next

ErrHandlerEPY:

        ErrorCheckingEPY = True

        Resume Next

    End Sub

End Class

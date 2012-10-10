Imports Inventor
Imports System.Windows.Forms

Public Class DrawLineArt

    Public invApp As Inventor.Application

    Public Sub DrawViews()

        ' Declare a variable for the Applications Documents
        Dim invDocs As Documents
        invDocs = invApp.Documents

        ' Declare a variable to hold the Assembly Document
        Dim invDoc As Document
        invDoc = invApp.ActiveDocument

        Dim strFullDocumentName As String
        strFullDocumentName = invDoc.FullDocumentName

        Dim invModel As Document
        invModel = invApp.Documents.Open(strFullDocumentName, False)

        ' Declare a variable to hold the New Drawing Document
        Dim invDrawingDoc As DrawingDocument
        invDrawingDoc = invDocs.Add(DocumentTypeEnum.kDrawingDocumentObject)

        ' Declare a variable to hold the Sheet for the new document
        Dim invSheet As Sheet
        invSheet = invDrawingDoc.ActiveSheet
        invSheet.Size = DrawingSheetSizeEnum.kBDrawingSheetSize

        ' Create the placement point object.
        Dim invPoint As Point2d
        invPoint = invApp.TransientGeometry.CreatePoint2d(6, 6)

        Dim invMovePoint As Point2d
        invMovePoint = invApp.TransientGeometry.CreatePoint2d(36, 6)

        ' Create Iso Top Right and Front.
        Dim invBaseView As DrawingView
        Dim invSideView As DrawingView
        Dim invTopView As DrawingView
        Dim invISOView As DrawingView

        invBaseView = invSheet.DrawingViews.AddBaseView(invModel, invPoint, 0.05, ViewOrientationTypeEnum.kCurrentViewOrientation, DrawingViewStyleEnum.kHiddenLineRemovedDrawingViewStyle, , , )
        invBaseView.DisplayTangentEdges = True
        invPoint.X = invPoint.X + 10
        invSideView = invSheet.DrawingViews.AddProjectedView(invBaseView, invPoint, DrawingViewStyleEnum.kFromBaseDrawingViewStyle, 0.05)
        invPoint.X = invPoint.X - 10
        invPoint.Y = invPoint.Y + 10
        invTopView = invSheet.DrawingViews.AddProjectedView(invBaseView, invPoint, DrawingViewStyleEnum.kFromBaseDrawingViewStyle, 0.05)
        invPoint.X = 26
        invPoint.Y = 6
        invBaseView.Position = invMovePoint
        invTopView.Aligned = False
        invTopView.DisplayDefinitionInBase = False
        invTopView.InheritBreak = False
        invTopView.ShowLabel = False
        invTopView.Position = invPoint
        invTopView.Align(invSideView, DrawingViewAlignmentEnum.kHorizontalViewAlignment)
        invPoint.X = invPoint.X + 20
        invPoint.Y = invPoint.Y + 10
        invISOView = invSheet.DrawingViews.AddProjectedView(invBaseView, invPoint, DrawingViewStyleEnum.kFromBaseDrawingViewStyle, 0.05)
        invMovePoint.X = 6
        invMovePoint.Y = 6
        invISOView.Position = invMovePoint
        invMovePoint.X = 26
        invSideView.Position = invMovePoint
        invMovePoint.X = 16
        invTopView.Position = invMovePoint
        invISOView.DisplayTangentEdges = True

    End Sub

    Public Sub New(ByVal ThisApp As Inventor.Application)

        Try
            invApp = ThisApp
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub
End Class

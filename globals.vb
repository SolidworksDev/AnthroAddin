Imports Inventor
Imports System.Text.RegularExpressions
Imports System.Math


Module globals

    Public bDrawSlotRunning As Boolean = False
    Public bLeavingDrawSlotDlg As Boolean = False
    Public bSlotFeatureRunning As Boolean = False
    Public bLeavingSlotFeatureDlg As Boolean = False
    Public Const HOST As String = "svr19"
    Public Const MAX_FILE_SIZE_BYTES As Integer = 49 * 1024 * 1024
    Public iFeatureCount As Integer = 0
    Public iCGCount As Integer = 0
   

    Public Function IsSheetMetal(ByVal strPartName As String) As Boolean

        Dim strFirstThreeChr As String
        strFirstThreeChr = strPartName.Substring(0, 4)
        Dim strFirstTwoChr As String
        strFirstTwoChr = strPartName.Substring(0, 2)

        If strFirstThreeChr = "225-" Or strFirstTwoChr = "M-" Or strFirstThreeChr = "500-" Or strFirstThreeChr = "835-" Or strFirstTwoChr = "W-" Then
            IsSheetMetal = True
        Else
            IsSheetMetal = False
        End If

    End Function

    Public Function IsDoc(ByVal strDocName As String) As Boolean

        If strDocName.Length <= 3 Then
            Return False
        End If

        Dim strFirstThreeChr As String
        strFirstThreeChr = strDocName.Substring(0, 4)
        Dim strFirstTwoChr As String
        strFirstTwoChr = strDocName.Substring(0, 2)

        If strFirstThreeChr = "100-" Then
            IsDoc = True
        ElseIf strFirstThreeChr = "101-" Then
            IsDoc = True
        ElseIf strFirstThreeChr = "125-" Then
            IsDoc = True
        ElseIf strFirstThreeChr = "225-" Then
            IsDoc = True
        ElseIf strFirstThreeChr = "500-" Then
            IsDoc = True
        ElseIf strFirstThreeChr = "835-" Then
            IsDoc = True
        ElseIf strFirstTwoChr = "M-" Then
            IsDoc = True
        ElseIf strFirstTwoChr = "W-" Then
            IsDoc = True
        ElseIf strFirstTwoChr = "T-" Then
            IsDoc = True
        ElseIf strFirstTwoChr = "VT" Then
            IsDoc = True
        Else
            IsDoc = False
        End If

    End Function

    Public Function AllLetters(ByVal txt As String) As Boolean

        Dim reg As New Regex("[A-Za-z]")
        Dim ok As Boolean = True

        If Not (reg.IsMatch(txt)) Then
            ok = False
        Else
            ok = True
        End If

        Return ok

    End Function

    Public Function AllNumbersDashOrLetters(ByVal txt As String) As Boolean

        Dim reg As New Regex("[A-Za-z0-9-]")
        Dim ok As Boolean = True

        If Not (reg.IsMatch(txt)) Then
            ok = False
        Else
            ok = True
        End If

        Return ok

    End Function

    Public Function RoundToFractional(ByVal DecimalNumber As Double, ByVal LargestDenominator As Integer, ByVal DecimalPlaces As Integer) As String

        Dim GCD As Long
        Dim TopNumber As Long
        Dim Remainder As Long
        Dim WholeNumber As Long
        Dim Numerator As Long
        Dim Denominator As Long

        WholeNumber = Fix(DecimalNumber)
        Denominator = LargestDenominator
        Numerator = Format(Denominator * Abs(DecimalNumber - WholeNumber), "0")

        If Numerator Then
            GCD = LargestDenominator
            TopNumber = Numerator

            Do
                Remainder = (GCD Mod TopNumber)
                GCD = TopNumber
                TopNumber = Remainder
            Loop Until Remainder = 0

            Numerator = Numerator \ GCD
            Denominator = Denominator \ GCD

            RoundToFractional = Format(Numerator / Denominator, "0.00")
        Else
            RoundToFractional = CStr(WholeNumber)
        End If

    End Function

    Public Function IsApproved(ByVal invDoc As Document) As Boolean

        ' Get the PropertySets object.
        Dim oPropSets As PropertySets

        ' Get the design tracking property set.
        Dim oPropSet As PropertySet

        ' Get the part number iProperty.
        Dim oPartAuthorityProp As Inventor.Property

        oPropSets = invDoc.PropertySets
        oPropSet = oPropSets.Item("Design Tracking Properties")
        oPartAuthorityProp = oPropSet.Item("Authority")

        If oPartAuthorityProp.Value = "" Then
            Return False
        Else
            Return True
        End If

    End Function

End Module

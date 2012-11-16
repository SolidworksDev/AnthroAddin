
Imports System.Runtime.InteropServices

Namespace AnthroAddIn

    Public NotInheritable Class PictureDispConverter
        <DllImport("OleAut32.dll", EntryPoint:= _
        "OleCreatePictureIndirect", ExactSpelling:=True, _
          PreserveSig:=False)> _
        Private Shared Function OleCreatePictureIndirect( _
      <MarshalAs(UnmanagedType.AsAny)> _
      ByVal picdesc As Object, _
      ByRef iid As Guid, _
      <MarshalAs(UnmanagedType.Bool)> _
      ByVal fOwn As Boolean) _
                         As stdole.IPictureDisp
        End Function

        Shared iPictureDispGuid As Guid = _
            GetType(stdole.IPictureDisp).GUID

        Private NotInheritable Class PICTDESC
            Private Sub New()
            End Sub

            'Picture Types
            Public Const PICTYPE_UNINITIALIZED As Short = -1
            Public Const PICTYPE_NONE As Short = 0
            Public Const PICTYPE_BITMAP As Short = 1
            Public Const PICTYPE_METAFILE As Short = 2
            Public Const PICTYPE_ICON As Short = 3
            Public Const PICTYPE_ENHMETAFILE As Short = 4

            <StructLayout(LayoutKind.Sequential)> _
            Public Class Icon
                Friend cbSizeOfStruct As Integer = _
                  Marshal.SizeOf(GetType(PICTDESC.Icon))
                Friend picType As Integer = _
                    PICTDESC.PICTYPE_ICON
                Friend hicon As IntPtr = IntPtr.Zero
                Friend unused1 As Integer
                Friend unused2 As Integer

                Friend Sub New(ByVal icon As  _
                               System.Drawing.Icon)
                    Me.hicon = icon.ToBitmap().GetHicon()
                End Sub
            End Class

            <StructLayout(LayoutKind.Sequential)> _
            Public Class Bitmap
                Friend cbSizeOfStruct As Integer = _
                  Marshal.SizeOf(GetType(PICTDESC.Bitmap))
                Friend picType As Integer = _
                  PICTDESC.PICTYPE_BITMAP
                Friend hbitmap As IntPtr = IntPtr.Zero
                Friend hpal As IntPtr = IntPtr.Zero
                Friend unused As Integer

                Friend Sub New(ByVal bitmap As  _
                               System.Drawing.Bitmap)
                    Me.hbitmap = bitmap.GetHbitmap()
                End Sub
            End Class
        End Class

        Public Shared Function ToIPictureDisp(ByVal icon As  _
           System.Drawing.Icon) As stdole.IPictureDisp
            Dim pictIcon As New PICTDESC.Icon(icon)
            Return OleCreatePictureIndirect(pictIcon, _
                                 iPictureDispGuid, True)
        End Function

        Public Shared Function ToIPictureDisp(ByVal bmp _
            As System.Drawing.Bitmap) As stdole.IPictureDisp
            Dim pictBmp As New PICTDESC.Bitmap(bmp)
            Return OleCreatePictureIndirect(pictBmp, _
                                  iPictureDispGuid, True)
        End Function
    End Class
End Namespace


Imports System.Runtime.InteropServices

Namespace ActiveDirectory

    ''' <summary>
    ''' The object picker dialog box.
    ''' </summary>
    <ComImport(), Guid("17D6CCD8-3B7B-11D2-B9E0-00C04FD8DBF7")>
    Friend Class DSObjectPicker
    End Class

    ''' <summary>
    ''' The IDsObjectPicker interface is used by an application to initialize and display an object picker dialog box. 
    ''' </summary>
    <ComImport(), Guid("0C87E64E-3B7A-11D2-B9E0-00C04FD8DBF7"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IDsObjectPicker
        <PreserveSig()>
        Function Initialize(ByRef pInitInfo As DSOP_INIT_INFO) As Integer
        <PreserveSig()>
        Function InvokeDialog(HWND As IntPtr, ByRef lpDataObject As IDataObject) As Integer
    End Interface

    ''' <summary>
    ''' Interface to enable data transfers
    ''' </summary>
    <ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010e-0000-0000-C000-000000000046")>
    Friend Interface IDataObject
        <PreserveSig()>
        Function GetData(ByRef pFormatEtc As FORMATETC, ByRef b As STGMEDIUM) As Integer
        Sub GetDataHere(ByRef pFormatEtc As FORMATETC, ByRef b As STGMEDIUM)
        <PreserveSig()>
        Function QueryGetData(a As IntPtr) As Integer
        <PreserveSig()>
        Function GetCanonicalFormatEtc(a As IntPtr, b As IntPtr) As Integer
        <PreserveSig()>
        Function SetData(a As IntPtr, b As IntPtr, c As Integer) As Integer
        <PreserveSig()>
        Function EnumFormatEtc(a As UInteger, b As IntPtr) As Integer
        <PreserveSig()>
        Function DAdvise(a As IntPtr, b As UInteger, c As IntPtr, ByRef d As UInteger) As Integer
        <PreserveSig()>
        Function DUnadvise(a As UInteger) As Integer
        <PreserveSig()>
        Function EnumDAdvise(a As IntPtr) As Integer
    End Interface


End Namespace

Imports System.Runtime.InteropServices

Namespace ActiveDirectory
    Friend Class PInvoke
        ''' <summary>
        ''' The GlobalLock function locks a global memory object and returns a pointer to the first byte of the object's memory block.
        ''' GlobalLock function increments the lock count by one.
        ''' Needed for the clipboard functions when getting the data from IDataObject
        ''' </summary>
        ''' <param name="hMem"></param>
        ''' <returns></returns>
        <DllImport("Kernel32.dll")>
        Public Shared Function GlobalLock(hMem As IntPtr) As IntPtr
        End Function

        ''' <summary>
        ''' The GlobalUnlock function decrements the lock count associated with a memory object.
        ''' </summary>
        ''' <param name="hMem"></param>
        ''' <returns></returns>
        <DllImport("Kernel32.dll")>
        Public Shared Function GlobalUnlock(hMem As IntPtr) As Boolean
        End Function

        <DllImport("Ole32.dll")>
        Public Shared Sub ReleaseStgMedium(ByRef pMedium As STGMEDIUM)
        End Sub
    End Class
End Namespace

Imports System.Runtime.InteropServices

Namespace ActiveDirectory
    ''' <summary>
    ''' This structure is used as a parameter in OLE functions and methods that require data format information.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure FORMATETC
        Public cfFormat As Integer
        Public ptd As IntPtr
        Public dwAspect As UInteger
        Public lindex As Integer
        Public tymed As UInteger
    End Structure

    ''' <summary>
    ''' The STGMEDIUM structure is a generalized global memory handle used for data transfer operations by the IDataObject
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure STGMEDIUM
        Public tymed As UInteger
        Public hGlobal As IntPtr
        Public pUnkForRelease As [Object]
    End Structure

    ''' <summary>
    ''' The DSOP_INIT_INFO structure contains data required to initialize an object picker dialog box. 
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Friend Structure DSOP_INIT_INFO
        Public cbSize As UInteger
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzTargetComputer As String
        Public cDsScopeInfos As UInteger
        Public aDsScopeInfos As IntPtr
        Public flOptions As UInteger
        Public cAttributesToFetch As UInteger
        Public apwzAttributeNames As IntPtr
    End Structure


    ''' <summary>
    ''' The DSOP_SCOPE_INIT_INFO structure describes one or more scope types that have the same attributes. 
    ''' A scope type is a type of location, for example a domain, computer, or Global Catalog, 
    ''' from which the user can select objects.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto), Serializable()> _
    Friend Structure DSOP_SCOPE_INIT_INFO
        Public cbSize As UInteger
        Public flType As UInteger
        Public flScope As UInteger
        <MarshalAs(UnmanagedType.Struct)> _
        Public FilterFlags As DSOP_FILTER_FLAGS
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzDcName As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzADsPath As String
        Public hr As UInteger
    End Structure

    ''' <summary>
    ''' The DSOP_UPLEVEL_FILTER_FLAGS structure contains flags that indicate the filters to use for an up-level scope. 
    ''' An up-level scope is a scope that supports the ADSI LDAP provider.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Friend Structure DSOP_UPLEVEL_FILTER_FLAGS
        Public flBothModes As UInteger
        Public flMixedModeOnly As UInteger
        Public flNativeModeOnly As UInteger
    End Structure

    ''' <summary>
    ''' The DSOP_FILTER_FLAGS structure contains flags that indicate the types of objects presented to the user 
    ''' for a specified scope or scopes.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Friend Structure DSOP_FILTER_FLAGS
        Public Uplevel As DSOP_UPLEVEL_FILTER_FLAGS
        Public flDownlevel As UInteger
    End Structure

    ''' <summary>
    ''' The DS_SELECTION structure contains data about an object the user selected from an object picker dialog box. 
    ''' The DS_SELECTION_LIST structure contains an array of DS_SELECTION structures.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Friend Structure DS_SELECTION
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzName As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzADsPath As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzClass As String
        <MarshalAs(UnmanagedType.LPWStr)> _
        Public pwzUPN As String
        Public pvarFetchedAttributes As IntPtr
        Public flScopeType As UInteger
    End Structure
    ''' <summary>
    ''' The DS_SELECTION_LIST structure contains data about the objects the user selected from an object picker dialog box.
    ''' This structure is supplied by the IDataObject interface supplied by the IDsObjectPicker::InvokeDialog method 
    ''' in the CFSTR_DSOP_DS_SELECTION_LIST data format.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Friend Structure DS_SELECTION_LIST
        Public cItems As UInteger
        Public cFetchedAttributes As UInteger
        Public aDsSelection As DS_SELECTION()
    End Structure

    ''' <summary>
    ''' Flags that indicate the scope types described by this structure. You can combine multiple scope types 
    ''' if all specified scopes use the same settings. 
    ''' </summary>
    Friend Class DSOP_SCOPE_TYPE_FLAGS
        Public Const DSOP_SCOPE_TYPE_TARGET_COMPUTER As UInteger = &H1
        Public Const DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN As UInteger = &H2
        Public Const DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN As UInteger = &H4
        Public Const DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN As UInteger = &H8
        Public Const DSOP_SCOPE_TYPE_GLOBAL_CATALOG As UInteger = &H10
        Public Const DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN As UInteger = &H20
        Public Const DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN As UInteger = &H40
        Public Const DSOP_SCOPE_TYPE_WORKGROUP As UInteger = &H80
        Public Const DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE As UInteger = &H100
        Public Const DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE As UInteger = &H200
    End Class

    ''' <summary>
    ''' Flags that determine the object picker options.
    ''' </summary>
    Friend Class DSOP_INIT_INFO_FLAGS
        Public Const DSOP_FLAG_MULTISELECT As UInteger = &H1
        Public Const DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK As UInteger = &H2
    End Class

    ''' <summary>
    ''' Flags that indicate the format used to return ADsPath for objects selected from this scope. 
    ''' The flScope member can also indicate the initial scope displayed in the Look in drop-down list. 
    ''' </summary>
    Friend Class DSOP_SCOPE_INIT_INFO_FLAGS
        Public Const DSOP_SCOPE_FLAG_STARTING_SCOPE As UInteger = &H1
        Public Const DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT As UInteger = &H2
        Public Const DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP As UInteger = &H4
        Public Const DSOP_SCOPE_FLAG_WANT_PROVIDER_GC As UInteger = &H8
        Public Const DSOP_SCOPE_FLAG_WANT_SID_PATH As UInteger = &H10
        Public Const DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH As UInteger = &H20
        Public Const DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS As UInteger = &H40
        Public Const DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS As UInteger = &H80
        Public Const DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS As UInteger = &H100
        Public Const DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS As UInteger = &H200
    End Class

    ''' <summary>
    ''' Filter flags to use for an up-level scope, regardless of whether it is a mixed or native mode domain. 
    ''' </summary>
    Friend Class DSOP_FILTER_FLAGS_FLAGS
        Public Const DSOP_FILTER_INCLUDE_ADVANCED_VIEW As UInteger = &H1
        Public Const DSOP_FILTER_USERS As UInteger = &H2
        Public Const DSOP_FILTER_BUILTIN_GROUPS As UInteger = &H4
        Public Const DSOP_FILTER_WELL_KNOWN_PRINCIPALS As UInteger = &H8
        Public Const DSOP_FILTER_UNIVERSAL_GROUPS_DL As UInteger = &H10
        Public Const DSOP_FILTER_UNIVERSAL_GROUPS_SE As UInteger = &H20
        Public Const DSOP_FILTER_GLOBAL_GROUPS_DL As UInteger = &H40
        Public Const DSOP_FILTER_GLOBAL_GROUPS_SE As UInteger = &H80
        Public Const DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL As UInteger = &H100
        Public Const DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE As UInteger = &H200
        Public Const DSOP_FILTER_CONTACTS As UInteger = &H400
        Public Const DSOP_FILTER_COMPUTERS As UInteger = &H800
    End Class

    ''' <summary>
    ''' Contains the filter flags to use for down-level scopes
    ''' </summary>
    Friend Class DSOP_DOWNLEVEL_FLAGS
        Public Const DSOP_DOWNLEVEL_FILTER_USERS As UInteger = &H80000001UI
        Public Const DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS As UInteger = &H80000002UI
        Public Const DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS As UInteger = &H80000004UI
        Public Const DSOP_DOWNLEVEL_FILTER_COMPUTERS As UInteger = &H80000008UI
        Public Const DSOP_DOWNLEVEL_FILTER_WORLD As UInteger = &H80000010UI
        Public Const DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER As UInteger = &H80000020UI
        Public Const DSOP_DOWNLEVEL_FILTER_ANONYMOUS As UInteger = &H80000040UI
        Public Const DSOP_DOWNLEVEL_FILTER_BATCH As UInteger = &H80000080UI
        Public Const DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER As UInteger = &H80000100UI
        Public Const DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP As UInteger = &H80000200UI
        Public Const DSOP_DOWNLEVEL_FILTER_DIALUP As UInteger = &H80000400UI
        Public Const DSOP_DOWNLEVEL_FILTER_INTERACTIVE As UInteger = &H80000800UI
        Public Const DSOP_DOWNLEVEL_FILTER_NETWORK As UInteger = &H80001000UI
        Public Const DSOP_DOWNLEVEL_FILTER_SERVICE As UInteger = &H80002000UI
        Public Const DSOP_DOWNLEVEL_FILTER_SYSTEM As UInteger = &H80004000UI
        Public Const DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS As UInteger = &H80008000UI
        Public Const DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER As UInteger = &H80010000UI
        Public Const DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS As UInteger = &H80020000UI
        Public Const DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE As UInteger = &H80040000UI
        Public Const DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE As UInteger = &H80080000UI
        Public Const DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON As UInteger = &H80100000UI
    End Class

    ''' <summary>
    ''' The IDsObjectPicker.InvokeDialog result
    ''' </summary>
    Friend Class HRESULT
        Public Const S_OK As Integer = 0 ' The method succeeded. 
        Public Const S_FALSE As Integer = 1 ' The user cancelled the dialog box. ppdoSelections receives NULL. 
        Public Const E_NOTIMPL As Integer = -2147467263 ' ?
    End Class

    ''' <summary>
    ''' The CFSTR_DSOP_DS_SELECTION_LIST clipboard format is provided by the IDataObject obtained by calling IDsObjectPicker.InvokeDialog
    ''' </summary>
    Friend Class CLIPBOARD_FORMAT
        Public Const CFSTR_DSOP_DS_SELECTION_LIST As String = "CFSTR_DSOP_DS_SELECTION_LIST"
    End Class

    ''' <summary>
    ''' The TYMED enumeration values indicate the type of storage medium being used in a data transfer. 
    ''' </summary>
    Friend Enum TYMED
        TYMED_HGLOBAL = 1
        TYMED_FILE = 2
        TYMED_ISTREAM = 4
        TYMED_ISTORAGE = 8
        TYMED_GDI = 16
        TYMED_MFPICT = 32
        TYMED_ENHMF = 64
        TYMED_NULL = 0
    End Enum

    ''' <summary>
    ''' The DVASPECT enumeration values specify the desired data or view aspect of the object when drawing or getting data.
    ''' </summary>
    Friend Enum DVASPECT
        DVASPECT_CONTENT = 1
        DVASPECT_THUMBNAIL = 2
        DVASPECT_ICON = 4
        DVASPECT_DOCPRINT = 8
    End Enum

End Namespace

' Copyright 2004 Armand du Plessis <armand@dotnet.org.za>
' http://dotnet.org.za/armand/articles/2453.aspx
' Thanks to Joe Cataford and Friedrich Brunzema.
' Also see MSDN: http://msdn2.microsoft.com/en-us/library/ms675899.aspx
' Enhancements by sgryphon@computer.org (PACOM):
' - Integrated with the CommonDialog API, e.g. returns a DialogResult, changed namespace, etc.
' - Marked COM interop code as internal; only the main dialog (and related classes) are public.
' - Added basic scope (location) and filter (object type) control, plus multi-select flag.

Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace ActiveDirectory
    ''' <summary>
    ''' Represents a common dialog that allows a user to select directory objects.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' The directory object picker dialog box enables a user to select one or more objects 
    ''' from either the global catalog, a Microsoft Windows 2000 domain or computer, 
    ''' a Microsoft Windows NT 4.0 domain or computer, or a workgroup. The object types 
    ''' from which a user can select include user, contact, group, and computer objects.
    ''' </para>
    ''' <para>
    ''' This managed class wraps the Directory Object Picker common dialog from 
    ''' the Active Directory UI.
    ''' </para>
    ''' <para>
    ''' It simplifies the scope (Locations) and filter (ObjectTypes) selection by allowing a single filter to be
    ''' specified which applies to all scopes (translating to both up-level and down-level
    ''' filter flags as necessary).
    ''' </para>
    ''' <para>
    ''' The object type filter is also simplified by combining different types of groups (local, global, etc)
    ''' and not using individual well known types in down-level scopes (only all well known types
    ''' can be specified).
    ''' </para>
    ''' <para>
    ''' The scope location is also simplified by combining down-level and up-level variations
    ''' into a single locations flag, e.g. external domains.
    ''' </para>
    ''' </remarks>
    Public Class DirectoryObjectPickerDialog
        Inherits CommonDialog
        Private m_selectedObjects As DirectoryObject()

        ''' <summary>
        ''' Constructor. Sets all properties to their default values.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' The default values for the DirectoryObjectPickerDialog properties are:
        ''' </para>
        ''' <para>
        ''' <list type="table">
        ''' <listheader><term>Property</term><description>Default value</description></listheader>
        ''' <item><term>AllowedLocations</term><description>All locations.</description></item>
        ''' <item><term>AllowedObjectTypes</term><description>All object types.</description></item>
        ''' <item><term>DefaultLocations</term><description>None. (Will default to first location.)</description></item>
        ''' <item><term>DefaultObjectTypes</term><description>All object types.</description></item>
        ''' <item><term>MultiSelect</term><description>false.</description></item>
        ''' <item><term>SelectedObject</term><description>null.</description></item>
        ''' <item><term>SelectedObjects</term><description>Empty array.</description></item>
        ''' <item><term>ShowAdvancedView</term><description>false.</description></item>
        ''' <item><term>TargetComputer</term><description>null.</description></item>
        ''' </list>
        ''' </para>
        ''' </remarks>
        Public Sub New()
            Reset()
        End Sub

        ''' <summary>
        ''' Gets or sets the scopes the DirectoryObjectPickerDialog is allowed to search.
        ''' </summary>
        Public Property AllowedLocations() As Locations

        ''' <summary>
        ''' Gets or sets the types of objects the DirectoryObjectPickerDialog is allowed to search for.
        ''' </summary>
        Public Property AllowedObjectTypes() As ObjectTypes

        ''' <summary>
        ''' Gets or sets the initially selected scope in the DirectoryObjectPickerDialog.
        ''' </summary>
        Public Property DefaultLocations() As Locations

        ''' <summary>
        ''' Gets or sets the initially seleted types of objects in the DirectoryObjectPickerDialog.
        ''' </summary>
        Public Property DefaultObjectTypes() As ObjectTypes

        ''' <summary>
        ''' Gets or sets whether the user can select multiple objects.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' If this flag is false, the user can select only one object.
        ''' </para>
        ''' </remarks>
        Public Property MultiSelect() As Boolean

        ''' <summary>
        ''' Gets the directory object selected in the dialog, or null if no object was selected.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' If MultiSelect is enabled, then this property returns only the first selected object.
        ''' Use SelectedObjects to get an array of all objects selected.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property SelectedObject() As DirectoryObject
            Get
                If m_selectedObjects Is Nothing Or m_selectedObjects.Length = 0 Then
                    Return Nothing
                End If
                Return m_selectedObjects(0)
            End Get
        End Property

        ''' <summary>
        ''' Gets an array of the directory objects selected in the dialog.
        ''' </summary>
        Public ReadOnly Property SelectedObjects() As DirectoryObject()
            Get
                If m_selectedObjects Is Nothing Then
                    Return New DirectoryObject(-1) {}
                End If
                Return DirectCast(m_selectedObjects.Clone(), DirectoryObject())
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether objects flagged as show in advanced view only are displayed (up-level).
        ''' </summary>
        Public Property ShowAdvancedView() As Boolean

        ''' <summary>
        ''' Gets or sets the name of the target computer. 
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' The dialog box operates as if it is running on the target computer, using the target computer 
        ''' to determine the joined domain and enterprise. If this value is null or empty, the target computer 
        ''' is the local computer.
        ''' </para>
        ''' </remarks>
        Public Property TargetComputer() As String

        ''' <summary>
        ''' Resets all properties to their default values. 
        ''' </summary>
        Public Overrides Sub Reset()
            AllowedLocations = Locations.All
            AllowedObjectTypes = ObjectTypes.All
            DefaultLocations = Locations.None
            DefaultObjectTypes = ObjectTypes.All
            MultiSelect = False
            m_selectedObjects = Nothing
            ShowAdvancedView = False
            TargetComputer = Nothing
        End Sub

        ''' <summary>
        ''' Displays the Directory Object Picker (Active Directory) common dialog, when called by ShowDialog.
        ''' </summary>
        ''' <param name="hwndOwner">Handle to the window that owns the dialog.</param>
        ''' <returns>If the user clicks the OK button of the Directory Object Picker dialog that is displayed, true is returned; 
        ''' otherwise, false.</returns>
        Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
            Dim dataObj As IDataObject = Nothing
            Dim ipicker As IDsObjectPicker = Initialize()
            If ipicker Is Nothing Then
                m_selectedObjects = Nothing
                Return False
            End If
            Dim hresult__1 As Integer = ipicker.InvokeDialog(hwndOwner, dataObj)
            m_selectedObjects = ProcessSelections(dataObj)
            Return (hresult__1 = HRESULT.S_OK)
        End Function

#Region "Private implementation"

        ' Convert ObjectTypes to DSCOPE_SCOPE_INIT_INFO_FLAGS
        Private Function GetDefaultFilter() As UInteger
            Dim defaultFilter As UInteger = 0
            If ((DefaultObjectTypes And ObjectTypes.Users) = ObjectTypes.Users) OrElse ((DefaultObjectTypes And ObjectTypes.WellKnownPrincipals) = ObjectTypes.WellKnownPrincipals) Then
                defaultFilter = defaultFilter Or DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS
            End If
            If ((DefaultObjectTypes And ObjectTypes.Groups) = ObjectTypes.Groups) OrElse ((DefaultObjectTypes And ObjectTypes.BuiltInGroups) = ObjectTypes.BuiltInGroups) Then
                defaultFilter = defaultFilter Or DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS
            End If
            If (DefaultObjectTypes And ObjectTypes.Computers) = ObjectTypes.Computers Then
                defaultFilter = defaultFilter Or DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS
            End If
            If (DefaultObjectTypes And ObjectTypes.Contacts) = ObjectTypes.Contacts Then
                defaultFilter = defaultFilter Or DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS
            End If
            Return defaultFilter
        End Function

        ' Convert ObjectTypes to DSOP_DOWNLEVEL_FLAGS
        Private Function GetDownLevelFilter() As UInteger
            Dim downlevelFilter As UInteger = 0
            If (AllowedObjectTypes And ObjectTypes.Users) = ObjectTypes.Users Then
                downlevelFilter = downlevelFilter Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS
            End If
            If (AllowedObjectTypes And ObjectTypes.Groups) = ObjectTypes.Groups Then
                downlevelFilter = downlevelFilter Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS
            End If
            If (AllowedObjectTypes And ObjectTypes.Computers) = ObjectTypes.Computers Then
                downlevelFilter = downlevelFilter Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_COMPUTERS
            End If
            ' Contacts not available in downlevel scopes
            'if ((allowedTypes & ObjectTypes.Contacts) == ObjectTypes.Contacts)
            ' Exclude build in groups if not selected
            If (AllowedObjectTypes And ObjectTypes.BuiltInGroups) = 0 Then
                downlevelFilter = downlevelFilter Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS
            End If
            If (AllowedObjectTypes And ObjectTypes.WellKnownPrincipals) = ObjectTypes.WellKnownPrincipals Then
                ' This includes all the following:
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_WORLD |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ANONYMOUS |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_BATCH |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_DIALUP |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_INTERACTIVE |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SERVICE |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SYSTEM |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE |
                'DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON;
                downlevelFilter = downlevelFilter Or DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS
            End If
            Return downlevelFilter
        End Function

        ' Convert Locations to DSOP_SCOPE_TYPE_FLAGS
        Private Shared Function GetScope(locations__1 As Locations) As UInteger
            Dim scope As UInteger = 0
            If (locations__1 And Locations.LocalComputer) = Locations.LocalComputer Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER
            End If
            If (locations__1 And Locations.JoinedDomain) = Locations.JoinedDomain Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN
            End If
            If (locations__1 And Locations.EnterpriseDomain) = Locations.EnterpriseDomain Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN
            End If
            If (locations__1 And Locations.GlobalCatalog) = Locations.GlobalCatalog Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG
            End If
            If (locations__1 And Locations.ExternalDomain) = Locations.ExternalDomain Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN
            End If
            If (locations__1 And Locations.Workgroup) = Locations.Workgroup Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_WORKGROUP
            End If
            If (locations__1 And Locations.UserEntered) = Locations.UserEntered Then
                scope = scope Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE Or DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE
            End If
            Return scope
        End Function

        ' Convert scope for allowed locations other than the default
        Private Function GetOtherScope() As UInteger
            Dim otherLocations As Locations = AllowedLocations And (Not DefaultLocations)
            Return GetScope(otherLocations)
        End Function

        ' Convert scope for default locations
        Private Function GetStartingScope() As UInteger
            Return GetScope(DefaultLocations)
        End Function

        ' Convert ObjectTypes to DSOP_FILTER_FLAGS_FLAGS
        Private Function GetUpLevelFilter() As UInteger
            Dim uplevelFilter As UInteger = 0
            If (AllowedObjectTypes And ObjectTypes.Users) = ObjectTypes.Users Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS
            End If
            If (AllowedObjectTypes And ObjectTypes.Groups) = ObjectTypes.Groups Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE
            End If
            If (AllowedObjectTypes And ObjectTypes.Computers) = ObjectTypes.Computers Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_COMPUTERS
            End If
            If (AllowedObjectTypes And ObjectTypes.Contacts) = ObjectTypes.Contacts Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_CONTACTS
            End If
            If (AllowedObjectTypes And ObjectTypes.BuiltInGroups) = ObjectTypes.BuiltInGroups Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS
            End If
            If (AllowedObjectTypes And ObjectTypes.WellKnownPrincipals) = ObjectTypes.WellKnownPrincipals Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_WELL_KNOWN_PRINCIPALS
            End If
            If ShowAdvancedView Then
                uplevelFilter = uplevelFilter Or DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW
            End If
            Return uplevelFilter
        End Function

        Private Function Initialize() As IDsObjectPicker
            Dim picker As New DSObjectPicker()
            Dim ipicker As IDsObjectPicker = DirectCast(picker, IDsObjectPicker)

            Dim scopeInitInfoList As New List(Of DSOP_SCOPE_INIT_INFO)()

            ' Note the same default and filters are used by all scopes
            Dim defaultFilter As UInteger = GetDefaultFilter()
            Dim upLevelFilter As UInteger = GetUpLevelFilter()
            Dim downLevelFilter As UInteger = GetDownLevelFilter()
            ' Internall, use one scope for the default (starting) locations.
            Dim startingScope As UInteger = GetStartingScope()
            If startingScope > 0 Then
                Dim startingScopeInfo As New DSOP_SCOPE_INIT_INFO()
                startingScopeInfo.cbSize = CUInt(Marshal.SizeOf(GetType(DSOP_SCOPE_INIT_INFO)))
                startingScopeInfo.flType = startingScope
                startingScopeInfo.flScope = DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE Or defaultFilter
                startingScopeInfo.FilterFlags.Uplevel.flBothModes = upLevelFilter
                startingScopeInfo.FilterFlags.flDownlevel = downLevelFilter
                startingScopeInfo.pwzADsPath = Nothing
                startingScopeInfo.pwzDcName = Nothing
                startingScopeInfo.hr = 0
                scopeInitInfoList.Add(startingScopeInfo)
            End If

            ' And another scope for all other locations (AllowedLocation values not in DefaultLocation)
            Dim otherScope As UInteger = GetOtherScope()
            If otherScope > 0 Then
                Dim otherScopeInfo As New DSOP_SCOPE_INIT_INFO()
                otherScopeInfo.cbSize = CUInt(Marshal.SizeOf(GetType(DSOP_SCOPE_INIT_INFO)))
                otherScopeInfo.flType = otherScope
                otherScopeInfo.flScope = defaultFilter
                otherScopeInfo.FilterFlags.Uplevel.flBothModes = upLevelFilter
                otherScopeInfo.FilterFlags.flDownlevel = downLevelFilter
                otherScopeInfo.pwzADsPath = Nothing
                otherScopeInfo.pwzDcName = Nothing
                otherScopeInfo.hr = 0
                scopeInitInfoList.Add(otherScopeInfo)
            End If

            Dim scopeInitInfo As DSOP_SCOPE_INIT_INFO() = scopeInitInfoList.ToArray()

            ' TODO: Scopes for alternate ADs, alternate domains, alternate computers, etc

            ' Allocate memory from the unmananged mem of the process, this should be freed later!??
            Dim refScopeInitInfo As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(DSOP_SCOPE_INIT_INFO)) * scopeInitInfo.Length)

            ' Marshal structs to pointers
            For index As Integer = 0 To scopeInitInfo.Length - 1
                'Marshal.StructureToPtr(scopeInitInfo[0],
                '    refScopeInitInfo, true);

                'Marshal.StructureToPtr(scopeInitInfo(index), CType(CInt(refScopeInitInfo) + index * Marshal.SizeOf(GetType(DSOP_SCOPE_INIT_INFO)), IntPtr), True)
                Marshal.StructureToPtr(scopeInitInfo(index), CType(CLng(refScopeInitInfo) + index * Marshal.SizeOf(GetType(DSOP_SCOPE_INIT_INFO)), IntPtr), False)
            Next

            ' Initialize structure with data to initialize an object picker dialog box. 
            Dim initInfo As New DSOP_INIT_INFO()
            initInfo.cbSize = CUInt(Marshal.SizeOf(initInfo))
            'initInfo.pwzTargetComputer = null; // local computer
            initInfo.pwzTargetComputer = TargetComputer
            initInfo.cDsScopeInfos = CUInt(scopeInitInfo.Length)
            initInfo.aDsScopeInfos = refScopeInitInfo
            ' Flags that determine the object picker options. 
            Dim flOptions As UInteger = 0
            ' Only set DSOP_INIT_INFO_FLAGS.DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK
            ' if we know target is not a DC (which then saves initialization time).
            If MultiSelect Then
                flOptions = flOptions Or DSOP_INIT_INFO_FLAGS.DSOP_FLAG_MULTISELECT
            End If
            initInfo.flOptions = flOptions

            ' We're not retrieving any additional attributes
            'string[] attributes = new string[] { "sAMaccountName" };
            'initInfo.cAttributesToFetch = (uint)attributes.Length; 
            'initInfo.apwzAttributeNames = Marshal.StringToHGlobalUni( attributes[0] );
            initInfo.cAttributesToFetch = 0
            initInfo.apwzAttributeNames = IntPtr.Zero

            ' Initialize the Object Picker Dialog Box with our options
            Dim hresult__1 As Integer = ipicker.Initialize(initInfo)

            If hresult__1 <> HRESULT.S_OK Then
                Return Nothing
            End If
            Return ipicker
        End Function

        Private Shared Function ProcessSelections(dataObj As IDataObject) As DirectoryObject()
            If dataObj Is Nothing Then
                Return Nothing
            End If

            Dim selections As DirectoryObject() = Nothing

            ' The STGMEDIUM structure is a generalized global memory handle used for data transfer operations
            Dim stg As New STGMEDIUM()
            stg.tymed = CUInt(TYMED.TYMED_HGLOBAL)
            stg.hGlobal = IntPtr.Zero
            stg.pUnkForRelease = Nothing

            ' The FORMATETC structure is a generalized Clipboard format.
            Dim fe As New FORMATETC()
            fe.cfFormat = System.Windows.Forms.DataFormats.GetFormat(CLIPBOARD_FORMAT.CFSTR_DSOP_DS_SELECTION_LIST).Id
            ' The CFSTR_DSOP_DS_SELECTION_LIST clipboard format is provided by the IDataObject obtained 
            ' by calling IDsObjectPicker::InvokeDialog
            fe.ptd = IntPtr.Zero
            fe.dwAspect = 1
            'DVASPECT_CONTENT    = 1,  
            fe.lindex = -1
            ' all of the data
            fe.tymed = CUInt(TYMED.TYMED_HGLOBAL)
            'The storage medium is a global memory handle (HGLOBAL)
            dataObj.GetData(fe, stg)

            Dim pDsSL As IntPtr = PInvoke.GlobalLock(stg.hGlobal)

            Try
                ' the start of our structure
                Dim current As IntPtr = pDsSL
                ' get the # of items selected
                Dim cnt As Integer = Marshal.ReadInt32(current)

                ' if we selected at least 1 object
                If cnt > 0 Then
                    selections = New DirectoryObject(cnt - 1) {}
                    ' increment the pointer so we can read the DS_SELECTION structure
                    current = CType(CLng(current) + (Marshal.SizeOf(GetType(UInteger)) * 2), IntPtr)
                    ' now loop through the structures
                    For i As Integer = 0 To cnt - 1
                        ' marshal the pointer to the structure
                        Dim s As DS_SELECTION = DirectCast(Marshal.PtrToStructure(current, GetType(DS_SELECTION)), DS_SELECTION)
                        'Marshal.DestroyStructure(current, GetType(DS_SELECTION))

                        ' increment the position of our pointer by the size of the structure
                        current = CType(CLng(current) + Marshal.SizeOf(GetType(DS_SELECTION)), IntPtr)

                        Dim name As String = s.pwzName
                        Dim path As String = s.pwzADsPath
                        Dim schemaClassName As String = s.pwzClass
                        Dim upn As String = s.pwzUPN
                        'string temp = Marshal.PtrToStringUni( s.pvarFetchedAttributes );
                        selections(i) = New DirectoryObject(name, path, schemaClassName, upn)
                    Next
                End If
            Finally
                PInvoke.GlobalUnlock(pDsSL)
                PInvoke.ReleaseStgMedium(stg)
                'Marshal.FreeHGlobal(stg.hGlobal)
            End Try
            Return selections
        End Function

#End Region
    End Class
End Namespace

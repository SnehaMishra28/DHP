Namespace ActiveDirectory
    ''' <summary>
    ''' Indicates the type of objects the DirectoryObjectPickerDialog searches for.
    ''' </summary>
    <Flags()>
    Public Enum ObjectTypes
        ''' <summary>
        ''' No object types.
        ''' </summary>
        None = &H0

        ''' <summary>
        ''' Includes user objects.
        ''' </summary>
        Users = &H1

        ''' <summary>
        ''' Includes security groups with universal scope. 
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' In an up-level scope, this includes distribution and security groups, with universal, global and domain local scope.
        ''' </para>
        ''' <para>
        ''' In a down-level scope, this includes local and global groups.
        ''' </para>
        ''' </remarks>
        Groups = &H2

        ''' <summary>
        ''' Includes computer objects.
        ''' </summary>
        Computers = &H4

        ''' <summary>
        ''' Includes contact objects.
        ''' </summary>
        Contacts = &H8

        ''' <summary>
        ''' Includes built-in group objects.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' In an up-level scope, this includes group objects with the built-in groupType flags.
        ''' </para>
        ''' <para>
        ''' In a down-level scope, not setting this object type excludes local built-in groups.
        ''' </para>
        ''' </remarks>
        BuiltInGroups = &H10

        ''' <summary>
        ''' Includes all well-known security principals. 
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' In an up-level scope, this includes the contents of the Well Known Security Principals container.
        ''' </para>
        ''' <para>
        ''' In a down-level scope, this includes all well-known SIDs.
        ''' </para>
        ''' </remarks>
        WellKnownPrincipals = &H20

        ''' <summary>
        ''' All object types.
        ''' </summary>
        All = &H3F
    End Enum
End Namespace

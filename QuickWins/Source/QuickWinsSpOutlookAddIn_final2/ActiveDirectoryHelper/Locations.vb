Namespace ActiveDirectory
    ''' <summary>
    ''' Indicates the scope the DirectoryObjectPickerDialog searches for objects.
    ''' </summary>
    <Flags()>
    Public Enum Locations
        ''' <summary>
        ''' No locations.
        ''' </summary>
        None = &H0

        ''' <summary>
        ''' The target computer (down-level).
        ''' </summary>
        LocalComputer = &H1

        ''' <summary>
        ''' A domain to which the target computer is joined (down-level and up-level).
        ''' </summary>
        JoinedDomain = &H2

        ''' <summary>
        ''' All Windows 2000 domains in the enterprise to which the target computer belongs (up-level).
        ''' </summary>
        EnterpriseDomain = &H4

        ''' <summary>
        ''' A scope containing objects from all domains in the enterprise (up-level). 
        ''' </summary>
        GlobalCatalog = &H8

        ''' <summary>
        ''' All domains external to the enterprise, but trusted by the domain to which the target computer 
        ''' is joined (down-level and up-level).
        ''' </summary>
        ExternalDomain = &H10

        ''' <summary>
        ''' The workgroup to which the target computer is joined (down-level). 
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' Applies only if the target computer is not 
        ''' joined to a domain. The only type of object that can be selected from a workgroup is a computer.
        ''' </para>
        ''' </remarks>
        Workgroup = &H20

        ''' <summary>
        ''' Enables the user to enter a scope (down-level and up-level). 
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' If not specified, the dialog box restricts the user to the scopes in the locations drop-down list.
        ''' </para>
        ''' </remarks>
        UserEntered = &H40

        ''' <summary>
        ''' All locations.
        ''' </summary>
        All = &H7F
    End Enum
End Namespace

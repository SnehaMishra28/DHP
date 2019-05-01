Namespace ActiveDirectory

    ''' <summary>
    ''' Details of a directory object selected in the DirectoryObjectPickerDialog.
    ''' </summary>
    Public Class DirectoryObject
        Private ReadOnly adsPath As String
        Private ReadOnly className As String
        Private ReadOnly m_name As String
        Private ReadOnly m_upn As String

        Public Sub New(name As String, path As String, schemaClass As String, upn As String)
            Me.m_name = name
            Me.adsPath = path
            Me.className = schemaClass
            Me.m_upn = upn
        End Sub

        ''' <summary>
        ''' Gets the Active Directory path for this directory object.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' The format of this string depends on the options specified in the DirectoryObjectPickerDialog
        ''' from which this object was selected.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property Path() As String
            Get
                Return adsPath
            End Get
        End Property

        ''' <summary>
        ''' Gets the name of the schema class for this directory object (objectClass attribute).
        ''' </summary>
        Public ReadOnly Property SchemaClassName() As String
            Get
                Return className
            End Get
        End Property

        ''' <summary>
        ''' Gets the directory object's relative distinguished name (RDN).
        ''' </summary>
        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property

        ''' <summary>
        ''' Gets the objects user principal name (userPrincipalName attribute).
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' If the object does not have a userPrincipalName value, this property is an empty string. 
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property Upn() As String
            Get
                Return m_upn
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function
    End Class
End Namespace

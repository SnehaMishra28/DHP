Namespace ActiveDirectory
    ''' <summary>
    ''' Active Directory name translation.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Translates names between Active Directory formats, e.g. from down-level NT4 
    ''' style names ("ACME\alice") to User Principal Name ("alice@acme.com").
    ''' </para>
    ''' <para>
    ''' This utility class encapsulates the ActiveDs.dll COM library.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class NameTranslator
        Private Sub New()
        End Sub
        Const NameTypeUpn As Integer = CInt(ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_USER_PRINCIPAL_NAME)
        Const NameTypeNt4 As Integer = CInt(ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_NT4)
        'Const NameTypeDn As Integer = CInt(ActiveDs.ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_1779)

        ''' <summary>
        ''' Convert from a down-level NT4 style name to an Active Directory User Principal Name (UPN).
        ''' </summary>
        Public Shared Function TranslateDownLevelToUpn(downLevelNt4Name As String) As String
            Dim userPrincipalName As String
            Dim nt As New ActiveDs.NameTranslate()
            nt.[Set](NameTypeNt4, downLevelNt4Name)
            userPrincipalName = nt.[Get](NameTypeUpn)
            Return userPrincipalName
        End Function

        ''' <summary>
        ''' Convert from an Active Directory User Principal Name (UPN) to a down-level NT4 style name.
        ''' </summary>
        Public Shared Function TranslateUpnToDownLevel(userPrincipalName As String) As String
            Dim downLevelName As String
            Dim nt As New ActiveDs.NameTranslate()
            nt.[Set](NameTypeUpn, userPrincipalName)
            downLevelName = nt.[Get](NameTypeNt4)
            Return downLevelName
        End Function
    End Class
End Namespace

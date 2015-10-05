Imports System.ComponentModel.DataAnnotations
Public Class ValidEventMetadata
    <Required()> _
    Public Property text() As String
        Get
            Return m_text
        End Get
        Set(ByVal value As String)
            m_text = Value
        End Set
    End Property
    Private m_text As String
    <Required()> _
    <RegularExpression("^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" & "((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." & "([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" & vbCr & vbLf & vbTab & vbTab & vbTab & vbTab & "[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" & "([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$", ErrorMessage:="Invalid email")> _
    Public Property email() As String
        Get
            Return m_email
        End Get
        Set(ByVal value As String)
            m_email = value
        End Set
    End Property
    Private m_email As String

End Class

<MetadataType(GetType(ValidEventMetadata))> _
Partial Class ValidEvent

End Class

Partial Class [Event]

End Class


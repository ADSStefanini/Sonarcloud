Public Partial Class Site
    Inherits System.Web.UI.MasterPage

    Private m_Help_err As String
    Private m_Help_info As String

    Public Property xHelp_err() As String
        Get
            Return m_Help_err
        End Get
        Set(ByVal value As String)
            m_Help_err = value
            Help_err.InnerHtml = m_Help_err
        End Set
    End Property

    Public Property xHelp_info() As String
        Get
            Return m_Help_info
        End Get
        Set(ByVal value As String)
            m_Help_info = value
            Help_info.InnerHtml = m_Help_info
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
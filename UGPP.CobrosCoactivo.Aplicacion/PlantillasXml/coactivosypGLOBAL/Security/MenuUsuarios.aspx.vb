Public Partial Class MenuOpcionDemo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then

        '    'Gestión de usuarios
        '    Dim a_Usuario As String = ""
        '    a_Usuario = CreaScriptClick("a_Usuario", 0, 1, 0)
        '    a_Usuario = vbNewLine & a_Usuario & CreaScriptOver("a_Usuario", 0, 1, 0)

        '    'Cambiar Clave
        '    Dim a_caClave As String = ""
        '    a_caClave = CreaScriptClick("a_caClave", 0, 1, 1)
        '    a_caClave = vbNewLine & a_caClave & CreaScriptOver("a_caClave", 0, 1, 1)


        '    Ejecutarjavascript("<script type=""text/javascript""> " & vbNewLine & "$(document).ready(function() { " & vbNewLine & "var permisos; var descrip = new Array(3); var IsPostBack = new Boolean() " & vbNewLine & "if (IsPostBack == false){ permisos = PermisosUsuarios('USUARIOS'); IsPostBack=true; }" _
        '    & vbNewLine & a_Usuario & vbNewLine & a_caClave & vbNewLine & "  }); </script>", "ScriptIsPostBack")
        'End If
    End Sub

    Private Sub Ejecutarjavascript(ByVal script As String, ByVal NomScript As String)
        Dim csname1 As [String] = NomScript
        Dim cstype As Type = Me.[GetType]()

        Dim cs As ClientScriptManager = Page.ClientScript

        If Not cs.IsStartupScriptRegistered(cstype, csname1) Then
            Dim cstext1 As String = script
            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString())
        End If
    End Sub

End Class
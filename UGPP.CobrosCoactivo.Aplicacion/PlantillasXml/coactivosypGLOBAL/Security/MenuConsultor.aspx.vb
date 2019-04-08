Public Partial Class Consultor
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then

        '    'Consultar Expedientes
        '    Dim a_ConExp As String = ""
        '    a_ConExp = CreaScriptClick("a_ConExp", 0, 1, 0)
        '    a_ConExp = vbNewLine & a_ConExp & CreaScriptOver("a_ConExp", 0, 1, 0)

        '    'Actualizar Expedientes
        '    Dim a_ActExp As String = ""
        '    a_ActExp = CreaScriptClick("a_ActExp", 0, 1, 1)
        '    a_ActExp = vbNewLine & a_ActExp & CreaScriptOver("a_ActExp", 0, 1, 1)

        '    'Consulta Diaria
        '    Dim a_ConDia As String = ""
        '    a_ConDia = CreaScriptClick("a_ConDia", 0, 1, 2)
        '    a_ConDia = vbNewLine & a_ConDia & CreaScriptOver("a_ConDia", 0, 1, 2)


        '    Ejecutarjavascript("<script type=""text/javascript""> " & vbNewLine & "$(document).ready(function() { " & vbNewLine & "var permisos; var descrip = new Array(3); var IsPostBack = new Boolean() " & vbNewLine & "if (IsPostBack == false){ permisos = PermisosUsuarios('CONSULTOR'); IsPostBack=true; }" _
        '    & vbNewLine & a_ConExp & vbNewLine & a_ActExp & a_ConDia & vbNewLine & "  }); </script>", "ScriptIsPostBack")
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
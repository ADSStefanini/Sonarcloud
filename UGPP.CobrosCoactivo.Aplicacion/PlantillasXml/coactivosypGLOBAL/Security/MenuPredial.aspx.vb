Public Partial Class MenuPredial
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then

        '    'Consultar Expedientes
        '    Dim a_OpPredial As String = ""
        '    a_OpPredial = CreaScriptClick("a_OpPredial", 0, 1, 0)
        '    a_OpPredial = vbNewLine & a_OpPredial & CreaScriptOver("a_OpPredial", 0, 1, 0)

        '    'Consultar Expedientes
        '    Dim a_opActuaciones As String = ""
        '    a_opActuaciones = CreaScriptClick("a_opActuaciones", 0, 1, 1)
        '    a_opActuaciones = vbNewLine & a_opActuaciones & CreaScriptOver("a_opActuaciones", 0, 1, 1)


        '    Ejecutarjavascript("<script type=""text/javascript""> " & vbNewLine & "$(document).ready(function() { " & vbNewLine & "var permisos; var descrip = new Array(3); var IsPostBack = new Boolean() " & vbNewLine & "if (IsPostBack == false){ permisos = PermisosUsuarios('PREDIAL'); IsPostBack=true; }" _
        '    & vbNewLine & a_OpPredial & vbNewLine & a_opActuaciones & vbNewLine & "  }); </script>", "ScriptIsPostBack")
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
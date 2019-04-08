Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica

Public Class Modulos
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            validateModules()
            poblarDatosUsuario()
        End If
    End Sub

    Private Sub validateModules()
        Dim moduloBLL As ModuloBLL = New ModuloBLL()
        Dim modulos As List(Of Entidades.Modulo) = moduloBLL.obtenerModulosPorPerfil(CType(Session("perfil"), Int32))
        If (modulos.Count() = 1) Then
            Response.Redirect(ResolveClientUrl("~/" & modulos.FirstOrDefault().val_url), True)
        End If
    End Sub

    Private Sub poblarDatosUsuario()
        Dim codigo As String = Session("sscodigousuario").ToString()
        Dim myclass_ As New DatosConsultasTablas
        Dim mytable As New DataTable
        myclass_.consultarUsuario(codigo, Trim(Session("mcobrador").ToString), mytable)

        If mytable.Rows.Count > 0 Then
            lblcodigo.Text = codigo.Trim
            lblNombre.Text = mytable.Rows(0).Item("nombre").ToString.Trim
            lblLogin.Text = mytable.Rows(0).Item("login").ToString.Trim
            lblcedula.Text = mytable.Rows(0).Item("documento").ToString.Trim

            Dim detalle As String = ""
            If mytable.Rows(0).Item("nivelacces").ToString.Trim = "1" Then
                detalle = "1 (Administrador)"
            ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "2" Then
                detalle = "2 (Operador sin permiso de actualizaci&#243;n)"
            ElseIf mytable.Rows(0).Item("nivelacces").ToString.Trim = "3" Then
                detalle = "3 (Operador sin permiso de actualizaci&#243;n y de visualizaci&#243;n de documentos "
            End If

            lbldetalle.Text = detalle
        End If
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Response.Redirect("../login.aspx", True)
    End Sub
End Class
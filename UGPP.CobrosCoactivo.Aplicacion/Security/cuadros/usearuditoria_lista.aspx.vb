Imports System.Data.SqlClient
Partial Public Class usearuditoria_lista
    Inherits System.Web.UI.Page

    Private Sub cargeUser()
        Dim Table As DatasetForm.usuariosDataTable = LoadDatos()
        Me.ViewState("DatosUser") = Table
        dtgUsuarios.DataSource = Table
        dtgUsuarios.DataBind()
    End Sub

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer")
        Dim MyAdapter As New SqlDataAdapter("SELECT * FROM USUARIOS", cnn)
        Dim Mytb As New DatasetForm.usuariosDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call cargeUser()
            Dim Mytb As DatasetForm.usuariosDataTable = CType(Me.ViewState("DatosUser"), DatasetForm.usuariosDataTable)
            lbldetalle.Text = "<b>Regidtros detectados : " & Mytb.Rows.Count & "</b>"

            Dim codigo As String = Session("sscodigousuario").ToString
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

                lbl_detalle.Text = detalle
            End If
        End If
    End Sub

    Protected Sub dtgUsuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgUsuarios.SelectedIndexChanged
        With Me
            Dim Mytb As DatasetForm.usuariosDataTable = CType(.ViewState("DatosUser"), DatasetForm.usuariosDataTable)
            With Mytb.Item(.dtgUsuarios.SelectedIndex)
                Response.Redirect("userauditoria.aspx?cod=" & .codigo & "&nombre=" & .nombre.ToUpper.Trim)
            End With
        End With
    End Sub
End Class
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class DICCIONARIO_AUDITORIA
    Inherits System.Web.UI.Page
    ''' <summary>
    ''' Evento Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            dataBindGrid(obtenerDiccionario())
        End If
    End Sub
    ''' <summary>
    ''' obtiene el diccionaro de datos 
    ''' para ser usado en la grilla
    ''' </summary>
    ''' <returns></returns>
    Private Function obtenerDiccionario() As List(Of DiccionarioAditoria)
        Return New TraductorAuditoriaBLL().obtenerDiccionario()
    End Function
    ''' <summary>
    ''' Obtiene el diccionario segun una llave de busqueda
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Private Function obtenerDiccionarioPorLlave(ByVal llave As String) As List(Of DiccionarioAditoria)
        Return New TraductorAuditoriaBLL().obtenerDiccionarioLike(llave)
    End Function
    ''' <summary>
    ''' Databind del gridview
    ''' </summary>
    ''' <param name="datos">datos para llenar</param>
    Private Sub dataBindGrid(ByVal datos As List(Of DiccionarioAditoria))
        gwDiccionario.DataSource = datos
        gwDiccionario.DataBind()
    End Sub
    ''' <summary>
    ''' Row command
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gwDiccionario_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gwDiccionario.RowCommand
        Dim llave As String
        llave = gwDiccionario.Rows(e.CommandArgument).Cells(0).Text
        If e.CommandName = "edit" Then
            Server.Transfer(String.Concat("EditDICCIONARIO_AUDITORIA.aspx?Edit=True&llave=", llave), True)
        End If
    End Sub
    ''' <summary>
    ''' Evento click boton guardar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Server.Transfer("EditDICCIONARIO_AUDITORIA.aspx?Edit=False&llave=", True)
    End Sub
    ''' <summary>
    ''' Evento clic de buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim result As List(Of DiccionarioAditoria)
        If (txtSearch.Text = String.Empty) Then
            result = New TraductorAuditoriaBLL().obtenerDiccionario()
        Else
            result = obtenerDiccionarioPorLlave(txtSearch.Text)
        End If
        dataBindGrid(result)
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub

    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub
End Class
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Public Class GESTION_DOCUMENTO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not Page.IsPostBack) Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            dataBindGrid(obtenerTipodDocumento())
        End If
    End Sub
    ''' <summary>
    ''' Obntiene los tipos
    ''' </summary>
    ''' <returns></returns>
    Private Function obtenerTipodDocumento() As List(Of DocumentoTituloTipoTitulo)
        Return New DocumentoTituloTipoTituloBLL().obtenerDocumentoTituloTipoTitulo()
    End Function
    ''' <summary>
    ''' llenado de la grilla con los respectivos datos
    ''' </summary>
    ''' <param name="datos"></param>
    Private Sub dataBindGrid(ByVal datos As List(Of DocumentoTituloTipoTitulo))
        gwDocumento.DataSource = datos
        gwDocumento.DataBind()
    End Sub

    Protected Sub gwDocumento_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gwDocumento.RowCommand
        Dim ID As Integer
        Dim Code As String
        Dim rowIndex As Integer = Integer.Parse(e.CommandArgument.ToString())
        ID = gwDocumento.DataKeys(rowIndex)(0)
        Code = gwDocumento.DataKeys(rowIndex)(1).ToString
        If e.CommandName = "edit" Then
            Server.Transfer(String.Concat("EditGESTION_DOCUMENTO.aspx?Edit=True&ID=", ID, "&Code=", Code), True)
        End If
    End Sub
    ''' <summary>
    ''' Consulta de la grilla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim result As List(Of DocumentoTituloTipoTitulo)
        Dim resultSearch As List(Of DocumentoTituloTipoTitulo)
        result = obtenerTipodDocumento()
        resultSearch = result.FindAll(Function(x) x.NOMBRE_DOCUMENTO_TITULO.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
        dataBindGrid(resultSearch)
    End Sub
    ''' <summary>
    ''' Boton de cerrar sesion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub
    ''' <summary>
    ''' boton regresar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Server.Transfer("EditGESTION_DOCUMENTO.aspx?Edit=False")
    End Sub
End Class
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Public Class EditGESTION_DOCUMENTO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Edit As Boolean
        Dim ID As Integer
        Dim Code As String
        Dim negocio As New DocumentoTituloTipoTituloBLL
        If Not Page.IsPostBack Then
            llenarCombos()
            Edit = Boolean.Parse(Request.Params.Get("Edit"))
            Dim documento As DocumentoTituloTipoTitulo
            If Edit Then
                'Editar
                ID = Integer.Parse(Request.Params.Get("ID"))
                Code = Request.Params.Get("Code")
                ddlDTitulo.SelectedValue = ID
                ddlTipo.SelectedValue = Code
                documento = negocio.obtenerDocumentoTituloTipoTitulo().Where(Function(x) x.ID_DOCUMENTO_TITULO = ID AndAlso x.COD_TIPO_TITULO = Code).FirstOrDefault
                If documento IsNot Nothing Then
                    chkActivo.Checked = documento.VAL_ESTADO
                    chkObli.Checked = documento.VAL_OBLIGATORIO
                End If
            Else
                'Nuevo
                chkActivo.Checked = False
                chkObli.Checked = False
                ddlDTitulo.SelectedValue = -1
                ddlTipo.SelectedValue = -1
            End If
        End If
    End Sub

    Private Sub llenarCombos()
        Dim negocio As New DocumentoTituloTipoTituloBLL
        ddlDTitulo.DataSource = negocio.obtenerDocumentosTitulo
        ddlDTitulo.DataBind()
        ddlDTitulo.Items.Add(New ListItem("--Seleccione--", "-1"))
        ddlTipo.DataSource = negocio.obtenerTiposTitulo
        ddlTipo.DataBind()
        ddlTipo.Items.Add(New ListItem("--Seleccione--", "-1"))
    End Sub


    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim negocio As New DocumentoTituloTipoTituloBLL(llenarAuditoria(ddlDTitulo.SelectedValue))
        Dim dato As New DocumentoTituloTipoTitulo
        dato.COD_TIPO_TITULO = ddlTipo.SelectedValue
        dato.ID_DOCUMENTO_TITULO = Integer.Parse(ddlDTitulo.SelectedValue)
        dato.VAL_ESTADO = chkActivo.Checked
        dato.VAL_OBLIGATORIO = chkObli.Checked
        Dim result As Boolean
        result = negocio.salvar(dato)
        Server.Transfer("GESTION_DOCUMENTO.aspx")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Server.Transfer("GESTION_DOCUMENTO.aspx")
    End Sub
    ''' <summary>
    ''' Evento salir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub
    ''' <summary>
    ''' evento atras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub
    ''' <summary>
    ''' Rellena el objeto auditoria
    ''' </summary>
    ''' <returns></returns>
    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "GESTION_DOCUMENTO"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function
End Class
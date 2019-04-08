Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica
Public Class EditDICCIONARIO_AUDITORIA
    Inherits System.Web.UI.Page
    Private log As New LogProcesos
    ''' <summary>
    ''' Evento load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim resultado As New List(Of DiccionarioAditoria)
        Dim llave As String
        Dim Edit As Boolean
        If Not Page.IsPostBack Then
            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            Edit = Boolean.Parse(Request.Params.Get("Edit"))
            llave = Request.Params.Get("llave")
            If (Edit) Then
                resultado = obtenerDiccionarioPorLlave(llave)
                llenarControles(resultado)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Obtiene el diccionario segun una llave de busqueda
    ''' </summary>
    ''' <param name="llave"></param>
    ''' <returns></returns>
    Private Function obtenerDiccionarioPorLlave(ByVal llave As String) As List(Of DiccionarioAditoria)
        Return New TraductorAuditoriaBLL().obtenerDiccionarioPorLlave(llave)
    End Function
    ''' <summary>
    ''' llena los controles
    ''' </summary>
    ''' <param name="datos"></param>
    Private Sub llenarControles(ByVal datos As List(Of DiccionarioAditoria))
        If datos IsNot Nothing Then
            txtDestino.Text = datos.FirstOrDefault().VALOR_DESTINO
            txtOrigen.Text = datos.FirstOrDefault().VALOR_ORIGINAL
            chkActivo.Checked = datos.FirstOrDefault().ACTIVO
        End If
    End Sub
    ''' <summary>
    ''' Evento guardar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim data As New DiccionarioAditoria
        Dim resultado As Boolean
        Try
            data.ACTIVO = chkActivo.Checked
            data.VALOR_DESTINO = txtDestino.Text
            data.VALOR_ORIGINAL = txtOrigen.Text.ToUpper
            resultado = New TraductorAuditoriaBLL(llenarAuditoria(txtDestino.Text, txtOrigen.Text)).salvarDiccionario(data)
            log.SaveLog(Session("ssloginusuario"), "Diccionario Auditoria ", "Diccionario Auditoria ", " Update DICCIONARIO_AUDITORIA ")
        Catch ex As Exception
            log.ErrorLog(ex.InnerException.Message, "EditDICCIONARIO_AUDITORIA.vb", "Diccionario_auditoria", LogProcesos.ErrorType.ErrorLog)
        End Try
        Server.Transfer("DICCIONARIO_AUDITORIA.aspx")
    End Sub
    ''' <summary>
    ''' Evento cancelar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Server.Transfer("DICCIONARIO_AUDITORIA.aspx")
    End Sub

    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        Response.Redirect("../Modulos.aspx")
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        Session.Clear()
        Session.RemoveAll()
        Response.Redirect("../../login.aspx")
    End Sub

    ''' <summary>
    ''' Diligencia el objecto auditoria
    ''' </summary>
    ''' <returns></returns>
    Private Function llenarAuditoria(ByVal valorDestino As String, ByVal valorOriginal As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "DICCIONARIO_AUDITORIA"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = "Destino:" & valorDestino & "Original:" & valorOriginal
        Return auditData
    End Function
End Class
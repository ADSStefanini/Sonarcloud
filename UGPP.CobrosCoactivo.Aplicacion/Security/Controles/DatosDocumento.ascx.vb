Imports coactivosyp.My.Resources
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class DatosDocumento
    Inherits System.Web.UI.UserControl

    Public Property idTitulo As String
    Public Property idDocumento As String
    Public Property idTituloDocumento As String
    Public Property modificarDoc As String
    Public Property muestraLabel As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Mensajes de error
            'reqFlUpEvidencias.Text = Formularios.errorDocumentoRequerido


            If String.IsNullOrEmpty(modificarDoc) OrElse IsNothing(modificarDoc) Then
                modificarDoc = True
            End If

            'btnVer.Enabled = False
            If (IsNothing(muestraLabel)) Then
                muestraLabel = False
            End If

            If Not muestraLabel Then
                lblUploadDpcumento.Visible = False
            End If

            If (IsNothing(idTituloDocumento) AndAlso (IsNothing(idTitulo)) And IsNothing(idDocumento)) Then
                lblErrorDatos.Text = My.Resources.Formularios.errorParametrosCargaDocumento
                lblErrorDatos.Visible = True
                btnCargarPopUp.Enabled = False
            Else
                Dim ExtensionesValidas As String() = StringsResourse.Documentos_extenciones.Split(",")
                ViewState("ExtValidas") = ExtensionesValidas
                HdnExtValidas.Value = StringsResourse.Documentos_extenciones

                If (Not IsNothing(idTituloDocumento) AndAlso idTituloDocumento <> "0") Then
                    Dim _maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()
                    Dim _maestroTitulosDocumentos = _maestroTitulosDocumentosBLL.obtenerMaestroTitulosDocumentosPorId(Convert.ToInt32(idTituloDocumento))

                    If Not IsNothing(_maestroTitulosDocumentos.DES_RUTA_DOCUMENTO) Then
                        linkVer.NavigateUrl = _maestroTitulosDocumentos.DES_RUTA_DOCUMENTO
                        hlinkViewDoc.NavigateUrl = _maestroTitulosDocumentos.DES_RUTA_DOCUMENTO

                        txtNumPaginas.Text = _maestroTitulosDocumentos.NUM_PAGINAS
                        txtObservacionLegibilidad.Text = _maestroTitulosDocumentos.OBSERVA_LEGIBILIDAD

                        linkver.Enabled = True
                        hlinkViewDoc.Visible = True
                    End If
                End If

                If modificarDoc <> "True" Then
                    btnCargar.Enabled = False
                End If

            End If
        End If
    End Sub

    Protected Sub btnCerrarCargaDocumento_Click(sender As Object, e As EventArgs) Handles btnCerrarCargaDocumento.Click
        If modificarDoc = "True" Then
            hlinkViewDoc.Text = String.Empty
        End If

        'txtNumPaginas.Text = String.Empty
        'txtObservacionLegibilidad.Text = String.Empty
        'mp1.Hide()
    End Sub

    Public Function obtenerIdMaestroDocumentoTitulo() As String
        Return HdnIdDoc.Value
    End Function

    Public Sub ocultarLinkVer()
        linkVer.Visible = False
    End Sub

    Protected Sub cmdAsignarDocumento2_Click(sender As Object, e As EventArgs) Handles cmdAsignarDocumento2.Click
        'Se valida que exista el documento
        If String.IsNullOrEmpty(HdnPathFile.Value) Then
            'Se comenta por q el flujo siempre cierra el popup y dejaba un div visible siempre , el mensaje esta corrido
            'lblErrorDoc.Text = Formularios.errorDocumentoRequerido
            'lblErrorDoc.Visible = True
            Exit Sub
        End If
        'Se llama la lógica
        Dim maestroTitulosDocumentosBLL As New MaestroTitulosDocumentosBLL()

        'Dim link = linkVer.NavigateUrl
        'Se crea el objeto con los datos que se van a cargar 
        Dim maestroTitulosDocumentos As New Entidades.MaestroTitulosDocumentos

        'Dim doc As New maes
        If (Not IsNothing(idTituloDocumento)) Then
            'Actualización del documento relacionado
            maestroTitulosDocumentos = maestroTitulosDocumentosBLL.obtenerMaestroTitulosDocumentosPorId(Convert.ToInt32(idTituloDocumento))
        Else
            'Creación de un nuevo documento relacionado
            maestroTitulosDocumentos.ID_DOCUMENTO_TITULO = idDocumento
            maestroTitulosDocumentos.ID_MAESTRO_TITULO = idTitulo
            maestroTitulosDocumentos.TIPO_RUTA = 1
        End If

        maestroTitulosDocumentos.DES_RUTA_DOCUMENTO = "1"
        maestroTitulosDocumentos.OBSERVA_LEGIBILIDAD = "LEGIBLE"
        maestroTitulosDocumentos.NUM_PAGINAS = txtNumPaginas.Text

        Dim maestroTituloDoc As Entidades.MaestroTitulosDocumentos

        If (Not IsNothing(idTituloDocumento)) Then
            maestroTituloDoc = maestroTitulosDocumentosBLL.ActualizarMaestroTitulosDocumentos(maestroTitulosDocumentos)
        Else
            maestroTituloDoc = maestroTitulosDocumentosBLL.crearMaestroTitulosDocumentos(maestroTitulosDocumentos)
        End If

        HdnIdDoc.Value = maestroTituloDoc.ID_MAESTRO_TITULOS_DOCUMENTOS.ToString()

        btnCerrarCargaDocumento_Click(sender, e)
        hdnResultUpload.Value = 1
        Try
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "AutoPostBackScript", "cerrarPopupDocumento();", True)
        Catch ex As Exception

        End Try
    End Sub

End Class
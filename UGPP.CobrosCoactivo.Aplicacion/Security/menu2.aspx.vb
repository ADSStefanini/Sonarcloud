Imports System.Data.SqlClient
Partial Public Class menu2
    Inherits System.Web.UI.Page


    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UsuarioValido") Is Nothing Then
            Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha caducado, para continuar vuelva a ingresar al sistema.<br />" _
              & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
              & "Protocolo de seguridad. Una de las cuestiones que hay que tener en cuenta por temas de seguridad es controlar el tiempo en el que está activa la sesión. Por ejemplo, para evitar que una persona olvide desconectarse y otro aproveche su usuario cuando no esté. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

            menssageError(amsbgox)
            Exit Sub
        End If

        If Not IsPostBack Then
            If Session("ConExp") <> True Then
                ConExp.Attributes.Add("class", "dialog_link")
            End If

            If Session("ActExp") <> True Then
                ActExp.Attributes.Add("class", "dialog_link")
            End If

            If Session("ConDia") <> True Then
                ConDia.Attributes.Add("class", "dialog_link")
            End If
        End If
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Session("UsuarioValido") = Nothing
        Response.Redirect("../login.aspx")
    End Sub

    'Protected Sub HyperLinkUExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HyperLinkUExportar.Click
    '    Dim exportar As New DatosConsultasTablas
    '    Dim TblDocum As DatasetForm.documentosDataTable = New DatasetForm.documentosDataTable
    '    Dim queryString As String = "SELECT * FROM DOCUMENTOS"
    '    TblDocum = exportar.docindexerToTecno(Me.Page, Session("mcobrador"))
    '    If TblDocum.Rows.Count > 0 Then
    '        Using connection As New SqlConnection(Funciones.CadenaConexion)
    '            Dim adapter As New SqlDataAdapter()
    '            adapter.SelectCommand = New SqlCommand(queryString, connection)
    '            Dim builder As SqlCommandBuilder = New SqlCommandBuilder(adapter)
    '            'adapter.Fill(DataSet)

    '            builder.GetUpdateCommand()
    '            builder.GetInsertCommand()

    '            adapter.Update(TblDocum)
    '        End Using
    '    End If
    'End Sub

    'Protected Sub HyperLinkArchivadores_Click(ByVal sender As Object, ByVal e As EventArgs) Handles HyperLinkArchivadores.Click
    '    Response.Redirect("Digitalizacion/digitalizacion.aspx")
    'End Sub
End Class
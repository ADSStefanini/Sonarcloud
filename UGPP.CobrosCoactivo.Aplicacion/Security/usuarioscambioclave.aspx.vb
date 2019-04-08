Imports System.Data.SqlClient
Partial Public Class usuarioscambioclave
    Inherits System.Web.UI.Page

    Private Sub menssageError(ByVal msn As String)
        Me.ViewState("Erroruseractivo") = msn
        ModalPopupError.Show()
    End Sub

    Private Sub cargeUser()
        Dim Table As DatasetForm.usuariosDataTable = LoadDatos()
        Me.ViewState("DatosUser") = Table
        dtgUsuarios.DataSource = Table
        dtgUsuarios.DataBind()
    End Sub

    Private Function LoadDatos() As DataTable
        Dim cnn As String = Session("ConexionServer")
        Dim MyAdapter As New SqlDataAdapter("SELECT codigo ,nombre ,documento ,clave ,nivelacces ,cobrador ,apppredial ,appvehic ,appcuotasp ,appindycom ,login,conuser, useractivo ,useremail ,usercamclave FROM usuarios", cnn)
        Dim Mytb As New DatasetForm.usuariosDataTable
        MyAdapter.Fill(Mytb)
        Return Mytb
    End Function

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("usuario") <> True Then
                Dim amsbgox As String = "<h2 class='err'>SISTEMA DE SEGURIDAD - SESIÓN</h2> <img src='images/icons/Security_Card.png' height = '100' width = '100' />La sesión ha  intentado una Entrada inválida.<br />" _
                                      & "<br /><hr /><h2>ERROR TÉCNICO</h2>" _
                                      & "Este usuario no tiene permisos para ingresar a esta ventana, favor consulte con su administrador. <br />Hay casos en que este protocolo se activa cuando pretenden hacer accesos no valida al sistema, consultar con su administrador del sistema."

                ModalPopupError.OnCancelScript = "mpeSeleccionOnCancel_()"
                menssageError(amsbgox)
                Exit Sub
            End If

            Try
                Call cargeUser()
                lblCobrador.InnerHtml = Session("mcobrador") & "::" & Session("mnombcobrador")
            Catch ex As Exception
                Dim amsbgox As String = "<h2 class='err'>ERROR</h2> Acceso invalido o conexión no habilitada.<br />" _
                & "<br /><hr /><h2>ERROR TÉCNICO</h2>"

                Messenger.InnerHtml = "<font color='#8A0808' >Error:" & ex.Message & " <br /> <b style='text-decoration:underline;'>Nota : si el error persiste intete salir y entrar al sistema. </b> </font>"
                menssageError(amsbgox & Messenger.InnerHtml)
            End Try
        End If
    End Sub

    Private Sub dtgUsuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dtgUsuarios.PageIndexChanging
        dtgUsuarios.PageIndex = e.NewPageIndex
        cargeUser()
    End Sub

    Protected Sub btnDesactivar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDesactivar.Click
        'Dim SQL As String = "UPDATE usuarios SET useractivo = 1 WHERE codigo IN ("

        'Dim presentado As CheckBox
        'Dim sw As Boolean
        'For Each row As GridViewRow In dtgUsuarios.Rows
        '    presentado = dtgUsuarios.Rows(row.RowIndex).FindControl("chkSelec")
        '    If presentado.Checked Then
        '        SQL += "'" & row.Cells(1).Text & "',"
        '        sw = True
        '    Else
        '        sw = False
        '    End If
        'Next
        Div1.InnerHtml = "<font color='#FFFFFF'>(<b>Usuario seleccionado : </b>)</font>"
    End Sub
End Class
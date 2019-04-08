Imports System

Namespace coactivosyp
    Public Class BasePage
        Inherits System.Web.UI.Page


        Protected Overridable Function Page_Load(ByVal sender As Object, ByVal e As EventArgs, Optional ByVal ModalPopupExtender As AjaxControlToolkit.ModalPopupExtender = Nothing) As Boolean
            Dim sw As Boolean = False
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 600) + 5))

            If (Session("ConexionServer") = Nothing) Or (Session("mnivelacces") = Nothing) Then
                CerrarSesion()

                If Not ModalPopupExtender Is Nothing Then
                    Dim amsbgox As String = "<img src='../images/sesion_expirada.jpg' height = '165px' width = '524px' />"
                    ModalPopupExtender.OnCancelScript = "mpeSeleccionOnCancel()"
                    menssageError(amsbgox, ModalPopupExtender)
                Else
                    Response.Redirect("~/Login.aspx")
                End If


                sw = True
                'Server.Transfer("../../../Login.aspx")
                ' Response.Redirect("~/Login.aspx")
            End If

            Return sw
        End Function

        Private Sub CerrarSesion()
            Session.Abandon()
            FormsAuthentication.SignOut()
            'Limpiar los cuadros de texto de busqueda
            'Limpiar cuadros de busqueda
            Session("EJEFISGLOBAL.txtSearchEFINROEXP") = Nothing
            Session("EJEFISGLOBAL.txtSearchED_NOMBRE") = Nothing
            Session("EJEFISGLOBAL.txtSearchEFINIT") = Nothing
            Session("EJEFISGLOBAL.cboEFIESTADO") = Nothing
            Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = Nothing
            Session("Paginacion") = 10
        End Sub

        Private Sub menssageError(ByVal msn As String, ByVal ModalPopupError As AjaxControlToolkit.ModalPopupExtender)
            ViewState("Erroruseractivo") = msn
            ModalPopupError.Show()
        End Sub

       
    End Class



End Namespace
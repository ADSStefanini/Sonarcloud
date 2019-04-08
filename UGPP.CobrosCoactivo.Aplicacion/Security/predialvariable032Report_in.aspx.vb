Public Partial Class predialvariable032Report_in
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("UsuarioValido") Is Nothing Then
            Dim data As DataSet = CType(Session("ssRegistroExcepciones"), DataSet)
            ViewState("TIPO_EXCEPCION") = data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("TIPO_EXCEPCION")
            ViewState("FECHA_DE_ACTO") = DateToLetters(data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("FECHA_DE_ACTO"))
            ViewState("HECHOS") = data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("HECHOS")
            ViewState("CONSIDERACIONES") = data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("CONSIDERACIONES")
            ViewState("ARTICULO_VARIABLE") = data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("ARTICULO_VARIABLE")
            ViewState("NUMERO_EXPEDIENTE") = "E" & String.Format("{0:000000}", CInt(data.Tables("REGISTRO_EXCEPCIONES").Rows(0).Item("NUMERO_EXPEDIENTE")))
            ViewState("RESOLUCION") = "R" & String.Format("{0:000000}", CInt(data.Tables("EJEFIS").Rows(0).Item("RESOLUCION")))

            ViewState("PreNum") = data.Tables("EJEFIS").Rows(0).Item("PreNum")
            ViewState("EfiDir") = data.Tables("EJEFIS").Rows(0).Item("EfiDir")
            ViewState("EfiNom") = data.Tables("EJEFIS").Rows(0).Item("EfiNom")
            ViewState("EfiNit") = data.Tables("EJEFIS").Rows(0).Item("EfiNit")
        Else
            Response.Redirect("~/Login.aspx")
        End If
    End Sub
End Class
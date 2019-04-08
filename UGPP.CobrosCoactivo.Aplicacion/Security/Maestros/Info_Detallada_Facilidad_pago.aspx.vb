Public Partial Class Info_Detallada_Facilidad_pago
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try
                Dim tb As DataTable = LlenarDatos("80582")

                If tb.Rows.Count > 0 Then
                    DtgAcuerdos.DataSource = tb
                    DtgAcuerdos.DataBind()
                Else
                    lblError.Text = "No hay datos para mostrar.."
                End If
            Catch ex As Exception
                lblError.Text = "Error: " & ex.ToString
            End Try
        End If
    End Sub

    Private Function LlenarDatos(ByVal expediente As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT * FROM DETALLE_FACILIDAD_PAGO WHERE EXPEDIENTE = '" & expediente & "'")
        Return tb
    End Function

End Class
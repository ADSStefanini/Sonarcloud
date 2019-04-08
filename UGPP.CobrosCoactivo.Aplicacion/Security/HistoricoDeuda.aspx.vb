Imports System.Data.SqlClient
Partial Public Class HistoricoDeuda
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then

            'Detecta si el viene de ejecuciones fiscales 
            Dim tipo As String = Request("tipo")
            If tipo = 1 Then
                btnAceptar.Visible = False
                Dim cedula As String = Request("cedula")
                If cedula <> Nothing Then
                    ejeFiscales.Attributes.Add("style", "position:absolute;top:50px; height:105px; width: 717px; left:38px; display:block;")

                    ejDeudor.Text = Request("cedula")
                    ejdeuNombre.Text = Request("deunom")
                    PttAnlisisDeuda(Request("cedula").Trim)
                End If
            End If
        End If
    End Sub

    Private Sub PttAnlisisDeuda(ByVal SjNit As String)
        Dim sqlcodigo As String = "SELECT  [PreNum],  [EfiNroExp], [EfiValDeu], [EfiValInt],([EfiValDeu] + [EfiValInt]) as total " _
        & "FROM [EJEFIS] " _
        & "WHERE ([EfiNit] = @nit) ORDER BY total DESC"

        Dim myadapter As New SqlDataAdapter(sqlcodigo, Funciones.CadenaConexionUnion)
        myadapter.SelectCommand.Parameters.Add("@nit", SqlDbType.VarChar)
        myadapter.SelectCommand.Parameters("@nit").Value = SjNit
        Dim mytable As New DataTable

        Dim row As DataRow
        Dim col As DataColumn
        Dim Msg As String = ""
        Dim catota As Integer
        Dim EfiValInt, EfiValDeu, totalta As Object

        myadapter.Fill(mytable)

        If mytable.Rows.Count > 0 Then
            EfiValInt = mytable.Compute("sum(EfiValInt)", "EfiValInt > 0")
            EfiValDeu = mytable.Compute("sum(EfiValDeu)", "EfiValDeu > 0")
            totalta = EfiValDeu + EfiValInt

            EfiValInt = 0
            EfiValDeu = 0


            Msg &= "<table  width=""100%"" class=""servicesT"">" _
            & "<tr><th>Predio</th><th>Expediente</th><th>Deuda</th><th>Interes</th><th>Total</th><th>Acumulado</th><th>%</th></tr>"
            For Each row In mytable.Rows
                Msg &= "<tr>"
                For Each col In mytable.Columns
                    If col.ColumnName = "total" Then
                        catota = catota + valorNull(row(col), 0)
                        Msg &= "<td>" & FormatCurrency(valorNull(row(col), "0"), 2) & "</td>"
                        Msg &= "<td>" & FormatCurrency(catota, 2) & "</td>"
                        Dim cuenta As String = System.Math.Round((Val(valorNull(row(col), 0)) * 100) / totalta, 2)
                        Msg &= "<td>" & cuenta & "%" & "</td>"
                    ElseIf col.ColumnName = "PreNum" Or col.ColumnName = "EfiNroExp" Then
                        Msg &= "<td>" & valorNull(row(col), "&nbsp") & "</td>"
                    Else
                        If IsNumeric(valorNull(row(col), 0)) Then
                            If col.ColumnName = "EfiValDeu" Then
                                EfiValDeu = EfiValDeu + valorNull(row(col), 0)
                            ElseIf col.ColumnName = "EfiValInt" Then
                                EfiValInt = EfiValInt + valorNull(row(col), 0)
                            End If
                            Msg &= "<td>" & FormatCurrency(valorNull(row(col), "0"), 2) & "</td>"
                        End If
                    End If
                Next
                Msg &= "</tr>"
            Next
            Msg &= "</table>"

            Deudadmind.InnerHtml = FormatCurrency(catota, 2)

            castota.InnerHtml = FormatCurrency(catota, 2)
            deuda.InnerHtml = FormatCurrency(EfiValDeu, 2)
            interes.InnerHtml = FormatCurrency(EfiValInt, 2)
            Me.contenidogrids.InnerHtml = Msg
        End If
    End Sub
End Class
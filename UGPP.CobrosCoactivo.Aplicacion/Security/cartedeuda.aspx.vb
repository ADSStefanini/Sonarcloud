Imports System.Data.SqlClient
Partial Public Class cartedeuda
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        If IsNumeric(txtCustos.Text) Then
            Try
                Dim myadapter As SqlDataAdapter
                Dim sqlcodigo As String

                'If ListBuscar.SelectedValue = "Valor" Then
                '    sqlcodigo = "SELECT  TOP 1000 [EfiNit] ,[EfiNom], sum([EfiValDeu]) AS EfiValDeu, sum([EfiValInt]) AS EfiValInt ,sum([EfiValDeu] + [EfiValInt]) as total " _
                '              & "FROM [EJEFIS] group by [EfiNit] ,[EfiNom] HAVING sum([EfiValDeu] + [EfiValInt]) >= @VALOR ORDER BY total DESC"

                '    myadapter = New SqlDataAdapter(sqlcodigo, Funciones.CadenaConexionUnion)
                '    myadapter.SelectCommand.Parameters.Add("@VALOR", SqlDbType.VarChar)
                '    myadapter.SelectCommand.Parameters("@VALOR").Value = txtdato.Text
                'ElseIf ListBuscar.SelectedValue = "ValorM" Then
                '    sqlcodigo = "SELECT  TOP 1000 [EfiNit] ,[EfiNom], sum([EfiValDeu]) AS EfiValDeu, sum([EfiValInt]) AS EfiValInt ,sum([EfiValDeu] + [EfiValInt]) as total " _
                '                         & "FROM [EJEFIS] group by [EfiNit] ,[EfiNom] HAVING sum([EfiValDeu] + [EfiValInt]) <= @VALOR ORDER BY total DESC"

                '    myadapter = New SqlDataAdapter(sqlcodigo, Funciones.CadenaConexionUnion)
                '    myadapter.SelectCommand.Parameters.Add("@VALOR", SqlDbType.VarChar)
                '    myadapter.SelectCommand.Parameters("@VALOR").Value = txtdato.Text

                'ElseIf ListBuscar.SelectedValue = "Igual" Then

                '    sqlcodigo = "SELECT  TOP 1000 [EfiNit] ,[EfiNom], sum([EfiValDeu]) AS EfiValDeu, sum([EfiValInt]) AS EfiValInt ,sum([EfiValDeu] + [EfiValInt]) as total " _
                '                         & "FROM [EJEFIS] group by [EfiNit] ,[EfiNom] HAVING sum([EfiValDeu] + [EfiValInt]) = @VALOR ORDER BY total DESC"

                '    myadapter = New SqlDataAdapter(sqlcodigo, Funciones.CadenaConexionUnion)
                '    myadapter.SelectCommand.Parameters.Add("@VALOR", SqlDbType.VarChar)
                '    myadapter.SelectCommand.Parameters("@VALOR").Value = txtdato.Text
                'End If

                sqlcodigo = "SELECT  TOP (@top) [EfiNit] ,[EfiNom], sum([EfiValDeu]) AS EfiValDeu, sum([EfiValInt]) AS EfiValInt ,sum([EfiValDeu] + [EfiValInt]) as total " _
                          & "FROM [EJEFIS] group by [EfiNit] ,[EfiNom] HAVING sum([EfiValDeu] + [EfiValInt]) >= @VALOR ORDER BY total DESC"

                myadapter = New SqlDataAdapter(sqlcodigo, Funciones.CadenaConexionUnion)
                myadapter.SelectCommand.Parameters.Add("@VALOR", SqlDbType.VarChar)
                myadapter.SelectCommand.Parameters("@VALOR").Value = txtdato.Text
                myadapter.SelectCommand.Parameters.Add("@top", SqlDbType.Int)
                myadapter.SelectCommand.Parameters("@top").Value = txtCustos.Text

                Dim mytable As New DataTable

                Dim row As DataRow
                Dim col As DataColumn
                Dim Msg As String = ""
                Dim catota As Double
                Dim EfiValInt, EfiValDeu, totalta As Double

                myadapter.Fill(mytable)
                EfiValInt = mytable.Compute("sum(EfiValInt)", "EfiValInt > 0")
                EfiValDeu = mytable.Compute("sum(EfiValDeu)", "EfiValDeu > 0")
                totalta = EfiValDeu + EfiValInt

                EfiValInt = 0
                EfiValDeu = 0

                mytable.Columns.Add(New DataColumn("acumulado", GetType(Double)))
                mytable.Columns.Add(New DataColumn("por", GetType(String)))


                If mytable.Rows.Count > 0 Then
                    Msg &= "<table  width=""100%"" class=""servicesT"">" _
                        & "<tr><th colspan='2'>Deudor</th><th>Deuda</th><th>Interes</th><th>Total</th><th>Acumulado</th><th>%</th></tr>"
                    For Each row In mytable.Rows
                        Msg &= "<tr>"
                        For Each col In mytable.Columns
                            If col.ColumnName = "total" Then
                                catota = catota + valorNull(row(col), 0)
                                Msg &= "<td>" & FormatCurrency(valorNull(row(col), "0"), 2) & "</td>"


                                'Msg &= "<td>" & FormatCurrency(catota, 2) & "</td>"
                                row("acumulado") = catota

                                Dim cuenta As String = System.Math.Round((Val(valorNull(row(col), 0)) * 100) / totalta, 2)
                                ' Msg &= "<td>" & cuenta & "%" & "</td>"
                                row("por") = cuenta & "%"
                            ElseIf col.ColumnName = "EfiNit" Or col.ColumnName = "EfiNom" Then
                                Msg &= "<td>" & valorNull(row(col), "&nbsp") & "</td>"
                            Else
                                If IsNumeric(valorNull(row(col), 0)) Then
                                    If col.ColumnName = "EfiValDeu" Then
                                        EfiValDeu = EfiValDeu + valorNull(row(col), 0)
                                    ElseIf col.ColumnName = "EfiValInt" Then
                                        EfiValInt = EfiValInt + valorNull(row(col), 0)
                                    End If
                                    Msg &= "<td>" & FormatCurrency(valorNull(row(col), "0"), 2) & "</td>"
                                Else
                                    Msg &= "<td>" & valorNull(row(col), "&nbsp") & "</td>"
                                End If
                            End If
                        Next
                        Msg &= "</tr>"
                    Next
                    Msg &= "</table>"

                    ViewState("datos") = CType(mytable, DataTable)

                    castota.InnerHtml = FormatCurrency(catota, 2)
                    deuda.InnerHtml = FormatCurrency(EfiValDeu, 2)
                    interes.InnerHtml = FormatCurrency(EfiValInt, 2)
                    Me.contenidogrids.InnerHtml = Msg
                End If
            Catch ex As Exception
                Validator.Text = "Error : " & ex.Message
                Me.Validator.IsValid = False
            End Try
        Else
            Validator.Text = "Error : " & "Digite un nuemro valido."
            Me.Validator.IsValid = False
        End If
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        btnAceptar_Click(Nothing, Nothing)
        Dim cr As New rptparatoconsolidado
        Dim xDato As DataTable = CType(ViewState("datos"), DataTable)
        Funciones.Exportar(Me, cr, xDato, "Prueba.Pdf", "")
    End Sub
End Class
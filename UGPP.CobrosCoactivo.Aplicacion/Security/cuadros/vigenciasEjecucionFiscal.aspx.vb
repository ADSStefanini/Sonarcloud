Imports System.Data.SqlClient
Partial Public Class vigenciasEjecucionFiscal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim refer As String = Request("refExp")
            Dim des As String = Request("des")
            Dim has As String = Request("has")
            Dim xdeudor As String = Request("deudor")
            Dim Mytb As DataTable = busqueda(refer, des, has)

            dtgViewVigencias.DataSource = Mytb
            dtgViewVigencias.DataBind()

            Predio.InnerHtml = Mytb.Rows(0).Item("LiqGen")
            deudor.InnerHtml = xdeudor.Trim
            vigencia.InnerHtml = "Periodos de <b>" & Request("des") & "</b> hasta <b>" & Request("has") & "</b>" 'Request("vigencia")
            tdExpedientes.InnerHtml = Request("expediente")

            Dim x As Integer
            Dim valor1, valor2, valor3, valor4 As Integer
            If Mytb.Rows.Count > 0 Then
                For x = 0 To Mytb.Rows.Count - 1
                    valor1 = valor1 + Val(Mytb.Rows(x).Item("liqTot"))
                    valor2 = valor2 + Val(Mytb.Rows(x).Item("liqint"))
                    valor3 = valor3 + Val(Mytb.Rows(x).Item("LiqTotAbo"))
                    valor4 = valor4 + Val(Mytb.Rows(x).Item("subtot"))
                Next

                Tdeuda.InnerHtml = FormatCurrency(valor1, 2)
                Tinteres.InnerHtml = FormatCurrency(valor2, 2)
                TAbono.InnerHtml = FormatCurrency(valor3, 2)
                Total.InnerHtml = FormatCurrency((valor1 + valor2), 2)
            Else
                Response.Write("<table class=""tabla""><tr><th>" & "Al parecer el predio " & Request("refExp") & " no tiene liquidaciones." & "</th></tr><tr><td>Numero de registros detectados : " & Mytb.Rows.Count & "</td></tr><tr><td><img src='../images/icons/error.png' style='float:left; margin-right:5px;'  alt='' /> <div style='color:#0B3861'>Este documento no posee registro con el cual verificar las vigencias, cierre esta ventana e inténtalo otra vez, si el error persiste favor llamar al administrador del sistema.</div></td></tr></table>")
                conten.Attributes.Add("style", "display:none;")
            End If
        Catch ex As Exception
            Response.Write("<table class=""tabla""><tr><th>" & "Al parecer el predio " & Request("refExp") & " no tiene liquidaciones." & "</th></tr><tr><td>" & ex.Message & "</td></tr><tr><td><img src='../images/icons/error.png' style='float:left; margin-right:5px;'  alt='' /> <div style='color:#0B3861'>Este documento no posee registro con el cual verificar las vigencias, cierre esta ventana e inténtalo otra vez, si el error persiste favor llamar al administrador del sistema.</div></td></tr></table>")
            conten.Attributes.Add("style", "display:none;")
        End Try
    End Sub

    Function busqueda(ByVal refExp As String, ByVal des As String, ByVal has As String) As DataTable
        Dim impuesto As Byte = Session("ssimpuesto")
        Dim sql As String = ""

        If impuesto = 1 Then '(PREDIAL)
            sql = "select LiqGen,PerCod,SUM(LiqTot) as liqTot,SUM(liqint) as liqint,SUM(LiqTotAbo) as LiqTotAbo,(SUM(liqtot) + SUM(liqint)) as subtot from LIQUIDAD  WHERE LiqGen = @REFERENCIA AND PerCod BETWEEN @DES AND @HAS and (LiqEst = 2 or LiqEst = 0 or LiqEst = 8) group by LiqGen,PerCod"
        ElseIf impuesto = 2 Then '(INDUSTRIA COMERCIO)
            sql = "select LiqGen,PerCod,SUM(LiqTot) as liqTot,SUM(liqint) as liqint,SUM(LiqTotAbo) as LiqTotAbo,(SUM(liqtot) + SUM(liqint)) as subtot from LIQUIDAD  WHERE LiqGen = @REFERENCIA AND PerCod BETWEEN @DES AND @HAS and (LiqEst = 2 or LiqEst = 0 or LiqEst = 8 or LiqEst = 3  or LiqEst = 4) group by LiqGen,PerCod"
        End If

        Dim myadapter As New SqlDataAdapter(sql, CadenaConexionUnion)
        myadapter.SelectCommand.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = refExp
        myadapter.SelectCommand.Parameters.Add("@DES", SqlDbType.VarChar).Value = des
        myadapter.SelectCommand.Parameters.Add("@HAS", SqlDbType.VarChar).Value = has
        Dim mydatatable As New DataTable

        myadapter.Fill(mydatatable)

        '----------------------------------------------------------------------------------------------------------------------------------
        '27 de junio de 2013. Incluir intereses en la tabla        
        Dim cmd As String = "SELECT dbo.SIMCOBCOAINT02(@Impuesto,@Vigencia,@Periodo,@Predio,@FechaCorteInteres) AS interes"
        Dim adapterInteres As New SqlDataAdapter(cmd, CadenaConexion)
        Dim dtInteres As New DataTable
        Dim X As Integer = 0
        Dim Interes As Decimal = 0
        Dim AcumInteres As Decimal = 0

        adapterInteres.SelectCommand.Parameters.Add("@Impuesto", SqlDbType.Int).Value = 1
        adapterInteres.SelectCommand.Parameters.Add("@Vigencia", SqlDbType.Int).Value = 2011
        adapterInteres.SelectCommand.Parameters.Add("@Periodo", SqlDbType.Int).Value = 1
        adapterInteres.SelectCommand.Parameters.Add("@Predio", SqlDbType.VarChar).Value = refExp
        adapterInteres.SelectCommand.Parameters.Add("@FechaCorteInteres", SqlDbType.VarChar).Value = DateTime.Now().ToString("dd/MM/yyyy")

        For X = 0 To mydatatable.Rows.Count - 1
            AcumInteres = 0
            adapterInteres.SelectCommand.Parameters("@Vigencia").Value = mydatatable.Rows(X).Item("PerCod")
            'Periodo 1
            adapterInteres.SelectCommand.Parameters("@Periodo").Value = 1
            adapterInteres.Fill(dtInteres)
            Interes = dtInteres.Rows(0).Item("interes")
            AcumInteres = AcumInteres + Interes
            dtInteres.Clear()
            'Periodo 2
            adapterInteres.SelectCommand.Parameters("@Periodo").Value = 2
            adapterInteres.Fill(dtInteres)
            Interes = dtInteres.Rows(0).Item("interes")
            AcumInteres = AcumInteres + Interes
            dtInteres.Clear()
            'Asignar valor a datatable de leyton
            mydatatable.Rows(X).Item("LiqInt") = AcumInteres
        Next
        '----------------------------------------------------------------------------------------------------------------------------------

        Return mydatatable
    End Function
End Class
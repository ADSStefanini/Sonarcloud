Imports System.Data.SqlClient
Partial Public Class deudoresactivos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim impuesto As Byte = Session("ssimpuesto")
            If impuesto = 1 Then '(PREDIAL)
                Radiobuscar.Items(2).Text = "Predio"
                GridView.Columns(1).HeaderText = "Predio"
            ElseIf impuesto = 2 Then '(INDUSTRIA COMERCIO)
                Radiobuscar.Items(2).Text = "Placa"
                GridView.Columns(1).HeaderText = "Placa"
            End If

            Dim tipo As String
            tipo = Request("tipo")
            If tipo <> Nothing Then
                Dim MyVal As Double = Request("val")
                ejDeudor.Text = "Vista previa – informe masivo"
                lblvalor.Text = "Expedientes con sumas mayores a : $" & MyVal.ToString("##,##0.00")
                txtValor.Text = Request("val")
                ejeFiscales.Attributes.Add("style", "position:absolute;top:65px; height:225px; width: 749px; left:14px; z-index:1001;")
                tbLinkretormas.Attributes.Add("style", "position: absolute; top: 181px; left: 16px; border-collapse:collapse; visibility:visible;")
                Call entrada(2)
            Else
                Call entrada(1)
            End If
        End If
    End Sub

    Private Sub entrada(ByVal tipo As Integer)
        Dim sql As String = ""

        Dim Command As SqlClient.SqlCommand = New SqlClient.SqlCommand()
        Command.Connection = New SqlClient.SqlConnection(Funciones.CadenaConexionUnion)

        Select Case tipo
            Case 1
                sql = "SELECT  TOP 80 E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND EFIEST = 0 AND EFIMODCOD = @IMPUESTO GROUP BY E.EFIGEN,E.EFINIT, E.EFINOM, E.EFIDIR ORDER BY MAN_VALORMANDA DESC"
            Case 2
                sql = "SELECT E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND EFIEST = 0 AND EFIMODCOD = @IMPUESTO GROUP BY E.EFIGEN,E.EFINIT, E.EFINOM, E.EFIDIR HAVING (SUM(L.LIQINT) +  SUM(L.LIQTOT)) >= @SUMA ORDER BY MAN_VALORMANDA DESC"
                Command.Parameters.Add("@SUMA", SqlDbType.Int).Value = Val(txtValor.Text)
        End Select

        Command.CommandText = sql
        Command.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Session("ssimpuesto")

        Dim myadapter As New SqlClient.SqlDataAdapter(Command)

        Dim mytable As New DataTable
        myadapter.Fill(mytable)
        If mytable.Rows.Count > 0 Then
            GridView.DataSource = mytable
            GridView.DataBind()
            ejDetalle.Text = UCase("Se detectaron " & Funciones.Num2Text(mytable.Rows.Count) & " registros.")
            Me.ViewState("datos") = CType(mytable, DataTable)
        Else

        End If
    End Sub

    Private Sub GridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With

        GridView.DataSource = CType(Me.ViewState("datos"), DataTable)
        GridView.DataBind()
    End Sub

    Protected Sub LinkRetomar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkRetomar.Click
        Call entrada(1)
    End Sub

    Protected Sub Linkvalor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkvalor.Click
        If IsNumeric(txtValor.Text) Then
            Call entrada(2)
        End If
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Dim sql As String = "SELECT  E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND EFIEST = 0 AND EFIMODCOD = @IMPUESTO "

        If Radiobuscar.SelectedIndex = 0 Then
            sql += " AND E.EFINIT =@PARAMETROS"
        ElseIf Radiobuscar.SelectedIndex = 1 Then
            sql += " AND E.EFINIT =@PARAMETROS"
        ElseIf Radiobuscar.SelectedIndex = 2 Then
            sql += " AND E.EFIGEN =@PARAMETROS"
        End If

        sql += " GROUP BY E.EFIGEN,E.EFINIT, E.EFINOM, E.EFIDIR"

        Dim myadapter As New SqlClient.SqlDataAdapter(sql, Funciones.CadenaConexionUnion)
        myadapter.SelectCommand.Parameters.Add("@PARAMETROS", SqlDbType.VarChar).Value = txtConsultar.Text
        myadapter.SelectCommand.Parameters.Add("@IMPUESTO", SqlDbType.VarChar).Value = Session("ssimpuesto")

        Dim mytable As New DataTable
        myadapter.Fill(mytable)
        If mytable.Rows.Count > 0 Then
            GridView.DataSource = mytable
            GridView.DataBind()
            ejDetalle.Text = UCase("Se detectaron " & Funciones.Num2Text(mytable.Rows.Count) & " registros.")
            Me.ViewState("datos") = CType(mytable, DataTable)
        Else

        End If


    End Sub

    Protected Sub Linkretormas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkretormas.Click
        Response.Redirect("cobranzasMasiva.aspx?tipo=1&val=" & Request("val"))
    End Sub
End Class
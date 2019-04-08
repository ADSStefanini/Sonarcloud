Imports System.Data.SqlClient
Partial Public Class verforpag
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Predio As String
        If Not Page.IsPostBack Then
            Predio = Request.QueryString("idpag")
            LoadGrid(Predio)
        End If
    End Sub

    Private Sub LoadGrid(ByVal pPago As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion

        'Hallar el consecutivo
        cmd = "SELECT ForPagCon, ForNumRec FROM formp WHERE ForNumRec = '" & pPago & "'"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtConsec As New DataTable
        Adaptador.Fill(dtConsec)        

        If dtConsec.Rows.Count > 0 Then
            Dim consec As String = dtConsec.Rows(0).Item(0).ToString
            txtFormaPago.Text = consec

            'Mostrar los datos del encabezado en base al consecutivo
            cmd = "SELECT ForPagCon, ForUltCon, ForEst, ForValEfe, ForValChe, ForValTar, ForValEsp, ForTot, ForFec, ForLiq, ForDif, " & _
                    "forUsu, ForCen, ForSub FROM formpago WHERE ForPagCon = '" & consec & "'"
            Dim Adaptador2 As New SqlDataAdapter(cmd, cnx)
            Dim dtHeader As New DataTable
            Adaptador2.Fill(dtHeader)
            If dtHeader.Rows.Count > 0 Then
                'Mostrar encabezado
                txtCentro.Text = dtHeader.Rows(0).Item(12).ToString
                txtSubcentro.Text = dtHeader.Rows(0).Item(13).ToString
                txtUsuario.Text = dtHeader.Rows(0).Item(11).ToString
                txtFechaPag.Text = dtHeader.Rows(0).Item(8).ToString
                txtEstado.Text = dtHeader.Rows(0).Item(2).ToString

                'Mostrar pie de pagina
                txtEfectivo.Text = dtHeader.Rows(0).Item(3).ToString
                txtCheque.Text = dtHeader.Rows(0).Item(4).ToString
                txtTarjeta.Text = dtHeader.Rows(0).Item(5).ToString
                txtTotalLiq.Text = dtHeader.Rows(0).Item(9).ToString
                txtOtros.Text = dtHeader.Rows(0).Item(6).ToString
                txtValTotal.Text = dtHeader.Rows(0).Item(7).ToString
                txtDiferencia.Text = dtHeader.Rows(0).Item(10).ToString
            End If

            'Mostrar datos del detalle del grid
            cmd = "SELECT formpag1.ForPagCon, formpag1.ForCon, formpag1.ForNroDoc, formpag1.ForCtaAut, formpag1.ForVal, " & _
                "formpag1.ForFecDoc, formpag1.ForTip, formpag1.ForBanCod, document.DocDes, centros.CenDes " & _
                "FROM formpag1 " & _
                "LEFT OUTER JOIN document ON formpag1.ForTip = document.DocCod " & _
                "LEFT OUTER JOIN centros  ON formpag1.ForBanCod = centros.CenCod " & _
                "WHERE formpag1.ForPagCon = " & consec

            Dim Adaptador3 As New SqlDataAdapter(cmd, cnx)
            Dim dtDetalle As New DataTable
            Adaptador3.Fill(dtDetalle)
            GridFormaPago.DataSource = dtDetalle
            GridFormaPago.DataBind()
        End If

    End Sub
End Class
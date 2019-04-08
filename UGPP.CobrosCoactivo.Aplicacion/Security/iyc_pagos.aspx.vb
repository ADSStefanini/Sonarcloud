Imports System.Data.SqlClient
Partial Public Class iyc_pagos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Placa As String
        Dim cnx As String = Funciones.CadenaConexionUnion
        Placa = Request.QueryString("idplaca")

        If Not Page.IsPostBack Then
            txtIdPlaca.Text = Placa
            LoadGridPagos(Placa)
        End If
    End Sub

    Private Sub LoadGridPagos(ByVal pPlaca As String)
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexionUnion        
        'Grid de Pagos
        cmd = "SELECT PagNroRec, PagFec, PagFecLiq, PagTot, PagPerDes, PagSubDes, PagPerHas, PagSubHas, TpaCod, PagEst, PagCtb " & _
                "FROM PAGOS WHERE PagCtb = '" & pPlaca & "' " & _
                "ORDER BY PagCtb, PagFecLiq"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPagos As New DataTable
        Adaptador.Fill(dtPagos)

        If dtPagos.Rows.Count > 0 Then
            'GridPagos
            GridPagos.DataSource = dtPagos
            GridPagos.DataBind()
        End If
    End Sub


    Private Sub GridPagos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridPagos.PageIndexChanging
        Try
            GridPagos.PageIndex = e.NewPageIndex
            LoadGridPagos(txtIdPlaca.Text.Trim)
        Catch ex As Exception

        End Try
    End Sub
End Class
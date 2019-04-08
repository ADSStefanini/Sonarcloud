Imports System.Data
Imports System.Data.SqlClient
Partial Public Class informepagos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub LlenarDatos()
        Dim cmd As String
        'Dim system.IFormatProvider cultura = new CultureInfo("es-CO");
        'dIni = Convert.ToDateTime(sFechaIni, cultura);

        'Dim cultura As System.IFormatProvider
        'cultura = New System.Globalization.CultureInfo("es-CO")
        Dim cnn As String = Funciones.CadenaConexionUnion
        'Dim MyAdapter As New SqlDataAdapter("SELECT PagNroRec,PagFec,PagFecLiq,PagValEfe,PagPerDes,PagSubDes,PagPerHas,TpaCod,PagEst FROM pagos WHERE PagCtb = '010500070016000'", cnn)
        'Dim MyAdapter As New SqlDataAdapter("SELECT PagNroRec,CAST(PagFec AS DATE) AS PagFec,CAST(PagFecLiq AS DATE) AS PagFecLiq,PagValEfe,PagPerDes,PagSubDes,PagPerHas,TpaCod FROM pagos WHERE PagCtb = '010500070016000'", cnn)

        cmd = "SELECT PagNroRec,PagFec,PagFecLiq,PagNom,PagValEfe,PagPerDes,PagSubDes,PagPerHas,TpaCod FROM pagos " & _
                "WHERE PagFec BETWEEN CONVERT(DATE, '" & Me.txtFechaIni.Text.Trim & "',103) AND " & _
                "CONVERT(DATE, '" & Me.txtFechaFin.Text.Trim & "',103)"
        Dim MyAdapter As New SqlDataAdapter(cmd, cnn)
        'MyAdapter.SelectCommand.Parameters.Add("@FechaIni", SqlDbType.Date)
        'MyAdapter.SelectCommand.Parameters.Add("@FechaFin", SqlDbType.Date)

        'MyAdapter.SelectCommand.Parameters("@FechaIni").Value = Me.txtFechaIni.Text
        'MyAdapter.SelectCommand.Parameters("@FechaIni").Value = Date.ParseExact(Me.txtFechaIni.Text.Trim, "dd/MM/yyyy", )
        'MyAdapter.SelectCommand.Parameters("@FechaIni").Value = Convert.ToDateTime(Me.txtFechaIni.Text.Trim, cultura)
        'MyAdapter.SelectCommand.Parameters("@FechaFin").Value = Convert.ToDateTime(Me.txtFechaFin.Text.Trim, cultura)

        Dim dtPagos As New DataTable
        MyAdapter.Fill(dtPagos)
        'Return dtPagos
        dtg_Pagos.DataSource = dtPagos
        dtg_Pagos.DataBind()
    End Sub
    Protected Sub btnConsultarPagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultarPagos.Click
        'Dim Table As DataTable = LoadDatos()
        'Me.ViewState("datos_pagos") = Table
        'dtg_Pagos.DataSource = Table
        'dtg_Pagos.DataBind()
        LlenarDatos()
    End Sub

    Protected Sub dtg_Pagos_SelectedIndexChanged(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles dtg_Pagos.PageIndexChanging
        dtg_Pagos.PageIndex = e.NewPageIndex
        'dtg_Pagos.DataBind()
        'Dim Table As DataTable = LoadDatos()        
        'dtg_Pagos.DataSource = Table
        'dtg_Pagos.DataBind()
        LlenarDatos()
    End Sub
End Class
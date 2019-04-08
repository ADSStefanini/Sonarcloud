Imports System.Data
Imports System.Data.SqlClient
Partial Public Class consultaracum
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub LlenarDatos()
        Dim cmd As String
        Dim cnx As String = Funciones.CadenaConexion

        'cmd = "SELECT PagNroRec,PagFec,PagFecLiq,PagNom,PagValEfe,PagPerDes,PagSubDes,PagPerHas,TpaCod FROM pagos " & _
        '       "WHERE PagFec BETWEEN CONVERT(DATE, '" & Me.txtFechaIni.Text.Trim & "',103) AND " & _
        '      "CONVERT(DATE, '" & Me.txtFechaFin.Text.Trim & "',103)"

        cmd = "SELECT documentos.entidad AS cedula,entesdbf.nombre,documentos.idacto,documentos.nomarchivo," & _
                "documentos.fecharadic,documentos.docexpediente,documentos.docproceso,documentos.docpredio_refecatrastal," & _
                "documentos.docacumulacio, documentos.docfechadoc, " & _
                "'historiaexpediente.aspx?expediente=' + RTRIM(documentos.docexpediente) +" & _
                "'&refcatastral=' + RTRIM(documentos.docpredio_refecatrastal) + " & _
                "'&tipo=1&cedula=' + RTRIM(documentos.entidad) + " & _
                "'&deunom=' + RTRIM(entesdbf.nombre) +" & _
                "'utilpas=301&des=1983&has=2002' AS enlace " & _
                "FROM documentos, entesdbf " & _
                "WHERE documentos.entidad = entesdbf.codigo_nit AND " & _
                "RTRIM(docacumulacio) = '" & Me.txtResAcum.Text.Trim & "'"

        Dim MyAdapter As New SqlDataAdapter(cmd, cnx)

        Dim dtAcum As New DataTable
        MyAdapter.Fill(dtAcum)
        'Return dtPagos
        dtg_acum.DataSource = dtAcum
        dtg_acum.DataBind()
    End Sub

    Protected Sub btnConsultarPagos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultarPagos.Click
        LlenarDatos()
    End Sub
End Class
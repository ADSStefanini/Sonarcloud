Imports System.Data.SqlClient
Imports System.IO


Partial Public Class Datos_Facilidad_Pago
    Inherits System.Web.UI.Page

    Protected Shared DetallesCuotas As New DataTable
    Protected Shared TotalDeudaSinintereses As New Integer
    Protected Shared TablaDetalle As New Reportes_Admistratiivos.DETALLES_ACUERDO_PAGODataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarInformacion()
        End If
    End Sub

    Private Function ValidarAcuerdo(ByVal expediente As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT A.* , B.DG_NRO_DOC, B.DG_FECHA_DOC , MA.CUOTAS,MA.FECHA_MAX,MI.FECHA_MIN FROM MAESTRO_ACUERDOS A, DOCUMENTOS_GENERADOS B, (SELECT TOP 1 MAESTRO_ACUERDOS.DOCUMENTO, MAX(CUOTA_NUMERO) AS CUOTAS,MAX(FECHA_CUOTA) AS FECHA_MAX FROM DETALLES_ACUERDO_PAGO, MAESTRO_ACUERDOS WHERE DETALLES_ACUERDO_PAGO.DOCUMENTO = MAESTRO_ACUERDOS.DOCUMENTO AND EXPEDIENTE = '" & expediente & "' GROUP BY MAESTRO_ACUERDOS.DOCUMENTO) MA,(SELECT TOP 1 MAESTRO_ACUERDOS.DOCUMENTO, MIN(FECHA_CUOTA) AS FECHA_MIN FROM   DETALLES_ACUERDO_PAGO, MAESTRO_ACUERDOS WHERE DETALLES_ACUERDO_PAGO.DOCUMENTO = MAESTRO_ACUERDOS.DOCUMENTO AND EXPEDIENTE = '" & expediente & "' GROUP BY MAESTRO_ACUERDOS.DOCUMENTO) MI WHERE A.DOCUMENTO = MA.DOCUMENTO AND   A.DOCUMENTO = MI.DOCUMENTO AND A.EXPEDIENTE = B.DG_EXPEDIENTE AND (DG_COD_ACTO = '331' OR DG_COD_ACTO = '337') AND EXPEDIENTE = '" & expediente & "'")
        Return tb
    End Function

    Private Function LoadDetalleAcuerdo(ByVal numero_acuerdo As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT CUOTA_NUMERO, PERIODO,FECHA_CUOTA,FECHA_PAGO,VALOR_CUOTA  FROM DETALLES_ACUERDO_PAGO WHERE DOCUMENTO =  '" & numero_acuerdo & "'")
        Return tb
    End Function

    Private Sub CargarInformacion()
        Dim tb As DataTable = ValidarAcuerdo(Request("pExpediente"))

        If tb.Rows.Count > 0 Then
            TxtNumAcuerdo.Text = tb.Rows(0).Item("DOCUMENTO").ToString
            TxtNroProceso.Text = Request("pExpediente")
            Txt_solicitante.Text = tb.Rows(0).Item("ID_SOLICITANTE").ToString
            TxtNom_Solicitante.Text = tb.Rows(0).Item("NOM_SOLICITANTE").ToString
            CmbSolicitante.SelectedValue = tb.Rows(0).Item("TP_SOLICITANTE").ToString
            TxtGarante.Text = tb.Rows(0).Item("ID_GARANTE").ToString
            TxtNom_garante.Text = tb.Rows(0).Item("NOM_GARANTE").ToString
            CmbGarante.SelectedValue = tb.Rows(0).Item("TP_GARANTE").ToString
            TxtDescripcionGarantia.Text = tb.Rows(0).Item("DESC_GARANTIA").ToString
            TxtNroresolucion.Text = "RCC - " & tb.Rows(0).Item("DG_NRO_DOC").ToString
            Txtfecharesolucion.Text = CDate(tb.Rows(0).Item("DG_FECHA_DOC")).ToString("dd/MM/yyyy")
            Txtvalortotalacuerdo.Text = FormatCurrency(tb.Rows(0).Item("TOTAL_DEUDA"), 0, TriState.True)
            Txtporcuotainicial.Text = "% " & CStr(Math.Round((tb.Rows(0).Item("CUOTA_INI").ToString * 100) / tb.Rows(0).Item("TOTAL_DEUDA").ToString, 2))
            Txtvalorcuotainicial.Text = FormatCurrency(tb.Rows(0).Item("CUOTA_INI").ToString, 0, TriState.True)
            Txtnrocuotas.Text = tb.Rows(0).Item("CUOTAS").ToString
            Txtfechapagoinicial.Text = CDate(tb.Rows(0).Item("FECHA_INICIO").ToString).ToString("dd/MM/yyyy")
            Txtvencprimcuota.Text = CDate(tb.Rows(0).Item("FECHA_MIN")).ToString("dd/MM/yyyy")
            Txtvencultcuota.Text = CDate(tb.Rows(0).Item("FECHA_MAX")).ToString("dd/MM/yyyy")


            Dim detalle As DataTable = LoadDetalleAcuerdo(TxtNumAcuerdo.Text.Trim)

            DtgAcuerdos.DataSource = detalle
            DtgAcuerdos.DataBind()

            lblError.Text = ""
        Else
            lblError.Text = "No se detectó infomación sobre de la facilidad de pago proceso Nro. <Strong>" & Request("pExpediente") & " </Strong>"
        End If

    End Sub

    Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoad.Click
        Try
            CargarInformacion()
        Catch ex As Exception
            lblError.Text = "Error: " & ex.ToString
        End Try
    End Sub
End Class




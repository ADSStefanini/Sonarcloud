Imports System.Data.SqlClient
Imports System.IO


Partial Public Class InformacionCuotasFacilidadPago
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarInformacion()
        End If
    End Sub

    Private Function ValidarAcuerdo(ByVal expediente As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT A.* FROM MAESTRO_ACUERDOS A  WHERE A.EXPEDIENTE = '" & expediente & "'")
        Return tb
    End Function

    Private Function LoadDetalleAcuerdo(ByVal expediente As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos(" SELECT CASE WHEN (B.FECHA_PAGO = '' OR B.FECHA_PAGO IS NULL ) AND (B.FECHA_CUOTA < GETDATE() AND B.VALOR_PAGADO = 0)  THEN 'CUOTA INCUMPLIDA'   " & _
                                                        " 		 WHEN (B.FECHA_CUOTA <=  B.FECHA_PAGO) AND (B.VALOR_CUOTA > C.VALOR_PAGADO) THEN 'PAGO INFERIOR'          " & _
                                                        " 		 WHEN (B.FECHA_CUOTA <  B.FECHA_PAGO) AND (B.VALOR_CUOTA = C.VALOR_PAGADO)  THEN 'PAGO ATRASADO'          " & _
                                                        " 		 WHEN (B.FECHA_CUOTA = B.FECHA_PAGO) AND (B.VALOR_CUOTA <= C.VALOR_PAGADO)  THEN 'PAGO PUNTUAL'           " & _
                                                        " 		 WHEN (B.FECHA_CUOTA > B.FECHA_PAGO) AND (B.VALOR_CUOTA <= C.VALOR_PAGADO)  THEN 'PAGO ADELANTADO'        " & _
                                                        " 		 ELSE 'CUOTA POR PAGAR'  END AS ESTADO_PAGO,                                                                                              " & _
                                                        " 		 B.CUOTA_NUMERO,B.FECHA_CUOTA,B.FECHA_PAGO,B.VALOR_CUOTA,C.VALOR_PAGADO                                                                   " & _
                                                        " 		 FROM MAESTRO_ACUERDOS A,                                                                                                                 " & _
                                                        " 			  DETALLES_ACUERDO_PAGO B ,                                                                                                           " & _
                                                        " 			  ( SELECT  DISTINCT NRO_CUOTA, VALOR_CUOTA , FECHA_CUOTA , SUM(B.MONTO_PAGO) AS VALOR_PAGADO, A.EXPEDIENTE                           " & _
                                                        " 				FROM DETALLE_FACILIDAD_PAGO A ,                                                                                                   " & _
                                                        " 					 SQL_PLANILLA B ,                                                                                                             " & _
                                                        " 					 EJEFISGLOBAL C                                                                                                               " & _
                                                        " 				WHERE B.EXPEDIENTE = A.EXPEDIENTE                                                                                                 " & _
                                                        " 				AND   A.SUBSISTEMA = B.SUBSISTEMA                                                                                                 " & _
                                                        " 				AND   A.NIT_EMPRESA = B.NIT_EMPRESA                                                                                               " & _
                                                        " 				AND   A.ANNO = B.ANNO                                                                                                             " & _
                                                        " 				AND   A.MES = B.MES                                                                                                               " & _
                                                        " 				AND   A.CEDULA = B.CEDULA                                                                                                         " & _
                                                        " 				AND   A.EXPEDIENTE = C.EFINROEXP                                                                                                  " & _
                                                        " 				AND   C.EFIESTADO = '05'                                                                                                          " & _
                                                        " 				AND   A.NRO_CUOTA > 0                                                                                                             " & _
                                                        " 				AND A.EXPEDIENTE = '" & expediente & "'                                                                                                        " & _
                                                        " 				GROUP BY A.NRO_CUOTA,VALOR_CUOTA,FECHA_CUOTA,A.EXPEDIENTE ) C                                                                     " & _
                                                        " WHERE A.DOCUMENTO = B.DOCUMENTO                                                                                                                 " & _
                                                        " AND   A.EXPEDIENTE = C.EXPEDIENTE                                                                                                               " & _
                                                        " AND   B.CUOTA_NUMERO = C.NRO_CUOTA   ORDER BY B.CUOTA_NUMERO                                                                                                           ")
        Return tb
    End Function


    Private Function LoadDetalleAcuerdoMultas(ByVal expediente As String) As DataTable
        Dim tb As DataTable = Funciones.RetornaCargadatos(" SELECT DISTINCT																														  " & _
                                                        " CASE WHEN (B.FECHA_PAGO = '' OR B.FECHA_PAGO IS NULL ) AND (B.FECHA_CUOTA < GETDATE() AND B.VALOR_PAGADO = 0)  THEN 'CUOTA INCUMPLIDA'  " & _
                                                        " 	 WHEN (B.FECHA_CUOTA <=  B.FECHA_PAGO) AND (B.VALOR_CUOTA > B.VALOR_PAGADO) THEN 'PAGO INFERIOR'      " & _
                                                        " 	 WHEN (B.FECHA_CUOTA <  B.FECHA_PAGO) AND (B.VALOR_CUOTA = B.VALOR_PAGADO)  THEN 'PAGO ATRASADO'      " & _
                                                        " 	 WHEN (B.FECHA_CUOTA = B.FECHA_PAGO) AND (B.VALOR_CUOTA <= B.VALOR_PAGADO)  THEN 'PAGO PUNTUAL'       " & _
                                                        " 	 WHEN (B.FECHA_CUOTA > B.FECHA_PAGO) AND (B.VALOR_CUOTA <= B.VALOR_PAGADO)  THEN 'PAGO ADELANTADO'    " & _
                                                        " 	 ELSE 'CUOTA POR PAGAR'  END AS ESTADO_PAGO,                                                                                          " & _
                                                        " 	 B.CUOTA_NUMERO,B.FECHA_CUOTA,B.FECHA_PAGO,B.VALOR_CUOTA,B.VALOR_PAGADO                                                               " & _
                                                        " 	 FROM MAESTRO_ACUERDOS A,                                                                                                             " & _
                                                        " 		  DETALLES_ACUERDO_PAGO B                                                                                                         " & _
                                                        " WHERE A.DOCUMENTO = B.DOCUMENTO                                                                                                         " & _
                                                        " AND   A.EXPEDIENTE = '" & expediente & "'   ORDER BY B.CUOTA_NUMERO                                                                                                           ")
        Return tb
    End Function


    Private Sub CargarInformacion()
        Dim tb As DataTable


        tb = ValidarAcuerdo(Request("pExpediente"))


        If tb.Rows.Count > 0 Then
            Dim detalle As New DataTable

            Dim titulo As DataTable = TipoTitulo(Request("pExpediente"))

            Dim codigotitulo As String = titulo.Rows(0).Item("codigo")
            If (codigotitulo = "01") Or (codigotitulo = "02") Or (codigotitulo = "04") Then
                detalle = LoadDetalleAcuerdo(Request("pExpediente"))
            ElseIf (codigotitulo = "05") Or (codigotitulo = "07") Then
                detalle = LoadDetalleAcuerdoMultas(Request("pExpediente"))
            Else
                detalle = Nothing
            End If

            If detalle.Rows.Count > 0 Then
                Txttotalcuotas.Text = String.Format("{0:C2}", CDbl(detalle.Compute("SUM(VALOR_CUOTA)", String.Empty)))
                txttotalpagos.Text = String.Format("{0:C2}", CDbl(detalle.Compute("SUM(VALOR_PAGADO)", String.Empty)))
                txtvalorporpagar.Text = String.Format("{0:C2}", CDbl(detalle.Compute("SUM(VALOR_CUOTA)", String.Empty)) - CDbl(detalle.Compute("SUM(VALOR_PAGADO)", String.Empty)))

                DtgAcuerdos.DataSource = detalle
                DtgAcuerdos.DataBind()
            End If

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

    Private Function TipoTitulo(ByVal expediente As String) As DataTable
        Dim sw As Boolean = False
        Dim tb As DataTable = Funciones.RetornaCargadatos("SELECT TOP 1 c.codigo, c.nombre FROM EJEFISGLOBAL A , MAESTRO_TITULOS B, TIPOS_TITULO C  WHERE A.EFINROEXP = B.MT_expediente AND b.MT_tipo_titulo = c.codigo and EFINROEXP = '" & expediente & "'")
        Return tb
    End Function

End Class




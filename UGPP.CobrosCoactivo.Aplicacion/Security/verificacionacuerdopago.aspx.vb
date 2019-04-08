Imports System.Data.SqlClient
Partial Public Class verificacionacuerdopago
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("ConexionServer") Is Nothing Then
            Session("ConexionServer") = Funciones.CadenaConexion
        End If
        If Not Me.Page.IsPostBack Then
            lblCobrador.Text = Session("mcobrador") & "::" & Session("mnombcobrador")

            Nuevo()
        End If
    End Sub

    Protected Sub LinkCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkCancelar.Click
        Response.Redirect("menuej.aspx")
    End Sub

    Protected Sub btnLiquidar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLiquidar.Click
    End Sub

    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Nuevo()
    End Sub

    Private Sub Nuevo()
        With Me
            txtExpediente.Text = ""
            txtNroAcuerdo.Text = ""
            .LlenarGrid()
            .DtgAcuerdos.DataBind()
            dgVigencia.DataSource = Nothing
            dgVigencia.DataBind()
        End With
    End Sub

    Private Sub LlenarGrid()
        With Me
            Dim row As New dAcuerdoPago.VERIFICAR_ACUERDODataTable
            Dim FilasAct As Integer

            If .ViewState("rowDetalle") Is Nothing Then
                FilasAct = 0
            Else
                row = CType(.ViewState("rowDetalle"), dAcuerdoPago.VERIFICAR_ACUERDODataTable)
                FilasAct = row.Rows.Count
            End If

            Dim FilasAdd As Integer
            Dim FilasGrid As Integer = .DtgAcuerdos.PageSize
            Dim x As Integer
            Select Case FilasAct
                Case 0
                    FilasAdd = FilasGrid
                Case Is < FilasGrid
                    FilasAdd = FilasGrid - FilasAct
                Case Is > FilasGrid
                    Dim Residuo As Integer = FilasAct Mod FilasGrid
                    Dim PaginasGrid As Integer = Int(FilasAct / FilasGrid)
                    If Residuo <> 0 Then
                        PaginasGrid = PaginasGrid + 1
                    End If
                    FilasAdd = (PaginasGrid * FilasGrid) - FilasAct
            End Select
            For x = 1 To FilasAdd
                row.AddVERIFICAR_ACUERDORow(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Next
            DtgAcuerdos.DataSource = row
            .DtgAcuerdos.DataBind()
        End With
    End Sub

    Private Sub AcuerdoPorPredio(ByVal Table As DataTable, ByVal predio As String)
        Using connection As New System.Data.SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim sql As String = " SELECT DOCUMENTO AS ACUERDO, TOTAL_DEUDA AS VALOR,EXPEDIENTE FROM MAESTRO_ACUERDOS WHERE PLACA = @PREDIO "

            Using Command As New System.Data.SqlClient.SqlCommand(sql, connection)
                Command.CommandTimeout = 60000
                Command.Parameters.AddWithValue("@PREDIO", predio)
                Dim Adap As New SqlDataAdapter(Command)
                Adap.Fill(Table)
                ViewState("datos") = Table
            End Using
        End Using
    End Sub

    Protected Sub TxtNumPredio_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TxtNumPredio.TextChanged
        Dim tb As New DataTable
        AcuerdoPorPredio(tb, TxtNumPredio.Text.Trim)
        If tb.Rows.Count > 0 Then
            dgVigencia.DataSource = tb
            dgVigencia.DataBind()
        Else
            Validator.ErrorMessage = "La predio/reg. Ind y Com " & TxtNumPredio.Text & " no se le ha generado acuerdo de pago..."
            Validator.IsValid = False
            Nuevo()
        End If
    End Sub

    Protected Sub dgVigencia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgVigencia.SelectedIndexChanged
        With Me
            Dim Mytb As DataTable = CType(ViewState("datos"), DataTable)
            Dim index As Integer = dgVigencia.SelectedIndex
            If Mytb.Rows.Count > 0 Then
                txtExpediente.Text = Mytb.Rows(index).Item("EXPEDIENTE")
                txtNroAcuerdo.Text = Mytb.Rows(index).Item("ACUERDO")
                DatosAcuerdo(Mytb.Rows(index).Item("EXPEDIENTE"))
            End If

        End With
    End Sub

    Private Sub DatosAcuerdo(ByVal expediente As String)
        Using connection As New System.Data.SqlClient.SqlConnection(Funciones.CadenaConexion)
            Dim sql As String = " SELECT SUM(PagTot) AS PAGO_TOTAL,B.PAGO_ACUERDO,C.TOTAL_DEUDA AS TOTAL_DEUDA,b.VIG_FINAL,b.VIG_INICIAL, " & _
                                " 		case when SUM(PagTot) < B.PAGO_ACUERDO then 'INCUMPLIDO' else 'VIGENTE'  end as ESTADO,           " & _
                                " 	   case when SUM(PagTot) >= C.TOTAL_DEUDA THEN 'TERMINADO' else 'EN_PROCESO'  end as ESTADO_GENERAL   " & _
                                " FROM impuestos.dbo.pagos A,                                                                             " & _
                                " 	 (SELECT  A.EFIGEN, B.DOCUMENTO ,                                                                     " & _
                                " 			  B.PAGO_ACUERDO,                                                                             " & _
                                " 			  convert(varchar,efiperdes)+ '-' + convert(varchar,efisubdes) AS VIG_INICIAL,                " & _
                                " 			  convert(varchar,efiperhas)+ '-' + convert(varchar,efisubhas) as VIG_FINAL,                  " & _
                                " 			  A.EFINROEXP AS EXPEDIENTE                                                                   " & _
                                " 			FROM	 EJEFISGLOBAL A,                                                                      " & _
                                " 					(SELECT SUM(VALOR_CUOTA) AS PAGO_ACUERDO,                                             " & _
                                " 							A.DOCUMENTO,                                                                  " & _
                                " 							A.EXPEDIENTE,                                                                 " & _
                                " 							A.PLACA                                                                       " & _
                                " 					  FROM MAESTRO_ACUERDOS A ,                                                           " & _
                                " 						DETALLES_ACUERDO_PAGO B                                                           " & _
                                " 					  WHERE A.DOCUMENTO = B.DOCUMENTO AND                                                 " & _
                                " 							FECHA_CUOTA <= GETDATE()                                                      " & _
                                " 							GROUP BY A.DOCUMENTO,A.EXPEDIENTE,A.PLACA) B	                              " & _
                                " 			WHERE A.EFIGEN = B.PLACA) B ,                                                                 " & _
                                " 	 (SELECT * FROM MAESTRO_ACUERDOS) c                                                                   " & _
                                " WHERE A.PagCtb = B.EFIGEN COLLATE DATABASE_DEFAULT  AND                                                 " & _
                                " B.DOCUMENTO = C.DOCUMENTO AND                                                                           " & _
                                " B.EXPEDIENTE = @expediente AND                                                                          " & _
                                " (A.PagFec >= C.FECHA_INICIO and A.PagFec <= GETDATE())                                                  " & _
                                " GROUP BY b.VIG_FINAL,b.VIG_INICIAL,b.PAGO_ACUERDO,C.TOTAL_DEUDA                                         "

            Using Command As New System.Data.SqlClient.SqlCommand(sql, connection)
                Command.CommandTimeout = 60000
                Command.Parameters.AddWithValue("@expediente", expediente)
                Dim Adap As New SqlDataAdapter(Command)
                Dim Table As New DataTable
                Adap.Fill(Table)
                If Table.Rows.Count > 0 Then
                    DtgAcuerdos.DataSource = Table
                    DtgAcuerdos.DataBind()
                End If
            End Using
        End Using
    End Sub

    Protected Sub gtnAcuerdopago_Click(ByVal sender As Object, ByVal e As EventArgs) Handles gtnAcuerdopago.Click
        Response.Redirect("acuerdopago.aspx")
    End Sub

End Class
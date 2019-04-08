Imports System.Data.SqlClient
Partial Public Class EditEMBARGOS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            LoadcboEstadoEm()
            LoadCboResolucion()

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from EMBARGOS where [NroResolEm] = @NroResolEm"
                '                 
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@NroResolEm", Request("ID"))
                'Declare a SqlDataReader Ojbect                
                Dim Reader As SqlDataReader = Command.ExecuteReader
                'If at least one record was found
                If Reader.Read Then
                    txtNroResolEm.Text = Reader("NroResolEm").ToString()
                    txtFecResolEm.Text = Left(Reader("FecResolEm").ToString().Trim, 10)
                    txtLimiteEm.Text = Reader("LimiteEm").ToString()
                    cboEstadoEm.SelectedValue = Reader("EstadoEm").ToString()
                    txtPorcentajeEmbargo.Text = Reader("porcentaje").ToString
                    txtObservaciones.Text = Reader("Observaciones").ToString()
                End If
                Reader.Close()
                Connection.Close()
            Else
                'Since this is an insert then you can't delete it yet because it's not in the database.
                cmdAddDetalle.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdSave.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar deudores"
                CustomValidator1.IsValid = False

                'Deshabilitar controles
                txtNroResolEm.Enabled = False
                txtFecResolEm.Enabled = False
                txtLimiteEm.Enabled = False
                cboEstadoEm.Enabled = False
            End If

        End If
    End Sub

    Protected Sub LoadCboResolucion()
        'CboResolucion
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "select codigo, nombre from TIPO_RESOLUCION_EMBARGO order by codigo"
        Dim Command As New SqlCommand(sql, Connection)
        CboResolucion.DataTextField = "nombre"
        CboResolucion.DataValueField = "codigo"
        CboResolucion.DataSource = Command.ExecuteReader()
        CboResolucion.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboEstadoEm()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "select codigo, nombre from [ESTADOS_EMBARGO] order by codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboEstadoEm.DataTextField = "nombre"
        cboEstadoEm.DataValueField = "codigo"
        cboEstadoEm.DataSource = Command.ExecuteReader()
        cboEstadoEm.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand

        'Comandos SQL 
        Dim InsertSQL As String = "Insert into EMBARGOS (NroExp, NroResolEm, FecResolEm, LimiteEm, EstadoEm,porcentaje,Observaciones ) VALUES ( @NroExp, @NroResolEm, @FecResolEm, @LimiteEm, @EstadoEm,@porcentaje,@Observaciones) "
        Dim UpdateSQL As String = "Update EMBARGOS set NroExp = @NroExp, FecResolEm = @FecResolEm, LimiteEm = @LimiteEm, EstadoEm = @EstadoEm , porcentaje =@porcentaje, Observaciones =@Observaciones where NroResolEm = @NroResolEm "

        'if ID > 0 run the update 
        'if ID = 0 run the Insert
        If String.IsNullOrEmpty(ID) Then
            'Insert  
            ID = txtNroResolEm.Text.Trim
            Command = New SqlCommand(InsertSQL, Connection)
            Command.Parameters.AddWithValue("@NroResolEm", ID)
        Else
            'update             
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@NroResolEm", ID)
        End If

        'Parametros
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente").Trim)

        If IsDate(Left(txtFecResolEm.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@FecResolEm", Left(txtFecResolEm.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@FecResolEm", DBNull.Value)
        End If

        If IsNumeric(txtLimiteEm.Text) Then
            Command.Parameters.AddWithValue("@LimiteEm", txtLimiteEm.Text)
        Else
            Command.Parameters.AddWithValue("@LimiteEm", DBNull.Value)
        End If

        If cboEstadoEm.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@EstadoEm", cboEstadoEm.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@EstadoEm", DBNull.Value)
        End If

        If IsNumeric(txtPorcentajeEmbargo.Text) Then
            Command.Parameters.AddWithValue("@porcentaje", txtPorcentajeEmbargo.Text)
        Else
            Command.Parameters.AddWithValue("@porcentaje", DBNull.Value)
        End If

        If txtObservaciones.Text <> "" Then
            Command.Parameters.AddWithValue("@Observaciones", txtObservaciones.Text)
        Else
            Command.Parameters.AddWithValue("@Observaciones", DBNull.Value)
        End If
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de embargos", "No. resolución de embargo " & ID, Command)

            Connection.Close()

            Dim pExpediente As String
            pExpediente = Request("pExpediente")
            Response.Redirect("EMBARGOS.aspx?pExpediente=" & pExpediente)

        Catch ex As Exception
            Dim MsgError As String
            MsgError = ex.Message
            CustomValidator1.Text = MsgError
            CustomValidator1.IsValid = False

        End Try
        
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Dim pExpediente As String
        pExpediente = Request("pExpediente")
        Response.Redirect("EMBARGOS.aspx?pExpediente=" & pExpediente)
    End Sub

    Protected Sub cmdAddDetalle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddDetalle.Click
        Response.Redirect("DETALLE_EMBARGO.aspx?pResolEm=" & Request("ID").Trim & "&pExpediente=" & Request("pExpediente").Trim)
    End Sub



    Protected Sub CboResolucion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CboResolucion.SelectedIndexChanged
        Dim tiporesolucion As String = CboResolucion.SelectedItem.Value.ToString.ToUpper.ToString

        Select Case tiporesolucion
            Case ""

        End Select

    End Sub    

    'Private Sub cboEstadoEm_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEstadoEm.SelectedIndexChanged
    '    Dim tiporesolucion As String = cboEstadoEm.SelectedItem.Value.ToString.ToUpper.ToString

    '    Select Case tiporesolucion
    '        Case "NO REGISTRADO"
    '            lblObservaciones.Visible = True
    '            txtObservaciones.Visible = True
    '            lblObservaciones.Text = ""
    '            txtPorcentajeEmbargo.Text = ""
    '        Case Else
    '            lblObservaciones.Visible = False
    '            txtObservaciones.Visible = False
    '            lblObservaciones.Text = ""
    '            txtPorcentajeEmbargo.Text = ""


    '    End Select
    'End Sub
End Class

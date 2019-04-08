Imports System.Data.SqlClient
Partial Public Class EditMANDAMIENTOS_PAGO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            LoadcboMedioPub()

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            '             
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM MANDAMIENTOS_PAGO where NroResolMP = @NroResolMP AND NroExp = @NroExp"

                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@NroResolMP", Request("ID"))
                Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
                '                 
                Dim Reader As SqlDataReader = Command.ExecuteReader
                '                 
                If Reader.Read Then
                    txtNroResolMP.Text = Reader("NroResolMP").ToString()
                    txtFecResolMP.Text = Left(Reader("FecResolMP").ToString().Trim, 10)
                    'txtFecEnvCitaCAD.Text = Left(Reader("FecEnvCitaCAD").ToString().Trim, 10)
                    txtNroOfiCita.Text = Reader("NroOfiCita").ToString()
                    txtFecOfiCita.Text = Left(Reader("FecOfiCita").ToString().Trim, 10)
                    'txtFecEnvCitaEj.Text = Left(Reader("FecEnvCitaEj").ToString().Trim, 10)
                    txtFecReciCita.Text = Left(Reader("FecReciCita").ToString().Trim, 10)
                    txtFecNotiPers.Text = Left(Reader("FecNotiPers").ToString().Trim, 10)
                    txtNroActaNotPer.Text = Reader("NroActaNotPer").ToString()
                    txtNroOfiNotCor.Text = Reader("NroOfiNotCor").ToString()
                    txtFecOfiNotCor.Text = Left(Reader("FecOfiNotCor").ToString().Trim, 10)
                    txtFecNotiCor.Text = Left(Reader("FecNotiCor").ToString().Trim, 10)
                    txtFecFijAviWeb.Text = Left(Reader("FecFijAviWeb").ToString().Trim, 10)
                    txtFecDesAviWeb.Text = Left(Reader("FecDesAviWeb").ToString().Trim, 10)
                    txtFecNotAvi.Text = Left(Reader("FecNotAvi").ToString().Trim, 10)
                    txtFecPubPrensa.Text = Left(Reader("FecPubPrensa").ToString(), 10)
                    'txtMedioPub.Text = Reader("MedioPub").ToString()
                    If Reader("MedioPub").ToString.Trim = "" Then
                        cboMedioPub.SelectedValue = "0" 'SIN DATOS
                    Else
                        cboMedioPub.SelectedValue = Reader("MedioPub").ToString()
                    End If

                    txtFecCondConc.Text = Left(Reader("FecCondConc").ToString().Trim, 10)
                End If
                '                 
                Reader.Close()
                Connection.Close()
            Else
                ' 
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdSaveMP.Visible = False
                CustomValidator2.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar datos"
                CustomValidator2.IsValid = False

                'Deshabilitar controles del mandamiento de pago
                'txtNroResolMP.Enabled = False
                'txtFecResolMP.Enabled = False
                'txtFecEnvCitaCAD.Enabled = False
                txtNroOfiCita.Enabled = False
                txtFecOfiCita.Enabled = False
                'txtFecEnvCitaEj.Enabled = False
                txtFecReciCita.Enabled = False
                txtFecNotiPers.Enabled = False
                txtNroActaNotPer.Enabled = False
                txtNroOfiNotCor.Enabled = False
                txtFecOfiNotCor.Enabled = False
                txtFecNotiCor.Enabled = False
                txtFecFijAviWeb.Enabled = False
                txtFecDesAviWeb.Enabled = False
                txtFecNotAvi.Enabled = False
                txtFecPubPrensa.Enabled = False
                'txtMedioPub.Enabled = False
                cboMedioPub.Enabled = False
                txtFecCondConc.Enabled = False
            End If

            ' El número y la fecha del mandamiento de pago siempre deben estar deshabilitados
            txtNroResolMP.Enabled = False
            txtFecResolMP.Enabled = False

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion            
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    cmdSaveMP.Visible = False
                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False
                End If

            End If

        End If
    End Sub

    Private Sub LoadcboMedioPub()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT codigo, nombre FROM MEDIOS_PUBLICACION ORDER BY codigo"
        Dim Command As New SqlCommand(sql, Connection)
        cboMedioPub.DataTextField = "nombre"
        cboMedioPub.DataValueField = "codigo"
        cboMedioPub.DataSource = Command.ExecuteReader()
        cboMedioPub.DataBind()
        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Overloads Sub cmdSaveMP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveMP.Click

        'Validacion. El MP DEBE tener como min Nro y fecha
        If txtNroResolMP.Text.Trim = "" Or txtFecResolMP.Text.Trim = "" Then
            CustomValidator1.Text = "El número de resolución del Mandamiento de pago y su respectiva fecha son datos obligatorios!"
            CustomValidator1.IsValid = False
        Else
            CustomValidator1.Text = ""
            CustomValidator1.IsValid = True
            '--------------------------------------------------------
            Dim ID As String = Request("ID") 'NroResolMP
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()

            Dim Command As SqlCommand

            Dim InsertSQL As String = "Insert into MANDAMIENTOS_PAGO ([NroResolMP], [FecResolMP], [NroExp], [NroOfiCita], [FecOfiCita], [FecReciCita], [FecNotiPers], [NroActaNotPer], [NroOfiNotCor], [FecOfiNotCor], [FecNotiCor], [FecFijAviWeb], [FecDesAviWeb], [FecNotAvi], [FecPubPrensa], [MedioPub], [FecCondConc] ) VALUES ( @NroResolMP, @FecResolMP, @NroExp, @NroOfiCita, @FecOfiCita, @FecReciCita, @FecNotiPers, @NroActaNotPer, @NroOfiNotCor, @FecOfiNotCor, @FecNotiCor, @FecFijAviWeb, @FecDesAviWeb, @FecNotAvi, @FecPubPrensa, @MedioPub, @FecCondConc ) "
            Dim UpdateSQL As String = "Update MANDAMIENTOS_PAGO SET [FecResolMP] = @FecResolMP, [NroExp] = @NroExp, [NroOfiCita] = @NroOfiCita, [FecOfiCita] = @FecOfiCita, [FecReciCita] = @FecReciCita, [FecNotiPers] = @FecNotiPers, [NroActaNotPer] = @NroActaNotPer, [NroOfiNotCor] = @NroOfiNotCor, [FecOfiNotCor] = @FecOfiNotCor, [FecNotiCor] = @FecNotiCor, [FecFijAviWeb] = @FecFijAviWeb, [FecDesAviWeb] = @FecDesAviWeb, [FecNotAvi] = @FecNotAvi, [FecPubPrensa] = @FecPubPrensa, [MedioPub] = @MedioPub, [FecCondConc] = @FecCondConc where [NroResolMP] = @NroResolMP "

            If String.IsNullOrEmpty(ID) Then
                'insert 
                Command = New SqlCommand(InsertSQL, Connection)
                ID = txtNroResolMP.Text.Trim
                Command.Parameters.AddWithValue("@NroResolMP", ID)
            Else
                'Set the command object with the update sql and connection. 
                Command = New SqlCommand(UpdateSQL, Connection)                
                Command.Parameters.AddWithValue("@NroResolMP", ID)
            End If

            If IsDate(Left(txtFecResolMP.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecResolMP", Left(txtFecResolMP.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecResolMP", DBNull.Value)
            End If

            'Command.Parameters.AddWithValue("@NroExp", txtNroExp.Text)
            Dim pExpediente As String
            pExpediente = Request("pExpediente")
            Command.Parameters.AddWithValue("@NroExp", pExpediente)


            'If IsDate(Left(txtFecEnvCitaCAD.Text.Trim, 10)) Then
            '    Command.Parameters.AddWithValue("@FecEnvCitaCAD", Left(txtFecEnvCitaCAD.Text.Trim, 10))
            'Else
            '    Command.Parameters.AddWithValue("@FecEnvCitaCAD", DBNull.Value)
            'End If

            Command.Parameters.AddWithValue("@NroOfiCita", txtNroOfiCita.Text)

            If IsDate(Left(txtFecOfiCita.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecOfiCita", Left(txtFecOfiCita.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecOfiCita", DBNull.Value)
            End If

            'If IsDate(Left(txtFecEnvCitaEj.Text.Trim, 10)) Then
            '    Command.Parameters.AddWithValue("@FecEnvCitaEj", Left(txtFecEnvCitaEj.Text.Trim, 10))
            'Else
            '    Command.Parameters.AddWithValue("@FecEnvCitaEj", DBNull.Value)
            'End If

            If IsDate(Left(txtFecReciCita.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecReciCita", Left(txtFecReciCita.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecReciCita", DBNull.Value)
            End If

            If IsDate(Left(txtFecNotiPers.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecNotiPers", Left(txtFecNotiPers.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecNotiPers", DBNull.Value)
            End If

            Command.Parameters.AddWithValue("@NroActaNotPer", txtNroActaNotPer.Text.Trim)
            Command.Parameters.AddWithValue("@NroOfiNotCor", txtNroOfiNotCor.Text.Trim)

            If IsDate(Left(txtFecOfiNotCor.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecOfiNotCor", Left(txtFecOfiNotCor.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecOfiNotCor", DBNull.Value)
            End If

            If IsDate(Left(txtFecNotiCor.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecNotiCor", Left(txtFecNotiCor.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecNotiCor", DBNull.Value)
            End If

            If IsDate(Left(txtFecFijAviWeb.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecFijAviWeb", Left(txtFecFijAviWeb.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecFijAviWeb", DBNull.Value)
            End If

            If IsDate(Left(txtFecDesAviWeb.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecDesAviWeb", Left(txtFecDesAviWeb.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecDesAviWeb", DBNull.Value)
            End If

            If IsDate(Left(txtFecNotAvi.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecNotAvi", Left(txtFecNotAvi.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecNotAvi", DBNull.Value)
            End If

            If IsDate(Left(txtFecPubPrensa.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecPubPrensa", Left(txtFecPubPrensa.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecPubPrensa", DBNull.Value)
            End If

            'Command.Parameters.AddWithValue("@MedioPub", txtMedioPub.Text.Trim)
            If cboMedioPub.SelectedValue.Length > 0 Then
                Command.Parameters.AddWithValue("@MedioPub", cboMedioPub.SelectedValue.Trim)
            Else
                Command.Parameters.AddWithValue("@MedioPub", DBNull.Value)
            End If

            If IsDate(Left(txtFecCondConc.Text.Trim, 10)) Then
                Command.Parameters.AddWithValue("@FecCondConc", Left(txtFecCondConc.Text.Trim, 10))
            Else
                Command.Parameters.AddWithValue("@FecCondConc", DBNull.Value)
            End If

            Try
                Command.ExecuteNonQuery()

                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Registro de Mandamientos de pago ", "No. resolución " & ID, Command)

            Catch ex As Exception

            End Try

            Connection.Close()
            Response.Redirect("MANDAMIENTOS_PAGO.aspx?pExpediente=" & Request("pExpediente").Trim)
        End If

        
    End Sub

    Protected Sub cmdCancelMP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancelMP.Click
        Response.Redirect("MANDAMIENTOS_PAGO.aspx?pExpediente=" & Request("pExpediente").Trim)
    End Sub

End Class
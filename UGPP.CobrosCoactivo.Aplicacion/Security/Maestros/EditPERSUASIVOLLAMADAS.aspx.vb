Imports System.Data.SqlClient

Partial Public Class EditPERSUASIVOLLAMADAS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Loadcbogestor()
            LoadcboTipificacion()

            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "SELECT * FROM PERSUASIVOLLAMADAS WHERE IdUnico = @IdUnico"
                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
                Dim Reader As SqlDataReader = Command.ExecuteReader
                If Reader.Read Then                    
                    txtFecha.Text = Left(Reader("Fecha").ToString().Trim, 10)
                    txtIdLlamada.Text = Reader("IdLlamada").ToString()
                    txtNoTelefono.Text = Reader("NoTelefono").ToString()
                    txtNombre.Text = Reader("Nombre").ToString()
                    cbogestor.SelectedValue = Reader("gestor").ToString()
                    txtInfoSolicitada.Text = Reader("InfoSolicitada").ToString()
                    txtInfoBrindada.Text = Reader("InfoBrindada").ToString()
                    cboTipificacion.Text = Reader("Tipificacion").ToString()
                    txtObservaciones.Text = Reader("Observaciones").ToString()
                End If
                Reader.Close()
                Connection.Close()

                If Not (Session("mnivelacces") = 1 Or Session("mnivelacces") = 3 Or Session("mnivelacces") = 8) Then
                    cmdDelete.Visible = False
                    cmdSave.Visible = False
                    txtFecha.Enabled = False
                    txtIdLlamada.Enabled = False
                    txtNoTelefono.Enabled = False
                    txtNombre.Enabled = False
                    cbogestor.Enabled = False
                    txtInfoSolicitada.Enabled = False
                    txtInfoBrindada.Enabled = False
                    cboTipificacion.Enabled = False
                    txtObservaciones.Enabled = False
                    imgBtnBorraFecha.Enabled = False
                    cboTipificacion.Enabled = False
                    '
                    CustomValidator1.Text = "Solo los revisores y gestores de información pueden editar este registro"
                    CustomValidator1.IsValid = False
                End If


            Else
                ' ADICIONAR
                ' Ocultar el boton borrar y colocar la fecha 
                cmdDelete.Visible = False
                txtFecha.Text = Left(Date.Now.ToString, 10)

                ' Dejar en el combo el usuario actual
                cbogestor.SelectedValue = Session("sscodigousuario")

            End If
            cbogestor.Enabled = False
        End If
    End Sub

    Protected Sub LoadcboTipificacion()
        'Llenar el combo de PERSUASIVOTIPIFICACION

        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM PERSUASIVOTIPIFICACION ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtPERSUASIVOTIPIFICACION As New DataTable
        Adaptador.Fill(dtPERSUASIVOTIPIFICACION)
        If dtPERSUASIVOTIPIFICACION.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaPERSUASIVOTIPIFICACION As DataRow = dtPERSUASIVOTIPIFICACION.NewRow()
            filaPERSUASIVOTIPIFICACION("codigo") = 0
            filaPERSUASIVOTIPIFICACION("nombre") = " "
            dtPERSUASIVOTIPIFICACION.Rows.Add(filaPERSUASIVOTIPIFICACION)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaPERSUASIVOTIPIFICACION As DataView = New DataView(dtPERSUASIVOTIPIFICACION)
            vistaPERSUASIVOTIPIFICACION.Sort = "codigo"
            '--------------------------------------------------------------------
            cboTipificacion.DataSource = vistaPERSUASIVOTIPIFICACION
            cboTipificacion.DataTextField = "nombre"
            cboTipificacion.DataValueField = "codigo"
            cboTipificacion.DataBind()
        End If
    End Sub

    Protected Sub Loadcbogestor()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [USUARIOS] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cbogestor.DataTextField = "nombre"
        'cbogestor.DataValueField = "codigo"
        'cbogestor.DataSource = Command.ExecuteReader()
        'cbogestor.DataBind()
        'Connection.Close()


        'Llenar el combo de usuarios
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM usuarios ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtUSUARIOS As New DataTable
        Adaptador.Fill(dtUSUARIOS)
        If dtUSUARIOS.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaUSUARIOS As DataRow = dtUSUARIOS.NewRow()
            filaUSUARIOS("codigo") = 0
            filaUSUARIOS("nombre") = " "
            dtUSUARIOS.Rows.Add(filaUSUARIOS)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaUSUARIOS As DataView = New DataView(dtUSUARIOS)
            vistaUSUARIOS.Sort = "codigo"
            '--------------------------------------------------------------------
            cbogestor.DataSource = vistaUSUARIOS
            cbogestor.DataTextField = "nombre"
            cbogestor.DataValueField = "codigo"
            cbogestor.DataBind()
        End If

    End Sub


    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "delete from PERSUASIVOLLAMADAS where IdUnico = @IdUnico"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@IdUnico", Request("ID"))
        Command.ExecuteNonQuery()

        Connection.Close()
        Response.Redirect("PERSUASIVOLLAMADAS.aspx")
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim ID As String = Request("ID")

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        Dim InsertSQL As String = "INSERT INTO PERSUASIVOLLAMADAS (NroExp, Fecha, IdLlamada, NoTelefono, Nombre, gestor, InfoSolicitada, InfoBrindada, Tipificacion, Observaciones) VALUES (@NroExp, @Fecha, @IdLlamada, @NoTelefono, @Nombre, @gestor, @InfoSolicitada, @InfoBrindada, @Tipificacion, @Observaciones)"
        Dim UpdateSQL As String = "UPDATE PERSUASIVOLLAMADAS SET NroExp = @NroExp, [Fecha] = @Fecha, IdLlamada = @IdLlamada, NoTelefono = @NoTelefono, Nombre = @Nombre, gestor = @gestor, InfoSolicitada = @InfoSolicitada, InfoBrindada = @InfoBrindada, Tipificacion = @Tipificacion, Observaciones = @Observaciones WHERE IdUnico = @IdUnico"

        If String.IsNullOrEmpty(ID) Then
            Command = New SqlCommand(InsertSQL, Connection)
        Else
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@IdUnico", ID)
        End If
        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))

        If IsDate(Left(txtFecha.Text.Trim, 10)) Then
            Command.Parameters.AddWithValue("@Fecha", Left(txtFecha.Text.Trim, 10))
        Else
            Command.Parameters.AddWithValue("@Fecha", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@IdLlamada", txtIdLlamada.Text)
        Command.Parameters.AddWithValue("@NoTelefono", txtNoTelefono.Text)
        Command.Parameters.AddWithValue("@Nombre", txtNombre.Text)

        If cbogestor.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@gestor", cbogestor.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@gestor", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@InfoSolicitada", txtInfoSolicitada.Text)
        Command.Parameters.AddWithValue("@InfoBrindada", txtInfoBrindada.Text)

        If cboTipificacion.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@Tipificacion", cboTipificacion.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@Tipificacion", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@Observaciones", txtObservaciones.Text)

        Try
            Command.ExecuteNonQuery()

            'Log de auditoría
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Gestión de llamadas", "No. Expediente " & Request("pExpediente").Trim, Command)

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Connection.Close()
            Return

        End Try

        Connection.Close()
        Response.Redirect("PERSUASIVOLLAMADAS.aspx?pExpediente=" & Request("pExpediente"))

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("PERSUASIVOLLAMADAS.aspx?pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub imgBtnBorraFecha_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBorraFecha.Click
        txtFecha.Text = ""
    End Sub
End Class
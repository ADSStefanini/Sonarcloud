Imports System.Data.SqlClient

Partial Public Class SolicitudCambioEstadoMasivo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))
            Dim MTG As New MetodosGlobalesCobro
            lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))

            CargarListaExp()

            'Cargar combos
            Loadcboestado()
            Loadcboestadosol()        

        End If
    End Sub

    'Protected Sub CheckHeader_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckHeader.CheckedChanged
    '    For i As Integer = 0 To ListExpedientes.Items.Count - 1
    '        ListExpedientes.Items(i).Selected = CheckHeader.Checked
    '    Next
    'End Sub

    Private Sub CargarListaExp()
        If Not Session("Dtexp") Is Nothing Then
            ListExpedientes.DataSource = Nothing
            ListExpedientes.Items.Clear()

            Dim Tb As New DataTable
            Tb = Session("Dtexp")

            ListExpedientes.DataSource = Tb 'Asignar el DataSource al combobox  
            ListExpedientes.DataValueField = Tb.Columns(0).ColumnName  'Campo a capturar
            ListExpedientes.DataTextField = Tb.Columns(1).ColumnName 'Campo a mostar
            ListExpedientes.DataBind()
            For i As Integer = 0 To ListExpedientes.Items.Count - 1
                ListExpedientes.Items(i).Selected = True
            Next
            ListExpedientes.Enabled = False
        End If
    End Sub

    Protected Sub Loadcboestado()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM estados_proceso WHERE nombre <> '" & NomEstadoProceso.Trim & "' AND nombre <> 'SIN DATOS' ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstados_Proceso As New DataTable
        Adaptador.Fill(dtEstados_Proceso)
        If dtEstados_Proceso.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim filaEstado As DataRow = dtEstados_Proceso.NewRow()
            filaEstado("codigo") = " "
            filaEstado("nombre") = "SELECCIONE ESTADO..."
            dtEstados_Proceso.Rows.Add(filaEstado)
            '- Crear un dataview para ordenar los valore y "00" quede de primero
            Dim vistaEstados_Proceso As DataView = New DataView(dtEstados_Proceso)
            vistaEstados_Proceso.Sort = "codigo"
            '--------------------------------------------------------------------

            cboestado.DataSource = vistaEstados_Proceso
            cboestado.DataTextField = "nombre"
            cboestado.DataValueField = "codigo"
            cboestado.DataBind()
        End If
    End Sub

    Protected Sub Loadcboestadosol()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [ESTADOS_SOL_CAM_EST] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboestadosol.DataTextField = "nombre"
        cboestadosol.DataValueField = "codigo"
        cboestadosol.DataSource = Command.ExecuteReader()
        cboestadosol.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        Dim NroExp As String = ""

        If cboestado.SelectedValue = " " Then
            CustomValidator1.Text = "Seleccione el estado al que desea trasladar el expediente por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        If taObservaciones.InnerHtml = "" Then
            CustomValidator1.Text = "Digite las observaciones del caso por favor"
            CustomValidator1.IsValid = False
            Return
        End If

        Dim dtProcesosCambioEstado As New DataTable
        dtProcesosCambioEstado = Session("Dtexp")

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        Dim FechaMasivo As Date = DateTime.Now
        Dim grupo As String = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds).ToString
        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO SOLICITUDES_CAMBIOESTADO (NroExp, abogado, fecha, estadoactual, estadosol, estado, observac, grupo) " & _
                                        "VALUES (@NroExp, @abogado, @fecha, @estadoactual, @estadosol, @estado, @observac, @grupo) "
        Command = New SqlCommand(InsertSQL, Connection)
        'Parametros
        Dim pExpediente As SqlParameter = New SqlParameter("@NroExp", SqlDbType.VarChar)
        Dim pAbogado As SqlParameter = New SqlParameter("@abogado", SqlDbType.VarChar)
        Dim pFecha As SqlParameter = New SqlParameter("@fecha", SqlDbType.Date)
        Dim pEstadoActual As SqlParameter = New SqlParameter("@estadoactual", SqlDbType.VarChar)
        Dim pEstadoSol As SqlParameter = New SqlParameter("@estadosol", SqlDbType.Int)
        Dim pEstado As SqlParameter = New SqlParameter("@estado", SqlDbType.VarChar)
        Dim pObservac As SqlParameter = New SqlParameter("@observac", SqlDbType.VarChar)
        Dim pGrupo As SqlParameter = New SqlParameter("@grupo", SqlDbType.VarChar)
        '
        Command.Parameters.Add(pExpediente)
        Command.Parameters.Add(pAbogado)
        Command.Parameters.Add(pFecha)
        Command.Parameters.Add(pEstadoActual)
        Command.Parameters.Add(pEstadoSol)
        Command.Parameters.Add(pEstado)
        Command.Parameters.Add(pObservac)
        Command.Parameters.Add(pGrupo)

        For Each row As DataRow In dtProcesosCambioEstado.Rows
            NroExp = row.Item("exp").ToString.Trim
            '
            pExpediente.Value = NroExp
            pAbogado.Value = Session("sscodigousuario")
            pFecha.Value = FechaMasivo
            pEstadoActual.Value = Session("EstadoMasivo")
            pEstadoSol.Value = 1
            pEstado.Value = cboestado.SelectedValue
            pObservac.Value = taObservaciones.InnerHtml
            pGrupo.Value = grupo

            If Not ExisteSolicitudCambioEstado(NroExp, Session("sscodigousuario")) Then
                'Run the statement
                Command.ExecuteNonQuery()
            End If
            
        Next row

        'Close the Connection Object 
        Connection.Close()

        CustomValidator1.Text = "Solicitud de cambio de estado masiva exitosa"
        CustomValidator1.IsValid = False
        cmdSave.Enabled = False

    End Sub

    Private Function ExisteSolicitudCambioEstado(ByVal pExpediente As String, ByVal pAbogado As String) As Boolean
        Dim Respuesta As Boolean = False

        'Dim sw As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM SOLICITUDES_CAMBIOESTADO WHERE NroExp = '" & pExpediente & "' AND abogado = '" & pAbogado & "' AND estadosol = 1"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            Respuesta = True            
        End If
        Reader.Close()
        Connection.Close()
        '
        Return Respuesta
    End Function

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Response.Redirect("EJEFISGLOBAL_MASIVO.aspx")
    End Sub

    Private Sub CerrarSesion()
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        CerrarSesion()
        Response.Redirect("../../login.aspx")
    End Sub

    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("EJEFISGLOBAL.aspx")
    End Sub

End Class
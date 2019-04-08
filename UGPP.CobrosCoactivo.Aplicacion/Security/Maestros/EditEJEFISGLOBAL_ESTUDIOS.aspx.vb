Imports System.Data.SqlClient

Public Class EditEJEFISGLOBAL_ESTUDIOS1
    Inherits coactivosyp.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            Dim msg As String = ""

            '09/abril/2015
            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim idGestorResp As String
            idGestorResp = MTG.GetIDGestorResp(Request("ID"))

            lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)

            ' Combos de la pestaña de clasificacion
            LoadcboTitEjecAntig()
            LoadcboCaducidad()
            LoadcboEstadoPersona()
            LoadcboProcesoCurso()
            LoadcboAcuerdoPago()

            If Len(Request("ID")) > 0 Then
                'Conexion a la base de datos
                Dim Connection As New SqlConnection(Funciones.CadenaConexion)
                Connection.Open()

                Dim sql As String = "SELECT EJEFISGLOBAL.*, ESTADOS_PROCESO.nombre AS NomEstadoProc, ISNULL (DATEDIFF(DAY, EFIFECENTGES, GETDATE()), 0) As ObliFechaPresentacion " &
                                    "FROM EJEFISGLOBAL " &
                                    "LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo " &
                                    "WHERE EJEFISGLOBAL.EFINROEXP = @EFINROEXP"


                Dim Command As New SqlCommand(sql, Connection)
                Command.Parameters.AddWithValue("@EFINROEXP", Request("ID"))
                Dim Reader As SqlDataReader = Command.ExecuteReader

                'Si se encontro el expediente
                If Reader.Read Then

                    'Mostrar informacion general del expediente
                    txtNroExp.Text = Reader("EFINROEXP").ToString()


                    'Mostrar datos de la ficha "Recepcion Titulo"
                    txtEFIFECHAEXP.Text = Left(Reader("EFIFECHAEXP").ToString().Trim, 10)
                    txtEFINUMMEMO.Text = Reader("EFINUMMEMO").ToString()
                    txtEFIEXPORIGEN.Text = Reader("EFIEXPORIGEN").ToString()
                    txtEFIFECCAD.Text = Left(Reader("EFIFECCAD").ToString().Trim, 10)
                    txtEFIEXPDOCUMENTIC.Text = Reader("EFIEXPDOCUMENTIC").ToString()
                End If


                'Mostrar clasificacion de cartera
                MostrarClasificacionCartera(txtNroExp.Text.Trim)

            End If

            'Cargar grid de cambios de estado
            BindGridCambioEstado()
        End If
    End Sub

    Private Sub MostrarClasificacionCartera(ByVal pNumExpediente As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM [CLASIF_CARTERA_REGISTRO] WHERE NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@NroExp", pNumExpediente)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            'cboRepartidor.SelectedValue = RepartidorActual
            cboTitEjecAntig.SelectedValue = Reader("TitEjecAntig").ToString()
            cboCaducidad.SelectedValue = Reader("Caducidad").ToString()
            cboEstadoPersona.SelectedValue = Reader("EstadoPersona").ToString()
            cboProcesoCurso.SelectedValue = Reader("ProcesoCurso").ToString()
            cboAcuerdoPago.SelectedValue = Reader("AcuerdoPago").ToString()
            txtExtadoInicial.Text = Reader("EstadoInicial").ToString()
        End If
        'Close the Data Reader we are done with it.
        Reader.Close()
        Connection.Close()
    End Sub

    'Combos relacionados con la clasificacion
    Private Sub LoadcboTitEjecAntig()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'TIPO PERSONA' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtTitAnt As New DataTable
        Adaptador.Fill(dtTitAnt)
        If dtTitAnt.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtTitAnt.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtTitAnt.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTitAnt As DataView = New DataView(dtTitAnt)
            vistaTitAnt.Sort = "codigo"
            'Enlazar combo 
            cboTitEjecAntig.DataSource = vistaTitAnt
            cboTitEjecAntig.DataTextField = "nombre"
            cboTitEjecAntig.DataValueField = "codigo"
            cboTitEjecAntig.DataBind()
        End If

    End Sub
    '
    Private Sub LoadcboCaducidad()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'CADUCIDAD' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtCaducidad As New DataTable
        Adaptador.Fill(dtCaducidad)
        If dtCaducidad.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtCaducidad.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtCaducidad.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaCaducidad As DataView = New DataView(dtCaducidad)
            vistaCaducidad.Sort = "codigo"
            'Enlazar combo 
            cboCaducidad.DataSource = vistaCaducidad
            cboCaducidad.DataTextField = "nombre"
            cboCaducidad.DataValueField = "codigo"
            cboCaducidad.DataBind()
        End If
    End Sub

    Private Sub LoadcboEstadoPersona()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'ESTADO PERSONA NATURAL O JURÍDICA' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtEstado As New DataTable
        Adaptador.Fill(dtEstado)
        If dtEstado.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtEstado.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtEstado.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaEstado As DataView = New DataView(dtEstado)
            vistaEstado.Sort = "codigo"
            'Enlazar combo 
            cboEstadoPersona.DataSource = vistaEstado
            cboEstadoPersona.DataTextField = "nombre"
            cboEstadoPersona.DataValueField = "codigo"
            cboEstadoPersona.DataBind()
        End If
    End Sub


    Private Sub LoadcboProcesoCurso()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'OTROS PROCESOS' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtProcesoCurso As New DataTable
        Adaptador.Fill(dtProcesoCurso)
        If dtProcesoCurso.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtProcesoCurso.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtProcesoCurso.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaProcesoCurso As DataView = New DataView(dtProcesoCurso)
            vistaProcesoCurso.Sort = "codigo"
            'Enlazar combo 
            cboProcesoCurso.DataSource = vistaProcesoCurso
            cboProcesoCurso.DataTextField = "nombre"
            cboProcesoCurso.DataValueField = "codigo"
            cboProcesoCurso.DataBind()
        End If
    End Sub

    Private Sub LoadcboAcuerdoPago()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT RTRIM(CONVERT(VARCHAR, idunico))+'V'+RTRIM(CONVERT(VARCHAR, valor)) AS codigo, concepto AS nombre FROM CLASIFICACION_CARTERA WHERE categoria = 'ACUERDOS DE PAGO' ORDER BY idunico"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtAcuerdos As New DataTable
        Adaptador.Fill(dtAcuerdos)
        If dtAcuerdos.Rows.Count > 0 Then
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dtAcuerdos.NewRow()
            fila("codigo") = ""
            fila("nombre") = ""
            dtAcuerdos.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaAcuerdos As DataView = New DataView(dtAcuerdos)
            vistaAcuerdos.Sort = "codigo"
            'Enlazar combo 
            cboAcuerdoPago.DataSource = vistaAcuerdos
            cboAcuerdoPago.DataTextField = "nombre"
            cboAcuerdoPago.DataValueField = "codigo"
            cboAcuerdoPago.DataBind()
        End If
    End Sub

    Private Sub BindGridCambioEstado()

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT CAMBIOS_ESTADO.*, USUARIOSrepartidor.nombre as USUARIOSrepartidornombre," &
             "USUARIOSabogado.nombre as USUARIOSabogadonombre, ESTADOS_PROCESOestado.nombre as ESTADOS_PROCESOestadonombre," &
             "ESTADOS_PAGOestadopago.nombre as ESTADOS_PAGOestadopagonombre " &
             "FROM CAMBIOS_ESTADO " &
              "left join USUARIOS USUARIOSrepartidor on CAMBIOS_ESTADO.repartidor = USUARIOSrepartidor.codigo " &
              "left join USUARIOS USUARIOSabogado on CAMBIOS_ESTADO.abogado = USUARIOSabogado.codigo " &
              "left join ESTADOS_PROCESO ESTADOS_PROCESOestado on CAMBIOS_ESTADO.estado = ESTADOS_PROCESOestado.codigo " &
              "left join ESTADOS_PAGO ESTADOS_PAGOestadopago on CAMBIOS_ESTADO.estadopago = ESTADOS_PAGOestadopago.codigo " &
             "WHERE CAMBIOS_ESTADO.NroExp = '" & Request("ID") & "'"
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grdCambiosEstado.DataSource = Command.ExecuteReader()
        grdCambiosEstado.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grdCambiosEstado.Rows.Count
        Connection.Close()
    End Sub

    Protected Sub ABack_Click(sender As Object, e As EventArgs) Handles ABack.Click
        CommonsCobrosCoactivos.llevarAListaExpedientes(Session, Response)
    End Sub

    Protected Sub A3_Click(sender As Object, e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        Session.RemoveAll()
    End Sub

    Protected Sub ABackRep_Click(sender As Object, e As EventArgs) Handles ABackRep.Click
        CommonsCobrosCoactivos.llevarAListaExpedientes(Session, Response)
    End Sub
End Class
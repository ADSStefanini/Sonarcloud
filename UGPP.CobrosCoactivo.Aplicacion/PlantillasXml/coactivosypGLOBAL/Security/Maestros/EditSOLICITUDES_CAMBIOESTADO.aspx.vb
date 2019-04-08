Imports System.Data.SqlClient

Partial Public Class EditSOLICITUDES_CAMBIOESTADO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            Loadcboestadosol()
            Loadcboestado()
            LoadcboAprob_Revisor()
            LoadcboAprob_Ejecutor()

            txtAbogado.Enabled = False
            txtRevisor.Enabled = False
            txtEjecutor.Enabled = False

            'Borrar las siguientes lineas. Esto es solo para mostrar DateTime.UtcNow
            'taObservaciones.InnerHtml = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds).ToString

            'Create a new connection to the database
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            'Opens a connection to the database.
            Connection.Open()
            ' 
            'if Request("ID") > 0 then this is an edit
            'if Request("ID") = 0 then this is an insert
            If Len(Request("ID")) > 0 Then
                Dim sql As String = "select * from SOLICITUDES_CAMBIOESTADO where idunico = @ID"
                ' 
                'Declare SQLCommand Object named Command
                'Create a new Command object with a select statement that will open the row referenced by Request("ID")
                Dim Command As New SqlCommand(sql, Connection)
                ' 
                ' 'Set the @idunico parameter in the Command select query
                Command.Parameters.AddWithValue("@ID", Request("ID"))
                ' 
                'Declare a SqlDataReader Ojbect
                'Load it with the Command's select statement
                Dim Reader As SqlDataReader = Command.ExecuteReader
                ' 
                'If at least one record was found
                If Reader.Read Then
                    'Datos que vienen de la tabla 
                    cboestadosol.SelectedValue = Reader("estadosol").ToString()
                    cboestado.SelectedValue = Reader("estado").ToString()
                    taObservaciones.InnerHtml = Reader("observac").ToString().Trim
                    '
                    cboAprob_Revisor.SelectedValue = Reader("aprob_revisor").ToString()
                    txtFecha_Aprob_Revisor.Text = Left(Reader("fecha_aprob_revisor").ToString(), 10)
                    taNota_Revisor.InnerHtml = Reader("nota_revisor").ToString().Trim
                    ' 
                    cboAprob_Ejecutor.SelectedValue = Reader("aprob_ejecutor").ToString()
                    txtFecha_Aprob_Supervisor.Text = Left(Reader("fecha_aprob_ejecutor").ToString(), 10)
                    taNota_Ejecutor.InnerHtml = Reader("nota_ejecutor").ToString().Trim

                    '24/sep/2014. Datos generados 'al vuelo'
                    Dim IdAbogado, NombreAbogado As String
                    IdAbogado = Reader("abogado").ToString().Trim
                    NombreAbogado = getNombreByID(IdAbogado)
                    txtAbogado.Text = NombreAbogado
                    '
                    Dim IdRevisor, NombreRevisor As String
                    IdRevisor = Reader("revisor").ToString().Trim
                    NombreRevisor = getNombreByID(IdRevisor)
                    txtRevisor.Text = NombreRevisor
                    '
                    Dim IdSupervisor, NombreSupervisor As String
                    IdSupervisor = Reader("ejecutor").ToString().Trim
                    NombreSupervisor = getNombreByID(IdSupervisor)
                    txtEjecutor.Text = NombreSupervisor

                    ' En base al nivel de escalamiento se muestran u ocultan los paneles
                    Dim nivel_escalamiento As Int16 = 1
                    nivel_escalamiento = Reader("nivel_escalamiento").ToString()
                    If nivel_escalamiento = 1 Then
                        panelSupervisor.Visible = False
                        'Si es perfil abogado => ocultar
                        If Session("mnivelacces") = 4 Or Session("mnivelacces") = 6 Or Session("mnivelacces") = 7 Then
                            panelRevisor.Visible = False
                            cmdSaveRevisor.Visible = False
                            panelSupervisor.Visible = False
                        Else
                            cmdImprimir.Visible = False
                        End If

                        cmdSaveSupervisor.Visible = False
                    Else
                        ' nivel_escalamiento = 2 o nivel_escalamiento = 3                        
                        taNota_Revisor.Attributes("readonly") = "readonly"
                        cboAprob_Revisor.Enabled = False
                        cmdSaveRevisor.Visible = False

                        If Session("mnivelacces") = 2 Then
                            panelSupervisor.Visible = True                            
                        Else
                            panelSupervisor.Visible = False
                        End If

                        If nivel_escalamiento = 2 And Session("mnivelacces") = 2 Then
                            cmdSaveSupervisor.Visible = True
                        Else
                            cmdSaveSupervisor.Visible = False
                        End If

                        If nivel_escalamiento = 3 Then
                            cboAprob_Ejecutor.Enabled = False
                            txtFecha_Aprob_Supervisor.Enabled = False                            
                            taNota_Ejecutor.Attributes("readonly") = "readonly"
                            panelSupervisor.Visible = True
                        End If

                    End If

                End If
                ' Cerrar objetos                
                Reader.Close()
                Connection.Close()
                '     
                cmdSave.Visible = False
                cboestado.Enabled = False                
                taObservaciones.Attributes("readonly") = "readonly"

            Else
                ' Si llega aca es porque es una solicitud nueva => Ocultar panel de supervisor y gestor
                txtAbogado.Visible = False
                lblGestor.Visible = False
                panelRevisor.Visible = False
                panelSupervisor.Visible = False
                cmdSaveSupervisor.Visible = False

                cmdSave.Visible = True
                cmdImprimir.Visible = False
                cmdSaveRevisor.Visible = False
            End If            

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            Dim MTG As New MetodosGlobalesCobro
            Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
            If idGestorResp <> Session("sscodigousuario") Then
                If Session("mnivelacces") <> 8 Then
                    cmdSave.Visible = False

                    ' Si es revisor o supervisor habilitar los campos que puede editar para el cambio de estado
                    If Session("mnivelacces") = 3 Or Session("mnivelacces") = 2 Then

                    Else
                        CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                        CustomValidator1.IsValid = False

                    End If

                End If
            End If
        End If
    End Sub


    Protected Sub LoadcboAprob_Ejecutor()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT * FROM ESTADOS_APROBACION_CAM_EST ORDER BY orden"
        Dim Command As New SqlCommand(sql, Connection)
        cboAprob_Ejecutor.DataTextField = "nombre"
        cboAprob_Ejecutor.DataValueField = "codigo"
        cboAprob_Ejecutor.DataSource = Command.ExecuteReader()
        cboAprob_Ejecutor.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboAprob_Revisor()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "SELECT * FROM ESTADOS_APROBACION_CAM_EST ORDER BY orden"
        Dim Command As New SqlCommand(sql, Connection)
        cboAprob_Revisor.DataTextField = "nombre"
        cboAprob_Revisor.DataValueField = "codigo"
        cboAprob_Revisor.DataSource = Command.ExecuteReader()
        cboAprob_Revisor.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub Loadcboestadosol()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from ESTADOS_SOL_CAM_EST order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboestadosol.DataTextField = "nombre"
        cboestadosol.DataValueField = "codigo"
        cboestadosol.DataSource = Command.ExecuteReader()
        cboestadosol.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub Loadcboestado()

        Dim cnx As String = Funciones.CadenaConexion
        'Dim cmd As String = "SELECT codigo, nombre FROM estados_proceso WHERE nombre <> '" & NomEstadoProceso.Trim & "' AND nombre <> 'SIN DATOS' ORDER BY nombre"
        ' 18/feb/2015. Johana Lima y Jayne Estupiñan solicitaron que aparezacn todos los estados.
        Dim cmd As String = "SELECT codigo, nombre FROM estados_proceso WHERE nombre <> 'SIN DATOS' ORDER BY nombre"

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


    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        'taObservaciones.InnerHtml = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds).ToString

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

        Dim revisor As String = ""
        revisor = getIdSuperior(Session("sscodigousuario"))
        Dim EstaActivo As Boolean = UsuarioEstaActivo(revisor)

        If revisor = "" Or Not EstaActivo Then
            CustomValidator1.Text = "El usuario actual NO tiene un revisor o supervisor asociado. Favor informar a administrador para que haga la asociación"
            CustomValidator1.IsValid = False
            Return
        End If

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand

        ' 
        'Declare string InsertSQL 
        Dim InsertSQL As String = "INSERT INTO solicitudes_cambioestado (NroExp, abogado, fecha, estadoactual, estadosol, estado, observac, revisor, ejecutor ) VALUES ( @NroExp, @abogado, @fecha, @estadoactual, @estadosol, @estado, @observac, @revisor, @ejecutor ) "
        Command = New SqlCommand(InsertSQL, Connection)

        Command.Parameters.AddWithValue("@NroExp", Request("pExpediente"))
        Command.Parameters.AddWithValue("@abogado", Session("sscodigousuario"))
        Command.Parameters.AddWithValue("@fecha", DateTime.Now)

        'estadoactual
        Dim MTG As New MetodosGlobalesCobro
        Dim EstadoExpediente As String = MTG.GetEstadoExpediente(Request("pExpediente"))
        Command.Parameters.AddWithValue("@estadoactual", EstadoExpediente)

        Command.Parameters.AddWithValue("@estadosol", 1) ' Estado solicitud 1 = pendiente

        If cboestado.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@estado", cboestado.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@estado", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@observac", taObservaciones.InnerHtml)

        '24/sep/2014. Obteniendo el revisor del gestor que lanza la solicitud
        'Session("sscodigousuario")
        'Dim revisor As String = ""
        'revisor = getIdSuperior(Session("sscodigousuario"))
        Command.Parameters.AddWithValue("@revisor", revisor)

        '02/act/2014
        Dim superior As String = ""
        superior = getIdSuperior(revisor)
        Command.Parameters.AddWithValue("@ejecutor", superior)

        'Run the statement
        Command.ExecuteNonQuery()

        'Close the Connection Object 
        Connection.Close()
        ' 
        'Go to the Summary page 
        Response.Redirect("SOLICITUDES_CAMBIOESTADO.aspx?pExpediente=" & Request("pExpediente"))

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'Si el perfil es revisor o supervisor => reenviar a SOLICITUDCAMBIOESTADO.aspx
        If Session("mnivelacces") = 3 Or Session("mnivelacces") = 2 Then
            Response.Redirect("SOLICITUDCAMBIOESTADO.aspx")
        Else
            If Session("mnivelacces") = 5 Then
                ' Si es un repartidor, devolver a EditEJEFISGLOBALREPARTIDOR.aspx?ID=
                Response.Redirect("EditEJEFISGLOBALREPARTIDOR.aspx?ID=" & Request("pExpediente"))
            Else
                Response.Redirect("SOLICITUDES_CAMBIOESTADO.aspx?pExpediente=" & Request("pExpediente"))
            End If

        End If

    End Sub

    Protected Sub cmdImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdImprimir.Click
        Dim w As New WordReport
        Dim op As Integer = 1
        Dim Tb As New Data.DataTable
        Dim fecReparto As String = ""
        Dim NomDeudor As String = ""
        Dim TipoTitulo As String = ""
        Dim TipoId As String = ""
        Dim ccNit As String = ""
        Dim DeudaActual As String = ""
        Dim Observaciones As String = taObservaciones.InnerHtml
        Dim Revisor As String = ""
        Dim NomRevisor As String = ""
        Dim NomEstadoInicial As String = ""
        Dim NomEstadoFinal As String = ""
        Dim NRec As Integer = 1

        'Estado inicial y final
        GetEstadosExpediente(Request("pExpediente"), NomEstadoInicial, NomEstadoFinal)
        '*
        Dim MTG As New MetodosGlobalesCobro
        Revisor = MTG.GetSuperior(Session("sscodigousuario"), NomRevisor)

        'Fecha de reparto del expediente
        fecReparto = GetDatosExpediente(Request("pExpediente"), NomDeudor, TipoTitulo, TipoId, ccNit, DeudaActual)

        ' Estructura
        Tb.Columns.Add("Fecha_Actual")
        Tb.Columns.Add("Proyecto")
        Tb.Columns.Add("FecReparto")
        Tb.Columns.Add("MAN_EXPEDIENTE")
        Tb.Columns.Add("ED_Nombre")
        Tb.Columns.Add("NomTipoTitulo")
        Tb.Columns.Add("TipoIdentificacion")
        Tb.Columns.Add("ED_Codigo_Nit")
        Tb.Columns.Add("DeudaActual")
        Tb.Columns.Add("Observaciones")
        Tb.Columns.Add("Revisor")
        Tb.Columns.Add("ESTADOINICIAL")
        Tb.Columns.Add("ESTADOFINAL")
        Tb.Columns.Add("NRec")
        '

        ' Registros
        Tb.Rows.Add(New Object() {Today.ToString("dd 'de' MMMM 'de' yyy"), _
                                  Session("ssnombreusuario"), _
                                  fecReparto, _
                                  Request("pExpediente"), _
                                  NomDeudor, _
                                  TipoTitulo, _
                                  TipoId, _
                                  ccNit, _
                                  DeudaActual, _
                                  Observaciones, _
                                  NomRevisor, _
                                  NomEstadoInicial, _
                                  NomEstadoFinal, _
                                  NRec _
                                  })

        ' Headers
        Response.ContentType = "application/msword"
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", "SolicitudCambioEstadoIndividual"))

        ' Aca llama a la funcion de combinacion "CreateReport"
        Response.Write(w.CreateReport(Tb, "acta-entrega-interna-expedientes_cambio-estado-generico.xml"))
        Response.End()

    End Sub


    Private Sub GetEstadosExpediente(ByVal pExpediente As String, ByRef pNomEstadoIni As String, ByRef pNomEstadoFin As String)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String
        sql = "SELECT ep1.nombre AS EstadoIni, ep2.nombre AS EstadoFin " & _
                "	FROM SOLICITUDES_CAMBIOESTADO                                                        " & _
                "	LEFT JOIN estados_proceso EP1 ON SOLICITUDES_CAMBIOESTADO.estadoactual = ep1.codigo  " & _
                "	LEFT JOIN estados_proceso EP2 ON SOLICITUDES_CAMBIOESTADO.estado = ep2.codigo        " & _
                "	WHERE NroExp = '" & pExpediente & "' AND estadosol = 1 "

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            pNomEstadoIni = Reader("EstadoIni").ToString()
            pNomEstadoFin = Reader("EstadoFin").ToString()
        End If

        Reader.Close()
        Connection.Close()

    End Sub

    Private Function GetDatosExpediente(ByVal pExpediente As String, ByRef pDeudor As String, ByRef pTipoTitulo As String, ByRef pTipoId As String, ByRef pCcNit As String, ByRef pDeudaActual As String) As String
        Dim FechaReparto As String = ""

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        ''Opens a connection to the database.
        Connection.Open()
        Dim sql As String

        sql = "SELECT EJEFISGLOBAL.EFINROEXP, TmpTitulo.NomTipoTitulo, TmpDeudor.ED_Nombre, TmpDeudor.TipoIdentificacion, " & _
                "		TmpDeudor.ED_Codigo_Nit, TmpDeudor.ED_DigitoVerificacion, TmpTitulo.DeudaActual, EJEFISGLOBAL.EFIFECENTGES, EJEFISGLOBAL.EFIFECHAEXP " & _
                "	FROM EJEFISGLOBAL " & _
                "	LEFT JOIN (SELECT COALESCE(TIPOS_IDENTIFICACION.nombre,'C.C.') AS TipoIdentificacion , ED_Codigo_Nit, " & _
                "				ED_DigitoVerificacion, ED_Nombre " & _
                "				FROM ENTES_DEUDORES " & _
                "				LEFT JOIN TIPOS_IDENTIFICACION ON ENTES_DEUDORES.ED_TipoId = TIPOS_IDENTIFICACION.codigo) TmpDeudor " & _
                "		ON EJEFISGLOBAL.EFINIT = TmpDeudor.ED_Codigo_Nit " & _
                "	LEFT JOIN (SELECT MT_expediente, COALESCE(TIPOS_TITULO.nombre,'LIQUIDACION OFICIAL') AS NomTipoTitulo, totaldeuda, " & _
                "				COALESCE(A.pagCapital,0) AS pagCapital, (totaldeuda-COALESCE(A.pagCapital,0)) AS DeudaActual " & _
                "					FROM MAESTRO_TITULOS " & _
                "				LEFT JOIN (SELECT NroExp, SUM(pagCapital) AS pagCapital FROM pagos WHERE NroExp = '" & pExpediente & "' " & _
                "                           GROUP BY NroExp) A " & _
                "				ON MAESTRO_TITULOS.MT_expediente = A.NroExp " & _
                "				LEFT JOIN TIPOS_TITULO ON MAESTRO_TITULOS.MT_tipo_titulo = TIPOS_TITULO.codigo " & _
                "				WHERE MT_expediente = '" & pExpediente & "') TmpTitulo " & _
                "		ON EJEFISGLOBAL.EFINROEXP = TmpTitulo.MT_expediente " & _
                "	WHERE EJEFISGLOBAL.EFINROEXP = '" & pExpediente & "'"


        Dim Command As New SqlCommand(sql, Connection)

        Dim Reader As SqlDataReader = Command.ExecuteReader
        ' 
        If Reader.Read Then
            If Len(Reader("EFIFECENTGES").ToString()) > 10 Then
                FechaReparto = Mid(Reader("EFIFECENTGES").ToString(), 1, 2) & _
                                " de " & _
                                MonthName(CInt(Mid(Reader("EFIFECENTGES").ToString(), 4, 2))) & _
                                " de " & _
                                Mid(Reader("EFIFECENTGES").ToString(), 7, 4)
            Else
                FechaReparto = Mid(Reader("EFIFECHAEXP").ToString(), 1, 2) & _
                                " de " & _
                                MonthName(CInt(Mid(Reader("EFIFECHAEXP").ToString(), 4, 2))) & _
                                " de " & _
                                Mid(Reader("EFIFECHAEXP").ToString(), 7, 4)
            End If

            pDeudor = Reader("ED_Nombre").ToString().ToUpper
            pTipoTitulo = Reader("NomTipoTitulo").ToString().ToUpper
            pTipoId = Reader("TipoIdentificacion").ToString().ToUpper
            pCcNit = Reader("ED_Codigo_Nit").ToString()
            'pDeudaActual = Reader("DeudaActual").ToString()
            If Reader("DeudaActual").ToString().Trim = "" Then
                pDeudaActual = "0"
            Else
                pDeudaActual = Convert.ToDouble(Reader("DeudaActual").ToString()).ToString("N0")
            End If

        End If    

        Reader.Close()
        Connection.Close()

        Return FechaReparto
    End Function

    Private Function UsuarioEstaActivo(ByVal pIdUsuario As String) As Boolean
        Dim Activo As Boolean = False

        If Len(pIdUsuario) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT codigo FROM usuarios WHERE codigo = @codigo AND useractivo = 1"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", pIdUsuario.Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                Activo = True
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return Activo
    End Function


    Private Function getIdSuperior(ByVal pIdGestor As String) As String
        Dim IdSuperior As String = ""

        If Len(pIdGestor) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT superior FROM usuarios WHERE codigo = @codigo"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", pIdGestor.Trim.Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                IdSuperior = Reader("superior").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return IdSuperior
    End Function

    Private Function HayRepartidores() As Boolean
        Dim Respuesta As Boolean = False

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT codigo FROM usuarios WHERE nivelacces = 5 AND useractivo = 1"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader

        If Reader.Read Then
            Respuesta = True
        End If
        Reader.Close()
        Connection.Close()

        Return Respuesta
    End Function

    Private Function getNombreByID(ByVal pIdGestor As String) As String
        Dim NomGestor As String = ""

        If Len(pIdGestor) > 0 Then
            Dim Connection As New SqlConnection(Funciones.CadenaConexion)
            Connection.Open()
            Dim sql As String = "SELECT nombre FROM usuarios WHERE codigo = @codigo"
            Dim Command As New SqlCommand(sql, Connection)
            Command.Parameters.AddWithValue("@codigo", pIdGestor.Trim.Trim)

            Dim Reader As SqlDataReader = Command.ExecuteReader
            If Reader.Read Then
                NomGestor = Reader("nombre").ToString().Trim
            End If
            Reader.Close()
            Connection.Close()
        End If

        Return NomGestor
    End Function

    Protected Sub cboAprob_Revisor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboAprob_Revisor.SelectedIndexChanged
        'Dim FechaAprobRechazo As Date = Date.Now
        'CustomValidator1.Text = FechaAprobRechazo.ToString.Trim
        'CustomValidator1.IsValid = False
        If cboAprob_Revisor.SelectedValue = "PE" Then
            txtFecha_Aprob_Revisor.Text = ""
        Else
            Dim fechaAprobRechazo As Date = Date.Now.Date
            txtFecha_Aprob_Revisor.Text = fechaAprobRechazo.ToString("dd/MM/yyyy")
        End If

    End Sub

    Protected Sub cboAprob_Ejecutor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboAprob_Ejecutor.SelectedIndexChanged        
        If cboAprob_Ejecutor.SelectedValue = "PE" Then
            txtFecha_Aprob_Supervisor.Text = ""
        Else
            Dim fechaAprobRechazo2 As Date = Date.Now.Date
            txtFecha_Aprob_Supervisor.Text = fechaAprobRechazo2.ToString("dd/MM/yyyy")
        End If

    End Sub

    Protected Sub cmdSaveRevisor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveRevisor.Click

        If cboAprob_Revisor.SelectedValue = "PE" Then
            CustomValidator1.Text = "Indique si aprueba o rechaza el escalamiento al supervisor por favor "
            CustomValidator1.IsValid = False
            Return
        End If

        '03/MARZO/2015 ----------------------------
        Dim supervisor As String = ""
        supervisor = getIdSuperior(Session("sscodigousuario"))
        Dim EstaActivo As Boolean = UsuarioEstaActivo(supervisor)

        If supervisor = "" Or Not EstaActivo Then
            CustomValidator1.Text = "El usuario actual NO tiene un revisor o supervisor asociado. Favor informar a administrador para que haga la asociación"
            CustomValidator1.IsValid = False
            Return
        End If
        '------------------------------------------


        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand
        Dim UpdateSQL As String = ""

        'Si el revisor NO aprueba el escalamiento, se debe cambiar el estado de la solicitud de pendiente(1) a rechazado (3)
        If cboAprob_Revisor.SelectedValue.Trim = "NO" Then
            UpdateSQL = "UPDATE solicitudes_cambioestado SET ejecutor = @ejecutor, aprob_Revisor = @aprob_Revisor, fecha_aprob_Revisor = @fecha_aprob_Revisor, nota_revisor = @nota_revisor, estadosol = 3 WHERE idunico = @ID "
        Else
            UpdateSQL = "UPDATE solicitudes_cambioestado SET ejecutor = @ejecutor, aprob_Revisor = @aprob_Revisor, fecha_aprob_Revisor = @fecha_aprob_Revisor, nota_revisor = @nota_revisor, nivel_escalamiento = 2 WHERE idunico = @ID "
        End If

        'Declare string UpdateSQL
        Command = New SqlCommand(UpdateSQL, Connection)
        Command.Parameters.AddWithValue("@ID", Request("ID"))
        Command.Parameters.AddWithValue("@ejecutor", supervisor.Trim)
        Command.Parameters.AddWithValue("@aprob_Revisor", cboAprob_Revisor.SelectedValue)
        Command.Parameters.AddWithValue("@fecha_aprob_Revisor", DateTime.Now)        
        Command.Parameters.AddWithValue("@nota_revisor", taNota_Revisor.InnerHtml)

        Try
            Command.ExecuteNonQuery()
            Connection.Close()
            CustomValidator1.Text = "Datos guardados con éxito"
            CustomValidator1.IsValid = False

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False

        End Try

    End Sub

    'cmdSaveSupervisor
    Protected Sub cmdSaveSupervisor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSaveSupervisor.Click

        If cboAprob_Ejecutor.SelectedValue = "PE" Then
            CustomValidator1.Text = "Indique si aprueba o rechaza el escalamiento al repartidor por favor "
            CustomValidator1.IsValid = False
            Return
        End If

        ' 03/marzo/2015/. Validar que exista al menos un repartidor activo --
        If Not HayRepartidores() Then
            CustomValidator1.Text = "No hay usuarios repartidores en el momento. Favor informar a administrador para que haga la asociación"
            CustomValidator1.IsValid = False
            Return
        End If
        '--------------------------------------------------------------------


        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim Command As SqlCommand
        ' 
        'Declare string UpdateSQL 
        'Dim UpdateSQL As String = "UPDATE solicitudes_cambioestado SET aprob_ejecutor = @aprob_ejecutor, fecha_aprob_ejecutor = @fecha_aprob_ejecutor, nota_ejecutor = @nota_ejecutor, nivel_escalamiento = 3 WHERE idunico = @ID "
        'Si el supervisor NO aprueba el escalamiento, se debe cambiar el estado de la solicitud de pendiente(1) a rechazado (3)
        Dim UpdateSQL As String = ""
        If cboAprob_Ejecutor.SelectedValue.Trim = "NO" Then
            UpdateSQL = "UPDATE solicitudes_cambioestado SET aprob_ejecutor = @aprob_ejecutor, fecha_aprob_ejecutor = @fecha_aprob_ejecutor, nota_ejecutor = @nota_ejecutor, estadosol = 3 WHERE idunico = @ID "
        Else
            UpdateSQL = "UPDATE solicitudes_cambioestado SET aprob_ejecutor = @aprob_ejecutor, fecha_aprob_ejecutor = @fecha_aprob_ejecutor, nota_ejecutor = @nota_ejecutor, nivel_escalamiento = 3 WHERE idunico = @ID "
        End If


        Command = New SqlCommand(UpdateSQL, Connection)

        Command.Parameters.AddWithValue("@ID", Request("ID"))
        Command.Parameters.AddWithValue("@aprob_ejecutor", cboAprob_Ejecutor.SelectedValue)
        Command.Parameters.AddWithValue("@fecha_aprob_ejecutor", DateTime.Now)
        Command.Parameters.AddWithValue("@nota_ejecutor", taNota_Ejecutor.InnerHtml)

        Try
            Command.ExecuteNonQuery()
            Connection.Close()
            CustomValidator1.Text = "Datos guardados con éxito"
            CustomValidator1.IsValid = False

        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False

        End Try

    End Sub
End Class
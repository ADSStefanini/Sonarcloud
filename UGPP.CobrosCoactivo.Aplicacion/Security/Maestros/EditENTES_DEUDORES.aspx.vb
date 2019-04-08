Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class EditENTES_DEUDORES
    Inherits PaginaBase
    ''' <summary>
    ''' Se declaran las variables 
    ''' </summary>
    Dim tareaAsignadaObject As TareaAsignada
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL
    ''' <summary>
    ''' se inicializan
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnInit(e As EventArgs)
        tareaAsignadaBLL = New TareaAsignadaBLL()
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'GetMunicipiosByDpto("13")

            ' Tipos de entes / personas
            ' 1 = DEUDOR PRINCIPAL
            ' 2 = DEUDOR SOLIDARIO
            ' 3 = REPRESENTANTE LEGAL
            ' 4 = APODERADO
            ' 5 = AUTORIZADO
            If Len(Request("pTipo")) > 0 Then
                If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
                    lblPage1.Text = "Información del deudor"

                ElseIf Request("pTipo").Trim = "3" Then
                    lblPage1.Text = "Información del Representante legal"

                ElseIf Request("pTipo").Trim = "4" Then
                    lblPage1.Text = "Información del apoderado"

                Else
                    lblPage1.Text = "Información del autorizado"
                End If

                'Si es deudor principal se activa "cboED_TipoPersona" y "cboED_EstadoPersona"
                'If Request("pTipo").Trim = "1" Then
                '    cboED_TipoPersona.Enabled = True
                '    cboED_EstadoPersona.Enabled = True
                'Else
                '    cboED_TipoPersona.Enabled = False
                '    cboED_EstadoPersona.Enabled = False
                'End If
                ' La tarjeta profesional solo le debe aparecer al apoderado
                If Request("pTipo").Trim = "4" Then
                    lblTarjetaProf.Visible = True
                    txtTarjetaProf.Visible = True
                End If
            Else
                lblPage1.Text = "Información del deudor"
            End If

            LoadcboED_TipoId()
            LoadcboED_TipoPersona()
            LoadcboED_EstadoPersona()
            LoadcboED_TipoAportante()

            If Len(Request("IdDeudor")) > 0 Then
                'Cargue
                MostrarDeudor(Request("IdDeudor").ToString())
            Else
                'Agregando
                MostrarDeudor(String.Empty)
            End If
            Dim MTG As New MetodosGlobalesCobro
            'TODO: VALIDAR CUANDO SE CONSULTE POR SUPERIOR 
            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            If Session("mnivelacces") <> 5 And Session("mnivelacces") <> 8 And Session("mnivelacces") <> 11 And Session("mnivelacces") <> 10 Then
                Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
                If idGestorResp <> Session("sscodigousuario") Then

                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False

                    'Deshabilitar / Ocultar controles
                    'DesactivarControles()
                End If
                nombreModulo = "Abogado"
            End If
            If Session("mnivelacces") <> 11 Then ' solo area origen puede modificar deudores
                pnlDatosDeudor.Enabled = False
                cmdBorrar.Visible = False
                cmdSave.Visible = False
                cmdBorrar.Enabled = False
                cmdSave.Enabled = False
                nombreModulo = "AREA_ORIGEN"
                'tblEditENTES_DEUDORES.rea = True
            End If
        End If
    End Sub

#Region "Datos Vista"

    Private Sub LoadTipoEnte(SwExisteDedudorPrincipal As Boolean)
        Dim Modo As String = "ADD" 'o EDT

        If Len(Request("IdDeudor")) > 0 Then
            Modo = "EDT"
        End If

        Dim dt As DataTable = New DataTable("Tabla")

        dt.Columns.Add("codigo")
        dt.Columns.Add("nombre")

        Dim dr As DataRow

        If Modo = "ADD" Then
            If SwExisteDedudorPrincipal Then
                dr = dt.NewRow()
                dr("codigo") = 0
                dr("nombre") = "SELECCIONE..."
                dt.Rows.Add(dr)

                dr = dt.NewRow()
                dr("codigo") = 2
                dr("nombre") = "DEUDOR SOLIDARIO"
                dt.Rows.Add(dr)

            Else
                dr = dt.NewRow()
                dr("codigo") = 1
                dr("nombre") = "DEUDOR PRINCIPAL"
                dt.Rows.Add(dr)

            End If
        Else
            'Modo = "EDT"
            dr = dt.NewRow()
            dr("codigo") = 0
            dr("nombre") = "SELECCIONE..."
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            dr("codigo") = 2
            dr("nombre") = "DEUDOR SOLIDARIO"
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            dr("codigo") = 1
            dr("nombre") = "DEUDOR PRINCIPAL"
            dt.Rows.Add(dr)

        End If

        cboTipoDeudor.DataSource = dt
        cboTipoDeudor.DataTextField = "nombre"
        cboTipoDeudor.DataValueField = "codigo"
        cboTipoDeudor.DataBind()
    End Sub
    Private Sub DesactivarControles()
        cmdSave.Visible = False
        txtED_Codigo_Nit.Enabled = False
        txtED_DigitoVerificacion.Enabled = False
        cboED_TipoId.Enabled = False
        txtED_Nombre.Enabled = False
        cboED_TipoPersona.Enabled = False
        cboED_EstadoPersona.Enabled = False
        cboED_TipoAportante.Enabled = False
        cboTipoDeudor.Enabled = False
        txtParticipacion.Enabled = False
    End Sub
    Protected Sub LoadcboED_TipoId()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [TIPOS_IDENTIFICACION] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_TipoId.DataTextField = "nombre"
        'cboED_TipoId.DataValueField = "codigo"
        'cboED_TipoId.DataSource = Command.ExecuteReader()
        'cboED_TipoId.DataBind()
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM [TIPOS_IDENTIFICACION] WHERE ind_estado = 1 order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposID As DataView = New DataView(dt1)
            vistaTiposID.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoId.DataSource = vistaTiposID
            cboED_TipoId.DataTextField = "nombre"
            cboED_TipoId.DataValueField = "codigo"
            cboED_TipoId.DataBind()
        End If
    End Sub
    Protected Sub LoadcboED_TipoPersona()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [TIPOS_PERSONA] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_TipoPersona.DataTextField = "nombre"
        'cboED_TipoPersona.DataValueField = "codigo"
        'cboED_TipoPersona.DataSource = Command.ExecuteReader()
        'cboED_TipoPersona.DataBind()
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "SELECT codigo, nombre FROM [TIPOS_PERSONA] WHERE ind_estado = 1 ORDER BY nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposP As DataView = New DataView(dt1)
            vistaTiposP.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoPersona.DataSource = vistaTiposP
            cboED_TipoPersona.DataTextField = "nombre"
            cboED_TipoPersona.DataValueField = "codigo"
            cboED_TipoPersona.DataBind()
        End If
    End Sub
    Protected Sub LoadcboED_EstadoPersona()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [ESTADOS_PERSONA] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_EstadoPersona.DataTextField = "nombre"
        'cboED_EstadoPersona.DataValueField = "codigo"
        'cboED_EstadoPersona.DataSource = Command.ExecuteReader()
        'cboED_EstadoPersona.DataBind()
        ''Close the Connection Object 
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [ESTADOS_PERSONA] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaEstadosP As DataView = New DataView(dt1)
            vistaEstadosP.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_EstadoPersona.DataSource = vistaEstadosP
            cboED_EstadoPersona.DataTextField = "nombre"
            cboED_EstadoPersona.DataValueField = "codigo"
            cboED_EstadoPersona.DataBind()
        End If
    End Sub
    Protected Sub LoadcboED_TipoAportante()
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_APORTANTES] WHERE ind_estado = 1 order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposA As DataView = New DataView(dt1)
            vistaTiposA.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoAportante.DataSource = vistaTiposA
            cboED_TipoAportante.DataTextField = "nombre"
            cboED_TipoAportante.DataValueField = "codigo"
            cboED_TipoAportante.DataBind()
        End If
    End Sub
    Private Shared Function GetCiudades() As DataTable
        Dim cmd, cnx As String
        cnx = Funciones.CadenaConexion
        cmd = "SELECT codigo, nombre, departamento FROM municipios"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtMunicipios As New DataTable
        Adaptador.Fill(dtMunicipios)
        Return dtMunicipios
    End Function
    ''' <summary>
    ''' Se carga la vista con el usuario enviado 
    ''' </summary>
    ''' <param name="deudorItem"></param>
    Private Sub CargarVista(deudorItem As Deudor)
        If String.IsNullOrEmpty(deudorItem.numeroIdentificacion) Then
            'Esta Adicionando
            cboED_TipoPersona.SelectedValue = "01" 'Natural
            cboED_EstadoPersona.SelectedValue = "07" 'Viva
            txtParticipacion.Text = 0
            cmdBorrar.Visible = False
        Else
            If String.IsNullOrEmpty(deudorItem.tipoIdentificacion) = False Then
                cboED_TipoId.SelectedValue = deudorItem.tipoIdentificacion
            End If

            If String.IsNullOrEmpty(deudorItem.numeroIdentificacion) = False Then
                txtED_Codigo_Nit.Text = deudorItem.numeroIdentificacion
            End If

            If String.IsNullOrEmpty(deudorItem.numeroIdentificacion) = False Then
                txtED_DigitoVerificacion.Text = deudorItem.digitoVerificacion
            End If

            If String.IsNullOrEmpty(deudorItem.tipoPersona) = False Then
                cboED_TipoPersona.SelectedValue = deudorItem.tipoPersona
            End If

            If String.IsNullOrEmpty(deudorItem.digitoVerificacion) = False Then
                txtED_DigitoVerificacion.Text = deudorItem.digitoVerificacion
            End If

            If String.IsNullOrEmpty(deudorItem.nombreDeudor) = False Then
                txtED_Nombre.Text = deudorItem.nombreDeudor
            End If

            If String.IsNullOrEmpty(deudorItem.PorcentajeParticipacion) = False Then
                txtParticipacion.Text = deudorItem.PorcentajeParticipacion
            End If


            If String.IsNullOrEmpty(deudorItem.EstadoPersona) Then
                cboED_EstadoPersona.SelectedValue = "00"
            Else
                cboED_EstadoPersona.SelectedValue = deudorItem.EstadoPersona
            End If

            If String.IsNullOrEmpty(txtParticipacion.Text) Then
                txtParticipacion.Text = "0"
            End If
            If String.IsNullOrEmpty(deudorItem.MatriculaMercantil) = False Then
                txtNoMatricula.Text = deudorItem.MatriculaMercantil
            End If

            If String.IsNullOrEmpty(deudorItem.TipoAportante) Then
                cboED_TipoAportante.SelectedValue = "00"
            Else
                cboED_TipoAportante.SelectedValue = deudorItem.TipoAportante
            End If

            If String.IsNullOrEmpty(deudorItem.TipoEnte) = False Then
                cboTipoDeudor.SelectedValue = deudorItem.TipoEnte
            End If
            'Si es apoderado => Mostrar tarjeta profesional
            If deudorItem.TipoEnte.ToString() = "4" Then
                txtTarjetaProf.Text = deudorItem.TarjetaProfesional.ToString().Trim
            End If
        End If

    End Sub
    Private Sub MostrarDeudor(NitDeudor As String)
        Dim deudorItem As Deudor
        If Len(Request("ID_TASK")) > 0 And Request("ID_TASK").ToString()<> "0" Then
            'Si ID_TASK tiene valor se carga valida el item
            tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(Request("ID_TASK").ToString()))
            Dim tituloEjecutivoObj As TituloEjecutivoExt

            Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
            tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)

            If String.IsNullOrEmpty(NitDeudor) = False Then
                deudorItem = tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.numeroIdentificacion) = NitDeudor.ToString()).FirstOrDefault()
            Else
                deudorItem = New Deudor()
            End If
            If tituloEjecutivoObj.LstDeudores.Count() > 0 Then
                LoadTipoEnte(tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.TipoEnte) = 1).FirstOrDefault() IsNot Nothing)
            Else
                LoadTipoEnte(False)
            End If
            CargarVista(deudorItem)
        Else
            deudorItem = New Deudor()
            LoadTipoEnte(False)
            CargarVista(deudorItem)
        End If

    End Sub
#End Region
    Private Function SumarPorcentajesSociosSolidarios(ByVal pExpediente As String, ByVal pDeudor As String, ByVal pPorcentaje As Decimal) As Decimal
        Dim Sw As Integer = 0
        Dim TotalPorcentaje As Decimal = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM DEUDORES_EXPEDIENTES WHERE NroExp = '" & pExpediente & "' AND tipo = 2"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        While Reader.Read
            If Reader("deudor").ToString().Trim = pDeudor Then
                TotalPorcentaje = TotalPorcentaje + pPorcentaje
                Sw = 1
            Else
                TotalPorcentaje = TotalPorcentaje + CDec(Reader("participacion").ToString().Trim)
            End If
        End While
        Reader.Close()
        Connection.Close()

        If Sw = 0 Then
            TotalPorcentaje = TotalPorcentaje + pPorcentaje
        End If

        Return TotalPorcentaje

    End Function

    Private Function ExisteEnte(ByVal pDeudor As String) As Boolean
        Dim Respuesta As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT ED_Codigo_Nit FROM ENTES_DEUDORES WHERE ED_Codigo_Nit = @ED_Codigo_Nit"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@ED_Codigo_Nit", pDeudor)
        '
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            Respuesta = True
        End If
        Return Respuesta

    End Function

    Private Function ExisteDEUDORES_EXPEDIENTES(ByVal pDeudor As String, ByVal pExpediente As String) As Integer
        Dim Respuesta As Integer = 0
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT deudor, NroExp FROM DEUDORES_EXPEDIENTES WHERE deudor = @deudor AND NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@deudor", pDeudor)
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        '                 
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        While Reader.Read
            'Respuesta = True
            Respuesta = Respuesta + 1
        End While
        Return Respuesta

    End Function

#Region "Eventos"
    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If String.IsNullOrEmpty(txtED_Codigo_Nit.Text) = True Then
            CustomValidator1.Text = "Es necesario el numero de identificaciòn"
            CustomValidator1.IsValid = False
        Else
#Region "GUARDAR"
            CustomValidator1.IsValid = True
            Dim DeudorItem As Deudor = New Deudor()
            If String.IsNullOrEmpty(cboED_TipoPersona.SelectedValue) = False Then
                DeudorItem.tipoPersona = cboED_TipoPersona.SelectedValue
                DeudorItem.NomtipoPersona = cboED_TipoPersona.SelectedItem.Text
            End If

            If String.IsNullOrEmpty(txtED_Nombre.Text) = False Then
                DeudorItem.nombreDeudor = txtED_Nombre.Text
            End If

            If String.IsNullOrEmpty(cboED_TipoId.SelectedValue) = False Then
                DeudorItem.tipoIdentificacion = cboED_TipoId.SelectedValue
            End If

            If String.IsNullOrEmpty(txtED_Codigo_Nit.Text) = False Then
                DeudorItem.numeroIdentificacion = txtED_Codigo_Nit.Text
            End If

            If String.IsNullOrEmpty(cboTipoDeudor.SelectedValue) = False Then
                DeudorItem.TipoEnte = cboTipoDeudor.SelectedValue
                DeudorItem.NomTipoEnte = cboTipoDeudor.SelectedItem.Text
            End If

            If String.IsNullOrEmpty(txtTarjetaProf.Text) = False Then
                DeudorItem.TarjetaProfesional = txtTarjetaProf.Text
            End If

            If String.IsNullOrEmpty(txtNoMatricula.Text) = False Then
                DeudorItem.MatriculaMercantil = txtNoMatricula.Text
            End If


            If String.IsNullOrEmpty(txtParticipacion.Text) = False Then
                DeudorItem.PorcentajeParticipacion = txtParticipacion.Text
            End If

            If String.IsNullOrEmpty(cboED_TipoAportante.SelectedValue) = False Then
                DeudorItem.NomTipoAportante = cboED_TipoAportante.SelectedItem.Text
                DeudorItem.TipoAportante = cboED_TipoAportante.SelectedValue
            End If
            If String.IsNullOrEmpty(cboED_EstadoPersona.SelectedValue) = False Then
                DeudorItem.NomEstadoPersona = cboED_EstadoPersona.SelectedItem.Text
                DeudorItem.EstadoPersona = cboED_EstadoPersona.SelectedValue
            End If
            If String.IsNullOrEmpty(txtED_DigitoVerificacion.Text) = False Then
                DeudorItem.digitoVerificacion = txtED_DigitoVerificacion.Text
            End If
            Dim almacenamientoTemportalItem As AlmacenamientoTemporal
            Dim tituloEjecutivoObj As TituloEjecutivoExt
            If Len(Request("ID_TASK")) > 0 And Request("ID_TASK").ToString() <> "0" Then
                almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Request("ID_TASK").ToString())
                tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
            Else
                almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Request("ID_TASK").ToString())
                tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
            End If





            If Len(Request("IdDeudor")) > 0 Then
                tituloEjecutivoObj.LstDeudores.Remove(tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.numeroIdentificacion).ToString() = Request("IdDeudor")).FirstOrDefault())
            Else
                'si ya existe un deudor principal
                If cboTipoDeudor.SelectedValue = "1" Then
                    Dim deudorPrincipal As Deudor = tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.TipoEnte) = 1).FirstOrDefault()
                    If deudorPrincipal IsNot Nothing Then
                        If deudorPrincipal.numeroIdentificacion <> DeudorItem.numeroIdentificacion Then
                            CustomValidator1.Text = "El titulo solo puede tener un deudor principal"
                            CustomValidator1.IsValid = False
                            Return
                        End If
                    End If
                End If
            End If
            Dim deudorDuplicado As Deudor = New Deudor()
            deudorDuplicado = tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.numeroIdentificacion) = DeudorItem.numeroIdentificacion).FirstOrDefault()
            If deudorDuplicado IsNot Nothing Then
                tituloEjecutivoObj.LstDeudores.Remove(deudorDuplicado)
            End If
            tituloEjecutivoObj.LstDeudores.Add(DeudorItem)
            almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(tituloEjecutivoObj)
            almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("jsonobj=" + almacenamientoTemportalItem.JSON_OBJ))
            almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
#End Region
            'Ir a pagina de resumen
            Response.Redirect("ENTES_DEUDORES.aspx?ID_TASK=" & Request("ID_TASK") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
        End If

    End Sub
    Public Sub crearTarea()
        'Si ID_TASKno tiene valor se crea una tarea
        tareaAsignadaObject = New TareaAsignada()
        tareaAsignadaObject.COD_TIPO_OBJ = 4 ' TITULO
        tareaAsignadaObject.VAL_USUARIO_NOMBRE = Session("ssloginusuario")
        tareaAsignadaObject.COD_ESTADO_OPERATIVO = 1
        tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("codtipo=" + tareaAsignadaObject.COD_TIPO_OBJ.ToString() + ",usunombre=" & tareaAsignadaObject.VAL_USUARIO_NOMBRE + ",codestadoope=" & tareaAsignadaObject.COD_ESTADO_OPERATIVO.ToString()))
        tareaAsignadaObject = tareaAsignadaBLL.registrarTarea(tareaAsignadaObject)
        Dim almacenamientoTemportalItem As AlmacenamientoTemporal = New AlmacenamientoTemporal()
        almacenamientoTemportalItem.ID_TAREA_ASIGNADA = tareaAsignadaObject.ID_TAREA_ASIGNADA
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(New TituloEjecutivoExt())
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("idtareaasig=" + almacenamientoTemportalItem.ID_TAREA_ASIGNADA.ToString() + ",jsonobj=" & almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.InsertarAlmacenamiento(almacenamientoTemportalItem)
        'si no hay id de la tarea es creacion del titulo
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("ENTES_DEUDORES.aspx?ID_TASK=" & Request("ID_TASK") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
    End Sub
    Protected Sub cmdBorrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBorrar.Click
        Response.Redirect("BorrarDEUDORES_EXPEDIENTES.aspx?ID_TASK=" & Request("ID_TASK") & "&IdDeudor=" & Request("IdDeudor"))
    End Sub


#End Region

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "Seguridad"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function
End Class
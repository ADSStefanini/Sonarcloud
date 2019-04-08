Imports System.Data.SqlClient
Imports System.Globalization
Imports UGPP.CobrosCoactivo
Imports UGPP.CobrosCoactivo.Logica
Imports UGPP.CobrosCoactivo.Entidades
Imports System.IO

Public Class CommonsCobrosCoactivos

    ''' <summary>
    ''' Función que retorna al usuario a la lista de expedientes dependiendo de su perfil
    ''' </summary>
    ''' <param name="prmObjSession">Objeto HTTPSession creada cuando el usuario se autentica ante el sistema</param>
    ''' <param name="prmObjResponse">Objeto HTTPResponse para definir el rediret</param>
    Public Shared Function llevarAListaExpedientes(ByVal prmObjSession As HttpSessionState, ByVal prmObjResponse As HttpResponse)
        'Response.Redirect("EJEFISGLOBALREPARTIDOR.aspx")
        Dim Perfil As String = CommonsCobrosCoactivos.getNomPerfil(prmObjSession)

        If Perfil = "REPARTIDOR" Then
            prmObjResponse.Redirect("EJEFISGLOBALREPARTIDOR.aspx")

        ElseIf Perfil = "VERIFICADOR DE PAGOS" Then
            prmObjResponse.Redirect("PAGOS.aspx")

        ElseIf Perfil = "GESTOR - ABOGADO" Then
            prmObjResponse.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "REVISOR" Then
            prmObjResponse.Redirect("EJEFISGLOBAL.aspx")

        ElseIf Perfil = "SUPERVISOR" Then
            prmObjResponse.Redirect("EJEFISGLOBAL.aspx")

        Else
            ' SUPER ADMINISTRADOR
            prmObjResponse.Redirect("EJEFISGLOBAL.aspx")
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Obtiene el nombre del perfil a partir del nivel de acceso de la sesión del usuario
    ''' </summary>
    ''' <param name="prmObjSession">Objeto HTTPSession creada cuando el usuario se autentica ante el sistema</param>
    ''' <returns></returns>
    Public Shared Function getNomPerfil(ByVal prmObjSession As HttpSessionState) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & prmObjSession("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    ''' <summary>
    ''' Verifica si el string que se pasa como paramentro es un número entero
    ''' </summary>
    ''' <param name="prmStrNumbertoCheck">String para validar si es entero</param>
    ''' <returns>Verdadero si el string es un entero, falso en caso contrario</returns>
    Public Shared Function checkIIsNumber(ByVal prmStrNumbertoCheck) As Boolean
        Try
            Convert.ToDouble(prmStrNumbertoCheck)
            Return True
        Catch
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Poblar dropdownlist con los perfiles de la base de datos
    ''' </summary>
    ''' <param name="ddlPerfiles">Dropdownlist de perfiles</param>
    ''' <returns></returns>
    Public Shared Function poblarPerfiles(ByVal ddlPerfiles As DropDownList)
        Dim perfilBLL As New PerfilBLL()
        Dim perfiles As List(Of Entidades.Perfiles) = perfilBLL.obtenerPerfilesActivos()
        For Each p As Entidades.Perfiles In perfiles
            ddlPerfiles.Items.Add(New ListItem(p.nombre_perfil, p.codigo.ToString()))
        Next
        Return ddlPerfiles
    End Function

    ''' <summary>
    ''' Poblar dropdownlist con estados operativos del título desde la base de datos
    ''' </summary>
    ''' <param name="ddlEstadosOperativos">Dropdownlist de estados operativos</param>
    ''' <returns></returns>
    Public Shared Function poblarEstadosOperativosTitulos(ByVal ddlEstadosOperativos As DropDownList)
        Dim estadoOperativosBLL As New EstadoOperativoBLL()
        Dim estadosOperativos As List(Of Entidades.EstadoOperativo) = estadoOperativosBLL.obtenerEstadosOperativosActivosTitulos()
        For Each estadoOperativo As Entidades.EstadoOperativo In estadosOperativos
            ddlEstadosOperativos.Items.Add(New ListItem(estadoOperativo.VAL_NOMBRE, estadoOperativo.ID_ESTADO_OPERATIVOS.ToString()))
        Next
        Return ddlEstadosOperativos
    End Function

    ''' <summary>
    ''' Poblar dropdownlist con estados procesales
    ''' </summary>
    ''' <param name="ddlEstadosProcesales">Dropdownlist de estados procesales</param>
    ''' <returns></returns>
    Public Shared Function poblarEstadosProcesalesTitulos(ByVal ddlEstadosProcesales As DropDownList)
        Dim estadosProcesalesBLL As New EstadosProcesoBLL()
        Dim estadosProcesales = estadosProcesalesBLL.obtenerEstadosProcesos()
        For Each estadoProcesal As Entidades.EstadosProceso In estadosProcesales
            ddlEstadosProcesales.Items.Add(New ListItem(estadoProcesal.nombre, estadoProcesal.codigo))
        Next
        Return ddlEstadosProcesales
    End Function

    ''' <summary>
    ''' Poblar las opciones de las causales de Priorización
    ''' </summary>
    ''' <param name="ddlCausalesPriorizacion">DropDownList que se agregan las opciones</param>
    ''' <returns>DropDownList con opciones agregadas</returns>
    Public Shared Function poblarCausalesPriorizacion(ByVal ddlCausalesPriorizacion As DropDownList)
        Dim _tiposCausalPriorizacionBLL As New TiposCausalPriorizacionBLL()
        Dim causalesPriorizacion = _tiposCausalPriorizacionBLL.obtenerCausalesPriorizacionActivos()
        For Each causalPriorizacion As Entidades.TiposCausalesPriorizacion In causalesPriorizacion
            ddlCausalesPriorizacion.Items.Add(New ListItem(causalPriorizacion.VAL_TIPO_CAUSAL_PRIORIZACION, causalPriorizacion.ID_TIPO_CAUSAL_PRIORIZACION))
        Next
        Return ddlCausalesPriorizacion
    End Function

    ''' <summary>
    ''' Poblar las opciones de las causales de reasignación
    ''' </summary>
    ''' <param name="ddlCausalesReasignacion">DropDownList que se agregan las opciones</param>
    ''' <returns>DropDownList con opciones agregadas</returns>
    Public Shared Function poblarCausalesReasignacion(ByVal ddlCausalesReasignacion As DropDownList)
        Dim _tiposCausalReasignacionBLL As New TiposCausalReasignacionBLL()
        Dim causalesReasigancion = _tiposCausalReasignacionBLL.obtenerCausalesReasignacionActivos()
        For Each causalReasignacion As Entidades.TiposCausalesReasignacion In causalesReasigancion
            ddlCausalesReasignacion.Items.Add(New ListItem(causalReasignacion.VAL_TIPO_CAUSAL_REASIGNACION, causalReasignacion.ID_TIPO_CAUSAL_REASIGNACION))
        Next
        ddlCausalesReasignacion.DataBind()
        Return ddlCausalesReasignacion
    End Function

    Public Shared Function setGrudViewNavText(ByVal gridView As GridView) As GridView
        gridView.PagerSettings.Mode = PagerButtons.NextPreviousFirstLast
        gridView.PagerSettings.FirstPageText = "Primero"
        gridView.PagerSettings.PreviousPageText = "Anterior"
        gridView.PagerSettings.NextPageText = "Siguiente"
        gridView.PagerSettings.LastPageText = "Último"
        'gridView.PagerSettings.Mode =
        Return gridView
    End Function

    '''' <summary>
    '''' Poblar cheboxslist con los datos de tipificacion cumple no cumple
    '''' </summary>
    '''' <param name="ChkBltsTipifica">CheckBoxList de tipificaciones</param>
    '''' <returns></returns>
    Public Shared Function poblarTipificaciones(ByVal ChkBltsTipifica As CheckBoxList)
        Dim tipificacionesBLL As New TipificacionCNCBLL()
        Dim tipificaciones As List(Of Entidades.TipificacionCNC) = tipificacionesBLL.obtenerTipificacionCNC()
        For Each tipifica As Entidades.TipificacionCNC In tipificaciones
            ChkBltsTipifica.Items.Add(New ListItem(tipifica.DESCRIPCION_TIPIFICACION.ToString(), tipifica.ID_TIPIFICACION))
        Next
        Return ChkBltsTipifica
    End Function

    ''' <summary>
    ''' Verificar si el expediente se clasifica por cuantia
    ''' </summary>
    ''' <param name="prmIntTareaAsignada">ID de la tarea asiganada</param>
    ''' <param name="prmStrExpedienteId">Id del expediente</param>
    ''' <returns>Verdadero si cumple la clasificación por cuantia, falso en caso contrario</returns>
    Public Shared Function ClasificacionTituloPorCuantia(ByVal prmIntTareaAsignada As Integer, ByVal prmStrExpedienteId As String) As Boolean
        Try
            'Valida la clasificación por cuantia del expediente
            Dim clasificacionManual As New ClasificacionManualBLL()
            clasificacionManual.idTareaAsiganada = prmIntTareaAsignada
            'Si el ID del expediente existe en la DB
            Dim clasfificacionCuantia = clasificacionManual.ClasificacionPorCuantia(prmStrExpedienteId)
            If clasfificacionCuantia Then
                Dim _expedienteBLL As New ExpedienteBLL()
                _expedienteBLL.asignarExpedientePorRepartir(prmStrExpedienteId) 'Se envía el expediente a repartir
                Dim _tareaAsignadaBLL As New TareaAsignadaBLL()
                _tareaAsignadaBLL.actualizarEstadoOperativoTareaAsignada(prmIntTareaAsignada, 6) 'Se actualiza el estado del título a aceptado
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Verificar si el expediente se clasifica por cuantia
    ''' </summary>
    ''' <param name="prmIntTareaAsignada">ID de la tarea asiganada</param>
    ''' <param name="prmStrExpedienteId">Id del expediente</param>
    ''' <returns>Verdadero si cumple la clasificación por cuantia, falso en caso contrario</returns>
    Public Shared Function ClasificacionUVT(ByVal prmIntTareaAsignada As Integer, ByVal prmStrExpedienteId As String) As Boolean
        Try
            'Valida la clasificación por cuantia del expediente
            Dim clasificacionManual As New ClasificacionManualBLL()
            clasificacionManual.idTareaAsiganada = prmIntTareaAsignada
            Return clasificacionManual.ClasificacionPorCuantia(prmStrExpedienteId, False)
        Catch ex As Exception
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Permite cargar el combo tipo proceso juridico 
    ''' </summary>
    Public Shared Sub CargarComboJuridica(ByVal ddlTipoProcesoJuridica As DropDownList)
        Dim tipoProcesosConcursales As New TiposProcesosConcursalesBLL
        Dim listatipoProcesosConcursales = tipoProcesosConcursales.ObtenerTipoProcesoJuridica()
        ddlTipoProcesoJuridica.DataSource = listatipoProcesosConcursales
        ddlTipoProcesoJuridica.DataValueField = "codigo"
        ddlTipoProcesoJuridica.DataTextField = "nombre"
        ddlTipoProcesoJuridica.DataBind()
        ddlTipoProcesoJuridica.Items.Insert(0, New ListItem("--Seleccione--", "0"))
    End Sub

    ''' <summary>
    ''' Permite cargar el combi tipo proceso natural
    ''' </summary>
    Public Shared Sub CargarComboNatural(ByVal ddlTipoProcesoNatural As DropDownList)
        Dim tipoProcesosConcursales As New TiposProcesosConcursalesBLL
        Dim listatipoProcesosConcursalesNatural = tipoProcesosConcursales.ObtenerTipoProcesoNatural()
        ddlTipoProcesoNatural.DataSource = listatipoProcesosConcursalesNatural
        ddlTipoProcesoNatural.DataValueField = "codigo"
        ddlTipoProcesoNatural.DataTextField = "nombre"
        ddlTipoProcesoNatural.DataBind()
        ddlTipoProcesoNatural.Items.Insert(0, New ListItem("--Seleccione--", "0"))
    End Sub


    ''' <summary>
    ''' funcion que permite generar la clasificación de acuerdo al orden establecido
    ''' </summary>
    Public Shared Sub ClasificacionAutomatica(ByVal idExpediente As String, ByVal prmStrCodigoUsuario As String, ByVal prmIntIdTask As Integer)
        Dim clasificarAutomatica As New ClasificacionAutomaticaBLL(idExpediente, prmIntIdTask)
        Dim valCodUsuario = prmStrCodigoUsuario 'Session("sscodigousuario")
        Dim valClasifPresc = clasificarAutomatica.porPrescripcion(idExpediente, valCodUsuario)
        If valClasifPresc = False Then
            Dim valClasifFacilPago = clasificarAutomatica.porFacilidadDePago(idExpediente)
            If valClasifFacilPago = False Then
                clasificarAutomatica.porTipoDeObligacion(idExpediente, valCodUsuario)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Pobla los usuarios a los que se les puede reasignar una tarea (título o expediente)
    ''' </summary>
    ''' <param name="ddlUsuarios">DropDownList a poblar</param>
    ''' <param name="prmIntTipoUsuarios">{1: Gestores estudio de títulos; 2: Gestores expedientes}</param>
    ''' <returns>DropDownList pasado como parámetro con opciones agregadas</returns>
    Public Shared Function PoblarUsuariosReasignacion(ByVal ddlUsuarios As DropDownList, ByVal prmIntTipoUsuarios As Integer) As DropDownList
        Dim _usuarioBLL As New UsuariosBLL()
        Dim _usuarios = _usuarioBLL.ObtenerUsuariosReasignacion(prmIntTipoUsuarios)
        For Each _usuario As Entidades.Usuario In _usuarios
            ddlUsuarios.Items.Add(New ListItem(_usuario.nombre, _usuario.login))
        Next
        Return ddlUsuarios
    End Function

    ''' <summary>
    ''' Pobla las opciones del estado de solicitud 
    ''' </summary>
    ''' <param name="ddlEstadoSolicitud">Dropdown a agregar las opciones</param>
    ''' <returns>Dropdown con las opciones agregadas</returns>
    Public Shared Function PoblarEstadoSolicitud(ByVal ddlEstadoSolicitud As DropDownList) As DropDownList
        Dim _dominioDetalle As New DominioDetalleBLL()
        Dim _estadosSolicitud = _dominioDetalle.consultarDominioPorIdDominio(Enumeraciones.Dominio.EstadoSolicitud)
        For Each _estadoSolicitud As Entidades.DominioDetalle In _estadosSolicitud
            ddlEstadoSolicitud.Items.Add(New ListItem(_estadoSolicitud.VAL_NOMBRE, _estadoSolicitud.VAL_VALOR))
        Next
        Return ddlEstadoSolicitud
    End Function

    Public Shared Function ObtenerUsuarioSuperiorSolicitud(ByVal prmStrLoginUsuarioSolicitante As String, ByVal prmIntTipoSolicitud As Integer) As Entidades.Usuario
        'Se inica la variable de usuario para asignación al superior
        Dim _usuarioSuperior As New Usuario
        'Se obtiene el usuario superior del usuario solicitante al que se le va a asiganar la solicitud
        Dim usuariosBLL As New UsuariosBLL()
        Dim usuario As Usuario
        Try
            usuario = usuariosBLL.obtenerUsuarioPorLogin(prmStrLoginUsuarioSolicitante)
            'Para priorización el superior del superior
            _usuarioSuperior = usuariosBLL.obtenerUsuarioSuperior(usuario.codigo)
            'Para reasignaciones se toma el superior del superior
            If prmIntTipoSolicitud = 9 Then
                _usuarioSuperior = usuariosBLL.obtenerUsuarioSuperior(_usuarioSuperior.codigo)
            End If
        Catch ex As Exception
            Return New Usuario
        End Try
        Return _usuarioSuperior
    End Function

    ''' <summary>
    ''' Verifica si el login que se pasa como parametro pertenece a la jerarquía que se pasa como parámetro
    ''' </summary>
    ''' <param name="prnStrUserLogin">Login del usuario</param>
    ''' <param name="prmObjIdJerarquia">Identificador de la jerarquía</param>
    ''' <returns>Objeto del tipo Entidades.Usuario</returns>
    Public Shared Function VerificaSiUsuarioPerteneceAJerarquia(ByVal prnStrUserLogin As String, ByVal prmObjIdJerarquia As Enumeraciones.JerarquiaUsuario) As Boolean
        Dim _usuariosBLL As New UsuariosBLL()
        Dim _usuario = _usuariosBLL.VerificarJerarquiaUsuarioPorLogin(prnStrUserLogin, prmObjIdJerarquia)
        If IsNothing(_usuario) Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Obtiene todos los gestores que pueden realizar estudio de títulos y expedientes
    ''' </summary>
    Public Shared Function poblarGestoresTodos(ByVal ddlGestor As DropDownList) As DropDownList
        Dim _ususarioBLL As New UsuariosBLL()
        Dim usuarios = _ususarioBLL.ObtenerGestoresTodos()
        If usuarios.Count() > 0 Then
            For Each usuario As Entidades.Usuario In usuarios
                ddlGestor.Items.Add(New ListItem(usuario.nombre, usuario.login))
            Next
        Else
            ddlGestor.Enabled = False
        End If
        Return ddlGestor
    End Function

    ''' <summary>
    ''' Método para insertar un mensaje de log en el fichero service_log.txt
    ''' </summary>
    ''' <param name="strMensaje">Mensaje a escribir en el log</param>
    Public Shared Sub EscribirMensajeLogServicio(ByVal strMensaje As String)
        Using w As StreamWriter = File.AppendText("service_log.txt")
            w.WriteLine($"{DateTime.Now.ToLongDateString} {DateTime.Now.ToLongTimeString()}  : {strMensaje}")
        End Using
    End Sub
End Class

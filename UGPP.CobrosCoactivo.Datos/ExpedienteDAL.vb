Imports AutoMapper
Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports System.Data.SqlClient

''' <summary>
''' Clase para realizar transacciones con la tabla EJEFIXGLOBAL relacionada con los expedientes
''' </summary>
Public Class ExpedienteDAL
    Inherits AccesObject(Of Entidades.EJEFISGLOBAL)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _Auditoria As LogAuditoria

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _Auditoria = auditLog
    End Sub

    ''' <summary>
    ''' Convierte un objeto del tipo Entidades.EJEFISGLOBAL a Datos.EJEFISGLOBAL
    ''' </summary>
    ''' <param name="prmObjExpedienteEntidad">Objeto de tipo Entidades.EJEFISGLOBAL</param>
    ''' <returns>Objeto de tipo Datos.EJEFISGLOBAL</returns>
    Public Function ConvertirADatosExpediente(ByVal prmObjExpedienteEntidad As Entidades.EJEFISGLOBAL) As Datos.EJEFISGLOBAL
        Dim expediente As Datos.EJEFISGLOBAL
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.EJEFISGLOBAL, Entidades.EJEFISGLOBAL)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        expediente = IMapper.Map(Of Entidades.EJEFISGLOBAL, Datos.EJEFISGLOBAL)(prmObjExpedienteEntidad)
        Return expediente
    End Function

    ''' <summary>
    ''' Retorna un expediente por su número ID
    ''' </summary>
    ''' <param name="prmStrExpedienteId">Número único del expediente de cobros</param>
    ''' <returns>Objeto del tipo Datos.EJEFISGLOBAL</returns>
    Public Function obtenerExpedientePorId(ByVal prmStrExpedienteId As String) As Datos.EJEFISGLOBAL
        Dim expediente = (From e In db.EJEFISGLOBAL
                          Where e.EFINROEXP = prmStrExpedienteId
                          Select e).FirstOrDefault()
        Return expediente
    End Function

    ''' <summary>
    ''' Método que retorna el último id insertado de la tabla de expedientes
    ''' </summary>
    ''' <returns>Número máximo del campo EFINROEXP</returns>
    Public Function obtenerUltimoExpediente() As Int32
        Dim intNumExpediente = db.EJEFISGLOBAL.Max(Function(e) e.EFINROEXP)
        Return intNumExpediente
    End Function

    ''' <summary>
    ''' Crea un expediente a partir de la entidad de negocio Entidades.EJEFISGLOBAL
    ''' </summary>
    ''' <param name="prmObjExpedienteEntidad">Objeto del tipo Entidades.EJEFISGLOBAL</param>
    ''' <returns>Objdeto del tipo Datos.EJEFISGLOBAL</returns>
    Public Function crearExpediente(ByVal prmObjExpedienteEntidad As Entidades.EJEFISGLOBAL) As Datos.EJEFISGLOBAL
        Dim expediente = ConvertirADatosExpediente(prmObjExpedienteEntidad)
        db.EJEFISGLOBAL.Add(expediente)
        Utils.salvarDatos(db)
        Dim array As ArrayList = New ArrayList
        Dim list As List(Of Char) = prmObjExpedienteEntidad.ToString.ToList
        For Each item In list
            array.Add(item)
        Next

        Utils.ValidaLog(_Auditoria, "INSERT INTO EJEFISGLOBAL ", array)
        Return expediente
    End Function

    ''' <summary>
    ''' Consulta que retorna los expedientes asignados a un gestor (o expedientes asignados a los gestores relacionados con un cordinador o superior) con la páginación y filtros
    ''' </summary>
    ''' <param name="prmObjFiltroExpedientes">Objeto donde se encapsula los datos necesarios para ejecutar el SP SP_OBTENER_EXPEDIENTES_ASIGNADOS</param>
    ''' <returns>Objeto del tipo DataTable con el resultado de ejecutar el SP SP_OBTENER_EXPEDIENTES_ASIGNADOS</returns>
    Public Function obtenerExpedientesAsignados(ByVal prmObjFiltroExpedientes As Entidades.ConsultaExpedientes, Optional prmObjDataTable As DataTable = Nothing) As DataTable
        Dim con As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("SP_OBTENER_EXPEDIENTES_ASIGNADOS", con)
        cmd.CommandType = CommandType.StoredProcedure

        cmd.Parameters.Add(New SqlParameter("@StartRecord", prmObjFiltroExpedientes.StartRecord))
        cmd.Parameters.Add(New SqlParameter("@StopRecord", prmObjFiltroExpedientes.StopRecord))
        cmd.Parameters.Add(New SqlParameter("@SortExpression", prmObjFiltroExpedientes.SortExpression))
        cmd.Parameters.Add(New SqlParameter("@SortDirection", prmObjFiltroExpedientes.SortDirection))
        cmd.Parameters.Add(New SqlParameter("@USULOG", prmObjFiltroExpedientes.UsuarioLogin))
        cmd.Parameters.Add(New SqlParameter("@EFINROEXP", prmObjFiltroExpedientes.NumeroExpediente))
        cmd.Parameters.Add(New SqlParameter("@ED_NOMBRE", prmObjFiltroExpedientes.NombreDeudor))
        cmd.Parameters.Add(New SqlParameter("@EFINIT", prmObjFiltroExpedientes.NumeroDocDeudor))
        cmd.Parameters.Add(New SqlParameter("@ESTADOPROC", prmObjFiltroExpedientes.CodEstadoProcesal))
        cmd.Parameters.Add(New SqlParameter("@MT_TIPO_TITULO", prmObjFiltroExpedientes.CodTipoTitulo))
        cmd.Parameters.Add(New SqlParameter("@FECTITULO", prmObjFiltroExpedientes.FechaEntragaTitulo))
        cmd.Parameters.Add(New SqlParameter("@FECENTGES", prmObjFiltroExpedientes.FechaAsignacionGestor))
        cmd.Parameters.Add(New SqlParameter("@ESTADO_OPERATIVO", prmObjFiltroExpedientes.EstadoOperativo))
        cmd.Parameters.Add(New SqlParameter("@USUNOINCLUIR", prmObjFiltroExpedientes.UsuarioNoIncluir))

        Dim table As DataTable
        If Not IsNothing(prmObjDataTable) Then
            table = prmObjDataTable
        Else
            table = New DataTable()
        End If
        'Dim table As DataTable = New DataTable()
        Dim DataAdapter As SqlDataAdapter = New SqlDataAdapter()
        DataAdapter.SelectCommand = cmd
        DataAdapter.Fill(table)
        Return table
    End Function
End Class

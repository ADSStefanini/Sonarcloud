Public Class Enumeraciones
    Public Enum Roles
        SUPERADMINISTRADOR = 1
        SUPERVISOR = 2
        REVISOR = 3
        GESTORABOGADO = 4
        REPARTIDOR = 5
        VERIFICADORPAGOS = 6
        CREADORUSUARIOS = 7
        GESTORINFORMACION = 8
        GESTORCOBROSCOACTIVO = 9
        ESTUDIOTITULOS = 10
        AREAORIGEN = 11
    End Enum

    Public Enum TipoCartera
        Parafiscales = 1
        Pensional = 2
        Disciplinarios = 3
        Administrativa = 4
    End Enum

    Public Enum AreaOrigen
        DIRECCION_PARAFISCALES = 1
        SUBDIRECCION_PARAFISCALES = 2
        SUBDIRECCION_NOMINA = 3
        SUBDIRECCION_JURIDICA_PENSIONAL = 4
        CONTROL_INTERNO_DISCIPLINARIO = 5
        ADMINISTRATIVA = 6
    End Enum


    Public Enum FuentesDirecciones
        Procesal = 1
        RUT = 2
        RUES = 3
        Otra = 4
    End Enum

    Public Enum TipoTitulo
        Parafiscales = 1
        Pensiones = 2
        Otros = 3
    End Enum

    Public Enum TipoObligacion
        SentenciasJudicioales = 1
    End Enum

    Public Enum FormaNotificacion
        FormaNotificacion = 1
    End Enum

    Public Enum Homologacion
        TipoCartera = 1
        AreaOrigen = 2
        TipoTituloOrigen = 3
        FormaNotificacion = 4
        TipoPersona = 5
        TipoIdentificacion = 6
        Departamento = 7
        Ciudad = 8
        FuenteDireccion = 9
    End Enum

    Public Enum Estado
        INACTIVO = 0
        ACTIVO = 1
    End Enum

    Public Enum Dominio
        TiposNotificacion = 1
        TiposObjeto = 2
        TiposSolicitud = 3
        TiempoCorreccionAreaOrigen = 4
        TiposDeRuta = 5
        AniosUVT = 6
        TiempoMaximoSuspension = 7
        EstadoSolicitud = 8
        EstadoProcesalEstudioTitulos = 9
    End Enum

    Public Enum DominioDetalle
        N_Titulo = 1
        TiposN_RecursoReposicion_SegundaInstanciaObjeto = 2
        N_Apelacion_Reconsideracion_Casacion = 3
        Titulo = 4
        Expediente = 5
        SolicitudSuspension = 6
        SolicitudCambioEstado = 7
        SolicitudPriorizacion = 8
        SolicitudResignacion = 9
        AlertaAmarillaBandejaAreaOrigen = 10
        AlertaRojaBandejaAreaOrigen = 11
        Local = 12
        Documentic = 13
        ANIOS = 14
        TiempoMaximoSuspension = 15
        EnEspera = 16
        Aprobada = 17
        Rechazada = 18
        EstadoProcesalEstudioTitulos = 19
    End Enum

    Public Enum JerarquiaUsuario
        Revisor = 1
        Cordinador = 2
        Superior = 3
    End Enum
End Class

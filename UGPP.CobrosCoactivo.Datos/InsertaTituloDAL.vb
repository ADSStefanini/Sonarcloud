Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Datos

Public Class InsertaTituloDAL

    Inherits AccesObject(Of TituloEjecutivo)

    ''' <summary>
    ''' Entidad de conección a la base de datos
    ''' </summary>
    Dim db As UGPPEntities
    Dim _auditLog As LogAuditoria
    Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()

    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    Public Function Insert(ByVal Titulo As TituloEjecutivo, Optional ByVal prmObjExpedienteId As String = Nothing) As List(Of TituloEjecutivo)
        Parameters = New List(Of SqlClient.SqlParameter)()
        If Not IsNothing(prmObjExpedienteId) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_expediente", prmObjExpedienteId))
        End If

        Parameters.Add(New SqlClient.SqlParameter("@MT_nro_titulo", Titulo.numeroTitulo))
        Parameters.Add(New SqlClient.SqlParameter("@MT_tipo_titulo", Titulo.tipoTitulo))
        Parameters.Add(New SqlClient.SqlParameter("@MT_fec_expedicion_titulo", Titulo.fechaTituloEjecutivo))
        Parameters.Add(New SqlClient.SqlParameter("@MT_fec_notificacion_titulo", Titulo.fechaNotificacion))
        Parameters.Add(New SqlClient.SqlParameter("@MT_for_notificacion_titulo", Titulo.formaNotificacion))
        Parameters.Add(New SqlClient.SqlParameter("@MT_Tip_notificacion", Titulo.CodTipNotificacion))
        'Recursos
        If Not String.IsNullOrEmpty(Titulo.tituloEjecutivoRecursoReposicion.numeroTitulo) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_res_resuelve_reposicion", Titulo.tituloEjecutivoRecursoReposicion.numeroTitulo))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_res_resuelve_reposicion", String.Empty))
        End If
        If Not IsNothing(Titulo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo) And Titulo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo <> Date.MinValue.AddYears(1990) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_expe_resolucion_reposicion", Titulo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_expe_resolucion_reposicion", String.Empty))
        End If

        If Not String.IsNullOrEmpty(Titulo.tituloEjecutivoRecursoReconsideracion.numeroTitulo) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_reso_resu_apela_recon", Titulo.tituloEjecutivoRecursoReconsideracion.numeroTitulo))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_reso_resu_apela_recon", String.Empty))
        End If
        If Not IsNothing(Titulo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo) And Titulo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo <> Date.MinValue.AddYears(1990) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_exp_reso_apela_recon", Titulo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_exp_reso_apela_recon", String.Empty))
        End If

        'Fin Recursos
        If Not IsNothing(Titulo.fechaEjecutoria) And Titulo.fechaEjecutoria <> Date.MinValue.AddYears(1990) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_fecha_ejecutoria", Titulo.fechaEjecutoria))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_fecha_ejecutoria", String.Empty))
        End If

        If Not IsNothing(Titulo.fechaExigibilidad) And Titulo.fechaExigibilidad <> Date.MinValue.AddYears(1990) Then
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_exi_liq", Titulo.fechaExigibilidad))
        Else
            Parameters.Add(New SqlClient.SqlParameter("@MT_fec_exi_liq", String.Empty))
        End If

        'Parameters.Add(New SqlClient.SqlParameter("@MT_fec_exi_liq", Titulo.fechaExigibilidad))
        Parameters.Add(New SqlClient.SqlParameter("@MT_procedencia", Titulo.areaOrigen))
        Parameters.Add(New SqlClient.SqlParameter("@totaldeuda", IIf((Titulo.TotalSancion IsNot Nothing), Titulo.TotalSancion, 0) + IIf((Titulo.totalObligacion IsNot Nothing), Titulo.totalObligacion, 0) + IIf((Titulo.partidaGlobal IsNot Nothing), Titulo.partidaGlobal, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@TotalRepartidor", IIf((Titulo.TotalSancion IsNot Nothing), Titulo.TotalSancion, 0) + IIf((Titulo.totalObligacion IsNot Nothing), Titulo.totalObligacion, 0) + IIf((Titulo.partidaGlobal IsNot Nothing), Titulo.partidaGlobal, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@TotalSancion", IIf((Titulo.TotalSancion IsNot Nothing), Titulo.TotalSancion, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_valor_obligacion", IIf((Titulo.valorTitulo IsNot Nothing), Titulo.valorTitulo, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_partida_global", IIf((Titulo.partidaGlobal IsNot Nothing), Titulo.partidaGlobal, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_sancion_omision", IIf((Titulo.sancionOmision IsNot Nothing), Titulo.sancionOmision, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_sancion_mora", IIf((Titulo.sancionMora IsNot Nothing), Titulo.sancionMora, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_sancion_inexactitud", IIf((Titulo.sancionInexactitud IsNot Nothing), Titulo.sancionInexactitud, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_total_obligacion", IIf((Titulo.totalObligacion IsNot Nothing), Titulo.totalObligacion, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@MT_total_partida_global", IIf((Titulo.partidaGlobal IsNot Nothing), Titulo.partidaGlobal, 0)))
        Parameters.Add(New SqlClient.SqlParameter("@CodTipSentencia", Titulo.CodTipSentencia))
        Parameters.Add(New SqlClient.SqlParameter("@NoExpedienteOrigen", Titulo.numeroExpedienteOrigen))
        Parameters.Add(New SqlClient.SqlParameter("@IdunicoTitulo", Titulo.IdunicoTitulo))
        Parameters.Add(New SqlClient.SqlParameter("@Automatico", Titulo.Automatico))
        Utils.ValidaLog(_auditLog, "SP_InsertaTituloEjecutivo", Parameters.ToArray)
        Return ExecuteList("SP_InsertaTituloEjecutivo", Parameters.ToArray())
    End Function
    Public Function InsertDeudor(ByVal Deudor As Deudor, ByVal ED_IDUNICO_TITULO_MAESTRO As Int32)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ED_TipoId", Deudor.tipoIdentificacion))
        Parameters.Add(New SqlClient.SqlParameter("@ED_Codigo_Nit", Deudor.numeroIdentificacion))
        Parameters.Add(New SqlClient.SqlParameter("@ED_DigitoVerificacion", If(IsNothing(Deudor.digitoVerificacion), String.Empty, Deudor.digitoVerificacion)))
        Parameters.Add(New SqlClient.SqlParameter("@ED_TipoPersona", Deudor.tipoPersona))
        Parameters.Add(New SqlClient.SqlParameter("@ED_Nombre", Deudor.nombreDeudor))
        Parameters.Add(New SqlClient.SqlParameter("@ED_IDUNICO_TITULO_MAESTRO", ED_IDUNICO_TITULO_MAESTRO))
        Parameters.Add(New SqlClient.SqlParameter("@TIPO_DEUDOR", Deudor.TipoEnte))
        Parameters.Add(New SqlClient.SqlParameter("@PARTICIPACION", Deudor.PorcentajeParticipacion))
        Parameters.Add(New SqlClient.SqlParameter("@ED_EstadoPersona", Deudor.EstadoPersona))
        Parameters.Add(New SqlClient.SqlParameter("@ED_TipoAportante", Deudor.TipoAportante))
        Parameters.Add(New SqlClient.SqlParameter("@ED_TarjetaProf", Deudor.TarjetaProfesional))
        Utils.ValidaLog(_auditLog, "EXECUTE SP_InsertaDeudor ", Parameters.ToArray)
        Return ExecuteList("SP_InsertaDeudor", Parameters.ToArray())

    End Function

    Public Function InsertDireccion(ByVal Direccion As DireccionUbicacion)
        Parameters = New List(Of SqlClient.SqlParameter)()
        If Direccion.otrasFuentesDirecciones Is Nothing Then
            Direccion.otrasFuentesDirecciones = String.Empty
        End If
        Parameters.Add(New SqlClient.SqlParameter("@deudor", Direccion.numeroidentificacionDeudor))
        Parameters.Add(New SqlClient.SqlParameter("@Direccion", Direccion.direccionCompleta))
        Parameters.Add(New SqlClient.SqlParameter("@Departamento", Direccion.idDepartamento))
        Parameters.Add(New SqlClient.SqlParameter("@Ciudad", Direccion.idCiudad))
        Parameters.Add(New SqlClient.SqlParameter("@Telefono", If(IsNothing(Direccion.telefono), String.Empty, Direccion.telefono)))
        Parameters.Add(New SqlClient.SqlParameter("@Email", If(IsNothing(Direccion.email), String.Empty, Direccion.email)))
        Parameters.Add(New SqlClient.SqlParameter("@Movil", If(IsNothing(Direccion.celular), String.Empty, Direccion.celular)))
        Parameters.Add(New SqlClient.SqlParameter("@ID_FUENTE", Direccion.fuentesDirecciones))
        Parameters.Add(New SqlClient.SqlParameter("@OTRA_FUENTE", If(IsNothing(Direccion.otrasFuentesDirecciones), String.Empty, Direccion.otrasFuentesDirecciones)))
        Utils.ValidaLog(_auditLog, "SP_InsertaDireccion", Parameters.ToArray)
        Try
            Return ExecuteList("SP_InsertaDireccion", Parameters.ToArray)
        Catch ex As Exception

        End Try

    End Function

    Public Sub InsertDocumentos(ByVal Documento As DocumentoMaestroTitulo, ByVal Id As Int32)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_DOCUMENTO_TITULO", If(Documento.ID_DOCUMENTO_TITULO > 0, Documento.ID_DOCUMENTO_TITULO, String.Empty)))
        Parameters.Add(New SqlClient.SqlParameter("@ID_MAESTRO_TITULO", Id))
        Parameters.Add(New SqlClient.SqlParameter("@DES_RUTA_DOCUMENTO", If(String.IsNullOrEmpty(Documento.DES_RUTA_DOCUMENTO), String.Empty, Documento.DES_RUTA_DOCUMENTO)))
        Parameters.Add(New SqlClient.SqlParameter("@TIPO_RUTA", Documento.TIPO_RUTA))
        Parameters.Add(New SqlClient.SqlParameter("@COD_GUID", If(String.IsNullOrEmpty(Documento.COD_GUID), String.Empty, Documento.COD_GUID)))
        Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_DOCUMENTO_AO", If(String.IsNullOrEmpty(Documento.COD_TIPO_DOCUMENTO_AO), String.Empty, Documento.COD_TIPO_DOCUMENTO_AO)))
        Parameters.Add(New SqlClient.SqlParameter("@NOM_DOC_AO", If(String.IsNullOrEmpty(Documento.NOM_DOC_AO), String.Empty, Documento.NOM_DOC_AO)))
        Parameters.Add(New SqlClient.SqlParameter("@OBSERVA_LEGIBILIDAD", If(String.IsNullOrEmpty(Documento.OBSERVA_LEGIBILIDAD), String.Empty, Documento.OBSERVA_LEGIBILIDAD)))
        Parameters.Add(New SqlClient.SqlParameter("@NUM_PAGINAS", If(Documento.NUM_PAGINAS > 0, Documento.NUM_PAGINAS, String.Empty)))
        Utils.ValidaLog(_auditLog, "SP_InsertaDocumento", Parameters.ToArray)
        ExecuteCommand("SP_InsertaDocumento", Parameters.ToArray)
    End Sub

    Public Function InsertNotificacion(ByVal ID_UNICO_MAESTRO_TITULOS As Int32, ByVal FEC_NOTIFICACION As DateTime, ByVal COD_FOR_NOT As String, ByVal COD_TIPO_NOTIFICACION As Int32)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_UNICO_MAESTRO_TITULOS", ID_UNICO_MAESTRO_TITULOS))
        Parameters.Add(New SqlClient.SqlParameter("@FEC_NOTIFICACION", FEC_NOTIFICACION))
        Parameters.Add(New SqlClient.SqlParameter("@COD_FOR_NOT", COD_FOR_NOT))
        Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_NOTIFICACION", COD_TIPO_NOTIFICACION))
        Utils.ValidaLog(_auditLog, "SP_InsertaNotificacion", Parameters.ToArray)
        Return ExecuteList("SP_InsertaNotificacion", Parameters.ToArray)

    End Function
    Public Sub ActualizarNotificacionTitulo(NotificacionActualizar As NotificacionTitulo)
        Parameters = New List(Of SqlClient.SqlParameter)()
        Dim Notificacion = (From n In db.MAESTRO_TITULOS_FOR_NOTIFICACION
                            Where n.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = NotificacionActualizar.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                            Select n).FirstOrDefault()
        Notificacion.COD_FOR_NOT = NotificacionActualizar.COD_FOR_NOT
        Notificacion.COD_TIPO_NOTIFICACION = NotificacionActualizar.COD_TIPO_NOTIFICACION
        Notificacion.FEC_NOTIFICACION = NotificacionActualizar.FEC_NOTIFICACION
        Parameters.Add(New SqlClient.SqlParameter("@COD_FOR_NOT", NotificacionActualizar.COD_FOR_NOT))
        Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_NOTIFICACION", NotificacionActualizar.COD_TIPO_NOTIFICACION))
        Parameters.Add(New SqlClient.SqlParameter("@FEC_NOTIFICACION", NotificacionActualizar.FEC_NOTIFICACION))
        Utils.ValidaLog(_auditLog, "Update MAESTRO_TITULOS_FOR_NOTIFICACION", Parameters.ToArray)
        Utils.salvarDatos(db)
    End Sub
End Class

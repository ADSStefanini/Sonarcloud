Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades

Public Class ConsultaTituloDAL
    Dim ObservacionCNCgeneral As ObservacionesCNCGralDAL
    Dim ObservacionCNCdocumento As ObservacionesCNCDocDAL
    Dim ObservacionesTipificacion As TipificacionCNCDAL
    Dim DeudoresExpedienteTitulo As DeudoresExpedienteDAL
    Dim context As UGPPEntities
    Dim p As Entidades.TituloEjecutivoExt = New Entidades.TituloEjecutivoExt
    Public Property Respuesta As String
    Dim _AuditLog As LogAuditoria

    Public Sub New()
        context = New UGPPEntities()
        ObservacionCNCgeneral = New ObservacionesCNCGralDAL()
        ObservacionCNCdocumento = New ObservacionesCNCDocDAL()
        ObservacionesTipificacion = New TipificacionCNCDAL()
        DeudoresExpedienteTitulo = New DeudoresExpedienteDAL()
    End Sub

    Public Sub New(ByVal auditLog As LogAuditoria)
        context = New UGPPEntities()
        ObservacionCNCgeneral = New ObservacionesCNCGralDAL()
        ObservacionCNCdocumento = New ObservacionesCNCDocDAL()
        ObservacionesTipificacion = New TipificacionCNCDAL()
        DeudoresExpedienteTitulo = New DeudoresExpedienteDAL()
        _AuditLog = auditLog
    End Sub

    Public Function consultarNotificaciones(codUnico As Int32) As List(Of Datos.MAESTRO_TITULOS_FOR_NOTIFICACION)
        Return (From noti In context.MAESTRO_TITULOS_FOR_NOTIFICACION Where noti.ID_UNICO_MAESTRO_TITULOS = codUnico).ToList()
    End Function

    Public Function consultarTituloEjecutivo(codUnico As Int64) As TituloEjecutivoExt
        Dim context As UGPPEntities = New UGPPEntities
        Dim resultado As Datos.MAESTRO_TITULOS
        Dim deudores As List(Of Datos.ENTES_DEUDORES)
        Dim documentos As List(Of Datos.MAESTRO_TITULOS_DOCUMENTOS)
        Dim notificaciones As List(Of Datos.MAESTRO_TITULOS_FOR_NOTIFICACION)
        Try
            resultado = (From g In context.MAESTRO_TITULOS Where g.idunico = codUnico).FirstOrDefault()

            Dim DeudoresExpedienteList As List(Of DeudoresExpediente) = New List(Of DeudoresExpediente)()
            DeudoresExpedienteList = DeudoresExpedienteTitulo.ConsultaDeudoresTituloExp(codUnico)
            Dim deudor_expedientesStrings = context.SP_OBTENER_DEUDOR_EXPENDIENTE(codUnico)

            deudores = (From deudor In context.ENTES_DEUDORES Where deudor_expedientesStrings.Contains(deudor.ED_Codigo_Nit)).ToList()
            documentos = (From doc In context.MAESTRO_TITULOS_DOCUMENTOS Where doc.ID_MAESTRO_TITULO = codUnico).ToList()
            notificaciones = consultarNotificaciones(codUnico)

            'Llenado del titulo'
            If resultado IsNot Nothing Then
                p.TituloEjecutivo.IdunicoTitulo = codUnico
                p.TituloEjecutivo.numeroTitulo = If(resultado.MT_nro_titulo = Nothing, String.Empty, resultado.MT_nro_titulo)
                p.TituloEjecutivo.numeroExpedienteOrigen = If(String.IsNullOrEmpty(resultado.NoExpedienteOrigen), String.Empty, resultado.NoExpedienteOrigen)
                p.TituloEjecutivo.tipoTitulo = If(resultado.MT_tipo_titulo = Nothing, String.Empty, resultado.MT_tipo_titulo)
                p.TituloEjecutivo.fechaTituloEjecutivo = If(resultado.MT_fec_expedicion_titulo Is Nothing, String.Empty, resultado.MT_fec_expedicion_titulo)
                p.TituloEjecutivo.tituloEjecutivoRecursoReposicion.numeroTitulo = If(resultado.MT_res_resuelve_reposicion = Nothing, String.Empty, resultado.MT_res_resuelve_reposicion)
                p.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaTituloEjecutivo = If(resultado.MT_fec_exp_reso_apela_recon Is Nothing, Date.MinValue, resultado.MT_fec_exp_reso_apela_recon)
                p.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.numeroTitulo = If(resultado.MT_reso_resu_apela_recon = Nothing, String.Empty, resultado.MT_reso_resu_apela_recon)
                p.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaTituloEjecutivo = If(resultado.MT_fec_exp_reso_apela_recon Is Nothing, Date.MinValue, resultado.MT_fec_exp_reso_apela_recon)
                p.TituloEjecutivo.fechaEjecutoria = If(resultado.MT_fecha_ejecutoria Is Nothing, Date.MinValue, resultado.MT_fecha_ejecutoria)
                p.TituloEjecutivo.fechaExigibilidad = If(resultado.MT_fec_exi_liq Is Nothing, Date.MinValue, resultado.MT_fec_exi_liq)
                p.TituloEjecutivo.fechaCaducidadPrescripcion = If(resultado.MT_fec_cad_presc Is Nothing, Nothing, resultado.MT_fec_cad_presc)
                p.TituloEjecutivo.areaOrigen = If(resultado.MT_procedencia Is Nothing, Nothing, resultado.MT_procedencia)
                p.TituloEjecutivo.TotalSancion = If(resultado.MT_totalSancion Is Nothing, 0, resultado.MT_totalSancion)
                p.TituloEjecutivo.valorTitulo = If(resultado.MT_valor_obligacion Is Nothing, 0, resultado.MT_valor_obligacion)
                p.TituloEjecutivo.partidaGlobal = If(resultado.MT_partida_global Is Nothing, 0, resultado.MT_partida_global)
                p.TituloEjecutivo.sancionMora = If(resultado.MT_sancion_mora Is Nothing, 0, resultado.MT_sancion_mora)
                p.TituloEjecutivo.sancionOmision = If(resultado.MT_sancion_omision Is Nothing, 0, resultado.MT_sancion_omision)
                p.TituloEjecutivo.sancionInexactitud = If(resultado.MT_sancion_inexactitud Is Nothing, 0, resultado.MT_sancion_inexactitud)
                p.TituloEjecutivo.totalObligacion = If(resultado.MT_total_obligacion Is Nothing, 0, resultado.MT_total_obligacion)
                p.TituloEjecutivo.partidaGlobal = If(resultado.MT_total_partida_global Is Nothing, 0, resultado.MT_total_partida_global)
                p.TituloEjecutivo.CodTipSentencia = If(resultado.MT_tiposentencia = Nothing, 0, resultado.MT_tiposentencia)
                p.TituloEjecutivo.MT_res_resuelve_reposicion = If(resultado.MT_res_resuelve_reposicion = Nothing, String.Empty, resultado.MT_res_resuelve_reposicion)
                p.TituloEjecutivo.MT_reso_resu_apela_recon = If(resultado.MT_reso_resu_apela_recon = Nothing, String.Empty, resultado.MT_reso_resu_apela_recon)
                p.TituloEjecutivo.Automatico = resultado.Automatico
                Dim lstTipificaciones = ObservacionesTipificacion.obtenerTipificacionCNCtitulo(codUnico)
                If lstTipificaciones.Count() > 0 Then
                    p.LstTipificaciones = New List(Of TipificacionCNC)()
                    For Each itemTipificacion As TIPIFICACION_CNC In lstTipificaciones
                        Dim tp As TipificacionCNC = New TipificacionCNC()
                        tp.ID_TIPIFICACION = itemTipificacion.ID_TIPIFICACION
                        tp.HABILITADO = True
                        p.LstTipificaciones.Add(tp)
                    Next
                End If

            End If
            ' Observacion
            Dim ObservacionTitulo = ObservacionCNCgeneral.obtenerObservacionCNC(p.TituloEjecutivo.IdunicoTitulo).OrderByDescending(Function(x) (x.ID_OBSERVACIONES)).FirstOrDefault()
            If ObservacionTitulo IsNot Nothing Then
                p.ObservacionTitulo = New ObservacionesCNC()
                p.ObservacionTitulo.CUMPLE_NOCUMPLE = ObservacionTitulo.CUMPLE_NOCUMPLE
            End If
            'Llenado Documentos
            If (documentos IsNot Nothing) Then
                p.LstDocumentos = New List(Of DocumentoMaestroTitulo)()
                For Each itemdocumento As MAESTRO_TITULOS_DOCUMENTOS In documentos
                    Dim documento As DocumentoMaestroTitulo = New DocumentoMaestroTitulo()
                    documento = New DocumentoMaestroTitulo
                    documento.ID_MAESTRO_TITULOS_DOCUMENTOS = If(itemdocumento.ID_MAESTRO_TITULOS_DOCUMENTOS = Nothing, Nothing, itemdocumento.ID_MAESTRO_TITULOS_DOCUMENTOS)
                    documento.ID_DOCUMENTO_TITULO = If(itemdocumento.ID_DOCUMENTO_TITULO Is Nothing, 0, itemdocumento.ID_DOCUMENTO_TITULO)
                    documento.DES_RUTA_DOCUMENTO = If(itemdocumento.DES_RUTA_DOCUMENTO = Nothing, String.Empty, itemdocumento.DES_RUTA_DOCUMENTO)
                    documento.TIPO_RUTA = If(itemdocumento.TIPO_RUTA Is Nothing, 0, itemdocumento.TIPO_RUTA)
                    documento.COD_TIPO_DOCUMENTO_AO = If(itemdocumento.COD_TIPO_DOCUMENTO_AO = Nothing, String.Empty, itemdocumento.COD_TIPO_DOCUMENTO_AO)
                    documento.NOM_DOC_AO = If(itemdocumento.NOM_DOC_AO = Nothing, String.Empty, itemdocumento.NOM_DOC_AO)
                    documento.OBSERVA_LEGIBILIDAD = If(itemdocumento.OBSERVA_LEGIBILIDAD = Nothing, String.Empty, itemdocumento.OBSERVA_LEGIBILIDAD)
                    documento.NUM_PAGINAS = If(itemdocumento.NUM_PAGINAS.HasValue = False, String.Empty, itemdocumento.NUM_PAGINAS)
                    documento.COD_GUID = itemdocumento.COD_GUID
                    documento.IND_DOC_SINCRONIZADO = If(itemdocumento.IND_DOC_SINCRONIZADO.HasValue = False, False, itemdocumento.IND_DOC_SINCRONIZADO)
                    Dim observacionDoc = ObservacionCNCdocumento.obtenerObservacionCNCDoc(p.TituloEjecutivo.IdunicoTitulo, itemdocumento.ID_MAESTRO_TITULOS_DOCUMENTOS).OrderByDescending(Function(x) (x.ID_OBSERVACIONESDOC)).FirstOrDefault()
                    If observacionDoc IsNot Nothing Then
                        documento.Observacion = New ObservacionesCNCDoc()
                        documento.Observacion.CUMPLE_NOCUMPLE = observacionDoc.CUMPLE_NOCUMPLE
                        documento.Observacion.ID_OBSERVACIONESDOC = observacionDoc.ID_OBSERVACIONESDOC
                    End If
                    p.LstDocumentos.Add(documento)
                Next
            End If
            'Llenado Notificacion
            If (notificaciones IsNot Nothing) Then
                p.LstNotificacion = New List(Of NotificacionTitulo)()
                For Each itemNoticacion As MAESTRO_TITULOS_FOR_NOTIFICACION In notificaciones
                    Dim notificacionItem As NotificacionTitulo = New NotificacionTitulo()
                    notificacionItem.FEC_NOTIFICACION = If(itemNoticacion.FEC_NOTIFICACION Is Nothing, Nothing, itemNoticacion.FEC_NOTIFICACION)
                    notificacionItem.COD_FOR_NOT = If(itemNoticacion.COD_FOR_NOT = Nothing, String.Empty, itemNoticacion.COD_FOR_NOT)
                    notificacionItem.COD_TIPO_NOTIFICACION = itemNoticacion.COD_TIPO_NOTIFICACION
                    notificacionItem.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                    p.LstNotificacion.Add(notificacionItem)
                Next
                If p.LstNotificacion.Count() > 0 Then
                    p.LstNotificacion = p.LstNotificacion.OrderBy(Function(n) n.ID_MAESTRO_TITULOS_FOR_NOTIFICACION).ToList()
                    Dim idNtitulo As Int64? = Nothing
                    Dim idNrepocision As Int64? = Nothing
                    Dim idNreconsideracion As Int64? = Nothing
                    For Each itemNoticacion As MAESTRO_TITULOS_FOR_NOTIFICACION In notificaciones
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 1 And String.IsNullOrEmpty(p.TituloEjecutivo.formaNotificacion) Then
                            p.TituloEjecutivo.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            p.TituloEjecutivo.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNtitulo = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 2 And String.IsNullOrEmpty(p.TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion) Then
                            p.TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            p.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNrepocision = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 3 And String.IsNullOrEmpty(p.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion) Then
                            p.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            p.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNreconsideracion = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                    Next
                    If idNtitulo IsNot Nothing Then
                        p.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNtitulo))
                    End If
                    If idNrepocision IsNot Nothing Then
                        p.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNrepocision))
                    End If
                    If idNreconsideracion IsNot Nothing Then
                        p.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNreconsideracion))
                    End If
                End If
            End If
            'Llenado Deudor
            If (deudores IsNot Nothing) Then
                Dim tiposEntes As List(Of TIPOS_ENTES) = (From g In context.TIPOS_ENTES).ToList()

                Dim EstadosPersona As List(Of ESTADOS_PERSONA) = (From g In context.ESTADOS_PERSONA).ToList()

                p.LstDeudores = New List(Of Deudor)()
                p.LstDireccionUbicacion = New List(Of DireccionUbicacion)()
                For Each ItemDeudor As ENTES_DEUDORES In deudores
                    Dim deudor As Deudor = New Deudor()
                    deudor.tipoIdentificacion = If(ItemDeudor.ED_TipoId = Nothing, String.Empty, ItemDeudor.ED_TipoId)
                    deudor.numeroIdentificacion = If(ItemDeudor.ED_Codigo_Nit = Nothing, String.Empty, ItemDeudor.ED_Codigo_Nit)
                    deudor.digitoVerificacion = If(ItemDeudor.ED_DigitoVerificacion = Nothing, String.Empty, ItemDeudor.ED_DigitoVerificacion)
                    deudor.tipoPersona = If(ItemDeudor.ED_TipoPersona = Nothing, String.Empty, ItemDeudor.ED_TipoPersona)
                    Dim deudortitulo As DeudoresExpediente = DeudoresExpedienteList.FirstOrDefault(Function(x) (x.deudor = ItemDeudor.ED_Codigo_Nit And x.ID_MAESTRO_TITULOS = codUnico))
                    deudor.TipoEnte = If(deudortitulo Is Nothing, 0, deudortitulo.tipo)
                    deudor.PorcentajeParticipacion = If(deudortitulo Is Nothing, 0, deudortitulo.participacion)
                    deudor.EstadoPersona = If(ItemDeudor.ED_EstadoPersona = Nothing, String.Empty, ItemDeudor.ED_EstadoPersona)
                    deudor.TipoAportante = If(ItemDeudor.ED_TipoAportante = Nothing, String.Empty, ItemDeudor.ED_TipoAportante)
                    deudor.TarjetaProfesional = If(ItemDeudor.ED_TarjetaProf = Nothing, String.Empty, ItemDeudor.ED_TarjetaProf)
                    deudor.nombreDeudor = If(ItemDeudor.ED_Nombre = Nothing, String.Empty, ItemDeudor.ED_Nombre)
                    deudor.NomTipoEnte = If(tiposEntes.FirstOrDefault(Function(x) (x.codigo = deudortitulo.tipo)) Is Nothing, String.Empty, tiposEntes.FirstOrDefault(Function(x) (x.codigo = deudortitulo.tipo)).nombre)
                    deudor.NomtipoPersona = If(ItemDeudor.TIPOS_APORTANTES.nombre = Nothing, String.Empty, ItemDeudor.TIPOS_APORTANTES.nombre)
                    deudor.NomEstadoPersona = If(EstadosPersona.FirstOrDefault(Function(x) (x.codigo = ItemDeudor.ED_EstadoPersona)) Is Nothing, String.Empty, EstadosPersona.FirstOrDefault(Function(x) (x.codigo = ItemDeudor.ED_EstadoPersona)).nombre)
                    deudor.NomTipoAportante = If(tiposEntes.FirstOrDefault(Function(x) (x.codigo = Int32.Parse(ItemDeudor.ED_TipoAportante))) Is Nothing, String.Empty, tiposEntes.FirstOrDefault(Function(x) (x.codigo = Int32.Parse(ItemDeudor.ED_TipoAportante))).nombre)

                    deudor.MatriculaMercantil = If(ItemDeudor.VAL_NO_MATRICULA_MERCANTIL = Nothing, String.Empty, ItemDeudor.VAL_NO_MATRICULA_MERCANTIL)
                    p.LstDeudores.Add(deudor)

                    If ItemDeudor.DIRECCIONES.Count() > 0 Then
                        For Each ItemDireccion As DIRECCIONES In ItemDeudor.DIRECCIONES
                            Dim direccion As DireccionUbicacion = New DireccionUbicacion()
                            direccion.fuentesDirecciones = ItemDireccion.ID_FUENTE
                            direccion.direccionCompleta = ItemDireccion.Direccion
                            direccion.idDepartamento = ItemDireccion.Departamento
                            direccion.idCiudad = ItemDireccion.Ciudad
                            direccion.telefono = ItemDireccion.Telefono
                            direccion.email = ItemDireccion.Email
                            direccion.celular = ItemDireccion.Movil
                            direccion.paginaweb = ItemDireccion.paginaweb
                            direccion.fuentesDirecciones = ItemDireccion.ID_FUENTE
                            direccion.otrasFuentesDirecciones = ItemDireccion.OTRA_FUENTE
                            direccion.numeroidentificacionDeudor = ItemDeudor.ED_Codigo_Nit
                            direccion.idunico = ItemDireccion.idunico
                            p.LstDireccionUbicacion.Add(direccion)
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            Respuesta = "Error al leer un titulo"
        End Try
        Return p
    End Function
    Public Function consultarTituloNotificaciones(codUnico As Int64) As TituloEjecutivoExt
        Dim context As UGPPEntities = New UGPPEntities
        Dim tituloNotificaciones As Entidades.TituloEjecutivoExt = New Entidades.TituloEjecutivoExt
        Dim notificaciones As List(Of Datos.MAESTRO_TITULOS_FOR_NOTIFICACION)
        Try
            notificaciones = consultarNotificaciones(codUnico)
            'Llenado Notificacion
            If (notificaciones IsNot Nothing) Then
                tituloNotificaciones.LstNotificacion = New List(Of NotificacionTitulo)()
                For Each itemNoticacion As MAESTRO_TITULOS_FOR_NOTIFICACION In notificaciones
                    Dim notificacionItem As NotificacionTitulo = New NotificacionTitulo()
                    notificacionItem.FEC_NOTIFICACION = If(itemNoticacion.FEC_NOTIFICACION Is Nothing, Nothing, itemNoticacion.FEC_NOTIFICACION)
                    notificacionItem.COD_FOR_NOT = If(itemNoticacion.COD_FOR_NOT = Nothing, String.Empty, itemNoticacion.COD_FOR_NOT)
                    notificacionItem.COD_TIPO_NOTIFICACION = itemNoticacion.COD_TIPO_NOTIFICACION
                    notificacionItem.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                    tituloNotificaciones.LstNotificacion.Add(notificacionItem)
                Next
                If tituloNotificaciones.LstNotificacion.Count() > 0 Then
                    tituloNotificaciones.LstNotificacion = tituloNotificaciones.LstNotificacion.OrderBy(Function(n) n.ID_MAESTRO_TITULOS_FOR_NOTIFICACION).ToList()
                    Dim idNtitulo As Int64? = Nothing
                    Dim idNrepocision As Int64? = Nothing
                    Dim idNreconsideracion As Int64? = Nothing
                    For Each itemNoticacion As MAESTRO_TITULOS_FOR_NOTIFICACION In notificaciones
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 1 And String.IsNullOrEmpty(tituloNotificaciones.TituloEjecutivo.formaNotificacion) Then
                            tituloNotificaciones.TituloEjecutivo.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            tituloNotificaciones.TituloEjecutivo.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNtitulo = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 2 And String.IsNullOrEmpty(tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion) Then
                            tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNrepocision = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                        If itemNoticacion.COD_TIPO_NOTIFICACION = 3 And String.IsNullOrEmpty(tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion) Then
                            tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion = itemNoticacion.COD_FOR_NOT
                            tituloNotificaciones.TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion = itemNoticacion.FEC_NOTIFICACION
                            idNreconsideracion = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                        End If
                    Next
                    If idNtitulo IsNot Nothing Then
                        tituloNotificaciones.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNtitulo))
                    End If
                    If idNrepocision IsNot Nothing Then
                        tituloNotificaciones.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNrepocision))
                    End If
                    If idNreconsideracion IsNot Nothing Then
                        tituloNotificaciones.LstNotificacion.RemoveAll(Function(x) (x.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = idNreconsideracion))
                    End If
                End If
            End If
        Catch ex As Exception
            Respuesta = "Error al leer un titulo"
        End Try
        Return tituloNotificaciones
    End Function

    Public Function ConsultarDocumentos(codUnico As Int64) As List(Of DocumentoMaestroTitulo)
        Dim context As UGPPEntities = New UGPPEntities
        Dim documentos As List(Of Datos.MAESTRO_TITULOS_DOCUMENTOS)
        Dim LstDocumentos As List(Of DocumentoMaestroTitulo) = New List(Of DocumentoMaestroTitulo)()
        documentos = (From doc In context.MAESTRO_TITULOS_DOCUMENTOS Where doc.ID_MAESTRO_TITULO = codUnico).ToList()
        If (documentos IsNot Nothing And documentos.Count() > 0) Then
            LstDocumentos = New List(Of DocumentoMaestroTitulo)()
            For Each itemdocumento As MAESTRO_TITULOS_DOCUMENTOS In documentos
                Dim documento As DocumentoMaestroTitulo = New DocumentoMaestroTitulo()
                documento = New DocumentoMaestroTitulo
                documento.ID_MAESTRO_TITULOS_DOCUMENTOS = If(itemdocumento.ID_MAESTRO_TITULOS_DOCUMENTOS = Nothing, Nothing, itemdocumento.ID_MAESTRO_TITULOS_DOCUMENTOS)
                documento.ID_DOCUMENTO_TITULO = If(itemdocumento.ID_DOCUMENTO_TITULO Is Nothing, 0, itemdocumento.ID_DOCUMENTO_TITULO)
                documento.DES_RUTA_DOCUMENTO = If(itemdocumento.DES_RUTA_DOCUMENTO = Nothing, String.Empty, itemdocumento.DES_RUTA_DOCUMENTO)
                documento.TIPO_RUTA = If(itemdocumento.TIPO_RUTA Is Nothing, 0, itemdocumento.TIPO_RUTA)
                documento.COD_TIPO_DOCUMENTO_AO = If(itemdocumento.COD_TIPO_DOCUMENTO_AO = Nothing, String.Empty, itemdocumento.COD_TIPO_DOCUMENTO_AO)
                documento.NOM_DOC_AO = If(itemdocumento.NOM_DOC_AO = Nothing, String.Empty, itemdocumento.NOM_DOC_AO)
                documento.OBSERVA_LEGIBILIDAD = If(itemdocumento.OBSERVA_LEGIBILIDAD = Nothing, String.Empty, itemdocumento.OBSERVA_LEGIBILIDAD)
                documento.NUM_PAGINAS = If(itemdocumento.NUM_PAGINAS.HasValue = False, String.Empty, itemdocumento.NUM_PAGINAS)
                documento.COD_GUID = itemdocumento.COD_GUID
                documento.IND_DOC_SINCRONIZADO = If(itemdocumento.IND_DOC_SINCRONIZADO.HasValue = False, False, itemdocumento.IND_DOC_SINCRONIZADO)
                LstDocumentos.Add(documento)
            Next
        End If
        Return LstDocumentos
    End Function
End Class

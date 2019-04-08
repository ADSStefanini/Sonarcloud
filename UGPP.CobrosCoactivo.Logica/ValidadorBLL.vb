Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades


Public Class ValidadorBLL
    Inherits ValidadorUtils

    Private ContadorMensaje As String
    Public Property Idunicotitulo As Int32
    Public Property respuesta As List(Of Entidades.RespuestaMallaValidacion)
    Public Property _DocumentosDAL As DocumentoTipoTituloDAL

    Private Property _Inserta As InsertaTituloDAL
    Private Property _Consulta As ConsultaTituloDAL
    Private Property _AuditEntity As Entidades.LogAuditoria

    Public Sub New()
        _Inserta = New InsertaTituloDAL()
        _Consulta = New ConsultaTituloDAL()
    End Sub

    Public Sub New(ByVal auditData As Entidades.LogAuditoria)
        _AuditEntity = auditData
        _Inserta = New InsertaTituloDAL(_AuditEntity)
        _Consulta = New ConsultaTituloDAL()
    End Sub

    ''' <summary>
    ''' Método para validar todos los titulos por tipo de validación Malla validadora
    ''' </summary>
    ''' <param name="TituloEjecutivo"></param>
    Public Function MallaValidadoraTituloEjecutivoGlobal(ByVal TituloEjecutivo As TituloEjecutivo, ByVal DocumentoMaestroTitulo As List(Of DocumentoMaestroTitulo), ByVal DeudoraValidar As List(Of Deudor), ByVal direccionUbicacion As List(Of DireccionUbicacion), Optional ByVal LsvNotificaciones As List(Of NotificacionTitulo) = Nothing, Optional ByVal valIDTAREA As Int64? = Nothing, Optional ByVal prmBoolInsert As Boolean = True, Optional ByVal prmObjExpedienteId As String = Nothing)

        Try
            'ContadorMensaje = My.Resources.RecursosTitulos.errorGeneral
            ContadorMensaje = String.Empty
            respuesta = New List(Of RespuestaMallaValidacion)
            Dim RespuestaIdunico As List(Of TituloEjecutivo)
            'validar título
            MallaValidadoraTituloEjecutivo(TituloEjecutivo)
            'validar deudor
            MallaValidadoraDeudor(DeudoraValidar)
            'validar ubicación
            MallaValidadoraDireccionUbicacion(direccionUbicacion)
            'validar documento
            If TituloEjecutivo.Automatico = False Then
                MallaValidadorDocumento(DocumentoMaestroTitulo, TituloEjecutivo)
            Else
                MallaValidadorDocumentoA(DocumentoMaestroTitulo, TituloEjecutivo)
            End If

            If LsvNotificaciones IsNot Nothing Then
                MallaValidadoraNotificaciones(LsvNotificaciones)
            End If

            If respuesta.Count = 0 And prmBoolInsert Then
                'If ContadorMensaje = My.Resources.RecursosTitulos.errorGeneral Then
                'Inserta el titulo ejecutivo
                RespuestaIdunico = InsertaTituloEjecutivo(TituloEjecutivo, prmObjExpedienteId)
                Idunicotitulo = RespuestaIdunico(0).IdunicoTitulo

                If Idunicotitulo <> -1 Then
                    'Inserta deudor
                    InsertaDeudor(DeudoraValidar, Idunicotitulo)
                    'Inserta Direccion
                    InsertaDireccion(direccionUbicacion)
                    'Inserta Documento
                    InsertaDocumentos(DocumentoMaestroTitulo, Idunicotitulo)
                    'Inserta Notificaciones Creacion del titulo
                    If TituloEjecutivo.IdunicoTitulo = 0 Then
                        If TituloEjecutivo.fechaNotificacion.HasValue Then
                            InsertaNotificacion(Idunicotitulo, TituloEjecutivo.fechaNotificacion, TituloEjecutivo.formaNotificacion, 1)
                        End If
                        If TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion.HasValue Then
                            InsertaNotificacion(Idunicotitulo, TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion, TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion, 2)
                        End If
                        If TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion.HasValue Then
                            InsertaNotificacion(Idunicotitulo, TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion, TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion, 3)
                        End If

                    End If
                    'Actualizar notificaciones Existentes Subsanar 
                    If Idunicotitulo = 1 Then
                        Dim lstNotificaciones As List(Of Datos.MAESTRO_TITULOS_FOR_NOTIFICACION) = _Consulta.consultarNotificaciones(TituloEjecutivo.IdunicoTitulo).OrderBy(Function(n) n.ID_MAESTRO_TITULOS_FOR_NOTIFICACION).ToList()
                        Dim idNtitulo = 0, idNrepocision = 0, idNreconsideracion = 0
                        For Each itemNoticacion As MAESTRO_TITULOS_FOR_NOTIFICACION In lstNotificaciones
                            If itemNoticacion.COD_TIPO_NOTIFICACION = 1 And idNtitulo = 0 And TituloEjecutivo.fechaNotificacion.HasValue Then
                                Dim ntTitulo = New NotificacionTitulo()
                                ntTitulo.COD_FOR_NOT = TituloEjecutivo.formaNotificacion
                                ntTitulo.FEC_NOTIFICACION = TituloEjecutivo.fechaNotificacion
                                ntTitulo.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                ntTitulo.COD_TIPO_NOTIFICACION = 1
                                idNtitulo = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                ActualizarNotificacionTitulo(ntTitulo)
                            End If
                            If itemNoticacion.COD_TIPO_NOTIFICACION = 2 And idNrepocision = 0 And TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion.HasValue Then
                                Dim ntTitulo = New NotificacionTitulo()
                                ntTitulo.COD_FOR_NOT = TituloEjecutivo.tituloEjecutivoRecursoReposicion.formaNotificacion
                                ntTitulo.FEC_NOTIFICACION = TituloEjecutivo.tituloEjecutivoRecursoReposicion.fechaNotificacion
                                ntTitulo.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                idNrepocision = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                ntTitulo.COD_TIPO_NOTIFICACION = 2
                                ActualizarNotificacionTitulo(ntTitulo)
                            End If
                            If itemNoticacion.COD_TIPO_NOTIFICACION = 3 And idNreconsideracion = 0 And TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion.HasValue Then
                                Dim ntTitulo = New NotificacionTitulo()
                                ntTitulo.COD_FOR_NOT = TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.formaNotificacion
                                ntTitulo.FEC_NOTIFICACION = TituloEjecutivo.tituloEjecutivoRecursoReconsideracion.fechaNotificacion
                                ntTitulo.ID_MAESTRO_TITULOS_FOR_NOTIFICACION = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                ntTitulo.COD_TIPO_NOTIFICACION = 3
                                idNreconsideracion = itemNoticacion.ID_MAESTRO_TITULOS_FOR_NOTIFICACION
                                ActualizarNotificacionTitulo(ntTitulo)
                            End If
                        Next
                        If LsvNotificaciones IsNot Nothing Then
                            For Each itemNotificacion As NotificacionTitulo In LsvNotificaciones
                                ActualizarNotificacionTitulo(itemNotificacion)
                            Next
                        End If
                    End If
                End If
            Else
                Idunicotitulo = 0
            End If

        Catch ex As Exception
            Idunicotitulo = -1  'cuando retorne -1 se debe mostrar un mensaje de error : Error inesperado
            respuesta.Add(CrearRespuestaMallaValidacion("031", " Error controlado", ContadorMensaje))
        End Try
        Return Idunicotitulo
    End Function

    Public Function InsertaTituloEjecutivo(ByVal TituloEjecutivo As TituloEjecutivo, Optional ByVal prmObjExpedienteId As String = Nothing) As List(Of TituloEjecutivo)
        Return _Inserta.Insert(TituloEjecutivo, prmObjExpedienteId)
    End Function

    Public Sub InsertaDeudor(ByVal Deudores As List(Of Deudor), ByVal ED_IDUNICO_TITULO_MAESTRO As Int32)
        For Each ItemDeudores In Deudores
            _Inserta.InsertDeudor(ItemDeudores, ED_IDUNICO_TITULO_MAESTRO)
        Next
    End Sub

    Public Sub InsertaDireccion(ByVal Direcciones As List(Of DireccionUbicacion))
        For Each ItemDirecciones In Direcciones
            _Inserta.InsertDireccion(ItemDirecciones)
        Next
    End Sub

    Public Sub InsertaDocumentos(ByVal Documentos As List(Of DocumentoMaestroTitulo), ByVal idunico As Int32)
        For Each ItemDocumentos In Documentos
            If Not String.IsNullOrEmpty(ItemDocumentos.DES_RUTA_DOCUMENTO) Or Not String.IsNullOrEmpty(ItemDocumentos.COD_GUID) Then
                _Inserta.InsertDocumentos(ItemDocumentos, idunico)
            End If
        Next
    End Sub

    Public Sub InsertaNotificacion(ByVal ID_UNICO_MAESTRO_TITULOS As Int32, ByVal FEC_NOTIFICACION As DateTime, ByVal COD_FOR_NOT As String, ByVal COD_TIPO_NOTIFICACION As Int32)
        _Inserta.InsertNotificacion(ID_UNICO_MAESTRO_TITULOS, FEC_NOTIFICACION, COD_FOR_NOT, COD_TIPO_NOTIFICACION)
    End Sub

    Public Function ConsultarTituloEjecutivo(codUnico As Int32) As TituloEjecutivoExt
        Dim ConsultaTitulo As TituloEjecutivoExt
        ConsultaTitulo = _Consulta.consultarTituloEjecutivo(codUnico)
        Return ConsultaTitulo
    End Function

    Public Function ConsultarTituloNotificacionestivo(codUnico As Int32) As TituloEjecutivoExt
        Dim ConsultaTitulo As TituloEjecutivoExt
        ConsultaTitulo = _Consulta.consultarTituloNotificaciones(codUnico)
        Return ConsultaTitulo
    End Function

    Public Function ConsultarDocumentos(codUnico As Int32) As List(Of DocumentoMaestroTitulo)
        Return _Consulta.ConsultarDocumentos(codUnico)
    End Function

    ''' <summary>
    ''' Función para Malla validadora de titulo ejecutivo simple
    ''' </summary>
    ''' <param name="TituloEjecutivo"></param>
    ''' <returns></returns>
    Public Function MallaValidadoraTituloEjecutivo(ByVal TituloEjecutivo As TituloEjecutivo) As List(Of Entidades.RespuestaMallaValidacion)
        If TituloEjecutivo Is Nothing Then
            respuesta.Add(CrearRespuestaMallaValidacion("027", My.Resources.RecursosTitulos.tituloejecutivo, ContadorMensaje))
        End If

        'Tipo de cartera
        If TituloEjecutivo.tipoCartera = 0 OrElse IsNothing(TituloEjecutivo.tipoCartera) Then
            respuesta.Add(CrearRespuestaMallaValidacion("001", My.Resources.RecursosTitulos.tipocartera, ContadorMensaje))
        End If
        'Tipo de obligación es el mismo tipo titulo
        If TituloEjecutivo.tipoTitulo = 0 OrElse IsNothing(TituloEjecutivo.tipoTitulo) Then
            respuesta.Add(CrearRespuestaMallaValidacion("002", My.Resources.RecursosTitulos.tipoobligacion, ContadorMensaje))
        End If
        'Número de expediente origen 
        If (TituloEjecutivo.numeroExpedienteOrigen = "" Or TituloEjecutivo.areaOrigen Is Nothing) Then
            respuesta.Add(CrearRespuestaMallaValidacion("003", My.Resources.RecursosTitulos.numeroexporigen, ContadorMensaje))
        End If
        'Número de Area origen
        If (TituloEjecutivo.areaOrigen = 0 Or TituloEjecutivo.areaOrigen Is Nothing) Then
            respuesta.Add(CrearRespuestaMallaValidacion("004", My.Resources.RecursosTitulos.areaorigen, ContadorMensaje))
        End If
        'Número del titulo ejecutivo
        If (TituloEjecutivo.numeroTitulo = "" Or TituloEjecutivo.numeroTitulo Is Nothing) Then
            respuesta.Add(CrearRespuestaMallaValidacion("005", My.Resources.RecursosTitulos.numerotituloejecutivo, ContadorMensaje))
        End If
        'Fecha del titulo ejecutivo
        If (TituloEjecutivo.fechaTituloEjecutivo = Nothing Or TituloEjecutivo.fechaTituloEjecutivo < Date.Parse("1/1/1753 12:00:00 AM")) Then
            respuesta.Add(CrearRespuestaMallaValidacion("006", My.Resources.RecursosTitulos.fchtituloejecutivo, ContadorMensaje))
        Else
            If (TituloEjecutivo.fechaTituloEjecutivo > Now) Then
                respuesta.Add(CrearRespuestaMallaValidacion("007", My.Resources.RecursosTitulos.fchtituloejecutivosuperior, ContadorMensaje))
            End If
        End If

        Dim obtenDatosValoresBLL As New ObtenDatosValoresBLL()
        Dim valorObligatorio = True
        Dim valorTitulo As Double = 0
        If Not IsNothing(TituloEjecutivo.tipoTitulo) Then
            Dim ValoresObligacion As Valores = obtenDatosValoresBLL.ConsultaDatValores().Where(Function(x) (x.ID_TIPO_OBLIGACION_VALORES) = TituloEjecutivo.tipoTitulo).FirstOrDefault()
            If (TituloEjecutivo.valorTitulo.HasValue = False And ValoresObligacion.VALOR_OBLIGACION) Or (TituloEjecutivo.valorTitulo.HasValue = True And String.IsNullOrEmpty(TituloEjecutivo.valorTitulo)) Then
                valorObligatorio = False
            End If
            If (TituloEjecutivo.sancionOmision.HasValue = False And ValoresObligacion.SANCION_OMISION) Or (TituloEjecutivo.sancionOmision.HasValue = True And String.IsNullOrEmpty(TituloEjecutivo.sancionOmision)) Then
                valorObligatorio = False
            End If
            If (TituloEjecutivo.sancionMora.HasValue = False And ValoresObligacion.SANCION_MORA) Or (TituloEjecutivo.sancionMora.HasValue = True And String.IsNullOrEmpty(TituloEjecutivo.sancionMora)) Then
                valorObligatorio = False
            End If
            If (TituloEjecutivo.sancionInexactitud.HasValue = False And ValoresObligacion.SANCION_INEXACTITUD) Or (TituloEjecutivo.sancionInexactitud.HasValue = True And String.IsNullOrEmpty(TituloEjecutivo.sancionInexactitud)) Then
                valorObligatorio = False
            End If
            If (TituloEjecutivo.partidaGlobal.HasValue = False And ValoresObligacion.PARTIDA_GLOBAL) Or (TituloEjecutivo.partidaGlobal.HasValue = True And String.IsNullOrEmpty(TituloEjecutivo.partidaGlobal)) Then
                valorObligatorio = False
            End If

            'Se obtiene el total del título
            valorTitulo = If(IsNothing(TituloEjecutivo.valorTitulo), 0, TituloEjecutivo.valorTitulo) + If(IsNothing(TituloEjecutivo.sancionOmision), 0, TituloEjecutivo.sancionOmision) + If(IsNothing(TituloEjecutivo.sancionMora), 0, TituloEjecutivo.sancionMora) + If(IsNothing(TituloEjecutivo.sancionInexactitud), 0, TituloEjecutivo.sancionInexactitud) + If(IsNothing(TituloEjecutivo.partidaGlobal), 0, TituloEjecutivo.partidaGlobal)

        Else
            valorObligatorio = False
        End If

        If valorObligatorio = False OrElse valorTitulo <= 0 Then
            respuesta.Add(CrearRespuestaMallaValidacion("008", My.Resources.RecursosTitulos.valoresObligatorios, ContadorMensaje))
        End If

        'Forma de notificación
        If IsNothing(TituloEjecutivo.formaNotificacion) OrElse TituloEjecutivo.formaNotificacion = -1 Then
            respuesta.Add(CrearRespuestaMallaValidacion("010", My.Resources.RecursosTitulos.formanotificacion, ContadorMensaje))
        End If
        'Fecha de notificación del titulo ejecutivo
        If TituloEjecutivo.fechaNotificacion Is Nothing Or TituloEjecutivo.fechaNotificacion < Date.Parse("1/1/1753 12:00:00 AM") Then
            respuesta.Add(CrearRespuestaMallaValidacion("011", My.Resources.RecursosTitulos.fchnotificaciontituloejec, ContadorMensaje))
        Else
            If (TituloEjecutivo.fechaNotificacion > Now) Then
                respuesta.Add(CrearRespuestaMallaValidacion("012", My.Resources.RecursosTitulos.fchnotificacionsuperior, ContadorMensaje))
            End If
        End If

        If TituloEjecutivo.Automatico Then
            'Si el título es automatico se validan los cuatro posibles recursos que se pueden presentar en el título
            If TituloEjecutivo.presentaRecursoReposicion.HasValue And TituloEjecutivo.presentaRecursoReposicion.Value Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReposicion, "Reposición")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If

            If TituloEjecutivo.presentaRecursoReconsideracion.HasValue And TituloEjecutivo.presentaRecursoReconsideracion.Value Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReconsideracion, "Reconsideracíon")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If

            If Not IsNothing(TituloEjecutivo.existeFalloCasacion) AndAlso (TituloEjecutivo.existeFalloCasacion.HasValue And TituloEjecutivo.existeFalloCasacion.Value) Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoFalloCasacion, "Casación")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If

            If TituloEjecutivo.existeSentenciaSegundaInstancia.HasValue And TituloEjecutivo.existeSentenciaSegundaInstancia.Value Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoSentenciaSegundaInstancia, "Segunda Instancia")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If
        Else
            'Se realiza la validación de los campos adicionales que se ingresan en el título
            'en el caso de que se haya presentado recurso de Resolución Reposición, Reconsideracíon, Segunda Instancia o Casación y llena alguno de los campos se deja como obligatorio todos los otros cuatro campos
            'Los recursos se dividen así Resolución Reposición/ Reconsideracíon, Segunda Instancia/ Casación, es decir solo se puede meter un tipo de estos datos por formulario

            'Primera validación bloque Resolución Reposición/ Reconsideracíon
            If ValidaRecursoTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReposicion) Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReposicion, "Reposición/ Reconsideracíon")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If
            'Segunda validación bloque Segunda Instancia/ Casación
            If ValidaRecursoTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReconsideracion) Then
                Dim _respuestaMallaValidacion = ValidarCompletitudTituloEspecial(TituloEjecutivo.tituloEjecutivoRecursoReconsideracion, " Segunda Instancia/ Casación")
                If _respuestaMallaValidacion.Count > 0 Then
                    respuesta.AddRange(_respuestaMallaValidacion)
                End If
            End If
        End If

        'El titulo se encuentra ejecutoriado (constancia de ejecutoria): -- Se deshabilita valiación, se deja como obligatorio la fecha ejecutoria
        If (TituloEjecutivo.tituloEjecutoriado) Then
            'Fecha de ejecutoria
            If (TituloEjecutivo.fechaEjecutoria Is Nothing Or TituloEjecutivo.fechaEjecutoria < Date.Parse("1/1/1753 12:00:00 AM")) Then
                respuesta.Add(CrearRespuestaMallaValidacion("013", My.Resources.RecursosTitulos.fchejecutoria, ContadorMensaje))
            Else
                If (TituloEjecutivo.fechaEjecutoria > Now) Then
                    respuesta.Add(CrearRespuestaMallaValidacion("014", My.Resources.RecursosTitulos.fchejecutoriasuperior, ContadorMensaje))
                End If
            End If
        End If

        If (TituloEjecutivo.tituloEjecutoriado) Or TituloEjecutivo.tipoCartera <> Enumeraciones.TipoCartera.Parafiscales Then
            'Fecha de exigibilidad
            If (TituloEjecutivo.fechaExigibilidad Is Nothing Or TituloEjecutivo.fechaExigibilidad < Date.Parse("1/1/1753 12:00:00 AM")) Then
                respuesta.Add(CrearRespuestaMallaValidacion("015", My.Resources.RecursosTitulos.fchexigibilidad, ContadorMensaje))
            Else
                If (TituloEjecutivo.fechaExigibilidad > Now) Then
                    respuesta.Add(CrearRespuestaMallaValidacion("016", My.Resources.RecursosTitulos.fchexigibilidadsuperior, ContadorMensaje))
                End If
            End If
        End If

        Return respuesta
    End Function


    ''' <summary>
    ''' Función para Malla validadora solo lso documentos para un titulo automatico
    ''' </summary>
    ''' <param name="TituloEjecutivo"></param>
    ''' <returns></returns>
    Public Function MallaValidadoraTituloAutomaticoDocumentos(ByVal TituloEjecutivo As TituloEjecutivo, ByVal DocumentoMaestroTitulo As List(Of DocumentoMaestroTitulo))

        Try
            'ContadorMensaje = My.Resources.RecursosTitulos.errorGeneral
            ContadorMensaje = String.Empty
            respuesta = New List(Of RespuestaMallaValidacion)
            'validar documento
            MallaValidadorDocumento(DocumentoMaestroTitulo, TituloEjecutivo)

            If respuesta.Count = 0 Then
                Idunicotitulo = TituloEjecutivo.IdunicoTitulo
                If Idunicotitulo <> -1 Then
                    'Inserta Documento
                    InsertaDocumentos(DocumentoMaestroTitulo, Idunicotitulo)
                End If
            Else
                Idunicotitulo = 0
            End If
        Catch ex As Exception
            Idunicotitulo = -1  'cuando retorne -1 se debe mostrar un mensaje de error : Error inesperado
            respuesta.Add(CrearRespuestaMallaValidacion("031", " Error controlado", ContadorMensaje))
        End Try
        Return Idunicotitulo
    End Function

    ''' <summary>
    '''  Función para Malla validadora de documentos títulos
    ''' </summary>
    ''' <param name="Titulo"></param>
    ''' <returns></returns>
    Public Function MallaValidadorDocumento(ByVal DocumentosMaestro As List(Of DocumentoMaestroTitulo), ByVal Titulo As TituloEjecutivo)
        If DocumentosMaestro.Count = 0 Then
            respuesta.Add(CrearRespuestaMallaValidacion("028", My.Resources.RecursosTitulos.documentos, ContadorMensaje))
        End If
        _DocumentosDAL = New DocumentoTipoTituloDAL()
        Dim DocumentoBusqueda As New List(Of DocumentoTipoTitulo)
        DocumentoBusqueda = _DocumentosDAL.consultarDocumentosPorTipo(Titulo.tipoTitulo)
        For Each ItemDocumentoTitulo In DocumentoBusqueda
            If ItemDocumentoTitulo.VAL_OBLIGATORIO = True And ItemDocumentoTitulo.VAL_ESTADO = True Then
                Dim DocumentoValidado = DocumentosMaestro.FirstOrDefault(Function(x) (x.ID_DOCUMENTO_TITULO) = ItemDocumentoTitulo.ID_DOCUMENTO_TITULO)
                If (DocumentoValidado Is Nothing) OrElse (DocumentoValidado IsNot Nothing And String.IsNullOrEmpty(DocumentoValidado.DES_RUTA_DOCUMENTO)) Then
                    respuesta.Add(CrearRespuestaMallaValidacion("017", My.Resources.RecursosTitulos.faltadocumento + " " + ItemDocumentoTitulo.NOMBRE_DOCUMENTO, ContadorMensaje))
                End If
            End If
        Next
        Return respuesta
    End Function

    ''' <summary>
    '''  Función para Malla validadora de documentos títulos desde el automatico
    ''' </summary>
    ''' <param name="Titulo"></param>
    ''' <returns></returns>
    Public Function MallaValidadorDocumentoA(ByVal DocumentosMaestro As List(Of DocumentoMaestroTitulo), ByVal Titulo As TituloEjecutivo)
        If DocumentosMaestro.Count = 0 Then
            respuesta.Add(CrearRespuestaMallaValidacion("028", My.Resources.RecursosTitulos.documentos, ContadorMensaje))
        End If
        For Each ItemDocumentoTitulo In DocumentosMaestro
            If ItemDocumentoTitulo.COD_GUID Is Nothing Or ItemDocumentoTitulo.COD_GUID = "" Then
                respuesta.Add(CrearRespuestaMallaValidacion("033", My.Resources.RecursosTitulos.codguid, ContadorMensaje))
            End If
            If ItemDocumentoTitulo.COD_TIPO_DOCUMENTO_AO Is Nothing Or ItemDocumentoTitulo.COD_TIPO_DOCUMENTO_AO = "" Then
                respuesta.Add(CrearRespuestaMallaValidacion("034", My.Resources.RecursosTitulos.codTipoDocumento, ContadorMensaje))
            End If
            If ItemDocumentoTitulo.DES_RUTA_DOCUMENTO Is Nothing Or ItemDocumentoTitulo.DES_RUTA_DOCUMENTO = "" Then
                respuesta.Add(CrearRespuestaMallaValidacion("035", My.Resources.RecursosTitulos.desrutaDoc, ContadorMensaje))
            End If
        Next
        Return respuesta
    End Function

    ''' <summary>
    '''  Función para Malla validadora de Deudor título
    ''' </summary>
    ''' <param name="Deudor"></param>
    ''' <returns></returns>
    Public Function MallaValidadoraDeudor(ByVal Deudor As List(Of Deudor))

        If Deudor.Count = 0 Then
            respuesta.Add(CrearRespuestaMallaValidacion("029", My.Resources.RecursosTitulos.deudor, ContadorMensaje))
        End If

        For Each ItemMallaValidadoraDeudo In Deudor
            'Tipo de persona
            If (ItemMallaValidadoraDeudo.tipoPersona = "00") Then
                respuesta.Add(CrearRespuestaMallaValidacion("018", My.Resources.RecursosTitulos.tipopersona, ContadorMensaje))
            End If
            'Nombre del deudor
            If (ItemMallaValidadoraDeudo.nombreDeudor = "" Or ItemMallaValidadoraDeudo.nombreDeudor Is Nothing) Then
                respuesta.Add(CrearRespuestaMallaValidacion("019", My.Resources.RecursosTitulos.nombredeudor, ContadorMensaje))
            End If
            'Tipo  de identificación
            If (ItemMallaValidadoraDeudo.tipoIdentificacion = "00") Then
                respuesta.Add(CrearRespuestaMallaValidacion("020", My.Resources.RecursosTitulos.tipoidentificacion, ContadorMensaje))
            End If
            'Número de identificación
            If (ItemMallaValidadoraDeudo.numeroIdentificacion = "" Or ItemMallaValidadoraDeudo.numeroIdentificacion Is Nothing) Then
                respuesta.Add(CrearRespuestaMallaValidacion("021", My.Resources.RecursosTitulos.numeroidentificacion, ContadorMensaje))
            End If

        Next
        Return respuesta
    End Function

    ''' <summary>
    '''  Función para Malla validadora dirección ubicación
    ''' </summary>
    ''' <param name="DireccionUbicacion"></param>
    ''' <returns></returns>
    Public Function MallaValidadoraDireccionUbicacion(ByVal DireccionUbicacion As List(Of DireccionUbicacion))

        If DireccionUbicacion.Count = 0 Then
            respuesta.Add(CrearRespuestaMallaValidacion("030", My.Resources.RecursosTitulos.direcciones, ContadorMensaje))
        End If

        For Each ItemMallaValidadoraUbica In DireccionUbicacion

            'Dirección completa
            If (ItemMallaValidadoraUbica.direccionCompleta = "" Or ItemMallaValidadoraUbica.direccionCompleta Is Nothing) Then
                respuesta.Add(CrearRespuestaMallaValidacion("022", My.Resources.RecursosTitulos.direccioncompleta, ContadorMensaje))
            End If

            'Departamento
            If (ItemMallaValidadoraUbica.idDepartamento = "") Then
                respuesta.Add(CrearRespuestaMallaValidacion("023", My.Resources.RecursosTitulos.departamento, ContadorMensaje))
            End If

            'Ciudad
            If (ItemMallaValidadoraUbica.idCiudad = "") Then
                respuesta.Add(CrearRespuestaMallaValidacion("024", My.Resources.RecursosTitulos.ciudad, ContadorMensaje))
            End If

            'Fuente de la dirección
            If Not (ItemMallaValidadoraUbica.fuentesDirecciones > 0) Then
                respuesta.Add(CrearRespuestaMallaValidacion("025", My.Resources.RecursosTitulos.fuendireccion, ContadorMensaje))
            Else
                If ItemMallaValidadoraUbica.fuentesDirecciones = 4 And (ItemMallaValidadoraUbica.otrasFuentesDirecciones = "" Or ItemMallaValidadoraUbica.otrasFuentesDirecciones Is Nothing) Then
                    respuesta.Add(CrearRespuestaMallaValidacion("026", My.Resources.RecursosTitulos.otrasfuentesdireccion, ContadorMensaje))
                End If
            End If

        Next
        Return respuesta
    End Function

    Private Function MallaValidadoraNotificaciones(LsvNotificaciones As List(Of NotificacionTitulo))
        For Each itemNotificacion As NotificacionTitulo In LsvNotificaciones
            If itemNotificacion.COD_FOR_NOT = -1 Then
                respuesta.Add(CrearRespuestaMallaValidacion("010", My.Resources.RecursosTitulos.formanotificacion, ContadorMensaje))
            End If
            'Fecha de notificación del titulo ejecutivo
            If itemNotificacion.FEC_NOTIFICACION Is Nothing Or itemNotificacion.FEC_NOTIFICACION < Date.Parse("1/1/1753 12:00:00 AM") Then
                respuesta.Add(CrearRespuestaMallaValidacion("032", My.Resources.RecursosTitulos.fchenotificacion, ContadorMensaje))
            Else
                If (itemNotificacion.FEC_NOTIFICACION > Now) Then
                    respuesta.Add(CrearRespuestaMallaValidacion("012", My.Resources.RecursosTitulos.fchnotificacionsuperior, ContadorMensaje))
                End If
            End If
        Next
        Return respuesta
    End Function

    Public Sub ActualizarNotificacionTitulo(NotificacionActualizar As NotificacionTitulo)
        _Inserta.ActualizarNotificacionTitulo(NotificacionActualizar)
    End Sub

End Class
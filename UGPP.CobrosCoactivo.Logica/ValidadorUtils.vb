Imports UGPP.CobrosCoactivo.Entidades

Public Class ValidadorUtils

    ''' <summary>
    ''' Valida si un título presenta recurso del tipo Reposición/ Reconsideracíon ó Segunda Instancia/ Casación
    ''' </summary>
    ''' <param name="prmObjTituloEspecial">Objeto del tipo Entidades.TituloEspecial</param>
    ''' <returns>Verdadero si alguno de los campos no se encuentra vacio, falso en caso contrario</returns>
    Public Function ValidaRecursoTituloEspecial(ByVal prmObjTituloEspecial As TituloEspecial) As Boolean
        If prmObjTituloEspecial.numeroTitulo <> "" OrElse Not String.IsNullOrEmpty(prmObjTituloEspecial.numeroTitulo) Then
            Return True
        End If
        If IsDate(prmObjTituloEspecial.fechaTituloEjecutivo) OrElse Not IsNothing(prmObjTituloEspecial.fechaTituloEjecutivo) Then
            Return True
        End If
        If IsDate(prmObjTituloEspecial.fechaNotificacion) OrElse Not IsNothing(prmObjTituloEspecial.fechaNotificacion) Then
            Return True
        End If
        If prmObjTituloEspecial.numeroTitulo <> "" OrElse Not String.IsNullOrEmpty(prmObjTituloEspecial.formaNotificacion) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Valida si un título especial tiene todos sus campos llenos
    ''' </summary>
    ''' <param name="prmObjTituloEspecial">Objeto del tipo Entidades.TituloEspecial</param>
    ''' <param name="prmStrNombreTitulo">Se espera "Reposición/ Reconsideracíon" ó Segunda "Instancia/ Casación"</param>
    ''' <returns>Objeto del tipo Entidades.RespuestaMallaValidacion</returns>
    Public Function ValidarCompletitudTituloEspecial(ByVal prmObjTituloEspecial As TituloEspecial, ByVal prmStrNombreTitulo As String, Optional ByVal prmStrMensajeBase As String = "") As List(Of RespuestaMallaValidacion)
        Dim respuesta As New List(Of RespuestaMallaValidacion)

        'Número titulo especial
        If (prmObjTituloEspecial.numeroTitulo = "" OrElse prmObjTituloEspecial.numeroTitulo Is Nothing) Then
            respuesta.Add(CrearRespuestaMallaValidacion("036", "Falta Número del título " & prmStrNombreTitulo, prmStrMensajeBase))
        End If
        'Fecha titulo especial
        If (IsNothing(prmObjTituloEspecial.fechaTituloEjecutivo) OrElse prmObjTituloEspecial.fechaTituloEjecutivo < Date.Parse("1/1/1753 12:00:00 AM")) Then
            respuesta.Add(CrearRespuestaMallaValidacion("037", "Falta Fecha Resolución " & prmStrNombreTitulo, prmStrMensajeBase))
        Else
            If (prmObjTituloEspecial.fechaTituloEjecutivo > Now) Then
                respuesta.Add(CrearRespuestaMallaValidacion("038", "Fecha Resolución " & prmStrNombreTitulo & " es mayor a la fecha actual", prmStrMensajeBase))
            End If
        End If

        'Forma de notificación titulo especial
        If IsNothing(prmObjTituloEspecial.formaNotificacion) OrElse prmObjTituloEspecial.formaNotificacion = "" Then
            respuesta.Add(CrearRespuestaMallaValidacion("039", "Falta Forma de Notificación Resolución " & prmStrNombreTitulo, prmStrMensajeBase))
        End If
        'Fecha de notificación titulo especial
        If prmObjTituloEspecial.fechaNotificacion Is Nothing OrElse prmObjTituloEspecial.fechaNotificacion < Date.Parse("1/1/1753 12:00:00 AM") Then
            respuesta.Add(CrearRespuestaMallaValidacion("040", "Falta Fecha de Notificación Resolución " & prmStrNombreTitulo, prmStrMensajeBase))
        Else
            If (prmObjTituloEspecial.fechaNotificacion > Now) Then
                respuesta.Add(CrearRespuestaMallaValidacion("041", "Fecha de Notificación Resolución " & prmStrNombreTitulo & " es mayor a la fecha actual actual", prmStrMensajeBase))
            End If
        End If

        Return respuesta
    End Function


    ''' <summary>
    ''' Crea y retorna un objeto del tipo Entidades.RespuestaMallaValidacion
    ''' </summary>
    ''' <param name="prmStrCodigoRespuesta">Código de la respuesta, normalmente representado en tres digotos llenados a la izquierda con ceros</param>
    ''' <param name="prmStrTextoRespuesta">Mensaje descriptivo de la validación que no se cumple</param>
    ''' <param name="prmStrMensajeBase">Mensaje base comun a todos los mensajes</param>
    ''' <returns></returns>
    Public Function CrearRespuestaMallaValidacion(ByVal prmStrCodigoRespuesta As String, ByVal prmStrTextoRespuesta As String, Optional ByVal prmStrMensajeBase As String = "") As RespuestaMallaValidacion
        Dim _respuestaMallaValidacion As New RespuestaMallaValidacion()
        _respuestaMallaValidacion.codigo = prmStrCodigoRespuesta
        _respuestaMallaValidacion.respuesta = prmStrMensajeBase & prmStrTextoRespuesta
        Return _respuestaMallaValidacion
    End Function
End Class

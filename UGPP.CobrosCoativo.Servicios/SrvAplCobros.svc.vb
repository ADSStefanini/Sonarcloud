Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class SrvAplCobros
    Implements ISrvAplCobros
    ''' <summary>
    ''' Método de entrada del servicio
    ''' </summary>
    ''' <param name="request"></param>
    ''' <returns></returns>
    Public Function OpIniciarInstancia(request As ContextoTransaccionalRequest) As UGPP.CobrosCoactivo.Entidades.ResponseContract Implements ISrvAplCobros.OpIniciarInstancia
        Dim response As New UGPP.CobrosCoactivo.Entidades.ResponseContract()
        Dim serviceLogic As New SrvAplCobrosBLL
        Try
            If request.contextoTransaccionalTipo IsNot Nothing Then
                'Se valida que el contexto transaccional este correctamente definido
                Dim validacionContextoTransaccional = serviceLogic.ValidarContextoTransaccional(request.contextoTransaccionalTipo)
                If (validacionContextoTransaccional.Count > 0) Then
                    response.codigoError = "ERROR"
                    response.detalleError = serviceLogic.FormatearRespuesta(validacionContextoTransaccional)
                Else
                    'Se procesa la solicitud de l acreación del título
                    response = serviceLogic.CrearTituloAutomatico(request)
                End If

            Else
                response.codigoError = "ERROR"
                response.detalleError = "Falta Conexto Transaccional"
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return response
    End Function
End Class

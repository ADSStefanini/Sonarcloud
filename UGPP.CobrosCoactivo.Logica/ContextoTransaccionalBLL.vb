Imports AutoMapper
Imports UGPP.CobrosCoactivo.Datos
Imports UGPP.CobrosCoactivo.Entidades
Public Class ContextoTransaccionalBLL
    Private Property dataAccess As ContextoTransaccionalDAL
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        dataAccess = New ContextoTransaccionalDAL()
    End Sub
    ''' <summary>
    ''' Obtiene el contexto Transaccional por idTitulo
    ''' </summary>
    ''' <param name="idTitulo"></param>
    ''' <returns></returns>
    Public Function ObtenerContextoPorIdTitulo(ByVal idTitulo As Integer) As Entidades.ContextoTransaccion
        Dim contextDal As Datos.CONTEXTO_TRANSACCIONAL
        Dim contexto As New Entidades.ContextoTransaccion
        contextDal = dataAccess.ObtenerContextoPorIdTitulo(idTitulo)
        Dim config As New MapperConfiguration(Function(cfg)
                                                  Return cfg.CreateMap(Of Datos.CONTEXTO_TRANSACCIONAL, Entidades.ContextoTransaccion)()
                                              End Function)
        Dim IMapper = config.CreateMapper()
        contexto = IMapper.Map(Of Datos.CONTEXTO_TRANSACCIONAL, Entidades.ContextoTransaccion)(contextDal)

        Return contexto
    End Function
End Class

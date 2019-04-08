Imports System.Configuration
Imports UGPP.CobrosCoactivo.Entidades


Public Class ObtenDatosValoresDAL

    Inherits AccesObject(Of Valores)

    Dim _auditLog As LogAuditoria
    Dim db As UGPPEntities
    Public Sub New()
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
    End Sub
    Public Sub New(ByVal auditLog As LogAuditoria)
        ConnectionString = ConfigurationManager.ConnectionStrings("ConnectionString").ToString()
        db = New UGPPEntities()
        _auditLog = auditLog
    End Sub
    ''' <summary>
    ''' Funcion para el llamado de los datos del gridview de Valores
    ''' </summary>
    ''' <returns></returns>
    Public Function ConsultaDatValores() As List(Of Valores)
        Return ExecuteList("SP_ObtenDatosValores")
    End Function
    ''' <summary>
    ''' Realiza el llamado al stored procedure que realiza el update de la parametrización de valores
    ''' </summary>
    ''' <param name="ID_TIPO_OBLIGACION_VALORES"></param>
    ''' <param name="VALOR_OBLIGACION"></param>
    ''' <param name="PARTIDA_GLOBAL"></param>
    ''' <param name="SANCION_OMISION"></param>
    ''' <param name="SANCION_INEXACTITUD"></param>
    ''' <param name="SANCION_MORA"></param>
    Public Sub InsertaDatValores(ByVal ID_TIPO_OBLIGACION_VALORES As String, ByVal VALOR_OBLIGACION As Boolean, ByVal PARTIDA_GLOBAL As Boolean, ByVal SANCION_OMISION As Boolean, ByVal SANCION_INEXACTITUD As Boolean, ByVal SANCION_MORA As Boolean)
        Dim Parameters As List(Of SqlClient.SqlParameter) = New List(Of SqlClient.SqlParameter)()
        Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_OBLIGACION_VALORES", ID_TIPO_OBLIGACION_VALORES))
        Parameters.Add(New SqlClient.SqlParameter("@VALOR_OBLIGACION", VALOR_OBLIGACION))
        Parameters.Add(New SqlClient.SqlParameter("@PARTIDA_GLOBAL", PARTIDA_GLOBAL))
        Parameters.Add(New SqlClient.SqlParameter("@SANCION_OMISION", SANCION_OMISION))
        Parameters.Add(New SqlClient.SqlParameter("@SANCION_INEXACTITUD", SANCION_INEXACTITUD))
        Parameters.Add(New SqlClient.SqlParameter("@SANCION_MORA", SANCION_MORA))
        ExecuteCommand("SP_GuardaDatosValores", Parameters.ToArray)
        Utils.ValidaLog(_auditLog, "EXECUTE SP_GuardaDatosValores", Parameters.ToArray)
    End Sub

End Class

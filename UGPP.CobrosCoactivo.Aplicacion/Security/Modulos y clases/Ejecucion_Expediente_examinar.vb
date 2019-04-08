Imports System.Data.SqlClient

Public Class Ejecucion_Expediente_examinar
    Private MyConexion As SqlConnection

    Public Property Conexion() As SqlConnection
        Get
            Return MyConexion
        End Get
        Set(ByVal value As SqlConnection)
            MyConexion = value
        End Set
    End Property

    Public Sub New(ByVal Conexion As SqlConnection, ByVal Etapa As String)
        MyConexion = Conexion
    End Sub

    'Muestra expediente tal cual esta en la base de datos
    Public Function ExamenCompleto(ByVal Expediente As String) As DataTable
        If Not MyConexion Is Nothing Then
            Dim Adap As SqlDataAdapter = New SqlDataAdapter("select entidad,entesdbf.nombre,idacto,actuaciones.nombre as nombreacto,nomarchivo,paginas,fecharadic,docfechadoc,docexpediente,docObservaciones  from documentos,entesdbf,actuaciones where docexpediente = @EXPEDIENTE and entidad = codigo_nit  and codigo = idacto", MyConexion)
            Adap.SelectCommand.Parameters.AddWithValue("@EXPEDIENTE", Expediente)
            Dim Table As DataTable = New DataTable
            Adap.Fill(Table)
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    'Muestra expediente tal cual esta en la base de datos
    Public Function ExamenCompleto(ByVal Expediente As String, ByVal Etapa As String) As DataTable
        If Not MyConexion Is Nothing Then
            Dim Adap As SqlDataAdapter = New SqlDataAdapter("select entidad,ED_Nombre AS nombre,idacto,b.nombre as nombreacto,nomarchivo,paginas,fecharadic,docfechadoc,docexpediente,docObservaciones,e.nombre AS nometapa,docacumulacio,docusuario FROM documentos,EJEFISGLOBAL,actuaciones b,etapas e, ENTES_DEUDORES c WHERE docexpediente = @EXPEDIENTE and EFINROEXP = DOCEXPEDIENTE and b.codigo = idacto and idetapa = @ETAPA and idetapa = e.codigo and c.ED_Codigo_Nit = EFINIT ", MyConexion)
            Adap.SelectCommand.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = Expediente
            Adap.SelectCommand.Parameters.Add("@ETAPA", SqlDbType.VarChar).Value = Etapa

            Dim Table As DataTable = New DataTable
            Adap.Fill(Table)
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    'Muestra expediente tal cual esta en la base de datos
    Public Function ExamenCompletocedula(ByVal cedula As String) As DataTable
        If Not MyConexion Is Nothing Then
            Dim Adap As SqlDataAdapter = New SqlDataAdapter("select entidad,entesdbf.nombre,idacto,actuaciones.nombre as nombreacto,nomarchivo,paginas,fecharadic,docfechadoc,docexpediente,docObservaciones from documentos,entesdbf,actuaciones where entidad = @cedula and entidad = codigo_nit  and codigo = idacto order by docexpediente", MyConexion)
            Adap.SelectCommand.Parameters.Add("@cedula", SqlDbType.VarChar)
            Adap.SelectCommand.Parameters("@cedula").Value = cedula

            Dim Table As DataTable = New DataTable
            Adap.Fill(Table)
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    Public Function NoExpediente(ByVal Expediente As String) As DataTable
        Dim table As DataTable = New DataTable("NoExpediente")
        Dim column As DataColumn
        ' Dim row As DataRow

        column = New DataColumn()
        column.ColumnName = "entidad"
        Return table
    End Function
End Class



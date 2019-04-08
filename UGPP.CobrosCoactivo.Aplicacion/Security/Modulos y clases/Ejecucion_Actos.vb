Imports System.Data.SqlClient
Public Class Ejecucion_Actos
    Private MyConexion As SqlConnection
    '''Private MyTblActos As DataTable
    Private MyTblSecuencia As DataTable
    Private MyTblDias As DataTable
    Private MyTblProyeccion As DatasetForm.ProcesoExpedienteDataTable
    Private tipo As Byte
    Private varEtapa As String

    Public Property Etapa() As String
        Get
            Return varEtapa
        End Get
        Set(ByVal value As String)
            varEtapa = value
        End Set
    End Property

    Public Property Conexion() As SqlConnection
        Get
            Return MyConexion
        End Get
        Set(ByVal value As SqlConnection)
            MyConexion = value
        End Set
    End Property

    Public ReadOnly Property Datos_Proyeccion() As DatasetForm.ProcesoExpedienteDataTable
        Get
            Return MyTblProyeccion
        End Get
    End Property

    Public Sub New(ByVal Conexion As SqlConnection, ByVal Etapa As String)
        tipo = 0
        MyConexion = Conexion
        MyTblProyeccion = New DatasetForm.ProcesoExpedienteDataTable
        ' ''MyTblActos = Load_Actos(Etapa)
        MyTblSecuencia = Load_Secuencia_Actos()
        MyTblDias = Load_Dias_No_Habiles()
    End Sub

    Public Sub New(ByVal Conexion As SqlConnection, ByVal Etapa As String, ByVal xtipo As Byte)
        tipo = xtipo
        varEtapa = Etapa
        MyConexion = Conexion
        MyTblProyeccion = New DatasetForm.ProcesoExpedienteDataTable
        ' ''MyTblActos = Load_Actos(Etapa)
        MyTblSecuencia = Load_Secuencia_Actos()
        MyTblDias = Load_Dias_No_Habiles()
    End Sub

    ' ''Private Function Load_Actos(ByVal Etapa As String) As DataTable
    ' ''    If Not MyConexion Is Nothing Then
    ' ''        Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT CODIGO, NOMBRE, TERMINO,ACTORDEN FROM ACTUACIONES WHERE IDETAPA = @ETAPA AND NOT ACTORDEN IS NULL ORDER BY ACTORDEN", MyConexion)
    ' ''        Dim Table As DataTable = New DataTable
    ' ''        Adap.SelectCommand.Parameters.Add("@ETAPA", SqlDbType.VarChar)
    ' ''        Adap.SelectCommand.Parameters("@ETAPA").Value = Etapa
    ' ''        Adap.Fill(Table)
    ' ''        Return Table
    ' ''    Else
    ' ''        Throw New Exception("No se ha Inicializado la Conexion")
    ' ''    End If
    ' ''End Function

    Private Function Load_Secuencia_Actos() As DataTable
        If Not MyConexion Is Nothing Then
            Dim Table As DataTable = New DataTable
            If tipo = 0 Then
                Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM DEPENDENCIA_ACTUACIONES ORDER BY dep_orden ASC", MyConexion)
                Adap.Fill(Table)
            ElseIf tipo = 1 Then
                Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM DEPENDENCIA_ACTUACIONES WHERE DEP_ETAPA = @ETAPA ORDER BY dep_orden ASC", MyConexion)
                Adap.SelectCommand.Parameters.Add("@ETAPA", SqlDbType.VarChar)
                Adap.SelectCommand.Parameters("@ETAPA").Value = varEtapa
                Adap.Fill(Table)
            End If
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    Private Function Load_Dias_No_Habiles() As DataTable
        If Not MyConexion Is Nothing Then
            Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT * FROM TDIAS_FESTIVOS ORDER BY FECHA", MyConexion)
            Dim Table As DataTable = New DataTable
            Adap.Fill(Table)
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    Private Function Load_Documentos_Expediente(ByVal Expediente As String) As DataTable
        If Not MyConexion Is Nothing Then
            Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT EFINIT AS ENTIDAD,EFINOM AS NOMBRE,IDACTO, FECHARADIC, NOMARCHIVO, DOCFECHADOC,PAGINAS,DOCOBSERVACIONES FROM DOCUMENTOS,EJEFISGLOBAL WHERE DOCEXPEDIENTE = @EXPEDIENTE AND EFIGEN = DOCPREDIO_REFECATRASTAL AND EFINROEXP = DOCEXPEDIENTE", MyConexion)
            Adap.SelectCommand.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar)
            Adap.SelectCommand.Parameters("@EXPEDIENTE").Value = Expediente
            Dim Table As DataTable = New DataTable
            Adap.Fill(Table)
            Return Table
        Else
            Throw New Exception("No se ha Inicializado la Conexion")
        End If
    End Function

    'Private Function Load_Documentos_Expediente(ByVal Expediente As String) As DataTable
    '    If Not MyConexion Is Nothing Then
    '        Dim Adap As SqlDataAdapter = New SqlDataAdapter("SELECT IDACTO, FECHARADIC, NOMARCHIVO, DOCFECHADOC FROM DOCUMENTOS WHERE ENTIDAD = @EXPEDIENTE", MyConexion)
    '        Adap.SelectCommand.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar)
    '        Adap.SelectCommand.Parameters("@EXPEDIENTE").Value = Expediente
    '        Dim Table As DataTable = New DataTable
    '        Adap.Fill(Table)
    '        Return Table
    '    Else
    '        Throw New Exception("No se ha Inicializado la Conexion")
    '    End If
    'End Function

    Private Function Nueva_Fecha_Acto(ByVal Fecha As Date) As Date
        Dim Inc As Integer = 0
        Do
            Fecha = Fecha.AddDays(Inc)
            If MyTblDias.Select("FECHA= '" & Fecha.Date & "'").Length > 0 OrElse _
               Fecha.DayOfWeek = DayOfWeek.Sunday Then
                If Fecha.DayOfWeek = DayOfWeek.Friday Then
                    Inc = 3
                Else
                    Inc = 1
                End If
            ElseIf Fecha.DayOfWeek = DayOfWeek.Saturday Then
                Inc = 2
            Else
                Inc = 0
            End If
        Loop While Inc > 0
        Return Fecha
    End Function

    Private Function Nueva_Fecha_Acto(ByVal Fecha As Date, ByVal termino As Integer) As Date
        Dim Inc As Integer = 0
        Dim incp As Integer = 0

        Do
            Fecha = Fecha.AddDays(Inc)
            If MyTblDias.Select("FECHA= '" & Fecha.Date & "'").Length > 0 Or Fecha.DayOfWeek = DayOfWeek.Sunday Then
                If Fecha.DayOfWeek = DayOfWeek.Friday Then
                    Inc = 3
                Else
                    Inc = 1
                End If
            ElseIf Fecha.DayOfWeek = DayOfWeek.Saturday Then
                Inc = 2
            Else
                Inc = 1
                incp += 1
            End If
        Loop While incp < termino
        Return Fecha
    End Function

    Public Function Proyeccion(ByVal Expediente As String) As DataTable
        With Me
            If MyTblSecuencia.Rows.Count = 0 Then
                Return Nodatos()
            End If

            Dim TblDocExp As DataTable = .Load_Documentos_Expediente(Expediente)
            Dim FechaApl As Date

            Dim Acto As DataRow = MyTblSecuencia.Rows(0)
            Dim FechaActSig As Date
            Dim FechaPrevia As Date
            Dim NomArchivo As String
            Dim paginas As String = ""
            Dim Entidad As String = ""
            Dim Nombre As String = ""
            Dim xt As Integer = 0
            Dim Salida As Boolean = False
            Dim docObservaciones As String = ""

            Dim ListActos As New List(Of String)

            While Not Salida
                Dim ActApl As DataRow() = TblDocExp.Select("IDACTO ='" & Acto.Item("DEP_CODACTO") & "'")
                Dim Sw As Boolean = False
                If Not ListActos.Contains(Acto.Item("DEP_CODACTO")) AndAlso ActApl.Length = 1 Then
                    ListActos.Add(Acto.Item("DEP_CODACTO"))
                    FechaApl = valorNull(ActApl(0).Item("FECHARADIC"), Date.Now)
                    NomArchivo = ActApl(0).Item("NOMARCHIVO")
                    paginas = ActApl(0).Item("PAGINAS")
                    Entidad = ActApl(0).Item("ENTIDAD")
                    Nombre = ActApl(0).Item("NOMBRE")
                    docObservaciones = ActApl(0).Item("DOCOBSERVACIONES")
                    'FechaActSig = .Nueva_Fecha_Acto(FechaApl.AddDays(Acto.Item("DEP_TERMINO")))
                    FechaActSig = .Nueva_Fecha_Acto(FechaApl, Acto.Item("DEP_TERMINO"))

                    If .MyTblProyeccion.Rows.Count = 0 Then
                        .MyTblProyeccion.AddProcesoExpedienteRow(Acto.Item("DEP_CODACTO"), Acto.Item("DEP_NOMBREPPAL"), FechaApl, Nothing, 0, Acto.Item("DEP_TERMINO"), NomArchivo, paginas, Entidad, Nombre, docObservaciones)
                    Else
                        .MyTblProyeccion.AddProcesoExpedienteRow(Acto.Item("DEP_CODACTO"), Acto.Item("DEP_NOMBREPPAL"), FechaApl, FechaPrevia, DateDiff(DateInterval.Day, FechaActSig, FechaApl), Acto.Item("DEP_TERMINO"), NomArchivo, paginas, Entidad, Nombre, docObservaciones)
                    End If

                    For Each RowSecu As DataRow In MyTblSecuencia.Select("DEP_CODACTO='" & Acto.Item("DEP_CODACTO") & "'")
                        Dim ActSec As DataRow() = TblDocExp.Select("IDACTO ='" & RowSecu.Item("DEP_DEPENDENCIA") & "'")
                        If ActSec.Length = 1 Then
                            'Acto = Secuencia.Select("DEP_CODACTO='" & Acto.Item("DEP_DEPENDENCIA") & "'")(0)
                            If MyTblSecuencia.Select("DEP_CODACTO='" & RowSecu.Item("DEP_DEPENDENCIA") & "'").Length = 0 Then
                                .MyTblProyeccion.AddProcesoExpedienteRow(RowSecu.Item("DEP_DEPENDENCIA"), RowSecu.Item("DEP_DESCRIPCION"), FechaApl, FechaPrevia, DateDiff(DateInterval.Day, FechaActSig, FechaApl), Acto.Item("DEP_TERMINO"), NomArchivo, paginas, Entidad, Nombre, docObservaciones)
                                Sw = False
                                Exit For
                            End If

                            Acto = MyTblSecuencia.Select("DEP_CODACTO='" & RowSecu.Item("DEP_DEPENDENCIA") & "'")(0)
                            Sw = True
                            Exit For
                        End If
                    Next
                    If Not Sw Then
                        Salida = True
                    End If
                    FechaPrevia = FechaActSig
                ElseIf ActApl.Length = 0 Then
                    If (MyTblSecuencia.Rows.Count - 1) = xt Then
                        Salida = True
                    Else
                        xt = xt + 1
                        Acto = MyTblSecuencia.Rows(xt)
                    End If
                Else
                    Salida = True
                End If
                If Salida Then
                    .MyTblProyeccion.AddProcesoExpedienteRow(Nothing, "EN ESPERA", FechaPrevia, FechaPrevia, DateDiff(DateInterval.Day, Date.Today, FechaPrevia), 0, Nothing, paginas, Entidad, Nombre, docObservaciones)

                    If MyTblProyeccion.Rows.Count = 1 Then
                        .MyTblProyeccion.AddProcesoExpedienteRow(Nothing, "Expediente mal configurado.", Nothing, FechaPrevia, DateDiff(DateInterval.Day, Date.Today, FechaPrevia), 0, Nothing, paginas, Entidad, Nombre, docObservaciones)
                    End If
                End If
            End While
            Return .MyTblProyeccion
        End With
    End Function

    Private Function Nodatos() As DataTable
        Dim table As DataTable = New DataTable
        Dim dr As DataRow
        Dim ds As New DataSet

        table.Columns.Add(New DataColumn("Notexpe", GetType(String)))
        'table.Columns.Add(New DataColumn("NombreEjecutivo", GetType(String)))

        dr = table.NewRow()
        dr("Notexpe") = "No Hay documentos"
        table.Rows.Add(dr)

        Return table
    End Function

End Class

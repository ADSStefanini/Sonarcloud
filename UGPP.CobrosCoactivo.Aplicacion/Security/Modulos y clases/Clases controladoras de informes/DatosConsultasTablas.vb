Imports System.Data.SqlClient
Public Class DatosConsultasTablas
    Public Function Load_Deudor(ByVal ID As String, ByVal Tabla As DataTable) As DataTable
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT * FROM ENTESDBF WHERE CODIGO_NIT = @CODIGO_NIT", New SqlClient.SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.Add("@CODIGO_NIT", SqlDbType.VarChar).Value = ID
            Command.Connection.Open()
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            Tabla.Load(reader)
            reader.Close()
            Command.Connection.Close()
            Return Tabla
        End Using
    End Function

    Public Function Expedientes_Deudor(ByVal Cobrador As String, ByVal Imp As String, ByVal Expediente As String, ByVal Tabla As DataTable) As DataTable
        Using Command As New System.Data.SqlClient.SqlCommand("SELECT A.ENTIDAD,A.IDACTO,A.RUTA,A.NOMARCHIVO,A.ID,A.PAGINAS,A.FECHARADIC,A.COBRADOR,A.DOCEXPEDIENTE,A.DOCPROCESO,A.DOCPREDIO_REFECATRASTAL,A.DOCACUMULACIO,A.DOCFECHADOC,A.DOCANULAR,(A.DOCUSUARIO + ' - ' + B.NOMBRE) AS DOCUSUARIO,A.DOCFECHASYSTEM,A.DOCACTIVOTRIBUTARIO,A.DOCIMPUESTO FROM DOCUMENTOS A INNER JOIN USUARIOS B ON A.DOCUSUARIO = B.CODIGO AND A.COBRADOR = B.COBRADOR WHERE A.DOCEXPEDIENTE = @DOCEXPEDIENTE AND A.COBRADOR = @COBRADOR", New SqlClient.SqlConnection(Funciones.CadenaConexion))
            Command.Parameters.Add("@DOCEXPEDIENTE", SqlDbType.VarChar).Value = Expediente
            Command.Parameters.Add("@COBRADOR", SqlDbType.VarChar).Value = Cobrador
            Command.Connection.Open()
            Dim reader As System.Data.SqlClient.SqlDataReader = Command.ExecuteReader(CommandBehavior.CloseConnection)
            Tabla.Load(reader)
            reader.Close()
            Command.Connection.Close()
            Return Tabla
        End Using
    End Function

    Public Function Load_ImpuestosEnte(ByVal ente As String, ByVal data As DataTable) As DataTable
        Dim Sql = "SELECT IMP_NOMBRE,IMP_VALUES,IMP_ENTECOBRADOR,IMP_CAMPOCLAVEID,IMP_ID FROM DOCUMENTO_IMPUESTO WHERE IMP_ENTECOBRADOR = @IMP_ENTECOBRADOR"
        Using adap As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
            adap.SelectCommand.Parameters.Add("@IMP_ENTECOBRADOR", SqlDbType.VarChar).Value = ente
            adap.Fill(data)
            Return data
        End Using
    End Function

    Function DiasLaborales(ByVal fechainicial As Date, ByVal fechafinal As Date, ByVal annio As String) As Integer
        Dim diaslab As Integer = 0
        Dim ini As Date = Format(fechainicial, "dd/MM/yyyy")
        Dim fin As Date = Format(fechafinal, "dd/MM/yyyy")
        'Dim com As New SqlCommand(”DECLARE @DiasLaborales int EXEC DifDias @FechaInicial,@FechaFinal,@DiasLaborales output PRINT @DiasLaborales”, conexion)
        Dim conexion As New SqlClient.SqlConnection
        conexion = conexionServer()

        Dim com As New SqlClient.SqlCommand("DifDias", conexion)
        com.CommandType = CommandType.StoredProcedure
        com.Parameters.Add("@FechaInicial", SqlDbType.DateTime).Value = ini
        com.Parameters.Add("@FechaFinal", SqlDbType.DateTime).Value = fin
        com.Parameters.Add("@Annio", SqlDbType.VarChar).Value = annio
        com.Parameters.Add("@DiasLaborales", SqlDbType.Decimal).Value = 0

        conexion.Open()
        diaslab = com.ExecuteScalar
        conexion.Close()
        Return diaslab
    End Function

    Public Function Etapas() As DataTable
        'Creacion de objeto SQLCommand
        Dim cmd As String
        Dim oscmd As SqlClient.SqlCommand

        cmd = "SELECT codigo, nombre FROM etapas ORDER BY codigo"
        oscmd = New SqlClient.SqlCommand(cmd, conexionServer)

        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, conexionServer)
        oDtAdapterSql1.SelectCommand = oscmd

        'Creacion del DataSet
        Dim Dataset1 As DataSet = New DataSet
        oDtAdapterSql1.Fill(Dataset1, "etapas")

        Return Dataset1.Tables("etapas")
    End Function

    Public Function EntesCobradores(ByVal Ente As String) As DatasetForm.entescobradoresDataTable
        Dim SQL As String
        SQL = "SELECT * FROM  entescobradores WHERE CODIGO = @CODIGO"
        Using MyAdapter As New SqlDataAdapter(SQL, Funciones.CadenaConexion)
            MyAdapter.SelectCommand.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Ente
            Dim MyTable As New DatasetForm.entescobradoresDataTable
            MyAdapter.Fill(MyTable)
            Return MyTable
        End Using
    End Function

    Public Function DEPENDENCIA_ACTUACIONES() As DataTable
        'Creacion de objeto SQLCommand
        Dim cmd As String
        Dim oscmd As SqlClient.SqlCommand

        cmd = "SELECT * FROM DEPENDENCIA_ACTUACIONES ORDER BY dep_orden ASC"
        oscmd = New SqlClient.SqlCommand(cmd, conexionServer)

        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(cmd, conexionServer)
        oDtAdapterSql1.SelectCommand = oscmd

        'Creacion del DataSet
        Dim Dataset1 As DataSet = New DataSet
        oDtAdapterSql1.Fill(Dataset1, "DEPENDENCIA_ACTUACIONES")

        Return Dataset1.Tables("DEPENDENCIA_ACTUACIONES")
    End Function

    Public Function conexionServer() As SqlClient.SqlConnection
        Dim mySqlConnection As New SqlClient.SqlConnection
        mySqlConnection.ConnectionString = Funciones.CadenaConexion
        Return mySqlConnection
    End Function

    Public Function Load_Actos() As DataTable
        Dim oscmd As SqlClient.SqlCommand

        oscmd = New SqlClient.SqlCommand("SELECT * FROM DEPENDENCIA_ACTUACIONES ORDER BY dep_orden ASC", conexionServer)

        'Creacion del objeto DataAdapter
        Dim oDtAdapterSql1 As SqlClient.SqlDataAdapter
        oDtAdapterSql1 = New SqlClient.SqlDataAdapter(oscmd)

        'Creacion del DataSet
        Dim Dataset1 As DataTable = New DataTable
        oDtAdapterSql1.Fill(Dataset1)

        Return Dataset1
    End Function

    'Cobro de ultimo acto 
    Public Function Preparar_expedientes(ByVal cobrador As String, ByVal con As SqlConnection) As DataTable
        Dim sql As String = "SELECT entidad, docexpediente FROM documentos where DOCANULAR = 0 and cobrador = @cobrador group by entidad, docexpediente"

        Dim Command As SqlClient.SqlCommand = New SqlClient.SqlCommand()
        Command.Connection = con
        Command.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = cobrador
        Command.CommandText = sql

        Dim myAdapterExpedientes As New SqlDataAdapter(Command)

        Dim mytable As New DataTable
        myAdapterExpedientes.Fill(mytable)

        Return mytable
    End Function

    Public Function consultarExpedinetedeldeudor(ByVal cobrador As String, ByVal con As SqlConnection, ByVal predioNit As String) As DatasetForm.ProcesoAcumuladoDataTable
        Dim sql As String = "SELECT DOCEXPEDIENTE, DOCPREDIO_REFECATRASTAL,FECHARADIC, 0 AS CHEKPPAL FROM DOCUMENTOS WHERE entidad = @predioNit AND DOCANULAR = 0 AND  COBRADOR = @cobrador and idacto = '013' ORDER BY FECHARADIC ASC"
        Dim Command As SqlClient.SqlCommand = New SqlClient.SqlCommand()

        Command.Connection = con
        Command.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = cobrador
        Command.Parameters.Add("@predioNit", SqlDbType.VarChar).Value = predioNit

        Command.CommandText = sql

        Dim myAdapterExpedientes As New SqlDataAdapter(Command)
        Dim mytable As New DatasetForm.ProcesoAcumuladoDataTable

        myAdapterExpedientes.Fill(mytable)
        Return mytable
    End Function

    Public Function consultarUsuario(ByVal codigo As String, ByVal cobrador As String, ByVal tabla As DataTable)
        Dim cmd As String = "SELECT * FROM usuarios WHERE codigo = @codigo AND cobrador = @cobrador"
        Dim myAdapter As New SqlDataAdapter(cmd, Funciones.CadenaConexion)
        myAdapter.SelectCommand.Parameters.AddWithValue("@codigo", codigo)
        myAdapter.SelectCommand.Parameters.AddWithValue("@cobrador", cobrador)
        myAdapter.Fill(tabla)
        Return tabla

    End Function

    Public Sub InsertaUltimo_Acto(ByVal ULT_EXPEDIENTE As String, ByVal ULT_ACTO As String, ByVal ULT_ACTODESCRIP As String, ByVal ULT_FECHA As String, ByVal ULT_DEUDOR As String)
        Using con As New SqlClient.SqlConnection(Funciones.CadenaConexion)
            Using Command As New SqlClient.SqlCommand()
                con.Open()
                Dim tran As SqlTransaction = con.BeginTransaction
                'Dim Command As SqlClient.SqlCommand = New SqlClient.SqlCommand()
                Command.Connection = con

                Command.CommandText = "INSERT INTO documento_ultimoacto(ULT_EXPEDIENTE,ULT_ACTO,ULT_ACTODESCRIP,ULT_FECHA,ULT_DEUDOR) VALUES (@ULT_EXPEDIENTE,@ULT_ACTO,@ULT_ACTODESCRIP,@ULT_FECHA,@ULT_DEUDOR)"
                Command.Parameters.Add("@ULT_EXPEDIENTE", SqlDbType.VarChar).Value = ULT_EXPEDIENTE
                Command.Parameters.Add("@ULT_ACTO", SqlDbType.VarChar).Value = ULT_ACTO
                Command.Parameters.Add("@ULT_ACTODESCRIP", SqlDbType.VarChar).Value = ULT_ACTODESCRIP
                Command.Parameters.Add("@ULT_FECHA", SqlDbType.DateTime).Value = ULT_FECHA
                Command.Parameters.Add("@ULT_DEUDOR", SqlDbType.VarChar).Value = ULT_DEUDOR
                Command.Transaction = tran
                Command.ExecuteNonQuery()

                Try
                    tran.Commit()
                Catch ex As Exception
                    tran.Rollback()
                End Try
            End Using
            con.Close()
            con.Dispose()
        End Using
    End Sub

    Public Sub Load_Configuracion(ByVal ente As String, ByVal datos As DataTable)
        Dim Sql As String = "SELECT codigo as ID_CLIENTE, nombre as NOMBRE,ent_foto as FOTO, ent_firma,ent_foto2,ent_foto3 FROM entescobradores WHERE codigo = @Cobrador"
        Using Eda As New SqlDataAdapter(Sql, Funciones.CadenaConexion)
            Eda.SelectCommand.Parameters.Add("@Cobrador", SqlDbType.VarChar).Value = ente
            Eda.Fill(datos)
        End Using
    End Sub

    Public Function Tipear_Tabla(ByVal TableOrigen As DataTable, ByVal TableDestino As DataTable) As Boolean
        For Each RowOrigen As DataRow In TableOrigen.Rows
            Dim Row As DataRow = TableDestino.NewRow
            For Each ColDes As DataColumn In TableDestino.Columns
                Dim Sw As Boolean = False
                For Each ColOri As DataColumn In TableOrigen.Columns
                    If ColDes.Caption = ColOri.Caption Then
                        Sw = True
                        Exit For
                    End If
                Next
                If Sw Then
                    Row(ColDes) = RowOrigen(ColDes.Caption)
                End If
            Next
            TableDestino.Rows.Add(Row)
        Next
        'Return TableDestino
    End Function

    Public Function Llenar_Fila_Documentos_impor(ByVal Row As DatasetForm.documentosRow) As DatasetForm.documentosRow
        Dim TblDocumentos As New DatasetForm.documentosDataTable
        For Each Column As DataColumn In TblDocumentos.Columns
            If Column.Caption = "ruta" Then
                Row(Column.Caption) = "ruta"
            Else
                If Row.IsNull(Column.Caption) Then
                    If Column.DataType.Name <> "String" Then
                        If Column.DataType.Name = "Boolean" Then
                            Row(Column.Caption) = False
                        ElseIf Column.DataType.Name = "DateTime" Then
                            Row(Column.Caption) = Date.Today
                        Else
                            Row(Column.Caption) = 0
                        End If
                    Else
                        Row(Column.Caption) = ""
                    End If
                End If
            End If
        Next
        Return Row
    End Function

    Public Function docindexerToTecno(ByVal Page As Page, ByVal cobrador As String) As DatasetForm.documentosDataTable
        'Sacar datos Docindexer
        Dim udlfile As String = Page.Server.MapPath("~") & "temp_arch/impdata.udl"
        Dim TblDocum As DatasetForm.documentosDataTable = New DatasetForm.documentosDataTable
        Using myConnection As New System.Data.OleDb.OleDbConnection("File Name = " & udlfile)
            myConnection.Open()
            If (myConnection.State = ConnectionState.Open) Then
                'Conexión se abrió correctamente
                Dim sql As String = "SELECT identifica as entidad,tipodocume as idacto, 'ruta' as ruta,a.imagen as nomarchivo, a.numimages as paginas,a.fecharadic," & cobrador & " as cobrador,a.expediente as docexpediente,a.expediente as docproceso,a.predio as docpredio_refecatrastal,a.acumulacio as docacumulacio,a.fechadoc as docfechadoc, 'ACTUALIZACION MASIVA' as docObservaciones , 0 as docanular, '123' as docusuario, GETDATE() as docfechasystem, a.tipo as docActivotributario from dbo.cobrocoactivo a where identifica IS NOT NULL and tipodocume IS NOT NULL and imagen IS NOT NULL and numimages IS NOT NULL and identifica <> '0'"
                Using consulta As New System.Data.OleDb.OleDbDataAdapter(sql, myConnection)
                    Dim tbOrigen As New DataTable
                    consulta.Fill(tbOrigen)
                    If tbOrigen.Rows.Count > 0 Then
                        For Each row As DataRow In tbOrigen.Rows
                            'Registrar en la tabla documentos
                            Dim RowDocum As DatasetForm.documentosRow = TblDocum.NewdocumentosRow
                            For Each Column As DataColumn In tbOrigen.Columns
                                For Each Coldestino As DataColumn In TblDocum.Columns
                                    If Coldestino.Caption = Column.Caption Then RowDocum.Item(Coldestino) = row.Item(Column)
                                    If Coldestino.Caption <> Column.Caption Then
                                        Dim ColHomologada As DataColumn = Column_tecno_doc(Column.Caption)
                                        If Not ColHomologada Is Nothing Then
                                            RowDocum.Item(ColHomologada) = row.Item(Column)
                                        End If
                                    End If
                                Next
                            Next

                            TblDocum.Rows.Add(Me.Llenar_Fila_Documentos_impor(RowDocum))
                        Next
                    End If

                    Return TblDocum
                End Using
            Else
                Throw New Exception("La conexión no se pudo establecer ")
            End If
        End Using
    End Function

    Public Function Column_tecno_doc(ByVal Name As String) As DataColumn
        Dim TblDocumentos As New DatasetForm.documentosDataTable
        Dim Column As DataColumn = Nothing

        Select Case Name
            Case "identifica"
                Column = TblDocumentos.Columns("entidad")
            Case "tipodocume"
                Column = TblDocumentos.Columns("idacto")
            Case "imagen"
                Column = TblDocumentos.Columns("nomarchivo")
            Case "numimages"
                Column = TblDocumentos.Columns("paginas")
            Case "expediente"
                Column = TblDocumentos.Columns("docexpediente")
        End Select

        Return Column
    End Function
End Class

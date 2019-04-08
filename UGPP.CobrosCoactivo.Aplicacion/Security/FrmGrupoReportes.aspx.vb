Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration
Partial Public Class FrmGrupoReportes
    Inherits System.Web.UI.Page
    Protected Shared datosReportes As New DataTable
    Const constReportes As String = "SELECT REP_CODIGO, REP_NOMBRE FROM MAESTRO_REPORTES WHERE REP_ESTADO=1 AND REP_GRUPO ='01' ORDER BY REP_CODIGO"
    Const constFuncionario As String = "SELECT CODIGO,NOMBRE FROM USUARIOS WHERE NIVELACCES=4"
    Dim Connection As New SqlConnection(Funciones.CadenaConexion)


    Private Sub CargarCombro()
        With Me
            If Connection.State = ConnectionState.Closed Then
                Connection.Open()
            End If
            lblError.Text = ""
            lblError.ForeColor = Drawing.Color.Black

            Dim tbl As New DataTable

            tbl = Funciones.consultar(constReportes, Connection)
            Dim rw As DataRow = tbl.NewRow
            rw("REP_CODIGO") = "00"
            rw("REP_NOMBRE") = "Seleccione un Reporte .."
            tbl.Rows.InsertAt(rw, 0)
            If tbl.Rows.Count > 0 Then
                .DdlReporte.DataSource = tbl
                .DdlReporte.DataTextField = "REP_NOMBRE"
                .DdlReporte.DataValueField = "REP_CODIGO"
                .DdlReporte.DataBind()
            Else
                lblError.Text = "No hay Opciones Agregadas, Verificar los reportes Creados."
                lblError.ForeColor = Drawing.Color.Red
            End If
        End With
    End Sub

    Private Sub CargarGestores()
        With Me
            If Connection.State = ConnectionState.Closed Then
                Connection.Open()
            End If
            lblError.Text = ""
            lblError.ForeColor = Drawing.Color.Black

            Dim tbl As New DataTable

            tbl = Funciones.consultar(constFuncionario, Connection)
            Dim rw As DataRow = tbl.NewRow
            rw("CODIGO") = "00"
            rw("NOMBRE") = "Seleccione un Funcionario .."
            tbl.Rows.InsertAt(rw, 0)
            If tbl.Rows.Count > 0 Then
                .DdlUsuario.DataSource = tbl
                .DdlUsuario.DataTextField = "NOMBRE"
                .DdlUsuario.DataValueField = "CODIGO"
                .DdlUsuario.DataBind()
            Else
                lblError.Text = "No hay Opciones Agregadas, Verificar los reportes Creados."
                lblError.ForeColor = Drawing.Color.Red
            End If
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        finsession()
        If Not IsPostBack Then
            Try
                CargarCombro()
                CargarGestores()

                '--Cargar información de los deudores por columnas para el informe de informacion general
                '-- Cargar información de los cambios de estado por columnas para el informe de informacion general
                Dim f As New FuncionesInformes
                f.CargarInformacionDeudores()
                f.CargarCambiosEstado()
                '--Fin carge de información


                'Instanciar clase de mensajes y estadisticas
                ' Dim MTG As New MetodosGlobalesCobro
                ' lblNomPerfil.Text = MTG.GetNomPerfil(Session("mnivelacces"))
                lblNomPerfil.Text = CommonsCobrosCoactivos.getNomPerfil(Session)
            Catch ex As Exception
                lblError.Text = "Error: " & ex.ToString
            End Try

        End If
    End Sub

    Protected Sub BtnGenerar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnGenerar.Click

        If DdlReporte.SelectedValue = "011" Then
            FilasColumnaMedidas()
            FilasColumnaOtrasResoluciones()
            FilasColumnaObservacionesPersuasivo()
        End If
        DataBindSource("REPORTES")
    End Sub

    Private Sub DataBindSource(ByVal NOMBREPROCEDIMIENTO As String)
        With Me
            Try
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = ""

                If Connection.State = ConnectionState.Closed Then
                    Connection.Open()
                End If
                Dim command As SqlCommand
                Dim dtTable As DataTable
                Dim adapter As SqlDataAdapter

                command = New SqlCommand(NOMBREPROCEDIMIENTO, Connection)
                command.CommandType = CommandType.StoredProcedure
                command.CommandTimeout = 1200 ' 600 = 10 minutos                
                adapter = New SqlDataAdapter(command)
                dtTable = New DataTable

                With command.Parameters
                    .Add(New SqlParameter("@OPCION", SqlDbType.VarChar)).Value = DdlReporte.SelectedValue
                    If dtpFechI.Value <> "" And dtpFechF.Value <> "" Then
                        .Add(New SqlParameter("@FECHI", SqlDbType.DateTime)).Value = Format(CDate(dtpFechI.Value), "dd/MM/yyyy")
                        .Add(New SqlParameter("@FECHF", SqlDbType.DateTime)).Value = Format(CDate(dtpFechF.Value), "dd/MM/yyyy")
                    Else
                        .Add(New SqlParameter("@FECHI", SqlDbType.DateTime)).Value = Format(CDate(Date.Today), "dd/MM/yyyy")
                        .Add(New SqlParameter("@FECHF", SqlDbType.DateTime)).Value = Format(CDate(Date.Today), "dd/MM/yyyy")
                    End If
                    Select Case DdlReporte.SelectedValue
                        Case "006", "020"
                            .Add(New SqlParameter("@USUARIO", SqlDbType.VarChar)).Value = DdlUsuario.SelectedValue
                        Case Else
                            .Add(New SqlParameter("@USUARIO", SqlDbType.VarChar)).Value = ""
                    End Select
                    'If DdlReporte.SelectedValue = "006" Then

                    'Else

                    'End If
                End With


                adapter.Fill(dtTable)
                If NOMBREPROCEDIMIENTO = "REPORTES" Then
                    DgvReporte.DataSource = dtTable
                    DgvReporte.DataBind()
                    datosReportes = dtTable
                End If



            Catch expSQL As SqlException
                lblError.ForeColor = Drawing.Color.Red
                lblError.Text = expSQL.Message
                Exit Sub
            End Try
        End With
    End Sub

    Private Sub BtnCancelar_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancelar.ServerClick
        With Me
            .DgvReporte.DataSource = Nothing
            .DgvReporte.DataBind()
            .DdlReporte.SelectedIndex = 0
            lblError.Text = ""
        End With
    End Sub

    Private Function GetNomPerfil(ByVal pUsuario As String) As String
        Dim NomPerfil As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        Dim sql As String = "SELECT nombre FROM perfiles WHERE codigo = " & Session("mnivelacces")

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            NomPerfil = Reader("nombre").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()
        Return NomPerfil
    End Function

    Protected Sub ABackRep_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABackRep.Click
        If Session("mnivelacces") = 1 Then
            'Administrador
            Response.Redirect("menu.aspx")
        Else
            'Revisor o supervisor
            Response.Redirect("Maestros/EJEFISGLOBAL.aspx")
        End If

    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        'Limpiar los cuadros de texto de busqueda
        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
        Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
        Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
        Session("Paginacion") = 10
        Response.Redirect("../login.aspx")
    End Sub

    Private Sub BtnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportar.Click
        With Me
            Try
                Dim MTG As New MetodosGlobalesCobro
                'Dim dt As DataTable = CType(ViewState("vsDatos"), DataTable)
                Dim ds As New DataSet
                ds.Merge(datosReportes)
                'MTG.ExportDataSetToExcel(ds, DdlReporte.Text & ".xls")
                If DdlReporte.SelectedValue = "045" Then
                    MTG.ExportDataSetToExcelActuaciones(ds, DdlReporte.SelectedItem.Text + " A " + Date.Today.ToShortDateString & ".xls")
                Else
                    MTG.ExportDataSetToExcel(ds, DdlReporte.SelectedItem.Text & " A " & Date.Today.ToShortDateString & ".xls")
                End If
                .lblError.Text = "Archivo Generado Exitosamente."
                'ViewState("vsDatos") = Nothing
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End With
    End Sub

    Private Sub ExportToExcel(ByVal nombreReporte As String, ByVal grilla As GridView)
        Dim responsePage As HttpResponse = Response
        Dim sw As New StringWriter()
        Dim htw As New HtmlTextWriter(sw)
        Dim prender As New Page()
        Dim form As New HtmlForm
        form.Controls.Add(grilla)
        prender.Controls.Add(form)
        responsePage.Clear()
        responsePage.Buffer = True
        responsePage.ContentType = "application/vnd.ms-excel"
        responsePage.AddHeader("Content-Disposition", "attachment;filename=" + nombreReporte)
        responsePage.Charset = "UTF-8"
        responsePage.ContentEncoding = Encoding.Default
        prender.RenderControl(htw)
        responsePage.Write(sw.ToString())
        responsePage.End()
    End Sub

    'Public Sub Limpiargrid()
    '    With Me
    '        DgvReporte.DataSource = Nothing
    '        DgvReporte.DataBind()
    '    End With
    'End Sub

    Private Sub DdlReporte_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlReporte.SelectedIndexChanged
        DgvReporte.DataSource = Nothing
        DgvReporte.DataBind()

        If DdlReporte.SelectedValue = "001" Then
            btnGuardar.Enabled = True
            btnGuardar.Text = "Guardar información general"
        ElseIf DdlReporte.SelectedValue = "021" Then
            btnGuardar.Enabled = True
            btnGuardar.Text = "Guardar gestor pagos"
        Else
            btnGuardar.Text = "..."
            btnGuardar.Enabled = False
        End If
        lblError.Text = ""

    End Sub

    Private Sub DgvReporte_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles DgvReporte.PageIndexChanging
        DgvReporte.PageIndex = e.NewPageIndex
        DataBindSource("REPORTES")
    End Sub

    Private Sub finsession()
        Dim MTG As New MetodosGlobalesCobro
        If MTG.IsSessionTimedOut Then
            FormsAuthentication.SignOut()
            'Limpiar los cuadros de texto de busqueda
            Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
            Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
            Session("EJEFISGLOBAL.txtSearchEFINIT") = ""
            Session("EJEFISGLOBAL.cboSearchEFIUSUASIG") = ""
            Session("EJEFISGLOBAL.cboSearchEFIESTADO") = ""
            Session("Paginacion") = 10
            Response.Redirect("../login.aspx")
        End If
    End Sub

    Protected Sub BtnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        If DdlReporte.SelectedValue = "001" Or DdlReporte.SelectedValue = "021" Then
            DataBindSource("SP_HISTORIALES")
            lblError.Text = "Información almacenada exitosamente..."
        End If

    End Sub

    Private Sub FilasColumnaMedidas()
        'expedientes con las medidas
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim sql As String = "select top 1 NroExp, COUNT(1) t from EMBARGOS   group by NroExp order by COUNT(1) desc"
        Dim sql2, sql3 As String
        Dim cmd As New SqlClient.SqlCommand(sql, cn)
        Dim tb As New DataTable
        Dim adap As New SqlDataAdapter(cmd)
        adap.Fill(tb)

        sql = "IF OBJECT_ID('PIVOTEMBARGOS') IS NOT NULL DROP TABLE PIVOTEMBARGOS; CREATE TABLE PIVOTEMBARGOS (NroExpMC [varchar](12) NULL, "
        sql2 = ""
        For x As Integer = 0 To CInt(tb.Rows(0).Item("T")) - 1
            sql2 = sql2 & "RESOLUCION_MEDIDAS_CAUTELARES" & x + 1 & " [varchar](30) NULL,"
            sql2 = sql2 & "FECHA_MEDIDAS_CAUTELARES" & x + 1 & " [datetime] NULL,"
            sql2 = sql2 & "LIMITE_MEDIDAS_CAUTELARES" & x + 1 & " [float] NULL,"
            sql2 = sql2 & "ESTADO_MEDIDAS_CAUTELARES" & x + 1 & " [int] NULL,"
            sql2 = sql2 & "OBSERVACIONES_MEDIDAS_CAUTELARES" & x + 1 & " [varchar](MAX) NULL,"
        Next
        sql = sql & sql2
        sql = sql.Substring(0, sql.Length - 1) & ")"
        cmd = New SqlCommand(sql, cn)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()


        ''----------Armar el insert dinamico.
        sql = "select NroExp,NroResolEm,FecResolEm,LimiteEm,EstadoEm,observaciones from EMBARGOS   order by NroExp, FecResolEm"
        cmd = New SqlClient.SqlCommand(sql, cn)
        tb = New DataTable
        adap = New SqlDataAdapter(cmd)
        adap.Fill(tb)


        ''----Amandao el inset dinamico.
        sql = "INSERT INTO PIVOTEMBARGOS ( NroExpMC, "

        'Agregar a campos
        sql2 = ""
        'Agregar valores
        sql3 = ""

        Dim a As Integer = 0
        Dim expNew, expOld As String
        expOld = ""
        expNew = ""

        For Each x As DataRow In tb.Rows
            expNew = x("NroExp")
            a += 1
            If expOld = "" Or expNew = expOld Then
                ''Continuar concatenando.
            Else
                sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
                sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
                cmd.CommandText = sql & sql2 & sql3
                'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
                cn.Open()
                cmd.ExecuteNonQuery()
                cn.Close()
                cmd.Parameters.Clear()
                a = 1
                sql2 = ""
                sql3 = ""
            End If

            If IsDBNull(x("LimiteEm")) Then
                x("LimiteEm") = 0
            End If

            If IsDBNull(x("NroResolEm")) Then
                x("NroResolEm") = ""
            End If
            If IsDBNull(x("EstadoEm")) Then
                x("EstadoEm") = "0"
            End If

            If IsDBNull(x("FecResolEm")) Then
                x("FecResolEm") = DBNull.Value
            End If


            sql2 = sql2 & "RESOLUCION_MEDIDAS_CAUTELARES" & a & ",FECHA_MEDIDAS_CAUTELARES" & a & ", LIMITE_MEDIDAS_CAUTELARES" & a & ", ESTADO_MEDIDAS_CAUTELARES" & a & ", OBSERVACIONES_MEDIDAS_CAUTELARES" & a & ","

            sql3 = sql3 & "@RESOLUCION_MEDIDAS_CAUTELARES" & a & ", @FECHA_MEDIDAS_CAUTELARES" & a & ", @LIMITE_MEDIDAS_CAUTELARES" & a & ", @ESTADO_MEDIDAS_CAUTELARES" & a & ", @OBSERVACIONES_MEDIDAS_CAUTELARES" & a & ","


            cmd.Parameters.AddWithValue("@RESOLUCION_MEDIDAS_CAUTELARES" & a, x("NroResolEm"))
            cmd.Parameters.AddWithValue("@FECHA_MEDIDAS_CAUTELARES" & a, x("FecResolEm"))
            cmd.Parameters.AddWithValue("@LIMITE_MEDIDAS_CAUTELARES" & a, x("LimiteEm"))
            cmd.Parameters.AddWithValue("@ESTADO_MEDIDAS_CAUTELARES" & a, x("EstadoEm"))
            cmd.Parameters.AddWithValue("@OBSERVACIONES_MEDIDAS_CAUTELARES" & a, x("observaciones"))

            'sql3 += String.Format("'" & x("NroResolEm") & "','" & IIf(x("FecResolEm") Is DBNull.Value, DBNull.Value, CDate(x("FecResolEm"))) & "',{0}" & "," & x("EstadoEm") & ",'" & x("observaciones").ToString.Replace("/", "-").Replace(" /", "-)").Replace("/ ", "-").Replace("'", " ") & "',", x("LimiteEm"))


            expOld = expNew

        Next



        sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
        sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
        'sql = sql & sql2 & sql3
        cmd.CommandText = sql & sql2 & sql3
        'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
        cn.Open()
        cmd.ExecuteNonQuery()
        cn.Close()




    End Sub

    Private Sub FilasColumnaOtrasResoluciones()
        'expedientes con las medidas
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim sql As String = "select top 1 NroExp, COUNT(1) t from OTRASRESOLUCIONES   group by NroExp order by COUNT(1) desc"
        Dim sql2, sql3 As String
        Dim cmd As New SqlClient.SqlCommand(sql, cn)
        Dim tb As New DataTable
        Dim adap As New SqlDataAdapter(cmd)
        adap.Fill(tb)

        sql = "IF OBJECT_ID('PIVOTOTRASRESOLUCIONES') IS NOT NULL DROP TABLE PIVOTOTRASRESOLUCIONES; CREATE TABLE PIVOTOTRASRESOLUCIONES (NroExpOr [varchar](12) NULL, "
        sql2 = ""
        For x As Integer = 0 To CInt(tb.Rows(0).Item("T")) - 1
            sql2 = sql2 & "TIPO_OTRAS_RESOLUCIONES" & x + 1 & " [varchar](90) NULL,"
            sql2 = sql2 & "NUMERO_OTRAS_RESOLUCIONES" & x + 1 & " [varchar](20) NULL,"
            sql2 = sql2 & "FECHA_OTRAS_RESOLUCIONES" & x + 1 & " [datetime] NULL,"
            sql2 = sql2 & "FORMA_NOTI_OTRAS_RESOLUCIONES" & x + 1 & " [varchar](200) NULL,"
            sql2 = sql2 & "OBSERVACIONES_OTRAS_RESOLUCIONES" & x + 1 & " text NULL,"
        Next
        sql = sql & sql2
        sql = sql.Substring(0, sql.Length - 1) & ")"
        cmd = New SqlCommand(sql, cn)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()


        ''----------Armar el insert dinamico.
        sql = "SELECT NroExp ,NombreTipo ,NroResol ,FechaResol,FormaNotif ,observaciones FROM OTRASRESOLUCIONES  order by NroExp,FechaResol"
        cmd = New SqlClient.SqlCommand(sql, cn)
        tb = New DataTable
        adap = New SqlDataAdapter(cmd)
        adap.Fill(tb)


        ''----Amandao el inset dinamico.
        sql = "INSERT INTO PIVOTOTRASRESOLUCIONES ( NroExpOr, "

        'Agregar a campos
        sql2 = ""
        'Agregar valores
        sql3 = ""

        Dim a As Integer = 0
        Dim expNew, expOld As String
        expOld = ""
        expNew = ""

        For Each x As DataRow In tb.Rows
            expNew = x("NroExp")
            a += 1
            If expOld = "" Or expNew = expOld Then
                ''Continuar concatenando.
            Else
                sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
                sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
                cmd.CommandText = sql & sql2 & sql3
                'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
                cn.Open()
                cmd.ExecuteNonQuery()
                cn.Close()
                cmd.Parameters.Clear()
                a = 1
                sql2 = ""
                sql3 = ""
            End If

            If IsDBNull(x("FechaResol")) Then
                x("FechaResol") = DBNull.Value
            End If


            sql2 = sql2 & "TIPO_OTRAS_RESOLUCIONES" & a & ",NUMERO_OTRAS_RESOLUCIONES" & a & ", FECHA_OTRAS_RESOLUCIONES" & a & ", FORMA_NOTI_OTRAS_RESOLUCIONES" & a & ", OBSERVACIONES_OTRAS_RESOLUCIONES" & a & ","

            sql3 = sql3 & "@TIPO_OTRAS_RESOLUCIONES" & a & ", @NUMERO_OTRAS_RESOLUCIONES" & a & ", @FECHA_OTRAS_RESOLUCIONES" & a & ", @FORMA_NOTI_OTRAS_RESOLUCIONES" & a & ", @OBSERVACIONES_OTRAS_RESOLUCIONES" & a & ","


            cmd.Parameters.AddWithValue("@TIPO_OTRAS_RESOLUCIONES" & a, x("NombreTipo"))
            cmd.Parameters.AddWithValue("@NUMERO_OTRAS_RESOLUCIONES" & a, x("NroResol"))
            cmd.Parameters.AddWithValue("@FECHA_OTRAS_RESOLUCIONES" & a, x("FechaResol"))
            cmd.Parameters.AddWithValue("@FORMA_NOTI_OTRAS_RESOLUCIONES" & a, x("FormaNotif"))
            cmd.Parameters.AddWithValue("@OBSERVACIONES_OTRAS_RESOLUCIONES" & a, x("observaciones"))

            'sql3 += String.Format("'" & x("NroResolEm") & "','" & IIf(x("FecResolEm") Is DBNull.Value, DBNull.Value, CDate(x("FecResolEm"))) & "',{0}" & "," & x("EstadoEm") & ",'" & x("observaciones").ToString.Replace("/", "-").Replace(" /", "-)").Replace("/ ", "-").Replace("'", " ") & "',", x("LimiteEm"))


            expOld = expNew

        Next



        sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
        sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
        'sql = sql & sql2 & sql3
        cmd.CommandText = sql & sql2 & sql3
        'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
        cn.Open()
        cmd.ExecuteNonQuery()
        cn.Close()




    End Sub

    Private Sub FilasColumnaObservacionesPersuasivo()
        'expedientes con las medidas
        Dim cn As New SqlConnection(Funciones.CadenaConexion)
        Dim sql As String = "select top 1 NroExp, COUNT(1) t from PERSUASIVOOBSERVACIONES  group by NroExp order by COUNT(1) desc"
        Dim sql2, sql3 As String
        Dim cmd As New SqlClient.SqlCommand(sql, cn)
        Dim tb As New DataTable
        Dim adap As New SqlDataAdapter(cmd)
        adap.Fill(tb)

        sql = "IF OBJECT_ID('PIVOTPERSUASIVOOBSERVACIONES') IS NOT NULL DROP TABLE PIVOTPERSUASIVOOBSERVACIONES; CREATE TABLE PIVOTPERSUASIVOOBSERVACIONES (NroExpObs [varchar](12) NULL, "
        sql2 = ""
        For x As Integer = 0 To CInt(tb.Rows(0).Item("T")) - 1
            sql2 = sql2 & "OBSERVACION" & x + 1 & " text NULL,"
        Next
        sql = sql & sql2
        sql = sql.Substring(0, sql.Length - 1) & ")"
        cmd = New SqlCommand(sql, cn)
        cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()


        ''----------Armar el insert dinamico.
        sql = "SELECT NroExp ,observaciones FROM PERSUASIVOOBSERVACIONES  order by NroExp,fecha"
        cmd = New SqlClient.SqlCommand(sql, cn)
        tb = New DataTable
        adap = New SqlDataAdapter(cmd)
        adap.Fill(tb)


        ''----Amandao el inset dinamico.
        sql = "INSERT INTO PIVOTPERSUASIVOOBSERVACIONES ( NroExpObs, "

        'Agregar a campos
        sql2 = ""
        'Agregar valores
        sql3 = ""

        Dim a As Integer = 0
        Dim expNew, expOld As String
        expOld = ""
        expNew = ""

        For Each x As DataRow In tb.Rows
            expNew = x("NroExp")
            a += 1
            If expOld = "" Or expNew = expOld Then
                ''Continuar concatenando.
            Else
                sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
                sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
                cmd.CommandText = sql & sql2 & sql3
                'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
                cn.Open()
                cmd.ExecuteNonQuery()
                cn.Close()
                cmd.Parameters.Clear()
                a = 1
                sql2 = ""
                sql3 = ""
            End If

            sql2 = sql2 & "OBSERVACION" & a & ","
            sql3 = sql3 & "@OBSERVACION" & a & ","


            cmd.Parameters.AddWithValue("@OBSERVACION" & a, x("observaciones"))

            expOld = expNew

        Next



        sql2 = sql2.Substring(0, sql2.Length - 1) & ") VALUES ('" & expOld & "',"
        sql3 = sql3.Substring(0, sql3.Length - 1) & ")"
        'sql = sql & sql2 & sql3
        cmd.CommandText = sql & sql2 & sql3
        'cmd.Connection = New SqlConnection(Funciones.CadenaConexion)
        cn.Open()
        cmd.ExecuteNonQuery()
        cn.Close()





    End Sub

End Class
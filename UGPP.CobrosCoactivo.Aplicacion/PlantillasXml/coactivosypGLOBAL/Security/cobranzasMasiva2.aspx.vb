Imports System.Data.SqlClient
Partial Public Class cobranzasMasiva2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            lblNomPerfil.Text = GetNomPerfil(Session("sscodigousuario"))
            CargarListaExp()
            Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", "01")
            BtnCoactivos.CssClass &= "ui-state-active"
        Else
            BtnCoactivos.CssClass &= "ui-state-active"

        End If
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
    Private Sub cargar_objeto(ByVal _obje As RadioButtonList, ByVal sql As String, ByVal Captura As String, ByVal CampoMostrar As String, ByVal etapa As String)

        Dim cn As New SqlClient.SqlConnection(Funciones.CadenaConexion) 'Nueva conexión indicando al SqlConnection la cadena de conexión  
        Try
            _obje.DataSource = Nothing
            _obje.Items.Clear()
            Dim cmd As New SqlClient.SqlCommand(sql, cn) 'Pasar la consulta sql y la conexión al Sql Command   
            cmd.Parameters.AddWithValue("@IDETAPA", etapa)
            cmd.Parameters.AddWithValue("@DXI_IMPUESTOVALUE", Session("ssimpuesto"))
            cmd.Parameters.AddWithValue("@DXI_HISTORIAL", True)

            Dim da As New SqlClient.SqlDataAdapter(cmd) 'Inicializar un nuevo SqlDataAdapter   
            Dim tb As New DataTable 'Crear y Llenar una Tabla
            da.Fill(tb) 'llena la tabla dependiendo la consulta realizada

            _obje.DataSource = tb 'Asignar el DataSource al combobox  
            _obje.DataValueField = Captura 'Campo a capturar
            _obje.DataTextField = CampoMostrar 'Campo a mostar
            _obje.DataBind() 'Enlaza un origen de datos al control de servidor invocado y a todos sus controles secundarios.            

        Catch ex As Exception
            ex.ToString()  'Si hay alguna excepcion
        Finally
        End Try
    End Sub
    Public Function cargar_representante(ByVal expediente As Integer, Optional ByVal OP As Integer = 1) As String
        Dim Ado As New SqlConnection(Funciones.CadenaConexion)
        Dim rep As String = ""
        Dim tblrep As New DataTable
        Dim sqldata As New SqlDataAdapter(" SELECT A.ED_NOMBRE ,A.ED_CODIGO_NIT" & _
                                          " FROM  ENTES_DEUDORES A, " & _
                                          " DEUDORES_EXPEDIENTES B, " & _
                                          " EJEFISGLOBAL C " & _
                                          " WHERE  C.EFINROEXP =B.NROEXP  " & _
                                          " AND A.ED_CODIGO_NIT = B.DEUDOR " & _
                                          " AND TIPO =3 and NroExp = '" & expediente & "'", Ado)
        sqldata.Fill(tblrep)
        Select Case OP

            Case 2
                If tblrep.Rows.Count > 0 Then
                    For i = 0 To tblrep.Rows.Count - 1
                        rep = tblrep.Rows.Item(0)(1)
                    Next
                End If
            Case 1
                If tblrep.Rows.Count > 0 Then
                    For i = 0 To tblrep.Rows.Count - 1
                        rep = tblrep.Rows.Item(0)(0)
                    Next
                End If
        End Select

        Return rep

    End Function
    Private Sub menssageError(ByVal msn As String)
        ViewState("message") = msn
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "message", "$(function() {$('#dialog-message').dialog({hide: 'fold',autoOpen: true,modal: true,buttons: {'Aceptar': function() {$( this ).dialog( 'close' );}}});});", True)
    End Sub
    Private Sub CargarListaExp()
        If Not Session("Dtexp") Is Nothing Then
            ListExpedientes.DataSource = Nothing
            ListExpedientes.Items.Clear()

            Dim Tb As New DataTable
            Tb = Session("Dtexp")

            ListExpedientes.DataSource = Tb 'Asignar el DataSource al combobox  
            ListExpedientes.DataValueField = Tb.Columns(0).ColumnName  'Campo a capturar
            ListExpedientes.DataTextField = Tb.Columns(1).ColumnName 'Campo a mostar
            ListExpedientes.DataBind()
            For i As Integer = 0 To ListExpedientes.Items.Count - 1
                ListExpedientes.Items(i).Selected = True
            Next
        End If
    End Sub
    Private Function LeerExpediente() As String
        'If ListExpedientes.SelectedItem. < 2 Then
        '    Return ""
        'End If
        Dim exp As String = ""
        Dim separador As String = ""
        'AND  (A.EFINROEXP ='' or A.EFINROEXP = '')
        Try
            For i As Integer = 0 To ListExpedientes.Items.Count - 1
                If ListExpedientes.Items(i).Selected Then
                    exp &= separador & " A.EFINROEXP = '" & ListExpedientes.Items(i).Value & "'"
                    separador = " OR "
                End If
            Next
            If exp <> "" Then
                exp = " AND ( " & exp & " )"
            End If

        Catch ex As Exception
            menssageError(ex.Message)
        End Try
        Return exp
    End Function

    Private Function LeerExpediente_individual() As String()
        'If ListExpedientes.SelectedItem. < 2 Then
        '    Return ""
        'End If

        Dim exp(ListExpedientes.Items.Count) As String
        Dim separador As String = ""
        'AND  (A.EFINROEXP ='' or A.EFINROEXP = '')
        Try

            For i As Integer = 0 To ListExpedientes.Items.Count - 1

                If ListExpedientes.Items(i).Selected Then
                    exp(i) = ListExpedientes.Items(i).Value
                End If
            Next


        Catch ex As Exception
            menssageError(ex.Message)
        End Try
        Return exp
    End Function


    Private Function LLenarDatos2(ByVal Tabla As DataTable, ByVal CodRep As Integer, ByVal ColumnasAdicionales As String, Optional ByVal parametros As String = "") As Integer
        Try
            Dim exp() As String = LeerExpediente_individual()
            Dim sql As String

            For i As Integer = 0 To exp.Length


                Select Case CodRep

                    Case 217, 219, 221, 222, 301, 307, 329
                        sql = _
                        "SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                        " 	   G.nombre AS ED_TipoId,(ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END)AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                        " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS Email, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as Telefono,                                                                                     " & _
                        " 	   E.MT_nro_titulo,F.nombre AS MT_tipo_titulo, E.MT_fec_expedicion_titulo,E.MT_fec_notificacion_titulo,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                        " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                        " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                        " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                        " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                        " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                        " 	   c.idunico as IdDireccion,F.codigo as codigotitulo,A.EFINUMMEMO" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & _
                        "FROM  EJEFISGLOBAL A,ENTES_DEUDORES B, DIRECCIONES C, DEUDORES_EXPEDIENTES D,MAESTRO_TITULOS E,TIPOS_TITULO F,TIPOS_IDENTIFICACION G,                                                                                                                   " & _
                        "		DEPARTAMENTOS H,MUNICIPIOS I, USUARIOS K,  USUARIOS L                                                                                                                                                                     " & _
                        "WHERE A.EFINROEXP = D.NROEXP AND D.DEUDOR = B.ED_CODIGO_NIT AND B.ED_CODIGO_NIT = C.DEUDOR AND E.MT_EXPEDIENTE = A.EFINROEXP                                                                                                                            " & _
                        "		AND   F.CODIGO = E.MT_TIPO_TITULO  AND   G.CODIGO  = B.ED_TIPOID AND   H.CODIGO = C.DEPARTAMENTO AND  I.CODIGO = C.CIUDAD                                                                                                                        " & _
                        "	    AND K.CODIGO = A.EFIUSUASIG AND L.CODIGO = A.EFIUSUREV                                                                                                                                             " & _
                        " " & parametros & " " & _
                        " " & exp(i) & "                                                                                                                                                                                                                 " & _
                        "ORDER BY A.EFINROEXP"
                    Case 228, 13, 233, 354, 355, 356, 226, 363
                        sql = _
                        " SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                        " 	   G.nombre AS ED_TipoId,(ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END) AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                        " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                        " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,E.MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                        " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                        " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                        " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                        " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                        " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                        " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,F.codigo as codigotitulo,A.EFIFECHAEXP" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & "" & _
                        " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                     " & _
                        " 	  ENTES_DEUDORES B,                                                                                                                                                                                                                     " & _
                        " 	  DIRECCIONES C ,                                                                                                                                                                                                                       " & _
                        " 	  DEUDORES_EXPEDIENTES D,                                                                                                                                                                                                               " & _
                        " 	  MAESTRO_TITULOS E,                                                                                                                                                                                                                    " & _
                        " 	  TIPOS_TITULO F,                                                                                                                                                                                                                       " & _
                        " 	  TIPOS_IDENTIFICACION G,                                                                                                                                                                                                               " & _
                        " 	  DEPARTAMENTOS H,                                                                                                                                                                                                                      " & _
                        " 	  MUNICIPIOS I,                                                                                                                                                                                                                         " & _
                        " 	  FORMAS_NOTIFICACION J,                                                                                                                                                                                                                " & _
                        " 	  USUARIOS K,                                                                                                                                                                                                                           " & _
                        " 	  USUARIOS L                                                                                                                                                                                                                            " & _
                        " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                        " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                        " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                        " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                        " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                        " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                        " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                        " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                        " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                        " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                        " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                        " " & parametros & " " & _
                        " " & exp(i) & "                                                                                                                                                                                                                               " & _
                        " ORDER BY TIPO"

                    Case Else
                        sql = _
                        "SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                        " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                        " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS Email, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as Telefono,                                                                                     " & _
                        " 	   E.MT_nro_titulo,F.nombre AS MT_tipo_titulo, E.MT_fec_expedicion_titulo,isnull(MT_fec_notificacion_titulo ,00) as MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                        " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                        " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                        " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                        " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                        " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                        " 	   c.idunico as IdDireccion,F.codigo as codigotitulo,A.EFINUMMEMO" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & _
                        "FROM  EJEFISGLOBAL A,ENTES_DEUDORES B, DIRECCIONES C, DEUDORES_EXPEDIENTES D,MAESTRO_TITULOS E,TIPOS_TITULO F,TIPOS_IDENTIFICACION G,                                                                                                                   " & _
                        "		DEPARTAMENTOS H,MUNICIPIOS I, USUARIOS K,  USUARIOS L,FORMAS_NOTIFICACION J                                                                                                                                                                      " & _
                        "WHERE A.EFINROEXP = D.NROEXP AND D.DEUDOR = B.ED_CODIGO_NIT AND B.ED_CODIGO_NIT = C.DEUDOR AND E.MT_EXPEDIENTE = A.EFINROEXP                                                                                                                            " & _
                        "		AND   F.CODIGO = E.MT_TIPO_TITULO  AND   G.CODIGO  = B.ED_TIPOID AND   H.CODIGO = C.DEPARTAMENTO AND  I.CODIGO = C.CIUDAD                                                                                                                        " & _
                        "		AND   J.CODIGO = isnull(MT_for_notificacion_titulo ,00) AND K.CODIGO = A.EFIUSUASIG AND L.CODIGO = A.EFIUSUREV                                                                                                                                             " & _
                        " " & parametros & " " & _
                        "AND A.EFINROEXP ='" & exp(i) & " '                                                                                                                                                                                                                " & _
                        "ORDER BY A.EFINROEXP"

                End Select

                Dim Ado As New SqlConnection(Funciones.CadenaConexion)
                Dim ad As New SqlClient.SqlDataAdapter(sql, Ado)
                ad.Fill(Tabla)

                Return Tabla.Rows.Count
            Next
        Catch ex As Exception
            menssageError(ex.Message)
        End Try

    End Function

    Private Function LLenarDatos(ByVal Tabla As DataTable, ByVal CodRep As Integer, ByVal ColumnasAdicionales As String, Optional ByVal parametros As String = "") As Integer
        Try
            Dim exp As String = LeerExpediente()
            Dim sql As String
            Select Case CodRep
                Case 217, 219, 221, 222, 301, 307, 329
                    sql = _
                    "SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                    " 	   G.nombre AS ED_TipoId,(ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END)AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                    " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS Email, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as Telefono,                                                                                     " & _
                    " 	   E.MT_nro_titulo,F.nombre AS MT_tipo_titulo, E.MT_fec_expedicion_titulo,E.MT_fec_notificacion_titulo,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                    " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                    " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                    " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                    " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                    " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                    " 	   c.idunico as IdDireccion,F.codigo as codigotitulo,A.EFINUMMEMO" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & _
                    "FROM  EJEFISGLOBAL A,ENTES_DEUDORES B, DIRECCIONES C, DEUDORES_EXPEDIENTES D,MAESTRO_TITULOS E,TIPOS_TITULO F,TIPOS_IDENTIFICACION G,                                                                                                                   " & _
                    "		DEPARTAMENTOS H,MUNICIPIOS I, USUARIOS K,  USUARIOS L                                                                                                                                                                     " & _
                    "WHERE A.EFINROEXP = D.NROEXP AND D.DEUDOR = B.ED_CODIGO_NIT AND B.ED_CODIGO_NIT = C.DEUDOR AND E.MT_EXPEDIENTE = A.EFINROEXP                                                                                                                            " & _
                    "		AND   F.CODIGO = E.MT_TIPO_TITULO  AND   G.CODIGO  = B.ED_TIPOID AND   H.CODIGO = C.DEPARTAMENTO AND  I.CODIGO = C.CIUDAD                                                                                                                        " & _
                    "	    AND K.CODIGO = A.EFIUSUASIG AND L.CODIGO = A.EFIUSUREV                                                                                                                                             " & _
                    " " & parametros & " " & _
                    " " & exp & "                                                                                                                                                                                                                 " & _
                    "ORDER BY A.EFINROEXP"
                Case 228, 13, 233, 354, 355, 356, 226, 363
                    sql = _
                    " SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                    " 	   G.nombre AS ED_TipoId,(ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END) AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                    " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS EMAIL, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as telefono,                                                                                     " & _
                    " 	   E.MT_nro_titulo,F.nombre AS MT_Tipo_Titulo, E.MT_fec_expedicion_titulo,E.MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                    " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                    " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                    " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                    " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                    " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                    " 	   c.idunico as IdDireccion,A.EFINUMMEMO,E.MT_fecha_ejecutoria,F.codigo as codigotitulo,A.EFIFECHAEXP" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & "" & _
                    " FROM  EJEFISGLOBAL A,                                                                                                                                                                                                                     " & _
                    " 	  ENTES_DEUDORES B,                                                                                                                                                                                                                     " & _
                    " 	  DIRECCIONES C ,                                                                                                                                                                                                                       " & _
                    " 	  DEUDORES_EXPEDIENTES D,                                                                                                                                                                                                               " & _
                    " 	  MAESTRO_TITULOS E,                                                                                                                                                                                                                    " & _
                    " 	  TIPOS_TITULO F,                                                                                                                                                                                                                       " & _
                    " 	  TIPOS_IDENTIFICACION G,                                                                                                                                                                                                               " & _
                    " 	  DEPARTAMENTOS H,                                                                                                                                                                                                                      " & _
                    " 	  MUNICIPIOS I,                                                                                                                                                                                                                         " & _
                    " 	  FORMAS_NOTIFICACION J,                                                                                                                                                                                                                " & _
                    " 	  USUARIOS K,                                                                                                                                                                                                                           " & _
                    " 	  USUARIOS L                                                                                                                                                                                                                            " & _
                    " WHERE A.EFINROEXP = D.NROEXP                                                                                                                                                                                                              " & _
                    " AND   D.DEUDOR = B.ED_Codigo_Nit                                                                                                                                                                                                          " & _
                    " AND   B.ED_Codigo_Nit = C.deudor                                                                                                                                                                                                          " & _
                    " AND   E.MT_expediente = A.EFINROEXP                                                                                                                                                                                                       " & _
                    " AND   F.codigo = E.MT_tipo_titulo                                                                                                                                                                                                         " & _
                    " AND   G.codigo  = B.ED_TipoId                                                                                                                                                                                                             " & _
                    " AND   H.codigo = C.Departamento                                                                                                                                                                                                           " & _
                    " AND   I.codigo = C.Ciudad                                                                                                                                                                                                                 " & _
                    " AND   J.codigo = E.MT_for_notificacion_titulo                                                                                                                                                                                             " & _
                    " AND   K.codigo = A.EFIUSUASIG                                                                                                                                                                                                             " & _
                    " AND   L.codigo = A.EFIUSUREV                                                                                                                                                                                                              " & _
                    " " & parametros & " " & _
                    " " & exp & "                                                                                                                                                                                                                               " & _
                    " ORDER BY TIPO"

                Case Else
                    sql = _
                    "SELECT A.EFINROEXP AS MAN_EXPEDIENTE,																																																		" & _
                    " 	   G.nombre AS ED_TipoId,ED_CODIGO_NIT + CASE WHEN ISNULL(ED_DigitoVerificacion,'') ='' THEN '' ELSE '-' END +CASE WHEN ED_DIGITOVERIFICACION =''THEN '' ELSE ISNULL(ED_DIGITOVERIFICACION,'') END AS ED_Codigo_Nit, B.ED_Nombre, CASE WHEN ED_TipoPersona = '02' THEN 'Señor(a)' ELSE 'Doctor(a)' END AS ED_TipoPersona, CASE WHEN ED_TipoPersona = '02' THEN 'Cuidadano' ELSE 'Empresario(a)' END AS Cuidadano,  " & _
                    " 	   C.Direccion,CASE WHEN C.EMAIL ='SIN DATOS' THEN '' ELSE C.EMAIL END AS Email, (case when isnull( C.Movil,'') = '' then '' else  case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' then '' else C.Movil end end)	 + 		(case when isnull( C.Movil,'')='Sin datos'or isnull( C.Movil ,'')= 'sin datos'or isnull( C.Movil ,'')= 'sin dato' or isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else ' - ' end)		 + 	    (case when isnull( C.Telefono ,'') = '' then '' else case when isnull( C.Telefono ,'')='Sin datos' or isnull( C.Telefono ,'')= 'sin datos'or isnull( C.Telefono ,'')= 'sin dato' then '' else C.Telefono  end end ) as Telefono,                                                                                     " & _
                    " 	   E.MT_nro_titulo,F.nombre AS MT_tipo_titulo, E.MT_fec_expedicion_titulo,isnull(MT_fec_notificacion_titulo ,00) as MT_fec_notificacion_titulo,J.nombre AS MT_for_notificacion_titulo  ,'$ '+CAST(CONVERT(varchar, CAST(E.totaldeuda AS money), 1) AS varchar) as Total_Deuda,E.totaldeuda,                                                                           " & _
                    " 	   CASE WHEN H.NOMBRE ='SIN DATOS'THEN '' ELSE H.NOMBRE END AS Departamento,                                                                                                                                                                                                            " & _
                    " 	   CASE WHEN I.NOMBRE ='SIN DATOS'THEN '' ELSE I.NOMBRE END AS Municipio,                                                                                                                                                                                                                 " & _
                    " 	   k.nombre as Proyecto,                                                                                                                                                                                                                " & _
                    " 	   L.nombre as Revisor,                                                                                                                                                                                                                 " & _
                    " 	   D.TIPO as Tipo_Deudor,                                                                                                                                                                                                               " & _
                    " 	   c.idunico as IdDireccion,F.codigo as codigotitulo,A.EFINUMMEMO" & IIf(ColumnasAdicionales = "", "", "," & ColumnasAdicionales & " ") & _
                    "FROM  EJEFISGLOBAL A,ENTES_DEUDORES B, DIRECCIONES C, DEUDORES_EXPEDIENTES D,MAESTRO_TITULOS E,TIPOS_TITULO F,TIPOS_IDENTIFICACION G,                                                                                                                   " & _
                    "		DEPARTAMENTOS H,MUNICIPIOS I, USUARIOS K,  USUARIOS L,FORMAS_NOTIFICACION J                                                                                                                                                                      " & _
                    "WHERE A.EFINROEXP = D.NROEXP AND D.DEUDOR = B.ED_CODIGO_NIT AND B.ED_CODIGO_NIT = C.DEUDOR AND E.MT_EXPEDIENTE = A.EFINROEXP                                                                                                                            " & _
                    "		AND   F.CODIGO = E.MT_TIPO_TITULO  AND   G.CODIGO  = B.ED_TIPOID AND   H.CODIGO = C.DEPARTAMENTO AND  I.CODIGO = C.CIUDAD                                                                                                                        " & _
                    "		AND   J.CODIGO = isnull(MT_for_notificacion_titulo ,00) AND K.CODIGO = A.EFIUSUASIG AND L.CODIGO = A.EFIUSUREV                                                                                                                                             " & _
                    " " & parametros & " " & _
                    "" & exp & "                                                                                                                                                                                                                 " & _
                    "ORDER BY A.EFINROEXP"

            End Select

            Dim Ado As New SqlConnection(Funciones.CadenaConexion)
            Dim ad As New SqlClient.SqlDataAdapter(sql, Ado)
            ad.Fill(Tabla)
            Return Tabla.Rows.Count
        Catch ex As Exception
            Return 0
        End Try
    End Function


    Private Sub Imprimir(ByVal CodInforme As String)
        Dim worddoc As New WordReport
        Dim worddocresult As String = "", NomInforme As String

        Select Case CodInforme
            'Persuasivos Masivos'
            Case "217"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 217, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioDeCobroPersuasivoomisos)
                    NomInforme = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "218"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 218, "'' as fecha1,'' as ED_Rep,'' as Letras, '' as fecha2") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioDeCobroPersuasivoomisos)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "219"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 219, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioDeCobroPersuasivoFosiga)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "220"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 220, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioDeCobroPersuasivoFosiga)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "221"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 221, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimeOficioPersuasivoMultaDirectoFosiga)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "222"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 222, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.TrasladoMinsterio)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "223"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 223, "'' as fecha1,'' as fecha2,'' as ED_Rep,'' as Letras,'' as informante") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("informante") = GETPROCEDENCIA(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        Else

                        End If
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SolicitudDocumentosPago)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If

            Case "224"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 223, "'' as fecha1,'' as fecha2,'' as ED_Rep,'' as Letras,'' as informante") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("informante") = GETPROCEDENCIA(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        Else

                        End If
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.VerificacionPagoAprobado)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If


            Case "301"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 301, "'' as fecha1,'' as ED_Rep,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioDeCobroPersuasivo)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "302"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 218, "'' as fecha1,'' as ED_Rep,'' as Letras, '' as fecha2") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioDeCobroPersuasivo)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "303"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 301, "'' as fecha1,'' as ED_Rep,'' as valorL") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("valorL") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioCondenaJudicial)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "304"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 304, "'' as fecha1,'' as ED_Rep,'' as valorL,'' as fecha2") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("valorL") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioCondenaJudicial)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "305"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 305, "'' as fecha1,'' as ED_Rep,'' as valorL,'' as fecha2") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("valorL") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioMulta1607)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "306"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 306, "'' as fecha1,'' as ED_Rep,'' as valorL,'' as fecha2") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("valorL") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioMulta1607)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "307"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 301, "'' as fecha1,'' as ED_Rep,'' as valorL") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("valorL") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioMulta1438)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "311"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 311, "'' as fecha1,'' as ED_Rep,'' as letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.RespuestaSolicitudPazSalvo)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "312"

                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 312, "'' as fecha1,'' as ED_Rep,'' as letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReport(TbDatos, Reportes.TrasladoPorCompetenciasDireccionParafiscales)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "313"

                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 313, "'' as fecha1,'' as ED_Rep,'' as letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                        If TbDatos.Rows(i).Item("Municipio") = "BOGOTA" Then
                            TbDatos.Rows(i).Item("Municipio") = "BOGOTA D.C"
                            TbDatos.Rows(i).Item("Departamento") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReport(TbDatos, Reportes.ContestacionRevocatoriaDirecta)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "329"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 329, "'' as fecha1,'' as ED_Rep,'' as letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PrimerOficioCuotasPartes)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If

            Case "330"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 330, "'' as fecha1,'' as fecha2,'' as ED_Rep,'' as letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.SegundoOficioCuotasPartes)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If


                '----COBRO COOACTIVO-----'

            Case "228"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 228, "'' as fecha1,'' as NRO,'' as Letras") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.TrasladoMinsterio)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If

            Case "233"
                Dim TbDatos As New Data.DataTable
                If LLenarDatos(TbDatos, 233, "'' as fecha1,'' as fecha2,'' as letras,'' as nro_resolucion2,'' as fecha_reg,'' as fecha_actual,'' as Nro_resolucion") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))

                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 233)
                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion2") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_reg") = resolucion(1)
                        End If
                        Dim guardar() As String = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 233)
                        If Not guardar Is Nothing Then
                            TbDatos.Rows(i).Item("fecha_actual") = guardar(0)
                            TbDatos.Rows(i).Item("Nro_resolucion") = guardar(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.Aprobaciónliquidacióndelcrédito)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "013"
                Dim TbDatos As New Data.DataTable
                NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                System.IO.Directory.CreateDirectory(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & NomInforme & CDate(Today.Date).ToString("dd.MM.yyyy"))
                If LLenarDatos(TbDatos, 13, "'' as fecha1,'' as fecha_actual,'' as letras,'' as Nro_Resolucion, '' as fecha_ejecutoria") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_ejecutoria") = CDate(TbDatos.Rows(i).Item("MT_fecha_ejecutoria")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim guardar() As String
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        SaveTable(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        If Not guardar Is Nothing Then
                            TbDatos.Rows(i).Item("Nro_Resolucion") = guardar(0)
                            TbDatos.Rows(i).Item("fecha_actual") = guardar(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()

                    worddocresult = worddoc.CreateReportMasivoResoluciones(TbDatos, Reportes.MandamientoPagoPorPilaMasivo, NomInforme, lblNomPerfil.Text)

                End If

            Case "354"

                Dim TbDatos As New Data.DataTable
                NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                System.IO.Directory.CreateDirectory(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & NomInforme & CDate(Today.Date).ToString("dd.MM.yyyy"))
                If LLenarDatos(TbDatos, 354, "'' as fecha1,'' as fecha_actual,'' as letras,'' as Nro_Resolucion,'' as fecha_ejecutoria") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_ejecutoria") = CDate(TbDatos.Rows(i).Item("MT_fecha_ejecutoria")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim guardar() As String
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        SaveTable(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        If Not guardar Is Nothing Then
                            TbDatos.Rows(i).Item("Nro_Resolucion") = guardar(0)
                            TbDatos.Rows(i).Item("fecha_actual") = guardar(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivoResoluciones(TbDatos, Reportes.CoactivoMPConPagoEnFosygaMasivo, NomInforme, lblNomPerfil.Text)

                End If
            Case "355"

                Dim TbDatos As New Data.DataTable
                NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                System.IO.Directory.CreateDirectory(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & NomInforme & CDate(Today.Date).ToString("dd.MM.yyyy"))
                If LLenarDatos(TbDatos, 354, "'' as fecha1,'' as fecha_actual,'' as letras,'' as Nro_Resolucion,'' as fecha_ejecutoria") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_ejecutoria") = CDate(TbDatos.Rows(i).Item("MT_fecha_ejecutoria")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim guardar() As String
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        SaveTable(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        guardar = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "013")
                        If Not guardar Is Nothing Then
                            TbDatos.Rows(i).Item("Nro_Resolucion") = guardar(0)
                            TbDatos.Rows(i).Item("fecha_actual") = guardar(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivoResoluciones(TbDatos, Reportes.CoactivoMPConRecursoModificatorioMasivo, NomInforme, lblNomPerfil.Text)

                End If
            Case "056"

                Dim TbDatos As New Data.DataTable
                If LLenarDatos(TbDatos, 56, "'' as fecha1,'' as fecha_actual,'' as letras,'' as nro_resolucion, '' as fecha_reg") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_reg") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "056")

                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_reg") = resolucion(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.NotificacionPorCorreo)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "317"
                Dim TbDatos As New Data.DataTable
                If LLenarDatos(TbDatos, 317, "'' as fecha1,'' as fecha_actual,'' as letras,'' as nro_resolucion, '' as fecha_resolucion,'' as fecha_reg") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_reg") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "317")

                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_resolucion") = resolucion(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.CitacionMandamientoPago)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "318"

                Dim TbDatos As New Data.DataTable
                If LLenarDatos(TbDatos, 318, "'' as fecha1,'' as fecha_actual,'' as letras,'' as nro_resolucion, '' as fecha_resolucion,'' as fecha_reg") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha_reg") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "317")

                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_resolucion") = resolucion(1)
                        End If

                    Next
                    TbDatos.AcceptChanges()

                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.CitacionMandamientoPago)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "314"

                Dim TbDatos As New Data.DataTable
                If LLenarDatos(TbDatos, 314, "'' as fecha1,'' as fecha_actual,'' as Letras,'' as nro_resolucion, '' as fecha_resolucion,'' as fecha_reg,'' as fecha2,'' as Nro_res") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1

                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        TbDatos.Rows(i).Item("fecha2") = CDate(TbDatos.Rows(i).Item("MT_fec_notificacion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "314")
                        ''Dim vcdatos() As String = overloadresolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "314")
                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_reg") = resolucion(1)
                        End If
                        Dim guardar() As String = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "314")
                        If Not guardar Is Nothing Then
                            TbDatos.Rows(i).Item("Nro_res") = guardar(0)
                            TbDatos.Rows(i).Item("fecha_actual") = guardar(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.AperturaDePruebasLiquidacion)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If


            Case "325"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 325, "'' as fecha1,'' as ED_Rep,'' as nro_resolucion, '' as fecha_reg ") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                        NomED_Rep = cargar_representante(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), 1)
                        If NomED_Rep <> Nothing Then
                            TbDatos.Rows(i).Item("ED_Rep") = NomED_Rep
                        Else
                            TbDatos.Rows(i).Item("ED_Rep") = TbDatos.Rows(i).Item("ED_Nombre")
                            TbDatos.Rows(i).Item("ED_Nombre") = ""
                        End If
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "325")
                        If Not resolucion Is Nothing Then
                            TbDatos.Rows(i).Item("nro_resolucion") = resolucion(0)
                            TbDatos.Rows(i).Item("fecha_reg") = resolucion(1)
                        End If
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.NOTIFICACIONABREAPRUEBAS)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If


                '----- PRESENTACION CONCURSAL -----'

            Case "244"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 244, "'' as fecha1,'' as Deuda_Letras, '' as Deuda_Valor ") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("Deuda_Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("Deuda_Valor") = FormatCurrency(TbDatos.Rows(i).Item("totaldeuda"))
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PresentacionDelCreditoCuotasPartes)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case "245"
                Dim TbDatos As New Data.DataTable
                Dim NomED_Rep As String
                If LLenarDatos(TbDatos, 245, "'' as fecha1,'' as Letras, '' as Deuda_Valor ") > 0 Then
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        TbDatos.Rows(i).Item("Letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("Deuda_Valor") = FormatCurrency(TbDatos.Rows(i).Item("totaldeuda"))
                        TbDatos.Rows(i).Item("fecha1") = CDate(TbDatos.Rows(i).Item("MT_fec_expedicion_titulo")).ToString("'del' dd 'de' MMMM 'de' yyy")
                    Next
                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.PresentacionDelCreditoParafiscal)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
                '--------Enbargos-------'
            Case "319"
                ModalPopupExtender3.Show()
                Exit Sub
            Case "319-1"
                Dim TbDatos As New Data.DataTable
                Dim embargos As Integer = Limite.Text.Trim
                Dim ant As String
                If LLenarDatos(TbDatos, 319, " '' as fecha1       ,  '' as letras,            '' as Deuda_Valor , '' as nro_anterior ,  '' as fecha_resolucion," & _
                                             " '' as fecha_actual ,  '' as Nro_resolucion ,   '' as pxem        , '' as Total_Embargo , '' as embargol ", "AND f.codigo ='01'") > 0 Then

                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        Dim embargos_r As Double = ((embargos / 100) * TbDatos.Rows.Item(i).Item("totaldeuda"))
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "319")
                        If Not resolucion Is Nothing Then
                            TbDatos.Rows.Item(i).Item("nro_anterior") = resolucion(0)
                            TbDatos.Rows.Item(i).Item("fecha_resolucion") = resolucion(1)
                        Else
                            menssageError("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO")
                            Exit Sub
                        End If

                        If embargos_r > 0 Then
                            Dim guardar() As String = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "319", 1)


                            TbDatos.Rows.Item(i).Item("fecha_actual") = guardar(1)
                            TbDatos.Rows.Item(i).Item("Nro_resolucion") = guardar(0)
                            TbDatos.Rows.Item(i).Item("pxem") = embargos
                            TbDatos.Rows.Item(i).Item("Total_Embargo") = String.Format("{0:C0}", embargos_r)
                            TbDatos.Rows.Item(i).Item("embargol") = Num2Text(embargos_r)
                            TbDatos.Rows.Item(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))

                            If TbDatos.Rows(i).Item("MAN_EXPEDIENTE") <> ant Then
                                SaveTable(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "319", guardar(0), CDate(guardar(2)), embargos_r, embargos)
                                ant = TbDatos.Rows(i).Item("MAN_EXPEDIENTE")
                            End If


                        Else
                            menssageError("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                            Exit Sub
                        End If
                    Next

                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.EmbargoBancarioLiquidacionOficial)
                    NomInforme = Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")

                End If


            Case "320"
                ModalPopupExtender3.Show()
                Exit Sub
            Case "320-1"
                Dim TbDatos As New Data.DataTable
                Dim embargos As Integer = Limite.Text.Trim
                Dim ant As String
                If LLenarDatos(TbDatos, 244, " '' as fecha1       ,  '' as letras,            '' as Deuda_Valor , '' as nro_anterior ,  '' as fecha_resolucion," & _
                                             " '' as fecha_actual ,  '' as Nro_resolucion ,   '' as pxem        , '' as Total_Embargo , '' as embargol ", "AND (f.codigo ='05' or f.codigo ='06' or f.codigo ='07')") > 0 Then
                    Dim NA(TbDatos.Rows.Count) As String
                    For i As Integer = 0 To TbDatos.Rows.Count - 1
                        'Dim NA(TbDatos.Rows.Count) As String

                        Dim embargos_r As Double = ((embargos / 100) * TbDatos.Rows.Item(i).Item("totaldeuda"))
                        Dim resolucion() As String = getResolucion_anterior(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "320")
                        If Not resolucion Is Nothing Then
                            TbDatos.Rows.Item(i).Item("nro_anterior") = resolucion(0)
                            TbDatos.Rows.Item(i).Item("fecha_resolucion") = resolucion(1)
                        Else
                            menssageError("NO HAY RESOLUCION DE (013)MANDAMIENTO DE PAGO PARA IMPRIMIR EL DOCUMENTO")
                            Exit Sub
                        End If

                        If embargos_r > 0 Then
                            Dim guardar() As String = saveResolucion(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "319", 1)


                            TbDatos.Rows.Item(i).Item("fecha_actual") = guardar(1)
                            TbDatos.Rows.Item(i).Item("Nro_resolucion") = guardar(0)
                            TbDatos.Rows.Item(i).Item("pxem") = embargos
                            TbDatos.Rows.Item(i).Item("Total_Embargo") = String.Format("{0:C0}", embargos_r)
                            TbDatos.Rows.Item(i).Item("embargol") = Num2Text(embargos_r)
                            TbDatos.Rows.Item(i).Item("letras") = Num2Text(TbDatos.Rows(i).Item("totaldeuda"))

                            If TbDatos.Rows(i).Item("MAN_EXPEDIENTE") <> ant Then
                                SaveTable(TbDatos.Rows(i).Item("MAN_EXPEDIENTE"), "320", guardar(0), CDate(guardar(2)), embargos_r, embargos)
                                ant = TbDatos.Rows(i).Item("MAN_EXPEDIENTE")
                            End If

                        Else
                            menssageError("NO HAY REGISTRADO PROCENTAJE DE PARA EL LIMITE  DEL EMBARGO ")
                            Exit Sub
                        End If

                    Next

                    'If TbDatos.Rows(i).Item("codigotitulo") = "05" Or TbDatos.Rows(i).Item("codigotitulo") = "06" Or TbDatos.Rows(i).Item("codigotitulo") = "07" Then
                    'Else

                    '    NA(i) = TbDatos.Rows(i).Item("MAN_EXPEDIENTE")
                    'End If

                    'list_embargos.Items.Clear()
                    'For i As Integer = 0 To NA.Length - 1
                    '    If NA(i) > "" Then
                    '        list_embargos.Items.Add(NA(i))
                    '    End If

                    'Next


                    TbDatos.AcceptChanges()
                    worddocresult = worddoc.CreateReportMasivo(TbDatos, Reportes.EmbargoCuentaBancariaMulta)
                    NomInforme = "Masivo-" & Replace(Replace(Lista.SelectedItem.Text.Trim, " ", "."), "-", ".")
                End If
            Case Else
                menssageError("ESTE INFORME NO SE ENCUENTRA DISPONIBLE ACTUALMENTE.")
        End Select
        If worddocresult = "" Then
            menssageError("No se pudo generar el informe.")
        Else
            SendReport(NomInforme, worddocresult)


        End If
    End Sub

    Private Sub SendReport(ByVal NombreArchivo As String, ByVal Plantilla As String)
        Dim LIST As String = Lista.SelectedItem.Value
        If LIST = "013" Or LIST = "354" Or LIST = "355" Then
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.zip", NombreArchivo))
            Response.WriteFile(Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "Masivos\" & NombreArchivo & CDate(Today.Date).ToString("dd.MM.yyyy") & ".zip")
            Response.Flush()
        Else
            Response.ContentType = "application/msword"
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.doc", NombreArchivo))
            Response.Write(Plantilla)
            Response.Flush()
        End If
        
    End Sub

    Protected Sub A3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles A3.Click
        FormsAuthentication.SignOut()
        'Limpiar cuadros de busqueda
        Session("EJEFISGLOBAL.txtSearchEFINROEXP") = ""
        Session("EJEFISGLOBAL.txtSearchEFINUMMEMO") = ""
        Session("EJEFISGLOBAL.txtSearchEFIEXPORIGEN") = ""
        Session("EJEFISGLOBAL.txtSearchEFIVALDEU") = ""
        Response.Redirect("../login.aspx")
    End Sub
    Protected Sub ABack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ABack.Click
        Response.Redirect("Maestros/EJEFISGLOBAL.aspx")
    End Sub

    Protected Sub CheckHeader_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckHeader.CheckedChanged
        For i As Integer = 0 To ListExpedientes.Items.Count - 1
            ListExpedientes.Items(i).Selected = CheckHeader.Checked
        Next
    End Sub


    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImprimir.Click
        If Session("UsuarioValido") Is Nothing Then
            menssageError("La session de usuario caduco")
            Exit Sub
        End If
        If Lista.SelectedValue = Nothing Then
            menssageError("Debe elegir un reporte para continuar.")
            Exit Sub
        End If
        If LeerExpediente() = "" Then
            menssageError("Debe seleccionar por lo menos 2 expedientes para continuar.")
            Exit Sub
        End If

        Imprimir(Lista.SelectedValue)

    End Sub
    Protected Sub BtnPersuasivo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnPersuasivo.Click
        Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA AND DXI_ACTOMASIVO = 1 ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", "01")
    End Sub
    Protected Sub BtnCoactivos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCoactivos.Click
        Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA AND DXI_ACTOMASIVO = 1 ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", "02")
    End Sub
    Protected Sub BtnMedidas_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnMedidas.Click
        Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA AND DXI_ACTOMASIVO = 1 ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", "05")
    End Sub
    Protected Sub BtnConcursales_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnConcursales.Click
        Call cargar_objeto(Lista, "SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] A, ACTUACIONES B WHERE  A.DXI_ACTO = B.CODIGO AND (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) AND B.IDETAPA = @IDETAPA AND DXI_ACTOMASIVO = 1 ORDER BY 1", "DXI_ACTO", "DXI_NOMBREACTO", "04")
    End Sub

    Private Sub btn_enviar_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_enviar_2.Click
        If IsNumeric(Limite.Text.Trim) Then
            Select Case Lista.SelectedValue
                Case "319"
                    Imprimir("319-1")
                    ModalPopupExtender3.Hide()
                Case "320"
                    Imprimir("320-1")
            End Select
        Else
            menssageError("EL VALOR DEL CAMPO LIMITE DEBE SER NUMERICO")
        End If



    End Sub
End Class
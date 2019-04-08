Imports System.Data
Imports System.Data.SqlClient

Partial Public Class EditENTES_DEUDORES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'GetMunicipiosByDpto("13")

            ' Tipos de entes / personas
            ' 1 = DEUDOR PRINCIPAL
            ' 2 = DEUDOR SOLIDARIO
            ' 3 = REPRESENTANTE LEGAL
            ' 4 = APODERADO
            ' 5 = AUTORIZADO
            If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
                lblPage1.Text = "Información del deudor"

            ElseIf Request("pTipo").Trim = "3" Then
                lblPage1.Text = "Información del Representante legal"

            ElseIf Request("pTipo").Trim = "4" Then
                lblPage1.Text = "Información del apoderado"

            Else
                lblPage1.Text = "Información del autorizado"
            End If

            'Si es deudor principal se activa "cboED_TipoPersona" y "cboED_EstadoPersona"
            If Request("pTipo").Trim = "1" Then
                cboED_TipoPersona.Enabled = True
                cboED_EstadoPersona.Enabled = True
            Else
                cboED_TipoPersona.Enabled = False
                cboED_EstadoPersona.Enabled = False
            End If

            If Len(Request("pNomTipoDeudor")) > 0 Then
                If Request("pNomTipoDeudor").Trim = "DEUDOR SOLIDARIO" Then
                    txtParticipacion.Enabled = True
                Else
                    txtParticipacion.Enabled = False
                End If
            End If

            If Not (Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2") Then
                cboTipoDeudor.Visible = False
                lblTipoDeudor.Visible = False
                cboED_TipoAportante.Visible = False
                lblTipoAportante.Visible = False
                txtParticipacion.Enabled = False
            End If

            ' La tarjeta profesional solo le debe aparecer al apoderado
            If Request("pTipo").Trim = "4" Then
                lblTarjetaProf.Visible = True
                txtTarjetaProf.Visible = True
            Else
                lblTarjetaProf.Visible = False
                txtTarjetaProf.Visible = False
            End If
            '
            LoadcboED_TipoId()
            LoadcboED_TipoPersona()
            LoadcboED_EstadoPersona()
            LoadcboED_TipoAportante()
            LoadTipoEnte()

            'Si no hay registros de este expediente en la tabla DEUDORES_EXPEDIENTES => Colocar Tipo de deudor en "DEUDOR PRINCIPAL"
            If Not ExisteDeudorPrincipal(Request("pExpediente").Trim) Then                
                cboTipoDeudor.SelectedValue = "1"
            Else
                'Existe deudor principal
                If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
                    cboTipoDeudor.SelectedValue = "2"

                    'El tipo de aportante que se coloco en deudor principal, colocarlo en deudor secudario
                    Dim DeudorPpal As String = GetDeudorPrincipal(Request("pExpediente").Trim)
                    Dim TipoAportante As String = GetTipoAportante(DeudorPpal)
                    If TipoAportante.Trim <> "" Then
                        cboED_TipoAportante.SelectedValue = TipoAportante

                        If Len(Request("ID")) = 0 Then ' Esta adicionando
                            cboED_TipoAportante.Enabled = False
                        End If

                    End If

                End If
            End If

            'MostrarDeudor
            If Len(Request("ID")) > 0 Then
                MostrarDeudor(Request("ID").Trim)
            Else
                'Esta adicionanado
                cboED_TipoPersona.SelectedValue = "01" 'Natural
                cboED_EstadoPersona.SelectedValue = "07" 'Viva
                txtParticipacion.Text = 0

                cmdBorrar.Visible = False
            End If

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO                
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar deudores"
                CustomValidator1.IsValid = False

                'Deshabilitar / Ocultar controles
                DesactivarControles()
            End If

            'Si el abogado que esta logeado es diferente al responsable del expediente => impedir edicion
            If Session("mnivelacces") <> 5 And Session("mnivelacces") <> 8 Then
                Dim idGestorResp As String = MTG.GetIDGestorResp(Request("pExpediente"))
                If idGestorResp <> Session("sscodigousuario") Then

                    CustomValidator1.Text = "Este expediente está a cargo de otro gestor. No permiten adicionar datos"
                    CustomValidator1.IsValid = False

                    'Deshabilitar / Ocultar controles
                    DesactivarControles()
                Else
                    'El gestor logueado es el reponsable, pero NO es el repartidor
                    If Request("pTipo").Trim = "1" Then 'Validacion para deudor principal

                        If Len(Request("ID")) > 0 Then
                            If Len(Request("pNomTipoDeudor")) > 0 Then

                                If Request("pNomTipoDeudor").Trim = "DEUDOR PRINCIPAL" Then
                                    'Esta editando
                                    CustomValidator1.Text = "El deudor principal solo es editable en el módulo de clasificación y reparto"
                                    CustomValidator1.IsValid = False

                                    DesactivarControles()
                                End If

                            End If
                        End If

                    End If
                End If
            End If


        End If
    End Sub

    Private Function GetDeudorPrincipal(ByVal pExpediente As String) As String
        Dim DeudorPrincipal As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT deudor FROM DEUDORES_EXPEDIENTES WHERE NroExp = '" & pExpediente.Trim & "' AND tipo = 1"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            DeudorPrincipal = Reader("deudor").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()

        Return DeudorPrincipal
    End Function

    Private Function GetTipoAportante(ByVal pDeudorPrincipal As String) As String
        Dim TipoAportante As String = ""
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT ED_TipoAportante FROM ENTES_DEUDORES WHERE ED_Codigo_Nit = '" & pDeudorPrincipal & "'"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        If Reader.Read Then
            TipoAportante = Reader("ED_TipoAportante").ToString().Trim
        End If
        Reader.Close()
        Connection.Close()

        Return TipoAportante
    End Function

    Private Function ExisteDeudorPrincipal(ByVal pExpediente As String) As Boolean
        Dim Respuesta As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT NroExp FROM DEUDORES_EXPEDIENTES WHERE NroExp = '" & pExpediente.Trim & "' AND tipo = 1"
        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            Respuesta = True
        End If
        Return Respuesta

    End Function

    Private Sub LoadTipoEnte()
        Dim Modo As String = "ADD" 'o EDT
        Dim SwExisteDedudorPrincipal As Boolean = ExisteDeudorPrincipal(Request("pExpediente").Trim)

        If Len(Request("ID")) > 0 Then
            Modo = "EDT"
        End If

        Dim dt As DataTable = New DataTable("Tabla")

        dt.Columns.Add("codigo")
        dt.Columns.Add("nombre")

        Dim dr As DataRow



        If Modo = "ADD" Then
            If SwExisteDedudorPrincipal Then
                dr = dt.NewRow()
                dr("codigo") = 0
                dr("nombre") = "SELECCIONE..."
                dt.Rows.Add(dr)

                dr = dt.NewRow()
                dr("codigo") = 2
                dr("nombre") = "DEUDOR SOLIDARIO"
                dt.Rows.Add(dr)

            Else
                dr = dt.NewRow()
                dr("codigo") = 1
                dr("nombre") = "DEUDOR PRINCIPAL"
                dt.Rows.Add(dr)

            End If
        Else
            'Modo = "EDT"
            dr = dt.NewRow()
            dr("codigo") = 0
            dr("nombre") = "SELECCIONE..."
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            dr("codigo") = 2
            dr("nombre") = "DEUDOR SOLIDARIO"
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            dr("codigo") = 1
            dr("nombre") = "DEUDOR PRINCIPAL"
            dt.Rows.Add(dr)

        End If        

        cboTipoDeudor.DataSource = dt
        cboTipoDeudor.DataTextField = "nombre"
        cboTipoDeudor.DataValueField = "codigo"
        cboTipoDeudor.DataBind()
    End Sub

    Private Sub DesactivarControles()
        cmdSave.Visible = False
        txtED_Codigo_Nit.Enabled = False
        txtED_DigitoVerificacion.Enabled = False
        cboED_TipoId.Enabled = False
        txtED_Nombre.Enabled = False
        cboED_TipoPersona.Enabled = False
        cboED_EstadoPersona.Enabled = False
        cboED_TipoAportante.Enabled = False
        cboTipoDeudor.Enabled = False
        txtParticipacion.Enabled = False
    End Sub

    Private Sub MostrarDeudor(ByVal pDeudor As String)
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()

        'Dim sql As String = "SELECT * FROM entes_deudores WHERE ED_Codigo_Nit = @ED_Codigo_Nit"
        Dim sql As String = "SELECT entes_deudores.*, deudores_expedientes.tipo,deudores_expedientes.participacion 				" & _
                            "	FROM entes_deudores LEFT JOIN deudores_expedientes 				" & _
                            "	ON entes_deudores.ED_Codigo_Nit = deudores_expedientes.deudor	" & _
                            "	WHERE entes_deudores.ED_Codigo_Nit = @ED_Codigo_Nit"

        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@ED_Codigo_Nit", pDeudor)
        '                 
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            cboED_TipoId.SelectedValue = Reader("ED_TipoId").ToString()

            'Colocar este dato solo si se esta editando
            If Len(Request("ID")) > 0 Then
                txtED_Codigo_Nit.Text = Reader("ED_Codigo_Nit").ToString()
            End If

            txtED_DigitoVerificacion.Text = Reader("ED_DigitoVerificacion").ToString()
            cboED_TipoPersona.SelectedValue = Reader("ED_TipoPersona").ToString()
            txtED_Nombre.Text = Reader("ED_Nombre").ToString()

            '19/06/2014. Si elestado de la persona viene NULL=>.ToString() lo transforma en vacio. Convrtir vacio en "00"
            If Reader("ED_EstadoPersona").ToString().Trim = "" Then
                cboED_EstadoPersona.SelectedValue = "00"
            Else
                cboED_EstadoPersona.SelectedValue = Reader("ED_EstadoPersona").ToString().Trim
            End If

            '11/AGO/2014 ---------------------------------------------
            txtParticipacion.Text = Reader("participacion").ToString.Trim
            If txtParticipacion.Text = "" Then
                txtParticipacion.Text = "0"
            End If

            If Reader("ED_TipoAportante").ToString() = "" Then
                cboED_TipoAportante.SelectedValue = "00"
            Else
                cboED_TipoAportante.SelectedValue = Reader("ED_TipoAportante").ToString()
            End If

            '29/jul/2014.
            'Si es deudor solidario El tipoo de aportante debe venir del deudor principal 
            If Len(Request("ID")) = 0 Then ' Esta adicionando
                If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
                    If cboTipoDeudor.SelectedValue = "2" Then
                        'xxxyyyy
                        Dim DeudorPpal As String = GetDeudorPrincipal(Request("pExpediente").Trim)
                        Dim TipoAportante As String = GetTipoAportante(DeudorPpal)
                        If TipoAportante.Trim <> "" Then
                            cboED_TipoAportante.SelectedValue = TipoAportante
                        End If
                    End If
                End If
            End If

            If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
                If Reader("tipo").ToString().Trim = "3" Or Reader("tipo").ToString().Trim = "4" Or Reader("tipo").ToString().Trim = "5" Then
                    Try
                        cboTipoDeudor.SelectedValue = "1"
                    Catch ex As Exception

                    End Try

                Else
                    Try
                        cboTipoDeudor.SelectedValue = Reader("tipo").ToString().Trim
                    Catch ex As Exception

                    End Try

                End If

            End If

            'Si es apoderado => Mostrar tarjeta profesional
            If Request("pTipo").Trim = "4" Then
                txtTarjetaProf.Text = Reader("ED_TarjetaProf").ToString().Trim
            End If
        End If
        Reader.Close()
        Connection.Close()

    End Sub

    Protected Sub LoadcboED_TipoId()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [TIPOS_IDENTIFICACION] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_TipoId.DataTextField = "nombre"
        'cboED_TipoId.DataValueField = "codigo"
        'cboED_TipoId.DataSource = Command.ExecuteReader()
        'cboED_TipoId.DataBind()
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_IDENTIFICACION] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposID As DataView = New DataView(dt1)
            vistaTiposID.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoId.DataSource = vistaTiposID
            cboED_TipoId.DataTextField = "nombre"
            cboED_TipoId.DataValueField = "codigo"
            cboED_TipoId.DataBind()
        End If
    End Sub

    Protected Sub LoadcboED_TipoPersona()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [TIPOS_PERSONA] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_TipoPersona.DataTextField = "nombre"
        'cboED_TipoPersona.DataValueField = "codigo"
        'cboED_TipoPersona.DataSource = Command.ExecuteReader()
        'cboED_TipoPersona.DataBind()
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_PERSONA] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposP As DataView = New DataView(dt1)
            vistaTiposP.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoPersona.DataSource = vistaTiposP
            cboED_TipoPersona.DataTextField = "nombre"
            cboED_TipoPersona.DataValueField = "codigo"
            cboED_TipoPersona.DataBind()
        End If
    End Sub

    Protected Sub LoadcboED_EstadoPersona()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [ESTADOS_PERSONA] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_EstadoPersona.DataTextField = "nombre"
        'cboED_EstadoPersona.DataValueField = "codigo"
        'cboED_EstadoPersona.DataSource = Command.ExecuteReader()
        'cboED_EstadoPersona.DataBind()
        ''Close the Connection Object 
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [ESTADOS_PERSONA] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaEstadosP As DataView = New DataView(dt1)
            vistaEstadosP.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_EstadoPersona.DataSource = vistaEstadosP
            cboED_EstadoPersona.DataTextField = "nombre"
            cboED_EstadoPersona.DataValueField = "codigo"
            cboED_EstadoPersona.DataBind()
        End If
    End Sub


    Protected Sub LoadcboED_TipoAportante()
        'Dim Connection As New SqlConnection(Funciones.CadenaConexion)        
        'Connection.Open()
        'Dim sql As String = "select codigo, nombre from [TIPOS_APORTANTES] order by nombre"
        'Dim Command As New SqlCommand(sql, Connection)
        'cboED_TipoAportante.DataTextField = "nombre"
        'cboED_TipoAportante.DataValueField = "codigo"
        'cboED_TipoAportante.DataSource = Command.ExecuteReader()
        'cboED_TipoAportante.DataBind()        
        'Connection.Close()
        '
        Dim cnx As String = Funciones.CadenaConexion
        Dim cmd As String = "select codigo, nombre from [TIPOS_APORTANTES] order by nombre"
        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dt1 As New DataTable
        Adaptador.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            '--------------------------------------------------------------------
            '- Ingresar el valor en blanco (el valor queda al final)
            Dim fila As DataRow = dt1.NewRow()
            fila("codigo") = "00"
            fila("nombre") = "SELECCIONE..."
            dt1.Rows.Add(fila)
            '- Crear un dataview para ordenar los valores y "00" quede de primero
            Dim vistaTiposA As DataView = New DataView(dt1)
            vistaTiposA.Sort = "codigo"
            '--------------------------------------------------------------------
            cboED_TipoAportante.DataSource = vistaTiposA
            cboED_TipoAportante.DataTextField = "nombre"
            cboED_TipoAportante.DataValueField = "codigo"
            cboED_TipoAportante.DataBind()
        End If
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        'Se debe ingresar al menos el código y el nombre del deudor/contacto
        If txtED_Codigo_Nit.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el No. de identificación por favor"
            CustomValidator1.IsValid = False
            Return
        End If
        If txtED_Nombre.Text.Trim = "" Then
            CustomValidator1.Text = "Digite el Nombre por favor"
            CustomValidator1.IsValid = False
            Return
        End If
        If cboED_TipoPersona.SelectedValue = "00" Then
            CustomValidator1.Text = "Seleccione el tipo de persona por favor"
            CustomValidator1.IsValid = False
            Return
        End If
        If cboED_EstadoPersona.SelectedValue = "00" Then
            '--------------------
            If Request("pTipo").Trim = "1" Then
                CustomValidator1.Text = "Seleccione el estado de la persona por favor"
                CustomValidator1.IsValid = False
                Return
            End If            
        End If

        If Request("pTipo") = 1 Or Request("pTipo") = 2 Then
            If cboTipoDeudor.SelectedValue = "0" Then
                CustomValidator1.Text = "Seleccione el tipo de deudor por favor"
                CustomValidator1.IsValid = False
                Return
            End If

            '29/JUL/2014. Si es un deudor, validar el tipo de aportante
            If cboTipoDeudor.SelectedValue = "1" Or cboTipoDeudor.SelectedValue = "2" Then
                If cboED_TipoAportante.SelectedValue = "00" Then
                    CustomValidator1.Text = "Seleccione el tipo de aportante por favor"
                    CustomValidator1.IsValid = False
                    Return
                End If
            End If

            '17/07/2014 pNomTipoDeudor
            If cboTipoDeudor.SelectedValue.Trim = "2" Then
                If txtParticipacion.Text.Trim = "" Or txtParticipacion.Text.Trim = "0" Then
                    CustomValidator1.Text = "Digite el porcentaje de participación por favor"
                    CustomValidator1.IsValid = False
                    Return

                ElseIf CDec(txtParticipacion.Text.Trim) > 100 Then
                    CustomValidator1.Text = "El porcentaje de participación no puede ser mayor a 100"
                    CustomValidator1.IsValid = False
                    Return

                Else
                    Dim TotalPorcentajes As Decimal = 0
                    TotalPorcentajes = SumarPorcentajesSociosSolidarios(Request("pExpediente"), txtED_Codigo_Nit.Text.Trim, CDec(txtParticipacion.Text.Trim))
                    If TotalPorcentajes > 100 Then
                        CustomValidator1.Text = "La suma de los porcentajes de los deudores solidarios supera el 100%"
                        CustomValidator1.IsValid = False
                        Return

                    End If

                End If
            End If
        End If

        'Si es apoderado, pedir tarjeta prof
        If Request("pTipo") = 4 Then
            If txtTarjetaProf.Text.Trim = "" Then
                CustomValidator1.Text = "Digite la tarjeta profesional por favor"
                CustomValidator1.IsValid = False
                Return
            End If
        End If

        'Participacion  
        If txtParticipacion.Text.Trim = "" Then
            txtParticipacion.Text = "0"
        End If


        Dim ID As String = Request("ID")
        '
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim Command As SqlCommand
        ' 
        'Comandos SQL
        Dim InsertSQL As String = "INSERT INTO ENTES_DEUDORES (ED_TipoId, ED_Codigo_Nit, ED_DigitoVerificacion, ED_TipoPersona, ED_Nombre, ED_EstadoPersona, ED_TipoAportante, ED_TarjetaProf ) VALUES ( @ED_TipoId, @ED_Codigo_Nit, @ED_DigitoVerificacion, @ED_TipoPersona, @ED_Nombre, @ED_EstadoPersona, @ED_TipoAportante, @ED_TarjetaProf) "
        Dim UpdateSQL As String = "UPDATE ENTES_DEUDORES SET ED_TipoId = @ED_TipoId, ED_DigitoVerificacion = @ED_DigitoVerificacion, ED_TipoPersona = @ED_TipoPersona, ED_Nombre = @ED_Nombre, ED_EstadoPersona = @ED_EstadoPersona, ED_TipoAportante = @ED_TipoAportante, ED_TarjetaProf = @ED_TarjetaProf WHERE ED_Codigo_Nit = @ED_Codigo_Nit "
        '
        If String.IsNullOrEmpty(ID) Then
            ' insert
            If ExisteEnte(txtED_Codigo_Nit.Text.Trim) Then
                'existe=> update
                Command = New SqlCommand(UpdateSQL, Connection)
                Command.Parameters.AddWithValue("@ED_Codigo_Nit", txtED_Codigo_Nit.Text.Trim)
            Else
                'insert
                Command = New SqlCommand(InsertSQL, Connection)
                Command.Parameters.AddWithValue("@ED_Codigo_Nit", txtED_Codigo_Nit.Text.Trim)
            End If

            ID = txtED_Codigo_Nit.Text.Trim
        Else
            ' update
            Command = New SqlCommand(UpdateSQL, Connection)
            Command.Parameters.AddWithValue("@ED_Codigo_Nit", ID)
        End If

        If cboED_TipoId.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@ED_TipoId", cboED_TipoId.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@ED_TipoId", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@ED_DigitoVerificacion", txtED_DigitoVerificacion.Text.Trim)

        If cboED_TipoPersona.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@ED_TipoPersona", cboED_TipoPersona.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@ED_TipoPersona", DBNull.Value)
        End If

        Command.Parameters.AddWithValue("@ED_Nombre", txtED_Nombre.Text)

        If cboED_EstadoPersona.SelectedValue.Length > 0 Then
            Command.Parameters.AddWithValue("@ED_EstadoPersona", cboED_EstadoPersona.SelectedValue)
        Else
            Command.Parameters.AddWithValue("@ED_EstadoPersona", DBNull.Value)
        End If


        If cboED_TipoAportante.SelectedValue.Length > 0 Then
            If cboED_TipoAportante.SelectedValue.Trim = "00" Then
                Command.Parameters.AddWithValue("@ED_TipoAportante", DBNull.Value)
            Else
                Command.Parameters.AddWithValue("@ED_TipoAportante", cboED_TipoAportante.SelectedValue)
            End If
        Else
            Command.Parameters.AddWithValue("@ED_TipoAportante", DBNull.Value)
        End If

        '29/07/2014. Validacion del tipo de aportante
        'Si es un deudor solidario, el campo tipo de aportante debe venir del deudor principal
        If Request("pTipo").Trim = "1" Or Request("pTipo").Trim = "2" Then
            If cboTipoDeudor.SelectedValue = "2" Then
                'xxxyyyy
                Dim DeudorPpal As String = GetDeudorPrincipal(Request("pExpediente").Trim)
                Dim TipoAportante As String = GetTipoAportante(DeudorPpal)
                If TipoAportante.Trim <> "" Then
                    cboED_TipoAportante.SelectedValue = TipoAportante
                End If
            End If
        End If        

        Command.Parameters.AddWithValue("@ED_TarjetaProf", txtTarjetaProf.Text.Trim)

        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Módulo de deudores", "Deudor " & ID, Command)
        Catch ex As Exception
            CustomValidator1.Text = ex.Message
            CustomValidator1.IsValid = False
            Return
        End Try


        Connection.Close()

        'Registrar en la tabla de movimiento con el tipo de ente 
        Dim lcTipo As Integer = 0
        If Request("pTipo") = 1 Then
            ' Es deudor y el combo define si es principal o solidario
            lcTipo = cboTipoDeudor.SelectedValue

        ElseIf Request("pTipo").Trim = "3" Then
            lcTipo = 3

        ElseIf Request("pTipo").Trim = "4" Then
            lcTipo = 4

        Else
            lcTipo = 5
        End If
        RegistrarDEUDORES_EXPEDIENTES(ID, Request("pExpediente"), lcTipo, txtParticipacion.Text.Trim)

        'Ir a pagina de resumen
        Response.Redirect("ENTES_DEUDORES.aspx?pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
    End Sub

    Private Function SumarPorcentajesSociosSolidarios(ByVal pExpediente As String, ByVal pDeudor As String, ByVal pPorcentaje As Decimal) As Decimal
        Dim Sw As Integer = 0
        Dim TotalPorcentaje As Decimal = 0

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT * FROM DEUDORES_EXPEDIENTES WHERE NroExp = '" & pExpediente & "' AND tipo = 2"

        Dim Command As New SqlCommand(sql, Connection)
        Dim Reader As SqlDataReader = Command.ExecuteReader
        While Reader.Read
            If Reader("deudor").ToString().Trim = pDeudor Then
                TotalPorcentaje = TotalPorcentaje + pPorcentaje
                Sw = 1
            Else
                TotalPorcentaje = TotalPorcentaje + CDec(Reader("participacion").ToString().Trim)
            End If
        End While
        Reader.Close()
        Connection.Close()

        If Sw = 0 Then
            TotalPorcentaje = TotalPorcentaje + pPorcentaje
        End If

        Return TotalPorcentaje

    End Function


    Private Sub RegistrarDEUDORES_EXPEDIENTES(ByVal pDeudor As String, ByVal pExpediente As String, ByVal pTipo As Integer, ByVal pParticipacion As Decimal)

        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = ""
        Dim Existe As Integer = 0
        Existe = ExisteDEUDORES_EXPEDIENTES(pDeudor, pExpediente)
        If Existe > 0 Then
            'Update 
            If Existe = 1 Then
                sql = "UPDATE DEUDORES_EXPEDIENTES SET tipo = @tipo, participacion = @participacion WHERE deudor = @deudor AND NroExp = @NroExp "
            Else
                sql = "UPDATE DEUDORES_EXPEDIENTES SET participacion = @participacion WHERE deudor = @deudor AND NroExp = @NroExp AND tipo = @tipo"
            End If

        Else
            'Insertar registro
            sql = "INSERT INTO DEUDORES_EXPEDIENTES (deudor, NroExp, tipo,participacion) VALUES (@deudor, @NroExp, @tipo,@participacion)"
        End If
        'Comando
        Dim Command As SqlCommand = New SqlCommand(sql, Connection)
        'Parametros
        Command.Parameters.AddWithValue("@deudor", pDeudor)
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        Command.Parameters.AddWithValue("@tipo", pTipo)
        Command.Parameters.AddWithValue("@participacion", pParticipacion)

        '
        Try
            Command.ExecuteNonQuery()
            'Después de cada GRABAR hay que llamar al log de auditoria
            Dim LogProc As New LogProcesos
            LogProc.SaveLog(Session("ssloginusuario"), "Asociación entre deudores y expedientes", "Deudor " & pDeudor, Command)
        Catch ex As Exception

        End Try

        Connection.Close()

        'Por comodidad.. actualizar EJEFISGLOBAL.EFINIT
        'Solo hacer UPDATE el registro en EJEFISGLOBAL debe existir
        If pTipo = 1 Then 'DEUDOR PRINCIPAL
            Connection.Open()
            sql = "UPDATE EJEFISGLOBAL SET EFINIT = '" & pDeudor & "' WHERE EFINROEXP = '" & pExpediente & "'"
            Dim Command2 As SqlCommand = New SqlCommand(sql, Connection)

            Try
                Command2.ExecuteNonQuery()
                'Después de cada GRABAR hay que llamar al log de auditoria
                Dim LogProc As New LogProcesos
                LogProc.SaveLog(Session("ssloginusuario"), "Deudor principal en expediente", "Deudor " & pDeudor, Command2)
            Catch ex As Exception

            End Try

            Connection.Close()
        End If

    End Sub

    Private Function ExisteEnte(ByVal pDeudor As String) As Boolean
        Dim Respuesta As Boolean = False
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT ED_Codigo_Nit FROM ENTES_DEUDORES WHERE ED_Codigo_Nit = @ED_Codigo_Nit"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@ED_Codigo_Nit", pDeudor)
        '
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        If Reader.Read Then
            Respuesta = True
        End If
        Return Respuesta

    End Function

    Private Function ExisteDEUDORES_EXPEDIENTES(ByVal pDeudor As String, ByVal pExpediente As String) As Integer
        Dim Respuesta As Integer = 0
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "SELECT deudor, NroExp FROM DEUDORES_EXPEDIENTES WHERE deudor = @deudor AND NroExp = @NroExp"
        Dim Command As New SqlCommand(sql, Connection)
        Command.Parameters.AddWithValue("@deudor", pDeudor)
        Command.Parameters.AddWithValue("@NroExp", pExpediente)
        '                 
        Dim Reader As SqlDataReader = Command.ExecuteReader
        'If at least one record was found
        While Reader.Read
            'Respuesta = True
            Respuesta = Respuesta + 1
        End While
        Return Respuesta

    End Function

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("ENTES_DEUDORES.aspx?pExpediente=" & Request("pExpediente") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
    End Sub

    Private Shared Function GetCiudades() As DataTable
        Dim cmd, cnx As String
        cnx = Funciones.CadenaConexion
        cmd = "SELECT codigo, nombre, departamento FROM municipios"

        Dim Adaptador As New SqlDataAdapter(cmd, cnx)
        Dim dtMunicipios As New DataTable
        Adaptador.Fill(dtMunicipios)
        Return dtMunicipios
    End Function

    Protected Sub txtED_Codigo_Nit_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtED_Codigo_Nit.TextChanged
        'MostrarDeudor
        If Len(txtED_Codigo_Nit.Text.Trim) > 0 Then
            MostrarDeudor(txtED_Codigo_Nit.Text.Trim)
        End If
    End Sub

    Protected Sub cmdBorrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBorrar.Click
        Response.Redirect("BorrarDEUDORES_EXPEDIENTES.aspx?pExpediente=" & Request("pExpediente") & "&pDeudor=" & Request("ID") & "&pScr=" & Request("pScr") & "&pTipo=" & Request("pTipo") & "&pNombre=" & txtED_Nombre.Text.Trim)
    End Sub

End Class
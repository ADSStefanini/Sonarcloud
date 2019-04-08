Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class EditDIRECCIONES
    Inherits PaginaBase
    ''' <summary>
    ''' Se declaran las propiedades necesarias 
    ''' </summary>
    Dim tareaAsignadaObject As TareaAsignada
    Dim fuenteDireccionBLL As FuenteDireccionBLL
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL

    Protected Overrides Sub OnInit(e As EventArgs)
        fuenteDireccionBLL = New FuenteDireccionBLL()
        tareaAsignadaBLL = New TareaAsignadaBLL()
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL()
    End Sub
    ''' <summary>
    ''' Carga los controles de la vista segun las propiedades del objeto
    ''' </summary>
    ''' <param name="prmdireccionUbicacion">Objeto de tipo direccion</param>
    Public Sub cargarObjetoDirreccion(prmdireccionUbicacion As DireccionUbicacion)
        If Session("mnivelacces") = 10 And prmdireccionUbicacion.idunico > 0 Then
            cmdSave.Visible = False
            cmdSave.Enabled = False
        End If
        If String.IsNullOrEmpty(prmdireccionUbicacion.direccionCompleta) = False Then
            txtDireccion.Text = prmdireccionUbicacion.direccionCompleta
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.idDepartamento) = False Then
            cboDepartamento.SelectedValue = prmdireccionUbicacion.idDepartamento
            LoadcboCiudad(prmdireccionUbicacion.idDepartamento)
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.idCiudad) = False Then
            cboCiudad.SelectedValue = prmdireccionUbicacion.idCiudad
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.telefono) = False Then
            txtTelefono.Text = prmdireccionUbicacion.telefono
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.email) = False Then
            txtEmail.Text = prmdireccionUbicacion.email
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.celular) = False Then
            txtMovil.Text = prmdireccionUbicacion.celular
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.paginaweb) = False Then
            txtpaginaweb.Text = prmdireccionUbicacion.paginaweb
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.fuentesDirecciones) = False Then
            CboFuente.SelectedValue = prmdireccionUbicacion.fuentesDirecciones
        End If

        If String.IsNullOrEmpty(prmdireccionUbicacion.otrasFuentesDirecciones) = False Then
            TxtOtraFuente.Text = prmdireccionUbicacion.otrasFuentesDirecciones
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then

            LoadcboDepartamento()
            LoadcboFuente()
            If Len(Request("ID_TASK")) > 0 Then
                'Si ID_TASK tiene valor se carga valida el item
                tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(Request("ID_TASK").ToString()))
                Dim tituloEjecutivoObj As TituloEjecutivoExt

                'Si TareaAsignadaObject.ID_UNICO_TITULO esta vacio realiza el cargue del objeto parcial
                Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(tareaAsignadaObject.ID_TAREA_ASIGNADA)
                tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
                HdnIdTask.Value = Request("ID_TASK")

                If String.IsNullOrEmpty(Request("IdDireccion")) = False Then
                    cargarObjetoDirreccion(tituloEjecutivoObj.LstDireccionUbicacion.Where(Function(x) (x.idunico).ToString() = Request("IdDireccion")).FirstOrDefault())
                Else
                    LoadcboCiudad(cboDepartamento.SelectedValue)
                End If

                If Session("mnivelacces") <> 11 Then 'Solo área origen puede editar la dirección 
                    pnlDirecciones.Enabled = False
                End If
            Else
                LoadcboCiudad(cboDepartamento.SelectedValue)
            End If

            If Session("mnivelacces") = 10 Then
                nombreModulo = "ESTUDIO_TITULOS"
            End If

            If Session("mnivelacces") = 11 Then
                nombreModulo = "AREA_ORIGEN"
            End If
#Region "EstudioTitulos"
            'TODO: VALIDAR COMPORTAMIENTO EN ESTUDIO DE TITULOS
            ''Create a new connection to the database
            'Dim Connection As New SqlConnection(Funciones.CadenaConexion)

            ''Opens a connection to the database.
            'Connection.Open()
            ''Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos                       
            'If NomEstadoProceso = "DEVUELTO" Or NomEstadoProceso = "TERMINADO" Then
            '    cmdSave.Visible = False
            '    CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten editar datos"
            '    CustomValidator1.IsValid = False

            '    'Deshabilitar controles
            '    txtDireccion.Enabled = False
            '    cboDepartamento.Enabled = False
            '    cboCiudad.Enabled = False
            '    txtTelefono.Enabled = False
            '    txtEmail.Enabled = False
            '    txtMovil.Enabled = False
            '    txtpaginaweb.Enabled = False
            'End If
#End Region
        End If
    End Sub

    Private Sub LoadcboFuente()
        CboFuente.DataSource = fuenteDireccionBLL.ConsultarFuentes()
        CboFuente.DataTextField = "DES_NOMBRE_FUENTE_DIRECCION"
        CboFuente.DataValueField = "ID_FUENTE_DIRECCION"
        CboFuente.DataBind()
    End Sub

    Protected Sub LoadcboDepartamento()
        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [DEPARTAMENTOS] order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboDepartamento.DataTextField = "nombre"
        cboDepartamento.DataValueField = "codigo"
        cboDepartamento.DataSource = Command.ExecuteReader()
        cboDepartamento.DataBind()
        'Close the Connection Object 
        Connection.Close()
    End Sub

    Protected Sub LoadcboCiudad(ByVal prmStrCodigoDepartamento As String)

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = "select codigo, nombre from [MUNICIPIOS] WHERE departamento = '" & prmStrCodigoDepartamento & "' order by nombre"
        Dim Command As New SqlCommand(sql, Connection)
        cboCiudad.DataTextField = "nombre"
        cboCiudad.DataValueField = "codigo"
        cboCiudad.DataSource = Command.ExecuteReader()
        cboCiudad.DataBind()

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Overloads Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Request("ID_TASK"))
        Dim tituloEjecutivoObj As TituloEjecutivoExt = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
        Dim direccionUbicacionItem As DireccionUbicacion = New DireccionUbicacion()

        If String.IsNullOrEmpty(txtDireccion.Text) = False Then
            direccionUbicacionItem.direccionCompleta = txtDireccion.Text
        End If

        If String.IsNullOrEmpty(cboDepartamento.SelectedValue) = False Then
            direccionUbicacionItem.idDepartamento = cboDepartamento.SelectedValue
            direccionUbicacionItem.NombreDepartamento = cboDepartamento.SelectedItem.Text
        End If

        If String.IsNullOrEmpty(cboCiudad.SelectedValue) = False Then
            direccionUbicacionItem.idCiudad = cboCiudad.SelectedValue
            direccionUbicacionItem.NombreMunicipio = cboCiudad.SelectedItem.Text

        End If

        If String.IsNullOrEmpty(txtTelefono.Text) = False Then
            direccionUbicacionItem.telefono = txtTelefono.Text
        End If

        If String.IsNullOrEmpty(txtEmail.Text) = False Then
            direccionUbicacionItem.email = txtEmail.Text
        End If

        If String.IsNullOrEmpty(txtMovil.Text) = False Then
            direccionUbicacionItem.celular = txtMovil.Text
        End If

        If String.IsNullOrEmpty(txtpaginaweb.Text) = False Then
            direccionUbicacionItem.paginaweb = txtpaginaweb.Text
        End If

        If String.IsNullOrEmpty(CboFuente.SelectedValue) = False Then
            direccionUbicacionItem.fuentesDirecciones = CboFuente.SelectedValue
            direccionUbicacionItem.nombreFuente = CboFuente.SelectedItem.Text
        End If

        If String.IsNullOrEmpty(TxtOtraFuente.Text) = False Then
            direccionUbicacionItem.otrasFuentesDirecciones = TxtOtraFuente.Text
        End If
        direccionUbicacionItem.numeroidentificacionDeudor = Request("IdDeudor").ToString()
        If String.IsNullOrEmpty(Request("IdDireccion")) = False Then
            direccionUbicacionItem.idunico = Int32.Parse(Request("IdDireccion").ToString())
            tituloEjecutivoObj.LstDireccionUbicacion.Remove(tituloEjecutivoObj.LstDireccionUbicacion.Where(Function(x) (x.idunico).ToString() = Request("IdDireccion")).FirstOrDefault())
        Else
            direccionUbicacionItem.idunico = -(tituloEjecutivoObj.LstDireccionUbicacion().Count() + 1)
        End If
        tituloEjecutivoObj.LstDireccionUbicacion.Add(direccionUbicacionItem)
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(tituloEjecutivoObj)
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("jsonobj=" + almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
        'Go to the Summary page 
        Response.Redirect("DIRECCIONES.aspx?ID_TASK=" & Request("ID_TASK") & "&IdDeudor=" & Request("IdDeudor") & "&IdDireccion=" & Request("IdDireccion"))
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Response.Redirect("DIRECCIONES.aspx?ID_TASK=" & Request("ID_TASK") & "&IdDeudor=" & Request("IdDeudor") & "&IdDireccion=" & Request("IdDireccion"))
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

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "Seguridad"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function

    Protected Sub cboDepartamento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDepartamento.SelectedIndexChanged
        LoadcboCiudad(cboDepartamento.SelectedValue)
    End Sub
End Class

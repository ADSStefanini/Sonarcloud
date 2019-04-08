Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class DIRECCIONES
    Inherits PaginaBase
    ''' <summary>
    ''' Se declaran las variables 
    ''' </summary>
    Dim tareaAsignadaObject As TareaAsignada
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Dim almacenamientoTemporalBLL As AlmacenamientoTemporalBLL
    ''' <summary>
    ''' se inicializan
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnInit(e As EventArgs)
        tareaAsignadaBLL = New TareaAsignadaBLL()
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL()
    End Sub

    ''' <summary>
    ''' Load de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Evaluates to true when the page is loaded for the first time.

        If Session("mnivelacces") = 10 Then
            nombreModulo = "ESTUDIO_TITULOS"
        End If

        If Session("mnivelacces") = 11 Then
            nombreModulo = "AREA_ORIGEN"
        End If

        If Len(Request("ID_TASK")) > 0 And Request("ID_TASK").ToString() <> "0" Then
            HdnIdTask.Value = Request("ID_TASK").ToString()
            'Si ID_TASK tiene valor se carga valida el item

            Dim tituloEjecutivoObj As TituloEjecutivoExt
            Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(HdnIdTask.Value))
            tituloEjecutivoObj = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
            If tituloEjecutivoObj IsNot Nothing Then
                Dim lstDireccionesDeudor As List(Of DireccionUbicacion) = tituloEjecutivoObj.LstDireccionUbicacion.Where(Function(x) (x.numeroidentificacionDeudor) = Request("IdDeudor")).ToList()
                HdnLstDirecciones.Value = JsonConvert.SerializeObject(lstDireccionesDeudor)
                BindGrid()
            End If
        End If

    End Sub

    Private Sub BindGrid()
        If String.IsNullOrEmpty(HdnLstDirecciones.Value) = False Then
            Dim LstDataDirecciones As List(Of DireccionUbicacion) = JsonConvert.DeserializeObject(Of List(Of DireccionUbicacion))(HdnLstDirecciones.Value)
            LstDataDirecciones = LstDataDirecciones.Where(Function(x) (x.numeroidentificacionDeudor).ToString() = Request("IdDeudor")).ToList()
            grd.DataSource = LstDataDirecciones
            grd.DataBind()
            lblRecordsFound.Text = "Direcciones encontradas " & grd.Rows.Count
        End If
    End Sub

    'Adicionar
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim LstDataDirecciones As List(Of DIRECCIONES) = JsonConvert.DeserializeObject(Of List(Of DIRECCIONES))(HdnLstDirecciones.Value)
        'Ir a la pagina de edicion
        If Len(Request("IdDeudor")) > 0 Then
            Response.Redirect("EditDIRECCIONES.aspx?ID_TASK=" & HdnIdTask.Value & "&IdDeudor=" & Request("IdDeudor"))
        Else
            CustomValidator1.IsValid = False
            CustomValidator1.Text = "Debe existir el deudor"
        End If

    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim deudor As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim Iddireccion As String = grd.Rows(e.CommandArgument).Cells(10).Text
            Response.Redirect("EditDIRECCIONES.aspx?ID_TASK=" & HdnIdTask.Value & "&IdDeudor=" & deudor & "&IdDireccion=" & Iddireccion)
        End If
    End Sub

    Private Function llenarAuditoria(ByVal valorAfectado As String) As LogAuditoria
        Dim log As New LogProcesos
        Dim auditData As New UGPP.CobrosCoactivo.Entidades.LogAuditoria
        auditData.LOG_APLICACION = log.AplicationName
        auditData.LOG_FECHA = Date.Now
        auditData.LOG_HOST = log.ClientHostName
        auditData.LOG_IP = log.ClientIpAddress
        auditData.LOG_MODULO = "TAREA ASIGNADA"
        auditData.LOG_USER_CC = String.Empty
        auditData.LOG_USER_ID = Session("ssloginusuario")
        auditData.LOG_DOC_AFEC = valorAfectado
        Return auditData
    End Function
    Public Sub crearTarea()
        'Si ID_TASKno tiene valor se crea una tarea
        tareaAsignadaObject = New TareaAsignada()
        tareaAsignadaObject.COD_TIPO_OBJ = 4 ' TITULO
        tareaAsignadaObject.VAL_USUARIO_NOMBRE = Session("ssloginusuario")
        tareaAsignadaObject.COD_ESTADO_OPERATIVO = 1
        tareaAsignadaBLL = New TareaAsignadaBLL(llenarAuditoria("codtipo=" + tareaAsignadaObject.COD_TIPO_OBJ.ToString() + ",usunombre=" & tareaAsignadaObject.VAL_USUARIO_NOMBRE + ",codestadoope=" & tareaAsignadaObject.COD_ESTADO_OPERATIVO.ToString()))
        tareaAsignadaObject = tareaAsignadaBLL.registrarTarea(tareaAsignadaObject)
        Dim almacenamientoTemportalItem As AlmacenamientoTemporal = New AlmacenamientoTemporal()
        almacenamientoTemportalItem.ID_TAREA_ASIGNADA = tareaAsignadaObject.ID_TAREA_ASIGNADA
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(New TituloEjecutivoExt())
        almacenamientoTemporalBLL = New AlmacenamientoTemporalBLL(llenarAuditoria("idtareaasig=" + almacenamientoTemportalItem.ID_TAREA_ASIGNADA.ToString() + ",jsonobj=" & almacenamientoTemportalItem.JSON_OBJ))
        almacenamientoTemporalBLL.InsertarAlmacenamiento(almacenamientoTemportalItem)
        HdnIdTask.Value = tareaAsignadaObject.ID_TAREA_ASIGNADA

    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        BindGrid()
    End Sub
End Class
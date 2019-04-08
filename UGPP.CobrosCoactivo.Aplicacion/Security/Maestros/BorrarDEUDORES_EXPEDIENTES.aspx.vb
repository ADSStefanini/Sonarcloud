Imports System.Data
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Partial Public Class BorrarDEUDORES_EXPEDIENTES
    Inherits System.Web.UI.Page
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Label1.Text = "Realmente desea eliminar el vinculo con el deudor a " & Request("pNombre") & "?"
        End If
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        Dim almacenamientoTemportalItem = almacenamientoTemporalBLL.consultarAlmacenamientoPorId(Long.Parse(Request("ID_TASK").ToString()))
        Dim tituloEjecutivoObj As TituloEjecutivoExt = JsonConvert.DeserializeObject(Of TituloEjecutivoExt)(almacenamientoTemportalItem.JSON_OBJ)
        tituloEjecutivoObj.LstDireccionUbicacion.Remove(tituloEjecutivoObj.LstDireccionUbicacion.Where(Function(x) (x.numeroidentificacionDeudor).ToString() = Request("IdDeudor")).FirstOrDefault())
        tituloEjecutivoObj.LstDeudores.Remove(tituloEjecutivoObj.LstDeudores.Where(Function(x) (x.numeroIdentificacion) = Request("IdDeudor")).FirstOrDefault())
        almacenamientoTemportalItem.JSON_OBJ = JsonConvert.SerializeObject(tituloEjecutivoObj)
        almacenamientoTemporalBLL.actualizarAlmacenamiento(almacenamientoTemportalItem)
        'TODO:ESTUDIO ELIMINAR EL DEUDOR DE DEUDORES_EXPEDIENTES
        Response.Redirect("ENTES_DEUDORES.aspx?ID_TASK=" & Request("ID_TASK") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))
    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Response.Redirect("ENTES_DEUDORES.aspx?ID_TASK=" & Request("ID_TASK") & "&pTipo=" & Request("pTipo") & "&pScr=" & Request("pScr"))

    End Sub
End Class
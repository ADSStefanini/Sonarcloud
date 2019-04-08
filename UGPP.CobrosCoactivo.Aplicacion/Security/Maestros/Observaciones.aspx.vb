Imports UGPP.CobrosCoactivo.Entidades
Imports UGPP.CobrosCoactivo.Logica

Public Class Observaciones
    Inherits PaginaBase
    Dim tareaAsignadaBLL As TareaAsignadaBLL
    Protected Overrides Sub OnInit(e As EventArgs)
        tareaAsignadaBLL = New TareaAsignadaBLL()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Len(Request("ID_TASK")) > 0 Then
            Dim tareaAsignadaObject = tareaAsignadaBLL.consultarTareaPorId(Long.Parse(Request("ID_TASK").ToString()))
            If tareaAsignadaObject.ID_UNICO_TITULO.HasValue And tareaAsignadaObject.ID_UNICO_TITULO <> 0 Then
                HdnIdUnico.Value = tareaAsignadaObject.ID_UNICO_TITULO
                PrintarGridHistoricoCNC()
            End If
        End If

    End Sub

    ''' <summary>
    ''' Llena la grilla de observaciones generales de Cumple no cumple
    ''' </summary>
    Protected Sub PrintarGridHistoricoCNC()
        If Not String.IsNullOrEmpty(HdnIdUnico.Value) Then
            Dim ObservaCNCGral As ObservacionesCNCGralBLL = New ObservacionesCNCGralBLL()
            Dim ObservaCNCGralList As List(Of ObservacionesCNC) = ObservaCNCGral.obtenerObservacionesCNCGral(Int64.Parse(HdnIdUnico.Value))
            If ObservaCNCGralList.Count() > 0 Then
                grdHistorico.DataSource = ObservaCNCGralList
                grdHistorico.DataBind()
                PaginadorGridView.UpdateLabels()
            End If
        End If
    End Sub

    Protected Sub PaginadorGridView_EventActualizarGrid()
        If String.IsNullOrEmpty(HdnIdUnico.Value) = False Then
            PrintarGridHistoricoCNC()
        End If

    End Sub
End Class
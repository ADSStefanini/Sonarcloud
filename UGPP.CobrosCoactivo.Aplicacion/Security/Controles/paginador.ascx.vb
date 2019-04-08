Public Class paginador
    Inherits System.Web.UI.UserControl
    ' Nombre del gridview control padre (IDCLIENT)
    Property gridViewIdClient As String
    ' Evento para generar al actualizacion de datos
    Public Event EventActualizarGrid(ByVal sender As Object, ByVal e As EventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Not IsNothing(gridViewIdClient) Then
                UpdateLabels()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Actuliza el label que informa la pagina actual
    ''' </summary>
    Public Sub UpdateLabels()
        lblPageNumber.Text = "Página " & (RetornarGridViewDePadre().PageIndex + 1) & " de " & RetornarGridViewDePadre().PageCount
    End Sub
    ''' <summary>
    ''' Navega a la primiera pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdFirst_Click(sender As Object, e As EventArgs) Handles cmdFirst.Click
        RetornarGridViewDePadre().PageIndex = 0
        ActualizarVista()
    End Sub
    ''' <summary>
    ''' Busca el control en la pagina padre que realiza el llamado
    ''' </summary>
    ''' <returns></returns>
    Public Function RetornarGridViewDePadre() As GridView
        Return CType(Me.Parent.FindControl(gridViewIdClient), GridView)
    End Function
    ''' <summary>
    ''' Convoca el evento en la pagina padre para actualizar el datasource del gridview
    ''' </summary>
    Public Sub ActualizarVista()
        UpdateLabels()
        RaiseEvent EventActualizarGrid(Nothing, Nothing)
    End Sub
    ''' <summary>
    ''' Cambia el index page a la pagina anterior
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdPrevious_Click(sender As Object, e As EventArgs) Handles cmdPrevious.Click
        Dim i As Int32 = RetornarGridViewDePadre().PageCount
        If (RetornarGridViewDePadre().PageIndex > 0) Then
            RetornarGridViewDePadre().PageIndex = RetornarGridViewDePadre().PageIndex - 1
        End If
        ActualizarVista()
    End Sub
    ''' <summary>
    ''' Avanza el index page a la siguiente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdNext_Click(sender As Object, e As EventArgs) Handles cmdNext.Click
        Dim i As Int32 = RetornarGridViewDePadre().PageIndex + 1
        If (i < RetornarGridViewDePadre().PageCount) Then
            RetornarGridViewDePadre().PageIndex = i
        End If
        ActualizarVista()
    End Sub
    ''' <summary>
    ''' Navega a la ultima pagina 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub cmdLast_Click(sender As Object, e As EventArgs) Handles cmdLast.Click
        RetornarGridViewDePadre().PageIndex = RetornarGridViewDePadre().PageCount - 1
        ActualizarVista()
    End Sub
End Class
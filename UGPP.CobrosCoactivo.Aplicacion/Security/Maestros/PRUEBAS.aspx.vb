Imports System.Data.SqlClient
Partial Public Class PRUEBAS1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            BindGrid()

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdAddNew.Visible = False
                'CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                'CustomValidator1.IsValid = False
            End If

            If Len(Request("pRadicado")) > 0 Then
                'bOTON ADICIONAR VISIBLE
            Else
                cmdAddNew.Visible = False
            End If
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "DECRETA PRUEBAS. Registros encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim pRadicado As String = ""
        If Len(Request("pRadicado")) > 0 Then
            pRadicado = Request("pRadicado")
        End If

        Dim sql As String = "SELECT * FROM pruebas WHERE NroRad = '" & pRadicado & "'"
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Response.Redirect("EditPRUEBAS.aspx?pRadicado=" & Request("pRadicado") & "&pExpediente=" & Request("pExpediente"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim NroAutoPru As String = grd.Rows(e.CommandArgument).Cells(2).Text
            Response.Redirect("EditPRUEBAS.aspx?ID=" & NroAutoPru & "&pRadicado=" & Request("pRadicado") & "&pExpediente=" & Request("pExpediente"))
        End If
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub

End Class
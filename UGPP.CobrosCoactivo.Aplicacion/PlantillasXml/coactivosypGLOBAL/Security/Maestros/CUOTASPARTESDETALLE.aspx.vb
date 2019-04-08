Imports System.Data.SqlClient

Partial Public Class CUOTASPARTESDETALLE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            BindGrid()
        End If

    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()

        'Create a new connection to the database
        Dim Connection As New SqlConnection(CadenaConexion())

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Registros encontrados " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim pExpediente As String = ""
        pExpediente = Request("pExpediente")
        '        
        Dim sql As String = ""
        sql = "select * FROM CUOTASPARTESDETALLE WHERE NroExp = '" & pExpediente & "'"
        Return sql

    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditCUOTASPARTESDETALLE.aspx
        Response.Redirect("EditCUOTASPARTESDETALLE.aspx?pExpediente=" & Request("pExpediente").ToString.Trim)
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim Pensionado As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditCUOTASPARTESDETALLE.aspx?pPensionado=" & Pensionado & "&pExpediente=" & Request("pExpediente").ToString.Trim)
        End If

    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("EditCUOTASPARTES.aspx?pExpediente=" & Request("pExpediente").ToString.Trim)
    End Sub

End Class
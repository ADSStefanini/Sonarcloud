Imports System.Data.SqlClient
Partial Public Class EXCEPCIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            'Habilitar el boton de crear la excepcion SOLO si esta en coactivo
            If NomEstadoProceso = "COACTIVO" Then
                cmdAddNew.Enabled = True
            Else
                cmdAddNew.Enabled = False
                CustomValidator1.Text = "Solo se pueden adicionar excepciones en estado COACTIVO"
                CustomValidator1.IsValid = False
            End If

            BindGrid()

            'Si el expediente esta en estado devuelto o terminado =>Impedir adicionar o editar datos 
            'Obtener estado del expediente
            Dim MTG As New MetodosGlobalesCobro
            Dim IdEstadoExp As String
            IdEstadoExp = MTG.GetEstadoExpediente(Request("pExpediente"))
            If IdEstadoExp = "04" Or IdEstadoExp = "07" Then
                '04=DEVUELTO, 07=TERMINADO
                cmdAddNew.Visible = False
                CustomValidator1.Text = "Los expedientes en estado " & NomEstadoProceso & " no permiten adicionar datos"
                CustomValidator1.IsValid = False
            End If
        End If
    End Sub

    'Display's the grid with the search criteria.
    Private Sub BindGrid()
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Excepciones encontradas " & grd.Rows.Count
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim pExpediente As String
        pExpediente = Request("pExpediente")
        Dim sql As String = "SELECT excepciones.*, EJEFISGLOBAL.EFIESTADO FROM excepciones LEFT JOIN EJEFISGLOBAL ON excepciones.NroExp = EJEFISGLOBAL.EFINROEXP WHERE excepciones.NroExp = '" & pExpediente.Trim & "'"
        Return sql
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim pExpediente As String
        pExpediente = Request("pExpediente")
        Response.Redirect("EditEXCEPCIONES.aspx?pExpediente=" & pExpediente.Trim)
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim NroRad As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Dim EstadoExpediente As String = grd.Rows(e.CommandArgument).Cells(1).Text
            Dim pExpediente As String
            pExpediente = Request("pExpediente")
            Response.Redirect("EditEXCEPCIONES.aspx?ID=" & NroRad & "&pExpediente=" & pExpediente & "&pEstadoExpediente=" & EstadoExpediente)
        End If
    End Sub

    Protected Sub grd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd.RowDataBound
    End Sub


End Class
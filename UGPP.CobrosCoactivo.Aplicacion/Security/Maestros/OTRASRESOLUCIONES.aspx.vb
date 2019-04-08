Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class OTRASRESOLUCIONES
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Evaluates to true when the page is loaded for the first time.
        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilita el botón Adicionar para este perfil
                cmdAddNew.Enabled = False
            Else
                'Se habilita el botón Adicionar para usuarios con perfil diferente a Verificador de Pagos
                cmdAddNew.Enabled = True
            End If

            BindGrid()
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
        lblRecordsFound.Text = "Resoluciones encontradas: " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()
    End Sub

    Private Function GetSQL() As String
        Dim NroExp As String = ""
        NroExp = Request("pExpediente")
        Dim sql As String = ""
        sql = sql & "SELECT OtrasResoluciones.*, FORMAS_NOTIFICACIONFormaNotif.nombre as FORMAS_NOTIFICACIONFormaNotifnombre " & _
                    "FROM OtrasResoluciones LEFT JOIN [FORMAS_NOTIFICACION] FORMAS_NOTIFICACIONFormaNotif ON OtrasResoluciones.FormaNotif = FORMAS_NOTIFICACIONFormaNotif.codigo " & _
                    "WHERE OtrasResoluciones.NroExp = '" & NroExp.Trim & "'"
        Return sql
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim NroExp As String = ""
        NroExp = Request("pExpediente")

        Response.Redirect("EditOTRASRESOLUCIONES.aspx?pExpediente=" & NroExp.Trim)
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        'Se inhabilita la opción de editar para usuarios con perfil Verificador de Pagos
        If Session("mnivelacces") <> CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
            If e.CommandName = "" Then
                Dim IdUnico As String = grd.Rows(e.CommandArgument).Cells(0).Text
                Response.Redirect("EditOTRASRESOLUCIONES.aspx?ID=" & IdUnico)
            End If
        End If
    End Sub

End Class
Imports System.Data.SqlClient
Imports UGPP.CobrosCoactivo.Entidades

Partial Public Class NOTAS
    Inherits System.Web.UI.Page

    Private PageSize As Long = 5

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Session("mnivelacces") = CInt(Enumeraciones.Roles.VERIFICADORPAGOS) Then
                'Se inhabilita el botón Adicionar observación / nota para este perfil
                cmdAddNew.Enabled = False
            Else
                'Se habilita el botón Adicionar observación / nota para usuarios con perfil diferente a Verificador de Pagos
                cmdAddNew.Enabled = True
            End If

            BindGrid()
        End If

    End Sub

    Private Sub BindGrid()

        'Create a new connection to the database
        'Dim Connection as new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        Dim Connection As New SqlConnection(Funciones.CadenaConexion)

        'Opens a connection to the database.
        Connection.Open()
        Dim sql As String = GetSQL()
        Dim Command As New SqlCommand()
        Command.Connection = Connection
        Command.CommandText = sql
        grd.DataSource = Command.ExecuteReader()
        grd.DataBind()
        lblRecordsFound.Text = "Observaciones encontradas " & grd.Rows.Count

        'Close the Connection Object 
        Connection.Close()

    End Sub

    Private Function GetSQL() As String
        Dim StartRecord As Long = PageSize * Session("NOTAS.CurrentPage") - PageSize + 1
        Dim StopRecord As Long = StartRecord + PageSize
        Dim sql As String = ""
        sql = sql & "SELECT * FROM notas WHERE NroExp = '" & Request("pExpediente") & "' AND Modulo = " & Request("pModulo")
        Return sql
    End Function

    Private Function GetPageCount() As Long
        Dim WholePageCount As Long = Math.Floor(Session("NOTAS.RecordsFound") / PageSize)
        Dim PartialRecordCount As Long = Session("NOTAS.RecordsFound") Mod PageSize

        If PartialRecordCount > 0 Then
            WholePageCount = WholePageCount + 1
        End If

        If WholePageCount = 0 Then
            WholePageCount = 1
        End If

        Return WholePageCount
    End Function

    'cmdAddNew_Click event is run when the user clicks the AddNew button
    Protected Sub cmdAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        'Go to the page : EditNOTAS.aspx
        Response.Redirect("EditNOTAS.aspx?pExpediente=" & Request("pExpediente") & "&pModulo=" & Request("pModulo"))
    End Sub

    Protected Sub grd_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd.RowCommand
        If e.CommandName = "" Then
            Dim IdUnico As String = grd.Rows(e.CommandArgument).Cells(0).Text
            Response.Redirect("EditNOTAS.aspx?ID=" & IdUnico & "&pExpediente=" & Request("pExpediente") & "&pModulo=" & Request("pModulo"))
        End If
    End Sub

End Class
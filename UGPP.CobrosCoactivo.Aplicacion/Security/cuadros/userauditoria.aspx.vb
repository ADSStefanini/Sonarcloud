Imports System.Data.SqlClient
Partial Public Class userauditoria
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Using myadacter As New SqlDataAdapter(SqlTran, Funciones.CadenaConexion)
                Dim cod As String = Request("cod")
                Dim nombre As String = Request("nombre")

                If Not cod Is Nothing Then
                    myadacter.SelectCommand.Parameters.Add("@cobrador", SqlDbType.VarChar).Value = Session("mcobrador")
                    myadacter.SelectCommand.Parameters.Add("@cod", SqlDbType.VarChar).Value = cod
                    Using tb As New DataSet1.Datos_User_ProcesosDataTable
                        myadacter.Fill(tb)
                        If tb.Rows.Count > 0 Then
                            GridView_user.DataSource = tb
                            GridView_user.DataBind()
                            lblProceso.Text = "Procesos registrados por el usuario " & tb.Rows(0).Item("nombre")
                            lbldetalle.Text = "<b>Regidtros detectados : " & tb.Rows.Count & "</b>"

                            Me.ViewState("Datos") = CType(tb, DataTable)
                        Else
                            no_reg.Attributes.Add("style", "width:100%;border-collapse:collapse;display:block;")
                            lblProceso.Text = "Procesos registrados por el usuario " & nombre
                            lbldetalle.Text = "No se detectaron registros"
                        End If
                    End Using
                End If
            End Using
        End If
    End Sub
    Function SqlTran() As String
        Dim sql As String
        sql = "SELECT top 1000 b.codigo, b.nombre, documentos.entidad, entesdbf.nombre AS nomente, documentos.idacto,actuaciones.nombre AS nomactuacion,documentos.docpredio_refecatrastal,documentos.docexpediente ,documentos.paginas, documentos.nomarchivo,documentos.docfechasystem,documentos.id FROM documentos inner join usuarios b on documentos.docusuario = b.codigo, entesdbf, actuaciones WHERE RTRIM(documentos.entidad) = RTRIM(entesdbf.codigo_nit) AND RTRIM(documentos.idacto) = RTRIM(actuaciones.codigo) AND RTRIM(documentos.cobrador) = @cobrador and docusuario = @cod and docfechasystem <= Getdate() ORDER BY docfechasystem DESC, idacto"
        Return sql
    End Function

    Private Sub GridView_user_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView_user.PageIndexChanging
        Dim grilla As GridView = CType(sender, GridView)
        With grilla
            .PageIndex = e.NewPageIndex()
        End With

        GridView_user.DataSource = CType(Me.ViewState("Datos"), DataTable)
        GridView_user.DataBind()
    End Sub

    Protected Sub btnSeperar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSeperar.Click
        Response.Redirect("usearuditoria_lista.aspx")
    End Sub
End Class
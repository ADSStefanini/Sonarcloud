Imports System.Data.SqlClient
Partial Public Class PermisosUser
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim menu As String
        menu = Context.Request.Params(0)

        Dim Ado As New SqlConnection()
        Ado.ConnectionString = "Password=leyton123;Persist Security Info=True;User ID=sa;Initial Catalog=bktecnoexpbd;Data Source=PC-LED\SQLEXPRESS"

        Dim MyAdapter As New SqlDataAdapter("SELECT  *  FROM USUARIO_PPALMENU WHERE (ME_MENU = '" & menu & "') ORDER BY ME_OPCIONINDEX ASC ", Ado)
        Dim Mytable As New DataTable
        MyAdapter.Fill(Mytable)

        Dim CadenaRetorno As String = ""
        If Mytable.Rows.Count > 0 Then
            Dim x As Integer = 0
            For x = 0 To Mytable.Rows.Count - 1
                CadenaRetorno = CadenaRetorno + CType(Mytable.Rows(x).Item("ME_PERMISO"), String) & "}" & CType(Mytable.Rows(x).Item("ME_DETALLEOPCION"), String)
                CadenaRetorno += "|"
            Next

            Response.Write(Mid(CadenaRetorno, 1, Len(CadenaRetorno) - 1))
        Else

        End If

        ''Response.Write("123")
    End Sub

End Class
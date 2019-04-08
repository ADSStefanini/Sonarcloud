Public Partial Class download
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim Arch As String = Request("documento")
            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = "application/pdf"
            Response.BufferOutput = True

            Response.AddHeader("content-disposition", "attachment; filename=" & Arch)
            Response.WriteFile(Arch)
            Response.End()
        End If
    End Sub
End Class
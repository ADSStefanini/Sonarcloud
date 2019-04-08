Imports System.Web
Imports System.Web.Services

Public Class download1
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim Arch As String = context.Request.QueryString("documento")

        context.Response.ClearHeaders()
        context.Response.ClearContent()
        context.Response.ContentType = "application/pdf"
        context.Response.BufferOutput = True

        context.Response.AddHeader("content-disposition", "attachment; filename=" & Arch)
        context.Response.WriteFile(Arch)
        context.Response.End()
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
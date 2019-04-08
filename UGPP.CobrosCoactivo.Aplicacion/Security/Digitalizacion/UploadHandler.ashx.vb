Imports System.Web
Imports System.Web.Services
Imports System.IO

Public Class UploadHandler
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'remove on production
        'Threading.Thread.Sleep(2000)

        Dim strFileName As String = Path.GetFileName(context.Request.Files(0).FileName)
        Dim strExtension As String = Path.GetExtension(context.Request.Files(0).FileName).ToLower()
        Dim strLocation As String = context.Server.MapPath("Upload") & "\" & context.Request.QueryString("archivador") & "\" & strFileName
        context.Request.Files(0).SaveAs(strLocation)

        context.Response.ContentType = "text/html"
        context.Response.Write("success: true")
    End Sub

    Public Function App_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
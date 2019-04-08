Imports System.Web
Imports System.Web.Services
Imports System.IO
Public Class pdfGenerator
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim ArchivoaBuscar As String = context.Request.QueryString("ImageFileName")
        Dim pdfPath As String = ArchivoaBuscar
        Dim client As New System.Net.WebClient()
        Dim buffer As [Byte]() = client.DownloadData(pdfPath)

        context.Response.Clear()
        context.Response.ClearContent()
        context.Response.ClearHeaders()
        context.Response.ContentType = "application/pdf"
        context.Response.BufferOutput = True
        context.Response.AddHeader("Content-Disposition", "filename=copyFileTecnoExpedientes.pdf")
        context.Response.AddHeader("content-length", buffer.Length.ToString())
        context.Response.BinaryWrite(buffer)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
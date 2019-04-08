Imports System.Web
Imports System.Web.Services

Public Class FileUploadHandler
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Private Sub IHttpHandler_ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim sRutaCargueLocal As String = AppDomain.CurrentDomain.BaseDirectory
        Dim sFullPathDocumento As String = "DocumentosLocal" & "\" & context.Session("ssloginusuario") & DateTime.Now.ToString("yyyymmddhhmmss")
        Dim fullpath As String = ""
        Try
            If context.Request.Files.Count > 0 Then
                Dim Files As HttpFileCollection = context.Request.Files
                Dim file As HttpPostedFile = Files(0)
                fullpath = sRutaCargueLocal & sFullPathDocumento & file.FileName
                file.SaveAs(fullpath)
                context.Response.Write("\" & sFullPathDocumento & file.FileName)
            End If
        Catch ex As Exception
        End Try
    End Sub


    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
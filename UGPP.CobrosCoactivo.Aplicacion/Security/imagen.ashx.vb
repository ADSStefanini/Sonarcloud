Imports System.Web
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Web.Services
Imports System.IO
Imports System.Data.SqlClient
Public Class imagen
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "image/jpeg"
        context.Response.Clear()
        context.Response.BufferOutput = True

        Dim connection As New SqlConnection(Funciones.CadenaConexion)
        Dim Command As New SqlCommand

        'Dim image As Image

        Dim photoId As String = context.Request.QueryString("ImageFileName")
        Dim photoTipo As String = context.Request.QueryString("tipo")
        If photoTipo = "Logo" Then
            Command.CommandText = "select ent_foto from  entescobradores where codigo = @PhotoId"
        ElseIf photoTipo = "Logo2" Then
            Command.CommandText = "select ent_foto2 from  entescobradores where codigo = @PhotoId"
        ElseIf photoTipo = "Logo3" Then
            Command.CommandText = "select ent_foto3 from  entescobradores where codigo = @PhotoId"
        Else
            Command.CommandText = "select ent_firma from  entescobradores where codigo = @PhotoId"
        End If

        Command.Parameters.AddWithValue("@PhotoId", photoId)
        connection.Open()
        Command.Connection = connection
        'image = Command.ExecuteScalar

        'CREO Q EL PROBLEMA EMPIEZA ACA -- VERIFICAR
        Try
            Dim bits As Byte() = CType(Command.ExecuteScalar, Byte())

            'If bits Is Nothing Then

            '    Dim Image As Image = System.Drawing.Image.FromFile(photoId)
            '    bits = CType(Image, Byte())
            'End If

            Dim memorybits As New MemoryStream(bits)
            Dim bitmap As New Bitmap(memorybits)


            'Dim fileName As String = HttpRuntime.AppDomainAppPath + "\" + context.Request.QueryString("ImageFileName")
            Dim bttmap As New Bitmap(bitmap)
            bttmap.Save(context.Response.OutputStream, ImageFormat.Jpeg)
        Catch

        End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
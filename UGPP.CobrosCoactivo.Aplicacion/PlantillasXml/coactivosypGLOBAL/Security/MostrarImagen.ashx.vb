Imports System.Web
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Web.Services
Imports System.IO

Public Class MostrarImagen
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "image/gif"
        context.Response.Clear()
        context.Response.BufferOutput = True

        'Dim fileName As String = HttpRuntime.AppDomainAppPath + context.Request.QueryString("ImageFileName")
        Dim fileName As String = context.Request.QueryString("ImageFileName")
        Dim Item As Integer = context.Request.QueryString("Item")

        'Dim imagen As New Bitmap(New Bitmap(fileName), CInt(800), CInt(1329))
        Dim imagen As New Bitmap(fileName)

        Dim myEncoder As Encoder
        Dim myEncoderParameter As EncoderParameter
        Dim myEncoderParameters As EncoderParameters

        'selecciona la pagina del tiff
        imagen.SelectActiveFrame(FrameDimension.Page, Item)
        myEncoderParameters = New EncoderParameters(2)

        myEncoder = Encoder.Quality
        myEncoderParameter = New EncoderParameter(myEncoder, CType(50L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter

        myEncoder = Encoder.ColorDepth
        myEncoderParameter = New EncoderParameter(myEncoder, CType(32L, Int32))
        myEncoderParameters.Param(1) = myEncoderParameter


        Dim tempStream As New MemoryStream
        imagen.Save(tempStream, GetEncoder(ImageFormat.Gif), myEncoderParameters)
        context.Response.OutputStream.Write(tempStream.ToArray(), 0, tempStream.Length)
        context.Response.Output.Write(imagen)

        imagen.Dispose()
        imagen = Nothing
    End Sub
    Private Function GetEncoder(ByVal format As ImageFormat) As ImageCodecInfo
        Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()
        Dim codec As ImageCodecInfo

        For Each codec In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next codec
        Return Nothing
    End Function
    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

Public Class Permisos_Expedientes
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"
        Dim Expediente As String = context.Request.Params("expediente")
        Dim Etapa As String = context.Request.Params("etapa")
        context.Response.Write(HTmlExpedientesinsecuenciador(Expediente, Etapa))
    End Sub

    Public Function HTmlExpedientesinsecuenciador(ByVal Expediente As String, Optional ByVal Etapa As String = "") As String
        Dim HtmlExpediente As New StringBuilder

        Dim Logic As Ejecucion_Expediente_examinar = New Ejecucion_Expediente_examinar(New SqlClient.SqlConnection(Funciones.CadenaConexion), Etapa)
        Dim Table As DataTable = Logic.ExamenCompleto(Expediente, Etapa)

        If Table.Rows.Count = 0 Then
            'No Trajo datos la aplicacion (expediente)
            Return "200"
        End If

        HtmlExpediente.Append("<table width=""100%"" class=""servicesT""><tr><th colspan='2'>Acto Administrativo</th><th style='text-align:left;width:100px;'>F. Radicación</th><th style='text-align:center;width:70px;'>Exp PPAL</th><th style='text-align:center;width:70px;'>Usuario</th></tr>")
        For Each row As DataRow In Table.Rows
            HtmlExpediente.Append("<tr><td style='text-align:left;width:15px;'>")
            HtmlExpediente.Append(valorNull(row("idacto"), "&nbsp"))
            HtmlExpediente.Append("</td><td class='servHd'>")
            HtmlExpediente.Append(ArmarHREF(row))
            HtmlExpediente.Append("</td><td>")
            HtmlExpediente.Append(valorNull(Format(CDate(row("fecharadic")), "dd/MM/yyyy"), "&nbsp"))
            HtmlExpediente.Append("</td><td style='text-align:center;'>")
            Dim Noppal As String = "<a href='javascript:void(0)' title='Este expediente no se encuentra acumulado.'>X</a>"
            HtmlExpediente.Append(IIf(valorNull(row("docacumulacio"), Noppal) = "", Noppal, row("docacumulacio")))
            HtmlExpediente.Append("</td><td style='text-align:center;'>")
            HtmlExpediente.Append(valorNull(row("docusuario"), "&nbsp"))
            HtmlExpediente.Append("</td></tr>")
        Next

        HtmlExpediente.Append("<tr style ='background-color:#ffffb2;'><td colspan='5'><p>El expediente <b>")
        HtmlExpediente.Append(Expediente)
        HtmlExpediente.Append("</b> posee ")
        HtmlExpediente.Append(Funciones.Num2Text(Table.Rows.Count))
        HtmlExpediente.Append(" (")
        HtmlExpediente.Append(Table.Rows.Count)
        HtmlExpediente.Append(") registro(s) indexado(s) a la etapa administrativa <b>")
        HtmlExpediente.Append(Table.Rows(0).Item("nometapa"))
        HtmlExpediente.Append("</b></p>")
        HtmlExpediente.Append("</table>")

        Return HtmlExpediente.ToString
    End Function

    Private Function ArmarHREF(ByVal row As DataRow) As String
        Dim reto As String = "<a title = '" & row("nombreacto") & "' href=""javascript:void(0)""  onclick=""window.open('TiffViewer.aspx?nomente=" & row("nombre") & "&idente=" & row("entidad") & "&F=" & row("nomarchivo") & "&totimg=" & row("paginas") & "&acto=" & row("nombreacto") & "&idacto=" & row("idacto") & "&folder=&Enabled=false&observacion=" & row("DOCOBSERVACIONES") & "&vsExpedienteAcu=', '', 'fullscreen=yes, scrollbars=auto')"">" & row("nombreacto") & "</a>"
        Return reto
    End Function

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
End Class
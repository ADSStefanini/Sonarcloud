Imports coactivosyp.My.Resources
Imports coactivosyp.PaginaBase

Public Class PaginaBase
    Inherits Page
    Dim log As LogProcesos
    Public Event prueba(ByVal sender As Object, ByVal e As EventArgs)
    Public Property nombreModulo As String
    ''' <summary>
    ''' propiedad con el html del mensaje
    ''' </summary>
    Private ReadOnly MsgError As String = StringsResourse.MensajeErrorHtml
    Delegate Sub EventoSub()
    ''' <summary>
    ''' Se inicializa el nombre del modulo
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnInit(e As EventArgs)
        nombreModulo = String.Empty
        MyBase.OnInit(e)
    End Sub
    ''' <summary>
    ''' Valida si la session esta activa
    ''' </summary>
    Protected Sub ValidadrSession()
        Dim newSessionIdCookie As HttpCookie = Request.Cookies("ASP.NET_SessionId")
        If Context.Session IsNot Nothing And Session.IsNewSession And newSessionIdCookie IsNot Nothing And newSessionIdCookie.Value <> String.Empty Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub
    ''' <summary>
    ''' Captura los errores no controlados de la pagina
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnError(e As EventArgs)
        log = New LogProcesos()
        Dim UltimaExeption As Exception = Server.GetLastError()
        If UltimaExeption IsNot Nothing Then
            log.ErrorLog(UltimaExeption.Message, Me.Page.GetType().ToString(), nombreModulo, LogProcesos.ErrorType.ErrorLog, UltimaExeption.GetBaseException().StackTrace)
        End If
        Response.Redirect("~/ErrorPage.aspx")
        Return
    End Sub
    ''' <summary>
    ''' Load de la pagina que valida la session
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnLoad(e As EventArgs)
        ValidadrSession()
        MyBase.OnLoad(e)
    End Sub
    ''' <summary>
    ''' Evento para control de erores de un metodo de tipo sub con mensaje de error por defecto
    ''' </summary>
    ''' <param name="Evento"></param>
    Protected Sub ControlSubExeptionLog(Evento As EventoSub)
        log = New LogProcesos()
        Try
            Evento()
        Catch ex As Exception
            Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "ClientScriptError", "<script type="" text/javascript""> " & "$('" & String.Format(MsgError, StringsResourse.MsgErrorIntento) & " ').appendTo('body');</script>")
            log.ErrorLog(ex.Message, Me.Page.GetType().ToString(), nombreModulo, LogProcesos.ErrorType.ErrorLog, ex.GetBaseException().StackTrace)
        End Try
    End Sub
    ''' <summary>
    ''' Evento para control de erores de un metodo de tipo sub con mensaje de error personalizado
    ''' </summary>
    ''' <param name="Evento"></param>
    ''' <param name="msg"></param>
    Protected Sub ControlSubExeptionLog(Evento As EventoSub, ByVal msg As String)
        log = New LogProcesos()
        Try
            Evento()
        Catch ex As Exception
            Me.Page.ClientScript.RegisterStartupScript(Me.Page.GetType(), "ClientScriptError", "<script type="" text/javascript""> " & "$('" & String.Format(MsgError, msg) & " ').appendTo('body');</script>")
            log.ErrorLog(ex.Message, Me.Page.GetType().ToString(), nombreModulo, LogProcesos.ErrorType.ErrorLog, ex.GetBaseException().StackTrace)
        End Try
    End Sub
End Class
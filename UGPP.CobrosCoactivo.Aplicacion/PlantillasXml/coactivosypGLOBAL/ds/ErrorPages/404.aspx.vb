Partial Public Class Formulario_web404
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.xHelp_err = "<b>HTTP 404</b>, error <b>404</b> o no encontrado es un código de estado <b>HTTP</b> que indica que el navegador web ha sido capaz de comunicarse con el servidor, pero no existe el fichero que ha sido pedido. Por ejemplo, si se accede a la URL http://coactivosyp/login.aspx el servidor que almacena la aplicacion devolve una página de error y el código de error <b>HTTP 404</b>. Este error no debe ser confundido con  " & Chr(32) & "servidor web no encontrado" & Chr(32) & " o errores similares en los que se indica que no se ha podido realizar la conexión con el servidor."

            Master.xHelp_info = "Esta excepción se produce cuando un modulo de de <b>Tecno Expedientes</b> no puede ser acezado por el usuario o por otro la lado dicho modulo no existe. <br /> " _
                            & "<br />A todos nos ha pasado alguna vez. Pinchas un enlace prometedor y te encuentras con una maravillosa página que dice " & Chr(32) & "error 403 o 404 - página no encontrada" & Chr(32) _
                            & "<hr /> El aspecto suele ser algo como esto: <hr /> <div style='text-align:left;'>En el navegador digitamos http://coactivosyp.net/login.aspx</div>" _
                            & "<hr /> Una vez hecho esto recibimos un mensaje de error lo cual más que un error es una advertencia " _
                            & "<br /><br /> <b>Nota:</b> Sino sabes que hacer solo haz clic en el boton de ayuda <b>Home</b> para ser re-direccionado al inicio de sesión y empezar a trabajar. " _
                            & "<br/><span style=color:#525252;font-size:xx-small>El botón ayuda dinámica Home es similar a este </span>" _
                            & "<img src='../images/1320452016_Home_01.png' height='50' width='50' style='float:right;margin-right:70px;' />"

        End If
    End Sub

End Class
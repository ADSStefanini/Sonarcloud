Partial Public Class Formulario_web403
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Master.xHelp_err = "<b>Error 403</b> <br/><br/>  El mensaje <b>Error 403</b> tiene relación con el error de Acceso Prohibido! <br /><br /> <b>Se debe o produce por las siguientes causas </b>" _
                             & "<div style='text-align:left;margin-left:10px;'><ul>" _
                                & "<li>No tiene permisos para acceder la pagina web</li>" _
                                & "<li>Protección contra lectura o no logra ser leído por el servidor. </li>" _
                                & "<li>Malas configuraciones en el navegador predeterminado (<b>Firefox</b>, <b>Internet Explorer</b>, <b>Google Chrome</b>). </li>" _
                             & "</ul></div>" _
                             & "<br/><div style='padding:7px;border:1px solid #A90000;background-color:#F8E5E5;'>Usted no tiene permiso para accesar al objeto solicitado. Existe la posibilidad de que este protegido contra lectura o que no haya podido ser leído por el servidor.</div>"

            Master.xHelp_info = "Esta excepción se produce cuando un modulo de de <b>Tecno Expedientes</b> no puede ser acezado por el usuario o por otro la lado dicho modulo no existe. <br /> " _
                             & "<br />A todos nos ha pasado alguna vez. Pinchas un enlace prometedor y te encuentras con una maravillosa página que dice " & Chr(32) & "error 403 o 404 - página no encontrada" & Chr(32) _
                             & "<hr /> El aspecto suele ser algo como esto: <hr /> <div style='text-align:left;'>En el navegador digitamos http://coactivosyp.net/login.aspx</div>" _
                             & "<hr /> Una vez hecho esto recibimos un mensaje de error lo cual más que un error es una advertencia " _
                             & "<br /><br /> <b>Nota:</b> Sino sabes que hacer solo haz clic en el boton de ayuda <b>Home</b> para ser re-direccionado al inicio de sesión y empezar a trabajar. " _
                             & "<br/><span style=color:#525252;font-size:xx-small;>El botón ayuda dinámica Home es similar a este </span>" _
                             & "<img src='../images/1320452016_Home_01.png' height='50' width='50' style='float:right;margin-right:70px;' />"

        End If
    End Sub

End Class
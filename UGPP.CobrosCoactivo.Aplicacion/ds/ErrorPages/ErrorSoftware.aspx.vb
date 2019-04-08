Imports System.Diagnostics
Imports System.IO
Partial Public Class ErrorSoftware
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Messenger As String

        Messenger = "<b style='font-size:xx-small'>POSIBLES ERRORES :</b><br />" _
        & "<strong>¿Por qué se produce este error?</strong>" _
        & "<ul>" _
        & "<li><b style=""font-size:12px;color:#000000;"">Este error se produce cuando sistema  intenta obtener acceso a un sitio web o ejecutar una operación compleja  y no lo consigue. Esto puede suceder por varias razones : </b><br /><br /> <span style=""font-size:10px""> 1. El equipo no está conectado a Internet. <br /> 2. La dirección del sitio web (dirección URL) no se escribió correctamente. <br /> 3. El servidor DNS no tiene un registro del sitio web, o el servidor web no está conectado. <br /> 4. Se termino o cerró la sesión de usuario por seguridad. <br /> 5. En la pagina o sitio que estaba operando se digito, escogió y relleno  mal una o más opciones. <br /> 6. El servidor web o la Base de Datos esta muy Concurrida. </span></li>" _
        & "<li><b style=""font-size:12px;color:#000000;"">Nota:</b> En caso de ignorar todos los errores llamar a su administrador del sistema.</li>" _
        & "</ul>"

        Master.xHelp_err = "<b>Tecno Expediente cometio una o mas fallas</b>" & "<br />" _
        & "<b>El error se detecto en :</b><br />" _
        & Request("aspxerrorpath") & "<br /><br />" _
        & "<b>Mensaje del error :</b><br />" _
        & "El sistema cometió una varias excepción incontrolables para re-direccionar a una herramienta  visual más agradable al usuario  <br /> " _
        & "<strong>¿Qué puedo hacer ?</strong>" _
        & "<ul>" _
        & "<li><span style=""font-size:12px;text-decoration:none;color:#000000;""><b>En primer lugar, asegúrese de que ha escrito correctamente la dirección web en la barra de direcciones.</b> Si la ortografía es correcta, vaya a otro sitio web para asegurarse de que el equipo está conectado a Internet. Si el equipo está conectado a Internet y el nombre se ha escrito correctamente, el servidor DNS no tiene un registro del sitio web o el sitio web no está disponible. Puede que desee intentarlo de nuevo más tarde.</span></li>" _
        & "<li><span style=""font-size:12px;text-decoration:none;color:#000000;""><b>Nota:</b> En caso de ignorar todos los errores llamar a su administrador del sistema.</span></li>" _
        & "</ul>"
     
        Master.xHelp_info = Messenger


        ''Historial de Errores
        Dim fic As String = Server.MapPath("~") & "\ErrorPages\recycle\recycle.html"
        Dim texto As String

        Dim sr As New System.IO.StreamReader(fic, System.Text.Encoding.Default)
        texto = sr.ReadToEnd()

        Using oSObj As New StreamWriter(Server.MapPath("~/ErrorPages/recycle/garbage/" & My.Computer.Name & "-" & DateString & "-" & DateTime.Now.ToString("HHmmss") & ".html"))
            oSObj.WriteLine(texto)
            oSObj.Flush()
            oSObj.Close()
        End Using
        sr.Close()


        'If Not Session("Error_in") Is Nothing Then
        '    Appli_Error(Session("Error_in"))
        'Else
        '    Error_inPanel.InnerHtml = "<div class='Error_in'><b>Tecno Expediente</b> no sabe que pudo haber ocurrido !!!<br /> <h3>Para más ayuda consulte con el administrador del sistema. </h3></div>"
        'End If
    End Sub
End Class
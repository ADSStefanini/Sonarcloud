<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usuariosweb_cambioclave.aspx.vb" Inherits="coactivosyp.usuariosweb_cambioclave" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gestión de usuarios</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <link href="tablacolor.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
     var decimas=0;
     $(document).ready(function(){
        $("#txtpwsAnterior").focus();
        
        $(window).scroll(function(){
	  		    $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		});
        
        $("#btnAceptar").click(function(e){
                if (($("#txtpwsAnterior").val() == "") || ($("#txtpwsNuevaContraseña").val() == "") || ($("#txtpwsConfirmar").val() == "")){
                    e.preventDefault();
                    $("#Messenger").html("<font color='#8A0808'><b style='text-decoration:underline;'>Faltan datos</b> para proceder con la operación.</font>");
                    $("#txtpwsAnterior").focus();
                    return 
                }
                
                var clav1 = $("#txtpwsNuevaContraseña").val();
                var clav2 = $("#txtpwsConfirmar").val();
                if (clav1 != clav2){
                    e.preventDefault();
                    $("#Messenger").html("<font color='#8A0808'>La cleve o contraseña no coincide, intente digitrala una vez mas.</font>");
                    $("#txtpwsNuevaContraseña").focus();
                }
                else{
                    var antes = $("#txtpwsAnterior").val() 
                    if (clav1 == antes){
                        e.preventDefault();
                        $("#Messenger").html("<font color='#8A0808'>La nueva contraseña no puede ser igual a la <b style='text-decoration:underline;'>contraseña anterior</b>.</font>");
                    }
                }
        });
        
        $("#btnCancelar").click(function(c){
              c.preventDefault();
              $("#Messenger").html("");  
              $("#txtpwsAnterior").val("");
              $("#txtpwsNuevaContraseña").val("");
              $("#txtpwsConfirmar").val("");
              $("#txtpwsAnterior").focus();
        });
     });
     
    function timerOper(){
      $("#btnCancelar").click();
      if (decimas == 0) { $("#imgcamclave").fadeOut(400, function() { $("#imgcamclave").attr('src', '../images/1321993766_Security_Reader2.png'); }).fadeIn(400); }
      CronoID = setTimeout("pagelogin()", 5000);
      $("#Messenger2").html("<font color='#454545' style='font-size:16px'><b>El sistema se cerrara en 5 segundos, para que inicie sesión con su nueva contraseña</b></font>");
       return false;
    }

    function pagelogin() {
      if (decimas == 0) { $("#imgcamclave").fadeOut(400, function() { $("#imgcamclave").attr('src', '../images/Security.png'); }).fadeIn(400); }
      
      decimas = decimas + 1;
      CronoID = setTimeout("pagelogin()", 1000);
      $("#Messenger2").html("<font color='#454545' style='font-size:16px'><b>El sistema se cerrara en breve(6seg) " + decimas + "</b></font>");
      if (decimas==6){
         document.location.replace("../login.aspx?clave=1");
         return false;
      }
      return false;
    }
    </script>
    
    <style type="text/css">
     .cam:hover 
     {
     text-decoration:underline;
     cursor:pointer;
     }
     .ws1
     {
         color:#fff;
     }
    </style>
</head>
<body>
    <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu4.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
     
   <div id="container">
    <form id="form1" runat="server">
     <h1 id="Titulo"><a href="#">Gestión de usuarios (cambiar clave)</a></h1>
       <div style="position:absolute;top:49px; left: 43px; width: 641px;" 
         class="info">
          Este modulo te permite cambiar la contraseña de tu cuenta de usuario, solo se afectaran los cambios cuando inicie la cuenta otra vez.
       </div>
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
       <div style="position:absolute;top:602px; left: 43px; width: 707px; padding-top:3px;border-top:2px solid #fff" 
         class="ws1">
          La seguridad de las contraseñas depende en gran medida de que se creen 
          contraseñas seguras, optimas y que contribuya a proteccion de la misma.
       </div>
       
       <div id ="Messenger" runat ="server" 
         style="position:absolute; top:309px; left:44px; width: 701px; font-size:13px;font-family:Verdana">
       </div>
       
       <a style="position:absolute;top:340px; left:629px; font-size:11px;color:White;" 
             href="usuariosweb.aspx">Gestión de usuarios</a>
       
       <div style="position:absolute; top:340px; left:214px; width: 325px;text-align: center;
           font-size:11px;font-family:Verdana;padding:10px;color:#ffffff;">
           <div style="height: 22px">Escriba una nueva contraseña :
           </div>
           <asp:TextBox ID="txtpwsNuevaContraseña" runat="server" CssClass="userBox" 
               TextMode="Password"></asp:TextBox>
               
           <div style="height: 22px"></div>                       
           <div style="height: 25px">Vuelva a escribir la nueva contraseña para confirmarla :
           </div>
           <asp:TextBox ID="txtpwsConfirmar" runat="server" CssClass="userBox" 
               TextMode="Password"></asp:TextBox>
           
           <br /><br />
           <asp:Button ID="btnAceptar" runat="server" Text="Crear contraseña" style="width:145px;background-image:url('images/icons/46.png');" CssClass="Botones" />
           &nbsp;
           <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" style="width:92px;background-image: url('images/icons/cancel.png');" CssClass="Botones" />
       </div>
       
        <table style="top: 123px; left: 44px; position: absolute;width: 701px;" border="0" 
               cellpadding="0" cellspacing="0" class="tabla">
                
                <tr class="modo1"><td colspan="3" align="center">Usted intenta cambiar la contraseña a este usuario </td></tr>
                <tr class="modo2"><td rowspan="5" style=" height:100px; width:100px;" ><img alt = "" src="imagenes/user3_128x128.png"  width="100" height="100" /></td></tr>
                <tr class="modo1">
                      <td style=" width:75px;">Nombre</td>
                      <td><asp:Label ID="lblNombre" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr class="modo2">
                      <td>Cedula o Id.</td>
                      <td><asp:Label ID="lblcedula" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr class="modo1">
                      <td>Login</td>
                      <td><asp:Label ID="lblLogin" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr class="modo2">
                      <td>Codigo</td>
                      <td><asp:Label ID="lblcodigo" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr class="modo1"><td colspan="3" align="center"><asp:Label ID="lbldetalle" runat="server" Text="##########"></asp:Label></td></tr>
          </table>
          
          <asp:Panel ID="pnlError" runat="server" CssClass="CajaDialogoErr" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
              <div id="logo">
                  <h1><a href="#" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
                  <p id="slogan">Gestión Documental.</p>
              </div>
              <div style="margin: 0  0 5px 0; ">
                 <% 
                     If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                         Response.Write(Me.ViewState("Erroruseractivo"))
                     End If
                 %>
              </div>
    		  <hr />	
		    				    
			  <asp:Button style="Z-INDEX: 116; width: 75px;" id="btnNoerror"
				    runat="server" Text="Aceptar" Height="23px" CssClass="RedButton"></asp:Button>    
         </asp:Panel>
		
		 <asp:Button ID="Button3" runat="server" Text="Button" style="visibility:hidden" />
		
         <asp:ModalPopupExtender ID="ModalPopupError" runat="server" 
            TargetControlID="Button3"
            PopupControlID="pnlError"

            CancelControlID="btnNoerror"
            
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
         >
         </asp:ModalPopupExtender>
           <script type="text/javascript">
               function mpeSeleccionOnCancel() {
                   var pagina = '../login.aspx?clave=1'
                   location.href = pagina
                   return false;
               }
        </script>
    </form>
   </div>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usuariosweb_Menu.aspx.vb" Inherits="coactivosyp.usuariosweb_Menu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Gestión de usuarios</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
     var decimas = 0;
     $(document).ready(function(){
     
        $(window).scroll(function(){
	  		    $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		});
        
        $("#btnAceptar").click(function(e){
               
        });
     });
     
     function timerOper(){
        CronoID = setTimeout("pagelogin()", 5000);
        $("#Messenger").html("<font color='#ffffff'><b style='text-decoration:underline;'>El sistema se cerrara en 5 segundos para que inicie sesión con su nueva contraseña</b></font>");
        return false;
     }

     function pagelogin() {
        decimas = decimas + 1;
        CronoID = setTimeout("pagelogin()", 1000); 
        $("#Messenger").html("<font color='#ffffff'><b style='text-decoration:underline;'>El sistema se cerrara en breve(5seg) " + decimas +  "</b></font>");
        if (decimas==5){
        document.location.replace("../login.aspx");
        return false;
        }
        return false;
     }
     
     function CheckVal(indexCheckVal){
         if (indexCheckVal == 1)
         {
          document.getElementById('chkConsultar_expedientes').checked = 1;
          document.getElementById('chkActualizar_expedientes').checked = 1;
          document.getElementById('chkConsulta_Diaria').checked = 1;
          document.getElementById('chkGestion_expediente').checked = 1;
          document.getElementById('chkGestion_usuario').checked = 1;
          document.getElementById('chkCambio_clave').checked = 1;
          document.getElementById('chkgestion_actuaciones').checked = 1;
         }
         else{
          document.getElementById('chkConsultar_expedientes').checked = 0;
          document.getElementById('chkActualizar_expedientes').checked = 0;
          document.getElementById('chkConsulta_Diaria').checked = 0;
          document.getElementById('chkGestion_expediente').checked = 0;
          document.getElementById('chkGestion_usuario').checked = 0;
          document.getElementById('chkCambio_clave').checked = 0;
          document.getElementById('chkgestion_actuaciones').checked = 0;
         }
     }
    </script>
    
    <style type="text/css">
     .cam:hover 
     {
         text-decoration:underline;
         cursor:pointer;
     }
     .ws2
     {
     	background-image: url('Menu_PPal/Li.png');
        background-repeat: repeat-x;

     	background-color:#0b4296;
     	border:1px solid #dedede;
        border-top:1px solid #eee;
        border-left:1px solid #eee;
        
        font-family:"Lucida Grande", Tahoma, Arial, Verdana, sans-serif;
        font-size:17px;
        text-decoration:none;
        font-weight:bold;
        color:#ffffff;
        padding:2px;
     	float:left;
     	text-align:left;
     	cursor:pointer;
     }
    
.tabla {
font-family: Verdana, Arial, Helvetica, sans-serif;
font-size:12px;
text-align: left;
}

.tabla .modo1 {
font-size: 12px;
font-weight:bold;
background-color: #e2ebef;
background-image: url(Imagenes/fondo_tr01.png);
background-repeat: repeat-x;
color: #34484E;
font-family: "Trebuchet MS", Arial;
}
.tabla .modo1 td {
padding: 5px;
border-right-width: 1px;
border-bottom-width: 1px;
border-right-style: solid;
border-bottom-style: solid;
border-right-color: #A4C4D0;
border-bottom-color: #A4C4D0;
text-align:left;
}

.tabla .modo2 {
font-size: 12px;
font-weight:bold;
background-color: #fdfdf1;
background-repeat: repeat-x;
color: #990000;
font-family: "Trebuchet MS", Arial;
text-align:left;
}
.tabla .modo2 td {
padding: 5px;
border-right-width: 1px;
border-bottom-width: 1px;
border-right-style: solid;
border-bottom-style: solid;
border-right-color: #EBE9BC;
border-bottom-color: #EBE9BC;
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
            <a href="menu.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
     
   <div id="container">
    <form id="form1" runat="server">
     <h1 id="Titulo"><a href="#">Gestión de usuarios (Asignar Opciones)</a>														
	 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
	 <asp:Button id="btnAceptar" runat="server" Text="Guardar Opciones" 
         style="position:absolute;top:1077px; left:262px; font-weight:bold;font-size:16px;width: 232px; height:54px; background-image: url('images/icons/Defragmentation.png'); right: 286px;" 
         CssClass="Botones"></asp:Button>
     
     </h1>
       <div style="position:absolute;top:39px; left: 43px; width: 641px;" 
         class="info">
          Este modulo le permite asignar o denegar los diferentes módulos que puede manipular el usuario para administrar de forma fácil y rápida la seguridad de sus datos.
       </div>
       
       <div id ="Messenger" runat ="server" 
             
             
         
         
         style="position:absolute; top:262px; left:41px; width: 709px; font-size:11px;font-family:Verdana">
       
       
       </div>
       
       <a style="position:absolute;top:284px; left:46px; font-size:11px;color:White;" 
             href="usuariosweb.aspx">Gestión de usuarios</a>
             
     <asp:CheckBox ID="chkCambio_clave" runat="server" 
         style="position:absolute; top: 722px; left: 44px;" Text="Cambiar Clave" 
         Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkgestion_salcontrires" runat="server" 
         style="position:absolute; top: 592px; left: 284px; width: 265px;" 
         Text="	Saldos Por Contribuyente Resumidos " Font-Size="12px" 
         ForeColor="White" />
             
     <div style="margin-left: 80px">
         <asp:CheckBox ID="chkgestion_meResolAcumulado" runat="server" 
         style="position:absolute; top: 566px; left: 549px; width: 195px;" 
         Text="Resolución de Acumulación" Font-Size="12px" ForeColor="White" />
         <asp:CheckBox ID="chkgestion_ejecuActuac" runat="server" 
         style="position:absolute; top: 566px; left: 284px; width: 213px;" 
         Text="Ejecución de Actuaciones" Font-Size="12px" ForeColor="White" />
     </div>
             
     <asp:CheckBox ID="chkgestion_impActAdmin" runat="server" 
         style="position:absolute; top: 592px; left: 42px; width: 213px;" 
         Text="Imp. de Actos Administrativos" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_meregisEntes" runat="server" 
         style="position:absolute; top: 919px; left: 549px; right: 99px;" 
         Text="Registrar Entes" Font-Size="12px" 
         ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_mefestivoadd" runat="server" 
         style="position:absolute; top: 894px; left: 549px; right: 33px;" 
         Text="Registrar Festivos" Font-Size="12px" 
         ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_meregisActos" runat="server" 
         style="position:absolute; top: 943px; left: 45px; right: 517px;" 
         Text="Registrar Actos" Font-Size="12px" 
         ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_meconfigimprimi" runat="server" 
         style="position:absolute; top: 943px; left: 281px;" 
         Text="Configuración actos imprimibles" Font-Size="12px" 
         ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_mesecueacto" runat="server" 
         style="position:absolute; top: 918px; left: 281px;" 
         Text="Secuencia de Actos" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_meregideu" runat="server" 
         style="position:absolute; top: 894px; left: 281px;" 
         Text="Registrar Deudores" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_meregisEtapa" runat="server" 
         style="position:absolute; top: 918px; left: 45px;" 
         Text="Registrar Etapas" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_datosbasicos" runat="server" 
         style="position:absolute; top: 894px; left: 45px;" 
         Text="Datos Básicos" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_usuario" runat="server" 
         style="position:absolute; top: 748px; left: 43px;" 
         Text="Gestión de Usuarios" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkGestion_Cobranza" runat="server" 
         style="position:absolute; top: 566px; left: 43px;" 
         Text="Cobranza" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkConsulta_Diaria" runat="server" 
         style="position:absolute; top: 443px; left: 43px;" 
         Text="Consulta Diaria" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkActualizar_expedientes" runat="server" 
         style="position:absolute; top: 420px; left: 43px; width: 203px;" 
         Text="Actualizar Expedientes" Font-Size="12px" ForeColor="White" />
             
     <asp:CheckBox ID="chkConsultar_expedientes" runat="server" 
         style="position:absolute; top: 396px; left: 43px; width: 193px;" 
         Text="Consultar Expedientes" Font-Size="12px" ForeColor="White" />
    
   
         
    <div class="ws2" style="position:absolute;top: 796px; left: 43px; width: 697px;">
        <img src="images/icons/Notes.png" alt="" width="75" height="75" />
        <div style="position:absolute; top:30px; left :100px">MAESTROS Y DATOS BASICOS</div>
    </div>
    
   
         
    <div style="position:absolute;top:1018px; left: 42px; width: 701px; padding-top:3px;border-top:2px solid #fff" 
         class="ws1">
    </div>
    
    <div class="ws2" style="position:absolute;top: 305px; left: 43px; width: 697px;">
        <img src="Menu_PPal/consultar.png" alt="" width="75" height="75" /> 
        <div style="position:absolute; top:30px; left :100px">CONSULTA DE EXPEDIENTE</div>
    </div>
    
    <div class="ws2" style="position:absolute;top: 472px; left: 41px; width: 697px;">
        <img src="Menu_PPal/gen-expedientes.png" alt="" width="75" height="75" />            
        <div style="position:absolute; top:30px; left :100px">PROCESO TRIBUTARIO</div> 
    </div>
    
    <div class="ws2" style="position:absolute;top: 632px; left: 43px; width: 697px;">
        <img src="Menu_PPal/PPalusuarios.png" alt="" width="75" height="75" />
        <div style="position:absolute; top:30px; left :100px">ADMINISTRACIÓN DE USUARIOS</div>
    </div>
    
    <a href="javascript:CheckVal(1)" 
         style="position:absolute;top:285px; left:516px; font-size: 11px; font-weight: normal; font-style: italic; color: #FFFFFF; width: 100px;">Seleccionar todos</a>
    
    <a href="javascript:CheckVal(0)" 
         style="position:absolute;top:284px; left:628px; font-size: 11px; font-weight: normal; font-style: italic; color: #FFFFFF;">Deseleccionar todos</a>
    
        <table style="top: 113px; left: 44px; position: absolute;width: 701px;" border="0" cellpadding="0" cellspacing="0" class="tabla">
                <tr class="modo1"><td colspan="2" align="center">Usted intenta cambiar las opciones de menú de este usuario </td></tr>
                <tr class="modo2">
                      <td style=" width:75px;">Nombre</td>
                      <td><asp:Label ID="lblNombre" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 
                 <tr class="modo2">
                      <td>Cedula o Id.</td>
                      <td><asp:Label ID="lblcedula" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 
                 <tr class="modo2">
                      <td>Codigo</td>
                      <td><asp:Label ID="lblcodigo" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 
                 <tr class="modo1"><td colspan="2" align="center"><asp:Label ID="lbldetalle" runat="server" Text="##########"></asp:Label></td></tr>
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
    </form>
   </div>
</body>
</html>

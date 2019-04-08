<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="menu.aspx.vb" Inherits="coactivosyp.menu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Entradas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="tablacolor.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
            .ws2 {background-image: url('Menu_PPal/Li.png');background-repeat: repeat-x;background-color:#0b4296;border:1px solid #3c5d9c;font-family:"Lucida Grande", Tahoma, Arial, Verdana, sans-serif;font-size:17px;text-decoration:none;font-weight:bold;color:#ffffff;padding:2px;float:left;text-align:left;cursor:pointer;}
            .tablemenu {margin: 0px;}
            .tablamenu tr td 
            {
                margin: 0px; padding: 0px;
                float: left;
                position: relative; /* Aquí ponemos posicionamiento absoluta */
                width: 105px;
                height: 105px;
            }
            .tablamenu tr td img
            {
                width: 75px; height: 75px; /* Aquí va el tamaño del thumbnail pequeño */
                border: 1px solid #ddd;
                padding: 5px;
                /*background: #f0f0f0;*/
                position: absolute;
                left: 37px; top: 0px;
                text-align:center;
            }
            .tablamenu tr td:hover 
            {
              /*background-color:#E0F8F7;*/
            }
            .tablamenu tr td
            {
              font-size:11px;
              color: #444444;    
              width:150px;
              text-align:center;        	
            }
            .tecno
            {
                position:absolute;
                top:969px;
                left: 200px;
                font-family:"Lucida Grande", Tahoma, Arial, Verdana, sans-serif;
                font-size:17px;
                font-weight:bold;
            }
            .tecno a
            {
            	color:#ffffff;
            }
            .tecno a:hover 
            {
            	color:red;
            } 
            .xparametrosInfo_Permisos {font-family: Verdana;font-size: 12px;font-style: normal}
          .tablamenu
        {
            height: 183px;
        }
            
            .divcentrado {
					left: 50%;
					top: 50%;
					height:76px; 
					width: 620px; 
					margin:0 auto 0 auto;	 
					overflow: auto;					
					/*border: 1px solid red;*/
			}
          </style>
          
          <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
          <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
          <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
          <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
          <script  language="javascript" type="text/javascript">
              $(document).ready(function() {
           
//            $("table.tablamenu td").hover(function() {
//                    $(this).css({'z-index' : '1515'}); /*Agregamos un valor de z-index mayor para que la imagen se mantenga arriba */ 
//                    $(this).find('img').addClass("hover").stop() /* Le agregamos la clase "hover" */
//                    .animate({
//                    marginTop: '-75px', /* Las siguientes 4 líneas alinearán verticalmente la imagen */ 
//                    marginLeft: '-75px',
//                    top: '50%',
//                    left: '50%',
//                    width: '105px', /* Aquí va la nueva medida para el ancho */
//                    height: '105px', /* Aquí va la nueva medida para el alto */
//                    padding: '20px'
//                    }, 200); /* Este valor de "200″ es la velocidad de cuán rápido/lento se anima este hover */
//                    } , function() {
//                    $(this).css({'z-index' : '1505'}); /* Volvemos a poner el z-index nuevamente a 0 */
//                    $(this).find('img').removeClass("hover").stop() /* Quitamos la clase "hover" y detenemos la animación*/
//                    .animate({
//                    marginTop: '0', /* Volvemos a poner el valor de alineación como el default */
//                    marginLeft: '0',
//                    top: '0',
//                    left: '37',
//                    width: '75px', /* Volvemos el valor de ancho como al inicio */
//                    height: '75px', /* Volvemos el valor de ancho como al inicio */
//                    padding: '5px'
//                    }, 400);
//                });

                // Dialog
                $('.xparametrosInfo_Permisos').dialog({
                    autoOpen: false,
                    width: 370,
                    modal: true,
                    buttons: {
                        "Aceptar": function() {
                            $(this).dialog("close");
                        }
                    },
                    hide: 'fold'
                });

                $('.dialog_link').click(function(evento) {
                    evento.preventDefault();
                    $('.xparametrosInfo_Permisos').dialog('open');
                    return false;
                });

            });
          </script>
</head>
<body>
    <div id="container">
        <h1 id="Titulo"><a href="#">Menu Principal  - SUBDIRECCION DE COBRANZAS</a></h1>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            
            <div class="xparametrosInfo_Permisos" style="display:none; text-align:left; " title="USUARIO - NO TIENE PERMISOS">
                <img src="../images/1321994028_watchman.png" alt="Seguridad" style="float:left;" title="Seguridad" />
                <span style="font-weight:bold;font-size:14px;">Atención de seguridad</span>
                <br />  
                <p style="text-align:justify;font-size:xx-small;">Lo sentimos pero el usuario con el cual se encuentra identificado no tiene <b>permisos</b> (o derechos de acceso)  para ingresar a este modulo.</p>
                <b>Verifique con el administrador del sistema ...</b>
            </div>
            
            <div class="ws2" style="position:absolute;top:53px;left:43px;width:697px;">
                <img src="images/icons/Rename.png" alt="" width="75" height="75" /> 
                <div style="position:absolute; top:30px; left :100px">MENU PRINCIPAL</div>
            </div>
            
            <div style="position:absolute;left:84px; top:147px; z-index:1001;">                       
                <table class="tablamenu">
                    <tr>
                        <td>
                            <a id ="datosbasicos" href="MenuMaestros.aspx" runat="server"><img alt ="" src="images/icons/HP-Control.png" height="75" width="75" /></a>
                        </td>
                        <td>
                            <a id ="A4" href="menu4.aspx"><img alt ="" src="images/icons/Keys.png" height="75" width="75" /></a>
                        </td>
                        <td>
                            <a id ="A7" href="Maestros/EJEFISGLOBALREPARTIDOR.aspx" runat="server"><img alt ="" src="images/icons/dossier.png" height="75" width="75" /></a>
                        </td>
                        <td>
                            <a id ="A5" href="Maestros/LOG_AUDITORIA.aspx"><img alt ="" src="images/icons/auditar.png" height="75" width="75" /></a>
                        </td>
                    </tr>
                    <tr style=" height:20px; ">
                      <td style=" height:20px; ">Datos Básicos</td>
                      <td style=" height:20px; ">Administrar usuarios</td>
                      <td style=" height:20px; ">Administrar expedientes</td>
                      <td style=" height:20px; ">Log / Auditoría de procesos</td>
                    </tr>
                    <tr>
                        <td colspan="4" style=" width:620px">
                            <table>
                                <tr>
                                    <td>
                                        <a id ="A8" href="FrmGrupoReportes.aspx">
                                            <img alt ="" src="images/icons/informes96x96.png" height="75" width="75" /> 
                                        </a>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="images/icons/Shutdown.png" height="75" width="75" /></asp:LinkButton>
                                    </td>
                                    <td>
                                        <a id ="A009" href="Maestros/sincronizarusuarios.aspx" runat="server">
                                            <img alt ="" src="images/usuarios.png" height="75" width="75" />
                                       </a>
                                    </td>
                                </tr>
                                <tr style=" height:20px; ">
                                    <td style=" height:20px; ">
                                        Informes
                                    </td>
                                    <td>
                                        Salir
                                    </td>
                                    <td>
                                        Sincronizar usuarios
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
           </div>
           <div style="position:absolute;left:43px; top:404px; z-index:1001; height: 2px; width: 697px; background-color:#fff;">
           </div>
           <div style="position:absolute;left:313px; top:404px; z-index:1001;">                       
                <table class="tablamenu">
                    <tr>
                        <td>
                            <%--<asp:LinkButton ID="A3" runat="server"><img alt ="" src="images/icons/Shutdown.png" height="75" width="75" /></asp:LinkButton>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>Cerrar Sesión</td>
                    </tr>
                </table>
           </div>
           
           
          <table style="top: 547px; left: 97px; position: absolute;width: 585px;" border="0" 
               cellpadding="0" cellspacing="0" class="tabla">
                
                <tr><th colspan="3" align="center">Usuario Activo</th></tr>
                <tr class="modo2"><td rowspan="5" style=" height:100px; width:100px;" ><img alt = "" src="imagenes/user3_128x128.png"  width="100" height="100" /></td></tr>
                <tr class="modo1">
                      <td>Nombre</td>
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
                 <tr><th colspan="3" align="center"><asp:Label ID="lbldetalle" runat="server" Text="##########"></asp:Label></th></tr>
           </table>
            
          
          <div style="background-image: url('Menu_PPal/Li.png');background-repeat: repeat-x;width:690px; padding:5px;background:#0b4295;top:479px; left:43px; position: absolute;color:#FFF;border:solid 1px #3c5d9c;">
			   <h2 style="margin:0px; padding:0px;text-transform:uppercase;">Coactivo SyP</h2>
			   <h3 style="margin:0px; padding:0px;"> Subdirección de Cobranzas <%--<%  Response.Write(Session("ssCodimpadm") & ".")%>--%> </h3>
		  </div>
           
           
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
            OnCancelScript="mpeSeleccionOnCancel()"
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
        >
        </asp:ModalPopupExtender>
           <script type="text/javascript">
               function mpeSeleccionOnCancel() {
                   var pagina = '../login.aspx'
                   location.href = pagina
                   return false;
               }
        </script> 
      </form>
    </div>
</body>
</html>

﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="menuej.aspx.vb" Inherits="coactivosyp.menuej" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Entradas</title>
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         .ws2
         {
     	    background-image: url('Menu_PPal/Li.png');
            background-repeat: repeat-x;

     	    background-color:#0b4296;
     	    border:1px solid #3c5d9c;
            
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
         
         
           .tablemenu
            {
                margin: 0px;
            }
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
            top: 2px;
            left: 2px;
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
          </style>
          <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
          <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
          <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
          <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
          <script  language="javascript" type="text/javascript">
           $(document).ready(function(){
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
        <h1 id="Titulo"><a href="#">PROCEDIMIENTO TRIBUTARIO - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 43px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
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
            
            <div class="ws2" style="position:absolute;top: 89px; left: 43px; width: 697px;">
                <img src="images/icons/insurance.png" alt="" width="75" height="75" /> 
                <div style="position:absolute; top:30px; left :100px">PROCEDIMIENTO TRIBUTARIO</div>
            </div>
            
            <div style="position:absolute;left:84px; top:193px; z-index:1001;">                       
                <table class="tablamenu">
                    <tr>
                        <td>
                            <a id ="A2" href="menu.aspx"><img alt ="" src="images/icons/UtilisateurSZ.png" height="75" width="75" /></a>
                        </td>
                        <td>
                            <a id ="datosB" href="historiaexpediente.aspx" class="dialog_link" ><img alt ="" src="images/icons/Chat.png" height="75" width="75" /></a>
                        </td>
                        <td>
                            <a id ="A1" href="subirexpedientes.aspx" class="dialog_link"><img alt ="" src="images/icons/Document.png" height="75" width="75" /></a>
                        </td>
                         <td>
                            <a id ="A4" href="consultardocumentos2.aspx" class="dialog_link"><img alt ="" src="images/icons/Maps.png" height="75" width="75" /></a>
                        </td>
                    </tr>
                    <tr>
                      <td>Menú Principal</td>
                      <td>Discusión</td>
                      <td>Liquidación Oficial</td>
                      <td>Fiscalización</td>
                    </tr>
                </table>
                <table class="tablamenu">
                    <tr>
                        <td>
                           <a id ="OpPredial" href="generador-expedientes.aspx" runat="server"><img alt ="" src="images/icons/Thumbs_Up.png" height="75" width="75" /></a> 
                        </td>
                        <td>
                            <a id ="A5" href="estado-cuenta.aspx" runat="server"><img alt ="" src="../images/estadocuenta.png" height="75" width="75" /></a> 
                        </td>
                        <td>
                            <a id ="A6" href="consulta-predial.aspx" runat="server"><img alt ="" src="../images/house-search.jpg" height="75" width="125" /></a> 
                        </td>
                        <td>
                            <!-- <a id ="A7" href="consultapazysalvo.aspx" runat="server"><img alt ="" src="../images/house-search.jpg" height="75" width="125" /></a> -->
                        </td>
                    </tr>
                    <tr>
                        <td>Cobranzas</td>
                        <td>Estado de cuenta</td>
                        <td>Consulta de Predial</td>
                        <!-- <td>Consulta Paz y Salvo</td> -->
                    </tr>
                </table>
           </div>
           <div style="position:absolute;left:43px; top:595px; z-index:1001; height: 2px; width: 697px; background-color:#fff;">
           </div>
           <div style="position:absolute;left:313px; top:618px; z-index:1001;">                       
                <table class="tablamenu">
                    <tr>
                        <td>
                            <a id ="A3" href="../login.aspx"><img alt ="" src="images/icons/Shutdown.png" height="75" width="75" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td>Cerrar Sesión</td>
                    </tr>
                </table>
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


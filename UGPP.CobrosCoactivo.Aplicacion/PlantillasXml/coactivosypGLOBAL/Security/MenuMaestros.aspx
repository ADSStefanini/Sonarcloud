<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuMaestros.aspx.vb" Inherits="coactivosyp.MenuMaestros" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Entradas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
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
          .style1
        {
            height: 434px;
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
        <h1 id="Titulo"><a href="#">Entradas y datos basicos</a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 42px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
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
            
            <div class="ws2" style="position:absolute;top: 89px; left: 43px; width: 697px; cursor:auto">
                <img src="images/icons/Notes.png" alt="" width="75" height="75" /> 
                <div style="position:absolute; top:30px; left :100px;"><a href="menu.aspx" style="color:#FFF">INICIO</a>>MAESTROS Y DATOS BASICOS</div>
            </div>
                                   
            <table style="position:absolute;left:84px; top:193px; z-index:1001;" 
                class="tablamenu">
                <tr>
                    <td>
                        <a id ="A2" href="menu.aspx"><img alt ="" src="images/icons/UtilisateurSZ.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        
                        <a id ="meregisEtapa" href="maestro_etapa.aspx" runat="server"><img alt ="" src="images/icons/Etapas.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="meregisActos" href="maestro_actuaciones.aspx" runat="server"><img alt ="" src="images/icons/Actos.png" height="75" width="75" /></a>
                    </td>
                     <td>
                        
                    </td>
                 </tr>
                <tr>
                  <td>Menú Principal</td>
                  <td>Registrar Etapas</td>
                  <td>Registrar Actos</td>
                  <td> </td>
                </tr>
           </table>
           
           <table style="position:absolute;left:84px; top:366px; z-index:1001;" class="tablamenu">
                <tr>
                    
                    <td>
                        <a id ="mesecueacto" href="configuracionActos.aspx" runat="server"><img alt ="" src="images/icons/Alarm.png" height="75" width="75" /></a>
                    </td>
                     <td>
                        <a id ="meregisEntes" href="EntesCobradores.aspx" runat="server"><img alt ="" src="images/icons/FermerSZ.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <%--<a id ="A7" href="Maestros/plantilla.aspx?pag=EJEFISGLOBAL" runat="server"><img alt ="" src="images/icons/dossier.png" height="75" width="75" /></a>--%>
                        <%--<a id ="A7" href="Maestros/EJEFISGLOBALREPARTIDOR.aspx" runat="server"><img alt ="" src="images/icons/dossier.png" height="75" width="75" /></a>--%>
                        <asp:LinkButton ID="A8" runat="server"><img alt ="" src="images/icons/Shutdown.png" height="75" width="75" /></asp:LinkButton>
                    </td>
                     <td>
                        <%--<asp:LinkButton ID="A8" runat="server"><img alt ="" src="images/icons/Shutdown.png" height="75" width="75" /></asp:LinkButton>--%>
                    </td>
                </tr>
                <tr>                  
                  <td>Secuencia de Actos</td>
                  <td>Registrar Entes</td>
                  <td>Cerrar Sesión</td>
                  <td><%--Cerrar Sesión--%></td>
                </tr>
           </table>
           
           
           <div class="ws2" 
                style="position:absolute;top: 537px; left: 43px; width: 697px;">
                <img src="images/icons/Select.png" alt="" width="75" height="75" /> 
                <div style="position:absolute; top:30px; left :100px">OTROS DATOS</div>
            </div>
            
            <table style="position:absolute;left:91px; top:672px; z-index:1001;" 
                class="tablamenu">
                <!-- Fila de imagenes -->
                <tr>
                    <td>
                        <a id ="mefestivoadd" href="Config/Festivos.aspx" runat="server"><img alt ="" src="images/icons/Clock.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A9" href="Maestros/plantilla.aspx?pag=TIPOS_IDENTIFICACION"><img alt ="" src="Imagenes/carnet.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A10" href="Maestros/plantilla.aspx?pag=TIPOS_PERSONA" ><img alt ="" src="Imagenes/1318550964_User.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A1" href="Maestros/plantilla.aspx?pag=DEPARTAMENTOS" ><img alt ="" src="Imagenes/colombia.png" height="75" width="75" /></a>
                    </td>
                </tr>
                <!-- Fila de etiquetas -->
                <tr>
                  <td class="style1">Registrar Festivos</td>
                  <td class="style1">Tipos de identificación</td>
                  <td class="style1">Tipos de persona</td>
                  <td class="style1">Maestro Departamentos</td>
                </tr>
                
                <!-- Fila de imagenes -->
                <tr>
                    <td>
                        <a id ="A3" href="Maestros/plantilla.aspx?pag=ESTADOS_PERSONA" runat="server"><img alt ="" src="imagenes/persona-estado.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A4" href="Maestros/plantilla.aspx?pag=TIPOS_APORTANTES"><img alt ="" src="Imagenes/aportes.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A13" href="Maestros/plantilla.aspx?pag=MUNICIPIOS"><img alt ="" src="Imagenes/ciudad3.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A15" href="Maestros/plantilla.aspx?pag=TIPOS_TITULO" runat="server"><img alt ="" src="imagenes/titulos.png" height="75" width="75" /></a>
                    </td>
                </tr>
                <!-- Fila de etiquetas -->
                <tr>
                  <td>Estados persona</td>
                  <td>Tipos de aportantes</td>
                  <td>Municipios</td>
                  <td>Tipos de título</td>
                </tr>
                
                <!-- Fila de imagenes -->
                <tr>
                    <td>
                        <a id ="A16" href="Maestros/plantilla.aspx?pag=FORMAS_NOTIFICACION" runat="server"><img alt ="" src="imagenes/notific.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A5" href="Maestros/plantilla.aspx?pag=EditCONFIGURACIONES_ACTIVE_DIRECTORY" runat="server"><img alt ="" src="imagenes/activedirectory.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A6" href="Maestros/plantilla.aspx?pag=ConfiguracionInteresesParafiscales" runat="server"><img alt ="" src="imagenes/liqoficial.png" height="75" width="75" /></a>
                    </td>
                    <td>
                        <a id ="A7" href="Maestros/alarmas.aspx" runat="server"><img alt ="" src="imagenes/alarma.png" height="75" width="75" /></a>
                    </td>
                </tr>
                <!-- Fila de etiquetas -->
                <tr>
                  <td>Formas de notificación</td>
                  <td>Configurar forma de autenticación</td>
                  <td>Configurar intereses liq. oficiales</td>
                  <td>Alarmas y escalamientos</td>
                </tr>
                
                <!-- Fila de imagenes -->
                <tr>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        <%--<a id ="A17" href="Maestros/plantilla.aspx?pag=MAESTRO_TITULOS" runat="server"><img alt ="" src="imagenes/maestrotitulos.png" height="75" width="75" /></a>--%>
                    </td>
                    <td>
                        
                    </td>
                </tr>
                <!-- Fila de etiquetas -->
                <tr>
                  <td></td>
                  <td></td>
                  <td><%--Maestro de títulos ejecutivos--%></td>
                  <td></td>
                </tr>
                
           </table>
           <!-- <div class="tecno"><a href="menu.aspx" style="text-decoration:none;">Quiero ir directamente a tecno expedientes</a></div> -->
           
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

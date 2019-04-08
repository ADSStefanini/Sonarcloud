<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cobranzatipo.aspx.vb" Inherits="coactivosyp.cobranzatipo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        a{color:#C8DCE5; }
        h3{ margin: 10px 10px 0 10px; color:#FFF; font:18pt Arial, sans-serif; letter-spacing:-1px; font-weight: bold;  }
        .boxgrid{ 
        width: 325px; 
        height: 260px; 
        
        float:left; 
        background:#232426; 
        border: solid 2px #0b4295; 
        overflow: hidden; 
        position: relative; 
        }
        .boxgrid img{ 
        position: absolute; 
        top: 0; 
        left: 0; 
        border: 0; 
        }
        .boxgrid p{ 
        padding: 0 10px; 
        color:#afafaf; 
        font-weight:bold; 
        font:10pt "Lucida Grande", Arial, sans-serif; 
        }
        .Xarticles {
		border: solid 2px #0b4295; 
		background:#437ACE; 
		color: #fff;
		}
	    .image { float: left; }
    </style>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            //To switch directions up/down and left/right just place a "-" in front of the top/left attribute
            //Vertical Sliding
            $('.boxgrid.slidedown').hover(function() {
                $(".cover", this).stop().animate({ top: '-260px' }, { queue: false, duration: 300 });
            }, function() {
                $(".cover", this).stop().animate({ top: '0px' }, { queue: false, duration: 300 });
            });
            //Horizontal Sliding
            $('.boxgrid.slideright').hover(function() {
                $(".cover", this).stop().animate({ left: '325px' }, { queue: false, duration: 300 });
            }, function() {
                $(".cover", this).stop().animate({ left: '0px' }, { queue: false, duration: 300 });
            });
            //Diagnal Sliding
            $('.boxgrid.thecombo').hover(function() {
                $(".cover", this).stop().animate({ top: '260px', left: '325px' }, { queue: false, duration: 300 });
            }, function() {
                $(".cover", this).stop().animate({ top: '0px', left: '0px' }, { queue: false, duration: 300 });
            });
            //Partial Sliding (Only show some of background)
            $('.boxgrid.peek').hover(function() {
                $(".cover", this).stop().animate({ top: '90px' }, { queue: false, duration: 160 });
            }, function() {
                $(".cover", this).stop().animate({ top: '0px' }, { queue: false, duration: 160 });
            });
            //Full Caption Sliding (Hidden to Visible)
            $('.boxgrid.captionfull').hover(function() {
                $(".cover", this).stop().animate({ top: '160px' }, { queue: false, duration: 160 });
            }, function() {
                $(".cover", this).stop().animate({ top: '260px' }, { queue: false, duration: 160 });
            });
            //Caption Sliding (Partially Hidden to Visible)
            $('.boxgrid.caption').hover(function() {
                $(".cover", this).stop().animate({ top: '160px' }, { queue: false, duration: 160 });
            }, function() {
                $(".cover", this).stop().animate({ top: '220px' }, { queue: false, duration: 160 });
            });

           
            $(window).scroll(function() {
                $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
            });
            
        });
		</script>
</head>
<body>
    <div id="message_box">
    <ul>
     <li style="height:36px;width:36px;">
        <a href="generador-expedientes.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
     </li>
     <li style="height:152px;width:36px;">
        <a href="generador-expedientes.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
     </li>
    </ul>
    </div>
    
    <div id="container">
        <h1 id="Titulo"><a href="#">Elegir Cobranzas - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 62px; padding:7px; width: 645px;" id ="Div2" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
        <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
           <div style="position:absolute; top:100px;left:63px; background-color:#0b4295; width: 658px;">
               <div style=" width:648px; padding:5px;">
                    <h3> <%  Response.Write(Session("ssCodimpadm")) %> </h3>
                    <font style="margin: 5px 0px 0 10px; color:#FFF; font:9pt Arial, sans-serif;font-weight: bold;"><%  Response.Write(Session("mnombcobrador")) %></font>
               </div>
               <div style="width:658px;">
                   <div class="boxgrid slideright">
				        <img class="cover" src="images/MP900285182.jpg" alt="" />
				        <h3>Cobro Individual</h3>
				        <p style="text-align:justify;"><b>Tecno Expediente</b> consultor documental te permite hacer cobro a los diferentes expedientes de un deudor o propietario determinado,  para un rápido y ágil rendimiento en la inspección de los recaudos.
				        <br/>
				        </p>
				        <p>
                        <a href="cobranzas2.aspx">Ir cobranza individual</a>
				        </p>
			        </div>
        			
			        <div class="boxgrid slideright">
				        <img class="cover" src="images/people.jpg" alt="" />
				        <h3>Cobro Masivo</h3>
				        <p style="text-align:justify;">Cualquier organización necesita controlar mejor sus procedimientos internos para mantener la competitividad y alcanzar sus objetivos, por eso es necesaria la automatización de datos grande y con esta opción a diferencia del cobro individual o casos específicos <b>Tecno Expediente</b> permite hacer cobros masivos y fiables a deudores morosos.
				        <br/>
				        </p>
				        <p>
                        <a href="cobranzasMasiva.aspx">Ir cobranza masiva</a>
				        </p>
			        </div>
			   </div>
			   <div style=" width:648px; padding:5px;">
			        <p style="margin: 5px 0px 0 10px; color:#FFF; font:9pt Arial, sans-serif;font-weight: bold;">Seleccione una opción para continuar.</p>
			   </div>
          </div>
          
          
          <div style=" width:648px; padding:5px;background:#0b4295;top:501px;left:63px;position: absolute; height: 30px; color:#FFF;">
			   <h2 style="margin:0px;">Seleccione una opción para continuar.</h2>
		  </div>
			   
          <div class="Xarticles" 
               style="position:absolute;top:567px; left:63px;width: 220px;">
            <div style="margin:0px; font-weight: bold; font-size:14px; background:#0b4295; text-transform: uppercase;padding:4px;">Desglosar  último Acto</div>
            <div  style="padding:8px; font-size:13px;">
		       <p style=" text-align:justify;margin:0px;">
		        <img src="images/icons/Settings.png" height="70" width="70" class="image" alt="" />Determinar de forma masiva y automática el último acto administrativo designado o concreto de cada expediente registrado en <b>Tecno-Expediente</b>.
		       </p>
	        </div>
	        <div style="padding:5px;background:#0b4295;"><asp:Button ID="btnEjecutarultimoacto" 
                    runat="server" Text="Ejecutar Herramienta" CssClass="Botones" 
                    style="background-image: url('images/icons/tools.png');" Width="151px"  /></div> 
          </div>
         
          <div class="Xarticles" 
               style="position:absolute;top:780px; left:63px; width: 220px;">
            <div style="margin:0px; font-weight: bold; font-size:14px; background:#0b4295; text-transform: uppercase;padding:4px;">INFORME DE PAGO</div>
            <div  style="padding:8px; font-size:13px;">
		       <p style=" text-align:justify;margin:0px;">
		        <img src="images/icons/Envelope.png" height="70" width="70" class="image" alt="" />Auditoria que permite examina los <b>Deudores</b> que pagaron en un rango de fecha especifico.
		       </p>
	        </div>
	        <div style="padding:5px;background:#0b4295;"><asp:Button ID="btnconsulta_1" 
                    runat="server" Text="Ejecutar Consulta" CssClass="Botones" 
                    style="background-image: url('images/icons/printer.png');" Width="140px"  /></div> 
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
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
        >
        </asp:ModalPopupExtender>
        </form>
    </div>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consultardocumentos2.aspx.vb" Inherits="coactivosyp.consultardocumentos2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
		<title>Consulta diaria</title>
		<link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
		<link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
		<script type="text/javascript" src="jquery-1.4.2.min.js"></script>
		<script type="text/javascript">
	        jQuery(document).ready(function($){
	            $("#btnConsultar").click(function(evento){
	                var text = $("#txtFechaRad").val();
	                if (text == "")
	                {
	                    evento.preventDefault();
	                    $("#Validator").css("visibility","visible");
	                    $("#Validator").html("No puede continuar sin haber digitado una fecha."); 
	                }
	            });
	            
		        $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		        });
	        });
        </script>
        <script type="text/javascript">
	        $(document).ready(function() {
	            $('a.Ntooltip').hover(function() {
	                $(this).find('span').stop(true, true).fadeIn("slow");
	            }, function() {
	                $(this).find('span').stop(true, true).hide("slow");
	            });
	        });
        </script>
        <style type="text/css">
            #message_box 
            {
            position: absolute;
            top:200px; left: 0;
            z-index: 10;
            background-color:#094194;
            
            background-image:url(imagenes/MenuflotaIzfondo.png);
            padding:0px;margin:0px 0px 0px;
            border:1px solid #0E0F10;
            -moz-box-shadow: 5px 5px 10px #000; /* Firefox */
            -webkit-box-shadow: 5px 5px 10px #000; /* Safari y Chrome */
            box-shadow: 5px 5px 10px #000;
            }
            
            #message_box ul {padding:0px;margin:0px;}
            #message_box li {list-style-type:none;margin:0px;padding:0px;}
            #message_box li a {margin:0px;padding:0px;text-decoration:none;display: block;}
            #message_box li a img {border: none;}
            
            
        table.servicesT
        {	
        font-family: Verdana;
        font-weight: normal;
        font-size: 11px;
        color: #404040;
        background-color: #fafafa;
        border-collapse: collapse;
        border-spacing: 0px;
        margin: 0px;}

        table.servicesT td, table.servicesT th
        {
        border: dotted 1px #6699CC;
        font-family: Verdana, sans-serif, Arial;
        font-weight: normal;
        font-size: 10px;
        color: #404040;
        background-color: white;
        text-align: left;
        padding-left: 3px;
        }
        table.servicesT td.servHd:hover  
        {
        background-color: #FFFFCC !important;
        color:#F78F08 !important; 
        border: 1px solid #6699CC;
        }
        table.servicesT td.EservEHd:hover  
        {
        background-color: #FEF4F4 !important;
        color:#F80808;
        padding-left:16px;
        cursor:pointer;
        border: 1px solid #6699CC;
        background-repeat: no-repeat;
        background-color: transparent;
        background: url('images/icons/151.png') no-repeat left 50%;
        background-position : 3px 2px;
        }
        table.servicesT th {font-weight: bold;}
        table.servicesT td a {text-decoration:none;}
        .contenedortitulos
        {
        height:18px;
        margin:0px;
        padding:6px;
        background-image: url('images/BarraActos.png');
        background-repeat: repeat-x;
        background-color:#006699;
        text-align:left;
        color:White;
        font-family:Tahoma;
        font-size:12px;
        }  
        .nombre, .docusuario, .docfechasystem
        {
        background-color:#e7fcff !important;
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
                <a href="menu2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
             </li>
            </ul>
         </div>
		
		<div id="container">
            <h1 id="Titulo"><a href="#">Consulta diaria - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
      <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 36px; padding:7px; width: 688px;" id ="Div2" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>      
		
		<form id="Form1" method="post" runat="server">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
                </asp:ToolkitScriptManager>
                
			    
			    
			    <asp:button id="btnConsultarHoy" runat="server" Text="Consultar Hoy"  
                    style="top: 248px; left: 159px; position: absolute; height: 26px; width: 119px;background-image: url('images/icons/clock2.png');" 
                    CssClass="Botones"></asp:button>
                    
                <asp:button id="btnConsultar" runat="server" Text="Consultar"  
                    style="top: 248px; left: 30px; position: absolute; height: 26px; width: 115px;background-image: url('images/icons/calendar3.png');" 
                    CssClass="Botones"></asp:button>                
                
                
			    
			    <asp:button id="btnUltimos90" runat="server" Text="Ultimos 90"  
                    style="top: 248px; left: 291px; position: absolute; height: 26px; width: 103px; background-image: url('images/icons/186.png');" 
                    CssClass="Botones"></asp:button>
                    
                
                <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtFechaRad"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
                >
                </asp:CalendarExtender>
                
                <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                        TargetControlID="txtFechaRad2"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
                >
                </asp:CalendarExtender>

			    
			    
			   
                                
                
    <div class='contenedortitulos' style=" position: absolute; TOP: 290px; LEFT: 30px; width: 716px; height:20px;background-color:#507CD1;">Consulta y examen de los expedientes digitalizados.</div>            
    <div id="contenidogrids" runat = "server"  
                    style=" position: absolute; TOP: 322px; LEFT: 30px; overflow:auto; width: 728px; height: 252px;background-color:#507CD1;">
        
        <p style='padding:10px;'>Presione un botón para inicializar una consulta.</p>
    </div> 
     										
                <asp:customvalidator id="Validator" runat="server" Font-Size="X-Small" 
                    Font-Names="Tahoma" ForeColor="White" ErrorMessage="CustomValidator" 
                    style="position:absolute; top: 262px; left: 443px; width: 312px;"></asp:customvalidator>
				
				<div style="position:absolute; top: 127px; left: 30px;color: #FFFFFF; font-size: 11px; width: 728px; height: 104px;border: 2px solid #FFFFFF;">
				    <asp:label id="Label1" runat="server" Font-Size="X-Small" Font-Names="Arial" 
                        ForeColor="White" 
                        
                        style="position:absolute; left: 32px; top: 6px;font-style: italic; font-weight: 700; font-size: medium;">Fecha Radicación</asp:label>
				    
				    <asp:TextBox ID="txtFechaRad" runat="server" 
                        style="top:56px; left:45px; position: absolute;width: 152px;" CssClass="CalendarioBox"></asp:TextBox>
    				        
                    <asp:TextBox ID="txtFechaRad2" runat="server" 
                        style="top: 56px; left: 302px; position: absolute;width: 152px" CssClass="CalendarioBox"></asp:TextBox>
                    
				    <div style="position:absolute; top: 40px; left: 353px; font-style: italic; color: #FFFFFF; font-size: 11px; height: 13px;">Hasta :</div>
                    
				    <div style="position:absolute; top: 40px; left: 99px; font-style: italic; color: #FFFFFF; font-size: 11px;">Desde :</div>
        
                    <div style="position:absolute;top:82px; left:46px; font-size: 10px; font-style: italic; font-family: Tahoma; color: #FFFFFF;">(Hacer click  en el cuadro para ingresar la fecha)</div>
                    
                     <a class="Ntooltip" href="#" id ="Ntooltip_1"  
                        
                        style="Z-INDEX: 101; POSITION: absolute; TOP: 7px; LEFT: 10px; width: 17px; height: 18px;">
                            <img src="images/icons/help.png" style="cursor:hand; cursor:pointer;" alt="" />
                       <span id ="Ntooltip_span1">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Fecha Radicación.
                        </b>
                        <br />
                          Digite el rango de fecha y luego presione el botón consultar en caso que quiera consultar un rango de fecha de lo contario presione una  consulta especifica.
                      </span>
                    </a>
				</div>
                <div style="position:absolute;top:575px; left:30px;width: 727px;" 
                    class="divhisto">
                    <table width="100%">
                        <tr>
                            <th style="text-align: right;font-size:11px;padding:4px; width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                            <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;">Ente :</th>
                        <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;" class="palanca"><asp:Label ID="lblCobrador" runat="server" Text=""></asp:Label></th>
                        </tr>
                    </table>
                </div>
		</form>
		</div>
	</body>
</html>

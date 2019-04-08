<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HistoricoDeuda.aspx.vb" Inherits="coactivosyp.HistoricoDeuda" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Actualización de expedientes</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="Libertad.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
     .CajaDialogo
     {
     position:absolute;
     background-color:#f0f0f0;
     border-width: 7px;
     border-style: solid;
     border-color: #72A3F8;
     padding: 0px;
     color:#514E4E;
     font-weight: bold;
     font-size:12px;
     font-style: italic;
     }
     
     .xws1
     {
        font-size:11px;
        font-family:Verdana;
        color:#ffffff;
     }
     .divhisto
     {
       /*border: 1px solid #dcdbe0; */
       z-index:1001; 
       background-color:#507CD1;
       border-right-width: 1px;
       border-bottom-width: 1px;
       border-right-style: solid;
       border-bottom-style: solid;
       border-right-color: #6E6E6E;
       border-bottom-color: #6E6E6E;
       overflow:auto;
     }
      .xcd
     {
        display:block;
        font-family:Tahoma;
        font-size:11px;
        padding:5px;
        text-decoration:none;
        cursor:pointer;
        text-align:center;
        color:#FFFFFF;
        background-color:#4977D3;
        border-left:10px solid #4371BF;
        border-bottom:1px solid #4371BF;
        border-top:1px solid #4371BF;
        border-right:1px solid #4371BF;
         
     }
     .xcd:hover
     {
        display:block;
        font-family:Tahoma;
        font-size:11px;
        padding:5px;
        text-decoration:none;
        text-align:center;
        color:#99CC00;
        background-color:#003366;
        border-left:10px solid #99CC00;
        border-bottom:1px solid #99CC00;
        border-top:1px solid #99CC00;
        border-right:1px solid #99CC00;
     }
     .palanca
     {
        border-left :10px solid #4977D3;
        color: #34484E;         
     }
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
    </style>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
       jQuery(document).ready(function($){
            $(window).scroll(function(){
	  		            $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		    });
        });
      </script>
</head>
<body>
    <!-- Definicion del menu -->  
    <div id="container">
    <h1 id="Titulo"><a href="#">Total detallado de la deuda</a></h1>
    <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
                EnableScriptGlobalization="True">
            </asp:ToolkitScriptManager>
            
            <!-- ejcuaciones Fiscvales -->
            <div id="ejeFiscales" runat="server" style="position:absolute;top:50px; height:105px; width: 717px; left:38px;" class="divhisto">
                <img src="images/icons/user_128.png" alt="" width="75" height="75" 
                    style="position: absolute; top: 14px; left: 12px;"  />
                
                <table class="xws1" style="position: absolute; top: 14px; left: 94px;border-collapse:collapse;">
                 <tr>
                    <td>Deudor :</td>
                    <td><asp:Label ID="ejDeudor" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                 <tr>
                    <td>Nombre :</td>
                    <td><asp:Label ID="ejdeuNombre" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                </table>
                
                <table  style="position: absolute; top: 7px; left: 585px;border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <a class ="xcd" href ="ejecucionesFiscales.aspx">Ejecuciones Fiscales</a>
                  </td>
                 </tr>
               </table>
           </div>
           
           <div style="position:absolute;top:165px; left:38px; width: 717px;" class="divhisto">
                <table width="100%">
                    <tr>
                     <th style="font-size:11px;text-align:left;padding:4px;">TOTAL DEUDA :</th>
                     <th style="text-align: right;font-size:11px;padding:4px;" class="palanca"><div id="Deudadmind" runat="server">0,0</div></th>
                    </tr>
                </table>
           </div>
           
	   <!-- CARTAGENA -->
	   <div style="position:absolute;top:209px; left:38px; width: 717px;" >
	        <div style=" font-size:15px; margin-top:3px; padding:5px; text-align:center;font-weight: bold; font-family:Verdana, Arial;color: #34484E;"  class="divhisto">DETALLES DE LA DEUDA</div>
	        <div id="contenidogrids" runat="server">
	        </div>
	        <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL DEUDA :</div> <div id ="deuda"  runat="server" style="text-align:right;float:right;"></div></div>
	        <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL INTERES :</div> <div id ="interes"  runat="server" style="text-align:right;float:right;"></div></div>
	        <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL A PAGAR :</div> <div id ="castota"  runat="server" style="text-align:right;float: right;"></div></div> 
       </div>
	  
         
	   <asp:Button id="btnAceptar" runat="server" Text="Guardar" 
            style="position:absolute;top:521px; left:38px; width: 92px; background-image: url('images/icons/46.png'); z-index:10; right: 923px;" 
            CssClass="Botones"></asp:Button>
            
          											
    </form>
 </div>
</body>
</html>

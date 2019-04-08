<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cartedeuda.aspx.vb" Inherits="coactivosyp.cartedeuda" %>
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
    <script src="js/jquery.MultiFile.js" type="text/javascript"></script>
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
    <h1 id="Titulo"><a href="#">Total detallado de la deuda</a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 49px; left: 36px; padding:7px; width: 582px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
                EnableScriptGlobalization="True">
            </asp:ToolkitScriptManager>
            
    <asp:customvalidator style="position:absolute;top:50px; left:40px; width: 580px;" 
        id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>     
    
    
    
    

            <div style="position:absolute;top:49px; background-color:#2461BF; left:636px; padding :5px 5px 5px 5px; width: 109px;">
                <table>
                 <tr>
                  <td> <asp:Label ID="Label1" runat="server" Text="Primeros" CssClass="ws2"></asp:Label></td>
                  <td> <asp:TextBox ID="txtCustos" runat="server" Width="40px" MaxLength="4">1000</asp:TextBox></td>
                 </tr>
                </table>
               
                
            </div>
            
            <div style="position:absolute;top:86px; background-color:#2461BF; left:38px; padding :10px 5px 10px 5px; width: 707px;">
                <table>
                    <tr>
                        <td>
                           <div class="ws2" >Valor mayor o igual</div>
                            <%--<asp:RadioButtonList ID="ListBuscar" runat="server" CssClass="ws2" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="Valor">Valor mayor o igual</asp:ListItem>
                                <asp:ListItem Value="ValorM">Valor menor  o igual</asp:ListItem>
                                <asp:ListItem Value="Igual">Igual a</asp:ListItem>
                            </asp:RadioButtonList>--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdato" runat="server" style="width: 110px;margin-left:9px;"></asp:TextBox>
                    
          <%--   <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                enabled="True" 
                targetcontrolid="txtEnte" 
                servicemethod="ObtListaEtidades_Est" 
                ServicePath="Servicios/Autocomplete.asmx"
                MinimumPrefixLength="1" 
                CompletionInterval="1000"
                EnableCaching="true"
                CompletionSetCount="15" 
                CompletionListCssClass="CompletionListCssClass"
                CompletionListItemCssClass="CompletionListItemCssClass"
                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
            >
            </asp:AutoCompleteExtender>--%>
           
                        </td>
                    </tr>
                </table>
                
                <asp:Button id="btnAceptar" runat="server" Text="Consultar" 
                    style="width: 92px; margin-left:5px;  background-image: url('images/icons/accept.png'); z-index:10;" 
                CssClass="Botones"></asp:Button>
                
                <asp:Button id="btnImprimir" runat="server" Text="Imprimir" 
                    style="width: 92px; margin-left:11px; margin-top:11px; background-image: url('images/icons/printer.png'); z-index:10;" 
                CssClass="Botones"></asp:Button>
            </div>
            
            <!-- ejcuaciones Fiscvales -->
           
	   <!-- CARTAGENA -->
	   <div style=" font-size:15px; padding:5px; text-align:center;font-weight: bold; font-family:Verdana, Arial;color: #34484E; width: 706px; top: 170px; left: 38px; position: absolute;"  
                class="divhisto">DETALLES DE LA DEUDA</div>
	   <div style="position:absolute;top:199px; left:38px; width: 717px;height:300px; overflow:auto;" >
	        <div style="width: 1000px" id="contenidogrids" runat="server">
	        </div>
	   </div>
       <div style="position:absolute;top:500px;left:38px;">
            <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL DEUDA :</div> <div id ="deuda"  runat="server" style="text-align:right;float:right;"></div></div>
	        <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL INTERES :</div> <div id ="interes"  runat="server" style="text-align:right;float:right;"></div></div>
	        <div style=" font-size:15px; margin-top:3px; padding:5px;width:370px; text-align:left;font-weight: bold; font-family:Verdana, Arial;"  class="divhisto"><div style="float:left;">TOTAL A PAGAR :</div> <div id ="castota"  runat="server" style="text-align:right;float: right;"></div></div> 
       </div>
	  
         
	   
            
          											
    </form>
 </div>
</body>
</html>

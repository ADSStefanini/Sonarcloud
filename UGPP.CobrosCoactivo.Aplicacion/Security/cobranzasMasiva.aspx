<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cobranzasMasiva.aspx.vb" Inherits="coactivosyp.cobranzasMasiva" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Tecno Expedientes !</title>
   <%--<link href="cssUpdateProgress.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript" language="javascript">
       var ModalProgress = '<%= ModalProgress.ClientID %>';         
   </script>--%>
   
   <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
   <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
   <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />

   <script src="../js/jquery-1.6.4.min.js" type="text/javascript"></script>
   <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
      
   <link href="css/Objetos.css" rel="stylesheet" type="text/css" />
   <style type="text/css">
        div#pas{color:#FFF;padding:5px; font:18pt Arial, sans-serif;font-weight: bold;}
        .Yup
        {
        text-align:center;
        color: #FFFFFF;
        }
        .xlist
        {
        text-align:left;
        font-family: Verdana;
        font-size: 11px;
        font-weight: bold;
        font-style: normal;
        color: #FFFFFF;
        }
        .divhisto
        {
        /*border: 1px solid #dcdbe0; */
        background-color:#507CD1;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        }
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
        .palanca
        {
        color: #34484E;         
        }
        a.Ntooltip:hover
        {
        background-color: Transparent;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
            $(window).scroll(function() {
                $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
            });
	    
	        $('a.Ntooltip').hover(function() {
	            $(this).find('span').stop(true, true).fadeIn("slow");
	        }, function() {
	            $(this).find('span').stop(true, true).hide("slow");
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
     
    <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
        <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
    </div>

   <div id="container">
        <h1 id="Titulo"><a href="#">Tecno Expedientes - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
    <form id="form1" runat="server">
      
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>  
     
   <%-- <asp:UpdatePanel ID="updatePanel" runat="server">
	  <ContentTemplate>--%>
         <div style="width:701px; background-color:#0b4295;position:absolute;top:53px; left:40px;">
                 <div id="pas">Informes </div>
         </div>    
         <div class="Yup divhisto"  style="width: 700px; position:absolute;top:92px; left:40px; background-color:#507CD1;height: 230px;">
             <div style=" width:515px; padding:5px;">
                  <asp:RadioButtonList ID="Lista" runat="server" 
                     DataSourceID="SqlDataSource1" DataTextField="nombre" 
                     DataValueField="codigo" CssClass="xlist">
                  </asp:RadioButtonList>
                 
                 <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                     SelectCommand="SELECT [codigo], [nombre] FROM [actuaciones] WHERE ([actmasivo] = @actmasivo)">
                     <SelectParameters>
                         <asp:Parameter DefaultValue="true" Name="actmasivo" Type="Boolean" />
                     </SelectParameters>
                 </asp:SqlDataSource>
             </div>
           </div>
             
           <div style="position:absolute;top:332px; left:40px; width:700px; background-color:#EDEDE9;">
               <div style=" font-size:11px;color:#fff;padding:5px;background-image: url('images/BarraActos.png'); background-repeat: repeat-x; height:18px;width:690px;">
                      <a class="Ntooltip" href="#"  style="width: 16px; height: 16px; float:left; margin-right:5px;">
                          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                          <span style="z-index:225;">
                            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                              Nota : Op. Buscar Sumas 
                            </b>
                            <br /><br />
                                Con esta opción puede buscar las sumas mayores a un valor especifico  y escatimar una búsqueda personalizada. 
                            
                            <br />
                            <br />
                            
                            <b>Ejemplo :</b> En el cuadro digite $5,000 y el resultado serán deudores con sumas mayores o iguales, tales como 
                            <ul>
                                <li>$5,000</li>
                                <li>$5,001</li>
                                <li>$5,002</li>
                                <li>$6,000</li>
                                <li>$7,000</li>
                            </ul> 
                          </span>
                      </a>
                      Detalles de la consulta 
                    </div>
                    <table style="width:100%" class="tabla">
                     <tr>
                        <th colspan="3">
                            <div style=" font-size:12px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">Digitar Valor :</div>
                        </th>
                     </tr>
                     <tr>
                         <td colspan="3">
                            <asp:TextBox ID="txtValor" runat="server" style="width: 149px" Text="0"  CausesValidation="True"></asp:TextBox>
                                 <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                                    TargetControlID="txtValor"
                                    Mask="9,999,999.99"
                                    MessageValidatorTip="true" 
                                    MaskType="Number" 
                                    InputDirection="RightToLeft" 
                                    AcceptNegative="Left" 
                                    DisplayMoney="Left"
                                    ErrorTooltipEnabled="true"
                                    ClearMaskOnLostFocus="true"
                                 >
                                 </asp:MaskedEditExtender>
                                <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server"
                                ControlExtender="MaskedEditExtender1"
                                    ControlToValidate="txtValor" 
                                    IsValidEmpty="False" 
                                    MaximumValue="9999999" 
                                    EmptyValueMessage="Se requiere un número"
                                    InvalidValueMessage="Este número no es válido"
                                    MaximumValueMessage="Número > 9,999,999.99"
                                    MinimumValueMessage="Número < -100"
                                    MinimumValue="-1000" 
                                    EmptyValueBlurredText="*" 
                                    InvalidValueBlurredMessage="*" 
                                    MaximumValueBlurredMessage="*" 
                                    MinimumValueBlurredText="*"
                                    Display="Dynamic" 
                                    TooltipMessage="Introducir un número:   -1000 hasta 9,999,999.99"
                                >
                                </asp:MaskedEditValidator>
                                
                                
                                
                         </td>
                     </tr>
                     <tr>
                      <td colspan="3">
                        <asp:CheckBox ID="chkVista" runat="server" 
                        style="width: 122px; height: 21px; font-weight:  bold; font-size: 11px; font-family: Arial, Helvetica, sans-serif; color: #666666;" 
                        Text="Vista previa " />
                      </td>
                     </tr>
                     <tr><th colspan="3">DETALLES DE LA COBRANZA</th></tr>
                     <tr><th>NUMERO TOTAL DE EXPEDIENTES :</th><td colspan="2" align="center"><div id = "totexp" runat = "server">Aun no se ha generado expediente </div></td></tr>
                     <tr><th>DESCARGAR ARCHIVO :</th><td colspan="2" align="center"><asp:LinkButton ID="LinkArchivo_expediente" runat="server">No hay documento generado</asp:LinkButton></td></tr>
                     <tr><th>DOCUMENTO :</th><td align = "center"><b style="color:Blue;"><asp:Label ID="lblcodigo" runat="server" Text="Label"></asp:Label></b></td>
                         <td><asp:Label ID="lblfechahoy" runat="server" Text="Label"></asp:Label></td>
                      </tr>
                    </table>
               </div>
			   
			   <asp:Button ID="btnImprimir" runat="server" CssClass="Botones" 
                            style="background-image: url('images/icons/add-printer.png'); position: absolute; top: 544px; left: 39px; width: 144px;" 
                            Text="Procesos en curso"  
        ValidationGroup="Xtextovalidados" />
        
        <asp:Button ID="btnImprnew" runat="server" CssClass="Botones" 
                            style="background-image: url('images/icons/printer.png'); position: absolute; top: 544px; left: 193px; width: 149px;" 
                            Text="Procesos nuevos" 
        ValidationGroup="textovalidados" />
               
               <asp:Button ID="btnSeperar" runat="server" CssClass="Botones" 
                            style="background-image: url('images/icons/186.png'); position: absolute; top: 544px; left: 591px;" 
                            Text="Separar Archivos" 
        ValidationGroup="textovalidados" Width="150px" />
               
                     
               <asp:customvalidator id="Validator"  runat="server" 
        ForeColor="Yellow" Font-Names="Arial" Font-Size="15px"  
        ErrorMessage="CustomValidator" Font-Bold="True" 
        
        style="top: 594px; left: 40px; position: absolute; height: 18px; width: 702px"></asp:customvalidator>
               <script type="text/javascript" src="jsUpdateProgress.js"></script>		
    		
		   <%--    <asp:Panel ID="panelUpdateProgress" runat="server" CssClass="updateProgress" Style="Display:none; z-index:1001;">
			        <asp:UpdateProgress ID="UpdateProg1" runat="server" DisplayAfter="0">
				        <ProgressTemplate>
					        <div style="text-align: center;">
						        <img src="44.gif" style="float:left; width:72px; height:76px;" alt="Procesando... " />
						        <div style="float:left;width:222px"><b>Procesando la informacion ...</b></div>
					        </div>
				        </ProgressTemplate>
			        </asp:UpdateProgress>
		       </asp:Panel>--%>
          
		       <%--<asp:ModalPopupExtender ID="ModalProgress" runat="server" TargetControlID="panelUpdateProgress"
			                BackgroundCssClass="modalBackground" PopupControlID="panelUpdateProgress" >
               </asp:ModalPopupExtender>--%>
                
	<%--    </ContentTemplate>
      </asp:UpdatePanel>
--%>
   
    </form>
   </div>
</body>
</html>

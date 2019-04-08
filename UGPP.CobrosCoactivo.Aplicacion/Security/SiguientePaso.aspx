<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SiguientePaso.aspx.vb" Inherits="coactivosyp.SiguientePaso" %>
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
     font-size:12px;
     }
     .CajaDialogo table {border-collapse:collapse;}
     .CajaDialogo th {text-align:left; background-color:#d4e3fc;}
     .CajaDialogo td, .CajaDialogo th {border:solid 1px #72A3F8; padding:3px;}
     .xws1
     {
        font-size:11px;
        font-family:Verdana;
        color:#ffffff;
     }
     .divhisto
     {
       /*border: 1px solid #dcdbe0; */
       z-index:1; 
       background-color:#507CD1;
       border-right-width: 1px;
       border-bottom-width: 1px;
       border-right-style: solid;
       border-bottom-style: solid;
       border-right-color: #6E6E6E;
       border-bottom-color: #6E6E6E;
     }
     .palanca
     {
        color: #fff;
        text-align:right;
     }
    </style>
<%--    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.MultiFile.js" type="text/javascript"></script>
    <script type="text/javascript">
       jQuery(document).ready(function($){
            $(window).scroll(function(){
	  		            $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		    });
        });
      </script>--%>
</head>
<body>
 <%--   <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div> --%>
     
    <div id="container">
    <h1 id="Titulo"><a href="#">Subida de expedientes al servidor y registro de los archivos en la base de datos</a></h1>
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
                 <tr>
                    <td>Predio :</td>
                    <td><asp:Label ID="ejPredio" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                 <tr>
                    <td>Expediente :</td>
                    <td><asp:Label ID="ejExpediente" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                 <tr>
                    <td>Ult. Acto :</td>
                    <td><asp:Label ID="ejutilpas" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                </table>
                
                <table  style="position: absolute; top: 7px; left: 525px;border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <a class="xcd" href="#" id = "linkHistorial" runat="server" title="Volver al historial del expediente">Cancelar</a>
                  </td>
                  <td id="ejecucionf" runat="server">
                    <a class ="xcd" href ="ejecucionesFiscales.aspx">Ejecuciones Fiscales</a>
                  </td>
                 </tr>
               </table>
           </div>
           
           <div style="position:absolute;top:165px;left:38px;width:717px;" class="divhisto">
                <table width="100%">
                    <tr>
                     <th style="font-size:11px;text-align:left;padding:4px;color:#f9ff99;">ESTA A PUNTO DE CREAR EL PASO:</th>
                     <th style="font-size:11px;padding:4px;" class="palanca"><div id="ActoAdmind" runat="server">ACTO</div></th>
                    </tr>
                </table>
           </div>
           
           <asp:customvalidator style="position:absolute;top:570px; left:38px; width: 717px;" 
            id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator> 
		   <!-- CARTAGENA -->
		   <div style="position:absolute;top:214px; left:38px; width: 717px; height:297px;" class="divhisto">
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtFechaRad"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
                    >
                    </asp:CalendarExtender> 

                    <asp:label id="Label3" runat="server" ForeColor="White" Font-Names="Arial" 
                     Font-Size="12px" style="position:absolute;top:24px; left:16px">Numero de Paginas :</asp:label> 

                    <asp:TextBox ID="txtnroPaginas" runat="server" 
                    style="position:absolute;top:20px; left:156px; width: 48px;"></asp:TextBox> 
               
                    <asp:label id="lblresolucion_ok" runat="server" ForeColor="White" Font-Names="Arial" 
                     style="position:absolute;top:22px;left:562px;font-size:x-small;">...</asp:label> 
                     
                    <asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" 
                     Font-Size="12px" style="position:absolute;top:24px;left:306px">Resolucion :</asp:label> 
                     
                    <asp:TextBox ID="txtResolucion" runat="server"  Width="90"  
                        style="position:absolute;top:20px;width:90px;left:460px;text-align:center;background-color:Transparent;border:1px solid #304a7d;color:#fff" ReadOnly="True"></asp:TextBox>
                                         
                    <asp:CalendarExtender ID="txtFechacreacion_CalendarExtender" runat="server"
                        TargetControlID="txtFechacreacion"
                        Format="dd/MM/yyyy"
                        TodaysDateFormat="dd, MMMM, yyyy"
                    >
                    </asp:CalendarExtender>

                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                        TargetControlID="txtFechacreacion"
                        Format="dd/MM/yyyy"
                        TodaysDateFormat="dd, MMMM, yyyy"
                    >
                    </asp:CalendarExtender>

                    <asp:TextBox ID="txtFechaRad" runat="server"
                    style="position:absolute;top:50px; left:156px; width:110px" 
                    CssClass="CalendarioBox"></asp:TextBox>
                    
                    <asp:TextBox ID="txtFechacreacion" runat="server"  Width="110"
                    style="position:absolute;top:50px; right:147px; width:110px; left: 460px;" 
                    CssClass="CalendarioBox"></asp:TextBox>
                    
                    <div style="position:absolute;top:78px; left:157px; font-size: 10px; font-style: italic; font-family: Tahoma; color: #FFFFFF;">(Hacer click  en el cuadro para ingresar la fecha)</div>
                    <asp:label style="position:absolute;top:53px; left:306px" id="Label14" 
                     runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Fecha del  Documento :</asp:label>
                     
                    <asp:label style="position:absolute;top:102px; left:16px" id="Label15" 
                     runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Seleccione Imagen :</asp:label>
                     
                    <asp:label style="position:absolute;top:54px; left:16px" id="Label13" 
                     runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Fecha de radicación :</asp:label>
                     
		            <input id="imagen1" type="file"  runat="server" style="position: absolute; top: 97px; left: 156px; width: 235px; height: 22px;" />
		                                
                    <asp:TextBox ID="txtObservaciones" runat="server" BackColor="White" 
                        BorderColor="Silver" BorderStyle="Solid" ForeColor="Black"                       
                        style="position:absolute;top:177px;left:17px;width:681px;height:102px;" 
                        TextMode="MultiLine"></asp:TextBox>
                    
                    <asp:Label ID="Label16" runat="server" 
                        style="position: absolute; top: 152px; left: 17px;bottom: 125px; color: #FFFFFF; font-size: 14px; font-weight: 700; font-family: Arial, Helvetica, sans-serif; " 
                        Text="Observaciones :"></asp:Label>
                     
		 </div>
		 
	     <asp:RequiredFieldValidator ID="rfvtxtResolucion" runat="server"  ErrorMessage="El campo <strong>Resolución.</strong> Debe generar una resolución previamente para continuar. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtResolucion" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" TargetControlID="rfvtxtResolucion">
         </asp:ValidatorCalloutExtender>
         		 
		 <asp:RequiredFieldValidator ID="rfvtxtnroPaginas" runat="server"  ErrorMessage="El campo <strong>PAGINAS</strong> es requerido para la operación. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtnroPaginas" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvtxtnroPaginas">
         </asp:ValidatorCalloutExtender>
         
         <asp:RequiredFieldValidator ID="rfvtxtFechaRad" runat="server"  ErrorMessage="El campo <strong>FECHA RADICACION</strong> es requerido para la operación. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtFechaRad" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="rfvtxtFechaRad">
         </asp:ValidatorCalloutExtender>
         
         <asp:RequiredFieldValidator ID="rfvtxtFechacreacion" runat="server"  ErrorMessage="El campo <strong>FECHA DOCUMENTO</strong> es requerido para la operación. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtFechacreacion" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="rfvtxtFechacreacion">
         </asp:ValidatorCalloutExtender>
         
         <asp:RequiredFieldValidator ID="rfvtxtObservaciones" runat="server"  ErrorMessage="El campo <strong>OBSERVACIONES</strong> es requerido para la operación. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtObservaciones" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="rfvtxtObservaciones">
         </asp:ValidatorCalloutExtender>
         
          <asp:RequiredFieldValidator ID="rfvimagen1" runat="server"  ErrorMessage="El campo <strong>IMAGEN</strong> es requerido para la operación. Verifique" ValidationGroup="textovalidados" ControlToValidate="imagen1" Display="None"></asp:RequiredFieldValidator>
         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="rfvimagen1">
         </asp:ValidatorCalloutExtender>
         
	   <asp:Button id="btnAceptar" runat="server" Text="Guardar" 
            style="position:absolute;top:521px; left:38px; width: 92px; background-image: url('images/icons/46.png'); z-index:10; right: 923px;" 
            CssClass="Botones" ValidationGroup="textovalidados"></asp:Button>
            
       <asp:Button id="btnVolver" runat="server" Text="Volver al Historial" 
            style="position:absolute;top:521px; left:140px;width:130px; background-image: url('images/icons/arrow-skip.png'); z-index:10; right: 923px;" 
            CssClass="Botones"></asp:Button>
            
       <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="position:absolute;width:341px;top:750px;padding:7px;display:none;">
          <div></div>
          <div style=" padding:5px 0 5px 0; text-align:center;font-size:xx-small;">
             Documento guardado con éxito.
          </div>
		  <div>
		        <table width="100%"> 
		            <tr>
		                <th colspan="2" style="text-align:center;">Archivo</th>
		            </tr>
		            <tr>
		                <td colspan="2" style="text-align:center;font-size:x-small;"><span id="archivoGenerado" runat="server"></span></td>
		            </tr>
		            <tr>
		                <th>Fecha Radicación:</th>
		                <td><script type="text/jscript">document.write(document.getElementById('txtFechaRad').value);</script></td>
		            </tr>
		            <tr>
		                <th>Fecha Documento:</th>
		                <td><script type="text/jscript">document.write(document.getElementById('txtFechacreacion').value);</script></td>
		            </tr>
		            <tr>
		                <th>Resolución:</th>
		                <td><script type="text/jscript">document.write(document.getElementById('txtResolucion').value);</script></td>
		            </tr>
		            <tr>
		                <td colspan="2" style="text-align:center;font-size:x-small;">
		                <script type="text/jscript">
		                    document.write(document.getElementById('ActoAdmind').innerHTML);
		                </script></td>
		            </tr>
		        </table>
		  </div>
		  <br />
		  <p style="font-size:xx-small;">Hacer click en el botón "si" Para continuar</p>		
		  <asp:Button style="Z-INDEX:116;width: 50px;background-image: url('images/icons/okay.png')" id="btnSi"
				runat="server" Text="Si" CssClass="Botones"></asp:Button>   
       </asp:Panel>
       
       <asp:Button ID="Button1" runat="server" Text="Button" style="visibility:hidden" />
       <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
            TargetControlID="Button1"
            PopupControlID="pnlSeleccionarDatos"
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
       >
       </asp:ModalPopupExtender>
    </form>
 </div>
</body>
</html>

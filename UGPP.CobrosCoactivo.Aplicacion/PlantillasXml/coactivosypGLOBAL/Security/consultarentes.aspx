<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="consultarentes.aspx.vb" Inherits="coactivosyp.consultarentes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
		<title>Consulta de documentos digitalizados de los entes</title>
		<!--<script src="event.js" type="text/javascript"></script> -->
		<link href="coactivosyp.css" type="text/css" rel="stylesheet" />
		<link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
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
            .Botones /* Estilo de los botones */
            {
            height: 24px; 
            background-color: #8DAFF2; border-bottom: 1px solid #555555;
            border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 12px;
            color: #000; 
            padding-left: 20px; background-repeat: no-repeat; cursor:hand; cursor:pointer;
            outline-width:0px;
            background-position: 4px 4px;outline-width:0px;
            }
            .Botones:hover /* Efectos del Mouse en los botones */
            {
            height: 24px; background-color: #D5E2FE; border-bottom: 1px solid #000;
            border-right:1px solid #000; border-top:0px; border-left:0px;
            font-size:12px; color:#000;padding-left: 20px; background-repeat: no-repeat;
            cursor:hand; cursor:pointer;outline-width:0px;
            }
		</style>
		<link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
		<script type="text/javascript" src="jquery-1.4.2.min.js"></script>
		<script type="text/javascript">
	        jQuery(document).ready(function($){
	            $("#btnConsultar").click(function(evento){
	                var text = $("#txtEnte").val();
	                if (text == "")
	                {
	                    evento.preventDefault();
	                    $("#Validator").css("visibility","visible");
	                    $("#Validator").html("Digite una entidad para continuar."); 
	                }
	            });
	            
		        $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		        });
	        });
        </script>

        <link href="css/message_box.css" rel="stylesheet" type="text/css" />
     
	</head>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" onload="document.forms.Form1.txtEnte.focus()"
		marginwidth="0" marginheight="0">
		
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
              						
		<form id="Form1" method="post" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
            EnableScriptGlobalization="True">
            </asp:ToolkitScriptManager>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0" id="container">
			
             <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
            enabled="True" 
            targetcontrolid="txtEnte" 
            servicemethod="ObtListaEtidades" 
            ServicePath="Servicios/Autocomplete.asmx"
            MinimumPrefixLength="1" 
            CompletionInterval="1000"
            EnableCaching="true"
            CompletionSetCount="15" 
            CompletionListCssClass="CompletionListCssClass"
            CompletionListItemCssClass="CompletionListItemCssClass"
            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
            >

            </asp:AutoCompleteExtender>
			
			
				<tr>
					<td width="50%"></td>
					<td background="images/bg_izdo.jpg"><IMG src="images/bg_izdo.jpg" width="32"></td>
					<td vAlign="top" width="780" bgColor="#618ce4" height="100%">
						<!-- Tabla del centro del diseño -->
						<table height="100%" cellSpacing="0" cellPadding="0" width="780" border="0">
							<!-- segunda fila de la tabla central tiene una sola celda (resultados_busca.jpg)-->
							<tr>
								<td width="780" background="images/resultados_busca.jpg" height="42"><font style="FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #ffffff; FONT-FAMILY: verdana">&nbsp; 
										Consulta de documentos asociados a los entes </font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td vAlign="middle" align="center" width="780">
									<DIV ms_positioning="GridLayout">
										<TABLE height="870" cellSpacing="0" cellPadding="0" width="747" border="0" ms_2d_layout="TRUE">
											<TR vAlign="top">
												<TD width="32" height="8"></TD>
												<TD width="56"></TD>
												<TD width="224"></TD>
												<TD width="296"></TD>
												<TD width="24"></TD>
												<TD width="115"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="36"></TD>
												<TD colSpan="2"><asp:hyperlink id="HyperLink4" runat="server" NavigateUrl="consultarentes.aspx" Font-Size="X-Small"
														Font-Names="Verdana" ForeColor="White">Consultar expedientes</asp:hyperlink></TD>
												<TD>
													<%
														If Session("mnivelacces") = "1" Then															
															Response.Write("<a id=""Hyperlink1"" href=""subirexpedientes.aspx"" style=""color:White;font-family:Verdana;font-size:X-Small;"">Actualizar expedientes</a>")
														End If
													%>
												</TD>
												<TD colSpan="2">
													<%
														If Session("mnivelacces") = "1" Then															
															Response.Write("<a id=""Hyperlink2"" href=""consultardocumentos2.aspx"" style=""color:White;font-family:Verdana;font-size:X-Small;"">Consulta diaria</a>")
														End If
													%>
												</TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="5" height="1"></TD>
												<TD rowSpan="3"><asp:button id="btnConsultar" runat="server" Text="Consultar" 
                                                        style ="background-image: url('images/icons/arrow-skip.png');" CssClass="Botones"></asp:button></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD colSpan="3" rowspan="2"><asp:TextBox ID="txtEnte" runat="server" style="width: 544px;z-index:1001"></asp:TextBox></TD>
											</TR>
											<TR vAlign="top">
												<TD height="32"></TD>
												<TD><asp:label id="Label1" runat="server" Font-Size="X-Small" Font-Names="Arial" ForeColor="White">Deudor</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD height="24"></TD>
												<TD colSpan="2"><asp:customvalidator id="Validator" runat="server" Font-Size="12px" 
                                                        Font-Names="Tahoma" ForeColor="#8A0808"
														ErrorMessage="CustomValidator"></asp:customvalidator></TD>
												<TD colSpan="3"></TD>
											</TR>
											<TR vAlign="top">
												<td colspan="6">
													<div id="contenidogrids" runat="server" ms_positioning="GridLayout">
														<table height="116" cellspacing="0" cellpadding="0" width="700" border="0" ms_2d_layout="TRUE">
															<tr valign="top">
																<td width="700" height="116"></td>
															</tr>
														</table>
													</div>
												</td>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<!-- fin de la tabla central --></table>
					</td>
					<td background="images/bg_dcho.jpg"><IMG src="images/bg_dcho.jpg" width="32">
                    </td>
					<td width="50%">&nbsp;</td>
				</tr>
			</table>
        
		    <!--<div class ="CajaDialogo" style="Height:231px; Width:358px; top: 582px; left: 0px; position: absolute;">-->
		       <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" 
                    style="height: 221px; width: 358px; top: 132px; left: 437px; z-index:1001" Visible="False"> 
                    
                    <asp:Button style="Z-INDEX: 116;width: 93px; top: 187px; left: 108px; position: absolute; right: 157px; background-image: url('images/icons/cancel.png');" id="btnNo"
                        runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
                    	
                    <asp:Button style="Z-INDEX: 116;width: 84px; top: 187px; left: 16px; position: absolute; right: 264px;background-image: url('images/icons/okay.png');" id="btnSi"
                        runat="server" Text="Enviar" Height="23px" CssClass="Botones"></asp:Button>
                    
                    <asp:ListBox ID="ListExpedientes" runat="server" 
                        style="top: 30px; left: 109px; position: absolute; height: 131px; width: 223px">
                    </asp:ListBox>
                           
                    <asp:Label ID="Label2" runat="server" Text="Expedientes" 
                         style="top: 12px; left: 108px; position: absolute; height: 15px; width: 220px"></asp:Label>
                    
                    <asp:Label ID="Label3" runat="server" 
                        style="top: 166px; left: 17px; position: absolute; height: 6px; width: 216px" 
                        Text="Se detecto más de un expediente."></asp:Label>
                        
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Menu_PPal/actualizar.png" 
                        style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
                        
                 </asp:Panel> 
            <!--</div>-->
        
		</form>
	</body>
</html>

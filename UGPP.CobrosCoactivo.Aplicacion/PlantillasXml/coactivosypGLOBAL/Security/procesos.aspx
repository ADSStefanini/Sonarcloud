<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="procesos.aspx.vb" Inherits="coactivosyp.procesos" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Consulta de documentos digitalizados de los entes</title>
		<meta content="JavaScript" name="vs_defaultClientScript">
		<LINK href="coactivosyp.css" type="text/css" rel="stylesheet">
			<script src="datepickercontrol.js" type="text/javascript"></script>
			<LINK href="datepickercontrol.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			&lt;
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
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
										<TABLE height="1030" cellSpacing="0" cellPadding="0" width="747" border="0" ms_2d_layout="TRUE">
											<TR vAlign="top">
												<TD width="32" height="24"></TD>
												<TD width="200"></TD>
												<TD width="128"></TD>
												<TD width="24"></TD>
												<TD width="363"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD colSpan="3" rowSpan="2"><INPUT id="txtEnte" type="text" size="66" name="txtEnte" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Entidad</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtProceso" type="text" size="14" name="txtProceso" runat="server"></TD>
												<TD colSpan="2" rowSpan="23"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. proceso</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtNumResol" type="text" size="14" name="txtNumResol" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label3" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. resolución</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFechaResol" type="text" size="14" name="txtFechaResol" runat="server" datepicker="true"
														datepicker_format="DD/MM/YYYY"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label4" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha resolución</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtValorDeuda" type="text" size="14" name="txtValorDeuda" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label5" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Valor deuda</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFechaMandamiento" type="text" size="14" name="txtFechaMandamiento" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label6" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha mandamiento</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtValMandam" type="text" size="14" name="txtValMandam" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label7" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Valor mandamiento</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtNumOficio" type="text" size="9" name="txtNumOficio" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label8" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. Oficio</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtEmpCorreo" type="text" size="14" name="txtEmpCorreo" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label9" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Empresa de correo</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtNumGuia" type="text" size="14" name="txtNumGuia" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label10" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. guía</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFechaEnvio" type="text" size="14" name="txtFechaEnvio" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label11" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de envío</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecRecED" type="text" size="14" name="txtFecRecED" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label12" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha recibido entidad deudora</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="FecNotif1" type="text" size="14" name="FecNotif1" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label13" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha para notificarse</asp:label></TD>
												<TD colSpan="2" rowSpan="5"><asp:label id="Label14" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">(15 días desde el recibido)</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="FecNotif2" type="text" size="14" name="FecNotif2" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label15" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha notificación</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="FecNotif3" type="text" size="14" name="FecNotif3" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label16" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha notificación por aviso</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD height="24"></TD>
												<TD><asp:label id="Label17" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Empresa de correo</asp:label></TD>
												<TD colSpan="3"><INPUT id="txtEmpCorreo2" type="text" size="14" name="txtEmpCorreo2" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="24"></TD>
												<TD><asp:label id="Label18" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. guía</asp:label></TD>
												<TD colSpan="3"><INPUT id="txtNumGuia2" type="text" size="14" name="txtNumGuia2" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="24"></TD>
												<TD><asp:label id="Label19" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de envío</asp:label></TD>
												<TD colSpan="3"><INPUT id="txtFecEnvio" type="text" size="14" name="txtFecEnvio" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="24"></TD>
												<TD><asp:label id="Label20" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha recibido entidad deudora</asp:label></TD>
												<TD colSpan="3"><INPUT id="txtFecRecED2" type="text" size="14" name="txtFecRecED2" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecNotif" type="text" size="14" name="txtFecNotif" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
												<TD colSpan="2"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label21" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha para notificarse</asp:label></TD>
												<TD colSpan="2" rowSpan="2"><asp:label id="Label22" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">(3 días desde el recibido)</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecNotif2" type="text" size="14" name="txtFecNotif2" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label23" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha notificación</asp:label></TD>
												<TD colSpan="2" rowSpan="12"><asp:label id="Label24" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">(Por conducta concluyente)</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="FecFijEstado" type="text" size="14" name="FecFijEstado" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label25" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtEmpCorreo3" type="text" size="14" name="txtEmpCorreo3" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label26" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Empresa de correo</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtNumGuia3" type="text" size="14" name="txtNumGuia3" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label27" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">No. guía</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecEnvio2" type="text" size="14" name="txtFecEnvio2" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label28" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de envío</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecRecED3" type="text" size="14" name="txtFecRecED3" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label29" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha recibido entidad deudora</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtVenciTerm" type="text" size="14" name="txtVenciTerm" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label30" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Vencimiento término</asp:label></TD>
												<TD colSpan="2" rowSpan="4"><asp:label id="Label31" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Para presentar excepciones (10 dias desde la notificación)</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecPresExcep" type="text" size="14" name="txtFecPresExcep" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label32" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha presentación excepciones</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecAuto" type="text" size="14" name="txtFecAuto" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label33" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha del auto</asp:label></TD>
												<TD colSpan="2" rowSpan="4"><asp:label id="Label34" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">mediante el cual se resuelven las excepciones</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst" type="text" size="14" name="txtFijEst" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label35" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecAuto2" type="text" size="14" name="txtFecAuto2" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label36" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha del auto </asp:label></TD>
												<TD colSpan="2" rowSpan="4"><asp:label id="Label46" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">mediante el cual se resuelve recurso de reposición</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst2" type="text" size="14" name="txtFijEst2" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label37" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecAuto3" type="text" size="14" name="txtFecAuto3" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label38" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha del auto</asp:label></TD>
												<TD colSpan="2" rowSpan="8"><asp:label id="Label47" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">mediante el cual se resuelve incidente de nulidad</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst3" type="text" size="14" name="txtFijEst3" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label39" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecSentencia" type="text" size="14" name="txtFecSentencia" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label40" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de la Sentencia</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst4" type="text" size="14" name="txtFijEst4" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label41" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecAutoLiq" type="text" size="14" name="txtFecAutoLiq" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label42" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha de Auto Liquidación</asp:label></TD>
												<TD colSpan="2" rowSpan="4"><asp:label id="Label48" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Crédito y Costas</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst5" type="text" size="14" name="txtFijEst5" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label43" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFecAutoAprob" type="text" size="14" name="txtFecAutoAprob" datepicker="true"
														datepicker_format="DD/MM/YYYY" runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label44" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha Auto de Aprobación</asp:label></TD>
												<TD colSpan="2" rowSpan="3"><asp:label id="Label49" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">de Liquidación Crédito y Costas</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFijEst6" type="text" size="14" name="txtFijEst6" datepicker="true" datepicker_format="DD/MM/YYYY"
														runat="server"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="29"></TD>
												<TD><asp:label id="Label45" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">Fecha fijación por estado</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="4" height="38"></TD>
												<TD><asp:button id="Button1" runat="server" Text="Aceptar"></asp:button></TD>
											</TR>
										</TABLE>
									</DIV>
								</td>
							</tr>
							<!-- fin de la tabla central --></table>
					</td>
					<td background="images/bg_dcho.jpg"><IMG src="images/bg_dcho.jpg" width="32"></td>
					<td width="50%"></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

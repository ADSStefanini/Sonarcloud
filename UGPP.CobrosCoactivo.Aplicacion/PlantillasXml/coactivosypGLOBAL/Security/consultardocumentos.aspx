<%@ Page Language="vb" AutoEventWireup="false" Codebehind="consultardocumentos.aspx.vb" Inherits="coactivosyp.consultardocumentos" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.OleDb"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Consulta de documentos digitalizados de los entes</title>
		<link rel="stylesheet" href="coactivosyp.css" type="text/css">
			<script type="text/javascript" src="datepickercontrol.js"></script>
			<link type="text/css" rel="stylesheet" href="datepickercontrol.css">
	</HEAD>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" onload="document.forms.Form1.txtFechaRad.focus()"
		marginwidth="0" marginheight="0">
		<form id="Form1" method="post" runat="server">
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
										<TABLE height="476" cellSpacing="0" cellPadding="0" width="747" border="0" ms_2d_layout="TRUE">
											<TR vAlign="top">
												<TD width="32" height="24"></TD>
												<TD width="128"></TD>
												<TD width="192"></TD>
												<TD width="395"></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="2" height="3"></TD>
												<TD rowSpan="2"><INPUT id="txtFechaRad" type="text" size="14" name="txtEnte" runat="server"></TD>
												<TD rowSpan="2"><asp:button id="Button1" runat="server" Text="Consultar"></asp:button></TD>
											</TR>
											<TR vAlign="top">
												<TD height="21"></TD>
												<TD><asp:label id="Label1" runat="server" Font-Size="X-Small" Font-Names="Arial" ForeColor="White">Fecha Radicación</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD height="32"></TD>
												<TD colSpan="3"><asp:customvalidator id="Validator" runat="server" Font-Size="X-Small" Font-Names="Tahoma" ForeColor="White"
														ErrorMessage="CustomValidator"></asp:customvalidator></TD>
											</TR>
											<TR vAlign="top">
												<TD height="396"></TD>
												<TD colSpan="3">
													<DIV id="contenidogrids" runat="server" ms_positioning="GridLayout">
														<TABLE height="116" cellSpacing="0" cellPadding="0" width="680" border="0" ms_2d_layout="TRUE">
															<TR vAlign="top">
																<TD width="680" height="116"></TD>
															</TR>
														</TABLE>
													</DIV>
												</TD>
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

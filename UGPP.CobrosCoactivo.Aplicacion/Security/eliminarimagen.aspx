<%@ Page CodeBehind="eliminarimagen.aspx.vb" Language="vb" AutoEventWireup="false" Inherits="coactivosyp.eliminarimagen" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Consulta de documentos digitalizados de los entes</title>
	</HEAD>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
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
										Subida de expedientes al servidor y registro de los archivos en la base de 
										datos </font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td vAlign="middle" align="center" width="780">
									<DIV ms_positioning="GridLayout">
										<TABLE height="564" cellSpacing="0" cellPadding="0" width="747" border="0" ms_2d_layout="TRUE">
											<TR vAlign="top">
												<TD width="32" height="8"></TD>
												<TD width="176"></TD>
												<TD width="28"></TD>
												<TD width="12"></TD>
												<TD width="152"></TD>
												<TD width="347"></TD>
											</TR>
											<TR vAlign="top">
												<TD height="88"></TD>
												<TD><asp:hyperlink id="HyperLink4" runat="server" NavigateUrl="consultarentes.aspx" ForeColor="White"
														Font-Names="Verdana" Font-Size="X-Small">Consultar expedientes</asp:hyperlink></TD>
												<TD colSpan="3"><asp:hyperlink id="HyperLink5" runat="server" NavigateUrl="subirexpedientes.aspx" ForeColor="White"
														Font-Names="Verdana" Font-Size="X-Small">Actualizar expedientes</asp:hyperlink></TD>
												<TD><asp:hyperlink id="HyperLink6" runat="server" NavigateUrl="consultardocumentos2.aspx" ForeColor="White"
														Font-Names="Verdana" Font-Size="X-Small">Consulta diaria</asp:hyperlink></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="3" height="48"></TD>
												<TD colSpan="3"><asp:label id="Label13" runat="server" ForeColor="White" Font-Names="Arial" Font-Size="X-Small">¿Confirma que desea eliminar la imagen?</asp:label></TD>
											</TR>
											<TR vAlign="top">
												<TD colSpan="4" height="420"></TD>
												<TD colSpan="2"><asp:button id="Button2" runat="server" Text="Aceptar" Width="216px"></asp:button></TD>
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

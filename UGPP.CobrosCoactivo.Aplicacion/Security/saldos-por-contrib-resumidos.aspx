<%@ Page Language="vb" AutoEventWireup="false" Codebehind="saldos-por-contrib-resumidos.aspx.vb" Inherits="coactivosyp.saldos_por_contrib_resumidos"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Saldos por Contribuyente resumidos</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
								<td width="780" background="images/resultados_busca.jpg" height="42">
									<font style="FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #ffffff; FONT-FAMILY: verdana">
										&nbsp; Saldos por Contribuyente resumidos </font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td width="780">
									<DIV style="WIDTH: 752px; POSITION: relative; HEIGHT: 570px" ms_positioning="GridLayout">&nbsp;
										<asp:label id="Label2" style="Z-INDEX: 103; LEFT: 40px; POSITION: absolute; TOP: 83px" runat="server"
											ForeColor="White" Font-Size="X-Small" Font-Names="Arial">Grupo (Apellidos y [Nombre])</asp:label>
										<asp:customvalidator id="Validator1" style="Z-INDEX: 108; LEFT: 40px; POSITION: absolute; TOP: 120px"
											runat="server" ForeColor="DarkRed" Font-Size="12px" Font-Names="Arial" Width="672px" ErrorMessage="Validator1"></asp:customvalidator><INPUT id="txtRefeCata" style="Z-INDEX: 109; LEFT: 232px; POSITION: absolute; TOP: 80px"
											type="text" name="txtRefeCata" runat="server">
										<asp:button id="Button1" style="Z-INDEX: 110; LEFT: 40px; POSITION: absolute; TOP: 160px" runat="server"
											Text="Imprimir"></asp:button></DIV>
								</td>
							</tr>
						</table>
						<!-- fin de la tabla central -->
					</td>
					<td background="images/bg_dcho.jpg"><IMG src="images/bg_dcho.jpg" width="32"></td>
					<td width="50%"></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>

<%@ Page Language="vb" AutoEventWireup="false" Codebehind="archivos-planos.aspx.vb" Inherits="coactivosyp.archivos_planos"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>cobranzas</title>
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
										&nbsp; Generación de archivos planos</font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td width="780">
									<DIV style="WIDTH: 752px; POSITION: relative; HEIGHT: 492px" ms_positioning="GridLayout">&nbsp;
										<asp:Label id="Label3" style="Z-INDEX: 105; LEFT: 40px; POSITION: absolute; TOP: 80px" runat="server"
											Font-Names="Arial" Font-Size="X-Small" ForeColor="White">Interface de</asp:Label>
										<asp:RadioButtonList id="RadioButtonList1" style="Z-INDEX: 106; LEFT: 160px; POSITION: absolute; TOP: 96px"
											runat="server" Width="440px" Font-Names="Arial" Font-Size="X-Small" ForeColor="White" BorderStyle="Dashed"
											BorderWidth="1px">
											<asp:ListItem Value="Auto de Mandamiento de Pago">Predios</asp:ListItem>
											<asp:ListItem Value="Citaci&#243;n para notificaci&#243;n personal de Mandamiento de Pago">Novedades</asp:ListItem>
											<asp:ListItem Value="Registro de predios">Registro de predios</asp:ListItem>
											<asp:ListItem Value="Oficio de desembargo bancario">Pagos</asp:ListItem>
											<asp:ListItem Value="Resoluci&#243;n de desembargo">Saldos por Ref. catastral</asp:ListItem>
										</asp:RadioButtonList>
										<asp:Button id="Button1" style="Z-INDEX: 107; LEFT: 40px; POSITION: absolute; TOP: 336px" runat="server"
											Text="Generar"></asp:Button></DIV>
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

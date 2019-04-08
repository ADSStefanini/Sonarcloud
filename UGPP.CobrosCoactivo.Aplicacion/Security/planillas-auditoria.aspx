<%@ Page Language="vb" AutoEventWireup="false" Codebehind="planillas-auditoria.aspx.vb" Inherits="coactivosyp.planillas_auditoria"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<head>
		<title>Planillas de auditoría</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0">
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
										&nbsp; Análisis contribuyente por cobrar</font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td width="780">
									<DIV style="WIDTH: 752px; POSITION: relative; HEIGHT: 560px" ms_positioning="GridLayout">
										<asp:Label id="Label1" style="Z-INDEX: 101; LEFT: 40px; POSITION: absolute; TOP: 32px" runat="server"
											Font-Names="Arial" Font-Size="X-Small" ForeColor="White">Fecha</asp:Label><INPUT style="Z-INDEX: 102; LEFT: 168px; POSITION: absolute; TOP: 32px" type="text">&nbsp;
										<asp:Button id="Button1" style="Z-INDEX: 107; LEFT: 40px; POSITION: absolute; TOP: 80px" runat="server"
											Text="Imprimir" Width="88px"></asp:Button></DIV>
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

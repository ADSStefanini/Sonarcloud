<%@ Page Language="vb" AutoEventWireup="false" Codebehind="analisis-contrib-por-cobrar.aspx.vb" Inherits="coactivosyp.analisis_contrib_por_cobrar"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>Análisis contribuyente por cobrar</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
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
											Font-Names="Arial" Font-Size="X-Small" ForeColor="White">Valor >=</asp:Label>&nbsp;
										<asp:Button id="Button1" style="Z-INDEX: 107; LEFT: 40px; POSITION: absolute; TOP: 96px" runat="server"
											Text="Imprimir" Width="88px"></asp:Button><INPUT id="txtRefeCata" style="Z-INDEX: 109; LEFT: 112px; POSITION: absolute; TOP: 29px"
											type="text" name="txtRefeCata" runat="server">
										<asp:customvalidator id="Validator1" style="Z-INDEX: 110; LEFT: 40px; POSITION: absolute; TOP: 64px"
											runat="server" ForeColor="DarkRed" Font-Size="12px" Font-Names="Arial" Width="672px" ErrorMessage="Validator1"></asp:customvalidator></DIV>
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

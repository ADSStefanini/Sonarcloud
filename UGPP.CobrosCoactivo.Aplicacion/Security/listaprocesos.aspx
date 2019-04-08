<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="listaprocesos.aspx.vb" Inherits="coactivosyp.listaprocesos" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Consulta de documentos digitalizados de los entes</title>
		<meta content="JavaScript" name="vs_defaultClientScript">
		<LINK href="coactivosyp.css" type="text/css" rel="stylesheet">
			<script src="datepickercontrol.js" type="text/javascript"></script>
			<LINK href="datepickercontrol.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" method="post" runat="server">
			<DIV ms_positioning="GridLayout">
				<TABLE height="546" cellSpacing="0" cellPadding="0" width="1208" border="0" ms_2d_layout="TRUE">
					<TR vAlign="top">
						<TD width="24" height="32"></TD>
						<TD width="1184"></TD>
					</TR>
					<TR vAlign="top">
						<TD height="514"></TD>
						<TD>
							<asp:DataGrid id="DataGrid1" runat="server" AutoGenerateColumns="False" BorderColor="#CCCCCC"
								BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3">
								<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#669999"></SelectedItemStyle>
								<ItemStyle Font-Size="11px" Font-Names="Verdana" ForeColor="#000066"></ItemStyle>
								<HeaderStyle Font-Size="11px" Font-Names="Arial" Font-Bold="True" ForeColor="White" BackColor="#006699"></HeaderStyle>
								<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="entidad" HeaderText="Id Entidad"></asp:BoundColumn>
									<asp:BoundColumn DataField="numproceso" HeaderText="No. proceso"></asp:BoundColumn>
									<asp:BoundColumn DataField="numresol" HeaderText="No. resol."></asp:BoundColumn>
									<asp:BoundColumn DataField="fecharesol" HeaderText="Fecha Resol."></asp:BoundColumn>
									<asp:BoundColumn DataField="valorresol" HeaderText="Valor"></asp:BoundColumn>
									<asp:BoundColumn DataField="fechamanda" HeaderText="Fecha M. Pago"></asp:BoundColumn>
									<asp:BoundColumn DataField="numoficio" HeaderText="Num Oficio"></asp:BoundColumn>
									<asp:BoundColumn DataField="empcorreo1" HeaderText="Emp. Correo"></asp:BoundColumn>
									<asp:BoundColumn DataField="numguia1" HeaderText="Num Gu&#237;a"></asp:BoundColumn>
									<asp:BoundColumn DataField="fecenvio1" HeaderText="Fecha env&#237;o"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Left" ForeColor="#000066" BackColor="White" Mode="NumericPages"></PagerStyle>
							</asp:DataGrid></TD>
					</TR>
				</TABLE>
			</DIV>
		</form>
	</body>
</HTML>

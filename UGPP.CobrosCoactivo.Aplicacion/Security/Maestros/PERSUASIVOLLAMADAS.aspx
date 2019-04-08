<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PERSUASIVOLLAMADAS.aspx.vb" Inherits="coactivosyp.PERSUASIVOLLAMADAS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
		<title>PERSUASIVOLLAMADAS</title>
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		
		<script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
		
		<script type="text/javascript">
			$(function() {
			    EndRequestHandler();
			});
			function EndRequestHandler() {
			    $('#cmdAddNew').button();
			    $('#cmdSearch').button();
			    $('#cmdFirst').button();
			    $('#cmdNext').button();
			    $('#cmdLast').button();
			    $('#cmdPrevious').button();
			    $('.GridEditButton').button();
			
			    $(".PCG-Content tr:gt(0)").mouseover(function() {
			        $(this).addClass("ui-state-highlight");
			    });
			
			    $(".PCG-Content tr:gt(0)").mouseout(function() {
			        $(this).removeClass("ui-state-highlight");
			    });
			}
		</script>
		<style type="text/css">			
		    * { font-size:12px;}	
		    .BoundFieldItemStyleHidden { display:none}	
		    .BoundFieldHeaderStyleHidden { display:none}		 
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
				<tr>
					<td align="right">
						<table style="width: 100%">
							<tr>
								<td class="ui-widget-header">
									No Telefono
								</td>
								<td>
									<asp:TextBox ID="txtSearchNoTelefono" runat="server" ></asp:TextBox>
								</td>
								<td class="ui-widget-header">
									Nombre
								</td>
								<td>
									<asp:TextBox ID="txtSearchNombre" runat="server" ></asp:TextBox>
								</td>
								<td>
									<asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
									<asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
						<asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
							<Columns>
								<asp:BoundField DataField="IdUnico" >
									<ItemStyle CssClass="BoundFieldItemStyleHidden" />
									<HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
								</asp:BoundField>
								<asp:BoundField DataField="NroExp" HeaderText="No. Exp" ></asp:BoundField>
								<asp:BoundField DataField="Fecha" HeaderText="Fecha" ></asp:BoundField>
								<asp:BoundField DataField="IdLlamada" HeaderText="Id Llamada" ></asp:BoundField>
								<asp:BoundField DataField="NoTelefono" HeaderText="No Teléfono" ></asp:BoundField>
								<asp:BoundField DataField="Nombre" HeaderText="Nombre" ></asp:BoundField>
								<asp:BoundField DataField="USUARIOSgestornombre" HeaderText="Gestor" ></asp:BoundField>
								<asp:BoundField DataField="InfoSolicitada" HeaderText="Info Solicitada" ></asp:BoundField>
								<asp:BoundField DataField="InfoBrindada" HeaderText="Info Brindada" ></asp:BoundField>
								<asp:BoundField DataField="NomTipific" HeaderText="Tipificación" ></asp:BoundField>
								<asp:BoundField DataField="Observaciones" HeaderText="Observaciones" ></asp:BoundField>
								<asp:ButtonField ButtonType="Button" Text="Editar">
									<ControlStyle CssClass="GridEditButton" />
								</asp:ButtonField>
							</Columns>
							<HeaderStyle CssClass="ui-widget-header"  />
							<RowStyle CssClass="ui-widget-content" />
							<AlternatingRowStyle/>
						</asp:GridView>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Button id="cmdFirst" runat="server" Text="Primero" cssClass="PCGButton"></asp:Button>
						<asp:Button id="cmdPrevious" runat="server" Text="Anterior" cssClass="PCGButton"></asp:Button>
						<asp:Label ID="lblPageNumber" runat="server"></asp:Label>
						&nbsp;&nbsp;
						<asp:Button id="cmdNext" runat="server" Text="Siguiente" cssClass="PCGButton"></asp:Button>
						<asp:Button id="cmdLast" runat="server" Text="Ultimo" cssClass="PCGButton"></asp:Button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>

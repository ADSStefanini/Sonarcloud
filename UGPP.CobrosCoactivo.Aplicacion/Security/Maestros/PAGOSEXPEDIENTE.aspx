<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PAGOSEXPEDIENTE.aspx.vb" Inherits="coactivosyp.PAGOSEXPEDIENTE" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>PAGOS</title>
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
                $('#cmdAdicionar').button();
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
		    * { font-size:12px; font-family: Arial;}
		    .BoundFieldItemStyleHidden { display:none;}	
		    .BoundFieldHeaderStyleHidden {display:none;}		    
	        .auto-style1 {
                height: 28px;
            }
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
                                    Nro Consignación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchNroConsignacion" runat="server" ></asp:TextBox>
                                </td>
                                <td class="ui-widget-header">
                                    Estado
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboSearchestado" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                    <asp:Button id="cmdAdicionar" runat="server" Text="Adicionar"></asp:Button>
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
                                <asp:BoundField DataField="NroConsignacion" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="NroConsignacion" HeaderText="Nro Consignación" SortExpression="PAGOS.NroConsignacion"></asp:BoundField>
                                <asp:BoundField DataField="FecSolverif" HeaderText="Fec. Sol. verif." SortExpression="PAGOS.FecSolverif" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="FecVerificado" HeaderText="Fec. Verificado" SortExpression="PAGOS.FecVerificado" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="ESTADOS_PAGOestadonombre" HeaderText="Estado" SortExpression="ESTADOS_PAGOestado.nombre"></asp:BoundField>
                                <asp:BoundField DataField="pagFecha" HeaderText="Fecha del pago" SortExpression="PAGOS.pagFecha" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="pagFechaDeudor" HeaderText="Fecha rep. deudor" SortExpression="PAGOS.pagFechaDeudor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="pagTotal" HeaderText="Total" SortExpression="PAGOS.pagTotal"></asp:BoundField>
                                <asp:BoundField DataField="ESTADOS_PROCESOpagestadoprocfrpnombre" HeaderText="Estado pago" SortExpression="ESTADOS_PROCESOpagestadoprocfrp.nombre"></asp:BoundField>
                                <asp:ButtonField ButtonType="Button" Text="Ver">
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
                    <td class="auto-style1">
                        <asp:Button id="cmdFirst" runat="server" Text="Primero" cssClass="PCGButton"></asp:Button>
                        <asp:Button id="cmdPrevious" runat="server" Text="Anterior" cssClass="PCGButton"></asp:Button>
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
                        <asp:Button id="cmdNext" runat="server" Text="Siguiente" cssClass="PCGButton"></asp:Button>
                        <asp:Button id="cmdLast" runat="server" Text="Ultimo" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
                <tr>
                   <td>
                       <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 340px;">
                            <iframe src="PAGOSOBSERVACIONES.aspx?pExpedientel= <%  Response.Write(Request("pExpediente"))%> " 
                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                    </td>
                </tr>
            </table>
        </form>

        <script src="<%=ResolveClientUrl("~/js/functions.js") %>" type="text/javascript"></script>
        <script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>

    </body>
</html>


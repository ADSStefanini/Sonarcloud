<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DIRECCIONES.aspx.vb" Inherits="coactivosyp.DIRECCIONES" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
    <head>
        <title>DIRECCIONES</title>
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
		    * { font-size:12px; font-family:Arial}	
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden {display:none;}
		</style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td align="left">
                        
                        <asp:Button id="cmdAddNew" runat="server" Text="Adicionar dirección" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>&nbsp;
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                            <Columns>
                                <asp:BoundField DataField="deudor" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="Direccion" HeaderText="Dirección"></asp:BoundField>
                                <asp:BoundField DataField="DEPARTAMENTOSDepartamentonombre" HeaderText="Departamento"></asp:BoundField>
                                <asp:BoundField DataField="MUNICIPIOSCiudadnombre" HeaderText="Ciudad"></asp:BoundField>
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono"></asp:BoundField>
                                <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                                <asp:BoundField DataField="Movil" HeaderText="Móvil"></asp:BoundField>
                                <asp:BoundField DataField="paginaweb" HeaderText="Página web"></asp:BoundField>
                                <asp:BoundField DataField="idunico" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
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
            </table>
        </form>
    </body>
</html>

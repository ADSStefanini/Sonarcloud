<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MAESTRO_APODERADOS.aspx.vb" Inherits="coactivosyp.MAESTRO_APODERADOS" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
    <head>
        <title>MAESTRO_APODERADOS</title>
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
		    body{ background-color:#01557C}	
	        * { font-size:12px;}				 
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
                            No. Identificación</td>
                            <td>
                            <asp:TextBox ID="txtSearchMAP_Codigo_Nit" runat="server" ></asp:TextBox></td>
                        <td class="ui-widget-header">
                        Nombre</td>
                        <td>
                        <asp:TextBox ID="txtSearchMAP_Nombre" runat="server" ></asp:TextBox></td>
                                            <td>                                                
                                                <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                            </td>
                                    </tr>
                                        <tr>
                    <td class="ui-widget-header">
                    Tarjeta profesional</td>
                    <td>
                    <asp:TextBox ID="txtSearchMAP_TarjetaProf" runat="server" ></asp:TextBox></td>
                                            <td>&nbsp;</td><td>&nbsp;</td>
                                            <td><asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button></td>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
                                        <Columns>
                                            <asp:BoundField DataField="MAP_Codigo_Nit" >
                                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MAP_Codigo_Nit" HeaderText="No. identificación" SortExpression="MAESTRO_APODERADOS.MAP_Codigo_Nit"></asp:BoundField>
                                            <asp:BoundField DataField="MAP_Nombre" HeaderText="Nombre" SortExpression="MAESTRO_APODERADOS.MAP_Nombre"></asp:BoundField>
                                            <asp:BoundField DataField="MAP_TarjetaProf" HeaderText="Tarjeta profesional" SortExpression="MAESTRO_APODERADOS.MAP_TarjetaProf"></asp:BoundField>
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
                                    <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
                                    <asp:Button id="cmdNext" runat="server" Text="Siguiente" cssClass="PCGButton"></asp:Button>
                                    <asp:Button id="cmdLast" runat="server" Text="Ultimo" cssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </form>
                </body>
            </html>

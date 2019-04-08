<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NOTAS.aspx.vb" Inherits="coactivosyp.NOTAS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Notas</title>
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
	        * { font-size:12px; font-family:Arial;}	
	        .BoundFieldItemStyleHidden { display: none;}
		    .BoundFieldHeaderStyleHidden {display: none;}		   
        </style>
                
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">                
                <tr>
                    <td align="left">                        
                        <asp:Button id="cmdAddNew" runat="server" Text="Adicionar observación / nota" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td align="left">                        
                        &nbsp;
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                            <Columns>
                                <asp:BoundField DataField="IdUnico" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones"></asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
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

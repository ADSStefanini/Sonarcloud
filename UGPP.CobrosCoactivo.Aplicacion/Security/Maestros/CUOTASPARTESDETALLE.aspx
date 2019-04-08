<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CUOTASPARTESDETALLE.aspx.vb" Inherits="coactivosyp.CUOTASPARTESDETALLE" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Grid de Detalle de proceso con Cuota Parte</title>
        
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
                $('#cmdBack').button();
                
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
		    * { font-size:12px; font-family:Arial; }		   
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden { display:none; }
	    </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td align="left">           
                        <asp:Button id="cmdBack" runat="server" Text="Regresar al encabezado de la Cuota Parte" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                            <Columns>
                                <asp:BoundField DataField="Pensionado" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="NroExp" HeaderText="Nro Exp"></asp:BoundField>
                                <asp:BoundField DataField="Pensionado" HeaderText="Pensionado"></asp:BoundField>
                                <asp:BoundField DataField="NomPensionado" HeaderText="Nom Pensionado"></asp:BoundField>
                                <asp:BoundField DataField="CapitalCP" HeaderText="Capital C P"></asp:BoundField>
                                <asp:BoundField DataField="PeriodoIniCob" HeaderText="Periodo Ini Cob" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="PeriodoFinCob" HeaderText="Periodo Fin Cob" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="FechaPresPerAnt" HeaderText="Fecha Pres Per Ant" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="FalloOrdenRecPen" HeaderText="Fallo Orden Rec Pen"></asp:BoundField>
                                <asp:BoundField DataField="ResolCumpleFallo" HeaderText="Resol Cumple Fallo"></asp:BoundField>                                
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

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Observaciones.aspx.vb" Inherits="coactivosyp.Observaciones" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Observaciones 
    </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

   <style>
       .ui-widget-header{
            VERTICAL-ALIGN: top;
            font-size:14px;
       }
       .PCGButton {
           font-size:small;
       }
      
   </style>
    <script type="text/javascript">
        $(function () {
            $('submit,input:button,input:submit').button();

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="HdnIdUnico" ClientIDMode="Static" />
        <div class="ui-widget-content ui-widget">
            <div class="row">
                <asp:GridView ID="grdHistorico" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True" CellPadding="4" PageSize="10"  PagerSettings-Visible="False" AllowPaging="True">
                    <Columns>
                        <asp:BoundField DataField="ID_OBSERVACIONESDOC" Visible="False"></asp:BoundField>
                        <asp:BoundField DataField="USUARIO" HeaderText="Usuario "></asp:BoundField>
                        <asp:BoundField DataField="DESTINATARIO" HeaderText="Destinatario "></asp:BoundField>
                        <asp:BoundField DataField="FCHOBSERVACIONES" HeaderText="Fecha y hora del envío "></asp:BoundField>
                        <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observación"></asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                    <RowStyle CssClass="ui-widget-content" />
                    <AlternatingRowStyle />
                </asp:GridView>
                 <uc1:Paginador id="PaginadorGridView" runat="server"  gridViewIdClient="grdHistorico"  OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid"    />
            </div>           
        </div>
    </form>
</body>
</html>

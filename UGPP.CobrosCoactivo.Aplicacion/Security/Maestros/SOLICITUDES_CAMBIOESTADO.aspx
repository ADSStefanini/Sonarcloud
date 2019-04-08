<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SOLICITUDES_CAMBIOESTADO.aspx.vb" Inherits="coactivosyp.SOLICITUDES_CAMBIOESTADO" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>SOLICITUDES CAMBIO ESTADO</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function () {
            EndRequestHandler();
        });
        function EndRequestHandler() {
            $('#cmdAddNew').button();
            $('.GridEditButton').button();

            $(".PCG-Content tr:gt(0)").mouseover(function () {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function () {
                $(this).removeClass("ui-state-highlight");
            });
        }
    </script>

    <style type="text/css">
        * {
            font-size: 12px;
            font-family: Arial
        }

        .BoundFieldItemStyleHidden {
            display: none;
        }

        .BoundFieldHeaderStyleHidden {
            display: none;
        }
    </style>

</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td align="right">
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    &nbsp;                     
                        <asp:Button ID="cmdAddNew" runat="server" Text="Adicionar" CssClass="PCGButton"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="idunico">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NroExp" HeaderText="Expediente" SortExpression="SOLICITUDES_CAMBIOESTADO.NroExp"></asp:BoundField>
                            <asp:BoundField DataField="USUARIOSabogadonombre" HeaderText="Gestor / Abogado" SortExpression="USUARIOSabogado.nombre"></asp:BoundField>
                            <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="SOLICITUDES_CAMBIOESTADO.fecha"></asp:BoundField>
                            <asp:BoundField DataField="ESTADOS_SOL_CAM_ESTestadosolnombre" HeaderText="Estado solicitud" SortExpression="ESTADOS_SOL_CAM_ESTestadosol.nombre"></asp:BoundField>
                            <asp:BoundField DataField="ESTADOS_PROCESOestadonombre" HeaderText="Estado solicitado" SortExpression="ESTADOS_PROCESOestado.nombre"></asp:BoundField>
                            <asp:BoundField DataField="USUARIOSrevisornombre" HeaderText="Revisor"></asp:BoundField>
                            <asp:BoundField DataField="aprob_revisor" HeaderText="Aprob."></asp:BoundField>
                            <asp:ButtonField ButtonType="Button" Text="Ver">
                                <ControlStyle CssClass="GridEditButton" />
                            </asp:ButtonField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>


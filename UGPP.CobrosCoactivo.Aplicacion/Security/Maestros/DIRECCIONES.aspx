<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DIRECCIONES.aspx.vb" Inherits="coactivosyp.DIRECCIONES" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>DIRECCIONES</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/bts-jquery.min.js"></script>
    <script type="text/javascript" src="../../assets/bootstrap/js/bootstrap.js"></script>
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
        <asp:HiddenField runat="server" ID="HdnIdTask" Value="0" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="HdnLstDirecciones" Value="" ClientIDMode="Static" />
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td align="left">

                    <asp:Button ID="cmdAddNew" runat="server" Text="Adicionar dirección" CssClass="PCGButton"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>&nbsp;
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" PageSize="10" AllowPaging="true"  PagerSettings-Visible="False" CssClass="PCG-Content">
                        <Columns>
                            <asp:BoundField DataField="numeroidentificacionDeudor">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="direccionCompleta" HeaderText="Dirección"></asp:BoundField>
                            <asp:BoundField DataField="nombreFuente" HeaderText="Fuente"></asp:BoundField>
                            <asp:BoundField DataField="otrasFuentesDirecciones" HeaderText="Otra Fuente"></asp:BoundField>
                            <asp:BoundField DataField="NombreDepartamento" HeaderText="Departamento"></asp:BoundField>
                            <asp:BoundField DataField="NombreMunicipio" HeaderText="Ciudad"></asp:BoundField>
                            <asp:BoundField DataField="telefono" HeaderText="Teléfono"></asp:BoundField>
                            <asp:BoundField DataField="email" HeaderText="Email"></asp:BoundField>
                            <asp:BoundField DataField="celular" HeaderText="Móvil"></asp:BoundField>
                            <asp:BoundField DataField="paginaweb" HeaderText="Página web"></asp:BoundField>
                            <asp:BoundField DataField="idunico">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:ButtonField ButtonType="Button" Text="Editar">
                                <ControlStyle CssClass="GridEditButton" />
                            </asp:ButtonField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle />
                    </asp:GridView>
                    <uc1:Paginador ID="PaginadorGridView" runat="server" gridViewIdClient="grd" OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

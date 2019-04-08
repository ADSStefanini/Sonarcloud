<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditDIRECCIONES.aspx.vb" Inherits="coactivosyp.EditDIRECCIONES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar direcciones
    </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function () {
            //$('#cmdDelete').button();
            $('#cmdSave').button();
            $('#cmdCancel').button();
            $("#TxtOtraFuente").hide();
            // Campos a mayusculas
            $("input[type=text]").keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });                //
            OtraFuenteCheck();

        });

        function SeleccionaOtraFuente(Sel) {

            if (Sel.value == 4) {
                $("#TxtOtraFuente").show();
            } else {
                $("#TxtOtraFuente").hide();
            }
            console.log(Sel.value)

        }

         function OtraFuenteCheck() {

            if ($("#CboFuente").children("option:selected").val() == "4") {
                $("#TxtOtraFuente").show();
            } else {
                $("#TxtOtraFuente").hide();
            }
        }
    </script>
    <style type="text/css">
        * {
            font-size: 12px;
            font-family: Arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="pnlDirecciones" runat="server">
                <asp:HiddenField runat="server" ID="HdnIdTask" Value="0" ClientIDMode="Static" />
            <table id="tblEditDIRECCIONES" class="ui-widget-content">
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Dirección Completa *
                    </td>
                    <td>
                        <asp:TextBox ID="txtDireccion" runat="server" MaxLength="180" CssClass="ui-widget" Columns="80" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Departamento *
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" ID="cboDepartamento" runat="server" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Ciudad *
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" ID="cboCiudad" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Teléfono
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefono" runat="server" MaxLength="40" CssClass="ui-widget numeros"  />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Email
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="80" CssClass="ui-widget" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Móvil
                    </td>
                    <td>
                        <asp:TextBox ID="txtMovil" runat="server" MaxLength="40" CssClass="ui-widget numeros" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Fuente de la dirección *
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" ID="CboFuente" runat="server" onchange="SeleccionaOtraFuente(this)"></asp:DropDownList>
                        <asp:TextBox ID="TxtOtraFuente" runat="server" MaxLength="80" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Página web
                    </td>
                    <td>
                        <asp:TextBox ID="txtpaginaweb" runat="server" MaxLength="40" CssClass="ui-widget" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">                       
                        <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="PCGButton" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
         <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="PCGButton" />&nbsp;&nbsp;&nbsp;
    </form>

    <script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>

</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DICCIONARIO_AUDITORIA.aspx.vb" Inherits="coactivosyp.DICCIONARIO_AUDITORIA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Diccionario</title>
    <link href="../../css/main.css" rel="stylesheet" />
    <link href="../../css/list.css" rel="stylesheet" />
    <link href="css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('submit,input:button,input:submit').button();
              $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
        });
    </script>
</head>
<body class="internal">
    <form id="form1" runat="server">
        <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td colspan="10" background="images/resultados_busca.jpg" height="42">
                    <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                    <div style="color: White; width: 150px; height: 20px; float: right; text-align: right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span>Cerrar sesión&nbsp&nbsp</span>
                    </div>

                    <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" /></asp:LinkButton>
                    </div>

                </td>
            </tr>
        </table>
        <div id="SearchParams" class="form-row search-list-form">
            <div class="col">
                <span class="ui-widget-header">Origen</span>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Buscar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" />
                <asp:Button ID="btnAdd" runat="server" Text="Adicionar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" />
            </div>
        </div>
        <div id="data">
            <asp:GridView ID="gwDiccionario" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content list tbl-indentation" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="VALOR_ORIGINAL" HeaderText="Origen">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VALOR_DESTINO" HeaderText="Destino">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:CheckBoxField DataField="ACTIVO" HeaderText="Estado">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:CheckBoxField>
                    <asp:ButtonField ButtonType="Button" Text="Editar" CommandName="edit">
                        <ControlStyle CssClass="GridEditButton" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                </Columns>

                <HeaderStyle CssClass="ui-widget-header" />
                <PagerSettings Visible="False" />
                <RowStyle CssClass="ui-widget-content" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>

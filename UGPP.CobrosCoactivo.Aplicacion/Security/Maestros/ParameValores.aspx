<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParameValores.aspx.vb" Inherits="coactivosyp.ParameValores" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ENTES_DEUDORES</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
     <link href="../../css/main.css" rel="stylesheet" />
    <link href="../../css/list.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
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
            text-align: center;
        }

        .BoundFieldItemStyleHidden {
            display: none
        }

        .BoundFieldHeaderStyleHidden {
            display: none
        }

        .auto-style1 {
            height: 43px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
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
        <br />
        <table id="Table" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td class="auto-style1">
                    <asp:GridView ID="grdValores" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True" CellPadding="4">
                        <Columns>
                            <asp:BoundField DataField="ID_TIPO_OBLIGACION_VALORES">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NOMBRETIPO" HeaderText="TIPO DE OBLIGACIÓN"></asp:BoundField>
                            <asp:TemplateField HeaderText="VALOR OBLIGACIÓN">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkValObliga" runat="server" Checked='<%# Eval("VALOR_OBLIGACION") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PARTIDA GLOBAL">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkPartidaGlobal" runat="server" Checked='<%# Eval("PARTIDA_GLOBAL") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SANCIÓN OMISIÓN">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSancionOmisio" runat="server" Checked='<%# Eval("SANCION_OMISION") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SANCIÓN INEXAXITUD">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSancionInexa" runat="server" Checked='<%# Eval("SANCION_INEXACTITUD") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ESTADO_PROCESO_GESTOR.aspx.vb" Inherits="coactivosyp.ESTADO_PROCESO_GESTOR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Estado Proceso Gestor</title>
    <link href="../../css/main.css" rel="stylesheet" />
    <link href="../../css/list.css" rel="stylesheet" />
    <link href="css/redmond/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('submit,input:button,input:submit').button();
            $(".PCG-Content tr:gt(0)").mouseover(function () {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function () {
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
                <span class="ui-widget-header">Usuario - Gestor</span>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Buscar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" />
                <asp:Button ID="btnAdd" runat="server" Text="Adicionar" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" />
            </div>
        </div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableScriptGlobalization="True" />
        <div id="tabsModal2">
            <asp:ModalPopupExtender ID="mp2" runat="server" PopupControlID="pnlEdit" TargetControlID="lblHide"
                CancelControlID="btnClose2" BackgroundCssClass="FondoAplicacion">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlEdit" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff; border-color: #01557c;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label ID="lblUsuario" runat="server" Text="Ingrese el login de Usuario*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" />
                            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqUsuario" ControlToValidate="txtUsuario" runat="server" ValidationGroup="Editestado" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                        <br />
                        <div class="form-group">
                            <asp:Label ID="lblEstado" runat="server" Text="Seleccionar Estado*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" />
                            <asp:DropDownList ID="ddlEstado" runat="server" DataTextField="nombre" DataValueField="codigo" InitialValue="0" ErrorMessage="Por favor seleccione un estado" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlEstado" InitialValue="0" ErrorMessage="*" ValidationGroup="Editestado" />
                        </div>
                        <br />

                        <div class="form-group">
                            <asp:Label ID="lblEtapa" runat="server" Text="Seleccionar Etapa*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" />
                            <asp:DropDownList ID="ddlEtapa" runat="server" DataTextField="VAL_ETAPA_PROCESAL" DataValueField="ID_ETAPA_PROCESAL" InitialValue="0" ErrorMessage="Por favor seleccione un etapa" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEtapa" InitialValue="0" ErrorMessage="*" ValidationGroup="Editestado" />
                        </div>
                        <br />
                        <div class="form-group">
                            <asp:Label ID="lblEstadoCheck" runat="server" Text="Activo" CssClass="col-sm-3 col-form-label col-form-label-sm style4" />
                            <asp:CheckBox ID="cbState" runat="server" Checked="true"></asp:CheckBox>
                        </div>
                        <br />
                        <div class="col-sm-12">
                            <asp:Button ID="cmdAceptar" runat="server" Text="Aceptar" CssClass="button" ClientIDMode="Inherit" ValidationGroup="Editestado" OnClick="cmdAceptar_Click" />
                            <asp:Button ID="btnClose2" CssClass="PCGButton button" runat="server" Text="Cancelar" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div id="data">
            <asp:GridView ID="gwEstados" runat="server" AllowPaging="true" CellPadding="4"   DataKeyNames="COD_ID_ESTADOS_PROCESOS" OnRowCommand="gwEstados_RowCommand" PageSize="14" PagerSettings-Visible="False" AutoGenerateColumns="False" CssClass="PCG-Content list tbl-indentation" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="VAL_USUARIO" HeaderText="Usuario - Gestor">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NOMBRE_ESTADOS_PROCESOS" HeaderText="Estado Proceso">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ETAPA_PROCESAL" HeaderText="Etapa" Visible="false">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NOMBRE_ETAPA" HeaderText="Etapa">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="USUARIO_ACTUALIZACION" HeaderText="Usuario Actualización">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FECHA_ACTUALIZACION" HeaderText="Fecha Actualización">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>                    
                    <asp:TemplateField HeaderText="Activo">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkActivo" runat="server" Checked='<%#Convert.ToBoolean(Eval("IND_ESTADO")) %>' EnableViewState="true" Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:ButtonField ButtonType="Button" Text="Editar" CommandName="edit">
                        <ControlStyle CssClass="GridEditButton" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:ButtonField>
                </Columns>

                <HeaderStyle CssClass="ui-widget-header" />
                <PagerSettings Visible="False" />
                <RowStyle CssClass="ui-widget-content" />
            </asp:GridView>
            <uc1:Paginador id="PaginadorGridView" runat="server"  gridViewIdClient="gwEstados"  OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
        </div>
        <asp:Label ID="lblHide" runat="server" CssClass="hide" />
    </form>

</body>
</html>

<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SolicitudReasignacion.ascx.vb" Inherits="coactivosyp.SolicitudReasignación" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="tabsModalReasignacion">
    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />--%>
    <!-- ModalPopupExtender -->
    <asp:ModalPopupExtender ID="modalReasignacion" runat="server" PopupControlID="PnlJust" TargetControlID="btnTest"
        CancelControlID="btnClose" BackgroundCssClass="FondoAplicacion">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnlJust" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
        <asp:UpdatePanel ID="UpnlModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hdnReloadPage" runat="server" Value="0" />
                <asp:HiddenField ID="hdnIdTareaAsiganada" runat="server" />
                <asp:HiddenField ID="hdnTipoGestor" runat="server" />

                <div class="form-group row">
                    <asp:Label ID="lblErrorReasignacion" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblNoUsuarioSuperior" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblReasignacionEnviada" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-success" Visible="False" Style="margin: 0 auto;" Text="Reasignación enviada correctamente"/>
                </div>
                

                <div class="form-group row">
                    <asp:Label ID="lblCausalReasignacion" runat="server" Text="Causal Reasignación*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="ddlCausalReasignacion" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlCausalReasignacion" runat="server">
                            <asp:ListItem Value="0" Text=" " />
                        </asp:DropDownList>
                        <asp:CompareValidator ControlToValidate="ddlCausalReasignacion" ID="ddlCausalReasignacionValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="formReasignacion" />
                    </div>
                </div>

                <div class="form-group row" id="divGestorSolicitado" runat="server">
                    <asp:Label ID="lblGestorSolicitado" runat="server" Text="Gestor Solicitado" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="ddlCausalReasignacion" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlGestorSolicitado" runat="server">
                            <asp:ListItem Value="0" Text=" " />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblReasignacionObservacion" runat="server" Text="Observación Reasignación*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" AssociatedControlID="txtReasignacionObservacion" />
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtReasignacionObservacion" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator ID="reqReasignacionObservacion" runat="server" ErrorMessage="" ControlToValidate="txtReasignacionObservacion" CssClass="error-msg" ValidationGroup="formReasignacion" />
                    </div>
                </div>
                <div class="col-sm-12">
                    <asp:Button ID="cmdSolicitarReasignacion" runat="server" Text="Enviar" CssClass="button" ClientIDMode="Inherit" ValidationGroup="formReasignacion" />
                    <asp:Button ID="btnClose" CssClass="PCGButton button" runat="server" Text="Cancelar" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>
    <!-- ModalPopupExtender -->
</div>

<asp:Button ID="btnTest" runat="server" Text="Button" CssClass="hide" style="display:none;" />
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SolicitudPriorizacion.ascx.vb" Inherits="coactivosyp.SolicitudPriorizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="tabsModalPriorizacion">
    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableScriptGlobalization="True" />--%>
    <!-- ModalPopupExtender -->
    <asp:ModalPopupExtender ID="modalPriorizacion" runat="server" PopupControlID="PnlPriorizacion" TargetControlID="btnTest2"
        CancelControlID="btnClose2" BackgroundCssClass="FondoAplicacion">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnlPriorizacion" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <asp:HiddenField ID="hdnIdTareaAsiganada" runat="server" />

                <div class="form-group row">
                    <asp:Label ID="lblErrorPriorizacion" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblPriorizacionEnviada" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-success" Visible="False" Style="margin: 0 auto;" Text="Priorización enviada correctamente"/>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblCausalPriorizacion" runat="server" Text="Causal Priorización*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" AssociatedControlID="ddlCausalPriorizacion" />
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlCausalPriorizacion" runat="server">
                            <asp:ListItem Selected="True" Value="0">-</asp:ListItem>
                        </asp:DropDownList>
                        <asp:CompareValidator ControlToValidate="ddlCausalPriorizacion" ID="CausalPriorizacionValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="formPriorizacion" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label ID="lblObservacionPriorizacion" runat="server" Text="Observación Priorización*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" AssociatedControlID="txtObservacionPriorizacion" />
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtObservacionPriorizacion" runat="server" CssClass="form-control form-control-sm" CausesValidation="True" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator ID="reqObservacionPriorizacion" runat="server" ErrorMessage="" ControlToValidate="txtObservacionPriorizacion" CssClass="error-msg" ValidationGroup="formPriorizacion" />
                    </div>
                </div>

                <div class="col-sm-12">
                    <asp:Button ID="cmdPriorizar" runat="server" Text="Enviar" CssClass="button" ClientIDMode="Inherit" ValidationGroup="formPriorizacion" />
                    <asp:Button ID="btnClose2" CssClass="PCGButton button" runat="server" Text="Cancelar" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>
    <!-- ModalPopupExtender -->
</div>
<asp:Button ID="btnTest2" runat="server" Text="Button" CssClass="hide" style="display:none;" />
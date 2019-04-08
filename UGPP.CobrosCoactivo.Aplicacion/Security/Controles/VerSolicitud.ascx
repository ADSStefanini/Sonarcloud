<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="VerSolicitud.ascx.vb" Inherits="coactivosyp.VerSolicitud" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="tabsModalVerSolicitud">
    <asp:ModalPopupExtender ID="modalVerSolicitud" runat="server" PopupControlID="PnlVerSolicitud" TargetControlID="btnTest"
        CancelControlID="btnClose" BackgroundCssClass="FondoAplicacion">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnlVerSolicitud" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
        <asp:UpdatePanel ID="UpnlModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnTareaSolicitud" runat="server" Value="0" />

                 <div class="form-group row">
                    <asp:Label ID="lblErrorVerSolicitud" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
                </div>

                <asp:Panel ID="pnlGestorSolicitado" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblGestorSolicitado" runat="server" Text="Gestor Solicitado" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                        <div class="col-sm-9">
                            <asp:Label ID="lblGestorSolicitadoText" runat="server" Text="" CssClass="col-sm-9 col-form-label col-form-label-sm" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlCausal" runat="server" Visible="false">
                    <div class="form-group row">
                        <asp:Label ID="lblCausal" runat="server" Text="Causal" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                        <div class="col-sm-9">
                            <asp:Label ID="lblCausalText" runat="server" Text="" CssClass="col-sm-9 col-form-label col-form-label-sm" />
                        </div>
                    </div>
                </asp:Panel>
                
                <div class="form-group row">
                    <asp:Label ID="lblObservacion" runat="server" Text="Observación" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                    <div class="col-sm-9">
                        <asp:Label ID="lblObservacionText" runat="server" Text="" CssClass="col-sm-9 col-form-label col-form-label-sm" />
                    </div>
                </div>

                <div class="col-sm-12">
                    <asp:Button ID="btnClose" CssClass="PCGButton button" runat="server" Text="Cerrar" />
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>

<asp:Button ID="btnTest" runat="server" Text="Button" CssClass="hide" style="display: none;" />
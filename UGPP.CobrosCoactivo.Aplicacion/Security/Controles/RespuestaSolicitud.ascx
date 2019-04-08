<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RespuestaSolicitud.ascx.vb" Inherits="coactivosyp.AprobarSolicitud" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<div id="tabsModalVerSolicitud">
    <asp:ModalPopupExtender ID="modalResSolicitud" runat="server" PopupControlID="PnlResSolicitud" TargetControlID="btnTest"
        CancelControlID="btnClose" BackgroundCssClass="FondoAplicacion">
    </asp:ModalPopupExtender>
    <asp:Panel ID="PnlResSolicitud" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display:none; background: #fff;">
        <asp:UpdatePanel ID="UpnlModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hdnTareasSolicitud" runat="server" Value="0" />
                <asp:HiddenField ID="hdnTipoSolicitud" runat="server" Value="0" />
                <asp:HiddenField ID="hdnReload" runat="server" Value="0" />

                 <div class="form-group row">
                    <asp:Label ID="lblErrorRespuestaSolicitud" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblRespuestaSolicitudEnviada" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-success" Visible="False" Style="margin: 0 auto;" Text="Priorización enviada correctamente"/>
                </div>

                <asp:Panel ID="pnlGestor" runat="server">
                        <div class="form-group row">
                        <asp:Label ID="lblGestorAsiganado" runat="server" Text="Gestor Asignado" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlGestorAsignado" runat="server">
                                <asp:ListItem Value="0" Text=" " />
                            </asp:DropDownList>
                        </div>
                    </div>
                </asp:Panel>
                

                <div class="form-group row">
                    <asp:Label ID="lblObservacion" runat="server" Text="Observación" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtRespuestaSolicitud" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator ID="reqRespuestaSolicitud" runat="server" ErrorMessage="" ControlToValidate="txtRespuestaSolicitud" CssClass="error-msg" ValidationGroup="formRespuestaSolicitud" />
                    </div>
                </div>

                <div class="col-sm-12 btns-respuesta-slicitud">
                    <asp:Button ID="btnAprobarSolicitud" CssClass="PCGButton button" runat="server" Text="Aprobar" ValidationGroup="formRespuestaSolicitud" />
                    <asp:Button ID="btnDenegarSolicitud" CssClass="PCGButton button" runat="server" Text="Denegar" ValidationGroup="formRespuestaSolicitud" />
                    <asp:Button ID="btnClose" CssClass="PCGButton button" runat="server" Text="Cerrar" />
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>

<asp:Button ID="btnTest" runat="server" Text="Button" CssClass="hide" style="display: none;" />
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="modulo.ascx.vb" Inherits="coactivosyp.modulo" %>

<asp:Panel ID="pnlModulo" runat="server" CssClass="form-content" DefaultButton="cmdSave">

    <asp:TextBox ID="txtModuloId" runat="server" Visible="False" />
        
    <div class="form-group row">
        <asp:Label ID="lblNombreModulo" runat="server" Text="Nombre*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtNombreModulo" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtNombreModulo" runat="server" CssClass="form-control form-control-sm" />
            <asp:RequiredFieldValidator ID="reqNombreModulo" runat="server" ErrorMessage="" ControlToValidate="txtNombreModulo" CssClass="error-msg" ValidationGroup="validadorModulo" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblUrlModulo" runat="server" Text="URL*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtUrlModulo" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtUrlModulo" runat="server" CssClass="form-control form-control-sm" />
            <asp:RequiredFieldValidator ID="reqUrlModulo" runat="server" ErrorMessage="" ControlToValidate="txtUrlModulo" CssClass="error-msg" ValidationGroup="validadorModulo" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblUrlIconoModulo" runat="server" Text="URL Icono" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtUrlIconoModulo" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtUrlIconoModulo" runat="server" CssClass="form-control form-control-sm" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblEstado" runat="server" Text="Estado*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
        <div class="col-sm-9">
            <div class="form-check radio-list-content">
                <asp:RadioButtonList ID="rdoEstado" runat="server" CssClass="form-check-input">
                    <asp:ListItem Value="1" Text="Activo" />
                    <asp:ListItem Value="0" Text="Inactivo" />
                </asp:RadioButtonList>
               <asp:RequiredFieldValidator ID="reqrdoEstado" runat="server" ErrorMessage="" ControlToValidate="rdoEstado" CssClass="error-msg" ValidationGroup="validadorModulo" />
            </div>
        </div>
    </div>

    <div class="col-sm-12">
        <asp:CustomValidator ID="cValErrorAlGuardar" runat="server" Visible="False" Display="Static" />
    </div>
    
    <div class="col-sm-12">
        <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="button" ClientIDMode="Inherit" ValidationGroup="validadorModulo" />
        <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="button" />
    </div>

</asp:Panel>

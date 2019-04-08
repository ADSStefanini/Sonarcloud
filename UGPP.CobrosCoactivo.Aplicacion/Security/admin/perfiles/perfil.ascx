<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="perfil.ascx.vb" Inherits="coactivosyp.perfil" %>
<asp:Panel ID="pnlPerfil" runat="server" CssClass="form-content" DefaultButton="cmdSave">

    <div class="form-group row">
        <asp:Label ID="lblNombrePerfil" runat="server" Text="Nombre*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtNombrePerfil" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtNombrePerfil" runat="server" CssClass="form-control form-control-sm" />
            <asp:RequiredFieldValidator ID="reqNombrePerfil" runat="server" ErrorMessage="" ControlToValidate="txtNombrePerfil" CssClass="error-msg" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblGrupoLdap" runat="server" Text="Grupo LDAP*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtGrupoLdap" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtGrupoLdap" runat="server" CssClass="form-control form-control-sm" />
            <asp:RequiredFieldValidator ID="reqGrupoLdap" runat="server" ErrorMessage="" ControlToValidate="txtGrupoLdap" CssClass="error-msg" />
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
               <asp:RequiredFieldValidator ID="reqrdoEstado" runat="server" ErrorMessage="" ControlToValidate="rdoEstado" CssClass="error-msg" />
            </div>
        </div>
    </div>

    <div class="col-sm-12">
        <asp:CustomValidator ID="cValErrorAlGuardar" runat="server" Visible="False" Display="Static"></asp:CustomValidator>
    </div>
    
    <div class="col-sm-12">
        <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="button" ClientIDMode="Inherit" />
        <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="button" />
    </div>

</asp:Panel>

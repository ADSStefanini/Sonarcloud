<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="paginas.ascx.vb" Inherits="coactivosyp.paginas1" %>

<asp:Panel ID="pnlPagina" runat="server" CssClass="form-content" DefaultButton="cmdSave">

    <div class="form-group row">
        <asp:Label ID="lblNombrePagina" runat="server" Text="Nombre*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtNombrePagina" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtNombrePagina" runat="server" CssClass="form-control form-control-sm" />
            <asp:RequiredFieldValidator ID="reqNombrePagina" runat="server" ErrorMessage="" ControlToValidate="txtNombrePagina" CssClass="error-msg" ValidationGroup="validadorPagina" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblUrlPagina" runat="server" Text="URL" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtUrlPagina" />
        <div class="col-sm-9">
            <asp:TextBox ID="txtUrlPagina" runat="server" CssClass="form-control form-control-sm" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblPadre" runat="server" Text="Padre" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="ddlPadre" />
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlPadre" runat="server" CssClass="form-control form-control-sm" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label ID="lblPaginaInterna" runat="server" Text="Pagina Interna" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
        <div class="col-sm-9">
            <div class="form-check radio-list-content">
                <asp:RadioButtonList ID="rdoPaginaInterna" runat="server" CssClass="form-check-input">
                    <asp:ListItem Value="1" Text="Activo" />
                    <asp:ListItem Value="0" Text="Inactivo" />
                </asp:RadioButtonList>
            </div>
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
               <asp:RequiredFieldValidator ID="reqrdoEstado" runat="server" ErrorMessage="" ControlToValidate="rdoEstado" CssClass="error-msg" ValidationGroup="validadorPagina" />
            </div>
        </div>
    </div>

    <div class="col-sm-12">
        <asp:CustomValidator ID="cValErrorAlGuardar" runat="server" Visible="True" Display="Static" />
    </div>
    
    <div class="col-sm-12">
        <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="button" ClientIDMode="Inherit" ValidationGroup="validadorPagina" />
        <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="button" />
    </div>

</asp:Panel>

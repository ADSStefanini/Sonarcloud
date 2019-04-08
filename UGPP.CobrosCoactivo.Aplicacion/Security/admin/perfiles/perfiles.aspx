<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Lista.Master" CodeBehind="perfiles.aspx.vb" Inherits="coactivosyp.perfiles" %>

<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="search" runat="server">
    <asp:Panel ID="pnlSearchPerfiles" runat="server" CssClass="form-row search-list-form" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="col">
            <asp:Label ID="lblSearchPerfiles" runat="server" Text="Nombre / Grupo: " CssClass="ui-widget-header"></asp:Label>
            <asp:TextBox ID="txtSearchPerfiles" runat="server" ValidationGroup="searchProfiles"></asp:TextBox>
            <%--<asp:RequiredFieldValidator ID="reqTerminoBusqueda" runat="server" ErrorMessage="" ControlToValidate="txtSearchPerfiles" CssClass="error-msg" SetFocusOnError="True" />--%>
        </div>
        <div class="col">
            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="searchProfiles"></asp:Button>
            <asp:Button ID="cmdAddNew" runat="server" Text="Adicionar" CssClass="PCGButton button"></asp:Button>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="table" runat="server">

    <asp:GridView ID="gvwPerfiles" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content list" AllowSorting="True" OnPageIndexChanging="gvwPerfiles_PageIndexChanged" >
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="Código">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
            </asp:BoundField>
            <asp:BoundField DataField="nombre_perfil" HeaderText="Nombre" />
            <asp:BoundField DataField="val_ldap_group" HeaderText="Grupo LDAP" />
            <asp:BoundField DataField="ind_estado" HeaderText="Estado" />
            <asp:ButtonField ButtonType="Button" Text="Editar">
                                    <ControlStyle CssClass="GridEditButton" />
                                </asp:ButtonField>
        </Columns>

        <HeaderStyle CssClass="ui-widget-header" />
        <PagerSettings Visible="False" />
        <RowStyle CssClass="ui-widget-content" />

    </asp:GridView>

</asp:Content>

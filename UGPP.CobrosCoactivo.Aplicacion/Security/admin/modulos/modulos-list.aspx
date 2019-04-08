<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Lista.Master" CodeBehind="modulos-list.aspx.vb" Inherits="coactivosyp.modulos_list" %>

<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >

</asp:Content>

<asp:Content ID="SearchModulos" ContentPlaceHolderID="search" runat="server">
   <asp:Panel ID="pnlSearchModulos" runat="server" CssClass="form-row search-list-form" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="col">
            <asp:Label ID="lblSearchModulos" runat="server" Text="Nombre / Grupo: " CssClass="ui-widget-header"></asp:Label>
            <asp:TextBox ID="txtSearchModulos" runat="server" ValidationGroup="searchProfiles"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="searchProfiles"></asp:Button>
            <asp:Button ID="cmdAddNew" runat="server" Text="Adicionar" CssClass="PCGButton button"></asp:Button>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="TableModulos" ContentPlaceHolderID="table" runat="server">
    <asp:GridView ID="gvwModulos" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content list" AllowSorting="True"  >
        <Columns>
            <asp:BoundField DataField="pk_codigo" HeaderText="Código">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
            </asp:BoundField>
            <asp:BoundField DataField="val_nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="val_url" HeaderText="URL" />
            <asp:BoundField DataField="val_url_icono" HeaderText="Icono" />
            <asp:BoundField DataField="ind_estado" HeaderText="Estado" />
            <asp:ButtonField ButtonType="Button" Text="Editar" CommandName="edit">
                                    <ControlStyle CssClass="GridEditButton" />
                                </asp:ButtonField>
        </Columns>

        <HeaderStyle CssClass="ui-widget-header" />
        <PagerSettings Visible="False" />
        <RowStyle CssClass="ui-widget-content" />

    </asp:GridView>
</asp:Content>

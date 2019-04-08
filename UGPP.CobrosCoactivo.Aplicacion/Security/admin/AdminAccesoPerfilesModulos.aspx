<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Bandeja.Master" CodeBehind="AdminAccesoPerfilesModulos.aspx.vb" Inherits="coactivosyp.AdminAccesoPerfilesModulos1" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #encabezado{display:none;}
        .list {
            margin-right: 1px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="headPageLinks" runat="server" />

<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server">
    <asp:Panel ID="pnlSearchModulosAcceso" runat="server" CssClass="form-row search-list-form" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="col">
            <asp:Label ID="lblSelectPerfil" runat="server" Text="Perfil: " CssClass="ui-widget-header" AssociatedControlID="ddlPerfiles"></asp:Label>
            <asp:DropDownList ID="ddlPerfiles" runat="server">
                <asp:ListItem Value="0"> -</asp:ListItem>
            </asp:DropDownList>
            <asp:CompareValidator ControlToValidate="ddlPerfiles" ID="ddlPerfilesValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" />

        </div>
        <div class="col"> 
            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button"></asp:Button>
        </div>
    </asp:Panel>
</asp:Content> 

<asp:Content ID="tableContent" ContentPlaceHolderID="InboxTable" runat="server">
    <asp:TextBox ID="txtIdPerfil" runat="server" Visible="False"></asp:TextBox> 
    <asp:GridView ID="gvwModulosAcceso" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content list" AllowSorting="True" OnRowCommand="gvwModulosAcceso_RowCommand"> 
        <Columns>
            <asp:BoundField DataField="pk_codigo" HeaderText="Código">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" /> 
            </asp:BoundField>
            <asp:BoundField DataField="val_nombre" HeaderText="Módulo" />
            <asp:TemplateField>
                <ItemTemplate> 
                    <asp:Image ID="imgEstadoAccesoModulo" runat="server" /> 
                </ItemTemplate>
                <HeaderStyle CssClass="xsmall-column" />
                <ItemStyle CssClass="xsmall-column" />
            </asp:TemplateField>
            <asp:ButtonField Text="Autorizar" CommandName="cmdUpdateAccess" ButtonType="Button">  
                <ControlStyle CssClass="GridEditButton button" />
                <HeaderStyle CssClass="small-column" /> 
                <ItemStyle CssClass="small-column" />
            </asp:ButtonField>
            <asp:BoundField> 
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />  
            </asp:BoundField>
        </Columns> 

        <HeaderStyle CssClass="ui-widget-header" /> 
        <PagerSettings Visible="False" />    
        <RowStyle CssClass="ui-widget-content" />

    </asp:GridView>
</asp:Content>
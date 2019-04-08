<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Bandeja.Master" CodeBehind="AdminAccesoPerfilesPaginas.aspx.vb" Inherits="coactivosyp.AdminAccesoPerfilesPaginas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #encabezado{display:none;}
        .list {
            margin-right: 1px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="headPageLinks" runat="server" />

<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server">
    <asp:Panel ID="pnlSearchPaginasAcceso" runat="server" CssClass="form-row search-list-form" ClientIDMode="Static" DefaultButton="cmdSearch">
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
    <asp:GridView ID="gvwPaginasAcceso" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content list tbl-indentation" AllowSorting="True" OnRowCommand="gvwPaginasAcceso_RowCommand"> 
        <Columns>
            <asp:BoundField DataField="pk_codigo" HeaderText="Código">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />  
            </asp:BoundField>
            <asp:BoundField DataField="ind_nivel" HeaderText="">
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />  
            </asp:BoundField> 
            <asp:BoundField DataField="val_nombre" HeaderText="Página" />
            <asp:TemplateField HeaderText="Ver">
                <ItemTemplate> 
                    <asp:Image ID="imgEstadoPuedeVer" runat="server" />
                </ItemTemplate>
                <HeaderStyle CssClass="xsmall-column" />
                <ItemStyle CssClass="xsmall-column" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Editar">
                <ItemTemplate> 
                    <asp:Image ID="imgEstadoPuedeEditar" runat="server" /> 
                </ItemTemplate>
                <HeaderStyle CssClass="xsmall-column" />
                <ItemStyle CssClass="xsmall-column" />
            </asp:TemplateField>
            <asp:ButtonField Text="Puede Ver" CommandName="cmdUpdateVer" ButtonType="Button" HeaderText="Puede Ver">  
                <ControlStyle CssClass="GridEditButton button" />
                <HeaderStyle CssClass="small-column" /> 
                <ItemStyle CssClass="small-column" />
            </asp:ButtonField>
            <asp:ButtonField Text="Puede Editar" CommandName="cmdUpdateEditar" ButtonType="Button" HeaderText="Puede Editar">  
                <ControlStyle CssClass="GridEditButton button" />
                <HeaderStyle CssClass="small-column" /> 
                <ItemStyle CssClass="small-column" />
            </asp:ButtonField>
            <asp:BoundField> 
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />  
            </asp:BoundField>
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

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Bandeja.Master" CodeBehind="Priorizacion.aspx.vb" Inherits="coactivosyp.Priorizacion" %>

<%@ Register TagPrefix="uc1" TagName="Bandeja" Src="~/Security/Controles/BandejaAprobacionReasignacionPriorizacion.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" ></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="headPageLinks" runat="server">
    <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
        <img alt ="Regresar al listado de módulos"  src="<%=ResolveClientUrl("~/Security/images/icons/regresar.png") %>" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" />
    </asp:LinkButton>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server"></asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="InboxTable" runat="server">
    <uc1:Bandeja id="BandejaPriorizacion" runat="server"  />
</asp:Content>

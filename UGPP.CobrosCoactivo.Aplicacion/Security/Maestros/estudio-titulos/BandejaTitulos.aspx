<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Bandeja.Master" CodeBehind="BandejaTitulos.aspx.vb" Inherits="coactivosyp.BandejaTitulos1" %>
<%@ Register TagPrefix="uc1" TagName="BandejaEstudioTitulos" Src="~/Security/Controles/BandejaEstudioTitulos.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="headPageLinks" runat="server" />
<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server" />

<asp:Content ID="Content4" ContentPlaceHolderID="InboxTable" runat="server">
    <uc1:BandejaEstudioTitulos ID="BandejaEsudioTitulos" runat="server" esEstudioTitulos="1" />

    <script type="text/javascript">
        jQuery(document).ready(function($){
            if (inIframe()) {
                window.parent.location.href = "<%=ResolveClientUrl("~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx") %>";
            }
        });
    </script>
</asp:Content>

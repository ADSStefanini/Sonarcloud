<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/BandejaSinLogin.Master" CodeBehind="BandejaAreaOrigen.aspx.vb" Inherits="coactivosyp.test" %>
<%@ Register Src="~/Security/Controles/BandejaTitulos.ascx" TagPrefix="uc1" TagName="BandejaTitulos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server" />

<asp:Content ID="Content4" ContentPlaceHolderID="InboxTable" runat="server">
    <uc1:BandejaTitulos runat="server" ID="BandejaTitulos" />

    <script type="text/javascript">
        jQuery(document).ready(function ($) {
            <% If (Len(Request("reloadParent")) And Request("reloadParent") = "1") Then %>
            //console.log(baseUrl + "Security/modulos/maestro-acceso.aspx");
            window.parent.location.href = baseUrl + "Security/modulos/maestro-acceso.aspx";
            <% End If %>
        });
    </script>
</asp:Content>

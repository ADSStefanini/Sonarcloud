<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="paginador.ascx.vb" Inherits="coactivosyp.paginador" %>
    <%--
        El ASPX que implemente este control debe llamar estos recursos para el funcionamiento de este    
            <link href="<%=ResolveClientUrl("~/css/main.css") %>" rel="stylesheet" />
            <link href="<%=ResolveClientUrl("~/Security/Maestros/css/redmond/jquery-ui-1.10.4.custom.css") %>" rel="stylesheet" />
            <script src="<%=ResolveClientUrl("~/Security/Maestros/js/jquery-ui-1.10.4.custom.min.js") %>"></script>
    --%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('submit,input:button,input:submit').button();
        });
    </script>
<asp:Panel ID="pnlPaginador" runat="server" class="ui-widget-content">
    <div class="col">
        <asp:Button ID="cmdFirst" runat="server" Text="Primero" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
        <asp:Button ID="cmdPrevious" runat="server" Text="Anterior" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
        <asp:Button ID="cmdNext" runat="server" Text="Siguiente" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
        <asp:Button ID="cmdLast" runat="server" Text="Ultimo" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
    </div>
</asp:Panel>

<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BandejaTitulos.ascx.vb" Inherits="coactivosyp.BandejaTitulos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />

<div class="ui-widget-content ui-widget">
    <asp:HiddenField ID="hdnRedirectParent" runat="server" value="0" ClientIDMode="Static"/>

    <asp:Panel ID="pnlFiltroBandejaTitulos" runat="server" CssClass="form-row search-list-form search-list-form-large" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="row">
            <div class="col-md-6">
                <asp:Label ID="lblNoTitulo" runat="server" Text="No. T&iacute;tulo: " CssClass="ui-widget-header" AssociatedControlID="txtNoTitulo"></asp:Label>
                <asp:TextBox ID="txtNoTitulo" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6" style="visibility:hidden">
                <asp:Label ID="lblEstadoOperativo" runat="server" Text="Estado operativo: " CssClass="ui-widget-header" AssociatedControlID="ddlEstadoOperativo"></asp:Label>
                <asp:DropDownList ID="ddlEstadoOperativo" runat="server">
                    <asp:ListItem Value="0"> -</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6">
                <asp:Label ID="lblNumIdentificacionDeudor" runat="server" Text="N&uacute;mero Identificaci&oacute;n Deudor:: "  CssClass="ui-widget-header" AssociatedControlID="txtNumIdentificacionDeudor"></asp:Label>
                <asp:TextBox ID="txtNumIdentificacionDeudor" runat="server"></asp:TextBox>
            </div>
            
            <div class="col-md-6">
                <asp:Label ID="lblFechaTitulo" runat="server" Text="Fecha de Envio a Cobranzas: " CssClass="ui-widget-header" AssociatedControlID="ddlEstadoProcesal"></asp:Label>
                <asp:TextBox ID="fecFechaEnvioTituloInicio" runat="server" CssClass="calendar"></asp:TextBox>
                <asp:ImageButton ID="imgBtnBorraFechaTituloInicio" runat="server" ImageUrl="~/Security/images/icons/borrar16x16.png" ToolTip="Borrar Fecha de Envio a Cobranzas Inicio" Style="vertical-align: middle;" />
                <asp:TextBox ID="fecFechaEnvioTituloFin" runat="server" CssClass="calendar"></asp:TextBox>
                <asp:ImageButton ID="imgBtnBorraFechaTituloFin" runat="server" ImageUrl="~/Security/images/icons/borrar16x16.png" ToolTip="Borrar Fecha de Envio a Cobranzas Fin" Style="vertical-align: middle;" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:Label ID="lblNombreDeudor" runat="server" Text="Nombre Deudor: " CssClass="ui-widget-header" AssociatedControlID="txtNombreDeudor"></asp:Label>
                <asp:TextBox ID="txtNombreDeudor" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="titleFilters"></asp:Button>
                <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" CssClass="button" />
            </div>
        </div>
        <div style="visibility: hidden;height:0;width:0;">
            <asp:Label ID="lblEstadoProcesal" runat="server" Text="Estado Procesal: " CssClass="ui-widget-header" AssociatedControlID="ddlEstadoProcesal"></asp:Label>
            <asp:DropDownList ID="ddlEstadoProcesal" runat="server">
                <asp:ListItem Value="0"> -</asp:ListItem>
            </asp:DropDownList>
        </div>
    </asp:Panel>
</div>

<asp:Panel ID="pnlGridBandejaTitulos" runat="server" CssClass="form-row pnlGridContent" ClientIDMode="Static" DefaultButton="cmdSearch">

    <asp:GridView ID="grdBandejaTituloAO" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowPaging="true" CellPadding="4" PageSize="14" ClientIDMode="Static"  PagerSettings-Visible="False">
        <Columns>
            <asp:BoundField DataField="ID_TAREA_ASIGNADA" Visible="False"></asp:BoundField>
            <asp:BoundField DataField="IDUNICO" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="NROTITULO" HeaderText="No. Título"></asp:BoundField>
            <asp:BoundField DataField="FCHEXPEDICIONTITULO" HeaderText="Fecha de expedición del título"></asp:BoundField>
            <asp:BoundField DataField="NOMBREDEUDOR" HeaderText="Nombre del deudor"></asp:BoundField>
            <asp:BoundField DataField="NRONITCEDULA" HeaderText="NIT / CC"></asp:BoundField>
            <asp:BoundField DataField="TIPOOBLIGACION" HeaderText="Tipo de obligación"></asp:BoundField>
            <asp:BoundField DataField="TOTALOBLIGACION" HeaderText="Total Deuda">
                <ItemStyle CssClass="format-number" />
            </asp:BoundField>
            <asp:BoundField DataField="FEC_ENTREGA_GESTOR" HeaderText="Fecha entrega Estudio títulos"></asp:BoundField>
            <asp:BoundField DataField="FCHLIMITE" HeaderText="Fecha límite"></asp:BoundField>
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:Label ID="LblColorAccion" runat="server" Text='<%# Eval("COLOR") %>' Visible="False"></asp:Label>
                    <asp:Label ID="LblAcciones" runat="server" Text='<%# Eval("ID_ESTADO_OPERATIVOS") %>' Visible="False"></asp:Label>
                    <asp:Button ID="BtnAcciones" class="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" runat="server" CommandName="ClickRedireccionar" CommandArgument='<%#Eval("ID_TAREA_ASIGNADA")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
        <RowStyle CssClass="ui-widget-content" />
        <AlternatingRowStyle />
    </asp:GridView>
    <uc1:Paginador id="PaginadorGridView" runat="server"  gridViewIdClient="grdBandejaTituloAO"  OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
</asp:Panel>


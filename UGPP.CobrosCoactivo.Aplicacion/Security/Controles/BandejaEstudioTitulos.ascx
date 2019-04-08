<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BandejaEstudioTitulos.ascx.vb" Inherits="coactivosyp.BandejaEstudioTitulos" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />--%>

<%-- Controles de Usuario para asignación y priorización --%>
<%@ Register TagPrefix="ucPriorizacion" TagName="SolicitudPriorizacion" Src="~/Security/Controles/SolicitudPriorizacion.ascx" %>
<%@ Register TagPrefix="ucReasignacion" TagName="SolicitudReasignacion" Src="~/Security/Controles/SolicitudReasignacion.ascx" %>

<div class="ui-widget-content ui-widget">
    <asp:Panel ID="pnlFiltroBandejaTitulos" runat="server" CssClass="form-row search-list-form search-list-form-large" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblNoTitulo" runat="server" Text="No. T&iacute;tulo: " CssClass="ui-widget-header" AssociatedControlID="txtNoTitulo"></asp:Label>
                <asp:TextBox ID="txtNoTitulo" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-6">
                <asp:Label ID="lblEstadoOperativo" runat="server" Text="Estado Operativo: " CssClass="ui-widget-header" AssociatedControlID="ddlEstadoOperativo"></asp:Label>
                <asp:DropDownList ID="ddlEstadoOperativo" runat="server">
                    <asp:ListItem Value="0"> -</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-2">
                <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="titleFilters"></asp:Button>
            </div>

        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="lblNumIdentificacionDeudor" runat="server" Text="N&uacute;mero Identificaci&oacute;n Deudor: " CssClass="ui-widget-header" AssociatedControlID="txtNumIdentificacionDeudor"></asp:Label>
                <asp:TextBox ID="txtNumIdentificacionDeudor" runat="server"></asp:TextBox>
            </div>

            <div class="col-md-6">
                <asp:Label ID="lblFechaTitulo" runat="server" Text="Fecha de envio a cobranzas: " CssClass="ui-widget-header" AssociatedControlID="fecFechaEnvioTituloInicio"></asp:Label>
                <asp:TextBox ID="fecFechaEnvioTituloInicio" runat="server" CssClass="calendar" />
                <asp:ImageButton ID="imgBtnBorraFechaTituloInicio" runat="server" ImageUrl="~/Security/images/icons/borrar16x16.png" ToolTip="Borrar Fecha de Envio a Cobranzas Inicio" Style="vertical-align: middle;" />
                <asp:TextBox ID="fecFechaEnvioTituloFin" runat="server" CssClass="calendar" />
                <asp:ImageButton ID="imgBtnBorraFechaTituloFin" runat="server" ImageUrl="~/Security/images/icons/borrar16x16.png" ToolTip="Borrar Fecha de Envio a Cobranzas Fin" Style="vertical-align: middle;" />
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" CssClass="button" />
            </div>
        </div>
        <div class="row">
            <%--<div class="col-md-4">
                <asp:Label ID="lblEstadoProcesal" runat="server" Text="Etado Procesal: " CssClass="ui-widget-header" AssociatedControlID="ddlEstadoProcesal"></asp:Label>
                <asp:DropDownList ID="ddlEstadoProcesal" runat="server">
                    <asp:ListItem Value="0"> -</asp:ListItem>
                </asp:DropDownList>
            </div>--%>
            <div class="col-md-4">
                <asp:Label ID="lblNombreDeudor" runat="server" Text="Nombre Deudor: " CssClass="ui-widget-header" AssociatedControlID="txtNombreDeudor"></asp:Label>
                <asp:TextBox ID="txtNombreDeudor" runat="server"></asp:TextBox>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlGridBandejaTitulos" runat="server" CssClass="form-row pnlGridContent" ClientIDMode="Static">
        <div id="noTitulosAsignados" class="alert alert-warning" runat="server" visible="False">
            <asp:Label ID="txtSinExpedientesAsignados" runat="server"></asp:Label>
        </div>
        <asp:GridView ID="grdBandejaTituloAO" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True" CellPadding="4" PagerSettings-Visible="False" AllowPaging="True">
            <Columns>
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
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkReasignar" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField Text="Priorizar" CommandName="cmdPriorizar" ButtonType="Button">
                    <ControlStyle CssClass="GridEditButton button" />
                    <HeaderStyle CssClass="small-column" />
                    <ItemStyle CssClass="small-column" />
                </asp:ButtonField>
                <asp:ButtonField Text="Continuar" CommandName="cmdContinuar" ButtonType="Button">
                    <ControlStyle CssClass="GridEditButton button" />
                    <HeaderStyle CssClass="small-column" />
                    <ItemStyle CssClass="small-column" />
                </asp:ButtonField>

                <asp:BoundField DataField="VAL_PRIORIDAD">
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                </asp:BoundField>
                <asp:BoundField DataField="ID_TAREA_ASIGNADA" HeaderText="Tarea Asiganada">
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                </asp:BoundField>
                <asp:BoundField DataField="ID_ESTADO_OPERATIVOS" HeaderText="Estado Operativo">
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
            <RowStyle CssClass="ui-widget-content" />
            <AlternatingRowStyle />
        </asp:GridView>
        <uc1:Paginador ID="PaginadorGridView" runat="server" gridViewIdClient="grdBandejaTituloAO" OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
        <%--<asp:Button ID="cmdCambiarEstado" runat="server" Text="Cambiar Estado" CssClass="PCGButton button"></asp:Button>--%>
        <%--<asp:Button ID="cmdReasignar" runat="server" Text="Solicitar reasignación" CssClass="PCGButton button"></asp:Button>--%>
    </asp:Panel>

    <asp:Button ID="btnSolicitarReasignacion" runat="server" Text="Solicitar reasignación" CssClass="PCGButton button" />

    <!-- Solicitudes sobre el expediente -->
    <ucReasignacion:SolicitudReasignacion ID="SolicitudReasignacionPanel" runat="server" />
    <ucPriorizacion:SolicitudPriorizacion ID="SolicitudPriorizacionControl" runat="server" />
</div>

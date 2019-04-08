<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BandejaAprobacionReasignacionPriorizacion.ascx.vb" Inherits="coactivosyp.BandejaAprobacionReasignacionPriorizacion" %>

<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<%@ Register TagPrefix="uc2" TagName="VerSolicitud" Src="~/Security/Controles/VerSolicitud.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ResSolicitud" Src="~/Security/Controles/RespuestaSolicitud.ascx" %>

<!-- Campos ocultos de control -->
<asp:HiddenField ID="hdnTipoSolicitud" runat="server" />
<asp:HiddenField ID="hdnTipoUsuario" runat="server" />

<div class="ui-widget-content ui-widget">
    <!-- Formulario de búsqueda -->
    <asp:Panel ID="pnlFiltroBandejaReasignacion" runat="server" CssClass="form-row search-list-form search-list-form-large" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="col-md-4">
            <asp:Label ID="lblNoTitulo" runat="server" Text="No. T&iacute;tulo: " CssClass="ui-widget-header" AssociatedControlID="txtNoTitulo" />
            <asp:TextBox ID="txtNoTitulo" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblSearchEFINROEXP" runat="server" Text="No. expediente cobranzas:" CssClass="ui-widget-header" AssociatedControlID="txtSearchEFINROEXP" />
            <asp:TextBox ID="txtSearchEFINROEXP" runat="server"></asp:TextBox>
        </div>

        <!-- Bóton buscar -->
        <div class="col-md-2">
            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="titleFilters" />
        </div>

        <div class="col-md-4">
            <asp:Label ID="Label1" runat="server" Text="Gestor:" CssClass="ui-widget-header" AssociatedControlID="ddlGestor" />
            <asp:DropDownList ID="ddlGestor" runat="server" AppendDataBoundItems="true" CssClass="col-md-5">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label2" runat="server" Text="Estado Solicitud:" CssClass="ui-widget-header" AssociatedControlID="txtNoTitulo" />
            <asp:DropDownList ID="ddlEstadoSolicitud" runat="server" AppendDataBoundItems="true" CssClass="col-md-5">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlGridBandejaAprobacion" runat="server" CssClass="form-row pnlGridContent" ClientIDMode="Static">
        <!--Inbox -->

        <div class="form-group row">
            <asp:Label ID="lblSinPermisos" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error"/>
        </div>

        <div class="form-group row">
            <asp:Label ID="lblNoSolicitudes" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" />
        </div>

        <asp:Label ID="lblRecordsFound" runat="server" CssClass="lblRecorsFond" />

        <asp:GridView ID="grdBandejaReasignacion" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content bandeja-solicitudes" AllowSorting="False" PagerSettings-Visible="False" AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="EFINROEXP" HeaderText="Expediente" />
                <asp:BoundField DataField="ID_UNICO_TITULO" HeaderText="Título" />
                <asp:BoundField DataField="GESTOR_SOLICITANTE" HeaderText="Gestor Solicitante" />
                <asp:BoundField DataField="FECHASOLICITUD" HeaderText="Fecha Solicitud" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ESTADO_ACTUAL" HeaderText="Estado Actual" />
                <asp:BoundField DataField="ESTADO_APROBACION" HeaderText="Estado Aprobacion" />
                <asp:BoundField DataField="TIPO_SOLICITUD" HeaderText="Tipo Solicitud" />
                <asp:BoundField DataField="GESTOR_SOLICITADO" HeaderText="Gestor Solicitado" />
                <asp:BoundField DataField="GESTOR_APROBADOR" HeaderText="Gestor Aprobador" />

                <%-- Para seleccionar las solicitudes de aprobaciones --%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkReasignar" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Para solicitar la priorización--%>
                <asp:ButtonField ButtonType="Button" CommandName="cmdView" Text="Ver">
                    <ControlStyle CssClass="GridEditButton button" />
                </asp:ButtonField>

                <asp:BoundField DataField="ID_TAREA_ASIGNADA">
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                </asp:BoundField>
                <asp:BoundField DataField="ID_TAREA_SOLICITUD">
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                </asp:BoundField>
                <asp:BoundField DataField="COD_ESTADO_SOLICITUD">
                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                </asp:BoundField>

            </Columns>
            <HeaderStyle CssClass="ui-widget-header" />
            <RowStyle CssClass="ui-widget-content" />
            <AlternatingRowStyle />
        </asp:GridView>

        

        <div style="position: relative;">
            <uc1:Paginador ID="PaginadorGridView" runat="server" gridViewIdClient="grdBandejaReasignacion" OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
            <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="PCGButton button btnAprobarInferior" ValidationGroup="titleFilters" />
        </div>

        <uc2:VerSolicitud ID="VerSolicitudUC" runat="server" />
        <uc3:ResSolicitud ID="ResSolicitudUC" runat="server" />
    </asp:Panel>
</div>

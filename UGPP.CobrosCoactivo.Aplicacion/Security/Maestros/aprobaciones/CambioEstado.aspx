<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Bandeja.Master" CodeBehind="CambioEstado.aspx.vb" Inherits="coactivosyp.CambioEstado" %>

<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<%@ Register TagPrefix="uc2" TagName="VerSolicitud" Src="~/Security/Controles/VerSolicitud.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ResSolicitud" Src="~/Security/Controles/RespuestaSolicitud.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .button {
            width: 61px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="headPageLinks" runat="server">
    <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
        <img alt ="Regresar al listado de módulos"  src="<%=ResolveClientUrl("~/Security/images/icons/regresar.png") %>" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" />
    </asp:LinkButton>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SearchForm" runat="server">
    <!-- Formulario de búsqueda -->
    <asp:Panel ID="pnlFiltroBandejaReasignacion" runat="server" CssClass="form-row search-list-form search-list-form-large" ClientIDMode="Static" DefaultButton="cmdSearch">
        <div class="col-md-4">
            <asp:Label ID="lblSearchEFINROEXP" runat="server" Text="No. expediente cobranzas:" CssClass="ui-widget-header" AssociatedControlID="txtSearchEFINROEXP" />
            <asp:TextBox ID="txtSearchEFINROEXP" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label1" runat="server" Text="Gestor:" CssClass="ui-widget-header" AssociatedControlID="ddlGestor" />
            <asp:DropDownList ID="ddlGestor" runat="server" AppendDataBoundItems="true" CssClass="col-md-5">
                <asp:ListItem Text="" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Bóton buscar -->
        <div class="col-md-2">
            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button" ValidationGroup="titleFilters" />
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="InboxTable" runat="server">
    <!--Inbox -->

    <asp:Label ID="lblRecordsFound" runat="server" CssClass="lblRecorsFond" />

    <div class="form-group row">
        <asp:Label ID="lblSinPermisos" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error" />
    </div>

    <div class="form-group row">
        <asp:Label ID="lblNoSolicitudes" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" />
    </div>

    <asp:GridView ID="grdBandejaAprobacion" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content bandeja-solicitudes" AllowSorting="False" PagerSettings-Visible="False" AllowPaging="True">
        <Columns>
            <asp:BoundField DataField="EFINROEXP" HeaderText="Nro Exp" />
            <asp:BoundField DataField="USUARIO_SOLICITUD" HeaderText="Gestor" />
            <asp:BoundField DataField="FECHA_SOLICITUD" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="ESTADOS_PROCESO_ACTUAL" HeaderText="Estado Actual" />
            <asp:BoundField DataField="ESTADOS_PROCESO_SOLICITADO" HeaderText="Estado Solicitado" />
            <asp:BoundField DataField="ESTADO_SOLICITUD" HeaderText="Estado Solicitud" />
            <asp:BoundField DataField="APROBADOR" HeaderText="Aprobador" />

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

            <asp:BoundField DataField="ID_SOLICITUD_REASIGNACION">
                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
            </asp:BoundField>

        </Columns>
        <HeaderStyle CssClass="ui-widget-header" />
        <RowStyle CssClass="ui-widget-content" />
        <AlternatingRowStyle />
    </asp:GridView>

    <div style="position: relative;">
        <uc1:Paginador ID="PaginadorGridView" runat="server" gridViewIdClient="grdBandejaAprobacion" OnEventActualizarGrid="PaginadorGridView_EventActualizarGrid" />
        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" CssClass="PCGButton button btnAprobarInferior" ValidationGroup="titleFilters" />
    </div>

    <uc2:VerSolicitud ID="VerSolicitudUC" runat="server" />
    <uc3:ResSolicitud ID="ResSolicitudUC" runat="server" />
</asp:Content>

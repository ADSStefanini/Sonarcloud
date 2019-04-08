<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Formulario.Master" CodeBehind="ClasificacionManual2.aspx.vb" Inherits="coactivosyp.ClasificacionManual2" %>

<%@ Register TagPrefix="uc1" TagName="Documento" Src="~/Security/Controles/DatosDocumento.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="formContent" runat="server">
    <header>
        <style>
            .minHeightDiv {
                min-height: 350px;
            }
        </style>
        <script type="text/javascript">
            function mostrarLoadingC() {
                $("#loadingClasificacion").addClass("loading");
            };
        </script>
        <link href="../../css/main.css" rel="stylesheet" />

    </header>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnGuardar2" CssClass="minHeightDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div runat="server" id="loadingClasificacion" clientidmode="Static"></div>
                <asp:HiddenField ID="hdnIdTitulo" runat="server" />
                <asp:HiddenField ID="hdnExpediente" runat="server" />
                <%--estado procesal provisional--%>
                <asp:HiddenField ID="hdnEPP" runat="server" Value="" />
                <asp:HiddenField ID="hdnEtapaEPP" runat="server" Value="" />

                <div class="form-group row">
                    <asp:Label ID="lblNoClasificable" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" />
                </div>

                <div class="form-group row">
                    <asp:Label ID="lblTipoPersona" runat="server" Text="¿Tipo de persona?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                    <div class="col-sm-9">
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlTipoPersona" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoPersona_SelectedIndexChanged">
                            <asp:ListItem Value="0" Text="--Seleccione--" />
                            <asp:ListItem Value="1" Text="Juridica" />
                            <asp:ListItem Value="2" Text="Natural" />
                        </asp:DropDownList>
                        <asp:CompareValidator ControlToValidate="ddlTipoPersona" ID="ddlTipoPersonaValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                    </div>
                </div>

                <div id="MatriculaMercantil" runat="server" clientidmode="Static" visible="false">
                    <div class="form-group row">
                        <asp:Label ID="lblMatriculaMercantil" runat="server" Text="¿Matricula mercantil vigente?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <div class="col-sm-12 row file-inline-upload">
                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2 float-left" ID="ddlMatriculaMercantil" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlMatriculaMercantil_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Seleccione--" />
                                    <asp:ListItem Value="1" Text="Si" />
                                    <asp:ListItem Value="2" Text="No" />
                                </asp:DropDownList>
                                <asp:CompareValidator ControlToValidate="ddlMatriculaMercantil" ID="ddlMatriculaMercantilValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                            </div>
                        </div>
                    </div>
                </div>

                <div id="PersonaViva" runat="server" clientidmode="Static" visible="false">
                    <div class="form-group row">
                        <asp:Label ID="lblPersonaViva" runat="server" Text="¿Persona se encuentra viva?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <div class="col-sm-12 row file-inline-upload">
                                <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2 float-left" ID="ddlPersonaViva" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPersonaViva_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="--Seleccione--" />
                                    <asp:ListItem Value="1" Text="Si" />
                                    <asp:ListItem Value="2" Text="No" />
                                </asp:DropDownList>
                                <asp:CompareValidator ControlToValidate="ddlPersonaViva" ID="ddlPersonaVivaValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                                <div class="col-sm-9 row" id="load-doacument-2"></div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="div_upload_file" runat="server" clientidmode="Static" class="file-inline-upload" visible="false">
                    <div class="form-group row">

                        <div class="col-sm-3 row" style="margin-left: 5px;">
                            <uc1:Documento ID="newDoc" runat="server" idTitulo="1" idDocumento="40" UpdateMode="Conditional" />
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlProcesoEspecialJuridica" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblProcesoEspecialJuridica" runat="server" Text="¿Esta inmerso en proceso especial (concursal o sucesión)?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlProcesoEspecialJuridica" ClientIDMode="Static" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="--Seleccione--" />
                                <asp:ListItem Value="1" Text="Si" />
                                <asp:ListItem Value="2" Text="No" />
                            </asp:DropDownList>
                            <asp:CompareValidator ControlToValidate="ddlProcesoEspecialJuridica" ID="ddlProcesoEspecialJuridicaValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlTipoProcesoJuridica" runat="server">
                    <div class="form-group row">

                        <asp:Label ID="lblTipoProcesoJuridica" runat="server" Text="¿Tipo de proceso?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlTipoProcesoJuridica" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoProcesoJuridica_SelectedIndexChanged" />
                            <asp:CompareValidator ControlToValidate="ddlTipoProcesoJuridica" ID="ddlTipoProcesoJuridicaValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>

                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlProcesoEspecialNatural" runat="server" Visible="false">
                    <div class="form-group row">
                        <asp:Label ID="lblProcesoEspecialNatural" runat="server" Text="¿Esta inmerso en proceso especial (concursal o sucesión)?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlProcesoEspecialNatural" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlProcesoEspecialNatural_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="--Seleccione--" />
                                <asp:ListItem Value="1" Text="Si" />
                                <asp:ListItem Value="2" Text="No" />
                            </asp:DropDownList>
                            <asp:CompareValidator ControlToValidate="ddlProcesoEspecialNatural" ID="ddlProcesoEspecialNaturalValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlTipoProcesoNatural" runat="server">
                    <div class="form-group row">

                        <asp:Label ID="lblTipoProcesoNatural" runat="server" Text="¿Tipo de proceso?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlTipoProcesoNatural" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoProcesoNatural_SelectedIndexChanged" />
                            <asp:CompareValidator ControlToValidate="ddlTipoProcesoNatural" ID="ddlTipoProcesoNaturalValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlBeneficioTributario" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblBeneficioTributario" runat="server" Text="¿Tiene beneficio tributario?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlBeneficioTributario" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlBeneficioTributario_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="--Seleccione--" />
                                <asp:ListItem Value="1" Text="Si" />
                                <asp:ListItem Value="2" Text="No" />
                            </asp:DropDownList>
                            <asp:CompareValidator ControlToValidate="ddlBeneficioTributario" ID="ddlBeneficioTributarioValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlPagosDeudor" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblPagosDeudor" runat="server" Text="¿Deudor realizo pagos?" CssClass="col-sm-2 col-form-label col-form-label-sm style4" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:DropDownList runat="server" CssClass="form-control form-control-sm col-sm-2" ID="ddlPagosDeudor" ClientIDMode="Static" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="--Seleccione--" />
                                <asp:ListItem Value="1" Text="Si" />
                                <asp:ListItem Value="2" Text="No" />
                            </asp:DropDownList>
                            <asp:CompareValidator ControlToValidate="ddlPagosDeudor" ID="ddlPagosDeudorValidator" CssClass="error-msg" runat="server" Display="Dynamic" Operator="NotEqual" ValueToCompare="0" Type="Integer" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlNumeroRadicado" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblNumeroRadicado" runat="server" Text="Número radicado" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtNumeroRadicado" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="txtNumeroRadicado" runat="server" CssClass="numeros" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="reqNumeroRadicado" runat="server" ErrorMessage="" ControlToValidate="txtNumeroRadicado" CssClass="error-msg" ValidationGroup="ClasificacionManualGroup" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlObservaciones" runat="server">
                    <div class="form-group row">
                        <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones" CssClass="col-sm-2 col-form-label col-form-label-sm style4 large-label" ClientIDMode="Static" />
                        <div class="col-sm-9">
                            <asp:TextBox ID="TxtAreaObservaciones" TextMode="multiline" Columns="50" Rows="5" runat="server" ClientIDMode="Static" />
                        </div>
                    </div>
                </asp:Panel>
                <div class="col-sm-12">
                    <asp:Button ID="btnGuardar2" runat="server" Text="Guardar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ClientIDMode="Static" ValidationGroup="ClasificacionManualGroup" OnClientClick="mostrarLoadingC();" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                </div>

                <asp:Panel ID="pnlMensaje" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="background: #fff;" Visible="false">
                    <div class="col-sm-12">
                        <asp:Label ID="lblMensajeClasificacion" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" />
                    </div>
                    <div class="col-sm-12">
                        <asp:Button ID="btnAceptarClasificacion" runat="server" Text="Aceptar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>



    <script src="<%=ResolveClientUrl("~/js/loadDocs.js") %>" type="text/javascript"></script>
</asp:Content>

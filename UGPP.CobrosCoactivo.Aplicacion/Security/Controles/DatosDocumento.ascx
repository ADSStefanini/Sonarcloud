<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DatosDocumento.ascx.vb" Inherits="coactivosyp.DatosDocumento" %>

<%-- 
    Para el funcionamiento de este módulo es necesario agregar la siguiente referecia js en el aspx que lo implementa
    <script src="<%=ResolveClientUrl("~/js/loadDocs.js") %>" type="text/javascript"></script>
--%>

<asp:Panel ID="pnlCargaDocumento" runat="server" CssClass="form-content upload-doc">

    <asp:HiddenField runat="server" ID="HdnExtValidas" Value="" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdnModificaDoc" Value="False" ClientIDMode="Static" />

    <div class="col-sm-12">
        <asp:Label ID="lblErrorDatos" runat="server" Text="" Visible="False"></asp:Label> 
    </div>

    <div class="form-group row">
        <asp:Label ID="lblUploadDpcumento" runat="server" Text="Documento*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="" />
        <div class="col-sm-5">
            <asp:Button ID="btnCargarPopUp" runat="server" Text="Cargar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ClientIDMode="Static" OnClientClick="capturarDatosDocPrecargado(); $('#docModal').modal(); return false" />
            <asp:HyperLink ID="hlinkViewDocOut" runat="server" Target="_blank" ClientIDMode="Static" Text="Ver archivo cargado" CssClass="link-view-file" Style="display: none;" />
            <%--<span id="lblDocName" style="display: none;"></span>--%>
            <asp:Label ID="lblDocName" runat="server" Text="" Style="display: none;" ClientIDMode="Static" />
        </div>
        <div class="col-sm-5">
<%--            <a id="linkVer" target="_blank">Ver</a>--%>
            <asp:hyperlink id="linkver" runat="server" text="Ver" target="_blank" clientidmode="static"  CssClass="ui-button ui-widget ui-state-default ui-corner-all espacioLink" />
        </div>
        <hr />
    </div>

</asp:Panel>

<div id="tabsModal">
    <!-- Modal -->
    <div class="modal fade" id="docModal" role="dialog" aria-hidden="true" >
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">

                    <asp:Panel ID="PnlDoc" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="background: #fff;">
                        <asp:UpdatePanel ID="UpnlModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <hr />
                                <div>
                                    <div class="form-group row">
                                        <asp:Label ID="lblCarga" runat="server" Text="Documento*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="btnCargar" />
                                        <div class="col-sm-9">
                                            <asp:Button ID="btnCargar" OnClientClick="AddFileUpload($(this).prop('id')); return false;" runat="server" Text="Cargar" CssClass="form-control form-control-sm button ui-button ui-widget ui-state-default ui-corner-all" />
                                            <asp:HyperLink ID="hlinkViewDoc" runat="server" Target="_blank" ClientIDMode="Static" Text="Ver archivo cargado" CssClass="link-view-file" Style="display: none;" />
                                            <asp:HiddenField runat="server" ID="HdnPathFile" ClientIDMode="Static" />
                                            <asp:HiddenField runat="server" ID="HdnIdDoc" Value="0" />
                                            <asp:FileUpload runat="server" ID="FlUpEvidencias" AllowMultiple="false" onchange="CambioEnFileUpload($(this).prop('id'))" Style="visibility: collapse; height: 0px; width: 0px;" CausesValidation="True" />
                                            <asp:Label ID="lblErrorDoc" runat="server" Text="" CssClass="error-msg" />

                                            <%--<asp:RequiredFieldValidator ID="reqFlUpEvidencias" runat="server" ErrorMessage="HdnPathFile"  CssClass="error-msg" ValidationGroup="formDocumento" />--%>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <asp:Label ID="lblNumPaginas" runat="server" Text="Número Páginas*" CssClass="col-sm-2 col-form-label col-form-label-sm style4"/>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtNumPaginas" runat="server" CssClass="form-control form-control-sm numeros" ClientIDMode="Static" style="text-align:left" Text="1"  />
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <asp:Label ID="lblObservacionLegibilidad" runat="server" Text="Observación Legibilidad*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtObservacionLegibilidad" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Style="width: 400px;" ClientIDMode="Static" Text="LEGIBLE"  />                                         
                                        </div>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:Button ID="cmdAsignarDocumento2" runat="server" Text="Asignar Documento" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ClientIDMode="Static"  />
                                        <asp:Button ID="btnCerrarCargaDocumento" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Cancelar" ClientIDMode="Static" data-dismiss="modal" />
                                    </div>

                                    <asp:HiddenField ID="hdnResultUpload" runat="server" ClientIDMode="Static" />

                                </div>
                                <hr />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>
</div>
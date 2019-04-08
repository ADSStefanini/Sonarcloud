<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMAESTRO_TITULOS_AORIGEN.aspx.vb" Inherits="coactivosyp.EditMAESTRO_TITULOS_AORIGEN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="Paginador" Src="~/Security/Controles/paginador.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Documento" Src="~/Security/Controles/DatosDocumento.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar Título Ejecutivos </title>
    <%--<script type="text/javascript" src="js/jquery.min.js"></script>--%>
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <%--<script type="text/javascript" src="js/bts-jquery.min.js"></script>--%>
    <script type="text/javascript" src="jquery.ui.button.js"></script>
    <script type="text/javascript" src="../../js/accounting.js"></script>
    <script src="../../assets/bootstrap/js/bootstrap.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <script type="text/javascript">
        var NivelPerfil = "<% Response.Write(Session("mnivelacces")) %>";
        var baseUrl = "<%= Request.Url.Scheme + "://" + Request.Url.Authority & Request.ApplicationPath.TrimEnd("/") + "/" %>"
        //console.log(baseUrl)
    </script>
    <!-- inclusión de librerias externas necesarias para el funcionamiento del formulario de área origen -->
    <script src="<%=ResolveClientUrl("~/js/jquery.number.min.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("~/js/moment.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("~/js/area-origen.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>
    <% if (Session("mnivelacces") = 10) Then %>
    <script src="<%=ResolveClientUrl("~/js/loadDocs.js") %>" type="text/javascript"></script>
    <% End If %>

    <script type="text/javascript">      
         function btnCancelarMallaC() {
            $('#<%= BtnCancelarAccion.ClientID%>').click();
        }
        function AbrirModal(nombre) {
            var strNombreDiv = nombre;
            var strNombreDiv = strNombreDiv.replace('BtnObservaciones', 'HdnIdMaestroTitulos');
            var DocumentoId = $('#' + strNombreDiv).val()
            $('#HdnIdDocumento').val(DocumentoId)
            $('#<%=BtnObservacionesModal.ClientID%>').click();
            $('#<%=BtnObservacionesModal2.ClientID%>').click();
        }

        function AbrirModal2(nombre) {
            $('#<%=BtnObservacionesModalCNC.ClientID%>').click();
        }
        var NombreItemDoc = "";
        // ------------------------------------------------------------------------------------------------------------
        // Función que se encarga de agregar los archivos
        function AddFileUpload(strNombreitem) {
            NombreItemDoc = strNombreitem;
            NombreItemDoc = NombreItemDoc.replace('btnCargar', 'FlUpEvidencias');
            $("#" + NombreItemDoc).click();
        }
        function AjaxFileUpload() {
            var files = $("#" + NombreItemDoc).get(0);
            var data = new FormData();
            var nameFile = '';
            for (var i = 0; i < files.files.length; i++) {
                data.append(files.files[i].name, files.files[i]);
                nameFile = files.files[i].name;
            }
            $.ajax({
                url: "../Controles/FileUploadHandler.ashx",
                type: "POST",
                data: data,
                contentType: false,
                processData: false,
                success: function (result) {
                    NombreItemDoc = NombreItemDoc.replace('FlUpEvidencias', 'LblArchivo');
                    $('#' + NombreItemDoc).text(nameFile);
                    NombreItemDoc = NombreItemDoc.replace('LblArchivo', 'HdnPathFile');
                    var baseUrl = "<%= Request.Url.Scheme + "://" + Request.Url.Authority & Request.ApplicationPath.TrimEnd("/") %>"
                    $('#' + NombreItemDoc).val(baseUrl + result);
                },
                error: function (err) {
                    alert("Información Ha ocurrido un problema al cargar el archivo seleccionado. si el problema persiste contacte al administrador.");
                }
            });
        }
        function CambioEnFileUpload(strNombreitem) {
            NombreItemDoc = strNombreitem;
            var ValidExt = $("#<%=HdnExtValidas.ClientID%>").val();
            var files = $('#' + NombreItemDoc).get(0);
            var iValidFiles = 0;
            var iSizeFiles = 0;
            if (files.files.length > 0) {
                for (i = 0; i < files.files.length; i++) {
                    var _FileExtension = files.files[i].name.substr(files.files[i].name.lastIndexOf('.')).toLowerCase().replace(".", "");
                    var pattern = new RegExp(_FileExtension);
                    iSizeFiles = (parseInt(files.files[i].size / 1024) / 1024).toFixed(3) // Mb
                    var iTotalSizeFileIpload = parseFloat(iSizeFiles);

                    if (iTotalSizeFileIpload > 100) {
                        alert("Se ha excedido el tamaño maximo de 100 Megabytes por caso.");
                    } else if (pattern.test(ValidExt) == true) {
                        var FileUploadInfo = files.files[i].name + "," + (parseInt(files.files[i].size / 1024) / 1024).toFixed(3) + "&";
                        AjaxFileUpload();
                    } else {
                        alert("Extencion no valida.");
                    }
                }

            }
        }

        $(function () {
            var mostratObserv = $("#<%=HdnMostrarObser.ClientID%>").val();
            if (mostratObserv == 0) {
                $("#TabObservaciones").css("display", "none");
            }
            // -------------------------------------------------------------------------------------------------
            var baseNuevoTitulo = $("#urlBandejaNuevoTitulo").val();
            $("#urlBandejaNuevoTitulo").val(baseNuevoTitulo + <%= Session("usrAreaOrgen") %>);
        });

    </script>
</head>

<body>
    <div runat="server" id="loading"></div>
    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />

        <% 'Se establecen las variables que se deben tener en cuenta para las redirecciones en la devolución y el retorno %>    
        <asp:HiddenField runat="server" ID="urlBandejaAreaOrigen" ClientIDMode="Static" Value="/Security/modulos/maestro-acceso.aspx" />
        <asp:HiddenField runat="server" ID="urlBandejaEstudioTitulos" ClientIDMode="Static" Value="/Security/Maestros/estudio-titulos/BandejaTitulos.aspx" />
        <asp:HiddenField runat="server" ID="urlBandejaNuevoTitulo" ClientIDMode="Static" Value="/Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx?AreaOrigenId=" />

        <asp:HiddenField runat="server" ID="HdnMostrarObser" ClientIDMode="Static" Value="0" />
        <asp:HiddenField runat="server" ID="HdnAutomatico" ClientIDMode="Static" Value="False" />
        <asp:HiddenField runat="server" ID="HdnAutomaticoCargue" ClientIDMode="Static" Value="False" />

        <div id="divTitleNumber" class="title-number-content" runat="server">
            <table id="infoexpediente">
                <tr>
                    <td style="color: #2e6e9e">
                        <div style="width: 170px; height: 41px">
                            <label style="width: 100px; float: left; height: 20px">
                                No. Título:</label>
                            <asp:Label ID="lblNroTitulo" runat="server" Text="" Style="width: 100px; float: left; height: 20px" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>

        <div id="tabs">
        
           <div style="float: right; text-align: right; padding-right: 16px; margin-top: 5px; z-index: -10;">
               <asp:Button ID="BtnNuevo" Text="Nuevo" runat="server" Visible="False"  CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
            </div>
            <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 8px; margin-top: 10px; z-index: -10;">
                <asp:LinkButton ID="ABack" runat="server" Visible="False">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>
            </div>
            <!-- Region: Tabs_Edicion_Titulo-->
            <ul id="listPaginas">
                <li><a href="#tabs1">Información General</a></li>
                <li><a href="#tabs2">Valores</a></li>
                <li><a href="#tabs3">Deudor</a></li>
                <li><a href="#tabs4">Documentos titulo</a></li>
                <li id="TabClasificacion"><a href="#tabs5">Clasificación</a></li>
                <li id="TabObservaciones"><a href="#tabs6">Observaciones</a></li>
            </ul>
            <!-- EndRegion: Tabs_Edicion_Titulo-->
            <div id="tabCumpleNoCumpleG">
                <!-- Region: Informacion general -->
                <div id="tabs1" runat="server">
                    <asp:Panel ID="PnlInformacionDocumento" runat="server">
                        <div style="margin-left: 2px; margin-top: 4px; width: auto; height: auto;">
                            <asp:HiddenField runat="server" ID="HdnIdTask" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="HdnExpediente" ClientIDMode="Static" />
                            <asp:HiddenField runat="server" ID="HdnIdEstadoOperativo" ClientIDMode="Static" />
                            <div>
                                <asp:Label ID="txtAutomatico" runat="server" ClientIDMode="Static" Font-Bold="True" Font-Italic="True" Font-Size="Medium"></asp:Label>
                            </div>
                            <table id="tblGralTitulos" class="ui-widget-content">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header" style="width: 360px;">Tipo de cartera *
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboTipo_Cartera" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Tipo de Obligación *
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboMT_tipo_titulo" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Tipo de interés
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboMT_tiposentencia" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">No. Expediente origen * 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEFIEXPORIGEN" runat="server" MaxLength="19"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header" style="width: 360px;">No. Título Origen*
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_nro_titulo" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de expedición *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_expedicion_titulo" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecExpTit" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de expedición del título" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de Notificación *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_notificacion_titulo" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecNotTit" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de notificación del título" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Forma de Notificación *
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboMT_for_notificacion_titulo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">No. Resolución Reposición/ Reconsideración
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_res_resuelve_reposicion" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha Resolución Reposición/ Reconsideración
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_expe_resolucion_reposicion" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecExpRR" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de expedición resolución reposición" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de Notificación Resolución Reposición/ Reconsideración
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_not_reso_resu_reposicion" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecNotRRR" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de Notificación Resolución Resuelve Reposición" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Forma de Notificación Resolución Reposición/ Reconsideración
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboMT_for_not_reso_resu_reposicion" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">No. Resolución de Segunda Instancia/ Casación
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_reso_resu_apela_recon" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha Resolución Segunda Instancia/ Casación
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_exp_reso_apela_recon" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecExpRAR" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de expedición resolución apelación o Reconsideración" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de Notificación Resolución Segunda Instancia/ Casación
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_not_reso_apela_recon" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecNotRAR" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de Notificación Resolución Apelación o Reconsideración" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Forma de Notificación Resolución Segunda Instancia/ Casación
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboMT_for_not_reso_apela_recon" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr runat="server" id="Tr1">
                                    <td>&nbsp;</td>
                                    <td class="ui-widget-header">El titulo se encuentra ejecutoriado?
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkTituloEjecutoriado" runat="server" OnCheckedChanged="chkTituloEjecutoriado_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>

                                <tr runat="server" id="FechaEjecutoriaObligatoria">
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        <asp:Label ID="lblFechaEjecutoria" runat="server" Text="Fecha de ejecutoria" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fecha_ejecutoriaObli" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecEjecObli" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de ejecutoria" />
                                    </td>
                                </tr>
                                <tr runat="server" id="FechaExiLiqObligatoria">
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de exigibilidad
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_exi_liqObli" runat="server" CssClass="ui-widget" MaxLength="10" autocomplete="off" />
                                        <asp:ImageButton ID="imgBtnBorraFecExiLObli" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                            ToolTip="Borrar fecha de exigibilidad " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Fecha de caducidad o prescripción
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMT_fec_cad_presc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td class="ui-widget-header">Procedencia *
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="ui-widget" ID="cboPROCEDENCIA" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:ListView runat="server" ID="LstNotificaciones">
                                <ItemTemplate>
                                    <asp:Panel ID="ItemNotificacion" runat="server">
                                        <asp:HiddenField runat="server" ID="HdnItemObj" />
                                        <table class="ui-widget-content" style="width: 780px;">
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td class="ui-widget-header" style="width: 360px;">Fecha de Notificación
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMT_fechaItem" runat="server" CssClass="ui-widget" onchange="CambiarFechaNotificacion($(this).prop('id'))"
                                                        MaxLength="10"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td class="ui-widget-header" style="width: 360px;">Tipo Notificaciòn
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="ui-widget" ID="cboTipoNotifiacion" runat="server" onchange="CambiarTipoNotificacion($(this).prop('id'))">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td class="ui-widget-header" style="width: 360px;">Forma de notificación
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="ui-widget" ID="cboMT_for_not" runat="server" onchange="CambiarFormaNotifica($(this).prop('id'))">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td class="ui-widget-header">Acción
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"  OnClientClick="Eliminar($(this).prop('id')); return false;"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:ListView>
                            <div>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>

                            </div>
                            <div>
                                <asp:Button ID="BtnGuardar1" runat="server" Text="Guardar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ></asp:Button>
                                <asp:Button ID="BtnAdiccionarNot" runat="server" Text="Adicionar Notificación button" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"  Visible="false"></asp:Button>
                                <asp:Button ID="BtnCancelar1" runat="server" Text="Cancelar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <!-- EndRegion: Informacion general -->

                <!-- Region: Valores -->
                <div id="tabs2" runat="server">
                    <asp:Panel ID="PnlValores" runat="server">
                        <table id="tblEditMAESTRO_TITULOS" class="ui-widget-content">
                            <tr>
                                <td class="auto-style2">
                                    <table id="tblTituloDeuda" class="ui-widget-content" style="width: 770px;">
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <b>INFORMACION DE DEUDA</b>
                                            </td>
                                            <td style="width: 360px;">&nbsp;
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                        <table id="tblTipoTitulo" class="ui-widget-content">
                            <tr>
                                <td class="ui-widget-header" style="width: 360px;" id="tTitulo" runat="server">
                                    <b>TIPO DE TíTULO</b>
                                </td>
                                <td class="ui-widget-header" style="width: 360px;">
                                    <asp:Label ID="lblNombreTitulo" runat="server" Style="width: 360px;" />
                                </td>
                            </tr>
                        </table>
                        <table id="TblTipoTitulo" runat="server">
                            <tr id="trValorObligacion" runat="server" style="width: 360px;">
                                <td class="ui-widget-header" style="width: 360px;">
                                    <b>VALOR OBLIGACIÓN</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorObligacion" runat="server" CssClass="ui-widget formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trPartidaGlobal" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>PARTIDA GLOBAL</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartidaGlobal" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trSancionOmision" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>SANCIÓN OMISIÓN</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSancionOmision" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trSancionMora" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>SANCIÓN MORA</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSancionMora" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trSancionInexactitud" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>SANCIÓN INEXACTITUD</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSancionInexactitud" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trTotalObligacion" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>TOTAL DE LA OBLIGACIÓN</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalObligacion" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trTotalPartidaGlobal" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>TOTAL PARTIDA GLOBAL</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalPartidaGlobal" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trTotalSancion" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>TOTAL SANCIÓN</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalSancion" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                            <tr id="trTotalDeuda" runat="server" style="width: 360px;">
                                <td class="ui-widget-header">
                                    <b>TOTAL DE LA DEUDA *</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalDeuda" runat="server" CssClass="numeros formatoColombia" />
                                </td>
                            </tr>
                        </table>
                        <table id="tblTotalRepartidor" class="ui-widget-content" style="width: 780px;">
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style2">&nbsp;
                         <asp:Button ID="BtnGuardar2" runat="server" Text="Guardar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ></asp:Button>
                                    <asp:Button ID="BtnCancelar2" runat="server" Text="Cancelar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <!-- EndRegion: Valores -->

                <!-- Region: Deudor -->
                <div id="tabs3" runat="server">
                    <asp:Panel ID="PnlDeudores" runat="server">
                        <div style="margin-left: 2px; margin-top: 4px; width: auto;">
                            <% If String.IsNullOrEmpty(HdnIdTask.Value.ToString) = False Then  %>
                            <iframe src="ENTES_DEUDORES.aspx?ID_TASK=<%Response.Write(HdnIdTask.Value)%>&pTipo=1" width="100%" scrolling="no" frameborder="0"></iframe>
                            <% Else %>
                            <asp:Label runat="server" ForeColor="Red" ID="Label1" Text="Debes guardar el titulo antes de agregar un deudor " />
                            <% End If %>
                        </div>
                    </asp:Panel>
                </div>
                <!-- EndRegion: Deudor -->

                <!-- Region: Documentos -->
                <div id="tabs4" runat="server">

                    <asp:Label runat="server" ForeColor="Red" ID="lblTexValidDocument" />
                    <asp:HiddenField runat="server" ID="HdnExtValidas" Value="" ClientIDMode="Static" />
                    <asp:ListView runat="server" ID="lsvListaDocumentos">
                        <ItemTemplate>
                            <table id="TblListViewDoc" class="ui-widget-content tbl-documentos-titulo" style="width: 880px;">
                                <tr>
                                    <td class="ui-widget-header">
                                        <asp:HiddenField runat="server" ID="HdnPathFile" />
                                        <asp:HiddenField runat="server" ID="HdnIdDoc" Value="0" />
                                        <asp:HiddenField runat="server" ID="HdnIdMaestroTitulos" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnObservacionDoc" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnCodTipoDodocumentoAO" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnNomDocAO" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnObservaLegibilidad" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnNumPaginas" Value="" />
                                        <asp:HiddenField runat="server" ID="HdnCodGuid" Value="" />
                                        <asp:HiddenField runat="server" ID="HdIndDocSincronizado" Value="" />
                                        <asp:FileUpload runat="server" ID="FlUpEvidencias" AllowMultiple="false" onchange="CambioEnFileUpload($(this).prop('id'))" Style="visibility: collapse; height: 0px; width: 0px;" />
                                        <asp:Label ID="lblNameDoc" runat="server" />
                                    </td>
                                    <td class="documentoTitulo1">
                                        <% If Session("mnivelacces") = 10 And Boolean.Parse(HdnAutomaticoCargue.Value) = False Then  %>
                                        <asp:Button ID="btnActualizarMetaDataDoc" runat="server" Text="Actualizar Datos" CommandName="updateMetaData" Visible="false"  CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                                        <% Else %>
                                        <asp:Button ID="btnCargar" OnClientClick="AddFileUpload($(this).prop('id')); return false;" runat="server" Text="Cargar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"  />
                                        <% End If %>
                                    </td>
                                    <td class="documentoTitulo3">
                                        <asp:Button ID="btnVer" runat="server" Text="Ver" OnClientClick="AbrirArchivo($(this).prop('id')); return false;"  CssClass="hide button ui-button ui-widget ui-state-default ui-corner-all" />
                                        <asp:Button ID="btnBorrar" runat="server" Text="Borrar" OnClientClick="borrarDocumento(this); return false;" CssClass="hide button ui-button ui-widget ui-state-default ui-corner-all" />
                                    </td>
                                    <td class="txtCumple">
                                        <asp:Label ID="LblCumple" class="ui-widget-header" runat="server" Text="Cumple" />
                                    </td>
                                    <td id="RbtnDocCNC" class="documentoTitulo4">
                                        <asp:RadioButton ID="RbtnSiCumpleDoc" runat="server" Text="SI" onclick="SeleccionaSIDOC($(this).prop('id'));" />
                                        <asp:RadioButton ID="RbtnNoCumpleDoc" runat="server" Text="NO" onclick="SeleccionaNODOC($(this).prop('id'));" />
                                    </td>
                                    <td id="BtnDocCNC" class="documentoTitulo4">
                                        <asp:Button ID="BtnObservaciones" runat="server" Text="Observaciones" OnClientClick="AbrirModal(this.id) ; return false;" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="LblArchivo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:ListView>

                    <asp:Panel ID="PnlDocAutomatico" runat="server" CssClass="form-row" Visible="false">
                        <div class="form-group row col-sm-12">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdDocAutomatico" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowPaging="True" ClientIDMode="Static" PagerSettings-Visible="False" OnRowDataBound="grdDocAutomatico_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID_MAESTRO_TITULOS_DOCUMENTOS">
                                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COD_TIPO_DOCUMENTO_AO" HeaderText="Tipo Documental" />
                                                <asp:BoundField DataField="COD_GUID" HeaderText="Codigo" />
                                                <asp:BoundField DataField="NOM_DOC_AO" HeaderText="Nombre" />
                                                <asp:ButtonField ButtonType="Button" Text="Ver">
                                                    <ControlStyle CssClass="GridEditButton PCGButton ui-button ui-widget ui-state-default ui-corner-alls" />
                                                </asp:ButtonField>
                                                <asp:TemplateField HeaderText="Cumple">            
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="RbtnSiCumpleDocServicio" runat="server" Text="SI" />
                                                        <asp:RadioButton ID="RbtnNoCumpleDocServicio" runat="server" Text="NO" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ui-widget-content" />
                                            <AlternatingRowStyle />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            <div class="col-sm-12 row">
                                <uc1:Paginador ID="PaginadorDocAutomatico" runat="server" gridViewIdClient="grdDocAutomatico" OnEventActualizarGrid="PaginadorDocAutomatico_EventActualizarGrid" />
                                <hr class="separadorDocumentosServicio" />
                            </div>
                            
                        </div>
                    </asp:Panel>

                    <div class="auto-style2">
                        <asp:Button ID="BtnBuscarDoc" runat="server" Text="Buscar en documentic" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="BtnEnviarTitulo" runat="server" Text="Enviar título" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="BtnGuardar3" runat="server" Text="Guardar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="BtnCancelar3" runat="server" Text="Cancelar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="btnShowTerminar" runat="server" Text="Terminar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" Visible="False" />
                        <asp:Button ID="BtnEliminar" runat="server" Text="Eliminar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="BtnCancelarAccion" runat="server" Style="display: none;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <asp:Button ID="BtnEnviarCalificacion" runat="server" Text="Enviar Calificación" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" OnClientClick="mostrarLoading()" />
                        <asp:Button ID="btnSuspenderTitulo" runat="server" Text="Suspender" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                        <%--<asp:Button ID="BtnVerInfo" runat="server" Text="Ver Documentos" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>--%>
                    </div>
                </div>
                <!-- EndRegion: Documentos -->
                <!-- Region: Clasificación -->
                <div id="tabs5" runat="server" clientidmode="Static">
                    <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                        <iframe src="ClasificacionManual2.aspx?ID_TASK=<%Response.Write(HdnIdTask.Value)%>&pTipo=1"
                            width="960px" height="740px" scrolling="no" frameborder="0"></iframe>
                    </div>
                </div>
                <!-- EndRegion: Clasificación -->

                <div id="tabs6" runat="server" clientidmode="Static">
                    <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                        <iframe src="Observaciones.aspx?ID_TASK=<%Response.Write(HdnIdTask.Value)%>"
                            width="960px" height="740px" scrolling="no" frameborder="0"></iframe>
                    </div>
                </div>



                <asp:Button ID="BtnHide" runat="server" Text="Enviar titulo" ClientIDMode="Static" Style="visibility: hidden;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                <div id="tabModalMallaDiv">

                    <!-- ModalPopupExtender -->
                    <asp:ModalPopupExtender ID="mpMalla" runat="server" PopupControlID="PnlMalla" TargetControlID="BtnHide"
                        CancelControlID="btnCancelarMalla" BackgroundCssClass="FondoAplicacion">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlMalla" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                        <asp:UpdatePanel ID="UpnlModalMalla" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <asp:Label ID="lblMallaTitulo" runat="server" Text="Titulo" CssClass="col-sm-2 col-form-label col-form-label-sm style4" />
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblMalla" runat="server" Text="Lista Errores" />


                                        <asp:ListView runat="server" ID="LsvResul">
                                            <ItemTemplate>
                                                <table class="ui-widget-content tbl-respuesta-malla" style="width: 100%;">
                                                    <tr>
                                                        <td class="ui-widget-header" width="35">
                                                            <asp:Label ID="lblCod" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblMensaje" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:ListView>

                                    </div>
                                </div>
                                <div class="col-sm-12">
                                    <asp:Button ID="btnCancelarMalla" runat="server" Text="Cerrar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" OnClientClick="btnCancelarMallaC();" />
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <!-- ModalPopupExtender -->
                </div>
                <div id="tabBuscadocumento">
                    <!-- ModalPopupExtender -->
                    <asp:ModalPopupExtender ID="ModalPopupBuscarDocumento" runat="server" PopupControlID="PnlBuscar" TargetControlID="BtnBuscarDoc" ClientIDMode="Static"
                        CancelControlID="BtnCerrarBuscar" BackgroundCssClass="FondoAplicacion">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlBuscar" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;height:430px;">
                        <div class="form-group row">
                            <div class="col-sm-9">
                                <asp:UpdatePanel ID="UpdBuscar" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTituto" runat="server" Text="Numero Expediente Origen" CssClass="style4" />
                                        <asp:TextBox ID="txtNoExp" runat="server"></asp:TextBox>
                                        <asp:GridView ID="grdBuscar" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" PageSize="10" AllowPaging="true" ClientIDMode="Static" PagerSettings-Visible="False" OnRowDataBound="grdBuscar_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="id">
                                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="nombreTipoDocumental" HeaderText="Tipo Documental"></asp:BoundField>
                                                <asp:BoundField DataField="datosDocumento.origenDocumento" HeaderText="Origen"></asp:BoundField>
                                                <asp:BoundField DataField="datosDocumento.documentTitle" HeaderText="Nombre"></asp:BoundField>
                                                <asp:BoundField DataField="datosDocumento.fechaDocumento" HeaderText="Fecha documento" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                                <asp:BoundField DataField="datosDocumento.agrupador1" HeaderText="Agrupador1"></asp:BoundField>
                                                <asp:ButtonField ButtonType="Button" Text="Ver" >
                                                    <ControlStyle CssClass="GridEditButton button ui-button ui-widget ui-state-default ui-corner-all" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <RowStyle CssClass="ui-widget-content" />
                                            <AlternatingRowStyle />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <uc1:Paginador ID="PaginadorGridView" runat="server" gridViewIdClient="grdBuscar" OnEventActualizarGrid="paginador_EventActualizarGrid" />
                            </div>
                            <div class="col-sm-12" style="height:40px">
                                <asp:Label ID="Lblresultados" runat="server" Text="" CssClass="style4" />
                            </div>
                            <div class="col-sm-12"style="height:40px">
                                <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" OnClick="cmdBuscar_Click"></asp:Button>
                                <asp:Button ID="BtnCerrarBuscar" runat="server" Text="Cerrar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>
                    <!-- ModalPopupExtender -->
                </div>
                <div id="tabVerDocumentos">
                    <!-- ModalPopupExtender -->
                    <%--<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="PnlDocAutomatico" TargetControlID="BtnVerInfo" ClientIDMode="Static"
                        CancelControlID="btnCerrarDocAutomatico" BackgroundCssClass="FondoAplicacion">
                    </asp:ModalPopupExtender>--%>
                    <%--<asp:Panel ID="PnlDocAutomatico" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                        <div class="form-group row">
                            <div class="col-sm-9">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdDocAutomatico" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" PageSize="10" AllowPaging="true" ClientIDMode="Static" PagerSettings-Visible="False" OnRowDataBound="grdDocAutomatico_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID_MAESTRO_TITULOS_DOCUMENTOS">
                                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="COD_TIPO_DOCUMENTO_AO" HeaderText="Tipo Documental"></asp:BoundField>
                                                <asp:BoundField DataField="COD_GUID" HeaderText="Codigo"></asp:BoundField>
                                                <asp:BoundField DataField="NOM_DOC_AO" HeaderText="Nombre"></asp:BoundField>
                                                <asp:ButtonField ButtonType="Button" Text="Descargar">
                                                    <ControlStyle CssClass="GridEditButton button ui-button ui-widget ui-state-default ui-corner-all" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <RowStyle CssClass="ui-widget-content" />
                                            <AlternatingRowStyle />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <uc1:Paginador ID="PaginadorDocAutomatico" runat="server" gridViewIdClient="grdDocAutomatico" OnEventActualizarGrid="PaginadorDocAutomatico_EventActualizarGrid" />
                            </div>
                            <div class="col-sm-12">
                                <asp:Label ID="Label2" runat="server" Text="" CssClass="style4" />
                            </div>
                            <div class="col-sm-12">
                                <asp:Button ID="btnCerrarDocAutomatico" runat="server" Text="Cerrar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                            </div>
                        </div>
                    </asp:Panel>--%>
                    <!-- ModalPopupExtender -->
                </div>
                <div id="tabsModal">

                    <!-- ModalPopupExtender -->
                    <asp:ModalPopupExtender ID="mp1" runat="server" PopupControlID="PnlJust" TargetControlID="btnShowTerminar"
                        CancelControlID="btnClose" BackgroundCssClass="FondoAplicacion">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlJust" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                        <div style="height: 70px;">
                            <asp:UpdatePanel ID="UpnlModal" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width: 100%; background: #fff;">
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <h3>Terminación del título</h3>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" Text="Justificación de la terminación:" CssClass="ui-widget-header"></asp:Label></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="TxtJustificacion" Style="margin: 0px; width: 320px; height: 40px;"></asp:TextBox>
                                                <asp:Label ID="lblValidaJust" runat="server" Text="* Debe justificar la terminación de un título ya creado" ForeColor="Red" Visible="False"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="BtnGuardarValidaJust" CssClass="PCGButton button ui-button ui-widget ui-state-default ui-corner-all ui-state-hover" runat="server" Text="Guardar" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table style="width: 40px; background: #fff;">
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnClose" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Cancelar" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <!-- ModalPopupExtender -->
                </div>
                <asp:Button ID="BtnObservacionesModal" runat="server" Style="visibility: hidden;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>

                <asp:UpdatePanel ID="UpnlObservaModal2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="BtnObservacionesModal2" runat="server" Style="display: none;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                        <asp:Button ID="BtnObservacionesModalCNC" runat="server" Style="display: none;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div id="tabsModalDocumentosCNC">

                    <!-- ModalPopupExtender -->
                    <asp:ModalPopupExtender ID="mp2" runat="server" PopupControlID="PnlDocCNC" TargetControlID="BtnObservacionesModal"
                        CancelControlID="btnCloseCNC" BackgroundCssClass="FondoAplicacion">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlDocCNC" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff; top: 200px;">

                        <asp:UpdatePanel ID="UpnlDocCNC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group row" style="margin-left: 20px;">
                                    <asp:GridView ID="grdHistoricoCNCDoc" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True" CellPadding="4">
                                        <Columns>
                                            <asp:BoundField DataField="ID_OBSERVACIONESDOC" Visible="False"></asp:BoundField>
                                            <asp:BoundField DataField="USUARIO" HeaderText="Usuario"></asp:BoundField>
                                            <asp:BoundField DataField="DESTINATARIO" HeaderText="Destinatario"></asp:BoundField>
                                            <asp:BoundField DataField="FCHENVIO" HeaderText="Fecha y hora del envío"></asp:BoundField>
                                            <asp:BoundField DataField="OBSERVACIONES" HeaderText="Observación"></asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                        <RowStyle CssClass="ui-widget-content" />
                                        <AlternatingRowStyle />
                                    </asp:GridView>
                                </div>
                                <div class="col-sm-9">
                                    <asp:Label runat="server" Text="Ingresar Comentario:"></asp:Label>
                                    <div>
                                        <asp:TextBox runat="server" ID="TxtComentarioCNC" Style="margin: 0px; width: 400px; height: 70px;"></asp:TextBox>
                                        <asp:Label ID="LblValidaComentarioCNC" runat="server" Text="* Debe ingresar un comentario" ForeColor="Red" Visible="False"></asp:Label>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <div class="col-sm-12">
                            <asp:Button ID="btnEnviarCNC" runat="server" Text="Guardar" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
                            <asp:Button ID="btnCloseCNC" runat="server" Text="Cancelar" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all" />
                        </div>
                    </asp:Panel>

                </div>

                <!-- EndRegion: DeDocumentos -->

                <!-- Modal Clasificación -->
                <asp:Button ID="btnModalClasificacion" runat="server" Text="Clasificación Manual" ClientIDMode="Static" Style="display: none;" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                <div id="tabModalClasificacionDiv">
                    <!-- ModalPopupExtender ModalClasificacion -->
                    <asp:ModalPopupExtender ID="ModalClasificacion" runat="server" PopupControlID="pnlModalClasificacion" TargetControlID="btnModalClasificacion" CancelControlID="btnAceptarClasificacion" BackgroundCssClass="FondoAplicacion" />
                    <asp:Panel ID="pnlModalClasificacion" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group row">

                                    <asp:Label ID="lblMensajeClasificacion" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" />

                                </div>
                                <div class="col-sm-12">
                                    <asp:Button ID="btnAceptarClasificacion" runat="server" Text="Aceptar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>
                    <!-- ModalPopupExtender ModalClasificacion-->
                </div>
                <!-- END Modal Clasificación -->

                <!-- Modal Validación Inicial -->
                <asp:Button ID="btnErrorValidacionInicial" runat="server" Text="Validación Inicial" ClientIDMode="Static" Style="display: none;"  CssClass="button ui-button ui-widget ui-state-default ui-corner-all"/>
                <div id="tabModalValidacionInicialDiv">
                    <!-- ModalPopupExtender ModalValidacionInicial -->
                    <asp:ModalPopupExtender ID="ModalValidacionInicial" runat="server" PopupControlID="pnlModalValidacionInicial" TargetControlID="btnErrorValidacionInicial" CancelControlID="btnRedirect" BackgroundCssClass="FondoAplicacion" />
                    <asp:Panel ID="pnlModalValidacionInicial" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                        <asp:UpdatePanel ID="uPnlModalValidacionInicial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group row">

                                    <asp:Label ID="lblErrorPrioridad" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" Visible="False" />
                                    <asp:Label ID="lblErrorUsuarioAsignado" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" Visible="False" />
                                    <asp:Label ID="lblErrorNoEsTitulo" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" Visible="False" />
                                    <asp:Label ID="lblErrorEstadoOperativoErroneo" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" Visible="False" />
                                    <asp:Label ID="lblErrorTituloClasificado" runat="server" CssClass="col-sm-12 col-form-label col-form-label-sm style4" Visible="False" />

                                </div>
                                <div class="col-sm-12">
                                    <asp:Button ID="btnRedirect" runat="server" Text="Aceptar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" Style="display: none;" />
                                    <asp:Button ID="btnRedirect2" runat="server" Text="Aceptar" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>
                    <!-- ModalPopupExtender ModalValidacionInicial-->
                </div>
                <!-- END Modal Validación Inicial -->

                <asp:HiddenField runat="server" ID="HdnIdunico" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="HiddenField1" Value="0" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="HdnIdDocumento" Value="0" ClientIDMode="Static" />
                <asp:HiddenField runat="server" ID="HdnIdTab" Value="" ClientIDMode="Static" />
                <div style="margin-left: -70px; margin-top: 4px; width: 960px; height: auto;">
                    <asp:Panel ID="PnlCNCOb" runat="server" align="center" Visible="False">
                        <table id="tblGralCumpleNoCumple" class="ui-widget-content" style="width: 780px;" runat="server">
                            <tr>
                                <td class="ui-widget-header" style="width: 360px;">Cumple
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RbtnSiNoCumple" AutoPostBack="True" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                        <asp:ListItem>Si</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="ui-widget-header" style="width: 360px;">Observaciones
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtobservaCumpleNoCumple" runat="server" TextMode="MultiLine" Style="width: 400px; height: 70px;"></asp:TextBox>

                                </td>
                            </tr>
                            <tr id="trTipificaciones" runat="server">
                                <td class="ui-widget-header" style="width: 360px;">Tipificaciones
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="ChkBltsTipifica" AutoPostBack="True" runat="server"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="LblobservaCNC" runat="server" Text="* Por favor diligencie los campos completos" ForeColor="Red" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="BtnEnviarCNCGral" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Guardar" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <asp:Button ID="btnCrearTarea" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ClientIDMode="Static" runat="server" Text="" Style="visibility: hidden" />
        <asp:Button ID="btnHabilitaCNCgral" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" ClientIDMode="Static" runat="server" Text="" Style="visibility: hidden" />



        <div id="tabsModalSuspender">

            <!-- ModalPopupExtender -->
            <asp:ModalPopupExtender ID="ModalSuspenderTitulo" runat="server" PopupControlID="PnlSuspenderTitulo" TargetControlID="btnSuspenderTitulo" CancelControlID="btnCerrarSuspenderTitulo" BackgroundCssClass="FondoAplicacion" />

            <asp:Panel ID="PnlSuspenderTitulo" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="form-group row">
                            <asp:Label ID="lblObservacionSuspenderTitulo" runat="server" Text="Observación Suspensión*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtObservacionSuspenderTitulo" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtObservacionSuspenderTitulo" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Style="width: 400px;" />
                                <asp:RequiredFieldValidator ID="reqObservacionSuspenderTitulo" runat="server" ErrorMessage="" ControlToValidate="txtObservacionSuspenderTitulo" CssClass="error-msg" ValidationGroup="formSuspenderTitulo" />
                            </div>
                        </div>

                        <div class="col-sm-12">
                            <asp:Button ID="cmdSuspenderTitulo" runat="server" Text="Suspender Título"  ClientIDMode="Inherit" ValidationGroup="formSuspenderTitulo" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" />
                            <asp:Button ID="btnCerrarSuspenderTitulo" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Cancelar" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </asp:Panel>
            <!-- ModalPopupExtender -->
        </div>

        <%--<div style="display: none;">
            <uc3:Documento ID="editDocEstudioTitulos" runat="server" idTituloDocumento="0" modificarDoc="0" muestraVer="0" validateLoad="No" style="display: none;" />
        </div>--%>


        <div id="tabsModalVerDocMD">
            <asp:ModalPopupExtender ID="modalVerDocMD" runat="server" PopupControlID="PnlDocMD" TargetControlID="btnActualizarMetaDataTest"
                CancelControlID="btnCloseVerDocMD" BackgroundCssClass="FondoAplicacion">
            </asp:ModalPopupExtender>
            <asp:Panel ID="PnlDocMD" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="background: #fff;">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnIdDocUpdate" runat="server" />
                        <div class="form-group row">
                            <asp:Label ID="lblErrorDocUpdate" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-danger" Visible="False" Style="margin: 0 auto;" Text="Error" />
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblDocUpdateEnviada" runat="server" CssClass="col-sm-11 col-form-label col-form-label-sm alert alert-success" Visible="False" Style="margin: 0 auto;" Text="Priorización enviada correctamente" />
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblCarga" runat="server" Text="Documento*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="hlinkViewDoc" />
                            <div class="col-sm-9">

                                <asp:HyperLink ID="hlinkViewDoc" runat="server" Target="_blank" ClientIDMode="Static" Text="Ver archivo cargado" CssClass="link-view-file" />
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblNumPaginas" runat="server" Text="Número Páginas*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtNumPaginas" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNumPaginas" runat="server" CssClass="form-control form-control-sm" ClientIDMode="Static" autocomplete="false" />
                                <asp:RequiredFieldValidator ID="reqNumPaginas" runat="server" ErrorMessage="" ControlToValidate="txtNumPaginas" CssClass="error-msg" ValidationGroup="formDocumento" />
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label ID="lblObservacionLegibilidad" runat="server" Text="Observación Legibilidad*" CssClass="col-sm-2 col-form-label col-form-label-sm style4" AssociatedControlID="txtObservacionLegibilidad" />
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtObservacionLegibilidad" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Style="width: 400px;" ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="reqObservacionLegibilidad" runat="server" ErrorMessage="" ControlToValidate="txtObservacionLegibilidad" CssClass="error-msg" ValidationGroup="formDocumento" />
                            </div>
                        </div>

                        <div class="col-sm-12">
                            <asp:Button ID="cmdAsignarDocumentoMetaData" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Actualizar Datos" ClientIDMode="Static" ValidationGroup="formDocumento" />
                            <asp:Button ID="btnCloseVerDocMD" CssClass="button ui-button ui-widget ui-state-default ui-corner-all" runat="server" Text="Cerrar" />
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>

        <asp:Button ID="btnActualizarMetaDataTest" runat="server" Text="Button" CssClass="hide button ui-button ui-widget ui-state-default ui-corner-all" />

    </form>

    <!-- Bootstrap -->
    <link href="<%=ResolveClientUrl("~/assets/bootstrap/css/bootstrap.min.css") %>" rel="stylesheet" />
    <script src="<%=ResolveClientUrl("~/assets/bootstrap/js/bootstrap.min.js") %>" type="text/javascript"></script>
    <script>
        $.fn.bootstrapBtn = $.fn.button.noConflict();
    </script>

    <link href="<%=ResolveClientUrl("~/css/main.css") %>" rel="stylesheet" />

</body>
</html>

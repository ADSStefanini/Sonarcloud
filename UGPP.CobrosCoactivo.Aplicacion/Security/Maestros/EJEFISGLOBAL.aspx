<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EJEFISGLOBAL.aspx.vb" Inherits="coactivosyp.EJEFISGLOBAL" %>

<%-- Controles de Usuario para asignación y priorización --%>
<%@ Register TagPrefix="ucPriorizacion" TagName="SolicitudPriorizacion" Src="~/Security/Controles/SolicitudPriorizacion.ascx" %>
<%@ Register TagPrefix="ucReasignacion" TagName="SolicitudReasignacion" Src="~/Security/Controles/SolicitudReasignacion.ascx" %>


<%@ Register TagPrefix="uc1" TagName="BandejaEstudioTitulos" Src="~/Security/Controles/BandejaEstudioTitulos.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>EJEFISGLOBAL</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script type="text/javascript" src="jquery.ui.button.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function () {
            EndRequestHandler();
        });

        function EndRequestHandler() {

            $('#lnkNumExpVencer').click(function () {
                window.open('EstadisticaxVencer.aspx', 'Estadistica de expedientes por vencer', 'width=600,height=250');
                return false;
            });

            $('#lnkNumExpVencidos').click(function () {
                window.open('EstadisticaVencidos.aspx', 'Estadistica de expedientes vencidos', 'width=600,height=250');
                return false;
            });

            $('#lnkMsjNoLeidos').click(function () {
                window.open('MENSAJES.aspx', 'Visor de mensajes', 'width=780,height=450');
                return false;
            });

            $('#cmdInformesGestion').click(function () {
                window.open('info_persuasivo.aspx', 'Estadistica de expedientes vencidos', 'width=400,height=250');
                return false;
            });

            $(".PCG-Content tr:gt(0)").mouseover(function () {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function () {
                $(this).removeClass("ui-state-highlight");
            });
        }

        $(function () {
            var perfil = <%  Response.Write(Session("mnivelacces")) %> ;
            //alert(perfil);
            // Jeisson Gómez 
            // 08/05/2017 HU_009 - Criterio de Aceptación 10 
            // Se cambia la validación para los perfiles 
            // 1 - SUPER ADMINISTRADOR, 2 - SUPERVISOR, 3 - REVISOR, 8 - GESTOR DE INFORMACION 
            // puedan ver el ícono de reportes desde el formulario EJEFISGLOBAL.
            if (perfil == 4 || perfil == 5 || perfil == 6 || perfil == 7) {
                $("#lnkInformes").css("display", "none");
            }
        });
    </script>
    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {
            document.getElementById('procesando_div').style.display = "";
            $("#dialog-modal").dialog({
                height: 150,
                modal: true
            });
            setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
        }
    </script>
    <link href="<%=ResolveClientUrl("~/css/main.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <asp:ToolkitScriptManager ID="tsm" runat="server"></asp:ToolkitScriptManager>
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td colspan="10" background="images/resultados_busca.jpg" height="42">
                    <div style="color: White; font-weight: bold; width: 500px; height: 20px; float: left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>

                    <div style="border: 1px; border-color: White; color: White; width: 530px; height: 20px; float: right; text-align: right">


                        <!-- Solicitudes de cambio de estado para revisores y supervisores -->
                        <div id="divCambioEstado" style="color: White; width: 60px; height: 20px; float: left; text-align: right;" runat="server">
                            <asp:LinkButton ID="ACambio" runat="server" ToolTip="Cambios de estado">
                                    <img alt ="Cambio de estado"  src="../images/icons/cambioestado.png" height="18" width="18" style=" vertical-align:middle" id="img2" title="Cambios de estado" />
                            </asp:LinkButton>
                            <span><%  Response.Write("(" & Session("ssNumSolicitudesCE") & ")")%>&nbsp&nbsp</span>
                        </div>


                        <!-- Mensajes -->
                        <div style="color: White; width: 60px; height: 20px; float: left; text-align: right; margin-left: 20px;">
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Mensajes">
                                    <img alt ="Mensajes"  src="../images/icons/comentarios.png" height="18" width="18" style=" vertical-align:middle" id="img3" title="Mensajes" />
                            </asp:LinkButton>
                            <span><%  Response.Write("(" & Session("ssNumMsgNoLeidos") & ")")%>&nbsp&nbsp</span>
                        </div>

                        <!-- Informes -->
                        <div style="color: White; width: 60px; height: 20px; float: left; text-align: right;">
                            <asp:LinkButton ID="lnkInformes" runat="server" ToolTip="Informes" OnClientClick="mostrar_procesar();"><img alt ="" src="../images/icons/informes16x16.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                        </div>

                        <!-- Consultar pagos -->
                        <div style="color: White; width: 60px; height: 20px; float: left; text-align: right;">
                            <asp:LinkButton ID="lnkConsultarPagos" runat="server" ToolTip="Consultar pagos"><img alt ="" src="../images/icons/plata.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                        </div>

                        <%--<!-- Capturar intereses -->
                            <div style="color:White; width:60px; height:20px; float:left; text-align:right; ">
                                <asp:LinkButton ID="lnkInteres" runat="server" ToolTip="Capturar intereses"><img alt ="" src="../images/icons/intereses.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>
                            
                            <!-- Intereses de multas -->
                            <div style="color:White; width:50px; height:20px; float:left; text-align:right">
                                <asp:LinkButton ID="lnkInterMultas" runat="server" 
                                    ToolTip="Capturar intereses de multas"><img alt ="" src="../images/icons/intermultas.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                            </div>--%>

                        <!-- Subir SQL -->
                        <div style="color: White; width: 50px; height: 20px; float: left; text-align: right">
                            <asp:LinkButton ID="lnkSql" runat="server" ToolTip="Subir sql"><img alt ="" src="../images/icons/sql.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                        </div>

                        <div style="color: White; width: 50px; height: 20px; float: left; text-align: right">
                            <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Subir sql"><img alt ="" src="../images/icons/sql.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                        </div>

                        <div style="color: White; width: 50px; height: 20px; float: left; text-align: right">
                            <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                                <img alt ="Regresar al listado de expedientes"  src="<%=ResolveClientUrl("~/Security/images/icons/regresar.png") %>" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" />
                            </asp:LinkButton>
                        </div>

                        <div style="color: White; width: 120px; height: 20px; float: left; text-align: right">
                            <!-- Cerrar sesion -->
                            <asp:LinkButton ID="A3" runat="server">
                                <img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" />
                                <span>Cerrar sesión&nbsp</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <table style="width: 100%">
                        <tr>
                            <td class="ui-widget-header">No. expediente cobranzas
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEFINROEXP" runat="server"></asp:TextBox>
                            </td>
                            <td class="ui-widget-header">Nombre deudor
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchED_NOMBRE" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton button"></asp:Button>
                            </td>
                            <td>Paginación
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" ID="cboNumExp" runat="server" AutoPostBack="True"></asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-widget-header">NIT / C.C.
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEFINIT" runat="server"></asp:TextBox>
                            </td>
                            <td class="ui-widget-header">Estado actual</td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" ID="cboEFIESTADO" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="cmdInformesGestion" runat="server" Text="Informes" CssClass="PCGButton button"></asp:Button>
                            </td>
                            <td>
                                <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" CssClass="button" />
                            </td>
                            <td>&nbsp;<asp:Button ID="cmdMostrarEstadisticas" runat="server" Text="Estadisticas" CssClass="button" />
                            </td>
                            <td>&nbsp;<asp:Button ID="cmdMasivo" runat="server" Text="Masivos" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-widget-header">Gestor
                            </td>
                            <td>
                                <asp:DropDownList ID="cboSearchEFIUSUASIG" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="ui-widget-header">Alerta / Término </td>
                            <td>
                                <asp:TextBox ID="txtTermino" runat="server" Columns="20" Enabled="False" Visible="False"></asp:TextBox>
                                <%-- 
                                    Jeisson Gómez  
                                    Se revierte la HU_003 07/03/2017 
                                    Se restablce filtro HU_003 19/05/2017
                                --%>
                                <asp:DropDownList ID="cboSearchTERMINO" runat="server" CssClass="cboSelect" AppendDataBoundItems="true" Enabled="true" Visible="true"></asp:DropDownList>
                            </td>
                            <td class="ui-widget-header">Tipo de título 
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" ID="cboMT_tipo_titulo" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="ui-widget-header">Fecha recepción Título
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchFECTITULO" CssClass="calendar" runat="server" Style="width: 90px;"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnBorraFechaRT" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de recepción del título" />
                            </td>
                            <td class="ui-widget-header">Fec entrega gestor
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEFIFECENTGES" CssClass="calendar" runat="server" Style="width: 90px;"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnBorraFechaEG" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de entrega al gestor" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="EFINROEXP">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EFINROEXP" HeaderText="Expediente" SortExpression="EJEFISGLOBAL.EFINROEXP" />
                            <asp:BoundField DataField="EFIFECHAEXP" HeaderText="Fecha recepción título" SortExpression="EJEFISGLOBAL.EFIFECHAEXP" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="ED_NOMBRE" HeaderText="Nombre del deudor" SortExpression="ENTES_DEUDORES.ED_NOMBRE" />
                            <asp:BoundField DataField="EFINIT" HeaderText="NIT / CC" SortExpression="EJEFISGLOBAL.EFINIT" />
                            <asp:BoundField DataField="NomTipoTitulo" HeaderText="Tipo de Título" SortExpression="TITULOSEJECUTIVOS.NomTipoTitulo" />
                            <%--<asp:BoundField DataField="EFIFECCAD" HeaderText="Fecha entrega al CAD" SortExpression="EJEFISGLOBAL.EFIFECCAD" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>--%>
                            <asp:BoundField DataField="EFIESTADO" HeaderText="Estado actual" />
                            <asp:BoundField DataField="ESTADO_OPERATIVO" HeaderText="Estado Operativo" />
                            <asp:BoundField DataField="EFIVALDEU" HeaderText="Valor Deuda" DataFormatString="{0:N0}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EFIPAGOSCAP" HeaderText="Pagos capital" DataFormatString="{0:N0}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EFISALDOCAP" HeaderText="Saldo capital actual" DataFormatString="{0:N0}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EFIESTUP" HeaderText="Estado actual último pago" />
                            <asp:BoundField DataField="GESTOR" HeaderText="Gestor" />
                            <asp:BoundField DataField="EFIFECENTGES" HeaderText="Fec entrega Gestor" SortExpression="EJEFISGLOBAL.EFIFECENTGES" DataFormatString="{0:dd/MM/yyyy}" />

                            <%--<asp:BoundField DataField="termino" HeaderText="Término" />--%>

                            <asp:TemplateField HeaderText="Término">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("termino") %>' ToolTip='<%# Bind("explicacion") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FECHALIMITE" HeaderText="Feha Límite" DataFormatString="{0:dd/MM/yyyy}">
                                <ItemStyle CssClass="fechalimite" />
                            </asp:BoundField>

                            <asp:ImageField DataImageUrlField="PictureURL"></asp:ImageField>

                            <asp:ButtonField ButtonType="Button" Text="Editar">
                                <ControlStyle CssClass="GridEditButton button" />
                            </asp:ButtonField>

                            <%-- Para solicitar la reasignación--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkReasignar" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Para solicitar la priorización--%>
                            <asp:ButtonField ButtonType="Button" CommandName="cmdPriorizar" Text="Priorizar">
                                <ControlStyle CssClass="GridEditButton button" />
                            </asp:ButtonField>

                            <asp:BoundField DataField="ID_TAREA_ASIGNADA">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VAL_PRIORIDAD">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COD_ESTADO_OPERATIVO">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>

                            <asp:BoundField DataField="COLORSUSPENSION">
                                <ItemStyle CssClass="BoundFieldItemStyleHidden alertasuspension" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="COLORALERTA"> <%--25--%>
                                <ItemStyle CssClass="BoundFieldItemStyleHidden alertaestadooperativo" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                            </asp:BoundField>

                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="cmdFirst" runat="server" Text="Primero" CssClass="PCGButton button" />
                    <asp:Button ID="cmdPrevious" runat="server" Text="Anterior" CssClass="PCGButton button" />
                    <asp:Label ID="lblPageNumber" runat="server" />&nbsp;&nbsp;
                    <asp:Button ID="cmdNext" runat="server" Text="Siguiente" CssClass="PCGButton button" />
                    <asp:Button ID="cmdLast" runat="server" Text="Ultimo" CssClass="PCGButton button" />

                    <asp:Button ID="btnSolicitarReasignacion" runat="server" Text="Solicitar reasignación" CssClass="PCGButton button" Style="float: right" />
                    <asp:Button ID="btnSolicitarCambioEstado" runat="server" Text="Solicitar cambio de estado" CssClass="PCGButton button" Style="float: right" OnClientClick="abrirPopUpCambioEstado()" Visible="false" />
                    <asp:HiddenField ID="hdnExpedientes" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
        <div id="Totales" style="background-color: White; color: Red; margin-top: 20px; margin-top: 0px; padding-top: 0px;">
            <table id="tblTotales" class="ui-widget-content">
                <tr>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Total Deuda
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalDeuda" runat="server" CssClass="ui-widget" Style="text-align: right;" ReadOnly="true" />
                    </td>
                    <td>&nbsp;
                    </td>
                    <td class="ui-widget-header">Total pagos capital
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalPagos" runat="server" CssClass="ui-widget" Style="text-align: right;" ReadOnly="true" />
                    </td>
                    <td class="ui-widget-header">Total saldo capital actual
                    </td>
                    <td>
                        <asp:TextBox ID="txtSaldoCapital" runat="server" CssClass="ui-widget" Style="text-align: right;" ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </div>


        <asp:Panel ID="pnlError" runat="server" Style="width: 524px; position: static; display: none; margin-top: 0px; margin-left: 30px">

            <div style="margin: 0  0 5px 0;">
                <% 
                    If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                        Response.Write(ViewState("Erroruseractivo"))
                    End If
                %>
            </div>
            <hr />

            <asp:Button Style="width: 100px; margin-left: 150px; margin-top: -40px" ID="btnNoerror"
                runat="server" Text="Iniciar sesión" Height="23px"></asp:Button>
        </asp:Panel>

        <asp:Button ID="Button3" runat="server" Text="Button" Style="visibility: hidden" />

        <asp:ModalPopupExtender ID="ModalPopupError" runat="server"
            TargetControlID="Button3"
            PopupControlID="pnlError"
            CancelControlID="btnNoerror"
            DropShadow="false">
        </asp:ModalPopupExtender>

        <script type="text/javascript">
            function mpeSeleccionOnCancel() {
                var pagina = '../../Login.aspx'
                location.href = pagina
                return false;
            }
        </script>

        <div id="dialog-modal" title="Por favor espere..." style="text-align: center">
            <span id="procesando_div" style="display: none;">
                <img src="../images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
            </span>
        </div>

        <!-- Solicitudes sobre el expediente -->
        <ucReasignacion:SolicitudReasignacion ID="SolicitudReasignacionPanel" runat="server" />
        <ucPriorizacion:SolicitudPriorizacion ID="SolicitudPriorizacionControl" runat="server" />


        <script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>
    </form>

    <!-- Bootstrap -->
    <link href="<%=ResolveClientUrl("~/assets/bootstrap/css/bootstrap.min.css") %>" rel="stylesheet" />
    <script src="<%=ResolveClientUrl("~/assets/bootstrap/js/bootstrap.min.js") %>" type="text/javascript"></script>
    <script>
        $.fn.bootstrapBtn = $.fn.button.noConflict();
    </script>
</body>
</html>

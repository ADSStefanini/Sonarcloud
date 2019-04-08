<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EJEFISGLOBAL_MASIVO.aspx.vb"
    Inherits="coactivosyp.EJEFISGLOBAL_MASIVO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>EJEFISGLOBAL MASIVO</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            EndRequestHandler();
        });
        function EndRequestHandler() {
            $('#cmdAddNew').button();
            $('#cmdSearch').button();
            $('#cmdFirst').button();
            $('#cmdNext').button();
            $('#cmdLast').button();
            $('#cmdPrevious').button();
            $('#cmdMostrarVencer').button();
            $('#cmdMostrarVencidos').button();
            $('.GridEditButton').button();            
            $('#btnExportarGrid').button();            

            $('#lnkNumExpVencer').click(function() {
                window.open('EstadisticaxVencer.aspx', 'Estadistica de expedientes por vencer', 'width=600,height=250');
                return false;
            });

            $('#lnkNumExpVencidos').click(function() {
                window.open('EstadisticaVencidos.aspx', 'Estadistica de expedientes vencidos', 'width=600,height=250');
                return false;
            });

            $('#lnkMsjNoLeidos').click(function() {
                window.open('MENSAJES.aspx', 'Visor de mensajes', 'width=780,height=450');
                return false;
            });
            
            $(".PCG-Content tr:gt(0)").mouseover(function() {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function() {
                $(this).removeClass("ui-state-highlight");
            });

            // xxx //
            //Array para dar formato en español
            $.datepicker.regional['es'] =
                {
                    
                    closeText: 'Cerrar',
                    prevText: 'Previo',
                    nextText: 'Próximo',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    monthStatus: 'Ver otro mes', yearStatus: 'Ver otro año',
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sáb'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    dateFormat: 'dd/mm/yy', firstDay: 1,
                    initStatus: 'Seleccione la fecha', isRTL: false
                };
            $.datepicker.setDefaults($.datepicker.regional['es']);

            $("#txtFechaPrimerPago").keypress(function(event) { event.preventDefault(); });
            $("#txtFechaPrimerPago").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFechaPrimerPago').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: null,
                minDate: new Date(2007, 6, 12),
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFechaSegundoPago").keypress(function(event) { event.preventDefault(); });
            $("#txtFechaSegundoPago").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFechaSegundoPago').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: null,
                minDate: new Date(2007, 6, 12),
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });


            /////               
            $("#txtSearchFECTITULO").keypress(function(event) { event.preventDefault(); });            
            $("#txtSearchFECTITULO").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });                        
            $('#txtSearchFECTITULO').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: new Date,
                minDate: new Date(2007, 6, 12),
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });
            /////
            $("#txtSearchEFIFECENTGES").keypress(function(event) { event.preventDefault(); });
            $("#txtSearchEFIFECENTGES").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtSearchEFIFECENTGES').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: new Date,
                minDate: new Date(2007, 6, 12),
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });
            
            /*
            $("#imgBtnBorraFechaTR").click(function() {
                $("#txtSearchFECTITULO").val("");
            });
            */
            // xxx //
        }
    </script>

    <style type="text/css">
        body
        {
            background-color: #01557C;
        }
        *
        {
            font-size: 12px;
            font-family: Arial;
        }
        th
        {
            padding-left: 8px;
            padding-right: 8px;
        }
        .BoundFieldItemStyleHidden
        {
            display: none;
        }
        .BoundFieldHeaderStyleHidden
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
    </div>
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td colspan="10" background="images/resultados_busca.jpg" height="42">
                <div style="color: White; font-weight: bold; width: 500px; height: 20px; float: left">
                    <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
                    <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                        Text="Label"></asp:Label>)</span></div>
                <div style="color: White; width: 360px; height: 20px; float: right; text-align: right">
                    
                    <!-- Regresar -->
                    <div style="color:White; width:30px; height:20px; float:left; text-align:left; padding-left:8px;">
                        <asp:LinkButton ID="ABack" runat="server" ToolTip="Regresar a la ventana anterior">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                    </div>
                                                    
                    <!-- Mensajes -->
                    <div style="color: White; width: 50px; height: 20px; float: left; text-align: right;">
                        <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Mensajes">
                                    <img alt ="Mensajes"  src="../images/icons/comentarios.png" height="18" width="18" style=" vertical-align:middle" id="img3" title="Mensajes" />
                        </asp:LinkButton>
                        <span>
                            <%  Response.Write("(" & Session("ssNumMsgNoLeidos") & ")")%>&nbsp&nbsp</span>
                    </div>
                    
                    <!-- Capturar intereses -->
                    <div style="color: White; width: 60px; height: 20px; float: left; text-align: right;">
                        <asp:LinkButton ID="lnkInteres" runat="server" ToolTip="Capturar intereses"><img alt ="" src="../images/icons/intereses.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                    </div>
                    <!-- Intereses de multas -->
                    <div style="color: White; width: 50px; height: 20px; float: left; text-align: right">
                        <asp:LinkButton ID="lnkInterMultas" runat="server" ToolTip="Capturar intereses de multas"><img alt ="" src="../images/icons/intermultas.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                    </div>
                    <!-- Subir SQL -->
                    <div style="color: White; width: 50px; height: 20px; float: left; text-align: right">
                        <asp:LinkButton ID="lnkSql" runat="server" ToolTip="Subir sql"><img alt ="" src="../images/icons/sql.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                    </div>
                    <!-- Cerrar sesion -->
                    <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                    <span>Cerrar sesión&nbsp</span>                                        
                    
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                <table style="width: 100%">
                    <tr>
                        <td class="ui-widget-header">
                            Fecha recepción Título
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchFECTITULO" runat="server" style="width:90px;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnBorraFechaRT" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de recepción del título" />
                        </td>
                        <td class="ui-widget-header">
                            Fec entrega gestor
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchEFIFECENTGES" runat="server" style="width:90px;"></asp:TextBox>
                            <asp:ImageButton ID="imgBtnBorraFechaEG" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de entrega al gestor" />
                        </td>
                        <td class="ui-widget-header">
                            Valor Deuda >=
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchEFIVALDEU" runat="server"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton ui-button ui-widget ui-state-default ui-corner-all"></asp:Button>
                        </td>
                        <td>
                            Paginación
                        </td>
                        <td>
                            <asp:DropDownList CssClass="ui-widget" ID="cboNumExp" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="ui-widget-header">
                            Tipo de título
                        </td>
                        <td>
                            <asp:DropDownList CssClass="ui-widget" id="cboMT_tipo_titulo" runat="server"></asp:DropDownList>
                            <%--<asp:TextBox ID="txtSearchEFINIT" runat="server"></asp:TextBox>--%>
                        </td>
                        <td class="ui-widget-header">
                            Estado actual
                        </td>
                        <td>
                            <asp:DropDownList CssClass="ui-widget" ID="cboEFIESTADO" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="cmdGenXLS" runat="server" Text="Generar XLS Liq. Ofi." class="PCGButton ui-button ui-widget ui-state-default ui-corner-all">
                            </asp:Button>
                        </td>
                        <%--<td>
                            <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" />
                        </td>--%>
                        <td>
                            <asp:Button ID="cmdGenXLS2" runat="server" Text="Generar XLS Sanción" class="PCGButton ui-button ui-widget ui-state-default ui-corner-all">
                            </asp:Button>
                        </td>

                        <td>
                            <asp:Button ID="cmdMasivos" runat="server" Text="Masivos" class="PCGButton ui-button ui-widget ui-state-default ui-corner-all">
                            </asp:Button>
                        </td>
                        <td>
                            <asp:Button ID="CambioEstadoMasivo" runat="server" Text="Cambio estado" class="PCGButton ui-button ui-widget ui-state-default ui-corner-all">
                            </asp:Button>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="OpcionesXLS" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                        </td>
                        <td style="width:331px">
                            &nbsp
                        </td>
                        <td class="ui-widget-header" style="width:100px">
                            Fecha 1er pago
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaPrimerPago" runat="server" style="width:103px"></asp:TextBox>
                        </td>
                        <td style="width:14px">
                            &nbsp
                        </td>
                        <td class="ui-widget-header" style="width:100px">
                            Fecha 2do pago
                        </td>
                        <td>
                            <asp:TextBox ID="txtFechaSegundoPago" runat="server" style="width:103px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="cmdAceptarXLSMultas" runat="server" Text="Aceptar" class="PCGButton ui-button ui-widget ui-state-default ui-corner-all">
                            </asp:Button>
                        </td>
                    </tr>
                </table>  
                </div>              
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content"
                    AllowSorting="True">
                    <Columns>
                        <asp:BoundField DataField="EFINROEXP">
                            <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                            <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EFINROEXP" HeaderText="Expediente" HeaderStyle-HorizontalAlign ="Center" ItemStyle-HorizontalAlign ="Center"
                        SortExpression="EJEFISGLOBAL.EFINROEXP">
                        </asp:BoundField>
                        <asp:BoundField DataField="EFIFECHAEXP" HeaderText="Fecha recepción título" SortExpression="EJEFISGLOBAL.EFIFECHAEXP"
                            DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                        <asp:BoundField DataField="ED_NOMBRE" HeaderText="Nombre del deudor" SortExpression="ENTES_DEUDORES.ED_NOMBRE">
                        </asp:BoundField>
                        <asp:BoundField DataField="EFINIT" HeaderText="NIT / CC" SortExpression="EJEFISGLOBAL.EFINIT">
                        </asp:BoundField>
                        <asp:BoundField DataField="NomTipoTitulo" HeaderText="Tipo de Título" SortExpression="TITULOSEJECUTIVOS.NomTipoTitulo">
                        </asp:BoundField>
                        <asp:BoundField DataField="EFIESTADO" HeaderText="Estado actual" />
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
                        <asp:BoundField DataField="EFIFECENTGES" HeaderText="Fec entrega Gestor" DataFormatString="{0:dd/MM/yyyy}">
                        </asp:BoundField>
                        <asp:BoundField DataField="termino" HeaderText="Término" />
                        <asp:ImageField DataImageUrlField="PictureURL">
                        </asp:ImageField>
                        <asp:TemplateField ControlStyle-Width="20px" FooterStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign ="Center">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="EfiCheck" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckHeader" OnCheckedChanged="CheckHeader_OnCheckedChanged" AutoPostBack="true" 
                                    runat="server" />
                            </HeaderTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="20px" Wrap="False"  />
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="ui-widget-header" />
                    <RowStyle CssClass="ui-widget-content" />
                    <AlternatingRowStyle />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="cmdFirst" runat="server" Text="Primero" CssClass="PCGButton"></asp:Button>
                <asp:Button ID="cmdPrevious" runat="server" Text="Anterior" CssClass="PCGButton">
                </asp:Button>
                <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
                <asp:Button ID="cmdNext" runat="server" Text="Siguiente" CssClass="PCGButton"></asp:Button>
                <asp:Button ID="cmdLast" runat="server" Text="Ultimo" CssClass="PCGButton"></asp:Button>
            </td>
        </tr>
    </table>
    <div id="Totales" style="background-color: White; color: Red; margin-top: 20px; margin-top: 0px;
        padding-top: 0px;">
        <table id="tblTotales" class="ui-widget-content">
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="ui-widget-header">
                    Total Deuda
                </td>
                <td>
                    <asp:TextBox ID="txtTotalDeuda" runat="server" MaxLength="20" CssClass="ui-widget"
                        Style="text-align: right;" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="ui-widget-header">
                    Total pagos capital
                </td>
                <td>
                    <asp:TextBox ID="txtTotalPagos" runat="server" MaxLength="20" CssClass="ui-widget"
                        Style="text-align: right;" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="ui-widget-header">
                    Total saldo capital actual
                </td>
                <td>
                    <asp:TextBox ID="txtSaldoCapital" runat="server" MaxLength="20" CssClass="ui-widget"
                        Style="text-align: right;" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

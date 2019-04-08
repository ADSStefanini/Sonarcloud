<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPAGOSLiquidacionSancion.aspx.vb"
    Inherits="coactivosyp.EditPAGOSLiquidacionSancion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar pagos </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <script src="jquery.ui.button.js" type="text/javascript"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <link href="css/csstable.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(function() {

            $('#cmdSave').button();
            $('#cmdSaveSancion').button();
            $('#cmdCancel').button();
            $('#cmdSolicitudCambioEstado').button();
            $('#btnExp').button();


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

            //------------------------------------------------------------------//
            //Ocultar todos los DatePicker si el estado del proceso es devuelto o terminado
            var PerfilUser = '<% Response.write(Session("mnivelacces")) %>';
            // alert(PerfilUser);

            if (PerfilUser == '6') {
                //
            } else {
                $.datepicker.datepicker('disable');
            }

            //------------------------------------------------------------------//
            $("#txtFecSolverif").keypress(function(event) { event.preventDefault(); });
            $("#txtFecSolverif").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecSolverif').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            //------------------------------------------------------------------//
            $("#txtFecVerificado").keypress(function(event) { event.preventDefault(); });
            $("#txtFecVerificado").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecVerificado').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtpagFecha").keypress(function(event) { event.preventDefault(); });
            $("#txtpagFecha").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtpagFecha').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtpagFechaSancion").keypress(function(event) { event.preventDefault(); });
            $("#txtpagFechaSancion").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtpagFechaSancion').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtpagFechaDeudor").keypress(function(event) { event.preventDefault(); });
            $("#txtpagFechaDeudor").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtpagFechaDeudor').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtpagFechaDeudorSancion").keypress(function(event) { event.preventDefault(); });
            $("#txtpagFechaDeudorSancion").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtpagFechaDeudorSancion').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtpagFecExi").keypress(function(event) { event.preventDefault(); });
            $("#txtpagFecExi").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtpagFecExi').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

        });
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
        tr
        {
            border-collapse: 1px solid #ccc;
        }
        td
        {
            padding: 2px;
        }
        #encabezado
        {
            background: url(images/resultados_busca.jpg);
            height: 37px;
            width: 100%;
            border: solid 1px #a6c9e2;
            padding-bottom: 5px;
        }
        #tituloencabezado
        {
            color: White;
            margin-top: 16px;
            margin-left: 2px;
            font: 12px Verdana;
            font-weight: bold;
        }
        #infoexpediente, #infoexpediente2, #infoexpediente3
        {
            background-color: #FFFFFF;
            width: 100%;
            margin-bottom: 10px;
            margin-top: 10px;
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
            font-weight: bold;
            font-size: 11px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            border-collapse: collapse;
        }
        .btn { float:left;}
        .auto-style1 {
            height: 21px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!---------------------- -->
    <div id="encabezado" runat="server"> 
        <div id="tituloencabezado">
            <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
            <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                Text="Label"></asp:Label>)</span>
            <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
                padding-right: 8px;">
                <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>
            </div>
            <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
                padding-right: 8px;">
                <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>
            </div>
              <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
                padding-right: 8px;">
                <asp:LinkButton ID="AHome" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/turn_left.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al Inicio" /></asp:LinkButton>
            </div>
        </div>
    </div>
    <!---------------------- -->
    <div class="tablecss">
         <table id="infoexpediente" runat="server">
            <tr>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 100px; float: left; height: 20px">
                            No. Expediente:</label>
                        <asp:TextBox ID="txtNroExpEnc" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 100px; float: left; height: 20px">
                            Cédula / NIT:</label>
                        <asp:TextBox ID="txtIdDeudor" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td colspan="2" style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 100px; float: left; height: 20px">
                            Nombre deudor:</label>
                        <asp:TextBox ID="txtNombreDeudor" TextMode="SingleLine" runat="server" Width="350px"
                            ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 100px; float: left; height: 20px">
                            Estado:</label>
                        <asp:TextBox ID="txtNombreEstado" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
               
                <td style="color: #2e6e9e">
                 
                </td>
            </tr>
            <!-- 2da Linea -->
            <tr>
                <td style="color: #2e6e9e;">
                    <div style="width: 160px; height: 41px">
                        <label style="width: 170px; float: left; height: 30px">
                            Tipo Título:</label>
                        <asp:TextBox ID="txtTIPOTITULO" runat="server" Width="220px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 30px">
                            Número Título Ejecutivo:</label>
                        <asp:TextBox ID="txtNUMTITULOEJECUTIVO" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 30px">
                            Fecha Título Ejecutivo:</label>
                        <asp:TextBox ID="txtFECTITULO" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 30px">
                            No. res. resuelve apelación o reconsideración:</label>
                        <asp:TextBox ID="txtresolucionApelacion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 30px">
                            Fecha res. resuelve apelación o reconsideració:</label>
                        <asp:TextBox ID="txtfechaapelacionreconsideracion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 20px">
                            Fecha Ejecutoría:</label>
                        <asp:TextBox ID="txtFechaEjecutoria" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e;">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 20px">
                            Fecha exigibilidad:</label>
                        <asp:TextBox ID="txtFechaExigTitulo" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 20px">
                            Fecha Prescripción:</label>
                        <asp:TextBox ID="txtFechaPrescripcion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <!-- 3da Linea -->
            <tr>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 20px">
                            Expediente Origen :</label>
                        <asp:TextBox ID="txtExpOrigen" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 170px; height: 41px">
                        <label style="width: 160px; float: left; height: 20px">
                            Expediente Documentic :</label>
                        <asp:TextBox ID="txtExpDocumentic" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div></td>
                <td colspan=2 style="color: #2e6e9e">
                     <div style="width: 170px; height: 41px">
                        <label style="width: 100px; float: left; height: 20px">
                            Gestor actual:</label>
                        <asp:TextBox ID="txtGestorResp" runat="server" Width="350px" ReadOnly="True" Style="float: left"></asp:TextBox>
                    </div></td>
                <td style="color: #2e6e9e">
                      <div style="width: 170px; height: 41px">
                        <label style="width: 170px; float: left; height: 20px">
                            Fecha entrega a gestor:</label>
                        <asp:TextBox ID="txtFECENTREGAGESTOR" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left"></asp:TextBox>
                    </div></td>
                
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Total de la Deuda:</label>
                        <asp:TextBox ID="txtTotalDeudaEA" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Capital inicial:</label>
                        <asp:TextBox ID="txtCapitalInicial" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Valor Revocatoria:</label>
                        <asp:TextBox ID="txtValorRevocatoria" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left; text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Saldo actual:</label>
                        <asp:TextBox ID="txtSaldoEA" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <!-- 4da Linea -->
            <tr>    
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Pagos a capital:</label>
                        <asp:TextBox ID="txtPagosCapitalEA" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left; text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label id="lblInte" runat="server" style="width: 180px; float: left; height: 20px">
                            Pago a Intereses:</label>
                        <asp:TextBox ID="txtpagInteresLiq" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Total Pagados:</label>
                        <asp:TextBox ID="txttotalpagLiq" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <!-- 5ta Linea -->
            <tr id="trsancion" runat="server">
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Capital inicial Sanción:</label>
                        <asp:TextBox ID="txtTotalDeudaSancion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left; text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Pagos capital Sanción:</label>
                        <asp:TextBox ID="txtPagosCapitalSancion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left; text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Valor IPC:</label>
                        <asp:TextBox ID="txtValorIPC" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Saldo actual Sanción:</label>
                        <asp:TextBox ID="txtsandoactualSancion" runat="server" Width="170px" ReadOnly="True"
                            Style="float: left; text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
                <td style="color: #2e6e9e">
                    <div style="width: 180px; height: 41px">
                        <label style="width: 180px; float: left; height: 20px">
                            Fecha ult. pago Sanción:</label>
                        <asp:TextBox ID="txtUltPag" runat="server" Width="170px" ReadOnly="True" Style="float: left;
                            text-align: right; color: Red"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="cmdSolicitudCambioEstado" runat="server" Text="Solicitud de cambio de estado" />
                </td>
            </tr>
        </table>
    </div>
    
   <table id="tblEditPAGOS" class="ui-widget-content">
        <%--<tr>
                        <td colspan="10" background="images/resultados_busca.jpg" height="42">                            
                            <div style="color:White; font-weight:bold; width:460px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                            <div style="color:White; width:340px; height:20px; float:right; text-align:right">
                                <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                                <span>Cerrar sesión</span>
                            </div>
                        </td>
                    </tr>--%>
        <tr>
            <td class="auto-style1">
            </td>
            <td colspan="2" class="auto-style1">
                  <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="Guardar Liquidación" CssClass="PCGButton">
                </asp:Button>
                <asp:Button ID="cmdSaveSancion" runat="server" Text="Guardar Sanción" CssClass="PCGButton">
                </asp:Button>
                <asp:Button ID="btnExp" runat="server" Text="Exportar Relacion IPC" CssClass="PCGButton">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                No. validador
            </td>
            <td>
                <asp:TextBox ID="txtNroConsignacion" runat="server" MaxLength="50" CssClass="ui-widget"
                    ForeColor="Red" Style="width: 300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                No. Consignación Sanción
            </td>
            <td>
                <asp:TextBox ID="txtNroConsignacionSancion" runat="server" MaxLength="50" CssClass="ui-widget"
                    ForeColor="Red" Style="width: 300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                No. Expediente
            </td>
            <td>
                <asp:TextBox ID="txtNroExp" runat="server" MaxLength="12" CssClass="ui-widget" ForeColor="Red"
                    ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha reporte del pago por gestor responsable
            </td>
            <td>
                <asp:TextBox ID="txtFecSolverif" runat="server" CssClass="ui-widget" MaxLength="10"
                    ForeColor="Red"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de verificación del pago
            </td>
            <td>
                <asp:TextBox ID="txtFecVerificado" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Estado del pago
            </td>
            <td>
                <asp:DropDownList CssClass="ui-widget" ID="cboestado" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de pago Liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagFecha" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de pago Sanción
            </td>
            <td>
                <asp:TextBox ID="txtpagFechaSancion" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de reporte del pago por el deudor liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagFechaDeudor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de reporte del pago por el deudor Sanción
            </td>
            <td>
                <asp:TextBox ID="txtpagFechaDeudorSancion" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                No.Título Judicial
            </td>
            <td>
                <asp:TextBox ID="txtpagNroTitJudicial" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Capital pagado Liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagCapital" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Capital pagado Sanción
            </td>
            <td>
                <asp:TextBox ID="txtpagCapitalSancion" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Ajuste Decreto 1406
            </td>
            <td>
                <asp:TextBox ID="txtpagAjusteDec1406" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td id="intIPC" runat="server" class="ui-widget-header">
                Intereses pagados liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagInteres" runat="server" CssClass="ui-widget"></asp:TextBox>
                <asp:ImageButton ID="imgBtnBorra" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                    ToolTip="Colocar en 0 " />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td id="Td1" runat="server" class="ui-widget-header">
                IPC Sanción
            </td>
            <td>
                <asp:TextBox ID="txtIpcSancion" runat="server" CssClass="ui-widget"></asp:TextBox>
                <asp:ImageButton ID="ImgBtnLimpiarSancion" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                    ToolTip="Colocar en 0 " />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Gastos del proceso
            </td>
            <td>
                <asp:TextBox ID="txtpagGastosProc" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Pagos en exceso liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagExceso" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Pagos en exceso Sanción
            </td>
            <td>
                <asp:TextBox ID="txtpagExcesoSancion" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Total pagado Liquidación
            </td>
            <td>
                <asp:TextBox ID="txtpagTotal" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Actualizar total pagado Liquidación"
                    ImageUrl="~/Security/images/icons/turn_left.png" ToolTip="Actualizar total pagado Liquidación" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Total pagado Sanción
            </td>
            <td>
                <asp:TextBox ID="txtPagTotalSancion" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                <asp:ImageButton ID="ImgTotalSancion" runat="server" AlternateText="Actualizar total pagado Sanción"
                    ImageUrl="~/Security/images/icons/turn_left.png" ToolTip="Actualizar total pagado Sanción" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Estado del proceso en la fecha de reporte del pago
            </td>
            <td>
                <asp:DropDownList CssClass="ui-widget" ID="cbopagestadoprocfrp" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                Datos de acuerdos / facilidades de pago
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Fecha de exigibilidad
            </td>
            <td>
                <asp:TextBox ID="txtpagFecExi" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Tasa de intereses de mora aplicable
            </td>
            <td>
                <asp:TextBox ID="txtpagTasaIntApl" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Días de mora
            </td>
            <td>
                <asp:TextBox ID="txtpagdiasmora" runat="server" CssClass="ui-widget" MaxLength="4"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                Valor Cuota
            </td>
            <td>
                <asp:TextBox ID="txtpagvalcuota" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                No. confirmación pago
            </td>
            <td>
                <asp:TextBox ID="txtpagNumConPag" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
          <td colspan="3">
                <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 340px;">
                    <iframe src="PAGOSOBSERVACIONES.aspx?pExpediente= <%  Response.Write(Request("pExpediente"))%> "
                        width="960" height="740" scrolling="no" frameborder="0"></iframe>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

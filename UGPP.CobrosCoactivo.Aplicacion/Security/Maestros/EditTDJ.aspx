<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditTDJ.aspx.vb" Inherits="coactivosyp.EditTDJ" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Manejo de Títulos de Depósito Judicial (TDJ)
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                // $('#cmdDelete').button();
                $('#cmdSave').button();
                $('#cmdCancel').button();
                $('#cmdFraccionar').button();
                $('#cmdAplicar').button();
                $('#cmdAdministrar').button();
                $('#cmdDevolver').button();
                            
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

                //Ocultar todos los DatePicker si el estado del proceso es devuelto o terminado
                var EstadoProc = '<%  Response.Write(Session("EstadoProcesoActual"))%>';
                if (EstadoProc == '04' || EstadoProc == '07') {
                    $.datepicker.datepicker('disable');
                }

                $("#txtFecRecibido").keypress(function(event) { event.preventDefault(); });
                $("#txtFecRecibido").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRecibido').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecEmision").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEmision").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEmision').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResolGes").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResolGes").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResolGes').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecEnvioMemo").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEnvioMemo").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEnvioMemo').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecDevol").keypress(function(event) { event.preventDefault(); });
                $("#txtFecDevol").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecDevol').datepicker({
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
	        * { font-size:12px; font-family:Arial}				 
	        .style2
            {
                margin-top: 0px;
            }
	        .style3
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                width: 262px;
            }
            .style4
            {
                height: 65px;
                width: 262px;
            }
            .style5
            {
                height: 65px;
            }
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditTDJ" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        No. de Depósito
                    </td>
                    <td>
                        <asp:TextBox id="txtNroDeposito" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        No. de Título Judicial 
                    </td>
                    <td>
                        <asp:TextBox id="txtNroTituloJ" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Valor del Título
                    </td>
                    <td>
                        <asp:TextBox id="txtValorTDJ" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Estado del Título
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboEstadoTDJ" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Fecha Recibido
                    </td>
                    <td>
                        <asp:TextBox id="txtFecRecibido" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Fecha Emisión del título
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEmision" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Consignante
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboConsignante" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Banco
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboBanco" runat="server"></asp:DropDownList>                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Observaciones
                    </td>
                    <td>
                        <asp:TextBox id="txtObservac" runat="server" CssClass="ui-widget" Columns="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" colspan="2">
                        GESTIÓN DEL TÍTULO DE DEPÓSITO JUDICIAL
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        No. de Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResolGes" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Fecha de Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResolGes" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        No. Memorando
                    </td>
                    <td>
                        <asp:TextBox id="txtNroMemoGes" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Fecha de envio de Memorando
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEnvioMemo" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Tipo de Resolución
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboTipoResolGes" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style3">
                        Fecha devolución
                    </td>
                    <td>
                        <asp:TextBox id="txtFecDevol" runat="server" CssClass="ui-widget"></asp:TextBox>
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
                    <td class=style5>
                        &nbsp;
                    </td>
                    <td class="style4">
                        <%--<asp:Button id="cmdDelete" runat="server" Text="Delete" cssClass="PCGButton"></asp:Button>--%>
                        <span lang="es">Adminstracion de titulos
                        <asp:Panel ID="Pnl_Admin" runat="server">
                            <span lang="es">
                            <asp:Button ID="cmdAdministrar" runat="server" Text="Administrar" 
                            CssClass="style2" Height="29px" Width="96px" />
                            &nbsp; <span lang="es">
                            <asp:Button ID="cmdFraccionar" runat="server" cssClass="PCGButton" 
                                Height="29px" Text="Fraccionar" Width="96px" />
                            <br />
                            <span lang="es">
                            <asp:Button ID="cmdAplicar" runat="server" Height="28px" Text="Aplicar" 
                                Width="96px" />
                            &nbsp; <span lang="es">
                            <asp:Button ID="cmdDevolver" runat="server" Height="29px" Text="Devolver" 
                                Width="95px" />
                            </span></span>
                            <br />
                            </span></span>
                        </asp:Panel>
                        </span>
                    </td>
                    <td class="style5">
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                        <span lang="es">&nbsp;
                        &nbsp;&nbsp;&nbsp;</span></td>
                </tr>
            </table>
        </form>
          <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
  </div>
    </body>
</html>

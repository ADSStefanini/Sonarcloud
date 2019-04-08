<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMANDAMIENTOS_PAGO.aspx.vb" Inherits="coactivosyp.EditMANDAMIENTOS_PAGO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>
            Editar mandamientos de pago
        </title>
                
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        
        <script type="text/javascript">
            $(function() {
                /* $('#cmdDelete').button(); */
                $('#cmdSaveMP').button();
                $('#cmdCancelMP').button();

                //
                $("input[type=text]").keyup(function() {
                    $(this).val($(this).val().toUpperCase());
                });
                //
                
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
                //var NomEstadoProc = $("#txtNombreEstado").val();
                var EstadoProc = '<%  Response.Write(Request("pEstadoExpediente"))%>';
                //alert(EstadoProc);

                if (EstadoProc == '04' || EstadoProc == '07') {
                    $.datepicker.datepicker('disable');
                }
                
                /*
                $("#txtFecResolMP").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResolMP").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResolMP').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                */
                /*
                $("#txtFecEnvCitaCAD").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEnvCitaCAD").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEnvCitaCAD').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                */
                //XXX
                $("#txtFecOfiCita").keypress(function(event) { event.preventDefault(); });
                $("#txtFecOfiCita").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCita').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                /*
                $("#txtFecEnvCitaEj").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEnvCitaEj").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEnvCitaEj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                */
                $("#txtFecReciCita").keypress(function(event) { event.preventDefault(); });
                $("#txtFecReciCita").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecReciCita').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotiPers").keypress(function(event) { event.preventDefault(); });
                $("#txtFecNotiPers").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotiPers').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecOfiNotCor").keypress(function(event) { event.preventDefault(); });
                $("#txtFecOfiNotCor").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiNotCor').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotiCor").keypress(function(event) { event.preventDefault(); });
                $("#txtFecNotiCor").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotiCor').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFijAviWeb").keypress(function(event) { event.preventDefault(); });
                $("#txtFecFijAviWeb").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFijAviWeb').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecDesAviWeb").keypress(function(event) { event.preventDefault(); });
                $("#txtFecDesAviWeb").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecDesAviWeb').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotAvi").keypress(function(event) { event.preventDefault(); });
                $("#txtFecNotAvi").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotAvi').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubPrensa").keypress(function(event) { event.preventDefault(); });
                $("#txtFecPubPrensa").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubPrensa').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecCondConc").keypress(function(event) { event.preventDefault(); });
                $("#txtFecCondConc").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecCondConc').datepicker({
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
		    /* body{ background-color:#01557C}	*/
	        * { font-size:12px; font-family:Arial;}				 
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditMANDAMIENTOS_PAGO" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nro. Resolución Mandamiento Pago
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResolMP" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Resolución Mandamiento pago
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResolMP" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de envío citación a CAD
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEnvCitaCAD" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Oficio de citación
                    </td>
                    <td>
                        <asp:TextBox id="txtNroOfiCita" runat="server" MaxLength="32" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Oficio Citación
                    </td>
                    <td>
                        <asp:TextBox id="txtFecOfiCita" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha envío citación a ejecutado
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEnvCitaEj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de recibo citación
                    </td>
                    <td>
                        <asp:TextBox id="txtFecReciCita" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Notificación Personal
                    </td>
                    <td>
                        <asp:TextBox id="txtFecNotiPers" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Acta Notificación personal
                    </td>
                    <td>
                        <asp:TextBox id="txtNroActaNotPer" runat="server" MaxLength="40" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Oficio notificación por correo
                    </td>
                    <td>
                        <asp:TextBox id="txtNroOfiNotCor" runat="server" MaxLength="30" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Oficio Notificación por correo
                    </td>
                    <td>
                        <asp:TextBox id="txtFecOfiNotCor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de notificación por correo
                    </td>
                    <td>
                        <asp:TextBox id="txtFecNotiCor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de Fijación aviso en pág. web
                    </td>
                    <td>
                        <asp:TextBox id="txtFecFijAviWeb" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de desfijación aviso en pág. web
                    </td>
                    <td>
                        <asp:TextBox id="txtFecDesAviWeb" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de notificación por aviso
                    </td>
                    <td>
                        <asp:TextBox id="txtFecNotAvi" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de publicación en prensa 
                    </td>
                    <td>
                        <asp:TextBox id="txtFecPubPrensa" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Medio de publicación
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtMedioPub" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:DropDownList CssClass="ui-widget" id="cboMedioPub" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha conducta concluyente
                    </td>
                    <td>
                        <asp:TextBox id="txtFecCondConc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
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
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button id="cmdCancelMP" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSaveMP" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
        <div style="margin-left:0px; margin-top:4px; width:660px; height:340px;">
            <iframe id="ifObsMandamiento" name="ifObsMandamiento" src="NOTAS.aspx?pExpediente=<%  Response.Write(Request("pExpediente"))%>&pModulo=22" width="660" height="340" scrolling="no" frameborder="0"></iframe>
        </div>
    </body>
</html>

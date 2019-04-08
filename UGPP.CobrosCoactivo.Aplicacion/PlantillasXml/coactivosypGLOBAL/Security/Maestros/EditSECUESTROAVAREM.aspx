<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditSECUESTROAVAREM.aspx.vb" Inherits="coactivosyp.EditSECUESTROAVAREM" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Gestión de Secuestro, Avaluo y Remate</title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {

                $('#cmdSave').button();
                $('#cmdBack').button();                
            
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
                
                //Calendarios
                $("#txtFecResSec").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResSec").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResSec').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                $("#txtFecResAva").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResAva").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResAva').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecObjAva").keypress(function(event) { event.preventDefault(); });
                $("#txtFecObjAva").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecObjAva').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResApruAva").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResApruAva").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResApruAva').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotAutoAva").keypress(function(event) { event.preventDefault(); });
                $("#txtFecNotAutoAva").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotAutoAva').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResRem").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResRem").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResRem').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecLicita1").keypress(function(event) { event.preventDefault(); });
                $("#txtFecLicita1").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecLicita1').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecLicita2").keypress(function(event) { event.preventDefault(); });
                $("#txtFecLicita2").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecLicita2').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecLicita3").keypress(function(event) { event.preventDefault(); });
                $("#txtFecLicita3").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecLicita3').datepicker({
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
	    </style>
	    
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEncabezado" class="ui-widget-content">
                <tr>
                    <td colspan="3">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Tipo de bien
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtTipoBienSec" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:DropDownList CssClass="ui-widget" id="cboTipoBienSec" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Identificación del bien
                    </td>
                    <td>
                        <asp:TextBox id="txtIdBienSec" runat="server" MaxLength="30" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
            <table id="tblEditSECAVAREM" class="ui-widget-content">
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        Secuestro
                    </td>
                </tr>
                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResSec" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResSec" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado
                    </td>
                    <td>
                        <asp:TextBox id="txtEstadoSec" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Número de identificación del secuestre
                    </td>
                    <td>
                        <asp:TextBox id="txtIdSecuestre" runat="server" MaxLength="13" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nombre secuestre
                    </td>
                    <td>
                        <asp:TextBox id="txtNomSecuestre" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        Avalúo
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Auto que ordena avalúo
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResAva" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha auto
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResAva" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Oficio traslado avalúo
                    </td>
                    <td>
                        <asp:TextBox id="txtOfiTrasAva" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha presentación objeción al avalúo
                    </td>
                    <td>
                        <asp:TextBox id="txtFecObjAva" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Auto que aprueba avalúo
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResApruAva" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha auto
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResApruAva" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de notificación del auto que aprueba avalúo
                    </td>
                    <td>
                        <asp:TextBox id="txtFecNotAutoAva" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        Orden de remate
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Auto
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResRem" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha auto
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResRem" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha <span lang="es">Audiencia </span>1
                    </td>
                    <td>
                        <asp:TextBox id="txtFecLicita1" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha <span lang="es">Audiencia </span>2
                    </td>
                    <td>
                        <asp:TextBox id="txtFecLicita2" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha <span lang="es">Audiencia</span> 3
                    </td>
                    <td>
                        <asp:TextBox id="txtFecLicita3" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan=3>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                    <td>
                        <asp:Button id="cmdBack" runat="server" Text="Regresar a la lista de embargo" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

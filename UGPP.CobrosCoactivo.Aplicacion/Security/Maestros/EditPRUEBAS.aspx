<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPRUEBAS.aspx.vb" Inherits="coactivosyp.EditPRUEBAS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditPRUEBAS
        </title>
                
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        
        <script type="text/javascript">
            $(function() {
                
                $('#cmdSave').button();
                $('#cmdCancel').button();

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

                // Evitar que el usuario edite los controles de fecha
                $("#txtFecAutoPru").keypress(function(event) { event.preventDefault(); });
                $("#txtFecAutoPru").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecAutoPru').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecIniTerPro").keypress(function(event) { event.preventDefault(); });
                $("#txtFecIniTerPro").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecIniTerPro').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFinTerPro").keypress(function(event) { event.preventDefault(); });
                $("#txtFecFinTerPro").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFinTerPro').datepicker({
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
	        * { font-size:12px; font-family:Arial;}				 
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditPRUEBAS" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Resolución que decreta pruebas
                    </td>
                    <td>
                        <asp:TextBox id="txtNroAutoPru" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Resolución que decreta pruebas
                    </td>
                    <td>
                        <asp:TextBox id="txtFecAutoPru" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Inicio término probatorio
                    </td>
                    <td>
                        <asp:TextBox id="txtFecIniTerPro" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fin término probatorio
                    </td>
                    <td>
                        <asp:TextBox id="txtFecFinTerPro" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>


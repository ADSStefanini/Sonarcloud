<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditCUOTASPARTESDETALLE.aspx.vb" Inherits="coactivosyp.EditCUOTASPARTESDETALLE" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Edición de Detalle de proceso con Cuota Parte</title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                $('#cmdDelete').button();
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

                //--------------------------------------------//
                $("#txtPeriodoIniCob").keypress(function(event) { event.preventDefault(); });
                $("#txtPeriodoIniCob").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtPeriodoIniCob').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtPeriodoFinCob").keypress(function(event) { event.preventDefault(); });
                $("#txtPeriodoFinCob").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtPeriodoFinCob').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaPresPerAnt").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaPresPerAnt").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaPresPerAnt').datepicker({
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
		    * { font-size:12px; font-family:Arial; }		   		   
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditCUOTASPARTESDETALLE" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Número de identificación *
                    </td>
                    <td>
                        <asp:TextBox id="txtPensionado" runat="server" MaxLength="14" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Beneficiario / causante *
                    </td>
                    <td>
                        <asp:TextBox id="txtNomPensionado" runat="server" MaxLength="70" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Valor capital cuota parte
                    </td>
                    <td>
                        <asp:TextBox id="txtCapitalCP" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Periodo inicial cobrado
                    </td>
                    <td>
                        <asp:TextBox id="txtPeriodoIniCob" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraPeriodoIniCob" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Periodo final cobrado
                    </td>
                    <td>
                        <asp:TextBox id="txtPeriodoFinCob" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFinCob" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha prescripción periodo mas antíguo
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaPresPerAnt" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFechaPresPerAnt" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fallo que ordena reconocimiento de pensión
                    </td>
                    <td>
                        <asp:TextBox id="txtFalloOrdenRecPen" runat="server" MaxLength="24" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Resolución que da cumplimiento al fallo
                    </td>
                    <td>
                        <asp:TextBox id="txtResolCumpleFallo" runat="server" MaxLength="24" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Observaciones
                    </td>
                    <td>
                        <asp:TextBox id="txtObservacion" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button id="cmdDelete" runat="server" Text="Borrar" cssClass="PCGButton"></asp:Button>
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

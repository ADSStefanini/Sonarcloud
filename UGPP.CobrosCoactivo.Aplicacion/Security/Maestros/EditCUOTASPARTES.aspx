<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditCUOTASPARTES.aspx.vb" Inherits="coactivosyp.EditCUOTASPARTES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Cuotas Partes</title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    
        <script type="text/javascript">
            $(function() {
                $('#cmdSave').button();
                $('#cmdEditPensionados').button();

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
                //-------------------------------------------------------------------------------//

                $("#txtFechaLey550").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaLey550").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaLey550').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaOfiConsulta").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaOfiConsulta").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaOfiConsulta').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecReciOfiConsul").keypress(function(event) { event.preventDefault(); });
                $("#txtFecReciOfiConsul").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecReciOfiConsul').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaEscritoObj").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaEscritoObj").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaEscritoObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaAcepCP").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaAcepCP").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaAcepCP').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaFormalSilPos").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaFormalSilPos").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaFormalSilPos').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaDocResObj").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaDocResObj").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaDocResObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResolCP").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResolCP").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResolCP').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFechaCuentaCobro").keypress(function(event) { event.preventDefault(); });
                $("#txtFechaCuentaCobro").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFechaCuentaCobro').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecEntregaCtaCobro").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEntregaCtaCobro").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEntregaCtaCobro').datepicker({
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
            <table id="tblEditCUOTASPARTES" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboEstadoEnte" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Ley 550
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaLey550" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecLey550" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha oficio consulta cuota
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaOfiConsulta" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecOfiConsulta" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de Recibido del oficio consulta cuota
                    </td>
                    <td>
                        <asp:TextBox id="txtFecReciOfiConsul" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecReciOfiConsul" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha escrito objeciones 
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaEscritoObj" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecescritoObj" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha aceptación cuota parte
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaAcepCP" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFechaAcepCP" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha formalización silencio positivo
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaFormalSilPos" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFechaFormalSilPos" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha documento resuelve objeciones
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaDocResObj" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFechaDocResObj" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Resolución determina cuota parte
                    </td>
                    <td>
                        <asp:TextBox id="txtNoResolCP" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de expedición Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResolCP" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecResolCP" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No Cuenta Cobro
                    </td>
                    <td>
                        <asp:TextBox id="txtNoCuentaCobro" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Cuenta Cobro
                    </td>
                    <td>
                        <asp:TextBox id="txtFechaCuentaCobro" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFechaCuentaCobro" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha entrega cuenta de cobro 
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEntregaCtaCobro" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecEntregaCtaCobro" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
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
                        <%--<asp:TextBox id="txtObservaciones" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:TextBox id="txtObservaciones" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                            
                    </td>
                    <td>
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdEditPensionados" runat="server" Text="Editar Beneficiarios / causantes" cssClass="PCGButton"></asp:Button>                            
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

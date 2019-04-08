<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPERSUASIVOOFICIOS.aspx.vb" Inherits="coactivosyp.EditPERSUASIVOOFICIOS" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Maestro de Oficios de persuasivo
        </title>
        
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


                // Evitar que el usuario edite los controles de fecha
                $("#txtFecOfi").keypress(function(event) { event.preventDefault(); });
                $("#txtFecOfi").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                //
                $("#txtFecEnvOfi").keypress(function(event) { event.preventDefault(); });
                $("#txtFecEnvOfi").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                //
                $("#txtFecAcuseO").keypress(function(event) { event.preventDefault(); });
                $("#txtFecAcuseO").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                
                
                $('#txtFecOfi').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                $('#txtFecEnvOfi').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                $('#txtFecAcuseO').datepicker({
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
            <table id="tblEditPERSUASIVOOFICIOS" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Tipificación
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboTipificacion" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nro. Oficio
                    </td>
                    <td>
                        <asp:TextBox id="txtNroOfi" runat="server" MaxLength="80" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Oficio
                    </td>
                    <td>
                        <asp:TextBox id="txtFecOfi" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecOfi" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Envío Oficio
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEnvOfi" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecEnvOfi" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Guía Entrega
                    </td>
                    <td>
                        <asp:TextBox id="txtNoGuiaEnt" runat="server" MaxLength="80" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Acuse
                    </td>
                    <td>
                        <asp:TextBox id="txtFecAcuseO" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecAcuseO" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Dirección envío
                    </td>
                    <td>
                        <asp:TextBox id="txtdirenvio" runat="server" MaxLength="120" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Email
                    </td>
                    <td>
                        <asp:TextBox id="txtemail" runat="server" MaxLength="80" CssClass="ui-widget"></asp:TextBox>
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
                                            Height="40px" TextMode="MultiLine" Width="450px" Columns="80" Rows="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
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
                        <asp:Button id="cmdCancel" runat="server" Text="Regresar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

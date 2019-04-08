<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPERSUASIVOLLAMADAS.aspx.vb" Inherits="coactivosyp.EditPERSUASIVOLLAMADAS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar PERSUASIVO - LLAMADAS
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
                $("#txtFecha").keypress(function(event) { event.preventDefault(); });
                $("#txtFecha").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                //
                
                $('#txtFecha').datepicker({
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
            <table id="tblEditPERSUASIVOLLAMADAS" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de llamada
                    </td>
                    <td>
                        <asp:TextBox id="txtFecha" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecha" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Id Llamada
                    </td>
                    <td>
                        <asp:TextBox id="txtIdLlamada" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Teléfono
                    </td>
                    <td>
                        <asp:TextBox id="txtNoTelefono" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nombre de quien atiende la llamada
                    </td>
                    <td>
                        <asp:TextBox id="txtNombre" runat="server" MaxLength="120" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Gestor
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cbogestor" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Info Solicitada
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtInfoSolicitada" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:TextBox id="txtInfoSolicitada" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Info Brindada
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtInfoBrindada" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:TextBox id="txtInfoBrindada" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
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
                        Observaciones
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtObservaciones" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:TextBox id="txtObservaciones" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
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

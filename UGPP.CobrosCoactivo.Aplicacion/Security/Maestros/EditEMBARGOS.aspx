<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditEMBARGOS.aspx.vb" Inherits="coactivosyp.EditEMBARGOS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar Medidas Caultelares (Embargos)
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                $('#cmdAddDetalle').button();
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
                $("#txtFecResolEm").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResolEm").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResolEm').datepicker({
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
            .style1
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                width: 244px;
            }
            .style2
            {
                width: 244px;
            }
            .style3
            {
                height: 85px;
            }
            .style4
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                width: 244px;
                height: 85px;
            }
        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditEMBARGOS" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" colspan="2" style=" height:25px;">
                        Captura de datos de la Resolución de Medida Cautelar
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
                    <td class="style1">
                        Nro. Resolución de embargo
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResolEm" runat="server" MaxLength="30" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Fecha Resolución
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResolEm" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Límite de la medida
                    </td>
                    <td>
                        <asp:TextBox id="txtLimiteEm" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Estado
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboEstadoEm" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        <span lang="es">Porcentaje de Embargo</span></td>
                    <td>
                        <asp:TextBox ID="txtPorcentajeEmbargo" runat="server" Width="143px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        <span lang="es">Tipo Resolucion</span></td>
                    <td>
                        <asp:DropDownList ID="CboResolucion" runat="server" Height="16px" Width="148px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;
                    </td>
                    <td class="style4">
                        <span lang="es">
                        <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
                        </span></td>
                    <td class="style3">
                        <asp:TextBox ID="txtObservaciones" runat="server" Width="302px" Height="78px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style2">
                        <asp:Button id="cmdAddDetalle" runat="server" Text="Gestionar detalle de embargo" cssClass="PCGButton"></asp:Button>
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


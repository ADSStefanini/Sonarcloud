<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditEXCEPCIONES.aspx.vb" Inherits="coactivosyp.EditEXCEPCIONES" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar Excepciones
        </title>        
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
           
                //Controles de solo lectura
                $(".SoloLectura").keypress(function(event) { event.preventDefault(); });
                $(".SoloLectura").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });             
                
                ///////////////////////////////
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
                //var NomEstadoProc = $("#txtNombreEstado").val();
                var EstadoProc = '<%  Response.Write(Request("pEstadoExpediente"))%>';
                //alert(EstadoProc);

                if (EstadoProc == '04' || EstadoProc == '07') {
                    $.datepicker.datepicker('disable');
                }
                

                $("#txtFecRad").keypress(function(event) { event.preventDefault(); });
                $("#txtFecRad").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRad').datepicker({
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

                $("#txtFecNotif").keypress(function(event) { event.preventDefault(); });
                $("#txtFecNotif").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotif').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                //////////////////
                //Calendarios del recurso de reposicion
                $("#txtFecRadRecurso").keypress(function(event) { event.preventDefault(); });
                $("#txtFecRadRecurso").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRadRecurso').datepicker({
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
                $("#txtFecResolRes").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResolRes").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResolRes').datepicker({
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
                
                $("#txtFecOfiCitaPers").keypress(function(event) { event.preventDefault(); });
                $("#txtFecOfiCitaPers").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCitaPers').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubEdic").keypress(function(event) { event.preventDefault(); });
                $("#txtFecPubEdic").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubEdic').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                ////////////////////////////////////////////
            });
        </script>
        <style type="text/css">
	        * { font-size:12px; font-family:Arial;}				 
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditEXCEPCIONES" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        Datos de la excepción
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Radicación
                    </td>
                    <td>
                        <asp:TextBox id="txtNroRad" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha radicación
                    </td>
                    <td>
                        <asp:TextBox id="txtFecRad" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" style="color:Navy">
                        No. Resolución que resuelve excepciones
                    </td>
                    <td>
                        <asp:TextBox id="txtNroResResuelve" runat="server" MaxLength="20" CssClass="SoloLectura"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" style="color:Navy">
                        Fecha resolución que resuelve excepciones
                    </td>
                    <td>
                        <asp:TextBox id="txtFecResResuelve" runat="server" CssClass="SoloLectura" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Oficio Notificación por correo
                    </td>
                    <td>
                        <asp:TextBox id="txtNroOfiNotCor" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
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
                        Fecha notificación
                    </td>
                    <td>
                        <asp:TextBox id="txtFecNotif" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        Recurso de Reposición
                    </td>                    
                </tr>
                <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            Fecha radicación recurso
		            </td>
		            <td>
			            <asp:TextBox id="txtFecRadRecurso" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header" style="color:Navy">
			            No. Resolución que resuelve Recurso de Reposición
		            </td>
		            <td>
			            <asp:TextBox id="txtNroResolRes" runat="server" CssClass="SoloLectura"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header" style="color:Navy">
			            Fecha resolución que resuelve Recurso de Reposición
		            </td>
		            <td>
			            <asp:TextBox id="txtFecResolRes" runat="server" CssClass="SoloLectura" MaxLength="10"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            No. Oficio Citación para notificación personal
		            </td>
		            <td>
			            <asp:TextBox id="txtNroOfiCitaPers" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            Fecha Oficio citación para notificación personal
		            </td>
		            <td>
			            <asp:TextBox id="txtFecOfiCitaPers" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            Fecha publicación edicto
		            </td>
		            <td>
			            <asp:TextBox id="txtFecPubEdic" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
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
                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
        <div style="margin-top:4px; width:660px; height:740px;">                                                            	
			<iframe src="PRUEBAS.aspx?pRadicado=<%  Response.Write(Request("ID"))%>&pExpediente=<%  Response.Write(Request("pExpediente"))%>" width="660" height="740" scrolling="no" frameborder="0"></iframe>								
		</div>
    </body>
</html>

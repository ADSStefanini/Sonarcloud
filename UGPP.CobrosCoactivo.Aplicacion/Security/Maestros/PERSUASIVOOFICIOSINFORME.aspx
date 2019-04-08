<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PERSUASIVOOFICIOSINFORME.aspx.vb" Inherits="coactivosyp.PERSUASIVOOFICIOSINFORME" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>PERSUASIVOOFICIOS</title>
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            EndRequestHandler();

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
            $("#txtSearchFechaInicial").keypress(function(event) { event.preventDefault(); });
            $("#txtSearchFechaInicial").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

            $("#txtSearchFechaFinal").keypress(function(event) { event.preventDefault(); });
            $("#txtSearchFechaFinal").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            //

            $('#txtSearchFechaInicial').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $('#txtSearchFechaFinal').datepicker({
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
        function EndRequestHandler() {
            $('#cmdExportar').button();
            $('#cmdSearch').button();
            $('#txtFromFecOfi').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both', 
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });
            $('#txtToFecOfi').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both', 
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });
            $('.GridEditButton').button();

            $(".PCG-Content tr:gt(0)").mouseover(function() {
                $(this).addClass("ui-state-highlight");
            });

            $(".PCG-Content tr:gt(0)").mouseout(function() {
                $(this).removeClass("ui-state-highlight");
            });
        }
    </script>
    <style type="text/css">			
		* { font-size:12px;}	
		.BoundFieldItemStyleHidden { display:none}	
		.BoundFieldHeaderStyleHidden { display:none}		 
	</style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table id="Table1" cellspacing="0" cellpadding="0" width="90%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td align="right">
                    <table style="width: 100%">
                        <tr>
                            <td class="ui-widget-header" colspan="5" align="left">Consulta de oficios enviados</td>                    
                        </tr>
                        <tr>
                            <td class="ui-widget-header">
                                Fecha inicial
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchFechaInicial" runat="server" Width="100px"></asp:TextBox>&nbsp;
                                <asp:ImageButton ID="imgBtnBorraFechaIni" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                            </td>
                            <td class="ui-widget-header">
                                Fecha final
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchFechaFinal" runat="server" Width="100px"></asp:TextBox>&nbsp;
                                <asp:ImageButton ID="imgBtnBorraFechaFin" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                            </td>
                            <td>
                                <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
								<asp:Button id="cmdExportar" runat="server" Text="Exportar" cssClass="PCGButton"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="NroExp" HeaderText="Nro Expediente" ></asp:BoundField>
                            <asp:BoundField DataField="NroOfi" HeaderText="Nro Oficio" ></asp:BoundField>
                            <asp:BoundField DataField="FecOfi" HeaderText="Fec Oficio" DataFormatString="{0:dd/MM/yyyy}" ></asp:BoundField>
                            <asp:BoundField DataField="FecEnvOfi" HeaderText="Fecha Envio Oficio" DataFormatString="{0:dd/MM/yyyy}" ></asp:BoundField>
                            <asp:BoundField DataField="NoGuiaEnt" HeaderText="No Guia Entrega" ></asp:BoundField>
                            <asp:BoundField DataField="FecAcuseO" HeaderText="Fecha Acuse Oficio" DataFormatString="{0:dd/MM/yyyy}" ></asp:BoundField>
                            <asp:BoundField DataField="direnvio" HeaderText="direnvio" ></asp:BoundField>
                            <asp:BoundField DataField="email" HeaderText="email" ></asp:BoundField>                                                    
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header"  />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle/>
                    </asp:GridView>   
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

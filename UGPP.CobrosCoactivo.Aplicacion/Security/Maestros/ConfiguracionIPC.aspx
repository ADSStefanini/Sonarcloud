<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfiguracionIPC.aspx.vb"
    Inherits="coactivosyp.Security.Maestros.ConfiguracionIPC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Configuracion IPC | Sanción 1607 </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            EndRequestHandler();
        });
        function EndRequestHandler() {
            $('#cmdAddNew').button();
            $('#cmdSearch').button();
            $('.GridEditButton').button();

            $("#tabs").tabs();
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

            $("#txtSearchdesde").keypress(function(event) { event.preventDefault(); });
            $("#txtSearchdesde").keydown(function(e) { if (e.keyCode == 46) { return false; }; });
            $('#txtSearchdesde').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });


        };   
    </script>

    <style type="text/css">
        *
        {
            font-size: 11px;
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
        }
        body
        {
            background-color: #01557C;
        }
        .style1
        {
            width: 168px;
        }
        .style2
        {
            width: 104px;
        }
        .style3
        {
            border: 1px solid #4297d7;
            background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
            color: #ffffff;
            font-weight: bold;
            width: 84px;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td colspan="9" background="images/resultados_busca.jpg" height="42">
                <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left">
                    <span style="font-weight: normal">Usuario Actual (
                        <asp:Label ID="lblNomPerfil" runat="server" Text="Label" />)</span></div>
            </td>
        </tr>
    </table>
    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">Configurar IPC</a></li>
            <li><a href="#tabs-2">Masivo Sanción 1607</a></li>
             <li><a href="#tabs-3">Masivo Liquidación/Sanción</a></li>
        </ul>
        <div id="tabs-1">
            <table>
                <tr>
                    <td align="left">
                        <table style="width: 100%">
                            <tr>
                                <td class="style3">
                                    Año
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtSearchanio" runat="server"></asp:TextBox>
                                </td>
                                <td class="style2">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Buscar" CssClass="PCGButton"></asp:Button>
                                </td>
                                <td>
                                    <asp:Button ID="cmdAddNew" runat="server" Text="Adicionar" CssClass="PCGButton">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content"
                            AllowPaging="True" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="ANIO" HeaderText="AÑO" SortExpression="IPC.ANIO">
                                    <ItemStyle HorizontalAlign="Center" Height="20px" Width="100px" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TASA" HeaderText="TASA" SortExpression="IPC.TASA">
                                    <ItemStyle HorizontalAlign="Center" Height="20px" Width="100px" />
                                </asp:BoundField>
                                <asp:ButtonField ButtonType="Button" Text="EDITAR" HeaderText="ACCIÓN">
                                    <ControlStyle CssClass="GridEditButton" />
                                </asp:ButtonField>
                            </Columns>
                            <HeaderStyle CssClass="ui-widget-header" />
                            <RowStyle CssClass="ui-widget-content" />
                            <AlternatingRowStyle />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-2">
            <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                <iframe src="capturarinteresesmulta.aspx" width="960" height="740" scrolling="no"
                    frameborder="0"></iframe>
            </div>
        </div>
         <div id="tabs-3">
            <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                <iframe src="GenerarIpcLiquidacionSancion.aspx" width="960" height="740" scrolling="no"
                    frameborder="0"></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

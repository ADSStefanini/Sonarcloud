<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LiquidacionInteresesDTF.aspx.vb"
    Inherits="coactivosyp.LiquidacionInteresesDTF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//ES" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Calcular intereses DTF</title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            //Botones de Guardar (efecto HOVER)
        $('#cmdCalcularInteres').button();
        $('#cmdImportarcsv').button();
        $('#cmdExportarExcel').button();

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

            $("#txtfechaPago").keypress(function(event) { event.preventDefault(); });
            $("#txtfechaPago").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtfechaPago').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                maxDate: "+10Y",
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

        });
    </script>

    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {

            document.getElementById('procesando_div').style.display = "";
            setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
        }
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-state-hover" style="text-align: center; font-size: medium;" colspan="4">
                <span lang="es">PANTALLA PARA EL CÁLCULO DE INTERESES DE DTF</span>
                <br />
                <span lang="es" style="font-size: 11px">Esta pantalla recive un archivo .csv delimitado
                    por coma con 2 columnas </span><span lang="es" style="font-size: 11px">1.FECHA_MESADA,
                        2.CAPITAL_ADEUDADO. La columnas deben llevar el mismo nombre descrito.</span>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:FileUpload ID="upload" runat="server" CssClass="PCGButton" />
            </td>
            <td>
                <asp:Button ID="cmdImportarcsv" runat="server" Text="Importar y procesar archivo">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header" style="width: 80px">
                Fecha pago :
            </td>
            <td>
                <asp:TextBox ID="txtfechaPago" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="cmdCalcularInteres" runat="server" Text="Calcula intereses" OnClientClick="mostrar_procesar();">
                </asp:Button>
            </td>
            <td>
                <asp:Button ID="cmdExportarExcel" runat="server" Text="Exportar archivo a Excel">
                </asp:Button>
            </td>
        </tr>
    </table>
    <br />
    <div style="overflow: scroll; width: 100%; height: 900px;">
        <asp:GridView ID="Gridinteres" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" PageSize="20" AllowPaging="True">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#EFF3FB" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    <span id="procesando_div" style="display: none; position: absolute; text-align: center;
        top: 270px; left: 43%;">
        <img src="../images/gif/ajax-loader.gif" id="procesando_gif" />
    </span>
    <div id="dialog-message" title="Alerta" style="text-align: left; font-size: 10px;
        display: none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;">
            </span>
            <%=ViewState("message")%>
        </p>
    </div>
    </form>
</body>
</html>

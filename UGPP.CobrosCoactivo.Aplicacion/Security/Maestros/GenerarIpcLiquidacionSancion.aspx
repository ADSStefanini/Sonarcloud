<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenerarIpcLiquidacionSancion.aspx.vb"
    Inherits="coactivosyp.GenerarIpcLiquidacionSancion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calcular intereses Multa</title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
            //Botones de Guardar (efecto HOVER)
            $('#cmdCalcularInteres').button();
            $('#cmdDescargar').button();

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

            $("#txtFecPago").keypress(function(event) { event.preventDefault(); });
            $("#txtFecPago").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecPago').datepicker({
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

            $("#txtFecDeuda").keypress(function(event) { event.preventDefault(); });
            $("#txtFecDeuda").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecDeuda').datepicker({
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

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            //agregar una nueva columna con todo el texto 
            //contenido en las columnas de la grilla 
            // contains de Jquery es CaseSentive, por eso a minúscula
            $(".filtrar tr:has(td)").each(function() {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#txtbuscar").keyup(function() {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function() {
                    $(".filtrar tr:visible .indexColumn:not(:contains('"
                     + this + "'))").parent().hide();
                });
            });
        });
    </script>

    <style type="text/css">
        *
        {
            font-size: 12px;
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
    <table id="Table1" cellspacing="0" cellpadding="0" width="78%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td class="ui-state-hover" style="text-align: center; font-size: medium;" colspan="2">
                <span lang="es">PANTALLA PARA EL CÁLCULO DE IPC DE LIQUIDACION/SANCION </span>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:Button ID="cmdCalcularInteres" runat="server" Text="Calcular IPC Masivo" CssClass="PCGButton">
                </asp:Button>
                <asp:Button ID="cmdDescargar" runat="server" Text="Descargar " CssClass="PCGButton">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
        <td>
        &nbsp; </td>
        <td>
            Buscar :
            <input id="txtbuscar" type="text" />
            <asp:CheckBox ID="chkMarcar" runat="server" Text="Marcar - Desmarcar Todo" AutoPostBack="true" />
        </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
            
               &nbsp;
            </td>
           
        </tr>
        
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <div style="overflow-y: scroll; height: 300px; width: 735px">
                    <asp:GridView ID="grdExpe" Width="715px" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content filtrar">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="NIT / C.C"></asp:BoundField>
                            <asp:BoundField DataField="deudor" HeaderText="DEUDOR"></asp:BoundField>
                            <asp:BoundField DataField="EFINROEXP" HeaderText="EXPEDIENTE"></asp:BoundField>
                            <asp:BoundField DataField="EFIVALDEU" HeaderText="DEUDA" ItemStyle-CssClass="hidden"
                                HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:TemplateField HeaderText="ACCIÓN">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSeleccion" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header" />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

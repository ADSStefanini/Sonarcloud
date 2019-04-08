<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditConfiguracionDTF.aspx.vb"
    Inherits="coactivosyp.Security.Maestros.EditConfiguracionDTF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar configuracion DTF </title>

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

    <script type="text/javascript">
        $(function() {
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

            $("#txtdesde").keypress(function(event) { event.preventDefault(); });
            $("#txtdesde").keydown(function(e) { if (e.keyCode == 46) { return false; }; });
            $('#txtdesde').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txthasta").keypress(function(event) { event.preventDefault(); });
            $("#txthasta").keydown(function(e) { if (e.keyCode == 46) { return false; }; });
            $('#txthasta').datepicker({
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
    <form id="Form1" method="post" runat="server">
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
        <tr>
            <td colspan="9" background="images/resultados_busca.jpg" height="42">
                <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left">
                    <span style="font-weight: normal">Usuario Actual (
                        <asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                <div style="color: White; width: 150px; height: 20px; float: right; text-align: right">
                    <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                    <span id="spancerrarsesion" runat="server">Cerrar sesión&nbsp&nbsp</span>
                </div>
                <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
                    padding-right: 0px;">
                    <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" />
                    </asp:LinkButton>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
    </table>
   
        <div id="tabs-1">
        
         <table class="ui-widget-content ui-widget">
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                AÑO
            </td>
            <td>
                <asp:TextBox ID="txtperiodo" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="ui-widget-header">
                TASA
            </td>
            <td>
                <asp:TextBox ID="txtdtf" runat="server" CssClass="ui-widget"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="cmdCancel" runat="server" Text="Cancelar" CssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="cmdSave" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
            </td>
        </tr>
    </table>
        </div>
   
   
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="capturarinteresesmulta.aspx.vb" Inherits="coactivosyp.capturarinteresesmulta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Calcular intereses Multa</title>
        
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    
    <script type="text/javascript">
            $(function() {
                //Botones de Guardar (efecto HOVER)
                $('#cmdCalcularInteres').button();

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
    <style type="text/css">
		    * { font-size:12px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
		    body{ background-color:#01557C}
        .style1
        {
            border: 1px solid #4297d7;
            background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
            color: #ffffff;
            font-weight: bold;
            width: 125px;
        }
        .style2
        {
            height: 42px;
        }
        .style3
        {
            border: 1px solid #4297d7;
            background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
            color: #ffffff;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
           <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                <td colspan="3" background="images/resultados_busca.jpg" class="style2">
                    <div style="color:White; font-weight:bold; width:450px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                    <div style="color:White; width:150px; height:20px; float:right; text-align:right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span id="spancerrarsesion" runat="server">Cerrar sesión&nbsp&nbsp</span>
                    </div>
                    
                    <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" />
                        </asp:LinkButton>
                    </div>
                    
                </td>
            </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                                    
            <td class="ui-state-hover" 
                            style="text-align: center; font-size: medium;" colspan="2">
                        <span lang="es">PANTALLA EL CÁLCULO DE INTERESES DE MULTAS.</span></td>
                
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Valor deuda o capital
                    </td>
                    <td>
                        <asp:TextBox id="txtDeudaCap" runat="server" CssClass="ui-widget" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Fecha de Exigibilidad
                    </td>
                    <td>
                        <asp:TextBox id="txtFecDeuda" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="style1">
                        Fecha estimada de pago
                    </td>
                    <td>
                        <asp:TextBox id="txtFecPago" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        Dias de Mora</td>
                    <td>
                        <asp:TextBox id="txtDiasdeMora" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        Tasa Interes Anual</td>
                    <td>
                        <asp:TextBox id="txtTasaAnualMulta" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        Tasa Interes Mensual</td>
                    <td>
                        <asp:TextBox id="txtTasaMensualMulta" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        Intereses de Mora</td>
                    <td>
                        <asp:TextBox id="txtInetesesMoraMulta" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        Deuda + Intereses</td>
                    <td>
                        <asp:TextBox id="txtDeudaMasIntereses" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:Button id="cmdCalcularInteres" runat="server" Text="Calcula intereses" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
          </table>
    </form>
</body>
</html>

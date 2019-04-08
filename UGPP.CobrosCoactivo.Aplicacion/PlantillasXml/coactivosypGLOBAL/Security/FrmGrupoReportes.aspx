<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FrmGrupoReportes.aspx.vb"
    Inherits="coactivosyp.FrmGrupoReportes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reportes</title>
    
    <script type="text/javascript" src="Maestros/js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Maestros/js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="Maestros/css/redmond/jquery-ui-1.10.4.custom.css" />
    
    
    <script type="text/jscript">
     
        $('#<%=DdlReporte.ClientID %>').change(function(e) {
            $('#Fechas').hide();
            $('#Gestor').hide();
            alert($(this).val())
            switch ($(this).val()) {
                case "001":
                    $('#Fechas').show();
                    break;
           
                case "002":
                    $('#Fechas').show();
                    break;
                case "003":
                    $('#Fechas').show();
                    break;
                case "004":
                $('#Fechas').show();
                break;
                
                case "005":
                $('#Fechas').show();
                break;
                
                case "006":
                    $('#Gestor').show();
                    break;
                case "007":
                    $('#Fechas').show();
                    break;
                case "008":
                    $('#Fechas').show();
                    break;
                case "009":
                    $('#Fechas').show();
                    break;
                case "010":
                    $('#Fechas').show();
                    break;
                    
                case "011":
                    $('#Fechas').show();
                    break;
                case "012":
                    $('#Fechas').show();
                    break;
                case "013":
                    $('#Fechas').show();
                    break;
                case "014":
                
                    $('#Fechas').show();
                    break;
                case "015":
                    $('#Fechas').show();
                    break;
                case "016":
                    $('#Fechas').show();
                    break;
                case "017":
                    $('#Fechas').show();
                    break;
                case "018":
                    $('#Fechas').show();
                    break;
                case "019":
                    $('#Fechas').show();
                    break;
                case "020":
                    $('#Fechas').show();
                    $('#Gestor').show();
                    break;
                case "021":
                    $('#Fechas').show();
                    break;
                case "022":
                    $('#Fechas').show();
                case "023":
                    $('#Fechas').show();

                    break;
                case "024":
                    $('#Fechas').show();
                case "025":
                    $('#Fechas').show();
                case "026":
                    $('#Fechas').show();
            };
        });
    </script>

    <script type="text/jscript">
        $(function() {
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
            $("#dtpFechI").keypress(function(event) { event.preventDefault(); });
            $("#dtpFechI").keydown(function(e) { if ( e.keyCode == 46) { return false; }; });
            $('#dtpFechI').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'Maestros/calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                changeMonth: true,
                maxDate: "+1000M, -2D",
                 beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
               });

            $("#dtpFechF").keypress(function(event) { event.preventDefault(); });
            $("#dtpFechF").keydown(function(e) { if ( e.keyCode == 46) { return false; }; });
            $('#dtpFechF').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'Maestros/calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                changeMonth: true,
                maxDate: "+1000M, -2D",
                beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
            });
        });      
    </script>
    
    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {
            document.getElementById('procesando_div').style.display = "";
            $("#dialog-modal").dialog({
                height: 150,
                modal: true
            });
            setTimeout(' document.getElementById("procesando_gif").src="images/gif/ajax-loader.gif"', 100000);
        }

      
    </script>

    <style type="text/css">
        .style1
        {
            width: 83px;
        }
        .style2
        {
            width: 206px;
        }
        *
        {
            font-size: 12px;
            font-family: Arial;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
            <tr>
                <td colspan="10" background="maestros/images/resultados_busca.jpg" height="42">
                    <div style="color: White; font-weight: bold; width: 450px; height: 20px; float: left">
                        <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
                        <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                            Text="Label"></asp:Label>)</span></div>
                    <div style="color: White; width: 150px; height: 20px; float: right; text-align: right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span>Cerrar sesión&nbsp&nbsp</span>
                    </div>
                    <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
                        padding-right: 0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" /></asp:LinkButton>
                    </div>
                </td>
            </tr>
            </table>
             <table align="center" border="0">
                    <tr>
                        <td class="style1">
                            <asp:Label ID="Label2" runat="server" Text="Reporte : "></asp:Label>
                        </td>
                        <td align="center" class="style2">
                            <asp:DropDownList ID="DdlReporte" runat="server" Style="margin-left: 0px;" Width="411px" AutoPostBack="true">
                                <asp:ListItem Text="Seleccione un Reporte" Value="0" />
                            </asp:DropDownList>
                        </td>
                        <td class="style1">
                            <asp:Button ID="BtnGenerar" Text="Generar" runat="server" OnClientClick="mostrar_procesar();" />
                        </td>
                        <td class="style1">
                            <asp:Button ID="BtnExportar" Text="Exportar" runat="server" />
                        </td>
                        <td class="style1">
                            <input id="BtnCancelar" type="button" value="Cancelar" runat="server" />
                        </td>
                        <td class="style1">
                            <asp:Button  id="btnGuardar" Text="Guardar" runat="server" OnClientClick="mostrar_procesar();"/>
                        </td>
                    </tr>
                </table>
           
        <div id="Fechas" style="text-align: center; display: none" runat="server">
            Fecha Inicial:<input type="text" id="dtpFechI" runat="server" />
            Fecha Final :<input type="text" id="dtpFechF" runat="server" />
        </div>
        <div id="Gestor" style="text-align: center; display: none" runat="server">
            <asp:DropDownList ID="DdlUsuario" runat="server" Style="margin-left: 0px;" Width="411px">
                <asp:ListItem Text="Seleccione un Funcionario" Value="0" />
            </asp:DropDownList>
        </div>
        <div style="width: 100%; text-align: center">
            <asp:Label ID="lblError" runat="server"></asp:Label></div>
    </div>

    <script type="text/jscript">
        $('#BtnGenerar').button();
        $('#BtnExportar').button();
        $('#BtnCancelar').button();
        $('#btnGuardar').button();
        
        $('#<%=DdlReporte.ClientID %>').change(function(e) {
            $('#Fechas').hide();
            $('#Gestor').hide();
            switch ($(this).val()) {
                case "001":
                    $('#Fechas').show();
                    break;
            
                case "002":
                    $('#Fechas').show();
                    break;
                case "003":
                    $('#Fechas').show();
                    break;
                case "004":
                    $('#Fechas').show();
                    break;
                case "005":
                    $('#Fechas').show();
                    break;
            
                case "006":
                    $('#Gestor').show();
                    break;
                case "007":
                    $('#Fechas').show();
                    break;
                case "008":
                    $('#Fechas').show();
                    break;
                case "009":
                    $('#Fechas').show();
                    break;
                case "010":
                    $('#Fechas').show();
                    break;
            
            
                case "011":
                    $('#Fechas').show();
                    break;
                case "012":
                    $('#Fechas').show();
                    break;
                case "013":
                    $('#Fechas').show();
                    break;
              
                case "014":
                    $('#Fechas').show();
                    break;
                case "015":
                    $('#Fechas').show();
                    break;
                case "016":
                    $('#Fechas').show();
                    break;
                case "017":
                    $('#Fechas').show();
                    break;
            
                case "018":
                    $('#Fechas').show();
                    break;
                case "019":
                    $('#Fechas').show();
                    break;
                case "020":
                    $('#Fechas').show();
                    $('#Gestor').show();
                    break;
                case "021":
                    $('#Fechas').show();
                    break;
                case "022":
                    $('#Fechas').show();
                case "023":
                    $('#Fechas').show();
                    break;
                case "024":
                    $('#Fechas').show();
                    break;
                case "025":
                    $('#Fechas').show();
                    break;
                case "026":
                    $('#Fechas').show();
                    break;
            };
        });
    </script>
    <asp:ScriptManager ID="ScriptManagerCiudades" runat="server"> 
    </asp:ScriptManager>
    <asp:UpdatePanel ID="PU_Grid_View" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div style="overflow:scroll ">
                <table align="center" border="0" style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="DgvReporte" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" AllowPaging="True" PageSize="15">
                                <RowStyle BackColor="#EFF3FB" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DdlReporte" EventName="SelectedIndexChanged"/>
        </Triggers>
    </asp:UpdatePanel>
    </form>
        <div id="dialog-modal" title="Procesando..." style="text-align:center">   
            <span id= "procesando_div" style="display:none;" >
            <img src="images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
            </span>
    </div>    
</body>
</html>

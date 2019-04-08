<%@ Page  Language="vb" AutoEventWireup="false" CodeBehind="capturarintereses2.aspx.vb" Inherits="coactivosyp.capturarintereses2"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//ES" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Calcular intereses Parafiscales</title>
        
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

            });   
    </script>
    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {

            document.getElementById('procesando_div').style.display = "";
            setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);

        }
</script>
    
    
    <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
		    body{ background-color:#01557C}
        </style>
    
   </head>
<body>
    <form id="form1" runat="server">
               <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                
                                    
                <tr>
                    <td>
                        &nbsp;</td>
                    
            <td class="ui-state-hover" 
                            style="text-align: center; font-size: medium;" colspan="8">
                        <span lang="es">PANTALLA PARA EL CÁLCULO DE INTERESES (DESCARGAR LA PLANTILLA EN LA VENTANA DE IMPORTACION Y EXPORTACION DE SQL)</span></td>
            </tr>
                
                    
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td >
                        <asp:FileUpload ID="upload" runat="server" cssClass="PCGButton"  />
                    </td>
                    <td >
                        <asp:Button id="cmdImportarcsv" runat="server" Text="Importar y procesar archivo" cssClass="PCGButton"></asp:Button>                    
                    </td>
                    <td class="ui-widget-header">
                        Salud capital
                    </td>
                                        <td >
                        <asp:TextBox ID="txtSaludCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Salud intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtSaludInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total Salud 
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalSalud" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    
                <tr>
                    <td>
                    </td>
                    <td class="ui-widget-header">
                        Calidad de aporte
                    </td>
                    <td>
                        <asp:DropDownList ID="cboCAPORTE" runat="server" Height="20px" Style="margin-left: 0px; "
                                                Width="190px" AutoPostBack="true">
                            <asp:ListItem Text="Selecciones..." Value="0" />
                            <asp:ListItem Text="TRABAJADORES INDEPENDIENTES" Value="1" />
                            <asp:ListItem Text="EMPRESAS CON MAS DE 200 TRABAJADORES" Value="2" />
                            <asp:ListItem Text="EMPRESAS CON MENOS DE 200 TRABAJADORES" Value="3" />                            
                        </asp:DropDownList> 
                      </td>
                        <td class="ui-widget-header">
                        Pensión capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtPensionCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Pensión intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtPensionInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total Pensión 
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalPension" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td>
                    </td>
                    <td class="ui-widget-header">
                    Día de pago maximo aportes
                    </td>
                    <td>
                        <asp:Label ID="lbldiaPago" runat="server" Text="0"></asp:Label>
                    </td>
                    
                    <td class="ui-widget-header">
                        ARL capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtArlCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        ARL intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtArlInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total ARL
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalARL" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    
                    </tr>
                    <tr>
                    <td>
                    </td>
                    <td >
                    <asp:Button id="cmdCalcularInteres" runat="server" Text="Calcula intereses" OnClientClick="mostrar_procesar();" cssClass="PCGButton"></asp:Button>
                    </td>
                    <td >
                    <asp:Button id="cmdExportarExcel" runat="server" Text="Exportar archivo a Excel" cssClass="PCGButton"></asp:Button>
                    </td>
                    
                    <td class="ui-widget-header">
                        Sena capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtSenaCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Sena intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtSenaInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total Sena 
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalSena" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    
                    <tr>
                    <td>
                        &nbsp;</td>
                    <td colspan="2" rowspan="3" >
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                        </td>
                        
                        <td class="ui-widget-header">
                        ICBF capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtIcbfCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        ICBF intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtIcbfInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total ICBF 
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalICBF" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    
                    <tr>
                    <td>
                        &nbsp;</td>
                        
                        <td class="ui-widget-header">
                        CCF capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtCcfCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        CCF intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtCcfInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total CCF
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalCCF" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    
                    <tr>
                    <td>
                        &nbsp;</td>
                        
                         <td class="ui-widget-header">
                        FSP capital
                    </td>
                    <td >
                        <asp:TextBox ID="txtFspCapital" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        FSP intereses
                    </td>
                    <td >
                        <asp:TextBox ID="txtFspInteres" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total FSP
                    </td>
                    <td >
                        <asp:TextBox ID="txtTotalFSP" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    </tr>
                    
                    </table>
               
               
                    <div style=" overflow: scroll;width: 100%;height: 900px;">
                    
               
                 
                <asp:GridView ID="Gridinteres" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" PageSize="50" AllowPaging="True" >
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                
        
          </div>
          <span id= "procesando_div" 
               style="display:none ; position:absolute; text-align:center; top: 270px; left: 43%;">
            <img src="../images/gif/ajax-loader.gif" id="procesando_gif"  />

</span>

    <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
    </div>
  
  
    </form>
</body>
</html>

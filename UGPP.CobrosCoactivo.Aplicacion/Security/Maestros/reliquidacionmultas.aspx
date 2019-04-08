<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="reliquidacionmultas.aspx.vb" Inherits="coactivosyp.reliquidacionmultas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Reliquidacion de Intereses</title>
        
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <script type="text/javascript">
            $(function() {
                //Botones de Guardar (efecto HOVER)
           $('#cmdImportarcsv').button();
           $('#cmdExportarExcel').button();
           $('#upload').button();
            
            
            

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
    
    <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
        </style>
    
    <link href="../css/Objetos.css" rel="stylesheet" type="text/css" />
   </head>
<body>
    <form id="form1" runat="server">
           <table id="Table1" class="ui-widget-content tabla" border="0"  cellspacing="0"
               style="height: 89px; width: 800px; "   > 
                <tr >
                    <td rowspan="2">
                        &nbsp;
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                    </td>
                    </tr>
                    
                <tr>
                    <td >
                        <asp:FileUpload ID="upload" runat="server" cssClass="PCGButton"  />
                    </td>
                    <td >
                        <asp:Button id="cmdImportarcsv" runat="server" Text="Importar y procesar archivo" cssClass="PCGButton"></asp:Button>                    
                    </td>
                    <td>
                        <asp:Button id="cmdExportarExcel" runat="server" 
                            Text="Exportar Registros a Excel." cssClass="PCGButton"></asp:Button>                    
                    </td>
                </tr>
                                    
                <tr>
                    <td colspan="4">
                 
                <asp:GridView ID="Gridinteres" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" PageSize="15" >
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                    </td>
                </tr>
                                    
                    </table>
               
               
    </form>
</body>
</html>

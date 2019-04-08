<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfiguracionInteresesParafiscales.aspx.vb" Inherits="coactivosyp.Security.Maestros.ConfiguracionInteresesParafiscales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head>
        <title>CALCULO_INTERESES_PARAFISCALES</title>
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
                $("#txtSearchdesde").keydown(function(e) { if ( e.keyCode == 46) { return false; }; });
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
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
		    body{ background-color:#01557C}
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
                    <div style="color:White; font-weight:bold; width:450px; height:20px; float:left"><span style="font-weight:normal">Usuario Actual ( <asp:Label ID="lblNomPerfil" runat="server" Text="Label" />)</span></div>
                    
                </td>
            </tr>
                <tr>
                    <td align="left">
                                            <table style="width: 100%">
                                                <tr>
                            <td class="style3">
                            Fecha inicial</td>
                            <td class="style1" >
                            <asp:TextBox ID="txtSearchdesde" runat="server" ></asp:TextBox>
                            </td>
                            <td class="style2">
                            <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                            </td>
                            <td>
                            <asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
                            </td>                            
                                                    
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" 
                                                CssClass="PCG-Content" AllowPaging="True" AllowSorting="True">
                                                <Columns>
                                                    <asp:BoundField DataField="consec" >
                                                        <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                        <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="p_trimestral" HeaderText="TASA TRIMESTRAL" 
                                                        SortExpression="CALCULO_INTERESES_PARAFISCALES.p_trimestral"></asp:BoundField>
                                                    <asp:BoundField DataField="desde" HeaderText="FECHA INICIAL" 
                                                        SortExpression="CALCULO_INTERESES_PARAFISCALES.desde" 
                                                        DataFormatString="{0:d}"></asp:BoundField>
                                                    <asp:BoundField DataField="hasta" HeaderText="FECHA FINAL" 
                                                        SortExpression="CALCULO_INTERESES_PARAFISCALES.hasta" 
                                                        DataFormatString="{0:d}"></asp:BoundField>
                                                    <asp:BoundField DataField="t_diaria" HeaderText="TASA DIARIA" 
                                                        SortExpression="CALCULO_INTERESES_PARAFISCALES.t_diaria"></asp:BoundField>
                                                    <asp:ButtonField ButtonType="Button" Text="EDITAR">
                                                        <ControlStyle CssClass="GridEditButton" />
                                                    </asp:ButtonField>
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

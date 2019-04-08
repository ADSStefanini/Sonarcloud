<%@ Page  Language="vb" AutoEventWireup="false" CodeBehind="subirSQL.aspx.vb" Inherits="coactivosyp.subirSQL"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//ES" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Subir SQL</title>
        
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <script type="text/javascript">
        $(function() {
        $(".numeros").keydown(function(event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
            // Allow: Ctrl+A
                        (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
                        (event.keyCode >= 35 && event.keyCode <= 39)) {
                if (this.value == '') { this.value = 0; } // 26/ene/2014: Si deja la entrada en blanco=>poner cero
                // let it happen, don't do anything
                return;
            } else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });
        
                //Botones de Guardar (efecto HOVER)
            $('#btnBuscar').button();
            $('#cmdImportarcsv').button();
            $('#cmdExportarExcel').button();
            $('#cmdVerSql').button();
            
            
            
            

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
		    .numeros { text-align:right; }
		    body{ background-color:#01557C}
        .style1
        {
            width: 203px;
        }
        .style3
        {
            overflow: scroll;
            width: 100%;
            height: 900px;
        }
        </style>
    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {
            document.getElementById('procesando_div').style.display = "";
            $("#dialog-modal").dialog({
                height: 150,
                modal: true
            });
            setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
        }
    </script>    
   </head>
<body>
    <form id="form1" runat="server">
           <span lang="es"></span><table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                <td colspan="4" background="images/resultados_busca.jpg" height="42">
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
            <td class="ui-state-hover" 
                            style="text-align: center; font-size: medium;" colspan="3">
                        <span lang="es">PANTALLA PARA LA IMPORTACION Y EXPORTACION DE LOS SQL.</span></td>
            </tr>
                <tr >
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" 
                            Font-Size="12px" ForeColor="Blue"></asp:Label>
                    </td>
                    </tr>
                    
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td >
                        <asp:FileUpload ID="upload" runat="server" cssClass="PCGButton"  />
                        <asp:Button id="cmdImportarcsv" runat="server" Text="Importar y guardar archivo" OnClientClick="mostrar_procesar();" ></asp:Button>                    
                    <asp:Button id="cmdExportarExcel" runat="server" Text="Exportar archivo a Excel"  ></asp:Button>
                    <asp:Button id="cmdVerSql" runat="server" Text="Ver SQL almacenados" OnClientClick="mostrar_procesar();" ></asp:Button>
                    </td>
                    <td class="style1" >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                                    
                <tr>
                    <td>
                        &nbsp;</td>
                    <td >
                        <asp:TextBox ID="txtExpediente" runat="server" CssClass="ui-widget numeros"></asp:TextBox>
                        <asp:Button id="btnBuscar" runat="server" Text="Buscar SQL" 
                            cssClass="PCGButton" OnClientClick="mostrar_procesar();"></asp:Button>                    
                        <asp:RadioButton ID="rbsqlinteres" runat="server" 
                            Text="Formato para cálculo de intereses." GroupName="sql" />
                        <asp:RadioButton ID="rbVerificacionpago" runat="server" 
                            Text="Formato para verificación de pago" GroupName="sql" />
                        <span lang="es">&nbsp;<asp:RadioButton ID="rbsql" runat="server" 
                            Text="Formato para subir SQL" GroupName="sql" />
                        </span>                    
                    </td>
                    <td class="style1" >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                                    
                    </table>
               
               
                    <div class="style3">
                    <table >
                <tr>
                <td>
                </td>
                <td colspan="2" > 
                 
                <asp:GridView ID="Gridinteres" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" PageSize="15" AllowPaging="True"   >
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
          </div>
           <div id="dialog-modal" title="Procesando..." style="text-align:center">   
                    <span id= "procesando_div" style="display:none;" >
                        <img src="../images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
                    </span>
        </div>
    </form>
</body>
</html>

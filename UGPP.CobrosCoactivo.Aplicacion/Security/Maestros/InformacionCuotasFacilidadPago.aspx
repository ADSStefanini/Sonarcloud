<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InformacionCuotasFacilidadPago.aspx.vb" Inherits="coactivosyp.InformacionCuotasFacilidadPago"  %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//ES" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Proyectar intereses Parafiscales</title>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
   
    <script type="text/javascript">
            $(function() {
                //Aceptar solo numeros
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
            $('#btnLoad').button();
            
            
            
            $("#accordion").accordion({ heightStyle: "content" });
            
            
            
            

               
                
            });

         
    </script>
    
    <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
        .numeros { text-align:right; }
        
        </style>

<script type="text/javascript" language="javascript">
    function mostrar_procesar() {
        
                document.getElementById('procesando_div').style.display = "";
                setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
               
                     }
</script>
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
   </head>
<body>
     
    
    <form id="form1" runat="server">
     
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>        
     
                <div><asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="12px" ForeColor="Blue"></asp:Label></div>
       <h3 class="ui-state-hover" 
                            style="text-align: center; font-size: medium;">
                        &nbsp;INFORMACIÓN DE LA CUOTAS DE LA FACILIDAD DE PAGO PROCESO NRO. <% =Val(Request("pExpediente"))%> </h3>
                        
                <div >
                    <table cellspacing="4px" >
                    
                    <tr> 
                    <td  colspan="6" >
                        <asp:Button ID="btnLoad" runat="server" Text="Actualizar Información" />
                        </td>
                        </tr>
                        <tr>
                      <td colspan="2">
                      
                 
                 <asp:GridView ID="DtgAcuerdos" runat="server"  CssClass="CSSTableGenerator"
              AutoGenerateColumns="False"  width = "100%"
                                
              PageSize="1">
                                <Columns>
                                    <asp:BoundField DataField="ESTADO_PAGO" HeaderText="Nro. Cuota" >
                                    
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="CUOTA_NUMERO" HeaderText="Nro. Cuota" >
                                    
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="FECHA_CUOTA" HeaderText="Fecha de Cuota" 
                                        DataFormatString="{0:dd/MM/yyyy}">
                                        </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_PAGO" HeaderText="Fecha de Pago" 
                                        DataFormatString="{0:dd/MM/yyyy}">
                                        
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VALOR_CUOTA" HeaderText="Valor Cuota" HeaderStyle-HorizontalAlign="Center" 
                                        DataFormatString="{0:N}">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VALOR_PAGADO" HeaderText="Valor Pagado" HeaderStyle-HorizontalAlign="Center" 
                                        DataFormatString="{0:N}">
                                    </asp:BoundField>
                                    
                                </Columns>
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                
                                
                            </asp:GridView>
                      
                          
                      </td>
                  </tr>
                  <tr>
                      <td></td>
                  </tr>
                  <tr>
                      
                      <td class="ui-widget-header" >
                        Valor total cuotas : </td>
                    <td  >
                        <asp:TextBox ID="Txttotalcuotas" runat="server" CssClass="ui-widget"  Width="150px"
                            ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                           <td class="ui-widget-header" >
                        Valor total pagos : </td>
                    <td >
                        <asp:TextBox ID="txttotalpagos" runat="server" CssClass="ui-widget"  Width="150px"
                            ReadOnly="True"></asp:TextBox>
                        </td>
                     </tr>
                    
                       <tr>
                           <td class="ui-widget-header" >
                        Valor por pagar : </td>
                    <td  >
                        <asp:TextBox ID="txtvalorporpagar" runat="server" CssClass="ui-widget"  Width="150px"
                            ReadOnly="True"></asp:TextBox>
                        </td>
                     </tr>
                     
                    
                    
                    </table>
               </div>
         
               
          <span id= "procesando_div" 
               style="display:none; position:absolute; text-align:center; top: 270px; left: 420px;">
            <img src="../images/gif/ajax-loader.gif" id="procesando_gif"  />

</span>
       </form>
</body>
</html>

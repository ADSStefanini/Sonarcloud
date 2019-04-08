<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Datos_Facilidad_Pago.aspx.vb" Inherits="coactivosyp.Datos_Facilidad_Pago"  %>
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
        
        .style1
        {
            background-color: #5C9CCC;
            background-repeat: repeat-x;
            background-position: 50% 50%;
        }
        
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
                        &nbsp;INFORMACIÓN DE LA FACILIDAD DE PAGO PROCESO NRO. <% =Val(Request("pExpediente"))%> </h3>
                        
                <div >
                    <div>
                        <asp:Button ID="btnLoad" runat="server" Text="Actualizar Información" />
                    </div>
                        
                    <table  rules="cols" cellspacing="2px" >
                    
                        <tr>
                      <td rowspan="19">
                      
                 <div style="overflow-y: scroll;height: 560px; font-size:12px"  >
                 
                 <asp:GridView ID="DtgAcuerdos" runat="server"  CssClass="CSSTableGenerator"
              AutoGenerateColumns="False"  width = "450px"
                                
              PageSize="1">
                                <Columns>
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
                                    
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    
                                    </asp:BoundField>
                                    
                                </Columns>
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                                
                                
                            </asp:GridView>
                      </div>      
                                </td>
                         </tr>     
                        <tr>
                    <td class="ui-widget-header" >
                        Nro de acuerdo : </td>
                    <td colspan="5" >
                        <asp:TextBox ID="TxtNumAcuerdo" runat="server" CssClass="ui-widget"  Width="150px"
                            ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                    <td class="ui-widget-header" >
                        Nro. de expdiente :</td>
                        
                    <td colspan="5">
                        <asp:TextBox ID="TxtNroProceso" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                            
                        </td>
                        </tr>
                        
                    <tr>
                    <td  class="ui-widget-header" ><span lang="es">Fecha de la solicitud:</span></td>
                      <td  colspan="5">
                
                        <asp:TextBox ID="TxtFechaSolisitud" runat="server" CssClass="ui-widget" Width="150px" ReadOnly="True"></asp:TextBox>
                        </td>
                      </tr>  
                          
                    <tr>
                    <td  class="ui-widget-header" ><span lang="es">Nro. resolución :</span></td>
                      <td colspan="5">
                
                        <asp:TextBox ID="TxtNroresolucion" runat="server" CssClass="ui-widget" Width="150px" 
                            ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                      <td class="ui-widget-header">
                
                          <span lang="es">Fecha resolución :</span></td>
                      <td colspan="5">
                
                        <asp:TextBox ID="Txtfecharesolucion" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                        </td>
                        </tr>
                      <tr>
                          <td class="ui-widget-header">
                              <span lang="es">Fecha notificación :</span>
                          </td>
                          <td colspan="5">
                            <asp:TextBox ID="Txtfechanotificacion" runat="server" 
                            CssClass="ui-widget" Width="150px" ReadOnly="True"></asp:TextBox>
                          </td>
                      </tr>  
                          
                    <tr>
                    <td  class="ui-widget-header" >Solicitante :</td>
                      <td  colspan="5">
                
                <asp:TextBox ID="Txt_solicitante" runat="server"  Width="80px" 
                              CssClass="ui-widget" ReadOnly="True" ></asp:TextBox>
                
                      <asp:TextBox ID="TxtNom_Solicitante" 
                          runat="server"  Width="400px" CssClass="ui-widget" ReadOnly="True"></asp:TextBox></td>
                      </tr>  
                          
                                <tr>
                      <td class="ui-widget-header" >
                      
                          Tipo Solicitante:</td>
                      <td colspan="5" >
                          <asp:DropDownList ID="CmbSolicitante" runat="server" 
                              Width="150px" CssClass="ui-widget" Enabled="False">
                          <asp:ListItem Selected="True" Value="01">Aportante</asp:ListItem>
								<asp:ListItem Value="02">Tercero</asp:ListItem>
								<asp:ListItem Value="03">Apoderado</asp:ListItem>
								<asp:ListItem Value="04">Representante Legal</asp:ListItem>
						  </asp:DropDownList>
                      </td>
                      </tr>
                      
                  
                  <tr>
                  <td  class="ui-widget-header" >Garante:</td>
            <td colspan="5"  >
                <asp:TextBox ID="TxtGarante" runat="server"  Width="80px" 
                    CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                
                <asp:TextBox ID="TxtNom_garante" runat="server" 
                    Width="400px" CssClass="ui-widget" ReadOnly="True" ></asp:TextBox>
                
                </td>
                </tr>
                <tr>
                      <td class="ui-widget-header" >
                          Tipo de Garante:</td>
                      <td colspan="5" >
                          <asp:DropDownList ID="CmbGarante" runat="server" Height="20px" Width="150px" 
                              CssClass="ui-widget" Enabled="False">
                                <asp:ListItem Selected="True" Value="00">Seleccione...</asp:ListItem>
                                <asp:ListItem Value="01">Compañía de Seguros</asp:ListItem>
								<asp:ListItem Value="02">Fifeicomiso</asp:ListItem>
								<asp:ListItem Value="03">Muebles</asp:ListItem>
								<asp:ListItem Value="04">Inmuebles</asp:ListItem>
								<asp:ListItem Value="05">Personales</asp:ListItem>
								<asp:ListItem Value="06">Bancarias</asp:ListItem>
								<asp:ListItem Value="07">Otros(as)</asp:ListItem>
								<asp:ListItem Value="08">Reales</asp:ListItem>
                          </asp:DropDownList>
                      </td>
                      </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Descripción Garantía:</td>
            <td colspan="5"  >
                
                <asp:TextBox TextMode="MultiLine" ID="TxtDescripcionGarantia" runat="server" 
                     Width="480px" CssClass="ui-widget" ReadOnly="True" ></asp:TextBox>
                
                </td>
                  </tr>
                  
                    
            <tr><td  class="ui-widget-header" ><span lang="es">Valor total del acuerdo :</span></td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtvalortotalacuerdo" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Porcentaje cuota inicial :</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtporcuotainicial" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Valor cuota inicial:</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtvalorcuotainicial" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >N° de Cuotas</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtnrocuotas" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Fecha pago cuota inicial:</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtfechapagoinicial" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Vencimiento primera cuota:</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtvencprimcuota" runat="server" CssClass="ui-widget" Width="150px"
                            ReadOnly="True"></asp:TextBox>
                
                </td>
                  </tr>
                      
                  
            <tr><td  class="ui-widget-header" >Vencimiento última cuota:</td>
            <td colspan="5"  >
                
                        <asp:TextBox ID="Txtvencultcuota" runat="server" CssClass="ui-widget" Width="150px"
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

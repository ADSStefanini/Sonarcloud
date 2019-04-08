<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="proyectarcuotaparafiscales.aspx.vb" Inherits="coactivosyp.proyectarcuotaparafiscales"  %>
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
            $('#cmdCalcularInteres').button();
            $('#btndescargar').button();
            $('#btnNuevo').button();
            $('#btnGuardar').button();
            
            
            
            $("#accordion").accordion({ heightStyle: "content" });
            
            
            
            

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
                $("#txtfechaPago").keypress(function(event) { event.preventDefault(); });
                $("#txtfechaPago").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtfechaPago').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    //minDate: "0D",
                    maxDate: "+10Y",
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            });

         
    </script>
    
    <style type="text/css">
		    * { font-size:11px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;}			    		 
        .numeros { text-align:right; }
        
        </style>

    <script type="text/javascript" language="javascript">
        function mostrar_procesar() {
            document.getElementById('procesando_div').style.display = "";
            $("#dialogmodal").dialog({
                height: 150,
                modal: true
            });
            setTimeout(' document.getElementById("procesando_gif").src="../images/gif/ajax-loader.gif"', 100000);
        }
    </script>
 
   </head>
<body>
     
    
    <form id="form1" runat="server">
     
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>        
     
     <asp:RequiredFieldValidator ID="rfvtxtfechaPago" runat="server"  ErrorMessage="El campo <strong>FECHA DE PAGO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtfechaPago"  Display="None"></asp:RequiredFieldValidator>
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvtxtfechaPago"></asp:ValidatorCalloutExtender> 
    
      <asp:RequiredFieldValidator ID="rvftxtTxtNCuotas" runat="server"  ErrorMessage="El campo <strong>NRO DE CUOTAS</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "TxtNCuotas"  Display="None"></asp:RequiredFieldValidator>
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="rvftxtTxtNCuotas"></asp:ValidatorCalloutExtender> 
     
      <asp:RequiredFieldValidator ID="rvftxtPorcentajeCuotaini" runat="server"  ErrorMessage="El campo <strong>% CUOTA INICIAL</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtPorcentajeCuotaini"  Display="None"></asp:RequiredFieldValidator>
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="rvftxtPorcentajeCuotaini"></asp:ValidatorCalloutExtender> 
     
    
         
                <div><asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="12px" ForeColor="Blue"></asp:Label>&nbsp;<asp:LinkButton 
                        ID="lkbdescargar" runat="server"></asp:LinkButton>
    </div>
                <div>Los campos con asterisco (*) son requeridos...</div>
                 
                  <div>
                   <asp:Button ID="btnNuevo" runat="server" Text="Nueva Facilidad" />
                    <asp:Button id="cmdCalcularInteres" runat="server" Text="Proyectar Cuotas" OnClientClick="mostrar_procesar();" ></asp:Button>
                    <asp:Button ID="btndescargar" runat="server" Text="Descargar detalle de la proyección" />
                    &nbsp;
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Facilidad" OnClientClick="mostrar_procesar();"  />
                   </div>
                 
                 <div id="accordion">
          
         <h3 class="ui-state-hover" 
                            style="text-align: center; font-size: medium;">
                        1. INFORMACIÓN DE LA FACILIDAD DE PAGO</h3>
         
         <div >
           
                    
                    <table class="ui-widget-content" style=" margin-left: 20%; " >
                
                <tr>
                    <td class="ui-widget-header">
                        &nbsp;*
                        Calidad de aporte
                    </td>
                    <td >
                        <asp:DropDownList ID="cboCAPORTE" runat="server" Height="20px" Style="margin-left: 0px; " CssClass="ui-widget" Width="125px" AutoPostBack="true">
                            <asp:ListItem Text="Selecciones..." Value="0" />
                            <asp:ListItem Text="TRABAJADORES INDEPENDIENTES" Value="1" />
                            <asp:ListItem Text="EMPRESAS CON MAS DE 200 TRABAJADORES" Value="2" />
                            <asp:ListItem Text="EMPRESAS CON MENOS DE 200 TRABAJADORES" Value="3" />                            
                        </asp:DropDownList> 
                    </td>
                    </tr>
                    <tr>
                    <td class="ui-widget-header">
                        *
                    Día de pago maximo aportes
                    </td>
                    <td >
                        <asp:TextBox ID="lbldiaPago" runat="server" CssClass="ui-widget" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Total periodos :</td>
                    <td >
                        <asp:TextBox ID="txttotalperiodos" runat="server" ForeColor="Red" ReadOnly="True" CssClass="numeros ui-widget"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td class="ui-widget-header">
                        *
                        Fecha de pago</td>
                    <td >
                        <asp:TextBox ID="txtfechaPago" runat="server" CssClass="ui-widget"  ></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Períodos - cuota inicial :</td>
                    <td >
                        <asp:TextBox ID="txtperiodocuotainicial" runat="server" ForeColor="Red" ReadOnly="True" CssClass="numeros ui-widget"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td class="ui-widget-header">
                        *
                        Nro de cuotas</td>
                    <td >
                          <asp:DropDownList ID="TxtNCuotas" runat="server" CssClass="ui-widget">
                          </asp:DropDownList>
                    </td>
                    <td class="ui-widget-header">
                        Períodos restantes :</td>
                    <td >
                        <asp:TextBox ID="txtperiodosrestante" runat="server" ForeColor="Red" ReadOnly="True" CssClass="numeros ui-widget"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td class="ui-widget-header">
                        *
                        % cuota inicial: </td>
                    <td >
                          <asp:DropDownList ID="txtPorcentajeCuotaini" runat="server" 
                            CssClass="ui-widget">
                          </asp:DropDownList>
                    </td>
                    <td class="ui-widget-header">
                        % exacto cuota inicial
                    </td>
                    <td >
                        <asp:TextBox ID="txtPorcentajeideal" runat="server" ReadOnly="True" ForeColor="Red" CssClass="numeros ui-widget"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td class="ui-widget-header">
                        &nbsp;Cuota Inicial: </td>
                    <td >
                        <asp:TextBox ID="txtCuotaInicial" runat="server" ReadOnly="True" CssClass="numeros ui-widget"></asp:TextBox>
                    </td>
                    <td class="ui-widget-header">
                        Interes de cuota inicial :                     </td>
                    <td >
                        <asp:TextBox ID="txtInteresCuotaInicial" runat="server" ReadOnly="True" CssClass="numeros ui-widget"></asp:TextBox>
                    </td>
                    </tr>
                   
                    
                    <tr>
                    
                    <td colspan="4">
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    
                    
                        <asp:CheckBox ID="chkexcluircuota" runat="server" CssClass="ui-widget"  AutoPostBack="true"
                            Text="Desea excluir la cuota inicial ? (RELIQUIDAR):" TextAlign="Left" />
                            
                             </ContentTemplate>
                            <Triggers >
                            <asp:PostBackTrigger  ControlID="chkexcluircuota"  />
                            </Triggers>
                    </asp:UpdatePanel>
                        </td>
                    </tr>
                   
                    
                    </table>
                    
                   
                    </div>
                   
                             
         <h3 class="ui-state-hover" 
                            style="text-align: center; font-size: medium;">
                        2. INFORMACIÓN ADICIONAL </h3>
                        
                <div >
                    <table class="ui-widget-content" style=" margin-left: 20%; " >
                    
                    <tr>
                    <td class="ui-widget-header" >
                        Nro de acuerdo : </td>
                    <td colspan="1" >
                        <asp:TextBox ID="TxtNumAcuerdo" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                        </td>
                    <td class="ui-widget-header" colspan="1">
                        Nro. de expdiente :</td>
                    <td >
                        <asp:TextBox ID="TxtNroProceso" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <td  class="ui-widget-header" >* Solicitante :</td>
                      <td  colspan="3">
                
                <asp:TextBox ID="Txt_solicitante" runat="server"  Width="80px" 
                              CssClass="ui-widget" ></asp:TextBox>
                
                      <asp:TextBox ID="TxtNom_Solicitante" 
                          runat="server"  Width="269px" CssClass="ui-widget"></asp:TextBox></td>
                      <td class="ui-widget-header" style="width: 110px">
                          * Tipo Solicitante:</td>
                      <td >
                          <asp:DropDownList ID="CmbSolicitante" runat="server" Height="20px" 
                              Width="150px" CssClass="ui-widget">
                                <asp:ListItem Selected="True" Value="00">Seleccione...</asp:ListItem>
                                <asp:ListItem Value="01">Aportante</asp:ListItem>
								<asp:ListItem Value="02">Tercero</asp:ListItem>
								<asp:ListItem Value="03">Apoderado</asp:ListItem>
								<asp:ListItem Value="04">Representante Legal</asp:ListItem>
						  </asp:DropDownList>
                      </td>
                  </tr>
                  <tr>
                  <td  class="ui-widget-header" >Garante:</td>
            <td  colspan="3"><asp:TextBox ID="TxtGarante" runat="server"  Width="81px" 
                    CssClass="ui-widget"></asp:TextBox>
                
                <asp:TextBox ID="TxtNom_garante" runat="server" 
                    Width="269px" CssClass="ui-widget" ></asp:TextBox>
                
                </td>
                      <td class="ui-widget-header" >Tipo de Garantía:</td>
                      <td >
                          <asp:DropDownList ID="CmbGarante" runat="server" Height="20px" Width="150px" 
                              CssClass="ui-widget">
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
            <tr><td  class="ui-widget-header" >Descripcion Garantía:</td>
            <td  colspan="5">
                
                <asp:TextBox TextMode="MultiLine" ID="TxtDescripcionGarantia" runat="server" 
                     Width="633px" CssClass="ui-widget" ></asp:TextBox>
                
                </td>
                  </tr>
                  
                    
                    </table>
               </div>
               </div>
               
                    <div style=" overflow: scroll;width: 1220px;height: 900px;">
                    <table style=" margin-left: 15%; " >
                <tr>
                <td>
                </td>
                <td colspan="2" > 
                 <div style=" height: 407PX; overflow-x: scroll; width: 537px;">
                 <asp:GridView ID="DtgAcuerdos" runat="server"  
              AutoGenerateColumns="False"  width = "400px"
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" style="font-size: 12px; margin-left :120px"   
              HorizontalAlign="Center" PageSize="1">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="CUOTA_NUMERO" HeaderText="Nro. Cuota" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle  HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PERIODO" HeaderText="Nro. Períodos">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FECHA_CUOTA" HeaderText="Fecha de Pago" 
                                        DataFormatString="{0:dd/MM/yyyy}">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle  HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VALOR_CUOTA" HeaderText="Valor Cuota" HeaderStyle-HorizontalAlign="Center" 
                                        DataFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                                        <ItemStyle HorizontalAlign ="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                             </div>
                </td>
                </tr>
               
          
          </table>
          </div>
          <div id="dialogmodal" title="Procesando..." style="text-align:center" >   
            <span id= "procesando_div" style="display:none;" runat="server" >
                <img src="../images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
            </span>
          </div>    
       </form>
</body>
</html>

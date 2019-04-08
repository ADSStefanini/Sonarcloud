<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditENTES_DEUDORES.aspx.vb" Inherits="coactivosyp.EditENTES_DEUDORES" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Maestro de Deudores
        </title>
                
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script src="json2.js" type="text/javascript"></script>
        
        <script type="text/javascript">
            var clicks = 0;
            function SubmitOnce(btn) {
                clicks = clicks + 1;
                if (clicks == 1) 
                    return true;
                else {
                    btn.disabled = 'true';
                    return false;
                }
            }
        </script>
        
        <script type="text/javascript">
            $(function() {
                $("#tabs").tabs();
                $('#cmdSave').button();
                $('#cmdCancel').button();
                $('#cmdBorrar').button();

                var contenedor = '<% Response.Write(Request("pScr")) %>';
                if (contenedor == "repartidor") {
                    // alert("param lleno");
                    //Ocultar TAB de direcciones                    
                    $("#tabs").tabs({ disabled: [1] }); //when initializing the tabs
                    $("#tabs").tabs("option", "disabled", [1]); // or setting after init
                }

                //Aceptar solo numeros
                $(".numeros").keydown(function(event) {
                    //alert(event.keyCode);
                    // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 188 ||
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
                //
                $("input[type=text]").keyup(function() {
                    $(this).val($(this).val().toUpperCase());
                });


                //
            });                             
        </script>
        <style type="text/css">
			.add_edit { width: 20px; height: 16px;  float:none; }
			.imgaddedt { position:relative; top: 2px; }	
		    * { font-size:12px; font-family: Arial;}	
			#infoexpediente { 				
				background-color:#FFFFFF; 
				width:100%; 
				margin-bottom:10px; 
				margin-top:10px; 
				font-family: Lucida Grande,Lucida Sans,Arial,sans-serif; 
				font-weight: bold; 
				font-size: 11px;  
				-moz-border-radius: 3px; 
				border-radius: 3px; 
				-webkit-border-radius: 3px; 
				padding-top:4px; 
				padding-bottom:4px;				
			}  
			.ui-tabs .ui-state-disabled { display: none; }
		    .style1
            {
                height: 34px;
            }
            .style2
            {
                height: 35px;
            }
            .style3
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                height: 35px;
                width: 175px;
            }
            .style4
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                width: 175px;
            }
            .style5
            {
                font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
                font-size: 1.1em;
            }
		</style>
    </head>
    <body>
        <form id="form1" runat="server">        	
        	<div id="tabs">
				<ul>
                    <li><a href="#tabs-1"><asp:Label ID="lblPage1" runat="server" Text="Información gral"></asp:Label></a></li>
                    <li><a href="#tabs-2">Información de ubicación</a></li>
              	</ul>
                <div id="tabs-1">
                      <table id="tblEditENTES_DEUDORES" class="ui-widget-content">
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                No. de identificación *
                            </td>
                            <td>
                                <asp:TextBox id="txtED_Codigo_Nit" runat="server" MaxLength="13" 
                                    CssClass="numeros" ForeColor="Red" AutoPostBack="True"></asp:TextBox>                                    
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                Dígito de verificación.
                            </td>
                            <td>
                                <asp:TextBox id="txtED_DigitoVerificacion" runat="server" MaxLength="1"  
                                    Columns="1" CssClass="numeros" Width="22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                Tipo Identificación
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboED_TipoId" runat="server"></asp:DropDownList>                         
                            </td>
                        </tr>  
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                Nombre / Razón social *
                            </td>
                            <td>
                                <asp:TextBox id="txtED_Nombre" runat="server" MaxLength="100" Columns="80" CssClass="ui-widget"></asp:TextBox>
                            </td>
                        </tr>                                              
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                Tipo de persona *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboED_TipoPersona" runat="server" width="270px"></asp:DropDownList>
                            </td>
                        </tr>  
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                Estado persona *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="style5" id="cboED_EstadoPersona" runat="server"  
                                    width="270px" Height="16px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                                
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblTipoAportante" runat="server" Text="Tipo aportante *"></asp:Label>                                
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboED_TipoAportante" runat="server"  width="270px"></asp:DropDownList>
                            </td>
                        </tr> 
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblTipoDeudor" runat="server" Text="Tipo de deudor *"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboTipoDeudor" runat="server"  width="270px"></asp:DropDownList>
                            </td>
                        </tr> 
                          <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style3">
                                <asp:Label ID="lblTarjetaProf" runat="server" Text="Tarjeta profesional *"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox id="txtTarjetaProf" runat="server" MaxLength="100" Columns="80" CssClass="numeros"></asp:TextBox>
                            </td>
                              </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style3">
                                <asp:Label ID="lblPorcentaje" runat="server" 
                                    Text="Porcentaje de Participacion "></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox id="txtParticipacion" runat="server" MaxLength="100" Columns="80" 
                                    CssClass="numeros" Width="156px">
                                </asp:TextBox>
                            </td>
                        </tr> 
                        
                        <tr>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td colspan="2" class="style1">
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                            </td>                            
                        </tr> 
                                                      
                    </table>
                </div>
                <div id="tabs-2">
                    <!-- 02/feb/2014 Grid de direcciones del deudor -->
                    <div style="margin-left:2px; margin-top:4px; width:860px; height:740px;">                                                            	
						<iframe src="direcciones.aspx?ID=<%  Response.Write(Request("ID"))%>&pExpediente=<%  Response.Write(Request("pExpediente"))%>" width="860" height="740" scrolling="no" frameborder="0"></iframe>								
					</div>                    
                    <!-- ----------------------------------------------------------------------------- -->
                </div>
			</div>  
            <div id="infoexpediente">                
                <asp:Button id="cmdCancel" runat="server" Text="Cancelar"></asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button id="cmdSave" runat="server" Text="Guardar" OnClientClick="SubmitOnce(this);"></asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button id="cmdBorrar" runat="server" Text="Borrar" OnClientClick="SubmitOnce(this);"></asp:Button>&nbsp;&nbsp;&nbsp;
            </div>          
        </form>
    </body>
</html>

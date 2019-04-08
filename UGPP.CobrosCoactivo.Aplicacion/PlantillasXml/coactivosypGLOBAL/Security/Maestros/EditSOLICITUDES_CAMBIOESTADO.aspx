<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditSOLICITUDES_CAMBIOESTADO.aspx.vb" Inherits="coactivosyp.EditSOLICITUDES_CAMBIOESTADO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar solicitudes de cambio de estado
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" /> 
        
        
        <script type="text/javascript">
            $(function() {
                $('#cmdDelete').button();
                $('#cmdSave').button();
                $('#cmdCancel').button();
                $('#cmdImprimir').button();
                $('#cmdSaveRevisor').button();
                $('#cmdSaveSupervisor').button();
                
                //
                //Controles de dolo lectura
                $(".SoloLectura").keypress(function(event) { event.preventDefault(); });
                $(".SoloLectura").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                                             
            });
        </script>
        
        <style type="text/css">
		    * { font-size:12px; font-family:Arial}	
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden {display:none;}
		</style>
		
    </head>
    <body>
        <form id="form1" runat="server">
            <div id="panelGestor" runat="server">
                <table id="tblEditSOLICITUDES_CAMBIOESTADO" class="ui-widget-content" style="width: 800px"> 
                <tr>
	                <td>
		                &nbsp;
	                </td>
	                <td colspan="2">
		                Información del gestor
	                </td>	
                </tr>  
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        <label id="lblGestor" runat="server">Gestor que solicita el cambio</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAbogado" runat="server" Columns="90"></asp:TextBox>
                    </td>
                </tr>  
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado al que se traslada * 
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestado" runat="server"></asp:DropDownList>
                    </td>
                </tr>  
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Observaciones *
                    </td>
                    <td>
                        <textarea id="taObservaciones" cols="90" rows="5" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
                    </td>
                </tr>                                                            
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado solicitud
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestadosol" runat="server" 
                            Enabled="False"></asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>                                
            </table>
            </div>
            
            <div id="panelRevisor" runat="server">
                <table id="tblEditSOLCAMBIOREVISOR" class="ui-widget-content" style="width: 800px"> 
                <tr>
	                <td>
		                &nbsp;
	                </td>
	                <td colspan="2">
		                Información del revisor
	                </td>	
                </tr>  
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            <label id="lblRevisor" runat="server">Revisor</label>
		            </td>
		            <td>
			            <asp:TextBox ID="txtRevisor" runat="server" Columns="90"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            <label id="Label1" runat="server">Aprobar escalamiento a supervisor? *</label>
		            </td>
		            <td>
			            <asp:DropDownList CssClass="ui-widget" id="cboAprob_Revisor" runat="server" AutoPostBack="True"></asp:DropDownList>
		            </td>
	            </tr>
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td class="ui-widget-header">
			            <label id="Label2" runat="server">Fecha de aprobación / rechazo *</label>
		            </td>
		            <td>
			            <asp:TextBox ID="txtFecha_Aprob_Revisor" runat="server" Columns="20" CssClass="SoloLectura"></asp:TextBox>
		            </td>
	            </tr>
	            <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Observaciones
                    </td>
                    <td>
                        <textarea id="taNota_Revisor" cols="90" rows="5" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
                    </td>
                </tr>   
            </table>
            </div>
            <div id="panelSupervisor" runat="server">
	            <table id="tblEditSOLCAMBIOSUPERVISOR" class="ui-widget-content" style="width: 800px"> 
		            <tr>
			            <td>
				            &nbsp;
			            </td>
			            <td colspan="2">
				            Información del supervisor
			            </td>	
		            </tr>  
		            <tr>
			            <td>
				            &nbsp;
			            </td>
			            <td class="ui-widget-header">
				            <label id="lblSupervisor" runat="server">Supervisor</label>
			            </td>
			            <td>
				            <asp:TextBox ID="txtEjecutor" runat="server" Columns="90"></asp:TextBox>
			            </td>
		            </tr>
		            <tr>
			            <td>
				            &nbsp;
			            </td>
			            <td class="ui-widget-header">
				            <label id="lblAprobarEscalamientoEjecutor" runat="server">Aprobar escalamiento a repartidor? *</label>
			            </td>
			            <td>
				            <asp:DropDownList CssClass="ui-widget" id="cboAprob_Ejecutor" runat="server" AutoPostBack="True"></asp:DropDownList>
			            </td>
		            </tr>
		            <tr>
			            <td>
				            &nbsp;
			            </td>
			            <td class="ui-widget-header">
				            <label id="lblFechaAprobacionEjecutor" runat="server">Fecha de aprobación / rechazo *</label>
			            </td>
			            <td>
				            <asp:TextBox ID="txtFecha_Aprob_Supervisor" runat="server" Columns="20" CssClass="SoloLectura"></asp:TextBox>
			            </td>
		            </tr>
		            <tr>
			            <td>
				            &nbsp;
			            </td>
			            <td class="ui-widget-header">
				            Observaciones
			            </td>
			            <td>
				            <textarea id="taNota_Ejecutor" cols="90" rows="5" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
			            </td>
		            </tr>   
	            </table>
            </div>
            <table id="tblBotones" class="ui-widget-content">
	            <tr>
		            <td>
			            &nbsp;
		            </td>
		            <td>
            			
		            </td>
		            <td>
			            <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
			            <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
			            <asp:Button id="cmdImprimir" runat="server" Text="Imprimir" cssClass="PCGButton"></asp:Button>
			            <asp:Button id="cmdSaveRevisor" runat="server" Text="Guardar datos del revisor" cssClass="PCGButton"></asp:Button>
			            <asp:Button id="cmdSaveSupervisor" runat="server" Text="Guardar datos del supervisor" cssClass="PCGButton"></asp:Button>
		            </td>
	            </tr>
            </table>
        </form>
    </body>
</html>

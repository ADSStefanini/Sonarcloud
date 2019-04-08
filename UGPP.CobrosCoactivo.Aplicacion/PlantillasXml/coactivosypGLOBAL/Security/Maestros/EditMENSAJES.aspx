<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMENSAJES.aspx.vb" Inherits="coactivosyp.EditMENSAJES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar mensajes
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {                
                $('#cmdSave').button();
                $('#cmdCancel').button();
                $('#cmdResponder').button();
                $('#cmdReenviar').button();
            });
        </script>
        <style type="text/css">
		    * { font-size:12px; font-family: Arial;}			    
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditMENSAJES" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        De:
                    </td>
                    <td>                        
                        <asp:TextBox id="txtUsuOrigen" runat="server" ReadOnly="True" ForeColor="Red" style="border: 1px solid #a9a9a9; width: 530px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Para:
                    </td>
                    <td>
                        <asp:dropdownlist id="cboUsuDestino" runat="server" style="z-index: 1; border: 1px solid #a9a9a9; width: 530px;" 
                            TabIndex="8">			
	                    </asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        Mensaje:
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>                    
                    <td colspan="2">                                            
                        <textarea id="taMensaje" cols="120" rows="10" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
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
                <tr>
                    <td>
                        &nbsp; 
                    </td>
                    <td class="ui-widget-header">
                        <asp:Button ID="cmdMostrarExpediente" runat="server" Text="Expediente" />
                    </td>
                    <td>
                        <asp:dropdownlist id="cboExpediente" runat="server" style="z-index: 1; border: 1px solid #a9a9a9; width: 530px;" 
                            TabIndex="8">			
	                    </asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de envío
                    </td>
                    <td>
                        <asp:TextBox id="txtFecEnvio" runat="server" style="border: 1px solid #a9a9a9; color:Red; width:140px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha Recibo
                    </td>
                    <td>
                        <asp:TextBox id="txtFecRecibo" runat="server" style="border: 1px solid #a9a9a9; color:Red; width:140px;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Leído
                    </td>
                    <td>
                        <asp:CheckBox id="chkleido" runat="server" CssClass="ui-widget"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Enviar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdResponder" runat="server" Text="Responder" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdReenviar" runat="server" Text="Reenviar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>


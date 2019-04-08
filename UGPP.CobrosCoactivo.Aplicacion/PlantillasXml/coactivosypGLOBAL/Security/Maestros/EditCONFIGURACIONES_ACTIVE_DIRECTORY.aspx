<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditCONFIGURACIONES_ACTIVE_DIRECTORY.aspx.vb" Inherits="coactivosyp.EditCONFIGURACIONES_ACTIVE_DIRECTORY" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditCONFIGURACIONES_ACTIVE_DIRECTORY
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" /> 
        
        <script type="text/javascript">
            $(function() {                
                $('#cmdSave').button();                
            });
        </script>
        
        <style type="text/css">
		    * { font-size:12px; font-family: Arial;}				
		</style>
		
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditCONFIGURACIONES_ACTIVE_DIRECTORY" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Servidor LDAP
                    </td>
                    <td>
                        <asp:TextBox id="txtservidor" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Puerto
                    </td>
                    <td>
                        <asp:TextBox id="txtpuerto" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Usuario Servicio
                    </td>
                    <td>
                        <asp:TextBox id="txtUsuarioServicio" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Clave Usuario Servicio
                    </td>
                    <td>
                        <asp:TextBox id="txtClaveUsuarioServ" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Usar Active Directory
                    </td>
                    <td>
                        <asp:CheckBox id="chkusarAD" runat="server" CssClass="ui-widget"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

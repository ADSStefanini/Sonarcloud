<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditDIRECCIONES.aspx.vb" Inherits="coactivosyp.EditDIRECCIONES" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar direcciones
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" /> 
        
        <script type="text/javascript">
            $(function() {
                //$('#cmdDelete').button();
                $('#cmdSave').button();
                $('#cmdCancel').button();
                
                // Campos a mayusculas
                $("input[type=text]").keyup(function() {
                    $(this).val($(this).val().toUpperCase());
                });
                //
            });                  
        </script>
        <style type="text/css">
		    * { font-size:12px; font-family: Arial;}				
		</style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditDIRECCIONES" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Dirección
                    </td>
                    <td>
                        <asp:TextBox id="txtDireccion" runat="server" MaxLength="180" CssClass="ui-widget" Columns="80"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Departamento
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboDepartamento" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Ciudad
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboCiudad" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Teléfono
                    </td>
                    <td>
                        <asp:TextBox id="txtTelefono" runat="server" MaxLength="40" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Email
                    </td>
                    <td>
                        <asp:TextBox id="txtEmail" runat="server" MaxLength="80" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Móvil
                    </td>
                    <td>
                        <asp:TextBox id="txtMovil" runat="server" MaxLength="40" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Página web
                    </td>
                    <td>
                        <asp:TextBox id="txtpaginaweb" runat="server" MaxLength="40" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>                                       
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <%--<asp:Button id="cmdDelete" runat="server" Text="Borrar" cssClass="PCGButton"></asp:Button>--%>
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>
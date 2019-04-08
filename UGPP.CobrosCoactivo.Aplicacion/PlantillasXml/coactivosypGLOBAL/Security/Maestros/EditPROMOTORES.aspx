<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPROMOTORES.aspx.vb" Inherits="coactivosyp.EditPROMOTORES" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditPROMOTORES
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {                
                $('#cmdSave').button();
                $('#cmdCancel').button();
            });
        </script>
        <style type="text/css">
            * { font-size:12px; font-family:Arial;}
        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditPROMOTORES" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Cédula
                    </td>
                    <td>
                        <asp:TextBox id="txtcedula" runat="server" MaxLength="13" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nombre
                    </td>
                    <td>
                        <asp:TextBox id="txtnombre" runat="server" MaxLength="50" style=" width:250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Dirección
                    </td>
                    <td>
                        <asp:TextBox id="txtdireccion" runat="server" MaxLength="150" style=" width:400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Teléfono
                    </td>
                    <td>
                        <asp:TextBox id="txttelefono" runat="server" MaxLength="40" CssClass="ui-widget"></asp:TextBox>
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
                        <asp:TextBox id="txtobservac" runat="server" style=" width:400px"></asp:TextBox>
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
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>
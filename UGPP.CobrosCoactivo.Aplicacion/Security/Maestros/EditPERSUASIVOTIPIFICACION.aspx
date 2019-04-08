<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPERSUASIVOTIPIFICACION.aspx.vb" Inherits="coactivosyp.EditPERSUASIVOTIPIFICACION" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditPERSUASIVOTIPIFICACION
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
            });
        </script>
        <style type="text/css">			
		    * { font-size:12px; font-family:Arial;}			     
		</style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditPERSUASIVOTIPIFICACION" class="ui-widget-content"  >
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nombre
                    </td>
                    <td>
                        <asp:TextBox id="txtnombre" runat="server" MaxLength="80" CssClass="ui-widget" style="width:40em;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button id="cmdDelete" runat="server" Text="Borrar" cssClass="PCGButton"></asp:Button>
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Regresar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

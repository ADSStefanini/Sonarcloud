<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPAGOSOBSERVACIONES.aspx.vb" Inherits="coactivosyp.EditPAGOSOBSERVACIONES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar observaciones de persuasivo
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
            <table id="tblEditPERSUASIVOOBSERVACIONES" class="ui-widget-content">                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha
                    </td>
                    <td>
                        <asp:TextBox id="txtFecha" runat="server" CssClass="ui-widget"></asp:TextBox>
                        <asp:ImageButton ID="imgBtnBorraFecha" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Fecha oficio" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Gestor
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cbogestor" runat="server"></asp:DropDownList>
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
                        <asp:TextBox id="txtObservaciones" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
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
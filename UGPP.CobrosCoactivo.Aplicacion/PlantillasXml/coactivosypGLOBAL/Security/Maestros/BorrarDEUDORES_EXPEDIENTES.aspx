<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BorrarDEUDORES_EXPEDIENTES.aspx.vb" Inherits="coactivosyp.BorrarDEUDORES_EXPEDIENTES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Borrar personas / entidades de los expedientes</title>
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
            <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
            <script src="jquery.ui.button.js" type="text/javascript"></script>
            <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
            
            <script src="json2.js" type="text/javascript"></script>
            
            <script type="text/javascript">
                $(function() {                                
                    $('#cmdSave').button();
                    $('#cmdCancel').button();
                });                             
            </script>
            
            <style type="text/css">
		        * { font-size:12px; font-family: Arial;}
		        .style4
            {
                border: 1px solid #4297d7;
                background: #5c9ccc url('css/redmond/images/ui-bg_gloss-wave_55_5c9ccc_500x100.png') repeat-x 50% 50%;
                color: #ffffff;
                font-weight: bold;
                width: 175px;
            }
		    </style>    		
    </head>
    
    <body>
        <form id="form1" runat="server">
        <div>
            <table id="tblEditENTES_DEUDORES" class="ui-widget-content">
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="3" class="style4" style="width: 400px;">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </td>                    
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button id="cmdSave" runat="server" Text="Borrar" ></asp:Button></td>
                    <td>&nbsp;</td>
                    <td><asp:Button id="cmdCancel" runat="server" Text="Cancelar"></asp:Button>&nbsp;&nbsp;&nbsp;</td>
                                                                                                
                </tr>
            </table>
        </div>
        </form>
    </body>
</html>

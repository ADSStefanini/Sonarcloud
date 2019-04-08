<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditAlarmas.aspx.vb" Inherits="coactivosyp.EditAlarmas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <title>Editar alarmas</title>
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
            <table id="tblEditALARMAS" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Código
                    </td>
                    <td>
                        <asp:TextBox id="txtcodigo" runat="server" CssClass="ui-widget"></asp:TextBox>
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
                        <asp:TextBox id="txtnombre" runat="server" MaxLength="254" CssClass="ui-widget" Columns="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Texto actuar pronto
                    </td>
                    <td>
                        <asp:TextBox id="txttextoactuarpronto" runat="server" CssClass="ui-widget" Columns="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Texto sin acción
                    </td>
                    <td>
                        <asp:TextBox id="txttextosinaccion" runat="server" CssClass="ui-widget" Columns="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Días actuar pronto inicio
                    </td>
                    <td>
                        <asp:TextBox id="txtdiasactuarprontoINI" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Días actuar pronto fin
                    </td>
                    <td>
                        <asp:TextBox id="txtdiasactuarprontoFIN" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Días sin acción inicio
                    </td>
                    <td>
                        <asp:TextBox id="txtdiassinaccionINI" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Días sin acción fin
                    </td>
                    <td>
                        <asp:TextBox id="txtdiassinaccionFIN" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Dias escalamiento
                    </td>
                    <td>
                        <asp:TextBox id="txtdiasescalamiento" runat="server" CssClass="ui-widget"></asp:TextBox>
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
                        <asp:TextBox id="txtobservaciones" runat="server" CssClass="ui-widget" Columns="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <%--<asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>--%>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

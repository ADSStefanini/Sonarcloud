﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMAESTRO_DEUDORSOLIDARIO.aspx.vb" Inherits="coactivosyp.EditMAESTRO_DEUDORSOLIDARIO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditMAESTRO_DEUDORSOLIDARIO
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
		    body{ background-color:#01557C}	
	        * { font-size:12px; font-family:Arial}				 
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditMAESTRO_DEUDORSOLIDARIO" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Tipo de identificación
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboMDS_TipoId" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. de identificación
                    </td>
                    <td>
                        <asp:TextBox id="txtMDS_Codigo_Nit" runat="server" MaxLength="13" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Dígito Verificación
                    </td>
                    <td>
                        <asp:TextBox id="txtMDS_DigitoVerificacion" runat="server" MaxLength="1" CssClass="ui-widget"></asp:TextBox>
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
                        <asp:TextBox id="txtMDS_Nombre" runat="server" MaxLength="100" Columns="80" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Tipo de persona
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboMDS_TipoPersona" runat="server"></asp:DropDownList>                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado Persona
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboMDS_EstadoPersona" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button id="cmdDelete" runat="server" Text="Borrar" cssClass="PCGButton"></asp:Button>
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

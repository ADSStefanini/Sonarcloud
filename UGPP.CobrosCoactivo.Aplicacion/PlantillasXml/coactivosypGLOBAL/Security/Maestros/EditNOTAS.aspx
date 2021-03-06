﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditNOTAS.aspx.vb" Inherits="coactivosyp.EditNOTAS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Editar notas / observaciones</title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                $('#cmdSave').button();
                //
                //Controles de solo lectura
                $(".SoloLectura").keypress(function(event) { event.preventDefault(); });
                $(".SoloLectura").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });             
            });
        </script>
        <style type="text/css">		    
	        * { font-size:12px; font-family:Arial;}		        	   
        </style>
    </head>
    
    <body>
        <form id="form1" runat="server">
            <table id="tblEditNOTAS" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha
                    </td>
                    <td>
                        <asp:TextBox id="txtFecha" runat="server" CssClass="SoloLectura"></asp:TextBox>
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
                        <%--<asp:TextBox id="txtObservaciones" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:TextBox id="txtObservaciones" runat="server"  
                                            Height="101px" TextMode="MultiLine" Width="450px" Columns="80" Rows="20"></asp:TextBox>
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
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

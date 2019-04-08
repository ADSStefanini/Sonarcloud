<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerPAGOS.aspx.vb" Inherits="coactivosyp.VerPAGOS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            EditPAGOS
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
		    * { font-size:12px; font-family: Arial;}			    
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditPAGOS" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nro Consignacion
                    </td>
                    <td>
                        <asp:TextBox id="txtNroConsignacion" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Nro Exp
                    </td>
                    <td>
                        <asp:TextBox id="txtNroExp" runat="server" MaxLength="12" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fec Solverif
                    </td>
                    <td>
                        <asp:TextBox id="txtFecSolverif" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fec Verificado
                    </td>
                    <td>
                        <asp:TextBox id="txtFecVerificado" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        estado
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestado" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        User Verif
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboUserVerif" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Fecha
                    </td>
                    <td>
                        <asp:TextBox id="txtpagFecha" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Fecha Deudor
                    </td>
                    <td>
                        <asp:TextBox id="txtpagFechaDeudor" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Nro Tit Judicial
                    </td>
                    <td>
                        <asp:TextBox id="txtpagNroTitJudicial" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Capital
                    </td>
                    <td>
                        <asp:TextBox id="txtpagCapital" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Ajuste Dec1406
                    </td>
                    <td>
                        <asp:TextBox id="txtpagAjusteDec1406" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Interes
                    </td>
                    <td>
                        <asp:TextBox id="txtpagInteres" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Gastos Proc
                    </td>
                    <td>
                        <asp:TextBox id="txtpagGastosProc" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Exceso
                    </td>
                    <td>
                        <asp:TextBox id="txtpagExceso" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pag Total
                    </td>
                    <td>
                        <asp:TextBox id="txtpagTotal" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        pagestadoprocfrp
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cbopagestadoprocfrp" runat="server"></asp:DropDownList>                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Regresar al listado" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>


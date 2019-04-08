<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditDETALLE_EMBARGO.aspx.vb" Inherits="coactivosyp.EditDETALLE_EMBARGO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar detalle de embargos
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                $('#cmdTitulo').button();
                $('#cmdSave').button();
                $('#cmdCancel').button();
            });
        </script>
        
        <style type="text/css">
	        * { font-size:12px; font-family:Arial;}	
	        .BoundFieldItemStyleHidden { display:none; }
	        .BoundFieldHeaderStyleHidden { display:none; }
	        .hidden { display:none; }
        </style>
    
    </head>
    <body>
        <form id="form1" runat="server">
            <table id="tblEditDETALLE_EMBARGO" class="ui-widget-content">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header" colspan="2" style=" height:25px;">
                        Edición del detalle de la medida cautelar
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Objeto de embargo
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboTipoBien" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Banco
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboBanco" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Identificación del Bien
                    </td>
                    <td>
                        <asp:TextBox id="txtIdentifBien" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Valor
                    </td>
                    <td>
                        <asp:TextBox id="txtvalorbien" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Efectivo
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtefectivo" runat="server" CssClass="ui-widget"></asp:TextBox>--%>
                        <asp:DropDownList CssClass="ui-widget" id="cboEfectivo" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Causal
                    </td>
                    <td>
                        <%--<asp:TextBox id="txtcausal" runat="server" MaxLength="50" CssClass="ui-widget" Columns="50"></asp:TextBox>--%>
                        <asp:DropDownList CssClass="ui-widget" id="cboCausal" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button id="cmdTitulo" runat="server" Text="Gestionar Título de Depósito Judicial (TDJ)" cssClass="PCGButton"></asp:Button>
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

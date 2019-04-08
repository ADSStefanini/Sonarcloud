<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="jerarquia_usuarios.aspx.vb" Inherits="coactivosyp.jerarquia_usuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>Jerarquía de usuarios</title>
        <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
        <link href="tablacolor.css" rel="stylesheet" type="text/css" />
        <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
        <style type="text/css">                    
        </style>
        <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
        <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
        <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
        <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>          
    </head>
<body>
    <div id="container">
        <h1 id="Titulo"><a href="#">Jerarquía de usuarios </a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 42px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            
            <asp:Panel ID="pnlError" runat="server" CssClass="CajaDialogoErr" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
                <hr />				    
                <asp:Button style="Z-INDEX: 116; width: 75px;" id="btnNoerror" runat="server" Text="Aceptar" Height="23px" CssClass="RedButton"></asp:Button>    
            </asp:Panel>		

        </form>
    </div>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="estado-cuenta.aspx.vb" Inherits="coactivosyp.estado_cuenta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Tecno Expedientes !</title>
   <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
   <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
   <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
</head>
<body>
<!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu4.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>

   <div id="container">
        <h1 id="Titulo"><a href="#">Estado de cuenta - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Label1" style="position:absolute;top:107px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:bold;">Digite el número del predial: </div>
            <asp:TextBox ID="txtEnte" runat="server" style="position:absolute;top:131px;left:34px;width:709px;z-index:777;" ></asp:TextBox>
            <asp:customvalidator style="position:absolute;top:164px; left:46px; width: 702px;" id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
            
            <asp:Button id="btnAceptar" runat="server" Text="Consultar" ValidationGroup="textovalidados"
                style="position:absolute;top:186px; left:34px; width: 92px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
            </asp:Button>
            
            <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
                style="position:absolute;top:186px; left:133px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
                CssClass="Botones">
            </asp:Button>
        </form>
   </div>
   
      
</body>
</html>
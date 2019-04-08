<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProbarFecha.aspx.vb" Inherits="coactivosyp.ProbarFecha" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Probar Fecha</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
      .hora
      {
      	background-color:#ffffff;
      	border: solid 1px #D8D8D8;
        width: 470px;
        
        -moz-box-shadow: 10px 10px 10px #000; /* Firefox */
        -webkit-box-shadow: 10px 10px 10px #000; /* Safari, Chrome */
        box-shadow: 10px 10px 10px #000; /* CSS3 */
        }
    </style>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <div id="container">
        <h1 id="Titulo"><a href="#">Configuraciones del servidor</a></h1>
        <form id="form1" runat="server">
        
        <div class="hora" style="position:absolute;left:39px; top: 108px;">
            <img alt="" src="images/icons/Clock.png" />
            <div style="position:absolute;top: 17px; left: 110px;height: 22px; width: 337px; text-align:center;font-weight:bold;">
             HORA DEL SERVIDOR</div>
            <asp:Label ID="lblFechaHora" runat="server" Text="Label" 
                
                
                style="position: absolute; text-align:center; width: 337px; top: 48px; left: 110px;"></asp:Label>
                
            <asp:LinkButton ID="LinkHora" runat="server" 
                
                style="position: absolute;text-align:center; width: 337px; top: 76px; left: 110px;">Recargar</asp:LinkButton>
        </div>
        
        <div class="hora" 
            style="position:absolute;left:39px; top: 233px; height: 156px;">
            <img alt="" src="images/icons/HP-Tower.png" />
            
            <div style="position:absolute;top: 17px; left: 110px;height: 22px; width: 337px; text-align:center;font-weight:bold;">
                CONEXION B.D</div>
            
            <asp:Label ID="lblservidor" runat="server" Text="Label" 
                
                style="position: absolute; text-align:center; width: 337px; top: 49px; left: 110px;"></asp:Label>
            
            <asp:Label ID="lblusuario" runat="server" Text="Label" 
                
                style="position: absolute; text-align:center; width: 337px; top: 74px; left: 110px;"></asp:Label>
                
            <asp:Label ID="lblBasedato" runat="server" Text="Label" 
                                
                style="position: absolute; text-align:center; width: 337px; top: 100px; left: 110px;"></asp:Label>
                                
            <asp:Label ID="lblTipo" runat="server" Text="Label" 
                                
                
                style="position: absolute; text-align:center; width: 337px; top: 127px; left: 110px;"></asp:Label>                             
            
        </div>
                    
        
        </form>
    </div>
</body>
</html>

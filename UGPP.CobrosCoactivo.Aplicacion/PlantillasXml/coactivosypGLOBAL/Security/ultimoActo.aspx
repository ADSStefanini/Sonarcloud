<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ultimoActo.aspx.vb" Inherits="coactivosyp.ultimoActo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Tecno Expedientes !</title>
   <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
   <style type="text/css">
    body { padding: 5px 0 0 0; margin: 0; font: .7em Tahoma, Arial, sans-serif; line-height: 1.7em; background: #fff url(images/bg.gif) repeat-x; color: #454545; }
    a { color: #2F637A; background: inherit; }
    a:hover { color: #808080; background: inherit; }
    p {	margin: 0  0 5px 0; }
    h1 {margin: 2px;  }
    h1 a, h2 a { color: #000; background: inherit; text-decoration: none;}
    #content { margin: 10px auto; margin-left:25px; margin-top:28px; }
    #logo { margin: 0 0 10px 0; }
    #slogan { font-size: 0.9em; margin: 0 0 10px 2px; padding: 0; color: #808080; background: #fff; }
   </style>
</head>
<body>
  <form id="form1" runat="server">
   <div id="content">
    <div id="logo">
			<h1><a href="#" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
			<p id="slogan">Gestión Documental.</p>
	</div>
    <asp:Button ID="btnUltimoActo" runat="server" Text="Ejecutar Proceso ......!!!!"         
                style="width: 159px" />
   </div>
  </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FORMULARIOPRUEBACONTROL.aspx.vb" Inherits="coactivosyp.FORMULARIOPRUEBACONTROL" %>

<%@ Register Src="~/Security/Controles/BandejaTitulos.ascx" TagPrefix="uc1" TagName="BandejaTitulos" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:BandejaTitulos runat="server" ID="BandejaTitulos" />
        </div>
    </form>
</body>
</html>

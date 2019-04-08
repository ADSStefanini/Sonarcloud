<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Importacion_automática_expediente.aspx.vb" Inherits="coactivosyp.Importacion_automática_expediente" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div>
          <h1>IMPORTACIÓN AUTOMÁTICA DE EXPEDIENTES</h1>
      </div>
      <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress runat="server" id="PageUpdateProgress">
            <ProgressTemplate>
                Loading...
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel runat="server" id="Panel">
            <ContentTemplate>
                <asp:Button ID="btnImpdatos" runat="server" Text="Importar Datos" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

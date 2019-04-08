<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestCargaDocumento.aspx.vb" Inherits="coactivosyp.TestCargaDocumento" %>
<%@ Register TagPrefix="HeadControl" TagName="HeadScripts" Src="~/Security/Controles/commons-head.ascx" %>
<%@ Register TagPrefix="FooterControl" TagName="FootScripts" Src="~/Security/Controles/commons-footer.ascx" %>

<!-- Requeridos para el funcionamiento de la carga del documento -->
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="Documento" Src="~/Security/Controles/DatosDocumento.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <HeadControl:HeadScripts id="headScripts" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" />

        <div>
            <!-- Mal configurado -->
            <%--<uc1:Documento id="addDoc" runat="server" />--%>
        </div>
         <div>
            <!-- Nuevo Documento -->
            <uc1:Documento id="newDoc" runat="server" idTitulo="15613" idDocumento="41"/>
        </div>
         <div>
            <!-- Editar Documento-->
            <%--<uc1:Documento id="editDoc" runat="server" idTituloDocumento="19" modificarDoc="0" />--%>
        </div>
        <asp:Button ID="btnTestIdMaestroDoc" runat="server" Text="Obtener ID de Carga"  />
    </form>
    
    <FooterControl:FootScripts id="footScripts1" runat="server" />
</body>
</html>

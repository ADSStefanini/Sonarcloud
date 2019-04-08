<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ErrorPages/Site.Master" CodeBehind="403.aspx.vb" Inherits="coactivosyp.Formulario_web403" %>
<%@ MasterType TypeName ="coactivosyp.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="background-color:#EEEEEE;">
	    <div id="header"><h1>Error del servidor</h1></div>
        <div id="content">
            <div class="content-container">
                <fieldset>
                    <h2>403 - Prohibido: acceso denegado.</h2>
                    <h3>No tiene permiso para ver este directorio o página con las credenciales que ha proporcionado.</h3>
                </fieldset>
            </div>
        </div>
        <br />
    </div>
</asp:Content>

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ErrorPages/Site.Master" CodeBehind="404.aspx.vb" Inherits="tecnoexpedientes.Formulario_web404" %>
<%@ MasterType TypeName ="tecnoexpedientes.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="background-color:#EEEEEE;">
	    <div id="header"><h1>Error del servidor</h1></div>        <div id="content">            <div class="content-container">                <fieldset>                    <h2>404: archivo o directorio no encontrado.</h2>                    <h3>Puede que se haya quitado el recurso que está buscando, que se le haya cambiado el nombre o que no esté disponible temporalmente.</h3>                </fieldset>            </div>        </div>        <br />    </div>
</asp:Content>

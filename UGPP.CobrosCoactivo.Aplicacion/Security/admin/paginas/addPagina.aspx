<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Formulario.Master" CodeBehind="addPagina.aspx.vb" Inherits="coactivosyp.addPagina" %>
<%@ Register TagPrefix="uc1" TagName="PaginaForm" Src="paginas.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="formContent" runat="server">

    <uc1:PaginaForm id="addForm" runat="server" />

</asp:Content>

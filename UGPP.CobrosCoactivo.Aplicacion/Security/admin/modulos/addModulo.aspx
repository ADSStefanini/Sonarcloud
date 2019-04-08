<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Formulario.Master" CodeBehind="addModulo.aspx.vb" Inherits="coactivosyp.addModulo" %>

<%@ Register TagPrefix="uc1" TagName="ModuloForm" Src="modulo.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="formContent" runat="server">
    <uc1:ModuloForm id="addForm" runat="server"></uc1:ModuloForm>
</asp:Content>

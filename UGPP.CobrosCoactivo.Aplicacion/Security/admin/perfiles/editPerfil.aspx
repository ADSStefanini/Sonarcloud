<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Security/Plantillas/Formulario.Master" CodeBehind="editPerfil.aspx.vb" Inherits="coactivosyp.editPerfil" %>

<%@ Register TagPrefix="uc1" TagName="PerfilForm" Src="perfil.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="formContent" runat="server">

    <uc1:PerfilForm id="addForm" runat="server"></uc1:PerfilForm>

</asp:Content>

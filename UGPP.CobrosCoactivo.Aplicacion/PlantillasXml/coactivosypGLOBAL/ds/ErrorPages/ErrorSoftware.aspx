<%@ Page Language="vb" MasterPageFile="~/ErrorPages/Site.Master" AutoEventWireup="false" CodeBehind="ErrorSoftware.aspx.vb" Inherits="coactivosyp.ErrorSoftware" %>
<%@ MasterType TypeName ="coactivosyp.Site" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .Error_in
  {
      padding:10px;
  }
 </style>
 <script type="text/javascript">
     jQuery(document).ready(function($) {
        $('#Error_inPanel').load("recycle/recycle.html");
     });
 </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="Error_inPanel"></div>
</asp:Content>
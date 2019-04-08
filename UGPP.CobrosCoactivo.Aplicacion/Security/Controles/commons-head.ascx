<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="commons-head.ascx.vb" Inherits="coactivosyp.commons_head" %>

<link rel="shortcut icon" type="image/x-icon" href="<%=ResolveClientUrl("~/Security/images/icons/web_page.ico") %>">

<!-- jquery-1.10.2.min.js y jquery-ui-1.10.4.custom.min.js -->
<script type="text/javascript" src="<%=ResolveClientUrl("~/Security/Maestros/js/jquery-1.10.2.min.js") %>"></script>
<script type="text/javascript" src="<%=ResolveClientUrl("~/Security/Maestros/js/jquery-ui-1.10.4.custom.min.js") %>"></script>
<link rel="stylesheet" href="<%=ResolveClientUrl("~/Security/Maestros/css/redmond/jquery-ui-1.10.4.custom.css") %>" />

<link href="<%=ResolveClientUrl("~/css/main.css") %>" rel="stylesheet" />

<script type="text/javascript">
    var NivelPerfil = "<% Response.Write(Session("mnivelacces")) %>";
    var baseUrl = "<%= Request.Url.Scheme + "://" + Request.Url.Authority & Request.ApplicationPath.TrimEnd("/") + "/" %>"
    //console.log(baseUrl)
</script>
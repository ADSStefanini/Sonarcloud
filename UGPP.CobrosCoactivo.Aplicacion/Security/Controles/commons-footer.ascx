<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="commons-footer.ascx.vb" Inherits="coactivosyp.commons_footer" %>

<!-- Bootstrap -->
<link href="<%=ResolveClientUrl("~/assets/bootstrap/css/bootstrap.min.css") %>" rel="stylesheet" />
<script src="<%=ResolveClientUrl("~/assets/bootstrap/js/bootstrap.min.js") %>" type="text/javascript"></script>
<script>
    $.fn.bootstrapBtn = $.fn.button.noConflict();
</script>

<!-- Custom Scritp -->
<script src="<%=ResolveClientUrl("~/js/jquery.number.min.js") %>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>
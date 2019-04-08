﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuPredial.aspx.vb" Inherits="coactivosyp.MenuPredial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Consultor</title>

<%
    If Not Page.IsPostBack Then
        If CType(Request.Browser.Browser, String) = "IE" Then
            Response.Write("<link rel=""stylesheet"" type=""text/css""  href=""IE_Main_Menu.css"" />")
        Else
            Response.Write("<link rel=""stylesheet"" type=""text/css""  href=""Navi_Main_Menu.css"" />")
        End If
    End If
%>

<script src="jquery.tools.min.js" type="text/javascript"></script>
<script type="text/javascript">
function cambiarContenido(texto,id){
  document.getElementById(id).innerHTML = texto;
}

$(function () {  
  $('.opcion a')   
	.append('<span class="hover" id="rsscolor"/>').each(function () {
	  var $span = $('> span.hover', this).css('opacity', 0);
	  $(this).hover(function () {
			$span.stop().fadeTo(700, 1);
	  }, function () {
			$span.stop().fadeTo(800, 0);
	  });
	});
});
</script>



</head>
<body>
<div id="message_box">
     
</div>
<div id="container">
<form id="MenuConsultor" method="post" runat="server">
<%
    If Not Page.IsPostBack Then
        Response.Write("<div class='opcion'><div id='OpPredial'>")
        If Session("OpPredial") = True Then
            Response.Write("<a href=""generador-expedientes.aspx"" target='_parent' class='imagen2' id='a_OpPredial'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                           & "</a><p>Proceso tributario</p></div></div>")
        Else
            Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_OpPredial'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                           & "</a><p>Proceso tributario</p></div></div>")
        End If
        
        Response.Write("<div class='opcion'><div id='opActuaciones'>")
        If Session("opActuaciones") = True Then
            Response.Write("<a href=""actuaciones.aspx"" target='_parent' class='imagen2' id='a_opActuaciones'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                           & "</a><p>Parametrización de actuaciones</p></div></div>")
        Else
            Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_opActuaciones'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                           & "</a><p>Parametrización de actuaciones</p></div></div>")
        End If
    End If
%>
      	<%--<div class="opcion">
			 <div id="OpPredial">
	          <a href="generador-expedientes.aspx" target="_parent" class="imagen2" id="a_OpPredial">
			     <span class="hover" id="rsscolor" style="opacity:0;"></span>
			  </a>
			  <p>Consultar Expedientes</p>
		</div>
		
		</div>
			<div class="opcion">
			 <div id="opActuaciones">
	          <a href="actuaciones.aspx" target="_parent" class="imagen2" id="a_opActuaciones">
			     <span class="hover" id="Span1" style="opacity:0;"></span>
			  </a>
			  <p>Gestión de actuaciones</p>
		    </div>
		</div>--%>
<script type="text/javascript"> 
$(document).ready(function(){
 //Alinear al centro el Objeto Contenedor
   var w = $(this).width();
   var h = $(this).height();
   var con_w = $("#container").width(); 
   var con_h = $("#container").height();
   
   var p = (w/2)-(con_w/2); 
   var t = (h/2)-(con_h/2);
   
   $("#container").css(  
	{  
		   position: 'absolute',  
		   left:  p + 'px',  
		   top:   t + 'px'  
	}); 
});
</script>
</form>		
</div>
</body>
</html>

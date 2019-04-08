<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MenuConsultor.aspx.vb" Inherits="coactivosyp.Consultor" %>
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

<form id="Form1" method="post" runat="server">
<div id="message_box">
     
</div>

<table  border="0" cellspacing="0"  align="center">
<tr>
 <td align="center" valign="middle"  id="container" class="container">
 <%
     If Not Page.IsPostBack Then
         Response.Write("<div class='opcion'><div id='ConExp'>")
         If Session("ConExp") = True Then
             Response.Write("<a href=""consultarentes.aspx"" target='_parent' class='imagen2' id='a_ConExp'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Consultar Expedientes</p></div></div>")
         Else
             Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_ConExp'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Consultar Expedientes</p></div></div>")
         End If
         
         Response.Write("<div class='opcion'><div id='ActExp'>")
         If Session("ActExp") = True Then
             Response.Write("<a href=""subirexpedientes.aspx"" target='_parent' class='imagen2' id='a_ActExp'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Actualizar Expedientes</p></div></div>")
         Else
             Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_ActExp'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Actualizar Expedientes</p></div></div>")
         End If
         
         Response.Write("<div class='opcion'><div id='ConDia'>")
         If Session("ConDia") = True Then
             Response.Write("<a href=""consultardocumentos2.aspx"" target='_parent' class='imagen2' id='a_ConDia'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Consulta Diaria</p></div></div>")
         Else
             Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_ConDia'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Consulta Diaria</p></div></div>")
         End If
         
         '----
         Response.Write("</td></tr><tr><td td align=""center"" valign=""middle"">")
         
         Response.Write("<div class='opcion'><div id='opHistoricoexp'>")
         If Session("opHistoricoexp") = True Then
             Response.Write("<a href=""historiaexpediente.aspx"" target='_parent' class='imagen2' id='a_ConDia'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Historial del Exp.</p></div></div>")
         Else
             Response.Write("<a href=""javascript:alert('No tiene permiso')"" target='_parent' class='imagen2' id='a_ConDia'><span class='hover' id='rsscolor' style='opacity:0;'></span>" _
                            & "</a><p>Historial del Exp.</p></div></div>")
         End If

     End If
 %>	
 </td>
</tr>
</table>
<script type="text/javascript"> 
$(document).ready(function(){
 //Alinear al centro el Objeto Contenedor
   var w = $(this).width();
   var h = $(this).height();
   var con_w = $(".container").width(); 
   var con_h = $(".container").height();
   
   var p = (w/2)-(con_w/2); 
   var t = (h/2)-(con_h/2);
   
   $(".container").css(  
	{  
		   position: 'absolute',  
		   left:  p + 'px',  
		   top:   t + 'px'
	}); 
});
</script>
</form>      
<%--</div>--%>
</body>
</html>

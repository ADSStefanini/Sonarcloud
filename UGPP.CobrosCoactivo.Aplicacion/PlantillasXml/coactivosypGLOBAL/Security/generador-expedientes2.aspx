<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="generador-expedientes2.aspx.vb" Inherits="coactivosyp.generador_expedientes2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Generador de expedientes</title>
		<link rel="stylesheet" href="estilos_2.css" type="text/css">
	</HEAD>
		<frameset cols="180,*" frameborder="no">
			<%
				If Session("mapppredial") = "S"
					Response.Write("<frame src='menu-predial.html' name='outlook' scrolling='no' marginwidth='0' marginheight='0' />")
				Else
					If Session("mappvehic") = "S"
						Response.Write("<frame src='menu-vehic.html' name='outlook' scrolling='no' marginwidth='0' marginheight='0' />")
					Else
						If Session("mappcuotasp") = "S"
							Response.Write("<frame src='menu-cuotasp.html' name='outlook' scrolling='no' marginwidth='0' marginheight='0' />")
						End If
					End If
				End If	
			%>						
			<frameset rows="*,40" frameborder="no">
				<frame src="contenido.html" name="main" scrolling="auto" />
				<frame src="pie-pagina.html" name="copyright" scrolling="auto" marginwidth="0" marginheight="0" />
			</frameset>
		</frameset>
</HTML>

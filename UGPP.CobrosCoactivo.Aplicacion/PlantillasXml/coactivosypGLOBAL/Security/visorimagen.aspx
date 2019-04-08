<%@ Page Language="vb" AutoEventWireup="false" Codebehind="visorimagen.aspx.vb" Inherits="coactivosyp.visorimagen" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>visorimagen</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="css/Objetos.css" rel="stylesheet" type="text/css" />
  
    <style type="text/css">
    .tabla {
    font-family: Verdana, Arial, Helvetica, sans-serif;
    font-size:11px;
    text-align: left;
    font-family: "Trebuchet MS", Arial;
    text-transform: uppercase;
    background-color: #EDEDE9;
    margin:0px;
    }
    .tabla th {
    padding: 2px;
    background-color: #83aec0;
    color: #FFFFFF;
    border-right-width: 1px;
    border-bottom-width: 1px;
    border-right-style: solid;
    border-bottom-style: solid;
    border-right-color: #558FA6;
    border-bottom-color: #558FA6;
    font-weight:bold;
    }
    .tabla td {
    padding: 2px;
    border-right-width: 1px;
    border-bottom-width: 1px;
    border-right-style: solid;
    border-bottom-style: solid;
    border-right-color: #A4C4D0;
    border-bottom-color: #A4C4D0;

    background-color: #EDEDE9;
    color: #34484E;
    }
    </style>
    <script language="JavaScript" type="text/javascript">
		function findDOM(objectId) {
			if (document.getElementById) {
				return (document.getElementById(objectId));}
			if (document.all) {
				return (document.all[objectId]);}
			}
			function zoom(type,imgx,sz) {
				imgd = findDOM(imgx);
			if (type=="+" && imgd.width < 1241) {
				imgd.width += 50;imgd.height += (50*sz);}
			if (type=="-" && imgd.width > 20) {
				imgd.width -= 50;imgd.height -= (50*sz);}
		} 
   </script>
  </head>
  <body MS_POSITIONING="GridLayout">

    <form id="Form1" method="post" runat="server">
		<% 
			Dim mArchivo, mNomEnte, mActo, mPaginas, mFolder, mFolderCobrador, mIdEnte, mIdActo	
			Dim ArchivoBase As String = ""			
			Dim X, Y As Integer
			Dim ArchivoX As String
			mArchivo = Request.QueryString("F")
			mArchivo = mArchivo.ToLower()
			mNomEnte = Request.QueryString("nomente")
			mIdEnte  = Request.QueryString("idente")
			mIdActo  = Request.QueryString("idacto")
		    mActo = Request.QueryString("acto")
		    
		    'numero de paginas para luego buscar
			mPaginas = Request.QueryString("totimg")
			mFolder  = Request.QueryString("folder")
			mFolderCobrador = Session("mcobrador")
			ArchivoBase = mArchivo.Substring(0,mArchivo.IndexOf("-"))			
			'----------------------------------
			Response.Write("<table class=""tabla"">")
			Response.Write("<tr><th colspan=""2"" style=""text-align: center;"">VISOR DE IMAGENES</th></tr>")
			Response.Write("<tr>")
			Response.Write("<th>ENTIDAD : </th><td>" & mNomEnte & " (" & mIdEnte & ")</td>")
			Response.Write("</tr>")
			Response.Write("<tr>")
			Response.Write("<th>ACTUACION : </th><td>" & mActo & "</td>")
			Response.Write("</tr>")
			Response.Write("<tr>")
			Response.Write("<th>NOMBRE DEL ARCHIVO : </th><td>" & mArchivo & "</td>")
			Response.Write("</tr>")
			Response.Write("<tr>")
			Response.Write("<th>NUMERO DE PAGINA(S) : </th><td>" & mPaginas & "</td>")
			Response.Write("</tr>")
			Response.Write("</table>")
			
			Response.Write("<br />")
			
			response.Write("<div class=""buttons"" align=""center"">")
		    Response.Write("<a href='consultarentes.aspx?ente=" & mIdEnte & "&nombente=" & mNomEnte & "'  class='ss'>Para regresar al consultar de expedientes hacer click aqui</a>")
			Response.Write("</div><br /><br />")
			
			'Enlaces para aumentar y reducir
			Response.Write("<div align=""center"">")
			Response.Write("<a href=""#"" onclick=""zoom('-','myimg',92/66)""><img src=""images/lupa2.jpg"" alt=""Reducir imagen"" style=""border:none"" /></a> |	<a href=""#"" onclick=""zoom('+','myimg',92/66)""><img src=""images/lupa1.jpg"" alt=""Ampliar imagen"" style=""border:none"" /></a>")
			Response.Write("</div>")
			
			
			'Generar la linea de enlaces de paginas (Superior)
			'--------------------------------------			
			X = 1
			Y = 0
			Do While X <= mPaginas Or Y = 1000 'El Y = 1000 es para que no se quede en un ciclo infinito (por si acaso)
				Y = Y + 1
				ArchivoX = ArchivoBase & "-"  & Y & ".jpg" 
				
				' Comprobar si la imagen existe en el servidor
				Dim ArchivoaBuscar As String
				ArchivoaBuscar = Server.MapPath("") & "\expedientes\" & Session("mcobrador") & "\" & mIdEnte & "\" & ArchivoX

				'El archivo existe
				If System.IO.File.Exists(ArchivoaBuscar) = True Then					
					'Crear el link
					Response.Write("<a href=""visorimagen.aspx?nomente=" & mNomEnte & "&idente=" & mIdEnte & "&idacto=" & mIdActo & "&F=" & ArchivoX & "&totimg=" & mPaginas & "&acto=" & mActo & "&folder=" & mFolder & """>" & X & "</a>")
					
					'Si el enlace es del archivo actual crear link de eliminar
					If ArchivoX = mArchivo
						'Validar permisos
						If Session("mnivelacces") = "1" Then
							Response.Write("<a href=""eliminarimagen.aspx?idente=" & mIdEnte & "&idacto=" & mIdActo & "&totimg=" & mPaginas & "&nomarchivo=" & mArchivo & """>" & "(Eliminar)" & "</a>")
						End If
					End If 				
					Response.Write("&nbsp;")
					
					'Incrementar el indice
					X = X + 1
				End If
			Loop 
			Response.Write("<br>")			
			'Imagen
			'---------------------------------------
			Response.Write("<img id=""myimg"" src=""" & "expedientes/" & mFolderCobrador & "/" & mFolder & "/" & mArchivo & """ />")
			Response.Write("<br>")
			
			'Generar la linea de enlaces de paginas (Inferior)
			'--------------------------------------
			'For X = 1 To mPaginas
			'	ArchivoX = ArchivoBase & "-"  & X & ".jpg" 
			'	Response.Write("<a href=""visorimagen.aspx?nomente=" & mNomEnte & "&F=" & ArchivoX & "&totimg=" & mPaginas & "&acto=" & mActo & "&folder=" & mFolder & """>" & X & "</a>")				
			'	If ArchivoX = mArchivo
			'		Response.Write(" (Eliminar) ")
			'	End If
			'	Response.Write("&nbsp;")
			'Next 
		%>
    </form>

  </body>
</html>

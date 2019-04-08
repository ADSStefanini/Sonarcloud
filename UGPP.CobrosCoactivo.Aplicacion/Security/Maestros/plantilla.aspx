<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="plantilla.aspx.vb" Inherits="coactivosyp.plantilla" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../EstiloPPal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tituloCap
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 21px;
            text-transform: uppercase;
            color: #FFFFFF;
            font-weight: 900;
        }
        
        .CajaDialogo
        {
            /*background-image: url(images/icons/MSNClose.png);
            background-repeat: no-repeat;*/
            position:absolute;
            padding:10px;
            background-color:#f0f0f0;
            border-width: 7px;
            border-style: solid;
            border-color: #72A3F8;
            color:#000;
            z-index:101;
       }
       .eliminar
       {
            
            font-size:10px;
            position:absolute;
            padding:10px 5px 5px 5px;
            background-color:#72A3F8;
            top: 355px;
            left: 350px;
            z-index:1;
        }
        .eliminar a
        {
         text-decoration:none;
         color:#B40404;
        }
       .CajaDialogo .inf
       {
            font-weight: bold;
            font-size:12px;
            font-style: italic;
       }
    </style>
    <script src="../js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function fechashow() {
            document.getElementById('txtDescripcion').focus();
        }

        $(document).ready(function() {
            $(window).scroll(function() {
                $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
            });
        });
    </script>
</head>
<body bgcolor="#01557c"  style="margin:0 0 0 0" >
    <div id="message_box">
    <ul style="width:36px; height:188px">
     <li style="height:36px;width:36px !important;">
        <a href="../MenuMaestros.aspx"><img alt="" src="../../imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
     </li>
     <li style="height:152px;width:36px;">
        <a href="../MenuMaestros.aspx"><img alt="" src="../../imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
     </li>
    </ul>
   </div>
		<form id="Form1" method="post" runat="server">
			<table height="1300px" width="100%" border="0">
				<tr>
					<td width="50%"></td>
					<td background="images/bg_izdo.jpg"><img src="images/bg_izdo.jpg" width="32"></td>
					<td vAlign="top" width="780" bgColor="#618ce4" height="125%">
						<!-- Tabla del centro del diseño -->
						<table height="100%" cellSpacing="0" cellPadding="0" width="780" border="0">
							<!-- segunda fila de la tabla central tiene una sola celda (resultados_busca.jpg)-->
							<tr>
								<td width="780" background="images/resultados_busca.jpg" height="42">
                                	<font style="FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #ffffff; FONT-FAMILY: verdana">&nbsp; 
										<div id="titulo" name="titulo" runat="server" style="width:700px; "></div>
									</font>
								</td>
							</tr>
							<!-- tercera fila de la tabla central tiene una sola celda (linea_azul2.jpg)-->
							<tr>
								<td vAlign="top" width="780">
									<!-- contenido -->
									<div id="contenido" name="contenido" runat="server" style="width:780px; height:900px; margin-left:20px "></div>									
								</td>
							</tr>
							<!-- fin de la tabla central --></table>
					</td>
					<td background="images/bg_dcho.jpg"><img src="images/bg_dcho.jpg" width="32"></td>
					<td width="50%"></td>
				</tr>
			</table>
		</form>
	</body>
</html>

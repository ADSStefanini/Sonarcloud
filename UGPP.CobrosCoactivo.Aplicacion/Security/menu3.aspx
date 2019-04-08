<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="menu3.aspx.vb" Inherits="coactivosyp.menu3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
    <title>Menú principal de opciones del sistema</title>
	<script src="jquery.tools.min.js" type="text/javascript"></script>	
	
	<link rel="stylesheet" type="text/css" href="scrollable-navig.css" /> 
	<link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
	<link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" /> 
    <style type="text/css"> 
	/* main vertical scroll (desplazamiento vertical principal) */
	#main {
		position:relative;
		overflow:hidden;
		height: 450px;
	}
	
	/* root element for pages (elemento raíz de las páginas) */
	#pages {
		position:absolute;
		height:20000em;
	}
	
	/* single page (cuadros contenedores) */ 
	.page {
		padding:10px;
		height: 450px;
		background:#2e5eb3 url(Menu_PPal/h600.png) 0 0 repeat-x;
		width:695px;
	}
	
	/* root element for horizontal scrollables (elemento de raíz para scrollables horizontal) */
	.scrollable {
		position:relative;
		overflow:hidden;
		width: 685px;
		height: 450px;
	}
	
	/* root element for scrollable items (elemento raíz de los artículos de desplazamiento) */
	.scrollable .items {
		
		position:absolute;
		clear:both;
            top: 0px;
            left: 0px;
        }
	
	/* single scrollable item (elemento desplazable) */
	.item {
		float:left;
		cursor:pointer;
		width:640px;
		height:420px;
		padding:10px;
	}
	
	/* main navigator (navegador principal) */
	#main_navi {
		float:left;
		padding:0px !important;
		margin:0px !important;
	}
	
	/* opciones del menu  */
	#main_navi li {
		background-color:#333;
		
		border-bottom:1px solid #2e5eb3;
		clear:both;
		color:#FFFFFF;
		font-size:13px;
		height:75px;
		list-style-type:none;
		padding:10px;
		width:85px; /* tamaño de la opcion */
		cursor:pointer;
		font-family:Verdana, Geneva, sans-serif;
	}
	
	#main_navi li:hover {
		background-color:#444;
	}
	
	#main_navi li.active {
		background-color:#2e5eb3;
		background:#2e5eb3 url(Menu_PPal/li.png) 0 0 repeat-x;
	}
		
	/* pestañas o navegador derecha - izquierda */
	#main div.navi {
		margin-left:300px;
		cursor:pointer;
	}
	</style> 
	
	<style type="text/css"> 
        #main_navi {
           /* margin-left:-45px; */
        }
        #main_navi li {
	        list-style-image:none !important;
	        margin-top:0 !important;
        }
    </style>
    <script type="text/javascript">
        function OpcionMenu(Option)
        {
            if (Option==0)
            {
                document.getElementById('tab_menu').innerHTML = 'CONSULTA DE EXPEDIENTE';
            }
            else if (Option==1)
            {
                document.getElementById('tab_menu').innerHTML = 'PROCESO TRIBUTARIO';
            }
            else if (Option==2)
            {
                document.getElementById('tab_menu').innerHTML = 'ADMINISTRACIÓN DE USUARIOS';
            }
        }
    </script>
</head> 
<body>

 <div id="container">
    <h1 id="Titulo"><a href="#">Menú principal de opciones del sistema</a></h1>

<!-- main navigator --> 
<ul id="main_navi"> 
 
	<li class="active" onclick="OpcionMenu(0);"> 
		<img alt="" src="Menu_PPal/consultar.png" /> 
		<!-- <strong>Consultor de expediente.</strong>  -->
		   
	</li> 
	<li onclick="OpcionMenu(1);"> 
		<img alt="" src="Menu_PPal/gen-expedientes.png" /> 
		<!-- <strong>Cobro coactivo.</strong> -->
		
	</li> 
	<li onclick="OpcionMenu(2);"> 
		<img alt="" src="Menu_PPal/PPalusuarios.png" /> 
		<!-- <strong>Gestion de usuarios.</strong> -->
	</li> 
</ul> 
 
<!-- root element for the main scrollable --> 
<div id="main"> 
 	<!-- root element for pages --> 
	<div id="pages"> 
 		<!-- page #1 --> 
		<div class="page"> 
 			<!-- sub navigator #1 --> 
			<div class="navi"></div> 
 			<!-- inner scrollable #1 --> 
			<div class="scrollable"> 
 				<!-- root element for scrollable items --> 
				<div class="items"> 
 					<!-- items  --> 
					<div class="item"> 
						<!--<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#fdfdfd;"> </div> -->
						<iframe src="MenuConsultor.aspx" width="630p" height="390" frameborder="1" > </iframe>
					</div> 
					<!--
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#E0F8F7;"> </div> 
					</div> 
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#FBEFEF;"> </div>
					</div> 
                    <div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#A9D0F5;"> </div>
					</div> 
					-->
				</div> 
 			</div> 
 		</div> 
 
		<!-- page #2 --> 
		<div class="page"> 
 
			<div class="navi"></div> 
 			<!-- inner scrollable #2 --> 
			<div class="scrollable"> 
 				<!-- root element for scrollable items --> 
				<div class="items"> 
 					<!-- items on the second page --> 
					<div class="item"> 
						<iframe src="MenuPredial.aspx" width="630p" height="390" frameborder="1"> </iframe>
					</div> 
					
					<!--
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#1295f1;"> </div>
					</div> 
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#1295f1;"> </div>
					</div> 
					-->
					
 				</div> 
			</div> 
		</div> 
 		<!-- page #3 --> 
		<div class="page"> 
 			<div class="navi"></div> 
 			<!-- inner scrollable #3 --> 
			<div class="scrollable"> 
 				<!-- root element for scrollable items --> 
				<div class="items"> 
 					<!-- items on the first page --> 
					<div class="item"> 
						<iframe src="MenuUsuarios.aspx" width="630p" height="390" frameborder="1"> </iframe> 
					</div> 
					
					<!--
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#1295f1;"> </div>
					</div> 
					<div class="item"> 
						<div style=" POSITION: absolute;width: 630px;height: 390px;background-color:#1295f1;"> </div> 
					</div>
                    -->					
 				</div> 
 			</div> 
 		</div> 
 	</div> 
 </div> 
 <div style=" border: 2px none #FFFFFF; padding: 5px; text-align: center; color: #FFFFFF; font-weight: 800; font-size: 20px; background-color: #3263B9; font-family: Arial, Helvetica, sans-serif; float: right; display: block; cursor: pointer; width: 665px;">
    <div id="tab_menu">CONSULTA DE EXPEDIENTE</div>
</div>
</div>

<script type="text/javascript"> 
// What is $(document).ready ? See: http://flowplayer.org/tools/documentation/basics.html#document_ready
$(document).ready(function() {
 // main vertical scroll
$("#main").scrollable({
 
	// basic settings
	vertical: true,
 
	// up/down keys will always control this scrollable
	keyboard: 'static',
 
	// assign left/right keys to the actively viewed scrollable
	onSeek: function(event, i) {
		horizontal.eq(i).data("scrollable").focus();
	}
 
// main navigator (thumbnail images)
}).navigator("#main_navi");
 
// horizontal scrollables. each one is circular and has its own navigator instance
var horizontal = $(".scrollable").scrollable({ circular: true }).navigator(".navi");
 
 
// when page loads setup keyboard focus on the first horzontal scrollable
horizontal.eq(0).data("scrollable").focus();
 
});
</script> 


</body>
</html>
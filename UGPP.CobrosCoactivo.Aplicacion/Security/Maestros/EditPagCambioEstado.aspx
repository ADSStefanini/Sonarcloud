<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPagCambioEstado.aspx.vb" Inherits="coactivosyp.EditPagCambioEstado" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Editar solicitudes de cambio de estado</title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />        
                
        <style type="text/css">
	        body{ background-color:#01557C;}		
		    * { font-size:12px; font-family:Arial;}
            td { padding:2px;} 	
            #encabezado { background:url(images/resultados_busca.jpg); height:37px; width:100%; border:solid 1px #a6c9e2; padding-bottom:5px}  
			#tituloencabezado { color:White; margin-top:16px; margin-left:2px; font: 12px Verdana; font-weight:bold; }  
			#infoexpediente { background-color:#FFFFFF; width:100%; margin-bottom:10px; margin-top:10px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif; font-weight: bold; font-size: 11px;  -moz-border-radius: 3px; border-radius: 3px; -webkit-border-radius: 3px;  }  
		    
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            
            <!---------------------- -->
             <div id="encabezado">
                <div id="tituloencabezado">
					<%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>                        
                    </div>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                    </div>
                    
				</div>                
            </div>
            <!---------------------- -->
            
            <div id="infoexpediente" style="padding-top:8px; padding-bottom:8px; width:100%; display:inline-table ">
                   <iframe src="SOLICITUDES_CAMBIOESTADO.aspx?pExpediente=<%  Response.Write(Request("pExpediente"))%>" width="960" height="740" scrolling="no" frameborder="0"></iframe>            
            </div> 
            
            
        </form>
    </body>
</html>

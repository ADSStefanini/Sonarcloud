<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="actualizarfechas.aspx.vb" Inherits="coactivosyp.actualizarfechas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
		<title>Consulta de documentos digitalizados de los entes</title>
		<link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
	    <style type="text/css">
            #txtFechaRad
            {
                top: 209px;
                left: 205px;
                position: absolute;
                width: 139px;
            }
            #txtEnte
            {
                top: 177px;
                left: 205px;
                position: absolute;
                width: 499px;
            }
        </style>
        <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
	</head>
	<body bgColor="#01557c" leftMargin="0" topMargin="0" onload="document.forms.Form1.txtEnte.focus()"
		marginheight="0" marginwidth="0">
		
		<div id="container">
            <h1 id="Titulo"><a href="#">Subida de expedientes al servidor y registro de los archivos en la base de datos</a></h1>
		
		<form id="Form1" method="post" runat="server">
            <asp:HyperLink id="HyperLink5" runat="server" ForeColor="White" 
            Font-Names="Verdana" Font-Size="X-Small" style="position:absolute;top:87px; left:347px;"
											            NavigateUrl="subirexpedientes.aspx">Actualizar expedientes</asp:HyperLink>

            <asp:HyperLink id="HyperLink6" runat="server" ForeColor="White" 
            Font-Names="Verdana" Font-Size="X-Small" style="position:absolute;top:87px; left:665px;"
											            NavigateUrl="consultardocumentos2.aspx">Consulta diaria</asp:HyperLink>	
            											
            <asp:HyperLink id="HyperLink4" runat="server" ForeColor="White" 
            Font-Names="Verdana" Font-Size="X-Small" style="position:absolute;top:87px; left:29px;"
											            NavigateUrl="consultarentes.aspx">Consultar expedientes</asp:HyperLink>
			
			<asp:label id="Label13" runat="server" ForeColor="White" Font-Names="Arial" 
                Font-Size="X-Small" 
                style="position:absolute; top: 213px; left: 28px; font-size: 12px;">Fecha 
            de radicación :</asp:label>
			
			<asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" 
                Font-Size="X-Small" 
                style="position:absolute; top: 179px; left: 29px; font-size: 12px;">Deaudor 
            :</asp:label>
			<asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" 
                Font-Size="X-Small" style="position:absolute; top: 251px; left: 30px;">Por favor seleccione la fecha de radicación</asp:label>
			
			<asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtFechaRad"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
            >
			</asp:CalendarExtender> 
											
			<asp:TextBox ID="txtFechaRad" runat="server" 
                        style="top:208px; left:203px; position: absolute;width: 152px;" 
                CssClass="CalendarioBox"></asp:TextBox>
			
			<input id="txtEnte" type="text" name="txtEnte" runat="server" readonly />
			
			<asp:customvalidator id="Validator" runat="server" ForeColor="Yellow" 
                Font-Names="Tahoma" Font-Size="X-Small"
				ErrorMessage="CustomValidator" Font-Bold="True" 
                
                
                style="top: 277px; left: 30px; position: absolute; height: 17px; font-size: 12px; width: 706px;">
            </asp:customvalidator>
            
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
                EnableScriptGlobalization="True">
            </asp:ToolkitScriptManager>    
            
														
            <asp:button id="Button1" runat="server" Text="Regresar al proceso" 
               
                
                style="top: 326px; left: 386px; position: absolute;width: 162px; bottom: 649px; background-image: url('images/icons/user_business.png');" 
                CssClass="Botones"></asp:button>
			
			<asp:button id="Button2" runat="server" Text="Actualizar fecha" 
                style="top: 326px; left: 222px; position: absolute;width: 149px;background-image: url('images/icons/46.png');" 
                CssClass="Botones"></asp:button>
                
														
		</form>
	</div>		
	</body>
</head>

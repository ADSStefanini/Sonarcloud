<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ver_cambios_estado.aspx.vb" Inherits="coactivosyp.ver_cambios_estado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Ver cambios de estado</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />      

        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <style type="text/css">
	        body{ background-color:#01557C}
		    * { font-size:12px; font-family:Arial;}
		    td { padding:2px;}	
		    .BoundFieldItemStyleHidden { display:none;}	
		    .BoundFieldHeaderStyleHidden {display:none;}
	    </style>
	    
    </head>
    
    <body>
        <form id="Form1" method="post" runat="server">
            <!-- Tabla para mostrar los cambios de estado -->
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td colspan="10" background="images/resultados_busca.jpg" height="42">
                        <div style="color:White; font-weight:bold; width:550px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>                                                
                        
                        <div style="color:White; width:120px; height:20px; float:right; text-align:right; padding-right:10px;">
                            <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                            <span>Cerrar sesión</span>
                        </div>
                        
                        <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                            <asp:LinkButton ID="ABack" runat="server" ToolTip="Regresar al listado de expedientes">
                            <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                         <!-- Grid  -->
                         <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                         
                         <asp:GridView ID="grdCambiosEstado" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                            <Columns>                                            
                                <asp:BoundField DataField="USUARIOSrepartidornombre" HeaderText="Funcionario que realiza el reparto"></asp:BoundField>
                                <asp:BoundField DataField="USUARIOSabogadonombre" HeaderText="Gestor responsable"></asp:BoundField>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha de reparto" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="ESTADOS_PROCESOestadonombre" HeaderText="Estado procesal"></asp:BoundField>
                                <asp:BoundField DataField="ESTADOS_PAGOestadopagonombre" HeaderText="Estado del pago"></asp:BoundField>                                
                            </Columns>
                            <HeaderStyle CssClass="ui-widget-header"  />
                            <RowStyle CssClass="ui-widget-content" />
                            <AlternatingRowStyle/>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
                        
        </form>
    </body>
    
</html>

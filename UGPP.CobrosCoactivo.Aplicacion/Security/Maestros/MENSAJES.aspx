<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MENSAJES.aspx.vb" Inherits="coactivosyp.MENSAJES" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head>
        <title>MENSAJES</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />

        <script type="text/javascript">
            $(function() {
                EndRequestHandler();
            });
            function EndRequestHandler() {
                $('#cmdAddNew').button();
                $('#cmdFirst').button();
                $('#cmdNext').button();
                $('#cmdLast').button();
                $('#cmdPrevious').button();
                $('.GridEditButton').button();

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
        </script>
        <style type="text/css">
		    * { font-size:12px; font-family: Arial;}
		    body{ background-color:#01557C}
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden {display:none; }
		    td { padding-left: 10px; padding-right: 10px; }
	    </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                <td colspan="10" background="images/resultados_busca.jpg" height="42">
                    <div style="color:White; font-weight:bold; width:450px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                    <div style="color:White; width:150px; height:20px; float:right; text-align:right">
                        <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                        <span id="spancerrarsesion" runat="server">Cerrar sesión&nbsp&nbsp</span>
                    </div>
                    
                    <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                            <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" />
                        </asp:LinkButton>
                    </div>
                </td>
            </tr>
                <tr>
                    <td align="left">
                        <!--  -->
                        <table id="tblEditEJEFISGLOBAL" class="ui-widget-content">		            
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="ui-widget-header">
                                    Leer mensajes 
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" id="cboTipoMensaje" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </td>
                                <td>
                                </td>                                    
                                <td style="width:360px; text-align: right;">
                                    <asp:Button id="cmdAddNew" runat="server" Text="Crear mensaje" cssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan = "5">
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
                            <Columns>
                                <asp:BoundField DataField="idunico" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="USUARIOSUsuOrigennombre" HeaderText="Remitente" SortExpression="USUARIOSUsuOrigen.nombre"></asp:BoundField>
                                <asp:BoundField DataField="USUARIOSUsuDestinonombre" HeaderText="Destinatario" SortExpression="USUARIOSUsuDestino.nombre"></asp:BoundField>
                                <asp:BoundField DataField="FecEnvio" HeaderText="Fecha y hora de envío" SortExpression="MENSAJES.FecEnvio"></asp:BoundField>
                                <asp:BoundField DataField="FecRecibo" HeaderText="Fecha y hora de recibo" SortExpression="MENSAJES.FecRecibo"></asp:BoundField>
                                <asp:BoundField DataField="leido" HeaderText="Leído" SortExpression="MENSAJES.leido"></asp:BoundField>
                                <asp:ButtonField ButtonType="Button" Text="Leer">
                                    <ControlStyle CssClass="GridEditButton" />
                                </asp:ButtonField>
                            </Columns>
                            <HeaderStyle CssClass="ui-widget-header"  />
                            <RowStyle CssClass="ui-widget-content" />
                            <AlternatingRowStyle/>
                        </asp:GridView>   
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button id="cmdFirst" runat="server" Text="Primero" cssClass="PCGButton"></asp:Button>
                        <asp:Button id="cmdPrevious" runat="server" Text="Anterior" cssClass="PCGButton"></asp:Button>
                        <asp:Label ID="lblPageNumber" runat="server"></asp:Label>&nbsp;&nbsp;
                        <asp:Button id="cmdNext" runat="server" Text="Siguiente" cssClass="PCGButton"></asp:Button>
                        <asp:Button id="cmdLast" runat="server" Text="Ultimo" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>

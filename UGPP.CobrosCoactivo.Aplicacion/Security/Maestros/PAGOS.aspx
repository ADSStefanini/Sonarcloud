<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PAGOS.aspx.vb" Inherits="coactivosyp.PAGOS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head>
        <title>PAGOS</title>
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
                $('#cmdAdicionar').button();
                $('#cmdAddNew').button();
                $('#cmdSearch').button();
                $('#cmdFirst').button();
                $('#cmdNext').button();
                $('#cmdLast').button();
                $('#cmdPrevious').button();
                $('#cmdVerCambiosEstado').button();                
                
                $('.GridEditButton').button();

                //Controles de dolo lectura
                $(".SoloLectura").keypress(function(event) { event.preventDefault(); });
                $(".SoloLectura").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $('#lnkNumExpVencer').click(function() {
                    window.open('EstadisticaxVencer.aspx', 'Estadistica de expedientes por vencer', 'width=600,height=250');
                    return false;
                });

                $('#lnkNumExpVencidos').click(function() {
                    window.open('EstadisticaVencidos.aspx', 'Estadistica de expedientes vencidos', 'width=600,height=250');
                    return false;
                });

                $('#lnkMsjNoLeidos').click(function() {
                    window.open('MENSAJES.aspx', 'Visor de mensajes', 'width=780,height=450');
                    return false;
                });

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
        </script>
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
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td colspan="10" background="images/resultados_busca.jpg" height="42">
                        <div style="color:White; font-weight:bold; width:550px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>                                                
                        
                        <div style="color:White; width:120px; height:20px; float:right; text-align:right; padding-right:10px;">
                            <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                            <span>Cerrar sesión</span>
                        </div>
                        
                        <!-- Subir SQL -->                          
                        <div style="color:White; width:50px; height:20px; float:right; text-align:right">
                            <asp:LinkButton ID="lnkSql" runat="server" ToolTip="Subir sql"><img alt ="" src="../images/icons/sql.png" height="16" width="16" style=" vertical-align:middle" /></asp:LinkButton>
                        </div>
                        
                        <div style="color:White; width:130px; height:20px; float:right; text-align:right; padding-right:10px;">
                            <asp:LinkButton ID="lbtsubirpagos" runat="server" ToolTip="Subir Pagos" Text="Subir Pagos" style="padding-right:20px;" >
                                <img alt ="Subir Pagos"  src="../images/icons/dollar.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Subir Pagos" />
                                </asp:LinkButton>
                            <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Mensajes">
                                <img alt ="Mensajes"  src="../images/icons/comentarios.png" height="18" width="18" style=" vertical-align:middle" id="img3" title="Mensajes" />
                            </asp:LinkButton>
                            <span><%  Response.Write("(" & Session("ssNumMsgNoLeidos") & ")")%>&nbsp&nbsp</span>
                        </div>
                        
                        <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                            <asp:LinkButton ID="ABack" runat="server" ToolTip="Regresar al listado de expedientes">
                            <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td align="right">
                                    <table style="width: 100%">
                                        <tr>
                            <td class="ui-widget-header">
                                No. Consignación / planilla
                            </td>
                            <td>
                            <asp:TextBox ID="txtSearchNroConsignacion" runat="server" ></asp:TextBox></td>
                        <td class="ui-widget-header">
                            Estado
                        </td>
                        <td>
                        <asp:DropDownList ID="cboSearchestado" runat="server" AppendDataBoundItems="true"><asp:ListItem Text="" Value=""></asp:ListItem></asp:DropDownList></td>
                                            <td>                                                
                                                <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                            </td>
                                    </tr>
                                        <tr>
                    <td class="ui-widget-header">
                        Gestor solicitante
                    </td>
                    <td>
                    <asp:DropDownList ID="cboSearchUserSolicita" runat="server" AppendDataBoundItems="true"><asp:ListItem Text="" Value=""></asp:ListItem></asp:DropDownList></td>
                                            <td class="ui-widget-header">Expediente</td>
                                            <td><asp:TextBox ID="txtSearchExpediente" runat="server" CssClass="SoloLectura"></asp:TextBox></td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td><asp:Button id="cmdAdicionar" runat="server" Text="Adicionar"></asp:Button></td>
                                                        <td><asp:Button id="cmdVerCambiosEstado" runat="server" Text="Ver cambios estado"></asp:Button></td>
                                                    </tr>
                                                </table>                                                
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
                                            <asp:BoundField DataField="NroConsignacion" >
                                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NroConsignacion" HeaderText="No. Consignación / planilla" SortExpression="PAGOSNOMBRE.NroConsignacion"></asp:BoundField>
                                            <asp:BoundField DataField="NroExp" HeaderText="No. Expediente" SortExpression="PAGOSNOMBRE.NroExp"></asp:BoundField>
                                            <asp:BoundField DataField="FecSolverif" HeaderText="Fecha reporte/Solicitud" SortExpression="PAGOSNOMBRE.FecSolverif" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="FecVerificado" HeaderText="Fecha verificación" SortExpression="PAGOSNOMBRE.FecVerificado" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="NombreEstadoPago" HeaderText="Estado del pago" SortExpression="PAGOSNOMBRE.NombreEstadoPago"></asp:BoundField>
                                            <asp:BoundField DataField="NombreUsuario" HeaderText="Gestor que reporta/solicita" SortExpression="PAGOSNOMBRE.NombreUsuario"></asp:BoundField>
                                            <asp:BoundField DataField="pagFecha" HeaderText="Fecha del pago" SortExpression="PAGOSNOMBRE.pagFecha" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="pagFechaDeudor" HeaderText="Fec. pago deudor" SortExpression="PAGOSNOMBRE.pagFechaDeudor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            
                                            <asp:BoundField DataField="pagCapital" HeaderText="Cap. pagado" DataFormatString="{0:N0}"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                            <asp:BoundField DataField="pagInteres" HeaderText="Int. pagado" DataFormatString="{0:N0}"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                            <asp:BoundField DataField="pagAjusteDec1406" HeaderText="Aju. 1406" DataFormatString="{0:N0}"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>                                            
                                            
                                            <asp:BoundField DataField="pagTotal" HeaderText="Total pagado" DataFormatString="{0:N0}" SortExpression="PAGOSNOMBRE.pagTotal"><ItemStyle HorizontalAlign="Right" /></asp:BoundField>
                                            <asp:BoundField DataField="NombreEstadoProc" HeaderText="Estado proc fec reporte" SortExpression="PAGOSNOMBRE.NombreEstadoProc"></asp:BoundField>
                                            <asp:ButtonField ButtonType="Button" Text="Editar">
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

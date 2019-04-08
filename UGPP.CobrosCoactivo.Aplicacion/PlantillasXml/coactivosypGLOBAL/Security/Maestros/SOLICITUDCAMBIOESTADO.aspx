<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SOLICITUDCAMBIOESTADO.aspx.vb" Inherits="coactivosyp.SOLICITUDCAMBIOESTADO" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
    <head>
        <title>SOLICITUDCAMBIOESTADO</title>
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
                $('#cmdSearch').button();
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
		    body{ background-color:#01557C}
		    * { font-size:12px; font-family:Arial;}
		    td { padding:2px;}
		    .BoundFieldItemStyleHidden { display: none;}
		    .BoundFieldHeaderStyleHidden {display: none;}
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
                        <span>Cerrar sesión&nbsp&nbsp</span>
                    </div>
                    
                    <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:0px;">
                        <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al menú principal">
                        <img alt ="Regresar al menú principal"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al menú principal" /></asp:LinkButton>
                    </div>
                    
                </td>
            </tr>
            <tr>                
                <td align="right">
                        <table style="width: 100%">
                            <tr>
                        <td class="ui-widget-header">
                        Nro. Expediente</td>
                        <td>
                        <asp:TextBox ID="txtSearchNroExp" runat="server" ></asp:TextBox></td>
                    <td class="ui-widget-header">
                    Gestor</td>
                    <td>
                    <asp:TextBox ID="txtSearchgestor" runat="server" ></asp:TextBox></td>
                                <td>                                        
                                    <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                </td>
                        </tr>
                            <tr>
                <td class="ui-widget-header">
                Estado actual</td>
                <td>
                <asp:TextBox ID="txtSearchestadoactual" runat="server" ></asp:TextBox></td>
            <td class="ui-widget-header">
            Estado solicitado</td>
            <td>
            <asp:TextBox ID="txtSearchestadosolicitado" runat="server" ></asp:TextBox></td>
                                <td></td>
                        </tr>
                            <tr>
        <td class="ui-widget-header">
        Acción</td>
        <td>
        <asp:TextBox ID="txtSearchaccion" runat="server" ></asp:TextBox></td>
                                <td>&nbsp;</td><td>&nbsp;</td>
                                <td>&nbsp;</td>
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
                                <asp:BoundField DataField="NroExp" HeaderText="Nro Exp." SortExpression="SOLICITUDCAMBIOESTADO.NroExp"></asp:BoundField>
                                <asp:BoundField DataField="gestor" HeaderText="Gestor" SortExpression="SOLICITUDCAMBIOESTADO.gestor"></asp:BoundField>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="SOLICITUDCAMBIOESTADO.fecha" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="estadoactual" HeaderText="Etapa actual" SortExpression="SOLICITUDCAMBIOESTADO.estadoactual"></asp:BoundField>
                                <asp:BoundField DataField="estadosolicitado" HeaderText="Etapa solicitada" SortExpression="SOLICITUDCAMBIOESTADO.estadosolicitado"></asp:BoundField>
                                <asp:BoundField DataField="accion" HeaderText="Estado solicitud" SortExpression="SOLICITUDCAMBIOESTADO.accion"></asp:BoundField>
                                <asp:BoundField DataField="nomRevisor" HeaderText="Revisor" SortExpression="SOLICITUDCAMBIOESTADO.nomRevisor"></asp:BoundField>
                                <asp:BoundField DataField="aprob_revisor" HeaderText="Aprob. Rev." SortExpression="SOLICITUDCAMBIOESTADO.aprob_revisor"></asp:BoundField>
                                <asp:BoundField DataField="fecha_aprob_revisor" HeaderText="Fecha aprob. Rev." SortExpression="SOLICITUDCAMBIOESTADO.fecha_aprob_revisor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="nomSupervisor" HeaderText="Supervisor" SortExpression="SOLICITUDCAMBIOESTADO.nomSupervisor"></asp:BoundField>
                                <asp:BoundField DataField="aprob_ejecutor" HeaderText="Aprob. Sup." SortExpression="SOLICITUDCAMBIOESTADO.aprob_ejecutor"></asp:BoundField>
                                <asp:BoundField DataField="fecha_aprob_ejecutor" HeaderText="Fecha aprob. Sup." SortExpression="SOLICITUDCAMBIOESTADO.fecha_aprob_ejecutor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
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
            </table>
        </form>
    </body>
</html>


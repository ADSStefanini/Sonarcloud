<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MAESTRO_TITULOS.aspx.vb" Inherits="coactivosyp.MAESTRO_TITULOS" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
    <head>
        <title>Maestro de títulos ejecutivos</title>
        <meta content="JavaScript" name="vs_defaultClientScript" />
        <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>        
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
           <script type="text/javascript">
            $(function() {
                //window.scrollTo(0, 0);
                //scrollTop: '0px';
                //$("html, body").animate({ scrollTop: 0 }, 600);
            
                EndRequestHandler();
            });
            function EndRequestHandler() {
                $('#cmdAddNew').button();
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
		    * { font-size:12px; font-family:Arial; }
		    th { padding-left:8px; padding-right:8px;}
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden { display:none; }
	    </style>
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
            <%--<a id="arriba" runat="server"></a>--%>
            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td align="left">
                        <asp:Button id="cmdAddNew" runat="server" Text="Adicionar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>&nbsp;&nbsp;
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                            <Columns>
                                <asp:BoundField DataField="MT_nro_titulo" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
                                <asp:BoundField DataField="MT_nro_titulo" HeaderText="No. Título"></asp:BoundField>
                                <asp:BoundField DataField="TIPOS_TITULOMT_tipo_titulonombre" HeaderText="Tipo"></asp:BoundField>
                                <asp:BoundField DataField="MT_fec_expedicion_titulo" HeaderText="Fecha expedición" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="MT_fec_notificacion_titulo" HeaderText="Fecha notificación" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="FORMAS_NOTIFICACIONMT_for_notificacion_titulonombre" HeaderText="Forma notificación"></asp:BoundField>
                                <asp:BoundField DataField="MT_fecha_ejecutoria" HeaderText="Fecha ejecutoría" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="MT_fec_cad_presc" HeaderText="Fecha caducidad" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                <asp:BoundField DataField="idunico" >
                                    <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                    <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                </asp:BoundField>
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


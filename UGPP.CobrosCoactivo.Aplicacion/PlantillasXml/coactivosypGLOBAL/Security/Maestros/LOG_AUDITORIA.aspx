<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LOG_AUDITORIA.aspx.vb" Inherits="coactivosyp.LOG_AUDITORIA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
    <head>
        <title>LOG_AUDITORIA</title>
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
                $('#cmdFirst').button();
                $('#cmdNext').button();
                $('#cmdLast').button();
                $('#cmdPrevious').button();
                $('.GridEditButton').button();
                $('#btnExportarGrid').button();                

                $(".PCG-Content tr:gt(0)").mouseover(function() {
                    $(this).addClass("ui-state-highlight");
                });

                $(".PCG-Content tr:gt(0)").mouseout(function() {
                    $(this).removeClass("ui-state-highlight");
                });
            }
        </script>
        
        <style type="text/css">
		    * { font-size:12px; font-family:Arial}	
		    .BoundFieldItemStyleHidden { display:none; }
		    .BoundFieldHeaderStyleHidden {display:none;}
		</style>
		
    </head>
    <body>
        <form id="Form1" method="post" runat="server">
        
            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
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
                        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" class="ui-widget-content ui-widget">
                <tr>
                    <td align="right">
                            <table style="width: 100%">
                                <tr>
                            <td class="ui-widget-header">
                            ID de usuario</td>
                            <td>
                            <asp:TextBox ID="txtSearchLOG_USER_ID" runat="server" ></asp:TextBox></td>
                        <td class="ui-widget-header">
                        Host / Equipo</td>
                        <td>
                        <asp:TextBox ID="txtSearchLOG_HOST" runat="server" ></asp:TextBox></td>
                                    <td>
                                        
                                        <asp:Button id="cmdSearch" runat="server" Text="Buscar" cssClass="PCGButton"></asp:Button>
                                    </td>
                            </tr>
                                <tr>
                    <td class="ui-widget-header">
                    IP</td>
                    <td>
                    <asp:TextBox ID="txtSearchLOG_IP" runat="server" ></asp:TextBox></td>
                <td class="ui-widget-header">
                Módulo</td>
                <td>
                <asp:TextBox ID="txtSearchLOG_MODULO" runat="server" ></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnExportarGrid" runat="server" Text="Exportar XLS" />
                                    </td>
                            </tr>
                                <tr>
                        <td class="ui-widget-header">
                            Documento afectado
                        </td>
            <td>
            <asp:TextBox ID="txtSearchLOG_DOC_AFEC" runat="server" ></asp:TextBox></td>
                                    <td>&nbsp;</td><td>&nbsp;</td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    Paginación&nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="ui-widget" id="cboNumExp" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                </td>
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
                                    <asp:BoundField DataField="LOG_CONSE" >
                                        <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                        <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LOG_USER_ID" HeaderText="Usuario" SortExpression="LOG_AUDITORIA.LOG_USER_ID"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_HOST" HeaderText="Host / Equipo" SortExpression="LOG_AUDITORIA.LOG_HOST"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_IP" HeaderText="IP" SortExpression="LOG_AUDITORIA.LOG_IP"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_FECHA" HeaderText="Fecha" SortExpression="LOG_AUDITORIA.LOG_FECHA"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_APLICACION" HeaderText="Aplicación" SortExpression="LOG_AUDITORIA.LOG_APLICACION"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_MODULO" HeaderText="Módulo" SortExpression="LOG_AUDITORIA.LOG_MODULO"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_DOC_AFEC" HeaderText="Docum afec." SortExpression="LOG_AUDITORIA.LOG_DOC_AFEC"></asp:BoundField>
                                    <asp:BoundField DataField="LOG_CONSULTA" HeaderText="Comando" SortExpression="LOG_AUDITORIA.LOG_CONSULTA"></asp:BoundField>
                                    
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
                    </td>
                </tr>
            </table>    
            </form>
        </body>
    </html>
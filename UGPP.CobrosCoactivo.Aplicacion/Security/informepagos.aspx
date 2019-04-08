<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="informepagos.aspx.vb" Inherits="coactivosyp.informepagos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Consulta de pagos realizados por los contribuyentes</title>
        <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
        <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
        <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
        
    </head>
    
    <body>
        <!-- Definicion del menu -->  
        <div id="message_box">
            <ul>
                <li style="height:36px;width:36px;">
                    <a href="informepagos.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
                </li>
                <li style="height:152px;width:36px;">
                    <a href="informepagos.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
                </li>
            </ul>
        </div>
 
        <div id="container">
            <h1 id="Titulo"><a href="#">Tecno Expedientes !</a></h1>
            <form id="form1" runat="server">  
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
                    EnableScriptGlobalization="True">
                </asp:ToolkitScriptManager>                          
                <div>
                    <asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" 
                        Font-Size="12px" style="position:absolute;top:86px; left:44px">
                        Fecha inicial: 
                    </asp:label>
                    
                    <asp:label style="position:absolute;top:112px; left:45px" id="Label13" 
                        runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">
                        Fecha final:
                    </asp:label>
                    
                    <asp:TextBox ID="txtFechaIni" runat="server" 
                        style="position:absolute;top:81px; left:181px; width: 110px;"
                        CssClass="CalendarioBox">
                    </asp:TextBox>
                    
                    <asp:TextBox ID="txtFechaFin" runat="server"
                        style="position:absolute;top:107px; left:181px; width:110px" 
                        CssClass="CalendarioBox">
                    </asp:TextBox>                    
                
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                        TargetControlID="txtFechaIni"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy">
                    </asp:CalendarExtender>
                    
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                        TargetControlID="txtFechaFin"
                        Format="dd/MM/yyyy"
                        PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy">
                    </asp:CalendarExtender>
                    
                    <asp:Button ID="btnConsultarPagos" runat="server" Text="Consultar" 
                        style="position:absolute;top:157px; left:44px; width:144px" />
                    
                    <div style="position:absolute;top:217px; left:44px; width:700px; background-color:White" />
                        <!-- En este espacio vamos a colocar el griview -->                        
                        <asp:GridView ID="dtg_Pagos" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" 
                            style="font-size: 12px" AllowPaging="True" PageSize="15">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:BoundField DataField="PagNroRec" HeaderText="No. Recibo" />
                                <asp:BoundField DataField="PagFec" HeaderText="Fecha de pago" 
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="PagNom" HeaderText="Nombre" />
                                <asp:BoundField DataField="PagValEfe" HeaderText="Valor" 
                                    DataFormatString="{0:N}" >
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PagPerDes" HeaderText="Desde" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PagSubDes">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PagPerHas" HeaderText="Hasta" >
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                                                
                    </div>
                </div>
            </form>
        </div>
    </body>
</html>
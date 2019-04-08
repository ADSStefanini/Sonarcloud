<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consultaracum.aspx.vb" Inherits="coactivosyp.consultaracum" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Consulta de expedientes por Resolución de Acumulación</title>
        <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
        <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
        <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    </head>

    <body>
        <!-- Definicion del menu -->  
        <div id="message_box">
            <ul>
                <li style="height:36px;width:36px;">
                    <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
                </li>
                <li style="height:152px;width:36px;">
                    <a href="menu4.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
                </li>
            </ul>
        </div>

        <div id="container">
            <h1 id="Titulo"><a href="#">Consulta por resolución de acumulación</a></h1>
            <form id="form1" runat="server">
                <div>
                    <asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" 
                        Font-Size="12px" style="position:absolute;top:86px; left:44px">
                        No. de Resolución de acumulación del expediente: 
                    </asp:label>
                    
                    <asp:TextBox ID="txtResAcum" runat="server" 
                        style="position:absolute;top:81px; left:340px; width: 110px;">
                    </asp:TextBox>
                                                            
                    <asp:Button ID="btnConsultarPagos" runat="server" Text="Consultar" 
                        style="position:absolute;top:157px; left:44px; width:117px; background-image: url('images/icons/okay.png')" 
                        CssClass="Botones" />
                    
                    <div style="position:absolute;top:217px;left:44px; width:700px; background-color:White;overflow:auto; height: 146px;">
                        <!-- En este espacio vamos a colocar el griview -->                        
                        <asp:GridView ID="dtg_acum" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="Small" style="width:100%;">
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:HyperLinkField DataNavigateUrlFields="enlace" DataTextField="cedula" 
                                    HeaderText="Cédula" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="idacto" HeaderText="Acto" />
                                <asp:BoundField DataField="docexpediente" HeaderText="Expediente" />
                                <asp:BoundField DataField="docpredio_refecatrastal" 
                                    HeaderText="Ref. Catastral" />
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

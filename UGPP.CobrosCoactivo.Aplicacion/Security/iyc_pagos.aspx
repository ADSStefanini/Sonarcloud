<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iyc_pagos.aspx.vb" Inherits="coactivosyp.iyc_pagos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Tecno Expedientes !</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
       .alinearderecha { text-align:right}
       .centrar{display: block; margin: auto;}
   </style>
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
        <h1 id="Titulo"><a href="#">Industria y Comercio - Pagos</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:96px;" >                
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Placa </div>
                <asp:TextBox ID="txtIdPlaca" runat="server" 
                    style="position:absolute;top:50px; left:116px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                                                 
            </div>
            <div id="DivGridView" style="width:708px; height:170px; left:34px; margin-left:34px; background-color:White">                
                <asp:GridView ID="GridPagos" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="708px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="PagNroRec" HeaderText="Recibo" />
                        <asp:BoundField DataField="PagFec" HeaderText="Fecha Pago" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="PagFecLiq" HeaderText="Fecha Liq." DataFormatString="{0:dd/MM/yyyy}" />                        
                        <asp:BoundField DataField="PagTot" HeaderText="Valor Pago" DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha" />
                        <asp:BoundField DataField="PagPerDes" HeaderText="Desde" />
                        <asp:BoundField DataField="PagSubDes" HeaderText="." />                        
                        <asp:BoundField DataField="PagPerHas" HeaderText="Hasta" />
                        <asp:BoundField DataField="PagSubHas" HeaderText="." />
                        <asp:BoundField DataField="TpaCod" HeaderText="Tipo" DataFormatString="{0:D3}" />
                        <asp:BoundField DataField="PagEst" HeaderText="Estado" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
        </form>
    </div>
</body>
</html>

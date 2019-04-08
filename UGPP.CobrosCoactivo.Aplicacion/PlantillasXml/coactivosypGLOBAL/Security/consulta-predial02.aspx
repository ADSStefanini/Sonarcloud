<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consulta-predial02.aspx.vb" Inherits="coactivosyp.consulta_predial02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
        <h1 id="Titulo"><a href="#">Consulta Avaluos - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <div style="color:#FFF; text-align:right; margin-right:10px; margin-top:-20px">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".") %> </div>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:106px;" >
                <div id="Label2" style="position:absolute;top:52px; left:194px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nro Predial </div>
                <asp:TextBox ID="txtPredial" runat="server" 
                    style="position:absolute;top:50px; left:286px; width:150px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                                
            </div>            
            <div id="DivGridView" style="width:408px; height:170px; width:100%; ">
                <asp:GridView ID="GridAvaluos" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="408px" AllowPaging="True" PageSize="20" CssClass="centrar">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="CarCod" HeaderText="Característica" 
                            DataFormatString="{0:D3}" />
                        <asp:BoundField DataField="PerCod" HeaderText="Periodo" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PreCarVal" HeaderText="Valor" 
                            DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha">
                        <ItemStyle />
                        </asp:BoundField>
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

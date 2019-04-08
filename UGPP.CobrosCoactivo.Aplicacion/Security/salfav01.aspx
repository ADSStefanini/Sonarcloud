<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="salfav01.aspx.vb" Inherits="coactivosyp.salfav01" %>

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
        <h1 id="Titulo"><a href="#">Consulta Predial - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <div style="color:#FFF; text-align:right; margin-right:10px; margin-top:-20px">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".") %> </div>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
                        
            <div id="DivGridView" style="width:700px; height:170px; left:34px; margin-left:34px; margin-top:70px; ;background-color:White">                
                <asp:GridView ID="GridSaldosFav" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="700px" AllowPaging="True" PageSize="16">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="SalNum" HeaderText="Consecutivo" />
                        <asp:BoundField DataField="MunCod" HeaderText="Municipio" 
                            DataFormatString="{0:D3}" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="SalPreNum" HeaderText="Nro Predial" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="SalIndNum" HeaderText="Placa Ind" />
                        <asp:BoundField DataField="SalVig" HeaderText="Vigencia" />
                        <asp:BoundField HeaderText="Saldo Favor" DataField="SalFav" 
                            DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha" />
                        <asp:BoundField HeaderText="Saldo contra" DataField="SalCon" 
                            DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha" />
                        <asp:BoundField HeaderText="Fecha" DataField="SalFec" 
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Usuario" DataField="SalCodUsu" />
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

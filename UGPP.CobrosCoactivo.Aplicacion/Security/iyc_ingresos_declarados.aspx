<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iyc_ingresos_declarados.aspx.vb" Inherits="coactivosyp.iyc_ingresos_declarados" %>

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
        <h1 id="Titulo"><a href="#">Industria y Comercio - Ingresos declarados</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:186px;" >                
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Matrícula </div>
                <asp:TextBox ID="txtIdPlaca" runat="server" 
                    style="position:absolute;top:50px; left:122px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                                                 
                
                <div id="Div1" style="position:absolute;top:82px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Identificación</div>
                <asp:TextBox ID="txtIdentificacion" runat="server" 
                    style="position:absolute;top:80px; left:122px; width:150px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div2" style="position:absolute;top:82px; left:380px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fecha inicio</div>
                <asp:TextBox ID="txtFecIni" runat="server" 
                    style="position:absolute;top:80px; left:460px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div3" style="position:absolute;top:112px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nombre</div>
                <asp:TextBox ID="txtNombre" runat="server" 
                    style="position:absolute;top:110px; left:122px; width:250px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div4" style="position:absolute;top:112px; left:380px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Dirección</div>                    
                <asp:TextBox ID="txtDireccion" runat="server" 
                    style="position:absolute;top:110px; left:460px; width:278px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                    
                <div id="Div5" style="position:absolute;top:142px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Actividad</div>
                <asp:TextBox ID="txtActividad" runat="server" 
                    style="position:absolute;top:140px; left:122px; width:616px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                                
            </div>
            <div id="DivGridView" style="width:708px; height:170px; left:34px; margin-left:34px; background-color:White">                
                <asp:GridView ID="GridDeclaraciones" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="708px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="DecMaeNum" HeaderText="Matrícula" />
                        <asp:BoundField DataField="DecVig" HeaderText="Vigencia" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="TipLla" HeaderText="Tipo" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="ActCod" HeaderText="Activ." />
                        <asp:BoundField DataField="DecIng" HeaderText="Ingresos" 
                            DataFormatString="{0:N2}" ItemStyle-CssClass="alinearderecha"/>
                        <asp:BoundField DataField="DecOtrIng" DataFormatString="{0:N2}" ItemStyle-CssClass="alinearderecha" 
                            HeaderText="Otros ingr." />
                        <asp:BoundField DataField="DecDed" DataFormatString="{0:N2}" ItemStyle-CssClass="alinearderecha" 
                            HeaderText="Deducciones" />
                        <asp:BoundField DataField="DecTar" DataFormatString="{0:F1}" HeaderText="Tarifa" />
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

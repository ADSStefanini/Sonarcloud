<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="verforpag.aspx.vb" Inherits="coactivosyp.verforpag" %>

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
        <h1 id="Titulo"><a href="#">Forma de pago - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:124px;" >
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:bold; width:714px; text-align:center ">Forma de pago</div>
                <div id="Div1" style="position:absolute;top:80px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Forma pago</div>
                <asp:TextBox ID="txtFormaPago" runat="server" 
                    style="position:absolute;top:102px; left:34px; width:70px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div2" style="position:absolute;top:80px; left:144px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Subcentro</div>
                <asp:TextBox ID="txtCentro" runat="server" 
                    style="position:absolute;top:102px; left:144px; width:40px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtSubcentro" runat="server" 
                    style="position:absolute;top:102px; left:190px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div3" style="position:absolute;top:80px; left:244px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Usuario</div>
                <asp:TextBox ID="txtUsuario" runat="server" 
                    style="position:absolute;top:102px; left:244px; width:120px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div4" style="position:absolute;top:80px; left:514px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Fecha Pago</div>
                <asp:TextBox ID="txtFechaPag" runat="server" 
                    style="position:absolute;top:102px; left:514px; width:66px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div5" style="position:absolute;top:80px; left:614px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:54px; text-align:left ">Estado</div>
                <asp:TextBox ID="txtEstado" runat="server" 
                    style="position:absolute;top:102px; left:614px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                                
            </div>
            <div id="DivGridView" style="width:714px; height:300px; left:34px; margin-left:34px; background-color:White">                
                <asp:GridView ID="GridFormaPago" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="714px" AllowPaging="True">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="ForCon" HeaderText="Cons."></asp:BoundField>
                        <asp:BoundField DataField="ForTip" HeaderText="Tipo" 
                            DataFormatString="{0:D3}" />
                        <asp:BoundField DataField="DocDes" HeaderText="Documento" >
                        </asp:BoundField>
                        <asp:BoundField DataField="ForBanCod" HeaderText="Código" 
                            DataFormatString="{0:D3}" >                                                
                        </asp:BoundField>
                        <asp:BoundField DataField="CenDes" HeaderText="Entidad" />
                        <asp:BoundField DataField="ForNroDoc" HeaderText="Nro Documento" />
                        <asp:BoundField DataField="ForCtaAut" HeaderText="Cta/Autoriza" />
                        <asp:BoundField DataField="ForVal" HeaderText="Valor" 
                            DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha" >
<ItemStyle CssClass="alinearderecha"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ForFecDoc" HeaderText="Fecha" 
                            DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            
            <div id="DivResumen" style="width:760px; height:164px; background-color:#7198E7" >
                <div id="Div6" style="position:absolute;top:450px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Efectivo</div>
                <asp:TextBox ID="txtEfectivo" runat="server" 
                    style="position:absolute;top:448px; left:90px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                                
                <div id="Div7" style="position:absolute;top:480px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Cheque</div>
                <asp:TextBox ID="txtCheque" runat="server" 
                    style="position:absolute;top:478px; left:90px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div8" style="position:absolute;top:510px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Tarjeta</div>
                <asp:TextBox ID="txtTarjeta" runat="server" 
                    style="position:absolute;top:508px; left:90px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div9" style="position:absolute;top:540px; left:90px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:114px; text-align:left ">Total Liquidado</div>
                <asp:TextBox ID="txtTotalLiq" runat="server" 
                    style="position:absolute;top:558px; left:90px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div10" style="position:absolute;top:450px; left:234px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Otros</div>
                <asp:TextBox ID="txtOtros" runat="server" 
                    style="position:absolute;top:448px; left:296px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                                
                <div id="Div11" style="position:absolute;top:480px; left:234px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:84px; text-align:left ">Val Total</div>
                <asp:TextBox ID="txtValTotal" runat="server" 
                    style="position:absolute;top:478px; left:296px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div12" style="position:absolute;top:540px; left:296px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; width:114px; text-align:left ">Diferencia</div>
                <asp:TextBox ID="txtDiferencia" runat="server" 
                    style="position:absolute;top:558px; left:296px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
            </div>
        </form>
   </div>      
</body>
</html>


<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iyc_estado_sanciones.aspx.vb" Inherits="coactivosyp.iyc_estado_sanciones" %>

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
        <h1 id="Titulo"><a href="#">Industria y Comercio - MATRICULAS EN PROCESO DE FISCALIZACION</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:286px;" >                
                <div id="Label1" style="position:absolute;top:52px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Placa </div>
                <asp:TextBox ID="txtIdPlaca" runat="server" 
                    style="position:absolute;top:50px; left:122px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                                                 
                
                <div id="Div1" style="position:absolute;top:82px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nombre</div>
                <asp:TextBox ID="txtNombre" runat="server" 
                    style="position:absolute;top:80px; left:122px; width:340px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div2" style="position:absolute;top:82px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Cédula-Nit</div>
                <asp:TextBox ID="txtCedula" runat="server" 
                    style="position:absolute;top:80px; left:538px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div3" style="position:absolute;top:112px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Dirección</div>
                <asp:TextBox ID="txtDireccion" runat="server" 
                    style="position:absolute;top:110px; left:122px; width:626px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                                                    
                    
                <div id="Div5" style="position:absolute;top:142px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Emplaz no dec.</div>
                <asp:TextBox ID="txtEmplazNoDec" runat="server" 
                    style="position:absolute;top:140px; left:122px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div4" style="position:absolute;top:142px; left:250px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fecha Emplaz.</div>
                
                <asp:TextBox ID="txtFecEmplazND" runat="server" 
                    style="position:absolute;top:140px; left:352px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div6" style="position:absolute;top:142px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Vigencias</div>
                
                <asp:TextBox ID="txtVigenciasEND" runat="server" 
                    style="position:absolute;top:140px; left:538px; width:210px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div7" style="position:absolute;top:172px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">No.Resolución</div>
                <asp:TextBox ID="txtNumResolEND" runat="server" 
                    style="position:absolute;top:170px; left:122px; width:60px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div8" style="position:absolute;top:202px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Sanción no dec</div>
                <asp:TextBox ID="txtSancionND" runat="server" 
                    style="position:absolute;top:200px; left:122px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div9" style="position:absolute;top:202px; left:250px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fecha Sanc.</div>
                <asp:TextBox ID="txtFechaSancionND" runat="server" 
                    style="position:absolute;top:200px; left:352px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div10" style="position:absolute;top:202px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fecha Lim.</div>
                <asp:TextBox ID="txtFechaLim" runat="server" 
                    style="position:absolute;top:200px; left:538px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div11" style="position:absolute;top:232px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">No Resol Sanc.</div>
                <asp:TextBox ID="txtNumResolSan" runat="server" 
                    style="position:absolute;top:230px; left:122px; width:60px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div12" style="position:absolute;top:232px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Vigencias</div>
                <asp:TextBox ID="txtVigenciasResolSan" runat="server" 
                    style="position:absolute;top:230px; left:538px; width:210px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div13" style="position:absolute;top:262px; left:20px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Emplazamiento para corregir (DIAN)</div>
                <asp:TextBox ID="txtEmplazCorregir" runat="server" 
                    style="position:absolute;top:260px; left:262px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div14" style="position:absolute;top:262px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Vigencias</div>
                <asp:TextBox ID="txtVigenciasEmpCor" runat="server" 
                    style="position:absolute;top:260px; left:538px; width:210px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
            </div>
            <div id="DivGridView" style="width:510px; height:170px; left:14px; margin-left:18px;">                
                <asp:GridView ID="GridSanciones" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="508px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="ModCod" HeaderText="Módulo" 
                            DataFormatString="{0:D3}" />
                        <asp:BoundField DataField="LiqGen" HeaderText="Contribuyente" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="ConCod" HeaderText="Concepto" 
                            DataFormatString="{0:D3}" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PerCod" HeaderText="Periodo" />
                        <asp:BoundField DataField="LiqVal" HeaderText="Valor Liq." 
                            ItemStyle-CssClass="alinearderecha" DataFormatString="{0:N2}">
<ItemStyle CssClass="alinearderecha"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="LiqEst" DataFormatString="{0:N2}" ItemStyle-CssClass="alinearderecha" 
                            HeaderText="Estado" >
<ItemStyle CssClass="alinearderecha"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            <div id="Div15" style="position:absolute;top:302px; left:540px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Descripción - Concepto</div>
            <div id="Div16" style="position:absolute;top:322px; left:540px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">7: Sanción de corrección</div>
            <div id="Div17" style="position:absolute;top:342px; left:540px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">8: Sanción por no declarar</div>
            <div id="Div18" style="position:absolute;top:362px; left:540px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">10: Sanción por extemporaneidad</div>
            <div id="Div19" style="position:absolute;top:382px; left:540px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">19: Sanción</div>
        </form>
    </div>
</body>
</html>

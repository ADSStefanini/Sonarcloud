<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iyc_estado_cuenta.aspx.vb" Inherits="coactivosyp.iyc_estado_cuenta" %>

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
        <h1 id="Titulo"><a href="#">Industria y Comercio - Estado de Cuenta</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:284px;" >
                <asp:TextBox ID="txtFecha" runat="server" 
                    style="position:absolute;top:50px; left:34px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div3" style="position:absolute;top:52px; left:134px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">ESTADO DE CUENTA - GENERAL DESDE:</div>
                <asp:TextBox ID="txtAnioDesde" runat="server" 
                    style="position:absolute;top:50px; left:490px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtMesDesde" runat="server" 
                    style="position:absolute;top:50px; left:434px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div4" style="position:absolute;top:52px; left:538px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">HASTA:</div>
                <asp:TextBox ID="txtAnioHasta" runat="server" 
                    style="position:absolute;top:50px; left:600px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtMesHasta" runat="server" 
                    style="position:absolute;top:50px; left:656px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Label1" style="position:absolute;top:82px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">REG-IND/CIO.</div>
                <div id="Div5" style="position:absolute;top:112px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">MATRICULAS ADICIONALES</div>
                <asp:TextBox ID="txtIdPlaca" runat="server" 
                    style="position:absolute;top:80px; left:130px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                   
                <div id="Div1" style="position:absolute;top:82px; left:234px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">NIT/CC</div>
                <asp:TextBox ID="txtIdentificacion" runat="server" 
                    style="position:absolute;top:80px; left:290px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div2" style="position:absolute;top:82px; left:434px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">FECHA DE INICIACION</div>
                <asp:TextBox ID="txtFechaIni" runat="server" 
                    style="position:absolute;top:80px; left:600px; width:110px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div6" style="position:absolute;top:142px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">NOMBRE</div>
                <asp:TextBox ID="txtNombre" runat="server" 
                    style="position:absolute;top:140px; left:130px; width:380px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div7" style="position:absolute;top:142px; left:516px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">CAMARA CIO.</div>
                <asp:TextBox ID="txtCamaraCom" runat="server" 
                    style="position:absolute;top:140px; left:612px; width:108px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div8" style="position:absolute;top:172px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">ACTIVIDAD</div>
                <asp:TextBox ID="txtActividad" runat="server" 
                    style="position:absolute;top:170px; left:130px; width:380px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div9" style="position:absolute;top:172px; left:516px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">ACUERDO P.</div>
                <asp:TextBox ID="txtAcuerdoPag" runat="server" 
                    style="position:absolute;top:170px; left:612px; width:108px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div10" style="position:absolute;top:202px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">DIRECCION</div>
                <asp:TextBox ID="txtDireccion" runat="server" 
                    style="position:absolute;top:200px; left:130px; width:590px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div11" style="position:absolute;top:232px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">DIR.NOTIFIC</div>
                <asp:TextBox ID="txtDirNotif" runat="server" 
                    style="position:absolute;top:230px; left:130px; width:590px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div12" style="position:absolute;top:262px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">DATOS ULT.P</div>
                <asp:TextBox ID="txtUltPag" runat="server" 
                    style="position:absolute;top:260px; left:130px; width:214px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div13" style="position:absolute;top:262px; left:350px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">MULTAS</div>
                <asp:TextBox ID="txtMultas" runat="server" 
                    style="position:absolute;top:260px; left:408px; width:16px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div14" style="position:absolute;top:262px; left:430px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">COSTAS</div>
                <asp:TextBox ID="txtCostas" runat="server" 
                    style="position:absolute;top:260px; left:494px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div15" style="position:absolute;top:262px; left:580px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">PAG.HAS</div>
                <asp:TextBox ID="txtAnioPagHasta" runat="server" 
                    style="position:absolute;top:260px; left:646px; width:48px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtMesPagHasta" runat="server" 
                    style="position:absolute;top:260px; left:700px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
            </div>
            <div id="DivGridView" style="width:708px; height:170px; left:34px; margin-left:34px; background-color:White">                
                <asp:GridView ID="GridLiquidacion" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="708px" AllowPaging="True" PageSize="10">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="anio" HeaderText="Año" />
                        <asp:BoundField DataField="E" HeaderText="E" />
                        <asp:BoundField DataField="T" HeaderText="T" />                        
                        <asp:BoundField DataField="Deuda" HeaderText="Deuda" DataFormatString="{0:N0}" 
                            ItemStyle-CssClass="alinearderecha" >
<ItemStyle CssClass="alinearderecha"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="interes" HeaderText="Interes" 
                            DataFormatString="{0:N0}" >
                        <ItemStyle CssClass="alinearderecha" />
                        </asp:BoundField>
                        <asp:BoundField DataField="sancion" HeaderText="Sanción" 
                            DataFormatString="{0:N0}" >                        
                        <ItemStyle CssClass="alinearderecha" />
                        </asp:BoundField>
                        <asp:BoundField DataField="saldopend" HeaderText="Saldo Pend" 
                            DataFormatString="{0:N0}" >
                        <ItemStyle CssClass="alinearderecha" />
                        </asp:BoundField>
                        <asp:BoundField DataField="total" HeaderText="Total" 
                            DataFormatString="{0:N0}" >
                        <ItemStyle CssClass="alinearderecha" />
                        </asp:BoundField>
                        <asp:BoundField DataField="acumulado" HeaderText="Acumulado" 
                            DataFormatString="{0:N0}" >
                        <ItemStyle CssClass="alinearderecha" />
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

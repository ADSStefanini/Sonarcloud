<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consulta-predial01.aspx.vb" Inherits="coactivosyp.consulta_predial01" %>

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
            <div id="Buscador" style="width:760px; height:156px;" >
                
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Municipio </div>
                <asp:TextBox ID="txtIdMunicipio" runat="server" 
                    style="position:absolute;top:50px; left:116px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label2" style="position:absolute;top:52px; left:194px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nro Predial </div>
                <asp:TextBox ID="txtPredial" runat="server" 
                    style="position:absolute;top:50px; left:286px; width:150px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label3" style="position:absolute;top:52px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Mat. Inmobiliaria </div>
                <asp:TextBox ID="txtMatInm" runat="server" 
                    style="position:absolute;top:50px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label4" style="position:absolute;top:83px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Dirección </div>
                <asp:TextBox ID="txtDireccion" runat="server" 
                    style="position:absolute;top:80px; left:116px; width:320px;z-index:777;" ></asp:TextBox>                    
                <div id="Label5" style="position:absolute;top:83px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Debe desde</div>
                <asp:TextBox ID="txtAnioDesde" runat="server" 
                    style="position:absolute;top:80px; left:578px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtMesDesde" runat="server" 
                    style="position:absolute;top:80px; left:648px; width:30px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label6" style="position:absolute;top:113px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Dir. Cobro </div>
                <asp:TextBox ID="txtDirCobro" runat="server" 
                    style="position:absolute;top:110px; left:116px; width:320px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                    
                <div id="Div2" style="position:absolute;top:113px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Pago Hasta</div>
                <asp:TextBox ID="txtAnioPago" runat="server" 
                    style="position:absolute;top:110px; left:578px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="txtMesPago" runat="server" 
                    style="position:absolute;top:110px; left:648px; width:30px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div1" style="position:absolute;top:146px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:Bold;">Propietarios</div>
            </div>            
            <div id="DivGridView" style="width:408px; height:170px; left:34px; margin-left:34px; background-color:White">                
                <asp:GridView ID="GridPropietarios" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="408px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="PrsCod" HeaderText="Código" />
                        <asp:BoundField DataField="PrsDoc" HeaderText="Documento" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PrsTipDoc" HeaderText="Tipo" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PreEstPer" HeaderText="Est" />
                        <asp:BoundField DataField="PrsNom" HeaderText="Nombre" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            <div id="DivGridView2" style="width:408px; height:170px; left:34px; margin-left:34px; margin-top:40px; background-color:White">
                <div id="Div3" style="position:absolute;top:356px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:Bold;">Características</div>
                <asp:GridView ID="GridCaracteristicas" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="408px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="PerCod" HeaderText="Periodo" />
                        <asp:BoundField DataField="CarCod" HeaderText="Id Car" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="CarDes" HeaderText="Carac." >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PreCarVal" HeaderText="Valor" 
                            DataFormatString="{0:N0}" ItemStyle-CssClass="alinearderecha" />
                        <asp:BoundField DataField="PreCarEst" HeaderText="Estado" />
                        <asp:BoundField DataField="PreCarSub" HeaderText="PreCarSub" />
                        <asp:BoundField DataField="PreFecAct" HeaderText="PreFecAct" 
                            DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            <div id="DivGridView3" style="width:408px; height:170px; left:34px; margin-left:34px; margin-top:80px; background-color:White">
                <div id="Div5" style="position:absolute;top:606px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:Bold;">Predio Matriz</div>
                <asp:GridView ID="GridMatriz" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="408px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="MatPreNum" HeaderText="Nro. Pre. Matriz" />
                        <asp:BoundField DataField="MatTipRes" HeaderText="Tip Res" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="MatVigRes" HeaderText="Vig. Res." >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="MatNumRes" HeaderText="Nro Res Mat" />
                        <asp:BoundField DataField="MatFecApl" HeaderText="Fec Apli Res" 
                            DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            <div id="DivGridView4" style="width:408px; height:170px; left:34px; margin-left:34px; margin-top:30px; background-color:White">
                <div id="Div6" style="position:absolute;top:806px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:Bold;">Resoluciones Aplicadas</div>
                <asp:GridView ID="GridResolucionesAplicadas" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="408px" AllowPaging="True" PageSize="6">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="ResTip" HeaderText="Tip Resol." />
                        <asp:BoundField DataField="ResVig" HeaderText="Resolución" >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="ResNum" HeaderText="Nro. Resol." >
                        <ItemStyle />
                        </asp:BoundField>
                        <asp:BoundField DataField="PreResFecA" HeaderText="Fecha aplicación" 
                            DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            <div id="DivCuadroInfo2" style="position:absolute; width:308px; height:170px; top:166px; left:450px; ; background-color:#7198E7">
                <div id="Div4" style="position:absolute;width:118px; top:12px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Predio Mun.</div>
                <asp:TextBox ID="txtPredioMun" runat="server" 
                    style="position:absolute; top:9px; left:128px; width:10px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div7" style="position:absolute;width:118px; top:38px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Eje. Fis</div>
                <asp:TextBox ID="TextBox1" runat="server" 
                    style="position:absolute; top:35px; left:128px; width:10px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div8" style="position:absolute;width:118px; top:64px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Acu. Pago</div>
                <asp:TextBox ID="TextBox2" runat="server" 
                    style="position:absolute; top:61px; left:128px; width:10px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div9" style="position:absolute;width:118px; top:91px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Exención</div>
                <asp:TextBox ID="TextBox3" runat="server" 
                    style="position:absolute; top:88px; left:128px; width:10px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div10" style="position:absolute;width:118px; top:118px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Est. Che.</div>
                <asp:TextBox ID="TextBox4" runat="server" 
                    style="position:absolute; top:115px; left:128px; width:10px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div11" style="position:absolute;width:118px; top:145px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Per. Cancel</div>
                <asp:TextBox ID="TextBox5" runat="server" 
                    style="position:absolute; top:142px; left:128px; width:40px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <asp:TextBox ID="TextBox6" runat="server" 
                    style="position:absolute; top:142px; left:178px; width:20px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                
            </div>
            <div id="DivCuadroInfo3" style="position:absolute; width:308px; height:170px; top:375px; left:450px; ; background-color:#7198E7">
                <div id="Div12" style="position:absolute;width:118px; top:12px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Fec. Ult. Pago</div>
                <asp:TextBox ID="TextBox7" runat="server" 
                    style="position:absolute; top:9px; left:128px; width:66px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Div13" style="position:absolute;width:118px; top:38px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Val. Ult. Pago</div>
                <asp:TextBox ID="TextBox8" runat="server" 
                    style="position:absolute; top:35px; left:128px; width:126px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                    
                <div id="Div14" style="position:absolute;width:118px; top:64px; left:10px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal; ">Ult. Rcb. Pago</div>
                <asp:TextBox ID="TextBox9" runat="server" 
                    style="position:absolute; top:61px; left:128px; width:126px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
            </div>
        </form>
   </div>
      
</body>
</html>

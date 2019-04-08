<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consulta-industriaycomercio.aspx.vb" Inherits="coactivosyp.consulta_industriaycomercio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Tecno Expedientes !</title>
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
        <h1 id="Titulo"><a href="#">Consulta de Industria y Comercio</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:144px;" >
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:bold;">Digite el número de la placa: </div>
                <asp:TextBox ID="txtEnte" runat="server" 
                    style="position:absolute;top:72px; left:34px; width:709px;z-index:777;" ></asp:TextBox>
                <asp:customvalidator style="position:absolute;top:98px; left:46px; width: 702px;" id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
                <asp:Button id="btnAceptar" runat="server" Text="Consultar" ValidationGroup="textovalidados"
                style="position:absolute;top:120px; left:34px; width: 92px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>                
                <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
                    style="position:absolute;top:120px; left:133px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
                    CssClass="Botones">
                </asp:Button>
                <div id="Divcriterio" style="position:absolute;top:124px; left:234px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Buscar por: </div>
                <asp:DropDownList ID="cmbBuscarPor" runat="server" style="position:absolute;top:124px; left:314px;">
                    <asp:ListItem Value="Placa">Placa</asp:ListItem>
                    <asp:ListItem Value="Cedula">Cédula</asp:ListItem>
                    <asp:ListItem Value="Direccion">Dirección</asp:ListItem>
                    <asp:ListItem Value="Nombre">Nombre</asp:ListItem>
                </asp:DropDownList>   
                <div id="DivSeleccionado" style="position:absolute;top:124px; left:474px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Placa selecccionada: </div>                                                        
                <asp:Label ID="LabelPredioSel" runat="server" Text="Ninguna" style="position:absolute;top:124px; left:626px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;"></asp:Label>
            </div>
            <div id="DivGridView" style="width:714px; height:300px; left:34px; margin-left:34px; background-color:White;">                
                <asp:GridView ID="GridVPredios" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="714px" AllowPaging="True">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:ButtonField CommandName="select" DataTextField="MaeNum"  
                            HeaderText="Placa"  Text="Placa" >
                        <ItemStyle Width="36px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="MaeProCod" HeaderText="Cédula o NIT" >
                        <ItemStyle Width="74px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MaeProNom" HeaderText="Nombre" >
                        <ItemStyle Width="130px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MaeDir" HeaderText="Dirección Establecimiento" >
                        <ItemStyle Width="210px" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
            
            <div id="DivOpciones" style="width:760px; height:180px; padding:left;" >                
                <asp:Button id="btnInfGral" runat="server" Text="Inf. General" ValidationGroup="textovalidados"
                style="position:absolute;top:504px; left:34px; width: 120px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
                <asp:Button id="btnCancelTraspasos" runat="server" Text="Canc-traspasos" ValidationGroup="textovalidados"
                style="position:absolute;top:504px; left:158px; width: 120px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
                <asp:Button id="btnDeclaraciones" runat="server" Text="Declaraciones" ValidationGroup="textovalidados"
                style="position:absolute;top:504px; left:282px; width: 120px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
                <asp:Button id="btnPagos" runat="server" Text="Pagos" ValidationGroup="textovalidados"
                style="position:absolute;top:504px; left:406px; width: 120px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
                <asp:Button id="btnSaldosFavCon" runat="server" Text="Estado-Sanciones" ValidationGroup="textovalidados"
                style="position:absolute;top:504px; left:530px; width: 128px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
                <asp:Button id="btnEstadoCta" runat="server" Text="Estado-Cuenta" ValidationGroup="textovalidados"
                style="position:absolute;top:534px; left:34px; width: 120px; background-image: url('images/icons/okay.png'); z-index:10" 
                CssClass="Botones">
                </asp:Button>
            </div>
        </form>
    </div>
</body>
</html>

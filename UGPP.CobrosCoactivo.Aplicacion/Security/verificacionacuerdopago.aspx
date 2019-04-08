<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="verificacionacuerdopago.aspx.vb" Inherits="coactivosyp.verificacionacuerdopago" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cobranzas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .Yup
        {
        padding:10px;
        text-align:center;
        color: #FFFFFF;
        }
        .xlist
        {
        text-align:left;
        font-family: Verdana;
        font-size: 11px;
        font-weight: bold;
        font-style: normal;
        color: #FFFFFF;
        }
        .divhisto
        {
        /*border: 1px solid #dcdbe0; */
        background-color:#507CD1;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        }
        .CajaDialogo
        {
        position:absolute;
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #72A3F8;
        padding: 0px;
        color:#514E4E;
        font-weight: bold;
        font-size:12px;
        font-style: italic;
        }
        .palanca
        {
        color: #34484E;         
        }
        .ppala{width:715px;
        }
        .ppala .cl
        {
            border: 1px solid #999;
            padding-left:3px;
            width:190px;
        }
          .ppala .cl2
        {
            border: 1px solid #999;
            padding-left:3px;
            margin-left: 80px;
        }
        #Text1
        {
            width: 60px;
        }
        #TxtAnoAcuerdo
        {
            width: 132px;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function($) {
            $("#rApoderado_0").click(function(e) {
                alert("mensaje")
            });
        });
    </script>
   
</head>
<body>
<form id="form1" runat="server">

  <div id="container">
    <h1 id="Titulo"><a href="#">Cobranzas</a></h1>
  

     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>        
     
  
     
     <div class="Yup divhisto" 
          
          
          
              style="width: 713px; position:absolute;top:177px; left:23px; background-color:#507CD1; z-index:1;">
         <h4>VERIFICAR ACUERDOS DE PAGOS</h4>
         
         <table style="text-align:left; font-size:11px;" class = "ppala" align="center">
            <tr><td  class="cl" style="width: 135px">Predio/ reg. Ind y Com: </td>
                <td class="cl2">
                    <asp:TextBox ID="TxtNumPredio" runat="server" Width="186px" AutoPostBack="true"
                        ></asp:TextBox></td></tr>
            <tr><td  class="cl" style="width: 135px">Nro. Acuerdo</td>
                <td class="cl2">
                    <asp:TextBox ID="txtNroAcuerdo" runat="server" Width="186px" Enabled="False"></asp:TextBox>
                </td></tr>
            <tr><td  class="cl" style="width: 135px">Nro. Proceso</td>
                <td class="cl2">
                    <asp:TextBox ID="txtExpediente" runat="server" Width="185px" Enabled="False"></asp:TextBox>
                </td></tr>
            </table>
         <br />
                            
                            <asp:GridView ID="dgVigencia" runat="server" 
              AutoGenerateColumns="False"  width = "214px"
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" style="font-size: 12px" 
              HorizontalAlign="Right" PageSize="1" >
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" 
                                        SelectImageUrl="~/Security/images/icons/1.png" ShowSelectButton="True" />
                                    <asp:BoundField DataField="ACUERDO" HeaderText="N. Acuerdo" />
                                    <asp:BoundField DataField="VALOR" HeaderText="T. Deuda" 
                                     DataFormatString="{0:N}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    
                                   
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            
                 <asp:GridView ID="DtgAcuerdos" runat="server" 
              AutoGenerateColumns="False"  width = "491px"
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" style="font-size: 12px" 
              HorizontalAlign="Left" PageSize="1">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" 
                                        SortExpression="EFIGEN" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="50px"  HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PAGO_TOTAL" HeaderText="Pago Total " 
                                        DataFormatString="{0:N}"  >
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="100px" HorizontalAlign ="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PAGO_ACUERDO" HeaderText="Pago Acuerdo" HeaderStyle-HorizontalAlign="Center" 
                                        DataFormatString="{0:N}">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                        <ItemStyle Width="150px" HorizontalAlign ="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Vig Inicial" 
                                        DataField="VIG_INICIAL" >
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Vig. Final" DataField="VIG_FINAL">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            
                            </div>
        <div  style="top:135px;left: 27px;position: absolute;">
         
         <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="Botones" 
             style ="background-image: url('images/icons/okay.png');" 
             Width="95px" />
         
         
         <asp:Button ID="btnLiquidar" runat="server" Text="Buscar" CssClass="Botones" 
             style ="background-image: url('images/icons/search.gif');" 
             Width="95px"/>
         
         <asp:Button ID="gtnAcuerdopago" runat="server" Text="Acuerdo Pagos" CssClass="Botones" 
             style ="background-image: url('images/icons/dollar.png');" 
             Width="110px"/>
             
         </div>
         
         
                
                    
       
         
             <div style="position:absolute;top:91px; left:26px; width: 602px; right: 151px;" 
         class="divhisto">
            <table width="100%">
                <tr>
                 <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                 <th style="font-size:11px;text-align:left;padding:4px;width:90px">Ente :</th>
                 <th style="text-align: right;font-size:11px;padding:4px;" class="palanca"><asp:Label ID="lblCobrador" runat="server" Text=""></asp:Label></th>
                 <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><asp:LinkButton ID="LinkCancelar" runat="server"><img src="images/icons/cancel.png" alt="" /></asp:LinkButton></th>
                </tr>
            </table>
       </div>
     <asp:customvalidator style="position:absolute;top:41px;left:24px;width:735px;" 
         id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="12px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
    
    </div> 
</form>
</body>
</html>


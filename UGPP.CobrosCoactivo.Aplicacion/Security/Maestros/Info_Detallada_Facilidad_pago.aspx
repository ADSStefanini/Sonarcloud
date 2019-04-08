<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Info_Detallada_Facilidad_pago.aspx.vb" Inherits="coactivosyp.Info_Detallada_Facilidad_pago" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Informacion detallada de la facilidad de pago</title>
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
        <asp:Label ID="lblError" runat="server" Text="..."></asp:Label> </div>
    
                 <asp:GridView ID="DtgAcuerdos" runat="server"  CssClass="CSSTableGenerator"
                                AutoGenerateColumns="False"  width = "400px" 
                                PageSize="1">
                                
                     <Columns>
                         <asp:BoundField DataField="EXPEDIENTE" HeaderText="EXPEDIENTE" />
                         <asp:BoundField DataField="LIQ_REC" HeaderText="LIQ_REC" />
                         <asp:BoundField DataField="INF" HeaderText="INF" />
                         <asp:BoundField DataField="SUBSISTEMA" HeaderText="SUBSISTEMA" />
                         <asp:BoundField DataField="NIT_EMPRESA" HeaderText="NIT_EMPRESA" />
                         <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="RAZON_SOCIAL" 
                             HtmlEncode="False" />
                         <asp:BoundField DataField="ANNO" HeaderText="AÑO" />
                         <asp:BoundField DataField="MES" HeaderText="MES" />
                         <asp:BoundField DataField="CEDULA" HeaderText="CEDULA" />
                         <asp:BoundField DataField="NOMBRE" HeaderText="NOMBRE" HtmlEncode="False" />
                         <asp:BoundField DataField="IBC" HeaderText="IBC" />
                         <asp:BoundField DataField="AJUSTE" HeaderText="AJUSTE" />
                         <asp:BoundField DataField="FECHA_PAGO" HeaderText="FECHA_PAGO" />
                         <asp:BoundField DataField="INTERESES" HeaderText="INTERESES" />
                         <asp:BoundField DataField="TOTAL_PAGAR" HeaderText="TOTAL_PAGAR" />
                         <asp:BoundField DataField="FECHA_EXIGIBILIDAD" 
                             HeaderText="FECHA_EXIGIBILIDAD" />
                         <asp:TemplateField HeaderText="ESTADO">
                             <EditItemTemplate>
                                 <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                             </EditItemTemplate>
                             <ItemTemplate>
                                 <asp:DropDownList ID="ddlistestado" runat="server">
                                 <asp:ListItem Value="PT" Text ="Pago Total">  </asp:ListItem>
                                 <asp:ListItem Value="PP" Text ="Pago Parcial">  </asp:ListItem>
                                 <asp:ListItem Value="PT" Text ="Sin Pago">  </asp:ListItem>
                                 </asp:DropDownList>
                             </ItemTemplate>
                         </asp:TemplateField>
                     </Columns>
                                
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />                            
               </asp:GridView>
    
    </div>
    </form>
</body>
</html>

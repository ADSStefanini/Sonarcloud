<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Pruebas_Toyle.aspx.vb" Inherits="coactivosyp.Pruebas_Toyle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Página sin título</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            Historial completo del expediente. 
        </HeaderTemplate>
        <ItemTemplate>
            <table>
                <tr>
                    <td ><div>Acto :</div></td>
                    <td ><div><%# Eval("ACTO") & "-" & Eval("DESCRIPCION")  %></div></td>
                 </tr>
                 <tr>
                    <td ><div>Fecha de Radicación :</div></td>
                    <td ><div><%#Eval("FECHA_R")%></div></td>
                </tr>
                <tr>
                    <td ><div>Fecha Proyectada</div></td>
                    <td ><div><%# Eval("FECHA_P") %></div></td>                
                </tr>
                <tr>
                 <td colspan="2"><hr /></td>
                </tr>
            </table>        
        </ItemTemplate>
    </asp:Repeater>
    </div>
    </form>
</body>
</html>

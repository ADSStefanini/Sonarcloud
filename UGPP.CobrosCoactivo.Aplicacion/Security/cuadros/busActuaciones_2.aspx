<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="busActuaciones_2.aspx.vb" Inherits="coactivosyp.busActuaciones_2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .pool
        {
            font-size:11px;
            text-align:left;
            font-family:Tahoma,Verdana;
        }
    </style>
    <script type="text/javascript">
     $(document).ready(function() {
      $(".busqueda tr:has(td)").each(function() {
              var t = $(this).text().toLowerCase(); 
                        $("<td class='indexColumn'></td>")
                        .hide().text(t).appendTo(this);
       });
      }); 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pool">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="busqueda" 
            CellPadding="4" ForeColor="#333333" GridLines="None" Width="700px">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="codigo" HeaderText="ID" >
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="13px" />
                </asp:BoundField>
                <asp:ButtonField CommandName="select" DataTextField="nombre" 
                    HeaderText="Actuacion" Text="Botón">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:ButtonField>
                <asp:BoundField DataField="idetapa" HeaderText="Cod." >
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="13px" />
                </asp:BoundField>
                <asp:BoundField DataField="etnom" HeaderText="Etapa" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
      </div>
      <input type="hidden" id="control" runat="server" />  
    </form>
</body>
</html>

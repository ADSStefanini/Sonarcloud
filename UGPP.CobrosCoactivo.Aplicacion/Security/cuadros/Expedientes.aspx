<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Expedientes.aspx.vb" Inherits="coactivosyp.Expedientes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Página sin título</title>
    
    <style type="text/css">
         .tabla {
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size:11px;
        text-align: left;
        font-family: "Trebuchet MS", Arial;
        text-transform: uppercase;
        background-color: #EDEDE9;
        margin:0px;
        width:100%;
        }
        
        .tabla th {
        padding: 2px;
        background-color: #e3e2e2;
        color: #000000;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        font-weight:bold;
        text-align:left;
        width:200px;
        }
        
        .tabla td {
        padding: 2px;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #D8D8D8;
        border-bottom-color: #D8D8D8;

        background-color: #EDEDE9;
        color: #34484E;
        }
    </style>
    
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" class="tabla" style="text-align:center;border-collapse:collapse;" cellspacing="0" rules="all" border="1">
         <tr>
           <td><div id = "pp" runat="server"></div></td>
         </tr>
        </table>
        
        <asp:GridView ID="Gridexpedinete" runat="server" AutoGenerateColumns="False" 
            Width="100%" CssClass="tabla">
            <Columns>
                <asp:ButtonField CommandName="select" DataTextField="docexpediente" 
                    HeaderText="Expedientes" Text="Botón">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:ButtonField>
            </Columns>
        </asp:GridView>
        <table width="100%" class="tabla" style="text-align:left;border-collapse:collapse;" cellspacing="0" rules="all" border="1">
         <tr>
           <td><div id = "Div1" runat="server"></div></td>
         </tr>
        </table>
    </div>
    </form>
</body>
</html>

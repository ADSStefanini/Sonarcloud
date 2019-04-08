<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Etapas.aspx.vb" Inherits="coactivosyp.Etapas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Etapas</title>
    <style type="text/css">
        #gh
        {
        	font-family:Verdana;
        	text-align:center;
        	text-transform:uppercase;
        	color:#D8D8D8;
        	margin:3px;
        }
    </style>
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
        <h2 id="gh">Etapas Administrativas</h2>
        <asp:GridView ID="GridEtapas" runat="server" AutoGenerateColumns="False" CssClass="tabla">
            <Columns>
                <asp:ButtonField CommandName="select" DataTextField="codigo" HeaderText="Cod." 
                    Text="Botón">
                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                </asp:ButtonField>
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            </Columns>
        </asp:GridView>
    </div>
    <input type="hidden" id="control" runat="server" />
    </form>
</body>
</html>

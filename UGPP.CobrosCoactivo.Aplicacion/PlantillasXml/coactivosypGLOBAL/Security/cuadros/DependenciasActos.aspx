<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DependenciasActos.aspx.vb" Inherits="coactivosyp.DependenciasActos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Dependencia de los actos </title>
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
        <table class="tabla" cellspacing="0" rules="all" border="1" style="border-collapse:collapse; text-decoration:none;">
            <tr><th colspan="4">Progresión de los actos administrativos</th></tr>
            <tr>
                <td><div id ="xtitle" runat="server"></div></td>
                <td width="7%" align="center"><asp:LinkButton ID="Linkrecargar" runat="server">Recargar</asp:LinkButton></td>
                <td width="22%" align="center"><div id ="otraEtapa" runat="server"></div></td>
                <td width="15%" align="center"><a href="javascript:self.close();">cerrar ventana</a></td>
            </tr>
        </table>
        <asp:GridView ID="GridDep" runat="server" AutoGenerateColumns="False" CssClass="tabla">
            <Columns>
                <asp:ButtonField CommandName="select" DataTextField="DEP_CODACTO" 
                    HeaderText="Cod." Text="Botón">
                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                </asp:ButtonField>
                <asp:BoundField DataField="DEP_NOMBREPPAL" HeaderText="Descripcion" />
                <asp:BoundField DataField="DEP_DEPENDENCIA" HeaderText="Dep." >
                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DEP_DESCRIPCION" HeaderText="Descripcion" />
                <asp:BoundField DataField="DEP_TERMINO" HeaderText="Termino" >
                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DEP_ORDEN" HeaderText="Orden" >
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
    <table class="tabla" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;">
            <tr>
                <td colspan="2">&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td><div id="resultado" runat="server"></div></td>
                <td width="8%"><asp:LinkButton ID="LinkImprimir" runat="server">Imprimir</asp:LinkButton></td>
            </tr>
        </table>
    <br />
    <input type="hidden" id="control" runat="server" />
    
    
    </form>
</body>
</html>

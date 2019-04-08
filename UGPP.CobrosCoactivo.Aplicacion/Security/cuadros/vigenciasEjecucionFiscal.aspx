<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="vigenciasEjecucionFiscal.aspx.vb" Inherits="coactivosyp.vigenciasEjecucionFiscal" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Vigencias</title>
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
        .to
        {
        color:#FF0000;
        }
   </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:left;" id="conten" runat="server">
        <table width="100%" class="tabla">
          <tr>
           <th colspan="6" style="text-align:left;font-size:15px">Vigencias del deudor</th>
          </tr>
          <tr>
           <td colspan="6"><div id="deudor" runat="server"></div></td>
          </tr>
          <tr>
           <th style="width:10%">Predio :</th>
           <td style="width:19%"><div id="Predio" runat="server"></div></td>
           <th style="width:12%">Vigencias :</th>
           <td><div id="vigencia" runat="server"></div></td>
           <th style="width:12%">Expedientes :</th>
           <td><div id="tdExpedientes" runat="server"></div></td>
          </tr>
          <tr><th colspan="6" style="text-align:left;font-size:15px;width:100%"><%  Response.Write(Session("ssCodimpadm"))%></th></tr>
        </table>
        <br />
        <asp:GridView ID="dtgViewVigencias" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="tabla">
            <Columns>
                <asp:BoundField DataField="PerCod" HeaderText="Año" />
                <asp:BoundField DataField="liqTot" HeaderText="Deuda" 
                    DataFormatString="{0:N}" />
                <asp:BoundField DataField="liqint" DataFormatString="{0:N}" 
                    HeaderText="Interes" />
                <asp:BoundField DataField="LiqTotAbo" DataFormatString="{0:N}" 
                    HeaderText="Abono" />
                <asp:BoundField DataField="subtot" DataFormatString="{0:N}" 
                    HeaderText="Sub Total" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
        <table width="100%" class="tabla"  id ="tablatotal" runat="server">
            <tr><th colspan="3">Totales</th></tr>
            <tr>
             <td>Total Deuda :<div id="Tdeuda" runat="server" class="to">0,0</div></td>
             <td>Total Interes :<div id="Tinteres" runat="server" class="to">0,0</div></td>
             <td>Total Abono :<div id="TAbono" runat="server" class="to">0,0</div></td>
            </tr>
            <tr>
             <th colspan="2" style="font-size:18px">Total De la deuda :</th>
             <th style="text-align:center;font-size:18px"><div id="Total" runat="server" class="to">0,0</div></th>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

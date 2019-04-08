<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SepararEquipos.aspx.vb" Inherits="coactivosyp.SepararEquipos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../css/Objetos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .Botones /* Estilo de los botones */
        {
        height: 25px; 
        background-color: #F2F2F2; border-bottom: 1px solid #555555;
        border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 12px;
        color: #000; 
        padding-left: 20px; background-repeat: no-repeat; cursor:hand; cursor:pointer;
        outline-width:0px;
        background-position: 4px 4px;outline-width:0px;
        }
        .Botones:hover /* Efectos del Mouse en los botones */
        {
        height: 25px; background-color: #cccccc; border-bottom: 1px solid #000;
        border-right:1px solid #000; border-top:0px; border-left:0px;
        font-size:12px; color:#000;padding-left: 20px; background-repeat: no-repeat;
        cursor:hand; cursor:pointer;outline-width:0px;
        }
    </style>
 </head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
         <tr>
             <th>
                Informes masivos generados 
             </th>
         </tr>
         <tr>
             <td align="left">
             <img src="../images/icons/Help-and-Support.png" style="float: left; margin-right:5px;"  alt="" />
              <div style="color:#0B3861">Al presionar el botón separa, tenga en cuenta que el documento masivo previamente diligenciado será dividido indexándolo  automáticamente en la base de datos, lo cual evita que el usuario escanee los documentos u hojas de forma manual.</div>
              <b style="color:#045FB4">Nota:  Esté proceso es irreversible.  </b> 
             </td>
         </tr>
        </table>
        <asp:GridView ID="GriDatos" runat="server" CssClass="tabla" Width="100%" 
            GridLines="None" AutoGenerateColumns="False" AllowPaging="True" 
            PageSize="4">
            <Columns>
                <asp:ButtonField DataTextField="DELL_DOCUMENTO" HeaderText="Documento" 
                    CommandName="Select">
                <HeaderStyle Width="60px" />
                </asp:ButtonField>
                <asp:BoundField DataField="DELL_ACTO" HeaderText="Acto">
                <HeaderStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" />
                <asp:BoundField DataField="DELL_FECHA" HeaderText="Fecha" 
                    DataFormatString="{0:d}">
                <HeaderStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="NOMBREMUNI" HeaderText="Ente cobrador" />
            </Columns>
        </asp:GridView>
        <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
         <tr>
             <th colspan="2">
                 <asp:Label ID="lbldetalle" runat="server" Text="Label"></asp:Label>
             </th>
         </tr>
          <tr>
             <th style="font-size:18px">
                Procesos...
             </th>
              <td style="text-align:center;font-size:16px">
                <b><asp:Label ID="lblProceso" runat="server" style="color:#045FB4"
                      Text="Elija un documento y presione el botón &quot;Separar Archivos&quot;"></asp:Label></b>
              </td>
         </tr>
        </table>
        <br />                    
        <asp:Button ID="btnSeperar" runat="server" CssClass="Botones" style="background-image: url('../images/icons/186.png');" Text="Separar Archivos" ValidationGroup="textovalidados" Width="150px" />
    </div>
    </form>
</body>
</html>

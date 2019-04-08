<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="userauditoria.aspx.vb" Inherits="coactivosyp.userauditoria" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

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
        .style1
        {
            color: #7F0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
         <tr>
             <th>Detalle de los registro por usuario</th>
         </tr>
         <tr>
             <td align="left">
              <img src="../images/icons/Help-and-Support.png" style="float: left; margin-right:5px;"  alt="Ayuda" />
              <div style="color:#0B3861">En esta ventana&nbsp;  podrá verificar, recopilará y auditar toda la información o registros  almacenados por un usuario en específico, En el momento que se necesite información precisa y actualizada según las labores de un usuario.</div>
              <b style="color:#045FB4">Nota:  Reduce la necesidad de archivos voluminosos en papel.  </b> 
             </td>
         </tr>
    </table>
    <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;display:None;" runat="server" id="no_reg">
      <tr>
        <td style="padding:5px !important;">
            <span class="style1" 
                style="font-family: Arial, Helvetica, sans-serif; font-size: 19px;">
              <img src="../images/icons/Stop.png"  width="60" height="60" style="float:left;margin-right:5px;"  alt="Ayuda" />Atención al <b style="color:#045FB4 !important;">
            usuario</b>... Los criterios criterios de búsqueda que ha suministrado a este servicio precisan que 
            <b style="color:#045FB4 !important;">el usuario esté registrado, pero no se reconoce ningún movimiento en la base de datos.</b></span>
        </td>
      </tr>
    </table>
    <asp:GridView ID="GridView_user" runat="server" CssClass="tabla" Width="100%" 
            AllowPaging="True" PageSize="9" AutoGenerateColumns="False" 
            meta:resourcekey="GridView_userResource1">
        <Columns>
            <asp:BoundField DataField="entidad" HeaderText="Entidad" 
                meta:resourcekey="BoundFieldResource1">
            <HeaderStyle Width="40px" />
            </asp:BoundField>
            <asp:BoundField DataField="nomente" HeaderText="Nombre" 
                meta:resourcekey="BoundFieldResource2" />
            <asp:BoundField DataField="nomactuacion" HeaderText="Acto" 
                meta:resourcekey="BoundFieldResource3" />
            <asp:BoundField DataField="docpredio_refecatrastal" HeaderText="Ref. Catras." 
                meta:resourcekey="BoundFieldResource4" >
            <HeaderStyle Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="docexpediente" HeaderText="Expediente" 
                meta:resourcekey="BoundFieldResource5" >
            <HeaderStyle Width="43px" />
            </asp:BoundField>
            <asp:BoundField DataField="docfechasystem" DataFormatString="{0:g}" 
                HeaderText="Fech. Sys" meta:resourcekey="BoundFieldResource6" />
        </Columns>
    </asp:GridView>
    <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
         <tr>
             <th colspan="2">
                 <asp:Label ID="lbldetalle" runat="server" Text="Label" 
                     meta:resourcekey="lbldetalleResource1"></asp:Label>
             </th>
         </tr>
          <tr>
             <th style="font-size:18px">
                Procesos...
             </th>
              <td style="text-align:center;font-size:16px">
                <b>
                  <asp:Label ID="lblProceso" runat="server" style="color:#045FB4"
                      Text="-Usuario-" meta:resourcekey="lblProcesoResource1"></asp:Label></b>
              </td>
         </tr>
        </table>
        <br />                    
        <asp:Button ID="btnSeperar" runat="server" CssClass="Botones" 
              style="background-image: url('../images/icons/_user.png');" 
              Text="Intentar con otro usuario" 
                Width="169px" meta:resourcekey="btnSeperarResource1" />
     </div>
    </form>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usearuditoria_lista.aspx.vb" Inherits="coactivosyp.usearuditoria_lista" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
             <th>Detalle de los registro por usuario</th>
         </tr>
         <tr>
             <td align="left">
              <img src="../images/usuarios.png"  width="55"  height="55" style="float: left; margin-right:5px;"  alt="Ayuda" />
              <div style="color:#0B3861">En esta lista se encontrara almacenados todos los usuarios, cliente o  personas que se conectan al sistema (tecno expedientes) para hacer uso de los servicios que este les proporciona. Seleccione uno de ellos para continuar con la auditoria.</div>
              <b style="color:#045FB4">Nota:  Dentro de los usuarios del sistema podemos distinguir diferentes perfiles o niveles de usuario.  </b> 
             </td>
         </tr>
    </table>
    <asp:GridView ID="dtgUsuarios" runat="server" CssClass="tabla" Width="100%" 
         AutoGenerateColumns="False" Font-Size="13px" 
         AllowPaging="True" AllowSorting="True">
        <RowStyle HorizontalAlign="Left" />
        <Columns>
            <asp:ButtonField CommandName="Select" DataTextField="Codigo" 
                HeaderText="Codigo" ImageUrl="../images/icons/_user.png" 
                ShowHeader="True" >
                <HeaderStyle Width="50px" />
                <ItemStyle Width="50px" />
            </asp:ButtonField>
            <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="nivelacces" HeaderText="Nivel" >
                <HeaderStyle Width="40px" HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Center" Width="40px" />
            </asp:BoundField>
        </Columns>
        <HeaderStyle 
            HorizontalAlign="Left" />
    </asp:GridView>
    <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
         <tr>
             <th>
                 <asp:Label ID="lbldetalle" runat="server" Text="Label"></asp:Label>
             </th>
         </tr>
         <tr>
             <td>
               <h3 style="margin:0px; padding:0px;color:#045FB4;">Tecno Expedientes !</h3>
			   <h3 style="margin:0px; padding:0px;color:#045FB4;"> <%  Response.Write(Session("ssCodimpadm") & ".")%> </h3>
             </td>
         </tr>
        </table>
        <table class="tabla" style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                <tr><th colspan="3" align="center">Usuario Activo - Usuario con el cual se encuentra auditando.</th></tr>
                <tr><td rowspan="5" style="height:100px;width:100px;" ><img alt = "" src="../imagenes/user3_128x128.png" width="100" height="100" /></td></tr>
                <tr>
                      <td style="width:60px;"><b>Nombre</b></td>
                      <td><asp:Label ID="lblNombre" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr>
                      <td><b>Cedula o Id.</b></td>
                      <td><asp:Label ID="lblcedula" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr>
                      <td><b>Login</b></td>
                      <td><asp:Label ID="lblLogin" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr>
                      <td><b>Codigo</b></td>
                      <td><asp:Label ID="lblcodigo" runat="server" Text="##########"></asp:Label></td>
                 </tr>
                 <tr><th colspan="3" align="center"><asp:Label ID="lbl_detalle" runat="server" Text="##########"></asp:Label></th></tr>
           </table>
     </div>
    </form>
</body>
</html>

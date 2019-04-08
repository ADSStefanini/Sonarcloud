<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="consultapredialvariable036.aspx.vb" Inherits="coactivosyp.consultapredialvariable036" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cobranzas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .Yup
        {
        padding:10px;
        text-align:center;
        color: #FFFFFF;
        }
        .xlist
        {
        text-align:left;
        font-family: Verdana;
        font-size: 11px;
        font-weight: bold;
        font-style: normal;
        color: #FFFFFF;
        }
        .divhisto
        {
        /*border: 1px solid #dcdbe0; */
        background-color:#507CD1;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        }
        .CajaDialogo
        {
        position:absolute;
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #72A3F8;
        padding: 0px;
        color:#514E4E;
        font-weight: bold;
        font-size:12px;
        font-style: italic;
        }
        .palanca
        {
        color: #34484E;         
        }
        .ppala{width:520px;}
        .ppala .cl
        {
            border: 1px solid #999;
            padding-left:3px;
            width:190px;
        }
          .ppala .cl2
        {
            border: 1px solid #999;
            padding-left:3px;
            
        }
    </style>
       
</head>
<body>
<form id="form1" runat="server">

  <div id="container">
    <h1 id="Titulo"><a href="#">Cobranzas</a></h1>
  

     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>        
  
  
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ErrorMessage="El campo <strong>NUMERO EXPEDIENTE</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtNumExp"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ErrorMessage="El campo <strong>CODIGO ACTO ADMINISTRATIVO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtCalendario"  Display="None"></asp:RequiredFieldValidator>
          
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator2"></asp:ValidatorCalloutExtender>   
     
     
     
     
     <div class="Yup divhisto" 
          
          style="width: 713px; position:absolute;top:210px; left:24px; background-color:#507CD1; z-index:1;">
         <h4>Informes Especiales</h4>
         
         <table style="text-align:left; font-size:11px;" class = "ppala" align="center">
            <tr><td  class="cl" style="width: 224px">Numero Expediente :</td><td class="cl2"><asp:TextBox ID="txtNumExp" runat="server" style="text-align:center;" AutoPostBack="true"></asp:TextBox></td></tr>
            <tr><td  class="cl" style="width: 224px">Tipo de exepción :</td><td class="cl2">
                <ASP:DROPDOWNLIST ID="DropNumero_expediente" runat="server" Height="19px" 
                        Width="376px">
                        <asp:ListItem Value="El pago efectivo.">1. El pago efectivo.</asp:ListItem>
                        <asp:ListItem Value="La existencia de acuerdo de pago. ">2. La existencia de acuerdo de pago. </asp:ListItem>
                        <asp:ListItem Value="La falta de ejecutoria del título">3. La falta de ejecutoria del título</asp:ListItem>
                        <asp:ListItem Value="La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.">4. La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.</asp:ListItem>
                        <asp:ListItem Value="La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. ">5. La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. </asp:ListItem>
                        <asp:ListItem Value="La prescripción de la acción de cobro. ">6. La prescripción de la acción de cobro. </asp:ListItem>
                        <asp:ListItem Value="La falta de título ejecutivo o incompetencia del funcionario que lo profirió">7. La falta de título ejecutivo o incompetencia del funcionario que lo profirió</asp:ListItem>
                </ASP:DROPDOWNLIST>
                </td>
             </tr>
           
            <tr><td  class="cl" style="width: 224px">Fecha del acto</td><td class="cl2"><asp:TextBox ID="txtCalendario" runat="server" style="text-align:center;"></asp:TextBox></td>
             </tr>
           
         </table>
         
         <br />
         <br />
         
         <asp:Button ID="LinkButton1" runat="server" Text="Guardar" CssClass="Botones" 
             style ="background-image: url('images/icons/add_small.png');" 
             Width="95px" ValidationGroup="textovalidados" />
         
                    
       </div>
         
       <h4 id ="acto" runat="server" 
         
          style="position:absolute;top:44px; left:24px; width: 733px; text-transform: uppercase; text-align:center;" >Titulo</h4>
         
       <div style="position:absolute;top:106px; left:24px; width: 602px;" 
         class="divhisto">
            <table width="100%">
                <tr>
                 <th style="text-align: right;font-size:11px;padding:4px; width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                 <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;width:90px">Ente :</th>
                 <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;" class="palanca"><asp:Label ID="lblCobrador" runat="server" Text=""></asp:Label></th>
                 <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><asp:LinkButton ID="LinkCancelar" runat="server"><img src="images/icons/cancel.png" alt="" /></asp:LinkButton></th>
                </tr>
            </table>
       </div>
           
       <asp:customvalidator style="position:absolute;top:177px; left:24px; width: 735px;" 
         id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="13px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
    
    </div> 
</form>
</body>
</html>


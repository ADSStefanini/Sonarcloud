<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="capturatrasladoavaluo.aspx.vb" Inherits="coactivosyp.capturatransladoavaluo" %>
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
  
  
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ErrorMessage="El campo <strong>NOMBRE DE LA EMPRESA</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtperito"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ErrorMessage="El campo <strong>NUMERO DE LA CUENTA</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtavaluo"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ErrorMessage="El campo <strong>TOTAL MONTO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtfecha"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"  ErrorMessage="El campo <strong>TOTAL MONTO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txthonorarioperito"  Display="None"></asp:RequiredFieldValidator>
     
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator2"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator3"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RequiredFieldValidator4"></asp:ValidatorCalloutExtender>   
     
     
     
     <div class="Yup divhisto" 
          
          style="width: 713px; position:absolute;top:275px; left:24px; background-color:#507CD1; z-index:1;">
         <h4>Informe</h4>
         
         <table style="text-align:left; font-size:11px;" class = "ppala" align="center">
            <tr><td  class="cl">Nombre del perito: </td><td class="cl2"><asp:TextBox ID="txtperito" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Avalúo bienes embargados y secuestrados: </td><td class="cl2"><asp:TextBox ID="txtavaluo" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Fecha bienes embargados y secuestrados: </td><td class="cl2"><asp:TextBox ID="txtfecha" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Honorario perito: </td><td  class="cl2"><asp:TextBox ID="txthonorarioperito" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
         </table>
         
         <br />
         <br />
         
         <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="Botones" 
             style ="background-image: url('images/icons/printer.png');" 
             Width="95px" ValidationGroup="textovalidados" />
         
             <asp:CalendarExtender ID="txtFechacreacion_CalendarExtender" runat="server"
            TargetControlID="txtfecha"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"/>
                    
       </div>
         
       <h4 id ="acto" runat="server" 
         
          style="position:absolute;top:44px; left:24px; width: 733px; text-transform: uppercase; text-align:center;" >Titulo</h4>
       <div style="position:absolute;top:170px; left:24px; width: 602px; right: 225px;" 
         class="divhisto">
            <table width="100%">
                <tr>
                 <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><img src="images/icons/script.png" alt="" /></th>
                 <th style="font-size:11px;text-align:left;padding:4px;width:90px">EXPEDIENTE :</th>
                 <th style="text-align: right;font-size:11px;padding:4px;" class="palanca"><asp:Label ID="lblExpediente" runat="server" Text="000000"></asp:Label></th>
                 <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><asp:LinkButton ID="LinkCancelar" runat="server"><img src="images/icons/cancel.png" alt="" /></asp:LinkButton></th>
                </tr>
            </table>
       </div>
     
       <div style="position:absolute;top:106px; left:24px; width: 602px;" 
         class="divhisto">
            <table width="100%">
                <tr>
                 <th style="text-align: right;font-size:11px;padding:4px; width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                 <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;width:90px">Ente :</th>
                 <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;" class="palanca"><asp:Label ID="lblCobrador" runat="server" Text=""></asp:Label></th>
                </tr>
            </table>
       </div>
       <div style="position:absolute;top:138px; left:24px; width: 602px;" 
         class="divhisto">
            <table width="100%">
                <tr>
                 <th style="text-align: right;font-size:11px;padding:4px; width:16px;"><img src="images/icons/vcard.png" alt="" /></th>
                 <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;width:90px">
                     DEUDOR :</th>
                 <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;" class="palanca">
                     <asp:Label ID="lblDeudor" runat="server"></asp:Label></th>
                </tr>
            </table>
       </div>
       
    
       <asp:customvalidator style="position:absolute;top:237px; left:24px; width: 735px;" 
         id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="13px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
    
    </div> 
</form>
</body>
</html>


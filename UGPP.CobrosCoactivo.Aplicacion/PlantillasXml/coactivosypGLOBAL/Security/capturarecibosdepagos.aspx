<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="capturarecibosdepagos.aspx.vb" Inherits="coactivosyp.capturarecibosdepagos" %>
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
<!-- Definicion del menu -->  
  <div id="message_box">
        <ul style="width:36px; height:188px">
             <li style="height:36px;width:36px !important;">
                <a href="cobranzas2.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
             </li>
             <li style="height:152px;width:36px;">
                <a href="cobranzas2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px; margin:0px" /></a>
             </li>
        </ul>
  </div>
  
<form id="form1" runat="server">

  <div id="container">
     <h1 id="Titulo"><a href="javascript:void(0)">Cobranzas - <% Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
     <div id="selectoresBus" style="position:absolute;top:60px;left:50px;width:700px;">
        <div id="rerein" style="background-color:#507CD1;width:130px;float:left;padding-right:5px;"><a href="cobranzas2.aspx" class="Ahlink" title="Regresar reportes individuales">Reportes individuales</a></div>
        <div id="cobra" style="background-color:#507CD1;width:100px;float:left;padding-right:5px;"><a href="cobranzatipo.aspx" class="Ahlink" title="Regresar al escritorio cobranzas">Cobranzas</a></div>
        <div id="Div2" style="background-color:#507CD1;width:100px;float:left;padding-right:5px;"><a href="cobranzatipo.aspx" class="Ahlink" title="Regresar tipo informe">Tipo informe</a></div>
        <div id="Div1" style="background-color:#507CD1;width:140px;float:left;"><a href="ejecucionesFiscales.aspx" class="Ahlink" title="Regresar ejecuciones fiscales">Ejecuciones fiscales</a></div>
     </div>
    
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>        
  
  
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ErrorMessage="El campo <strong>EL NUMERO DE RECIBO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtnro"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ErrorMessage="El campo <strong>LA FECHA DE RECIBO</strong> es requerida. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtfecha"  Display="None"></asp:RequiredFieldValidator>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  ErrorMessage="El campo <strong>EL VALOR DE RECIBO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "txtvalor"  Display="None"></asp:RequiredFieldValidator>
   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RequiredFieldValidator1"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator2"></asp:ValidatorCalloutExtender>   
     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RequiredFieldValidator3"></asp:ValidatorCalloutExtender>   
     
     
     <div class="Yup divhisto" 
          
          style="width: 713px; position:absolute;top:100px; left:24px; background-color:#507CD1; z-index:1;">
         <h4>REGISTRO DE RECIBOS DE PAGOS</h4>
         
         <table style="text-align:left; font-size:11px;" class = "ppala" align="center">
         <tr><td  class="cl">Expediente : </td><td class="cl2"><asp:TextBox ID="txtExpediente" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Nro de recibo : </td><td class="cl2"><asp:TextBox ID="txtnro" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Fecha de recibo : </td><td class="cl2"><asp:TextBox ID="txtfecha" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            <tr><td  class="cl">Valor de recibo : </td><td class="cl2"><asp:TextBox ID="txtvalor" runat="server" style="text-align:center;"></asp:TextBox></td></tr>
            
         </table>
         
                           
         <asp:Button id="btnAceptar" runat="server" Text="Guardar" 
         style="position:absolute;top:186px; left:83px; width: 92px; background-image: url('images/icons/46.png');" 
         CssClass="Botones" ValidationGroup="textovalidados"></asp:Button>
     
        <asp:Button id="btnCancelar" runat="server" Text="Cancelar"  style="position:absolute;top:186px; left:182px; width: 92px; background-image: url('images/icons/cancel.png');" CssClass="Botones"></asp:Button>
        <asp:CalendarExtender ID="txtFechacreacion_CalendarExtender" runat="server"
            TargetControlID="txtfecha"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"/>
         
       </div>
       <asp:customvalidator style="position:absolute;top:37px; left:24px; width: 735px;" id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="13px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
    </div> 
        <!-- Error y cerrar sesion -->
        <asp:Panel ID="pnlError" runat="server" CssClass="CajaDialogoErr" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
              <div id="logo">
                  <h1><a href="javascript:void(0)" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
                  <p id="slogan">Gestión Documental.</p>
              </div>
              <div style="margin: 0  0 5px 0; ">
                 <% 
                     If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                         Response.Write(Me.ViewState("Erroruseractivo"))
                     End If
                 %>
              </div>
    		  <hr />	
		    				    
			  <asp:Button style="Z-INDEX: 116; width: 75px;" id="btnNoerror"
				    runat="server" Text="Aceptar" Height="23px" CssClass="RedButton"></asp:Button>    
        </asp:Panel>
        
        
<asp:Button ID="Button3" runat="server" Text="Button" style="visibility:hidden" />
		
        <asp:ModalPopupExtender ID="ModalPopupError" runat="server" 
            TargetControlID="Button3"
            PopupControlID="pnlError"

            CancelControlID="btnNoerror"
            OnCancelScript="mpeSeleccionOnCancel()"
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
        >
        </asp:ModalPopupExtender>
        <script type="text/javascript">
               function mpeSeleccionOnCancel() {
                   var pagina = '../login.aspx'
                   location.href = pagina
                   return false;
               }
        </script>
        
        
</form>
</body>
</html>


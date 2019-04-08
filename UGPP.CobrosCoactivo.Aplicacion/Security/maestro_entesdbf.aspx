<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="maestro_entesdbf.aspx.vb" Inherits="coactivosyp.maestro_entesdbf" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Entrada de Deudores</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" /> 
    <link href="css/Objetos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
      .CajaDialogo
      {
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #72A3F8;
        padding: 0px;
        color:#514E4E;
        /* font-weight: bold; */
        font-size:12px;
      }
      .FondoAplicacion
      {
        background-color: black;
        filter: alpha(opacity=70);
        opacity: 0.7;
      }
      a.Ntooltip:hover
      {
        background-color:#507CD1;
      }
     </style>
     
     <script language="javascript" type="text/javascript">
        function mpeSeleccionOnCancel()
        {
            document.getElementById('txtCodigo').focus();
            document.getElementById('txtBuscarConsul').value = "";
            return false;
        }
      
        function Jcancelar()
        {
           document.getElementById('txtNombre').value = "";
           document.getElementById('txtCodigo').value = "";
           document.getElementById('txtDireccion').value = "";
           document.getElementById('txtTelefono').value = "";
           document.getElementById('txtCodigo').focus();
           return false;
       }
        
     </script>
     
     <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
     <script type="text/javascript">
        $(document).ready(function() {
            $(window).scroll(function(){
	  		    $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
	        });
	        $("#LinkBuscar").click(function() {
	            $("#txtBuscarConsul").focus();
	        });
		});   
	</script>
	<script language="javascript" type="text/javascript">
	    function ShowOptions(control, args) {
	        control._completionListElement.style.zIndex = 10000001;
	    }
    </script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('a.Ntooltip').hover(function() {
                $(this).find('span').stop(true, true).fadeIn("slow");
            }, function() {
                $(this).find('span').stop(true, true).hide("slow");
            });
        });
    </script>
</head>
<body>

   <!-- Definicion del menu -->  
   <div id="message_box">
    <ul style="width:36px; height:188px">
     <li style="height:36px;width:36px !important;">
        <a href="MenuMaestros.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
     </li>
     <li style="height:152px;width:36px;">
        <a href="MenuMaestros.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
     </li>
    </ul>
   </div>
   
  <div id="container">
    <h1 id="Titulo"><a href="#">Entrada de Deudores (Propietario)</a></h1>
    <form id="form1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    
    
    <div style="position:absolute; top: 158px; left: 59px; z-index:1001; color: #FFFFFF; font-size: 14px; font-weight: 700;">
            Nit/Cedula :
        </div>
        
        <div style="position:absolute; top: 309px; left: 58px; z-index:1001; color: #FFFFFF;font-size: 14px; font-weight: 700;">
            Telefono :
        </div>
        
        <div style="position:absolute; top: 208px; left: 59px; z-index:1001; color: #FFFFFF;font-size: 14px; font-weight: 700;">
            Nombre :
        </div>
        
        <div style="position:absolute;background-color:#2461BF; top: 127px; left: 36px; width: 349px; height: 284px;border: 1px double #EFF3FB;">
        </div>
        
        <asp:TextBox ID="txtCodigo" runat="server" 
               
        
        style="top: 180px; left: 59px; position: absolute;width: 141px; text-align:center;z-index:1001;" AutoPostBack="True" 
               ></asp:TextBox>
               
        <div style="position:absolute; top: 260px; left: 59px; z-index:1001; color: #FFFFFF;font-size: 14px; font-weight: 700;">
            Direccion :
        </div>
        
        <asp:TextBox ID="txtDireccion" runat="server" 
        style="top: 281px; left: 59px; position: absolute; width: 304px; z-index:1001; right: 417px;"></asp:TextBox>
        
        <asp:TextBox ID="txtTelefono" runat="server" 
        
        
        
        style="top: 328px; left: 59px; position: absolute; width: 141px; z-index:1001; right: 580px;"></asp:TextBox>
        
        <asp:TextBox ID="txtNombre" runat="server" 
        style="top: 231px; left: 59px; position: absolute; width: 304px; z-index:1001; right: 417px;"></asp:TextBox>
        
        
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
               style="top: 372px; left: 169px; position: absolute; width: 98px; z-index:5;background-image: url('images/icons/cancel.png');" 
                   CssClass="Botones" OnClientClick="return Jcancelar();" />
        
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
               style="top: 372px; left: 60px; position: absolute; width: 98px; z-index:5 ;background-image: url('images/icons/45.png');" 
                   CssClass="Botones" ValidationGroup="textovalidados" />
        
    <div style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 414px; left: 36px; padding:7px; width: 338px;" 
        id ="Dtal" runat="server">...</div>
    
     <div id ="Buscar" runat="server" 
         style="position:absolute;background-color:#507CD1; top: 104px; left:53px;">
         <asp:LinkButton ID="LinkBuscar" runat="server" CssClass="Ahlink">
         
         <img alt ="" style="float:left" src="images/icons/magnify.png" />
         &nbsp; Buscar propietarios creados. &nbsp;
         </asp:LinkButton>
    </div>
    
    
    <div id ="cobradortv" runat="server" 
        style="position:absolute;background-color:#507CD1; top: 441px; right:392px; color:#ffffff;font-size:11px;padding:5px">
         Cobrador
    </div>
    
    <div id ="Div1" style="position:absolute;background-color:#507CD1; top: 128px; left:402px; color:#ffffff;font-size:11px;width: 360px;">
           <div style="background-color:#507CD1; height:20px; padding:7px">
               <a class="Ntooltip" href="#"  style="position:absolute;left:5px;top:10px;z-index:225;width: 17px; height: 18px; right: 712px;">
                  <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                  <span style="z-index:225;">
                    <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                      Nota : Op. Consultas Rápidas 
                    </b>
                    <br /><br />
                    Con estas consultas podrá verificar o auditar datos de los diferentes documentos asociados a un deudor. 
                    
                    <br />
                    <br />
                    
                    <b>Ejemplo :</b> Expedientes mal digitalizados. 
                  </span>
                </a>
                <div style=" padding-left:20px; padding-top:4px;">Accesos rápidos y consultas</div>
           </div>
           
           <div style="background-color:#fff; padding:5px;">
              <asp:Button ID="btnracExpediente" runat="server" 
                   Text="¿Cuántos expedientes tiene digitalizados?" 
                   style="z-index:5;background-image: url('images/icons/24.png');" 
                   CssClass="Botones" ValidationGroup="textovalidados" Width="349px" />
              <br /><br />
              <asp:Button ID="queExpediente" runat="server" 
                   Text="¿Qué documentos tiene digitalizado este deudor?" 
                   style="z-index:5;background-image: url('images/icons/24.png');" 
                   CssClass="Botones" ValidationGroup="textovalidados" Width="349px" />     
           </div>        
    </div>
    
    <div id = "Messenger"  runat="server" style="position:absolute; top: 50px; left: 46px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
        
        <a href ="MenuMaestros.aspx" 
        style="position:absolute;top:969px; left: 349px;color:#ffffff;font-size:14px; text-decoration:none;	float: left;">
        <img src="images/icons/61.png" alt="" style="	float: left;" />Menu Principal</a>
        
        <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" 
                    
        style="height: 112px; width: 697px; display:none;" Width="697px"> 
                    <asp:Button style="width: 105px; top: 80px; left: 108px; position: absolute;background-image: url('images/icons/cancel.png');" id="btnNo"
                        runat="server" Text="Cancelar" CssClass="Botones"></asp:Button>
                    	
                    <asp:Button style="width: 84px; top: 80px; left: 16px; position: absolute;background-image: url('images/icons/okay.png');" id="btnSi"
                        runat="server" Text="Enviar"  CssClass="Botones"></asp:Button>
                    
                    <asp:Label ID="Label2" runat="server" Text="Buscar Deudor :" 
                         
                        style="top: 19px; left: 15px; position: absolute; height: 15px; width: 132px"></asp:Label>
                    
                    <asp:TextBox ID="txtBuscarConsul" runat="server" autocomplete="off"
                        style="width: 660px; top: 43px; left: 17px;position: absolute"></asp:TextBox>
         </asp:Panel> 
                 
                               
         <asp:ModalPopupExtender ID="mpeSeleccion" runat="server" 
                TargetControlID="LinkBuscar"
                PopupControlID="pnlSeleccionarDatos"
                CancelControlID="btnNo"
                OnCancelScript="mpeSeleccionOnCancel()" 
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
                >
          </asp:ModalPopupExtender>
               
          <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" OnClientShown="ShowOptions"
                    enabled="True" 
                    targetcontrolid="txtBuscarConsul" 
                    servicemethod="ObtListaEtidades" 
                    ServicePath="Servicios/Autocomplete.asmx"
                    MinimumPrefixLength="1" 
                    CompletionInterval="1000"
                    EnableCaching="true"
                    CompletionSetCount="15" 
                    CompletionListCssClass="CompletionListCssClass"
                    CompletionListItemCssClass="CompletionListItemCssClass"
                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"  
          >
          </asp:AutoCompleteExtender>
       
       
          <asp:RequiredFieldValidator ID="rfvCedula" runat="server"  ErrorMessage="El campo <strong>CEDULA</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtCodigo" Display="None"></asp:RequiredFieldValidator>

          <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="rfvCedula">
          </asp:ValidatorCalloutExtender>
       
          <asp:RequiredFieldValidator ID="rfvNombre" runat="server"  ErrorMessage="El campo <strong>NOMBRE</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtNombre" Display="None"></asp:RequiredFieldValidator>

          <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvNombre">
          </asp:ValidatorCalloutExtender>
          
          <asp:Panel ID="Panel1" runat="server" CssClass="CajaDialogo" style="z-index:1001; width:200px;display:none; padding:11px;">
                <div>
                <table width="100%" class="tabla" style="text-align:center;border-collapse:collapse;" cellspacing="0" rules="all" border="1">
                <tr>
                <td><div id = "pp">PRESIONE UN EXPEDIENTE PARA EXAMINARLO </div></td>
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
                <td><div id = "Divtota" runat="server"></div></td>
                </tr>
                </table>
                <br />
                <asp:Button ID="btncanbott" runat="server" Text="Aceptar"  style="width: 98px; z-index:5 ;background-image: url('images/icons/accept.png');"  CssClass="Botones" />
                </div>
          </asp:Panel>
          <asp:Button ID="Button1" runat="server" Text="Button" style="visibility:hidden"  />
          <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                TargetControlID="Button1"
                PopupControlID="Panel1"
                CancelControlID="btncanbott"
                OnCancelScript="mpeSeleccionOnCancel()" 
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
                >
          </asp:ModalPopupExtender>
          
          <!-- documentos asociados al deudor -->
           <asp:Panel ID="datosinf" runat="server" CssClass="CajaDialogo" style="z-index:1001; width:630px; margin-left: auto;margin-right: auto; text-align:left;display:none; padding:11px;">
               <div style="width:100%;height:500px;overflow:auto;">
                <table class="tabla">
                <tr><th colspan="2" style="text-align: center;">Examen completo del expediente </th></tr>
                <tr>
                <th>ENTIDAD : </th><td><% Response.Write("(ID " & Me.ViewState("entidad") & ") - " & Me.ViewState("nombre"))%></td>
                </tr>
                <tr>
                <th>EXPEDIENTE : </th><td><% Response.Write(Me.ViewState("docexpediente"))%></td>
                </tr>
                <tr><th colspan="2" style="text-align: center;"> ACTOS ADMINISTRATIVOS </th></tr>
                </table> 

                <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>

                </HeaderTemplate>
                <SeparatorTemplate>
                <table class="tabla">
                <tr>
                <td><b> &nbsp; </b> <br /> </td>
                </tr>
                </table>  
                </SeparatorTemplate>
                <ItemTemplate>
                <table class="tabla">
                <tr>
                <th>ACTUACION : </th><td><%#Eval("idacto") & " - " & Eval("nombreacto")%></td>
                </tr>
                <tr>
                <th>NOMBRE DEL ARCHIVO : </th>
                <td>
                <a href="javascript:;"  onclick="window.open('../TiffViewer.aspx?nomente=<%#Eval("nombre")%>&idente=<%#Eval("idacto")%>&F=<%#Eval("nomarchivo")%>&totimg=<%#Eval("paginas")%>&acto=<%#Eval("nombreacto")%>&idacto=<%#Eval("idacto")%>&folder=&Enabled=false&observacion=<%#Eval("docObservaciones")%>', '', 'fullscreen=yes, scrollbars=auto')"> 
                <%#Eval("nomarchivo")%>
                </a>   
                </td>
                </tr>
                <tr>
                <th>NUMERO DE PAGINA(S) : </th><td><%#Eval("paginas")%></td>
                </tr>
                <tr>
                <th>FECHA RADICACIÓN : </th><td><%#Eval("fecharadic")%></td>
                </tr>
                <tr>
                <th>FECHA CREACIÓN : </th><td><%#Eval("docfechadoc")%></td>
                </tr>
                <tr>
                <th>EXPEDIENTE : </th><td><% Response.Write(Me.ViewState("docexpediente"))%></td>
                </tr>
                </table>
                </ItemTemplate>
                <FooterTemplate>
                <table class="tabla">
                <tr><th colspan="2" style="text-align: center;">Examen completo del expediente </th></tr>
                </table> 
                </FooterTemplate>
                </asp:Repeater>
              </div>  
                <br />
                <asp:Button ID="AcepPanel2" runat="server" Text="Aceptar"  style="width: 98px; z-index:5 ;background-image: url('images/icons/accept.png');"  CssClass="Botones" />
            </asp:Panel>
            <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden"  />
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                TargetControlID="Button2"
                PopupControlID="datosinf"
                CancelControlID="AcepPanel2"
                OnCancelScript="mpeSeleccionOnCancel()" 
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
                >
            </asp:ModalPopupExtender>
          
    </form>
    </div>
</body>
</html>

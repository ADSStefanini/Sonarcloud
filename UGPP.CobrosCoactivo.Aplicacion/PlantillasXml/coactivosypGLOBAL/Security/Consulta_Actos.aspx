<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Consulta_Actos.aspx.vb" Inherits="coactivosyp.Consulta_Actos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <title>Tecno Expedientes !</title>
   <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
   <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
   <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
   <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
   <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
   <style type="text/css">    
       table.servicesT
        {	
        font-family: Verdana;
        font-weight: normal;
        font-size: 11px;
        color: #404040;
        background-color: #fafafa;
        border-collapse: collapse;
        border-spacing: 0px;
        margin: 0px;}

        table.servicesT td, table.servicesT th
        {
        border: dotted 1px #6699CC;
        font-family: Verdana, sans-serif, Arial;
        font-weight: normal;
        font-size: 10px;
        color: #404040;
        background-color: white;
        text-align: left;
        padding-left: 3px;
        
        }
        table.servicesT td.servHd:hover  
        {
        background-color: #FFFFCC !important;
        color:#F78F08 !important; 
        border: 1px solid #6699CC;
        }
        table.servicesT td.EservEHd:hover  
        {
        background-color: #FEF4F4 !important;
        color:#F80808;
        padding-left:16px;
        cursor:pointer;
        border: 1px solid #6699CC;
        background-repeat: no-repeat;
        background-color: transparent;
        background: url('images/icons/151.png') no-repeat left 50%;
        background-position : 3px 2px;
        }
        table.servicesT th {font-weight: bold;}
        table.servicesT td a {text-decoration:none;}
        .xa {color: #fff; text-decoration: none;} 
        .xa:hover {color: #fff; text-decoration: none; font-weight: bold; cursor:pointer;}
        .contenedortitulos
        {
        height:18px;
        margin:0px;
        padding:6px;

        background-image: url('images/BarraActos.png');
        background-repeat: repeat-x;

        background-color:#006699;
        /*border-bottom: #CCCCCC solid 1px; */
        text-align:left;
        color:White;
        font-family:Tahoma;
        font-size:11px;
        }
        contenedortitulos table tr th
        {
        text-align:left; 
        }
   </style>
</head>
<body>
    <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
   <div id="container">
        <h1 id="Titulo"><a href="#">Consulta de Expedientes - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div id="Label1" 
            style="position:absolute;top:58px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:bold;">Digite un acto administrativo :</div>
        <asp:TextBox ID="txtActo" runat="server" 
            style="position:absolute;top:79px;  left: 34px; width: 709px; z-index:777;"></asp:TextBox>
        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
            enabled="True" 
            targetcontrolid="txtActo" 
            servicemethod="ObtListaActuaciones" 
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
        <asp:customvalidator style="position:absolute;top:164px; left:46px; width: 702px;" id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator> 
        <asp:Button id="btnAceptar" runat="server" Text="Consultar" ValidationGroup="textovalidados"
             style="position:absolute;top:186px; left:34px; width: 92px; background-image: url('images/icons/okay.png'); z-index:10" 
             CssClass="Botones"></asp:Button>
             
        <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>ACTOS</strong> es requerido para la consulta. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtActo" Display="None"></asp:RequiredFieldValidator>
        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
                <Animations>
                <OnShow>                                    
                <Sequence>   
                <HideAction Visible="true" /> 
                <FadeIn Duration="1" MinimumOpacity="0" MaximumOpacity="1" />
                </Sequence>
                </OnShow>
                <OnHide>
                <Sequence>    
                <FadeOut Duration="1" MinimumOpacity="0" MaximumOpacity="1" />
                <HideAction Visible="false" />
                </Sequence>
                </OnHide>
                </Animations>
        </asp:ValidatorCalloutExtender>
             
        <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
             style="position:absolute;top:186px; left:133px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
             CssClass="Botones"></asp:Button>
             
        <div style="position:absolute; top: 218px; left: 34px; width: 711px; font-family:Verdana;background-color:White;">
            <div style="padding:5px;margin:0px;background-color:#507CD1;font-size:16px;color:#fff;width: 701px;overflow: hidden;">
                <img src="images/icons/help.png" width="16" height="16" alt="" style="width:16px; height:16px;vertical-align: text-top;float: left; padding-right:3px;" border="0" />
                <span  style="float: left;"> Historial completo del expediente.
                (</span><asp:Label ID="lblExpediente" runat="server" Text="Expediente" CssClass="xa" style="float: left;"></asp:Label> <span  style="float: left;">)</span> 
                <!-- herramientas -->
                <div id="ssherramienta2" title="Regresar a la consulta por deudor." runat="server" style="float:right;width:16px;height:16px;cursor:pointer;"><a href='historiaexpediente.aspx'><img src="images/icons/user_business.png" width="16" height="16" alt="" id="Img3" /></a></div>
            </div>
            <div style="padding:0px;margin:0px;background-color:White;font-size:11px;text-transform: uppercase;" id="examinar" runat="server">
                <div id="contenidogrids" runat="server">
                   <p style='padding:10px;margin:0px;'>Digite el <b>acto administrativo (Paso)</b> y prosiga a presionar el  botón <b>consultar</b> para inicializar la consulta.</p>
                </div>
                <div id="contenidogrids_2" runat="server" style="overflow:auto;">
                    <asp:GridView ID="Grid_Datos" runat="server" Width="100%"  
                        AllowPaging="True" AutoGenerateColumns="False" PageSize="16" 
                        CellPadding="4" ForeColor="#333333" GridLines="None">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:ButtonField HeaderText="DOC" Text="VER" CommandName="Select" />
                            <asp:BoundField DataField="entidad" HeaderText="ID" />
                            <asp:BoundField DataField="nombre" HeaderText="NOMBRE" />
                            <asp:BoundField DataField="docexpediente" HeaderText="EXPEDIENTE" />
                            <asp:BoundField DataField="docpredio_refecatrastal" HeaderText="PREDIO" />
                            <asp:BoundField DataField="fecharadic" HeaderText="FEC. RAD." />
                            <asp:BoundField DataField="docacumulacio" HeaderText="EXP. PPAL" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
                <div id="contenidogrids_3" runat="server" style="padding:5px;">
                </div>
            </div>
        </div>
        
          <asp:Panel ID="pnlError" runat="server" CssClass="CajaDialogoErr" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
              <div id="logo">
                  <h1><a href="#" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
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
   </div>
</body>
</html>


<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="prodeacumulado.aspx.vb" Inherits="coactivosyp.prodeacumulado" %>
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
     .mipp{ padding:7px;}
   </style>
</head>
<body>
<!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="generador-expedientes.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="generador-expedientes.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>

   <div id="container">
    <h1 id="Titulo"><a href="#">Tecno Expedientes !</a></h1>
    <form id="form1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   
	 <asp:Button id="btnResolAcu" runat="server" 
        Text="Resolución de Acumulación (Auto)" ValidationGroup="textovalidados"
         style="position:absolute;top:382px; left:45px; width: 248px; background-image: url('images/icons/68.png'); z-index:10; right: 487px;" 
         CssClass="Botones"></asp:Button>
         
     
	 <asp:Button id="btnResolAcuManual" runat="server" 
        Text="Resolución de Acumulación (Manual)" ValidationGroup="textovalidados"
         style="position:absolute;top:382px; left:300px; width: 248px; background-image: url('images/icons/address_book.png'); z-index:10" 
         CssClass="Botones"></asp:Button>
         
     
	 <asp:Button id="btnAceptar" runat="server" Text="Consultar" ValidationGroup="textovalidados"
         style="position:absolute;top:162px; left:45px; width: 92px; background-image: url('images/icons/okay.png'); z-index:10" 
         CssClass="Botones"></asp:Button>
         
     
   <asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" 
         Font-Size="15px" style="position:absolute;top:107px; left:46px" 
        Font-Bold="True">Digite 
    nombre o cedula del deudor :</asp:label>
    
    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
        enabled="True" 
        targetcontrolid="txtEnte" 
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
   
    <asp:TextBox ID="txtEnte" runat="server" 
            
        style="position:absolute;top:131px;  left:45px; width:697px; z-index:1001"></asp:TextBox>
    
    <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>DEUDOR</strong> es requerido para la consulta. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtEnte" Display="None"></asp:RequiredFieldValidator>
   
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
        
     <div style="position:absolute;left:45px; top:195px; z-index:1001; height: 2px; width: 700px; background-color:#fff;">
     </div>
     
     <div style="position:absolute;top: 208px; left: 45px; width: 475px; background-color:#fff; height: 161px; overflow:auto">
         <asp:GridView ID="dtgAcumuladoCual" runat="server"  Width="100%"
            style="font-size: 11px;" 
            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
            GridLines="None" Enabled="False">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:ButtonField CommandName="select" DataTextField="docexpediente" 
                    HeaderText="Expediente" Text="Botón">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle ForeColor="#0099FF" />
                </asp:ButtonField>
                <asp:BoundField DataField="docpredio_refecatrastal" HeaderText="Predio">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fecha Rac" DataField="fecharadic">
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Enabled="False" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
          </asp:GridView>
     </div>
     
     <div style="position:absolute; background-color:#fff;padding:10px; top: 209px; left: 528px; width: 196px; text-align:center; font-size:11px; height: 140px; margin-bottom: 3px;">
                RESOLUCIÓN DE ACUMULACIÓN <br />
                Se acapara según <b>Expediente</b> más antiguo : <br /><br />
                <b><asp:Label ID="lblAcumulado" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblfecharadic" runat="server" Text=""></asp:Label>
                </b>
                <br /><br />
                <b><asp:Label ID="lbldetalle" runat="server" Text=""></asp:Label></b>
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
    
    <asp:customvalidator id="Validator"  runat="server" ForeColor="Yellow" 
        Font-Names="Arial" Font-Size="15px"	ErrorMessage="CustomValidator" Font-Bold="True" 
        style="top: 416px; left: 48px; position: absolute; width: 696px"></asp:customvalidator>   
        
    </form>
   </div>
</body>
</html>

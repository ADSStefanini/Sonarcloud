<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ejecucionesFiscales.aspx.vb" Inherits="coactivosyp.ejecucionesFiscales" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Ejecuciones Fiscales</title>
	<link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
	<link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
	<link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
	<link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
   <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    
    <style type="text/css">
     .OpMenuBusquedax
     {
       position:absolute;
       top:89px;
       left:29px;
       height: 26px;
     }   
     .OpMenuBusquedax div
     {
        float:left;
        /*position: relative; */
        font-size: 15px;
        font-variant: small-caps;
        margin: 0px 0px 0px 5px;
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
    .selcc
    {
        display:block;
        padding:5px;
        text-decoration:none;
        cursor:pointer;
        text-decoration:none;
        text-align:center;
        font-size:11px;
        color:#99CC00;
        background-color:#003366;
        border-left:10px solid #99CC00; 
    }
    .normal
    {
        display:block;
        padding:5px;
        text-decoration:none;
        cursor:pointer;
        text-decoration:none;
        text-align:center;
        font-size:11px;
        color:#FFFFFF;
        background-color:#507CD1;
        border-left:10px solid #4371BF; 
    }
    a.Ntooltip:hover
      {
        background-color:#507CD1;
      }
      .palanca
     {
        border-left :10px solid #4977D3;
        color: #34484E;         
     }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#Deudor").click(function() {
                $("#opcamp option[value=1]").attr("selected", true);
                $("#txtEnte").focus();
            });

            $("#NroPre").click(function() {
                $("#opcamp option[value=2]").attr("selected", true);
                $("#txtEnte").focus();
            });

            $("#Expedi").click(function() {
                $("#opcamp option[value=3]").attr("selected", true);
                $("#txtEnte").focus();
            });

            $("#ActosAdmin").click(function() {
                $("#opcamp option[value=4]").attr("selected", true);
                $("#txtEnte").focus();
            });

            $("#btnBusDeuda").click(function() {
                $("#TextBoxDeuda").focus();
            });
        });
    </script>
    <script type="text/javascript">
        $(window).scroll(function() {
            $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
        });
   </script>
   <script type="text/javascript">
	    $(document).ready(function() {
	        $('a.Ntooltip').hover(function() {
	            $(this).find('span').stop(true, true).fadeIn("slow");
	        }, function() {
	            $(this).find('span').stop(true, true).hide("slow");
	        });


	        $("#buscaexpediente").click(function() {

	            $('#overflow').fadeTo('slow', 0.6, function() {
	                var cssObj = {
	                    'background-color': '#000',
	                    'width': $(window).width() + 'px',
	                    'height': $(document).height() + 'px',
	                    'position': 'absolute',
	                    'z-index': '1001',
	                    'display': 'block'
	                }
	                $('#overflow').css(cssObj);
	                $('#xxslow').css({
	                    position: 'absolute',
	                    left: ($('#container').width() - $('#xxslow').outerWidth()) / 2,
	                    top: (400 + $(window).scrollTop() - $('#xxslow').outerHeight()) / 2
	                });
	                $("#xxslow").fadeTo('slow', 1)
	                $('#txtbuscaexpedientesimple').focus();
	            });
	        });

	        $("#cerrarexpediente").click(function() {
	            $('#overflow').fadeTo('slow', 0, function() {
	                var cssObj = {
	                    'display': 'none'
	                }

	                $("#xxslow").fadeTo('slow', 0, function() {
	                    $('#overflow').css(cssObj);
	                    $('#xxslow').css(cssObj);    
	                });
	            });
	        });
	    });
    </script>
    <script type="text/javascript">
         function ShowImage() {
             document.getElementById('txtEnte').style.backgroundImage = 'url(images/ajax-loader.gif)';
             document.getElementById('txtEnte').style.backgroundRepeat = 'no-repeat';
             document.getElementById('txtEnte').style.backgroundPosition = 'right';
         }

         function HideImage() {
             document.getElementById('txtEnte').style.backgroundImage = 'none';
         }
    </script>
     <script type="text/javascript">
     // Dialog
                $('.xparametrosInfo_Permisos').dialog({
                    autoOpen: false,
                    width: 400,
                    modal: true,
                    buttons: {
                        "Aceptar": function() {
                            $(this).dialog("close");
                        }
                    },
                    hide: 'fold'
                });

                $('.dialog_link').click(function(evento) {
                    evento.preventDefault();
                    $('.xparametrosInfo_Permisos').dialog('open');
                    return false;
                });

                function ValidarNivel(pNivelAcceso) {
                    if (pNivelAcceso == 1) {
                        window.open('cuadros/NuevoExpeIndividual.aspx', 'mywindow', 'location=0,status=0,scrollbars=1, width=479,height=350');
                    } else {
                        alert("Solo los usuarios administradores pueden crear expedientes");
                    }
                }    
         </script>
</head>
<body>
   <!-- Definicion del menu -->  
   <div id="message_box">
    <ul style="width:36px; height:188px">
     <li style="height:36px;width:36px !important;">
        <a href="generador-expedientes.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
     </li>
     <li style="height:152px;width:36px;">
        <a href="generador-expedientes.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
     </li>
    </ul>
   </div>
   <div id="overflow"></div>

   <div id="container">
    <h1 id="Titulo"><a href="javascript:void(0)">Ejecuciones Fiscales - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 43px; left: 409px; padding:7px; width: 333px;" id ="Div3" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    <form id="form1" runat="server">
       <!-- Formulario -->
       <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout ="360000">
       </asp:ToolkitScriptManager>
       
    
     
     <div style="position:absolute;top:429px; left:487px; height: 100px; width: 262px;background-color:#2461BF; padding:5px; font-size:14px;color:#ffffff; font-family:Arial;font-weight: bold; z-index: 1;">
      <table width="100%" height="100%">
       <tr>
        <td>Vlr. Deuda :</td>
        <td align="right" style="color:#610B0B;font-size:17px;"><div id ="lbldeuda" runat="server">0,0</div></td>
       </tr>
       <tr>
        <td>Vlr. Interes :</td>
        <td align="right" style="color:#610B0B;font-size:17px;"><div id="lblInteres" runat="server">0,0</div></td>
       </tr>
       <tr>
        <td colspan="2">
         <hr />
        </td>
       </tr>
       <tr>
        <td>Total :</td>
        <td align="right" style="color:#610B0B;font-size:17px;"><div id="lbltotal" runat="server">0,0</div></td>
       </tr>
      </table>
     </div>
     
           
    <select id="opcamp" runat="server" 
           style="top: 545px; left: 488px; position: absolute; width: 140px; visibility:hidden">
        <option value="1">Opcion 1</option>
        <option value="2">Opcion 2</option>
        <option value="3">Opcion 3</option>
        <option value="4">Opcion 4</option>
    </select>
     
     <div id = "detalle" runat="server" 
           style="position:absolute;top:426px; left:21px; height: 15px; width: 728px; background-color:#2461BF; padding:5px; font-size:11px;color:#ffffff; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;">
     </div>
     
     <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
       <ContentTemplate>--%>
        <div style="top: 192px; left: 21px; position: absolute; height: 234px; width: 738px; overflow:auto; background-color:#2461BF;">
                 <asp:GridView ID="GridView_datos" runat="server" AutoGenerateColumns="False"  width = "1110px"
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" style="font-size: 12px" HorizontalAlign="Left">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Security/images/icons/1.png" 
                                        ShowSelectButton="True" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="16px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="EfiNit" HeaderText="ID" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ED_Nombre" HeaderText="Deudor" 
                                        SortExpression="ED_Nombre" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="400px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EfiNroExp" HeaderText="Expediente" 
                                        SortExpression="EfiNroExp" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EfiUltPas" HeaderText="Ult. Acto" 
                                        SortExpression="EfiUltPas" Visible = "false">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Left" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
         </div>  
        
         <div id="messagesof" runat = "server" style="position:absolute;top:450px; left:21px; height: 15px; width: 457px; background-color:#2461BF; padding:5px; font-size:11px;color:#ffffff; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
          AUN NO SELECCIONA UN EXPEDIENTE O REGISTRO DE LA CUADRICULA.
         </div>
         <div style="position:absolute;top:67px;right:21px; text-align:left;left: 15px; width: 457px;padding:5px; font-size: xx-small;color:#ffffff; font-family:Arial;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
          Seleccione un expediente para examinar el historial.</div>
       <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="Este <strong>CAMPO</strong> es requerido para la consulta. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtEnte" Display="None"></asp:RequiredFieldValidator>
       <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
       </asp:ValidatorCalloutExtender>
         
       <%-- </ContentTemplate>
       </asp:UpdatePanel>--%>
        
         <asp:Button ID="btnHistorial" runat="server" CssClass="Botones" 
            style="top:518px;left:21px;position:absolute;width:145px;background-image: url('images/icons/chart_bar.png');" 
            Text="Examinar Historial" />
         
         <%
             Response.Write("<input type=""button"" onclick=""ValidarNivel(" & Session("mnivelacces").ToString & ");"" name=""btnNuevoExpediente"" id=""btnNuevoExpediente"" runat=""server"" value=""Nuevo Expediente"" class=""Botones"" style=""top:551px;left:21px;position:absolute;width:145px;background-image: url('images/icons/chart_bar_add.png');""  />")
         %>
           
         <asp:Button ID="btnCobroIndividual" runat="server" CssClass="Botones" 
            style="top:551px;left:176px;position:absolute;width:137px;background-image: url('images/icons/add-printer.png');" 
            Text="Cobro Individual" />
                      
         <asp:Button ID="btnConsultar" runat="server" CssClass="Botones"  ValidationGroup="textovalidados"
            style="top: 163px; left: 21px;position: absolute;width: 98px; background-image: url('images/icons/user_female.png'); right: 661px;" 
            Text="Consultar" />
            
       
        <asp:Button ID="btnBusDeuda" runat="server" CssClass="Botones" style="top: 163px; left: 127px; position: absolute;width: 142px; background-image: url('images/icons/dollar.png');" Text="Limite de Deuda" />
        <input type ="button" id="buscaexpediente" value="Consulta simple" style="z-index:5;background-image: url('images/icons/magnify.png');width:130px;top:163px;left:277px; position: absolute;" class="Botones" />   
        <!-- Criterios de busqueda -->
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
             <div id="selectoresBus" class="OpMenuBusquedax">
                  <div id="Deudor" style="background-color:#507CD1;"><asp:LinkButton ID="LinkBuDeudor" runat="server" CssClass="Ahlink">Deudor (Nombre o Cedula)</asp:LinkButton></div>
                  <div id="Expedi" style="background-color:#507CD1;"><asp:LinkButton ID="LinkExpedienteBus" runat="server" CssClass="Ahlink">Expediente</asp:LinkButton></div>
                  <div id="ActosAdmin" style="background-color:#507CD1;"><asp:LinkButton ID="LinkBusActuaciones" runat="server" CssClass="Ahlink">Actos Administrativos</asp:LinkButton></div>
             </div>
                    
             <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                enabled="True" 
                targetcontrolid="txtEnte" 
                servicemethod="ObtListaEtidades_Est" 
                ServicePath="Servicios/Autocomplete.asmx"
                MinimumPrefixLength="1" 
                CompletionInterval="1000"
                EnableCaching="true"
                CompletionSetCount="15" 
                CompletionListCssClass="CompletionListCssClass"
                CompletionListItemCssClass="CompletionListItemCssClass"
                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass" 
                onclientpopulated="HideImage" onclientpopulating="ShowImage">
            </asp:AutoCompleteExtender>
           
            <div style="position:absolute;top:112px; background-color:#304a7d;left:21px;padding:10px 5px 5px 5px;width:727px;z-index:2;">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEnte" runat="server" style="width: 715px;"></asp:TextBox>
                            <!-- 
                            <script type="text/javascript">
                                 function CommonKeyPressIsAlpha(evt) {
                                     var charCode = (evt.which) ? evt.which : event.keyCode
                                     if (charCode == 13) {
                                         document.getElementById('<%=btnConsultar.ClientId%>').click();
                                         document.getElementById('<%=txtEnte.ClientId%>').focus();
                                     }
                                 }
	                         </script> 
	                         -->
                        </td>
                    </tr>
                </table>
            </div>
            <div id = "titulocap" runat="server" style="position:absolute;top:151px;color:#fff; background-color:#304a7d;right:22px;padding:7px 9px 4px 9px;font-size:10px;">En este momento está buscando por <b>deudor</b></div>
            <asp:Button ID="btnDeuda" runat="server" CssClass="Botones" 
                style="top: 518px; left: 176px; position: absolute;width: 137px; background-image: url('images/icons/pie_chart.png');" 
                Text="Examinar Deuda" />
     <%--      </ContentTemplate>
        </asp:UpdatePanel>--%>
        
        <asp:Panel ID="pnlSeleccionarDatosDEUDA" runat="server" CssClass="CajaDialogo"                     
            style="height: 98px; width: 260px; top: 90px; left: 0px; display:none;"> 
                    
                    <asp:Button style="Z-INDEX: 116;width: 100px; top: 60px; left: 120px; position: absolute; background-image: url('images/icons/cancel.png');" id="btnNo"
                        runat="server"  CssClass="Botones" Text="Cancelar"></asp:Button>
                    	
                    <asp:Button style="Z-INDEX: 116;width: 100px; top: 60px; left: 9px; position: absolute; background-image: url('images/icons/okay.png');" id="btnSiConsulDeuda"
                        runat="server" Text="Enviar"  
                        CssClass="Botones"></asp:Button>
                    
                    <div style="top: 10px; left: 78px; position: absolute; width: 115px">Deuda Mayor A :</div>
                    
                    <asp:TextBox ID="TextBoxDeuda" runat="server" 
                        
                        style="width: 237px; top: 29px; left: 9px; z-index: 1001; position: absolute"></asp:TextBox>
                        
       </asp:Panel>
       
       <asp:ModalPopupExtender ID="mpeSeleccion" runat="server" 
            TargetControlID="btnBusDeuda"
            PopupControlID="pnlSeleccionarDatosDEUDA"
            CancelControlID="btnNo"
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
       >
       </asp:ModalPopupExtender>
       
       <div id="Messenger1" runat = "server"  style=" position:absolute;top:45px; left:23px; font-size:14px;color:#D7DF01; width: 737px;"></div>
       
       <!-- Codigo seleccionar expediente-->
       <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo"                     
        style="height: 245px; width: 358px; top: 174px; left: 212px; z-index:1001; display:none;"> 
                    
                    <asp:Button style="Z-INDEX: 116;width: 105px; top: 211px; left: 108px; position: absolute; right: 145px; background-image: url('images/icons/cancel.png');" id="Button1"
                        runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
                    	
                    <asp:Button style="Z-INDEX: 116;width: 84px; top: 212px; left: 16px; position: absolute; right: 258px; background-image: url('images/icons/okay.png');" id="btnSi"
                        runat="server" Text="Enviar" Height="23px" CssClass="Botones"></asp:Button>
                    
                    <asp:ListBox ID="ListExpedientes" runat="server" 
                        style="top: 30px; left: 109px; position: absolute; height: 131px; width: 223px">
                    </asp:ListBox>
                           
                    <div id="Label2" runat="server" 
                         style="top: 12px; left: 108px; position: absolute;width: 220px">Predios</div>
                    
                    <div id="Label3" runat="server" 
                        style="top: 166px; left: 17px; position: absolute;">Se detecto más de un predio</div>
                        
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Security/Menu_PPal/property.png" 
                        
                        
                        style="top: 0px; left: 0px; position: absolute; height: 75px; width: 98px" />
                    <asp:CheckBox ID="chktodopanelPredio" runat="server" 
                        style="height: 20px; width: 236px; top: 184px; left: 12px; z-index: 1001; position: absolute" 
                        Text="Seleccionar todo" />
       </asp:Panel> 
       
       <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden" />
       <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            TargetControlID="Button2"
            PopupControlID="pnlSeleccionarDatos"
            CancelControlID="Button1"
            
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
       >
       </asp:ModalPopupExtender>
       
       
       <asp:Button ID="btnConsulrac" runat="server" CssClass="Botones" 
                style="top: 518px; left: 324px; position: absolute;width: 127px; background-image: url('images/icons/user.png');" 
                Text="Consulta Rapida" />
           
           
       <asp:Panel ID="Panel1" runat="server" style="background-color:#507CD1;width: 360px;border-width: 7px; border-style: solid; border-color: #D8D8D8;display:none;">
           <div id ="Div1" style="color:#ffffff;font-size:11px;width: 360px;">
               <div style="background-color:#507CD1; height:20px; padding:7px">
                   <a class="Ntooltip" href="javascript:void(0)"  style="position:absolute;left:5px;top:10px;z-index:225;width: 17px; height: 18px; right: 712px;">
                      <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                      <span style="z-index:225;">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Consultas Rápidas 
                        </b>
                        <br /><br />
                        Con estas consultas podrá verificar o auditar datos de los diferentes documentos asociados a un deudor. 
                        
                        <br />
                        <br />
                        
                        <b>Ejemplo :</b> Verificar expedientes con vigencias gravadas. 
                      </span>
                    </a>
                    <div style=" padding-left:20px; padding-top:4px;">Accesos rápidos y consultas</div>
               </div>
               
               <div style="background-color:#f0f0f0; padding:5px;">
                  <asp:Button ID="btnracExpediente" runat="server" 
                       Text="¿Deudores con vigencias pendientes?" 
                       style="z-index:5;background-image: url('images/icons/24.png');" 
                       CssClass="Botones"  Width="349px" />
                  <br />
                  <asp:Button ID="btnconsultaracum" runat="server" 
                       Text="Consulta por resolución de acumulación" 
                       style="z-index:5;background-image: url('images/icons/24.png'); margin-top:3px;" 
                       CssClass="Botones"  Width="349px" />    
                     
                  <br /><br />
                  <asp:Button ID="AcepPanel1" runat="server" Text="Aceptar"  style="width: 98px; z-index:5 ;background-image: url('images/icons/accept.png');"  CssClass="Botones" />        
               </div>
               
           </div>
           
       </asp:Panel>
       
       <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                TargetControlID="btnConsulrac"
                PopupControlID="Panel1"
                CancelControlID="AcepPanel1"
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
                >
            </asp:ModalPopupExtender>
            
            <div style="position:absolute;top:472px; left:21px; width: 469px; background-color:#2461BF;font-size:11px;color:#ffffff; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                     <th style="text-align:left;padding:5px; width:61px">IMPUESTO :</th>
                     <th style="text-align: left;padding:5px;"><div id="ActoImpuesto" runat="server">......</div></th>
                    </tr>
                </table>
           </div>
           
           
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
        
        <div id ="xxslow" style="background-color:#507CD1;width: 360px;border-width: 7px; border-style: solid; border-color: #D8D8D8;display:none; z-index:1001;">
          <div id ="Div2" style="color:#ffffff;font-size:11px;width: 360px;">
             <div style="background-color:#507CD1; height:20px; padding:7px;">
                   <a class="Ntooltip" href="javascript:void(0)"  style="position:absolute;left:5px;top:10px;z-index:225;width: 17px; height: 18px; right: 712px;">
                      <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                      <span style="z-index:225;">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Consultas Rápidas 
                        </b>
                        <br /><br />
                            Especifique en el cuadro de texto el dato a buscar.
                      </span>
                    </a>
                    <div style=" padding-left:20px; padding-top:4px;">Accesos rápidos y consultas</div>
               </div>
               <div style="background-color:#f0f0f0; padding:5px;">
               <table width="100%">
                  <tr>
                    <td><div style="color:#507CD1;">Buscar : </div></td>
                    <td><asp:TextBox ID="txtbuscaexpedientesimple" runat="server" Width="167px" onkeypress="return CommonKeyPressIsAlpha(event);"></asp:TextBox></td>
                  </tr>
                  <tr><td colspan="2"><hr style=" background-color:#507CD1" /></td>
                  </tr> 
                  <tr>
                    <td colspan="2">
                       <input type ="button" id="btnEnviar"  class="Botones" 
                        value="Enviar" 
                        style="background-image: url('images/icons/tick-circle-frame.png');width:74px;" onclick="OnClickboton()"  />
                        
                        <script type="text/javascript">
                            function CommonKeyPressIsAlpha(evt) {
                                var charCode = (evt.which) ? evt.which : event.keyCode
                                if (charCode == 13) {
                                    OnClickboton();
                                }
                            }
                            
                            function OnClickboton() {
                                document.getElementById('<%=txtEnte.ClientId%>').value = "::" + document.getElementById('<%=txtbuscaexpedientesimple.ClientId%>').value
                                document.getElementById('<%=txtEnte.ClientId%>').focus();
                                document.getElementById('<%=txtbuscaexpedientesimple.ClientId%>').value= ""
                                document.getElementById('<%=btnConsultar.ClientId%>').click();
                            }
	                    </script> 
	                         
                        <input type ="button" id="cerrarexpediente" value="Cerrar ventana"  
                           style="z-index:5;background-image: url('images/icons/sign_cacel.png');width:118px;"  
                           class="Botones" />
                     </td>
                  </tr>
               </table>
               </div>
        </div>
      </div>
         <div id="validateUser" runat="server" class="xparametrosInfo_Permisos"  style="display:none; text-align:left; " title="USUARIO - NO TIENE PERMISOS">
                <img src="../images/1321994028_watchman.png" alt="Seguridad" style="float:left;" title="Seguridad" />
                <span style="font-weight:bold;font-size:14px;">Atención de seguridad</span>
                <br />  
                <p style="text-align:justify;font-size:xx-small;">Lo sentimos pero el usuario con el cual se encuentra identificado no tiene <b>permisos</b> (o derechos de acceso)  para procesar este expediente.</p>
               
            </div>
    
    </form>
    
    
</div>
</body>
</html>
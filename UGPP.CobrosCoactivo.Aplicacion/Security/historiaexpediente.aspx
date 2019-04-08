<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="historiaexpediente.aspx.vb" Inherits="coactivosyp.historiaexpediente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" >
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Consulta de Expedientes</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link href="Libertad.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        .CajaDialogo
        {
        position:absolute;
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #72A3F8;
        padding: 0;
        color:#514E4E;
        font-weight: bold;
        font-size:12px;
        font-style: italic;  
        }
        .wsrt
        {
        color:White;
        text-decoration:none;
        font-family:Verdana;
        font-size:14px;
        }
        .wsrt:hover
        {
        color:#EEE45F !important;
        cursor:pointer;
        }
        .contenedortitulos
        {
        margin:0px;
        padding:6px;
        background-image: url('images/BarraActos.png');
        background-repeat: repeat-x;
        background-color:#507CD1;
        text-align:left;
        color:#fff;
        font-family:Tahoma;
        font-size:12px;     
        text-align:left; 
        }
        .xws1
        {
        font-size:11px;
        font-family:Verdana;
        color:#ffffff;
        }
        .divhisto
        {
        /*border: 1px solid #dcdbe0; */
        z-index:888; 
        background-color:#507CD1;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        }
        table.servicesT
        {	
        font-family: Verdana;
        font-weight: normal;
        font-size: 11px;
        color: #404040;
        background-color: #fafafa;
        border-collapse: collapse;
        border-spacing:0;
        margin:0;}
        table.servicesT td, table.servicesT th, .ajaxetapa
        {
        border: solid 1px #2356aa;
        font-family: Verdana, sans-serif, Arial;
        font-weight: normal;
        font-size: 10px;
        color: #404040;
        text-align: left;
        padding-left: 3px;
        }
        table.servicesT td.servHd:hover  
        {
        background-color: #ffffc1 !important;
        color:#F78F08 !important; 
        border: 1px solid #2356aa;
        }
        table.servicesT td.EservEHd{padding:4px;}
        table.servicesT td.EservEHd:hover  
        {
        background-color: #fdefef !important;
        color:#F80808;
        padding-left:16px;
        cursor:pointer;
        border: 1px solid #2356aa;
        background-repeat: no-repeat;
        background-color: transparent;
        background: url('images/icons/151.png') no-repeat;
        background-position : 3px 5px;
        }
        table.servicesT th {font-weight:bold;background:#f4f4f4 url('images/icons/grey-up.png') repeat-x 0 -5px;padding:3px;color:#5e5a5a;}
        table.servicesT td a {text-decoration:none;}
        .xa {color: #fff; text-decoration: none;} 
        .xa:hover {color:#fff;text-decoration:none;font-weight:bold;cursor:pointer;}
        div.ie-fix {
        overflow: visible;
        height: 0;}
        div#ejPredio {color:#fff;}
        div#ejPredio:hover {color:#dbc42c; font-weight:bold; cursor:pointer;}
    </style>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <!-- TOOLTIP  -->
	<!--[if IE]><script src="tooltip/excanvas.js" type="text/javascript" charset="utf-8"></script><![endif]-->
	<script src="tooltip/jquery.bt.min.js" type="text/javascript" charset="utf-8"></script>
	<link href="tooltip/tooltip.css" rel="stylesheet" type="text/css" />
	
    <script type="text/javascript">
        function posicionCapa(capa) {
            capa = $(capa);
            var cssObj = {
                'position': 'absolute',
                'top': capa.offset().top + capa.innerHeight() + 10,
                'left': capa.offset().left 
            }
            $("#div-obs").css(cssObj);
        }
        function ObAjax(e) {
            //var myString = $(this).attr('rel');
            //alert(myString);
            if ($('#div-obs').is(':visible')) { 
                $("body").append("<div id='div-obs'>ppppppp</div>");
            }
            posicionCapa(e)
            $("#div-obs").css({'width': '300px', 'background-color': '#fff', 'z-index': 999 });
        }
        jQuery(document).ready(function($) {

            $("#txtEnte").focus();
            $(window).scroll(function() {
                $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
            });

            $(".actoclicksec").click(function(e) {
                var myString = $(this).attr('rel');
                var array = myString.split('$');
                var content = $('#content' + array[1]);
                $(content).html('<div class="ajaxetapa"><p><img src="images/ajax-loader.gif" alt="Cargando" /> CARGANDO LOS DATOS POR FAVOR ESPERE....</p></div>');
                $(content).children().first().filter('.ajaxetapa').load("Servicios/Permisos_Expedientes.ashx", { expediente: array[0], etapa: array[1] }, function(response, status, xhr) {
                    if (status == "error") {
                        var msg = "<b>Lo sentimos pero hubo un error:</b> <br />";
                        $(content).children().first().filter('.ajaxetapa').html(msg + xhr.status + " " + xhr.statusText).hide().fadeTo('slow', 1);
                    }
                    else if (status == "success") {
                        if (response == "200") {
                            $(content).children().first().filter('.ajaxetapa').html("La consulta no retorno datos...").hide().fadeTo('slow', 1);
                            $(this).css({ 'background-color': '#ffff7f', 'font-weight': 'bolder', 'padding': '5px' });
                        }
                        else {
                            $(this).css({ 'border': 'none', 'padding-left': '0' });
                        }
                    }
                }).hide().fadeTo('slow', 1);
            });

            $("#btnAceptar").click(function(e) {
                var entidad = $("#txtEnte").val();

                if (entidad == "") {
                    e.preventDefault();
                    $("#Validator").css("visibility", "visible");
                    $("#Validator").html("Para continuar digite una entidad.");
                    return
                }
            });

            $("#btnCancelar").click(function(f) {
                $("#txtEnte").val("");
                $("#txtEnte").focus();
                $("#contenidogrids").html("<p style='padding:10px;'>Digite el <b>deudor (propietario)</b> y prosiga a presionar el  botón <b>consultar</b> para inicializar la consulta.</p>");
                $("#lblExpediente").html("Expediente");
                f.preventDefault();
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

            $('#example-17-content').hide();
            $('#example17').bt({
                positions: 'bottom',
                contentSelector: "$('#example-17-content')", /*hidden div*/

                trigger: 'click',
                width: 220,
                centerPointX: .9,
                spikeLength: 55,
                spikeGirth: 40,
                padding: 15,
                cornerRadius: 20,
                fill: '#fff',
                strokeStyle: '#094194',
                strokeWidth: 4
            });

            $("#cerrar_ejPredio").click(function(evento) {
                $('#xShow_ejPredio_Content').fadeOut('show');
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
</head>
<body>
    <!-- Definicion del menu -->  
    <div runat="server" id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
     <div id="overflow"></div>
    <div id="container">
    <h1 id="Titulo"><a href="javascript:void(0)">Consulta de Expedientes - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 36px; padding:7px; width: 688px;" id ="Div2" runat="server">
        Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    <form id="form1" runat="server">
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
      </asp:ToolkitScriptManager>
                
      <div id="example-17-content" class="gmap" style=" display:none;">
        <div class="gmap-title"  style="color:#094194;">Consultas:</div>
        <div class="gmap-addr">
          <div style="color:#094194"><b><%  Response.Write(Session("ssCodimpadm") & ".")%></b></div>
        </div>
        
        <div class="gmap-bottom">
            <fieldset>
                <legend>Predio</legend>
                <%   
                    If Not ViewState("vsPredio") Is Nothing Then
                        Response.Write(ViewState("vsPredio"))
                    Else
                        Response.Write("En espera de un predio.")
                    End If
                %>
                <asp:LinkButton ID="LinkssPredioExaminarExpedientes" runat="server"></asp:LinkButton>
            </fieldset>
            <br />
            <fieldset>
                <legend>Expediente Acumulado</legend>
                <%   
                    If Not ViewState("vsExpedienteAcu") Is Nothing Then
                        Response.Write(ViewState("vsExpedienteAcu"))
                    Else
                        Response.Write("En espera de un expediente.")
                    End If
                %>
                <br />
                <asp:LinkButton ID="LinkvsExpedienteAcu" runat="server"></asp:LinkButton>
            </fieldset>
            <!-- <br />
            <a href="generador-expedientes.aspx" title="Cobranza">Quiero ir a Cobranzas</a> -->
        </div>
        <a href="javascript:void($('#example17').btOff());"><img src="images/icons/101.png" alt="cerrar" width="12" height="12" class="gmap-close" /></a>
      </div>
      
      <div id ="xxslow" style="background-color:#507CD1;width: 360px;border-width: 7px; border-style: solid; border-color: #D8D8D8;display:none; z-index:1001;">
        <div id ="Div1" style="color:#ffffff;font-size:11px;width: 360px;">
             <div style="background-color:#507CD1; height:20px; padding:7px;">
                   <a class="Ntooltip" href="javascript:void(0)"  style="position:absolute;left:5px;top:10px;z-index:225;width: 17px; height: 18px; right: 712px;">
                      <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                      <span style="z-index:225;">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Consultas Rápidas 
                        </b>
                        <br /><br />
                        Con estas consultas podrá verificar o auditar datos de los diferentes 
                   documentos asociados a un deudor. 
                        
                        <br />
                        <br />
                        
                        <b>Ejemplo :</b> Verificar expedientes con vigencias gravadas. 
                      </span>
                    </a>
                    <div style=" padding-left:20px; padding-top:4px;">Accesos rápidos y consultas</div>
               </div>
               <div style="background-color:#f0f0f0; padding:5px;">
               <table width="100%">
                  <tr>
                    <td><div style="color:#507CD1;">Expediente</div></td>
                    <td><asp:TextBox ID="txtbuscaexpedientesimple" runat="server" Width="167px"></asp:TextBox></td>
                  </tr>
                  <tr><td colspan="2"><hr style=" background-color:#507CD1" /></td>
                  </tr> 
                  <tr>
                    <td colspan="2">
                       <asp:Button ID="btnEnviar" runat="server" CssClass="Botones" 
                        Text="Enviar" 
                        style="background-image: url('images/icons/tick-circle-frame.png');" 
                        Width="74px" />
                        
                        <input type ="button" id="cerrarexpediente" value="Cerrar ventana"  
                           style="z-index:5;background-image: url('images/icons/sign_cacel.png');width:118px;"  
                           class="Botones" />
                   
                     </td>
                  </tr>
               </table>
               </div>
        </div>
    </div>
    <asp:Panel ID="Panel1" runat="server" CssClass="CajaDialogo" style="z-index:1001; width:200px;display:none; padding:11px;">
            <div>
            <table width="100%" class="xxtabla" style="text-align:center;border-collapse:collapse;" cellspacing="0" rules="all" border="1">
            <tr>
            <td><div id = "pp">PRESIONE UN EXPEDIENTE PARA EXAMINARLO </div></td>
            </tr>
            </table>

            <asp:GridView ID="Gridexpedinete" runat="server" AutoGenerateColumns="False" 
            Width="100%" CssClass="xxtabla">
            <Columns>
            <asp:ButtonField CommandName="select" DataTextField="docexpediente" 
                HeaderText="Expedientes" Text="Botón">
                <ItemStyle HorizontalAlign="Left" />
            </asp:ButtonField>
            </Columns>
            </asp:GridView>
            <table width="100%" class="xxtabla" style="text-align:left;border-collapse:collapse;" cellspacing="0" rules="all" border="1">
            <tr>
            <td><div  style=" font-size:10px; font-weight:normal;" id = "Divtota" runat="server"></div></td>
            </tr>
            <tr>
            <td><div  style=" font-size:10px; font-weight:normal;"><b>Predio: </b> <% Response.Write(ViewState("ssPredioExamen"))%>  </div></td>
            </tr>
            </table>
            <br />
            <asp:Button ID="btncanbott" runat="server" Text="Aceptar"  style="width: 98px; z-index:5 ;background-image: url('images/icons/accept.png');"  CssClass="Botones" />
            </div>
     </asp:Panel>
     <asp:Button ID="Button4" runat="server" Text="Button" style="visibility:hidden"  />
     <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
            TargetControlID="Button4"
            PopupControlID="Panel1"
            CancelControlID="btncanbott"
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
            >
     </asp:ModalPopupExtender>
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
        onclientpopulated="HideImage" onclientpopulating="ShowImage">
     </asp:AutoCompleteExtender>
     <div id="Label1" style="position:absolute;top:107px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:bold;">
         Digite nombre o cedula del deudor :</div>
     <asp:TextBox ID="txtEnte" runat="server" style="position:absolute;top:131px;left:34px;width:709px;z-index:777;" onkeypress="return CommonKeyPressIsAlpha(event);"></asp:TextBox>
       <script type="text/javascript">
           function CommonKeyPressIsAlpha(evt) {
               var charCode = (evt.which) ? evt.which : event.keyCode
               if (charCode == 13) {
                   OnClickboton();
                   return false;
               }
           }

           function OnClickboton() {
               document.getElementById('<%=txtEnte.ClientId%>').focus();
               document.getElementById('<%=btnAceptar.ClientId%>').click();
           }
	   </script>
    <asp:customvalidator style="position:absolute;top:164px; left:46px; width: 702px;" id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>     
    <div style="position:absolute; top: 218px; left: 34px; width: 711px; font-family:Verdana;background-color:White;">
        <div style="padding:5px;background-color:#507CD1;font-size:16px;color:#fff;width: 701px;overflow: hidden;">
            <img src="images/icons/help.png" width="16" height="16" alt="" style="width:16px; height:16px;vertical-align: text-top;float: left; padding-right:3px;" border="0" />
            <span  style="float: left;"> Historial completo del expediente. (</span><asp:Label ID="lblExpediente" runat="server" Text="Expediente" CssClass="xa" style="float: left;"></asp:Label> <span  style="float: left;">
            )</span> 
            <!-- herramientas -->
            <div id="buscaexpediente" title="Buscar un número de expediente en específico." runat="server" style="float:right;width:16px; height:16px;cursor:pointer;"><img src="images/icons/magnify.png" width="16" height="16" alt="" id="Img1" /></div>
            <div id="ssherramienta1"  title="Cosultas." runat="server" style="float:right;width:16px; height:16px;margin-right:4px;"><img src="images/icons/address_book.png" width="16" height="16" alt="" id="example17" /></div>
            <div id="ssherramienta2"  title="Buscar un acto administrativo en específico." runat="server" style="float:right;width:16px; height:16px;margin-right:4px;cursor:pointer;"><a href='Consulta_Actos.aspx'><img src="images/icons/vcard.png" width="16" height="16" alt="" id="Img3" /></a></div>
        </div>
        
        <div style="background-color:White;font-size:11px;text-transform: uppercase;" id="examinar" runat="server">
            <div id="contenidogrids" runat="server">
               <p style='padding:10px;'>Digite el <b>deudor (propietario)</b> y prosiga a presionar 
                   el botón <b>consultar</b> para inicializar la consulta.</p>
            </div>
        </div>
        <div  class="divhisto" id="Anexo_adjuntados" runat="server" style="display:none;">
            <table  style="border-collapse:collapse;" cellspacing="0">
             <tr>
              
              <td>
                <a class ="xcd" href ="ejecucionesFiscales.aspx">Ejecuciones Fiscales</a>
              </td>
              <td>
                <asp:LinkButton ID="LinksiguentePasoAdministrativo2" CssClass ="xcd" runat="server">Siguiente 
                  Paso</asp:LinkButton>
              </td>
             </tr>
           </table>
       </div>
    </div>
    <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
         style="position:absolute;top:186px; left:133px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
         CssClass="Botones"></asp:Button>
           
     <asp:Button id="btnRegresar" Visible="false" runat="server" Text="Quiero regresar a la creación de deudores y consultas rápidas" 
         style="position:absolute;top:186px; left:232px; width: 390px; background-image: url('images/icons/user.png'); z-index:10" 
         CssClass="Botones"></asp:Button>    
         
	 <asp:Button id="btnAceptar" runat="server" Text="Consultar" ValidationGroup="textovalidados"
         style="position:absolute;top:186px; left:34px; width: 92px; background-image: url('images/icons/okay.png'); z-index:10" 
         CssClass="Botones"></asp:Button>
       
       <script language="javascript" type="text/javascript">
           function ShowOptions(control, args) {
               control._completionListElement.style.zIndex = 10000001;
           }
    </script>
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
     
       <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo"                     
        style="height: 221px; width: 358px; top: 174px; left: 212px; z-index:1001; display:none;"> 
                    
                    <asp:Button style="Z-INDEX: 116;width: 105px; top: 187px; left: 108px; position: absolute; right: 171px;background-image: url('images/icons/cancel.png');" id="btnNo"
                        runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
                    	
                    <asp:Button style="Z-INDEX: 116;width: 84px; top: 187px; left: 16px; position: absolute; right: 264px;background-image: url('images/icons/okay.png');" id="btnSi"
                        runat="server" Text="Enviar" Height="23px" CssClass="Botones"></asp:Button>
                    
                    <asp:ListBox ID="ListExpedientes" runat="server" 
                        style="top: 30px; left: 109px; position: absolute; height: 131px; width: 223px">
                    </asp:ListBox>
                           
                    <asp:Label ID="Label2" runat="server" Text="Expedientes" 
                         style="top: 12px; left: 108px; position: absolute; height: 15px; width: 220px"></asp:Label>
                    
                    <asp:Label ID="Label3" runat="server" 
                        style="top: 166px; left: 17px; position: absolute;" 
                        Text="Se detecto más de un expediente."></asp:Label>
                        
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Menu_PPal/actualizar.png" 
                        style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
                        
       </asp:Panel>     
       <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden" />
       <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
            TargetControlID="Button2"
            PopupControlID="pnlSeleccionarDatos"
            CancelControlID="btnNo"
            
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
       >
       </asp:ModalPopupExtender>
       
       <!-- ejcuaciones Fiscales -->
       <div id="ejeFiscales" runat="server" style="position:absolute;top:90px;height:105px;width:735px;left:23px;display:none;" class="divhisto">
            <img src="images/icons/<%= IIf((Session("ssimpuesto")=1 orElse Session("ssimpuesto")=2) ,"user_128.png","vehiculo_imp.png") %>" alt="" width="75" height="75" style="position: absolute; top: 14px; left: 12px;"  />
            <table class="xws1" style="position: absolute; top: 14px; left: 94px;border-collapse:collapse;">
             <tr>
                <td>Expediente :</td>
                <td><asp:Label ID="ejExpediente" runat="server" Text="Label"></asp:Label></td>
             </tr>
             <tr>
                <td>Deudor :</td>
                <td><asp:Label ID="ejDeudor" runat="server" Text="Label"></asp:Label></td>
             </tr>
             <tr>
                <td>Nombre :</td>
                <td><asp:Label ID="ejdeuNombre" runat="server" Text="Label"></asp:Label></td>
             </tr>
             </table>
            
            <table  style="position: absolute; top: 7px; left: 513px;border-collapse:collapse;" cellspacing="0">
             <tr>
              
              <td>
                <a class ="xcd" href ="ejecucionesFiscales.aspx" id="EjecucionesFiscalesLink" runat="server">
                  Ejecuciones Fiscales</a>
              </td>
              <td>
                <asp:LinkButton ID="LinksiguentePasoAdministrativo" CssClass ="xcd" runat="server">Siguiente 
                  Paso</asp:LinkButton>
                <!-- <a  href ="javascript:void(0)" id = "linksiguientepaso2" runat="server"></a> -->
              </td>
             </tr>
           </table>
       </div>
       <!-- Siguiente Paso -->
       <asp:Panel ID="pnlSeleccionarSiguientePaso" runat="server" style="font-family:Verdana; display:none;" CssClass="CajaDialogo" >
         <div style=" text-align:left;width:790px; margin:10px">
            <table width="100%" class="xxtabla" >
              <tr>
               <th colspan="2" style="text-align:center;font-size:15px;width:100%">Asigne el 
                   siguiente acto administrativo</th>
              </tr>
              <tr>
              <td width="11%">Deudor :</td>
               <td><div id="lblxcdeudor" runat="server"></div></td>
              </tr>
              <tr>
               <td width="11%">Expediente :</td>
               <td><div id="lblxcExpediente" runat="server"></div></td>
              </tr>
            </table>
            <br />
            <table width="100%" class="xxtabla" cellspacing="0" rules="all" border="1">
                <tr>
                 <th style="font-size:12px;width:40%">ULTIMO ACTO ADMINISTRATIVO DETECTADO :</th>
                 <th style="text-align:center;font-size:12px;width:60%"><div id="ActoAdmind" runat="server" class="to">
                     ACTO</div></th>
                </tr>
            </table>
            
            <asp:GridView ID="dtgViewActos" runat="server" AutoGenerateColumns="False" style=" font-family:Verdana;" Width="100%" CssClass="xxtabla" BorderStyle="None">
                <Columns>
                    <asp:ButtonField DataTextField="DEP_DEPENDENCIA" HeaderText="COD." 
                        CommandName="select">
                    <HeaderStyle Width="22px" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="DEP_DESCRIPCION" HeaderText="NOMBRE" />
                    <asp:BoundField DataField="DEP_TERMINO" HeaderText="TERMINO" >
                    <HeaderStyle Width="20px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
       </div>
       <asp:Button ID="btnCancelarsiguiente" runat="server" Text="Cancelar" CssClass="Botones" style=" margin-bottom:10px; margin-left:10px;width: 92px;background-image: url('images/icons/cancel.png');" />
     </asp:Panel>
     
     <asp:Button ID="Button1" runat="server" Text="Button" style="visibility:hidden" />
     <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
            TargetControlID="Button1"
            PopupControlID="pnlSeleccionarSiguientePaso"
            CancelControlID="btnCancelarsiguiente"
            
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
     >
     </asp:ModalPopupExtender>  
     
     
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
                   var pagina = '../login.aspx';
                   location.href = pagina;
                   return false;
               }
         </script>
        <div id="xShow_ejPredio_Content" style="position:absolute;z-index:1001;top:10px;left:320px;width:420px;background-color:#fff;border: 1px solid #3c5d9c;display:none;">
            <div id="xShow_ejPredio"></div>
            <div id="cerrar" style="padding:0px 5px 7px 9px;">
              <input id="cerrar_ejPredio" type="button" value="Cerrar" />
            </div>
        </div>
        <input id="HiddenPredio" type="hidden" value="<%= Request("refcatastral") %>" />
    </form>
   </div>
</body>
</html>


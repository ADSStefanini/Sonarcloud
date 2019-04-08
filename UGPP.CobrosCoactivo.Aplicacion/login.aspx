<%@ Page Language="vb" AutoEventWireup="false" Codebehind="login.aspx.vb" Inherits="coactivosyp.login" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
		<title>Iniciar Sesion</title>
		<link rel="shortcut icon" type="image/x-icon" href="web_page.ico" />
	    <style type="text/css">
            div#container {position:relative;width: 1000px;margin-top: 0px;margin-left: auto;margin-right: auto;margin-bottom: 0px;text-align:left;background-color:#01557c;}
            body {text-align:center;margin:0;background-color:#062c63;font-family:Verdana, Geneva, sans-serif;}
  	        #sliderWrap {z-index:99;margin: 0 auto;width: 300px;}
            #slider {z-index:99;position: absolute;background-color:transparent;background-image:url(slider.png);background-repeat:no-repeat;background-position: bottom;width: 300px;height: 163px;margin-top: -137px;}
            #slider img {outline: none !Important;border: 0;}
            #sliderContent {margin: 10px 0 0 10px;position: absolute;text-align:left;color:white;font-weight:bold;padding: 3px 10px 10px 10px;}
            #openCloseWrap {position:absolute;margin: 143px 0 0 120px;font-size:12px;font-weight:bold;}
            .RedButton {height: 23px; background-color: #DF0101; border-bottom: 1px solid #555555;border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 10px;color:#FFF;}
            .RedButton:hover /* Efectos del Mouse en los botones */
            {height: 23px; background-color: #FE2E2E; border-bottom: 1px solid #AAAAAA;border-right:1px solid #AAAAAA; border-top:0px; border-left:0px;}
            img {border: none;}
            .CajaDialogo {background: #fff;border-width: 7px;border-style: solid;border-color: #FB6464;padding: 0px;font: .7em Tahoma, Arial, sans-serif; line-height: 1.7em; color: #454545;background: #fff url(images/bg.png) repeat-x;}
            a { color: #2F637A; background: inherit;}
            a:hover { color: #808080; background: inherit;}
            p {	margin: 0  0 5px 0; }
            h1 {margin: 2px; font-size:20px;}
            h2 {margin: 0px; font-size:13px;}
            h1 a, h2 a {color: #000; background: inherit; text-decoration: none;}
            #logo {margin: 0 0 2px 0; }
            #slogan {font-size: 0.9em; margin: 0 0 10px 2px; padding: 0; color: #808080; background: #fff;}
            hr {background-color: #DF0101;height: 3px; border: 0;}
            .Botones {height: 25px;background-color: #dddddd; border-bottom: 1px solid #555555;border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 12px;color: #000;padding-left: 20px; background-repeat: no-repeat;cursor:hand;cursor:pointer;outline-width:0px;background-position: 4px 4px;outline-width:0px;}
            .Botones:hover {height: 25px; background-color: #cccccc; border-bottom: 1px solid #000;border-right:1px solid #000; border-top:0px; border-left:0px;font-size:12px; color:#000;padding-left: 20px; background-repeat: no-repeat;cursor:hand;cursor:pointer;outline-width:0px;}
            .FondoAplicacion {background-color: black;filter: alpha(opacity=70); opacity: 0.7;z-index:1001;}

            #PnlPerfil { padding: 5px 40px;}
            #PnlPerfil #lblPerfil {
                display: block;
                padding: 2px 0;
            }
            #PnlPerfil #ddlPerfil {
                margin: 5px 0px;
            }
   </style>
       
       
       <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
       <script language="javascript" type="text/javascript" >
           $(document).ready(function() {
               $("#TxtUserId").focus();
               $("#BtnAceptar").click(function(evento) {
                   var user = $("#TxtUserId").val();
                   var passw = $("#TxtPwd").val();
                   if (user == "" && passw == "") {
                       evento.preventDefault();
                       $("#CustomValidator1").css("visibility", "visible");
                       $("#CustomValidator1").hide().fadeIn();
                       $("#CustomValidator1").html("Para ingresar al sistema digite su usuario y contraseña.");
                       $("#TxtUserId").focus();
                       return false;
                   }
                   else if (user == "" || passw == "") {
                       evento.preventDefault();
                       $("#CustomValidator1").css("visibility", "visible");
                       if (user == "") {
                           $("#TxtUserId").focus();
                           $("#CustomValidator1").html("No puede ingresar al sistema sin un usuario.");
                       }
                       else if (passw == "") {
                           $("#TxtPwd").focus();
                           $("#CustomValidator1").html("No puede ingresar al sistema si una cotraseña.");
                       }
                       $("#CustomValidator1").hide().fadeIn();
                   }
               });

               $(".topMenuAction").click(function() {
                   if ($("#openCloseIdentifier").is(":hidden")) {
                       $("#slider").animate({
                           marginTop: "-137px"
                       }, 500);
                       $("#topMenuImage").html('<img src="open.png" alt="open" />');
                       $("#openCloseIdentifier").show();
                   } else {
                       $("#slider").animate({
                           marginTop: "0px"
                       }, 500);
                       $("#topMenuImage").html('<img src="close.png" alt="close" />');
                       $("#openCloseIdentifier").hide();
                   }
               });

               $("#cmdAceptar").click(function () {
                   $("#cmdAceptar, #btnClose2").attr("readonly", true)
               });
           });
	    </script>
	    <script language="javascript" type="text/javascript">
	               
	        function init_pagina() {
                try {
                    
                    document.getElementById('TxtUserId').focus();
                }
                catch (err) {
                     txt = "Había un error en esta página.\n\n";
                     txt += "Descripcion del Error: " + err.description + "\n\n";
                     txt += "Click en aceptar o continuar.\n\n";
                     alert(txt);
                }
                return false;
            }
	    </script>
	</head>
	<body onload="javascript:init_pagina()" oncontextmenu="return false" onselectstart="return false" ondragstart="return false" oncopy="return false">
	 <div id="container">
		 <form id="Form1" runat="server">
		   
		   <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
            EnableScriptGlobalization="True">
           </asp:ToolkitScriptManager>
           
    
	       <div id="sliderWrap">
		       <div id="openCloseIdentifier"></div>
		           <div id="slider">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="sliderContent">
		                       Entidad : 
		                       <br />
                               <asp:dropdownlist id="DropDownList1" runat="server" DataValueField="codigo" DataTextField="nombre" DataMember="entescobradores" DataSource="<%# DsPensiones1 %>" Width="222px"  AutoPostBack="True">
					           </asp:dropdownlist>
					           <br />
					           Impuesto :
					           <asp:dropdownlist id="DropImpuesto" runat="server"  Width="260px">
					           </asp:dropdownlist>
					           <div id="LoginSD" runat="server" style="font-size:xx-small;color:#f7f714; padding-top:3px;width:260px"></div>
		                    </div>
		                    <input id="Hiddenssrutalocalexpediente" type="hidden" runat="server" />
                            <input id="Hiddenssrutaexpediente" type="hidden" runat="server" />
                            <input id="HiddenssCampoClave" type="hidden" runat="server" />
                        </ContentTemplate>
                       </asp:UpdatePanel>
			           <div id="openCloseWrap">
			                <a href="#" class="topMenuAction" id="topMenuImage" style="outline: none !Important;">
				                <img src="open.png" alt="open" />
			                </a>
			           </div>
		            </div>
	        </div>
	        
	       
		    
            <asp:TextBox style="z-index:7;position:absolute;top:302px;left:787px; width:170px;" 
               id="TxtUserId" runat="server" ></asp:TextBox> 
                
            <asp:TextBox style="z-index:7;position:absolute;top:351px;left:787px; width:170px;" 
               id="TxtPwd" runat="server" TextMode="Password"></asp:TextBox>  
            
            <asp:Button CssClass="Botones" style="z-index:7;position:absolute;top:385px;left:787px;width:85px; background-image: url('images/1318603145_arrow_in.png');"  
               id="BtnAceptar" runat="server" ValidationGroup="textovalidados" 
               Text="Aceptar" />  
            

			<asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>USUARIO</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "TxtUserId"  Display="None"></asp:RequiredFieldValidator>
            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
            </asp:ValidatorCalloutExtender>   
     
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ErrorMessage="El campo <strong>CONTRASEÑA</strong> es requerido. Verifique"  ValidationGroup="textovalidados" ControlToValidate = "TxtPwd"  Display="None"></asp:RequiredFieldValidator>
            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RequiredFieldValidator1">
            </asp:ValidatorCalloutExtender>
																	
		    <asp:customvalidator style="z-index:7;position:absolute;top:419px; left:724px; height:40px; width:268px" 
               id="CustomValidator1" runat="server" 
                                                                    Font-Names="Verdana" 
               Font-Size="XX-Small"></asp:customvalidator>
			<!-- Insert content here -->
			<div style="z-index:5;position:absolute;top:0px; left:0px;background-repeat: no-repeat;background-image: url(Imagenes/chicaplantillanet_2_01.png);height:86px;width:1000px; background-color:#ffffff;"> </div>
			
			<div style="z-index:5;position:absolute;top:86px; left:0px;background-repeat: no-repeat;background-image: url(Frontal.jpg);height:384px;width:1000px;background-color:#ffffff;"> </div>
			
			<div style="z-index:5;position:absolute;top:470px;left:0px;width:1000px; height:700px;padding:0px;background-color:#ffffff;">
			    <iframe width="1000px" height="700px" src="Public.html" frameborder="0" scrolling="no">
                     Si ves este mensaje, significa que tu navegador no soporta esta característica o está deshabilitada.
                </iframe>
			</div>
			
                
			<div style="z-index:5;position:absolute;top:1170px; left:0px;padding:7px;background-color:#1295f1;font-size:12px;font-family:Tahoma, Geneva, sans-serif;color:#FFF;background-repeat: no-repeat;background-image: url(Imagenes/chicaplantillanet_2_02.jpg);width:986px;">
				Todos los derechos Reservados - 2000-2015 Global Corporation S.A.- UGPP Direccion de Tecnologias de la Información Versión 1.50 30 de Enero de 2019.&nbsp;
			</div>
			
			
			<asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
              <div id="logo">
                  <h1><a href="#" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
                  <p id="slogan">Gestión Documental.</p>
              </div>
              <div style="margin: 0  0 5px 0;">
                 <% 
                     If Not Me.ViewState("useractivo") Is Nothing Then
                         Response.Write(Me.ViewState("useractivo"))
                     End If
                 %>
              </div>
    		  <hr />	
		      <asp:Button style="Z-INDEX: 116; width: 105px; margin-right:5px;" id="btnSi"
				    runat="server" Text="Intentar reconexión" Height="23px" CssClass="RedButton" 
                    Width="50px"></asp:Button>   
				    
			  <asp:Button style="Z-INDEX: 116; width: 27px;" id="btnNo"
				    runat="server" Text="No" Height="23px" CssClass="RedButton"></asp:Button>    
           </asp:Panel>
		
		   <asp:Button ID="Button1" runat="server" Text="Button" style="visibility:hidden" />
		
           <asp:ModalPopupExtender ID="mpeSeleccion" runat="server" 
            TargetControlID="Button1"
            PopupControlID="pnlSeleccionarDatos"

            CancelControlID="btnNo"
            
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
            >
           </asp:ModalPopupExtender>
        <div id="tabsModal2">
        <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableScriptGlobalization="True" />--%>
        <!-- ModalPopupExtender -->
        <asp:ModalPopupExtender ID="mp2" runat="server" PopupControlID="PnlPerfil" TargetControlID="btnTest2"
            CancelControlID="btnClose2" BackgroundCssClass="FondoAplicacion">
        </asp:ModalPopupExtender>
        <asp:Panel ID="PnlPerfil" runat="server"  CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;  border-color:#01557c;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>                    
                    <div class="form-group row">
                        <asp:Label ID="lblPerfil" runat="server" Text="Seleccionar Perfil*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" />
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlPerfil" runat="server" InitialValue="Por valor Seleccione" ErrorMessage="Por favor seleccione un perfil">
                               </asp:DropDownList>
                        </div>
                    </div>                   
                    <div class="col-sm-12">
                        <asp:Button ID="cmdAceptar" runat="server" Text="Aceptar" CssClass="button" ClientIDMode="Inherit"   ValidationGroup="formPerfil"/>
                        <asp:Button ID="btnClose2" CssClass="PCGButton button" runat="server" Text="Cancelar" />
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:Panel>
        <!-- ModalPopupExtender -->
    </div>


    <asp:Button ID="btnTest2" runat="server" Text="Button" CssClass="hide" />
		    </form>
		    
		    </div>
	</body>
</html>

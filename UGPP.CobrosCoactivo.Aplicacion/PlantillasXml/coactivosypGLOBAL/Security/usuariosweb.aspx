<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usuariosweb.aspx.vb" Inherits="coactivosyp.usuariosweb2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Gestión de usuarios</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
      .RedButton 
      {
      	/*display: none;*/
      	height: 23px; background-color: #DF0101; border-bottom: 1px solid #555555;
        border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 10px;
        color:#FFF; 
      }
      .RedButton:hover /* Efectos del Mouse en los botones */
      {
        height: 23px; background-color: #FE2E2E; border-bottom: 1px solid #AAAAAA;
        border-right:1px solid #AAAAAA; border-top:0px; border-left:0px;
      }
      .FondoAplicacion
      {
        background-color: black;
        filter: alpha(opacity=70);
        opacity: 0.7;
      }
      .CajaDialogo
      {
      	background-image: url(images/icons/MSNClose.png);
      	background-repeat: no-repeat;
      	
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #FB6464;
        padding: 0px;
        color:#514E4E;
        font-weight: bold;
        font-size:12px;
        font-style: italic;
      }
      .FondoAplicacion
        {
        background-color: black;
        filter: alpha(opacity=70);
        opacity: 0.7;
        z-index:1001;
      }
      .CajaDialogo div
      {
        margin: 5px;
        text-align: center;
      }
      .ws1{color:#fff;}
      
    </style>
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#txtLogin").focus();

            $(window).scroll(function() {
                $('#message_box').animate({ top: 200 + $(window).scrollTop() + "px" }, { queue: false, duration: 700 });
            });

            $("#btnAceptar").click(function(e) {
                if (($("#txtCodigo").val() == "") || ($("#txtNombre").val() == "") || ($("#txtClave").val() == "")) {
                    e.preventDefault();
                    $("#Messenger").html("<font color='#8A0808'><b style='text-decoration:underline;'>Faltan datos<b> para proceder con la operación.</font>");
                    $("#txtLogin").focus();
                    return
                }

                var clav1 = $("#txtClave").val();
                var clav2 = $("#txtConfirmarClave").val();
                if (clav1 != clav2) {
                    e.preventDefault();
                    $("#Messenger").html("<font color='#8A0808'>La cleve o contraseña no coincide, intente digitrala una vez mas.  </font>");
                    $("#txtClave").focus();
                }

            });

            $("#btnCancelar").click(function(e) {
                e.preventDefault();
                $("#Messenger").html("");
                $("#txtNombre").val("");
                $("#txtLogin").val("");
                $("#txtCedula").val("");
                $("#txtClave").val("");
                $("#txtConfirmarClave").val("");
                $("#txtemail").val("");
                document.getElementById('txtClave').removeAttribute('disabled');
                document.getElementById('txtConfirmarClave').removeAttribute('disabled');
                $("#txtLogin").focus();

                $.ajax({
                    type: "POST",
                    url: "usuariosweb.aspx/GetData",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        $("#txtCodigo").val(msg.d);
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert("Error: " + XMLHttpRequest.responseText);
                    }
                });

            });
        });
        
        function mpeSeleccionOnCancel()
        {
            return false;
        }
        function mpeSeleccionOnOk()
        {        
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
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu4.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
    </div>
             
    <div id="container">
    <h1 id="Titulo"><a href="#">Gestión de usuarios</a></h1>
	
	<form id="form1" runat="server">
    <asp:CheckBox ID="CheckBox1" runat="server" Font-Names="Verdana" 
        Font-Size="11px" ForeColor="White" 
        style="top: 45px; left: 66px; position: absolute; height: 21px; width: 544px" 
        Text="Guardar la imagen que conforma el documento en el visor de imagen " />
    <asp:CheckBox ID="CheckBox2" runat="server" Font-Names="Verdana" 
        Font-Size="11px" ForeColor="White" 
        style="top: 62px; left: 66px; position: absolute; height: 21px; width: 400px" 
        Text="Reimprimir documentos del visor de imagen" />
        
        <asp:CheckBox ID="chkactivo" runat="server" Font-Names="Verdana" 
        Font-Size="11px" ForeColor="White" 
        style="top: 78px; left: 66px; position: absolute; height: 21px; width: 400px" 
        Text="Activar usuario." />
        
	
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:label id="Label3" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" 
         style="position:absolute;top:201px; left:80px; height: 13px;">Cedula (ID) :</asp:label>														
																																								
    <asp:label id="Label2" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" style="position:absolute;top:173px; left:80px;">Nombre 
     del Usuario :</asp:label>														
																																								
    <asp:label id="Label1" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" 
        style="position:absolute;top:145px; left:80px; bottom: 812px;">Código :</asp:label>														
     
    <asp:label id="Label7" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" style="position:absolute;top:145px; left:342px;">Nombre 
     de la sesión (Login) :</asp:label>
     
    <asp:label id="Label6" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" 
         style="position:absolute;top:229px; left:80px; bottom: 728px;">Clave 
     (Password) :</asp:label>	
        
    <asp:label id="Label5" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" style="position:absolute;top:258px; left:80px;">Nivel 
     (Permisos) :</asp:label>	
     
    <asp:label id="Label8" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" style="position:absolute;top:288px; left:80px;">Superior 
    </asp:label>													
																																								
    <asp:label id="Label4" runat="server" Font-Size="11px" Font-Names="Verdana" 
          ForeColor="White" style="position:absolute;top:229px; left:388px;">Confirmar 
     Clave :</asp:label>
     
     <span id="email" 
        
        style="position:absolute;top:201px; left:388px; color:#fff;font-size:11px; font-family:Verdana;">E-mail :</span>														
	
	<asp:TextBox ID="txtemail" runat="server" 
        style="position:absolute;top:196px; left:520px;" Width="156" TabIndex="4"></asp:TextBox>
	
	<asp:Button id="btnAceptar" runat="server" Text="Guardar" 
         style="position:absolute;top:346px; left:83px; width: 92px; background-image: url('images/icons/46.png');" 
         CssClass="Botones"></asp:Button>
     
    <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
                
         style="position:absolute;top:346px; left:182px; width: 92px; background-image: url('images/icons/cancel.png');" 
         CssClass="Botones"></asp:Button>  
         
    <asp:Button id="btnCambioClave" runat="server" Text="Cambiar Clave" title="Este botón cambia la contraseña del usuario activo. Si desea cambiar la contraseña específica de un usuario específico use el link Cambio de contraseña forzosa."
         style="position:absolute;top:346px; left:283px; width: 127px; background-image: url('images/icons/key.png');" 
    CssClass="Botones"></asp:Button>       
         
          
    <div id ="Div1" runat="server" 
        style="Z-INDEX: 125; POSITION: absolute; TOP: 880px; LEFT: 60px; font-size:11px; font-family:Verdana; width: 603px; height: 17px;"></div>
         
    <asp:Button id="btnDesactivar" runat="server" 
        Text="Activar usuarios selec." style="position:absolute;top:811px; left:314px; height:54px;width:196px; font-size:14px; background-image: url('images/icons/Config.png'); right: 270px;" 
         CssClass="Botones"></asp:Button>           
         <asp:Button id="btndesausu" runat="server" 
        Text="Desactivar usuarios selec." style="position:absolute;top:810px; left:520px; height:54px;width:216px; font-size:14px; background-image: url('images/icons/user_lock.png');" 
         CssClass="Botones"></asp:Button>           
      
    <asp:Button id="btnRecargarUser" runat="server" Text="Recargar" style="position:absolute;top:346px; left:424px; width: 98px; background-image: url('images/icons/turn_left.png');" 
         CssClass="Botones"></asp:Button> 
    
    <asp:CheckBox ID="chkselccion" runat="server" 
        style="position:absolute;top:359px; left:627px" Text = "Seleccionar todos" AutoPostBack="true" />
        
    <asp:TextBox ID="txtCedula" runat="server" 
                style="position:absolute;top:196px; left:216px;" Width="156" 
        TabIndex="3"></asp:TextBox>
                  
    <asp:TextBox ID="txtCodigo" runat="server" 
                style="position:absolute;top:141px; left:216px; width: 114px;text-align:center" 
         Enabled="False"></asp:TextBox>
       
    <asp:TextBox ID="txtLogin" runat="server" 
                style="position:absolute;top:141px; left:520px;" Width="156" 
        TabIndex="1"></asp:TextBox>   
        
    <asp:TextBox ID="txtNombre" runat="server"                 
                style="position:absolute;top:168px; left:216px; width: 460px;" 
        TabIndex="2"></asp:TextBox>
        
    <asp:TextBox ID="txtClave" runat="server" 
                
        style="position:absolute;top:224px; left:216px; Width:131px; right: 433px;" Width="131" 
                CssClass="userBox" TextMode="Password" TabIndex="5"></asp:TextBox>
         
    <asp:TextBox ID="txtConfirmarClave" runat="server" 
        style="position:absolute;top:224px; left:520px; Width:131px" Width="131" 
        CssClass="userBox" TextMode="Password" TabIndex="6"></asp:TextBox>    
     
    <asp:dropdownlist id="lstNivel" runat="server" 
        style="z-index: 1; left: 216px; top: 253px; position: absolute; width: 434px;" 
        CssClass="dropDownListStyle" TabIndex="7">
			<asp:ListItem Value="1">1 (Super Administrador)</asp:ListItem>
			<asp:ListItem Value="2">2 (Supervisor)</asp:ListItem>
			<asp:ListItem Value="3">3 (Revisor)</asp:ListItem>
			<asp:ListItem Value="4">4 (Gestor - Abogado)</asp:ListItem>
			<asp:ListItem Value="5">5 (Repartidor)</asp:ListItem>
			<asp:ListItem Value="6">6 (Verificador de pagos)</asp:ListItem>
			<asp:ListItem Value="7">7 (Creador de usuarios)</asp:ListItem>
			<asp:ListItem Value="8">8 (Gestor de información)</asp:ListItem>
	</asp:dropdownlist>
	
	
	<asp:dropdownlist id="cboSuperior" runat="server" 
        style="z-index: 1; left: 216px; top: 283px; position: absolute; width: 434px;" 
        CssClass="dropDownListStyle" TabIndex="8">			
	</asp:dropdownlist>
    
   	<asp:Panel runat="server" ScrollBars="Vertical"  style="POSITION: absolute; TOP: 385px; LEFT: 4px; width: 764px; height: 301px" > 
    <asp:GridView ID="dtgUsuarios" runat="server"  
         style="Z-INDEX: 125; POSITION: absolute; LEFT: 32px; width: 714px; height: 18px;" 
         CellPadding="3" ForeColor="#333333" GridLines="None" 
         AutoGenerateColumns="False" Font-Size="13px" 
         AllowSorting="True" DataKeyNames="Codigo">
        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
        <Columns>
            <asp:ButtonField CommandName="Select" DataTextField="Codigo" HeaderText="Codigo" >
                <HeaderStyle Width="50px" />
                <ItemStyle Width="50px" />
                
            </asp:ButtonField>
            <asp:BoundField DataField="login" HeaderText="Usuario">
                <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="70px" />
            </asp:BoundField>
            <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="400px" />
            </asp:BoundField>
            <asp:BoundField DataField="nivelacces" HeaderText="Nivel">
                <HeaderStyle Width="40px" HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Center" Width="10px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Activar">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelec" runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
           </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
     		</asp:Panel>																
	<a class="Ntooltip" href="#"         
        style="POSITION: absolute; z-index:9999; TOP: 254px; LEFT: 661px; width: 17px; height: 18px; right: 103px;">
          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
          <span style="z-index:9999">
            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Guardar Usuario.
            </b>
            <br />
             Permiten asignar permisos o  derechos de acceso  a los diferentes módulos para determinados <font color="#000"><b>usuarios</b></font> y grupos de usuarios.
          </span>
     </a>
     
     <div id ="Messenger" runat="server" 
        
        style="Z-INDEX: 125; POSITION: absolute; TOP: 313px; LEFT: 82px; font-size:11px; font-family:Verdana; width: 596px;"></div>
     <div style="position:absolute;top:722px; left: 35px; width: 710px; padding-top:3px;border-top:2px solid #fff" 
        class="ws1">
          <b>Nota :</b>
          <ol style=" margin:4px 0 0 40px;padding:0;text-align:justify;">
             <li>Puede Asignar o denegar los diferentes módulos u opciones del menú principal que puede manipular el usuario pulsando el botón Asignar Opciones. </li>
             <li>Asegúrese de que el modulo de manipulación de usuario solo lo manipulen los usuarios administradores.</li>
          </ol>
     </div>
       
     <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="height: 61px; width: 341px;display: none;">
          <div>
             <% 
                 If Me.ViewState("useractivo") Is Nothing Then
                 Else
                     If ViewState("useractivo") = 1 Then
                         Response.Write("Esta apunto de activar los usuarios seleccionado ¿Desea continuar?.")
                     Else
                         Response.Write("Esta apunto de desactivar los usuarios seleccionado <br> tenga encuenta no desactivar los usuaios admin <br> ¿Desea continuar..?.")
                     End If
                 End If
             %>
          </div>
       
              
          <asp:Button style="Z-INDEX: 116; POSITION: absolute; TOP: 31px; LEFT: 304px; width: 27px;" id="btnNo"
				runat="server" Text="No" Height="23px" CssClass="RedButton"></asp:Button>
				
		  <asp:Button style="Z-INDEX: 116; POSITION: absolute; TOP: 31px; LEFT: 268px; width: 30px;" id="btnSi"
				runat="server" Text="Si" Height="23px" CssClass="RedButton"></asp:Button>   
      </asp:Panel>
       
    <%--<asp:ModalPopupExtender ID="mpeSeleccion" runat="server" 
            TargetControlID="btnDesactivar"
            PopupControlID="pnlSeleccionarDatos"
             
            CancelControlID="btnNo"
            
            OnCancelScript="mpeSeleccionOnCancel()" 
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
      >
      </asp:ModalPopupExtender>--%>
      
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
		    				    
			  <asp:Button style="Z-INDEX: 116; width: 75px;" id="btnNoerror" runat="server" Text="Aceptar" Height="23px" CssClass="RedButton"></asp:Button>    
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
              function mpeSeleccionOnCancel_() {
                  var pagina = 'menu4.aspx'
                  location.href = pagina
                  return false;
              }
        </script>   
        <div style="position:absolute;top:105px; left:28px; width: 727px;" class="divhisto">
                <table width="100%">
                    <tr>
                        <th style="text-align: right;font-size:11px;padding:4px; width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                        <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;">Ente :</th>
                        <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;"><span ID="lblCobrador" runat="server"></span></th>
                    </tr>
                </table>
        </div>
        <div id="Opcionevarias" style="position:absolute;top:699px; left:63px;">
            <%--<asp:LinkButton ID="HyperLinkImprimir" runat="server">Imprimir lista completa de usuarios</asp:LinkButton>--%>
            <%--<asp:LinkButton ID="HyperLinkUser" runat="server" title="Cambia la contraseña de un usuario de forma forzosa en caso que no recuerde su contraseña.">Cambio de contraseña forzosa</asp:LinkButton>--%>
        </div>
   </form>    
  </div>
    
 </body>
</html>

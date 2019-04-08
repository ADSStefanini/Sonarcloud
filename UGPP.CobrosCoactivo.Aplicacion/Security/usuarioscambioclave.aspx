<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="usuarioscambioclave.aspx.vb" Inherits="coactivosyp.usuarioscambioclave" %>
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
    
    
	
    <div id ="Div1" runat="server" style="Z-INDEX: 125; POSITION: absolute; TOP: 943px; LEFT: 63px; font-size:11px; font-family:Verdana; width: 603px; height: 17px;"></div>
         
    <asp:Button id="btnDesactivar" runat="server" Text="Activar Usuario" style="position:absolute;top:483px; left:44px; height:54px;width:242px;font-size:14px; background-image: url('images/icons/Config.png');" 
         CssClass="Botones"></asp:Button>           
      
    <asp:GridView ID="dtgUsuarios" runat="server"  
         style="Z-INDEX: 125; POSITION: absolute; TOP: 157px; LEFT: 37px; width: 714px; height: 18px;" 
         CellPadding="3" ForeColor="#333333" GridLines="None" 
         AutoGenerateColumns="False" Font-Size="13px" 
         AllowPaging="True" AllowSorting="True">
        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
        <Columns>
            <asp:ButtonField CommandName="Select" DataTextField="Codigo" HeaderText="Codigo">
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
     										
	
     
     <div id ="Messenger" runat="server" 
        
        
        style="Z-INDEX: 125; POSITION: absolute; TOP: 559px; LEFT: 48px; font-size:11px; font-family:Verdana; width: 596px;"></div>
       
     <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="height: 61px; width: 341px;display: none;">
          <div>
             <% 
                 If Me.ViewState("useractivo") Is Nothing Then
                     Response.Write("Seleccione un usuario")
                 Else
                     Dim activodes As String = Me.ViewState("useractivo")
                     If activodes = True Then
                         Response.Write("Desea desactivar el usuario.")
                     Else
                         Response.Write("Desea activar el usuario.")
                     End If
                 End If
             %>
          </div>
       
              
          <asp:Button style="Z-INDEX: 116; POSITION: absolute; TOP: 31px; LEFT: 304px; width: 27px;" id="btnNo"
				runat="server" Text="No" Height="23px" CssClass="RedButton"></asp:Button>
				
		  <asp:Button style="Z-INDEX: 116; POSITION: absolute; TOP: 31px; LEFT: 268px; width: 30px;" id="btnSi"
				runat="server" Text="Si" Height="23px" CssClass="RedButton"></asp:Button>   
      </asp:Panel>
       
      <asp:ModalPopupExtender ID="mpeSeleccion" runat="server" 
            TargetControlID="btnDesactivar"
            PopupControlID="pnlSeleccionarDatos"
             
            CancelControlID="btnNo"
            
            OnCancelScript="mpeSeleccionOnCancel()" 
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion"
      >
      </asp:ModalPopupExtender>
       
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
   </form>    
  </div>
    
 </body>
</html>

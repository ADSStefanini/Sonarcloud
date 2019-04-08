<%@Import Namespace="System.Data.OleDb"%>
<%@Import Namespace="System.Data"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="actuaciones.aspx.vb" Inherits="coactivosyp.actuaciones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Consulta de documentos digitalizados de los entes</title>
        
        <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
        img {border: none;}

        .contenedor
        {
        float:left;	
        position:absolute;
        z-index:9999;
                
        -moz-box-shadow: 10px 10px 10px #000; /* Firefox */
        -webkit-box-shadow: 10px 10px 10px #000; /* Safari, Chrome */
        box-shadow: 10px 10px 10px #000; /* CSS3 */
        
        opacity:0;
        filter: alpha(opacity=0); 
        display:none;
        }

        .contenedor .barratitle{
        width:341px;
        height:20px;
        padding:5px;
        font-size:16px;

        font-family:verdana;
        background-color:#045FB4;
        background-image:url(images/BarraTitulo.png);
        background-repeat: repeat-x;

        cursor:pointer;
        color:#ffffff;
        }

        .contenedor .barratitle img{
        position: absolute;
        float:right;
        height:16px;
        width:16px;
        margin:2px;
        vertical-align: text-bottom;
        left: 326px;
        top: 5px;
        }

        .contenedor .parefer{
        /*position: absolute;*/
        width:349px;
        height:358px;
        /*position:absolute;*/
        background-color:#B9CFFC;
        overflow:auto;
        border-right: 1px #B9CFFC solid;
        /*
        border-bottom: 2px solid #507CD1;
        border-left: 1px #B9CFFC solid;
        border-bottom: 1px #B9CFFC solid;*/
        }

        .fadebox {
        display: none;
        position: absolute;
        top: 0%;
        left: 0%;
        width: 100%;
        height: 100%;
        background-color: black;
        z-index:1001;
        -moz-opacity: 0.8;
        opacity:.80;
        filter: alpha(opacity=80);
        }
        </style>
        <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
        <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
	    <script type="text/javascript">
            $(document).ready(function() {
                $("#txtCodigo").focus();
                
                $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		        });
		        
		        $("#btnCancelar").click(function(e){
                    e.preventDefault();
                    document.getElementById('txtTermino').disabled = true; 
                    document.getElementById('lstdependencia1').disabled = true; 
                    document.getElementById('lstdependencia2').disabled = true; 
                    
                    document.getElementById('txtTermino').value = "";
                    document.getElementById('txtCodigo').value = "";
                    document.getElementById('txtNombre').value = "";
                });
                
                function Show_Busqueda(Contenedor){
                    var elemento = $(Contenedor);

                    $("#fade").fadeTo('slow', 0.8,function(k){
                        $("#Ntooltip_span1").hide();
                        $("#contenedordial").fadeTo("slow",1)
                    });
                }
                
                $("#Ntooltip_1").bind('click', function() {
                     Show_Busqueda("#contenedordial");
                });
                
                $("#cerrar").click(function(){
                    $("#contenedordial").fadeOut("slow",function(){
                         $("#fade").fadeOut("slow");
                    });
                });
                
                 $("ul.thumb li").hover(function() {
                    $(this).css({'z-index' : '1515'}); /*Agregamos un valor de z-index mayor para que la imagen se mantenga arriba */ 
                    $(this).find('img').addClass("hover").stop() /* Le agregamos la clase "hover" */
                    .animate({
                    marginTop: '-75px', /* Las siguientes 4 líneas alinearán verticalmente la imagen */ 
                    marginLeft: '-75px',
                    top: '50%',
                    left: '50%',
                    width: '75px', /* Aquí va la nueva medida para el ancho */
                    height: '75px', /* Aquí va la nueva medida para el alto */
                    padding: '20px'
                    }, 200); /* Este valor de "200″ es la velocidad de cuán rápido/lento se anima este hover */
                    } , function() {
                    $(this).css({'z-index' : '1505'}); /* Volvemos a poner el z-index nuevamente a 0 */
                    $(this).find('img').removeClass("hover").stop() /* Quitamos la clase "hover" y detenemos la animación*/
                    .animate({
                    marginTop: '0', /* Volvemos a poner el valor de alineación como el default */
                    marginLeft: '0',
                    top: '0',
                    left: '0',
                    width: '65px', /* Volvemos el valor de ancho como al inicio */
                    height: '65px', /* Volvemos el valor de ancho como al inicio */
                    padding: '5px'
                    }, 400);
                });
                
                /**/              
                $("#cerrarsesion").hover(function(e){
                    $("#escoger").stop();
                    $("#escoger").text("Cerrar la sesión");
                    $("#escoger").fadeTo("slow",1)
                }, function() {
                    $("#escoger").fadeOut("slow");
                });
                /**/
                 $("#cambiarclave").hover(function(j){
                    $("#escoger").stop();
                    $("#escoger").text("Cambiar Clave");
                    $("#escoger").fadeTo("slow",1);
                }, function() {
                    $("#escoger").fadeOut("slow");
                });
                /**/
                 $("#cancelarsesion").hover(function(p){
                    $("#escoger").stop();
                    $("#escoger").text("Cancelar");
                    $("#escoger").fadeTo("slow",1);
                }, function() {
                    $("#escoger").fadeOut("slow");
                });    
                
		    });
		    
//             $("#dtgetapa_acto tr ").click(function(e){
//                   $(this).children('td').each(function() {
//                        car = car + $(this).text() + "|";
//                    });
//                    if (car != ""){
//                        $('#cerrar').click();                                        
//                    }
//             });	
		    
		    function CheckVal(indexCheckVal)
		    {
		        if (indexCheckVal == 0)
                {
                    if (document.getElementById('chkmanejaterm').checked == 1)
                    {
                      document.getElementById('txtTermino').disabled = false;
                      document.getElementById('lstdependencia1').disabled = false;
                      document.getElementById('lstdependencia2').disabled = false;
                    }
                    else {
                      document.getElementById('txtTermino').disabled = true; 
                      document.getElementById('lstdependencia1').disabled = true; 
                      document.getElementById('lstdependencia2').disabled = true; 
                      
                      document.getElementById('txtTermino').value = "";
                    }
                }
                return false;
		    }
            function showLightbox() {
                document.getElementById('over').style.display='block';
                document.getElementById('fade').style.display='block';
            }
            function hideLightbox() {
                document.getElementById('over').style.display='none';
                document.getElementById('fade').style.display='none';
                document.getElementById('contenedordial').style.display='none';
            }    
		</script> 
	</head>
	
	<body>
	
	
	 <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="javascript:showLightbox();"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
             
    <div id="container">
    <h1 id="Titulo"><a href="#">Gestión de actuaciones </a></h1>
		<form id="Form1" method="post" runat="server">
			
			<asp:label id="Label4" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" style="position: absolute; top: 197px; left: 38px;">Término</asp:label>
			<asp:label id="Label5" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" style="position: absolute; top: 228px; left: 38px;">Dependencia 1</asp:label>
			<asp:label id="Label6" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" 
                style="position: absolute; top: 258px; left: 38px; height: 15px;">Dependencia 2</asp:label>
			<asp:label id="Label7" runat="server" ForeColor="White" Font-Names="Arial" 
                Font-Size="X-Small" style="position: absolute; top: 98px; left: 368px;">días</asp:label>
			<asp:label id="Label3" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" style="position: absolute; top: 131px; left: 38px;">Etapa</asp:label>
			<asp:label id="Label2" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" 
                style="position: absolute; top: 99px; left: 38px; bottom: 889px;">Nombre</asp:label>
			<asp:label id="Label1" runat="server" Font-Size="11px" Font-Names="Verdana" 
                ForeColor="White" style="position: absolute; left: 38px; top: 72px;">Código</asp:label>

			<asp:TextBox id="txtTermino" runat="server" Width="56px" Enabled="False" 
                
                style="top: 193px; left: 166px; position: absolute; right: 560px;"></asp:TextBox>
			
			<asp:TextBox id="txtCodigo" runat="server" Width="56px" AutoPostBack="True" 
                style="top: 68px; left: 166px; position: absolute;"></asp:TextBox>
			
			<asp:DropDownList id="lstdependencia1" runat="server" Width="529px" 
                DataSource="<%# DsExpedientes1 %>" DataMember="actuaciones" 
                DataTextField="nombre" DataValueField="codigo" Enabled="False" 
                style="top: 224px; left: 166px; position: absolute; height: 22px">
													</asp:DropDownList>
													
			<asp:Button id="btnAceptar" runat="server" Text="Aceptar" 
                style="top: 343px; left: 280px; position: absolute; width: 92px; background-image: url('images/icons/46.png');" 
                CssClass="Botones"></asp:Button>
                
                <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
         style="position:absolute;top:343px; left:393px; width: 92px; background-image: url('images/icons/cancel.png');" 
         CssClass="Botones"></asp:Button>  
																								
			<input id="txtNombre" type="text" size="96" name="txtNombre" runat="server" style=" top: 99px;
                left: 166px;
                position: absolute;
                width: 529px;
                margin-top: 0px;"
             />
													
			<asp:CustomValidator id="CustomValidator1" runat="server" 
                ForeColor="Yellow" Font-Names="Verdana"
														Font-Size="12px" ErrorMessage="CustomValidator" 
                
                style="top: 297px; left: 38px; position: absolute; height: 16px; width: 704px;" 
                Font-Bold="True" Font-Overline="False" Font-Strikeout="False" 
                Font-Underline="False"></asp:CustomValidator>
														
			<asp:CheckBox id="chkmanejaterm" runat="server" Font-Size="11px" 
                Font-Names="Verdana" ForeColor="White"
														Text="Maneja téminos" 
                style="position: absolute; top: 164px; left: 163px;"></asp:CheckBox>
			
			<asp:DropDownList id="lstdependencia2" runat="server" Width="529px" 
                DataValueField="codigo" DataTextField="nombre" DataMember="actuaciones" 
                DataSource="<%# DsExpedientes1 %>" Enabled="False" 
                style="top: 254px; left: 166px; position: absolute; height: 22px">
													</asp:DropDownList>
			 <a class="Ntooltip" href="#" id ="Ntooltip_1"
                style="Z-INDEX: 101; POSITION: absolute; TOP: 71px; LEFT: 233px; width: 17px; height: 18px;">
                            <img src="images/icons/magnify.png" style="cursor:hand; cursor:pointer;" alt="" />
                       <span id ="Ntooltip_span1">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Codigo del Acto.
                        </b>
                        <br />
                          Si no conoce los diferentes códigos que identifican los diferentes  actos presione este botón.
                      </span>
             </a>	
     
             
             
             <div class="contenedor" id="contenedordial" style="position:absolute;top:100px;left:50%; margin-left: -179px; z-index :9999">
                 <div class="barratitle" id="BarraTitle"> Actuaciones <img id ="cerrar" src="images/icons/sign_cacel.png" alt ="" /></div>
                     <div class="parefer" id ="ContenidoDialogo"> 
                        <asp:GridView ID="dtgetapa_acto" runat="server"  
                        style="Z-INDEX: 1001;" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" 
                        AutoGenerateColumns="False" Font-Size="10px" AllowSorting="True" 
                        HorizontalAlign="Left" Height="44px">
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                        <Columns>
                        <asp:ButtonField CommandName="Select" DataTextField="codigo" 
                            HeaderText="Cod." >
                            <HeaderStyle Width="30px" Font-Size="10px" />
                            <ItemStyle Width="32px" Font-Size="10px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                            <HeaderStyle Font-Size="10px" HorizontalAlign="Left" />
                            <ItemStyle Font-Size="10px" HorizontalAlign="Left" />
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
             </div>
         	
         	
            <div id="over" class="overbox" style="z-index:9999;">
                <div id="contentx">
                    <font size="2"><strong>Gestión de usuarios</strong></font>
                    <div id="contentx_msn">Presione un botón para continuar.</div>
                    <ul class="thumb">
                    <li><a href="#"><img src="images/icons/Shutdown.png" alt="" id="cerrarsesion" /></a></li>
                    <li><a href="#"><img src="images/icons/Keys.png" alt="" id="cambiarclave" /></a></li>
                    <li><a href="javascript:hideLightbox();"><img src="images/icons/Stop.png" alt="" id="cancelarsesion" /></a></li>
                    </ul>
                </div>
            </div>
         	<div id = "escoger" 
                style="position:absolute;z-index:9999; top: 12%; left: 256px; font-weight: 700; color: #FFFFFF; font-size: 20px;-moz-opacity: 0.8;opacity:.80;filter: alpha(opacity=80); width: 302px; text-align: center;">
              
            </div>
		    <asp:Button ID="BTNmAS" runat="server" 
                style="top: 409px; left: 291px; position: absolute; height: 26px; width: 56px" 
                Text="Button" />
		</form>
	</div>
	
	<div id="fade" class="fadebox" style="color:white; text-align:left; font-size:11px;" ><a href="javascript:hideLightbox();" alt="">Cancelar</a></div>	
	</body>
</html>

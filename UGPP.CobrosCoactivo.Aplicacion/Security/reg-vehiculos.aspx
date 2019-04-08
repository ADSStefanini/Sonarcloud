<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="reg-vehiculos.aspx.vb" Inherits="coactivosyp.reg_vehiculos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title></title>
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
    <form id="form1" runat="server">
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
        <div id="container">
            <h1 id="Titulo"><a href="javascript:void(0)">Registro de Vehículos - <% Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
            <div id="selectoresBus" style="position:absolute;top:60px;left:50px;width:700px;">
                <div id="rerein" style="background-color:#507CD1;width:130px;float:left;padding-right:5px;"><a href="cobranzas2.aspx" class="Ahlink" title="Regresar reportes individuales">Reportes individuales</a></div>
                <div id="cobra" style="background-color:#507CD1;width:100px;float:left;padding-right:5px;"><a href="cobranzatipo.aspx" class="Ahlink" title="Regresar al escritorio cobranzas">Cobranzas</a></div>
                <div id="Div2" style="background-color:#507CD1;width:100px;float:left;padding-right:5px;"><a href="cobranzatipo.aspx" class="Ahlink" title="Regresar tipo informe">Tipo informe</a></div>
                <div id="Div1" style="background-color:#507CD1;width:140px;float:left;"><a href="ejecucionesFiscales.aspx" class="Ahlink" title="Regresar ejecuciones fiscales">Ejecuciones fiscales</a></div>
            </div>
            <div style="position:absolute;top:100px;left:50px;color:#fff;">
                <div>
                    <div style="float:left; width:200px">Placa</div>
                    <asp:TextBox ID="TxtPlaca" AutoPostBack="true" runat="server" MaxLength="6"></asp:TextBox>
                </div>
                <div>
                    <div style="float:left; width:200px">Propietario</div>
                    <asp:TextBox ID="TxtPropietario" AutoPostBack="true" runat="server" ></asp:TextBox>
                </div>            
                <div>
                    <div style="float:left; width:200px">Organismo de Transito</div>
                    <asp:TextBox ID="TxtOrganismo"  runat="server" ></asp:TextBox>
                </div>                
                <div>
                    <div style="float:left; width:200px">Marca</div>
                    <asp:TextBox ID="TxtMarca"  runat="server"></asp:TextBox>
                </div>        
                <div>
                    <div style="float:left; width:200px">Linea</div>
                    <asp:TextBox ID="TxtLinea" runat="server"></asp:TextBox>
                </div>
                <div>
                    <div style="float:left; width:200px">Color</div>
                    <asp:TextBox ID="TxtColor"  runat="server"></asp:TextBox>
                </div>        
                <div>
                    <div style="float:left; width:200px">Modelo</div>
                    <asp:TextBox ID="TxtModelo" style="text-align:right"  runat="server"></asp:TextBox>
                </div>                     
                <div>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" style="background-image: url('images/icons/45.png'); width:85px;" CssClass="Botones"  />
                    <input id="Button1" type="button" value="Cancelar" onclick="parent.location='reg-vehiculos.aspx'" style="background-image: url('images/icons/cancel.png');width:85px;" class="Botones"  />
                </div>
                <div style="padding:10px;padding-left:0;font-size:x-small;color:#fff;" id="resul" runat="server">Digite todos los datos para continuar.</div>
            </div>
        </div>
    </form>
</body>
</html>


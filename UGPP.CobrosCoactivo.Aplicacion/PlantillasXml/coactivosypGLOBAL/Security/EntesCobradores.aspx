<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EntesCobradores.aspx.vb" Inherits="coactivosyp.EntesCobradores" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cobranzas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        .Yup
        {
        padding: 0px;
        text-align:left;
        color: #FFFFFF;
        overflow:auto;
        font-size:11px;
        }
        .Yup table tr th
        {
        text-align :left;
        }
        div.upload {
        position: relative;
        width: 80px;
        height: 24px;
        overflow:hidden;
        background:url(btn_upload.png) left top no-repeat;
        clip:rect(0px, 80px, 24px, 0px );
        }  
        div.upload input {
        position: absolute;
        left: auto;
        right: 0px;
        top: 0px;
        margin:0;
        padding:0;
        filter: Alpha(Opacity=0);
        -moz-opacity: 0;
        opacity: 0;
        }
        a.Ntooltip:hover
        {
            background-color:#D1DDF1;
        }
        
        .xa {text-decoration: none;} 
        .xa:hover {text-decoration: underline; font-weight: bold;}
        
        div#medioarc{
        top: 512px;
        left: 41px;
        position: absolute;
        width: 697px; 
        background-color:#0b4295; 
        padding:0px; 
        margin:0px;
        }
        div#medioarc table {
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size:12px;
        text-align: left;
        margin:0px;
        width:100%;
        }
        div#medioarc table tr {
        font-size: 12px;
        font-weight:bold;
        background-color: #e2ebef;
        background-image: url(Imagenes/fondo_tr01.png);
        background-repeat: repeat-x;
        color: #34484E;
        font-family: "Trebuchet MS", Arial;
        }
        div#medioarc table tr td {
        padding: 5px;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #A4C4D0;
        border-bottom-color: #A4C4D0;
        text-align:left;
        }
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
        .ImagenesManagerClass
        {
        border: solid 2px #6E6E6E;
        overflow:auto;
        text-align: center;
        background-color: #E6E6E6;
        padding: 10px;
        text-align:center;
        margin:0;
        }
    </style>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
	  		});
		});   
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
    <h1 id="Titulo"><a href="#">Entes</a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 41px; left: 38px; padding:7px; width: 677px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    <form id="form1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   
    
    <asp:TextBox ID="txtArchivadores" runat="server" 
        style="top: 184px; left: 51px; position: absolute;width: 345px; z-index:2; bottom: 794px;"></asp:TextBox>
    
    <asp:TextBox ID="txtNombre" runat="server" 
        style="top: 140px; left: 51px; position: absolute;width: 345px; z-index:2; bottom: 838px;"></asp:TextBox>
    <asp:TextBox ID="txtCodigo" runat="server" 
        style="top: 95px; left: 51px; position: absolute;width: 82px; z-index:2;text-align:center" 
         Enabled="False"></asp:TextBox>

     <div  style="width: 150px; position:absolute;top:78px; left:52px; z-index:2; font-size: 11px; font-weight: 700; color: #666666;">
         Codigo : 
     </div>
   
     <div  style="width: 148px; position:absolute;top:167px; left:92px; z-index:2; color: #666666; font-weight: 700; font-size: 11px;">
         Archivadores :</div>
   
     <div  style="width: 148px; position:absolute;top:122px; left:52px; z-index:2; color: #666666; font-weight: 700; font-size: 11px;">
         Nombre :</div>
   
     <div class="Yup" 
        
        
        style="width: 372px; height:181px; position:absolute;top:223px; left:39px; background-color:#507CD1; z-index:1; right: 369px;">
         <asp:GridView ID="GridCobradores" runat="server" AutoGenerateColumns="False" 
             CellPadding="4" DataKeyNames="codigo" 
             ForeColor="#333333" GridLines="None" Width="100%">
             <RowStyle BackColor="#EFF3FB" />
             <Columns>
                 <asp:ButtonField CommandName="select" DataTextField="codigo" 
                     HeaderText="Codigo" Text="Botón" ShowHeader="True" >
                     <ItemStyle Width="7%" />
                 </asp:ButtonField>
                 <asp:BoundField DataField="nombre" HeaderText="Nombre" 
                     SortExpression="nombre" />
             </Columns>
             <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
             <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
             <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
             <EditRowStyle BackColor="#2461BF" />
             <AlternatingRowStyle BackColor="White" />
         </asp:GridView>
     </div>
     <div class="Yup" 
        
        
        style="position:absolute; top: 70px; left: 413px; height: 324px; width:306px; background-color:#507CD1;padding:5px; bottom: 596px;" >
        
        <h3>Subir Imagen</h3>
        <p><b>Nota :</b>
        Para asignar un logo de empresa,  favor seleccionar un cobrador y luego proceda a presionar el botón asignar imagen.
        </p>
        
        <table width="100%">
         <tr>
            <td>
                <img alt="" src="Imagenes/logo.png" id="ImgLogo" runat="server" height="126" width="126" style="width:126px; height:126px;border: solid 1px #000000;" />
            </td>
            <td style="padding:5px;">
             Seleccione Imagen
             
             <div class="upload" style="margin:4px 0px 0px 0px;">
                <asp:FileUpload ID="upload" runat="server" />
             </div>
            </td>
         </tr>
        </table>
        <br />
        <asp:DropDownList ID="DropDownListTipo" runat="server" AutoPostBack ="true" 
                style="margin: 0px 0px 5px 0px;" Width="200px">
            <asp:ListItem Value="Firma">Firma Digital</asp:ListItem>
            <asp:ListItem Value="Logo">Logo 1</asp:ListItem>
            <asp:ListItem Value="Logo2">Logo 2</asp:ListItem>
            <asp:ListItem Value="Logo3">Logo Pie pagina</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="lblCodigo" runat="server"></asp:Label>
        
        <br />
        <asp:Button ID="btnImagena" runat="server" Text="Asignar Imagen" 
                CssClass="Botones" 
                style="background-image: url('images/icons/arrow-skip.png');margin: 5px 0px 0px 0px;" 
                Width="133" />
     </div>
     
     <div  style="width: 372px; height: 150px; position:absolute;top:70px; left:39px; background-color:#D1DDF1; z-index:1; margin-bottom: 3px;">
      
     </div>  
     
    <a class="Ntooltip" href="#"
            
        style="POSITION: absolute;z-index:225;TOP: 165px; LEFT: 51px; width: 17px; height: 18px; right: 712px;">
          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
          <span style="z-index:225;">
            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Guardar
            </b>
            <br /><br />
             Nota cuando asigne la ruta, favor no digitar el ultimo backslash
            
            <br />
            <br />
            
            <b>Ejemplo :</b> C:\imagenes
          </span>
        </a>
        
        <asp:LinkButton ID="LinkProbarRuta" runat="server"  CssClass="Ntooltip" style="POSITION: absolute;z-index:225;TOP: 165px; LEFT: 73px; width: 16px; height: 16px; right: 690px;">
          <img alt="" src="images/icons/burn.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
          <span style="z-index:225;">
            <b style="background:url('images/icons/153.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Ruta
            </b>
            <br /><br />
             Este botón te permite probar una ruta previamente digitada por el usuario y verificar si este directorio es valido para la administración de los archivos o Actos Administrativos que conforman los Expedientes.
            
            <br /><br />
            <b>Favor hacer "Click" en este icono para ejecutar la operación.</b>
            <br /><br />
            
            <b>Ejemplo :</b> C:\imagenes
          </span>
        </asp:LinkButton>
        
    <asp:Button ID="btnGuardar" runat="server" CssClass="Botones" 
        style="top: 418px; left: 40px; position: absolute; height: 26px; width: 102px; background-image: url('images/icons/46.png');" 
        Text="Guardar" ValidationGroup="textovalidados" />
        
     <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
                
         style="position:absolute;top:419px; left:288px; width: 92px; background-image: url('images/icons/cancel.png');" 
         CssClass="Botones"></asp:Button>  
     
     
      <div id = "Messenger"  runat="server" 
        style="position:absolute; top: 457px; left: 40px; width: 699px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
        
     <asp:Button id="btnImprimir" runat="server" Text="Probar Impresión" 
                
         style="position:absolute;top:419px; left:150px; width: 130px; background-image: url('images/icons/add-printer.png');" 
         CssClass="Botones"></asp:Button>  
     
     <asp:RequiredFieldValidator ID="rfvNombre" runat="server"  ErrorMessage="El campo <strong>NOMBRE</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtNombre" Display="None"></asp:RequiredFieldValidator>

     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="rfvNombre">
     </asp:ValidatorCalloutExtender>
       
     <asp:RequiredFieldValidator ID="rfvArchivadores" runat="server"  ErrorMessage="El campo <strong>ARCHIVADORES</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtArchivadores" Display="None"></asp:RequiredFieldValidator>

     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvArchivadores">
     </asp:ValidatorCalloutExtender>
     
     <div style="position:absolute;left:40px; top:500px; z-index:1001; height: 2px; width: 697px; background-color:#fff;">
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
               function mpeSeleccionOnCancel_() {
                   return false;
               }
        </script>
            
        <div id="medioarc" style="display:none;" runat="server">
            <div style="background-color:#fff; margin:0px;padding:5px;">
              <h3 style="text-transform: uppercase; padding:0px; margin:0px;">Algunos archivos y muestras encontradas</h3>
            </div>
            <div style="padding:0px;">
                <asp:Table ID="Table1" runat="server">
        
                </asp:Table>
            </div>
        </div> 
   
   
             <!-- documentos asociados al deudor -->
           <asp:Panel ID="datosinf" runat="server" CssClass="CajaDialogo" style="z-index:9999; width:630px; margin-left: auto;margin-right: auto; text-align:left;display:none; padding:11px;">
              <div class="ImagenesManagerClass" style=" border-bottom:0px;width:615px;padding:5px" id="titleimg" runat="server"></div>
              <div id ="ImagenesManager" runat="server"  class="ImagenesManagerClass" style=" width:605px;height:420px;">
              
              </div>  
              <br />
               <asp:Button ID="AcepPanel2" runat="server" Text="Aceptar"  style="width: 98px; z-index:5 ;background-image: url('images/icons/accept.png');"  CssClass="Botones" />
            </asp:Panel>
            
            <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden"  />
            
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                TargetControlID="Button2"
                PopupControlID="datosinf"
                CancelControlID="AcepPanel2"
                
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
                >
            </asp:ModalPopupExtender>
            
            <script type="text/javascript">
                function imgview(){
                    document.getElementById('ImagenesManager').innerHTML = ""
                }
            </script>
    </form> 
    </div>
</body>
</html>

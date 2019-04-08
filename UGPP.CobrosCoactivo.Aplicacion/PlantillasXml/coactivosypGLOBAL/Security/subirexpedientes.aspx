<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="subirexpedientes.aspx.vb" Inherits="coactivosyp.subirexpedientes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Actualización de expedientes</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        span
        {
        	text-decoration: none;
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
                
       
        
        .SF1
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10px;
        }
      
        </style>
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.MultiFile.js" type="text/javascript"></script>
    <script type="text/javascript">
        var vaAnexo_adjuntados = 0;
        $("#txtEnte").focus();
        
        $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
		});
		
        $(function(){
            $('#PortugueseFileUpload').MultiFile({
                accept:'gif|jpg|png|jpeg|tif|tiff', max:100, STRING: {
                remove:'<img src="images/icons/150.png" height="10" border="0" width="10" alt="x" />',
                selected:'Selecionado: $file',
                denied:'Tipo de archivo invalido. (gif|jpg|png|jpeg|tif) $ext!',
                duplicate:'Este archivo fue seleccionado con anterioridad :\n$file!'
                },
                afterFileSelect: function(element, value, master_element){
                     vaAnexo_adjuntados = vaAnexo_adjuntados + 1;   
                     $("#adjuntados_server").html(vaAnexo_adjuntados);
                     $('#Anexo_adjuntados').fadeIn('slow', function() {
                        // Animation completada
                        $("#Anexo_adjuntados").html(vaAnexo_adjuntados + " expedientes adjuntados (" + vaAnexo_adjuntados + " de 1 posibles)");
                     });
                },
                afterFileRemove: function(element, value, master_element){
                  vaAnexo_adjuntados = vaAnexo_adjuntados - 1; 
                  $("#Anexo_adjuntados").html(vaAnexo_adjuntados + " expedientes adjuntados (" + vaAnexo_adjuntados + " de 1 posibles)");
                }
            });
            
            $("#btnAceptar").click(function(e){
                var numero_expedientes = vaAnexo_adjuntados;
                var fecha = $("#txtFechaRad").val();
                var entidad = $("#txtEnte").val();
                
                if (fecha == "" || entidad == "")
                {
                  e.preventDefault();
                  $("#Validator").css("visibility","visible");
                  $("#Validator").html("Para continuar digite una entidad y la fecha de radicación.");
                  return   
                }
                
                if (numero_expedientes == 0)
                {
                  e.preventDefault();
                  $("#Validator").css("visibility","visible");
                  $("#Validator").html("No puede continuar sin haber adjuntado por lo menos un expediente."); 
                }
            });
            
            $("#btnCancelar").click(function(f){
               $("#txtEnte").val("");
               $("#txtFechaRad").val("");
               $('#Anexo_adjuntados').fadeOut();
               $("input:file").MultiFile('reset');
               vaAnexo_adjuntados = 0;
               f.preventDefault();
            });
        });
      </script>
      <script type="text/javascript">
      function Rep()
      {
       window.open('cuadros/Expedientes.aspx?textbox=txtExpediente&deudor=' + document.getElementById('txtEnte').value ,'cal','width=300,height=150,left=270,top=180,scrollbars=yes')
      }
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
            <a href="menu2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
     </div>
     
    <div id="container">
    <h1 id="Titulo"><a href="#">Subida de expedientes al servidor y registro de los archivos en la base de datos</a></h1>
    
    <form id="form1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    
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
    
    
    
    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
            TargetControlID="txtFechaRad"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
     >
     </asp:CalendarExtender> 
        
    
     <asp:label id="Label1" runat="server" ForeColor="White" Font-Names="Arial" 
         Font-Size="12px" style="position:absolute;top:58px; left:45px">Deudor :</asp:label>
        
        <asp:label id="Label2" runat="server" ForeColor="White" Font-Names="Arial" 
         Font-Size="12px" style="position:absolute;top:86px; left:44px">Referencia 
    catastral : </asp:label> 
    
    <asp:label id="Label3" runat="server" ForeColor="White" Font-Names="Arial" 
         Font-Size="12px" style="position:absolute;top:86px; left:440px">Expediente :</asp:label>
     
     <asp:TextBox ID="txtEnte" runat="server" 
        style="position:absolute;top:55px; left:291px; width: 450px;"></asp:TextBox>
        
     <asp:TextBox ID="txtNroPaginas" runat="server" 
        style="position:absolute;top:134px; left:291px; width: 50px;"></asp:TextBox> 
     
     <asp:TextBox ID="txtrefecatras" runat="server" 
        style="position:absolute;top:81px; left:291px; width: 128px;"></asp:TextBox> 
     
     <a href='javascript:;' onclick = "Rep();" 
        style="position:absolute;left:589px; top:83px; text-decoration:none;"><img src="images/icons/magnify.png"  alt="" /></a>  
      
     <asp:TextBox ID="txtExpediente" runat="server" 
        style="position:absolute;top:81px; left:613px; width: 128px;"></asp:TextBox>     
     
     <asp:TextBox ID="txtFechacreacion" runat="server"  Width="110"
        style="position:absolute;top:107px; right:34px; width:110px;" 
        CssClass="CalendarioBox"></asp:TextBox>
        
    
    
    <asp:CalendarExtender ID="txtFechacreacion_CalendarExtender" runat="server"
            TargetControlID="txtFechacreacion"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
     >
     </asp:CalendarExtender>
     
     <asp:CalendarExtender ID="CalendarExtender2" runat="server"
            TargetControlID="txtFechacreacion"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy"
     >
     </asp:CalendarExtender>
      
        
     <asp:TextBox ID="txtFechaRad" runat="server"
        style="position:absolute;top:107px; left:291px; width:110px" 
        CssClass="CalendarioBox"></asp:TextBox>
        
     <div style="position:absolute;top:132px; left:530px; font-size: 10px; font-style: italic; font-family: Tahoma; color: #FFFFFF;">(Hacer click  en el cuadro para ingresar la fecha)</div>
     <asp:label style="position:absolute;top:112px; left:440px" id="Label14" 
         runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Fecha del 
        Documento :</asp:label>
         
     <asp:label style="position:absolute;top:140px; left:45px" id="Label15" 
         runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Nro de paginas del documento :</asp:label>
         
     <asp:label style="position:absolute;top:112px; left:45px" id="Label13" 
         runat="server" ForeColor="White" Font-Names="Arial" Font-Size="12px">Fecha de radicación :</asp:label>
         
    <asp:customvalidator style="position:absolute;top:185px; left:45px; width: 702px;" 
        id="Validator" runat="server" ForeColor="Yellow" Font-Names="Tahoma" Font-Size="12px"
														ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>     
    
    
    
    
    <!--  Debajo -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
            <div style="position:absolute; top: 218px; left: 395px; width: 347px; height: 284px;border: 1px double #EFF3FB;background-color:#2461BF; z-index:1">
                <div style="z-index:5;font-size:10px;color:#fff;background-color:#507CD1;padding:4px" 
                id = "Div1" runat = "server">&nbsp;&nbsp;&nbsp;<b>Actuacion seleccionada.</b></div>
            </div>
                
   	        <div style="position:absolute; top: 245px; left: 414px; width: 315px;z-index:5;" 
                id = "Etapa" runat = "server" class="wsrt"></div>
                											
   	        <div style="position:absolute; top: 267px; left: 414px; width: 315px;z-index:5;padding-top:3px; border-bottom:solid 2px #fff;" 
                id = "DivEtapa" runat = "server" class="wsrt"></div>											
           	
            <div style="position:absolute; top: 308px; left: 415px; width: 315px;z-index:5" 
                id = "Acto" runat = "server" class="wsrt"></div>
                
            <div style="position:absolute; top: 330px; left: 415px; width: 315px;z-index:5;padding-top:3px; border-bottom:solid 2px #fff;" 
                id = "DivActo" runat = "server" class="wsrt"></div>											
            
            <div style="z-index:5;position:absolute; top: 435px; left: 413px; width: 318px; color:White;font-family:Verdana;font-size:11px;padding-top:3px;border-top:2px solid #fff;">
            <b>Nota : </b> solo puede subir (Guardar) los archivos que hagan referencia ah una actuación especifica.  Puede escoger cualquier  actuación habilitada en  la tabla de la izquierda.</div>
            														
            <div style="Z-INDEX: 125;POSITION: absolute; TOP: 218px; LEFT: 45px;padding:0px;overflow:auto;border: 1px double #EFF3FB;width: 349px; height: 283px;">
                <asp:GridView ID="dtgetapa_acto" runat="server"  Width="100%" 
                     style="Z-INDEX: 125;" 
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
             										
             <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
                 style="position:absolute;top:405px; left:512px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
                 CssClass="Botones"></asp:Button>  
                 
	         
        </ContentTemplate>
      </asp:UpdatePanel>   
      <!-- Examinar expediente  -->   
     
     <asp:Button id="btnAceptar" runat="server" Text="Guardar" 
                 style="position:absolute;top:405px; left:413px; width: 92px; background-image: url('images/icons/46.png'); z-index:10" 
                 CssClass="Botones"></asp:Button>
     
     <div style="position:absolute; top: 503px; left: 45px; width: 699px; font-family:Verdana;background-color:White; z-index:1001; border-top: solid 1px #fff;">
        <div style="padding:5px;background-color:#507CD1;font-size:16px;color:#fff;">
            <img src="images/icons/help.png" width="16" height="16" alt="" style="width:16px; height:16px;vertical-align: text-top;" border="0" />
            Presione el botón examinar para agregar uno o varios expedientes.
        </div>
        <div style="background-color:White;padding:10px;font-size:11px; margin:3px;" id="examinar">
            <input id="PortugueseFileUpload" type="file" runat="server" />
        </div>
        <div id="adjuntados_server" runat="server" style="visibility:hidden;display: none;"></div>
        <div id="Anexo_adjuntados" style="padding: 2px 5px 4px 11px;font-size:10px;display: none; border-bottom: 2px solid #507CD1;"></div>
    </div>
     
    </form>
   </div>
</body>
</html>

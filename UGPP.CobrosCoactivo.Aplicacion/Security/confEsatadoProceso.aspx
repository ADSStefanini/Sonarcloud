<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="confEsatadoProceso.aspx.vb" Inherits="coactivosyp.confEsatadoProceso" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Configuracion estado del proceso</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        .tt
        {
         padding:2px;
         background-color:#507CD1;
         color:#ffffff;
         font-size:11px;
         margin: auto;
        }
        .tt img
        {
         background-color:#507CD1; 
         
        }
         a.Ntooltip:hover
        {
         z-index:1001; /* va a estar por encima de todo */
         background-color:#507CD1; /* DEBE haber un color de fondo */
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

    <form id="form1" runat="server">
    <div id="container">
    <h1 id="Titulo"><a href="#">Actos imprimibles desde el sistema</a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 36px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
        
    <div style="Z-INDEX:10;top:137px; left:29px; position:absolute;width:723px; height:250px;overflow:auto;background-color:#2461BF;border-bottom:1px solid #4371BF;border-top:1px solid #4371BF;">
        <asp:GridView ID="dtgetapa_actoCreados" runat="server"  
         style="Z-INDEX: 10; width:100%" 
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
            <asp:TemplateField HeaderText="Individual">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </ItemTemplate>
                <HeaderStyle Width="50px" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Masivo">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox2" runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                <ItemStyle HorizontalAlign="Center" Width="50px" />
            </asp:TemplateField>
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
    
    
    <div id = "Messenger"  runat="server" 
            style="position:absolute; top: 58px; left: 47px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
    
    <div id ="DetalleVisor" runat="server"  
            
            style="position:absolute;left:31px; top:471px; width: 720px; height: 16px;">
    </div>        
    
    <div style="position:absolute;background-color:#507CD1; top: 114px; left: 80px;">
        <a href ="maestro_actuaciones.aspx" class="Ahlink">Si desea registrar una nueva actuación click aquíació</a>
    </div>
    
    <div style="position:absolute;background-color:#507CD1; top: 114px; left: 416px;">
        <a href ="PrioridadEtapas.aspx" class="Ahlink">Prioridades entre procesos </a>
    </div>
    
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"  CssClass="Botones"
                
            style="position: absolute;top: 427px; left: 30px; width: 94px; background-image: url('images/icons/45.png');" />
             
    <div style="position:absolute;top:389px; right:99px;" class="tt">
        <a class="Ntooltip" href="#"
            style="width: 18px; height: 18px;">
          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
          <span style="z-index:225;">
            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Filtrar.
            </b>
            <br />
            Puede filtrar para mayor velocidad en la ejecución de su tarea  los procesos por su respectiva etapa.
          </span>
        </a>
    
        Filtrar por Etapas : <asp:TextBox ID="txtBuscar" runat="server" 
            width="300" AutoPostBack="True"></asp:TextBox>
    </div> 
    
    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
        enabled="True" 
        targetcontrolid="txtBuscar" 
        servicemethod="ObtListaEtapas" 
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
    
    <asp:Button id="btnCancelar" runat="server" Text="Cancelar" 
         style="position:absolute;top:427px; left:134px; width: 92px; background-image: url('images/icons/cancel.png'); z-index:10" 
         CssClass="Botones"></asp:Button> 	
         	
    <a href ="MenuMaestros.aspx" 
        style="position:absolute;top:969px; left: 349px;color:#ffffff;font-size:14px; text-decoration:none;	float: left;"><img src="images/icons/61.png" alt="" style="	float: left;" />Menu Principal</a>
              
    </div>
    </form>
    </body>
</html>

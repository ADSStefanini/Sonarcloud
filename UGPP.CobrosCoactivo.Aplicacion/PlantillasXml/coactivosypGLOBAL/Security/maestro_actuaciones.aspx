<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="maestro_actuaciones.aspx.vb" Inherits="coactivosyp.maestro_actuaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Entrada de Actuaciones</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
        .tt {padding:2px;background-color:#2461BF;color:#ffffff;font-size:11px;margin: auto;}
        .tt img {background-color:#2461BF;}
        .wsrt {color:#ffffff;}
        a.Ntooltip:hover {z-index:1001;background-color:#2461BF;}
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
        <h1 id="Titulo"><a href="#">Entrada de Actuaciones</a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 42px; padding:7px; width: 688px;" id ="Div2" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
        <form id="form1" runat="server">
        
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        
        <div style="position:absolute;background-color:#507CD1; top: 158px; left: 53px;">
            <a href ="maestro_etapa.aspx" class="Ahlink">Nueva Etapa Procesal</a>
        </div>
        
        <%--<div style="position:absolute;background-color:#507CD1; top: 158px; left: 208px;">
            <a href ="confEsatadoProceso.aspx" class="Ahlink">Configurar actos imprimibles desde el sistema </a>
        </div>--%>
        
        <div 
            style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 16px; font-weight: 700; top: 180px; left: 45px; padding:10px; width: 679px;">Seleccione una etapa para continuar.</div>
        
    <div style="Z-INDEX: 125;POSITION: absolute; TOP: 218px; LEFT: 45px;padding:0px;overflow:auto;border: 1px double #EFF3FB;width: 349px; height: 315px; background-color:#2461BF;">
            <asp:GridView ID="dtgetapa_acto" runat="server"  
                 style="Z-INDEX: 125;Width:100%;" 
                 CellPadding="4" ForeColor="#333333" GridLines="None" 
                 AutoGenerateColumns="False" Font-Size="10px" AllowSorting="True" 
                    HorizontalAlign="Left" Height="44px" Width="349px">
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
        
        
        <div style="position:absolute; top: 328px; left: 414px;z-index:15; color: #FFFFFF;">
            Codigo del acto :
        </div>
        <div style="position:absolute; top: 376px; left: 414px; z-index:15; color: #FFFFFF;">
            Nombre :
        </div>
        
        <asp:Label ID="lblDetalle" runat="server"  
            style ="position:absolute; top: 501px; left: 414px; width: 321px; z-index:150; color: #FFFFFF; font-size: 9px; font-weight: 700;"></asp:Label>
        <asp:TextBox ID="txtCodigo" runat="server" 
               style="top: 350px; left: 413px; position: absolute;width: 89px;text-align:center;z-index:5;" 
               Enabled="False"></asp:TextBox>
        <asp:TextBox ID="txtNombre" runat="server"         
            style="top: 398px; left: 413px; position: absolute; width: 311px; z-index:5;"></asp:TextBox>
        
        
    <div style="position:absolute; top: 218px; left: 395px; width: 347px; height: 315px;border: 1px double #EFF3FB;background-color:#2461BF; z-index:1">
        <div style="z-index:5;font-size:10px;color:#fff;background-color:#507CD1;padding:4px" 
        id = "Div1" runat = "server">&nbsp;&nbsp;&nbsp;<b>Etapa seleccionada.</b></div>
    </div>
   	
    <asp:Button ID="btnRefrescar" runat="server" Text="Refrescar Actuaciones" 
               style="top: 858px; left: 44px; position: absolute; width: 158px; z-index:5;background-image: url('images/icons/turn_left.png');" 
                   CssClass="Botones" />
                   
    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir Actos" 
               style="top:858px;left:208px;position:absolute;width:138px;z-index:5;background-image:url('images/icons/add-printer.png');" CssClass="Botones" />
        
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
               style="top: 470px; left: 561px; position: absolute; width: 98px; z-index:5;background-image: url('images/icons/cancel.png');" 
                   CssClass="Botones" />
        
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Actuacion" 
               style="top: 470px; left: 413px; position: absolute; width: 137px; z-index:5; background-image: url('images/icons/45.png');" 
                   CssClass="Botones" ValidationGroup="textovalidados" />
        
   	<div style="position:absolute; top: 245px; left: 414px; width: 315px;z-index:5;" 
        id = "Etapa" runat = "server" class="wsrt"></div>
        											
   	<div style="position:absolute; top: 267px; left: 414px; width: 315px;z-index:5;padding-top:3px; border-bottom:solid 2px #fff;" 
        id = "DivEtapa" runat = "server" class="wsrt"></div>											
   	
    <div style="z-index:5;position:absolute; top: 114px; left: 45px; width: 318px; color:White;font-family:Verdana;font-size:11px;padding-top:3px;border-top:2px solid #fff;">
    <b>Nota : </b> Para crear una actuación debe seleccionar un etapa para asociarla a la misma.</div>
      
    <div style="Z-INDEX:10;top:574px; left:44px; position:absolute;width:701px; height:250px;overflow:auto;background-color:#2461BF;">
    <asp:GridView ID="dtgetapa_actoCreados" runat="server"  
         style="Z-INDEX: 10;width:100%" 
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
    
    <div id = "Messenger"  runat="server" 
                   style="position:absolute; top: 52px; left: 46px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
    
    <div 
            style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 16px; font-weight: 700; top: 536px; left: 44px; padding:10px; width: 681px;">Actuaciones Creadas</div>
        
               <asp:RadioButtonList ID="RadioButton" runat="server" 
                   RepeatDirection="Horizontal" 
                   
                   
                
            style="top: 541px; left: 501px; position: absolute;width: 234px;z-index:5; color: #FFFFFF; font-size: 11px; font-weight: 700;">
                   <asp:ListItem Selected="True">Todos</asp:ListItem>
                   <asp:ListItem>Filtrar por etapa</asp:ListItem>
               </asp:RadioButtonList>
        
        <a href ="MenuMaestros.aspx" 
        style="position:absolute;top:969px; left: 349px;color:#ffffff;font-size:14px; text-decoration:none;	float: left;"><img src="images/icons/61.png" alt="" style="	float: left;" />Menu Principal</a>

        <asp:CheckBox ID="ChkMasivos" runat="server" 
            
            style ="position:absolute; top: 444px; left: 411px; z-index:101; font-size: 11px; font-weight: 400; color: #FFFFFF;" 
            Text="Escatimar como acto de movimientos  masivo " />
            
        <asp:CheckBox ID="ChkHistorial" runat="server" 
            
            style ="position:absolute; top: 423px; left: 411px; z-index:101; font-size: 11px; font-weight: 400; color: #FFFFFF;" 
            Text="Actos imprimibles desde el sistema" />
            
        <a class="Ntooltip" href="#"
            style="POSITION: absolute;z-index:225;TOP: 305px; LEFT: 415px; width: 17px; height: 18px;">
          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
          <span style="z-index:225;">
            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Guardar Actuaciones.
            </b>
            <br />
             Todas las actuaciones dependen de una etapa, asegúrese de escoger primero la etapa a la que pertenecerá esta nueva actuación.
            
            <br />
            <br />
            <b>Acto imprimible desde el sistema,</b> funciona para hacer seguimiento a las actuaciones procesalesa nivel de informes.
            
            <br />
            <br />
            <b>Nota :</b> Todas las opciones afectan al registro al momento de guardar o actualizar.
          </span>
        </a> 
        
         <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 9px; font-weight: 700; top: 825px; left: 44px; padding:7px; width: 238px;" id ="Dtal" runat="server">Total Actuaciones</div>
        
        <div style="position:absolute;top:825px; right:35px; width: 433px;" class="tt">
        <a class="Ntooltip" href="#"
            style="width: 16px; height: 16px;float:left;">
          <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;float:left;" />
          <span style="z-index:225;">
            <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
              Nota : Op. Filtrar.
            </b>
            <br />
            Puede filtrar para mayor velocidad en la ejecución de su tarea  los procesos por su respectiva etapa.
          </span>
        </a>
    
        <div style="float:left; font-size:10px; padding:3px;">Filtrar por Etapas : </div><asp:TextBox ID="txtBuscar" runat="server" 
            width="300" AutoPostBack="True"  style="float:left;"></asp:TextBox>
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
      <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>NOMBRE</strong> es requerido para la consulta. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtNombre" Display="None"></asp:RequiredFieldValidator>
   
       <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
       </asp:ValidatorCalloutExtender>      
        </form>
    </div>
</body>
</html>

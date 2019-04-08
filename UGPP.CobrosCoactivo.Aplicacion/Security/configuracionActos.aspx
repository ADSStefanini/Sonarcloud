<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="configuracionActos.aspx.vb" Inherits="coactivosyp.configuracionActos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    
    <title>Dependencia Actos</title>
    <style type="text/css">
        .optTerminos
        {
             font-size:11px;
             color:#ffffff;
             z-index:2;
        }

    </style>
    <script type="text/javascript">
      function Rep()
      {
       window.open('cuadros/DependenciasActos.aspx?textbox=txtservicio&etapa=' + document.getElementById('txtBuscar').value ,'cal','width=750,height=325,left=270,top=180,scrollbars=yes')
      }
    </script>
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
	        
	        //$('#txtBuscar').val("xxx")
	    });
    </script>
</head>
<body > <!-- onload="document.getElementById('txtBuscar').value = 'COBRO COACTIVO::02';" -->
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
    <h1 id="Titulo"><a href="#">Secuencia de Actos</a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 40px; left: 44px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
    <form id="form1" runat="server">
    
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
     </asp:ToolkitScriptManager>
    
    <div class="divhisto" 
         style="width: 702px; height: 63px; position:absolute;top:166px; left:43px; background-color:#507CD1; z-index:1;">
         
        <a class="Ntooltip" href="#"
            style="width: 18px; height: 18px;position:absolute;top:10px; left:10px; z-index:3"><img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;z-index:5;" />
                <span style="z-index:7;">
                 <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                  Nota : Op. Filtrar.
                 </b>
                 <br />
                    Puede filtrar para mayor velocidad en la ejecución de su tarea  los procesos por su respectiva etapa.
                </span>
          </a>
    
          <div class="ws2"  
            style="width: 214px; height: 18px;position:absolute;top:10px; left:39px; z-index:2;">
                Filtrar por Etapas : 
          </div>
    
          <asp:TextBox ID="txtBuscar" runat="server" 
                style="position:absolute;top:31px; left:10px; width: 440px;z-index:2;" 
                AutoPostBack="True">
          </asp:TextBox>
    
    </div>
    
    
    <div  style="border-right: 1px solid #6E6E6E; border-bottom: 1px solid #6E6E6E; width: 702px; height: 446px; position:absolute;top:237px; left:43px; background-color:#D1DDF1;">
         <div  style="width: 214px; height: 18px;position:absolute;top:10px; left:10px; color: #555555; font-size: 14px; text-transform: uppercase; font-weight: 700;">
            Acto principal : 
         </div>
        
         <asp:DropDownList ID="DropDownListActo1" runat="server" style="top: 32px; left: 10px; position: absolute; width: 660px;" AutoPostBack="true">
         </asp:DropDownList>
        
         <div  style="width: 691px; height: 18px;position:absolute;top:63px; left:10px; color: #555555; font-size: 14px; text-transform: uppercase; font-weight: 700;">
            Dependencia que subsiste  después del acto principal : 
         </div>
         
         <div id="DivGridView" style="width:674px; height:300px; left:12px; margin-left:12px; margin-top:90px; background-color:White">                
                <asp:GridView ID="GridDependencias" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" Width="674px" AllowPaging="True">
                    <RowStyle ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="DEP_DEPENDENCIA" HeaderText="ID." >
                        <ItemStyle Width="20px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DEP_DESCRIPCION" HeaderText="Nombre" >
                        <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DEP_TERMINO" HeaderText="Término" >
                        <ItemStyle Width="20px" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </div>
    
    </div>
    
    <div class="divhisto" 
         style="width: 702px; height: 65px; position:absolute;top:687px; left:43px; background-color:#507CD1; z-index:1;">
         <!--
        <div  style="width: 220px; height: 18px;position:absolute;top:10px; left:10px; z-index:2; color: #FFFFFF; font-size: 14px; font-weight: 700;">
            Termino : 
        </div>
        -->
        <!--
        <asp:TextBox ID="txtTermino" runat="server" 
            style="top: 30px; left: 10px; position: absolute; width: 70px; right: 686px; text-align: center;"></asp:TextBox>
        -->
        <!--
        <asp:RadioButtonList ID="OptTermino" runat="server" 
            RepeatDirection="Horizontal" 
            style="top: 25px; left: 95px; position: absolute; height: 29px; width: 149px; right: 458px;" 
            CssClass="optTerminos">
            <asp:ListItem  Value="DIA" Selected="True">Dias</asp:ListItem>
            <asp:ListItem Value="MES">Meses</asp:ListItem>
        </asp:RadioButtonList>
        -->
    </div>
    
    
    
     <asp:TextBox ID="txtservicio" runat="server" 
         style="top: 71px; left: 708px; position: absolute; visibility:hidden;" Width="25px"></asp:TextBox>
     
     <div style="position:absolute;left:58px; top:144px; background-color:#507CD1;">
         <a href='javascript:;' onclick = "Rep();" class="Ahlink">Examinar Lista de Dependencias</a>
     </div>
     
     <div id = "Messenger"  runat="server" 
         style="position:absolute; top: 496px; left: 41px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
    
    <!--
      <asp:Button ID="btnCancelar" runat="server" 
         style="top: 757px; left: 150px; position: absolute; width: 98px; background-image: url('images/icons/cancel.png'); " 
         CssClass="Botones" Text="Cancelar"  /> -->
    <!-- 
      <asp:Button ID="btnMas" runat="server" 
         style="top: 757px; left: 43px; position: absolute; width: 98px; background-image: url('images/icons/45.png'); right: 644px;" 
         CssClass="Botones" Text="Guardar" ValidationGroup="textovalidados" />
    -->
      
     
      
      
    
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
    
    
        <a href ="MenuMaestros.aspx" 
        style="position:absolute;top:969px; left: 349px;color:#ffffff;font-size:14px; text-decoration:none;	float: left;"><img src="images/icons/61.png" alt="" style="	float: left;" />Menu Principal</a>
        
        <div style="position:absolute;top:65px; left: 43px; width: 641px;" 
         class="info" id="info" runat="server">
          <b>Nota : </b>En la sección Filtrar por etapas, digite un valor, por ejemplo: Cobro coactivo. 

        </div>
        <asp:RequiredFieldValidator ID="rfvBuscar" runat="server"  ErrorMessage="El campo <strong>ETAPA</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtBuscar" Display="None"></asp:RequiredFieldValidator>

        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="rfvBuscar">
        </asp:ValidatorCalloutExtender>
        
        <asp:RequiredFieldValidator ID="rfvTermino" runat="server"  ErrorMessage="El campo <strong>TERMINO</strong> es requerido para la proceso. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtTermino" Display="None"></asp:RequiredFieldValidator>

        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvTermino">
        </asp:ValidatorCalloutExtender>
    </form>
   </div> 
</body>
</html>

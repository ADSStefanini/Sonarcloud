<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="maestro_etapa.aspx.vb" Inherits="coactivosyp.maestro_etapa" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Entrada de Etapas (ETAPAS PROCESALES) </title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $(window).scroll(function(){
	  		        $('#message_box').animate({top:200+$(window).scrollTop()+"px" },{queue: false, duration: 700});
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
    <h1 id="Titulo"><a href="#">Entrada de Etapas (ETAPAS PROCESALES)</a></h1>
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 44px; left: 36px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
        
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    
    <div style="position:absolute; top: 155px; left: 59px; z-index:1001; color: #FFFFFF;">
            Codigo :
        </div>
        
        <div style="position:absolute; top: 216px; left: 59px; z-index:1001; color: #FFFFFF;">
            Nombre :
        </div>
        
        <div style="position:absolute;background-color:#2461BF; top: 127px; left: 36px; width: 349px; height: 284px;border: 1px double #EFF3FB;">
        </div>
        
        <asp:TextBox ID="txtCodigo" runat="server" 
               style="top: 180px; left: 59px; position: absolute;width: 89px;text-align:center;z-index:1001;" 
               Enabled="False"></asp:TextBox>
               
        <asp:TextBox ID="txtNombre" runat="server" 
               
        style="top: 239px; left: 59px; position: absolute; width: 304px; z-index:1001; right: 417px;"></asp:TextBox>
        
        
       <div style="Z-INDEX: 125;POSITION: absolute; TOP: 127px; LEFT: 387px; padding:0px;overflow:auto;border: 1px double #EFF3FB;width: 349px; height: 284px;background-color:#2461BF;">
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
    
    
    <div style="z-index:5;position:absolute; top: 360px; left: 58px; width: 306px; color:White;font-family:Verdana;font-size:11px;padding-top:3px;border-top:2px solid #fff;">
    <b>Nota : </b> Para crear una nueva etapa administrativo solo debe digitar su nombre y presionar el botón guardar.</div>
      
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
               style="top: 318px; left: 188px; position: absolute; width: 99px; z-index:5; background-image: url('images/icons/cancel.png');" 
                   CssClass="Botones" />
        
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Etapa" 
               style="top: 318px; left: 60px; position: absolute; width: 121px; z-index:5; background-image: url('images/icons/45.png');" 
                   CssClass="Botones" ValidationGroup="textovalidados" />
        
    <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 414px; left: 36px; padding:7px; width: 688px;" id ="Dtal" runat="server">Etapa Creadas</div>
    
    
    <div style="position:absolute;background-color:#507CD1; top: 104px; left: 45px;">
        <asp:LinkButton ID="LinkIrActo" runat="server" 
             CssClass="Ahlink">Crear Actuaciones</asp:LinkButton>
    </div>
    
    <div id = "Messenger"  runat="server" 
                   
        style="position:absolute; top: 50px; left: 46px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
        
        
        <a href ="MenuMaestros.aspx" 
        style="position:absolute;top:969px; left: 349px;color:#ffffff;font-size:14px; text-decoration:none;	float: left;"><img src="images/icons/61.png" alt="" style="	float: left;" />Menu Principal</a>
        
        <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>NOMBRE</strong> es requerido para la consulta. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtNombre" Display="None"></asp:RequiredFieldValidator>
   
       <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
       </asp:ValidatorCalloutExtender>
        
        
    </form>
    </div>
    
    
    
    </body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Festivos.aspx.vb" Inherits="coactivosyp.Festivos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Captura de festivos y días no hábiles </title>
    <link href="../EstiloPPal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tituloCap
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 21px;
            text-transform: uppercase;
            color: #FFFFFF;
            font-weight: 900;
        }
        
        .CajaDialogo
        {
            /*background-image: url(images/icons/MSNClose.png);
            background-repeat: no-repeat;*/
            position:absolute;
            padding:10px;
            background-color:#f0f0f0;
            border-width: 7px;
            border-style: solid;
            border-color: #72A3F8;
            color:#000;
            z-index:101;
       }
       .eliminar
       {
            
            font-size:10px;
            position:absolute;
            padding:10px 5px 5px 5px;
            background-color:#72A3F8;
            top: 355px;
            left: 350px;
            z-index:1;
        }
        .eliminar a
        {
         text-decoration:none;
         color:#B40404;
        }
       .CajaDialogo .inf
       {
            font-weight: bold;
            font-size:12px;
            font-style: italic;
       }
    </style>
    <script src="../js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function fechashow()
        {
            document.getElementById('txtDescripcion').focus();
        }

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
        <a href="../MenuMaestros.aspx"><img alt="" src="../imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
     </li>
     <li style="height:152px;width:36px;">
        <a href="../MenuMaestros.aspx"><img alt="" src="../imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
     </li>
    </ul>
   </div>
   
    <div id="container">
    <h1 id="Titulo"><a href="#">Captura de festivos y días no hábiles </a></h1>
    <form id="form1" runat="server">
    
    <div id = "Messenger"  runat="server" 
                   
        
        style="position:absolute; top: 388px; left: 40px; width: 703px; color: #FFFFFF; font-size: 12px; font-weight: 700;"></div>
        
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True" >
    </asp:ToolkitScriptManager>
    
            
    
    <div style="position:absolute; top: 83px; left: 164px;" class="tituloCap">Captura de festivos y días no hábiles</div>
    
    <div class = "CajaDialogo" 
        style="position:absolute;top: 140px; left: 286px; width: 200px;">
        <div class="ws1 inf">
             Ingresar Fecha :
        </div>
        
        <br /> 
        
        <asp:TextBox ID="txtFecha" runat="server" style="width: 152px" 
            class="CalendarioBox"></asp:TextBox>
        
        <br /> <br />
        
        <div class="ws1 inf">
             Ingresar Descripción :
        </div>
        
        <br />
        
        <asp:TextBox ID = "txtDescripcion" runat="server" 
            Width="171px"  
        ></asp:TextBox>
        
        <br /> 
        <br />
        
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="Botones" 
            style ="background-image: url('../images/icons/45.png');" Width="92px" />
            
        &nbsp;<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="Botones" 
            style ="background-image: url('../images/icons/cancel.png');" 
            Width="92px" />    
        <br />
        
            
        <asp:Label ID="lbleditar" runat="server" 
            style="font-weight: 700; font-size: 10px"></asp:Label>
    </div>
    
    
    <div style="position: absolute;color:White;font-family:Verdana;font-size:11px;padding-bottom:3px;padding-top:3px;border-top:2px solid #fff; top: 419px; left: 40px; width: 715px;"><b>Nota :</b> Puede seleccionar una fecha previamente creada y editarla </div>
            
    <div style="Z-INDEX: 125;POSITION: absolute; TOP: 444px; LEFT: 40px; padding:0px;overflow:auto;border: 1px double #EFF3FB;width: 349px; height: 284px;background-color:#2461BF;">
    <asp:GridView ID="dtfecha" runat="server"  
         style="Z-INDEX: 125;Width:100%;" 
         CellPadding="4" ForeColor="#333333" GridLines="None" 
         AutoGenerateColumns="False" Font-Size="10px" AllowSorting="True" 
            HorizontalAlign="Left" Height="44px">
        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
        <Columns>
            <asp:ButtonField CommandName="Select" DataTextField="ID_DNL" 
                HeaderText="Cod." >
                <HeaderStyle Width="30px" Font-Size="10px" />
                <ItemStyle Width="32px" Font-Size="10px" />
            </asp:ButtonField>
            <asp:BoundField DataField="DESCRIPCION" HeaderText="Descripción" >
                <HeaderStyle Font-Size="10px" HorizontalAlign="Left" />
                <ItemStyle Font-Size="10px" HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="FECHA" HeaderText="Fecha">
                <FooterStyle HorizontalAlign="Left" />
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
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
            
<asp:DropDownList ID="ListAnnios" runat="server" 
        style="position:absolute; top: 480px; left: 401px; width: 100px;" 
        AutoPostBack="True">
</asp:DropDownList>      

    
    
            
    <asp:Label ID="Label1" runat="server" 
        
        
        style="font-weight: 700; font-size: 10px;position:absolute; top: 446px; left: 401px; width: 258px;"><font 
        color="#ffffff">Años detectados en base de datos </font>
    <br />
    (Previamente digitados por el usuario)</asp:Label>
    
    <asp:CheckBox ID="chkAnnio" runat="server" 
        style="position:absolute; top: 508px; left: 399px;" Font-Bold="True" 
        Font-Names="Verdana" Font-Overline="False" Font-Size="11px" 
        Font-Strikeout="False" Font-Underline="False" ForeColor="White" 
        Text="Solo quiero trabajar con el año escogido en la lista " />
    
    <asp:CalendarExtender ID="CalendarExtender1" runat="server"
            TargetControlID="txtFecha"
            Format="dd/MM/yyyy"
            PopupButtonID="Image1" TodaysDateFormat="dd, MMMM, yyyy" 
        DefaultView="Years" onclienthidden="fechashow"
     >
     </asp:CalendarExtender>
     
    <asp:Panel ID="Panel1" runat="server" CssClass="eliminar" Visible="false">
        <asp:LinkButton ID="LinkEliminarFestivo" runat="server"><img src="../images/icons/101.png" alt ="" style="float:left;" />&nbsp;Eliminar Festivo</asp:LinkButton>
    </asp:Panel>
     
    </form>
    </div>
</body>
</html>

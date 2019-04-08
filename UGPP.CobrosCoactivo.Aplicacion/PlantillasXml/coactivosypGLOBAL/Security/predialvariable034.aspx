<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="predialvariable034.aspx.vb" Inherits="coactivosyp.predialvariable034" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Tecno Expedientes !</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <!--   codigo wprens   -->
    <!-- 
    <script type="text/javascript" src="scripts/jquery-1.3.2.js"></script> 
    <link rel="Stylesheet" type="text/css" href="style/EstilosFormulario.css" />    
    -->
    <link rel="Stylesheet" type="text/css" href="style/jHtmlArea.css" />
    <script src="scripts/jHtmlArea-0.7.5.min.js" type="text/javascript"></script>
 
    <script type="text/javascript">
        $(function() {
            $("textarea").htmlarea(); // Initialize jHtmlArea's with all default values            
        });
        function TextArea1_onclick() { }
    </script>
   
    <style type="text/css">
        #TextArea1{width:772px;height:161px;}
        #TextArea2{width:770px;height:104px;}
    </style>
    <style type="text/css">
	    .ctl00_Menu1_0 { background-color:white;visibility:hidden;display:none;position:absolute;left:0px;top:0px; }
	    .ctl00_Menu1_1 { color:#284E98;font-family:Verdana;font-size:0.8em;text-decoration:none; }
	    .ctl00_Menu1_2 { color:#284E98;background-color:#B5C7DE;font-family:Verdana;font-size:0.8em;height:16px;width:759px; }
	    .ctl00_Menu1_3 {  }
	    .ctl00_Menu1_4 { padding:2px 5px 2px 5px; }
	    .ctl00_Menu1_5 {  }
	    .ctl00_Menu1_6 { padding:2px 5px 2px 5px; }
	    .ctl00_Menu1_7 { background-color:#B5C7DE; }
	    .ctl00_Menu1_8 {  }
	    .ctl00_Menu1_9 { background-color:#507CD1; }
	    .ctl00_Menu1_10 {  }
	    .ctl00_Menu1_11 { background-color:#507CD1; }
	    .ctl00_Menu1_12 { color:White; }
	    .ctl00_Menu1_13 { color:White;background-color:#284E98; }
	    .ctl00_Menu1_14 { color:White; }
	    .ctl00_Menu1_15 { color:White;background-color:#284E98; }
	    .texto-negrilla {
	        text-align:center;
	        font-family: Arial, Helvetica, sans-serif;
	        font-weight:bold;
	        font-size:15px;
	        padding-bottom:15px;
	        width: 770px;
        }
        .separarBoton  
        {
	        margin-top:50px;
	        height: 53px;
        }
        	
        .boton {
	        width: 131px;
	        height:51px;
	        background-image:url('images/boton1.png');	
	        background-repeat:no-repeat;
	        float:left;
        }

        .boton2 {
	        width: 135px;
	        height:70px;
	        background-image:url('images/imprimir1.png') ;	
	        background-repeat:no-repeat;
	        float:right;
        }
        .boton:hover, .boton:active {
	        background-image:url('images/boton2.png');
	        background-repeat:no-repeat;
        }
        	
        .boton2:hover, .boton2:active {
	        background-image:url('images/imprimir2.png');
	        background-repeat:no-repeat;
        }
        </style>
        <!-- fin codigo wprens -->
</head>
<body>
    <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="cobranzas2.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="cobranzas2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
    </div>
    
    <div id="container">
        <h1 id="Titulo"><a href="#">(034) Declara no probadas excepciones (rechaza excepción)</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:96px; position:relative; top: 40px; margin-left:6px; color:White" >                
                <!--   codigo wprens   -->
                <div class="texto-negrilla" style="text-align:left;"> 
                    Seleccione el expediente: 
                    <br />
                <ASP:TEXTBOX ID="txtNumExp" runat="server" Width="231px" AutoPostBack="True"></ASP:TEXTBOX>                  
                    <br /><br />
                    Seleccione el tipo de excepci&oacute;n que se presenta
                    
                    <ASP:LABEL ID="LBLRES" runat="server" ForeColor="#CC0000"></ASP:LABEL>
                </div>
                <div id="texto-normal" style="text-align:left; width: 774px; height: 115px;">
                    <ASP:DROPDOWNLIST ID="DropCodigoActo" runat="server" Height="19px" 
                        Width="376px">
                        <asp:ListItem Value="El pago efectivo.">1. El pago  efectivo.</asp:ListItem>
                        <asp:ListItem Value="La existencia de acuerdo de pago. ">2. La existencia de acuerdo de pago. </asp:ListItem>
                        <asp:ListItem Value="La falta de ejecutoria del título">3. La falta de ejecutoria del título</asp:ListItem>
                        <asp:ListItem Value="La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.">4. La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.</asp:ListItem>
                        <asp:ListItem Value="La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. ">5. La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. </asp:ListItem>
                        <asp:ListItem Value="La prescripción de la acción de cobro. ">6. La prescripción de la acción de cobro. </asp:ListItem>
                        <asp:ListItem Value="La falta de título ejecutivo o incompetencia del funcionario que lo profirió">7. La falta de título ejecutivo o incompetencia del funcionario que lo profirió</asp:ListItem>
                </ASP:DROPDOWNLIST>
                    <br /><br />
                    Fecha del acto administrativo<br />                    
                    <ASP:TEXTBOX ID="txtCalendario" runat="server" Width="178px"></ASP:TEXTBOX>
                    
                    <asp:CalendarExtender ID="txtCalendario_CalendarExtender" runat="server"
                      TargetControlID="txtCalendario" FirstDayOfWeek="Monday"
                      DaysModeTitleFormat="dd/MM/yy" TodaysDateFormat="dd/MM/yy" Format="dd/MM/yyyy" >
                    </asp:CalendarExtender>
                
                </div>
                
                <div class="texto-negrilla"> HECHOS:<br /> </div>
                <div style="width: 770px; background-color:White">
                    <textarea id="TextArea1" runat="server" style="width:770px; height:114px;" 
                        enableviewstate="True" onclick="return TextArea1_onclick()"></textarea></div>
                
               <br />
               <div class="texto-negrilla"> CONSIDERACIONES:<br />
                    </div>
                <div style="width: 773px; height: 144px; background-color:White">
                <textarea id="TxtArea2" runat="server" style="width:770px; height:114px;" ></textarea></div>
                <br />
                <div class="texto-negrilla"> ARTICULO :<br />
                </div>
                <div style="width: 773px; height: 144px; background-color:White">
                <textarea id="TextAreaArticulo" runat="server" style="width:770px; height:114px;" ></textarea></div>
                      
                 <div class="separarBoton" style="position:absolute; left:0px; top:740px; width:774px; ">
                    <div class="boton">
                        <ASP:LINKBUTTON ID="LinkButton1" runat="server" Width="135" Height="55"></ASP:LINKBUTTON>
                    </div>           
                    <div class="boton2">
                        <ASP:LINKBUTTON ID="Link" runat="server" Width="135" Height="55"></ASP:LINKBUTTON>
                    </div>
               </div>
                <!-- fin codigo wprens -->
            </div>
        </form>
    </div>
</body>
</html>

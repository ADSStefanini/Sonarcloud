<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="predialvariable011.aspx.vb" Inherits="coactivosyp.predialvariable011" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Tecno Expedientes !</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <style type="text/css">
	    .texto-negrilla {
	        text-align:center;
	        font-family: Arial, Helvetica, sans-serif;
	        font-weight:bold;
	        font-size:15px;
	        padding-bottom:15px;
	        width: 730px;
        }
        .separarBoton  
        {
	        margin-top:10px;
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
                <a href="cobranzas2.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;margin:0;padding:0;" /></a>   
             </li>
             <li style="height:152px;width:36px;">
                <a href="cobranzas2.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;margin:0;padding:0;" /></a>
             </li>
        </ul>
    </div>
    <div id="container">
        <h1 id="Titulo"><a href="#">(011) Declara probada la excepción - <% Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:730px;position:relative;padding:15px;margin:0px 10px 10px 10px;color:#fff;">                
                <div style="text-align:left;"> 
                    Seleccione el expediente: 
                    <br />
                    <ASP:TEXTBOX ID="txtNumExp" runat="server" Width="231px" AutoPostBack="True"></ASP:TEXTBOX>                  
                    <br /><br />
                    Seleccione el tipo de excepci&oacute;n que se presenta
                </div>
                <div id="texto-normal" style="text-align:left;width:774px;">
                    <ASP:DROPDOWNLIST ID="DropCodigoActo" runat="server" Width="376px">
                        <asp:ListItem Value="El pago efectivo.">1. El pago efectivo.</asp:ListItem>
                        <asp:ListItem Value="La existencia de acuerdo de pago.">2. La existencia de acuerdo de pago. </asp:ListItem>
                        <asp:ListItem Value="La falta de ejecutoria del título">3. La falta de ejecutoria del título</asp:ListItem>
                        <asp:ListItem Value="La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.">4. La pérdida de ejecutoria del título por revocación o suspensión provisional del acto administrativo.</asp:ListItem>
                        <asp:ListItem Value="La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. ">5. La interposición de demandas de restablecimiento del derecho o de proceso de revisión de impuestos, ante la jurisdicción de lo contencioso administrativo. </asp:ListItem>
                        <asp:ListItem Value="La prescripción de la acción de cobro.">6. La prescripción de la acción de cobro. </asp:ListItem>
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
                <div><asp:Label ID="lblres" runat="server" Text="Digite el número de expediente para continuar." ForeColor="#800000" Font-Bold="true"></asp:Label></div>
                <div class="texto-negrilla">HECHOS:</div>
                <div style="width:730px; background-color:White">
                    <cc1:Editor ID="EditorHechos" runat="server" Height="180" AutoFocus="false" />
                </div>
                <br />
                <div class="texto-negrilla">CONSIDERACIONES:</div>
                <div style="width:730px;background-color:White">
                    <cc1:Editor ID="EditorConsideraciones" runat="server" Height="180" AutoFocus="false" />
                </div>
                <br />
                <div class="texto-negrilla">ARTICULO:</div>
                <div style="width:730px;background-color:White">
                    <cc1:Editor ID="EditorArticulos" runat="server" Height="180" AutoFocus="false" />
                </div>
                <br />
                <div style=" clear:both;"></div>
                <div class="separarBoton" style="width:730px;">
                    <div class="boton">
                        <ASP:LINKBUTTON ID="LinkButton1" runat="server" Width="135" Height="55"></ASP:LINKBUTTON>
                    </div>           
                    <div class="boton2">
                        <ASP:LINKBUTTON ID="Link" runat="server" Width="135" Height="55"></ASP:LINKBUTTON>
                    </div>
                    <div style=" clear:both;"></div>
                </div>
            </div>
        </form>
    </div>
</body>
</html>



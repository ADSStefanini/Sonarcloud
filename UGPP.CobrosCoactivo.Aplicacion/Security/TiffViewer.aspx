<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TiffViewer.aspx.vb" Inherits="coactivosyp.TiffViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Visualizador de imágenes</title>
    <link href="Libertad.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .Botones /* Estilo de los botones */
        {
        height: 25px; 
        background-color: #FFF; border-bottom: 1px solid #555555;
        border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 12px;
        color: #000; 
        padding-left: 20px; background-repeat: no-repeat; cursor:hand; cursor:pointer;
        outline-width:0px;
        background-position: 4px 4px;outline-width:0px;
        }
        .Botones:hover /* Efectos del Mouse en los botones */
        {
        height: 25px; background-color: #F2F2F2; border-bottom: 1px solid #000;
        border-right:1px solid #000; border-top:0px; border-left:0px;
        font-size:12px; color:#000;padding-left: 20px; background-repeat: no-repeat;
        cursor:hand; cursor:pointer;outline-width:0px;
        }
        .CajaDialogo
        {
        position:absolute;
        background-color:#f0f0f0;
        border-width: 7px;
        border-style: solid;
        border-color: #72A3F8;
        padding: 0px;
        color:#514E4E;
        font-size:12px;
        } 
        div#container
        {
        position:relative;
        width: 1020px;
        height:500px;
        margin-top: 0px;
        margin-left: auto;
        margin-right: auto;
        margin-bottom: 0px;
        text-align:left; 
        background-color:#7f7f7f;
        }
        body 
        {
        text-align:center;
        margin:0;
        background-color:#7f7f7f;

        font-family:Verdana, Geneva, sans-serif;
        font-weight: normal;
        }
        img 
        {
        border: none;
        }
        .Viewer
        {

        }
        
        .tabla {
        font-family: Verdana, Arial, Helvetica, sans-serif;
        font-size:11px;
        text-align: left;
        font-family: "Trebuchet MS", Arial;
        text-transform: uppercase;
        background-color: #EDEDE9;
        margin:0px;
        width:1040px;
        }
        
        .tabla th {
        padding: 2px;
        background-color: #e3e2e2;
        color: #000000;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #6E6E6E;
        border-bottom-color: #6E6E6E;
        font-weight:bold;
        text-align:left;
        width:200px;
        }
        
        .tabla td {
        padding: 2px;
        border-right-width: 1px;
        border-bottom-width: 1px;
        border-right-style: solid;
        border-bottom-style: solid;
        border-right-color: #D8D8D8;
        border-bottom-color: #D8D8D8;

        background-color: #EDEDE9;
        color: #34484E;
        }
        
        .ImagenesDetalle
        {
        width:1020px;
        
        border-left: 2px solid #6E6E6E; 
        border-right: 2px solid #6E6E6E;
        border-bottom: 2px solid #6E6E6E;

        background-color:#e3e2e2;
        text-align:left;
        }
        .disenno
        {
        text-decoration: none;
        border:solid 1px #848484;

        margin-left: auto;
        margin-right: auto;
        }
        .ImagenesManagerClass
        {
        width:1020px;
        height:500px;
        border: solid 2px #6E6E6E;
        overflow:auto;
        text-align: center;
        background-color: #E6E6E6;
        padding: 10px;
        text-align:center;
        margin:0;
        }
        
        .Barra
        {
        background-color:#e3e2e2;
        width:1040px;
        height:27px;
        border-left: 2px solid #6E6E6E; 
        border-right: 2px solid #6E6E6E;
        border-top: 2px solid #6E6E6E;
        /*
        height:32px;
        padding:5px;*/

        /*margin: 0 3%;*/
        font: 10px normal Verdana, Arial, Helvetica, sans-serif;
        }

        /* Estilo de la lista */
        .Barra ul {
        padding: 0; margin: 0;
        float: left;
        width: 100%;
        list-style: none;
        border-top: 1px solid #fff; /*--Da la sensación de bisel en el panel--*/
        font-size: 1.1em;
        }

        /* alineacion de la lista (izquierda) */
        .Barra ul li{
        padding: 0; margin: 0;
        float: left;
        position: relative;
        }

        .Barra ul li a{
        padding: 5px;
        float: left;
        text-indent: -9999px; /*--Para el reemplazo del texto - Empujar el texto fuera de la página --*/
        height: 16px; width: 16px;
        text-decoration: none;
        color: #333;
        position: relative;
        cursor:pointer;
        }

        .Barra ul li select
        {
        /*padding: 5px;*/
        margin-top:4px;
        float: left;
        position: relative;
        height: 16px;/* width: 16px;*/
        font-size:10px;
        margin-left:3px;
        margin-right:3px;
        }

        html .Barra ul li a:hover{background-color: #fff; }

        html .Barra ul li a.active { /*--estado activo cuando sub-panel está abierto--*/
        background-color: #fff;
        height: 17px;
        margin-top: -2px; /*--Empuje hacia arriba 2px para fijar el botón activo sub-panel--*/
        border: 1px solid #555;
        border-top: none;
        z-index: 200; /*--Mantiene el vínculo activo en la parte superior de la sub-panel--*/
        position: relative;
        }
        /* Declarar la sustitución de texto e imagen para cada enlace. */
        .Barra a.Proceso{
        background: url(images/icons/user_business.png) no-repeat 15px center;
        width: 120px;
        padding-left: 40px;
        border-right: 1px solid #bbb;
        text-indent: 0; /*--Reset text indent since there will be a combination of both text and image--*/
        }
        a.Antes{ background: url(images/icons/2.png) no-repeat center center;
        border-left: 1px solid #bbb;
        }
        a.Siguiente{ background: url(images/icons/1.png) no-repeat center center;
        border-right: 1px solid #bbb;
        }
        a.lupaMenos{background: url(images/icons/magnifier--minus.png) no-repeat center center;}
        a.lupaMas{background: url(images/icons/magnifier--plus.png) no-repeat center center;}
        .Barra a.Normal{text-indent: 0px;width: 25px;}
        .Barra a.Linkcerrar{background: url(images/icons/cancel.png) no-repeat center center;}
        .Barra a.todosdoc{
        background: url(images/icons/address_book.png) no-repeat 15px center;
        width: 126px;
        border-left: 1px solid #bbb;
        border-right: 1px solid #bbb;
        padding-left: 40px;
        text-indent: 0; /*--Reset text indent since there will be a combination of both text and image--*/
        }
        .Barra li#allDoc {float: right;}
        .Barra li#cerrar {float: right;}  
        .FondoAplicacion
        {
        background-color: black;
        filter: alpha(opacity=70);
        opacity: 0.7;
        z-index:1001;
        }        
     </style>
     <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
     <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
     <script language="JavaScript" type="text/javascript">
         jQuery(document).ready(function($) {
             $(".lupaMas").click(function() {
                 var ancho = $(".disenno").width();
                 var alto = $(".disenno").height();
                 var sz = 92 / 66;
                 if (ancho < 1500) {
                     ancho += 100;
                     alto += (100 * sz)
                     $(".disenno").stop();
                     $(".disenno").animate({
                         width: ancho + "px",
                         height: alto + "px"
                     }, 1500);
                 }
             });

             $(".Normal").click(function() {
                 $(".disenno").stop();
                 $(".disenno").animate({
                     width: "1000px",
                     height: "1500px"
                 }, 1500);
             });

             $(".lupaMenos").click(function() {
                 var ancho = $(".disenno").width();
                 var alto = $(".disenno").height();
                 var sz = 92 / 66;
                 if (ancho > 100) {
                     ancho -= 100;
                     alto -= (100 * sz)
                     $(".disenno").stop();
                     $(".disenno").animate({
                         width: ancho + "px",
                         height: alto + "px"
                     }, 1500);
                 }
             });
         });
     </script>
     <script language="JavaScript" type="text/javascript">
		function findDOM(objectId) {
			if (document.getElementById) {
				return (document.getElementById(objectId));}
			if (document.all) {
				return (document.all[objectId]);}
			}
			function zoom(type,imgx,sz) {
				imgd = findDOM(imgx);
			if (type=="+" && imgd.width < 1241) {
				imgd.width += 50;imgd.height += (50*sz);}
			if (type=="-" && imgd.width > 20) {
				imgd.width -= 50;imgd.height -= (50*sz);}
			if (type=="++") {
				imgd.width = 1000;imgd.height =1500;}
            }
            function cerrarse()
            {
                self.close()
                window.close();
            } 
     </script>
</head>
<body>
 <div id="container">
    <form id="form1" runat="server">
     <asp:ToolkitScriptManager  ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout ="360000" >
     </asp:ToolkitScriptManager>
    
  <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
      <div id="Viewer" class="Viewer">
        <div id="Barra" class="Barra">
            <ul id="mainpanel">
                <li>
                    <asp:LinkButton ID="LinkProceso" runat="server" CssClass="Proceso">Acumulación Exp.</asp:LinkButton>
                </li>
                <li><a href="#" class="lupaMenos" onclick="zoom('-','myimg',92/66)">items2</a></li>
                <li><a href="#" class="Normal" onclick="zoom('++','myimg',92/66)">100%</a></li>
                <li><a href="#" class="lupaMas" onclick="zoom('+','myimg',92/66)">items3</a></li>
                <li><asp:LinkButton ID="LinkAntes" runat="server" CssClass="Antes">items4</asp:LinkButton></li>
                <li>
                    <asp:DropDownList ID="paginas" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </li>
                <li><asp:LinkButton ID="LinkSiguiente" runat="server" CssClass="Siguiente">items5</asp:LinkButton></li>
                <li id="cerrar">
                    <a id="LinkButton1"  class="Linkcerrar" onclick="cerrarse()">Todo el Documento</a>
                </li>
                <li id="allDoc">
                    <asp:LinkButton ID="LinkTodoDoc"  CssClass="todosdoc" runat="server">Todo el Documento</asp:LinkButton>
                </li>
            </ul>
        </div>
        <div id ="ImagenesManager" runat="server"  class="ImagenesManagerClass">
        </div>
        
        <div id ="DetalleVisor" runat="server"  class="ImagenesDetalle">
        </div>
      </div>
      
      <!-- 
        Acumulado
      -->
      
      <asp:Panel ID="pnlSeleccionarSiguientePaso" runat="server" 
                style="font-family:Verdana;z-index:9999; top: 62px; left: 0px;width:810px; display:none;" 
                CssClass="CajaDialogo" >
             <div style=" text-align:left;width:790px;margin:10px">
                <table width="100%" class="xxtabla" >
                  <tr>
                   <th colspan="2" style="text-align:center;font-size:15px;width:100%">Conjunto de documentos que conforman el  expediente</th>
                  </tr>
                  <tr>
                    <td align="left">
                    <img src="images/icons/Help-and-Support.png" style="float: left; margin-right:5px;"  alt="" />
                    <div style="color:#0B3861">ESTA VENTANA LE PERMITE UN ACCESO RAPIDO A LAS IMÁGENES DEL DOCUMENTO,  SELECCIÓNE UN ACTO ADMINISTRATIVO CUALQUIERA DE LA TABLA INFERIOR Y TECNOEXPEDIENTE AUTOMATICAMENTE VISUALIZARA LA IMAGEN ASOCIADA AL REGISTO DE SU PREFERENCIA.</div>
                    <b style="color:#045FB4">Nota:  EN LA TABLA INFERIOR APARECEN TODOS LOS ACTOS ADMINISTRATIVOS ASOCIADOS AL ESPEDIENTE PRINCIPAL <% Response.Write(Request.QueryString("vsExpedienteAcu"))%>.  </b> 
                    </td>
                  </tr>
                </table>
                
                <br />
                <table width="100%" class="xxtabla" cellspacing="0" rules="all" border="0">
                    <tr>
                     <th style="font-size:12px;width:40%">ENTE O DEUDOR PROCESADO : </th>
                     <th style="text-align:center;font-size:12px;width:60%"><div id="ActoAdmind" runat="server" class="to">ACTO</div></th>
                    </tr>
                </table>
                
                <asp:GridView ID="dtgViewActos" runat="server" AutoGenerateColumns="False" 
                     style=" font-family:Verdana;" Width="100%" CssClass="xxtabla" 
                     AllowPaging="True" PageSize="5">
                    <Columns>
                        <asp:ButtonField DataTextField="idacto" HeaderText="COD." 
                            CommandName="select">
                        <HeaderStyle Width="22px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="nombre" HeaderText="NOMBRE" >
                            <ItemStyle Width="450px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="docexpediente" HeaderText="EXPEIDENTE" >
                        </asp:BoundField>
                        <asp:BoundField DataField="fecharadic" HeaderText="F. RADICACIÓN" 
                            DataFormatString="{0:d}">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <table width="100%" class="xxtabla" cellspacing="0" rules="all" border="0">
                    <tr>
                        <th style="width:200px;font-size:14px;">Expediente principal</th>
                        <td style="text-align:center;color:#045FB4;font-size:16px;"><% Response.Write(Request.QueryString("vsExpedienteAcu"))%></td>
                        <th style="width:200px;font-size:14px">TOTAL REGISTROS :</th>
                        <td style="text-align:center;color:#045FB4;font-size:16px;"><% Response.Write(Me.ViewState("nroacu")) %></td>
                    </tr>
                </table>
           </div>
           <asp:Button ID="btnCancelarsiguiente" runat="server" Text="Cancelar" CssClass="Botones" style=" margin-bottom:10px; margin-left:10px;width: 92px;background-image: url('images/icons/cancel.png');" />
         </asp:Panel>
         
         <asp:Button ID="Button1" runat="server" Text="Button" style="visibility:hidden" />
         <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
                TargetControlID="Button1"
                PopupControlID="pnlSeleccionarSiguientePaso"
                CancelControlID="btnCancelarsiguiente"
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
         >
         </asp:ModalPopupExtender>
        
         <asp:Panel ID="Panel1" runat="server" style="font-family:Verdana;z-index:9999; top: 62px; left: 0px;width:620px;display:none;" CssClass="CajaDialogo">
           <div style=" text-align:left;width:600px;margin:10px">
               <table width="100%" class="xxtabla" >
                 <tr>
                        <th colspan="2">Mensaje del sistema</th>
                 </tr>
                 <tr>
                    <td style="width:32px;height:32px;padding:0px;margin:0px;">
                        <img src="images/icons/alerta.png"  height="32" width="32" alt="" />
                    </td>
                    <td>
                        <% Response.Write(ViewState("PpalM"))%>
                    </td>
                 </tr>
               </table>
            </div>
            <asp:Button ID="Button3" runat="server" Text="Cancelar" CssClass="Botones" style=" margin-bottom:10px; margin-left:10px;width: 92px;background-image: url('images/icons/cancel.png');" />
         </asp:Panel>
         <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden" />
         <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                TargetControlID="Button2"
                PopupControlID="Panel1"
                CancelControlID="Button3"
                
                
                DropShadow="False"
                BackgroundCssClass="FondoAplicacion"
         >
         </asp:ModalPopupExtender>
        
      <%--</ContentTemplate>
     </asp:UpdatePanel>--%>
    </form>
  </div>  
</body>
</html>


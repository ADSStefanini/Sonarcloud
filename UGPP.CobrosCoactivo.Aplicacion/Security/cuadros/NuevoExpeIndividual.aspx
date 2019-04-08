<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NuevoExpeIndividual.aspx.vb" Inherits="coactivosyp.NuevoExpeIndividual" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../css/Objetos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body 
        {
        margin:0;
        font-family:Verdana, Geneva, sans-serif;
        }
        .Botones /* Estilo de los botones */
        {
        height: 25px; 
        background-color: #f5f5f5; border-bottom: 1px solid #555555;
        border-right:1px solid #555555; border-top:0px; border-left:0px; font-size: 12px;
        color:#000; 
        padding-left: 20px; background-repeat: no-repeat; cursor:hand; cursor:pointer;
        outline-width:0px;
        background-position: 4px 4px;outline-width:0px;
        }
        .Botones:hover /* Efectos del Mouse en los botones */
        {
        height: 25px; background-color: #cccccc; border-bottom: 1px solid #000;
        border-right:1px solid #000; border-top:0px; border-left:0px;
        font-size:12px; color:#000;padding-left: 20px; background-repeat: no-repeat;
        cursor:hand; cursor:pointer;outline-width:0px;
        }
        /*Estilo uso del tooltiptex*/
        a.Ntooltip 
        {
        text-decoration: none !important; /* forzar sin subrayado */
        color:#FFFFFF !important; /* forzar color del texto */ 
        font-family:Verdana, Geneva, sans-serif; 
        font-size:11px;
        cursor:pointer;
        height:16px !important;
        width:16px !important;
        }

        a.Ntooltip span 
        {
        z-index:1001;
        display: none; /* el elemento va a estar oculto */
        position: absolute; /* se fuerza a que se ubique en un lugar de la pantalla */
        top:2.3em; left:1.7em; /* donde va a estar */
        width:200px; /* el ancho por defecto que va a tener */
        padding:6px; /* la separación entre el contenido y los bordes */
        background-color: #0080C0; /* el color de fondo por defecto */
        color: #FFFFFF; /* el color de los textos por defecto */
        border: 1px solid #0080C0;

        -moz-border-radius: 7px;
        -webkit-border-radius: 7px;
        }
        a.Ntooltip:hover span 
        {
        position: absolute; /* se fuerza a que se ubique en un lugar de la pantalla */
        top:2.3em; left:1.7em; /* donde va a estar */
        width:200px; /* el ancho por defecto que va a tener */
        padding:6px; /* la separación entre el contenido y los bordes */
        background-color: #0080C0; /* el color de fondo por defecto */
        color: #FFFFFF; /* el color de los textos por defecto */
        border: 1px solid #0080C0;

        -moz-border-radius: 7px;
        -webkit-border-radius: 7px;
        }
	    .style1
        {
            width: 123px;
        }
	</style>
</head>
<body>

    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
     <asp:Panel ID="block" runat="server" style="background-color:#fff;">
           <div id ="Div1" style="color:#ffffff;font-size:11px; margin:7px;">
              <table class="tabla" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                 <tr>
                     <th>
                         Abrir expediente individual</th>
                 </tr>
                 <tr>
                     <td align="left">
                        <img src="../images/icons/property.png"  width="55" height="55" style="float: left; margin-right:5px;"  alt="" />
                        <div style="color:#0B3861">Con esta opción puede buscar las sumas mayores a un valor especifico y escatimar una búsqueda personalizada por medio de la lista que aparece a la derecha del cuadro de búsqueda.
                        </div>
                        <b style="color:#045FB4">Nota:  Predios sin procesos generados.</b> 
                     </td>
                 </tr>
               </table>
               <div style="background-color: #e3e2e2;color: #000000;padding:7px;border-right-width: 1px;border-bottom-width: 1px;border-right-style: solid;border-bottom-style: solid;border-right-color: #6E6E6E;border-bottom-color: #6E6E6E;font-weight:bold;text-align:left; ">
                   <a style="z-index:225;width:16px; height:16px;border:0px; text-decoration:none;">
                      <img alt="" src="../images/icons/help.png" width="16" height="16" style="cursor:pointer;border:0px;" />
                   </a>
                   <span style="font-size:16px;text-transform: uppercase" id="Resultado_consul" title="Deudores que nunca se les ha ejecutado un proceso (expediente)." runat="server">Modulo de cobro individual.</span>
               </div>
               <div style="background-color:#f0f0f0; padding:5px;color:#000;">
                      <table width="100%">
                         <tr>
                            <td colspan="2">
                                <asp:GridView ID="GridPredios" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="tabla">
                                    <Columns>
                                        <asp:BoundField HeaderText="Nit" DataField="EDC_NITCC" />
                                        <asp:BoundField HeaderText="Nombre" DataField="EDC_NOMBRE" />
                                        <asp:ButtonField HeaderText="Nro Pre." DataTextField="NROPREDIO" 
                                            CommandName="select" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                         </tr>
                         <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:LinkButton ID="LinkConsultaNroPre" runat="server" Visible="true">DEUDORES CON DEUDA VIGENTE</asp:LinkButton>
                            </td>
                         </tr>
                         <tr>
                            <td class="style1"><strong><span ID="LabelTexto" runat="server">Predio</span></strong></td>
                            <td>
                                <asp:TextBox ID="Textbus" runat="server" Width="294px"></asp:TextBox>
                            </td>
                         </tr>
                         <tr>   
                            <td class="style1"><strong><span>Vigencia Inicial</span></strong></td>
                            <td>
                                <asp:TextBox ID="txtVigenciaInicial" runat="server" Width="94px"></asp:TextBox>
                            </td>
                          </tr>
                          <tr> 
                            <td class="style1"><strong><span>Vigencia Final</span></strong></td>
                            <td>
                                <asp:TextBox ID="txtVigenciaFinal" runat="server" Width="94px"></asp:TextBox>
                            </td>
                          </tr>
                          <tr>
                             <td colspan="2">
                                
                                <asp:Button ID="bntGenerar" runat="server" CssClass="Botones" 
                                    Text="Ejecutar Paso (Acto)" 
                                    style="background-image: url('../images/icons/padlock_closed.png');" 
                                    Width="147px" />
                                
                                <input type ="button" id="AcepPanel" value="Cerrar ventana"  
                                   style="z-index:5;background-image: url('../images/icons/sign_cacel.png');width:118px;"  
                                   class="Botones"  onclick="mpeSeleccionOnCancel();" />
                                    <script type="text/javascript">
                                        function mpeSeleccionOnCancel() {
                                            self.close();
                                            window.close();
                                            return false;
                                        }
                                    </script>
                             </td>
                           </tr>
                          <tr>
                            <td colspan="2">
                            <hr />
                            </td>
                          </tr>   
                          <tr>
                             <td colspan="2">
                                 <span id ="message" runat="server" style="color:#4c4c4c;padding:2px; font-size:x-small;">
                                        <img src="../images/icons/159.png" alt="Actos Administrativos" style="vertical-align: middle;border:0px;" /> <b>Secuenciador</b> 
                                 </span>
                             </td>
                          </tr>
                    </table>
                    
                    <table class="tabla">
                         <tr>
                            <th>
                                Documento:
                            </th>
                            <td style="text-align:center;color:Blue;">
                                <asp:Label ID="lblcodigo" runat="server" Text="00000000"></asp:Label>
                            </td>
                         </tr>
                         <tr>
                          <th>
                                <asp:Label ID="Validator" runat="server" Text="Label" ForeColor="Yellow" 
                                 Font-Names="Arial" Font-Size="15px"	ErrorMessage="CustomValidator" Font-Bold="True" 
                                 style="color: #CC9900; text-transform: uppercase; font-size: xx-small"></asp:Label>
                          </th>
                          <td style="text-align:center;">
                             <asp:LinkButton ID="LinkArchivo_expediente" runat="server" Visible="true">Descargar Archivo</asp:LinkButton>
                          </td>
                         </tr>
                   </table>
            </div>
           </div>
         </asp:Panel>
    </form>
  </body>
 </html>
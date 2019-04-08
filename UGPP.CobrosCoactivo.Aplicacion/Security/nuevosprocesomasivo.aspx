<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="nuevosprocesomasivo.aspx.vb" Inherits="coactivosyp.nuevosprocesomasivo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Consultas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="Libertad.css" rel="stylesheet" type="text/css" />
    <link href="css/Objetos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         .ws2
         {
     	    background-image: url('Menu_PPal/Li.png');
            background-repeat: repeat-x;

     	    background-color:#0b4296;
     	    border:1px solid #dedede;
            border-top:1px solid #eee;
            border-left:1px solid #eee;
            
            font-family:"Lucida Grande", Tahoma, Arial, Verdana, sans-serif;
            font-size:17px;
            text-decoration:none;
            font-weight:bold;
            color:#ffffff;
            padding:2px;
     	    float:left;
     	    text-align:left;
     	    cursor:pointer;
         }
         .tablemenu
         {
            margin: 0px;
         }
         .tablamenu tr td 
         {
            margin: 0px; padding: 0px;
            float: left;
            position: relative; /* Aquí ponemos posicionamiento absoluta */
            width: 105px;
            height: 105px;
         }
         .tablamenu tr td img
         {
            width: 75px; height: 75px; /* Aquí va el tamaño del thumbnail pequeño */
            border: 1px solid #ddd;
            padding: 5px;
            /*background: #f0f0f0;*/
            position: absolute;
            left: 37px; top: 0px;
            text-align:center;
          }
          .tablamenu tr td:hover 
           {
             /*background-color:#E0F8F7;*/
           }
          .tablamenu tr td
          {
            font-size:11px;
            color: #444444;    
            width:150px;
            text-align:center;        	
          }
          .tecno
          {
            position:absolute;
            top:969px;
            left: 200px;
            font-family:"Lucida Grande", Tahoma, Arial, Verdana, sans-serif;
            font-size:17px;
            font-weight:bold;
          }
          .tecno a
          {
          	color:#ffffff;
          }
          .tecno a:hover 
          {
          	color:red;
          } 
          .xws1
          {
            font-size:11px;
            font-family:Verdana;
            color:#ffffff;
          }
          .xcd
          {
            display:block;
            font-family:Tahoma;
            font-size:11px;
            padding:5px;
            text-decoration:none;
            cursor:pointer;
            text-align:center;
            color:#FFFFFF;
            background-color:#4977D3;
            border-left:10px solid #4371BF;
            border-bottom:1px solid #4371BF;
            border-top:1px solid #4371BF;
            border-right:1px solid #4371BF;
          }
          .xcd:hover
          {
            display:block;
           font-family:Tahoma;
           font-size:11px;
           padding:5px;
           text-decoration:none;
           text-align:center;
           color:#99CC00;
           background-color:#003366;
           border-left:10px solid #99CC00;
           border-bottom:1px solid #99CC00;
           border-top:1px solid #99CC00;
           border-right:1px solid #99CC00;
         }
         a.Ntooltip:hover
         {
           background-color: Transparent;
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
           font-weight: bold;
           font-size:12px;
           font-style: italic;
         }
     </style>
     <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
     <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
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
    <div id="container">
        <h1 id="Titulo"><a href="#">Consultas - <%  Response.Write(Session("ssCodimpadm") & ".")%></a></h1>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
        
        
            <div style="top:416px; left: 13px; position: absolute;width: 750px; background-color:#2461BF;">
                    <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False"  width = "100%"
                                CellPadding="4" ForeColor="#333333" GridLines="None" style="font-size: 11px" HorizontalAlign="Left" AllowPaging="True" PageSize="15">                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Security/images/icons/1.png" 
                                        ShowSelectButton="True" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="16px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="LIQGEN" HeaderText="Predio">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PRECARVAL" HeaderText="E">
                                    <ItemStyle Width="5px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PREPRSDOC" HeaderText="ID">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PREPRSNOM" HeaderText="Deudor">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VIGENCIAS" HeaderText="Vigencias">
                                    <ItemStyle Width="78px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TOTALPAGAR" HeaderText="Total" DataFormatString="{0:N}">
                                    <HeaderStyle HorizontalAlign="Right" Width="85px" />
                                    <ItemStyle HorizontalAlign="Right" ForeColor="#CC0000" />
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
            
         <div id="EJEFISGLOBALcales" runat="server" 
                style="position:absolute;top:50px; height:138px; width: 749px; left:14px;z-index:100;" 
                class="divhisto">
                <img src="images/icons/property.png" alt="" width="75" height="75" 
                    style="position: absolute; top: 11px; left: 12px;"  />
                
                <table class="xws1" 
                    style="position: absolute; top: 30px; left: 104px; border-collapse:collapse; width:610px">
                 <tr>
                   
                    <td align="center"><asp:Label ID="ejDeudor" runat="server" 
                            Text="Deudores con vigencias pendientes" 
                            style="text-transform: uppercase; font-size: 22px; font-weight: 700"></asp:Label></td>
                 </tr>
                 <tr>
                     <td align="center"><b> <%  Response.Write(Session("ssCodimpadm")) %> </b></td>
                 </tr>
                 <tr>
                     <td align="center"><asp:Label ID="ejDetalle" runat="server" Text="Label"></asp:Label></td>
                 </tr>
                 <tr>
                     <td align="center">&nbsp;</td>
                 </tr>
                 <tr>
                  <td style=" text-align:right;" align="right">
                    <asp:customvalidator id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="11px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
                  </td>
                 </tr>
                </table>
                
                <table  style="position: absolute; top: 6px; left: 583px; border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <a class ="xcd" href ="cobranzasMasiva.aspx">Regresar a cobranza masiva</a>
                  </td>
                 
                 </tr>
                </table>
               
			   <asp:Button ID="btnImprimir" runat="server" CssClass="Botones" 
                            style="background-image: url('images/icons/padlock_closed.png'); position: absolute; top: 107px; left: 14px; width: 133px;" 
                            Text="Generar  Proceso" 
        ValidationGroup="textovalidados" />
        
                              
               <asp:Button ID="btnSeperar" runat="server" CssClass="Botones" 
                            style="background-image: url('images/icons/186.png'); position: absolute; top: 107px; left:152px;" 
                            Text="Separar Archivos" 
        ValidationGroup="textovalidados" Width="150px" />
               
                     
               
           </div>
           
            <asp:Panel ID="pnlSeleccionarSiguientePaso" runat="server" 
                style="font-family:Verdana; display: none; z-index:9999; top: 62px; left: 0px;" 
                CssClass="CajaDialogo" >
             <div style=" text-align:left;width:790px;margin:10px">
                <table width="100%" class="xxtabla" >
                  <tr>
                   <th colspan="2" style="text-align:center;font-size:15px;width:100%">Asigne el acto 
                       administrativo</th>
                  </tr>
                  <tr>
                    <td align="left">
                    <img src="images/icons/Help-and-Support.png" style="float: left; margin-right:5px;"  alt="" />
                    <div style="color:#0B3861">Para continuar con la operación seleccione el acto administrativo, una vez hecho esto, se relacionará cada predio con un numero de proceso (Expediente), para hacer seguimiento y cobros según las vigencias halladas.</div>
                    <b style="color:#045FB4">Nota:  Esté proceso es irreversible.  </b> 
                    </td>
                  </tr>
                </table>
                
                <br />
                <table width="100%" class="xxtabla" cellspacing="0" rules="all" border="1">
                    <tr>
                     <th style="font-size:12px;width:60%">Acto administrativo con el que se inicializara el proceso  :</th>
                     <th style="text-align:center;font-size:12px;width:40%"><div id="ActoAdmind" runat="server" class="to">ACTO</div></th>
                    </tr>
                </table>
                
                <asp:GridView ID="dtgViewActos" runat="server" AutoGenerateColumns="False" style=" font-family:Verdana;" Width="100%" CssClass="xxtabla">
                    <Columns>
                        <asp:ButtonField DataTextField="DEP_CODACTO" HeaderText="COD." 
                            CommandName="select">
                        <HeaderStyle Width="22px" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="DEP_NOMBREPPAL" HeaderText="NOMBRE" />
                        <asp:BoundField DataField="DEP_TERMINO" HeaderText="TERMINO" >
                        <HeaderStyle Width="20px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
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
         
         <div style="position:absolute;top:196px; left:13px; width:750px; height:99px; background-color:#D1DDF1;">
                <div style=" font-size:11px;color:#fff;padding:5px;background-image: url('images/BarraActos.png'); background-repeat: repeat-x; height:18px;">
                  <div style=" position:absolute;top:80px; left:12px; font-size:11px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
                     <asp:LinkButton ID="Linkvalor" runat="server">Buscar Procesos</asp:LinkButton>
                  </div>
                  <a class="Ntooltip" href="#"  style="width: 16px; height: 16px; float:left; margin-right:5px;">
                      <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                      <span style="z-index:225;">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Buscar Sumas 
                        </b>
                        <br /><br />
                            Con esta opción puede buscar las sumas mayores a un valor especifico  y escatimar una búsqueda personalizada. 
                        
                        <br />
                        <br />
                        
                        <b>Ejemplo :</b> En el cuadro digite $5,000 y el resultado serán deudores con sumas mayores o iguale tales como 
                        <ul>
                            <li>$5,000</li>
                            <li>$5,001</li>
                            <li>$5,002</li>
                            <li>$6,000</li>
                            <li>$7,000</li>
                        </ul> 
                        <hr />
                        <b>Observación :</b> Las consultas no se mesclan.
                      </span>
                  </a>
                  Consultar sumas mayores a
                </div>
                
                <asp:TextBox ID="txtEstrato" runat="server" 
                style="top: 54px; left: 194px; position: absolute;  width: 118px" 
                    MaxLength="1"></asp:TextBox>
                
                <asp:TextBox ID="txtVigencias" runat="server" 
                style="top: 54px; left: 377px; position: absolute;  width: 118px" 
                    MaxLength="4"></asp:TextBox>
                
                <asp:TextBox ID="txtValor" runat="server" 
                style="top: 54px; left: 12px; position: absolute;  width: 118px"></asp:TextBox>
                
                <div style="position:absolute;top:100px; left:0px; width:750px; background-color:#EDEDE9;">
                    <div style=" font-size:11px;color:#fff;padding:5px; height:15px;background-color:#507CD1;">
                      Detalles de la consulta  (<asp:Label ID="lblacto" runat="server" Text="#"></asp:Label>)
                    </div>
                    <table style="width:100%" class="tabla">
                     <tr><th>NUMERO TOTAL DE EXPEDIENTES :</th><td colspan="5" align="center"><div id = "totexp" runat = "server">Aun no se ha generado expediente </div></td></tr>
                     <tr><th>DESCARGAR ARCHIVO :</th><td colspan="5" align="center"><asp:LinkButton ID="LinkArchivo_expediente" runat="server">No hay documento generado</asp:LinkButton></td></tr>
                     <tr><th>DOCUMENTO :</th><td align = "center" style="text-align:center;"><b style="color:Blue;"><asp:Label ID="lblcodigo" runat="server" Text="#"></asp:Label></b></td>
                         <td align = "center" style="text-align:center;" colspan="4"><asp:Label ID="lblfechahoy" runat="server" Text="#"></asp:Label></td>
                      </tr>
                      <tr>
                         <th>TOTAL :</th>
                         <td align = "center" style="text-align:center;"><b style="color:Blue;"><asp:Label ID="lbltotal" runat="server" Text="#"></asp:Label></b></td>
                         
                         <th style="width:120px;">TOTAL REGISTROS :</th>
                         <td align = "center" style="text-align:center; "><b style="color:Blue;"><asp:Label ID="lblnroregistros" runat="server" Text="#"></asp:Label></b></td>
                         
                         <th style="width:70px;">VIGENCIAS :</th>
                         <td align = "center" style="text-align:center; "><b style="color:Blue;"><asp:Label ID="lblvigencias" runat="server" Text="#"></asp:Label></b></td>
                      </tr>
                    </table>
               </div>
               
                
                <div style=" position:absolute;top:34px; left:377px; font-size:12px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase; height: 15px; width: 160px;">Vigencias Mayores a :</div>
                
                <div style=" position:absolute;top:34px; left:194px; font-size:12px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase; width: 131px;">Estrato Mayor a :</div>
                
                <div style=" position:absolute;top:34px; left:12px; font-size:12px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase; width: 113px;">Digitar Valor :</div>
           </div>
         
           
        </form>
    </div>
</body>
</html>
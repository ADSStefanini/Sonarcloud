<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="deudoresactivos.aspx.vb" Inherits="coactivosyp.deudoresactivos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Consultas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
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
        <h1 id="Titulo"><a href="#">Consultas</a></h1>
        <div 
        style="position:absolute;color:#fff;background-color:#507CD1;
        background-image: url('images/BarraActos.png');background-repeat: repeat-x; 
        font-size: 11px; font-weight: 700; top: 40px; left: 36px; padding:7px; width: 688px;" id ="Div1" runat="server">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div>
        <form id="form1" runat="server">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
        
            <div style="position:absolute;top:180px;left:356px; width:203px; height:105px; background-color:#D1DDF1;">
                <div style=" font-size:11px;color:#fff;padding:5px;background-image: url('images/BarraActos.png'); background-repeat: repeat-x; height:18px;width:193px;">
                  <div style=" position:absolute;top:84px; left:12px; font-size:11px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
                     <asp:LinkButton ID="Linkvalor" runat="server">Buscar Sumas</asp:LinkButton>
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
                
                <asp:TextBox ID="txtValor" runat="server" 
                style="top: 57px; left: 11px; position: absolute;  width: 149px"></asp:TextBox>
                
                
                <div style=" position:absolute;top:37px; left:12px; font-size:12px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">Digitar Valor :</div>
           </div>
        
        
            <div style="top: 302px; left: 17px; position: absolute;width: 743px; background-color:#2461BF;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 <ContentTemplate>
                    <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False"  width = "743px"
                                CellPadding="4" ForeColor="#333333" GridLines="None" style="font-size: 11px" HorizontalAlign="Left" AllowPaging="True" PageSize="15">                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Security/images/icons/1.png" 
                                        ShowSelectButton="True" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="16px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="MAN_REFCATRASTAL" HeaderText="Predio" 
                                        SortExpression="MAN_REFCATRASTAL" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MAN_DEUSDOR" HeaderText="ID" 
                                        SortExpression="MAN_DEUSDOR" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MAN_NOMDEUDOR" HeaderText="Deudor" 
                                        SortExpression="MAN_NOMDEUDOR" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MAN_VALORMANDA" HeaderText="Total" 
                                        DataFormatString="{0:N}">
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                    <ItemStyle HorizontalAlign="Right" />
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
                 </ContentTemplate>
                </asp:UpdatePanel>
                 
              
         </div>
            
            <div id="ejeFiscales" runat="server" 
                style="position:absolute;top:65px; height:105px; width: 749px; left:14px; z-index:1001;" 
                class="divhisto">
                <img src="images/icons/property.png" alt="" width="75" height="75" 
                    style="position: absolute; top: 14px; left: 12px;"  />
                
                <table class="xws1" 
                    style="position: absolute; top: 39px; left: 130px; border-collapse:collapse; width:550px">
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
                     <td align="center"><asp:Label ID="lblvalor" runat="server" 
                            Text="" 
                            style="text-transform: uppercase; font-size: 22px; font-weight: 700"></asp:Label></td>
                 </tr>
                </table>
                
                <table  style="position: absolute; top: 7px; left: 245px; border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <a class ="xcd" href ="menuej.aspx">Regresar al menu procedimiento tributario</a>
                  </td>
                 </tr>
                </table>
                
                <table  style="position: absolute; top: 7px; left: 614px; border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <a class ="xcd" href ="ejecucionesFiscales.aspx">Ejecuciones Fiscales</a>
                  </td>
                 </tr>
               </table>
               <table  style="position: absolute; top: 7px; left: 475px; border-collapse:collapse;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <asp:LinkButton ID="LinkRetomar" runat="server"  CssClass ="xcd">Retomar la información</asp:LinkButton>
                  </td>
                 </tr>
               </table>
               
               <table id="tbLinkretormas"  runat="server" style="position: absolute; top: 181px; left: 16px; border-collapse:collapse; visibility: hidden;" 
                    cellspacing="0">
                 <tr>
                  <td>
                    <asp:LinkButton CssClass = "xcd" ID="Linkretormas" runat="server" style=" font-size:14px;font-weight: bold">Regresar a la impresión de informes masivos </asp:LinkButton>
                  </td>
                 </tr>
               </table>
           </div>
           
           <div style="position:absolute;top:180px;left:14px;width:334px; height:105px; background-color:#D1DDF1;">
                <div style=" font-size:11px;color:#fff;padding:5px;background-image: url('images/BarraActos.png'); background-repeat: repeat-x; height:18px;width:324px;">
                     <a class="Ntooltip" href="#"  style="width: 16px; height: 16px; float:left; margin-right:5px;">
                      <img alt="" src="images/icons/help.png" width="16" height="16" style="cursor:hand; cursor:pointer;" />
                      <span style="z-index:225;">
                        <b style="background:url('images/icons/105.png') no-repeat left 50%;background-color: transparent;padding-left:12px;">
                          Nota : Op. Consulta en Pantalla  
                        </b>
                        <br /><br />
                        Con esta opción pude buscar un deudor específico de la lista de deudores morosos
                        
                        <br />
                        <br />
                        
                        <b>Ejemplo :</b> En el cuadro digite un nombre, una cedula o un predio  y la lista se reducirá.
                        <br /> <br />
                        <hr />
                        <b>Observación :</b> Las consultas no se mesclan.
                      </span>
                  </a>
                    
                    Consulta en Pantalla 
                </div>
                <asp:RadioButtonList ID="Radiobuscar" runat="server" 
                    RepeatDirection="Horizontal" 
                    
                   style="top: 31px; left: 44px; position: absolute; height: 10px; width: 250px; color: #000; font-size: 12px">
                    <asp:ListItem Selected="True">Cedula</asp:ListItem>
                    <asp:ListItem>Nombre</asp:ListItem>
                    <asp:ListItem>Predio</asp:ListItem>
                </asp:RadioButtonList>
                
                <asp:TextBox ID="txtConsultar" runat="server" 
                style="top: 57px; left: 11px; position: absolute;  width: 307px"></asp:TextBox>
                
                <div style=" position:absolute;top:84px; left:11px; font-size:11px;color:#000; font-family:Arial;text-align:left;font-weight: bold; margin-bottom: 0px;text-transform: uppercase;">
                    <asp:LinkButton ID="LinkButton1" runat="server">Click aqui para consultar</asp:LinkButton></div>
           </div>
        
           
           
           <asp:Button ID="btnIndividual" runat="server" Text="Examinar Individual" 
             CssClass="Botones" style = "background-image: url('images/icons/24.png'); top: 229px; left: 570px; position: absolute;" 
             Width="187px" Visible="False" />           
           
           <asp:Button ID="btnTodos" runat="server" Text="Masivo  Contribuyente" 
             CssClass="Botones" style = "background-image: url('images/icons/statics.png'); top: 260px; left: 570px; position: absolute;" 
             Width="187px" Visible="False" />           
           
        </form>
    </div>
</body>
</html>

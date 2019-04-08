<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cobranzas2.aspx.vb" Inherits="coactivosyp.cobranzas2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cobranzas</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link href="css/autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <link href="../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    
    <style type="text/css">
        .Yup {text-align:center;color: #FFFFFF;}
        .xlist {text-align:left;font-family: Verdana;font-size: 11px;font-weight: bold;font-style: normal;color: #FFFFFF;}
        .divhisto {background-color:#507CD1;border-right: solid 1px #304a7d;border-bottom: solid 1px #304a7d;}
        .CajaDialogo {position:absolute;background-color:#f0f0f0;border-width: 7px;border-style: solid;border-color: #72A3F8;padding: 0px;color:#514E4E;font-weight: bold;font-size:12px;font-style: italic;}
        .palanca {color: #34484E;}
        .xparametrosInfo {font-family: Verdana;font-size: 12px;font-style: normal}
        .xparametrosInfo table {border-spacing:0px 5px; width:100%;}
        .xparametrosInfo table th {text-transform: uppercase;width:250px;text-align:left;font-weight: bold;}
        .xparametrosInfo table td {text-align:right;}
        .xdt {width:100%;}
        .xdt th {background-color: #efefef;font-weight:bold; padding:5px; font-size:xx-small;width:165px; text-align:left !important;}
        .xdt td {background-color: #f9f9f9;font-style:italic; padding:5px;font-size:xx-small;color:#093b86;}
        .xdt td:hover {background-color:#fefefe;cursor:pointer;color:#36c0e4;}
        .menu{width:100%;background-color: #fefefe; }
        .menu ul {margin: 0; padding: 0;float: left;}
        .menu ul li{display: inline;}
        .menu ul li a{width:95%;float:left; text-decoration:none;color:#000;padding:7px;background-color: #fefefe; }
        .menu ul li a:visited{color:#000;}
        .menu ul li a:hover, .menu ul li .current{color: #fff;background-color:#0b75b2;}  
        body {           
            background-color:White;
            background-image: none;
        }
        div#container {            
            background-color:White;
            margin-left:0px;
            margin-right:0px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function() {
            $('a.Ntooltip').hover(function() {
                $(this).find('span').stop(true, true).fadeIn("slow");
            }, function() {
                $(this).find('span').stop(true, true).hide("slow");
            });

            //evitar edicion en el campo del ente
            $("#txtEnte").keypress(function(event) { event.preventDefault(); });
            $("#txtEnte").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

            // Dialog
            $('.xparametrosInfo').dialog({
                autoOpen: false,
                width: 500,
                modal: true,
                buttons: {
                    "Aceptar": function() {
                        $(this).dialog("close");
                    }
                },
                show: 'blind',
                hide: 'blind'
            });
            $('#dialog-mas-informes').dialog({
                resizable: false,
                autoOpen: false,
                width: 500,
                modal: true,
                buttons: {
                    "Aceptar": function() {
                        $(this).dialog("close");
                    }
                },
                show: 'blind',
                hide: 'blind'
            });
            /////
            $('#btnmasinformes').click(function() {
                $('#dialog-mas-informes').dialog('open');
                return false;
            });
            $('#dialog_link').click(function() {
                $('.xparametrosInfo').dialog('open');
                return false;
            });
            $('#dialog-variables-report').dialog({
                autoOpen: false,
                width: 350,
                modal: true,
                buttons: {
                    "Aceptar": function() {
                        $(this).dialog("close");
                    }
                },
                show: 'blind',
                hide: 'blind'
            });

        });
    </script>
    <script type="text/javascript">
        // Dialog
        $('.xparametrosInfo_Permisos').dialog({
            autoOpen: false,
            width: 400,
            modal: true,
            buttons: {
                "Aceptar": function() {
                    $(this).dialog("close");
                }
            },
            hide: 'fold'
        });

        $('.dialog_link').click(function(evento) {
            evento.preventDefault();
            $('.xparametrosInfo_Permisos').dialog('open');
            return false;
        });

        function ValidarNivel(pNivelAcceso) {
            if (pNivelAcceso == 1) {
                window.open('cuadros/NuevoExpeIndividual.aspx', 'mywindow', 'location=0,status=0,scrollbars=1, width=479,height=350');
            } else {
                alert("Solo los usuarios administradores pueden crear expedientes");
            }
        }       
    </script>
     <script type="text/javascript" language="javascript">
        function mostrar_procesar() {
            document.getElementById('procesando_div').style.display = "";
            $("#dialog-modal").dialog({
                height: 150,
                modal: true
            });
            setTimeout(' document.getElementById("procesando_gif").src="images/gif/ajax-loader.gif"', 1000000);
            
        }
        
        function ocultar_procesar() {
            document.getElementById('procesando_div').style.display="none";
        }   
</script>

</head>
<body>
  <!-- Definicion del menu -->  
  <div id="overflow"></div>
  <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
  </div>
  <div id="container" style ="width :980px;">
    <form id="form1" runat="server">    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>

     
     <div class="Yup divhisto" style = "width: 950px; position:absolute;top:106px;left:24px;background-color:#6189d5;z-index:1;">
         <div style=" background-color:#304a7d;text-align:left; overflow:hidden; height:34px; background-color:#86b5d9">
             <div style="float:left;text-transform:uppercase;font-weight:bold;height:20px;font-size:15px;padding:9px 12px 5px 17px;font-family:Arial; background-color:#86b5d9">
                Impresión de Actos administrativos
             </div>
             <div style="float:right;padding:5px 5px 5px 17px;">
             <asp:Button ID="btnProyectar" runat="server" Text="Proyectar" CssClass="Botones"  ValidationGroup="textovalidados" style ="background-image: url('images/icons/printer.png');" Width="95px" />
                 <asp:Button ID="btnImprimir" runat="server" Text="Generar" CssClass="Botones"  ValidationGroup="textovalidados" style ="background-image: url('images/icons/printer.png');" Width="95px"/>
                 
             </div>
         </div>
         
         <div style="padding:10px;background-color:#ffffff;">
           <div style="height:300px; overflow:auto;">
             <asp:RadioButtonList ID="Lista" runat="server" CssClass="xlist" style="color:#000000; font-weight: normal">
             </asp:RadioButtonList>
             <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [DXI_ACTO], ('(' + [DXI_ACTO] + ') ' + [DXI_NOMBREACTO]) AS DXI_NOMBREACTO FROM [DOCUMENTO_INFORMEXIMPUESTO] WHERE (([DXI_HISTORIAL] = @DXI_HISTORIAL) AND ([DXI_IMPUESTOVALUE] = @DXI_IMPUESTOVALUE)) ORDER BY 1">
                 <SelectParameters>
                     <asp:Parameter Name="DXI_HISTORIAL" Type="Boolean" DefaultValue="True" />
                     <asp:SessionParameter Name="DXI_IMPUESTOVALUE" 
                         SessionField="ssimpuesto" Type="Int32" />
                    
                 </SelectParameters>
             </asp:SqlDataSource>
             <br />
             <asp:customvalidator id="Validator"  runat="server" ForeColor="Yellow" Font-Names="Arial" Font-Size="15px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:customvalidator>
		   </div>
         </div>
       </div>
       <div class="xparametrosInfo" style="display:none;" title="COBRANZAS  - <%= Session("ssCodimpadm")%>">
         <div style="font-size:xx-small;">Modulo de cobranzas.</div>
         <% 
             Dim Mytable As system.Data.DataTable
             Mytable = ViewState("DatosEnte")
             If Mytable Is Nothing Then
                 Mytable = New System.Data.DataTable
                 Mytable.Columns.Add("codigo", GetType(String))
                 Mytable.Columns.Add("nombre", GetType(String))
                 Mytable.Columns.Add("ent_direccionlocalidad", GetType(String))
                 Mytable.Rows.Add("...", "...", "...")
             End If
         %>
         <img style="display:block;margin-left:auto;margin-right:auto;" src="imagen.ashx?ImageFileName=<%= Mytable.Rows(0).Item("codigo") %>&tipo=Logo" alt="" />
         <table>
            <tr>
                <th>Codigo</th>
                <td><%= Mytable.Rows(0).Item("codigo") %></td>
            </tr>
            <tr>
                <th>Entidad Cobradora</th>
                <td><%= Mytable.Rows(0).Item("nombre") %></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;"><%= Mytable.Rows(0).Item("ent_direccionlocalidad") %></td>
            </tr>
         </table>
       </div>
       <div id ="mensup" runat="server" style="position:absolute;top:169px; left:538px; color:#fff;font-size:xx-small;font-family:arial;width:224px;">...</div>
       <div style="width:950px;height:100px;position:absolute;left:24px;border:solid 1px #d5e0f2;background:#d1ddf1 url('images/icons/bgclaro.jpg') repeat-x;z-index:1;">
            <asp:TextBox ID="txtEnte" runat="server" style="top: 37px; left: 17px; position: absolute;width: 697px; z-index:2;"></asp:TextBox>
            <div  style="width: 214px;position:absolute;top:16px; left:17px; z-index:2;" class="ws1">
                Propietario (Deudor) : 
            </div>                        
           <asp:Label ID="fechaRac" runat="server" Text="" style="width:93px;position:absolute;top:74px; right:20px;z-index:2;text-align:right; font-size:xx-small;" class="ws1"></asp:Label>
           
           <div style="position:absolute;top:64px;left:18px;width:160px;" >
                <table width="100%">
                    <tr>
                     <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><img src="images/icons/script.png" alt="" /></th>
                     <th style="font-size:11px;text-align:left;padding:4px;">EXPEDIENTE :</th>
                     <th style="text-align: right;font-size:11px;padding:4px;" class="palanca"><asp:Label ID="lblExpediente" runat="server" Text="000000"></asp:Label></th>                 
                    </tr>
                </table>
           </div>
           
           <div style="position:absolute;top:64px;left:558px;width:160px;" >
                <table width="100%">
                    <tr>
                     <th style="text-align: right;font-size:11px;padding:4px;width:16px;"><img src="images/icons/user_business.png" alt="" /></th>
                     <th style="font-size:11px;text-align:left;padding:4px; text-transform: uppercase;"><a id="dialog_link" href="javascript:void(0)" style="color:#000; text-decoration:none;" >Ente :</a></th>
                     <th style="text-align: right;font-size:11px;padding:4px; text-transform: uppercase;" class="palanca"><asp:Label ID="lblCobrador" runat="server" Text=""></asp:Label></th>
                    </tr>
                </table>
           </div>
       </div>
       
       
       
       <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden" />
       
       <asp:RequiredFieldValidator ID="rfvCedulanit" runat="server"  ErrorMessage="El campo <strong>DEUDOR</strong> es requerido para la impresión individual. Verifique" ValidationGroup="textovalidados" ControlToValidate="txtEnte" Display="None"></asp:RequiredFieldValidator>
       <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvCedulanit">
       </asp:ValidatorCalloutExtender>
        <!-- Error y cerrar sesion -->
        <asp:Panel ID="pnlError" runat="server" CssClass="CajaDialogoErr" style="width: 341px;Z-INDEX: 116; position:absolute;display: none; padding:5px;">
              <div id="logo">
                  <h1><a href="javascript:void(0)" title="Tecno Expedientes !">Tecno Expedientes !</a></h1>
                  <p id="slogan">Gestión Documental.</p>
              </div>
              <div style="margin: 0  0 5px 0; ">
                 <% 
                     If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                         Response.Write(Me.ViewState("Erroruseractivo"))
                     End If
                 %>
              </div>
    		  <hr />	
			  <asp:Button style="Z-INDEX: 116;width:75px;" id="btnNoerror" runat="server" Text="Aceptar" Height="23px" CssClass="RedButton"></asp:Button>    
        </asp:Panel>
       
        <asp:Button ID="Button3" runat="server" Text="Button" style="visibility:hidden" />
		<asp:ModalPopupExtender ID="ModalPopupError" runat="server" 
            TargetControlID="Button3"
            PopupControlID="pnlError"
            CancelControlID="btnNoerror"
            OnCancelScript="mpeSeleccionOnCancel()"
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion">
        </asp:ModalPopupExtender>
        
         <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="height: 221px; width: 358px; top: 174px; left: 212px; z-index:1001;display:none; "> 
            <asp:Button style="Z-INDEX:116;width:105px;top:187px;left:108px;position:absolute;right:171px;background-image: url('images/icons/cancel.png');" id="btnNo" runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:Button style="Z-INDEX:116;width:84px;top:187px;left:16px;position:absolute;right:264px;background-image: url('images/icons/okay.png');" id="btnSi" runat="server" Text="Enviar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:Button style="Z-INDEX:116;width:90px;top:187px;left:250px;position:absolute;right:290px;background-image: url('images/icons/okay.png');" id="btnadicionar" runat="server" Text="Adicionar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:ListBox ID="list_embargos" runat="server" style="top: 30px; left: 109px; position: absolute; height: 131px; width: 223px"></asp:ListBox>
            <asp:Label ID="Label2" runat="server" Text="Resoluciones" style="top: 12px; left: 108px; position: absolute; height: 15px; width: 220px"></asp:Label>
            <asp:Label ID="Label3" runat="server" style="top:166px;left:17px;position:absolute;" Text="Se detecto más de una Resolucion."></asp:Label>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Menu_PPal/actualizar.png" style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
       </asp:Panel> 
       
        <asp:Button ID="button1" runat="server" Text="Button1" style="visibility:hidden" />
       
       <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" 
            TargetControlID="Button7"
            PopupControlID="pnl_datos"
            CancelControlID="btn_cancel_2"
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion">
       </asp:ModalPopupExtender>
                <asp:Panel ID="pnl_datos" runat="server" CssClass="CajaDialogo" style="height: 221px; width: 358px; top: 174px; left: 212px; z-index:1001;display:none; "> 
            <asp:Button style="Z-INDEX:116;width:105px;top:187px;left:108px;position:absolute;right:171px;background-image: url('images/icons/cancel.png');" id="btn_cancel_2" runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:Button style="Z-INDEX:116;width:84px; top:187px;left:16px;position:absolute;right:264px;background-image: url('images/icons/okay.png');" id="btn_enviar_2" runat="server" Text="Enviar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:TextBox ID ="Limite" runat ="server"  style=" top: 100px; left: 17px; position: absolute;width: 100px;"></asp:TextBox>
            <asp:Label   ID="Label1"  runat="server" Text="Digite Limite del Embargo" style="top: 12px; left: 108px; position: absolute; height: 15px; width: 220px"></asp:Label>
            <asp:Label   ID="Label5"  runat="server" style="top:150px;left:17px;position:absolute;" Text="Favor digitar el campo con el porcentaje de embargo."></asp:Label>
            <asp:Label   ID="Label4"  runat="server" style="top:166px;left:17px;position:absolute;" Text="No hay datos de limite de embargo."></asp:Label>
            <asp:Image   ID="Image2"  runat="server" ImageUrl="~/Menu_PPal/actualizar.png" style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
       </asp:Panel> 
       
        <asp:Button ID="button7" runat="server" Text="Button1" style="visibility:hidden" />
       
       <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
            TargetControlID="Button2"
            PopupControlID="pnlSeleccionarDatos"
            CancelControlID="btnNo"
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion">
       </asp:ModalPopupExtender>

        <script type="text/javascript">
            function mpeSeleccionOnCancel() {
                var pagina = '../login.aspx'
                location.href = pagina
                return false;
            }
        </script>
    </form> 
    </div>
    
     <div id="validateUser" runat="server" class="xparametrosInfo_Permisos"  style="display:none; text-align:left; " title="USUARIO - NO TIENE PERMISOS">
                <img src="../images/1321994028_watchman.png" alt="Seguridad" style="float:left;" title="Seguridad" />
                <span style="font-weight:bold;font-size:14px;">Atención de seguridad</span>
                <br />  
                <p style="text-align:justify;font-size:xx-small;">Lo sentimos pero el usuario con el cual se encuentra identificado no tiene <b>permisos</b> (o derechos de acceso)  para procesar este expediente.</p>
               
            </div>
             <div id="dialog-modal" title="Procesando..." style="text-align:center">   
            <span id= "procesando_div" style="display:none;" >
            <img src="images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
            </span>
    </div> 
    
</body>
</html>
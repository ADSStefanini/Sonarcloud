<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="cobranzasMasiva2.aspx.vb"
    Inherits="coactivosyp.cobranzasMasiva2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <!-- jquery-1.10.2.min.js y jquery-ui-1.10.4.custom.min.js -->

<%--<script src="../js/jqueryblockUI.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="Maestros/js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="Maestros/js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="Maestros/css/redmond/jquery-ui-1.10.4.custom.css" />
    <style type="text/css">
        
     div.growlUI { background: url(check48.png) no-repeat 8px 8px; }
     
     div.growlUI h1, div.growlUI h2 {
     color: white; padding: 5px 5px 5px 60px; text-align: left;
}
.CajaDialogo {position:absolute;background-color:#f0f0f0;border-width: 7px;border-style: solid;border-color: #72A3F8;padding: 0px;color:#514E4E;font-weight: bold;font-size:12px;font-style: italic;}
        body
        {
            background-color: #01557C;
        }
        #encabezado
        {
            background: url('Maestros/images/resultados_busca.jpg');
            height: 37px;
            width: 100%;
            border: solid 1px #a6c9e2;
            padding-bottom: 5px;
        }
        #tituloencabezado
        {
            color: White;
            margin-top: 16px;
            margin-left: 2px;
            font: 12px Verdana;
            font-weight: bold;
            width: 50%;
        }
        #infoexpediente
        {
            background-color: #FFFFFF;
            width: 30%;
            float: left;
            height: 510px;
            margin-bottom: 10px;
            margin-top: 10px;
            margin-left:2px;
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
            font-weight: bold;
            font-size: 11px;
            -moz-border-radius: 3px;
            -border-radius: 3px;
            -webkit-border-radius: 3px;
                    }
        #inforeport
        {
            background-color: #FFFFFF;
            width: 69%;
            float: right;
            height: 510px;
            margin-bottom: 10px;
            margin-top: 10px;
            font-family: Lucida Grande,Lucida Sans,Arial,sans-serif;
            font-weight: bold;
            font-size: 11px;
            -moz-border-radius: 3px;
            -border-radius: 3px;
            -webkit-border-radius: 3px;
        }
        .add_edit
        {
            width: 20px;
            height: 16px;
            float: none;
        }
        .numeros
        {
            text-align: right;
        }
        .BoundFieldItemStyleHidden
        {
            display: none;
        }
        .BoundFieldHeaderStyleHidden
        {
            display: none;
        }
        th
        {
            padding-left: 8px;
            padding-right: 8px;
        }
        .xlist
        {
            text-align: left;
            font-family: Verdana;
            font-size: 9px;
            font-weight: normal;
            font-style: normal;
            color: Black;
        }
        .Botones /* Estilo de los botones */
        {
            height: 22px;
            background-color: #FFF;
            border-bottom: 1px solid #555555;
            border-right: 1px solid #555555;
            border-top: 0px;
            border-left: 0px;
            font-size: 12px !important;
            color: #000;
            padding-left: 20px;
            background-repeat: no-repeat;
            cursor: hand;
            cursor: pointer;
            outline-width: 0px;
            background-position: 4px 4px;
            outline-width: 0px;
            margin-top: 4px;
            margin-right: 4px;
        }
        .Botones:hover /* Efectos del Mouse en los botones */
        {
            height: 25px;
            background-color: #F2F2F2;
            border-bottom: 1px solid #000;
            border-right: 1px solid #000;
            border-top: 0px;
            border-left: 0px;
            font-size: 12px !important;
            color: #000;
            padding-left: 20px;
            background-repeat: no-repeat;
            cursor: hand;
            cursor: pointer;
            outline-width: 0px;
        }
    </style>

    <script type="text/javascript">

        $("#tabs").tabs();
        $("#persuasivo-tabs").tabs();
        $("#coactivo-tabs").tabs();
        /* mantener tab actual nivel 1 */
        $(function() {
            //  Atención: Esto funciona para jQueryUI 1.10 y HTML5.
            //  Define friendly index name
            var index = 'valornivel1';
            //  Define friendly data store name
            var dataStore = window.sessionStorage;
            //  Start magic!
            try {
                // getter: Fetch previous value
                var oldIndex = dataStore.getItem(index);
            } catch (e) {
                // getter: Always default to first tab in error state
                var oldIndex = 0;
            }
            $('#tabs').tabs({
                // The zero-based index of the panel that is active (open)
                active: oldIndex,
                // Triggered after a tab has been activated
                activate: function(event, ui) {
                    //  Get future value
                    var newIndex = ui.newTab.parent().children().index(ui.newTab);
                    //  Set future value
                    dataStore.setItem(index, newIndex)
                }
            });
        });

                
    </script>
<%--<script type ="text/javascript" >
    $(document).ready(function() {
        $('#btnImprimir').click(function() {
        $.growlUI('', '<img src="Security/images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />', 2000);
        });
    }); 
      

</script>--%>
<script type="text/javascript" language="javascript">
    function mostrar_procesar() {
        document.getElementById('procesando_div').style.display = "";
        $("#dialog-modal").dialog({
            height: 150,
            modal: true
        });
        setTimeout(' document.getElementById("procesando_gif").src="images/gif/ajax-loader.gif"', 50);
    }
</script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
     </asp:ToolkitScriptManager>
    <div id="dialog-message" title="Alerta" style="text-align:left;font-size:10px;display:none;">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
            <%=ViewState("message")%>
        </p>
    </div>
  
    <div id="encabezado">
        <div id="tituloencabezado">
            <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
            <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                Text="Label"></asp:Label>)</span>
        </div>
        <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
            padding-right: 8px; margin-top: -18px;">
            <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>
        </div>
        <div style="color: White; width: 30px; height: 20px; float: right; margin-top: -18px;
            text-align: right; padding-right: 8px;">
            <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>
        </div>
    </div>
    <div style="border: solid 0px white; height: 600px; width: 100%">
        <div id="infoexpediente">
            <div id="Div1" style="margin: 2px; height: 500px" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
                <div class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all"
                    style="height: 25px;">
                    <div style="float: left; height: 20px; width: 106px;">
                        <asp:CheckBox ID="CheckHeader" runat="server" Text="" OnCheckedChanged="CheckHeader_OnCheckedChanged" AutoPostBack="true"  /></div>
                    Expedientes seleccionados
                </div>
                <div style="width: 100%; height: 470px; overflow: auto">
                    <asp:CheckBoxList ID="ListExpedientes" runat="server" CssClass="xlist">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
        <div id="inforeport">
            <div id="tabs" style="margin: 2px; height: 500px" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
                <div style="float: right; height: 20px;">
                    <asp:Button ID="btnImprimir" runat="server" Text="Generar" CssClass="Botones" Style="background-image: url('images/icons/printer.png');"
                        Width="95px" />
                </div>
                <ul class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all" style="">
                    <li class="ui-state-default ui-corner-top ui-tabs-active ui-state-active">
                        <asp:Button ID="BtnPersuasivo" runat="server" Text="Persuasivo" CssClass="ui-state-default ui-corner-top ui-tabs-active ui-state-active" />
                    </li>
                    <li class="ui-state-default ui-corner-top">
                        <asp:Button ID="BtnCoactivos" runat="server" Text="Coactivos" CssClass="ui-state-default ui-corner-top ui-tabs-active" />
                    </li>
                    <li>
                        <asp:Button ID="BtnMedidas" runat="server" Text="Medidas Cautelares" 
                            CssClass="ui-state-default ui-corner-top ui-tabs-active" Height="23px" />
                    </li>
                    <li>
                        <asp:Button ID="BtnConcursales" runat="server" Text="Concursales" 
                            CssClass="ui-state-default ui-corner-top ui-tabs-active" Height="22px" 
                            style="margin-top: 0px" Width="142px" />
                    </li>
                </ul>
                <div id="persuasivo-tabs" style ="width:100%;height:470px;overflow:auto;">
                    <asp:RadioButtonList ID="Lista" runat="server" CssClass="xlist">
                    </asp:RadioButtonList>
                   <%-- <asp:CustomValidator ID="Validator" runat="server" ForeColor="Yellow" Font-Names="Arial"
                        Font-Size="15px" ErrorMessage="CustomValidator" Font-Bold="True"></asp:CustomValidator>--%>
                </div>
            </div>
        </div>
    </div>
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
            <asp:Label   ID="Label4"  runat="server" style="top:166px;left:17px;position:absolute;" Text="No hay datos de limite de embargo ."></asp:Label>
            <asp:Image   ID="Image2"  runat="server" ImageUrl="~/Menu_PPal/actualizar.png" style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
       </asp:Panel> 
       
        <asp:Button ID="button7" runat="server" Text="Button1" style="visibility:hidden" />
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" 
            TargetControlID="Button1"
            PopupControlID="pnlSeleccionarDatos"
            CancelControlID="btnNo"
            DropShadow="False"
            BackgroundCssClass="FondoAplicacion">
       </asp:ModalPopupExtender>
       <asp:Panel ID="pnlSeleccionarDatos" runat="server" CssClass="CajaDialogo" style="height: 221px; width: 358px; top: 174px; left: 212px; z-index:1001;display:none; "> 
            <asp:Button style="Z-INDEX:116;width:105px;top:187px;left:108px;position:absolute;right:171px;background-image: url('images/icons/cancel.png');" id="btnNo" runat="server" Text="Cancelar" Height="23px" CssClass="Botones"></asp:Button>
            <asp:ListBox ID="list_embargos" runat="server" style="top: 30px; left: 109px; position: absolute; height: 131px; width: 223px"></asp:ListBox>
            <asp:Label ID="Label2" runat="server" Text="Expedientes" style="top: 12px; left: 108px; position: absolute; height: 15px; width: 220px"></asp:Label>
            <asp:Label ID="Label3" runat="server" style="top:166px;left:17px;position:absolute;" Text=" NO SON LIQUIDACIONES OFICIALES "></asp:Label>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Menu_PPal/actualizar.png" style="top: 0px; left: 0px; position: absolute; height: 75px; width: 115px" />
       </asp:Panel> 
    </form>
   
        
    <div id="dialog-modal" title="Procesando..." style="text-align:center">   
            <span id= "procesando_div" style="display:none;" >
            <img src="images/gif/ajax-loader.gif" alt="Procesando..." id="procesando_gif" />
            </span>
    </div>  
    
   
</body>
</html>

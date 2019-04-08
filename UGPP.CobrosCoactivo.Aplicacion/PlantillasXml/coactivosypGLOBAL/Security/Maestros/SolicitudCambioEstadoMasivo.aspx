<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SolicitudCambioEstadoMasivo.aspx.vb" Inherits="coactivosyp.SolicitudCambioEstadoMasivo" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Solicitudes de cambio de estado masivas</title>
    
    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="jquery.ui.button.js" type="text/javascript"></script>
    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    
    <style type="text/css">
        * { font-size:12px; font-family:Arial}	
        div.growlUI { background: url(check48.png) no-repeat 8px 8px; }     
        div.growlUI h1, div.growlUI h2 {color: white; padding: 5px 5px 5px 60px; text-align: left;}
        .CajaDialogo {position:absolute;background-color:#f0f0f0;border-width: 7px;border-style: solid;border-color: #72A3F8;padding: 0px;color:#514E4E;font-weight: bold;font-size:12px;font-style: italic;}
        body { background-color: #01557C; }
        #encabezado{
            background: url('images/resultados_busca.jpg');
            height: 37px;
            width: 100%;
            border: solid 1px #a6c9e2;
            padding-bottom: 5px;
        }
        #tituloencabezado{
            color: White;
            margin-top: 16px;
            margin-left: 2px;
            font: 12px Verdana;
            font-weight: bold;
            width: 50%;
        }
        #infoexpediente {
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
        #inforeport{
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
        .add_edit{width: 20px; height: 16px; float: none; }
        .numeros {text-align: right;}
        .BoundFieldItemStyleHidden {display: none;}
        .BoundFieldHeaderStyleHidden{display: none;}
        th{ padding-left: 8px; padding-right: 8px;}
        .xlist {
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
        $(function() {            
            $('#cmdSave').button();
            $('#cmdCancel').button();            
        });
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <!-- Inicio del diseño -->
    <div id="encabezado">
        <div id="tituloencabezado">
            <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
            <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                Text="Label"></asp:Label>)</span>
        </div>
        <div style="color: White; width: 30px; height: 20px; float: right; text-align: right;
            padding-right: 8px; margin-top: -18px;">
            <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>
        </div>
        <div style="color: White; width: 30px; height: 20px; float: right; margin-top: -18px;
            text-align: right; padding-right: 8px;">
            <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>
        </div>
    </div>
    <div style="border: solid 0px white; height: 600px; width: 100%">
        <div id="infoexpediente">
            <div id="Div1" style="margin: 2px; height: 500px" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
                <div class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all"
                    style="height: 25px;">
                    <div style="float: left; height: 20px; width: 106px;">
                        <%--<asp:CheckBox ID="CheckHeader" runat="server" Text="" OnCheckedChanged="CheckHeader_OnCheckedChanged" AutoPostBack="true"  />--%>
                    </div>
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
                
                <div id="persuasivo-tabs" style ="width:100%;height:470px;overflow:auto;">
                   <table id="tblEditSOLICITUDES_CAMBIOESTADO" class="ui-widget-content"> 
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado al que se traslada * 
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestado" runat="server"></asp:DropDownList>
                    </td>
                </tr>  
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Observaciones *
                    </td>
                    <td>
                        <textarea id="taObservaciones" cols="90" rows="6" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
                    </td>
                </tr>                                                            
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado solicitud
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestadosol" runat="server" 
                            Enabled="False"></asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar solicitud" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;                        
                    </td>
                </tr>
            </table>
                   
                </div>
            </div>
        </div>
    </div>
      <asp:Button ID="button1" runat="server" Text="Button1"  /> 
      <asp:Button ID="button7" runat="server" Text="Button1"  />       

      <!-- Fin del diseño    -->
    
    </form>
</body>
</html>

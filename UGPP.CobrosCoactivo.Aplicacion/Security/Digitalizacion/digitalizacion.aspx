<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="digitalizacion.aspx.vb" Inherits="coactivosyp.digitalizacion" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Digitalizacion</title>
    <link href="../ErrorDialog.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/menu_style.css" type="text/css" media="all" />
     <link href="../../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    
    <style type="text/css">
        body {text-align:center;margin:0px;font-family:"Arial", "Helvetica", "Verdana", "sans-serif";font-size:10px;}
        #wrapper {width:100%;text-align:left; margin-left:auto; margin-right:auto;}
        #header {width:100%;}
        #header a {color:#fff;text-decoration:none;}
        #content {margin-bottom: -1em;margin-left:auto;margin-right:auto;width:1002px;position:relative;z-index:0;}
        .ImagenesManagerClass {height:100%;text-align: center;background-color: #E6E6E6;margin:0;position:absolute;z-index:0;}
        .disenno {border-top:solid 3px #0b4296; border-bottom:solid 1px #0b4296; border-left:solid 1px #0b4296; border-right:solid 1px #0b4296;z-index:0;}
        .label {padding:3px 0 3px 0}
        #taskbar {height:25px;width:70%;margin:auto;position: fixed;left:auto;right:0;bottom:0;z-index: 1001;}
        #taskbar a {text-decoration:none;}
        #taskbar #container {background-color:#E5E5E5;border:1px solid #B5B5B5;display:block;margin-left:15px;margin-right:15px;height:25px;}
        #taskbar #container .block-left {position:relative;float:left;height:25px;border-right:1px solid #B5B5B5;}
        #taskbar #container .block-center {position:relative;float:left;height:25px;border-right:1px solid #B5B5B5;margin-left:5px;}
        #taskbar #container .block-right{position:relative;float:left;height:25px;border-right:1px solid #B5B5B5;}
        #taskbar #container .btns {font-family:Verdana, Arial, Helvetica, sans-serif;font-size:10px;padding:3px;display:inline-block;vertical-align:middle;line-height:14px;}
        .jtable th, td {padding:5px;}
        .jtable th {font-weight:bold;}
        #Formulario_Dig input[type=text] {width:162px;}
    </style>
    <script type="text/javascript">
        $(document).ready(function(e) {
            // Dialog Guardar nuevo registro
            $("#archivadores").tabs();
            var Modulo_dlgd = jQuery('#Modulo_xdialog_dig').dialog({
                resizable: false,
                autoOpen: true,
                minHeight: 10,
                width: 227,
                modal: false,
                buttons: {
                    "Guardar": function() {
                        $("#<%=btnguardar.ClientID%>").click();
                    },
                    "Cancelar": function() {
                        limpiar_cajas();
                    }
                },
                hide: 'fold'
            });
            $('#arc').click(function(evento) {
                evento.preventDefault();
                $('#Modulo_xdialog_dig').dialog('open');
            });
            Modulo_dlgd.parent().appendTo(jQuery("form:first"));
            Modulo_dlgd.dialog('option', 'position', [10, 40]);
            function limpiar_cajas() {
                $("#txtExpediente").val("");
                $("#txtid").val("");
                $("#txtNombred").val("");
                $("#txtpredio").val("");
                $("#txtactadmin").val("");
                $("#txtAcum").val("");
                $("#txtesta").val("");
                $("#txtTipo").val("");
            }
            $("#dialog-confirm").dialog({
                resizable: false,
                autoOpen: false,
                modal: true,
                buttons: {
                    "Si": function() {
                        $(this).dialog("close");
                        window.open("Archivadores.aspx", "mywindow", "location=0,status=1,scrollbars=1,modal=yes,width=440,height=350,screenX=150,left=150,screenY=50,top=50");
                    },
                    "No": function() {
                        $(this).dialog("close");
                    }
                },
                hide: 'fold'
            });
            $('#Archivadores').click(function(evento) {
                evento.preventDefault();
                $('#dialog-confirm').dialog('open');
                return false;
            });
            $('#imporD').click(function(evento) {
                evento.preventDefault();
                window.open("Importacion_automática_expediente.aspx", "mywindow", "location=0,status=1,scrollbars=1,modal=yes,width=440,height=350,screenX=150,left=150,screenY=50,top=50");
                return false;
            });
            $(".jtable th").each(function() {
                $(this).addClass("ui-state-default");
            });
            $(".jtable td").each(function() {
                $(this).addClass("ui-widget-content");
            });
            $("#fechRac").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'drop',
                dateFormat: 'dd/mm/yy',
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
                    'Junio', 'Julio', 'Agosto', 'Septiembre',
                    'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
                    'May', 'Jun', 'Jul', 'Ago',
                    'Sep', 'Oct', 'Nov', 'Dic']
            });
            $("#fechDoc").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'drop',
                dateFormat: 'dd/mm/yy',
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo',
                    'Junio', 'Julio', 'Agosto', 'Septiembre',
                    'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr',
                    'May', 'Jun', 'Jul', 'Ago',
                    'Sep', 'Oct', 'Nov', 'Dic']
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
             
    </script>
</head>
<body>
    <form id="form_digitalizacion" runat="server">
        <div id="wrapper">
            <div id="header">
                <ul id='menu'>
                    <li><a class='current' href='../menu2.aspx' >Regresar menú principal</a></li>
                    <li><a href='#' id="arc">Digitalización</a></li>
                    <li><a id="Archivadores" href="Archivadores.aspx">Nuevo Carpeta</a></li>
                    <li><a id="regDeudor" href="../maestro_entesdbf.aspx">Registrar Deudores</a></li>
                    <li><a id="imporD" href='#' title="Esta opción le permite importar información de otra base de datos a Tecno Expedientes facilitando procesos largos de digitalización.">Importación automática del expediente</a></li>
                    <li> <a id="A1" href='#' <div id ="Div1">Usuario: <%  Response.Write(Session("ssnombreusuario") & ".")%> </div></a></li>
                </ul>
            </div>
            <div id="content">
                <div id ="ImagenesManager" runat="server"  class="ImagenesManagerClass">
                  
                </div>
            </div>
        </div>
        <div id="Modulo_xdialog_dig" title="Digitalización">
            <div id="archivadores" style="text-align:left;">
                <ul>
                    <li><a href="#tabs-1">Digitacion</a></li>
                    <li><a href="#tabs-2" id="selectTabs2">Archivos</a></li>
                </ul>
                <div id="tabs-1">
                    <div id="Formulario_Dig" style=" font-size:11px;">
                        <div>
                            <div class="label">Expediente</div>
                            <asp:TextBox ID="txtExpediente" runat="server" CssClass="ui-widget-content ui-corner-all" AutoPostBack="True" title="Numero de expediente"></asp:TextBox>
                        </div>
                        <div>
                            <div class="label">CC/Nit</div>
                            <input  type="text" id = "txtid" runat="server" class="ui-widget-content ui-corner-all" />
                        </div>
                        <div>
                            <div class="label">Nombre deudor</div>
                            <input  type="text" id = "txtNombred" runat="server" class="ui-widget-content ui-corner-all" />
                            
                        </div>
                        <div>
                            <div class="label">Predio/</div>
                            <input  type="text" id = "txtpredio" runat="server" class="ui-widget-content ui-corner-all" />
                        </div>
                        <div>
                            <div class="label">Acto Administrativo</div>
                            <input maxlength="3"  runat="server" style="width:30px;text-align:center;" type="text" id = "txtactadmin" class="ui-widget-content ui-corner-all" />
                            
                        </div>
                        <div>
                            <div class="label">Fecha Expedición</div>
                            <input id="fechDoc" runat="server" type="text" class="ui-widget-content ui-corner-all" title="Asegúrese de guardar la fecha en días, mes,  y años (dd/mm/yyy)" />
                        </div>
                        <div>
                            <div class="label">Fecha Radicación</div>
                            <input id="fechRac" runat="server" type="text" class="ui-widget-content ui-corner-all" title="Asegúrese de guardar la fecha en días, mes,  y años (dd/mm/yyy)" />
                        </div>
                        <div>
                            <div class="label">Acumulacion</div>
                            <input  type="text" id = "txtAcum"  runat="server"  class="ui-widget-content ui-corner-all" />
                        </div>
                        <div>
                            <div class="label">Tipo</div>
                            <asp:DropDownList ID="DropDownListTipo" runat="server" Width="166px" Font-Size="XX-Small" ></asp:DropDownList>
                        </div>
                        <div style="padding:10px 6px 0 6px;border: 2px dashed #ccc;margin-top:8px;">
                            <p style="margin:0px;padding:0px;line-height:0;font-size:xx-small;">Ativo tributario</p>
                            <asp:RadioButtonList ID="RadioButtonEstado" runat="server" RepeatDirection="Horizontal" style="margin:0;padding:0;line-height:0;">
                                <asp:ListItem Value="S" Selected="True">Activo</asp:ListItem>
                                <asp:ListItem Value="N">Inactivo</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div style=" margin-top:4px;">
                            <asp:CheckBox ID="CheckDeshabilitado" runat="server" Text="Deshabilitado" />
                        </div>
                    </div>
                </div>
                <div id="tabs-2">
                    <asp:ListBox ID="ListBox" runat="server"  Width="166px" Height="330px" 
                        AutoPostBack="True" Font-Names="Arial" Font-Size="12px">
                    </asp:ListBox>
                </div> 
                <asp:Button ID="btnguardar" runat="server" Text="Button" style="display:none;" />
            </div> 
        </div>
        <div id="dialog-confirm" title="¿Nuevo Carpeta?" style="display:none;font-size:11px;text-align:left;">
	        <p><span class="ui-icon ui-icon-alert" style="float:left;margin:0 7px 5px 0;"></span>¿Desea crear una nueva carpeta?</p>
            <p style="text-align:justify;"><b>Nota:</b> Una carpeta es donde se guardaran las imágenes, para luego digitalizarar la información asociada a la misma. </p>
        </div>
        <div id="gridexpedientes" style="display:none;text-align:left; max-height:300px;overflow:auto;" title="Expediente">
            <asp:GridView ID="GridExp" runat="server" AutoGenerateColumns="False" CssClass="jtable" Width="100%">
                <Columns>
                    <asp:BoundField DataField="idacto" HeaderText="Acto" />
                    <asp:ButtonField CommandName="select" DataTextField="nomarchivo" 
                        HeaderText="Archivo" Text="Archivo" />
                    <asp:BoundField DataField="paginas" HeaderText="Pag." >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="fecharadic" HeaderText="Fec. Rad" 
                        DataFormatString="{0:d}" />
                    <asp:BoundField DataField="docacumulacio" HeaderText="Acumulado" />
                    <asp:BoundField DataField="DOCUSUARIO" HeaderText="Usuario" />
                    <asp:BoundField DataField="DOCFECHASYSTEM" HeaderText="Fecha Sys" />
                </Columns>
            </asp:GridView>
        </div>
        <div id="dialog-message" title="Mensaje del sistema" style="text-align:left;font-size:10px;display:none;">
            <p>
            <span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
                <%=ViewState("message")%>
            </p>
        </div>
        <div id="dialog-Archivadores" title="Carpetas creadas" style="text-align:left;font-size:10px;display:none;">
            <div style="max-height:350px;overflow:auto;font-size:xx-small;">
                <asp:GridView ID="GridArchivos" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="jtable">
                    <Columns>
                        <asp:ButtonField CommandName="select" DataTextField="ARC_COD" HeaderText="Cod." Text="Botón"></asp:ButtonField>
                        <asp:BoundField DataField="ARC_NOMBRE" HeaderText="Nombre" />
                        <asp:BoundField DataField="ARC_FECHA" HeaderText="Fecha" />
                        <asp:CommandField ButtonType="Image" 
                            EditImageUrl="~/Security/images/icons/1324668272_folder_add.png" HeaderText="+" 
                            ShowEditButton="True">
                            <HeaderStyle Height="16px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:CommandField ButtonType="Image" 
                            DeleteImageUrl="~/Security/images/icons/1324668350_folder_closed-delete2.png" 
                            HeaderText="-" ShowDeleteButton="True">
                            <HeaderStyle Height="16px" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div id="taskbar">
            <div id="container">
                <div class="block-left">
                    <asp:LinkButton ID="LinkExaminarExpediente" runat="server" CssClass="btns" title="Utilice este botón para examinar los registros que pertenecen al expediente antes digitado (cuadro expediente).">Expediente (0)</asp:LinkButton>
                </div>
                <div class="block-center">
                    <asp:LinkButton ID="LinkAntes" runat="server" CssClass="btns"><img src="../images/icons/2.png" style="float:left;border:none;" alt="Página anterior" title="Página anterior" /></asp:LinkButton>
                    <div class="btns"><asp:DropDownList Font-Size="10px" ID="paginas" runat="server" AutoPostBack="True" title="Página actual" ></asp:DropDownList></div>
                    <asp:LinkButton ID="LinkSiguiente" runat="server" CssClass="btns"><img src="../images/icons/1.png" style="float:left;border:none;" alt="Página siguiente" title="Página siguiente" /></asp:LinkButton>
                </div>
                <div class="block-right">
                    <asp:LinkButton ID="LinkListaArchivadores" runat="server" CssClass="btns" title="Lista de carpetas."><img src="../images/icons/1323785936_Card_file.png" style="float:left;border:none; margin-right:5px;" alt="Carpetas" />Carpetas</asp:LinkButton>
                </div>
                <div class="block-right">
                   <div class="btns"><img src="../images/icons/_user.png" alt="Usuario"style="float:left;border:none;" /><span style=" margin-left:5px; padding-right:5px;color:Blue;" runat="server" id="_user"></span></div>
                </div>
                <div class="block-right" style="display:none;">
                   <div class="btns"><img src="../images/icons/minus_circle.png" alt="Usuario"style="float:left;border:none;" /><span style=" margin-left:5px; padding-right:5px;color:Blue;">Error archivo no detectado</span></div>
                </div>
            </div>
        </div>
    </form>
     <div id="validateUser" runat="server" class="xparametrosInfo_Permisos"  style="display:none; text-align:left; " title="USUARIO - NO TIENE PERMISOS">
                <img src="../../images/1321994028_watchman.png" alt="Seguridad" style="float:left;" title="Seguridad" />
                <span style="font-weight:bold;font-size:14px;">Atención de seguridad</span>
                <br />  
                <p style="text-align:justify;font-size:xx-small;">Lo sentimos pero el usuario con el cual se encuentra identificado no tiene <b>permisos</b> (o derechos de acceso)  para procesar este expediente.</p>
               
            </div>
</body>
</html>

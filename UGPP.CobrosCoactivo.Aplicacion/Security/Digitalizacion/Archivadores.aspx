<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Archivadores.aspx.vb" Inherits="coactivosyp.Archivadores" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Archivadores</title>
    <link href="../../css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="../../js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <link href="uploadify.css" rel="stylesheet" type="text/css" />
    <script src="swfobject.js" type="text/javascript"></script>
    <script src="jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <style type="text/css">
        .text {font-size:12px;color:#fff;font-family:Arial, Verdana, Tahoma;text-transform:uppercase;font-weight:bold;}
        .idmessage {background-color:#618ce4;padding:10px;border-bottom:2px dashed #d34324;}
        body {margin:0;padding:0; font-family:Arial; font-size:11px;}
        .title {background-color:#304a7d;text-align:left;padding:5px 0 5px 10px;font-size:15px;}
        .RedColor {padding:5px 0 5px 10px;background-color:#e47861;}
        .RedColor2 {padding:0;background-color:#fff;margin:10px;border:dashed 2px #d34324;display:none;}
        .error {background-color:#d34324;margin:0;padding:3px;color:#fff;font-size:xx-small;}
        .panel {background-color:#fff;font-size:13px;}
    </style>
    <script type="text/javascript">
        $(function() {
            $("input:submit, a, button", "#idmessage").button();
            $('#custom_file_upload').uploadify({
                'uploader': 'uploadify.swf',
                'script': 'UploadHandler.ashx?archivador=<%=ViewState("archivador")%>',
                'cancelImg': 'cancel.png',
                'folder': '/uploads',
                'multi': true,
                'auto': false,
                'buttonText': 'Adjuntar imagen',
                'fileExt': '*.jpg;*.gif;*.png;*.tiff;*.tif;*.pdf',
                'fileDesc': 'Archivos de imagen Web (.JPG, .GIF, .PNG, .TIFF, .PDF)',
                'queueID': 'custom-queue',
                'simUploadLimit': 4,
                'removeCompleted': true,
                'onSelectOnce': function(event, data) {
                 //   $('#status-message').text(data.fileCount + ' Archivos adjuntados.');
                },
                'onCancel': function(event, ID, fileObj, data) {
                  //  $('#status-message').text(data.fileCount + ' Archivos adjuntados.');
                },
                'onQueueFull': function(event, queueSizeLimit) {
                 //   $('#di_status-message').text('Estoy lleno, por favor no poner más archivos!');
                 //   $('#de_status-message').text('Usted ha excedido el límite de archivos que se pueden adjuntar.');
                 //   $('#dialog').dialog("open");
                 //   return false;
                },
                'onAllComplete': function(event, data) {
                 //   $('#status-message').text(data.filesUploaded + ' Archivos subidos, ' + data.errors + ' errores.');
                  //  $('#xtabs-2').click();
                },
                'onComplete': function(event, ID, fileObj, response, data) {
                    //$(".preimagenes").show(1000, function() {
                    //    $(".principal").append("<img style='display:none;' src='Products/gimp.png' border='0' alt='' />");
                    //    $(".principal img:hidden:first").fadeIn("slow");
                    //});
                }
            });
        });
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="title text"><span>Nombre del archivador : </span></div>
        <div class="RedColor2 text" id="Messenger" runat="server" >
            <div class="error"> Error : Ha ocurrido una excepción. </div>
            <div style="padding:5px 0 5px 10px;" id="Messenger_contenido" runat="server">
                   Digite el nombre del archivador para continuar
            </div>
        </div>
        <div id="idmessage" runat="server"  class="idmessage">
            <input id="txtArchivador" type="text" runat="server" class="ui-widget-content ui-corner-all" style="width:100%;margin-top:5px;"  />
            <br /> <br />
            <asp:LinkButton ID="LinkGuardar" runat="server">Guardar</asp:LinkButton>
        </div>
        <!-- Panel si hay algun Error-->
        
        <!-- panel para subir imagen -->
        <div id="custom_demo" style="display:none;" runat="server" class="idmessage panel">
            <div id="controles_subir_imagen">
                <a href="javascript:$('#custom_file_upload').uploadifyUpload();">Subir imagenes</a> | <a href="javascript:$('#custom_file_upload').uploadifyClearQueue();">Limpiar archivos adjuntados</a>
            </div>
            <div id="status-message">
                Seleccione algunos archivos para subir:
            </div>
            <div id="custom-queue" style="padding-top:3px;"></div>
            <br />
            <input id="custom_file_upload" type="file" name="Filedata" />  
        </div>
        <div class="RedColor text" id="_Error" runat="server" >Digite el nombre del archivador para continuar</div>
    </form>
</body>
</html>

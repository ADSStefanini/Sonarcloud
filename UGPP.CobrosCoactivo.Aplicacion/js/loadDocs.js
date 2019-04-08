/**Carga de documentos */
// Varables para carturar valores antiguos
var nomDoc;
var linkDoc;

jQuery(document).ready(function ($) {
    $(document).on("click", "#cmdAsignarDocumento2", function (e) {
        $('.modal-backdrop').hide();
        nombreDocActualizadoCount = 0;
        $("#linkver, #linkVer").attr("href", "#");
        $("#lblDocName").html("");

        $("#hdnResultUpload").val("0");
        var _interval = setInterval(function () {
            finalUrlStrign = $('[id*="HdnPathFile"]').val();
            linkDoc = finalUrlStrign;
            nomDoc = finalFileName;
            if (nombreDocActualizadoCount > 0) {
                clearInterval(_interval);
            }
                cerrarPopupDocumento();          
        }, 100);
    });

    $(document).on("click", "#btnCerrarCargaDocumento", function (e) {
        //$("#lblDocName").hide();
    });

    setInterval(function () {
        if (typeof nomDoc == "undefined" || nomDoc != "") {
            $("#lblDocName").html("").html(nomDoc).show();
            $("#linkver").attr("href", linkDoc).show();
        }
    }, 500);

});

function capturarDatosDocPrecargado() {
    setTimeout(function () {
        nomDoc = $("#lblDocName").html();
        linkDoc = $("#linkVer").attr("href");
        console.log(nomDoc)
    }, 1000);   
}

var NombreItemDoc

// Función que se encarga de agregar los archivos
function AddFileUpload(strNombreitem) {
    setTimeout(function () {
        //$("#hlinkViewDoc").hide();
        //$("#newDoc_lblErrorDoc").hide();
        NombreItemDoc = strNombreitem;
        NombreItemDoc = NombreItemDoc.replace('btnCargar', 'FlUpEvidencias');
        //console.log(NombreItemDoc)
        $("#" + NombreItemDoc).click();

    }, 500);
}

function AjaxFileUpload() {
    $("#HdnPathFile").val("");
    var files = $("#" + NombreItemDoc).get(0);
    var data = new FormData();
    var nameFile = '';
    for (var i = 0; i < files.files.length; i++) {
        data.append(files.files[i].name, files.files[i]);
        nameFile = files.files[i].name;
    }

    $.ajax({
        url: baseUrl + "Security/Controles/FileUploadHandler.ashx",
        type: "POST",
        data: data,
        contentType: false,
        processData: false,
        success: function (result) {
            NombreItemDoc = NombreItemDoc.replace('FlUpEvidencias', 'LblArchivo');
            //console.log(NombreItemDoc);
            $('#' + NombreItemDoc).text(nameFile);
            NombreItemDoc = NombreItemDoc.replace('LblArchivo', 'HdnPathFile');
            result = result.replace(/\\/g, "/");
            var finalFileName = result.split("/");
            finalFileName = finalFileName[finalFileName.length - 1];
            console.log(finalFileName);
            $("#lblDocName").html("").html(finalFileName).show();
            $("#linkVer").prop("disabled", false);
            $('#HdnPathFile').val(baseUrl + result);
            $("#hlinkViewDoc, #linkVer").attr("href", baseUrl + result);
            $("#hlinkViewDoc").show();
            capturarDatosDocPrecargado();
        },
        error: function (err) {
            alert("Información Ha ocurrido un problema al cargar el archivo seleccionado. si el problema persiste contacte al administrador.");
        }
    });
}
function CambioEnFileUpload(strNombreitem) {
    //console.log(strNombreitem)
    NombreItemDoc = strNombreitem;
    var ValidExt = $("#HdnExtValidas").val();
    var files = $('#' + NombreItemDoc).get(0);
    var iValidFiles = 0;
    var iSizeFiles = 0;
    if (files.files.length > 0) {
        for (i = 0; i < files.files.length; i++) {
            var _FileExtension = files.files[i].name.substr(files.files[i].name.lastIndexOf('.')).toLowerCase().replace(".", "");
            var pattern = new RegExp(_FileExtension);
            iSizeFiles = (parseInt(files.files[i].size / 1024) / 1024).toFixed(3) // Mb
            var iTotalSizeFileIpload = parseFloat(iSizeFiles);

            if (iTotalSizeFileIpload > 100) {
                alert("Se ha excedido el tamaño maximo de 100 Megabytes por caso.");
            } else if (pattern.test(ValidExt) == true) {
                var FileUploadInfo = files.files[i].name + "," + (parseInt(files.files[i].size / 1024) / 1024).toFixed(3) + "&";
                AjaxFileUpload();
            } else {
                alert("Extencion no valida.");
            }
        }
    }
}


var nombreDocActualizadoCount = 0;
var finalFileName = "";
var finalName = "";
function cerrarPopupDocumento() {   
    if ($("#hdnResultUpload").length && $("#hdnResultUpload").val() == "1") {
        if (finalUrlStrign == "") {
            finalUrlStrign = $("#HdnPathFile").val();
            if (finalUrlStrign == "") {
                return false;
            }
        }

        if ($("#linkver, #linkVer").attr("href") != "" && $("#lblDocName").html() != "") {
            return
        }

        $("#btnCerrarCargaDocumento").trigger("click")
        $("#hdnResultUpload").val("0");
        setTimeout(function () {
            // Actualización del link
            jQuery("#mapa-modal").modal("hide");
            $("#linkver, #linkVer").prop("disabled", false);
            $("#linkver, #linkVer").attr("href", finalUrlStrign);
            //Actualización del nombre dle archivo 
            finalFileName = finalUrlStrign.split("/");
            finalFileName = finalFileName[finalFileName.length - 1];
            $("#lblDocName").html("").html(finalFileName);
            $("#lblDocName").show();
            nombreDocActualizadoCount++;
        }, 1000);
    }
}
/**Carga de documentos / END */
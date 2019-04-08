function mostrarLoading() {
    jQuery("#loading").addClass("loading");
}

$(function () {
    $('.formatearNumero').each(function (k, v) {
        $(v).number(true, 2);
    });

    if ($('.format-number').length) {
        $('.format-number').each(function (k, v) {
            var _text = $(v).text()
            $(v).text(_text.replace(',', '.'))
            $(v).number(true, 2);
        });
    }    
});
jQuery(document).ready(function ($) {
    if ($('.enabled').length) {
        setTimeout(function () {
            $('.enabled').each(function (k, v) {
                $(v).prop('disabled', false);
            });
        }, 1000);
    }

    setInterval(function () {
        if ($('iframe').length) {
            // Se consulta si le tamaño del contenido del iframe es mayor al tamaño del iframe para actualizar su altura y no quede pisado el contenido
            $('iframe').each(function (k, v) {
                if ($(v).contents().find("body").length) {
                    $(v).height("auto");
                    var contentHeight = $(this).contents().find("body").height();
                    var iframeHeight = $(v).height();
                    if (contentHeight > iframeHeight) {
                        $(v).height(contentHeight + 10);
                        $(v).parent().height(contentHeight + 100);
                    }
                }
            });
        }
    }, 1000);

    
    setInterval(function () {
        $('body').find(".button").button();
    }, 5000)
    $(".button").each(function (k, v) {
        $(v).button();
    });

    $('#cmdAddNew').button();
    $('#cmdMostrarVencer').button();
    $('#cmdMostrarVencidos').button();

    //Array para dar formato en español
    $.datepicker.regional['es'] =
        {
            closeText: 'Cerrar',
            prevText: 'Previo',
            nextText: 'Próximo',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            monthStatus: 'Ver otro mes', yearStatus: 'Ver otro año',
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
            dateFormat: 'dd/mm/yy', firstDay: 1,
            initStatus: 'Seleccione la fecha', isRTL: false
        };
    $.datepicker.setDefaults($.datepicker.regional['es']);

    var baseUrlCalendar = (typeof baseUrl !== 'undefined') ? baseUrl + 'Security/Maestros/' : "";

    $(".calendar").keypress(function (event) { event.preventDefault(); });
    $(".calendar").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
    $(".calendar").datepicker({
        numberOfMonths: 1,
        showButtonPanel: true,
        showOn: 'both',
        buttonImage: baseUrlCalendar + 'calendar.gif',
        buttonImageOnly: false,
        changeYear: true,
        beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
        //              ,changeMonth: true
    });
    //});

    // Identar el texto de las tablas para definir relación padres e hijos
    $('.tbl-indentation').each(function (k, v) {
        $("tbody tr").each(function (i, tr) {
            var _indent = $("td:eq( 1 )", $(tr)).text()
            if (_indent.trim() != "") {
                //console.log(_indent)
                $("td:eq( 1 )", $(tr)).next().addClass("text-indent-" + _indent);
            }
        });
    });

    //Agrega estilo a la fila con alerta por suspensión
    if ($(".alertasuspension").length) {
        $(".alertasuspension").each(function (k, v) {
            if ($(v).html() == "Rojo") {
                $(v).closest("tr").addClass("alert-suspension");
                // console.log($(v).html())
            }
        });
    }

    //Agrega estilo a la fila con alerta por suspensión
    if ($(".alertaestadooperativo").length) {
        $(".alertaestadooperativo").each(function (k, v) {
            //console.log($(v).html())
            var tdStyle = "";
            if ($(v).html() == "Amarillo") {
                tdStyle = "yellowalert-estadooperativo"
            } else if ($(v).html() == "Rojo") {
                tdStyle = "redalert-estadooperativo"
            }

            if (tdStyle != "") {
                $(v).closest("tr").find(".fechalimite").addClass(tdStyle);
            }
        });
    }
    //

    //Aceptar solo numeros
    $(document).on("keydown", ".numeros", function (event) {
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 188 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            if (this.value == '') { this.value = 0; } // 26/ene/2014: Si deja la entrada en blanco=>poner cero
            // let it happen, don't do anything
            return;
        } else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });
    
    if ($('.tbl-documentos-titulo').length) {
        //Se oculta el botón de ver y borrar si no hay documento, se agrega listener que muestra dichos botones si se agrega documento
        setInterval(function () {
            $('.tbl-documentos-titulo').each(function (k, v) {
                var _doc = $("[id*='LblArchivo']", $(v));
                var btnView = $("[id*='btnVer']", $(v));
                var btnDelete = $("[id*='btnBorrar']", $(v));
                if (_doc.html() == "") {
                    btnView.hide('fast');
                    btnDelete.hide('fast');
                } else {
                    btnView.show('slow');
                    //Si el documento cumple se oculta el botón de borrar
                    var _rdoSiCumple = $("[id*='RbtnSiCumpleDoc']", $(v));
                    if (_rdoSiCumple.is(":checked")) {
                        btnDelete.hide('fast');
                    } else {
                        btnDelete.show('slow');
                    }
                }
            });
        }, 1000);

        //Se valida si existe un docuemto al cual se le puedan realizar obseraciones, en caso que no exista se oculta la opción de calificar el doc y agregar observaciones
        $('.tbl-documentos-titulo').each(function (k, v) {
            var _doc = $("[id*='LblArchivo']", $(v));
            var _tableContent = _doc.closest('table');
            var _btnActualizarMetaDataDoc = $("[id*='btnActualizarMetaDataDoc']", $(v));
            if (_doc.html() == "") {
                $('.txtCumple, .documentoTitulo4', _tableContent).hide();
                _btnActualizarMetaDataDoc.hide()
            }
        });
    }

    if ($('.force-hide').length) {
        $('.force-hide').remove();
    }
});

function WindowRefresh() {
    window.location.reload(true);
}

function inIframe() {
    try {
        return window.self !== window.top;
    } catch (e) {
        return true;
    }
}

//Se borran todas las referencias en los campos ocultos del documento cargado
function borrarDocumento(objBtn) {
    var _this = $(objBtn);  
    var _tableContent = _this.closest('table');

    $("[id*='LblArchivo']", _tableContent).html("");

    //$("[id*='HdnIdDoc']", _tableContent).val("");
    $("[id*='HdnPathFile']", _tableContent).val("");
    $("[id*='HdnIdMaestroTitulos']", _tableContent).val("");
    $("[id*='HdnCodTipoDodocumentoAO']", _tableContent).val("");
    $("[id*='HdnNomDocAO']", _tableContent).val("");
    $("[id*='HdnNumPaginas']", _tableContent).val("");
    $("[id*='HdnNumPaginas']", _tableContent).val("");
    $("[id*='HdnCodGuid']", _tableContent).val("");
    $("[id*='HdIndDocSincronizado']", _tableContent).val("");
    
}

function abrirPopUpCambioEstado() {
    //setTimeout(function () {
    //    console.log($("#hdnExpedientes").val())
    //}, 2000)
    return false;
}
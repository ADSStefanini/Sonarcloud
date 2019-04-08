function SeleccionaSIDOC(nombre) {
    var strNombreDiv = nombre;
    var strNombreDiv = strNombreDiv.replace('RbtnSiCumpleDoc', 'RbtnNoCumpleDoc');
    var strNombreDivButton = strNombreDiv.replace('RbtnNoCumpleDoc', 'BtnObservaciones');
    $('#' + strNombreDiv).prop("checked", false);
    $('#' + strNombreDivButton).hide();
    $('#' + strNombreDivButton).prop("disabled", false);
}

function validarIdTab(nombre) {
    //debugger; 
    $('#HdnIdTab').val(nombre);
    if (nombre > 2) {
        $('#tblGralCumpleNoCumple').hide();
    }
    else {
        $('#tblGralCumpleNoCumple').show();
    }
}

function SeleccionaNODOC(nombre) {
    var strNombreDiv = nombre;
    var strNombreDiv = strNombreDiv.replace('RbtnNoCumpleDoc', 'RbtnSiCumpleDoc');
    var strNombreDivButton = strNombreDiv.replace('RbtnSiCumpleDoc', 'BtnObservaciones');
    $('#' + strNombreDiv).prop("checked", false);
    $('#' + strNombreDivButton).show();
    $('#' + strNombreDivButton).prop("disabled", false);
}

function Eliminar(strNombreitem) {
    var strNombreDiv = strNombreitem
    var strNombreDiv = strNombreDiv.replace('btnEliminar', 'ItemNotificacion');
    $('#' + strNombreitem).closest('#' + strNombreDiv).remove();;
}
function CambiarFechaNotificacion(strNombreitem) {
    var NombreItem = strNombreitem;
    var NuevaFecha = $('#' + NombreItem).val();
    NombreItem = NombreItem.replace('txtMT_fechaItem', 'HdnItemObj');
    var itemjson = $('#' + NombreItem).val();
    console.log(itemjson);
    var itemdos = JSON.parse(itemjson);
    itemdos.FEC_NOTIFICACION = NuevaFecha;
    var stritem = JSON.stringify(itemdos);
    $('#' + NombreItem).val(stritem);
    console.log(stritem);
}

function CambiarTipoNotificacion(strNombreitem) {
    var NombreItem = strNombreitem;
    var NuevoValor = parseInt($('#' + NombreItem).val());
    NombreItem = NombreItem.replace('cboTipoNotificacion', 'HdnItemObj');
    var itemjson = $('#' + NombreItem).val();
    console.log(itemjson);
    var itemdos = JSON.parse(itemjson);
    itemdos.COD_TIPO_NOTIFICACION = NuevoValor;
    var stritem = JSON.stringify(itemdos);
    $('#' + NombreItem).val(stritem);
    console.log(stritem);
}

function CambiarFormaNotifica(strNombreitem) {
    var NombreItem = strNombreitem;
    var NuevoValor = $('#' + NombreItem).val();
    NombreItem = NombreItem.replace('cboMT_for_not', 'HdnItemObj');
    var itemjson = $('#' + NombreItem).val();
    console.log(itemjson);
    var itemdos = JSON.parse(itemjson);
    itemdos.COD_FOR_NOT = NuevoValor;
    var stritem = JSON.stringify(itemdos);
    $('#' + NombreItem).val(stritem);
    console.log(stritem);
}

function AbrirArchivo(strNombreitem) {
    NombreItemDoc = strNombreitem;
    NombreItemDoc = NombreItemDoc.replace('btnVer', 'HdnPathFile');
    var archivoRuta = $('#' + NombreItemDoc).val();
    AbrirNuevaPagina(archivoRuta);
}
function AbrirNuevaPagina(strUrl) {
    window.open(strUrl, '_blank');
}

$(document).ready(function () {
    $("#PnlMalla").css('max-height', (window.innerHeight - 78).toString() + 'px');
    $("#PnlDocCNC").css('max-height', (window.innerHeight - 78).toString() + 'px');
    $("#pnlObservaCNC").css('max-height', (window.innerHeight - 78).toString() + 'px');
   

    $('.number').keypress(function (event) {
        if (event.which < 46 || event.which > 59) {
            event.preventDefault();
        } // prevent if not number/dot

        if (event.which == 46 && $(this).val().indexOf('.') != -1) {
            event.preventDefault();
        } // prevent if already dot
    });
    $("#dataTable tbody tr").on("click", function () {
        console.log($(this).text());
    });
    $(".formatoColombia").on(
        "paste keyup", function () {
            formatoValor($(this));
            SumarTotal();
        });
})
//Formatea el numero en formato colombiana al formato base
function formatoBase(NuevoValor) {
    NuevoValor = NuevoValor.replace(/\./g, '');
    NuevoValor = NuevoValor.replace(/\,/g, '.');
    return NuevoValor;
}

//Formatea el numero al formato colombiano para sus calculos
function formatoAsignar(NuevoValor) {
    if (NuevoValor != '' && NuevoValor != '0') {
        NuevoValor = NuevoValor.replace(/\,/g, '');
        NuevoValor = NuevoValor.replace(/\./g, ',');
    } else { NuevoValor = '0'; }

    return NuevoValor;
}
// funcion para el formato de valores 
function formatoValor(objetoTexto) {
    debugger;
    if (objetoTexto.val() != '' && objetoTexto.val().substring(objetoTexto.val().length - 1, objetoTexto.val().length) != ',') {
        var NuevoValor = objetoTexto.val();
        NuevoValor = formatoBase(NuevoValor);
        if (NuevoValor.includes(".")) {
            if (NuevoValor.split('.')[1].length > 1) {
                objetoTexto.val(accounting.formatNumber(NuevoValor, 2, '.', ','));
            } else {
                objetoTexto.val(accounting.formatNumber(NuevoValor, 1, '.', ','));
            }
        } else {
            objetoTexto.val(accounting.formatNumber(NuevoValor, 0, '.', ','));
        }
    }

}
function removeThousandSeparator(prmStrValue) {
    var valor = prmStrValue;
    if (valor == null || valor == "") {
        return 0
    }
    //console.log(prmStrValue)
    valor = formatoBase(valor);
    res = parseFloat(valor).toFixed(2);
    console.log(res);
    return res;
}

function SumarTotal() {
    var txttotalsancion = 0;
    if (parseFloat(removeThousandSeparator($("#txtSancionOmision").val())) > 0) {
        txttotalsancion = parseFloat(removeThousandSeparator($("#txtSancionOmision").val()));
        //console.log(parseFloat(removeThousandSeparator($("#txtSancionOmision").val())).toFixed(2))
        //console.log(txttotalsancion)
    }

    if (parseFloat(removeThousandSeparator($("#txtSancionMora").val())) > 0) {
        txttotalsancion = parseFloat(txttotalsancion) + parseFloat(removeThousandSeparator($("#txtSancionMora").val()));
        //console.log(parseFloat(removeThousandSeparator($("#txtSancionMora").val())).toFixed(2))
    }

    if (parseFloat(removeThousandSeparator($("#txtSancionInexactitud").val())) > 0) {
        txttotalsancion = parseFloat(txttotalsancion) + parseFloat(removeThousandSeparator($("#txtSancionInexactitud").val()));
        //console.log(txttotalsancion)
    }

    //console.log(parseFloat(txttotalsancion).toFixed(2))
    $("#txtTotalSancion").val(formatoAsignar(parseFloat(txttotalsancion).toFixed(2)));
    formatoValor($("#txtTotalSancion"));
    var txttotalobligacion = 0
    if (parseFloat(removeThousandSeparator($("#txtValorObligacion").val())) > 0) {
        txttotalobligacion = parseFloat(removeThousandSeparator($("#txtValorObligacion").val())).toFixed(2);
        //console.log(parseFloat(removeThousandSeparator($("#txtValorObligacion").val())).toFixed(2))
    }

    $("#txtTotalObligacion").val(formatoAsignar(txttotalobligacion));
    formatoValor($("#txtTotalObligacion"));
    var txtpartidaglobal = 0
    if (parseFloat(removeThousandSeparator($("#txtPartidaGlobal").val())) > 0) {
        txtpartidaglobal = parseFloat(removeThousandSeparator($("#txtPartidaGlobal").val())).toFixed(2);
    }

    $("#txtTotalPartidaGlobal").val(formatoAsignar(txtpartidaglobal));
    formatoValor($("#txtTotalPartidaGlobal"));

    var total = parseFloat(txttotalsancion) + parseFloat(txttotalobligacion) + parseFloat(txtpartidaglobal)
    $("#txtTotalDeuda").val(formatoAsignar(parseFloat(total).toFixed(2)));
    formatoValor($("#txtTotalDeuda"));


    //TotalDeuda();
}

function ElementoFecha(nameItem) {
    var FechaExpTitulo = $("#txtMT_fec_expedicion_titulo").val();
    var AnioFET = FechaExpTitulo.substring(6, 10);
    var MesFET = FechaExpTitulo.substring(3, 5);
    var DiaFET = FechaExpTitulo.substring(0, 2);

    if ($("#" + nameItem).prop("disabled")) {       
    } else {
        $("#" + nameItem).keypress(function (event) { event.preventDefault(); });
        $("#" + nameItem).keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
        $("#" + nameItem).datepicker({
            numberOfMonths: 1,
            minDate: new Date(AnioFET, MesFET - 1, DiaFET),
            showButtonPanel: true,
            showOn: 'both',
            buttonImage: 'calendar.gif',
            buttonImageOnly: false,
            changeYear: true,
            format: 'dd/MM/yyyy',
            beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
            //              ,changeMonth: true
        });

    }
}

$(function () {
    $('#txtTotalPartidaGlobal').css("background-color", "#ebebe4");
    $('#txtTotalSancion').css("background-color", "#ebebe4");
    $('#txtTotalObligacion').css("background-color", "#ebebe4");
    $('#txtTotalDeuda').css("background-color", "#ebebe4");
    //Manejo de tabs
    $("#tabs").tabs();

    $('submit,input:button,input:submit').button();

    ElementoFecha("txtfecharevocatoria");
    ElementoFecha("txtMT_fec_expedicion_titulo");
    ElementoFecha("txtMT_fec_notificacion_titulo");
    ElementoFecha("txtMT_fec_expe_resolucion_reposicion");
    ElementoFecha("txtMT_fec_not_reso_resu_reposicion");
    ElementoFecha("txtMT_fec_exp_reso_apela_recon");
    ElementoFecha("txtMT_fec_not_reso_apela_recon");
    ElementoFecha("txtMT_fecha_ejecutoriaObli");
    ElementoFecha("txtMT_fec_exi_liqObli");
    ElementoFecha("txtMT_fec_cad_presc");
    ElementoFecha("txtFecMemoDev");

    $('input[name=txtMT_fecha_ejecutoriaObli]').change(function () {
        debugger;
        if ($("#cboTipo_Cartera option:selected").val() == "1" && $("#txtMT_fecha_ejecutoriaObli").val()!="") {
            var _val = $("#txtMT_fecha_ejecutoriaObli").val();
            var _date = moment(_val, 'DD/MM/YYYY', true).format();
            var _date2 = moment(_date).add(1, 'day').format('DD/MM/YYYY');
            $("#txtMT_fec_exi_liqObli").prop("value", _date2.toString());
        }
    });

    $("input[type=text]").keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });
    //
});
//
$(function () {
    //Desactivar los totales
    $("#txttotalmultas").keypress(function (event) { event.preventDefault(); });
    $("#txttotalmultas").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    $("#txttotalomisos").keypress(function (event) { event.preventDefault(); });
    $("#txttotalomisos").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    $("#txttotalmora").keypress(function (event) { event.preventDefault(); });
    $("#txttotalmora").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    $("#txttotalinexactos").keypress(function (event) { event.preventDefault(); });
    $("#txttotalinexactos").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    $("#txttotalsentencias").keypress(function (event) { event.preventDefault(); });
    $("#txttotalsentencias").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    $("#txttotaldeuda").keypress(function (event) { event.preventDefault(); });
    $("#txttotaldeuda").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

    // Descativar la informacion de deuda de los parafiscales ya que va a ser importada del "SQL"//
    $(".numerosSL").keypress(function (event) { event.preventDefault(); });
    $(".numerosSL").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });


    // Borrar la fecha de devolucion del titulo
    $("#imgBtnBorraFechaDev").click(function () {
        //alert( "Handler for .click() called." );
        //txtFecMemoDev
        $("#txtFecMemoDev").val("");
    });

    var perfil = NivelPerfil;
    if (perfil != 10) {
        //Oculta la clasificacion manual
        $("#TabClasificacion").css("display", "none");
    }

});

/* mantener tab actual nivel 1 */
$(function () {
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
        activate: function (event, ui) {
            //  Get future value
            var newIndex = ui.newTab.parent().children().index(ui.newTab);
            //  Set future value
            dataStore.setItem(index, newIndex);
            SumarTotal();
            //validarIdTab(newIndex)
        }
    });
});
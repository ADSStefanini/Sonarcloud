var proccessTabs = 0;
//var process = 0;
jQuery(document).ready(function ($) {
    if (NivelPerfil == "") {
        throw new Error(['var NivelPerfil is empty, that\'s required for tabs module']);
    }
    // Construye HTML de tabs
    getPagesByPerfil(NivelPerfil, 0);
    // visual
    var intervalTabs = setInterval(function () { 
        if (proccessTabs <= 60) {
            if ($(".tabs").length > 0) {
                $(".tabs").tabs();
            }
            proccessTabs++;
        } else {
            clearInterval(intervalTabs);
        }
    }, 1000);
});

function getPagesByPerfil(perfil, parentPage)
{
    var json = getJsonPagesRequest(perfil, parentPage);
    jQuery.ajax({
        type: "POST",
        url: baseUrl + '/Security/webrequest/seguridad.aspx/getPages',
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var pages = $.parseJSON(data.d);
            processTabPages(pages);
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function getJsonPagesRequest(perfil, parentPage)
{
    var json = {
        paginasPorPerfil:
            {
                "idPerfil": perfil,
                "idPaginaPadre": parentPage
            }
    }
    return json;
}

function processTabPages(tabPages)
{
    var url = window.location.href;
    var parameters = "";
    var urlParameters = url.split('?');
    if (urlParameters.length == 2) {
        parameters = urlParameters[1];
    }
    var _tabs = jQuery('<div/>', {
        class: 'tabs'
    });
    if (jQuery('#tabs-content .tabs').length == 0) {
        $('#tabs-content').html(_tabs);
    }

    var _mainContent = jQuery('#tabs-content .tabs:last');
    var _ulTabs = jQuery('<ul></ul>')

    jQuery.each(tabPages, function (k, tabPage) {
        var idTab = "tab-content-" + tabPage.pk_codigo;
        var _tabMainContent = jQuery('<div/>', {
            id: "parent-"+idTab
        });
        _mainContent.append(_tabMainContent.get(0))

        var _tabContent = jQuery('<div/>', {
            id: idTab,
            class: 'tabs'
        });
        jQuery('div', _mainContent).last().append(_tabContent.get(0))

        var _li = jQuery('<li/>');
        var _link = jQuery('<a/>', {
            text: tabPage.val_nombre,
            title: tabPage.val_nombre,
            href: "#parent-" + idTab
        })
        _li.append(_link.get(0))
        _ulTabs.append(_li.get(0));

        if (tabPage.val_url == null) {
            // Cargar las sub-pestañas
            getPagesByPerfil(window.NivelPerfil, tabPage.pk_codigo);
        } else {
            // Ajusto URL para enviar parametros
            var fullUrl = baseUrl + tabPage.val_url;
            if (parameters != "") {
                console.log(fullUrl.indexOf("?"))
                var conector = (fullUrl.indexOf("?")>= 0) ? "&" : "?";
                fullUrl += conector + parameters;
            }
            // Agregar iFrame
            // Asjutamos el width y restamos la margen
            var iframe = $('<iframe/>', {
                src: fullUrl,
                width: (window.innerWidth - 78),
                height: "440",
                scrolling: "no",
                frameborder: "0"
            });           
            $("#" + idTab).append(iframe.get(0))
        }
    });

    //jQuery('#tabs-content .tabs').last().prepend(_ulTabs.get(0))

    jQuery(_mainContent).first().prepend(_ulTabs.get(0))
}
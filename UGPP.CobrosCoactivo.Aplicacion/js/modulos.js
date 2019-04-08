$(document).ready(function () {

    if (NivelPerfil == "") {
        throw new Error(['var NivelPerfil is empty, that\'s required for tabs module']);
    }

    // Dialog
    $('.xparametrosInfo_Permisos').dialog({
        autoOpen: false,
        width: 370,
        modal: true,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        },
        hide: 'fold'
    });

    $('.dialog_link').click(function (evento) {
        evento.preventDefault();
        $('.xparametrosInfo_Permisos').dialog('open');
        return false;
    });

    getModulesByProfile(NivelPerfil)
});

function getModulesByProfile(profile) {
    jQuery.ajax({
        type: "POST",
        url: baseUrl + '/Security/webrequest/seguridad.aspx/getModules',
        data: "{prmIntPerfilId: '" + profile + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var modules = $.parseJSON(data.d);
            proccessModules(modules)
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function proccessModules(modules) {
    jQuery.each(modules, function(k, module){
        var _parentDiv = jQuery('<div/>', {
            class: 'col-sm-3'
        });

        var _link = jQuery('<a/>', {
            id: module.pk_codigo,
            title: module.val_nombre,
            href: baseUrl + module.val_url
        })

        var _img = jQuery('<img/>', {
            alt: module.val_nombre,
            title: module.val_nombre,
            src: baseUrl + module.val_url_icono
        })

        var _label = jQuery('<label/>', {
            text: module.val_nombre,
            for: "#" + module.pk_codigo
        })

        _link.append(_img)
        _parentDiv.append(_link)
        _parentDiv.append(_label)
        $(".modules-content").prepend(_parentDiv)
    });
}

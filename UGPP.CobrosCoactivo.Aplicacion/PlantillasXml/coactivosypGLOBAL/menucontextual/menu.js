//Cuando el documento esté listo...
$(document).ready(function() {
    //variables de control
    var menuId = "menu_contextual";
    var menu = $("#" + menuId);

    //EVITAMOS que se muestre el MENU CONTEXTUAL del sistema operativo al hacer CLICK con el BOTON DERECHO del RATON
    $(document).bind("contextmenu", function(e) {
        menu.css({ 'display': 'block', 'left': e.pageX, 'top': e.pageY });
        menu.hide().fadeIn();
        return false;
    });

    //controlamos ocultado de menu cuando esta activo
    //click boton principal raton
    $(document).click(function(e) {
        if (e.button == 0) {
            menu.fadeOut();
        }
    });
    //pulsacion tecla escape
    $(document).keydown(function(e) {
    if (e.keyCode == 27) {
            menu.fadeOut();
        }
    });

    //Control sobre las opciones del menu contextual
    menu.click(function(e) {
        //si la opcion esta desactivado, no pasa nada
        if (e.target.className == "disabled") {
            return false;
        }
        //si esta activada, gestionamos cada una y sus acciones
        else {
            switch (e.target.id) {
                case "menu_anterior":
                    history.back(-1);
                    break;
                case "menu_siguiente":
                    alert("ha seleccionado siguiente");
                    break;
                case "menu_recargar":
                    document.location.reload();
                    break;
                case "menu_favoritos":
                    var title = "Tutoriales de Desarrollo y Diseño Web | Web.Ontuts";
                    var url = "http://web.ontuts.com";
                    //para firefox
                    if (window.sidebar)
                        window.sidebar.addPanel(title, url, "");
                    //para Internet Explorer
                    else if (window.external)
                        window.external.AddFavorite(url, title);
                    break;
            }
            menu.css("display", "none");
        }
    });

});
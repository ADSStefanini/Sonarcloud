jQuery(document).ready(function($) {
    $('#ssherramienta2').bt({
    showTip: function(box) {
        $(box).fadeIn(500);
    },
    hideTip: function(box, callback) {
        $(box).animate({ opacity: 0 }, 500, callback);
    },

    shrinkToFit: true,
    hoverIntentOpts: {
        interval: 0,
        timeout: 0
    }
    });
    $('#buscaexpediente').bt({
        showTip: function(box) {
            $(box).fadeIn(500);
        },
        hideTip: function(box, callback) {
            $(box).animate({ opacity: 0 }, 500, callback);
        },

        shrinkToFit: true,
        hoverIntentOpts: {
            interval: 0,
            timeout: 0
        }
    });
});
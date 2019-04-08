$(function () {
    EndRequestHandler();
});
function EndRequestHandler() {

    $(".PCG-Content tr:gt(0)").mouseover(function () {
        $(this).addClass("ui-state-highlight");
    });

    $(".PCG-Content tr:gt(0)").mouseout(function () {
        $(this).removeClass("ui-state-highlight");
    });
}
function MostrarOcultarDiv() {
    var div = document.getElementById("divAgregar");
    if (div.style.display == "none")
        div.style.display = "block";
    else
        div.style.display = "none";
}
function MostrasFechas(op) {
    if (op == 4)
        $("#Fechas").visible = true;
    else
        $("#Fechas").visible = false;
}
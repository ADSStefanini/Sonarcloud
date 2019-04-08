// Funcion para sumar dias a una fecha ------------------------- //
function SumarDiasFecha(FechaInicialSTR, NumeroDias) {

    if (FechaInicialSTR == "" || NumeroDias == "") {
        if (document.getElementById("txtFecEstiFin").value == "") {
            return "";
        } else {
            return document.getElementById("txtFecEstiFin").value;
        }
    }

    // Forzar el numero de dias como tipo entero
    NumeroDias = parseInt(NumeroDias, 10);

    // Convierto la cadena de fecha en un objeto fecha
    var parts = FechaInicialSTR.split("/");
    var FechaFinal = new Date(parts[2], parts[1] - 1, parts[0]);

    // Sumo los días de la politica del persuasivo
    FechaFinal.setMonth(FechaFinal.getMonth() + NumeroDias);

    var FechaFinalSTR = ConvertirFechaEnCadena(FechaFinal);

    //return FechaFinal;
    //alert(FechaFinalSTR);
    return FechaFinalSTR;
}
// ------------------------------------------------------------- //

function ConvertirFechaEnCadena(objFecha) {
    var yyyy = objFecha.getFullYear().toString();
    var mm = (objFecha.getMonth() + 1).toString(); // getMonth() is zero-based
    var dd = objFecha.getDate().toString();
    // return yyyy + "-" + (mm[1]?mm:"0"+mm[0]) + "-" + (dd[1]?dd:"0"+dd[0]);
    return (dd[1] ? dd : "0" + dd[0]) + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + yyyy;
}


function SumarLiqCredito() {
    // Si esta vacio el capital => colocar cero
    if (document.getElementById('txtLiqCredCapital').value == '') {
        document.getElementById('txtLiqCredCapital').value = 0
    }
    var txtLiqCredCapital = parseInt(document.getElementById('txtLiqCredCapital').value);

    // Si esta vacio el interes => colocar cero
    if (document.getElementById('txtLiqCredInteres').value == '') {
        document.getElementById('txtLiqCredInteres').value = 0
    }
    var txtLiqCredInteres = parseInt(document.getElementById('txtLiqCredInteres').value);

    // Total Liquidacion de Credito
    document.getElementById('txtLiqCredTotal').value = txtLiqCredCapital + txtLiqCredInteres;
}

function reloadParentIframe() {
    console.log("reloaded")
    window.parent.location.reload();
}
function createXMLHttpRequest() {
    try { return new XMLHttpRequest(); } catch(e) {}
    try { return new ActiveXObject("Msxml2.XMLHTTP"); } catch (e) {}
    try { return new ActiveXObject("Microsoft.XMLHTTP"); } catch (e) {}
    alert("XMLHttpRequest not supported");
    return null;
}

function PermisosUsuarios(OpcionMenu){
    try { 
        var xmlHttpReq= createXMLHttpRequest();
        xmlHttpReq.open("GET", "Servicios/PermisosUser.aspx?v1=" + OpcionMenu, false);
        xmlHttpReq.send(null);


        cadenaTexto = xmlHttpReq.responseText;
        fragmentoTexto = cadenaTexto.split('|');
        return fragmentoTexto;
    }
    catch (err) {
        txt = "Existe un error en esta página.\n\n";
        txt += "Descripcion del Error : " + err.description + "\n\n";
        txt += "Presione ok o continue.\n\n";
        alert(txt);
    } 
}
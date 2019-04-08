<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="predialvariable032Report_in.aspx.vb" Inherits="coactivosyp.predialvariable032Report_in" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="css/screen.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
              <div id="container">
                    <div id="wrap" class="wrap">	
                            <div style="text-align:center;position:relative;">
                                <div style=" position:absolute;right:5px;top:10px;">Expediente. <b><%=ViewState("NUMERO_EXPEDIENTE")%></b></div>
                                <img alt="Logo" src="imagen.ashx?ImageFileName=<%= Session("mcobrador") %>&tipo=Logo" width="340" height="115" style="width:340px; height:115px;" /><br /><br />
                                <b>ALCALDIA DE BUCARAMANGA</b><br />
                                <b>SECRETARIA DE HACIENDA MUNICIPAL</b><br />
                                <b>TESORERIA MUNICIPAL</b> <br />
                                <b>RESOLUCIÓN No. <%=ViewState("NUMERO_EXPEDIENTE")%></b><br />
                            </div>
                            <br />
                            <br />
                            <br />
                            <b><i><%=ViewState("FECHA_DE_ACTO")%></i></b><br />
                            <br />
                            <br />
                            <div style="text-align:center;"><b><i>“POR MEDIO DE LA CUAL SE RESUELVE UNA EXCEPCIÓN DENTRO DEL PROCESO ADMINISTRATIVO DE COBRO COACTIVO”</i></b></div><br />
                            <br />
                            El Tesorero General del Municipio de Bucaramanga, en ejercicio de la facultades legales dispuestas en el Decreto municipal No. 0050 del 31 marzo 2006, Decreto municipal No. 0232 del 7 Noviembre 2008, además de la potestad contemplada en el numeral 6, literal ‘D’, del Artículo No. 91 de la Ley 136 de 1994 y los atributos reglamentarios otorgados a través de los Artículos No. 441 y 450 del Acuerdo Municipal 044 de 22 de Diciembre de 2008 y el inciso 2 del Artículo 817 E.T.N., procede a efectuar el estudio a excepción propuesta por <b><%=ViewState("EfiNom")%></b>, en representación del establecimiento comercial identificado con el registro de industria y comercio No. <b><%=ViewState("NUMERO_EXPEDIENTE")%></b> dentro del proceso administrativo de cobro coactivo adelantado por concepto de impuesto de industria y comercio, su complementario de avisos y tableros, sobretasa bomberil, gastos de sistematización y Sanciones, teniendo en cuenta los siguientes: <br />
                            <br />
                            <br />
                            <br />
                            <div style="text-align:center;"><b>HECHOS: </b></div><br />
                            <%=ViewState("HECHOS")%><br />
                            <br />
                            Las anteriores situaciones fácticas recopiladas&nbsp; constituyen&nbsp; el norte que fundamentan el presente estudio y por lo anterior este Despacho argumenta su decisión basándose &nbsp;en las siguientes:<br />
                            <br />
                            <br />
                            <br />
                            <div style="text-align:center;"><b>CONSIDERACIONES: </b></div><br />
                            <%=ViewState("CONSIDERACIONES")%>
                            <br />
                            <br />
                            <div style="text-align:center;"><b>RESUELVE:</b></div><br /> 
                            <br />
                            <br />
                            <%=ViewState("ARTICULO_VARIABLE")%>
                            <br />
                            <br />
                            <br />
                            <b>Notifíquese y cúmplase,</b><br />
                            <br />
                            <img alt="Firma" src ="imagen.ashx?ImageFileName=<%= Session("mcobrador")%>&tipo=Firma" id="ImgFirma" width="174" height="123" style="width:174px;height:123px;" />
                            <br />
                            <b>RICARDO ORDOÑEZ RODRIGUEZ</b><br />
                            Tesorero General Bucaramanga<br />
                    </div>
                </div>
    </form>
</body>
</html>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="expedinteexaminar.aspx.vb" Inherits="coactivosyp.expedinteexaminar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Expediente</title>
    <style type="text/css">
        .tabla {font-family: Verdana, Arial, Helvetica, sans-serif;font-size:11px;text-align: left;font-family: "Trebuchet MS", Arial;text-transform: uppercase;background-color: #EDEDE9;margin:0px;width:100%;}
        .tabla th {padding: 2px;background-color: #e3e2e2;color: #000000;border-right-width: 1px;border-bottom-width: 1px;border-right-style: solid;border-bottom-style: solid;border-right-color: #6E6E6E;border-bottom-color: #6E6E6E;font-weight:bold;text-align:left;width:200px;}
        .tabla td {padding: 2px;border-right-width: 1px;border-bottom-width: 1px;border-right-style: solid;border-bottom-style: solid;border-right-color: #D8D8D8;border-bottom-color: #D8D8D8; background-color: #EDEDE9;color: #34484E;}
        .dft {font-family:Verdana;font-size:13px;background-color: #EDEDE9;padding: 5px;width:600px !important;display: block !important; border-right-width: 1px;border-bottom-width: 1px; border-right-style: solid;border-bottom-style: solid;border-right-color: #D8D8D8;border-bottom-color: #D8D8D8;}
        .dteall {text-align:left;}
        .dteall, .dteall td, .dteall th {border:1px solid black;}        
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <div style="margin-left: auto;margin-right: auto; text-align:left;" id="datosinf" runat="server">
        <table class="tabla">
            <tr><th colspan="4" style="text-align: center;">Examen completo del expediente </th></tr>
            <tr>
                <th>ENTIDAD : </th><td colspan="3"><% Response.Write("(ID " & Me.ViewState("entidad") & ") - " & Me.ViewState("nombre"))%></td>
            </tr>
            <tr>
                <th>EXPEDIENTE : </th><td><% Response.Write(Me.ViewState("docexpediente"))%></td>
                <th>NRO DE ACTOS EJECUTADOS : </th>
                <td style="text-align:center;"><%=Me.ViewState("nroeje")%></td>
            </tr>
            <tr><th colspan="4" style="text-align: center;"> ACTOS ADMINISTRATIVOS </th></tr>
        </table> 
         
         <asp:Repeater ID="Repeater1" runat="server">
             <HeaderTemplate>
                    
             </HeaderTemplate>
            <SeparatorTemplate>
            <table class="tabla">
             <tr>
                <td><b> &nbsp; </b> <br /> </td>
             </tr>
             </table>  
            </SeparatorTemplate>
            <ItemTemplate>
                <table class="tabla">
                    <tr>
                        <th>ACTUACION : </th><td><%#Eval("idacto") & " - " & Eval("nombreacto")%></td>
                    </tr>
                    <tr>
                        <th>NOMBRE DEL ARCHIVO : </th>
                        <td>
                            <a title ="Imagen que corresponde al acto administrativo (Paso) escaneado." href="javascript:;"  onclick="window.open('../TiffViewer.aspx?nomente=<%#Eval("nombre")%>&idente=<%#Eval("idacto")%>&F=<%#Eval("nomarchivo")%>&totimg=<%#Eval("paginas")%>&acto=<%#Eval("nombreacto")%>&idacto=<%#Eval("idacto")%>&folder=&Enabled=false&observacion=<%#Eval("docObservaciones")%>&vsExpedienteAcu=<%= Request("vsExpedienteAcu")%>', 'Expediente', 'fullscreen=yes, scrollbars=auto')"> 
                                <%#Eval("nomarchivo")%>
                            </a>  
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <span style="font-style:italic;color:#9aa4a7;">Hacer clic para observar el documento</span> 
                        </td>
                    </tr>
                    <tr>
                        <th>NUMERO DE PAGINA(S) : </th><td><%#Eval("paginas")%></td>
                    </tr>
                    <tr>
                        <th>FECHA RADICACIÓN : </th><td><%#Eval("fecharadic")%></td>
                    </tr>
                    <tr>
                        <th>FECHA CREACIÓN : </th><td><%#Eval("docfechadoc")%></td>
                    </tr>
                </table>
            </ItemTemplate>
            <FooterTemplate>
                <table class="tabla">
                  <tr><th colspan="2" style="text-align: center;">Examen completo del expediente </th></tr>
                 </table> 
            </FooterTemplate>
          </asp:Repeater>
        </div>
    </form>
  
</body>
</html>

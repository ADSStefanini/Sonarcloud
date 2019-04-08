<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="iyc_infogral.aspx.vb" Inherits="coactivosyp.iyc_infogral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Tecno Expedientes !</title>
    <link href="EstiloPPal.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="images/icons/web_page.ico" />
    <script src="js/jquery-1.4.2.min.js" type="text/javascript"></script>
</head>
<body>
    <!-- Definicion del menu -->  
    <div id="message_box">
        <ul>
         <li style="height:36px;width:36px;">
            <a href="menu.aspx"><img alt="" src="imagenes/MeSesion.png" style="height:36px;width:36px;" /></a>   
         </li>
         <li style="height:152px;width:36px;">
            <a href="menu4.aspx"><img alt="" src="imagenes/blsidebarimg.png" style="height:152px;width:36px;" /></a>
         </li>
        </ul>
    </div>
    
    <div id="container">
        <h1 id="Titulo"><a href="#">Industria y Comercio - Información general de matrículas</a></h1>
        <form id="form1" runat="server" style=" margin-top:-30px;">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div id="Buscador" style="width:760px; height:396px;" >
                
                <div id="Label1" style="position:absolute;top:52px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Placa </div>
                <asp:TextBox ID="txtIdPlaca" runat="server" 
                    style="position:absolute;top:50px; left:116px; width:50px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label2" style="position:absolute;top:52px; left:204px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nit-Cédula</div>
                <asp:TextBox ID="txtCedula" runat="server" 
                    style="position:absolute;top:50px; left:286px; width:150px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label3" style="position:absolute;top:52px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nro. Predial</div>
                <asp:TextBox ID="txtNumPredial" runat="server" 
                    style="position:absolute;top:50px; left:558px; width:120px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                
                <div id="Label4" style="position:absolute;top:83px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Nombre </div>
                <asp:TextBox ID="txtNombre" runat="server" 
                    style="position:absolute;top:80px; left:116px; width:320px;z-index:777;" ></asp:TextBox>                    
                <div id="Label5" style="position:absolute;top:83px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Estado</div>
                <asp:TextBox ID="txtEstado" runat="server" 
                    style="position:absolute;top:80px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>                
                
                <div id="Label6" style="position:absolute;top:113px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Dirección </div>
                <asp:TextBox ID="txtDireccion" runat="server" 
                    style="position:absolute;top:110px; left:116px; width:320px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div1" style="position:absolute;top:146px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fec. Ini Act.</div>
                <asp:TextBox ID="txtFecIniAct" runat="server" 
                    style="position:absolute;top:143px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div2" style="position:absolute;top:146px; left:214px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Barrio</div>
                <asp:TextBox ID="txtBarrio" runat="server" 
                    style="position:absolute;top:143px; left:256px; width:60px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div3" style="position:absolute;top:146px; left:338px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Zona</div>
                    <asp:TextBox ID="txtZona" runat="server" 
                    style="position:absolute;top:143px; left:376px; width:60px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div4" style="position:absolute;top:146px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fec-Renovación</div>
                <asp:TextBox ID="txtFecRenovacion" runat="server" 
                    style="position:absolute;top:143px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div5" style="position:absolute;top:176px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fec-Canc</div>
                <asp:TextBox ID="txtFecCanc" runat="server" 
                    style="position:absolute;top:173px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div6" style="position:absolute;top:176px; left:214px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Resolución Cancelac.</div>
                <asp:TextBox ID="txtResolCan" runat="server" 
                    style="position:absolute;top:173px; left:356px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div7" style="position:absolute;top:176px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Fec Cámara Com</div>
                <asp:TextBox ID="txtFecCamCom" runat="server" 
                    style="position:absolute;top:173px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div8" style="position:absolute;top:206px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">FecUlt Pago</div>
                <asp:TextBox ID="txtFecUltPag" runat="server" 
                    style="position:absolute;top:203px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div9" style="position:absolute;top:206px; left:214px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Recibo últ.pago</div>
                <asp:TextBox ID="txtRecUltPag" runat="server" 
                    style="position:absolute;top:203px; left:318px; width:118px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div10" style="position:absolute;top:206px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Valor últ.pago</div>
                <asp:TextBox ID="txtValUltPag" runat="server" 
                    style="position:absolute;top:203px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div11" style="position:absolute;top:236px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Año hasta</div>
                <asp:TextBox ID="txtAnoHas" runat="server" 
                    style="position:absolute;top:233px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div12" style="position:absolute;top:236px; left:214px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Periodo hasta</div>
                <asp:TextBox ID="txtPerHas" runat="server" 
                    style="position:absolute;top:233px; left:418px; width:18px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div13" style="position:absolute;top:236px; left:454px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Teléfono</div>
                <asp:TextBox ID="txtTel" runat="server" 
                    style="position:absolute;top:233px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div14" style="position:absolute;top:266px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Horario</div>
                <asp:TextBox ID="txtHorario" runat="server" 
                    style="position:absolute;top:263px; left:116px; width:562px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div15" style="position:absolute;top:296px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Acuer. pago</div>
                <asp:TextBox ID="txtAcuPag" runat="server" 
                    style="position:absolute;top:293px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div16" style="position:absolute;top:296px; left:214px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Emplaz por no declarar</div>
                <asp:TextBox ID="txtEmpNoDec" runat="server" 
                    style="position:absolute;top:293px; left:368px; width:98px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div17" style="position:absolute;top:296px; left:470px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Emplaz corregir</div>
                <asp:TextBox ID="txtEmpCor" runat="server" 
                    style="position:absolute;top:293px; left:578px; width:100px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div18" style="position:absolute;top:326px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Act Bas</div>
                <asp:TextBox ID="txtActBas" runat="server" 
                    style="position:absolute;top:323px; left:116px; width:80px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
                <div id="Div19" style="position:absolute;top:356px; left:34px;color:#fff; font-family:Arial; font-size:15px; font-weight:normal;">Descrip. Actividad</div>
                <asp:TextBox ID="txtDescrip" runat="server" 
                    style="position:absolute;top:353px; left:156px; width:522px;z-index:777;" 
                    ReadOnly="True" ></asp:TextBox>
            </div>            
        </form>
    </div>
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPAGOS.aspx.vb" Inherits="coactivosyp.EditPAGOS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar pagos
        </title>
                
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />        
        
        <script type="text/javascript">
            $(function() {
                
                $('#cmdSave').button();
                $('#cmdCancel').button();
                $('#cmdSolicitudCambioEstado').button();

                //Array para dar formato en español
                $.datepicker.regional['es'] =
                {                    
                    closeText: 'Cerrar',
                    prevText: 'Previo',
                    nextText: 'Próximo',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    monthStatus: 'Ver otro mes', yearStatus: 'Ver otro año',
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sáb'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    dateFormat: 'dd/mm/yy', firstDay: 1,
                    initStatus: 'Seleccione la fecha', isRTL: false
                };
                $.datepicker.setDefaults($.datepicker.regional['es']);

                //------------------------------------------------------------------//
                //Ocultar todos los DatePicker si el estado del proceso es devuelto o terminado
                var PerfilUser = '<% Response.write(Session("mnivelacces")) %>';
                // alert(PerfilUser);

                if (PerfilUser == '6') {
                    //
                } else {
                    $.datepicker.datepicker('disable');
                }

                //------------------------------------------------------------------//
                $("#txtFecSolverif").keypress(function(event) { event.preventDefault(); });
                $("#txtFecSolverif").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecSolverif').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                //------------------------------------------------------------------//
                $("#txtFecVerificado").keypress(function(event) { event.preventDefault(); });
                $("#txtFecVerificado").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecVerificado').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtpagFecha").keypress(function(event) { event.preventDefault(); });
                $("#txtpagFecha").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtpagFecha').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtpagFechaDeudor").keypress(function(event) { event.preventDefault(); });
                $("#txtpagFechaDeudor").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtpagFechaDeudor').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtpagFecExi").keypress(function(event) { event.preventDefault(); });
                $("#txtpagFecExi").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtpagFecExi').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

            });
        </script>
        <style type="text/css">
	        body{ background-color:#01557C;}		
		    * { font-size:12px; font-family:Arial;}
            td { padding:2px;} 	
            #encabezado { background:url(images/resultados_busca.jpg); height:37px; width:100%; border:solid 1px #a6c9e2; padding-bottom:5px}  
			#tituloencabezado { color:White; margin-top:16px; margin-left:2px; font: 12px Verdana; font-weight:bold; }  
			#infoexpediente { background-color:#FFFFFF; width:100%; margin-bottom:10px; margin-top:10px; font-family: Lucida Grande,Lucida Sans,Arial,sans-serif; font-weight: bold; font-size: 11px;  -moz-border-radius: 3px; border-radius: 3px; -webkit-border-radius: 3px;  }  
		    
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            
            <!---------------------- -->
             <div id="encabezado">
                <div id="tituloencabezado">
					<%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>                        
                    </div>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                    </div>
                    
				</div>                
            </div>
            <!---------------------- -->
            
            <div id="infoexpediente" style="padding-top:8px; padding-bottom:8px; width:100%; display:inline-table ">
                <div style="color:#2e6e9e; width:92px; float:left; line-height:26px; padding-left:8px; height:28px; ">
                    No. Expediente:
                </div>
                <div style="width:90px; float:left;height:28px;">
                    <asp:TextBox id="txtNroExpEnc" runat="server" Columns="8" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style="color:#2e6e9e; width:50px; float:left; line-height:26px;height:28px;">
                    Deudor:
                </div>
                <div style="width:105px; float:left;height:28px;">
                    <asp:TextBox id="txtIdDeudor" runat="server" Columns="12" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                <div style="width:470px; float:left;height:28px;">
                    <asp:TextBox id="txtNombreDeudor" runat="server" Columns="70" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style="color:#2e6e9e; width:60px; float:left; line-height:26px;height:28px;">
                    Estado:
                </div>
                <div style="width:170px; float:left;height:28px; ">
                    <asp:TextBox id="txtNombreEstado" runat="server"  Columns="22" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style="color:#2e6e9e; width:140px; float:left; line-height:26px; padding-left:8px;height:28px;">
                    Número Título Ejecutivo
                </div>
                <div style="width:100px; float:left;height:28px; ">
                    <asp:TextBox id="txtNUMTITULOEJECUTIVO" runat="server"  Columns="11" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style="color:#2e6e9e; width:80px; height:28px; float:left; line-height:26px; padding-left:8px;">
                    Fecha Título
                </div>
                <div style="width:100px; height:28px; float:left ">
                    <asp:TextBox id="txtFECTITULO" runat="server"  Columns="12" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style="color:#2e6e9e; width:60px; height:28px; float:left; line-height:26px; padding-left:8px;">
                    Tipo Título
                </div>
                <div style="width:210px; height:28px; float:left; ">
                    <asp:TextBox id="txtTIPOTITULO" runat="server"  Columns="28" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                </div>
                
                <div style=" width:240px; float:left; height:28px; ">
                    <div style="color:#2e6e9e; width:140px; line-height:26px; float:left;">
                        Fecha entrega al gestor
                    </div>
                    <div style="width:100px; float:left; ">
                        <asp:TextBox id="txtFECENTREGAGESTOR" runat="server"  Columns="12" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
                
                <!-- Total deuda -->
                <div style="width:174px; float:left; height:28px; padding-left:8px; ">
                    <div style="color:#2e6e9e; width:72px; line-height:26px; float:left;">
                        Total deuda:
                    </div>
                    <div style="width:100px; float:left; ">
                        <asp:TextBox id="txtTotalDeudaEA" runat="server" Columns="13" CssClass="ui-widget" ForeColor="Red" ReadOnly="True" style="text-align:right"></asp:TextBox>
                    </div>
                </div>
                
                <!-- Pagos capital -->
                <div style="width:200px; float:left; height:28px; padding-left:8px; ">
                    <div style="color:#2e6e9e; width:90px; line-height:26px; float:left;">
                        Pagos capital:
                    </div>
                    <div style="width:110px; float:left; ">
                        <asp:TextBox id="txtPagosCapitalEA" runat="server" Columns="13" CssClass="ui-widget" ForeColor="Red" ReadOnly="True" style="text-align:right"></asp:TextBox>
                    </div>
                </div>
                
                <!-- Saldo actual -->
                <div style="width:200px; float:left; height:28px; padding-left:8px; ">
                    <div style="color:#2e6e9e; width:90px; line-height:26px; float:left;">
                        Saldo actual:
                    </div>
                    <div style="width:110px; float:left; ">
                        <asp:TextBox id="txtSaldoEA" runat="server" Columns="13" CssClass="ui-widget" ForeColor="Red" ReadOnly="True" style="text-align:right"></asp:TextBox>
                    </div>
                </div>
                
                <!-- Gestor responsable -->
                <div style="width:380px; float:left; height:28px; padding-left:8px; ">
                    <div style="color:#2e6e9e; width:140px; line-height:26px; float:left;">
                        Gestor responsable:
                    </div>
                    <div style="width:150px; float:left; ">
                        <asp:TextBox id="txtGestorResp" runat="server" Columns="35" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
                
                <!-- Boton de cambio de estado -->
                <div style="width:380px; float:left; height:28px; padding-left:8px; ">
                    <div style="color:#2e6e9e; width:140px; line-height:26px; float:left;">
                        <asp:Button ID="cmdSolicitudCambioEstado" runat="server" Text="Solicitud de cambio de estado" />
                    </div>                    
                </div>
                               
            </div> 
            
            <table id="tblEditPAGOS" class="ui-widget-content">
                    <%--<tr>
                        <td colspan="10" background="images/resultados_busca.jpg" height="42">                            
                            <div style="color:White; font-weight:bold; width:460px; height:20px; float:left"><%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span></div>
                            <div style="color:White; width:340px; height:20px; float:right; text-align:right">
                                <asp:LinkButton ID="A3" runat="server"><img alt ="" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" /></asp:LinkButton>
                                <span>Cerrar sesión</span>
                            </div>
                        </td>
                    </tr>--%>
                    <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Consignación / planilla
                    </td>
                    <td>
                        <asp:TextBox id="txtNroConsignacion" runat="server" MaxLength="50" CssClass="ui-widget" ForeColor="Red" style=" width: 300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. Expediente
                    </td>
                    <td>
                        <asp:TextBox id="txtNroExp" runat="server" MaxLength="12" CssClass="ui-widget" ForeColor="Red" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha reporte del pago por gestor responsable
                    </td>
                    <td>
                        <asp:TextBox id="txtFecSolverif" runat="server" CssClass="ui-widget" MaxLength="10" ForeColor="Red"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de verificación del pago
                    </td>
                    <td>
                        <asp:TextBox id="txtFecVerificado" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado del pago
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cboestado" runat="server"></asp:DropDownList>                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de pago 
                    </td>
                    <td>
                        <asp:TextBox id="txtpagFecha" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de reporte del pago por el deudor
                    </td>
                    <td>
                        <asp:TextBox id="txtpagFechaDeudor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No.Título Judicial
                    </td>
                    <td>
                        <asp:TextBox id="txtpagNroTitJudicial" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Capital pagado
                    </td>
                    <td>
                        <asp:TextBox id="txtpagCapital" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Ajuste Decreto 1406
                    </td>
                    <td>
                        <asp:TextBox id="txtpagAjusteDec1406" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Intereses pagados
                    </td>
                    <td>
                        <asp:TextBox id="txtpagInteres" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Gastos del proceso
                    </td>
                    <td>
                        <asp:TextBox id="txtpagGastosProc" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Pagos en exceso
                    </td>
                    <td>
                        <asp:TextBox id="txtpagExceso" runat="server" CssClass="ui-widget"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Total pagado
                    </td>
                    <td>
                        <asp:TextBox id="txtpagTotal" runat="server" CssClass="ui-widget" 
                            ReadOnly="True"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" 
                            AlternateText="Actualizar total pagado" 
                            ImageUrl="~/Security/images/icons/turn_left.png" 
                            ToolTip="Actualizar total pagado" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Estado del proceso en la fecha de reporte del pago
                    </td>
                    <td>
                        <asp:DropDownList CssClass="ui-widget" id="cbopagestadoprocfrp" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        Datos de acuerdos / facilidades de pago
                    </td>                    
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Fecha de exigibilidad
                    </td>
                    <td>
                        <asp:TextBox id="txtpagFecExi" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Tasa de intereses de mora aplicable
                    </td>
                    <td>
                        <asp:TextBox id="txtpagTasaIntApl" runat="server" CssClass="ui-widget" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Días de mora 
                    </td>
                    <td>
                        <asp:TextBox id="txtpagdiasmora" runat="server" CssClass="ui-widget" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        Valor Cuota
                    </td>
                    <td>
                        <asp:TextBox id="txtpagvalcuota" runat="server" CssClass="ui-widget" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="ui-widget-header">
                        No. confirmación pago
                    </td>
                    <td>
                        <asp:TextBox id="txtpagNumConPag" runat="server" CssClass="ui-widget" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                        <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>


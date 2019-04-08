<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditEJEFISGLOBALREPARTIDOR.aspx.vb" Inherits="coactivosyp.EditEJEFISGLOBALREPARTIDOR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Edición de asignación de procesos
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            $(function() {
                //window.scrollTo(0, 0);
                //scrollTop: '0px';
                //$("html, body").animate({ scrollTop: 0 }, 600);

                $('#cmdSave').button();
                $('#cmdSave2').button();
                $('#cmdCancel').button();
                $('#cmdMostrarEstadisticas').button();
                $('#btnObtenerEstado').button();
                $('#cmdRefrescar').button();
                $('#cmdGestionar').button();
                $('.GridEditButton').button();

                // Ocultar tabs si se va a crear un expediente nuevo

                var pNumExpediente = '<% Response.Write(Request("ID")) %>';
                if (pNumExpediente == "") {
                    // alert("param vacio");
                    // Ocultar TAB
                    $("#tabs").tabs({ disabled: [1, 2, 3, 4, 5] }); //when initializing the tabs
                    $("#tabs").tabs("option", "disabled", [1, 2, 3, 4, 5]); // or setting after init
                }

                //Aceptar solo numeros
                $(".numeros").keydown(function(event) {
                    // Allow: backspace, delete, tab, escape, and enter
                    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                    // Allow: Ctrl+A
                        (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
                        (event.keyCode >= 35 && event.keyCode <= 39)) {
                        if (this.value == '') { this.value = 0; } // 26/ene/2014: Si deja la entrada en blanco=>poner cero
                        // let it happen, don't do anything
                        return;
                    } else {
                        // Ensure that it is a number and stop the keypress
                        if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                            event.preventDefault();
                        }
                    }
                });

                $('#cmdMostrarEstadisticas').click(function() {
                    window.open('EstadisticaxExpediente1.aspx', 'Estadisticas', 'width=400,height=250');
                    return false;
                });

                //Boton Refrescar
                $('#cmdRefrescar').click(function() {
                    location.reload();
                });
                //Al hacer click en #tabs1 actualizar los datos 
                $('#lblTab1').click(function() {
                    var valorExpediente = $("#txtEFINROEXP").val();
                    var url = document.URL;
                    if (url.indexOf('?') != -1) {
                        setEstadoExpediente(valorExpediente);
                    }
                    //setEstadoExpediente(valorExpediente);
                });
                // Funcion para actualizar el estado del expediente
                function setEstadoExpediente(pExpediente) {
                    var parametros = {
                        "pExpediente": pExpediente
                    };
                    //alert(pExpediente);                   
                    $.ajax({
                        data: parametros,
                        url: 'AjaxGetEstadoProceso.aspx',
                        type: 'post',
                        success: function(response) {
                            $("#cboEFIESTADO").val(response);
                            //alert(response);
                        }
                    });
                }


                //Manejo de tabs
                $("#tabs").tabs();

                //Cuando seseleccione un gestor => Mostrar el nombre de su revisor
                $("#cboEFIUSUASIG").change(function() {
                    var valorcboEFIUSUASIG = $("#cboEFIUSUASIG").val();
                    setNombreRevisor(valorcboEFIUSUASIG);
                });
                function setNombreRevisor(pIdGestor) {
                    var parametros = {
                        "pIdGestor": pIdGestor
                    };
                    //alert(pIdGestor);
                    //$("#txtRevisor").val(pIdGestor);

                    $.ajax({
                        data: parametros,
                        url: 'obtenerNombreRevisor.aspx',
                        type: 'post',
                        success: function(response) {
                            $("#txtRevisor").val(response);
                        }
                    });
                }
                // -------------------------------------------------------------------

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


                $("#txtEFIFECHAEXP").keypress(function(event) { event.preventDefault(); });
                $("#txtEFIFECHAEXP").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtEFIFECHAEXP').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: new Date,
                    minDate: new Date(2007, 6, 12),
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtEFIFECENTGES").keypress(function(event) { event.preventDefault(); });
                $("#txtEFIFECENTGES").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtEFIFECENTGES').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: new Date,
                    minDate: new Date(2007, 6, 12),
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtEFIFECCAD").keypress(function(event) { event.preventDefault(); });
                $("#txtEFIFECCAD").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtEFIFECCAD').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: new Date,
                    minDate: new Date(2007, 6, 12),
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            });
            /* mantener tab actual nivel 1 */
            $(function() {
                //  Atención: Esto funciona para jQueryUI 1.10 y HTML5.
                //  Define friendly index name
                var index = 'valornivel1';
                //  Define friendly data store name
                var dataStore = window.sessionStorage;
                //  Start magic!
                try {
                    // getter: Fetch previous value
                    var oldIndex = dataStore.getItem(index);
                } catch (e) {
                    // getter: Always default to first tab in error state
                    var oldIndex = 0;
                }
                $('#tabs').tabs({
                    // The zero-based index of the panel that is active (open)
                    active: oldIndex,
                    // Triggered after a tab has been activated
                    activate: function(event, ui) {
                        //  Get future value
                        var newIndex = ui.newTab.parent().children().index(ui.newTab);
                        //  Set future value
                        dataStore.setItem(index, newIndex)
                    }
                });
            });
        </script>
          
        
        <style type="text/css">
		    body{ background-color:#01557C;}		
		     * { font-size:12px; font-family:Arial; }
		    th { padding-left:8px; padding-right:8px;}
            td { padding:2px;}  
            .add_edit { width: 20px; height: 16px;  float:none; }
            #encabezado { background:url(images/resultados_busca.jpg); height:37px; width:100%; border:solid 1px #a6c9e2; padding-bottom:5px}  
			#tituloencabezado { color:White; margin-top:16px; margin-left:2px; font: 12px Verdana; font-weight:bold; } 
			.numeros { text-align:right; }			 
	    </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <a id="lnkArriba"></a>
            <div id="encabezado">
                <div id="tituloencabezado">
					<%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%> <span style="font-weight:normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server" Text="Label"></asp:Label>)</span>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión"><img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>                        
                    </div>
					<div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="ABack" runat="server" ToolTip="Regresar al listado de expedientes"><img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>                        
                    </div>
                    
                    <div style="color:White; width:30px; height:20px; float:right; text-align:right; padding-right:8px;">
                        <asp:LinkButton ID="AShowExp" runat="server" ToolTip="Examinar expediente completo"><img alt ="Examinar expediente completo"  src="../images/icons/documento1.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Examinar expediente completo" /></asp:LinkButton>                        
                    </div>
				</div>
            </div>
            
        
            <!-- Contenido -->
            <div id="tabs">
                
              <ul>
                <li><a href="#tabs1"><label id="lblTab1">Creación</label></a></li>
                <li><a href="#tabs4">Clasificación de cartera</a></li>
                <li><a href="#tabs2">Título ejecutivo</a></li>
                <li><a href="#tabs3">Deudores</a></li>
                <li><a href="#tabs12">Reparto</a></li>
                <li><a href="#tabs6">Solicitudes cambio estado</a></li>
              </ul>
              
              <div id="tabs1">
                    <table id="tblEditEJEFISGLOBAL" class="ui-widget-content">		            
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                No. expediente cobranzas * 
                            </td>
                            <td>
                                <asp:TextBox id="txtEFINROEXP" runat="server" MaxLength="12" CssClass="ui-widget" ReadOnly="true" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Fecha recepción título ejecutivo *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFIFECHAEXP" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                No. Memorando *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFINUMMEMO" runat="server" MaxLength="20" CssClass="numeros"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                No. Expediente origen *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFIEXPORIGEN" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                            </td>
                        
                        </tr>


                          <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                No. Expediente Documentic *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFIEXPDOCUMENTIC" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                            </td>
                           
                        </tr>

                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <%--<asp:Button id="cmdMostrarEstadisticas" runat="server" Text="Mostrar estadísticas" cssClass="PCGButton"></asp:Button>--%>
                                <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                                <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>                     
                            </td>
                            <td>
                                <asp:Button id="cmdGestionar" runat="server" Text="Gestionar" cssClass="PCGButton"></asp:Button>        
                            </td>
                        </tr>
                    </table>
              </div>
              
              <div id="tabs12">
                    <table id="tblEditEJEFISGLOBAL2" class="ui-widget-content">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Fecha entrega al CAD para registro *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFIFECCAD" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Funcionario que realiza el reparto *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboRepartidor" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Revisor
                            </td>
                            <td>                                                                        
                                <asp:TextBox id="txtRevisor" runat="server" ReadOnly="True" style=" width:190px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Gestor responsable * 
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboEFIUSUASIG" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Estado actual * 
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" style=" color:Red" id="cboEFIESTADO" runat="server"></asp:DropDownList>                    </td>
                        </tr>                                
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Estado del pago *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboEFIESTADOPAGO" runat="server"></asp:DropDownList>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header">
                                Fecha entrega al gestor *
                            </td>
                            <td>
                                <asp:TextBox id="txtEFIFECENTGES" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button id="cmdMostrarEstadisticas" runat="server" Text="Mostrar estadísticas" cssClass="PCGButton"></asp:Button>
                            </td>
                            <td>                                
                                <asp:Button id="cmdSave2" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>                                
                                <asp:Button id="cmdRefrescar" runat="server" Text="Refrescar" cssClass="PCGButton"></asp:Button>
                            </td>
                        </tr>
                    </table>	
              </div>
              
              <div id="tabs2">
                    <!-- Info del Titulo :: inicio -->
                    <div style="margin-left:2px; margin-top:4px; width:960px; height:1840px;">                                                            	
						<iframe src="MAESTRO_TITULOS.aspx?pExpediente=<%  Response.Write(Session("ssExpedienteActual"))%>" width="960" height="1840" scrolling="no" frameborder="0"></iframe>								
					</div>
                    <!-- Info del Titulo :: fin    -->
              </div>
              
              <div id="tabs3">
                    <!-- Deudores :: inicio -->
                    <div style="margin-left:2px; margin-top:4px; width:960px; height:740px;">                                                            															
						<iframe src="expedientes/ENTES_DEUDORES_E.aspx?pExpediente=<%  Response.Write(Session("ssExpedienteActual"))%>&pTipo=1&pScr=repartidor" width="960" height="740" scrolling="no" frameborder="0"></iframe>
					</div>
                    <!-- Deudores :: fin    -->
              </div>
              
              <div id="tabs4">
                    <table id="tblCalsificacion" class="ui-widget-content">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                ¿Título ejecutivo a cargo de la nación, entidad territorial o descentralizada con antigüedad inferior a 10 meses? *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboTitEjecAntig" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                Caducidad *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboCaducidad" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                Estado Persona Natural o Jurídica *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboEstadoPersona" runat="server"></asp:DropDownList>
                            </td>
                        </tr>                                                
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                ¿Con proceso de cobro coactivo en curso? *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboProcesoCurso" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                Acuerdos de pago *
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" id="cboAcuerdoPago" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="ui-widget-header" style="width: 400px;">
                                <asp:Button ID="btnObtenerEstado" runat="server" Text="Determinar Estado inicial del proceso" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtExtadoInicial" runat="server" ReadOnly="True" Columns="80"></asp:TextBox>
                            </td>
                        </tr>
                        
                    </table>
              </div>
              
              <div id="tabs6">
                    <!-- Colocar aqui grid para mostrar las solicitudes de cambio de estado ......... -->
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content" AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="idunico" >
                                <ItemStyle CssClass="BoundFieldItemStyleHidden" />
                                <HeaderStyle CssClass="BoundFieldHeaderStyleHidden" />            
                            </asp:BoundField>
                            <asp:BoundField DataField="NroExp" HeaderText="Nro Exp." SortExpression="SOLICITUDCAMBIOESTADO.NroExp"></asp:BoundField>
                            <asp:BoundField DataField="gestor" HeaderText="Gestor" SortExpression="SOLICITUDCAMBIOESTADO.gestor"></asp:BoundField>
                            <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="SOLICITUDCAMBIOESTADO.fecha" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="estadoactual" HeaderText="Etapa actual" SortExpression="SOLICITUDCAMBIOESTADO.estadoactual"></asp:BoundField>
                            <asp:BoundField DataField="estadosolicitado" HeaderText="Etapa solicitada" SortExpression="SOLICITUDCAMBIOESTADO.estadosolicitado"></asp:BoundField>
                            <asp:BoundField DataField="accion" HeaderText="Estado solicitud" SortExpression="SOLICITUDCAMBIOESTADO.accion"></asp:BoundField>
                            <asp:BoundField DataField="nomRevisor" HeaderText="Revisor" SortExpression="SOLICITUDCAMBIOESTADO.nomRevisor"></asp:BoundField>
                            <asp:BoundField DataField="aprob_revisor" HeaderText="Aprob. Rev." SortExpression="SOLICITUDCAMBIOESTADO.aprob_revisor"></asp:BoundField>
                            <asp:BoundField DataField="fecha_aprob_revisor" HeaderText="Fecha aprob. Rev." SortExpression="SOLICITUDCAMBIOESTADO.fecha_aprob_revisor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:BoundField DataField="nomSupervisor" HeaderText="Supervisor" SortExpression="SOLICITUDCAMBIOESTADO.nomSupervisor"></asp:BoundField>
                            <asp:BoundField DataField="aprob_ejecutor" HeaderText="Aprob. Sup." SortExpression="SOLICITUDCAMBIOESTADO.aprob_ejecutor"></asp:BoundField>
                            <asp:BoundField DataField="fecha_aprob_ejecutor" HeaderText="Fecha aprob. Sup." SortExpression="SOLICITUDCAMBIOESTADO.fecha_aprob_ejecutor" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                            <asp:ButtonField ButtonType="Button" Text="Ver">
                                <ControlStyle CssClass="GridEditButton" />
                            </asp:ButtonField>
                        </Columns>
                        <HeaderStyle CssClass="ui-widget-header"  />
                        <RowStyle CssClass="ui-widget-content" />
                        <AlternatingRowStyle/>
                    </asp:GridView> 
                    <!-- Colocar aqui grid para mostrar las solicitudes de cambio de estado :: fin    -->
              </div>
              
            </div>
        </form>
    </body>
</html>


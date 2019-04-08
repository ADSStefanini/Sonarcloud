<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditMAESTRO_TITULOS.aspx.vb" Inherits="coactivosyp.EditMAESTRO_TITULOS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title>
            Editar Título Ejecutivos
        </title>
        
        <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>
        <script src="jquery.ui.button.js" type="text/javascript"></script>        
        <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
        
        <script type="text/javascript">
            function ActualizarMultas() {
                var txtcapitalmulta = parseInt(document.getElementById('txtcapitalmulta').value);
                document.getElementById('txttotalmultas').value = txtcapitalmulta;
                SumarTotal();
            }
            function ActualizarSentenciasJ() {
                var txtsentenciasjudiciales = parseInt(document.getElementById('txtsentenciasjudiciales').value);
                document.getElementById('txttotalsentencias').value = txtsentenciasjudiciales;
                SumarTotal();
            }
                                    
            function SumarOmisos() {
                var txtomisossalud = parseInt(document.getElementById('txtomisossalud').value);
                var txtomisospensiones = parseInt(document.getElementById('txtomisospensiones').value);
                var txtomisosfondosolpen = parseInt(document.getElementById('txtomisosfondosolpen').value);
                var txtomisosarl = parseInt(document.getElementById('txtomisosarl').value);
                var txtomisosicbf = parseInt(document.getElementById('txtomisosicbf').value);
                var txtomisossena = parseInt(document.getElementById('txtomisossena').value);
                var txtomisossubfamiliar = parseInt(document.getElementById('txtomisossubfamiliar').value);
                // Total Omisos
                document.getElementById('txttotalomisos').value = txtomisossalud + txtomisospensiones + txtomisosfondosolpen + txtomisosarl + txtomisosicbf + txtomisossena + txtomisossubfamiliar;
                SumarTotal();
            }
            function SumarMora() {
                var txtmorasalud = parseInt(document.getElementById('txtmorasalud').value);
                var txtmorapensiones = parseInt(document.getElementById('txtmorapensiones').value);
                var txtmorafondosolpen = parseInt(document.getElementById('txtmorafondosolpen').value);
                var txtmoraarl = parseInt(document.getElementById('txtmoraarl').value);
                var txtmoraicbf = parseInt(document.getElementById('txtmoraicbf').value);
                var txtmorasena = parseInt(document.getElementById('txtmorasena').value);
                var txtmorasubfamiliar = parseInt(document.getElementById('txtmorasubfamiliar').value);
                // Total Mora
                document.getElementById('txttotalmora').value = txtmorasalud + txtmorapensiones + txtmorafondosolpen + txtmoraarl + txtmoraicbf + txtmorasena + txtmorasubfamiliar;
                SumarTotal();
            }
            function SumarInexactos() {
                var txtinexactossalud = parseInt(document.getElementById('txtinexactossalud').value);
                var txtinexactospensiones = parseInt(document.getElementById('txtinexactospensiones').value);
                var txtinexactosfondosolpen = parseInt(document.getElementById('txtinexactosfondosolpen').value);
                var txtinexactosarl = parseInt(document.getElementById('txtinexactosarl').value);
                var txtinexactosicbf = parseInt(document.getElementById('txtinexactosicbf').value);
                var txtinexactossena = parseInt(document.getElementById('txtinexactossena').value);
                var txtinexactossubfamiliar = parseInt(document.getElementById('txtinexactossubfamiliar').value)
                // Total Mora
                document.getElementById('txttotalinexactos').value = txtinexactossalud + txtinexactospensiones + txtinexactosfondosolpen + txtinexactosarl + txtinexactosicbf + txtinexactossena + txtinexactossubfamiliar;
                SumarTotal();
            }
            
            function SumarTotal() {
                var txttotalmultas = parseInt(document.getElementById('txttotalmultas').value);
                var txttotalomisos = parseInt(document.getElementById('txttotalomisos').value);
                var txttotalmora = parseInt(document.getElementById('txttotalmora').value);
                var txttotalinexactos = parseInt(document.getElementById('txttotalinexactos').value);
                var txttotalsentencias = parseInt(document.getElementById('txttotalsentencias').value);
                var txtcuotaspartesacum = parseInt(document.getElementById('txtcuotaspartesacum').value);
                // Total Mora
                document.getElementById('txttotaldeuda').value = txttotalmultas + txttotalomisos + txttotalmora + txttotalinexactos + txttotalsentencias + txtcuotaspartesacum;
            }

            $(function() {
                //Manejo de tabs
                $("#tabs").tabs();
                
                //window.scrollTo(0, 0);
                //scrollTop: '0px';
                //$("html, body").animate({ scrollTop: 0 }, 600);
            
                       
                $('#cmdSave').button();
                $('#cmdCancel').button();
                //
                $('#cmdSave2').button();
                $('#cmdCancel2').button();
                //
                $('#cmdSave3').button();
                $('#cmdCancel3').button();

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

                // Evitar que el usuario edite los controles de fecha
                $("#txtMT_fec_expedicion_titulo").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_expedicion_titulo").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_expedicion_titulo').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                var FechaExpTitulo = $("#txtMT_fec_expedicion_titulo").val();
                var AnioFET = FechaExpTitulo.substring(6,10);
                var MesFET  = FechaExpTitulo.substring(3,5);
                var DiaFET  = FechaExpTitulo.substring(0,2);
                //alert(DiaFET);
                
                $("#txtMT_fec_notificacion_titulo").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_notificacion_titulo").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_notificacion_titulo').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,                    
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                   //
                $("#txtMT_fec_expe_resolucion_reposicion").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_expe_resolucion_reposicion").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_expe_resolucion_reposicion').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtMT_fec_not_reso_resu_reposicion").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_not_reso_resu_reposicion").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_not_reso_resu_reposicion').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtMT_fec_exp_reso_apela_recon").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_exp_reso_apela_recon").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_exp_reso_apela_recon').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtMT_fec_not_reso_apela_recon").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_not_reso_apela_recon").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_not_reso_apela_recon').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtMT_fecha_ejecutoria").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fecha_ejecutoria").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fecha_ejecutoria').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtMT_fec_exi_liq").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_exi_liq").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtMT_fec_exi_liq').datepicker({
                    numberOfMonths: 1,
                    ///minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecMemoDev").keypress(function(event) { event.preventDefault(); });
                $("#txtFecMemoDev").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecMemoDev').datepicker({
                    numberOfMonths: 1,
                    minDate: new Date(AnioFET, MesFET - 1, DiaFET),
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                
                $("#txtMT_fec_cad_presc").keypress(function(event) { event.preventDefault(); });
                $("#txtMT_fec_cad_presc").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                /*
                $('#txtMT_fec_cad_presc').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both', 
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                */
                //
                $("input[type=text]").keyup(function() {
                    $(this).val($(this).val().toUpperCase());
                });
                //
            });
            //
            $(function() {

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

                //Desactivar los totales
                $("#txttotalmultas").keypress(function(event) { event.preventDefault(); });
                $("#txttotalmultas").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $("#txttotalomisos").keypress(function(event) { event.preventDefault(); });
                $("#txttotalomisos").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $("#txttotalmora").keypress(function(event) { event.preventDefault(); });
                $("#txttotalmora").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $("#txttotalinexactos").keypress(function(event) { event.preventDefault(); });
                $("#txttotalinexactos").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $("#txttotalsentencias").keypress(function(event) { event.preventDefault(); });
                $("#txttotalsentencias").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

                $("#txttotaldeuda").keypress(function(event) { event.preventDefault(); });
                $("#txttotaldeuda").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                
                // Descativar la informacion de deuda de los parafiscales ya que va a ser importada del "SQL"//
                $(".numerosSL").keypress(function(event) { event.preventDefault(); });
                $(".numerosSL").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                                
                
                // Borrar la fecha de devolucion del titulo                
                $("#imgBtnBorraFechaDev").click(function() {
                    //alert( "Handler for .click() called." );
                    //txtFecMemoDev
                    $("#txtFecMemoDev").val("");
                });

                // ----------------------------------------------------------------------------
                // Mostrar / Ocultar cuadro de los conceptos de la deuda en funcion del perfil.
                // A los repartidores se les oculta el cuadro, a los abogados se les muestra
                // ----------------------------------------------------------------------------
                var perfil = <%  Response.Write(Session("mnivelacces") ) %> ;
                //alert(perfil);
                if(perfil == 5) {
                    $("#infoDetalleDeuda").css("display", "none");
                    $("#infoTotalesDeuda").css("display", "none");  
                    
                    // Si es repartidor => Ocultar la mayoria de los calendarios                    
                    $("#txtMT_fec_notificacion_titulo").datepicker("destroy");
                    $("#txtMT_fec_expe_resolucion_reposicion").datepicker("destroy");
                    $("#txtMT_fec_not_reso_resu_reposicion").datepicker("destroy");
                    $("#txtMT_fec_exp_reso_apela_recon").datepicker("destroy");
                    $("#txtMT_fec_not_reso_apela_recon").datepicker("destroy");
                    $("#txtMT_fecha_ejecutoria").datepicker("destroy");
                    $("#txtMT_fec_exi_liq").datepicker("destroy");
                    //$("#txtMT_fec_cad_presc").datepicker("destroy");
                    
                    //Ocultar boton de eliminar fecha de los calendarios
                    
                } else {
                    $("#infoDetalleDeuda").css("display", "block");
                    $("#infoTotalesDeuda").css("display", "block");
                   
                    // Evitar que el usuario edite control de txtTotalRepartidor
                    $("#txtTotalRepartidor").keypress(function(event) { event.preventDefault(); });
                    $("#txtTotalRepartidor").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                    
                    // Si NO es repartidor => Ocultar un calendario
                    if(perfil == 8) {
                        // 16/sep/2014. Si es perfil 8, es decir, gestor de información, tambien puede 
                        //              editar la fecha de expedicion del titulo ejecutivo
                    }else{
                        $("#txtMT_fec_expedicion_titulo").datepicker("destroy");
                    }
                    
                    
                    //Mostrar boton de eliminar fecha de los calendarios
                    
                }                
                // -------------------------------------------------------------------------------------------------
                
                
                
                // Dependiendo del tipo de titulo, se activan / desactivan los valores             
                $("#cboMT_tipo_titulo").change(function() {
                    ValidarTipoTitulo(this.value);
                    //alert($("#cboMT_tipo_titulo").val());
                    ActualizarMultas();
                    ActualizarSentenciasJ();
                    SumarOmisos();
                    SumarMora();
                    SumarInexactos();
                });
                // -------------------------------------------------------------------------------------------------
                
            });
            
            function LimpiarEntradasDeuda(){
                $("#txtomisossalud").val(0);
                $("#txtmorasalud").val(0);
                $("#txtinexactossalud").val(0);
                $("#txtomisospensiones").val(0);
                $("#txtmorapensiones").val(0);
                $("#txtinexactospensiones").val(0);
                $("#txtomisosfondosolpen").val(0);
                $("#txtmorafondosolpen").val(0);
                $("#txtinexactosfondosolpen").val(0);
                $("#txtomisosarl").val(0);
                $("#txtmoraarl").val(0);
                $("#txtinexactosarl").val(0);
                $("#txtomisosicbf").val(0);
                $("#txtmoraicbf").val(0);
                $("#txtinexactosicbf").val(0);
                $("#txtomisossena").val(0);
                $("#txtmorasena").val(0);
                $("#txtinexactossena").val(0);
                $("#txtomisossubfamiliar").val(0);
                $("#txtmorasubfamiliar").val(0);
                $("#txtinexactossubfamiliar").val(0);
            }
            
            function AjustarPrescripcion(Anios){
                // Si txtMT_fecha_ejecutoria no esta vacia => sumar los años que dice el parametro
                var fecha_ejecutoria = $("#txtMT_fecha_ejecutoria").val();
                
                if(!fecha_ejecutoria == ""){                                      
                    var anio = fecha_ejecutoria.substring(6);
                    var NumAnio = parseInt(anio,10);                   
                    NumAnio = NumAnio + Anios;                    
                    var NuevaFecha = fecha_ejecutoria.substring(0,6) + NumAnio;
                    //alert(NuevaFecha);
                    $("#txtMT_fec_cad_presc").val(NuevaFecha);
                }
            }
            
            function ValidarTipoTitulo(tipoTitulo){
                var perfilUser = <%  Response.Write(Session("mnivelacces") ) %> ;
                var AniosPrescrip = 5; //Años para que aplique prescripcion
                
                if(tipoTitulo == '04'){
                    //alert(tipoTitulo);
                    // Mostrar calendario                    
                    //$("#txtMT_fec_cad_presc").datepicker();
                    $('#txtMT_fec_cad_presc').datepicker({
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        showOn: 'both', 
                        buttonImage: 'calendar.gif',
                        buttonImageOnly: false,
                        changeYear: true,
                        beforeShow: function() { $('#ui-datepicker-div').css("z-index", 999999); }
                        //              ,changeMonth: true
                    });
                    
                    //04 = Requerimiento para declarar => No aplica
                    var AniosPrescrip = 0;
                }else{
                    //alert(tipoTitulo);
                    // Ocultar calendario                    
                    $("#txtMT_fec_cad_presc").datepicker("destroy");                    
                    
                    //03 = Cuotas partes
                    if(tipoTitulo == '03'){
                        var AniosPrescrip = 3;
                    }else{
                        // Para los demas tipos de titulo = 5 años
                        var AniosPrescrip = 5;
                    }
                }                
                // Llamar a la funcion AjustarPrescripcion 
                AjustarPrescripcion(AniosPrescrip);
                
                // Si esta logueado un repartidor = > solo dejar el campo "total repartidor"
                if (perfilUser == 5){
                    $("#infoDetalleDeuda").css("display", "none");
                    $("#infoSentencias").css("display", "none");
                    $("#infoMulta").css("display", "none");
                }else{
                    // 01=Liquidacion oficial, 02=Informe de fiscalizacion, 04=requerimiento ...
                    if(tipoTitulo == '01' || tipoTitulo == '02'  || tipoTitulo == '04'){
                        $("#infoDetalleDeuda").css("display", "block");
                        $("#infoSentencias").css("display", "none");
                        $("#infoMulta").css("display", "none");
                        
                        $("#txtcapitalmulta").prop('disabled', true);
                        $("#txttotalmultas").hide();
                        $("#txtsentenciasjudiciales").prop('disabled', true);
                        $("#txttotalsentencias").hide();
                        $("#txtcuotaspartesacum").hide();
                        //
                        $("#txttotalomisos").show();
                        $("#txttotalmora").show();
                        $("#txttotalinexactos").show();
                        //
                        $("#txtcapitalmulta").val(0);
                        $("#txttotalmultas").val(0);
                        $("#txtsentenciasjudiciales").val(0);
                        $("#txttotalsentencias").val(0);
                        $("#txtcuotaspartesacum").val(0);                        
                        
                    }else if(tipoTitulo == '03'){
                        //Cuotas partes acumuladas
                        $("#txtcuotaspartesacum").show(); 
                        //
                        $("#infoDetalleDeuda").css("display", "none");
                        $("#infoSentencias").css("display", "none");
                        $("#infoMulta").css("display", "none");
                        //
                        $("#txttotalmultas").hide();
                        $("#txttotalomisos").hide();
                        $("#txttotalmora").hide();
                        $("#txttotalinexactos").hide();
                        $("#txttotalsentencias").hide();
                        //
                        $("#txtcapitalmulta").val(0);
                        $("#txttotalmultas").val(0);                        
                        $("#txttotalomisos").val(0);
                        $("#txttotalmora").val(0);
                        $("#txttotalinexactos").val(0);
                        $("#txtsentenciasjudiciales").val(0);
                        $("#txttotalsentencias").val(0);
                        //
                        LimpiarEntradasDeuda();
                        
                    }else if (tipoTitulo == '05' || tipoTitulo == '07') {
                        //Resolucion Multa L1438/11 o L1607/12
                        $("#txtcapitalmulta").prop('disabled', false);
                        $("#txttotalmultas").show();
                        
                        $("#infoDetalleDeuda").css("display", "none");
                        $("#infoSentencias").css("display", "none");
                        $("#infoMulta").css("display", "block");
                        //
                        $("#txttotalomisos").hide();
                        $("#txttotalmora").hide();
                        $("#txttotalinexactos").hide();
                        $("#txttotalsentencias").hide();
                        $("#txtcuotaspartesacum").hide();
                        //
                        $("#txttotalomisos").val(0);
                        $("#txttotalmora").val(0);
                        $("#txttotalinexactos").val(0);
                        $("#txtsentenciasjudiciales").val(0);
                        $("#txttotalsentencias").val(0);
                        $("#txtcuotaspartesacum").val(0);
                        LimpiarEntradasDeuda();
                         
                    }else if (tipoTitulo == '06'){
                        // Sentencias
                        $("#txtsentenciasjudiciales").prop('disabled', false);
                        $("#txttotalsentencias").show();
                        
                        $("#infoDetalleDeuda").css("display", "none");
                        $("#infoSentencias").css("display", "block");
                        $("#infoMulta").css("display", "none");
                        //
                        $("#txttotalmultas").hide();
                        $("#txttotalomisos").hide();
                        $("#txttotalmora").hide();
                        $("#txttotalinexactos").hide();                        
                        $("#txtcuotaspartesacum").hide();
                        //
                        $("#txtcapitalmulta").val(0);
                        $("#txttotalmultas").val(0);
                        $("#txttotalomisos").val(0);
                        $("#txttotalmora").val(0);
                        $("#txttotalinexactos").val(0);
                        $("#txtcuotaspartesacum").val(0);                        
                        LimpiarEntradasDeuda();
                    }
                }
                                
            }
            
            $(document).ready(function() {		
	            ValidarTipoTitulo($("#cboMT_tipo_titulo").val());
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
		    * { font-size:12px; font-family:Arial;}	
		    .numeros { text-align:right; }
		    .numerosSL { text-align:right; } /* SL = Solo Lectura */
		</style>
    </head>
    <body>
        <form id="form1" runat="server">
            
            <div id="tabs">
	            <ul>
		            <li><a href="#tabs1">Información general</a></li>
		            <li><a href="#tabs2">Valores</a></li>
		            <li><a href="#tabs3">Devolución</a></li>
	            </ul>

	            <div id="tabs1">
	                <table id="tblGralTitulos" class="ui-widget-content" style="width: 780px;">
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header" style="width: 360px;">
		                        No. Título * 
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_nro_titulo" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
	                        </td>
                        </tr>                
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Tipo de Título *
	                        </td>
	                        <td>
		                        <asp:DropDownList CssClass="ui-widget" id="cboMT_tipo_titulo" runat="server"></asp:DropDownList>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Número de títulos acumulados
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_titulo_acumulado" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de expedición *
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_expedicion_titulo" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecExpTit" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de expedición del título" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de Notificación
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_notificacion_titulo" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecNotTit" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de notificación del título" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Forma de Notificación
	                        </td>
	                        <td>
		                        <asp:DropDownList CssClass="ui-widget" id="cboMT_for_notificacion_titulo" runat="server"></asp:DropDownList>                    </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        No. Resolución resuelve reposición
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_res_resuelve_reposicion" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de expedición resolución reposición
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_expe_resolucion_reposicion" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecExpRR" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de expedición resolución reposición" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de Notificación Resolución Resuelve Reposición 
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_not_reso_resu_reposicion" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecNotRRR" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de Notificación Resolución Resuelve Reposición" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Forma de Notificación Resolución Resuelve Reposición 
	                        </td>
	                        <td>
		                        <asp:DropDownList CssClass="ui-widget" id="cboMT_for_not_reso_resu_reposicion" runat="server"></asp:DropDownList>                    </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        No. Resolución resuelve apelación o reconsideración
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_reso_resu_apela_recon" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de expedición resolución apelación o Reconsideración
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_exp_reso_apela_recon" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecExpRAR" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de expedición resolución apelación o Reconsideración" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de Notificación Resolución Apelación o Reconsideración
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_not_reso_apela_recon" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecNotRAR" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de Notificación Resolución Apelación o Reconsideración" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Forma de notificación resolución apelación o reconsideración
	                        </td>
	                        <td>
		                        <asp:DropDownList CssClass="ui-widget" id="cboMT_for_not_reso_apela_recon" runat="server"></asp:DropDownList>                    </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de ejecutoria
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fecha_ejecutoria" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecEjec" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de ejecutoria" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de exigibilidad liquidación oficial
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_exi_liq" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFecExiLO" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de exigibilidad liquidación oficial " />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de caducidad o prescripción
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtMT_fec_cad_presc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
	                        </td>
                        </tr>	                        
                        <tr>
		                        <td>
			                        &nbsp;
		                        </td>
		                        <td class="ui-widget-header">
			                        Procedencia *
		                        </td>
		                        <td>
			                        <asp:DropDownList CssClass="ui-widget" id="cboPROCEDENCIA" runat="server"></asp:DropDownList>
		                        </td>
	                        </tr>
                        
                            <tr>
	                            <td colspan="2">
		                            &nbsp;
		                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
	                            </td>
                            </tr>
                            <tr>                   
	                            <td colspan="2">
		                            &nbsp;
		                            <asp:Button id="cmdCancel" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
		                            <asp:Button id="cmdSave" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
	                            </td>
                            </tr>
                        
                        
                    </table>
	            </div>
	            
	            <div id="tabs2">
	                <table id="tblEditMAESTRO_TITULOS" class="ui-widget-content">
                        <tr>
                            <td>
                                <table id = "tblTituloDeuda" class="ui-widget-content" style="width: 780px;">
	                                <tr>
	                                    <td>
	                                        &nbsp;
	                                    </td>		                        
		                                <td>
			                                <b>INFORMACION DE DEUDA</b>
		                                </td>
		                                <td style="width: 360px;">
		                                    &nbsp;
		                                </td>
	                                </tr>
                                </table>
                                <div id = "infoMulta">
                                    <table id = "tblInfoMulta" class="ui-widget-content" style="width: 780px;">	                            
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header" style="width: 360px;">
			                                    Capital multa
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtcapitalmulta" runat="server" CssClass="numeros" onblur="ActualizarMultas();"></asp:TextBox>
		                                    </td>
	                                    </tr>
                                    </table>
                                </div>
                                <div id = "infoDetalleDeuda">
                                    <table id = "tblInfoDeuda" class="ui-widget-content" style="width: 780px;">
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header" style="width: 360px;">
			                                    Omisos salud
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisossalud" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora salud
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmorasalud" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos salud
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactossalud" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos pensiones
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisospensiones" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora pensiones
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmorapensiones" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos pensiones
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactospensiones" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos Fondo Solidaridad Pensional
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisosfondosolpen" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora Fondo de Solidaridad Pensional
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmorafondosolpen" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos Fondo de Solidaridad Pensional
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactosfondosolpen" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos ARL
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisosarl" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora ARL
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmoraarl" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos ARL
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactosarl" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos ICBF
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisosicbf" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora ICBF
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmoraicbf" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos ICBF
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactosicbf" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos SENA
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisossena" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora SENA
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmorasena" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos SENA
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactossena" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Omisos Sub. Familiar
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtomisossubfamiliar" runat="server" CssClass="numerosSL" BackColor="#FFFF99" onblur="SumarOmisos();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Mora Sub. Familiar
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtmorasubfamiliar" runat="server" CssClass="numerosSL" BackColor="#EAD5FF" onblur="SumarMora();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Inexactos Sub. Familiar
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtinexactossubfamiliar" runat="server" CssClass="numerosSL" BackColor="#E6FFCC" onblur="SumarInexactos();"></asp:TextBox>
		                                    </td>
	                                    </tr>	                            
                                    </table>
                                </div>
                                <div id = "infoSentencias">
	                                <table id = "tblInfoSentencias" class="ui-widget-content" style="width: 780px;">	                            
		                                <tr>
			                                <td>
				                                &nbsp;
			                                </td>
			                                <td class="ui-widget-header" style="width: 360px;">
				                                Sentencias Judiciales
			                                </td>
			                                <td>
				                                <asp:TextBox id="txtsentenciasjudiciales" runat="server" CssClass="numeros" onblur="ActualizarSentenciasJ();"></asp:TextBox>
			                                </td>
		                                </tr>
	                                </table>
                                </div>
                            </td>                    
                        </tr>
                           
                        <tr>
                            <td>
                                <div id = "infoTotalesDeuda">
                                    <table id = "tblTotales" class="ui-widget-content" style="width: 780px;">
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>   
		                                    <td colspan="2">
			                                    <b>TOTALES</b>
		                                    </td>                                 
	                                    </tr>                             
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header" style="width: 360px;">
			                                    Total multas
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotalmultas" runat="server" CssClass="numeros" style="text-align:right"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total omisos
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotalomisos" runat="server" CssClass="numeros" BackColor="#FFFF99" style="text-align:right"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total mora
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotalmora" runat="server" CssClass="numeros" BackColor="#EAD5FF"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total inexactos
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotalinexactos" runat="server" CssClass="numeros" BackColor="#E6FFCC"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total sentencias judiciales
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotalsentencias" runat="server" CssClass="numeros" style="text-align:right"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total Cuotas Partes acumuladas
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txtcuotaspartesacum" runat="server" CssClass="numeros" onblur="SumarTotal();"></asp:TextBox>
		                                    </td>
	                                    </tr>
	                                    <tr>
		                                    <td>
			                                    &nbsp;
		                                    </td>
		                                    <td class="ui-widget-header">
			                                    Total deuda
		                                    </td>
		                                    <td>
			                                    <asp:TextBox id="txttotaldeuda" runat="server" CssClass="numeros" style="text-align:right"></asp:TextBox>
		                                    </td>
	                                    </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>                             
                        
                        <tr>
                            <td>
                                <table id = "tblTotalRepartidor" class="ui-widget-content" style="width: 780px;">
                                    <tr>
                                        <td>
			                                &nbsp;
		                                </td>
		                                <td class="ui-widget-header" style="width: 360px;">
			                                Total título (repartidor)
		                                </td>
		                                <td>
			                                <asp:TextBox id="txtTotalRepartidor" runat="server" CssClass="numeros"></asp:TextBox>
		                                    <asp:Label ID="lblLeyendaRepartidor" runat="server" ForeColor="Red" 
                                                Text="Este valor se copiará en el campo total deuda"></asp:Label>
		                                </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>                                
                                        
                        <tr>
                            <td colspan="2">
                                &nbsp;
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>                   
                            <td colspan="2">
                                &nbsp;
                                <asp:Button id="cmdCancel2" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
                                <asp:Button id="cmdSave2" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
                            </td>
                        </tr>
                    </table>
	            </div>
	            
	            <div id="tabs3">
	                <table id = "tblDevolucion" class="ui-widget-content" style="width: 780px;">
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td colspan="2">
		                        <b>DEVOLUCION DEL TITULO EJECUTIVO</b>
	                        </td>                                    
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Número de memorando
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtNumMemoDev" runat="server" CssClass="numeros"></asp:TextBox>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Fecha de memorando
	                        </td>
	                        <td>
		                        <asp:TextBox id="txtFecMemoDev" runat="server"></asp:TextBox>
		                        <asp:ImageButton ID="imgBtnBorraFechaDev" runat="server" ImageUrl="../images/icons/borrar16x16.png" ToolTip="Borrar fecha de devolución" />
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Causal de devolución
	                        </td>
	                        <td>
		                        <asp:DropDownList CssClass="ui-widget" id="cboCausalDevol" runat="server"></asp:DropDownList>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        &nbsp;
	                        </td>
	                        <td class="ui-widget-header">
		                        Observaciones
	                        </td>
	                        <td>
		                        <textarea id="taObsDevol" cols="90" rows="6" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
	                        </td>
                        </tr>
                        
                        <tr>
	                        <td colspan="4">
		                        &nbsp;
		                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
	                        </td>
                        </tr>
                        <tr>                   
	                        <td colspan="3">
		                        &nbsp;
		                        <asp:Button id="cmdCancel3" runat="server" Text="Cancelar" cssClass="PCGButton"></asp:Button>&nbsp;&nbsp;&nbsp;
		                        <asp:Button id="cmdSave3" runat="server" Text="Guardar" cssClass="PCGButton"></asp:Button>
	                        </td>
                        </tr>
                    </table>
	            </div>
                
            </div>
                                    
        </form>
    </body>
</html>

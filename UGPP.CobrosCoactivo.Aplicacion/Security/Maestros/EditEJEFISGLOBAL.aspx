<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditEJEFISGLOBAL.aspx.vb"
    Inherits="coactivosyp.EditEJEFISGLOBAL" %>

<%@ Import Namespace="coactivosyp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Editar información de procesos</title>
    <!-- jquery-1.10.2.min.js y jquery-ui-1.10.4.custom.min.js -->

    <script type="text/javascript" src="js/jquery-1.10.2.min.js"></script>

    <script type="text/javascript" src="js/jquery-ui-1.10.4.custom.min.js"></script>

    <link rel="stylesheet" href="css/redmond/jquery-ui-1.10.4.custom.css" />
    <link href="css/csstable.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $(function () {
            SumarLiqCredito();
            //

            // 15/01/2016. Politica persuasivo debe afectar a fecha final de persuasivo
            $("#txtPoliticaPersuasivo, #txtFecIniCC").blur(function () {
                var FechaFinal = SumarDiasFecha($("#txtFecIniCC").val(), $("#txtPoliticaPersuasivo").val());
                $("#txtFecEstiFin").val(FechaFinal);
            });

            $("input[type=text]").keyup(function () {
                $(this).val($(this).val().toUpperCase());
            });
            //

            $("#tabs-nivel2-infogral").tabs({ disabled: [6, 7] }); //when initializing the tabs
            $("#tabs-nivel2-infogral").tabs("option", "disabled", [6, 7]); // or setting after init

            //Aceptar solo numeros y letras
            $('.alfanumericos').keypress(function (e) {

                // Allow:         backspace, del, tab.
                if ($.inArray(e.keyCode, [46, 8, 9]) !== -1 ||
                    // Allow: Ctrl+A
                    (e.keyCode == 65 && e.ctrlKey === true) ||
                    // Allow: home, end, left, right
                    (e.keyCode >= 35 && e.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }

                var regex = new RegExp("^[A-Z0-9]+$");
                var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
                if (regex.test(str)) {
                    return true;
                }

                e.preventDefault();
                return false;
            });

            //Controles de dolo lectura
            $(".SoloLectura").keypress(function (event) { event.preventDefault(); });
            $(".SoloLectura").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });

            //Aceptar solo numeros
            $(".numeros").keydown(function (event) {
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
            //$("#txttotalmultas").keypress(function(event) { event.preventDefault(); });
            //$("#txttotalmultas").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });                

            //Manejo de tabs
            $("#tabs").tabs();
            $("#persuasivo3-tabs-3").tabs();
            $("#persuasivo3-tabs-2").tabs();
            $("#tabs-nivel2-infogral").tabs();
            $("#tabs-nivel2-persuasivo").tabs();
            $("#tabs-nivel2-coactivo").tabs();
            $("#tabs-nivel2-pagos").tabs();
            $("#tabs-nivel2-concursales").tabs();
            $("#tabs-nivel2-facilidades").tabs();
            $("#tabs-nivel2-intereses").tabs();
            $("#tabs-nivel3").tabs();
            //
            $("#tabs-nivel3-facilidades").tabs();
            $("#tabs-nivel3-multas").tabs();
            $("#tabs-nivel2-medidas").tabs();
            //
            $("#facilidades-nivel3-container").tabs();
            //
            $("#tabs-nivel3-persuasivo").tabs();

            //Botones de Guardar (efecto HOVER)                
            $('#cmdSaveConcursal').button();
            $('#cmdSaveTitulo').button();
            $('#cmdSavePersuasivo').button();
            $('#cmdSaveFacilidades').button();
            $('#cmdSaveCoactivo1').button();
            $('#cmdSaveCoactivo1').button();
            $('#cmdSolCambioEstado').button();
            $('#cmdSaveSuspension').button();
            $('#cmdSaveTerminacionCoactivo').button();
            $('#cmdSaveLiquidacionCredito').button();
            $('#cmdSaveOrdenEjecucion').button();
            $('#cmdSaveLevantamiento').button();

            //Ocultar todos los DatePicker si el estado del proceso es devuelto o terminado
            var NomEstadoProc = $("#txtNombreEstado").val();
            //alert(NomEstadoProc);
            var NivelPerfil = <% Response.Write(Session("mnivelacces")) %>;
            //alert(NivelPerfil);
            /*
            if(NivelPerfil == 8){
                alert("gestor de informacion");
            }else{
                alert("otro perfil diferente a gestor de informacion");
            }
            */

            if (NomEstadoProc == 'DEVUELTO' || NomEstadoProc == 'TERMINADO') {
                $.datepicker.datepicker('disable');
            }

            // Evitar que el usuario edite los controles de fecha
            $("#txtFecEstiFin").keypress(function (event) { event.preventDefault(); });
            $("#txtFecEstiFin").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            //$("#txtFecEstiFin").datepicker('disable'); -- FUNCIONA                

            $("#txtFecPago").keypress(function (event) { event.preventDefault(); });
            $("#txtFecPago").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecPago').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFecDeuda").keypress(function (event) { event.preventDefault(); });
            $("#txtFecDeuda").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecDeuda').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });


            // ********************************************************************************* //
            // Calendarios de la pestaña datos en persuasivo
            // ********************************************************************************* //
            //alert(NivelPerfil);
            if ((NomEstadoProc == 'COACTIVO' || NomEstadoProc == 'CONCURSAL') && (NivelPerfil != 8)) {
                //
            } else {

                var $condicionE1 = ($("#txtFecEnvioCC").val() == "" && (NivelPerfil != 2 || NivelPerfil != 3));
                var $condicionE2 = ($("#txtFecEnvioCC").val() != "" && (NivelPerfil == 2 || NivelPerfil == 3));
                if ($condicionE1 || $condicionE2) {
                    $("#txtFecEnvioCC").keypress(function (event) { event.preventDefault(); });
                    $("#txtFecEnvioCC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                    $('#txtFecEnvioCC').datepicker({
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        showOn: 'both',
                        buttonImage: 'calendar.gif',
                        buttonImageOnly: false,
                        changeYear: true,
                        yearRange: '-8:+1',
                        beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                        //              ,changeMonth: true
                    });

                }

                // 03/02/2016 

                var $condicionI1 = (NivelPerfil == 2 || NivelPerfil == 3);

                if ($condicionI1) {
                    $("#txtFecIniCC").keypress(function (event) { event.preventDefault(); });
                    $("#txtFecIniCC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                    $('#txtFecIniCC').datepicker({
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        showOn: 'both',
                        buttonImage: 'calendar.gif',
                        buttonImageOnly: false,
                        changeYear: true,
                        beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); },
                        onSelect: function () { $("#txtFecEstiFin").val(SumarDiasFecha($("#txtFecIniCC").val(), $("#txtPoliticaPersuasivo").val())); }
                    });

                }


                // 03/02/2016 

                var $condicionF1 = (NivelPerfil == 2 || NivelPerfil == 3);
                if ($condicionF1) {

                    $("#txtFecFinCC").keypress(function (event) { event.preventDefault(); });
                    $("#txtFecFinCC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                    $('#txtFecFinCC').datepicker({
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        showOn: 'both',
                        buttonImage: 'calendar.gif',
                        buttonImageOnly: false,
                        changeYear: true,
                        yearRange: '-8:+1',
                        beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                        //              ,changeMonth: true
                    });
                }

                $("#txtFecTerm").keypress(function (event) { event.preventDefault(); });
                $("#txtFecTerm").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecTerm').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    yearRange: '-8:+1',
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            }

            // ********************************************************************************* //
            // Calendarios de la pestaña de info gral en coactivo
            // ********************************************************************************* //
            if ((NomEstadoProc == 'PERSUASIVO' || NomEstadoProc == 'CONCURSAL') && (NivelPerfil != 8)) {
                //
            } else {
                $("#txtFecEstiFinCoac").keypress(function (event) { event.preventDefault(); });
                $("#txtFecEstiFinCoac").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecEstiFinCoac').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResAvoca").keypress(function (event) { event.preventDefault(); });
                $("#txtFecResAvoca").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResAvoca').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotAvoca").keypress(function (event) { event.preventDefault(); });
                $("#txtFecNotAvoca").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotAvoca').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecRadRecurso").keypress(function (event) { event.preventDefault(); });
                $("#txtFecRadRecurso").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRadRecurso').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });


                $("#txtFecOfiCitaPers").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiCitaPers").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCitaPers').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubEdic").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPubEdic").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubEdic').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });


                $("#txtFecOfiCitaCor").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiCitaCor").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCitaCor').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotifCor").keypress(function (event) { event.preventDefault(); });
                $("#txtFecNotifCor").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotifCor').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubAviso").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPubAviso").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubAviso').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtLiqCredFecCorte").keypress(function (event) { event.preventDefault(); });
                $("#txtLiqCredFecCorte").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtLiqCredFecCorte').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecOfiCitCorLC").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiCitCorLC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCitCorLC').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotCorLC").keypress(function (event) { event.preventDefault(); });
                $("#txtFecNotCorLC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotCorLC').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubAvisoLC").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPubAvisoLC").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubAvisoLC').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecRadObj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecRadObj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRadObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });


                $("#txtFecOfiCorLD").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiCorLD").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiCorLD').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecNotLD").keypress(function (event) { event.preventDefault(); });
                $("#txtFecNotLD").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecNotLD').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPubLD").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPubLD").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPubLD').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResAdj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecResAdj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResAdj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });


                $("#txtFecResExtin").keypress(function (event) { event.preventDefault(); });
                $("#txtFecResExtin").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResExtin').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
                /*
                $("#txtFecResLevMC").keypress(function(event) { event.preventDefault(); });
                $("#txtFecResLevMC").keydown(function(e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResLevMC').datepicker({
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
                //txtPerIniExtin
                $("#txtPerIniExtin").keypress(function (event) { event.preventDefault(); });
                $("#txtPerIniExtin").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtPerIniExtin').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                //txtPerFinExtin
                $("#txtPerFinExtin").keypress(function (event) { event.preventDefault(); });
                $("#txtPerFinExtin").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtPerFinExtin').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecTras").keypress(function (event) { event.preventDefault(); });
                $("#txtFecTras").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecTras').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            }
            //
            // ********************************************************************************* //
            // Calendarios de la pestaña de info gral en consursales
            // ********************************************************************************* //
            if ((NomEstadoProc == 'PERSUASIVO' || NomEstadoProc == 'COACTIVO') && (NivelPerfil != 8)) {
                //
                $("#txtFecResSusp").keypress(function (event) { event.preventDefault(); });
                $("#txtFecResSusp").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResSusp').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });
            } else {
                $("#txtFecRes").keypress(function (event) { event.preventDefault(); });
                $("#txtFecRes").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRes').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFijAvisoAdm").keypress(function (event) { event.preventDefault(); });
                $("#txtFecFijAvisoAdm").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFijAvisoAdm').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecDesfAvisoAdm").keypress(function (event) { event.preventDefault(); });
                $("#txtFecDesfAvisoAdm").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecDesfAvisoAdm').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecLimPresCred").keypress(function (event) { event.preventDefault(); });
                $("#txtFecLimPresCred").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecLimPresCred').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecOfiPres").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiPres").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiPres').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPres").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPres").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPres').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecTrasProy").keypress(function (event) { event.preventDefault(); });
                $("#txtFecTrasProy").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecTrasProy').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecOfiObj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiObj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPresObj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPresObj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPresObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecDecObj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecDecObj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecDecObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecRecRep").keypress(function (event) { event.preventDefault(); });
                $("#txtFecRecRep").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecRecRep').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPresAcu").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPresAcu").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPresAcu').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecAudConf").keypress(function (event) { event.preventDefault(); });
                $("#txtFecAudConf").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecAudConf').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecTermAcu").keypress(function (event) { event.preventDefault(); });
                $("#txtFecTermAcu").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecTermAcu').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecResAcu").keypress(function (event) { event.preventDefault(); });
                $("#txtFecResAcu").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecResAcu').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecOfiIncump").keypress(function (event) { event.preventDefault(); });
                $("#txtFecOfiIncump").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecOfiIncump').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPresOfiInc").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPresOfiInc").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPresOfiInc').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecAudInc").keypress(function (event) { event.preventDefault(); });
                $("#txtFecAudInc").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecAudInc').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecPresDemSSInc").keypress(function (event) { event.preventDefault(); });
                $("#txtFecPresDemSSInc").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecPresDemSSInc').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecTrasConcursal").keypress(function (event) { event.preventDefault(); });
                $("#txtFecTrasConcursal").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecTrasConcursal').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFinCobConcursal").keypress(function (event) { event.preventDefault(); });
                $("#txtFecFinCobConcursal").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFinCobConcursal').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecInsCamCom").keypress(function (event) { event.preventDefault(); });
                $("#txtFecInsCamCom").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecInsCamCom').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecadmis").keypress(function (event) { event.preventDefault(); });
                $("#txtFecadmis").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecadmis').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecLimPreObj").keypress(function (event) { event.preventDefault(); });
                $("#txtFecLimPreObj").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecLimPreObj').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecIniPagAcu").keypress(function (event) { event.preventDefault(); });
                $("#txtFecIniPagAcu").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecIniPagAcu').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFinPagAcu").keypress(function (event) { event.preventDefault(); });
                $("#txtFecFinPagAcu").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFinPagAcu').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecDemSupInt").keypress(function (event) { event.preventDefault(); });
                $("#txtFecDemSupInt").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecDemSupInt').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });

                $("#txtFecFinConCursal").keypress(function (event) { event.preventDefault(); });
                $("#txtFecFinConCursal").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
                $('#txtFecFinConCursal').datepicker({
                    numberOfMonths: 1,
                    showButtonPanel: true,
                    showOn: 'both',
                    buttonImage: 'calendar.gif',
                    buttonImageOnly: false,
                    changeYear: true,
                    beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                    //              ,changeMonth: true
                });


            }
            /////////////////////////////////////////////////////////////////////////////////
            $("#txtFecResolFac").keypress(function (event) { event.preventDefault(); });
            $("#txtFecResolFac").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecResolFac').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFecSolFac").keypress(function (event) { event.preventDefault(); });
            $("#txtFecSolFac").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecSolFac').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFecNotif").keypress(function (event) { event.preventDefault(); });
            $("#txtFecNotif").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecNotif').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFecGarantia").keypress(function (event) { event.preventDefault(); });
            $("#txtFecGarantia").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecGarantia').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtFecPagCuoIni").keypress(function (event) { event.preventDefault(); });
            $("#txtFecPagCuoIni").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtFecPagCuoIni').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtVencPrimCuo").keypress(function (event) { event.preventDefault(); });
            $("#txtVencPrimCuo").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtVencPrimCuo').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });

            $("#txtVenUltCuo").keypress(function (event) { event.preventDefault(); });
            $("#txtVenUltCuo").keydown(function (e) { if (e.keyCode == 8 || e.keyCode == 46) { return false; }; });
            $('#txtVenUltCuo').datepicker({
                numberOfMonths: 1,
                showButtonPanel: true,
                showOn: 'both',
                buttonImage: 'calendar.gif',
                buttonImageOnly: false,
                changeYear: true,
                beforeShow: function () { $('#ui-datepicker-div').css("z-index", 999999); }
                //              ,changeMonth: true
            });
            /////////////////////////////////////////////////////////////////////////////// yyy
            //$("#txtFecEstiFinCoac").datepicker("option", "disabled", true);
            /*
            var NomEstadoProc = $("#txtNombreEstado").val();                
            if (NomEstadoProc == 'DEVUELTO' || NomEstadoProc == 'TERMINADO') {
            $.datepicker.datepicker('disable');
            }
            */
            //$("#txtFecEstiFinCoac").datepicker("option", "disabled", true);
            //$("#txtFecEstiFinCoac").datepicker('disable');
            //$("#txtFecEstiFinCoac").datepicker.datepicker('disable');
            //$.datepicker.datepicker('disable');
            ///////////////////
        });


        /* mantener tab actual nivel 1 */
        $(function () {
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
                activate: function (event, ui) {
                    //  Get future value
                    var newIndex = ui.newTab.parent().children().index(ui.newTab);
                    //  Set future value
                    dataStore.setItem(index, newIndex)
                }
            });
        });

        /* Mantener tab actual nivel 2 informacion general*/
        $(function () {
            var index2 = 'valornivel2infogral';
            var dataStore2 = window.sessionStorage;
            try {
                var oldIndex2 = dataStore2.getItem(index2);
            } catch (e) {
                var oldIndex2 = 0;
            }
            $('#tabs-nivel2-infogral').tabs({
                active: oldIndex2,
                activate: function (event, ui) {
                    var newIndex2 = ui.newTab.parent().children().index(ui.newTab);
                    dataStore2.setItem(index2, newIndex2)
                }
            });
        });

        /* tabs-nivel2-persuasivo */
        /* Mantener tab actual nivel 2 persuasivo*/
        $(function () {
            var index21 = 'valornivel2persuasivo';
            var dataStore21 = window.sessionStorage;
            try {
                var oldIndex21 = dataStore21.getItem(index21);
            } catch (e) {
                var oldIndex21 = 0;
            }
            $('#tabs-nivel2-persuasivo').tabs({
                active: oldIndex21,
                activate: function (event, ui) {
                    var newIndex21 = ui.newTab.parent().children().index(ui.newTab);
                    dataStore21.setItem(index21, newIndex21)
                }
            });
        });


        /* Mantener tab actual nivel 2 coactivo*/
        $(function () {
            var index22 = 'valornivel2coactivo';
            var dataStore22 = window.sessionStorage;
            try {
                var oldIndex22 = dataStore22.getItem(index22);
            } catch (e) {
                var oldIndex22 = 0;
            }
            $('#tabs-nivel2-coactivo').tabs({
                active: oldIndex22,
                activate: function (event, ui) {
                    var newIndex22 = ui.newTab.parent().children().index(ui.newTab);
                    dataStore22.setItem(index22, newIndex22)
                }
            });
        });

        /* Mantener tab actual nivel 2 medidas cautelares*/
        $(function () {
            var index23 = 'valornivel2medidas';
            var dataStore23 = window.sessionStorage;
            try {
                var oldIndex23 = dataStore23.getItem(index23);
            } catch (e) {
                var oldIndex23 = 0;
            }
            $('#tabs-nivel2-medidas').tabs({
                active: oldIndex23,
                activate: function (event, ui) {
                    var newIndex23 = ui.newTab.parent().children().index(ui.newTab);
                    dataStore23.setItem(index23, newIndex23)
                }
            });
        });

    </script>

    <script language="javascript" type="text/javascript">
        function reportes(etapa) {
            var expediente = $('#<%=txtNroExp.ClientID%>');
            var nit = $('#<%=txtIdDeudor.ClientID%>');
            //var nombre = $('#<%=txtNombreDeudor.ClientID%>');
            var nombre = $("#txtNombreDeudor").val();
            alert("mensaje " + expediente.val() + " " + nit.val() + " " + nombre);

            var opconsul = 1;
            var ejecuciones = 1;
            var etapas = etapa;

            document.location.href = "../cobranzas2.aspx?cedula=" + nombre + "::" + nit.val() + "&expediente=" + expediente.val() + "&refcatastral=" + nit.val() + "&tipo=1&deunom=" + nombre + "&opconsul=" + opconsul + "&ejecuciones=1&etapa=" + etapa;

        }

        function ShowHideRequired(blnValor) {
            if (blnValor) {
                $('#tdNroOficio').hide();
                $('#tdFechaOficioPresentacion').hide();
                $('#tdRqrNroOficio').show();
                $('#tdRqrFechaOficioPresentacion').show();
            }
            else {
                $('#tdNroOficio').show();
                $('#tdFechaOficioPresentacion').show();
                $('#tdRqrNroOficio').hide();
                $('#tdRqrFechaOficioPresentacion').hide();
            }
        }

        $(document).ready(function () {
            $('#tdRqrNroOficio').hide();
            $('#tdRqrFechaOficioPresentacion').hide();
        });

    </script>

    <link href="<%=ResolveClientUrl("~/css/main.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <%
'Dim MTG2 As New MetodosGlobalesCobro
'Dim msg As String = ""

'Dim idGestorResp2 As String
'idGestorResp2 = MTG2.GetIDGestorResp(Request("ID"))
'If idGestorResp2 <> Session("sscodigousuario") Then
'    If Session("mnivelacces") = 4 Or Session("mnivelacces") = 6 Or Session("mnivelacces") = 7 Then
'        msg = "Este expediente está a cargo de otro gestor."
'        Response.Write("<h1 style='font-size:20px;'>" & msg & "</h1>")
'        Exit Sub
'    End If
'End If
        %>
        <asp:ToolkitScriptManager ID="tsm" runat="server">
        </asp:ToolkitScriptManager>
        <div id="encabezado">
            <div id="tituloencabezado">
                <%  Response.Write("&nbsp Usuario actual: " & Session("ssnombreusuario") & ".")%>
                <span style="font-weight: normal">Perfil (<asp:Label ID="lblNomPerfil" runat="server"
                    Text="Label"></asp:Label>)</span>
                <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 8px;">
                    <asp:LinkButton ID="A3" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Cerrar sesión" longdesc="Cerrar sesión" src="../images/icons/Shutdown.png" height="18" width="18" style=" vertical-align:middle" id="imgCerrarSesion" title="Cerrar sesión" /></asp:LinkButton>
                </div>
                <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 8px;">
                    <asp:LinkButton ID="ABack" runat="server" ToolTip="Cerrar sesión">
                        <img alt ="Regresar al listado de expedientes"  src="../images/icons/regresar.png" height="18" width="18" style=" vertical-align:middle" id="imgBack" title="Regresar al listado de expedientes" /></asp:LinkButton>
                </div>
                <div style="color: White; width: 30px; height: 20px; float: right; text-align: right; padding-right: 8px;">
                    <asp:LinkButton ID="ABackRep" runat="server" ToolTip="Regresar al escritorio repartidor">
                        <img alt ="Regresar al escritorio repartidor"  src="../images/icons/regresarrep.png" height="18" width="18" style=" vertical-align:middle" id="img1" title="Regresar al escritorio repartidor" /></asp:LinkButton>
                </div>
            </div>
        </div>
        <!---------------------- -->
        <div class="tablecss">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table id="infoexpediente">
                        <tr>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 100px; float: left; height: 20px">
                                        No. Expediente:</label>
                                    <asp:TextBox ID="txtNroExp" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 163px; float: left; height: 20px">
                                        No. Exp Documentic:</label>
                                    <asp:TextBox ID="txtNRODocumentic" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 100px; float: left; height: 20px">
                                        Cédula / NIT:</label>
                                    <asp:TextBox ID="txtIdDeudor" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td colspan="2" style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 100px; float: left; height: 20px">
                                        Nombre deudor:</label>
                                    <asp:TextBox ID="txtNombreDeudor" TextMode="SingleLine" runat="server" Width="370px"
                                        ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>

                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 100px; float: left; height: 20px">
                                        Estado:</label>
                                    <asp:TextBox ID="txtNombreEstado" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <!-- 2da Linea -->
                        <tr>
                            <td style="color: #2e6e9e;">
                                <div style="width: 160px; height: 41px">
                                    <label style="width: 170px; float: left; height: 30px">
                                        Tipo Título:</label>
                                    <asp:TextBox ID="txtTIPOTITULO" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 30px">
                                        &nbsp;Título Ejecutivo:</label>
                                    <asp:TextBox ID="txtNUMTITULOEJECUTIVO" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 30px">
                                        Fecha Título Ejecutivo:</label>
                                    <asp:TextBox ID="txtFECTITULO" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>

                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 30px">
                                        No. res. resuelve apelación o reconsideración:</label>
                                    <asp:TextBox ID="txtresolucionApelacion" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>

                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 30px">
                                        Fecha res. resuelve apelación o reconsideració:</label>
                                    <asp:TextBox ID="txtfechaapelacionreconsideracion" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>

                            </td>


                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 30px">
                                        Estado actual de la deuda:</label>
                                    <asp:TextBox ID="txtEstadoActualDeuda" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>

                            </td>




                        </tr>
                        <tr>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 100px; float: left; height: 20px">
                                        Gestor actual:</label>
                                    <asp:TextBox ID="txtGestorResp" runat="server" Width="170px" ReadOnly="True" Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e;">

                                <div style="width: 170px; height: 41px">
                                    <label style="width: 170px; float: left; height: 20px">
                                        Fecha entrega a gestor:</label>
                                    <asp:TextBox ID="txtFECENTREGAGESTOR" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 20px">
                                        Fecha Ejecutoría:</label>
                                    <asp:TextBox ID="txtFechaEjecutoria" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 20px">
                                        Fecha exigibilidad:</label>
                                    <asp:TextBox ID="txtFechaExigTitulo" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left"></asp:TextBox>
                                </div>

                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 170px; height: 41px">
                                    <label style="width: 160px; float: left; height: 20px">
                                        Fecha Prescripción:</label>
                                    <asp:TextBox ID="txtFechaPrescripcion" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left"></asp:TextBox>
                                </div>
                            </td>
                            <td></td>
                        </tr>


                        <!-- 3da Linea -->
                        <tr>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Total de la Deuda:</label>
                                    <asp:TextBox ID="txtTotalDeudaEA" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Capital inicial:</label>
                                    <asp:TextBox ID="txtCapitalInicial" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Valor Revocatoria:</label>
                                    <asp:TextBox ID="txtValorRevocatoria" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Saldo actual:</label>
                                    <asp:TextBox ID="txtSaldoEA" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <!-- 4da Linea -->
                        <tr>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Pagos a capital:</label>
                                    <asp:TextBox ID="txtPagosCapitalEA" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label id="lblInte" runat="server" style="width: 180px; float: left; height: 20px">
                                        Pago a Intereses:</label>
                                    <asp:TextBox ID="txtpagInteresLiq" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Total Pagados:</label>
                                    <asp:TextBox ID="txttotalpagLiq" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>

                        <!-- 4da Linea -->

                        <!-- 5da Linea -->
                        <tr id="lineaBotonPriorizar" runat="server" visible="false">
                            <td colspan="6">
                                <asp:Button ID="btnSolicitarSuspension" runat="server" Text="Solicitar Suspensión" CssClass="PCGButton button" />
                            </td>
                        </tr>
                        <!-- 5da Linea END-->
                        <tr id="trsancion" runat="server">
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Capital inicial Sanción:</label>
                                    <asp:TextBox ID="txtTotalDeudaSancion" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Pagos capital Sanción:</label>
                                    <asp:TextBox ID="txtPagosCapitalSancion" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Valor IPC:</label>
                                    <asp:TextBox ID="txtValorIPC" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Saldo actual Sanción:</label>
                                    <asp:TextBox ID="txtsandoactualSancion" runat="server" Width="170px" ReadOnly="True"
                                        Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td style="color: #2e6e9e">
                                <div style="width: 180px; height: 41px">
                                    <label style="width: 180px; float: left; height: 20px">
                                        Fecha ult. pago Sanción:</label>
                                    <asp:TextBox ID="txtUltPag" runat="server" Width="170px" ReadOnly="True" Style="float: left; text-align: right; color: Red"></asp:TextBox>
                                </div>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="tabsModal2">
                <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableScriptGlobalization="True" />--%>
                <!-- ModalPopupExtender -->
                <asp:ModalPopupExtender ID="mp2" runat="server" PopupControlID="PnlPriorizacion" TargetControlID="btnTest2"
                    CancelControlID="btnClose2" BackgroundCssClass="FondoAplicacion">
                </asp:ModalPopupExtender>
                <asp:Panel ID="PnlPriorizacion" runat="server" CssClass="modalPopup form-row CajaDialogo popup" align="center" Style="display: none; background: #fff;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <asp:TextBox ID="txtIdTareaSuspension" runat="server" Visible="False" />


                            <div class="form-group row">
                                <asp:Label ID="lblObservacionSuspension" runat="server" Text="Observación Suspesión*" CssClass="col-sm-3 col-form-label col-form-label-sm style4" AssociatedControlID="txtObservacionSuspension" />
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtObservacionSuspension" runat="server" Columns="50" Rows="5" CssClass="form-control form-control-sm" CausesValidation="True" TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ID="reqObservacionSuspension" runat="server" ErrorMessage="" ControlToValidate="txtObservacionSuspension" CssClass="error-msg" ValidationGroup="formSuspension" />
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <asp:Button ID="cmdSuspender" runat="server" Text="Solicitar Suspensión" CssClass="button" ClientIDMode="Inherit" ValidationGroup="formSuspension" />
                                <asp:Button ID="btnClose2" CssClass="PCGButton button" runat="server" Text="Cancelar" />
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </asp:Panel>
                <!-- ModalPopupExtender -->
            </div>
            <asp:Button ID="btnTest2" runat="server" Text="Button" CssClass="hide" />
        </div>
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Información General</a></li>
                <li><a href="#tabs-2">Cobro persuasivo</a></li>
                <li><a href="#tabs-3">Cobro coactivo</a></li>
                <li><a href="#tabs-4">Medidas Cautelares</a></li>
                <li><a href="#tabs-5">Procesos concursales</a></li>
                <li><a href="#tabs-6">Facilidades de pago</a></li>
                <!-- <li><a href="#tabs-7">Cuotas partes</a></li> -->
                <li><a href="#tabs-8">Pagos</a></li>
                <li><a href="#tabs-9">IPC Ó Intereses</a></li>
                <li><a href="#tabs-10">Mensajes</a></li>
                <li><a href="#tabs-11">Suspensión</a></li>
                <li><a href="#tabs-12">Cuotas P</a></li>
            </ul>
            <div id="tabs-1">
                <!-- Tab nivel 1 de informacion general :: inicio -->
                <div id="tabs-nivel2-infogral">
                    <ul>
                        <li><a href="#infogral-tabs-1">Deudor</a></li>
                        <li><a href="#infogral-tabs-2">Representante legal</a></li>
                        <li><a href="#infogral-tabs-3">Apoderado</a></li>
                        <li><a href="#infogral-tabs-32">Autorizado</a></li>
                        <li><a href="#infogral-tabs-4">Título ejecutivo</a></li>
                        <li><a href="#infogral-tabs-42">Recepción Título</a></li>
                        <li><a href="#infogral-tabs-5">Info deuda</a></li>
                        <li><a href="#infogral-tabs-6">Reparto inicial</a></li>
                        <li><a href="#infogral-tabs-7">Cambios de estado</a></li>
                        <li><a href="#infogral-tabs-8">Solicitud cambio estado</a></li>
                        <li><a href="#infogral-tabs-9">Otras resoluciones</a></li>
                    </ul>

                    <div id="infogral-tabs-1">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                            <iframe src="expedientes/ENTES_DEUDORES_E.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pTipo=1"
                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                    <div id="infogral-tabs-2">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                            <iframe src="expedientes/ENTES_DEUDORES_E.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pTipo=3"
                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                    <div id="infogral-tabs-3">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                            <iframe src="expedientes/ENTES_DEUDORES_E.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pTipo=4"
                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                    <div id="infogral-tabs-32">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                            <iframe src="expedientes/ENTES_DEUDORES_E.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pTipo=5"
                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                    <div id="infogral-tabs-4">
                        <!-- Info del Titulo :: inicio -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 940px;">
                            <iframe src="MAESTRO_TITULOS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="960" height="940" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- Info del Titulo :: fin    -->
                    </div>
                    <div id="infogral-tabs-42">
                        <!-- Info de recepción del Titulo :: inicio -->
                        <table id="tblRecepcionTitulo" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Recepción Título Ejecutivo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFIFECHAEXP" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Memorando
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFINUMMEMO" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Expediente origen
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFIEXPORIGEN" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Expediente Documentic
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFIEXPDOCUMENTIC" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha entrega al CAD para registro
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFIFECCAD" runat="server" CssClass="ui-widget" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <!-- Info de recepción del Titulo :: fin    -->
                    </div>
                    <div id="infogral-tabs-5">
                        <!-- Informacion de la deuda -->
                        <%--<table id="tblEditDEUDAS" class="ui-widget-content">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Capital multa
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtcapitalmulta" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Omisos salud
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtomisossalud" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmorasalud" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactossalud" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisospensiones" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmorapensiones" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactospensiones" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisosfondosolpen" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmorafondosolpen" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactosfondosolpen" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisosarl" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmoraarl" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactosarl" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisosicbf" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmoraicbf" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactosicbf" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisossena" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmorasena" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactossena" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtomisossubfamiliar" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtmorasubfamiliar" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtinexactossubfamiliar" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Sentencias Judiciales
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtsentenciasjudiciales" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>   
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
                                    <td class="ui-widget-header">
                                        Total multas
                                    </td>
                                    <td>
                                        <asp:TextBox id="txttotalmultas" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txttotalomisos" runat="server" CssClass="numeros" BackColor="#FFFF99" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txttotalmora" runat="server" CssClass="numeros" BackColor="#EAD5FF" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txttotalinexactos" runat="server" CssClass="numeros" BackColor="#E6FFCC" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txttotalsentencias" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txtcuotaspartesacum" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox id="txttotaldeuda" runat="server" CssClass="numeros" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>                                
                            </table>--%>
                    </div>
                    <div id="infogral-tabs-6">
                        <!-- reparto inicial -->
                        <table id="tblEditCAMBIOS_ESTADO" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Funcionario que realiza el reparto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtrepartidor" runat="server" CssClass="ui-widget" Columns="60"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Gestor responsable
                                </td>
                                <td>
                                    <asp:TextBox ID="txtabogado" runat="server" CssClass="ui-widget" Columns="60" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha del reparto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfecha" runat="server" CssClass="ui-widget" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Estado procesal
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcboestado" runat="server" CssClass="ui-widget" Columns="60" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Estado del pago
                                </td>
                                <td>
                                    <asp:TextBox ID="txtestadopago" runat="server" CssClass="ui-widget" Columns="60"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <!-- -------------------- -->
                    </div>
                    <div id="infogral-tabs-7">
                        <!-- cambios de estado -->
                        <table id="tblCambiosEstado" cellspacing="0" cellpadding="0" width="100%" border="0"
                            class="ui-widget-content ui-widget">
                            <tr>
                                <td align="right"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRecordsFound" runat="server"></asp:Label>
                                    <asp:GridView ID="grdCambiosEstado" runat="server" AutoGenerateColumns="False" CssClass="PCG-Content">
                                        <Columns>
                                            <asp:BoundField DataField="USUARIOSrepartidornombre" HeaderText="Funcionario que realiza el reparto"></asp:BoundField>
                                            <asp:BoundField DataField="USUARIOSabogadonombre" HeaderText="Gestor responsable"></asp:BoundField>
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha de reparto" DataFormatString="{0:dd/MM/yyyy}"></asp:BoundField>
                                            <asp:BoundField DataField="ESTADOS_PROCESOestadonombre" HeaderText="Estado procesal"></asp:BoundField>
                                            <asp:BoundField DataField="ESTADOS_PAGOestadopagonombre" HeaderText="Estado del pago"></asp:BoundField>
                                            <asp:BoundField DataField="ESTADOOPERATIVOestadooperativonombre" HeaderText="Estado operativo"></asp:BoundField>
                                            <asp:BoundField DataField="ETAPAPROCESALetapaprocesalnombre" HeaderText="Etapa procesal"></asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="ui-widget-header" />
                                        <RowStyle CssClass="ui-widget-content" />
                                        <AlternatingRowStyle />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <!-- ----------------------------------------------------- -->
                    </div>
                    <div id="infogral-tabs-8">
                        <!-- Solicitud de cambio de estado -->
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 980px; height: 740px;">
                            <iframe src="SOLICITUDES_CAMBIOESTADO.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="980" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <!-- Solicitud de cambio de estado -->
                    </div>
                    <div id="infogral-tabs-9">
                        <!-- Otras resoluciones -->
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 980px; height: 740px;">
                            <iframe src="OTRASRESOLUCIONES.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="980" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <!-- Otras resoluciones -->
                    </div>
                </div>
            </div>
            <!-- Tab nivel 1 de informacion general :: fin -->
            <div id="tabs-2">
                <!-- Tab nivel 1 de persuasivo :: inicio -->
                <div id="tabs-nivel2-persuasivo">
                    <ul>
                        <li><a href="#persuasivo-tabs-1">Datos</a></li>
                        <li><a href="#persuasivo-tabs-2">Generar Actos</a></li>
                    </ul>
                    <div id="persuasivo-tabs-1">
                        <div id="tabs-nivel3-persuasivo">
                            <ul>
                                <li><a href="#persuasivo3-tabs-1">Resumen Gestión Persuasiva</a></li>
                                <li><a href="#persuasivo3-tabs-2">Oficios Gestión Persuasiva</a></li>
                                <li><a href="#persuasivo3-tabs-3">Llamadas Gestión Persuasiva</a></li>
                            </ul>
                            <div id="persuasivo3-tabs-1">
                                <table id="tblEditPERSUASIVO" class="ui-widget-content">
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Fecha estimada terminación persuasivo
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecEstiFin" runat="server" CssClass="ui-widget" ReadOnly="true"
                                                MaxLength="10" BorderColor="#009933"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Fecha de envío a la DSIAC
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecEnvioCC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnBorraFecEnvioCC" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                                ToolTip="Borrar Fecha de envío al DSIAC" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Fecha Primera Acción Persuasiva
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecIniCC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                            <%--<asp:ImageButton ID="imgBtnBorraFecIniCC" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                                            ToolTip="Borrar Fecha primera acción persuasivo" />--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Política persuasivo (meses)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPoliticaPersuasivo" runat="server" CssClass="numeros" Style="text-align: right"
                                                MaxLength="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Fecha culminación persuasivo en DSIAC
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecFinCC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnBorraFecFinCC" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                                ToolTip="Borrar Fecha culminación persuasivo en DSIAC" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Resultado acciones persuasivas
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="ui-widget" ID="cboResCobCC" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Criterios para ordenar Medidas Cautelares
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="ui-widget" ID="cboCriteriosMC" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Estado Final del pago en Cobro Persuasivo
                                        </td>
                                        <td>
                                            <!-- <asp:TextBox id="txtEstaFinPag" runat="server" CssClass="ui-widget"></asp:TextBox> -->
                                            <asp:DropDownList CssClass="ui-widget" ID="cboEstaFinPag" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header" colspan="2">Observaciones
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <!-- ------------------------------------------------------------------------------------------------------ -->
                                            <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 340px;">
                                                <iframe src="PERSUASIVOOBSERVACIONES.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                                    width="960" height="740" scrolling="no" frameborder="0"></iframe>
                                            </div>
                                            <!-- ------------------------------------------------------------------------------------------------------ -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header" colspan="2">Terminación del proceso
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Causal Terminación
                                        </td>
                                        <td>
                                            <%--<asp:TextBox id="txtCausalTerm" runat="server" Columns="50" CssClass="ui-widget"></asp:TextBox>--%>
                                            <asp:DropDownList CssClass="ui-widget" ID="cboCausalFinPers" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">No. Auto
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAutoTerm" runat="server" Columns="20" CssClass="alfanumericos"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td class="ui-widget-header">Fecha Auto
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecTerm" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="imgBtnBorraFecTerm" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                                ToolTip="Borrar Fecha Auto" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="cmdSavePersuasivo" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="persuasivo3-tabs-2">
                                <div>
                                    <ul>
                                        <%--<li><a href="#idpersuasivo4-nivel5-tabs-3-tabs-1">Agregar datos gestión persuasiva</a></li>--%>
                                        <li><a href="#idpersuasivo4-nivel5-tabs-3-tabs-2">Consultar gestión persuasiva</a></li>


                                    </ul>

                                    <%-- <div id="idpersuasivo4-nivel5-tabs-3-tabs-1">
                                                <!-- ------------------------------------------------------------------------------------------------------ -->
                                                <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 340px;">
                                                    <iframe src="PERSUASIVOOFICIOS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                                                </div>
                                            </div>--%>
                                    <div id="idpersuasivo4-nivel5-tabs-3-tabs-2">
                                        <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 440px;">
                                            <iframe src="PERSUASIVOOFICIOSINFORME.aspx?pExpediente=<%  Response.Write(Request("ID"))%>" width="960" height="440" scrolling="no"
                                                frameborder="0"></iframe>
                                        </div>
                                    </div>

                                </div>

                            </div>
                            <div id="persuasivo3-tabs-3">
                                <div id="idpersuasivo4-nivel4-tabs-3">
                                    <ul>
                                        <%--<li><a href="#idpersuasivo4-nivel4-tabs-3-tabs-1">Agregar datos de llamadas</a></li>--%>
                                        <li><a href="#idpersuasivo4-nivel4-tabs-3-tabs-2">Consultar llamadas</a></li>


                                    </ul>
                                    <%--<div id="idpersuasivo4-nivel4-tabs-3-tabs-1">
                                                <!-- ------------------------------------------------------------------------------------------------------ -->
                                                <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 540px;">
                                                    <iframe src="PERSUASIVOLLAMADAS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                                width="960" height="740" scrolling="no" frameborder="0"></iframe>
                                                </div>
                                            </div>--%>

                                    <div id="idpersuasivo4-nivel4-tabs-3-tabs-2">
                                        <div style="margin-left: 2px; margin-top: 4px; width: 100%; height: 240px;">
                                            <iframe src="PERSUASIVOLLAMADASINFORME.aspx?pExpediente=<%  Response.Write(Request("ID"))%>" width="100%" height="440" scrolling="no" frameborder="0"></iframe>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div id="persuasivo-tabs-2">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 980px; height: 740px;">
                            <%
                'Declarar variables
                Dim expediente As String = txtNroExp.Text.Trim
                Dim nit As String = txtIdDeudor.Text.Trim
                Dim nombre As String = txtNombreDeudor.Text.Trim
                Dim opconsul As Integer = 1
                Dim ejecuciones As Integer = 1
                Dim etapas As String = "01"
                Dim cmd As String = ""
                Dim tmpNomEstadoExp As String = txtNombreEstado.Text.Trim
                '20/03/2014. Expediente editable solo por responsable xxx
                Dim MTG As New coactivosyp.MetodosGlobalesCobro
                Dim idGestorResp As String = MTG.GetIDGestorResp(Request("ID"))
                Dim NivelAccesoPerfil As Integer
                NivelAccesoPerfil = Session("mnivelacces")

                '--------------------------------------------------------------------------------------------

                'Datos del iframe
                cmd = "<iframe src='../cobranzas2.aspx?cedula=" & nombre & "::" & nit & "&expediente=" & expediente & "&refcatastral=" & nit & "&tipo=1&deunom=" & nombre & "&opconsul=" & opconsul & "&ejecuciones=1&etapa=" & etapas & "' width='980' height='740' scrolling='no' frameborder='0'></iframe>"

                If NivelAccesoPerfil = 8 Then
                    Response.Write(cmd)
                Else
                    If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                        'Si el estado es devuelto o terminado => No crear el iframe
                        cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "COACTIVO" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado COACTIVO no permiten generar actuaciones de PERSUASIVO</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "CONCURSAL" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado CONCURSAL no permiten generar actuaciones de PERSUASIVO</label>"
                        Response.Write(cmd)
                    ElseIf idGestorResp <> Session("sscodigousuario") Then
                        cmd = "<label style='color:Red'>Este expediente está a cargo de otro gestor. No permiten adicionar datos</label>"
                        Response.Write(cmd)
                    Else
                        '"Pintar" el iframe
                        Response.Write(cmd)
                    End If
                    '
                End If

                            %>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>

                </div>
            </div>
            <!-- Tab nivel 1 de persuasivo :: fin -->
            <div id="tabs-3">
                <!-- Tab nivel 1 del cobro coactivo :: inicio -->
                <div id="tabs-nivel2-coactivo">
                    <ul>
                        <li><a href="#coactivo-tabs-1" onclick="window.location.reload()">General</a></li>
                        <li><a href="#coactivo-tabs-2" onclick="var iframe = document.getElementById('ifMandamientosPago');iframe.src = iframe.src;">Mandamiento de pago</a></li>
                        <li><a href="#coactivo-tabs-5" onclick="window.location.reload()">Orden de Ejecución</a></li>
                        <li><a href="#coactivo-tabs-6" onclick="window.location.reload()">Liquidación del crédito</a></li>
                        <li><a href="#coactivo-tabs-7" onclick="window.location.reload()">Terminación</a></li>
                        <li><a href="#coactivo-tabs-3" onclick="var iframe2 = document.getElementById('ifExcepciones');iframe2.src = iframe2.src;">Excepciones</a></li>
                        <li><a href="#coactivo-tabs-4">Generar Actos</a></li>
                    </ul>
                    <div id="coactivo-tabs-1">
                        <!-- contenido de tab nivel 2 (coactivo) :: inicio -->
                        <table id="tblEditCOACTIVO" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha estimada terminación coactivo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecEstiFinCoac" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Resolución que Avoca Conocimiento
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResAvocaCon" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResAvoca" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha notificación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecNotAvoca" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Vinculación deudores solidarios
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResVinDeudorSol" runat="server" MaxLength="25" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecVinDeudorSol" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="ui-widget-header">
                                        Deudores solidarios vinculados
                                    </td>
                                    <td>
                                                        <asp:TextBox id="txtDeudoresVinc" runat="server" CssClass="ui-widget"></asp:TextBox>
                                    </td>
                                /tr>--%>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Resolución de adjudicación
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Adjudicatario
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdjudicat" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResAdj" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResAdj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Extinción atípica de la obligación
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Causal
                                </td>
                                <td>
                                    <asp:DropDownList ID="CboExtincion" runat="server" Height="16px" Width="126px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Decreto
                                </td>
                                <td>
                                    <asp:DropDownList ID="CboDecreto" runat="server" Height="16px" Width="125px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Alcance
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboAlcance" runat="server" Height="16px" Width="127px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Período inicial
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPerIniExtin" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Período final
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPerFinExtin" runat="server" MaxLength="50" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Valor
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValExtin" runat="server" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResExtin" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResExtin" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Resultado de la gestión
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Observaciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObsReselGes" runat="server" Height="101px" TextMode="MultiLine"
                                        Width="450px" Columns="80" Rows="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Estado del pago en cobro coactivo
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboEstadoPagCoac" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Estado al que se traslada
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboEstadoTras" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha del Traslado
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecTras" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="cmdSaveCoactivo1" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>




                        </table>
                        <div style="margin-left: 0px; margin-top: 4px; width: 660px; height: 340px;">
                            <iframe id="ifObsGral" name="ifObsGral" src="NOTAS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pModulo=21"
                                width="660" height="300" scrolling="yes" frameborder="0"></iframe>
                        </div>
                        <!-- contenido de tab nivel 2 (coactivo) :: fin     -->
                    </div>
                    <div id="coactivo-tabs-2">
                        <!-- contenido de tab nivel 2 (coactivo) (Mandamiento(s) de pago) :: inicio     -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 660px; height: 740px;">
                            <iframe id="ifMandamientosPago" name="ifMandamientosPago" src="mandamientos_pago.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="660" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- contenido de tab nivel 2 (coactivo) (Mandamiento(s) de pago) :: fin     -->
                    </div>
                    <div id="coactivo-tabs-5">
                        <table id="tblEditORDENEJECUCION" class="ui-widget-content">
                            <tr>
                                <td colspan="3">&nbsp;
                                <asp:CustomValidator ID="CustomValidatorOrdenEjecucion" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Resolución que ordena continuar ejecución
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. de Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResolEjec" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResolEjec" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Oficio Notificación por correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiCitaCor" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Oficio Notificación por Correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecOfiCitaCor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Notificación por Correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecNotifCor" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de publicación del aviso
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPubAviso" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="cmdSaveOrdenEjecucion" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-left: 0px; margin-top: 4px; width: 660px; height: 340px;">
                            <iframe id="ifObsOrdenEjecucion" name="ifObsOrdenEjecucion" src="NOTAS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pModulo=23"
                                width="660" height="300" scrolling="yes" frameborder="0"></iframe>
                        </div>
                    </div>
                    <div id="coactivo-tabs-6">
                        <table id="tblEditLIQUIDACIONCREDITO" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:CustomValidator ID="CustomValidatorLiquidacionCredito" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Liquidación de crédito y costas
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. de Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResLiquiCred" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResLiquiCred" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Capital
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLiqCredCapital" runat="server" CssClass="numeros" Style="text-align: right"
                                        onblur="SumarLiqCredito();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Intereses
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLiqCredInteres" runat="server" CssClass="numeros" Style="text-align: right"
                                        onblur="SumarLiqCredito();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Total
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLiqCredTotal" runat="server" CssClass="numeros" Style="text-align: right"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de corte de intereses
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLiqCredFecCorte" runat="server" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Oficio Notificación por correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiCitCorLC" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Oficio Notificación por correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecOfiCitCorLC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Notificación por correo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecNotCorLC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Publicación del aviso
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPubAvisoLC" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Objeciones contra la liquidación de crédito y costas
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Radicación de Objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecRadObj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Radicación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroRad" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. de Resolución que aprueba Liquidación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResApLiq" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Resolución aprueba Liquidación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResApLiq" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. de Oficio Notificación por correo Liquidación Definitiva
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiCorLD" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Oficio Notificación por correo Liquidación Definitiva
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecOfiCorLD" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de Notificación por correo Liquidación Definitiva
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecNotLD" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de publicación por correo Liquidación Definitiva
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPubLD" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="cmdSaveLiquidacionCredito" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-left: 0px; margin-top: 4px; width: 660px; height: 340px;">
                            <iframe id="ifObsLiquidacionCred" name="ifObsLiquidacionCred" src="NOTAS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pModulo=24"
                                width="660" height="300" scrolling="yes" frameborder="0"></iframe>
                        </div>
                    </div>
                    <div id="coactivo-tabs-7">
                        <table id="tblEditTERMINACIONCOACTIVO" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:CustomValidator ID="CustomValidatorTerminacionCoactivo" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Terminación del proceso
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Causal
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboCausalFinPro" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResFinPro" runat="server" MaxLength="15" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResFinPro" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="cmdSaveTerminacionCoactivo" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        <div style="margin-left: 0px; margin-top: 4px; width: 660px; height: 340px;">
                            <iframe id="ifObsTerminacionCoac" name="ifObsTerminacionCoac" src="NOTAS.aspx?pExpediente=<%  Response.Write(Request("ID"))%>&pModulo=25"
                                width="660" height="300" scrolling="yes" frameborder="0"></iframe>
                        </div>
                    </div>
                    <div id="coactivo-tabs-3">
                        <!-- contenido de tab nivel 2 (coactivo) (Excepciones) :: inicio     -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 660px; height: 740px;">
                            <iframe id="ifExcepciones" name="ifExcepciones" src="EXCEPCIONES.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="660" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- contenido de tab nivel 2 (coactivo) (Excepciones) :: fin     -->
                    </div>
                    <div id="coactivo-tabs-4">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 990px; height: 740px;">
                            <%
                expediente = txtNroExp.Text.Trim
                nit = txtIdDeudor.Text.Trim
                nombre = txtNombreDeudor.Text.Trim
                opconsul = 1
                ejecuciones = 1
                etapas = "02"
                'tmpNomEstadoExp = txtNombreEstado.Text.Trim                                        

                'Datos del iframe
                cmd = "<iframe src='../cobranzas2.aspx?cedula=" & nombre & "::" & nit & "&expediente=" & expediente & "&refcatastral=" & nit & "&tipo=1&deunom=" & nombre & "&opconsul=" & opconsul & "&ejecuciones=1&etapa=" & etapas & "' width='990' height='740' scrolling='no' frameborder='0'></iframe>"

                If NivelAccesoPerfil = 8 Then
                    Response.Write(cmd)
                Else
                    If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                        'Si el estado es devuelto o terminado => No crear el iframe
                        cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "PERSUASIVO" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado PERSUASIVO no permiten generar actuaciones de COACTIVO</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "CONCURSAL" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado CONCURSAL no permiten generar actuaciones de COACTIVO</label>"
                        Response.Write(cmd)
                    ElseIf idGestorResp <> Session("sscodigousuario") Then
                        cmd = "<label style='color:Red'>Este expediente está a cargo de otro gestor. No permiten adicionar datos</label>"
                        Response.Write(cmd)
                    Else
                        '"Pintar" el iframe
                        Response.Write(cmd)
                    End If
                End If

                            %>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                    <!--                      
                      <div id="coactivo-tabs-5">
                            <p>.</p>
                                                                                                                                          </div>
                      -->
                </div>
            </div>
            <!-- Tab nivel 1 del cobro coactivo :: fin -->
            <div id="tabs-4">
                <div id="tabs-nivel2-medidas">
                    <ul>
                        <li><a href="#medidas-tabs-1" onclick="var iframe = document.getElementById('ifEmbargos');iframe.src = iframe.src;">Datos</a></li>
                        <li><a href="#medidas-tabs-3" onclick="window.location.reload()">Levantamiento</a></li>
                        <li><a href="#medidas-tabs-2">Generar Actos</a></li>
                    </ul>
                    <div id="medidas-tabs-1">
                        <!-- contenido de tab nivel 1 (MEDIDAS CAUTELARES)  :: inicio     -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 760px; height: 740px;">
                            <iframe id="ifEmbargos" name="ifEmbargos" src="embargos.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="860" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- contenido de tab nivel 1 (MEDIDAS CAUTELARES)  :: fin     -->
                    </div>
                    <div id="medidas-tabs-3">
                        <!-- contenido de tab nivel 1 (MEDIDAS CAUTELARES)  :: xxxyyy     -->
                        <table id="tblEditLEVANTAMIENTO" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">Levantamiento de medidas cautelares
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Resolución
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResLevMC" runat="server" MaxLength="15" CssClass="SoloLectura"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResLevMC" runat="server" CssClass="SoloLectura" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Tipo
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboTipoLevMC" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="cmdSaveLevantamiento" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        <!-- contenido de tab nivel 1 (MEDIDAS CAUTELARES)  :: fin     -->
                    </div>
                    <div id="medidas-tabs-2">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 990px; height: 740px;">
                            <%
                expediente = txtNroExp.Text.Trim
                nit = txtIdDeudor.Text.Trim
                nombre = txtNombreDeudor.Text.Trim
                opconsul = 1
                ejecuciones = 1
                etapas = "05"
                'tmpNomEstadoExp = txtNombreEstado.Text.Trim                                        

                'Datos del iframe
                cmd = "<iframe src='../cobranzas2.aspx?cedula=" & nombre & "::" & nit & "&expediente=" & expediente & "&refcatastral=" & nit & "&tipo=1&deunom=" & nombre & "&opconsul=" & opconsul & "&ejecuciones=1&etapa=" & etapas & "' width='990' height='740' scrolling='no' frameborder='0'></iframe>"

                If NivelAccesoPerfil = 8 Then
                    Response.Write(cmd)
                Else
                    If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                        'Si el estado es devuelto o terminado => No crear el iframe
                        cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                        Response.Write(cmd)
                    ElseIf idGestorResp <> Session("sscodigousuario") Then
                        cmd = "<label style='color:Red'>Este expediente está a cargo de otro gestor. No permiten adicionar datos</label>"
                        Response.Write(cmd)
                    Else
                        '"Pintar" el iframe
                        Response.Write(cmd)
                    End If

                End If
                            %>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                </div>
            </div>
            <div id="tabs-5">
                <div id="tabs-nivel2-concursales">
                    <ul>
                        <li><a href="#concursales-tabs-1">Datos</a></li>
                        <li><a href="#concursales-tabs-2">Generar Actos</a></li>
                    </ul>
                    <div id="concursales-tabs-1">
                        <!-- contenido de tab nivel 2 (consursales-datos) :: inicio -->
                        <table id="tblEditCONCURSALES" runat="server" class="ui-widget-content">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>INICIO DEL PROCESO
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Tipo de proceso concursal *
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboTipoProcCon" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. auto de apertura del proceso *
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResApertura" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha auto *    
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecRes" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecRes" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Promotor y/o liquidador designado
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboPromotor" runat="server">
                                    </asp:DropDownList>
                                    <span class="add_edit" style="vertical-align: bottom"><a href="PROMOTORES.aspx" target="_blank"
                                        onclick="window.open(this.href, this.target, 'width=760,height=500'); return false;">
                                        <img src="images/adicionar16x16.png" title="Adicionar" alt="Adicionar" class="imgaddedt" />
                                    </a></span>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">PRESENTACIÓN DEL CRÉDITO
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de admisión * 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecadmis" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecadmis" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de fijación aviso de admisión
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecFijAvisoAdm" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecFijAvisoAdm" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de desfijación aviso de admisión
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecDesfAvisoAdm" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecDesfAvisoAdm" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha límite para presentar el crédito *
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecLimPresCred" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecLimPresCred" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>


                            <!--- 
                              Nombre: Jeisson Gómez  
                              HU    : Campos Obligatorios   
                              Objetivo: Se ponen obligatorios los campos. Fecha límite para presentar el crédito y No. Oficio de presentación. 
                              Siempre y cuando sean 10 días después de la fecha de entrega al gestor. 

                            --->
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td id="tdNroOficio" class="ui-widget-header">No. Oficio de presentación
                                </td>
                                <td id="tdRqrNroOficio" class="ui-widget-header">No. Oficio de presentación *
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiPres" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td id="tdFechaOficioPresentacion" class="ui-widget-header">Fecha oficio de presentación
                                </td>
                                <td id="tdRqrFechaOficioPresentacion" class="ui-widget-header">Fecha oficio de presentación *
                                </td>

                                <td>
                                    <asp:TextBox ID="txtFecOfiPres" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecOfiPres" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>

                            <!--- 
                              Nombre: Jeisson Gómez  
                              HU    : Campos Obligatorios   
                              Objetivo: Se ponen obligatorios los campos. Fecha límite para presentar el crédito y No. Oficio de presentación. 
                              Siempre y cuando sean 10 días después de la fecha de entrega al gestor. 
                         --->

                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de presentación 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPres" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecPres" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. De guía / Radicación 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroGuia" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">CALIFICACIÓN Y GRADUACIÓN DEL CRÉDITO
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de traslado del proyecto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecTrasProy" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecTrasProy" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Límite para presentar objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecLimPreObj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecLimPreObj" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Oficio objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiObj" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha oficio objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecOfiObj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecOfiObj" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha presentación objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPresObj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecPresObj" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha decisión objeciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecDecObj" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecDecObj" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Oficio reposición
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiRep" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha recurso de reposición
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecRecRep" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecRecRep" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Porcentaje del derecho al voto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPorDerVoto" runat="server" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Oficio demanda ante la Superintendencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOfiDemSupInt" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha demanda ante la Superintencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecDemSupInt" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecDemSupInt" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">ACUERDO DE ACREEDORES
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de presentación del acuerdo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPresAcu" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecPresAcu" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de audiencia de confirmación
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecAudConf" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecAudConf" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de terminación del acuerdo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecTermAcu" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecTermAcu" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. De auto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroResAcu" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha del auto
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecResAcu" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecResAcu" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha inicio pago según Acuerdo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecIniPagAcu" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecIniPagAcu" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha Fin pago según acuerdo
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecFinPagAcu" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecFinPagAcu" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">INCUMPLIMIENTO DEL ACUERDO
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">No. Oficio denuncia incumplimiento
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNroOfiDenInc" runat="server" MaxLength="20" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha oficio
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecOfiIncump" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecOfiIncump" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha presentación del oficio
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPresOfiInc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecPresOfiInc" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha audiencia
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecAudInc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecAudInc" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Decisión
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDecisionInc" runat="server" MaxLength="30" CssClass="ui-widget"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de presentación demanda ante Supersociedades
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecPresDemSSInc" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecPresDemSSInc" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">RESULTADO GESTIÓN
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de terminación del proceso concursal
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecFinConCursal" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecFinConCursal" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Observaciones
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacConcursal" runat="server" CssClass="ui-widget" Height="78px"
                                        TextMode="MultiLine" Width="288px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Estado al que se traslada
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="ui-widget" ID="cboEstadoTrasConcur" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha del traslado
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecTrasConcursal" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecTrasConcursal" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de inscripción cámara de comercio
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecInsCamCom" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecInsCamCom" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="ui-widget-header">Fecha de terminación del proceso de cobro
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFecFinCobConcursal" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnBorraFecFinCobConcursal" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                        ToolTip="Borrar fecha" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="cmdSaveConcursal" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        <!-- contenido de tab nivel 2 (consursales-datos) :: fin -->
                    </div>
                    <div id="concursales-tabs-2">
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 1060px; height: 740px;">
                            <%
                expediente = txtNroExp.Text.Trim
                nit = txtIdDeudor.Text.Trim
                nombre = txtNombreDeudor.Text.Trim
                opconsul = 1
                ejecuciones = 1
                etapas = "04"
                '------------------------------------------------------------------------------
                'Datos del iframe. Generacion de formatos de concursales
                cmd = "<iframe src='../cobranzas2.aspx?cedula=" & nombre & "::" & nit & "&expediente=" & expediente & "&refcatastral=" & nit & "&tipo=1&deunom=" & nombre & "&opconsul=" & opconsul & "&ejecuciones=1&etapa=" & etapas & "' width='1060' height='740' scrolling='no' frameborder='0'></iframe>"

                If NivelAccesoPerfil = 8 Then
                    Response.Write(cmd)
                Else
                    If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                        'Si el estado es devuelto o terminado => No crear el iframe
                        cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "PERSUASIVO" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado PERSUASIVO no permiten generar actos de CONCURSALES</label>"
                        Response.Write(cmd)
                    ElseIf tmpNomEstadoExp = "COACTIVO" Then
                        cmd = "<label style='color:Red'>Los expedientes en estado COACTIVO no permiten generar actos de CONCURSALES</label>"
                        Response.Write(cmd)
                    ElseIf idGestorResp <> Session("sscodigousuario") Then
                        cmd = "<label style='color:Red'>Este expediente está a cargo de otro gestor. No permiten adicionar datos</label>"
                        Response.Write(cmd)
                    Else
                        '"Pintar" el iframe
                        Response.Write(cmd)
                    End If

                End If
                '------------------------------------------------------------------------------
                            %>
                        </div>
                        <!-- ------------------------------------------------------------------------------------------------------ -->
                    </div>
                </div>
            </div>
            <div id="tabs-6">
                <div id="tabs-nivel2-facilidades">
                    <ul>
                        <li><a href="#facilidades-tabs-1">Parafiscales</a></li>
                        <li><a href="#facilidades-tabs-2">Multas</a></li>
                        <li><a href="#facilidades-tabs-3">Generar Actos</a></li>
                        <li><a href="#facilidades-tabs-4">Información de la facilidad de pago</a></li>
                    </ul>
                    <div id="facilidades-tabs-1">
                        <!-- contenido de tab nivel 2 (facilidades-datos) :: inicio -->
                        <div id="tabs-nivel3-facilidades">
                            <div id="facilidades-nivel2-parafiscales">
                                <ul>
                                    <li><a href="#facilidades-paraf-proycuotas">Proyectar cuotas</a></li>
                                </ul>
                            </div>
                            <div id="facilidades-paraf-proycuotas">
                                <!-- ------------------------------------------------------------------------------------------------------ -->
                                <%
                expediente = txtNroExp.Text.Trim
                nit = txtIdDeudor.Text.Trim

                '------------------------------------------------------------------------------
                'Datos del iframe
                cmd = "<iframe src='proyectarcuotaparafiscales.aspx?pNit=" & nit & "&pExpediente=" & expediente & "' width='100%' height='740' scrolling='no' frameborder='0'></iframe>"

                If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                    'Si el estado es devuelto o terminado => No crear el iframe
                    cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                    Response.Write(cmd)
                Else
                    '"Pintar" el iframe
                    Response.Write(cmd)
                End If
                '------------------------------------------------------------------------------
                                %>
                                <!-- ------------------------------------------------------------------------------------------------------ -->
                            </div>
                        </div>
                        <!-- contenido de tab nivel 2 (facilidades-datos) :: fin -->
                    </div>
                    <div id="facilidades-tabs-2">
                        <!-- contenido de tab nivel 2 (facilidades-generador) :: inicio xxx -->
                        <div id="tabs-nivel3-multas">
                            <div id="multas-nivel2-parafiscales">
                                <ul>
                                    <li><a href="#multas-paraf-proycuotas">Proyectar cuotas</a></li>
                                </ul>
                            </div>
                            <div id="multas-paraf-proycuotas">
                                <!-- ------------------------------------------------------------------------------------------------------ -->
                                <%
                expediente = txtNroExp.Text.Trim
                cmd = "<iframe src='proyectarcuotaMULTAS.aspx?pExpediente=" & expediente & "&pValorDeuda=" & txtSaldoEA.Text & "' width='960' height='740' scrolling='no' frameborder='0'></iframe>"
                                %>
                                <div style="margin-left: 2px; margin-top: 4px; width: 960px; height: 740px;">
                                    <% Response.Write(cmd) %>
                                </div>
                                <!-- ------------------------------------------------------------------------------------------------------ -->
                            </div>
                        </div>
                        <!-- contenido de tab nivel 2 (facilidades-generador) :: fin -->
                    </div>
                    <div id="facilidades-tabs-3">
                        <%
                expediente = txtNroExp.Text.Trim
                nit = txtIdDeudor.Text.Trim
                nombre = txtNombreDeudor.Text.Trim
                opconsul = 1
                ejecuciones = 1
                etapas = "03"
                '------------------------------------------------------------------------------
                'Datos del iframe
                cmd = "<iframe src='../cobranzas2.aspx?cedula=" & nombre & "::" & nit & "&expediente=" & expediente & "&refcatastral=" & nit & "&tipo=1&deunom=" & nombre & "&opconsul=" & opconsul & "&ejecuciones=1&etapa=" & etapas & "' width='100%' height='940' scrolling='no' frameborder='0'></iframe>"

                If NivelAccesoPerfil = 8 Then
                    Response.Write(cmd)
                Else
                    If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                        'Si el estado es devuelto o terminado => No crear el iframe
                        cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                        Response.Write(cmd)
                    ElseIf idGestorResp <> Session("sscodigousuario") Then
                        cmd = "<label style='color:Red'>Este expediente está a cargo de otro gestor. No permiten adicionar datos</label>"
                        Response.Write(cmd)
                    Else
                        '"Pintar" el iframe
                        Response.Write(cmd)
                    End If

                End If
                '------------------------------------------------------------------------------
                        %>
                    </div>
                    <div id="facilidades-tabs-4">
                        <!--  -->
                        <div id="facilidades-nivel3-container">
                            <div id="facilidades-nivel3-enlaces">
                                <ul>
                                    <li><a href="#facilidades-info-gral">Información general</a></li>
                                    <li><a href="#facilidades-info-cuotas">Información cuotas</a></li>
                                </ul>
                            </div>
                            <div id="facilidades-info-gral">
                                <!--  -->
                                <%
                expediente = txtNroExp.Text.Trim
                '------------------------------------------------------------------------------
                'Datos del iframe
                cmd = "<iframe src='Datos_Facilidad_Pago.aspx?pExpediente=" & expediente & "' width='100%' height='840' scrolling='no' frameborder='0'></iframe>"

                If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                    'Si el estado es devuelto o terminado => No crear el iframe
                    cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                    Response.Write(cmd)
                Else
                    '"Pintar" el iframe
                    Response.Write(cmd)
                End If
                '------------------------------------------------------------------------------
                                %>
                            </div>
                            <div id="facilidades-info-cuotas">
                                <%
                expediente = txtNroExp.Text.Trim
                '------------------------------------------------------------------------------
                'Datos del iframe
                cmd = "<iframe src='InformacionCuotasFacilidadPago.aspx?pExpediente=" & expediente & "' width='100%' height='840' scrolling='no' frameborder='0'></iframe>"

                If tmpNomEstadoExp = "DEVUELTO" Or tmpNomEstadoExp = "TERMINADO" Then
                    'Si el estado es devuelto o terminado => No crear el iframe
                    cmd = "<label style='color:Red'>Los expedientes en estado DEVUELTO o TERMINADO no permiten generar actos administrativos</label>"
                    Response.Write(cmd)
                Else
                    '"Pintar" el iframe
                    Response.Write(cmd)
                End If
                '------------------------------------------------------------------------------
                                %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--<div id="tabs-7">
            <p>cuotas partes</p>
        </div>--%>
            <div id="tabs-8">
                <div id="tabs-nivel2-pagos">
                    <ul>
                        <li><a href="#pagos-tabs-1">Registros de pagos</a></li>
                        <li><a href="#pagos-tabs-2">Solicitudes de verificación de pagos</a></li>
                    </ul>
                    <div id="pagos-tabs-1">
                        <div style="margin-left: 2px; margin-top: 4px; width: 100%; height: 740px;">
                            <iframe src="pagosexpediente.aspx?pExpediente=<% Response.Write(txtNroExp.Text.Trim) %>"
                                width="100%" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                    </div>
                    <div id="pagos-tabs-2">
                        <!-- contenido de tab nivel 2 (coactivo) (Mandamiento(s) de pago) :: inicio     -->
                        <div style="margin-left: 2px; margin-top: 4px; width: 760px; height: 740px;">
                            <iframe src="verificacionespago.aspx?pExpediente=<%  Response.Write(Request("ID"))%>"
                                width="760" height="740" scrolling="no" frameborder="0"></iframe>
                        </div>
                        <!-- contenido de tab nivel 2 (coactivo) (Mandamiento(s) de pago) :: fin     -->
                    </div>
                </div>
            </div>
            <div id="tabs-9">
                <!-- -->
                <asp:Label ID="lbltipoTotulo" runat="server" Text="Label" Visible="false"></asp:Label>
                <div id="tabs-nivel2-intereses">
                    <%        
                If lbltipoTotulo.Text = "01" Or lbltipoTotulo.Text = "02" Or lbltipoTotulo.Text = "04" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-1'>" & txtTIPOTITULO.Text & " </a></li></ul>")
                    Response.Write("<div id='intereses-tabs-1'> <div style='margin-left:2px; margin-top:4px; width:1239px; height:1239px;'><iframe src='capturarintereses.aspx?pExpediente= " & txtNroExp.Text.Trim & " &pNitCedula=" & txtIdDeudor.Text.Trim & "&pValorDeuda= " & txtSaldoEA.Text.Trim & "' width='1239px' height='1239px' scrolling='no' frameborder='0'></iframe></div></div>")

                ElseIf lbltipoTotulo.Text = "05" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-2'>" & txtTIPOTITULO.Text & " </a></li></ul>")
                    Response.Write("<div id='intereses-tabs-2'> " &
                                   "<div style='margin-left:2px; margin-top:4px; width:100%; height:740px;'> " &
                                   " <iframe src='capturarinteresesmulta2.aspx?pExpediente=" & txtNroExp.Text.Trim & "'  width='100%' height='740' scrolling='no' frameborder='0'></iframe></div></div>")

                ElseIf lbltipoTotulo.Text = "07" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-3'>" & txtTIPOTITULO.Text & " </a></li></ul>")
                    'Response.Write("<div id='intereses-tabs-3'> " & _
                    '               "<div style='margin-left:2px; margin-top:4px; width:100%; height:740px;'> " & _
                    '               " <iframe src='capturarinteresesmulta.aspx?pExpediente=" & txtNroExp.Text.Trim & "&totalDeuda= " & txtTotalDeudaEA.Text & "'  width='100%' height='740' scrolling='no' frameborder='0'></iframe></div></div>")


                ElseIf lbltipoTotulo.Text = "06" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-1'>" & txtTIPOTITULO.Text & " IPC</a></li><li><a href='#intereses-tabs-2'>" & txtTIPOTITULO.Text & " INTERESES</a></li></ul>")
                    Response.Write("<div id='intereses-tabs-1'> " &
                               "<div style='margin-left:2px; margin-top:4px; width:100%; height:740px;'> " &
                               " <iframe src='capturainteresSentenciajudicial_IPC.aspx?pExpediente=" & txtNroExp.Text.Trim & "'  width='100%' height='740' scrolling='no' frameborder='0'></iframe></div></div>")
                    Response.Write("<div id='intereses-tabs-2'> " &
                                  "<div style='margin-left:2px; margin-top:4px; width:100%; height:740px;'> " &
                                  " <iframe src='capturarinteresesmulta2.aspx?pExpediente=" & txtNroExp.Text.Trim & "'  width='100%' height='740' scrolling='no' frameborder='0'></iframe></div></div>")


                ElseIf lbltipoTotulo.Text = "08" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-1'>" & txtTIPOTITULO.Text & " </a></li> </ul>")
                    Response.Write("<div id='intereses-tabs-1'> <div style='margin-left:2px; margin-top:4px; width:1239px; height:1239px;'><iframe src='capturarintereses.aspx?pExpediente= " & txtNroExp.Text.Trim & " &pNitCedula=" & txtIdDeudor.Text.Trim & "&pValorDeuda= " & txtSaldoEA.Text.Trim & "' width='1239px' height='1239px' scrolling='no' frameborder='0'></iframe></div></div>")
                    'Response.Write("<div id='intereses-tabs-2'> " & _
                    '           "<div style='margin-left:2px; margin-top:4px; width:100%; height:740px;'> " & _
                    '           " <iframe src='capturarinteresesmulta2.aspx?pExpediente=" & txtNroExp.Text.Trim & "'  width='100%' height='740' scrolling='no' frameborder='0'></iframe></div></div>")
                ElseIf lbltipoTotulo.Text = "11" Or lbltipoTotulo.Text = "12" Then
                    Response.Write("<ul> <li><a href='#intereses-tabs-1'>" & txtTIPOTITULO.Text & " </a></li> </ul>")
                    Response.Write("<div id='intereses-tabs-1'> <div style='margin-left:2px; margin-top:4px; width:1239px; height:1239px;'><iframe src='LiquidacionInteresesDTF.aspx?pExpediente= " & txtNroExp.Text.Trim & " &pNitCedula=" & txtIdDeudor.Text.Trim & "&pValorDeuda= " & txtSaldoEA.Text.Trim & "' width='1239px' height='1239px' scrolling='no' frameborder='0'></iframe></div></div>")
                End If

                    %>
                    <%--  <div id="intereses-tabs-5">
	                            <div style="margin-left:2px; margin-top:4px; width:1239px; height:1239px;">
                                    <iframe src="capturainteresSentenciajudicial_IPC.aspx?pExpediente=<% Response.Write(txtNroExp.text.trim) %>&pNitCedula=<% Response.Write(txtIdDeudor.text.trim) %>&pValorDeuda=<% Response.Write(txtSaldoEA.text.trim) %>" width="1239px" height="1239px" scrolling="no" frameborder="0"></iframe>
                                                                                                                    			                    </div>
	                      </div>--%>
                </div>
                <!-- ---------------------------------------------------------------------------------------------------------------  -->
            </div>
            <div id="tabs-10">
                <div style="margin-left: 2px; margin-top: 4px; width: 100%; height: 740px;">
                    <iframe src="mensajes.aspx?modo=2&pExpediente=<% Response.Write(txtNroExp.text.trim) %>"
                        width="100%" height="740" scrolling="no" frameborder="0"></iframe>
                </div>
            </div>
            <div id="tabs-11">
                <div style="margin-left: 2px; margin-top: 4px; width: 100%; height: 740px;">
                    <table id="tblEditSUSPENSION" class="ui-widget-content">
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:CustomValidator ID="CustomValidatorSUSPENSION" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td colspan="2">Suspensión del proceso
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="ui-widget-header">Causal
                            </td>
                            <td>
                                <asp:DropDownList CssClass="ui-widget" ID="cboCausalSusp" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="ui-widget-header">No. Resolución
                            </td>
                            <td>
                                <asp:TextBox ID="txtNroResSusp" runat="server" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="ui-widget-header">Fecha
                            </td>
                            <td>
                                <asp:TextBox ID="txtFecResSusp" runat="server" CssClass="ui-widget" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnBorraFecResSusp" runat="server" ImageUrl="../images/icons/borrar16x16.png"
                                    ToolTip="Borrar fecha de Resolución de suspensión del proceso" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td class="ui-widget-header">Observaciones
                            </td>
                            <td>
                                <textarea id="taObsSuspension" cols="90" rows="6" runat="server" style="border: 1px solid #a9a9a9; width: 614px;"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td></td>
                            <td>
                                <asp:Button ID="cmdSaveSuspension" runat="server" Text="Guardar" CssClass="PCGButton"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="tabs-12">
                <div style="margin-left: 2px; margin-top: 4px; width: 100%; height: 740px;">
                    <iframe src="EditCUOTASPARTES.aspx?pExpediente=<% Response.Write(txtNroExp.text.trim) %>"
                        width="100%" height="740" scrolling="no" frameborder="0"></iframe>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="selected_tab" runat="server" />
        <asp:Panel ID="pnlError" runat="server" Style="width: 524px; position: static; display: none; margin-top: 0px; margin-left: 30px">
            <div style="margin: 0  0 5px 0;">
                <% 
                If Not Me.ViewState("Erroruseractivo") Is Nothing Then
                    Response.Write(ViewState("Erroruseractivo"))
                End If
                %>
            </div>
            <hr />
            <asp:Button Style="width: 100px; margin-left: 150px; margin-top: -40px" ID="btnNoerror"
                runat="server" Text="Iniciar sesión" Height="23px"></asp:Button>
        </asp:Panel>
        <asp:Button ID="Button3" runat="server" Text="Button" Style="visibility: hidden" />
        <asp:ModalPopupExtender ID="ModalPopupError" runat="server" TargetControlID="Button3"
            PopupControlID="pnlError" CancelControlID="btnNoerror" DropShadow="false">
        </asp:ModalPopupExtender>

        <script type="text/javascript">
            function mpeSeleccionOnCancel() {
                var pagina = '../../Login.aspx'
                location.href = pagina
                return false;
            }
        </script>

        <script src="<%=ResolveClientUrl("~/js/functions.js") %>" type="text/javascript"></script>
        <script src="<%=ResolveClientUrl("~/js/main.js") %>" type="text/javascript"></script>

    </form>
</body>
</html>

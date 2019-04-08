/**
* Libreria con funciones relacionadas con ClasificaciónManual.aspx
*
* author Yeferson Alba
*/
jQuery(document).ready(function ($) {
    //debugger;
    /**
    * Función que me permite ocultar los controles de clasificación manual una vez se carga la página
    */
    function ocultarControlesClasificacionManual() {
        ocultarPersonaJuridica();
        ocultarPersonaNatural();
        bloquearComplemento();
    };
    /**
    * Función que me permite ocultar los controles relacionados a la persona juridica
    */
    function ocultarPersonaJuridica() {
        $("#Juridica, #JuridicaCarga").hide();
    };
    /**
    * Función que me permite ocultar los controles relacionados a la persona natural
    */
    function ocultarPersonaNatural() {
        $("#Natural, #NaturalCarga").hide();
    };
    /**
    * Función que me permite bloquear los controles relacionados a la persona juridica
    */
    function bloquearComplementoJurdica() {
        $("#fuCargarArchivoJuridica").attr('disabled', 'true');
        $("#txtCargarArchivoJuridica").attr('disabled', 'true');
        $("#ddlProcesoEspecialJuridica").attr('disabled', 'true');
        $("#ddlTipoProcesoJuridica").attr('disabled', 'true');
    };
    /**
    * Función que me permite bloquear los controles relacionados a la persona Natural
    */
    function bloquearComplementoNatural() {
        $("#ddlProcesoEspecialNatural").attr('disabled', 'true');
        $("#ddlTipoProcesoNatural").attr('disabled', 'true');
        $("#fuCargarArchivoNatural").attr('disabled', 'true');
    };
    /**
    * Función que me permite bloquear los controles relacionados tanto a la persona juridica como a la persona natural
    */
    function bloquearComplemento() {
        $("#ddlBeneficioTributario").attr('disabled', 'true');
        $("#ddlPagosDeudor").attr('disabled', 'true');
        $("#txtNombreModulo").attr('disabled', 'true');
        $("#TxtAreaObservaciones").attr('disabled', 'true');
    };
    /**
    * Función que me permite habilitar los controles relacionados con la persona juridica
    */
    function mostrarComplementoJurdica() {
        $("#fuCargarArchivoJuridica").removeAttr("disabled");
        $("#ddlProcesoEspecialJuridica").removeAttr("disabled");
        $("#ddlTipoProcesoJuridica").attr('disabled', 'true');
    };
    /**
    * Función que me permite habilitar los controles relacionados con la persona Natural
    */
    function mostrarComplementoNatural() {
        $("#ddlProcesoEspecialNatural").removeAttr("disabled");
        $("#ddlTipoProcesoNatural").attr('disabled', 'true');
        $("#fuCargarArchivoNatural").removeAttr("disabled");
    };
    /**
    * Función que me permite habilitar los controles relacionados con la persona juridica si tiene matricula mercantil vigente
    */
    function habilitarComplementoJurdica() {
        $("#fuCargarArchivoJuridica").removeAttr("disabled");
        $("#ddlProcesoEspecialJuridica").removeAttr("disabled");
        $("#ddlTipoProcesoJuridica").removeAttr("disabled");
    };
    /**
   * Función que me permite inhabilitar los controles relacionados con la persona Natural 
   */
    function inhabilitarComplementoJurdica() {
        $("#fuCargarArchivoJuridica").attr('disabled', 'true');
        $("#ddlProcesoEspecialJuridica").attr('disabled', 'true');
        $("#ddlTipoProcesoJuridica").attr('disabled', 'true');
    };
    /**
    * Función que me permite habilitar los controles relacionados con la persona Natural
    */
    function habilitarComplementoNatural() {
        $("#ddlProcesoEspecialNatural").removeAttr("disabled");
        $("#ddlTipoProcesoNatural").removeAttr("disabled");
        $("#fuCargarArchivoNatural").removeAttr("disabled");
    };
    /**
    * Función que me permite inhabilitar los controles relacionados con la persona Natural 
    */
    function inhabilitarComplementoNatural() {
        $("#ddlProcesoEspecialNatural").attr('disabled', 'true');
        $("#ddlTipoProcesoNatural").attr('disabled', 'true');
        $("#fuCargarArchivoNatural").attr('disabled', 'true');
    };
    /**
    * Función que me permite habilitar los controles relacionados tanto a la persona juridica como a la persona natural
    */
    function mostrarComplemento() {
        $("#ddlBeneficioTributario").removeAttr("disabled");
        $("#ddlPagosDeudor").removeAttr("disabled");
        $("#txtNombreModulo").removeAttr("disabled");
        $("#TxtAreaObservaciones").removeAttr("disabled");
    };
    /**
    * Función que me permite habilitar los controles relacionados a la persona juridica
    */
    function mostrarPersonaJuridica() {
        $("#Juridica, #JuridicaCarga").show();
    };
    /**
    * Función que me permite habilitar los controles relacionados a la persona natural
    */
    function mostrarPersonaNatural() {
        $("#Natural, #NaturalCarga").show();
    };

    /*
     * Cambiar de posición la selección de documento dependiendo de la selección 
     */
    $(document).on("change", "#ddlTipoPersona", function () {
        var _selected = $("option:selected", $(this)).val();
        if (_selected == 1 || _selected == 2) {
            $("#div_upload_file").show("fast")
        } else {
            $("#div_upload_file").hide("slow")
        }
        //if (_selected == 1) {
        //    var _html = $("#load-doacument-1").html()
        //    if (_html != "") {
        //        $("#load-doacument-2").html("").html(_html);
        //    }
        //} else if(_selected == 2){
        //    var _html = $("#load-doacument-2").html()
        //    if (_html != "") {
        //        $("#load-doacument-1").html("").html(_html);
        //    }
        //}
    });

    /**
    * Función que me permite habilitar los controles relacionados a la persona natural o juridica dependiendo de la selección en el tipo persona
    */
    $('#ddlTipoPersona').change(function () {
        var valueDdlTipoPersona = $("#ddlTipoPersona option:selected").val();
        switch (valueDdlTipoPersona) {
            case '1':
                ocultarPersonaNatural();
                mostrarPersonaJuridica();
                inhabilitarComplementoJurdica()
                restaurarcontroles()
                bloquearComplemento();
                break;
            case '2':
                ocultarPersonaJuridica();
                mostrarPersonaNatural();
                inhabilitarComplementoNatural()
                restaurarcontroles()
                bloquearComplemento();
                break;
            default:
                ocultarPersonaJuridica();
                ocultarPersonaNatural();
                inhabilitarComplementoJurdica()
                limpiarcontroles();
                bloquearComplemento();
        }
    });
    /**
    * Función que me permite habilitar o inhabilitar los controles relacionados a la persona juridica dependiendo de la selección de matricula mercantil
    */
    $('#ddlMatriculaMercantil').change(function () {
        var ValueDdlMatriculaMercantil = $("#ddlMatriculaMercantil option:selected").val();
        switch (ValueDdlMatriculaMercantil) {
            case '1':
                mostrarComplementoJurdica();
                break;
            case '2':
                bloquearComplementoJurdica();
                $("#ddlProcesoEspecialJuridica").val(0);
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
                break;
            default:
                bloquearComplementoJurdica();
                $("#ddlProcesoEspecialJuridica").val(0);
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
        }
    });
    /**
    * Función que me permite habilitar o inhabilitar los controles relacionados a la persona Natural dependiendo de la selección de ddlPersonaViva
    */
    $('#ddlPersonaViva').change(function () {
        var ValueDdlPersonaViva = $("#ddlPersonaViva option:selected").val();
        switch (ValueDdlPersonaViva) {
            case '1':
                mostrarComplementoNatural()
                break;
            case '2':
                bloquearComplementoNatural()
                $("#ddlProcesoEspecialNatural").val(0);
                break;
            default:
                bloquearComplementoNatural()
                $("#ddlProcesoEspecialNatural").val(0);
        }
    });

    /**
    * Función que me permite habilitar o inhabilitar los controles relacionados al complemento 
    */
    $('#ddlPagosDeudor').change(function () {
        var ValueDdlPagosDeudor = $("#ddlPagosDeudor option:selected").val();
        switch (ValueDdlPagosDeudor) {
            case '1':
                mostrarComplementofinal()
                break;
            case '2':
                limpiarcontrolesfinal()
                bloquearComplementofinal() 
                break;
            default:
                limpiarcontrolesfinal()
                bloquearComplementofinal()
        }
    });

    /**
    * Función que me permite habilitar o inhabilitar tipo de proceso juridico dependiendo de la selección de ddlProcesoEspecialJuridica
    */
    $('#ddlProcesoEspecialJuridica').change(function () {
        var ValueDdlProcesoEspecialJuridica = $("#ddlProcesoEspecialJuridica option:selected").val();
        switch (ValueDdlProcesoEspecialJuridica) {
            case '1':
                $("#ddlTipoProcesoJuridica").removeAttr("disabled");
                break;
            case '2':
                $("#ddlTipoProcesoJuridica").attr('disabled', 'true');
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
                break;
            default:
                $("#ddlTipoProcesoJuridica").attr('disabled', 'true');
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
        }
    });
    /**
    * Función que me permite habilitar o inhabilitar tipo de proceso natural dependiendo de la selección de ddlProcesoEspecialNatural
    */
    $('#ddlProcesoEspecialNatural').change(function () {
        var ValueDdlProcesoEspecialNatural = $("#ddlProcesoEspecialNatural option:selected").val();
        console.log(ValueDdlProcesoEspecialNatural)
        switch (ValueDdlProcesoEspecialNatural) {
            case '1':
                $("#ddlTipoProcesoNatural").removeAttr("disabled");
                break;
            case '2':
                $("#ddlTipoProcesoNatural").attr('disabled', 'true');
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
                break;
            default:
                $("#ddlTipoProcesoNatural").attr('disabled', 'true');
                restaurarcontrolesProcEspecial()
                bloquearComplemento()
        }
    });
    /**
    * Función que me permite habilitar el complemento una vez se seleccione el tipo de proceso de persona natural
    */
    $('#ddlTipoProcesoNatural').change(function () {
        var ValueDdlTipoProcesoNatural = $("#ddlTipoProcesoNatural option:selected").val();

        if (ValueDdlTipoProcesoNatural != '-1') {
            mostrarComplemento();
        }
        else {
            bloquearComplemento();
            restaurarcontrolesComplemento()
        }
    });
    /**
    * Función que me permite habilitar el complemento una vez se seleccione el tipo de proceso de persona juridica
    */
    $('#ddlTipoProcesoJuridica').change(function () {
        var ValueDdlTipoProcesoJuridica = $("#ddlTipoProcesoJuridica option:selected").val();
        if (ValueDdlTipoProcesoJuridica != '-1') {
            mostrarComplemento();
        }
        else {
            bloquearComplemento();
            restaurarcontrolesComplemento();
        }
    });

    /**
    * Función que me permite habilitar los controles relacionados al complemento final
    *  */
    function mostrarComplementofinal() {             
        $("#txtNombreModulo").removeAttr("disabled");
        $("#TxtAreaObservaciones").removeAttr("disabled");
    };

    
    /**
    * Función que me permite bloquear los controles relacionados al complemento final
    */
    function bloquearComplementofinal() {       
        $("#txtNombreModulo").attr('disabled', 'true');
        $("#TxtAreaObservaciones").attr('disabled', 'true');
    };

    /**
   * Función que me permite limpiar controles
   */
    function limpiarcontrolesfinal() {       
        $("#txtNombreModulo").val("");
        $("#TxtAreaObservaciones").val("");
    };

    /**
    * Función que me permite limpiar todos los controles
    */
    function limpiarcontroles() {
        $("#ddlTipoPersona").val(0);
        $("#ddlMatriculaMercantil").val(0);
        $("#fuCargarArchivoJuridica").val("");
        $("#ddlProcesoEspecialJuridica").val(0);
        $("#ddlTipoProcesoJuridica").val(-1);
        $("#ddlPersonaViva").val(0);
        $("#fuCargarArchivoNatural").val("");
        $("#ddlProcesoEspecialNatural").val(0);
        $("#ddlTipoProcesoNatural").val(-1);
        $("#ddlBeneficioTributario").val(0);
        $("#ddlPagosDeudor").val(0);
        $("#txtNombreModulo").val("");
        $("#TxtAreaObservaciones").val("");
    };
    /**
  * Función que me permite limpiar los controles menos ddlTipoPersona
  */
    function restaurarcontroles() {
        $("#ddlMatriculaMercantil").val(0);
        $("#fuCargarArchivoJuridica").val("");
        $("#ddlProcesoEspecialJuridica").val(0);
        $("#ddlTipoProcesoJuridica").val(-1);
        $("#ddlPersonaViva").val(0);
        $("#fuCargarArchivoNatural").val("");
        $("#ddlProcesoEspecialNatural").val(0);
        $("#ddlTipoProcesoNatural").val(-1);
        $("#ddlBeneficioTributario").val(0);
        $("#ddlPagosDeudor").val(0);
        $("#txtNombreModulo").val("");
        $("#TxtAreaObservaciones").val("");
    };

    function restaurarcontrolesProcEspecial() {
        $("#ddlTipoProcesoJuridica").val(-1);
        //$("#ddlPersonaViva").val(0);
        $("#fuCargarArchivoNatural").val("");
        //$("#ddlProcesoEspecialNatural").val(0);
        $("#ddlTipoProcesoNatural").val(-1);
        $("#ddlBeneficioTributario").val(0);
        $("#ddlPagosDeudor").val(0);
        $("#txtNombreModulo").val("");
        $("#TxtAreaObservaciones").val("");
    };

    function restaurarcontrolesComplemento() {
        $("#ddlBeneficioTributario").val(0);
        $("#ddlPagosDeudor").val(0);
        $("#txtNombreModulo").val("");
        $("#TxtAreaObservaciones").val("");
    };

    function deshabilitarBtnGuardar() {
        var valueDdlTipoPersona = $("#ddlTipoPersona option:selected").val();
        if (valueDdlTipoPersona != '-1') {
            $('#btnGuardar').attr("disabled", false);
        }
        else {
            $('#btnGuardar').attr("disabled", true);
        }
    }

    /**
    * oculta los controles una vez se carga la pagina
    */
    ocultarControlesClasificacionManual();
    limpiarcontroles();

});
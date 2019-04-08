Namespace control_informe_x_impuesto
    Public Class cobro_pensiones
        Inherits control_informe_x_impuesto.controlador_de_informes

        Protected Ejecucion_fiscal_cobro_coactivo As String = "SELECT @IMP AS MAN_IMPUESTO,EFINROEXP AS MAN_EXPEDIENTE,EFIFECHAEXP,EFINUMMEMO,EFIEXPORIGEN,EFIFECCAD,EFINIT,EFIVALDEU,EFIVALINT,EFIPAGOSCAP,EFISALDOCAP,EFIMODCOD,EFIUSUASIG ,EFIDEVUELTO,EFIDIFICILCOBRO,EFICODESTEXP,EFIESTUP,EFIANULAR,EFIESTADO,EFIULTPAS,EFIESTADOPAGO,EFIUSUREV,EFIFECENTGES,TitEjecAntig,Caducidad,EstadoPersona,CausalesInc,ProcesoCurso,AcuerdoPago,FecSistema FROM EJEFISGLOBAL WHERE EFINROEXP = @Expediente"


        Protected Ejecucion_fiscal_cobro_coactivo_caratula As String = "SELECT @IMP AS MAN_IMPUESTO,EFINROEXP ,EFIFECHAEXP ,EFINUMMEMO ,EFIEXPORIGEN ,EFIFECCAD ,EFINIT ,EFIVALDEU ,EFIVALINT ,EFIPAGOSCAP ,EFISALDOCAP ,EFIMODCOD ,EFINROTITULO ,EFIUSUASIG ,EFIDEVUELTO ,EFIDIFICILCOBRO ,EFICODESTEXP ,EFIESTUP ,EFIANULAR,'' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE, '' AS SUB_SERIE,GETDATE() AS MAN_FECHADOC,'' AS USUARIO FROM EJEFISGLOBA WHERE EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto  AND EFIANULAR = 0 "

        'INTERESES
        Protected Ejecucion_fiscal_cobro_coactivo81 As String = "SELECT @IMP AS MAN_IMPUESTO,E.EFINROEXP AS MAN_EXPEDIENTE,'' AS MAN_FECHLA_IQUIDACION,'' AS MAN_LIQOFI, E.EFIGEN AS MAN_REFCATRASTAL,E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES,E.EfiSubDes AS MAN_EFISUBDES,E.EFIPERHAS AS MAN_EFIPERHAS,E.EfiSubHas AS MAN_EFISUBHAS,SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_INTERES) AS MAN_INTERESES, DBO.TOTALINTERES(E.EFINROEXP) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE ,L.EDC_PREDIO AS MAN_PRENUM,GETDATE() AS MAN_FECHADOC FROM EJEFISGLOBAL E,EJEFISGLOBALLIQUIDAD L WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND EDC_ESTRATO BETWEEN @Estrato AND @EstratoHasta AND EFIPERDES BETWEEN @VigenciaInicial AND @VigenciaFinal GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO HAVING (SUM(L.EDC_TOTALDEUDA) >= @Totaldeuda AND SUM(L.EDC_TOTALDEUDA) <= @TotaldeudaHasta)"
        'Dim sql3 As String = "SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,'' AS MAN_FECHLA_IQUIDACION,'' AS MAN_LIQOFI, E.EFIGEN AS MAN_REFCATRASTAL,E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES,E.EfiSubDes AS MAN_EFISUBDES,E.EFIPERHAS AS MAN_EFIPERHAS,E.EfiSubHas AS MAN_EFISUBHAS,SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_INTERES) AS MAN_INTERESES,(SUM(L.EDC_TOTALDEUDA)* 2) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE ,L.EDC_PREDIO AS MAN_PRENUM,GETDATE() AS MAN_FECHADOC,@USUARIO AS USUARIO  FROM EJEFISGLOBAL E,EJEFISGLOBALLIQUIDAD L WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND EDC_ESTRATO BETWEEN @Estrato AND @EstratoHasta AND EFIPERDES BETWEEN @VigenciaInicial AND @VigenciaFinal AND (EfiMatInm IS NOT NULL AND EfiMatInm <> '') AND EFIANULAR = 0 AND EFIUSUASIG = @CODUSU  GROUP BY  E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO HAVING (SUM(L.EDC_TOTALDEUDA) >= @Totaldeuda AND SUM(L.EDC_TOTALDEUDA) <= @TotaldeudaHasta)"
        'Dim sql4 As String = "SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,'' AS MAN_FECHLA_IQUIDACION,'' AS MAN_LIQOFI, E.EFIGEN AS MAN_REFCATRASTAL,E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES,E.EfiSubDes AS MAN_EFISUBDES,E.EFIPERHAS AS MAN_EFIPERHAS,E.EfiSubHas AS MAN_EFISUBHAS,SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_INTERES) AS MAN_INTERESES,(SUM(L.EDC_TOTALDEUDA)* 2) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE ,L.EDC_PREDIO AS MAN_PRENUM,GETDATE() AS MAN_FECHADOC,@USUARIO AS USUARIO  FROM EJEFISGLOBAL E,EJEFISGLOBALLIQUIDAD L WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND EDC_ESTRATO BETWEEN @Estrato AND @EstratoHasta AND EFIPERDES BETWEEN @VigenciaInicial AND @VigenciaFinal AND (EfiMatInm IS NOT NULL AND EfiMatInm <> '') AND EFIANULAR = 0 GROUP BY  E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO HAVING (SUM(L.EDC_TOTALDEUDA) >= @Totaldeuda AND SUM(L.EDC_TOTALDEUDA) <= @TotaldeudaHasta)"
        'Protected Ejecucion_fiscal_cobro_coactivo81 As String = IIf(mnivelacces = 2, sql3, sql4)



        Protected Ejecucion_fiscal_cobro_coactivo025 As String = "SELECT @imp as man_impuesto,E.EfiCon,P.EfiResNum AS MAN_ESTRATOCD,P.EfiResFec AS MAN_FECHARAC, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L ,EJEFIS2 P WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente  AND P.EfiCon = E.EfiCon AND P.EfiResTip = '202' AND E.EfiModCod = @Impuesto GROUP BY E.EfiCon,P.EfiResNum,P.EfiResFec,E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected NotificacionMP As String = "SELECT * FROM entra_documentoma WHERE doc_expediente = @Expediente and doc_actoadministrativo = @actoadmin"
        'MASIVO (Consultas masivas de expedientes.)
        Protected Masivo_Ejecucion_fiscal_cobro_coactivo As String = "SELECT @imp as man_impuesto, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND E.Efiest = 0 GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"

        Public Overloads Overrides Function Informe_retorno() As Object
            Dim retornaDatos As String = ""

            Select Case acto_paso_administrativo
                Case "001"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "013"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "056"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "217"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "218"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "219"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "220"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "221"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "222"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "223"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "224"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "225"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "226"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "227"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "228"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "229"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "230"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "232"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "233"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "235"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "236"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "237"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "238"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "239"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "240"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "241"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "242"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "243"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "244"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "301"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "302"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "303"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "304"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "305"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "306"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "307"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "308"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "310"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "311"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "312"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "312", "314", "315", "316", "317", "318", "319", "320", "328", "320", "321", "322", "324", "325", "323", "326", "327", "351", "352", "353", "354", "355", "356", "357", "358", "359", "364", "365", "366", "367", "368", "369", "370", "380"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "313", "329", "330", "331", "332", "334", "335", "333", "336", "337", "338", "339", "340", "341", "342", "343", "344", "345", "346", "347", "348", "349", "350", "360", "361", "362", "363", "245"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo

            End Select

            Return retornaDatos
        End Function

        'Sub xxx()
        '    'ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        '    ''Dim formulario As Form
        '    'Dim tipoObjeto As Type = Type.GetType(ApplicationTitle & "." & ssForm)
        '    'Dim objeto As Object = Activator.CreateInstance(tipoObjeto)
        '    'formulario = DirectCast(objeto, Form)
        '    'formulario.MdiParent = Me
        '    'formulario.Show()
        'End Sub

        Public Overrides Function QueReporte() As CrystalDecisions.CrystalReports.Engine.ReportDocument
            Select Case acto_paso_administrativo
                Case "001"
                    Return New GeneracionCaratula_ugpp
                Case "013"
                    Return New Mandamiento_de_pago_pen
                Case "036"
                    Return New Levanta_Medidas_Cautelares
                Case "056"
                    Return New NOTF_X_CORREO
                Case "217"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "218"
                    Return New SEGUNDO_PERSUASIVO_PAGO_PILA
                Case "219"
                    Return New PRIMER_PERSUASIVO_PAGO_DIRECTO_FOSYGA
                Case "220"
                    Return New SEGUNDO_PERSUASIVO_PAGO_DIRECTO_FOSYGA
                Case "221"
                    Return New PRIMER_PERSUASIVO_MULTA_PAGO_DIRECTO_FOSYGA
                Case "222"
                    Return New OFICIO_TRASLADO_AL_MINISTERIO_PUBLICO
                Case "223"
                    Return New FORMATO_SOLICITANDO_DOC_CUANDO_SE_REPORTA_PAGO
                Case "224"
                    Return New OFICIO_RESPUESTA_VERIFICACION_DE_PAGO
                Case "225"
                    Return New ACTA_ENTREGA_INTERNA_EXPEDIENTES
                Case "226"
                    Return New RESOL_TERMINACION_ARCHIVO
                Case "227"
                    Return New OFICIO_RESOL_TERMINACION_ARCHIVO
                Case "228"
                    Return New DEVOLUCION_TESORERIA
                Case "229"
                    Return New Levanta_Medidas_Cautelares
                Case "230"
                    Return New LIQUIDACION_DE_CREDITO_Y_COSTAS
                Case "232"
                    Return New DEVOLUCION_DE_TITULOS_2
                Case "233"
                    Return New APROBACION_DE_LIQUIDACION_DEL_CREDITO
                Case "235"
                    Return New AV_VILLAS_LEVANTA_EMBARGO
                Case "236"
                    Return New Auto_que_ordena_la_suspension_del_proceso_por_facilidad_de_pago
                Case "237"
                    Return New Auto_que_ordena_la_suspension_del_proceso_por_tramite_concursal_y_levantamiento_de_medidas_cautelares
                Case "238"
                    Return New Escrito_de_objeciones_al_proyecto_de_graduacion_de_creditos
                Case "239"
                    Return New Escrito_de_objeciones_al_proyecto_de_inventario_de_bienes_y_gastos_del_proceso
                Case "240"
                    Return New Escrito_de_respuesta_a_objeciones_de_otros_acreedores_inventario_de_bienes
                Case "241"
                    Return New Escrito_de_respuesta_a_objeciones_de_otros_acreedores
                Case "242"
                    Return New Escrito_denuncia_incumplimiento_acuerdo
                Case "243"
                    Return New Escrito_para_la_reunion_de_determinacion_de_votos_y_acreencias
                Case "244"
                    Return New Oficio_de_presentacion_del_credito_proceso_concursal
                Case "301"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "302"
                    Return New SEGUNDO_PERSUASIVO_PAGO_PILA
                Case "303"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "304"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "305"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "306"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "307"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "308"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "310"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "311"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "312", "314", "315", "316", "317", "328", "320", "321", "322", "319", "323", "324", "325", "326", "327", "344", "345", "346", "347", "348", "349", "355", "356", "360", "362", "361", "362", "363", "364", "365", "366", "367", "368", "369", "370"
                    Return New PRIMER_PERSUASIVO_PAGO_PILA
                Case "313", "318", "329", "330", "331", "332", "333", "334", "335", "336", "337", "338", "339", "340", "341", "342", "343", "350", "351", "352", "353", "354", "357", "358", "359", "363", "364", "365", "245", "380"

                    Return New PRIMER_PERSUASIVO_PAGO_PILA
            End Select
        End Function
    End Class
End Namespace


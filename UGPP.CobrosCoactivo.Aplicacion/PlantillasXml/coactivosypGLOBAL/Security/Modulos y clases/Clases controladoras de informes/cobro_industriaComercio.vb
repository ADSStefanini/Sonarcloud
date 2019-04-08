Namespace control_informe_x_impuesto
    Public Class cobro_industriaComercio
        Inherits control_informe_x_impuesto.controlador_de_informes

        Protected Ejecucion_fiscal_cobro_coactivo As String = "SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 												     " & _
                                                         " E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                     " & _
                                                         " E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                 " & _
                                                         " E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,   " & _
                                                         " SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE, '' AS SUB_SERIE                                                            " & _
                                                         " FROM EJEFISGLOBAL E, EJEFISGLOBALLIQUIDAD L                                                                                               " & _
                                                         " WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                     " & _
                                                         " AND E.EfiModCod = @Impuesto  	                                                                                                             " & _
                                                         " GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EFISUBHAS                    "

        Protected Ejecucion_fiscal_cobro_coactivo025 As String = "SELECT @imp as man_impuesto,E.EfiCon,P.EfiResNum AS MAN_ESTRATOCD,P.EfiResFec AS MAN_FECHARAC, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L ,EJEFIS2 P WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente  AND P.EfiCon = E.EfiCon AND P.EfiResTip = '202' AND E.EfiModCod = @Impuesto GROUP BY E.EfiCon,P.EfiResNum,P.EfiResFec,E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected NotificacionMP As String = "SELECT * FROM [entra_documentoma] WHERE doc_expediente = @Expediente and doc_actoadministrativo = @actoadmin"
        'MASIVO (Consultas masivas de expedientes.)
        Protected Masivo_Ejecucion_fiscal_cobro_coactivo As String = "SELECT @imp as man_impuesto, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND E.Efiest = 0 GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected liquidaciondecredito As String = "SELECT * FROM LIQUIDACION_CREDITO WHERE LC_EXPEDIENTE = @Expediente AND LC_ACTOADMIN = @actoadmin"

        Protected Ejecucion_fiscal_cobro_coactivo81 As String = "SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,'' AS MAN_FECHLA_IQUIDACION,'' AS MAN_LIQOFI, E.EFIGEN AS MAN_REFCATRASTAL,E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES,E.EfiSubDes AS MAN_EFISUBDES,E.EFIPERHAS AS MAN_EFIPERHAS,E.EfiSubHas AS MAN_EFISUBHAS,SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_INTERES) AS MAN_INTERESES,(SUM(L.EDC_TOTALDEUDA)* 2) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE ,L.EDC_PREDIO AS MAN_PRENUM,GETDATE() AS MAN_FECHADOC  FROM EJEFISGLOBAL E,EJEFISGLOBALLIQUIDAD L WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND EFIANULAR = 0 GROUP BY  E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO "



        Public Overloads Overrides Function Informe_retorno() As Object
            Dim retornaDatos As String = ""
            Select Case acto_paso_administrativo
                Case "001"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "005"
                    retornaDatos = NotificacionMP
                Case "014"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "017"
                    retornaDatos = NotificacionMP
                Case "056"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "067"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo81
                Case "068"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "069"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "081"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo81
                Case "122"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "201"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "202"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                    'Reportes Cesar
                Case "071"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "089"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "101"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "072"
                    retornaDatos = NotificacionMP
                Case "073"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "120"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "160"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "161"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "162"
                    retornaDatos = NotificacionMP
                Case "163"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "164"
                    retornaDatos = NotificacionMP
                Case "007"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "100"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "060"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "075"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "028"
                    retornaDatos = liquidaciondecredito
                Case "204"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "205"
                    retornaDatos = NotificacionMP
                Case "206"
                    retornaDatos = NotificacionMP
                Case "207"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "208"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "209"
                    retornaDatos = NotificacionMP
                Case "210"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "211"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "212"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "213"
                    retornaDatos = NotificacionMP
                Case "097"
                    retornaDatos = NotificacionMP
                Case "070", "203", "013"
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
                    Return New generacion_caratula
                Case "005"
                    Return New in_notificacion_personal_del_mandamiento_de_pago_005
                Case "013"
                    Return New in_mandamiento_de_pago_013
                Case "014"
                    Return New in_citacion_notificacion_personal_mandamiento_de_pago_014
                Case "017"
                    Return New in_notificacion_personal_de_resolucion_que_decide_excepciones_dentro_del_proceso_de_cobro_coactivo_017
                Case "056"
                    Return New in_notificacion_correo_certificado_056
                Case "067"
                    Return New in_carta_embargo_proceso_adminitrativa_cooactivo_suma_dinero
                Case "068"
                    Return New in_embargo_previo_establecimiento_comercio
                Case "069"
                    Return New in_carta_embargo_Previo_establecimiento_comercio
                Case "081"
                    Return New in_decreto_de_embargo_previo_suma_dinero
                Case "122"
                    Return New in_citacion_notificacion_personal_resolucion_que_decide_excepciones_122
                Case "201"
                    Return New in_embargo_previo_inmueble
                Case "202"
                    Return New in_Inscripcion_de_Embargo_Previo_del_inmueble_identificado_con_matricula_Inmobiliaria
                Case "071"
                    Return New in_inscripcion_de_termino_y_levantamiento_de_medida
                Case "089"
                    Return New in_excepcion_de_acuerdo_de_pago
                Case "101"
                    Return New in_citacion_notificacion_personal_resolucion_decreta_suspension_proceso_101
                Case "072"
                    Return New in_resolución_que_ordena_diligencia_de_secuestro_de_inmueble
                Case "073"
                    Return New in_acta_de_diligencia_de_posesión_de_secuestre
                Case "120"
                    Return New in_acuerdo_de_pago_120
                Case "160"
                    Return New in_declara_sin_vigencia_un_acuerdo_de_pago_160
                Case "161"
                    Return New in_citacion_notificacion_resolucion_declara_sin_vigencia_un_acuerdo_de_pago_161
                Case "162"
                    Return New In_diligencia_de_notificacion_personal_declara_sin_vigencia_un_acuerdo_de_pago_162
                Case "163"
                    Return New in_notificacion_por_correo_certificado_declara_sin_vigencia_un_acuerdo_de_pago_163
                Case "164"
                    Return New In_interrumpe_el_proceso_administrativo_de_cobro_coactivo
                Case "007"
                    Return New in_notificación_por_conducta_concluyente_del_mandamiento_de_pago
                Case "100"
                    Return New in_levantamiento_medida_cautelar_embargo_matricula_inmobiliarira
                Case "060"
                    Return New in_acta_de_entrega_de_depósitos_judiciales_060
                Case "075"
                    Return New in_resolución_ordena_aplicación_depósitos_judiciales_075
                Case "028"
                    Return New in_resolucion_que_liquida_el_credito_028
                Case "203"
                    Return New in_oficio_adjunta_resolucion_resuelve_excepciones_203
                Case "204"
                    Return New in_acta_de_posesión_de_perito_avaluador
                Case "205"
                    Return New in_por_medio_de_la_cual_se_aprueba_el_avaluo__realizado_por_perito_o_experto
                Case "206"
                    Return New in_auto_de_traslado_de_avaluo
                Case "207"
                    Return New in_por_medio_de_la_cual_se_decreta_el_embargo_de_vehiculos_automotores_207
                Case "208"
                    Return New in_oficio_comunica_embargo_de_vehiculo_al_transito_208
                Case "209"
                    Return New in_por_medio_de_la_cual_se_decreta_el_embargo_de_salario_del_contribuyente_209
                Case "210"
                    Return New in_oficio_comunica_embargo_de_sueldo
                Case "211"
                    Return New in_minuta_de_cobro_persuasivo_211
                Case "212"
                    Return New in_resolucion_decreta_el_embargo_previo_de_vehiculos_automotor_212
                Case "213"
                    Return New in_por_medio_de_la_cual_se_decreta_el_embargo_previo_de_salario
                Case "097"
                    Return New in_notificacion_personal_de_resolucion_resuelve_excepciones_097
                Case "070"
                    Return New in_oficio_comunica_desembargo_de_bien_inmueble
            End Select
        End Function
    End Class
End Namespace


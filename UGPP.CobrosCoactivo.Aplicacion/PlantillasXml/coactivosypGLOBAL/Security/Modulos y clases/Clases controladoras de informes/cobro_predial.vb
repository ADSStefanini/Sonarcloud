Namespace control_informe_x_impuesto
    Public Class cobro_predial
        Inherits control_informe_x_impuesto.controlador_de_informes
        Protected Ejecucion_fiscal_cobro_coactivo As String = " SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,E.EFIGEN AS MAN_REFCATRASTAL, 												     " & _
                                                            " E.EfiMatInm AS MAN_MATINMOB,  E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,   	                                                     " & _
                                                            " E.EFIDIR AS MAN_DIR_ESTABL,   E.EFIPERDES AS MAN_EFIPERDES, E.EfiSubDes  AS MAN_EFISUBDES,E.EFIPERHAS   AS MAN_EFIPERHAS,	                 " & _
                                                            " E.EfiSubHas AS MAN_EFISUBHAS, SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,   SUM(L.EDC_TOTALABO) AS MAN_PAGOS, SUM(L.EDC_INTERES) AS MAN_INTERESES,   " & _
                                                            " SUM(L.EDC_TOTALDEUDA) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE, '' AS SUB_SERIE                                                            " & _
                                                            " FROM EJEFISGLOBAL E,  EJEFISGLOBALLIQUIDAD L                                                                                               " & _
                                                            " WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente    	                     " & _
                                                            " AND E.EfiModCod = @Impuesto  	                                                                                                             " & _
                                                            " GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EFISUBHAS                    "


        Protected Ejecucion_fiscal_cobro_coactivo025 As String = "SELECT @imp as man_impuesto,E.EfiCon,P.EfiResNum AS MAN_ESTRATOCD,P.EfiResFec AS MAN_FECHARAC, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L ,EJEFIS2 P WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente  AND P.EfiCon = E.EfiCon AND P.EfiResTip = '202' AND E.EfiModCod = @Impuesto GROUP BY E.EfiCon,P.EfiResNum,P.EfiResFec,E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected NotificacionMP As String = "SELECT * FROM entra_documentoma  WHERE doc_expediente = @Expediente and doc_actoadministrativo = @actoadmin "
        'MASIVO (Consultas masivas de expedientes.)
        Protected Masivo_Ejecucion_fiscal_cobro_coactivo As String = "SELECT @imp as man_impuesto, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND E.Efiest = 0 GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected liquidaciondecredito As String = "SELECT * FROM LIQUIDACION_CREDITO  WHERE LC_EXPEDIENTE = @Expediente AND LC_ACTOADMIN = @actoadmin "

        Protected Ejecucion_fiscal_cobro_coactivo81 As String = "SELECT @IMP AS MAN_IMPUESTO, E.EFINROEXP AS MAN_EXPEDIENTE,'' AS MAN_FECHLA_IQUIDACION,'' AS MAN_LIQOFI, E.EFIGEN AS MAN_REFCATRASTAL,E.EfiMatInm AS MAN_MATINMOB,E.EFINIT AS MAN_DEUSDOR,E.EFINOM AS MAN_NOMDEUDOR,E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES,E.EfiSubDes AS MAN_EFISUBDES,E.EFIPERHAS AS MAN_EFIPERHAS,E.EfiSubHas AS MAN_EFISUBHAS,SUM(L.EDC_IMPUESTO) AS MAN_TOTAL,SUM(L.EDC_INTERES) AS MAN_INTERESES,(SUM(L.EDC_TOTALDEUDA)* 2) AS MAN_VALORMANDA, '' AS MAN_ACTOPRE, '' AS MAN_FEACTOPRE ,L.EDC_PREDIO AS MAN_PRENUM,GETDATE() AS MAN_FECHADOC  FROM EJEFISGLOBAL E,EJEFISGLOBALLIQUIDAD L WHERE E.EFIGEN = L.EDC_ID AND L.EDC_VIGENCIA BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto  AND (EfiMatInm IS NOT NULL AND EfiMatInm <> '') AND EFIANULAR = 0 GROUP BY  E.EFINROEXP, E.EFIGEN,E.EFINIT,E.EFINOM, E.EFIDIR, E.EFIPERDES, E.EFIPERHAS, E.EfiMatInm, EfiSubDes, EfiSubHas,L.EDC_PREDIO "


        Public Overloads Overrides Function Informe_retorno() As Object
            Dim retornaDatos As String = ""

            Select Case acto_paso_administrativo
                Case "001"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "002"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "005"
                    retornaDatos = NotificacionMP
                Case "009"
                    Select Case TipoReporte
                        Case 1
                            retornaDatos = Ejecucion_fiscal_cobro_coactivo
                        Case 2
                            retornaDatos = Masivo_Ejecucion_fiscal_cobro_coactivo
                    End Select
                Case "013"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "014"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "016"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "017"
                    retornaDatos = NotificacionMP
                Case "007"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "122"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "023"
                    retornaDatos = NotificacionMP
                Case "024"
                    retornaDatos = NotificacionMP
                Case "025"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "028"
                    retornaDatos = liquidaciondecredito
                Case "029"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "037"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "056"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "057"
                    retornaDatos = "SELECT ENTIDAD AS doc_deudorcedula ,NOMBRE AS doc_deudornombre,DOCEXPEDIENTE AS doc_expediente, DOCPREDIO_REFECATRASTAL AS doc_predio_refecatrastal,FECHARADIC AS doc_fecharac, idacto AS doc_actoadministrativo, PREDIR AS doc_direccion FROM DOCUMENTOS,PREDIOS,entesdbf WHERE entidad = @EnteDeudorPropietario AND DOCANULAR = 0 AND  DOCUMENTOS.COBRADOR = @Ente and idacto = '013' AND Muncod = 1 and preestmun2 = 0 AND PRENUM = DOCPREDIO_REFECATRASTAL AND codigo_nit = entidad ORDER BY FECHARADIC ASC"
                Case "058"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "060"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "067"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo81
                Case "068"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo81
                Case "069"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "070"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "071"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "072"
                    retornaDatos = NotificacionMP
                Case "073"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "075"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "086"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "081"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo81
                Case "082"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "090"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "097"
                    retornaDatos = NotificacionMP
                Case "100"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "101"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "120"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "160"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "161"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "162"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "163"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "164"
                    retornaDatos = NotificacionMP
                Case "036"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "035"
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
                Case "002"
                    Return New Carta_Cobro_Persuasivo_002
                Case "005"
                    Return New notificacion_personal_del_mandamiento_de_pago_005
                Case "009"
                    'Ojo Cambiar esto es un prueba (este no tiene reporte)
                    'Return New rptPlantillaMandamiento_Pago
                Case "013"
                    Return New Mandamiento_de_Pago_013
                Case "014"
                    Return New citacion_notif_personal_mp
                Case "016"
                    Return New rptOrdenaLlevarAdelanteLaEjecucion_016
                Case "017"
                    Return New NOTIFICACION_PERSONAL_DE_RESOLUCION_QUE_DECIDE_EXCEPCIONES_DENTRO_DEL_PROCESO_DE_COBRO_COACTIVO_017
                Case "007"
                    'Return New NOTIFICACIoN_POR_PERSONAL_DE_RESOL_RESUELVE_EXCEPCIONES
                    Return New Notificacion_por_Conducta_Concluyente_del_Mandamiento_de_Pago_007
                Case "035"
                    Return New CITACION_PERSONAL_PARA_NOTIFICAR_LA_RESOLUCION_QUE_DECIDE_EXCEPCIONES_DENTRO_DEL_PROCESO_DE_COBRO_COACTIVO
                Case "023"
                    Return New notifiacion_personal_resolucion_resuelve_recurso_reposicion_023
                Case "024"
                    Return New notificacion_edicto_rrr
                Case "025"
                    Return New rptOrdenaLlevarAdelanteLaEjecucion_016
                Case "028"
                    Return New RESOLUCION_QUE_LIQUIDA_EL_CREDITO
                    ' Return New rptResolucionLiquidaciondeCreditos
                Case "029"
                    Return New notif_correo_resol_liq_cre
                Case "037"
                    Return New rptliquidacion_oficial
                Case "056"
                    'Return New NOTIFICACIoN_POR_PERSONAL_DE_RESOL_RESUELVE_EXCEPCIONES
                    Return New Notificacion_Correo_Certificado_Mandamiento_Pago_056
                Case "057"
                    Return New Resolucion_acumulacion
                Case "058"
                    Return New resol_embargo_veh
                Case "060"
                    Return New ACTA_DE_ENTREGA_DE_DEPOSITOS_JUDICIALES_060
                Case "067"
                    Return New Embargo_067
                Case "068"
                    Return New aDECRETO_DE_EMBARGO_DE_INMUEBLES068
                Case "069"
                    Return New Inscripcion_de_Embargo_Previo_del_inmueble
                Case "070"
                    Return New OFICIO_COMUNICA_DESEMBARGO_DE_SALDOS_BANCARIOS
                Case "071"
                    Return New OFICIO_COMUNICA_DESEMBARGO_DE_BIEN_INMUEBLE
                Case "072"
                    Return New RESOLUCION_QUE_ORDENA_DILIGENCIA_DE_SECUESTRO_DE_INMUEBLE
                Case "073"
                    Return New DILIGENCIA_DE_POSESION_DEL_SECUESTRE
                Case "075"
                    Return New RESOLUCION_ORDENA_APLICACION_DEPOSITOS_JUDICIALES_075
                Case "086"
                    Return New rptResolucion_de_desembargo
                Case "081"
                    Return New DECRETO_DE_EMBARGO_DE_INMUEBLES
                Case "082"
                    Return New OFICIO_CITACION_PARA_NOTIF_PERSONAL
                Case "090"
                    Return New CARTA_A_LA_ALCALDIA_PARA_QUE_SE_APLIQUEN_NUEVOS_VALORES_EN_EL_SISTEMA_DE_IMPUESTOS_AL_CONTRIBUYENTE
                Case "097"
                    Return New NOTIFICACION_PERSONAL_DE_RESOLUCION_RESUELVE_EXCEPCIONES_097
                Case "100"
                    Return New LEVANTAMIENTO_MEDIDA_CAUTELAR_EMBARGO_MATRICULA_INMOBILIARIRA
                Case "101"
                    Return New CITACION_NOTIFICACION_PERSONAL_RESOLUCION_SUSP_COBRO_COACTIVO
                Case "120"
                    Return New ACUERDO_DE_PAGO
                Case "160"
                    Return New DECLARA_SIN_VIGENCIA_UN_ACUERDO_DE_PAGO
                Case "161"
                    Return New CITACION_DECLARA_SIN_VIGENCIA_UN_ACUERDO_DE_PAGO
                Case "162"
                    Return New DILIGENCIA_DE_NOTIFICACION_PERSONAL_DECLARA_SIN_VIGENCIA_UN_ACUERDO_DE_PAGO
                Case "163"
                    Return New NOTIFICACION_POR_CORREO_CERTIFICADO_DECLARA_SIN_VIGENCIA_UN_ACUERDO_DE_PAGO
                Case "164"
                    Return New INTERRUMPE_EL_PROCESO_ADMINISTRATIVO_DE_COBRO_COACTIVO
                Case "036"
                    Return New resolucion_declara_terminado_proceso_036
                Case "122"
                    Return New OFICIO_CITACIÓN_PARA_NOTIFICAR_RESOL_RESUELVE_REC_REPOSICIÓN_122

            End Select
        End Function
    End Class
End Namespace


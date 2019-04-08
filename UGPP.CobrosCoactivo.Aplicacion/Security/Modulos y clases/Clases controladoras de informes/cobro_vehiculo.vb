Namespace control_informe_x_impuesto
    Public Class cobro_vehiculo
        Inherits control_informe_x_impuesto.controlador_de_informes

        'UNO AH UNO 
        Protected Ejecucion_fiscal_cobro_coactivo As String = "SELECT @imp as man_impuesto, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected Ejecucion_fiscal_cobro_coactivo025 As String = "SELECT @imp as man_impuesto,E.EfiCon,P.EfiResNum AS MAN_ESTRATOCD,P.EfiResFec AS MAN_FECHARAC, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +   SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L ,EJEFIS2 P WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS AND E.EFINROEXP = @Expediente  AND P.EfiCon = E.EfiCon AND P.EfiResTip = '202' AND E.EfiModCod = @Impuesto GROUP BY E.EfiCon,P.EfiResNum,P.EfiResFec,E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"
        Protected NotificacionMP As String = "SELECT * FROM [entra_documentoma] WHERE doc_expediente = @Expediente and doc_actoadministrativo = @actoadmin"

        'MASIVO (Consultas masivas de expedientes.)
        Protected Masivo_Ejecucion_fiscal_cobro_coactivo As String = "SELECT @imp as man_impuesto, E.EFINROEXP AS MAN_EXPEDIENTE, E.EFIGEN AS MAN_REFCATRASTAL,E.EFINIT AS MAN_DEUSDOR, E.EFINOM AS MAN_NOMDEUDOR, E.EFIDIR AS MAN_DIR_ESTABL,E.EFIPERDES AS MAN_EFIPERDES, E.EFIPERHAS AS MAN_EFIPERHAS,SUM(L.LIQTOT) AS MAN_TOTAL, SUM(L.LIQTOTABO) AS MAN_PAGOS, SUM(L.LIQINT) AS MAN_INTERESES,(SUM(L.LIQINT) +  SUM(L.LIQTOT)) AS MAN_VALORMANDA FROM EJEFIS E, LIQUIDAD L WHERE E.EFIGEN = L.LIQGEN AND L.PerCod BETWEEN E.EFIPERDES AND E.EFIPERHAS  AND E.EFINROEXP = @Expediente AND E.EfiModCod = @Impuesto AND E.Efiest = 0 GROUP BY E.EFINROEXP, E.EFIGEN,E.EFINIT, E.EFINOM,  E.EFIDIR, E.EFIPERDES, E.EFIPERHAS"

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
                Case "023"
                    retornaDatos = NotificacionMP
                Case "024"
                    retornaDatos = NotificacionMP
                Case "025"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
                Case "028"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
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
                Case "086"
                    retornaDatos = Ejecucion_fiscal_cobro_coactivo
            End Select

            Return retornaDatos
        End Function

        Public Overrides Function QueReporte() As CrystalDecisions.CrystalReports.Engine.ReportDocument
            Select Case acto_paso_administrativo
                
            End Select
        End Function
    End Class
End Namespace


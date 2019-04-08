Public Class MaestroTitulos
    Public Property MT_nro_titulo As String
    Public Property MT_expediente As String
    Public Property MT_tipo_titulo As String
    Public Property MT_titulo_acumulado As String
    Public Property MT_fec_expedicion_titulo As Nullable(Of Date)
    Public Property MT_res_resuelve_reposicion As String
    Public Property MT_fec_expe_resolucion_reposicion As Nullable(Of Date)
    Public Property MT_reso_resu_apela_recon As String
    Public Property MT_fec_exp_reso_apela_recon As Nullable(Of Date)
    Public Property MT_fecha_ejecutoria As Nullable(Of Date)
    Public Property MT_fec_exi_liq As Nullable(Of Date)
    Public Property MT_fec_cad_presc As Nullable(Of Date)
    Public Property MT_proc_cobro_ini_prev As Nullable(Of Boolean)
    Public Property MT_procedencia As Nullable(Of Integer)
    Public Property capitalmulta As Nullable(Of Double)
    Public Property omisossalud As Nullable(Of Double)
    Public Property morasalud As Nullable(Of Double)
    Public Property inexactossalud As Nullable(Of Double)
    Public Property omisospensiones As Nullable(Of Double)
    Public Property morapensiones As Nullable(Of Double)
    Public Property inexactospensiones As Nullable(Of Double)
    Public Property omisosfondosolpen As Nullable(Of Double)
    Public Property morafondosolpen As Nullable(Of Double)
    Public Property inexactosfondosolpen As Nullable(Of Double)
    Public Property omisosarl As Nullable(Of Double)
    Public Property moraarl As Nullable(Of Double)
    Public Property inexactosarl As Nullable(Of Double)
    Public Property omisosicbf As Nullable(Of Double)
    Public Property moraicbf As Nullable(Of Double)
    Public Property inexactosicbf As Nullable(Of Double)
    Public Property omisossena As Nullable(Of Double)
    Public Property morasena As Nullable(Of Double)
    Public Property inexactossena As Nullable(Of Double)
    Public Property omisossubfamiliar As Nullable(Of Double)
    Public Property morasubfamiliar As Nullable(Of Double)
    Public Property inexactossubfamiliar As Nullable(Of Double)
    Public Property sentenciasjudiciales As Nullable(Of Double)
    Public Property cuotaspartesacum As Nullable(Of Double)
    Public Property totalmultas As Nullable(Of Double)
    Public Property totalomisos As Nullable(Of Double)
    Public Property totalmora As Nullable(Of Double)
    Public Property totalinexactos As Nullable(Of Double)
    Public Property totalsentencias As Nullable(Of Double)
    Public Property totaldeuda As Nullable(Of Double)
    Public Property NumMemoDev As String
    Public Property FecMemoDev As Nullable(Of Date)
    Public Property CausalDevol As Nullable(Of Integer)
    Public Property ObsDevol As String
    Public Property totalrepartidor As Nullable(Of Double)
    Public Property estado As Nullable(Of Integer)
    Public Property idunico As Long
    Public Property MT_totalSancion As Nullable(Of Double)
    Public Property MT_tiposentencia As String
    Public Property revocatoria As String
    Public Property nroResolRevoca As Nullable(Of Integer)
    Public Property fechaRevoca As Nullable(Of Date)
    Public Property valorRevoca As Nullable(Of Double)
    Public Property MT_valor_obligacion As Nullable(Of Double)
    Public Property MT_partida_global As Nullable(Of Double)
    Public Property MT_sancion_omision As Nullable(Of Double)
    Public Property MT_sancion_mora As Nullable(Of Double)
    Public Property MT_sancion_inexactitud As Nullable(Of Double)
    Public Property MT_total_obligacion As Nullable(Of Double)
    Public Property MT_total_partida_global As Nullable(Of Double)
    Public Property Automatico As Nullable(Of Boolean)
    Public Property NoExpedienteOrigen As String
    Public Property MT_fec_notificacion_titulo As Nullable(Of Date)
    Public Property MT_for_notificacion_titulo As String
    Public Property MT_fec_not_reso_resu_reposicion As Nullable(Of Date)
    Public Property MT_for_not_reso_resu_reposicion As String
    Public Property MT_fec_not_reso_apela_recon As Nullable(Of Date)
    Public Property MT_for_not_reso_apela_recon As String

    Public Overridable Property MAESTRO_TITULOS_DOCUMENTOS As ICollection(Of MaestroTitulosDocumentos) = New HashSet(Of MaestroTitulosDocumentos)
    'Public Overridable Property MAESTRO_TITULOS_FOR_NOTIFICACION As ICollection(Of MaestroTitulosForNotificacion) = New HashSet(Of MaestroTitulosForNotificacion)
    'Public Overridable Property TIPOS_TITULO As TipoTitulo
    Public Overridable Property EJEFISGLOBAL As EJEFISGLOBAL

End Class

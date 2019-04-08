IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaTituloEjecutivo]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaTituloEjecutivo]
END


GO


-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 31/10/2018
-- Description: INSERTA EL TÍTULO EJECUTIVO


--EXECUTE  [SP_InsertaTituloEjecutivo] @VAL_USUARIO_NOMBRE ='DLEON' ,@MT_nro_titulo= '5154165135',@MT_tipo_titulo= '01',@MT_fec_expedicion_titulo='2018-11-22 13:53:07.737',
--@MT_fec_notificacion_titulo = '2018-11-22 13:53:07.737',@MT_for_notificacion_titulo= '12' ,@MT_Tip_notificacion= 67, @MT_res_resuelve_reposicion= '12',@MT_fec_expe_resolucion_reposicion='2018-11-22 13:53:07.737',
--@MT_reso_resu_apela_recon = '12',@MT_fec_exp_reso_apela_recon= '2018-11-22 13:53:07.737',@MT_fecha_ejecutoria = '2018-11-22 13:53:07.737', @MT_fec_exi_liq= '2018-11-22 13:53:07.737',
--@MT_procedencia = 1, @totaldeuda = 123451,@TotalRepartidor = 123452,@TotalSancion =123453,@MT_valor_obligacion =123454,@MT_partida_global = 123455,@MT_sancion_omision =123456,
--@MT_sancion_mora= 123457,@MT_sancion_inexactitud = 123458,@MT_total_obligacion = 123459,@MT_total_partida_global = 123456,@CodTipSentencia =87

-- =============================================
CREATE PROCEDURE [dbo].[SP_InsertaTituloEjecutivo]    
   @MT_nro_titulo VARCHAR(20),   
   @MT_expediente VARCHAR(12) = NULL,
   @MT_tipo_titulo CHAR(2),
   @MT_fec_expedicion_titulo DATETIME,
   @MT_fec_notificacion_titulo DATETIME,
   @MT_for_notificacion_titulo CHAR(2),
   @MT_Tip_notificacion INT,
   @MT_res_resuelve_reposicion VARCHAR(50),
   @MT_fec_expe_resolucion_reposicion DATETIME,
   @MT_reso_resu_apela_recon VARCHAR(50),
   @MT_fec_exp_reso_apela_recon DATETIME,
   @MT_fecha_ejecutoria DATETIME,
   @MT_fec_exi_liq DATETIME,
   @MT_procedencia INT,
   @totaldeuda FLOAT,
   @TotalRepartidor FLOAT,
   @TotalSancion FLOAT,
   @MT_valor_obligacion FLOAT,
   @MT_partida_global FLOAT,
   @MT_sancion_omision FLOAT,
   @MT_sancion_mora FLOAT , 
   @MT_sancion_inexactitud FLOAT,
   @MT_total_obligacion FLOAT,
   @MT_total_partida_global FLOAT,
   @CodTipSentencia INT,
   @NoExpedienteOrigen VARCHAR(30)=NULL,
   @IdunicoTitulo AS BIGINT= NULL,
   @Automatico BIT
   AS
   
BEGIN                     
      
SET NOCOUNT ON   
        DECLARE @IdunicoTituloR INT = -1 
        DECLARE @capitalmulta AS INT = 0
        DECLARE @omisossalud AS INT = 0
        DECLARE @morasalud AS INT = 0
        DECLARE @inexactossalud AS INT = 0
        DECLARE @omisospensiones AS INT = 0
        DECLARE @morapensiones AS INT = 0
        DECLARE @inexactospensiones AS INT = 0
        DECLARE @omisosfondosolpen AS INT = 0
        DECLARE @morafondosolpen AS INT = 0
        DECLARE @inexactosfondosolpen AS INT = 0
        DECLARE @omisosarl AS INT = 0
        DECLARE @moraarl AS INT = 0
        DECLARE @inexactosarl AS INT = 0
        DECLARE @omisosicbf AS INT = 0
        DECLARE @moraicbf AS INT = 0
        DECLARE @inexactosicbf AS INT = 0
        DECLARE @omisossena AS INT = 0
        DECLARE @morasena AS INT = 0
        DECLARE @inexactossena AS INT = 0
        DECLARE @omisossubfamiliar AS INT = 0
        DECLARE @morasubfamiliar AS INT = 0
        DECLARE @inexactossubfamiliar AS INT = 0
        DECLARE @sentenciasjudiciales AS INT = 0
        DECLARE @cuotaspartesacum AS INT = 0
        DECLARE @totalmultas AS INT = 0
        DECLARE @totalomisos AS INT = 0
        DECLARE @totalmora AS INT = 0
        DECLARE @totalinexactos AS INT = 0
        DECLARE @totalsentencias AS INT = 0


IF @IdunicoTitulo <> 0

	BEGIN

		UPDATE [dbo].[MAESTRO_TITULOS] SET 
           [MT_expediente] = @MT_expediente
           ,[MT_tipo_titulo] = @MT_tipo_titulo
           ,[MT_titulo_acumulado] = '' 
           ,[MT_fec_expedicion_titulo] = @MT_fec_expedicion_titulo
           ,[MT_res_resuelve_reposicion] = @MT_res_resuelve_reposicion
           ,[MT_fec_expe_resolucion_reposicion] = @MT_fec_expe_resolucion_reposicion
           ,[MT_reso_resu_apela_recon] = @MT_reso_resu_apela_recon
           ,[MT_fec_exp_reso_apela_recon] = @MT_fec_exp_reso_apela_recon
           ,[MT_fecha_ejecutoria] = @MT_fecha_ejecutoria
           ,[MT_fec_exi_liq] = @MT_fec_exi_liq
           ,[MT_fec_cad_presc] = CASE 
			@MT_fecha_ejecutoria 
		    WHEN NULL THEN GETDATE()+1
			ELSE 
			DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=@MT_tipo_titulo),CAST(@MT_fecha_ejecutoria AS SMALLDATETIME))   		END
           ,[MT_proc_cobro_ini_prev] = NULL
           ,[MT_procedencia] = @MT_procedencia
           ,[capitalmulta] = @capitalmulta
           ,[omisossalud] = @omisossalud
           ,[morasalud] = @morasalud
           ,[inexactossalud] = @inexactossalud
           ,[omisospensiones] = @omisospensiones
           ,[morapensiones] = @morapensiones
           ,[inexactospensiones] = @inexactospensiones
           ,[omisosfondosolpen] = @omisosfondosolpen
           ,[morafondosolpen] = @morafondosolpen
           ,[inexactosfondosolpen] = @inexactosfondosolpen
           ,[omisosarl] = @omisosarl
           ,[moraarl] = @moraarl
           ,[inexactosarl] = @inexactosarl
           ,[omisosicbf] = @omisosicbf
           ,[moraicbf] = @moraicbf
           ,[inexactosicbf] = @inexactosicbf
           ,[omisossena] = @omisossena
           ,[morasena] = @morasena
           ,[inexactossena] = @inexactossena
           ,[omisossubfamiliar] = @omisossubfamiliar
           ,[morasubfamiliar] = @morasubfamiliar
           ,[inexactossubfamiliar] = @inexactossubfamiliar
           ,[sentenciasjudiciales] = @sentenciasjudiciales
           ,[cuotaspartesacum] = @cuotaspartesacum
           ,[totalmultas] = @totalmultas
           ,[totalomisos] = @totalomisos
           ,[totalmora] = @totalmora
           ,[totalinexactos] = @totalinexactos
           ,[totalsentencias] = @totalsentencias
           ,[totaldeuda] = @totaldeuda
           ,[NumMemoDev] = NULL
           ,[FecMemoDev] = NULL
           ,[CausalDevol] = NULL
           ,[ObsDevol] = NULL
           ,[totalrepartidor] = @TotalRepartidor
           ,[estado] = 1
           ,[MT_totalSancion] = @TotalSancion
           ,[MT_tiposentencia] = @CodTipSentencia
           ,[revocatoria] = NULL
           ,[nroResolRevoca]= NULL
           ,[fechaRevoca]= NULL
           ,[valorRevoca]= NULL
           ,[MT_valor_obligacion] = @MT_valor_obligacion
           ,[MT_partida_global] = @MT_partida_global
           ,[MT_sancion_omision] = @MT_sancion_omision
           ,[MT_sancion_mora] = @MT_sancion_mora
           ,[MT_sancion_inexactitud] = @MT_sancion_inexactitud
           ,[MT_total_obligacion] = @MT_total_obligacion
           ,[MT_total_partida_global] = @MT_total_partida_global
		   ,[Automatico] =  @Automatico
		   ,[NoExpedienteOrigen]=@NoExpedienteOrigen		   
		   WHERE idunico = @IdunicoTitulo

		SET @IdunicoTituloR =1

	END

ELSE

	BEGIN
			INSERT INTO [dbo].[MAESTRO_TITULOS]
					   ([MT_nro_titulo]
					   ,[MT_expediente]
					   ,[MT_tipo_titulo]
					   ,[MT_titulo_acumulado]
					   ,[MT_fec_expedicion_titulo]
					   ,[MT_res_resuelve_reposicion]
					   ,[MT_fec_expe_resolucion_reposicion]
					   ,[MT_reso_resu_apela_recon]
					   ,[MT_fec_exp_reso_apela_recon]
					   ,[MT_fecha_ejecutoria]
					   ,[MT_fec_exi_liq]
					   ,[MT_proc_cobro_ini_prev]
					   ,[MT_procedencia]
					   ,[capitalmulta]
					   ,[omisossalud]
					   ,[morasalud]
					   ,[inexactossalud]
					   ,[omisospensiones]
					   ,[morapensiones]
					   ,[inexactospensiones]
					   ,[omisosfondosolpen]
					   ,[morafondosolpen]
					   ,[inexactosfondosolpen]
					   ,[omisosarl]
					   ,[moraarl]
					   ,[inexactosarl]
					   ,[omisosicbf]
					   ,[moraicbf]
					   ,[inexactosicbf]
					   ,[omisossena]
					   ,[morasena]
					   ,[inexactossena]
					   ,[omisossubfamiliar]
					   ,[morasubfamiliar]
					   ,[inexactossubfamiliar]
					   ,[sentenciasjudiciales]
					   ,[cuotaspartesacum]
					   ,[totalmultas]
					   ,[totalomisos]
					   ,[totalmora]
					   ,[totalinexactos]
					   ,[totalsentencias]
					   ,[totaldeuda]
					   ,[NumMemoDev]
					   ,[FecMemoDev]
					   ,[CausalDevol]
					   ,[ObsDevol]
					   ,[totalrepartidor]
					   ,[estado]
					   ,[MT_totalSancion]
					   ,[MT_tiposentencia]
					   ,[revocatoria]
					   ,[nroResolRevoca]
					   ,[fechaRevoca]
					   ,[valorRevoca]
					   ,[MT_valor_obligacion]
					   ,[MT_partida_global]
					   ,[MT_sancion_omision]
					   ,[MT_sancion_mora]
					   ,[MT_sancion_inexactitud]
					   ,[MT_total_obligacion]
					   ,[MT_total_partida_global]
					   ,[Automatico]
					   ,[NoExpedienteOrigen]
					    ,[MT_fec_cad_presc]
					   )

			VALUES 
					(@MT_nro_titulo, @MT_expediente, @MT_tipo_titulo, '' , @MT_fec_expedicion_titulo,  
					@MT_res_resuelve_reposicion, @MT_fec_expe_resolucion_reposicion, @MT_reso_resu_apela_recon, @MT_fec_exp_reso_apela_recon, @MT_fecha_ejecutoria, 
					@MT_fec_exi_liq,NULL,@MT_procedencia, @capitalmulta, 
					@omisossalud, @morasalud, @inexactossalud, @omisospensiones,@morapensiones, 
					@inexactospensiones, @omisosfondosolpen, @morafondosolpen, @inexactosfondosolpen,@omisosarl, 
					@moraarl, @inexactosarl, @omisosicbf, @moraicbf, @inexactosicbf, 
					@omisossena, @morasena,@inexactossena, @omisossubfamiliar, @morasubfamiliar, 
					@inexactossubfamiliar, @sentenciasjudiciales, @cuotaspartesacum, @totalmultas, @totalomisos, 
					@totalmora, @totalinexactos, @totalsentencias, @totaldeuda, NULL, 
					NULL,NULL ,NULL, @TotalRepartidor, 1, 
					@TotalSancion, @CodTipSentencia,NULL, NULL , NULL , 
					NULL , @MT_valor_obligacion, @MT_partida_global, @MT_sancion_omision,@MT_sancion_mora, 
					@MT_sancion_inexactitud,  @MT_total_obligacion, @MT_total_partida_global ,  @Automatico,@NoExpedienteOrigen,DATEADD(year, ISNULL((SELECT TOP(1) T.ANOS_FECHA_PRESCRIPCION FROM  [dbo].[TIPOS_TITULO] T WHERE CODIGO=@MT_tipo_titulo),5), CAST(@MT_fecha_ejecutoria AS SMALLDATETIME)) )

			SET @IdunicoTituloR = SCOPE_IDENTITY()

			IF @Automatico = 1
			BEGIN 
				INSERT INTO [dbo].[TAREA_ASIGNADA]
					   ([VAL_USUARIO_NOMBRE]
					   ,[COD_TIPO_OBJ]
					   ,[ID_UNICO_TITULO]
					   ,[EFINROEXP_EXPEDIENTE]
					   ,[FEC_ACTUALIZACION]
					   ,[FEC_ENTREGA_GESTOR]
					   ,[VAL_PRIORIDAD]
					   ,[COD_ESTADO_OPERATIVO])
				 VALUES
				 ('',4,@IdunicoTituloR,NULL,GETDATE(),NULL,0,2)
			END

	END

  SELECT @IdunicoTituloR AS IdunicoTitulo

 END




GO



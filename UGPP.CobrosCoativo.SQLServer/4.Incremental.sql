IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'NoExpedienteOrigen'
          AND Object_ID = Object_ID(N'dbo.[MAESTRO_TITULOS]'))
BEGIN
	ALTER TABLE [dbo].[MAESTRO_TITULOS]
	DROP COLUMN [NoExpedienteOrigen];

	ALTER TABLE [dbo].[MAESTRO_TITULOS]
	ADD [NoExpedienteOrigen] VARCHAR(30);
END


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


INSERT [dbo].[TIPO_OBLIGACION_VALORES] ([ID_TIPO_OBLIGACION_VALORES], [VALOR_OBLIGACION], [PARTIDA_GLOBAL], [SANCION_OMISION], [SANCION_INEXACTITUD], [SANCION_MORA])
 VALUES (N'01', 1, 0, 0, 1, 0),
		(N'02', 1, 0, 0, 0, 0),
		(N'03', 0, 1, 1, 1, 0),
		(N'04', 0, 0, 1, 0, 0),
		(N'05', 1, 1, 1, 1, 0),
		(N'06', 1, 0, 1, 1, 0),
		(N'07', 1, 1, 1, 1, 0),
		(N'08', 1, 0, 1, 0, 0),
		(N'09', 1, 0, 0, 0, 0),
		(N'10', 0, 1, 0, 1, 0),
		(N'11', 1, 0, 0, 0, 0),
		(N'12', 1, 0, 0, 0, 0),
		(N'13', 1, 0, 0, 0, 0),
		(N'14', 1, 0, 0, 0, 0),
		(N'15', 1, 0, 1, 1, 0),
		(N'16', 1, 0, 0, 0, 0),
		(N'17', 1, 1, 0, 0, 0),
		(N'18', 0, 1, 1, 1, 0)

GO
UPDATE [dbo].[MAESTRO_TITULOS] 
SET [MT_fec_cad_presc] = DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=MT_tipo_titulo),CAST(MT_fecha_ejecutoria AS SMALLDATETIME))   	
WHERE idunico>18750

GO

INSERT INTO [dbo].[TIPO_SENTENCIA] (CODIGO,NOMBRE) 
VALUES 
(CONVERT(char(1),(SELECT MAX(codigo)+1 from [dbo].[TIPO_SENTENCIA])),'DTF')
GO
INSERT INTO [dbo].[TIPO_SENTENCIA] (CODIGO,NOMBRE) 
VALUES 
(CONVERT(char(1),(SELECT MAX(codigo)+1 from [dbo].[TIPO_SENTENCIA])),'TASA DE USURA')
GO
UPDATE [dbo].[TIPO_SENTENCIA]
set NOMBRE='%6'
where nombre='CALCULO INTERÉS MORA 6%'
GO

UPDATE [dbo].[TIPO_SENTENCIA]
set NOMBRE='IPC'
where nombre='ACTUALIZACIÓN OBLIGACIÓN IPC'
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[REPARTO_AUTOMATICO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[REPARTO_AUTOMATICO]
END
GO
-- =============================================
-- Author:		<Luis Mario Lenis Ojeda>
-- Create date: <15-Nov-2018>
-- Description:	<ASIGNACIÓN AUTOMATICA DEL REPARTO>
-- =============================================
CREATE PROCEDURE [dbo].[REPARTO_AUTOMATICO] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
--EXEC [REPARTO_AUTOMATICO]
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @TABLA TABLE(
Numero_Registro int,
Id bigint,
Codigo_Titulo varchar(MAX),
Codigo_Expediente varchar(MAX),
Tipo_Titulo varchar(2),
Fecha_Prescripcion DateTime,
Fecha_Recepcion DateTime
);

DECLARE @USERS TABLE(
NUMERO_FILA INT,
CONUSER INT,
USUARIO VARCHAR(MAX));

DECLARE @USERSDESC TABLE(
NUMERO_FILA INT,
CONUSER INT,
USUARIO VARCHAR(MAX));

DECLARE @TABLA2 TABLE(
Numero_Registro int,
Id bigint,
Codigo_Titulo varchar(MAX),
Codigo_Expediente varchar(MAX),
Tipo_Titulo varchar(2),
Fecha_Prescripcion DateTime,
Fecha_Recepcion DateTime
);

DECLARE @ASIGNACION TABLE(
USUARIO VARCHAR(MAX),
TITULO VARCHAR(MAX)
);
declare @IdUnico bigint;
declare @Cod_Tipo varchar(2);
declare @Cod_Titulo varchar(max);
declare @Cod_Exp varchar(max);
declare @Fecha DateTime;
declare @Fecha_Recepcion DateTime;
declare @Fecha1 DateTime;
declare @Number_Register int;
declare @i int=1;
declare @r int=1;
declare @bandera int=0;
declare @bande int=0;
declare @Tota_Titulos int;
declare @Total_User int;
declare @login varchar(max);
declare @titulo varchar(max);


DECLARE fecha_prescripcion CURSOR FOR 
  SELECT numero_registro, 
         id, 
         codigo_titulo, 
         codigo_expediente, 
         tipo_titulo, 
         fecha_prescripcion, 
         fecha_recepcion 
  FROM   @TABLA 

-- Insert statements for procedure here 
INSERT INTO @TABLA 
SELECT Row_number() 
         OVER ( 
           ORDER BY M.idunico), 
       M.idunico, 
       M.mt_nro_titulo, 
       mt_expediente, 
       mt_tipo_titulo, 
       mt_fec_cad_presc, 
       mt_fec_expedicion_titulo 
FROM   maestro_titulos M 
JOIN TAREA_ASIGNADA T
ON T.ID_UNICO_TITULO=M.idunico
WHERE T.COD_ESTADO_OPERATIVO=2 OR T.COD_ESTADO_OPERATIVO=4; 

OPEN fecha_prescripcion 

FETCH next FROM fecha_prescripcion INTO @Number_Register, @IdUnico, @Cod_Titulo, 
@Cod_Exp, @Cod_Tipo, @Fecha, @Fecha_Recepcion 

WHILE @@fetch_status = 0 
  BEGIN 
      IF @Fecha IS NULL 
        BEGIN 
            SET @Fecha1=NULL; 
            SET @Fecha1=(SELECT TOP 1 Max(fec_notificacion) 
                         FROM   maestro_titulos_for_notificacion 
                         WHERE  id_unico_maestro_titulos = @IdUnico 
                                AND cod_tipo_notificacion = 1 
                                AND fec_notificacion IS NOT NULL); 
            SET @Fecha=Dateadd(year, 1, @Fecha1); 
        END 

      INSERT INTO @TABLA2 
      VALUES      (@Number_Register, 
                   @IdUnico, 
                   @Cod_Titulo, 
                   @Cod_Exp, 
                   @Cod_Tipo, 
                   @Fecha, 
                   @Fecha_Recepcion); 

      FETCH next FROM fecha_prescripcion INTO @Number_Register, @IdUnico, 
      @Cod_Titulo, @Cod_Exp, @Cod_Tipo, @Fecha, @Fecha_Recepcion 
  END 

CLOSE fecha_prescripcion 

DEALLOCATE fecha_prescripcion 

SET @Tota_Titulos=(SELECT Count(*) 
                   FROM   @TABLA2); 

INSERT INTO @USERS 
SELECT Row_number() 
         OVER ( 
           ORDER BY U.conuser), 
       U.conuser, 
       U.login 
FROM   usuarios U 
WHERE  U.useractivo = 1 
       AND U.ind_gestor_estudios = 1 
ORDER  BY U.conuser; 

INSERT INTO @USERSDESC 
SELECT Row_number() 
         OVER ( 
           ORDER BY U.conuser DESC), 
       U.conuser, 
       U.login 
FROM   usuarios U 
WHERE  U.useractivo = 1 
       AND U.ind_gestor_estudios = 1 
ORDER  BY U.conuser DESC; 

SET @Total_User=(SELECT Count(*) 
                 FROM   @USERS); 

WHILE ( @r <= @Tota_Titulos ) 
  BEGIN 
      SET @titulo=(SELECT id 
                   FROM   @TABLA2 
                   WHERE  numero_registro = @r); 

      IF @bandera = 0 
        BEGIN 
            SET @login=(SELECT usuario 
                        FROM   @USERS 
                        WHERE  numero_fila = @i); 
						print(@login)
        END 
      ELSE 
        BEGIN 
            SET @login=(SELECT usuario 
                        FROM   @USERSDESC 
                        WHERE  numero_fila = @i); 
						
        END 

      IF( @i = @Total_User ) 
        --Validacion de Usuarios vs Total para cambio de orden de los usuarios 
        BEGIN 
            SET @bandera=1; 
            SET @i=1; 
            SET @bande=@bande + 1 
        END 
      ELSE
	BEGIN
	SET @i=@i + 1; --incremento para asignacion de usuarios 
	END


      IF @bande = 2 --Validacion Seteo de variables 
        BEGIN 
            SET @bandera=0; 
            SET @bande=0; 
        END 
 
      SET @r=@r + 1; --incremento while 
     
      --Asignacion Titulo a Gestor 
	 
	  
      UPDATE tarea_asignada 
      SET    val_usuario_nombre = @LOGIN, 
             fec_entrega_gestor = Getdate(), 
			 FEC_ACTUALIZACION=Getdate(),
             cod_estado_operativo = 4 
      WHERE  id_unico_titulo = @titulo; 
	  print(@titulo)
      ---Cambio Estado Operativo Titulo 
      --UPDATE maestro_titulos 
      --SET    estado = 4 
      --WHERE  idunico = @titulo; 
  END 
 
END
GO
---------27/12/2018
UPDATE PROCEDENCIA_TITULOS SET NOMBRE='Subdirección Juridica Pensional' WHERE CODIGO = 4

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ACTUALIZAR_PRIORIDAD_TITULOS]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_ACTUALIZAR_PRIORIDAD_TITULOS]  
END
GO

-- =============================================
-- Author:		Eduar Fabián Hernández Nieves
-- Create date: 2018-11-16
-- Description:	SP para actualizar el orden de priorización de los títulos asignados a un gestor de estudio de títulos
-- EXEC SP_ACTUALIZAR_PRIORIDAD_TITULOS 'MFONG'
-- =============================================
CREATE PROCEDURE [dbo].[SP_ACTUALIZAR_PRIORIDAD_TITULOS]
	@USULOG AS VARCHAR(20) -- Usuario de quien se lee la priorización
AS
BEGIN
	SET NOCOUNT ON;

	--Asigana todas las tareas con baja prioridad
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 0 WHERE VAL_USUARIO_NOMBRE = @USULOG AND COD_TIPO_OBJ = 4
	-- Actualiza la prioridad de las tareas que tienen prioridad
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE VAL_USUARIO_NOMBRE  = @USULOG  AND COD_TIPO_OBJ = 4 AND (COD_ESTADO_OPERATIVO IN(5, 7, 9) OR IND_TITULO_PRIORIZADO = 1)

	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE ID_TAREA_ASIGNADA IN(
		SELECT TOP 1 TA.ID_TAREA_ASIGNADA 
		FROM TAREA_ASIGNADA TA
		JOIN MAESTRO_TITULOS MT ON TA.ID_UNICO_TITULO = MT.idunico
		WHERE 
			(TA.IND_TITULO_PRIORIZADO IS NULL OR TA.IND_TITULO_PRIORIZADO = 0)
			AND TA.VAL_USUARIO_NOMBRE = @USULOG
			AND TA.COD_ESTADO_OPERATIVO IN (4, 9, 7)
			AND COD_TIPO_OBJ = 4
		ORDER BY
			MT.MT_fec_cad_presc DESC,
			TA.FEC_ENTREGA_GESTOR ASC
	)
END

GO


IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'ESTADOS_PROCESO'))
BEGIN
	ALTER TABLE dbo.ESTADOS_PROCESO ADD
		max_dias_gestion int NULL
END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ACTUALIZAR_PRIORIDAD_EXPEDIENTES]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_ACTUALIZAR_PRIORIDAD_EXPEDIENTES]  
END
GO

-- =============================================
-- Author:		Eduar Fabián Hernández Nieves
-- Create date: 2018-12-27
-- Description:	SP para actualizar el orden de priorización de los expedientes asignados a un gestor
-- EXEC SP_ACTUALIZAR_PRIORIDAD_EXPEDIENTES
-- =============================================
CREATE PROCEDURE [dbo].[SP_ACTUALIZAR_PRIORIDAD_EXPEDIENTES]
AS
BEGIN
	SET NOCOUNT ON;
	
	--Asigana todas los expedientes asignados con baja prioridad
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 0 WHERE COD_TIPO_OBJ = 5 AND COD_ESTADO_OPERATIVO = 11
	-- Actualiza la prioridad de las tareas que tienen prioridad
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE COD_TIPO_OBJ = 5 AND (COD_ESTADO_OPERATIVO IN(12, 14, 19) OR IND_TITULO_PRIORIZADO = 1)

	DECLARE @Usuario AS nvarchar(40)

	DECLARE UsuariosConExpedientesParaGestionar CURSOR READ_ONLY FOR
		SELECT DISTINCT VAL_USUARIO_NOMBRE
		FROM TAREA_ASIGNADA
		WHERE 
			COD_TIPO_OBJ = 5
			AND COD_ESTADO_OPERATIVO IN(11, 15)
			AND VAL_USUARIO_NOMBRE IS NOT NULL
			AND VAL_USUARIO_NOMBRE <> ''
	
	OPEN UsuariosConExpedientesParaGestionar

	FETCH NEXT FROM UsuariosConExpedientesParaGestionar INTO @Usuario
	WHILE @@fetch_status = 0
	BEGIN

		UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 
		WHERE ID_TAREA_ASIGNADA = (
			SELECT TOP 1 TA.ID_TAREA_ASIGNADA
			FROM TAREA_ASIGNADA AS TA
			JOIN EJEFISGLOBAL AS E ON TA.EFINROEXP_EXPEDIENTE = E.EFINROEXP
			JOIN MAESTRO_TITULOS AS MT ON E.EFINROEXP = MT.MT_expediente
			WHERE 
				TA.VAL_USUARIO_NOMBRE = @Usuario
				AND TA.COD_ESTADO_OPERATIVO  IN(11, 15)
				AND TA.COD_TIPO_OBJ = 5
				AND TA.EFINROEXP_EXPEDIENTE IS NOT NULL
				AND MT.MT_fec_cad_presc IS NOT NULL
			ORDER BY MT.MT_fec_cad_presc ASC, E.EFISALDOCAP DESC, MT.MT_fec_exi_liq ASC
		)

		FETCH NEXT FROM UsuariosConExpedientesParaGestionar INTO @Usuario
	END
	CLOSE UsuariosConExpedientesParaGestionar
	DEALLOCATE UsuariosConExpedientesParaGestionar

END
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_OBTENER_EXPEDIENTES_ASIGNADOS]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_OBTENER_EXPEDIENTES_ASIGNADOS]  
END
GO

-- =============================================
-- Author:		Eduar Fabian Hernandez Nieves - Stefanini
-- Create date: 2018-12-21
-- Description:	Procedimiento almacenado para obtener los expedientes asignados a un gestor
-- EXEC SP_OBTENER_EXPEDIENTES_ASIGNADOS 1, 100, '', '', 'LTORRES', '', '', '', '', '', '', '', 15
-- =============================================
CREATE PROCEDURE [dbo].[SP_OBTENER_EXPEDIENTES_ASIGNADOS]
	-- Variables de páginación, oblitarias para poblar en GridView en la aplicación
	@StartRecord INT,
	@StopRecord INT,
	--Orden que se le debe dar a la consulta
	@SortExpression VARCHAR(30) = '',
	@SortDirection VARCHAR(10) = '',
	-- Filtros
	@USULOG AS VARCHAR(20) = '', -- Usuario al cual se le asigno el expediente, login del LDAP
	@EFINROEXP VARCHAR(10) = '', -- Número del expediente en cobros y coactivos
	@ED_NOMBRE VARCHAR(120) = '', -- Nombre del deudor
	@EFINIT VARCHAR(13) = '', -- Número de identificación del deudor
	@ESTADOPROC CHAR(2) = '', -- Estado procesal del deudor
	@MT_TIPO_TITULO CHAR(2) = '', -- Tipo de título
	@FECTITULO VARCHAR(10) = '', -- Fecha de entrega del título por parte del área origen
	@FECENTGES VARCHAR(10) = '', -- Fecha de entrega al gestor del título
	@ESTADO_OPERATIVO INT = 0 --Estado operativo del expediente
AS
BEGIN
	SET NOCOUNT ON;

	--Se realiza la priorización de títulos para los gestores
	EXEC SP_ACTUALIZAR_PRIORIDAD_EXPEDIENTES

	IF @SortExpression = '' BEGIN
		SET @SortExpression = 'EFINROEXP'
	END

	IF @SortDirection = '' BEGIN
		SET @SortDirection = 'ASC'
	END

	-- Se realizan los filtros dependiendo de los parámetros del SP y se guardan en una tabla temporal
	SELECT
		DISTINCT TAREA_ASIGNADA.ID_TAREA_ASIGNADA AS ID_TAREA_ASIGNADA,
		CONVERT (VARCHAR, TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE) EFINROEXP,
		EJEFISGLOBAL.EFIFECHAEXP,
		EJEFISGLOBAL.EFINIT,
		EJEFISGLOBAL.EFIFECENTGES,
		EJEFISGLOBAL.EFIFECCAD,
		EJEFISGLOBAL.EFIVALDEU,
		EJEFISGLOBAL.EFIPAGOSCAP,
		EJEFISGLOBAL.EFISALDOCAP,
		ESTADOS_PROCESO.CODIGO AS EFIESTADOCODIGO,
		ESTADOS_PROCESO.nombre AS EFIESTADO,
		ESTADOS_PAGO.nombre AS EFIESTUP,
		PERSUASIVO.FecEstiFin,
		ENTES_DEUDORES.ED_NOMBRE,
		'OK' AS termino,
		'      ' AS explicacion,
		'                    ' AS PictureURL,
		USUARIOS.codigo AS USUARIOSCODIGO, 
		USUARIOS.nombre AS GESTOR, 
		--TAREA_ASIGNADA.VAL_USUARIO_NOMBRE AS GESTOR,
		TITULOSEJECUTIVOS.MT_tipo_titulo AS MT_TIPO_TITULO,
		COALESCE(TITULOSEJECUTIVOS.NomTipoTitulo,'') AS NomTipoTitulo,
		ESTADO_OPERATIVO.ID_ESTADO_OPERATIVOS AS COD_ESTADO_OPERATIVO,
		ESTADO_OPERATIVO.VAL_NOMBRE AS ESTADO_OPERATIVO,
		TAREA_ASIGNADA.VAL_PRIORIDAD AS VAL_PRIORIDAD,
		0.00 AS PagoyAjuste
	INTO #ExpedientesAsignados
	FROM
		TAREA_ASIGNADA
		LEFT JOIN EJEFISGLOBAL ON TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE = EJEFISGLOBAL.EFINROEXP
		LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo
		LEFT JOIN ESTADOS_PAGO ON EJEFISGLOBAL.EFIESTUP = ESTADOS_PAGO.codigo
		LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp
		LEFT JOIN ENTES_DEUDORES ON EJEFISGLOBAL.EFINIT = ENTES_DEUDORES.ED_Codigo_Nit
		LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo
		LEFT JOIN TITULOSEJECUTIVOS ON EJEFISGLOBAL.EFINROEXP = TITULOSEJECUTIVOS.MT_expediente
		LEFT JOIN ESTADO_OPERATIVO ON TAREA_ASIGNADA.COD_ESTADO_OPERATIVO = ESTADO_OPERATIVO.ID_ESTADO_OPERATIVOS
	WHERE
		-- Filtros
		( (@USULOG = '') OR ([TAREA_ASIGNADA].[VAL_USUARIO_NOMBRE] = @USULOG) )
		AND ((@EFINROEXP = '') OR (CONVERT (VARCHAR, [TAREA_ASIGNADA].[EFINROEXP_EXPEDIENTE]) = @EFINROEXP ) )
		AND ((@ED_NOMBRE = '') OR ([ENTES_DEUDORES].[ED_Nombre] LIKE '%' + @ED_NOMBRE + '%'))
		AND ((@EFINIT = '') OR ([EJEFISGLOBAL].[EFINIT] LIKE '%' + @EFINIT + '%'))
		AND ((@ESTADOPROC = '') OR ([EJEFISGLOBAL].[EFIESTADO] = @ESTADOPROC ))
		AND ((@MT_TIPO_TITULO = '') OR ([TITULOSEJECUTIVOS].[MT_tipo_titulo] = @MT_TIPO_TITULO ))
		AND ((@ESTADO_OPERATIVO = 0) OR ([TAREA_ASIGNADA].[COD_ESTADO_OPERATIVO] = @ESTADO_OPERATIVO))
		AND
		(
			(
				((@FECTITULO = '') OR (CONVERT(VARCHAR(10),[TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR],103) = @FECTITULO))
				AND ((@FECENTGES = '') OR (CONVERT(VARCHAR(10),[TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR],103) = @FECENTGES))
			)--Se valida sobre la tabla tarea_asiganada los valores que se consultan
			OR
			(
				((@FECTITULO = '') OR (CONVERT(VARCHAR(10),[EJEFISGLOBAL].[EFIFECHAEXP],103) = @FECTITULO))
				AND ((@FECENTGES = '') OR (CONVERT(VARCHAR(10),[EJEFISGLOBAL].[EFIFECENTGES],103) = @FECENTGES))
			)--Se valida sobre la tabla ya existente
		)
		-- Condicionales obligatorias
		AND TAREA_ASIGNADA.COD_TIPO_OBJ = 5
		AND TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 14, 15, 19) 
		AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE IS NOT NULL
		AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE <> ''

	-- Se cuentan los registros obtenidos
	DECLARE @count VARCHAR(MAX) = (SELECT COUNT(*) FROM #ExpedientesAsignados)
	-- Se arma la consulta para que sea dinámico
	DECLARE @ConsultaExpedientes VARCHAR(MAX) = 'SELECT * FROM(
													SELECT ROW_NUMBER() OVER (ORDER BY VAL_PRIORIDAD DESC, ' + @SortExpression + ' ' + @SortDirection + ') AS RecordSetID, 
													*,
													'+ @count+' AS RecordSetCount
													FROM #ExpedientesAsignados
												) AS t
												WHERE RecordSetID >= ' + CONVERT(VARCHAR(10), @StartRecord) + ' And RecordSetID <= ' + CONVERT(VARCHAR(10), @StopRecord)
	-- Se ejecuta la consulta que se arma de forma dinámica
	EXEC(@ConsultaExpedientes)
	
	--Se elimina la tabla temporal
	DROP TABLE #ExpedientesAsignados
END
GO


-----------Script de migración de datos para expedientes de Cobros y Coactivos---------------

-- Se eliminan los registros en la tabla TAREA_SOLICITUD relacionados con los expedientes
DELETE FROM TAREA_SOLICITUD WHERE ID_TAREA_ASIGNADA IN (SELECT ID_TAREA_ASIGNADA FROM TAREA_ASIGNADA WHERE EFINROEXP_EXPEDIENTE IS NOT NULL)
-- Se eliminan los expedientes que se hayan creado en la tabla TAREA_ASIGNADA
DELETE TAREA_ASIGNADA WHERE EFINROEXP_EXPEDIENTE IS NOT NULL AND EFINROEXP_EXPEDIENTE IN(SELECT DISTINCT EFINROEXP FROM EJEFISGLOBAL) -- Se deben eleiminar estos registros de la tabla

-- Se debe selececionar el estado procesal del expediente y dependiendo de este se asigna un estado operativo

---Se ingresan los expedientes que están en estado procesal "Reparto" sin gestor y se le coloca el estado operativo "Por Repartir" sin gestor
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	'' AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	10 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL
WHERE EFIESTADO = '09' AND EFIUSUASIG IS NULL

---Se ingresan los expedientes que están en estado procesal "Reparto" con gestor y se le coloca el estado operativo "Por Repartir" con el gestor asignado
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	u.login AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	E.EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	E.EFIFECENTGES AS FEC_ENTREGA_GESTOR,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	11 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL AS E
LEFT JOIN USUARIOS AS U ON E.EFIUSUASIG = U.codigo
WHERE E.EFIESTADO = '09' AND E.EFIUSUASIG IS NOT NULL

---Todos los expedientes que se encuentren con el estado procesal "Terminado" se deja con el estado operativo "Terminado"
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	u.login AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	E.EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	E.EFIFECENTGES AS FEC_ENTREGA_GESTOR,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	16 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL AS E
LEFT JOIN USUARIOS AS U ON E.EFIUSUASIG = U.codigo
WHERE E.EFIESTADO = '07' 

---Todos los expedientes que se encuentren con el estado procesal "Devuelto" se deja con el estado operativo "Subsanar"
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	u.login AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	E.EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	E.EFIFECENTGES AS FEC_ENTREGA_GESTOR,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	19 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL AS E
LEFT JOIN USUARIOS AS U ON E.EFIUSUASIG = U.codigo
WHERE E.EFIESTADO = '04'

--Los expedientes que estén con una solicitud de cambio de estado sin aprobar se colocan en estado operativo en solicitud, se inicia la validación de no creación del ID del expediente en la tabla tarea asiganda
---Se entiende que en la aplicación actual no se ha autorizado el cambio de estado procesal si el último registro de solicitud de cambio en su columna estadosol se encuentra con valor 1
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	u.login AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	E.EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	E.EFIFECENTGES AS FEC_ENTREGA_GESTOR,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	13 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL AS E
LEFT JOIN USUARIOS AS U ON E.EFIUSUASIG = U.codigo
WHERE E.EFIESTADO NOT IN ('07', '09', '04') AND EFINROEXP IN(
	SELECT DISTINCT NroExp FROM(
		SELECT NroExp, MAX(fecha) AS fecha, estadosol 
		FROM solicitudes_cambioestado 
		GROUP BY NroExp, fecha, estadosol 
		HAVING estadosol = 1
	) AS t2
) --Solicitud de cambio de estado sin aprobar 

-- Los expedientes que no tienen una solicitud de cambio de estado pendiente y que se encuentran en los estados procesales:
	---Cartera Incobrable
	---Coactivo
	---Concursal
	---Facilidad de Pago
	---Persuasivo
	---Suspendido
	---Para devolver
	---Verificación de Pago
	---Difícil cobro
--se les asigna el estado operativo "En Gestión"
INSERT INTO 
	TAREA_ASIGNADA(VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, IND_TITULO_PRIORIZADO, COD_ESTADO_OPERATIVO)
SELECT 
	CASE
		WHEN E.EFIUSUASIG IS NULL 
			THEN '' 
			ELSE u.login 
		END 
	AS VAL_USUARIO_NOMBRE,
	5 AS COD_TIPO_OBJ,
	E.EFINROEXP AS EFINROEXP_EXPEDIENTE,
	GETDATE() AS FEC_ACTUALIZACION,
	E.EFIFECENTGES AS FEC_ENTREGA_GESTOR,
	0 AS VAL_PRIORIDAD,
	0 AS IND_TITULO_PRIORIZADO,
	15 AS COD_ESTADO_OPERATIVO
FROM EJEFISGLOBAL AS E
LEFT JOIN USUARIOS AS U ON E.EFIUSUASIG = U.codigo
WHERE E.EFIESTADO IN ('01', '02', '03', '05', '06', '08', '10', '11', '12') AND E.EFINROEXP NOT IN(
	SELECT DISTINCT EFINROEXP_EXPEDIENTE FROM TAREA_ASIGNADA WHERE EFINROEXP_EXPEDIENTE IS NOT NULL
)
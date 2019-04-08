-- =============================================
-- Author:		Sergio J. Caceres C.
-- Create date: 2019-02-11
-- Se agregan los valores IPC de los años 2013 al 2018, verificando antes que estos no existan en BD. 
-- =============================================
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2013'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2013',0.0194)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2013'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2013'
GO
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2014'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2014',0.0366)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2014'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2014'
GO
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2015'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2015',0.0677)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2015'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2015'
GO
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2016'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2016',0.0575)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2016'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2017'
GO
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2017'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2017',0.0409)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2017'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2017'
GO
IF (NOT EXISTS (SELECT * FROM [dbo].[IPC] WHERE ANIO='2018'))
	BEGIN
		INSERT INTO [dbo].[IPC]
           ([ANIO]
           ,[TASA])
		VALUES
           ('2018',0.0318)
		PRINT 'SE ADICIONO EL IPC DEL AÑO: 2018'
	END
ELSE 
	PRINT 'YA EXISTIA EL IPC DEL AÑO: 2018'
GO

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'LOG_IPC_INTERESES')
)
BEGIN
SET ANSI_NULLS ON
	ALTER TABLE dbo.LOG_IPC_INTERESES ADD NROEXP varchar(12) NULL
END
GO
-- =============================================
-- Author:		Sergio J. Caceres C.
-- Create date: 2019-02-11
-- Se adiciona a la tabla: LOG_IPC_INTERESES el campo: NROEXP para poder relacionar el número de expediente.
-- =============================================
IF EXISTS (SELECT *
	FROM SYS.OBJECTS
	WHERE object_id = OBJECT_ID(N'[dbo].[SP_LOG_IPC_INTERESES]')
	AND type IN (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[SP_LOG_IPC_INTERESES]
END
GO

CREATE PROCEDURE [dbo].[SP_LOG_IPC_INTERESES]
    (@TIPO  char(2)
    ,@VALOR_CAPITAL  decimal(18,2)
    ,@FECHA_EXIG  date
    ,@FECHA_PAGO date
    ,@DIAS_MORA int
    ,@INTERESES decimal(18,2)
    ,@TOTAL_DEUDA decimal(18,2)
    ,@IPC decimal(18,2)
    ,@VALOR_IPC decimal(18,2)
    ,@ANIO_IPC int
    ,@TASA decimal(18,4)
    ,@USUARIO VARCHAR(50)
	,@NROEXP VARCHAR(12))
AS 
INSERT INTO [dbo].[LOG_IPC_INTERESES]
    ([TIPO]
    ,[VALOR_CAPITAL]
    ,[FECHA_EXIG]
    ,[FECHA_PAGO]
    ,[DIAS_MORA]
    ,[INTERESES]
    ,[TOTAL_DEUDA]
    ,[IPC]
    ,[VALOR_IPC]
    ,[ANIO_IPC]
    ,[TASA]
    ,[FECHA_SYS]
    ,[USUARIO]
    ,[NROEXP])
VALUES
    (@TIPO,@VALOR_CAPITAL,@FECHA_EXIG,@FECHA_PAGO,@DIAS_MORA,@INTERESES,@TOTAL_DEUDA,@IPC,@VALOR_IPC,@ANIO_IPC,@TASA,GETDATE(),@USUARIO,@NROEXP)
GO

IF EXISTS (SELECT *
	FROM SYS.OBJECTS
	WHERE object_id = OBJECT_ID(N'[dbo].[SP_ACTUALIZAR_IPC_TITULOS_NEW]')
	AND type IN (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[SP_ACTUALIZAR_IPC_TITULOS_NEW]
END
GO

-- =============================================
-- Author:		Sergio J. Caceres C.
-- Create date: 2019-02-11
-- Description:	Procedimiento que permite actualizar el valor con el IPC de los títulos tipo "SANCIÓN L1607/12" o "LIQUIDACIÓN OFICIAL / SANCIÓN"
-- Se incluyen impresiones con las cuales se facilita ejecutar el seguimiento de los valores de cada Titulo
--EXEC SP_ACTUALIZAR_IPC_TITULOS_NEW 'AUTOMÁTICO', '82523' /*8 - LIQUIDACIÓN OFICIAL / SANCIÓN [SIN PAGOS]*/
--EXEC SP_ACTUALIZAR_IPC_TITULOS_NEW 'AUTOMÁTICO', '81901' /*7 - SANCIÓN L1607/12*/  
--EXEC SP_ACTUALIZAR_IPC_TITULOS_NEW 'AUTOMÁTICO', '82446' /*8 - LIQUIDACIÓN OFICIAL / SANCIÓN [CON PAGOS]*/  
--EXEC SP_ACTUALIZAR_IPC_TITULOS_NEW 'AUTOMÁTICO'	/*SELECCIONAR TODOS LOS EXPEDIENTES*/
CREATE PROCEDURE [dbo].[SP_ACTUALIZAR_IPC_TITULOS_NEW]
	@USUARIO NVARCHAR(12),
	@NroExpediente NVARCHAR(12)
AS
BEGIN
	--SET NOCOUNT ON;	
	--PRINT '[Prueba basada en el Nuevo Liquidador por Sancion Individial]'
	-- Variables necesarias para la actualización de datos
	DECLARE @Expediente AS NVARCHAR(40)
	DECLARE @ValorBase AS FLOAT
	DECLARE @FechaExigibilidad AS DATE
	DECLARE @AnioExigibilidad AS NVARCHAR(4)
	DECLARE @TipoTitulo AS NVARCHAR(4)
	-- Variables necesarias para el funcionamiento del segundo cursor
	DECLARE @AnioASeleccionar AS NVARCHAR(12)

	--Se seleccionan los expedientes a los que se les aplica IPC ("SANCIÓN L1607/12" o "LIQUIDACIÓN OFICIAL / SANCIÓN") y cuya deuda inicial es mayor a cero
	SELECT  
		@Expediente = MT.MT_expediente --Número de expediente
		,@ValorBase = CASE 
			WHEN MT.MT_tipo_titulo = '07' THEN E.EFIVALDEU
			ELSE 
				(SELECT SUM(MT_totalSancion) FROM MAESTRO_TITULOS WITH(NOLOCK) WHERE MT_Expediente = MT.MT_expediente)
		END
		,@FechaExigibilidad = (SELECT TOP 1 CAST(MT_fec_exi_liq AS DATE) FROM MAESTRO_TITULOS WITH(NOLOCK) WHERE MT_expediente = MT.MT_expediente ORDER BY MT_fec_exi_liq DESC)
		,@TipoTitulo = MT.MT_tipo_titulo
	FROM MAESTRO_TITULOS AS MT WITH(NOLOCK)
	LEFT JOIN EJEFISGLOBAL AS E ON MT.MT_expediente = E.EFINROEXP
	WHERE
		MT.MT_expediente IS NOT NULL
		AND MT.MT_tipo_titulo IN ('07','08') -- {07: SANCIÓN L1607/12; 08: LIQUIDACIÓN OFICIAL / SANCIÓN}
		AND (MT.MT_expediente = @NroExpediente)
		AND MT.MT_fecha_ejecutoria IS NOT NULL
	GROUP BY MT.MT_expediente, E.EFIVALDEU, MT.MT_tipo_titulo
	
	--SELECT @TipoTitulo
	
	-- Se obtiene la fecha de exibilidad del titulo
	IF (@FechaExigibilidad IS NULL)
	BEGIN
		DECLARE @FechaEjecutoria DATE = (SELECT TOP 1 MT_fecha_ejecutoria FROM MAESTRO_TITULOS WITH(NOLOCK) WHERE MT_expediente = @Expediente ORDER BY MT_fecha_ejecutoria DESC )
		IF @FechaEjecutoria IS NOT NULL
		BEGIN
			SET @FechaExigibilidad = DATEADD(DAY, 1, @FechaEjecutoria)
			UPDATE MAESTRO_TITULOS SET MT_fec_exi_liq = @FechaExigibilidad WHERE idunico = (SELECT TOP 1 idunico FROM MAESTRO_TITULOS WITH(NOLOCK) WHERE MT_expediente = @Expediente ORDER BY MT_fecha_ejecutoria DESC )
		END
	END
	--SELECT @FechaExigibilidad
	
	IF (@FechaExigibilidad IS NOT NULL)
	BEGIN
		SET @AnioExigibilidad = YEAR(@FechaExigibilidad)
	END
	Print 'Fecha Exigibilidad: ' + CAST (@FechaExigibilidad AS NVARCHAR(MAX))
	IF @AnioExigibilidad IS NULL
	BEGIN
		RETURN
	END

	-- Variable para capturar el saldo final del expediente
	DECLARE @ValorSaldo AS FLOAT = @ValorBase			
	-- Variable para capturar todo eL IPC total del expediente
	DECLARE @IPCTotal AS FLOAT = 0

	--Se verifica que el expediente haya tenido pagos antes de la fecha de exigibilidad AFE:AntesFechaExigibilidad
	DECLARE @pagosAFE AS FLOAT
	IF @TipoTitulo = '07'
	BEGIN 		
		SET @pagosAFE = (SELECT SUM(pagCapital) 
						FROM PAGOS WITH(NOLOCK)
						WHERE NroExp = @Expediente AND pagFecha <= @FechaExigibilidad
						GROUP BY NroExp)
	END
	ELSE
	BEGIN
		SET @pagosAFE = (SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0))
						FROM PAGOS WITH(NOLOCK)
						WHERE NroExp = @Expediente AND pagFecha <= @FechaExigibilidad AND pagLiqSan = 1 
						GROUP BY NroExp)
	END

	--SELECT @pagosAFE
	IF (@pagosAFE IS NULL)
		SET @pagosAFE = 0

	SET @ValorSaldo = @ValorSaldo - @pagosAFE
	--SELECT @ValorSaldo
	PRINT 'Saldo con Pagos anteriores a la fecha de exigibilidad: $' + CONVERT (VARCHAR, CAST(ROUND(@ValorSaldo,0) AS MONEY), 1)
	--Se crea cursor con los años para los cuales se les va a aplicar el IPC
	DECLARE AniosExpedienteQueAplicanIPC CURSOR LOCAL READ_ONLY FOR
		SELECT ANIO FROM IPC WITH(NOLOCK) WHERE ANIO >= @AnioExigibilidad
	OPEN AniosExpedienteQueAplicanIPC
	FETCH NEXT FROM AniosExpedienteQueAplicanIPC INTO @AnioASeleccionar
		WHILE @@fetch_status = 0
			BEGIN
				-- Si no se encuentra definido el IPC de ese año no se calcula y continua con el siguiente
				DECLARE @AnioIPC NVARCHAR(5) = (SELECT TOP 1 ANIO FROM IPC WITH(NOLOCK) WHERE ANIO = @AnioASeleccionar)
				IF (@AnioIPC IS NULL)
				BEGIN
					CONTINUE
				END

				PRINT '__/Expediente #: ' + @Expediente + ' en Año: '+ @AnioASeleccionar + '\__'
				--Variable para captura el IPC del año en curso
				DECLARE @IPC FLOAT = 0
				--Variable para controlar el valor base de la deuda cuando inicia un año
				DECLARE @ValorBaseAnioAnterior AS FLOAT = @ValorSaldo

				-- Se captura la suma total de los pagos realizados en el año
				
				DECLARE @pagos AS FLOAT
				IF @TipoTitulo = '07'
				BEGIN 
					
					SET @pagos = (SELECT SUM(pagCapital) 
									FROM PAGOS WITH(NOLOCK)
									WHERE 
										NroExp = @Expediente 
										AND YEAR(pagFecha) = @AnioASeleccionar
										AND pagFecha > CAST(@FechaExigibilidad AS DATE)
									GROUP BY NroExp)
				END
				ELSE
				BEGIN
					SET @pagos = (SELECT SUM(COALESCE(pagcapital,0) + COALESCE(pagAjusteDec1406,0))
									FROM PAGOS WITH(NOLOCK)
									WHERE
										NroExp = @Expediente 
										AND YEAR(pagFecha) = @AnioASeleccionar
										AND pagFecha > CAST(@FechaExigibilidad AS DATE)
										AND pagLiqSan = 1 
									GROUP BY NroExp)
				END
					
				IF (@pagos IS NULL)
				BEGIN
					SET @pagos = 0
				END
				PRINT 'Pagos: $' + CONVERT(VARCHAR, CAST(ROUND (@pagos,0) AS MONEY), 1)

				--Se restan los pagos al saldo de la deuda
				SET @ValorSaldo = @ValorSaldo - @pagos

				DECLARE @DedudaInicial FLOAT = @ValorSaldo
				PRINT 'Valor Saldo : $' + CONVERT (VARCHAR, CAST(ROUND(@ValorSaldo,0) AS MONEY), 1)
				
				--Si el valor del saldo es menor o igual a cero finaliza el proceso
				IF @ValorSaldo <= 0
				BEGIN
					BREAK
				END

				-- Se captura el porcentaje del IPC del año actual del loop
				DECLARE @valorIPCAnioLoop DECIMAL = (SELECT TOP 1 TASA FROM IPC WHERE ANIO = @AnioASeleccionar)
				PRINT 'IPC: ' + CONVERT(VARCHAR, CAST(ROUND (@IPC,0) AS MONEY), 1)

				SET @IPC = (SELECT @ValorSaldo * (TASA) FROM IPC WITH(NOLOCK) WHERE ANIO = @AnioASeleccionar)						
				--PRINT 'Valor IPC: $' + CONVERT (VARCHAR, CAST(ROUND(@IPC,0) AS MONEY), 1)
				--Se le suma el IPC al saldo de ese expediente
				SET @ValorSaldo = @ValorSaldo + @IPC
				--PRINT 'Valor actualizado (ValorSaldo + IPC): $' + CONVERT (VARCHAR, CAST(ROUND (@valorSaldo, 0) AS MONEY),1)

				PRINT 'Saldo a Pagar: $' + CONVERT (VARCHAR, CAST(ROUND(@ValorSaldo,0) AS MONEY), 1)

				--Se aumenta el IPC Total del expediente
				SET @IPCTotal = @IPCTotal + @IPC		

				IF (@ValorSaldo <> @valorBaseAnioAnterior)
				BEGIN						
				-- Actualización del Log de actualización de valores del Expediente
				EXEC SP_LOG_IPC_INTERESES 'IP', @DedudaInicial, @FechaExigibilidad, NULL, 0, 0, @ValorSaldo, @valorIPCAnioLoop, @IPC, @AnioASeleccionar, 0, @USUARIO, @Expediente

					IF(@pagos = 0)
					BEGIN 
						-- Se agrega el valor al segundo log [dbo].[ACTUALIZACION_IPC_SIN_PAGO]
						INSERT INTO ACTUALIZACION_IPC_SIN_PAGO(expdediente, valor_viejo, valor_nuevo, valoripc, fecha)
						VALUES(@Expediente, @ValorBaseAnioAnterior, @ValorSaldo, @IPC, GETDATE())
					END
				END

			FETCH NEXT FROM AniosExpedienteQueAplicanIPC INTO @AnioASeleccionar
		END
	CLOSE AniosExpedienteQueAplicanIPC
	DEALLOCATE AniosExpedienteQueAplicanIPC

	PRINT 'Expediente: ' + @Expediente
	PRINT 'Saldo: ' + CONVERT (VARCHAR, CAST(ROUND(@ValorSaldo,0) AS MONEY), 1)
	PRINT 'IPC Total: ' + CONVERT (VARCHAR, CAST(ROUND(@IPCTotal,0) AS MONEY), 1)
	--Fin cursor que recorre los años que se les calcula el IPC
	-- Se actualiza el valor del expediente
	--EXEC SP_ACTUALIZAR_VALORES_IPC_EXPEDIENTE @ValorSaldo, @IPCTotal, @Expediente
	--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	UPDATE EJEFISGLOBAL SET EFISALDOCAP = @ValorSaldo, EFIIPC = @IPCTotal WHERE EFINROEXP = @Expediente

	--SELECT @ValorSaldo AS ValorSaldo, @IPCTotal AS IPCTotal, @Expediente AS Expediente
	--BEGIN TRANSACTION ActualizarIPC
		--UPDATE EJEFISGLOBAL SET EFISALDOCAP = @ValorSaldo, EFIIPC = @IPCTotal WHERE EFINROEXP = @Expediente
	--COMMIT TRANSACTION ActualizarIPC
END
GO

INSERT INTO IPC VaLUES('2019', 0)
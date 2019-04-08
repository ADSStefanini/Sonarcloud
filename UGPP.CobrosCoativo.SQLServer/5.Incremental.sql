IF (NOT EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ESTADOS_PROCESO_GESTOR')
)
BEGIN
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[ESTADOS_PROCESO_GESTOR](
		[VAL_USUARIO] [nvarchar](150) NOT NULL,
		[COD_ID_ESTADOS_PROCESOS] [char](2) NOT NULL,
		[IND_ESTADO] [bit] NOT NULL	
	) 
END
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ESTADOS_PROCESO_GESTOR')
)
BEGIN
ALTER TABLE [dbo].[ESTADOS_PROCESO_GESTOR] WITH CHECK ADD CONSTRAINT [FK_ESTADOS_PROCESO_GESTOR_COD_ESTADOS_PROESO] FOREIGN KEY ([COD_ID_ESTADOS_PROCESOS])
REFERENCES [dbo].[ESTADOS_PROCESO] ([codigo])
END
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ESTADOS_PROCESO_GESTOR')
)
BEGIN
ALTER TABLE [dbo].[ESTADOS_PROCESO_GESTOR] CHECK CONSTRAINT [FK_ESTADOS_PROCESO_GESTOR_COD_ESTADOS_PROESO]
END
GO


-- La siguiente consulta es temporal y se realiza para el funcionamiento del reparto de expedientes por etapa procesal
IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ESTADOS_PROCESO_GESTOR')
)
BEGIN

-- Cantidad de estados procesales existentes
DECLARE @CountEstadosProcesales INT = (SELECT
		COUNT(*)
	FROM ESTADOS_PROCESO)
-- Control de estados procesales que se asignan a los usuarios
DECLARE @ControlEstadosProcesales INT = 1
-- Usuario que se lee de la tabla usuarios
DECLARE @Usuario AS NVARCHAR(40)
--Variable que se utiliza para asiganar estados procesales aleatorios
DECLARE @ramdonAsignado INT = 1
--Tabla temporal para asignaciones ramdon
CREATE TABLE #EstadosProcesalesAsigandos (
	codigo CHAR(2)
)

--Se vacía la tabla donde esta la relación de los usuarios con el estado procesal
DELETE FROM ESTADOS_PROCESO_GESTOR

-- Se crea cursor
DECLARE UsuariosParaAsignarEstadoProcesal CURSOR READ_ONLY FOR
-- Se seleccionan los usuarios que pueden gestionar expedientes
SELECT DISTINCT
	login
FROM USUARIOS
WHERE ind_gestor_expedientes = 1

OPEN UsuariosParaAsignarEstadoProcesal

FETCH NEXT FROM UsuariosParaAsignarEstadoProcesal INTO @Usuario
WHILE @@fetch_status = 0
BEGIN
-- Se asigna el estado procesal en el orden que aparece en la tabla, con esto se asegura que todos los estados procesales quedan con un gestor asignado
DECLARE @codigo CHAR(2) = (SELECT
		RIGHT('00' + CAST(@ControlEstadosProcesales AS VARCHAR(2)), 2))
IF (@ControlEstadosProcesales >= @CountEstadosProcesales)
SET @ControlEstadosProcesales = 1
		ELSE
SET @ControlEstadosProcesales = @ControlEstadosProcesales + 1

--PRINT CONCAT(@Usuario, ' - ', @codigo)
INSERT INTO ESTADOS_PROCESO_GESTOR (VAL_USUARIO, COD_ID_ESTADOS_PROCESOS, IND_ESTADO)
	VALUES (@Usuario, @codigo, 1)
INSERT INTO #EstadosProcesalesAsigandos
	VALUES (@codigo)

--Se asignan dos estados procesales aleatorios
WHILE @ramdonAsignado < 3
BEGIN
SET @codigo = (SELECT TOP 1
		codigo
	FROM ESTADOS_PROCESO
	WHERE codigo NOT IN (SELECT
			codigo COLLATE Modern_Spanish_CI_AS
		FROM #EstadosProcesalesAsigandos)
	ORDER BY NEWID())
INSERT INTO ESTADOS_PROCESO_GESTOR (VAL_USUARIO, COD_ID_ESTADOS_PROCESOS, IND_ESTADO)
	VALUES (@Usuario, @codigo, 1)
INSERT INTO #EstadosProcesalesAsigandos
	VALUES (@codigo)
--PRINT CONCAT(@Usuario, ' - ', @codigo)
SET @ramdonAsignado = @ramdonAsignado + 1
		END
SET @ramdonAsignado = 1

--Se eliminan registros de la tabla temporal
DELETE FROM #EstadosProcesalesAsigandos

-- Siguiente registro
FETCH NEXT FROM UsuariosParaAsignarEstadoProcesal INTO @Usuario
END

CLOSE UsuariosParaAsignarEstadoProcesal
DEALLOCATE UsuariosParaAsignarEstadoProcesal

--Se elimina tabla temporal 
DROP TABLE #EstadosProcesalesAsigandos

--Consulta de control
--SELECT * FROM ESTADOS_PROCESO_GESTOR
END
GO


SET IDENTITY_INSERT [dbo].[MAESTRO_HOMOLOGACION] ON
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1214, 2, N'DIRECCION_PARAFISCALES', N'1', 1)
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1215, 2, N'SUBDIRECCION_PARAFISCALES', N'2', 1)
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1216, 2, N'SUBDIRECCION_NOMINA', N'3', 1)
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1217, 2, N'SUBDIRECCION_JURIDICA_PENSIONAL', N'4', 1)
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1218, 2, N'CONTROL_INTERNO_DISCIPLINARIO', N'5', 1)
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO])
	VALUES (1219, 2, N'ADMINISTRATIVA', N'6', 1)
SET IDENTITY_INSERT [dbo].[MAESTRO_HOMOLOGACION] OFF
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TAREA_SOLICITUD')
)
BEGIN
ALTER TABLE dbo.TAREA_SOLICITUD ADD
IND_SOLICITUD_PROCESADA BIT NOT NULL CONSTRAINT DF_TAREA_SOLICITUD_IND_SOLICITUD_PROCESADA DEFAULT 0
END
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TAREA_SOLICITUD')
)
BEGIN
ALTER TABLE [dbo].[TAREA_SOLICITUD] WITH CHECK ADD CONSTRAINT [FK_TAREA_SOLICITUD_TAREA_OBSERVACION] FOREIGN KEY ([ID_TAREA_OBSERVACION])
REFERENCES [dbo].[TAREA_OBSERVACION] ([COD_ID_TAREA_OBSERVACION])
END
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TAREA_SOLICITUD')
)
BEGIN
ALTER TABLE [dbo].[TAREA_SOLICITUD] CHECK CONSTRAINT [FK_TAREA_SOLICITUD_TAREA_OBSERVACION]
END
GO

INSERT INTO DOMINIO (DESCRIPCION)
	VALUES ('Tiempo Máximo de Suspensión')
DECLARE @DominioId INT = ( SELECT
		MAX(ID_DOMINIO)
	FROM DOMINIO)
INSERT INTO DOMINIO_DETALLE (ID_DOMINIO, VAL_NOMBRE, DESC_DESCRIPCION, VAL_VALOR)
	VALUES (@DominioId, 'TiempoMaximoSuspension', 'Tiempo Máximo que un título o expediente puede estar en suspensión', '5')

IF EXISTS (SELECT
		*
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[dbo].[SP_OBTENER_EXPEDIENTES_ASIGNADOS]')
	AND type IN (N'P', N'PC'))

BEGIN
DROP PROCEDURE [dbo].[SP_OBTENER_EXPEDIENTES_ASIGNADOS]
END
GO

-- =============================================
-- Author:		Eduar Fabian Hernandez Nieves - Stefanini
-- Create date: 2018-12-21
-- Description:	Procedimiento almacenado para obtener los expedientes asignados a un gestor
-- EXEC SP_OBTENER_EXPEDIENTES_ASIGNADOS 1, 10, '', '', 'ORONCANCIO', '', '', '', '', '', '', '', 15
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

IF @SortExpression = ''
BEGIN
SET @SortExpression = 'EFINROEXP'
	END

	IF @SortDirection = '' BEGIN
SET @SortDirection = 'ASC'
	END

	--Se consulta el numero de días máximos para la suspensión
	DECLARE @MaxDiasSuspension AS VARCHAR(10) = ( SELECT
		VAL_VALOR
	FROM DOMINIO_DETALLE
	WHERE VAL_NOMBRE = 'TiempoMaximoSuspension')

--Se crea tabla temporal con las fechas de inicio de gestión según la fecha de cambio de estado procesal
SELECT
	E.EFINROEXP AS NroExp
   ,MAX(CE.fecha) AS FechaCambioUltimoEstado
   ,MIN(HTA.FEC_ACTUALIZACION) AS FechaGestionDespuesCambioEstado INTO #FechasInicioGestion
FROM EJEFISGLOBAL AS E
JOIN CAMBIOS_ESTADO AS CE
	ON E.EFINROEXP = CE.NroExp
JOIN TAREA_ASIGNADA AS TA
	ON E.EFINROEXP = TA.EFINROEXP_EXPEDIENTE
JOIN HISTORICO_TAREA_ASIGNADA AS HTA
	ON TA.ID_TAREA_ASIGNADA = HTA.ID_TAREA_ASIGNADA
WHERE CE.estado = E.EFIESTADO
-- Validaciones Obligatorias
AND TA.EFINROEXP_EXPEDIENTE IS NOT NULL
AND TA.VAL_USUARIO_NOMBRE IS NOT NULL
AND TA.VAL_USUARIO_NOMBRE <> ''
AND TA.COD_ESTADO_OPERATIVO IN (11, 12, 14, 15, 17, 19)
GROUP BY E.EFINROEXP
		,E.EFIESTADO
		,CE.estado
		,HTA.FEC_ACTUALIZACION
HAVING HTA.FEC_ACTUALIZACION >= MAX(CE.fecha)
ORDER BY E.EFINROEXP

-- Se realizan los filtros dependiendo de los parámetros del SP y se guardan en una tabla temporal
SELECT
DISTINCT
	TAREA_ASIGNADA.ID_TAREA_ASIGNADA AS ID_TAREA_ASIGNADA
   ,CONVERT(VARCHAR, TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE) EFINROEXP
   ,EJEFISGLOBAL.EFIFECHAEXP
   ,EJEFISGLOBAL.EFINIT
   ,EJEFISGLOBAL.EFIFECENTGES
   ,EJEFISGLOBAL.EFIFECCAD
   ,EJEFISGLOBAL.EFIVALDEU
   ,EJEFISGLOBAL.EFIPAGOSCAP
   ,EJEFISGLOBAL.EFISALDOCAP
   ,ESTADOS_PROCESO.CODIGO AS EFIESTADOCODIGO
   ,ESTADOS_PROCESO.nombre AS EFIESTADO
   ,ESTADOS_PAGO.nombre AS EFIESTUP
   ,PERSUASIVO.FecEstiFin
   ,ENTES_DEUDORES.ED_NOMBRE
   ,'OK' AS termino
   ,'      ' AS explicacion
   ,'                    ' AS PictureURL
   ,USUARIOS.codigo AS USUARIOSCODIGO
   ,USUARIOS.nombre AS GESTOR
   ,
	--TAREA_ASIGNADA.VAL_USUARIO_NOMBRE AS GESTOR,
	TITULOSEJECUTIVOS.MT_tipo_titulo AS MT_TIPO_TITULO
   ,COALESCE(TITULOSEJECUTIVOS.NomTipoTitulo, '') AS NomTipoTitulo
   ,ESTADO_OPERATIVO.ID_ESTADO_OPERATIVOS AS COD_ESTADO_OPERATIVO
   ,ESTADO_OPERATIVO.VAL_NOMBRE AS ESTADO_OPERATIVO
   ,TAREA_ASIGNADA.VAL_PRIORIDAD AS VAL_PRIORIDAD
   ,0.00 AS PagoyAjuste
   ,COLORSUSPENSION =
	CASE
		WHEN (CONVERT(INT, DATEDIFF(DAY, FEC_ACTUALIZACION, GETDATE())) >= CONVERT(INT, @MaxDiasSuspension) AND
			TAREA_ASIGNADA.COD_ESTADO_OPERATIVO = 14) THEN 'Rojo'
		ELSE NULL
	END
   ,COLORALERTA =
	CASE
		WHEN ((CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) >= CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_amarillo) AND
			CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) < CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_rojo)) AND
			TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 14, 15, 17, 19)) THEN 'Amarillo'
		WHEN ((CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) >= CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_rojo)) AND
			TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 14, 15, 17, 19)) THEN 'Rojo'
		ELSE NULL
	END
   ,CAST(MT.MT_fec_exi_liq AS DATE) AS FECHALIMITE INTO #ExpedientesAsignados
FROM TAREA_ASIGNADA
LEFT JOIN EJEFISGLOBAL
	ON TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE = EJEFISGLOBAL.EFINROEXP
LEFT JOIN ESTADOS_PROCESO
	ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo
LEFT JOIN ESTADOS_PAGO
	ON EJEFISGLOBAL.EFIESTUP = ESTADOS_PAGO.codigo
LEFT JOIN PERSUASIVO
	ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp
LEFT JOIN ENTES_DEUDORES
	ON EJEFISGLOBAL.EFINIT = ENTES_DEUDORES.ED_Codigo_Nit
LEFT JOIN USUARIOS
	ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo
LEFT JOIN TITULOSEJECUTIVOS
	ON EJEFISGLOBAL.EFINROEXP = TITULOSEJECUTIVOS.MT_expediente
LEFT JOIN ESTADO_OPERATIVO
	ON TAREA_ASIGNADA.COD_ESTADO_OPERATIVO = ESTADO_OPERATIVO.ID_ESTADO_OPERATIVOS
LEFT JOIN #FechasInicioGestion AS FIG
	ON EJEFISGLOBAL.EFINROEXP = FIG.NroExp
LEFT JOIN MAESTRO_TITULOS AS MT
	ON EJEFISGLOBAL.EFINROEXP = MT.MT_expediente
WHERE
-- Filtros
((@USULOG = '')
OR ([TAREA_ASIGNADA].[VAL_USUARIO_NOMBRE] = @USULOG))
AND ((@EFINROEXP = '')
OR (CONVERT(VARCHAR, [TAREA_ASIGNADA].[EFINROEXP_EXPEDIENTE]) = @EFINROEXP))
AND ((@ED_NOMBRE = '')
OR ([ENTES_DEUDORES].[ED_Nombre] LIKE '%' + @ED_NOMBRE + '%'))
AND ((@EFINIT = '')
OR ([EJEFISGLOBAL].[EFINIT] LIKE '%' + @EFINIT + '%'))
AND ((@ESTADOPROC = '')
OR ([EJEFISGLOBAL].[EFIESTADO] = @ESTADOPROC))
AND ((@MT_TIPO_TITULO = '')
OR ([TITULOSEJECUTIVOS].[MT_tipo_titulo] = @MT_TIPO_TITULO))
AND ((@ESTADO_OPERATIVO = 0)
OR ([TAREA_ASIGNADA].[COD_ESTADO_OPERATIVO] = @ESTADO_OPERATIVO))
AND (
(
((@FECTITULO = '')
OR (CONVERT(VARCHAR(10), [TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR], 103) = @FECTITULO))
AND ((@FECENTGES = '')
OR (CONVERT(VARCHAR(10), [TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR], 103) = @FECENTGES))
)--Se valida sobre la tabla tarea_asiganada los valores que se consultan
OR
(
((@FECTITULO = '')
OR (CONVERT(VARCHAR(10), [EJEFISGLOBAL].[EFIFECHAEXP], 103) = @FECTITULO))
AND ((@FECENTGES = '')
OR (CONVERT(VARCHAR(10), [EJEFISGLOBAL].[EFIFECENTGES], 103) = @FECENTGES))
)--Se valida sobre la tabla ya existente
)
-- Condicionales obligatorias
AND TAREA_ASIGNADA.COD_TIPO_OBJ = 5
AND TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 14, 15, 19)
AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE IS NOT NULL
AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE <> ''

-- Se cuentan los registros obtenidos
DECLARE @count VARCHAR(MAX) = (SELECT
		COUNT(*)
	FROM #ExpedientesAsignados)
-- Filtro para paginador
DECLARE @FilterRows VARCHAR(50) = 'WHERE RecordSetID >= ' + CONVERT(VARCHAR(10), @StartRecord) + ' And RecordSetID <= ' + CONVERT(VARCHAR(10), @StopRecord)
-- Se arma la consulta para que sea dinámico
DECLARE @ConsultaExpedientes NVARCHAR(MAX) = 'SELECT * FROM(
													SELECT ROW_NUMBER() OVER (ORDER BY VAL_PRIORIDAD DESC, ' + @SortExpression + ' ' + @SortDirection + ') AS RecordSetID, 
													*,
													' + @count + ' AS RecordSetCount
													FROM #ExpedientesAsignados
												) AS t '
+ @FilterRows

-- Se ejecuta la consulta que se arma de forma dinámica
EXECUTE SP_EXECUTESQL @ConsultaExpedientes

--Se elimina la tablas temporales
DROP TABLE #ExpedientesAsignados
DROP TABLE #FechasInicioGestion
END
GO

IF NOT EXISTS (SELECT
		1
	FROM sys.columns
	WHERE Name = N'MT_fec_notificacion_titulo'
	AND Object_ID = OBJECT_ID(N'dbo.MAESTRO_TITULOS'))
BEGIN
ALTER TABLE [dbo].[MAESTRO_TITULOS]
ADD
[MT_fec_notificacion_titulo] [datetime] NULL,
[MT_for_notificacion_titulo] [char](2) NULL,
[MT_fec_not_reso_resu_reposicion] [datetime] NULL,
[MT_for_not_reso_resu_reposicion] [char](2) NULL,
[MT_fec_not_reso_apela_recon] [datetime] NULL,
[MT_for_not_reso_apela_recon] [char](2) NULL

END
GO

IF NOT EXISTS (SELECT
		1
	FROM sys.columns
	WHERE Name = N'OTRA_FUENTE'
	AND Object_ID = OBJECT_ID(N'dbo.DIRECCIONES'))
BEGIN
ALTER TABLE [dbo].[DIRECCIONES]
ADD OTRA_FUENTE VARCHAR(20) NULL
END
GO

IF EXISTS (SELECT
		*
	FROM sys.objects
	WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertaDireccion]')
	AND type IN (N'P', N'PC'))

BEGIN
DROP PROCEDURE [dbo].[SP_InsertaDireccion]
END

GO
CREATE PROCEDURE [dbo].[SP_InsertaDireccion] 
	@idunico bigint=Null,
	@deudor VARCHAR(13),
	@Direccion VARCHAR(180),
	@Departamento CHAR(2),
	@Ciudad CHAR(5),
	@Telefono VARCHAR(40),
	@Email VARCHAR(160),
	@Movil VARCHAR(40),
	@ID_FUENTE INT,
	@OTRA_FUENTE VARCHAR(20) = NULL
AS
   
BEGIN
SET NOCOUNT ON
   
	IF @idunico <> null
		BEGIN
UPDATE [dbo].[DIRECCIONES]
SET [deudor] = @deudor
   ,[Direccion] = @Direccion
   ,[Departamento] = @Departamento
   ,[Ciudad] = @Ciudad
   ,[Telefono] = @Telefono
   ,[Email] = @Email
   ,[Movil] = @Movil
   ,[paginaweb] = NULL
   ,[ID_FUENTE] = @ID_FUENTE
   ,[OTRA_FUENTE] = @OTRA_FUENTE


WHERE idunico = @idunico

END
ELSE
BEGIN
INSERT INTO [dbo].[DIRECCIONES] ([deudor]
, [Direccion]
, [Departamento]
, [Ciudad]
, [Telefono]
, [Email]
, [Movil]
, [paginaweb]
, [ID_FUENTE]
, [OTRA_FUENTE])
	VALUES (@deudor, @Direccion, @Departamento, @Ciudad, @Telefono, @Email, @Movil, NULL, @ID_FUENTE, @OTRA_FUENTE)

END
END

GO


GO

IF EXISTS (SELECT
		*
	FROM sys.triggers
	WHERE object_id = OBJECT_ID(N'[dbo].[TrgHISTORICO_TAREA_ASIGNADA]'))
BEGIN
DROP TRIGGER [dbo].[TrgHISTORICO_TAREA_ASIGNADA]
END
GO

CREATE TRIGGER [dbo].[TrgHISTORICO_TAREA_ASIGNADA] ON [dbo].[TAREA_ASIGNADA]
	AFTER INSERT,UPDATE
AS
	BEGIN
INSERT INTO [dbo].[HISTORICO_TAREA_ASIGNADA] (ID_TAREA_ASIGNADA, VAL_USUARIO_NOMBRE, COD_TIPO_OBJ, ID_UNICO_TITULO, EFINROEXP_EXPEDIENTE, FEC_ACTUALIZACION, FEC_ENTREGA_GESTOR, VAL_PRIORIDAD, COD_ESTADO_OPERATIVO, IND_TITULO_PRIORIZADO, ID_TAREA_OBSERVACION)
	SELECT
		i.ID_TAREA_ASIGNADA
	   ,i.VAL_USUARIO_NOMBRE
	   ,i.COD_TIPO_OBJ
	   ,i.ID_UNICO_TITULO
	   ,i.EFINROEXP_EXPEDIENTE
	   ,i.FEC_ACTUALIZACION
	   ,i.FEC_ENTREGA_GESTOR
	   ,i.VAL_PRIORIDAD
	   ,i.COD_ESTADO_OPERATIVO
	   ,i.IND_TITULO_PRIORIZADO
	   ,i.ID_TAREA_OBSERVACION
	FROM inserted i
END
GO

ALTER TABLE [dbo].[TAREA_ASIGNADA] ENABLE TRIGGER [TrgHISTORICO_TAREA_ASIGNADA]
GO

SET IDENTITY_INSERT [dbo].[PAGINA] ON
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado])
	VALUES (24, N'Estudio de Títulos', N'Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx', NULL, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[PAGINA] OFF
GO

INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado])
	VALUES (10, 24, 1, 1, 1)
GO


SET IDENTITY_INSERT [dbo].[MODULO] ON
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado])
	VALUES (12, N'Bandeja Estudio de Títulos', N'~/Security/Maestros/estudio-titulos/BandejaTitulos.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado])
	VALUES (13, N'Área Origen', N'~/Security/modulos/maestro-acceso.aspx', N'/Security/images/usuarios.png', 1)
GO
SET IDENTITY_INSERT [dbo].[MODULO] OFF
GO

INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado])
	VALUES (10, 12, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado])
	VALUES (11, 13, 1)
GO

IF (EXISTS (SELECT
		*
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ESTADOS_PROCESO')
)
BEGIN
EXECUTE sp_rename N'dbo.ESTADOS_PROCESO.max_dias_gestion'
				 ,N'Tmp_max_dias_gestion_amarillo_1'
				 ,'COLUMN';
EXECUTE sp_rename N'dbo.ESTADOS_PROCESO.Tmp_max_dias_gestion_amarillo_1'
				 ,N'max_dias_gestion_amarillo'
				 ,'COLUMN';
ALTER TABLE dbo.ESTADOS_PROCESO ADD max_dias_gestion_rojo INT NULL;
END
GO

	IF NOT EXISTS (SELECT
		*
	FROM DBO.MODULO
	WHERE val_nombre = 'Gestores/Estado Procesal')
BEGIN
INSERT INTO DBO.MODULO (val_nombre, val_url, val_url_icono, ind_estado)
	VALUES ('Gestores/Estado Procesal', '/Security/Maestros/ESTADO_PROCESO_GESTOR.aspx', '/Security/images/icons/deusdor.png', 1)
DECLARE @ID AS INT
SET @ID = @@IDENTITY
INSERT INTO dbo.PERFIL_MODULO (fk_perfil_id, fk_modulo_id, ind_estado)
	VALUES (1, @ID, 1)
END
GO

INSERT INTO DOMINIO(DESCRIPCION) VALUES('Estado Solicitud')
GO
DECLARE @LastIdDominio INT = (SELECT MAX(ID_DOMINIO) FROM DOMINIO)
INSERT INTO DOMINIO_DETALLE(ID_DOMINIO, VAL_NOMBRE, DESC_DESCRIPCION, VAL_VALOR)
VALUES
	(@LastIdDominio, 'En espera', 'Solicitud en espera de aprobación', '1'),
	(@LastIdDominio, 'Aprobada', 'Solicitud aprobada', '2'),
	(@LastIdDominio, 'Rechazada', 'Solicitud rechazada', '3')

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TAREA_SOLICITUD')
)
BEGIN
	ALTER TABLE [dbo].[TAREA_SOLICITUD] ADD 
	[COD_ESTADO_SOLICITUD] [int] NULL,
	[FEC_SOLICITUD] [datetime] NULL
END
GO

INSERT INTO MODULO(val_nombre, val_url, val_url_icono, ind_estado)
VALUES('Expedientes', 'Security/Maestros/EJEFISGLOBAL.aspx', '/Security/images/icons/dossier.png', 1)
GO
DECLARE @LASTID INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO PERFIL_MODULO(fk_perfil_id, fk_modulo_id, ind_estado)
VALUES
	(2, @LASTID, 1),
	(3, @LASTID, 1),
	(4, @LASTID, 1),
	(6, @LASTID, 1),
	(8, @LASTID, 1)
GO
INSERT INTO MODULO(val_nombre, val_url, val_url_icono, ind_estado)
VALUES('Reasignaciones', 'Security/Maestros/aprobaciones/Reasignacion.aspx', '/Security/images/icons/dossier.png', 1)
GO
DECLARE @LASTID2 INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO PERFIL_MODULO(fk_perfil_id, fk_modulo_id, ind_estado)
VALUES
	(2, @LASTID2, 1),
	(3, @LASTID2, 1)
GO
INSERT INTO MODULO(val_nombre, val_url, val_url_icono, ind_estado)
VALUES('Informes', 'Security/FrmGrupoReportes.aspx', '/Security/images/icons/informes96x96.png', 1)
GO
DECLARE @LASTID3 INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO PERFIL_MODULO(fk_perfil_id, fk_modulo_id, ind_estado)
VALUES
	(8, @LASTID3, 1)
GO

INSERT INTO MODULO(val_nombre, val_url, val_url_icono, ind_estado)
VALUES('Cambios de Estado', 'Security/Maestros/aprobaciones/CambioEstado.aspx', '/Security/images/icons/dossier.png', 1)
GO
DECLARE @LASTID4 INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO PERFIL_MODULO(fk_perfil_id, fk_modulo_id, ind_estado)
VALUES
	(2, @LASTID4, 1),
	(3, @LASTID4, 1),
	(6, @LASTID4, 1),
	(8, @LASTID4, 1)
GO

INSERT INTO MODULO(val_nombre, val_url, val_url_icono, ind_estado)
VALUES('Priorizacion', 'Security/Maestros/aprobaciones/Priorizacion.aspx', '/Security/images/icons/dossier.png', 1)
GO
DECLARE @LASTID5 INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO PERFIL_MODULO(fk_perfil_id, fk_modulo_id, ind_estado)
VALUES
	(2, @LASTID5, 1),
	(3, @LASTID5, 1)
GO

	IF NOT EXISTS (SELECT
		*
	FROM DBO.MODULO
	WHERE val_nombre = 'Valores Tipo Obligación')
BEGIN
INSERT INTO DBO.MODULO (val_nombre, val_url, val_url_icono, ind_estado)
	VALUES ('Valores Tipo Obligación', '/Security/Maestros/ParameValores.aspx', '/Security/images/icons/deusdor.png', 1)
DECLARE @ID AS INT
SET @ID = @@IDENTITY
INSERT INTO dbo.PERFIL_MODULO (fk_perfil_id, fk_modulo_id, ind_estado)
	VALUES (1, @ID, 1)
END
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_VERIFICA_SI_USUARIO_ES_TERCER_NIVEL]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_VERIFICA_SI_USUARIO_ES_TERCER_NIVEL]
END
GO

-- =============================================
-- Author:		Edward Fabian Hernández Nieves
-- Create date: 2019-01-11
-- Description:	Verifica si el usuario pasado como parámetro es un usuario de tercer nivel
-- EXEC SP_VERIFICA_SI_USUARIO_ES_TERCER_NIVEL 'LGONZALEZC'
-- =============================================
CREATE PROCEDURE [dbo].[SP_VERIFICA_SI_USUARIO_ES_TERCER_NIVEL]
	@USULOG NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT *
	FROM USUARIOS 
	WHERE codigo IN(
		SELECT superior FROM USUARIOS 
		where codigo IN(
			SELECT DISTINCT superior FROM USUARIOS 
			where codigo IN(
				SELECT DISTINCT superior FROM USUARIOS
			)
		)
	)
	AND login = @USULOG
END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_OBTENER_SOLICITUDES_POR_TIPO_SOLICITUD]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_POR_TIPO_SOLICITUD]
END
GO

-- =============================================
-- Author:		Edward Fabian Hernández Nieves
-- Create date: 2019-01-11
-- Description:	Consulta todas las solicitudes de priorización para ser aprobadas
-- EXEC SP_OBTENER_SOLICITUDES_POR_TIPO_SOLICITUD 'LGONZALEZC', 8, '', '', '', 0
-- =============================================
CREATE PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_POR_TIPO_SOLICITUD]
	@IdTipoSolicitud INT, --Sacdo de la tabla DOMINIO_DETALLE con el ID_COMINIO = 3
	@LoginUsuarioSuperior NVARCHAR(20) = '', -- Login del usuario superior (se realiza con el login para tener en cuenta LDAP)
	@EFINROEXP AS NVARCHAR(20) = '',
	@ID_UNICO_TITULO AS NVARCHAR(20) = '',
	@LoginUsuarioSolicitante AS NVARCHAR(20) = '',
	@EstadoSolicitud AS INT = 0
AS
BEGIN
	SET NOCOUNT ON; 

	--Se actualiza el estado de la solicitud a en espera a todas las solicitudes con este campo en nulo
	UPDATE TAREA_SOLICITUD SET COD_ESTADO_SOLICITUD = 16 WHERE COD_ESTADO_SOLICITUD IS NULL

	-- Se obtiene el código del usuario para buscar por código y por login
	DECLARE @codigoUsuarioSuperior NVARCHAR(20) = (SELECT codigo FROM USUARIOS WHERE login = @LoginUsuarioSuperior)

	SELECT
		TA.ID_TAREA_ASIGNADA,
		TS.ID_TAREA_SOLICITUD,
		TA.EFINROEXP_EXPEDIENTE AS EFINROEXP, -- Número de expediente
		TA.ID_UNICO_TITULO AS ID_UNICO_TITULO, -- ID Temporal para el usuario abogado
		
		USUARIO_SOLICITANTE.nombre AS GESTOR_SOLICITANTE,
		TS.FEC_SOLICITUD AS FECHASOLICITUD,
		EP.nombre AS ESTADO_ACTUAL,
		TS.COD_ESTADO_SOLICITUD,
		ESTADO_SOLICITUD.VAL_NOMBRE AS ESTADO_APROBACION,
		TIPO_SOLICITUD.VAL_NOMBRE AS TIPO_SOLICITUD,
		USUARIO_SOLICITADO.nombre AS GESTOR_SOLICITADO,
		USUARIO_APROBADOR.nombre AS GESTOR_APROBADOR
	FROM TAREA_SOLICITUD AS TS
	LEFT JOIN TAREA_ASIGNADA AS TA ON TS.ID_TAREA_ASIGNADA = TA.ID_TAREA_ASIGNADA
	LEFT JOIN USUARIOS AS USUARIO_SOLICITANTE ON TS.VAL_USUARIO_SOLICITANTE = USUARIO_SOLICITANTE.login
	LEFT JOIN USUARIOS AS USUARIO_SOLICITADO ON TS.VAL_USUARIO_DESTINO = USUARIO_SOLICITADO.login
	LEFT JOIN USUARIOS AS USUARIO_APROBADOR ON TS.VAL_USUARIO_APROBADOR = USUARIO_APROBADOR.login
	LEFT JOIN EJEFISGLOBAL AS E ON TA.EFINROEXP_EXPEDIENTE = E.EFINROEXP
	LEFT JOIN ESTADOS_PROCESO AS EP ON E.EFIESTADO = EP.codigo
	LEFT JOIN DOMINIO_DETALLE AS ESTADO_SOLICITUD ON TS.COD_ESTADO_SOLICITUD = ESTADO_SOLICITUD.ID_DOMINIO_DETALLE
	LEFT JOIN DOMINIO_DETALLE AS TIPO_SOLICITUD ON TS.VAL_TIPO_SOLICITUD = TIPO_SOLICITUD.ID_DOMINIO_DETALLE
	WHERE
		TS.VAL_TIPO_SOLICITUD = @IdTipoSolicitud --ID de la tabla DOMINIO_DETALLE
		AND (@LoginUsuarioSuperior = '' OR TS.VAL_USUARIO_APROBADOR IN(@LoginUsuarioSuperior, @codigoUsuarioSuperior))
		AND (@EFINROEXP = '' OR TA.EFINROEXP_EXPEDIENTE = @EFINROEXP)
		AND (@ID_UNICO_TITULO = '' OR TA.ID_UNICO_TITULO = CAST(@ID_UNICO_TITULO AS INT))
		AND (@LoginUsuarioSolicitante = '' OR TS.VAL_USUARIO_SOLICITANTE = @LoginUsuarioSolicitante)
		AND (@EstadoSolicitud = 0 OR TS.COD_ESTADO_SOLICITUD = @EstadoSolicitud)
	ORDER BY TS.COD_ESTADO_SOLICITUD ASC, TS.FEC_SOLICITUD DESC
END
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO]
END
GO

-- =============================================
-- Author:		Edward Fabián Hernández Nieves
-- Create date: 2019-01-11
-- Description:	Procedimiento almacenado que obtiene las solicitudes de cambio de estado pendientes por aprobar
-- EXEC SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO 'LGONZALEZC', '', '', ''
-- =============================================
CREATE PROCEDURE [dbo].[SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO]
	@USULOG NVARCHAR(20), --Usuario que se encuentra logeado, se toma como revisor
	@EFINROEXP AS NVARCHAR(20) = '',
	@LoginUsuarioSolicitante AS NVARCHAR(20) = ''
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		SCE.idunico AS ID_SOLICITUD_REASIGNACION,
		SCE.NroExp AS EFINROEXP,
		USUARIO_SOLICITUD = 
			CASE 
				WHEN USUARIO_SOLICITUD.login IS NOT NULL THEN 
					USUARIO_SOLICITUD.nombre
				ELSE 
					GESTOR_SOLICITUD.nombre
			END,
		SCE.FECHA as FECHA_SOLICITUD,
		ESTADOS_PROCESO_ACTUAL.nombre AS ESTADOS_PROCESO_ACTUAL,
		ESTADOS_PROCESO_SOLICITADO.nombre AS ESTADOS_PROCESO_SOLICITADO,
		ESTADO_SOLICITUD.nombre AS ESTADO_SOLICITUD,
		APROBADOR = 
			CASE 
				WHEN USUARIO_APROBADOR.login IS NOT NULL THEN 
					USUARIO_APROBADOR.nombre
				ELSE 
					GESTOR_APROBADOR.nombre
			END
	FROM 
		SOLICITUDES_CAMBIOESTADO AS SCE
		LEFT JOIN ESTADOS_PROCESO AS ESTADOS_PROCESO_ACTUAL ON SCE.estadoactual = ESTADOS_PROCESO_ACTUAL.codigo
		LEFT JOIN ESTADOS_PROCESO AS ESTADOS_PROCESO_SOLICITADO ON SCE.estadosol = ESTADOS_PROCESO_SOLICITADO.codigo
		LEFT JOIN ESTADOS_SOL_CAM_EST AS ESTADO_SOLICITUD ON SCE.estadosol = ESTADO_SOLICITUD.codigo
		LEFT JOIN TAREA_ASIGNADA AS TA ON SCE.NroExp = TA.EFINROEXP_EXPEDIENTE
		LEFT JOIN TAREA_SOLICITUD AS TS ON TA.ID_TAREA_ASIGNADA = TS.ID_TAREA_ASIGNADA
		LEFT JOIN USUARIOS AS USUARIO_SOLICITUD ON TS.VAL_USUARIO_SOLICITANTE = USUARIO_SOLICITUD.login
		LEFT JOIN USUARIOS AS USUARIO_APROBADOR ON TS.VAL_USUARIO_APROBADOR = USUARIO_APROBADOR.login
		LEFT JOIN USUARIOS AS GESTOR_SOLICITUD ON SCE.abogado = GESTOR_SOLICITUD.codigo
		LEFT JOIN USUARIOS AS GESTOR_APROBADOR ON SCE.revisor = GESTOR_APROBADOR.codigo
	WHERE 
		SCE.estadosol = 1
		AND (USUARIO_APROBADOR.login = @USULOG OR GESTOR_APROBADOR.login = @USULOG)
		AND (@EFINROEXP = '' OR (SCE.NroExp = @EFINROEXP) OR (TA.EFINROEXP_EXPEDIENTE = @EFINROEXP) )
		AND (@LoginUsuarioSolicitante = '' OR (USUARIO_SOLICITUD.login = @LoginUsuarioSolicitante) OR (GESTOR_SOLICITUD.login = @LoginUsuarioSolicitante) )
END
GO

--Se agrega update a tabla con datos necesarios para el funcionamiento de la aplicación
UPDATE ESTADOS_PROCESO SET max_dias_gestion_amarillo = 18, max_dias_gestion_rojo = 30
GO

--Se eliminan los datos que existían previamente
DELETE FROM PAGINA
DELETE FROM PERFIL_PAGINA
DELETE FROM MODULO
DELETE FROM PERFIL_MODULO
GO

SET IDENTITY_INSERT [dbo].[PAGINA] ON 
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (1, N'Seguridad', NULL, NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (2, N'Páginas', N'/Security/admin/paginas/paginas.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (3, N'Agregar Página', N'/Security/admin/paginas/addPagina.aspx', 2, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (5, N'Editar Pagina', N'/Security/paginas/editPagina.aspx?idPagina=', 2, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (6, N'Perfiles', N'/Security/admin/perfiles/perfiles.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (7, N'Agregar Perfil', N'/Security/admin/perfiles/addPerfil.aspx', 6, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (8, N'Editar Perfil', N'/Security/admin/perfiles/editPerfil.aspx?idPerfil=', 6, 1, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (10, N'Acceso por Perfil', N'/Security/admin/adminAccesoPerfilesPaginas.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (12, N'Modulos', N'/Security/admin/modulos/modulos-list.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (14, N'Paginas por perfil', N'/Security/admin/AdminAccesoPerfilesPaginas.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (18, N'Agregar modulo', N'/Security/admin/modulos/addModulo.aspx', 12, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (19, N'Editar Módulo', N'/Security/admin/modulos/editModulo.aspx?idModulo=', 12, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (20, N'Acceso Módulos por Perfil', N'Security/admin/AdminAccesoPerfilesModulos.aspx', 1, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (22, N'Bandeja', N'/Security/Area_Origen/BandejaAreaOrigen.aspx', NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (23, N'Crear titulo', N'/Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx', NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (24, N'Estudio de Títulos', N'Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx', NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (25, N'Información General', N'/Security/Maestros/estudio-titulos/info-general.aspx', 24, 0, 0)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (26, N'Valores', N'/Security/Maestros/estudio-titulos/valores-pagina.aspx', 24, 0, 0)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (27, N'Deudor', N'/Security/Maestros/ENTES_DEUDORES.aspx', 24, 0, 0)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (28, N'Documentos titulo', N'/Security/Maestros/estudio-titulos/documentos-pagina.aspx', 24, 0, 0)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (29, N'Estudio de Títulos', N'Security/Maestros/EditMAESTRO_TITULOS_AORIGEN.aspx', NULL, 0, 1)
GO
INSERT [dbo].[PAGINA] ([pk_codigo], [val_nombre], [val_url], [fk_padre], [ind_pagina_interna], [ind_estado]) VALUES (30, N'Área Origen', N'Security/Modulos/MaestroAreaOrigen.aspx', NULL, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[PAGINA] OFF
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 1, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 2, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 3, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 5, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 6, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 7, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 8, 1, 0, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 10, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 12, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 14, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 18, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (1, 20, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (2, 1, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (2, 2, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (2, 3, 0, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (2, 5, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (2, 7, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (3, 2, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (3, 3, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (3, 5, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (3, 6, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (4, 1, 1, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (4, 2, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (4, 3, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (4, 5, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (6, 5, 1, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (6, 12, 1, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (6, 14, 0, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (7, 1, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 14, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 23, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 24, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 25, 1, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 26, 1, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 27, 1, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 28, 1, 1, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (10, 29, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (11, 1, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (11, 19, 0, 0, 0)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (11, 22, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (11, 23, 1, 1, 1)
GO
INSERT [dbo].[PERFIL_PAGINA] ([fk_perfil_id], [fk_pagina_id], [ind_puede_ver], [ind_puede_editar], [ind_estado]) VALUES (11, 30, 1, 1, 1)
GO

SET IDENTITY_INSERT [dbo].[MODULO] ON 
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (2, N'Auditoria', N'/Security/Maestros/LOG_AUDITORIA.aspx', N'/Security/images/icons/auditar.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (17, N'Diccionario Auditoria', N'/Security/Maestros/DICCIONARIO_AUDITORIA.aspx', N'/Security/images/icons/Blocnote.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (19, N'Seguridad', N'/Security/modulos/maestro-acceso.aspx', N'/Security/images/icons/Keys.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (25, N'Datos basicos', N'/Securiry/MenuMaestros.aspx', N'/Security/images/icons/HP-Control.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (26, N'Administrar usarios', N'/Security/menu4.aspx', N'/Security/images/icons/Keys.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (27, N'Administar expedientes', N'/Security/Maestros/EJEFISGLOBALREPARTIDOR.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (28, N'Log / Auditoria procesos', N'/Security/Maestros/LOG_AUDITORIA.aspx', N'/Security/images/icons/auditar.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (29, N'Informes', N'/Security/FrmGrupoReportes.aspx', N'/Security/images/icons/informes96x96.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (30, N'Sincronizar usuarios', N'/Security/Maestros/sincronizarusuarios.aspx', N'/Security/images/usuarios.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (31, N'Bandeja Estudio de Títulos', N'/Security/Maestros/estudio-titulos/BandejaTitulos.aspx?test=1', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (32, N'Gestión Documento', N'/Security/Maestros/GESTION_DOCUMENTO.aspx', N'/Security/images/icons/Document.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (33, N'Administracion Estado procesal_Etapa procesal', N'/Security/Maestros/AdminEstadoProcesal_EtapaProcesal.aspx', N'/Security/images/icons/icon_06_r.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (34, N'Gestión estado procesal/etapa procesal', N'/Security/Maestros/AdminEstadoProcesal_EtapaProcesal.aspx', N'/Security/images/icons/Bloc_NotesSZ.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (38, N'Estudio de Títulos', N'Security/Maestros/estudio-titulos/BandejaTitulos.aspx', N'/Security/images/icons/Keys.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (39, N'Área Origen', N'Security/modulos/maestro-acceso.aspx', N'/Security/images/usuarios.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (40, N'Gestores/Estado Procesal', N'Security/Maestros/ESTADO_PROCESO_GESTOR.aspx', N'/Security/images/icons/deusdor.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (47, N'Expedientes', N'Security/Maestros/EJEFISGLOBAL.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (48, N'Reasignaciones', N'Security/Maestros/aprobaciones/Reasignacion.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (49, N'Informes', N'Security/FrmGrupoReportes.aspx', N'/Security/images/icons/informes96x96.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (50, N'Cambios de Estado', N'Security/Maestros/aprobaciones/CambioEstado.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (51, N'Priorizacion', N'Security/Maestros/aprobaciones/Priorizacion.aspx', N'/Security/images/icons/dossier.png', 1)
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (52, N'Valores Tipo Obligación', N'/Security/Maestros/ParameValores.aspx', N'/Security/images/icons/deusdor.png', 1)
GO
SET IDENTITY_INSERT [dbo].[MODULO] OFF
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 2, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 17, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 19, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 25, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 26, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 27, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 28, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 29, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 31, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 32, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 33, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 34, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 40, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, 52, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (2, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (2, 48, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (2, 50, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (2, 51, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (3, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (3, 48, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (3, 50, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (3, 51, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (4, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (6, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (6, 50, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (8, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (8, 49, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (8, 50, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (10, 31, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (10, 33, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (11, 2, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (11, 19, 0)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (11, 39, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 2, 1)
GO

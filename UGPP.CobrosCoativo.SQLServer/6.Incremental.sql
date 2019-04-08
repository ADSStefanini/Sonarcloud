﻿IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TIPOS_IDENTIFICACION')
)
BEGIN
	ALTER TABLE dbo.TIPOS_IDENTIFICACION ADD ind_estado bit NULL
	ALTER TABLE dbo.TIPOS_IDENTIFICACION ADD CONSTRAINT DF_TIPOS_IDENTIFICACION_ind_estado DEFAULT 1 FOR ind_estado
END
GO

IF COL_LENGTH('dbo.TIPOS_IDENTIFICACION','ind_estado') IS NOT NULL
BEGIN
	UPDATE dbo.TIPOS_IDENTIFICACION SET ind_estado = 1
	UPDATE dbo.TIPOS_IDENTIFICACION SET ind_estado = 0 WHERE nombre = 'NR'
END
GO

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TIPOS_PERSONA')
)
BEGIN
	ALTER TABLE dbo.TIPOS_PERSONA ADD ind_estado bit NULL
	ALTER TABLE dbo.TIPOS_PERSONA ADD CONSTRAINT DF_TIPOS_PERSONA_ind_estado DEFAULT 1 FOR ind_estado
END
GO

IF COL_LENGTH('dbo.TIPOS_PERSONA','ind_estado') IS NOT NULL
BEGIN
	UPDATE dbo.TIPOS_PERSONA SET ind_estado = 1
	UPDATE dbo.TIPOS_PERSONA SET ind_estado = 0 WHERE nombre = 'SIN DATOS'
END
GO

INSERT [dbo].[MODULO] ([val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES ( N'Administración de Expedientes', N'Security/Maestros/EJEFISGLOBALREPARTIDOR.aspx', N'/Security/images/icons/deusdor.png', 1)
GO
DECLARE @LastIdModulo INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (1, @LastIdModulo, 1)
GO

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TIPO_OBLIGACION_VALORES')
)
BEGIN
	UPDATE TIPO_OBLIGACION_VALORES SET SANCION_INEXACTITUD = 0 WHERE ID_TIPO_OBLIGACION_VALORES = '01'
END
GO


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
			AND TA.COD_ESTADO_OPERATIVO IN (4, 9)
			AND COD_TIPO_OBJ = 4
		ORDER BY
			MT.MT_fec_cad_presc DESC,
			TA.FEC_ENTREGA_GESTOR ASC
	)
END

GO
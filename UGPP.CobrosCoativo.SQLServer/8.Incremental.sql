IF (EXISTS (SELECT * 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'dbo' 
            AND  TABLE_NAME = 'MAESTRO_HOMOLOGACION'))
BEGIN
	UPDATE MAESTRO_HOMOLOGACION SET FUENTE = 'Parafiscales' WHERE FUENTE = 'ParaFiscal'
END

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FUENTE_DIRECCION')
)
BEGIN
	ALTER TABLE dbo.FUENTE_DIRECCION ADD ind_estado bit NULL
	ALTER TABLE dbo.FUENTE_DIRECCION ADD CONSTRAINT DF_FUENTE_DIRECCION_ind_estado DEFAULT 1 FOR ind_estado
END
GO

UPDATE FUENTE_DIRECCION SET ind_estado = 1

SET IDENTITY_INSERT [dbo].[FUENTE_DIRECCION] ON 
GO
INSERT [dbo].[FUENTE_DIRECCION] ([ID_FUENTE_DIRECCION], [DES_NOMBRE_FUENTE_DIRECCION], [ind_estado]) VALUES (5, N'PILA', 1)
GO
INSERT [dbo].[FUENTE_DIRECCION] ([ID_FUENTE_DIRECCION], [DES_NOMBRE_FUENTE_DIRECCION], [ind_estado]) VALUES (6, N'RUA', 1)
GO
SET IDENTITY_INSERT [dbo].[FUENTE_DIRECCION] OFF
GO

UPDATE MAESTRO_HOMOLOGACION SET FUENTE = '330001' WHERE DESTINO = 1 AND ID_TIPO_HOMOLOGACION = 9
UPDATE MAESTRO_HOMOLOGACION SET FUENTE = '330002' WHERE DESTINO = 2 AND ID_TIPO_HOMOLOGACION = 9
UPDATE MAESTRO_HOMOLOGACION SET FUENTE = '330004' WHERE DESTINO = 3 AND ID_TIPO_HOMOLOGACION = 9
UPDATE MAESTRO_HOMOLOGACION SET FUENTE = '330006' WHERE DESTINO = 4 AND ID_TIPO_HOMOLOGACION = 9

SET IDENTITY_INSERT [dbo].[MAESTRO_HOMOLOGACION] ON 
GO
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO]) VALUES (1220, 9, N'330003', N'5', 1)
GO
INSERT [dbo].[MAESTRO_HOMOLOGACION] ([ID_HOMOLOGACION], [ID_TIPO_HOMOLOGACION], [FUENTE], [DESTINO], [ACTIVO]) VALUES (1220, 9, N'330005', N'6', 1)
GO
SET IDENTITY_INSERT [dbo].[MAESTRO_HOMOLOGACION] OFF
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_FUENTE_DIRECCIONES]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_FUENTE_DIRECCIONES] 
END
GO


CREATE PROCEDURE [dbo].[SP_FUENTE_DIRECCIONES]
AS
	 SELECT F.ID_FUENTE_DIRECCION,F.DES_NOMBRE_FUENTE_DIRECCION FROM FUENTE_DIRECCION F WHERE F.ind_estado = 1
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
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE VAL_USUARIO_NOMBRE  = @USULOG  AND COD_TIPO_OBJ = 4 AND (COD_ESTADO_OPERATIVO IN(7, 9) OR IND_TITULO_PRIORIZADO = 1)

	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE ID_TAREA_ASIGNADA IN(
		SELECT TOP 1 TA.ID_TAREA_ASIGNADA 
		FROM TAREA_ASIGNADA TA
		JOIN MAESTRO_TITULOS MT ON TA.ID_UNICO_TITULO = MT.idunico
		WHERE 
			(TA.IND_TITULO_PRIORIZADO IS NULL OR TA.IND_TITULO_PRIORIZADO = 0)
			AND TA.VAL_USUARIO_NOMBRE = @USULOG
			AND TA.COD_ESTADO_OPERATIVO IN (4, 5)
			AND COD_TIPO_OBJ = 4
		ORDER BY
			TA.COD_ESTADO_OPERATIVO DESC,
			MT.MT_fec_cad_presc DESC,
			TA.FEC_ENTREGA_GESTOR ASC
	)
END

GO


IF EXISTS ( SELECT  *
			FROM    sys.objects
			WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ObtenerTitulosEstudioTitulos]')
					AND type IN ( N'P', N'PC' ) ) 
BEGIN 
	DROP PROCEDURE [dbo].[SP_ObtenerTitulosEstudioTitulos] 
END
GO

-- =============================================
-- Author: Eduar Fabian Hernandez Nieves
-- Create date: 2018-11-14
-- Description: Carga los títulos para el festor de estudio de títulos
--- EXEC [SP_ObtenerTitulosEstudioTitulos] 'MFONG','',0,0,'','','',''
-- =============================================

CREATE PROCEDURE [dbo].[SP_ObtenerTitulosEstudioTitulos] 
   @USULOG AS VARCHAR(20), -- Usuario al cual se le asigno la tarea
   @NROTITULO AS VARCHAR(50) , -- Número de título
   @ESTADOPROCESAL AS INT,
   @ESTADOSOPERATIVO AS INT,
   @FCHENVIOCOBRANZADESDE AS VARCHAR(10),
   @FCHENVIOCOBRANZAHASTA AS VARCHAR(10),
   @NROIDENTIFICACIONDEUDOR AS VARCHAR(50),
   @NOMBREDEUDOR AS VARCHAR (50)
   AS
   
BEGIN                     
      
SET NOCOUNT ON

	SELECT
		MT.idunico AS IDUNICO,
		TA.ID_TAREA_ASIGNADA,
		MT.MT_nro_titulo AS NROTITULO,
		CONVERT(VARCHAR(10),MT.MT_fec_expedicion_titulo,103) AS FCHEXPEDICIONTITULO,
		ED.ED_Nombre AS NOMBREDEUDOR,
		ED.ED_Codigo_Nit AS NRONITCEDULA,
		TT.nombre AS TIPOOBLIGACION,
		(
			MT.MT_valor_obligacion + MT.MT_partida_global + MT.MT_sancion_omision + MT.MT_sancion_inexactitud + MT.MT_sancion_mora
		) AS TOTALOBLIGACION, --Se realiza la suma de los valores
		CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) AS FEC_ENTREGA_GESTOR, 
		CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  TT.DIAS_MAX_GESTION_ROJO,103)  AS FCHLIMITE, 
		CASE WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >=  TT.DIAS_MAX_GESTION_ROJO THEN 'ROJO' 
		WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >= TT.DIAS_MAX_GESTION_AMARILLO  AND  DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) <  TT.DIAS_MAX_GESTION_ROJO  THEN 'AMARILLO'END
		AS COLOR,
		EO.ID_ESTADO_OPERATIVOS,
		TA.VAL_PRIORIDAD  
	FROM 
		TAREA_ASIGNADA TA
		LEFT JOIN [dbo].[ESTADO_OPERATIVO] EO WITH (NOLOCK) ON TA.COD_ESTADO_OPERATIVO = EO.ID_ESTADO_OPERATIVOS
		LEFT JOIN MAESTRO_TITULOS MT WITH (NOLOCK) ON MT.idunico = TA.ID_UNICO_TITULO
		LEFT JOIN [dbo].[TIPOS_TITULO] TT WITH (NOLOCK) ON TT.codigo = MT.MT_tipo_titulo
		LEFT JOIN [dbo].[DEUDORES_EXPEDIENTES] DE WITH (NOLOCK) ON MT.idunico = DE.ID_MAESTRO_TITULOS
		LEFT JOIN [dbo].[ENTES_DEUDORES] ED WITH (NOLOCK) ON DE.deudor = ED.ED_Codigo_Nit
	WHERE 
		(
			MT.MT_expediente IS NULL OR (MT.MT_expediente IS NOT NULL AND TA.COD_ESTADO_OPERATIVO IN(3, 4, 5, 7, 9, 13)) 
			AND 
			MT.MT_expediente IN(SELECT EFINROEXP FROM EJEFISGLOBAL WHERE EFINROEXP = MT.MT_expediente AND (EFIESTADO IS NULL OR EFIESTADO = 13) )
		)
		AND TA.ID_UNICO_TITULO IS NOT NULL
		AND ((@USULOG IS NULL) OR (VAL_USUARIO_NOMBRE = @USULOG))
		AND ((@NROTITULO = '') OR (MT.MT_nro_titulo LIKE '%' + @NROTITULO + '%'))
		--AND ((@ESTADOPROCESAL IS NULL) OR (col3 = @ESTADOPROCESAL))
		AND ((@ESTADOSOPERATIVO = 0) OR (EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO))
		AND ((@FCHENVIOCOBRANZADESDE = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) >= @FCHENVIOCOBRANZADESDE))
		AND ((@FCHENVIOCOBRANZAHASTA = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) <= @FCHENVIOCOBRANZAHASTA))
		AND ((@NROIDENTIFICACIONDEUDOR = '') OR (UPPER(ED.ED_Codigo_Nit) LIKE '%' + UPPER(@NROIDENTIFICACIONDEUDOR)  + '%'))
		AND ((@NOMBREDEUDOR = '') OR (UPPER(ED.ED_Nombre) LIKE '%' + UPPER(@NOMBREDEUDOR) + '%'))
		AND (TA.COD_ESTADO_OPERATIVO IN(3, 4, 5, 9, 7, 13))
	ORDER BY 
		TA.IND_TITULO_PRIORIZADO DESC, 
		TA.VAL_PRIORIDAD DESC, 
		MT.fechaRevoca ASC, 
		TA.FEC_ENTREGA_GESTOR ASC 
 END
GO


INSERT INTO [dbo].[MODULO] ([val_nombre],[val_url],[val_url_icono],[ind_estado])
VALUES('Sincronizar usuarios','Security/Maestros/sincronizarusuarios.aspx','/Security/images/usuarios.png',1)
GO
DECLARE @ID INT = (SELECT MAX(pk_codigo) FROM MODULO)
INSERT INTO [dbo].[PERFIL_MODULO] ([fk_perfil_id],[fk_modulo_id],[ind_estado])
VALUES(1 ,@ID ,1)
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaDocumento]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaDocumento]  
END
GO

-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE
-- Create date: 31/10/2018
-- Description: INSERTA EL DOCUMENTO
/* EXEC SP_InsertaDocumento
	@ID_DOCUMENTO_TITULO = 0,
	@ID_MAESTRO_TITULO = 15679,
	@DES_RUTA_DOCUMENTO = '',
	@TIPO_RUTA = 1,
	@COD_GUID = 'D68C860F-4063-4D0A-81D5-2A3DE0505454',
	@COD_TIPO_DOCUMENTO_AO = '1',
	@NOM_DOC_AO = 'Prueba DOC 1',
	@OBSERVA_LEGIBILIDAD = '',
	@NUM_PAGINAS = 0 */
-- =============================================
CREATE PROCEDURE [dbo].[SP_InsertaDocumento]
       @ID_DOCUMENTO_TITULO INT,
       @ID_MAESTRO_TITULO BIGINT,
       @DES_RUTA_DOCUMENTO VARCHAR(200),
       @TIPO_RUTA  INT,
       @COD_GUID VARCHAR(50)=NULL,
       @COD_TIPO_DOCUMENTO_AO VARCHAR(50)=null,
       @NOM_DOC_AO VARCHAR(50)=NULL,
       @OBSERVA_LEGIBILIDAD VARCHAR(50)=NULL,
       @NUM_PAGINAS INT=NULL
   AS
BEGIN
	SET NOCOUNT ON
	IF @ID_DOCUMENTO_TITULO > 0 AND (EXISTS (SELECT * FROM [dbo].[MAESTRO_TITULOS_DOCUMENTOS] WHERE  ID_DOCUMENTO_TITULO =  @ID_DOCUMENTO_TITULO AND ID_MAESTRO_TITULO = @ID_MAESTRO_TITULO))
	BEGIN
		-- Se actualiza el documento relacionado para el tipo de documento
		UPDATE  [dbo].[MAESTRO_TITULOS_DOCUMENTOS]
		SET
			[ID_DOCUMENTO_TITULO] = @ID_DOCUMENTO_TITULO
			,[ID_MAESTRO_TITULO] = @ID_MAESTRO_TITULO
			,[DES_RUTA_DOCUMENTO] = @DES_RUTA_DOCUMENTO
			,[TIPO_RUTA] = @TIPO_RUTA
			,[COD_GUID] = ISNULL(@COD_GUID,[COD_GUID])
			,[COD_TIPO_DOCUMENTO_AO] = ISNULL(@COD_TIPO_DOCUMENTO_AO,[COD_TIPO_DOCUMENTO_AO])
			,[NOM_DOC_AO] = ISNULL(@NOM_DOC_AO,[NOM_DOC_AO])
			,[OBSERVA_LEGIBILIDAD] = ISNULL(@OBSERVA_LEGIBILIDAD,[OBSERVA_LEGIBILIDAD])
			,[NUM_PAGINAS] = ISNULL(@NUM_PAGINAS,[NUM_PAGINAS])
		WHERE  ID_DOCUMENTO_TITULO =  @ID_DOCUMENTO_TITULO AND ID_MAESTRO_TITULO = @ID_MAESTRO_TITULO
	END
	ELSE
	BEGIN
		IF @COD_GUID <> '' AND @COD_GUID IS NOT NULL AND (EXISTS (SELECT * FROM [dbo].[MAESTRO_TITULOS_DOCUMENTOS] WHERE COD_GUID = @COD_GUID AND ID_MAESTRO_TITULO = @ID_MAESTRO_TITULO))
		BEGIN
			-- Si es un código GUID se actualiza los datos para el título, solo para los títulos automáticos
			UPDATE  [dbo].[MAESTRO_TITULOS_DOCUMENTOS]
			SET
				[ID_DOCUMENTO_TITULO] = @ID_DOCUMENTO_TITULO
			   ,[ID_MAESTRO_TITULO] = @ID_MAESTRO_TITULO
			   ,[DES_RUTA_DOCUMENTO] = @DES_RUTA_DOCUMENTO
			   ,[TIPO_RUTA] = @TIPO_RUTA
			   ,[COD_GUID] = ISNULL(@COD_GUID,[COD_GUID])
			   ,[COD_TIPO_DOCUMENTO_AO] = ISNULL(@COD_TIPO_DOCUMENTO_AO,[COD_TIPO_DOCUMENTO_AO])
			   ,[NOM_DOC_AO] = ISNULL(@NOM_DOC_AO,[NOM_DOC_AO])
			   ,[OBSERVA_LEGIBILIDAD] = ISNULL(@OBSERVA_LEGIBILIDAD,[OBSERVA_LEGIBILIDAD])
			   ,[NUM_PAGINAS] = ISNULL(@NUM_PAGINAS,[NUM_PAGINAS])
			WHERE  COD_GUID = @COD_GUID AND ID_MAESTRO_TITULO = @ID_MAESTRO_TITULO
		END
		ELSE
		BEGIN
			INSERT INTO [dbo].[MAESTRO_TITULOS_DOCUMENTOS]
				([ID_DOCUMENTO_TITULO]
				,[ID_MAESTRO_TITULO]
				,[DES_RUTA_DOCUMENTO]
				,[TIPO_RUTA]
				,[COD_GUID]
				,[COD_TIPO_DOCUMENTO_AO]
				,[NOM_DOC_AO]
				,[OBSERVA_LEGIBILIDAD]
				,[NUM_PAGINAS])
			VALUES
				(CASE
					WHEN @ID_DOCUMENTO_TITULO = 0 THEN NULL
					ELSE @ID_DOCUMENTO_TITULO
				END
				,@ID_MAESTRO_TITULO
				,@DES_RUTA_DOCUMENTO
				,@TIPO_RUTA
				,@COD_GUID
				,@COD_TIPO_DOCUMENTO_AO
				,@NOM_DOC_AO
				,@OBSERVA_LEGIBILIDAD,@NUM_PAGINAS)
		END
	END
 END
GO

UPDATE TAREA_ASIGNADA SET VAL_USUARIO_NOMBRE = '' WHERE COD_TIPO_OBJ = 4

-- Se eliminan datos de tablas foraneas
DELETE FROM DOCUMENTO_TITULO_TIPO_TITULO
GO
DELETE FROM HISTORICO_CLASIFICACION_MANUAL
GO
DELETE FROM MAESTRO_TITULOS_DOCUMENTOS
GO
DELETE FROM DOCUMENTO_TITULO  
GO


DBCC CHECKIDENT ('DOCUMENTO_TITULO', RESEED, 0) 
GO
INSERT INTO DOCUMENTO_TITULO (NOMBRE_DOCUMENTO) VALUES
('Resolución Liquidación Oficial'),
('Oficio de Notificación Liquidación Oficial'),
('Constancia de Notificación de Liquidación Oficial (guía correo y/o acta de notificación personal o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Resolución que Resuelve el Recurso de Reconsideración'),
('Oficio de notificación del Recurso'),
('Constancia de Notificación del Recurso (guía correo y/o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Constancia de Ejecutoria'),
('Archivo con la información desagregada de la deuda (SQL).'),
('Resolución Liquidación Oficial / Sanción'),
('Oficio de Notificación Resolución Liquidación Oficial / Sanción'),
('Constancia de Notificación (guía correo y/o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Constancia de Notificación del Recurso (guía correo y/o acta de notificación personal o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Requerimiento para declarar y/o corregir'),
('Oficio de Notificación'),
('Pliego de cargos'),
('Resolución Sanción'),
('Oficio de Notificación Resolución Sanción'),
('Constancia de Notificación de Resolución (guía correo y/o acta de notificación personal o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Copia de documento de identidad del Pensionado'),
('Proyecto de Resolución de Reconocimiento'),
('Oficio de Consulta de Proyecto de Resolución de Reconocimiento'),
('Constancia de Notificación del Proyecto (guía de entrega)'),
('Aceptación del proyecto'),
('Acto Administrativo de Reconocimiento'),
('Oficio de Notificación del Acto de Reconocimiento'),
('Constancia de Notificación del Acto que Reconoce (guía de entrega)'),
('Resolución que Resuelve el Recurso de Reposición'),
('Oficio de Notificación del Recurso de Reposición'),
('Constancia de Notificación del Recurso de Reposición (guía de entrega)'),
('Cuenta de Cobro'),
('Constancia de Notificación de la Cuenta de Cobro (guía de entrega)'),
('Certificado de Pago de la Mesada Pensional'),
('Liquidación de la cuota parte.'),
('Resolución que Determina Mayor Valor Pagado'),
('Oficio de Notificación de la Resolución'),
('Constancia de Notificación de la Resolución (guía de entrega)'),
('Resolución que Modifica Mayor Valor Pagado'),
('Oficio de Notificación de Resolución que Modifica'),
('Liquidación detallada del Mayor Valor Pagado'),
('Certificación Descuentos de Nómina'),
('Copia de documento de identidad del Deudor'),
('Copia de Sentencia Judicial de Primera Instancia'),
('Copia de Sentencia Judicial de Segunda Instancia'),
('Copia de Sentencia Judicial de Casación'),
('Auto Aclaratorio'),
('Sentencia'),
('Constancia de Ejecutoria de la Sentencia'),
('Auto que Liquida Costas'),
('Auto que Aprueba Costas'),
('Constancia de Ejecutoria Auto que Aprueba'),
('Resolución Sancionatoria')
GO

INSERT INTO [dbo].DOCUMENTO_TITULO_TIPO_TITULO
	(ID_DOCUMENTO_TITULO,COD_TIPO_TITULO,VAL_ESTADO,VAL_OBLIGATORIO)
VALUES
(1,'01', 1, 1),
(2,'01', 1, 1),
(3,'01', 1, 1),
(4,'01', 1, 0),
(5,'01', 1, 0),
(6,'01', 1, 1),
(7,'01', 1, 1),
(8,'01', 1, 1),
(9,'08', 1, 1),
(10,'08', 1, 1),
(11,'08', 1, 1),
(4,'08', 1, 0),
(5,'08', 1, 0),
(12,'08', 1, 0),
(7,'08', 1, 1),
(8,'08', 1, 1),
(13,'04', 1, 1),
(14,'04', 1, 1),
(11,'04', 1, 1),
(8,'04', 1, 1),
(15,'17', 1, 1),
(14,'17', 1, 1),
(11,'17', 1, 1),
(16,'07', 1, 1),
(17,'07', 1, 1),
(18,'07', 1, 1),
(4,'07', 1, 0),
(5,'07', 1, 0),
(6,'07', 1, 0),
(7,'07', 1, 1),
(16,'05', 1, 1),
(17,'05', 1, 1),
(18,'05', 1, 1),
(4,'05', 1, 0),
(5,'05', 1, 0),
(6,'05', 1, 0),
(7,'05', 1, 1),
(19,'03', 1, 1),
(20,'03', 1, 1),
(21,'03', 1, 1),
(22,'03', 1, 1),
(23,'03', 1, 0),
(24,'03', 1, 1),
(25,'03', 1, 0),
(26,'03', 1, 0),
(27,'03', 1, 0),
(28,'03', 1, 0),
(29,'03', 1, 0),
(30,'03', 1, 1),
(31,'03', 1, 1),
(32,'03', 1, 0),
(33,'03', 1, 0),
(34,'15', 1, 1),
(35,'15', 1, 0),
(36,'15', 1, 1),
(27,'15', 1, 0),
(28,'15', 1, 0),
(29,'15', 1, 0),
(37,'15', 1, 0),
(38,'15', 1, 0),
(39,'15', 1, 0),
(40,'15', 1, 0),
(41,'15', 1, 1),
(7,'15', 1, 1),
(42,'06', 1, 1),
(43,'06', 1, 0),
(44,'06', 1, 0),
(45,'06', 1, 0),
(7,'06', 1, 1),
(46,'16', 1, 1),
(47,'16', 1, 1),
(48,'16', 1, 1),
(49,'16', 1, 1),
(50,'16', 1, 1),
(41,'16', 1, 1),
(51,'10', 1, 1),
(35,'10', 1, 0),
(18,'10', 1, 1),
(27,'10', 1, 0),
(29,'10', 1, 0),
(6,'10', 1, 0),
(7,'10', 1, 1)
GO

-- Nuevos Usuarios
INSERT INTO dbo.USUARIOS (codigo,nombre,documento,clave,nivelacces,cobrador,apppredial,appvehic,appcuotasp,appindycom,login,useractivo,useremail,usercamclave,superior,ind_gestor_estudios,ind_gestor_expedientes) VALUES 
('0185','AIDA LUCERO ROJAS GARCIA','52013051','e10adc3949ba59abbe56e057f20f883e',11,'01','S','S','S','S','AROJASG',1,'AROJASG@UGPP.GOV.CO',0,'',NULL,NULL)
,('0186','ESPERANZA TRUJILLO RODRIGUEZ','52198914','e10adc3949ba59abbe56e057f20f883e',11,'01','S','S','S','S','ETRUJILLO',1,'ETRUJILLO@UGPP.GOV.CO',0,'',NULL,NULL)
,('0187','LUIS CARLOS CRUZ GAMEZ','80357359','e10adc3949ba59abbe56e057f20f883e',11,'01','S','S','S','S','LCRUZG',1,'LCRUZG@UGPP.GOV.CO',0,'',NULL,NULL)
,('0188','MARIA FERNANDA REY CAMACHO','39574166','e10adc3949ba59abbe56e057f20f883e',11,'01','S','S','S','S','MREY',1,'MREY@UGPP.GOV.CO',0,'',NULL,NULL)
;

UPDATE DOCUMENTO_TITULO_TIPO_TITULO SET VAL_OBLIGATORIO = 0 WHERE ID_DOCUMENTO_TITULO = 6 AND COD_TIPO_TITULO = '01'

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_GrillaAreaOrigen]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_GrillaAreaOrigen] 
END
GO

-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 13/11/2018
-- Description: CARGA LOS DATOS PARA LA GRILLA DE LA BANDEJA DE TITULOS AREA ORIGEN
--- EXEC [SP_GrillaAreaOrigen] 'DLEON','',0,0,'','','',''
-- =============================================

CREATE PROCEDURE [dbo].[SP_GrillaAreaOrigen] 

   @USULOG AS VARCHAR(20), -- Usuario al cual se le asigno la tarea
   @NROTITULO AS VARCHAR(50) , -- Número de título
   @ESTADOPROCESAL AS INT,
   @ESTADOSOPERATIVO AS INT,
   @FCHENVIOCOBRANZADESDE AS VARCHAR(10),
   @FCHENVIOCOBRANZAHASTA AS VARCHAR(10),
   @NROIDENTIFICACIONDEUDOR AS VARCHAR(50),
   @NOMBREDEUDOR AS VARCHAR (50)
   AS
   
BEGIN                     
      
SET NOCOUNT ON 

DECLARE @NUMERODIAS INT 
SELECT @NUMERODIAS = VAL_VALOR FROM DOMINIO_DETALLE WHERE ID_DOMINIO_DETALLE =11
IF @ESTADOSOPERATIVO = 1
	BEGIN 		
			   SELECT 
			   TA.ID_UNICO_TITULO AS IDUNICO,
			   TA.ID_TAREA_ASIGNADA,
			   MT.MT_nro_titulo AS NROTITULO,
				CONVERT(VARCHAR(10),MT.MT_fec_expedicion_titulo,103) AS FCHEXPEDICIONTITULO,
			   ED.ED_Nombre AS NOMBREDEUDOR,
			   ED.ED_Codigo_Nit AS NRONITCEDULA,
			   TT.nombre AS TIPOOBLIGACION,
			   (
					MT.MT_valor_obligacion + MT.MT_partida_global + MT.MT_sancion_omision + MT.MT_sancion_inexactitud + MT.MT_sancion_mora
			   ) AS TOTALOBLIGACION, --Se realiza la suma de los valores
			   CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) AS FEC_ENTREGA_GESTOR, 
			   CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  @NUMERODIAS ,103)  AS FCHLIMITE, 
			   CASE WHEN   CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  @NUMERODIAS ,103) < = CONVERT(VARCHAR(10),GETDATE()  ,103)THEN 'ROJO' 
					END
				AS COLOR,
			   EO.ID_ESTADO_OPERATIVOS,
			   AT.JSON_OBJ AS JSON_ALMACENAMIENTO
			   FROM TAREA_ASIGNADA TA
				LEFT JOIN [dbo].[ESTADO_OPERATIVO] EO WITH (NOLOCK) ON TA.COD_ESTADO_OPERATIVO = EO.ID_ESTADO_OPERATIVOS
				LEFT JOIN MAESTRO_TITULOS MT WITH (NOLOCK) ON MT.idunico = TA.ID_UNICO_TITULO
				LEFT JOIN [dbo].[TIPOS_TITULO] TT WITH (NOLOCK) ON TT.codigo = MT.MT_tipo_titulo
				LEFT JOIN [dbo].[DEUDORES_EXPEDIENTES] DE WITH (NOLOCK) ON MT.idunico = DE.ID_MAESTRO_TITULOS
				LEFT JOIN [dbo].[ENTES_DEUDORES] ED WITH (NOLOCK) ON DE.deudor = ED.ED_Codigo_Nit
				LEFT JOIN [dbo].[ALMACENAMIENTO_TEMPORAL] AT WITH (NOLOCK) ON AT.ID_TAREA_ASIGNADA = TA.ID_TAREA_ASIGNADA
				LEFT JOIN [dbo].[EJEFISGLOBAL] EJEF WITH (NOLOCK) ON EJEF.EFINROEXP = MT.MT_expediente
		
				WHERE 
				EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO AND
				((@USULOG IS NULL) OR (VAL_USUARIO_NOMBRE = @USULOG)) AND
				((@NROTITULO = '') OR (UPPER(MT_nro_titulo) LIKE '%' + UPPER(@NROTITULO) + '%')) AND
				((@ESTADOSOPERATIVO = 0) OR (EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO)) AND
				((@FCHENVIOCOBRANZADESDE = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) >= @FCHENVIOCOBRANZADESDE)) AND
				((@FCHENVIOCOBRANZAHASTA = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) <= @FCHENVIOCOBRANZAHASTA)) AND
				((@NROIDENTIFICACIONDEUDOR = '') OR (ED.ED_Codigo_Nit LIKE '%' + @NROIDENTIFICACIONDEUDOR+ '%')) AND
				((@NOMBREDEUDOR = '') OR (UPPER(ED.ED_Nombre) LIKE '%' + UPPER(@NOMBREDEUDOR) + '%'))
					ORDER BY TA.ID_TAREA_ASIGNADA DESC

	END 

ELSE
	BEGIN 		
			SELECT 
			   TA.ID_UNICO_TITULO AS IDUNICO,
			   TA.ID_TAREA_ASIGNADA,
			   MT.MT_nro_titulo AS NROTITULO,
				CONVERT(VARCHAR(10),MT.MT_fec_expedicion_titulo,103) AS FCHEXPEDICIONTITULO,
			   ED.ED_Nombre AS NOMBREDEUDOR,
			   ED.ED_Codigo_Nit AS NRONITCEDULA,
			   TT.nombre AS TIPOOBLIGACION,
			   (
					MT.MT_valor_obligacion + MT.MT_partida_global + MT.MT_sancion_omision + MT.MT_sancion_inexactitud + MT.MT_sancion_mora
				) AS TOTALOBLIGACION, --Se realiza la suma de los valores
			   CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) AS FEC_ENTREGA_GESTOR, 
			   CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  @NUMERODIAS ,103)  AS FCHLIMITE, 
			   CASE WHEN   CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  @NUMERODIAS ,103) < = CONVERT(VARCHAR(10),GETDATE()  ,103)THEN 'ROJO' 
					END
				AS COLOR,
			   EO.ID_ESTADO_OPERATIVOS,
			   AT.JSON_OBJ AS JSON_ALMACENAMIENTO
			FROM TAREA_ASIGNADA TA
				LEFT JOIN [dbo].[ESTADO_OPERATIVO] EO WITH (NOLOCK) ON TA.COD_ESTADO_OPERATIVO = EO.ID_ESTADO_OPERATIVOS
				LEFT JOIN MAESTRO_TITULOS MT WITH (NOLOCK) ON MT.idunico = TA.ID_UNICO_TITULO
				LEFT JOIN [dbo].[TIPOS_TITULO] TT WITH (NOLOCK) ON TT.codigo = MT.MT_tipo_titulo
				LEFT JOIN [dbo].[DEUDORES_EXPEDIENTES] DE WITH (NOLOCK) ON MT.idunico = DE.ID_MAESTRO_TITULOS
				LEFT JOIN [dbo].[ENTES_DEUDORES] ED WITH (NOLOCK) ON DE.deudor = ED.ED_Codigo_Nit
				LEFT JOIN [dbo].[ALMACENAMIENTO_TEMPORAL] AT WITH (NOLOCK) ON AT.ID_TAREA_ASIGNADA = TA.ID_TAREA_ASIGNADA
				LEFT JOIN [dbo].[EJEFISGLOBAL] EJEF WITH (NOLOCK) ON EJEF.EFINROEXP = MT.MT_expediente
		
				WHERE 

				((@USULOG IS NULL) OR (VAL_USUARIO_NOMBRE = @USULOG)) AND
				((@NROTITULO = '') OR (UPPER(MT_nro_titulo) LIKE '%' + UPPER(@NROTITULO) + '%')) AND
				((@ESTADOSOPERATIVO = 0) OR (EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO)) AND
				((@FCHENVIOCOBRANZADESDE = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) >= @FCHENVIOCOBRANZADESDE)) AND
				((@FCHENVIOCOBRANZAHASTA = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) <= @FCHENVIOCOBRANZAHASTA)) AND
				((@NROIDENTIFICACIONDEUDOR = '') OR (ED.ED_Codigo_Nit LIKE '%' + @NROIDENTIFICACIONDEUDOR+ '%')) AND
				((@NOMBREDEUDOR = '') OR (UPPER(ED.ED_Nombre) LIKE '%' + UPPER(@NOMBREDEUDOR) + '%')) AND
				 EO.VAL_NOMBRE IN ('EnCreacion','Subsanar')
				 ORDER BY TA.FEC_ENTREGA_GESTOR DESC

	END 
 END
 
GO

EXEC sp_rename 'TIPOS_PROCESOS_CONCURSALES.ID_TIPO_PROCESO', 'PROCESO_JURIDICO', 'COLUMN';

EXEC sp_rename 'TIPOS_PROCESOS_CONCURSALES.NUMERO_PROCESO', 'PROCESO_NATURAL', 'COLUMN';
GO
ALTER TABLE [dbo].[TIPOS_PROCESOS_CONCURSALES] 
ADD ESTADO_PROCESO_J VARCHAR(5) NULL

ALTER TABLE [dbo].[TIPOS_PROCESOS_CONCURSALES] 
ADD ESTADO_PROCESO_N VARCHAR(5) NULL
GO
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='0',PROCESO_NATURAL='0',ESTADO_PROCESO_J='0',ESTADO_PROCESO_N='0' WHERE CODIGO=1;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='0' WHERE CODIGO=2;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='0' WHERE CODIGO=3;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='02',ESTADO_PROCESO_N='0' WHERE CODIGO=4;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='0' WHERE CODIGO=5;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='1',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='03' WHERE CODIGO=6;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='0',PROCESO_NATURAL='1',ESTADO_PROCESO_J='0',ESTADO_PROCESO_N='03' WHERE CODIGO=7;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='0' WHERE CODIGO=8;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='1',PROCESO_NATURAL='0',ESTADO_PROCESO_J='03',ESTADO_PROCESO_N='0' WHERE CODIGO=9;
UPDATE [dbo].[TIPOS_PROCESOS_CONCURSALES] SET PROCESO_JURIDICO ='0',PROCESO_NATURAL='1',ESTADO_PROCESO_J='0',ESTADO_PROCESO_N='03' WHERE CODIGO=10;
GO
IF	ISNULL((SELECT u.IdUVT FROM UVT U where u.Anio=2019 ),0)	=0
BEGIN 
	INSERT INTO UVT (Activo,Anio,Valor) VALUES(1,2019,34270) 
	UPDATE UVT SET Activo=0 WHERE Anio<2019 AND Activo=1
END

UPDATE PERFILES SET ind_estado = 0 WHERE codigo = 9

--#Region Aumento de longitud a 25 el campo MT_nro_titulo de la tabla MAESTRO_TITULOS
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT FK_MAESTRO_TITULOS_EJEFISGLOBAL
GO
ALTER TABLE dbo.EJEFISGLOBAL SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT FK_MAESTRO_TITULOS_TIPOS_TITULO
GO
ALTER TABLE dbo.TIPOS_TITULO SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT FK_MAESTRO_TITULOS_PROCEDENCIA_TITULOS
GO
ALTER TABLE dbo.PROCEDENCIA_TITULOS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_capitalmulta
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisossalud
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_morasalud
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactossalud
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisospensiones
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_morapensiones
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactospensiones
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisosfondosolpen
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_morafondosolpen
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactosfondosolpen
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisosarl
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_moraarl
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactosarl
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisosicbf
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_moraicbf
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactosicbf
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisossena
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_morasena
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactossena
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_omisossubfamiliar
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_morasubfamiliar
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_inexactossubfamiliar
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_sentenciasjudiciales
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_cuotaspartesacum
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalmultas
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalomisos
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalmora
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalinexactos
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalsentencias
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totaldeuda
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF_MAESTRO_TITULOS_totalrepartidor
GO
ALTER TABLE dbo.MAESTRO_TITULOS
	DROP CONSTRAINT DF__MAESTRO_T__estad__4048A6D1
GO
CREATE TABLE dbo.Tmp_MAESTRO_TITULOS
	(
	MT_nro_titulo varchar(25) NOT NULL,
	MT_expediente varchar(12) NULL,
	MT_tipo_titulo char(2) NULL,
	MT_titulo_acumulado varchar(50) NULL,
	MT_fec_expedicion_titulo datetime NULL,
	MT_res_resuelve_reposicion varchar(50) NULL,
	MT_fec_expe_resolucion_reposicion datetime NULL,
	MT_reso_resu_apela_recon varchar(50) NULL,
	MT_fec_exp_reso_apela_recon datetime NULL,
	MT_fecha_ejecutoria datetime NULL,
	MT_fec_exi_liq datetime NULL,
	MT_fec_cad_presc datetime NULL,
	MT_proc_cobro_ini_prev bit NULL,
	MT_procedencia int NULL,
	capitalmulta float(53) NULL,
	omisossalud float(53) NULL,
	morasalud float(53) NULL,
	inexactossalud float(53) NULL,
	omisospensiones float(53) NULL,
	morapensiones float(53) NULL,
	inexactospensiones float(53) NULL,
	omisosfondosolpen float(53) NULL,
	morafondosolpen float(53) NULL,
	inexactosfondosolpen float(53) NULL,
	omisosarl float(53) NULL,
	moraarl float(53) NULL,
	inexactosarl float(53) NULL,
	omisosicbf float(53) NULL,
	moraicbf float(53) NULL,
	inexactosicbf float(53) NULL,
	omisossena float(53) NULL,
	morasena float(53) NULL,
	inexactossena float(53) NULL,
	omisossubfamiliar float(53) NULL,
	morasubfamiliar float(53) NULL,
	inexactossubfamiliar float(53) NULL,
	sentenciasjudiciales float(53) NULL,
	cuotaspartesacum float(53) NULL,
	totalmultas float(53) NULL,
	totalomisos float(53) NULL,
	totalmora float(53) NULL,
	totalinexactos float(53) NULL,
	totalsentencias float(53) NULL,
	totaldeuda float(53) NULL,
	NumMemoDev varchar(20) NULL,
	FecMemoDev datetime NULL,
	CausalDevol int NULL,
	ObsDevol varchar(MAX) NULL,
	totalrepartidor float(53) NULL,
	estado int NULL,
	idunico bigint NOT NULL IDENTITY (1, 1),
	MT_totalSancion float(53) NULL,
	MT_tiposentencia char(1) NULL,
	revocatoria varchar(1) NULL,
	nroResolRevoca int NULL,
	fechaRevoca date NULL,
	valorRevoca float(53) NULL,
	MT_valor_obligacion float(53) NULL,
	MT_partida_global float(53) NULL,
	MT_sancion_omision float(53) NULL,
	MT_sancion_mora float(53) NULL,
	MT_sancion_inexactitud float(53) NULL,
	MT_total_obligacion float(53) NULL,
	MT_total_partida_global float(53) NULL,
	Automatico bit NULL,
	NoExpedienteOrigen varchar(30) NULL,
	MT_fec_notificacion_titulo datetime NULL,
	MT_for_notificacion_titulo char(2) NULL,
	MT_fec_not_reso_resu_reposicion datetime NULL,
	MT_for_not_reso_resu_reposicion char(2) NULL,
	MT_fec_not_reso_apela_recon datetime NULL,
	MT_for_not_reso_apela_recon char(2) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_capitalmulta DEFAULT ((0)) FOR capitalmulta
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisossalud DEFAULT ((0)) FOR omisossalud
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_morasalud DEFAULT ((0)) FOR morasalud
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactossalud DEFAULT ((0)) FOR inexactossalud
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisospensiones DEFAULT ((0)) FOR omisospensiones
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_morapensiones DEFAULT ((0)) FOR morapensiones
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactospensiones DEFAULT ((0)) FOR inexactospensiones
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisosfondosolpen DEFAULT ((0)) FOR omisosfondosolpen
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_morafondosolpen DEFAULT ((0)) FOR morafondosolpen
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactosfondosolpen DEFAULT ((0)) FOR inexactosfondosolpen
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisosarl DEFAULT ((0)) FOR omisosarl
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_moraarl DEFAULT ((0)) FOR moraarl
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactosarl DEFAULT ((0)) FOR inexactosarl
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisosicbf DEFAULT ((0)) FOR omisosicbf
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_moraicbf DEFAULT ((0)) FOR moraicbf
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactosicbf DEFAULT ((0)) FOR inexactosicbf
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisossena DEFAULT ((0)) FOR omisossena
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_morasena DEFAULT ((0)) FOR morasena
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactossena DEFAULT ((0)) FOR inexactossena
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_omisossubfamiliar DEFAULT ((0)) FOR omisossubfamiliar
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_morasubfamiliar DEFAULT ((0)) FOR morasubfamiliar
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_inexactossubfamiliar DEFAULT ((0)) FOR inexactossubfamiliar
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_sentenciasjudiciales DEFAULT ((0)) FOR sentenciasjudiciales
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_cuotaspartesacum DEFAULT ((0)) FOR cuotaspartesacum
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalmultas DEFAULT ((0)) FOR totalmultas
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalomisos DEFAULT ((0)) FOR totalomisos
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalmora DEFAULT ((0)) FOR totalmora
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalinexactos DEFAULT ((0)) FOR totalinexactos
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalsentencias DEFAULT ((0)) FOR totalsentencias
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totaldeuda DEFAULT ((0)) FOR totaldeuda
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF_MAESTRO_TITULOS_totalrepartidor DEFAULT ((0)) FOR totalrepartidor
GO
ALTER TABLE dbo.Tmp_MAESTRO_TITULOS ADD CONSTRAINT
	DF__MAESTRO_T__estad__4048A6D1 DEFAULT ((1)) FOR estado
GO
SET IDENTITY_INSERT dbo.Tmp_MAESTRO_TITULOS ON
GO
IF EXISTS(SELECT * FROM dbo.MAESTRO_TITULOS)
	 EXEC('INSERT INTO dbo.Tmp_MAESTRO_TITULOS (MT_nro_titulo, MT_expediente, MT_tipo_titulo, MT_titulo_acumulado, MT_fec_expedicion_titulo, MT_res_resuelve_reposicion, MT_fec_expe_resolucion_reposicion, MT_reso_resu_apela_recon, MT_fec_exp_reso_apela_recon, MT_fecha_ejecutoria, MT_fec_exi_liq, MT_fec_cad_presc, MT_proc_cobro_ini_prev, MT_procedencia, capitalmulta, omisossalud, morasalud, inexactossalud, omisospensiones, morapensiones, inexactospensiones, omisosfondosolpen, morafondosolpen, inexactosfondosolpen, omisosarl, moraarl, inexactosarl, omisosicbf, moraicbf, inexactosicbf, omisossena, morasena, inexactossena, omisossubfamiliar, morasubfamiliar, inexactossubfamiliar, sentenciasjudiciales, cuotaspartesacum, totalmultas, totalomisos, totalmora, totalinexactos, totalsentencias, totaldeuda, NumMemoDev, FecMemoDev, CausalDevol, ObsDevol, totalrepartidor, estado, idunico, MT_totalSancion, MT_tiposentencia, revocatoria, nroResolRevoca, fechaRevoca, valorRevoca, MT_valor_obligacion, MT_partida_global, MT_sancion_omision, MT_sancion_mora, MT_sancion_inexactitud, MT_total_obligacion, MT_total_partida_global, Automatico, NoExpedienteOrigen, MT_fec_notificacion_titulo, MT_for_notificacion_titulo, MT_fec_not_reso_resu_reposicion, MT_for_not_reso_resu_reposicion, MT_fec_not_reso_apela_recon, MT_for_not_reso_apela_recon)
		SELECT MT_nro_titulo, MT_expediente, MT_tipo_titulo, MT_titulo_acumulado, MT_fec_expedicion_titulo, MT_res_resuelve_reposicion, MT_fec_expe_resolucion_reposicion, MT_reso_resu_apela_recon, MT_fec_exp_reso_apela_recon, MT_fecha_ejecutoria, MT_fec_exi_liq, MT_fec_cad_presc, MT_proc_cobro_ini_prev, MT_procedencia, capitalmulta, omisossalud, morasalud, inexactossalud, omisospensiones, morapensiones, inexactospensiones, omisosfondosolpen, morafondosolpen, inexactosfondosolpen, omisosarl, moraarl, inexactosarl, omisosicbf, moraicbf, inexactosicbf, omisossena, morasena, inexactossena, omisossubfamiliar, morasubfamiliar, inexactossubfamiliar, sentenciasjudiciales, cuotaspartesacum, totalmultas, totalomisos, totalmora, totalinexactos, totalsentencias, totaldeuda, NumMemoDev, FecMemoDev, CausalDevol, ObsDevol, totalrepartidor, estado, idunico, MT_totalSancion, MT_tiposentencia, revocatoria, nroResolRevoca, fechaRevoca, valorRevoca, MT_valor_obligacion, MT_partida_global, MT_sancion_omision, MT_sancion_mora, MT_sancion_inexactitud, MT_total_obligacion, MT_total_partida_global, Automatico, NoExpedienteOrigen, MT_fec_notificacion_titulo, MT_for_notificacion_titulo, MT_fec_not_reso_resu_reposicion, MT_for_not_reso_resu_reposicion, MT_fec_not_reso_apela_recon, MT_for_not_reso_apela_recon FROM dbo.MAESTRO_TITULOS WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_MAESTRO_TITULOS OFF
GO
ALTER TABLE dbo.MAESTRO_TITULOS_FOR_NOTIFICACION
	DROP CONSTRAINT FK_MAESTRO_TITULOS_FOR_NOTIFICACION_MAESTRO_TITULOS
GO
ALTER TABLE dbo.RESPUESTA_PREGUNTA_CLASIFICACION
	DROP CONSTRAINT FK_RESPUESTAS_PREGUNTAS_CLASIFICACION_MAESTRO_TITULOS
GO
ALTER TABLE dbo.MAESTRO_TITULOS_DOCUMENTOS
	DROP CONSTRAINT FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_MAESTRO_TITULO
GO
DROP TABLE dbo.MAESTRO_TITULOS
GO
EXECUTE sp_rename N'dbo.Tmp_MAESTRO_TITULOS', N'MAESTRO_TITULOS', 'OBJECT' 
GO
ALTER TABLE dbo.MAESTRO_TITULOS ADD CONSTRAINT
	PK_MAESTRO_TITULOS PRIMARY KEY CLUSTERED 
	(
	idunico
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.MAESTRO_TITULOS ADD CONSTRAINT
	FK_MAESTRO_TITULOS_PROCEDENCIA_TITULOS FOREIGN KEY
	(
	MT_procedencia
	) REFERENCES dbo.PROCEDENCIA_TITULOS
	(
	codigo
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MAESTRO_TITULOS ADD CONSTRAINT
	FK_MAESTRO_TITULOS_TIPOS_TITULO FOREIGN KEY
	(
	MT_tipo_titulo
	) REFERENCES dbo.TIPOS_TITULO
	(
	codigo
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MAESTRO_TITULOS ADD CONSTRAINT
	FK_MAESTRO_TITULOS_EJEFISGLOBAL FOREIGN KEY
	(
	MT_expediente
	) REFERENCES dbo.EJEFISGLOBAL
	(
	EFINROEXP
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS_DOCUMENTOS ADD CONSTRAINT
	FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_MAESTRO_TITULO FOREIGN KEY
	(
	ID_MAESTRO_TITULO
	) REFERENCES dbo.MAESTRO_TITULOS
	(
	idunico
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MAESTRO_TITULOS_DOCUMENTOS SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RESPUESTA_PREGUNTA_CLASIFICACION ADD CONSTRAINT
	FK_RESPUESTAS_PREGUNTAS_CLASIFICACION_MAESTRO_TITULOS FOREIGN KEY
	(
	ID_TITULO
	) REFERENCES dbo.MAESTRO_TITULOS
	(
	idunico
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RESPUESTA_PREGUNTA_CLASIFICACION SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.MAESTRO_TITULOS_FOR_NOTIFICACION ADD CONSTRAINT
	FK_MAESTRO_TITULOS_FOR_NOTIFICACION_MAESTRO_TITULOS FOREIGN KEY
	(
	ID_UNICO_MAESTRO_TITULOS
	) REFERENCES dbo.MAESTRO_TITULOS
	(
	idunico
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.MAESTRO_TITULOS_FOR_NOTIFICACION SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
--#endregion

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
   @MT_nro_titulo VARCHAR(25),   
   @MT_expediente VARCHAR(12) = NULL,
   @MT_tipo_titulo CHAR(2),
   @MT_fec_expedicion_titulo DATETIME,
   @MT_fec_notificacion_titulo DATETIME,
   @MT_for_notificacion_titulo CHAR(2),
   @MT_Tip_notificacion INT,
   @MT_res_resuelve_reposicion VARCHAR(50) = NULL,
   @MT_fec_expe_resolucion_reposicion DATETIME = NULL,
   @MT_reso_resu_apela_recon VARCHAR(50) = NULL,
   @MT_fec_exp_reso_apela_recon DATETIME = NULL,
   @MT_fecha_ejecutoria DATETIME = NULL,
   @MT_fec_exi_liq DATETIME = NULL,
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
           ,[MT_fec_cad_presc] =
		    CASE @MT_fecha_ejecutoria WHEN NULL THEN 
			GETDATE()+1
			ELSE 
			DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=@MT_tipo_titulo),@MT_fecha_ejecutoria)   		END
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
					   ,[MT_fec_cad_presc]
					   ,[NoExpedienteOrigen]
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
					@MT_sancion_inexactitud,  @MT_total_obligacion, @MT_total_partida_global ,  @Automatico,
		    (CASE @MT_fecha_ejecutoria WHEN NULL THEN 
			GETDATE()+1
			ELSE 
			DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=@MT_tipo_titulo),@MT_fecha_ejecutoria)   		END),
					@NoExpedienteOrigen)

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

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ASIGNAR_EXPEDIENTE_A_TITULO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_ASIGNAR_EXPEDIENTE_A_TITULO] 
END
GO

-- =============================================
-- Author:		Edward Fabian Hernandez Nieves - Stefanini
-- Create date: 2019-03-12
-- Description:	Asigna el número de expediente a un título
-- =============================================
CREATE PROCEDURE [dbo].[SP_ASIGNAR_EXPEDIENTE_A_TITULO]
	@MT_nro_titulo AS VARCHAR(25),
	@MT_expediente AS VARCHAR(12)
AS
BEGIN
	SET NOCOUNT ON;
    UPDATE MAESTRO_TITULOS SET MT_expediente = @MT_expediente WHERE idunico = @MT_nro_titulo
END
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[REPARTO_EXPEDIENTES]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[REPARTO_EXPEDIENTES]
END
GO
CREATE PROCEDURE [dbo].[REPARTO_EXPEDIENTES]
	-- Add the parameters for the stored procedure here

AS
BEGIN
-- EXEC [REPARTO_EXPEDIENTES2]
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @TABLA TABLE(
Numero_Registro int,
Codigo_Expediente varchar(MAX)
);

DECLARE @TABLA2 TABLE(
Numero_Registro int,
Codigo_Expediente varchar(MAX),
Estado varchar(MAX)
);


    -- Insert statements for procedure here

declare @i int=1;
declare @r int=1;
declare @Tota_Expedientes int;
declare @Total_User int;
declare @login varchar(max);
declare @expediente varchar(max);
declare @estado varchar(max);
declare @estadoBandera varchar(max)='';
declare @bandera int=0;
declare @bande int=0;
declare @validador int=0;

DECLARE @USERS TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX));

DECLARE @USERSDESC TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX));

DECLARE @USERSP TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX));


INSERT INTO @TABLA
SELECT Row_number() 
         OVER ( 
           ORDER BY T.EFINROEXP_EXPEDIENTE),
  T.EFINROEXP_EXPEDIENTE FROM TAREA_ASIGNADA T
 WHERE T.COD_ESTADO_OPERATIVO=10 OR T.COD_ESTADO_OPERATIVO=11 GROUP BY T.EFINROEXP_EXPEDIENTE; 
   
 INSERT INTO @TABLA2
 SELECT Row_number() 
         OVER ( 
           ORDER BY E.EFIESTADO), T.Codigo_Expediente,E.EFIESTADO FROM EJEFISGLOBAL E
 INNER JOIN @TABLA T
 ON T.Codigo_Expediente=E.EFINROEXP 
WHERE E.EFIESTADO IS NOT NULL OR E.EFIESTADO NOT IN('07','08','09')
ORDER BY E.EFIESTADO;

INSERT INTO @USERSP
SELECT Row_number() 
         OVER ( 
           ORDER BY E.COD_ID_ESTADOS_PROCESOS),E.VAL_USUARIO,
		   E.COD_ID_ESTADOS_PROCESOS FROM  ESTADOS_PROCESO_GESTOR E
INNER JOIN USUARIOS U
ON E.VAL_USUARIO=U.LOGIN
WHERE U.ind_gestor_expedientes = 1 
AND U.useractivo = 1 
ORDER BY E.COD_ID_ESTADOS_PROCESOS;

 SET @Tota_Expedientes=(SELECT count(*) FROM @TABLA2);

 WHILE  ( @r <= @Tota_Expedientes ) 
 BEGIN 
      SET @expediente=(SELECT Codigo_Expediente 
                   FROM    @TABLA2 
                   WHERE  Numero_Registro = @r); 
	 SET @estado =(SELECT Estado 
                   FROM    @TABLA2 
                   WHERE  Numero_Registro = @r);
	   
	   
	   IF @estado<>@estadoBandera 	   
	   BEGIN
	   DELETE FROM @USERS;
	   DELETE FROM @USERSDESC;
	   INSERT INTO @USERS SELECT  Row_number() 
         OVER ( 
           ORDER BY USUARIO),USUARIO,ESTADO FROM @USERSP WHERE ESTADO=@estado ORDER BY USUARIO;

	   INSERT INTO @USERSDESC SELECT  Row_number() 
         OVER ( 
           ORDER BY USUARIO),USUARIO,ESTADO FROM @USERSP WHERE ESTADO=@estado ORDER BY USUARIO DESC;
	   SET @Total_User=(SELECT COUNT(*) FROM @USERS);	
	   SET @i=1;
	   
	   END
	   
	   IF @bandera = 0 
        BEGIN 
            SET @login=(SELECT usuario 
                        FROM   @USERS 
                        WHERE  numero_fila = @i); 
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
	   SET @estadoBandera=@estado
	   SET @r=@r+1;		
	  
	  If (SELECT TOP(1) FEC_ENTREGA_GESTOR from tarea_asignada where  EFINROEXP_EXPEDIENTE = @expediente) IS NULL
		  BEGIN
			  UPDATE tarea_asignada 
				SET  VAL_USUARIO_NOMBRE = @login,
				  FEC_ENTREGA_GESTOR=GETDATE(),
				  COD_ESTADO_OPERATIVO=11
			  WHERE  EFINROEXP_EXPEDIENTE = @expediente; 
		  END 
	  ELSE
		  BEGIN
			  UPDATE tarea_asignada 
				SET  VAL_USUARIO_NOMBRE = @login,				 
				  COD_ESTADO_OPERATIVO=11
			  WHERE  EFINROEXP_EXPEDIENTE = @expediente; 
		  END 

 END
 END

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[REPARTO_AUTOMATICO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[REPARTO_AUTOMATICO]
END
GO

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
	 
	  IF (SELECT TOP(1) fec_entrega_gestor FROM tarea_asignada WHERE  id_unico_titulo = @titulo) is null
		  BEGIN      
		  UPDATE tarea_asignada 
		  SET    val_usuario_nombre = @LOGIN, 
				 fec_entrega_gestor = Getdate(), 
				 FEC_ACTUALIZACION = Getdate(),
				 cod_estado_operativo = 4 
		  WHERE  id_unico_titulo = @titulo; 
	  
		  END
	  ELSE
		  BEGIN 
			  UPDATE tarea_asignada 
			  SET    val_usuario_nombre = @LOGIN, 
					 FEC_ACTUALIZACION=Getdate(),
					 cod_estado_operativo = 4 
			  WHERE  id_unico_titulo = @titulo; 
		  END
      ---Cambio Estado Operativo Titulo 
      --UPDATE maestro_titulos 
      --SET    estado = 4 
      --WHERE  idunico = @titulo; 
	  print(@titulo)
  END 
 
END

GO

UPDATE DOCUMENTO_TITULO set NOMBRE_DOCUMENTO='Oficio de notificación del recurso' where ID_DOCUMENTO_TITULO=6 or  ID_DOCUMENTO_TITULO=12
UPDATE DOCUMENTO_TITULO set NOMBRE_DOCUMENTO='Oficio de notificación del recurso' where ID_DOCUMENTO_TITULO=6 or  ID_DOCUMENTO_TITULO=12

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ObtenerTitulosEstudioTitulos]')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN 
	DROP PROCEDURE [dbo].[SP_ObtenerTitulosEstudioTitulos] 
END
GO

-- =============================================
-- Author: Eduar Fabian Hernandez Nieves
-- Create date: 2018-11-14
-- Description: Carga los títulos para el festor de estudio de títulos
--- EXEC [SP_ObtenerTitulosEstudioTitulos] 'MFONG','',0,0,'','','',''
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerTitulosEstudioTitulos] 
   @USULOG AS VARCHAR(20), -- Usuario al cual se le asigno la tarea
   @NROTITULO AS VARCHAR(50) , -- Número de título
   @ESTADOPROCESAL AS INT,
   @ESTADOSOPERATIVO AS INT,
   @FCHENVIOCOBRANZADESDE AS VARCHAR(10),
   @FCHENVIOCOBRANZAHASTA AS VARCHAR(10),
   @NROIDENTIFICACIONDEUDOR AS VARCHAR(50),
   @NOMBREDEUDOR AS VARCHAR (50)
   AS
   
BEGIN                     
      
SET NOCOUNT ON

	SELECT
		MT.idunico AS IDUNICO,
		TA.ID_TAREA_ASIGNADA,
		MT.MT_nro_titulo AS NROTITULO,
		CONVERT(VARCHAR(10),MT.MT_fec_expedicion_titulo,103) AS FCHEXPEDICIONTITULO,
		ED.ED_Nombre AS NOMBREDEUDOR,
		ED.ED_Codigo_Nit AS NRONITCEDULA,
		TT.nombre AS TIPOOBLIGACION,
		(
			MT.MT_valor_obligacion + MT.MT_partida_global + MT.MT_sancion_omision + MT.MT_sancion_inexactitud + MT.MT_sancion_mora
		) AS TOTALOBLIGACION, --Se realiza la suma de los valores
		CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) AS FEC_ENTREGA_GESTOR, 
		CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  TT.DIAS_MAX_GESTION_ROJO,103)  AS FCHLIMITE, 
		CASE WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >=  TT.DIAS_MAX_GESTION_ROJO THEN 'ROJO' 
		WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >= TT.DIAS_MAX_GESTION_AMARILLO  AND  DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) <  TT.DIAS_MAX_GESTION_ROJO  THEN 'AMARILLO'END
		AS COLOR,
		EO.ID_ESTADO_OPERATIVOS,
		TA.VAL_PRIORIDAD  
	FROM 
		TAREA_ASIGNADA TA
		LEFT JOIN [dbo].[ESTADO_OPERATIVO] EO WITH (NOLOCK) ON TA.COD_ESTADO_OPERATIVO = EO.ID_ESTADO_OPERATIVOS
		LEFT JOIN MAESTRO_TITULOS MT WITH (NOLOCK) ON MT.idunico = TA.ID_UNICO_TITULO
		LEFT JOIN [dbo].[TIPOS_TITULO] TT WITH (NOLOCK) ON TT.codigo = MT.MT_tipo_titulo
		LEFT JOIN [dbo].[DEUDORES_EXPEDIENTES] DE WITH (NOLOCK) ON MT.idunico = DE.ID_MAESTRO_TITULOS
		LEFT JOIN [dbo].[ENTES_DEUDORES] ED WITH (NOLOCK) ON DE.deudor = ED.ED_Codigo_Nit
	WHERE 
		(MT.MT_expediente IS NULL OR (MT.MT_expediente IS NOT NULL AND TA.COD_ESTADO_OPERATIVO IN(5, 7, 9, 13)) AND MT.MT_expediente IN(SELECT EFINROEXP FROM EJEFISGLOBAL WHERE EFINROEXP = MT.MT_expediente AND EFIESTADO IS NULL)) AND
		TA.ID_UNICO_TITULO IS NOT NULL AND
		((@USULOG IS NULL) OR (VAL_USUARIO_NOMBRE = @USULOG)) AND
		((@NROTITULO = '') OR (MT.MT_nro_titulo LIKE '%' + @NROTITULO + '%')) AND
		--((@ESTADOPROCESAL IS NULL) OR (col3 = @ESTADOPROCESAL))
		((@ESTADOSOPERATIVO = 0) OR (EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO)) AND
		((@FCHENVIOCOBRANZADESDE = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) >= @FCHENVIOCOBRANZADESDE)) AND
		((@FCHENVIOCOBRANZAHASTA = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) <= @FCHENVIOCOBRANZAHASTA)) AND
		((@NROIDENTIFICACIONDEUDOR = '') OR (UPPER(ED.ED_Codigo_Nit) LIKE '%' + UPPER(@NROIDENTIFICACIONDEUDOR)  + '%')) AND
		((@NOMBREDEUDOR = '') OR (UPPER(ED.ED_Nombre) LIKE '%' + UPPER(@NOMBREDEUDOR) + '%'))
		AND (TA.COD_ESTADO_OPERATIVO IN(4, 5, 9, 7, 13))
	ORDER BY 
		TA.IND_TITULO_PRIORIZADO DESC, 
		TA.VAL_PRIORIDAD DESC, 
		MT.fechaRevoca ASC, 
		TA.FEC_ENTREGA_GESTOR ASC 
 END
GO

INSERT INTO [dbo].[TIPOS_TITULO]
           ([codigo]
           ,[nombre]
           ,[ID_TIPO_CARTERA]
           ,[COD_PROCEDENCIA]
           ,[DIAS_MAX_GESTION_AMARILLO]
           ,[DIAS_MAX_GESTION_ROJO]
           ,[ANOS_FECHA_PRESCRIPCION])
     VALUES
           ('20'
           ,'APORTE PATRONAL NACIONALES'
           ,2
           ,3
           ,22
           ,28
           ,3)

UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='01' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=0,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='02' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=3,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='03' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='04'
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='05' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=4,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='06' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='07' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='08' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=NULL, COD_PROCEDENCIA=NULL,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='09' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=3, COD_PROCEDENCIA=5,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='10' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='11' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='12' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='13' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='14' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='15' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='16' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=1, COD_PROCEDENCIA=2,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='17' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=4, COD_PROCEDENCIA=6,Anos_fecha_prescripcion=5,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='18' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=3,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='19' 
UPDATE [TIPOS_TITULO] SET ID_TIPO_CARTERA=2, COD_PROCEDENCIA=3,Anos_fecha_prescripcion=3,DIAS_MAX_GESTION_AMARILLO=22,DIAS_MAX_GESTION_ROJO=28 WHERE codigo='20'
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_TIPOS_TITULO_POR_AREA]
	@COD_PROCEDENCIA  AS INT
AS
	SELECT TT.codigo,TT.nombre,TT.COD_PROCEDENCIA,TT.ID_TIPO_CARTERA FROM TIPOS_TITULO TT 	
	WHERE TT.COD_PROCEDENCIA = @COD_PROCEDENCIA
	
	

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
-- EXEC SP_OBTENER_EXPEDIENTES_ASIGNADOS 1, 10, '', '', 'LUROJAS', '', '', '', '', '', '', '', 0, ''
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
	@ESTADO_OPERATIVO INT = 0, --Estado operativo del expediente
	@USUNOINCLUIR AS VARCHAR(20) = '' --Usuario que no se debe incluir en los resultados
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

	-- Se crea table temporal que muestra los expedientes que están en estudio de títulos y que no están en retono
	SELECT
		DISTINCT E.EFINROEXP AS NroExp
	INTO #ExpedientesRetornoEstudioTitulos
	FROM EJEFISGLOBAL AS E
		JOIN TAREA_ASIGNADA AS TA ON E.EFINROEXP = TA.EFINROEXP_EXPEDIENTE
	WHERE 
		((@USULOG = '') OR (TA.VAL_USUARIO_NOMBRE = @USULOG))
		AND E.EFIESTADO = 13
		AND TA.COD_ESTADO_OPERATIVO = 17

	--Se crea tabla temporal con las fechas de inicio de gestión según la fecha de cambio de estado procesal
	SELECT
		DISTINCT E.EFINROEXP AS NroExp
	   ,MAX(CE.fecha) AS FechaCambioUltimoEstado
	   ,MIN(HTA.FEC_ACTUALIZACION) AS FechaGestionDespuesCambioEstado 
	INTO #FechasInicioGestion
	FROM 
		EJEFISGLOBAL AS E
		JOIN CAMBIOS_ESTADO AS CE ON E.EFINROEXP = CE.NroExp
		JOIN TAREA_ASIGNADA AS TA ON E.EFINROEXP = TA.EFINROEXP_EXPEDIENTE
		JOIN HISTORICO_TAREA_ASIGNADA AS HTA ON TA.ID_TAREA_ASIGNADA = HTA.ID_TAREA_ASIGNADA
	WHERE 
		CE.estado = E.EFIESTADO
		-- Validaciones Obligatorias
		AND TA.EFINROEXP_EXPEDIENTE IS NOT NULL
		AND TA.VAL_USUARIO_NOMBRE IS NOT NULL
		AND TA.VAL_USUARIO_NOMBRE <> ''
		AND TA.COD_ESTADO_OPERATIVO IN (11, 12, 13, 14, 15, 17, 19)
	GROUP BY E.EFINROEXP
			,E.EFIESTADO
			,CE.estado
			,HTA.FEC_ACTUALIZACION
	HAVING HTA.FEC_ACTUALIZACION >= MAX(CE.fecha)
	ORDER BY E.EFINROEXP

	-- Se realizan los filtros dependiendo de los parámetros del SP y se guardan en una tabla temporal
	SELECT
		DISTINCT TAREA_ASIGNADA.ID_TAREA_ASIGNADA AS ID_TAREA_ASIGNADA
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
	   ,COLORSUSPENSION = CASE
			WHEN (CONVERT(INT, DATEDIFF(DAY, FEC_ACTUALIZACION, GETDATE())) >= CONVERT(INT, @MaxDiasSuspension) AND TAREA_ASIGNADA.COD_ESTADO_OPERATIVO = 14) 
				THEN 'Rojo'
			ELSE NULL
		END
	   ,COLORALERTA = ( 
			CASE
				WHEN(
					CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) >= CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_rojo) 
					AND
					TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 13, 14, 15, 17, 19)
				)THEN 'Rojo'
				WHEN(
					(
						CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) >= CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_amarillo) 
						AND
						CONVERT(INT, DATEDIFF(DAY, FIG.FechaGestionDespuesCambioEstado, GETDATE())) < CONVERT(INT, ESTADOS_PROCESO.max_dias_gestion_rojo)
					)
					AND TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 13, 14, 15, 17, 19)
				) THEN 'Amarillo'
				ELSE NULL
			END
		)	
	   ,FECHALIMITE = (
			SELECT MIN(CAST(MT_fec_exi_liq AS datetime)) 
			FROM MAESTRO_TITULOS 
			WHERE MT_expediente = CONVERT(VARCHAR, TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE)
	   ) --CAST(MT.MT_fec_exi_liq AS DATE) AS FECHALIMITE 
	INTO #ExpedientesAsignados
	FROM TAREA_ASIGNADA
		LEFT JOIN EJEFISGLOBAL ON TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE = EJEFISGLOBAL.EFINROEXP
		LEFT JOIN ESTADOS_PROCESO ON EJEFISGLOBAL.EFIESTADO = ESTADOS_PROCESO.codigo
		LEFT JOIN ESTADOS_PAGO ON EJEFISGLOBAL.EFIESTUP = ESTADOS_PAGO.codigo
		LEFT JOIN PERSUASIVO ON EJEFISGLOBAL.EFINROEXP = PERSUASIVO.NroExp
		LEFT JOIN ENTES_DEUDORES ON EJEFISGLOBAL.EFINIT = ENTES_DEUDORES.ED_Codigo_Nit
		LEFT JOIN USUARIOS ON EJEFISGLOBAL.EFIUSUASIG = USUARIOS.codigo
		LEFT JOIN TITULOSEJECUTIVOS ON EJEFISGLOBAL.EFINROEXP = TITULOSEJECUTIVOS.MT_expediente
		LEFT JOIN ESTADO_OPERATIVO ON TAREA_ASIGNADA.COD_ESTADO_OPERATIVO = ESTADO_OPERATIVO.ID_ESTADO_OPERATIVOS
		LEFT JOIN #FechasInicioGestion AS FIG ON TAREA_ASIGNADA.EFINROEXP_EXPEDIENTE = FIG.NroExp
		LEFT JOIN MAESTRO_TITULOS AS MT ON EJEFISGLOBAL.EFINROEXP = MT.MT_expediente
	WHERE
		-- Filtros
		((@USULOG = '') OR ([TAREA_ASIGNADA].[VAL_USUARIO_NOMBRE] = @USULOG))
		AND ((@USUNOINCLUIR = '') OR ([TAREA_ASIGNADA].[VAL_USUARIO_NOMBRE] <> @USUNOINCLUIR))
		AND ((@EFINROEXP = '') OR (CONVERT(VARCHAR, [TAREA_ASIGNADA].[EFINROEXP_EXPEDIENTE]) = @EFINROEXP))
		AND ((@ED_NOMBRE = '') OR ([ENTES_DEUDORES].[ED_Nombre] LIKE '%' + @ED_NOMBRE + '%'))
		AND ((@EFINIT = '') OR ([EJEFISGLOBAL].[EFINIT] LIKE '%' + @EFINIT + '%'))
		AND ((@ESTADOPROC = '') OR ([EJEFISGLOBAL].[EFIESTADO] = @ESTADOPROC))
		AND ((@MT_TIPO_TITULO = '') OR ([TITULOSEJECUTIVOS].[MT_tipo_titulo] = @MT_TIPO_TITULO))
		AND ((@ESTADO_OPERATIVO = 0) OR ([TAREA_ASIGNADA].[COD_ESTADO_OPERATIVO] = @ESTADO_OPERATIVO))
		AND ((
			(
				(@FECTITULO = '') OR (CONVERT(VARCHAR(10), [TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR], 103) = @FECTITULO))
				AND ((@FECENTGES = '') OR (CONVERT(VARCHAR(10), [TAREA_ASIGNADA].[FEC_ENTREGA_GESTOR], 103) = @FECENTGES))
			)--Se valida sobre la tabla tarea_asiganada los valores que se consultan
			OR
			(
				((@FECTITULO = '') OR (CONVERT(VARCHAR(10), [EJEFISGLOBAL].[EFIFECHAEXP], 103) = @FECTITULO))
				AND ((@FECENTGES = '') OR (CONVERT(VARCHAR(10), [EJEFISGLOBAL].[EFIFECENTGES], 103) = @FECENTGES))
			)--Se valida sobre la tabla ya existente
		)
		-- Condicionales obligatorias
		AND TAREA_ASIGNADA.COD_TIPO_OBJ = 5
		AND TAREA_ASIGNADA.COD_ESTADO_OPERATIVO IN (11, 12, 13, 14, 15, 17, 19)
		AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE IS NOT NULL
		AND TAREA_ASIGNADA.VAL_USUARIO_NOMBRE <> ''
		AND
		(
			(
				[EJEFISGLOBAL].EFIESTADO <> 13 -- No se incluye los expedientes de estucio de títulos
			)
			OR
			(
				[EJEFISGLOBAL].EFINROEXP IN( SELECT NroExp FROM #ExpedientesRetornoEstudioTitulos)
			)
		)
		
	-- Se cuentan los registros obtenidos
	DECLARE @count VARCHAR(MAX) = (SELECT COUNT(*) FROM #ExpedientesAsignados)
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
	DROP TABLE #ExpedientesRetornoEstudioTitulos
END
GO

IF (NOT EXISTS (SELECT * FROM [dbo].[ESTADOS_PROCESO] WHERE nombre = 'ESTUDIO DE TÍTULOS'))
BEGIN
	DECLARE @Codigo INT = (SELECT MAX(codigo) FROM ESTADOS_PROCESO)

	INSERT INTO [dbo].[ESTADOS_PROCESO]
			   ([codigo]
			   ,[nombre]
			   ,[termino]
			   ,[max_dias_gestion_amarillo]
			   ,[max_dias_gestion_rojo])
		 VALUES(
			(@Codigo + 1)
			,'ESTUDIO DE TÍTULOS'
			,15
			,18
			,30
		)
END
GO

IF (NOT EXISTS (SELECT * FROM [dbo].[DOMINIO] WHERE DESCRIPCION = 'Estado Procesal Estudio Títulos'))
BEGIN
	INSERT INTO DOMINIO(DESCRIPCION) VALUES('Estado Procesal Estudio Títulos')
	DECLARE @DominioId AS INT = @@IDENTITY
	INSERT INTO DOMINIO_DETALLE(ID_DOMINIO, VAL_NOMBRE, DESC_DESCRIPCION, VAL_VALOR)
	VALUES(
		@DominioId
		,'EstadoProcesalEstudioTitulos'
		,'Estado procesal para identificar que un expediente se devolvió a estudio de títulos'
		,(SELECT MAX(codigo) FROM ESTADOS_PROCESO)
	)
END
GO

INSERT INTO [dbo].DOCUMENTO_TITULO_TIPO_TITULO
	(ID_DOCUMENTO_TITULO,COD_TIPO_TITULO,VAL_ESTADO,VAL_OBLIGATORIO)
VALUES
(34,'14', 1, 1),
(35,'14', 1, 0),
(36,'14', 1, 1),
(27,'14', 1, 0),
(28,'14', 1, 0),
(29,'14', 1, 0),
(37,'14', 1, 0),
(38,'14', 1, 0),
(39,'14', 1, 0),
(40,'14', 1, 0),
(41,'14', 1, 1),
(34,'13', 1, 1),
(35,'13', 1, 0),
(36,'13', 1, 1),
(27,'13', 1, 0),
(28,'13', 1, 0),
(29,'13', 1, 0),
(37,'13', 1, 0),
(38,'13', 1, 0),
(39,'13', 1, 0),
(40,'13', 1, 0),
(41,'13', 1, 1),
(34,'12', 1, 1),
(35,'12', 1, 0),
(36,'12', 1, 1),
(27,'12', 1, 0),
(28,'12', 1, 0),
(29,'12', 1, 0),
(37,'12', 1, 0),
(38,'12', 1, 0),
(39,'12', 1, 0),
(40,'12', 1, 0),
(41,'12', 1, 1),
(34,'11', 1, 1),
(35,'11', 1, 0),
(36,'11', 1, 1),
(27,'11', 1, 0),
(28,'11', 1, 0),
(29,'11', 1, 0),
(37,'11', 1, 0),
(38,'11', 1, 0),
(39,'11', 1, 0),
(40,'11', 1, 0),
(41,'11', 1, 1),
(34,'19', 1, 1),
(35,'19', 1, 0),
(36,'19', 1, 1),
(27,'19', 1, 0),
(28,'19', 1, 0),
(29,'19', 1, 0),
(37,'19', 1, 0),
(38,'19', 1, 0),
(39,'19', 1, 0),
(40,'19', 1, 0),
(41,'19', 1, 1),
(34,'20', 1, 1),
(35,'20', 1, 0),
(36,'20', 1, 1),
(27,'20', 1, 0),
(28,'20', 1, 0),
(29,'20', 1, 0),
(37,'20', 1, 0),
(38,'20', 1, 0),
(39,'20', 1, 0),
(40,'20', 1, 0),
(41,'20', 1, 1),
(13,'02', 1, 1),
(14,'02', 1, 1),
(11,'02', 1, 1),
(8,'02', 1, 1)
INSERT [dbo].[TIPO_OBLIGACION_VALORES] ([ID_TIPO_OBLIGACION_VALORES], [VALOR_OBLIGACION], [PARTIDA_GLOBAL], [SANCION_OMISION], [SANCION_INEXACTITUD], [SANCION_MORA]) VALUES (N'19', 1, 0, 1, 1, 0)
INSERT [dbo].[TIPO_OBLIGACION_VALORES] ([ID_TIPO_OBLIGACION_VALORES], [VALOR_OBLIGACION], [PARTIDA_GLOBAL], [SANCION_OMISION], [SANCION_INEXACTITUD], [SANCION_MORA]) VALUES (N'20', 1, 0, 1, 1, 0)

ALTER TABLE [dbo].[OBSERVACIONES_CUMPLE_NOCUMPLE]
DROP COLUMN [ID_TIPIFICACION]



/****** Object:  StoredProcedure [dbo].[SP_InsertaTituloEjecutivo]    Script Date: 29/03/2019 17:21:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
ALTER PROCEDURE [dbo].[SP_InsertaTituloEjecutivo]    
   @MT_nro_titulo VARCHAR(25),   
   @MT_expediente VARCHAR(12) = NULL,
   @MT_tipo_titulo CHAR(2),
   @MT_fec_expedicion_titulo DATETIME,
   @MT_fec_notificacion_titulo DATETIME,
   @MT_for_notificacion_titulo CHAR(2),
   @MT_Tip_notificacion INT,
   @MT_res_resuelve_reposicion VARCHAR(50) = NULL,
   @MT_fec_expe_resolucion_reposicion DATETIME = NULL,
   @MT_reso_resu_apela_recon VARCHAR(50) = NULL,
   @MT_fec_exp_reso_apela_recon DATETIME = NULL,
   @MT_fecha_ejecutoria DATETIME = NULL,
   @MT_fec_exi_liq DATETIME = NULL,
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
           ,[MT_fec_cad_presc] =
		    CASE @MT_fecha_ejecutoria WHEN NULL THEN 
			GETDATE()+1
			ELSE 
			DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=@MT_tipo_titulo),@MT_fecha_ejecutoria)   		END
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
					   ,[MT_fec_cad_presc]
					   ,[NoExpedienteOrigen]
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
					@MT_sancion_inexactitud,  @MT_total_obligacion, @MT_total_partida_global ,  @Automatico,
		    (CASE @MT_fecha_ejecutoria WHEN NULL THEN 
			GETDATE()+1
			ELSE 
			DATEADD(YEAR,(SELECT top(1) ANOS_FECHA_PRESCRIPCION FROM TIPOS_TITULO WHERE codigo=@MT_tipo_titulo),@MT_fecha_ejecutoria)   		END),
					@NoExpedienteOrigen)

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
INSERT [dbo].[TIPO_OBLIGACION_VALORES] ([ID_TIPO_OBLIGACION_VALORES], [VALOR_OBLIGACION], [PARTIDA_GLOBAL], [SANCION_OMISION], [SANCION_INEXACTITUD], [SANCION_MORA]) VALUES (N'19', 1, 0, 1, 1, 0)
INSERT [dbo].[TIPO_OBLIGACION_VALORES] ([ID_TIPO_OBLIGACION_VALORES], [VALOR_OBLIGACION], [PARTIDA_GLOBAL], [SANCION_OMISION], [SANCION_INEXACTITUD], [SANCION_MORA]) VALUES (N'20', 1, 0, 1, 1, 0)
GO

ALTER PROCEDURE [dbo].[SP_ObtenDatosValores] 
      
AS                        
      
BEGIN                     
      
SET NOCOUNT ON       
	SELECT A.ID_TIPO_OBLIGACION_VALORES,
		   B.nombre AS NOMBRETIPO,
		   A.VALOR_OBLIGACION,
		   A.PARTIDA_GLOBAL,
		   A.SANCION_OMISION,
		   A.SANCION_INEXACTITUD,
		   A.SANCION_MORA
	FROM [TIPO_OBLIGACION_VALORES] A INNER JOIN [TIPOS_TITULO] B WITH(NOLOCK) ON A.ID_TIPO_OBLIGACION_VALORES = B.codigo
END 



ALTER TABLE [ESTADOS_PROCESO_GESTOR] 
ADD [ETAPA_PROCESAL] INT;
ALTER TABLE [ESTADOS_PROCESO_GESTOR] 
ADD [USUARIO_ACTUALIZACION] NVARCHAR(10);
ALTER TABLE [ESTADOS_PROCESO_GESTOR] 
ADD [FECHA_ACTUALIZACION] DATE;


INSERT INTO DOCUMENTO_TITULO (NOMBRE_DOCUMENTO) VALUES
('Soporte  Matricula Mercantil'),
('Contrato'),
('Otro sis del contrato'),
('Resolución mediante la cual se liquida unilateralmente el contrato')

DELETE DOCUMENTO_TITULO_TIPO_TITULO where COD_TIPO_TITULO='18'

INSERT INTO [dbo].DOCUMENTO_TITULO_TIPO_TITULO
	(ID_DOCUMENTO_TITULO,COD_TIPO_TITULO,VAL_ESTADO,VAL_OBLIGATORIO)
VALUES
(7,'18',1,1),
(18,'18',1,0),
(27,'18',1,0),
(28,'18',1,0),
(30,'18',1,0),
(35,'18',1,0),
(36,'18',1,0),
(53,'18',1,1),
(54,'18',1,0),
(55,'18',1,1)



SET IDENTITY_INSERT [dbo].[ETAPA_PROCESAL] ON 

INSERT [dbo].[ETAPA_PROCESAL] 
([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) 
VALUES 
(17,N'13','Estudio de Títulos'),
(18,N'02','Viabilidad'),
(19,N'02','Normalización'),
(20,N'03','Concursales'),
(21,N'05','Facilidad de Pago'),
(22,N'12','Dificil Cobro'),
(23,N'01','Cartera Incobrable'),
(24,N'08','Suspendido'),
(25,N'07','Terminado'),
(26,N'11','Incidente'),
(27,N'03','Sucesiones')


SET IDENTITY_INSERT [dbo].[ETAPA_PROCESAL] OFF

UPDATE [ETAPA_PROCESAL] set VAL_ETAPA_PROCESAL='Medidas Cautelares' where codigo='02' and ID_ETAPA_PROCESAL=5


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TrgHISTORICO_TAREA_ASIGNADA] ON [dbo].[TAREA_ASIGNADA]
	AFTER INSERT,UPDATE
AS
	BEGIN
	INSERT INTO [dbo].[HISTORICO_TAREA_ASIGNADA] (ID_TAREA_ASIGNADA,VAL_USUARIO_NOMBRE,	COD_TIPO_OBJ,ID_UNICO_TITULO,EFINROEXP_EXPEDIENTE,FEC_ACTUALIZACION,FEC_ENTREGA_GESTOR,VAL_PRIORIDAD,COD_ESTADO_OPERATIVO, IND_TITULO_PRIORIZADO, ID_TAREA_OBSERVACION)
				SELECT 
						i.ID_TAREA_ASIGNADA,
						i.VAL_USUARIO_NOMBRE,
						i.COD_TIPO_OBJ,
						i.ID_UNICO_TITULO,
						i.EFINROEXP_EXPEDIENTE,
						i.FEC_ACTUALIZACION,
						i.FEC_ENTREGA_GESTOR,
						i.VAL_PRIORIDAD,
						i.COD_ESTADO_OPERATIVO,
						i.IND_TITULO_PRIORIZADO,
						i.ID_TAREA_OBSERVACION
				FROM inserted i
	IF(UPDATE(VAL_USUARIO_NOMBRE))
	BEGIN 
		UPDATE
				E
			SET
				E.EFIUSUASIG = U.codigo
			FROM
				inserted  AS T
				INNER JOIN EJEFISGLOBAL AS E
					ON T.EFINROEXP_EXPEDIENTE = E.EFINROEXP AND COD_TIPO_OBJ=5
				INNER JOIN USUARIOS U ON T.VAL_USUARIO_NOMBRE= U.[login]
			WHERE T.VAL_USUARIO_NOMBRE<>''
		 END 
	END


SET IDENTITY_INSERT [dbo].[PERFILES] ON 
IF NOT EXISTS (SELECT * FROM dbo.PERFILES WHERE CODIGO IN (12))
BEGIN
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (12, N'COORDINADOR', N'COAC_U_COORDINADOR', 1)
END
SET IDENTITY_INSERT [dbo].[PERFILES] OFF


SET IDENTITY_INSERT [dbo].[MODULO] ON
GO 
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (55, N'Gestor Etapa', N'Security/Maestros/ESTADO_PROCESO_GESTOR.aspx', N'/Security/images/usuarios.png', 1)
SET IDENTITY_INSERT [dbo].[MODULO] OFF
GO

INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 47, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 48, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 50, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 51, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (12, 55, 1)
GO

(55,'18',1,1)

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[ASIGNACION_GESTORES_EXPEDIENTES]') AND type IN ( N'P', N'PC' ) ) 
BEGIN 
	DROP PROCEDURE [dbo].[ASIGNACION_GESTORES_EXPEDIENTES]  
END
GO

-- =============================================
-- Author:		<Luis Mario Lenis Ojeda>
-- Create date: <27-Nov-2018>
-- Description:	<Designacion de Usuarios Gestores>
-- =============================================
CREATE PROCEDURE [dbo].[ASIGNACION_GESTORES_EXPEDIENTES] 
	-- Add the parameters for the stored procedure here
	@USERS AS VARCHAR(200)
AS
BEGIN
 -- EXEC [dbo].[ASIGNACION_GESTORES_EXPEDIENTES] 
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	UPDATE USUARIOS  SET ind_gestor_expedientes=1 WHERE login=@USERS

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
-- EXEC SP_OBTENER_SOLICITUDES_CAMBIO_ESTADO 'ABLANCO', '81433', ''
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
		LEFT JOIN ESTADOS_PROCESO AS ESTADOS_PROCESO_SOLICITADO ON SCE.estado = ESTADOS_PROCESO_SOLICITADO.codigo
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
	ORDER BY aprob_revisor DESC, SCE.FECHA ASC
END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaDireccion]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaDireccion]  
END

GO
-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 31/10/2018
-- Description: INSERTA LA DIRECCION
-- =============================================
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

SET IDENTITY_INSERT [dbo].[MODULO] ON 
GO
INSERT [dbo].[MODULO] ([pk_codigo], [val_nombre], [val_url], [val_url_icono], [ind_estado]) VALUES (56, N'Todos los Expedientes', N'Security/Maestros/EJEFISGLOBAL.aspx?filtro=1', N'/Security/images/icons/dossier.png', 1)
GO
SET IDENTITY_INSERT [dbo].[MODULO] OFF
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (3, 56, 1)
GO
INSERT [dbo].[PERFIL_MODULO] ([fk_perfil_id], [fk_modulo_id], [ind_estado]) VALUES (2, 56, 1)
GO
UPDATE [dbo].[PERFIL_MODULO] SET [ind_estado] = 0 WHERE [fk_perfil_id] = 2 AND [fk_modulo_id] = 47

UPDATE PERFIL_MODULO SET fk_modulo_id = 27 WHERE fk_modulo_id = 47 AND fk_perfil_id = 8
UPDATE PERFIL_MODULO SET fk_modulo_id = 56 WHERE fk_modulo_id = 47 AND fk_perfil_id = 2


  
/****** Object:  StoredProcedure [dbo].[REPARTO_EXPEDIENTES]    Script Date: 04/04/2019 17:24:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

												
									
							 
												   
												
ALTER PROCEDURE [dbo].[REPARTO_EXPEDIENTES]
	-- Add the parameters for the stored procedure here

AS
BEGIN
-- EXEC [REPARTO_EXPEDIENTES2]
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @TABLA TABLE(
Numero_Registro int,
Codigo_Expediente varchar(MAX)
);

DECLARE @TABLA2 TABLE(
Numero_Registro int,
Codigo_Expediente varchar(MAX),
Estado varchar(MAX),
Etapa int null
);


    -- Insert statements for procedure here

declare @i int=1;
declare @r int=1;
declare @Tota_Expedientes int;
declare @Total_User int;
declare @login varchar(max);
declare @expediente varchar(max);
declare @estado varchar(max);
declare @estadoBandera varchar(max)='';
declare @etapa int=null;
declare @bandera int=0;
declare @bande int=0;
declare @validador int=0;

DECLARE @USERS TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX),
ETAPA int null);

DECLARE @USERSDESC TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX),
ETAPA int null);

DECLARE @USERSP TABLE(
NUMERO_FILA INT,
USUARIO VARCHAR(MAX),
ESTADO VARCHAR(MAX),
ETAPA int null);


INSERT INTO @TABLA
SELECT Row_number() 
         OVER ( 
           ORDER BY T.EFINROEXP_EXPEDIENTE),
  T.EFINROEXP_EXPEDIENTE FROM TAREA_ASIGNADA T
 WHERE T.COD_ESTADO_OPERATIVO=10 OR T.COD_ESTADO_OPERATIVO=11 GROUP BY T.EFINROEXP_EXPEDIENTE; 
   
 INSERT INTO @TABLA2
 SELECT Row_number() 
         OVER ( 
           ORDER BY E.EFIESTADO), T.Codigo_Expediente,E.EFIESTADO,E.EFIETAPAPROCESAL FROM EJEFISGLOBAL E
 INNER JOIN @TABLA T
 ON T.Codigo_Expediente=E.EFINROEXP 
WHERE (E.EFIESTADO is not null OR E.EFIESTADO NOT IN('07','08','09'))
AND e.EFIETAPAPROCESAL is not null
AND (select TOP(1)g.VAL_USUARIO from ESTADOS_PROCESO_GESTOR g where g.ETAPA_PROCESAL= E.EFIETAPAPROCESAL and g.COD_ID_ESTADOS_PROCESOS=e.EFIESTADO) is not null
ORDER BY E.EFIESTADO;


INSERT INTO @USERSP
SELECT Row_number() 
         OVER ( 
           ORDER BY E.COD_ID_ESTADOS_PROCESOS),E.VAL_USUARIO,
		   E.COD_ID_ESTADOS_PROCESOS,E.ETAPA_PROCESAL FROM  ESTADOS_PROCESO_GESTOR E
INNER JOIN USUARIOS U
ON E.VAL_USUARIO=U.LOGIN
WHERE U.ind_gestor_expedientes = 1 
AND U.useractivo = 1 
ORDER BY E.COD_ID_ESTADOS_PROCESOS;

 SET @Tota_Expedientes=(SELECT count(*) FROM @TABLA2);

 WHILE  ( @r <= @Tota_Expedientes ) 
 BEGIN 
      SET @expediente=(SELECT Codigo_Expediente 
                   FROM    @TABLA2 
                   WHERE  Numero_Registro = @r); 
	 SET @estado =(SELECT Estado 
                   FROM    @TABLA2 
                   WHERE  Numero_Registro = @r);
	  SET @etapa =(SELECT Etapa 
                   FROM    @TABLA2 
                   WHERE  Numero_Registro = @r);
	   
	
	   IF @estado<>@estadoBandera 	   
	   BEGIN
	   DELETE FROM @USERS;
	   DELETE FROM @USERSDESC;	  
											
				
																													  

	   INSERT INTO @USERS 
	   SELECT  Row_number() 
         OVER 
		 (ORDER BY USUARIO),USUARIO,ESTADO,ETAPA 
		   FROM @USERSP 
		   WHERE ESTADO=@estado AND ETAPA=@etapa ORDER BY USUARIO;


	   INSERT INTO @USERSDESC 
	   SELECT  Row_number() 
         OVER 
		 (ORDER BY USUARIO DESC),USUARIO,ESTADO,ETAPA
		   FROM @USERSP
		    WHERE ESTADO=@estado AND ETAPA=@etapa ORDER BY USUARIO DESC;

	   SET @Total_User=(SELECT COUNT(*) FROM @USERS);	
	   SET @i=1;
	   
	   END
	   
	   IF @bandera = 0 
        BEGIN 
		
            SET @login=(SELECT usuario 
                        FROM   @USERS 
                        WHERE  numero_fila = @i); 
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
	   SET @estadoBandera=@estado
	   SET @r=@r+1;		
	  
	  If (SELECT TOP(1) FEC_ENTREGA_GESTOR from tarea_asignada where  EFINROEXP_EXPEDIENTE = @expediente) IS NULL
		  BEGIN
			  UPDATE tarea_asignada 
				SET  VAL_USUARIO_NOMBRE = @login,
				  FEC_ENTREGA_GESTOR=GETDATE(),
				  COD_ESTADO_OPERATIVO=11,
				  FEC_ACTUALIZACION=GETDATE()
			  WHERE  EFINROEXP_EXPEDIENTE = @expediente; 
		  END 
	  ELSE
		  BEGIN
			  UPDATE tarea_asignada 
				SET  VAL_USUARIO_NOMBRE = @login,				 
				  COD_ESTADO_OPERATIVO=11,
				  FEC_ACTUALIZACION=GETDATE()
			  WHERE  EFINROEXP_EXPEDIENTE = @expediente; 
		  END 

 END
 END




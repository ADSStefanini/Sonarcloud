

GO
ALTER TABLE [dbo].[EJEFISGLOBAL] ADD 
[EFIETAPAPROCESAL] [int] NULL
GO
ALTER TABLE [dbo].[HISTORICO_TAREA_ASIGNADA] ADD 
[IND_TITULO_PRIORIZADO] [bit] NULL,
[ID_TAREA_OBSERVACION] [int] NULL
GO
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'DOCUMENTO_TITULO_TIPO_TITULO'))
BEGIN
CREATE TABLE [dbo].[DOCUMENTO_TITULO_TIPO_TITULO]
(
	[ID_DOCUMENTO_TITULO] [int] NULL,
	[COD_TIPO_TITULO] [char] (2) COLLATE Modern_Spanish_CI_AS NULL,
	[VAL_ESTADO] [bit] NULL,
	[VAL_OBLIGATORIO] [bit] NULL
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[DOCUMENTO_TITULO_TIPO_TITULO] ADD CONSTRAINT [FK_COD_TIPO_DOCUMENTO] FOREIGN KEY
	(
		[ID_DOCUMENTO_TITULO]
	)
	REFERENCES [dbo].[DOCUMENTO_TITULO]
	(
		[ID_DOCUMENTO_TITULO]
	)
GO

ALTER TABLE [dbo].[MAESTRO_TITULOS_DOCUMENTOS] ADD CONSTRAINT [FK_ID_DOCUMENTO_TITULO_MAESTRO_TITULOS_ID_DOCUMENTO_TITULO] FOREIGN KEY
	(
		[ID_DOCUMENTO_TITULO]
	)
	REFERENCES [dbo].[DOCUMENTO_TITULO]
	(
		[ID_DOCUMENTO_TITULO]
	)
GO

GO
ALTER TABLE dbo.DOCUMENTO_TITULO ADD
ID_TIPO_DOCUMENTAL int NULL
GO

CREATE TABLE [dbo].[TAREA_OBSERVACION](
	[COD_ID_TAREA_OBSERVACION] [int] IDENTITY(1,1) NOT NULL,
	[OBSERVACION] [nvarchar](250) NOT NULL,
	[IND_ESTADO] [bit] NOT NULL,
	[FEC_CREACION] [datetime] NOT NULL,
 CONSTRAINT [PK_TAREA_OBSERVACION] PRIMARY KEY CLUSTERED 
(
	[COD_ID_TAREA_OBSERVACION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE dbo.TAREA_ASIGNADA ADD
	ID_TAREA_OBSERVACION int NULL
GO
ALTER TABLE dbo.TAREA_ASIGNADA ADD CONSTRAINT
	FK_TAREA_ASIGNADA_TAREA_OBSERVACION FOREIGN KEY
	(
	ID_TAREA_OBSERVACION
	) REFERENCES dbo.TAREA_OBSERVACION
	(
	COD_ID_TAREA_OBSERVACION
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.TAREA_SOLICITUD ADD
	ID_TAREA_OBSERVACION int NULL
GO
ALTER TABLE dbo.TAREA_SOLICITUD ADD CONSTRAINT
	FK_TAREA_SOLICITUD_TAREA_OBSERVACION FOREIGN KEY
	(
	ID_TAREA_OBSERVACION
	) REFERENCES dbo.TAREA_OBSERVACION
	(
	COD_ID_TAREA_OBSERVACION
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO] 
END


GO
-- =============================================
-- Author:		Oscar Diaz
-- Create date: 2018-03-12
-- Description:	Ingresa/actualiza un registro en la relación
-- =============================================
CREATE PROCEDURE [dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO] 
@ID_DOCUMENTO_TITULO AS INT,
@COD_TIPO_TITULO AS VARCHAR(2),
@VAL_ESTADO AS BIT,
@VAL_OBLIGATORIO AS BIT
AS
BEGIN
SET NOCOUNT ON;

	IF EXISTS (SELECT TOP 1
		1
	FROM DOCUMENTO_TITULO_TIPO_TITULO
	WHERE ID_DOCUMENTO_TITULO = @ID_DOCUMENTO_TITULO
	AND COD_TIPO_TITULO = @COD_TIPO_TITULO)
BEGIN

UPDATE [dbo].[DOCUMENTO_TITULO_TIPO_TITULO]
SET [VAL_ESTADO] = @VAL_ESTADO
   ,[VAL_OBLIGATORIO] = @VAL_OBLIGATORIO
WHERE ID_DOCUMENTO_TITULO = @ID_DOCUMENTO_TITULO
AND COD_TIPO_TITULO = @COD_TIPO_TITULO
END
ELSE
INSERT INTO [dbo].[DOCUMENTO_TITULO_TIPO_TITULO] ([ID_DOCUMENTO_TITULO]
, [COD_TIPO_TITULO]
, [VAL_ESTADO]
, [VAL_OBLIGATORIO])
	VALUES (@ID_DOCUMENTO_TITULO, @COD_TIPO_TITULO, @VAL_ESTADO, @VAL_OBLIGATORIO)


END
GO


GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_GrillaAreaOrigen]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_GrillaAreaOrigen]
END


GO


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
			   MT.totaldeuda AS TOTALOBLIGACION,
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
					EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO
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
			   MT.totaldeuda AS TOTALOBLIGACION,
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
-- Create date: <26-NOV-2018>
-- Description:	<Reparto Automatico de Expedientes>
-- =============================================
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
	
      ---Cambio Estado Operativo Titulo 
      UPDATE maestro_titulos 
      SET    estado = 4 
      WHERE  idunico = @titulo; 
  END 
 
END

 GO
SET IDENTITY_INSERT [dbo].[PERFILES] ON 

UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_A_SUPERADMIN' , ind_estado = 1 WHERE [codigo] = 1
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_SUPERVISOR' , ind_estado = 1 WHERE [codigo] = 2
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_REVISOR' , ind_estado = 1 WHERE [codigo] = 3
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_GESTOR' , ind_estado = 1 WHERE [codigo] = 4
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_REPARTIDOR' , ind_estado = 1 WHERE [codigo] = 5
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_VERIFICADOROPAGOS' , ind_estado = 1 WHERE [codigo] = 6
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_CREADOROUSUARIOS' , ind_estado = 1 WHERE [codigo] = 7
UPDATE [dbo].[PERFILES] SET [val_ldap_group] = N'COAC_U_GESTOROINFORMACION' , ind_estado = 1 WHERE [codigo] = 8
IF NOT EXISTS (SELECT * FROM dbo.PERFILES WHERE CODIGO IN (9,10,11))
BEGIN
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (9, N'GESTOR COBROS PERSUASIVO', N'COAC_U', 1)
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (10, N'ESTUDIO DE TÍTULOS', N'COAC_U_ESTUDIOTITULOS', 1)
INSERT [dbo].[PERFILES] ([codigo], [nombre], [val_ldap_group], [ind_estado]) VALUES (11, N'AREA ORIGEN', N'COAC_U_CARGUETITULOS', 1)
END
SET IDENTITY_INSERT [dbo].[PERFILES] OFF
GO


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
--- EXEC [SP_GrillaAreaOrigen] 'MFONG','',0,0,'','','',''
--  EXEC [SP_GrillaAreaOrigen] 'MFONG','',0,0,'18/11/2018','21/11/2018','',''
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
	
			   SELECT 
			   TA.ID_UNICO_TITULO AS IDUNICO,
			   TA.ID_TAREA_ASIGNADA,
			   MT.MT_nro_titulo AS NROTITULO,
				CONVERT(VARCHAR(10),MT.MT_fec_expedicion_titulo,103) AS FCHEXPEDICIONTITULO,
			   ED.ED_Nombre AS NOMBREDEUDOR,
			   ED.ED_Codigo_Nit AS NRONITCEDULA,
			   TT.nombre AS TIPOOBLIGACION,
			   MT.totaldeuda AS TOTALOBLIGACION,
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
				 EO.VAL_NOMBRE IN ('Subsanar')
				 ORDER BY TA.FEC_ENTREGA_GESTOR DESC

 END

 GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_TAREA_SOLICITUD_INGRESAR]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_TAREA_SOLICITUD_INGRESAR]
END


GO
CREATE PROCEDURE [dbo].[SP_TAREA_SOLICITUD_INGRESAR]
	@ID_TAREA_ASIGNADA AS bigint,
	@VAL_USUARIO_SOLICITANTE AS varchar(50),
	@VAL_USUARIO_APROBADOR AS varchar(50),
	@VAL_USUARIO_DESTINO AS varchar(50)=null,
	@VAL_TIPO_SOLICITUD int,
	@COD_SOLICITUD_CAMBIO_ESTADO AS bigint=null,
	@VAL_TIPOLOGIA AS varchar(50)=null,
	@ID_TAREA_OBSERVACION AS INT=null
AS
	 INSERT INTO [dbo].[TAREA_SOLICITUD]
           ([ID_TAREA_ASIGNADA]
           ,[VAL_USUARIO_SOLICITANTE]
           ,[VAL_USUARIO_APROBADOR]
           ,[VAL_USUARIO_DESTINO]
           ,[VAL_TIPO_SOLICITUD]
           ,[COD_SOLICITUD_CAMBIO_ESTADO]
           ,[VAL_TIPOLOGIA]
           ,[ID_TAREA_OBSERVACION])
     VALUES
	 (@ID_TAREA_ASIGNADA,@VAL_USUARIO_SOLICITANTE,@VAL_USUARIO_APROBADOR,@VAL_USUARIO_DESTINO,@VAL_TIPO_SOLICITUD,@COD_SOLICITUD_CAMBIO_ESTADO,@VAL_TIPOLOGIA,@ID_TAREA_OBSERVACION)
	
	SELECT * FROM TAREA_SOLICITUD WHERE ID_TAREA_ASIGNADA= SCOPE_IDENTITY()


GO
	IF NOT EXISTS( SELECT * FROM DBO.MODULO WHERE val_nombre='Gestión Documento')
	BEGIN
		INSERT INTO DBO.MODULO (val_nombre, val_url, val_url_icono, ind_estado)
			VALUES ('Gestión Documento', '/Security/Maestros/GESTION_DOCUMENTO.aspx', '/Security/images/icons/Document.png', 1)

		  DECLARE @ID AS INT
		SET @ID = @@IDENTITY
		INSERT INTO dbo.PERFIL_MODULO (fk_perfil_id, fk_modulo_id, ind_estado)
			VALUES (1, @ID, 1)
	END
GO



IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO]
END
GO
-- =============================================
-- Author:		Oscar Diaz
-- Create date: 2018-03-12
-- Description:	Ingresa/actualiza un registro en la relación
-- =============================================
CREATE PROCEDURE [dbo].[SP_INGRESAR_DOCUMENTOTITULO_TIPOTITULO] 
@ID_DOCUMENTO_TITULO AS INT,
@COD_TIPO_TITULO AS VARCHAR(2),
@VAL_ESTADO AS BIT,
@VAL_OBLIGATORIO AS BIT
AS
BEGIN
SET NOCOUNT ON;

	IF EXISTS (SELECT TOP 1
		1
	FROM DOCUMENTO_TITULO_TIPO_TITULO
	WHERE ID_DOCUMENTO_TITULO = @ID_DOCUMENTO_TITULO
	AND COD_TIPO_TITULO = @COD_TIPO_TITULO)
BEGIN

UPDATE [dbo].[DOCUMENTO_TITULO_TIPO_TITULO]
SET [VAL_ESTADO] = @VAL_ESTADO
   ,[VAL_OBLIGATORIO] = @VAL_OBLIGATORIO
WHERE ID_DOCUMENTO_TITULO = @ID_DOCUMENTO_TITULO
AND COD_TIPO_TITULO = @COD_TIPO_TITULO
END
ELSE
INSERT INTO [dbo].[DOCUMENTO_TITULO_TIPO_TITULO] ([ID_DOCUMENTO_TITULO]
, [COD_TIPO_TITULO]
, [VAL_ESTADO]
, [VAL_OBLIGATORIO])
	VALUES (@ID_DOCUMENTO_TITULO, @COD_TIPO_TITULO, @VAL_ESTADO, @VAL_OBLIGATORIO)


END
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[SP_DOCUMENTOS_TITULO_TIPO_TITULO]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[SP_DOCUMENTOS_TITULO_TIPO_TITULO]
END
GO
-- =============================================
-- Author:		OSCAR ORLANDO DIAZ
-- Create date: 2018-12-03
-- Description:	Obtiene todos los documentos titulo tipo
-- =============================================
CREATE PROCEDURE [dbo].[SP_DOCUMENTOS_TITULO_TIPO_TITULO]

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
SELECT
	D.ID_DOCUMENTO_TITULO
   ,DT.NOMBRE_DOCUMENTO AS NOMBRE_DOCUMENTO_TITULO
   ,D.COD_TIPO_TITULO
   ,T.nombre AS NOMBRE_TIPO_TITULO
   ,D.VAL_ESTADO
   ,D.VAL_OBLIGATORIO
FROM DOCUMENTO_TITULO_TIPO_TITULO D
INNER JOIN DOCUMENTO_TITULO DT
	ON DT.ID_DOCUMENTO_TITULO = D.ID_DOCUMENTO_TITULO
INNER JOIN TIPOS_TITULO T
	ON T.codigo = D.COD_TIPO_TITULO
END
GO


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'RELACION_ESTADO_ETAPA'))
BEGIN
CREATE TABLE [dbo].[RELACION_ESTADO_ETAPA]
(
	[codigo_estado] [varchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[codigo_etapa] [int] NOT NULL,
	CONSTRAINT [PK_RELACION_ESTADO_ETAPA] PRIMARY KEY CLUSTERED
	(
		[codigo_etapa] ASC,
		[codigo_estado] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'HISTORICO_CLASIFICACION_MANUAL'))
BEGIN 
DROP TABLE [dbo].[HISTORICO_CLASIFICACION_MANUAL]
END
BEGIN
CREATE TABLE [dbo].[HISTORICO_CLASIFICACION_MANUAL]
(
	[ID_REGISTRO_CLASIFICACION_MANUAL] [int] IDENTITY (1,1) NOT NULL,
	[ID_EXPEDIENTE] [varchar] (12) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[ID_USUARIO] [varchar] (4) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[FECHA] [datetime] NOT NULL,
	[PERSONA_JURIDICA] [bit] NULL,
	[PERSONA_NATURAL] [bit] NULL,
	[PERSONA_VIVA] [bit] NULL,
	[MATRICULA_MERCANTIL] [bit] NULL,
	[ID_MTD_DOCUMENTO] [bigint] NULL,
	[PROCESO_ESPECIAL] [bit] NULL,
	[TIPO_PROCESO] [int] NULL,
	[BENEFICIO_TRIBUTARIO] [bit] NULL,
	[PAGOS_DEUDOR] [bit] NULL,
	[NUMERO_RADICADO] [int] NULL,
	[OBSERVACIONES] [varchar] (1000) COLLATE Modern_Spanish_CI_AS NULL,
	[VALOR_MENOR_UVT] [bit] NULL,
	CONSTRAINT [PK_dbo.HISTORICO_CLASIFICACION_MANUAL] PRIMARY KEY CLUSTERED
	(
		[ID_REGISTRO_CLASIFICACION_MANUAL] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	CONSTRAINT [FK_dbo.HISTORICO_CLASIFICACION_MANUAL_EJEFISGLOBAL] FOREIGN KEY
	(
		[ID_EXPEDIENTE]
	)
	REFERENCES [dbo].[EJEFISGLOBAL]
	(
		[EFINROEXP]
	),
	CONSTRAINT [FK_dbo.HISTORICO_CLASIFICACION_MANUAL_MAESTRO_TITULOS_DOCUMENTOS] FOREIGN KEY
	(
		[ID_MTD_DOCUMENTO]
	)
	REFERENCES [dbo].[MAESTRO_TITULOS_DOCUMENTOS]
	(
		[ID_MAESTRO_TITULOS_DOCUMENTOS]
	),
	CONSTRAINT [FK_dbo.HISTORICO_CLASIFICACION_MANUAL_TIPOS_PROCESOS_CONCURSALES] FOREIGN KEY
	(
		[TIPO_PROCESO]
	)
	REFERENCES [dbo].[TIPOS_PROCESOS_CONCURSALES]
	(
		[codigo]
	),
	CONSTRAINT [FK_dbo.HISTORICO_CLASIFICACION_MANUAL_USUARIOS] FOREIGN KEY
	(
		[ID_USUARIO]
	)
	REFERENCES [dbo].[USUARIOS]
	(
		[codigo]
	)
) ON [PRIMARY]
END

GO

UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='EnCreacion',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Puede estar todo titulo que se guardo parcialmente en el momento de cargue, Aun no se registra ' WHERE [ID_ESTADO_OPERATIVOS] =1
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='PorRepartir',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='El título se encuentra pendiente de reparto' WHERE [ID_ESTADO_OPERATIVOS] =2
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='EnSolicitud',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Se solicita un reasignación o priorización' WHERE [ID_ESTADO_OPERATIVOS] =3
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Asignado',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='En bandeja sin abrir / posible reparto ' WHERE [ID_ESTADO_OPERATIVOS] =4
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Engestión',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Abre el titulo o fue priorizado, o un retorno(Puede editar)' WHERE [ID_ESTADO_OPERATIVOS] =5
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Aceptado',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Se termina la gestión del titulo y se convierte en un expediente' WHERE [ID_ESTADO_OPERATIVOS] =6
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Stand By',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Puede estar todo titulo que se pauso por medio de una suspensión ' WHERE [ID_ESTADO_OPERATIVOS] =7
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Subsanar',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='En este estado están los títulos devueltos por estudio de títulos' WHERE [ID_ESTADO_OPERATIVOS] =8
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Retorno',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Titulo que tiene la respuesta de una devolución' WHERE [ID_ESTADO_OPERATIVOS] =9
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='PorRepartir',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='El Expediente se encuentra expediente de reparto' WHERE [ID_ESTADO_OPERATIVOS] =10
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Asignado',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='En bandeja sin abrir / posible reparto si no se abre' WHERE [ID_ESTADO_OPERATIVOS] =11
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Retorno ',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Expediente que tiene la respuesta de una devolución' WHERE [ID_ESTADO_OPERATIVOS] =12
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='EnSolicitud',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Se solicita un cambio de estado, reasignación o priorización' WHERE [ID_ESTADO_OPERATIVOS] =13
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Stand By',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='En este estado puede estar el expediente que se pauso por medio de una solicitud ' WHERE [ID_ESTADO_OPERATIVOS] =14
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='EnGestión',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Abre el expediente ' WHERE [ID_ESTADO_OPERATIVOS] =15
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Terminado',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Se termina la gestión del expediente' WHERE [ID_ESTADO_OPERATIVOS] =16
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Retorno ',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Expediente que tiene la respuesta de una devolución' WHERE [ID_ESTADO_OPERATIVOS] =17
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='CierreAdministrativo',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Cuando un expediente se retorna hasta estudio de títulos y no se debe continuar con su gestión' WHERE [ID_ESTADO_OPERATIVOS] =18
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 5,[VAL_NOMBRE]='Subsanar',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='En este estado están los expediente devueltos por estudio de títulos' WHERE [ID_ESTADO_OPERATIVOS] =19
UPDATE [ESTADO_OPERATIVO]  SET  [COD_TIPO_OBJ]= 4,[VAL_NOMBRE]='Terminado',[IND_ESTADO]=1,[DEC_ESTADO_OPERATIVO]='Se termina la gestion del titulo' WHERE [ID_ESTADO_OPERATIVOS] =20
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[OBSERVACIONES_CUMPLE_NOCUMPLE]')
                    AND type IN ( N'P', N'PC','U' ) ) 

BEGIN 
	DROP TABLE [dbo].[OBSERVACIONES_CUMPLE_NOCUMPLE]
END
GO

CREATE TABLE [dbo].[OBSERVACIONES_CUMPLE_NOCUMPLE](
	[ID_OBSERVACIONES] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_UNICO_MT] [bigint] NOT NULL,
	[USUARIO] [varchar](100) NOT NULL,
    [FCHOBSERVACIONES] DATETIME NOT NULL,
	[OBSERVACIONES] [varchar](500) NOT NULL,
	[DESTINATARIO] [varchar](100) NOT NULL,
	[CUMPLE_NOCUMPLE] [BIT] NOT NULL
)

ALTER TABLE [OBSERVACIONES_CUMPLE_NOCUMPLE] 
ADD CONSTRAINT [PK_ID_OBSERVACIONES]
PRIMARY KEY CLUSTERED([ID_OBSERVACIONES] ASC) 

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[OBSERVACIONESDOC_CUMPLE_NOCUMPLE]')
                    AND type IN ( N'P', N'PC','U' ) ) 

BEGIN 
	DROP TABLE [dbo].[OBSERVACIONESDOC_CUMPLE_NOCUMPLE]
END

GO

CREATE TABLE [dbo].[OBSERVACIONESDOC_CUMPLE_NOCUMPLE](
	[ID_OBSERVACIONESDOC] [bigint] IDENTITY(1,1) NOT NULL,
	[ID_UNICO_MT] [bigint] NOT NULL,
	[ID_DOCUMENTO] [bigint] NOT NULL,
	[USUARIO] [varchar](100) NOT NULL,
	[DESTINATARIO] [varchar](100) NOT NULL,
    [FCHENVIO] DATETIME NOT NULL,
	[OBSERVACIONES] [varchar](500) NOT NULL,
	[CUMPLE_NOCUMPLE] [BIT] NOT NULL
) ON [PRIMARY]

ALTER TABLE [OBSERVACIONESDOC_CUMPLE_NOCUMPLE] 
ADD CONSTRAINT [PK_ID_OBSERVACIONESDOC]
PRIMARY KEY CLUSTERED([ID_OBSERVACIONESDOC] ASC) 
GO

ALTER TABLE [TIPIFICACION_CNC] 
ADD CONSTRAINT [PK_ID_TIPIFICACIONCNC]
PRIMARY KEY CLUSTERED([ID_TIPIFICACIONCNC] ASC) 

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaObservacionCNC]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaObservacionCNC] 
END


GO


-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 21/11/2018
-- Description: INSERTA LA OBSERVACION DE CUMPLE NO CUMPLE 
-- =============================================



CREATE PROCEDURE [dbo].[SP_InsertaObservacionCNC] 

		   @ID_UNICO_MT BIGINT,
		   @USUARIO VARCHAR(100),
		   @OBSERVACIONES VARCHAR(500),
		   @CUMPLE_NOCUMPLE BIT,
		   @DESTINATARIO VARCHAR(100)
      AS
   
BEGIN                     
      
SET NOCOUNT ON   

INSERT INTO [dbo].[OBSERVACIONES_CUMPLE_NOCUMPLE]
           ([ID_UNICO_MT]
           ,[USUARIO]
           ,[FCHOBSERVACIONES]
           ,[OBSERVACIONES]
           ,[CUMPLE_NOCUMPLE]
		   ,[DESTINATARIO])
     VALUES
        (@ID_UNICO_MT,@USUARIO,GETDATE(),@OBSERVACIONES,@CUMPLE_NOCUMPLE,@DESTINATARIO)
END


GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaObservacionCNCDoc]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaObservacionCNCDoc] 
END
GO

-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 21/11/2018
-- Description: INSERTA LA OBSERVACION DE CUMPLE NO CUMPLE DOCUMENTOS
-- =============================================


CREATE PROCEDURE [dbo].[SP_InsertaObservacionCNCDoc] 

   @ID_UNICO_MT BIGINT,
   @ID_DOCUMENTO BIGINT,
   @USUARIO VARCHAR(100),
   @DESTINATARIO VARCHAR(100),
   @OBSERVACIONES VARCHAR(500),
   @CUMPLENOCUMPLE BIT

      AS
   
BEGIN                     
      
SET NOCOUNT ON   

INSERT INTO [dbo].[OBSERVACIONESDOC_CUMPLE_NOCUMPLE]
           ([ID_UNICO_MT]
           ,[ID_DOCUMENTO]
           ,[USUARIO]
           ,[DESTINATARIO]
           ,[FCHENVIO]
		   ,[CUMPLE_NOCUMPLE]
           ,[OBSERVACIONES])
     VALUES
        (@ID_UNICO_MT,@ID_DOCUMENTO,@USUARIO,@DESTINATARIO,GETDATE(),@CUMPLENOCUMPLE,@OBSERVACIONES)
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
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 0 WHERE VAL_USUARIO_NOMBRE  = @USULOG
	-- Actualiza la prioridad de las tareas que tienen prioridad
	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE VAL_USUARIO_NOMBRE  = @USULOG AND (COD_ESTADO_OPERATIVO IN(5, 7, 9) OR IND_TITULO_PRIORIZADO = 1)

	UPDATE TAREA_ASIGNADA SET VAL_PRIORIDAD = 1 WHERE ID_TAREA_ASIGNADA IN(
		SELECT TOP 1 TA.ID_TAREA_ASIGNADA 
		FROM TAREA_ASIGNADA TA
		JOIN MAESTRO_TITULOS MT ON TA.ID_UNICO_TITULO = MT.idunico
		WHERE 
			(TA.IND_TITULO_PRIORIZADO IS NULL OR TA.IND_TITULO_PRIORIZADO = 0) AND
			TA.VAL_USUARIO_NOMBRE = @USULOG AND
			TA.COD_ESTADO_OPERATIVO IN (4, 9, 7)
		ORDER BY
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
		   MT.totaldeuda AS TOTALOBLIGACION,
		   CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) AS FEC_ENTREGA_GESTOR, 
		   CONVERT(VARCHAR(10),FEC_ENTREGA_GESTOR +  TT.DIAS_MAX_GESTION_ROJO,103)  AS FCHLIMITE, 
		   CASE WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >=  TT.DIAS_MAX_GESTION_ROJO THEN 'ROJO' 
		        WHEN DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) >= TT.DIAS_MAX_GESTION_AMARILLO  AND  DATEDIFF(DAY ,FEC_ENTREGA_GESTOR  ,GETDATE()) <  TT.DIAS_MAX_GESTION_ROJO  THEN 'AMARILLO'END
		    AS COLOR,
		   EO.ID_ESTADO_OPERATIVOS,
		   TA.VAL_PRIORIDAD

		   FROM TAREA_ASIGNADA TA
			LEFT JOIN [dbo].[ESTADO_OPERATIVO] EO WITH (NOLOCK) ON TA.COD_ESTADO_OPERATIVO = EO.ID_ESTADO_OPERATIVOS
			LEFT JOIN MAESTRO_TITULOS MT WITH (NOLOCK) ON MT.idunico = TA.ID_UNICO_TITULO
			LEFT JOIN [dbo].[TIPOS_TITULO] TT WITH (NOLOCK) ON TT.codigo = MT.MT_tipo_titulo
			LEFT JOIN [dbo].[DEUDORES_EXPEDIENTES] DE WITH (NOLOCK) ON MT.idunico = DE.ID_MAESTRO_TITULOS
			LEFT JOIN [dbo].[ENTES_DEUDORES] ED WITH (NOLOCK) ON DE.deudor = ED.ED_Codigo_Nit
			
			WHERE 
			(MT.MT_expediente IS NULL OR (MT.MT_expediente IS NOT NULL AND TA.COD_ESTADO_OPERATIVO IN(5, 7, 9)) AND MT.MT_expediente IN(SELECT EFINROEXP FROM EJEFISGLOBAL WHERE EFINROEXP = MT.MT_expediente AND EFIESTADO IS NULL)) AND
			TA.ID_UNICO_TITULO IS NOT NULL AND
			((@USULOG IS NULL) OR (VAL_USUARIO_NOMBRE = @USULOG)) AND
			((@NROTITULO = '') OR (MT.MT_nro_titulo = @NROTITULO)) AND
			--((@ESTADOPROCESAL IS NULL) OR (col3 = @ESTADOPROCESAL))
			((@ESTADOSOPERATIVO = 0) OR (EO.ID_ESTADO_OPERATIVOS = @ESTADOSOPERATIVO)) AND
			((@FCHENVIOCOBRANZADESDE = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) >= @FCHENVIOCOBRANZADESDE)) AND
			((@FCHENVIOCOBRANZAHASTA = '') OR (CONVERT(VARCHAR(10),TA.FEC_ENTREGA_GESTOR,103) <= @FCHENVIOCOBRANZAHASTA)) AND
			((@NROIDENTIFICACIONDEUDOR = '') OR (UPPER(ED.ED_Codigo_Nit) LIKE '%' + UPPER(@NROIDENTIFICACIONDEUDOR)  + '%')) AND
			((@NOMBREDEUDOR = '') OR (UPPER(ED.ED_Nombre) LIKE '%' + UPPER(@NOMBREDEUDOR) + '%'))
			AND (TA.COD_ESTADO_OPERATIVO IN(4, 5, 9, 7))
			ORDER BY TA.VAL_PRIORIDAD DESC, MT.fechaRevoca ASC, TA.FEC_ENTREGA_GESTOR ASC 
 END
GO



IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaSolicitudCE]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaSolicitudCE] 
END


GO
-- =============================================
-- Author: Luis Mario Lenis Ojeda
-- Create date: 30/11/2018
-- Description: INSERTA LA SOLICITUD DE CAMBIO DE ESTADO
-- =============================================


CREATE PROCEDURE [dbo].[SP_InsertaSolicitudCE] 

   @ID_TAREA_ASIGNADA BIGINT,
   @VAL_USUARIO_SOLICITANTE VARCHAR(50),
   @VAL_USUARIO_APROBADOR VARCHAR(50),
   @VAL_USUARIO_DESTINO VARCHAR(50),
   @VAL_TIPO_SOLICITUD int,
   @COD_SOLICITUD_CAMBIO_ESTADO BIGINT,
   @VAL_TIPOLOGIA VARCHAR(50)

      AS
   
BEGIN                     
      
SET NOCOUNT ON   


INSERT INTO [dbo].[TAREA_SOLICITUD]
           ([ID_TAREA_ASIGNADA]
           ,[VAL_USUARIO_SOLICITANTE]
           ,[VAL_USUARIO_APROBADOR]
           ,[VAL_USUARIO_DESTINO]
           ,[VAL_TIPO_SOLICITUD]
		   ,[COD_SOLICITUD_CAMBIO_ESTADO]
		   ,[VAL_TIPOLOGIA])

     VALUES
        ( @ID_TAREA_ASIGNADA,@VAL_USUARIO_SOLICITANTE,@VAL_USUARIO_APROBADOR,@VAL_USUARIO_DESTINO,@VAL_TIPO_SOLICITUD, @COD_SOLICITUD_CAMBIO_ESTADO,@VAL_TIPOLOGIA)
END
GO

ALTER TABLE [dbo].[CAMBIOS_ESTADO] ADD estadooperativo int null;
ALTER TABLE [dbo].[CAMBIOS_ESTADO] ADD etapaprocesal int null;
ALTER TABLE [dbo].[SOLICITUDES_CAMBIOESTADO] ADD efietapaprocesal INT

ALTER TABLE [dbo].[CAMBIOS_ESTADO]  WITH CHECK ADD  CONSTRAINT [FK_CAMBIOS_ESTADO_ETAPA_PROCESAL] FOREIGN KEY([etapaprocesal])
REFERENCES [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL]);

ALTER TABLE [dbo].[CAMBIOS_ESTADO]  WITH CHECK ADD  CONSTRAINT [FK_CAMBIOS_ESTADO_ESTADO_OPERATIVO] FOREIGN KEY([estadooperativo])
REFERENCES [dbo].[ESTADO_OPERATIVO] ([ID_ESTADO_OPERATIVOS]);

GO

ALTER TABLE dbo.DOCUMENTO_TITULO ADD
	ID_TIPO_DOCUMENTAL int NULL
GO


SET IDENTITY_INSERT [dbo].[ETAPA_PROCESAL] ON 

INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (1, N'06', N'Beneficio Tributario')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (2, N'06', N'Compromiso de Pago')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (3, N'06', N'DSIAC')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (4, N'06', N'SCOB')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (5, N'02', N'Embargos')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (6, N'02', N'Mandamiento de Pago')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (7, N'02', N'Control Juridico')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (8, N'02', N'Orden de Ejecucion')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (9, N'02', N'Liquidación del Credito')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (10, N'02', N'Secuestro')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (11, N'02', N'Avaluos')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (12, N'02', N'Remate')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (13, N'11', N'Normalizacion')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (14, N'11', N'Validacion')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (15, N'11', N'Beneficio Tributario')
INSERT [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL], [codigo], [VAL_ETAPA_PROCESAL]) VALUES (16, N'03', N'Beneficio Tributario')
SET IDENTITY_INSERT [dbo].[ETAPA_PROCESAL] OFF

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
	@ID_FUENTE INT

AS   
BEGIN                         
	SET NOCOUNT ON   
	IF @idunico <> null
		BEGIN
			UPDATE  [dbo].[DIRECCIONES] 
			SET 
	  			 [deudor] =@deudor
				,[Direccion] = @Direccion
				,[Departamento] =@Departamento
				,[Ciudad] = @Ciudad
				,[Telefono] = @Telefono
				,[Email] = @Email
				,[Movil] = @Movil
				,[paginaweb] = NULL
				,[ID_FUENTE] = @ID_FUENTE

			WHERE  idunico=@idunico

		END
	ELSE
		BEGIN
		INSERT INTO [dbo].[DIRECCIONES]
				   ([deudor]
				   ,[Direccion]
				   ,[Departamento]
				   ,[Ciudad]
				   ,[Telefono]
				   ,[Email]
				   ,[Movil]
				   ,[paginaweb]
				   ,[ID_FUENTE])
			 VALUES
			 (@deudor,@Direccion, @Departamento,@Ciudad,@Telefono,@Email,@Movil,NULL,@ID_FUENTE)
		END
END
GO

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
   @NoExpedienteOrigen BIGINT=NULL,
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
					@MT_sancion_inexactitud,  @MT_total_obligacion, @MT_total_partida_global ,  @Automatico,@NoExpedienteOrigen)

			SET @IdunicoTituloR = SCOPE_IDENTITY()

	END

  SELECT @IdunicoTituloR AS IdunicoTitulo

 END

GO


GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ObtenerPaginasPorPerfil]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_ObtenerPaginasPorPerfil]  
END


GO
-- =============================================
-- Author:		Stefanini - Edward Hernandez
-- Create date: 2018-10-23
-- Description:	Obtener las lista de páginas activas asociadas a un perfil
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerPaginasPorPerfil]
	@ID_PERFIL INT,
	@ID_PARENTPAGE INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #ids ( id INT NOT NULL);

	IF @ID_PARENTPAGE = 0
			INSERT INTO #ids (id)
			SELECT pk_codigo FROM pagina
			WHERE fk_padre IS NULL AND ind_pagina_interna = 0
	ELSE
			INSERT INTO #ids (id)
			SELECT pk_codigo FROM pagina
			WHERE fk_padre = @ID_PARENTPAGE AND ind_pagina_interna = 0

    SELECT 
		p.pk_codigo
		,p.val_nombre
		,p.val_url 
		,p.fk_padre
		,CAST(CASE p.ind_estado WHEN 1 THEN 1 ELSE 0 END AS INT) AS ind_estado
	FROM 
		PAGINA p
	JOIN 
		PERFIL_PAGINA pp ON p.pk_codigo = pp.fk_pagina_id
	WHERE 
		p.ind_estado = 1
		AND pp.fk_perfil_id = @ID_PERFIL
		and pp.ind_puede_ver = 1
		AND p.pk_codigo IN(
			SELECT id FROM #ids
		)
		AND p.ind_pagina_interna = 0

	DROP TABLE #ids
END
GO
-- =============================================
-- Author:		Stefanini - Luis Mario Lenis Ojeda
-- Create date: 2018-12-11
-- Description:	Insertar Data tabla [MODULO Y PERFIL_MODULO]
-- =============================================
	IF NOT EXISTS( SELECT * FROM DBO.MODULO WHERE val_nombre='Gestión estado procesal/etapa procesal')
	BEGIN
		INSERT INTO DBO.MODULO (val_nombre, val_url, val_url_icono, ind_estado)
		VALUES ('Gestión estado procesal/etapa procesal', '/Security/Maestros/AdminEstadoProcesal_EtapaProcesal.aspx', '/Security/images/icons/Bloc_NotesSZ.png', 1)
		  DECLARE @ID AS INT
			SET @ID = @@IDENTITY
			INSERT INTO dbo.PERFIL_MODULO (fk_perfil_id, fk_modulo_id, ind_estado)
				VALUES (1, @ID, 1)
	END
	GO


-- =============================================
-- Author:		Stefanini - Oscar Gonzalez
-- Create date: 2018-12-13
-- Description:	Se modifica campo a varchar 
-- ============================================

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'VALOR_MENOR_UVT'
          AND Object_ID = Object_ID(N'dbo.[HISTORICO_CLASIFICACION_MANUAL]'))
BEGIN
ALTER TABLE [dbo].[HISTORICO_CLASIFICACION_MANUAL]
ADD VALOR_MENOR_UVT BIT NULL;
END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[ASIGNACION_GESTORES_EXPEDIENTES]')
                    AND type IN ( N'P', N'PC' ) ) 
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

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[REPARTO_EXPEDIENTES]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[REPARTO_EXPEDIENTES]
END
GO

/****** Object:  StoredProcedure [dbo].[REPARTO_EXPEDIENTES]    Script Date: 17/12/2018 16:14:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Luis Mario Lenis Ojeda>
-- Create date: <26-NOV-2018>
-- Description:	<Reparto Automatico de Expedientes>
-- =============================================
CREATE PROCEDURE [dbo].[REPARTO_EXPEDIENTES]
	-- Add the parameters for the stored procedure here

AS
BEGIN
-- EXEC [REPARTO_EXPEDIENTES]
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
	   
	   UPDATE tarea_asignada 
		SET  VAL_USUARIO_NOMBRE = @login,
		  FEC_ENTREGA_GESTOR=GETDATE(),
		  COD_ESTADO_OPERATIVO=11
      WHERE  EFINROEXP_EXPEDIENTE = @expediente; 
 
 END

 END

GO

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'HISTORICO_CLASIFICACION_MANUAL'))
BEGIN
	ALTER TABLE dbo.HISTORICO_CLASIFICACION_MANUAL
		DROP CONSTRAINT [FK_dbo.HISTORICO_CLASIFICACION_MANUAL_TIPOS_PROCESOS_CONCURSALES]
END
GO


IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[SP_ConsultarDeudoresExpedienteTitulo]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[SP_ConsultarDeudoresExpedienteTitulo]
END
GO

/*
Nombre		:	[SP_ConsultarDeudoresExpedienteTitulo]
Descripcion	:	Se consulta
Parametros	:	@NROTITULO Numero de titulo				

Historia	:
VERSION 	FECHA 			AUTOR 					DESCRIPCION
1.00.000	2018/12/13	OSCAR GONZALEZ 			CREACION
*/
CREATE PROCEDURE [dbo].[SP_ConsultarDeudoresExpedienteTitulo]
	@NROTITULO AS BIGINT = null		
AS
BEGIN
	SELECT  [deudor],
			[NroExp],
			[tipo],	
			[participacion],
			[ID_MAESTRO_TITULOS] 
	FROM [dbo].[DEUDORES_EXPEDIENTES]
	WHERE [ID_MAESTRO_TITULOS] = ISNULL(@NROTITULO,[ID_MAESTRO_TITULOS])
END
GO

IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'NoExpedienteOrigen'
          AND Object_ID = Object_ID(N'dbo.[MAESTRO_TITULOS]'))
BEGIN
	ALTER TABLE [dbo].[MAESTRO_TITULOS]
	DROP COLUMN [NoExpedienteOrigen];

	ALTER TABLE [dbo].[MAESTRO_TITULOS]
	ADD [NoExpedienteOrigen] VARCHAR(30);
END
GO

IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[SP_OBTENER_DEUDORES_POR_TITULO]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[SP_OBTENER_DEUDORES_POR_TITULO]
END
GO
-- =============================================
-- Author:		Eduar Fabian Hernandezn Nieves - Stefanini
-- Create date: 2018-11-29
-- Description:	Obtener los deudores relacionados con un título
-- EXEC SP_OBTENER_DEUDORES_POR_TITULO 15531
-- =============================================
CREATE PROCEDURE [dbo].[SP_OBTENER_DEUDORES_POR_TITULO]
	@ID_UNICO_TITULO AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET NOCOUNT ON;
	SELECT 
		ed.ED_TipoId
		,ed.ED_Codigo_Nit
		,ed.ED_DigitoVerificacion
		,ed.ED_TipoPersona
		,ed.ED_Nombre
		,ed.ED_EnteReal
		,ed.ED_idzapen
		,ed.ED_Excluir
		,ed.ED_Fecha550
		,ed.ED_EstadoPersona
		,ed.ED_TipoAportante
		,ed.ED_TarjetaProf
		,ed.VAL_NO_MATRICULA_MERCANTIL
	FROM 
		ENTES_DEUDORES ed
	JOIN
		 DEUDORES_EXPEDIENTES de ON ed.ED_Codigo_Nit = de.deudor
	WHERE 
		de.ID_MAESTRO_TITULOS = @ID_UNICO_TITULO
END
GO


IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'CONTEXTO_TRANSACCIONAL'))
BEGIN
	DROP TABLE [dbo].[CONTEXTO_TRANSACCIONAL]
END
BEGIN
	CREATE TABLE [dbo].[CONTEXTO_TRANSACCIONAL]
(
	[ID_CONTEXTO] [int] IDENTITY (1,1) NOT NULL,
	[ID_TX] [varchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
	[FECHA_INICIO] [datetime] NOT NULL,
	[ID_DEF_PROCESO] [varchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
	[NOMBRE_PROCESO] [varchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
	[ID_USUARIO_APP] [varchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[ID_EMISOR] [varchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	[FECHA_CREACION] [datetime] NOT NULL,
	[ID_TITULO] [int] NOT NULL,
	[COD_TX] [varchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
	CONSTRAINT [PK_CONTEXTO_TRANSACCIONAL] PRIMARY KEY CLUSTERED
	(
		[ID_CONTEXTO] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY  = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


IF(SELECT TOP(1) ID_TIPO_CARTERA from TIPOS_TITULO where codigo='01' ) is null
begin 
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='08'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='07'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='05'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='04'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='01'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='17'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='06'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='03'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='16'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='12'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=3 WHERE codigo='10'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=4 WHERE codigo='18'


UPDATE [dbo].[TIPOS_TITULO] SET [ANOS_FECHA_PRESCRIPCION]=3 WHERE codigo='03'
UPDATE [dbo].[TIPOS_TITULO] SET [ANOS_FECHA_PRESCRIPCION]=5 WHERE codigo<>'03'

END
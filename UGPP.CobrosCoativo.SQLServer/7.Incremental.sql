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
			((@NROTITULO = '') OR (MT.MT_nro_titulo LIKE '%' + @NROTITULO + '%')) AND
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


IF (EXISTS (SELECT * 
			FROM INFORMATION_SCHEMA.TABLES 
			WHERE TABLE_SCHEMA = 'dbo' 
			AND  TABLE_NAME = 'TIPO_SENTENCIA'))
BEGIN
	DECLARE @CODIGO_TIPO_SENTENCIA AS CHAR = (SELECT MAX(codigo) FROM TIPO_SENTENCIA)
	INSERT INTO TIPO_SENTENCIA(codigo, nombre) VALUES( (@CODIGO_TIPO_SENTENCIA + 1), 'TASA DE USURA E IPC' )
END
GO


IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_VERIFICAR_JERARQUIA_USUARIO_POR_LOGIN]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_VERIFICAR_JERARQUIA_USUARIO_POR_LOGIN] 
END
GO

-- =============================================
-- Author:		Edward Fabián Hernández Nieves
-- Create date: 2019-02-08
-- Description:	Verifica si el login del usuario que se pasa como parametro tiene la jerarquía que se consulta y también se pasa como parametro
-- =============================================
CREATE PROCEDURE SP_VERIFICAR_JERARQUIA_USUARIO_POR_LOGIN
	@LoginUsuario AS NVARCHAR(12),
	@Jerarquia AS INT -- {1:Revisor, 2:Coordinador, 3:Superior}
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	If(@Jerarquia = 1)
	BEGIN
		SELECT 
			codigo
			,nombre
			,documento
			,clave
			,nivelacces
			,cobrador
			,apppredial
			,appvehic
			,appcuotasp
			,appindycom
			,login
			,conuser
			,useractivo
			,useremail
			,usercamclave
			,superior
			,ind_gestor_estudios
			,ind_gestor_expedientes
		FROM USUARIOS 
		WHERE
			codigo IN(SELECT DISTINCT superior FROM USUARIOS)
			AND login = @LoginUsuario
	END

	If(@Jerarquia = 2)
	BEGIN
		SELECT 
			codigo
			,nombre
			,documento
			,clave
			,nivelacces
			,cobrador
			,apppredial
			,appvehic
			,appcuotasp
			,appindycom
			,login
			,conuser
			,useractivo
			,useremail
			,usercamclave
			,superior
			,ind_gestor_estudios
			,ind_gestor_expedientes
		FROM USUARIOS 
		WHERE
			codigo IN(SELECT superior FROM USUARIOS where codigo IN(SELECT DISTINCT superior FROM USUARIOS))
			AND login = @LoginUsuario
	END

	If(@Jerarquia = 3)
	BEGIN
		SELECT 
			codigo
			,nombre
			,documento
			,clave
			,nivelacces
			,cobrador
			,apppredial
			,appvehic
			,appcuotasp
			,appindycom
			,login
			,conuser
			,useractivo
			,useremail
			,usercamclave
			,superior
			,ind_gestor_estudios
			,ind_gestor_expedientes
		FROM USUARIOS 
		WHERE
			codigo IN(
				SELECT superior FROM USUARIOS 
				WHERE codigo IN(
					SELECT DISTINCT superior FROM USUARIOS 
					WHERE codigo IN(
						SELECT DISTINCT superior FROM USUARIOS
					)
				)
			)
			AND login = @LoginUsuario
	END
END
GO

IF (EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'TIPOS_APORTANTES')
)
BEGIN
	ALTER TABLE dbo.TIPOS_APORTANTES ADD ind_estado bit NULL
	ALTER TABLE dbo.TIPOS_APORTANTES ADD CONSTRAINT DF_TIPOS_APORTANTES_ind_estado DEFAULT 1 FOR ind_estado
END
GO

IF COL_LENGTH('dbo.TIPOS_APORTANTES','ind_estado') IS NOT NULL
BEGIN
	UPDATE dbo.TIPOS_APORTANTES SET ind_estado = 1
	UPDATE dbo.TIPOS_APORTANTES SET ind_estado = 0 WHERE nombre = 'SIN DATOS'
END
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_DOCUMENTOS_TIPO_TITULO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_DOCUMENTOS_TIPO_TITULO] 
END
GO

CREATE PROCEDURE [dbo].[SP_DOCUMENTOS_TIPO_TITULO]
	@COD_TIPO_TITULO		AS CHAR(2)
AS
	 SELECT DISTINCT DT.[ID_DOCUMENTO_TITULO],D.NOMBRE_DOCUMENTO,DT.[COD_TIPO_TITULO],DT.[VAL_ESTADO],DT.[VAL_OBLIGATORIO]
	 FROM [dbo].[DOCUMENTO_TITULO] D
		INNER JOIN [dbo].[DOCUMENTO_TITULO_TIPO_TITULO] DT ON D.ID_DOCUMENTO_TITULO =DT.ID_DOCUMENTO_TITULO
	 WHERE DT.[COD_TIPO_TITULO]=@COD_TIPO_TITULO
	
GO

INSERT INTO DOCUMENTO_TITULO (NOMBRE_DOCUMENTO) VALUES
('Oficios de notificación de resolución sanción'),
('Oficios de notificación del recurso de reconsideración o reposición')
GO

INSERT INTO [dbo].DOCUMENTO_TITULO_TIPO_TITULO (ID_DOCUMENTO_TITULO, COD_TIPO_TITULO, VAL_ESTADO, VAL_OBLIGATORIO)
VALUES
(40,'07',1,0),
(41,'07',1,0)
GO

DELETE FROM DOCUMENTO_TITULO_TIPO_TITULO
WHERE ID_DOCUMENTO_TITULO = 27 AND COD_TIPO_TITULO = '07'
GO

ALTER TABLE dbo.DIRECCIONES ADD
	OTRA_FUENTE varchar(20) NULL
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
	@OTRA_FUENTE VARCHAR(20) NULL
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
				,[OTRA_FUENTE]=@OTRA_FUENTE
				

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
				   ,[ID_FUENTE]
				   ,[OTRA_FUENTE])
			 VALUES
			 (@deudor,@Direccion, @Departamento,@Ciudad,@Telefono,@Email,@Movil,NULL,@ID_FUENTE,@OTRA_FUENTE)

		 END
END

GO


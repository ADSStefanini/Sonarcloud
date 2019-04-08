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



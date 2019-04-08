IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaDeudor]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaDeudor]  
END


GO






-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 31/10/2018
-- Description: INSERTA EL DEUDOR
-- =============================================

CREATE PROCEDURE [dbo].[SP_InsertaDeudor] 
   
   @ED_TipoId CHAR(2),
   @ED_Codigo_Nit VARCHAR(13),
   @ED_DigitoVerificacion VARCHAR(1)=NULL,
   @ED_TipoPersona CHAR(2),
   @ED_Nombre  VARCHAR(120),
   @ED_TipoAportante  VARCHAR(120)= NULL, 
   @ED_EstadoPersona VARCHAR(120)=NULL,
   @ED_TarjetaProf  VARCHAR(120)=NULL,
   @ED_IDUNICO_TITULO_MAESTRO INT,
   @TIPO_DEUDOR INT,
   @PARTICIPACION  FLOAT

   AS
   
BEGIN                     
    
SET NOCOUNT ON   

		IF EXISTS (SELECT * FROM [dbo].[ENTES_DEUDORES] WHERE  [ED_Codigo_Nit] = @ED_Codigo_Nit)

		BEGIN

		UPDATE  [dbo].[ENTES_DEUDORES] 
		SET 
		[ED_TipoId] = @ED_TipoId,
        [ED_Codigo_Nit] = @ED_Codigo_Nit,
        [ED_DigitoVerificacion] = @ED_DigitoVerificacion,
        [ED_TipoPersona] = @ED_TipoPersona,
        [ED_Nombre] = @ED_Nombre,
        [ED_EnteReal] = NULL,
        [ED_idzapen] = NULL,
        [ED_Excluir] = NULL,
        [ED_Fecha550] = NULL,
        [ED_EstadoPersona] = @ED_EstadoPersona,
        [ED_TipoAportante] = @ED_TipoAportante,
        [ED_TarjetaProf] = @ED_TarjetaProf
		

		 WHERE [ED_Codigo_Nit] = @ED_Codigo_Nit


		END

		ELSE
			BEGIN
			INSERT INTO [dbo].[ENTES_DEUDORES]
			   ([ED_TipoId]
			   ,[ED_Codigo_Nit]
			   ,[ED_DigitoVerificacion]
			   ,[ED_TipoPersona]
			   ,[ED_Nombre]
			   ,[ED_EnteReal]
			   ,[ED_idzapen]
			   ,[ED_Excluir]
			   ,[ED_Fecha550]
			   ,[ED_EstadoPersona]
			   ,[ED_TipoAportante]
			   ,[ED_TarjetaProf])
		 VALUES
			   (@ED_TipoId,
			   @ED_Codigo_Nit,
			   @ED_DigitoVerificacion,
			   @ED_TipoPersona,
			   @ED_Nombre,
			   NULL,
			   NULL,
			   NULL,
			   NULL,
			   @ED_EstadoPersona,
			   @ED_TipoAportante,
			   @ED_TarjetaProf			
			   )

		END

		

		IF EXISTS (SELECT * FROM [dbo].[DEUDORES_EXPEDIENTES] WHERE  deudor = @ED_Codigo_Nit AND [ID_MAESTRO_TITULOS]= @ED_IDUNICO_TITULO_MAESTRO)
		BEGIN 
			UPDATE [dbo].[DEUDORES_EXPEDIENTES]
			   SET 
				  [tipo] = @TIPO_DEUDOR,
				  [participacion] = @PARTICIPACION
			 WHERE deudor = @ED_Codigo_Nit AND [ID_MAESTRO_TITULOS]= @ED_IDUNICO_TITULO_MAESTRO
		END
		ELSE
		BEGIN 
			INSERT INTO [dbo].[DEUDORES_EXPEDIENTES]
			   ([deudor]
			   ,[tipo]
			   ,[participacion]
			   ,[ID_MAESTRO_TITULOS])
		 VALUES
			   (@ED_Codigo_Nit
			   ,@TIPO_DEUDOR
			   ,@PARTICIPACION
			   ,@ED_IDUNICO_TITULO_MAESTRO)
		END


 END

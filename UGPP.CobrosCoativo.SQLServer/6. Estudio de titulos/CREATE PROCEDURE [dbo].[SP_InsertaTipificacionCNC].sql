IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaTipificacionCNC]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaTipificacionCNC] 
END


GO


-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 21/11/2018
-- Description: INSERTA LA TIPIFICACION DE CUMPLE NO CUMPLE
-- =============================================


CREATE PROCEDURE [dbo].[SP_InsertaTipificacionCNC] 

   @ID_TIPIFICACION BIGINT,
   @ID_UNICO_MT BIGINT,
   @USUARIO VARCHAR(100)

      AS
   
BEGIN                     
      
SET NOCOUNT ON  
 
INSERT INTO [dbo].[TIPIFICACION_CNC]
           ([ID_TIPIFICACION]
           ,[ID_UNICO_MT]
           ,[USUARIO]
           ,[FCHOBSERVACIONES])
     VALUES
          (@ID_TIPIFICACION,@ID_UNICO_MT,@USUARIO,GETDATE())
END


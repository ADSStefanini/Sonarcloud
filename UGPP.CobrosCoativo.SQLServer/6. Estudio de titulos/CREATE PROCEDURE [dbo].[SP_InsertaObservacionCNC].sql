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


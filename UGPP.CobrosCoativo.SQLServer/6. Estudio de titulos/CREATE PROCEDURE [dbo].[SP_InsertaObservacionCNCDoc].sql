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
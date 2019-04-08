IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaNotificacion]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaNotificacion] 
END


GO



-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 31/10/2018
-- Description: INSERTA NOTIFICACIÓN
-- =============================================

CREATE PROCEDURE [dbo].[SP_InsertaNotificacion] 

      @ID_UNICO_MAESTRO_TITULOS BIGINT,
      @FEC_NOTIFICACION DATETIME,
      @COD_FOR_NOT CHAR(2),
      @COD_TIPO_NOTIFICACION INT

   AS
   
BEGIN                     
    
SET NOCOUNT ON   



INSERT INTO [dbo].[MAESTRO_TITULOS_FOR_NOTIFICACION]
           ([ID_UNICO_MAESTRO_TITULOS]
           ,[FEC_NOTIFICACION]
           ,[COD_FOR_NOT]
           ,[COD_TIPO_NOTIFICACION])
     VALUES
(@ID_UNICO_MAESTRO_TITULOS,@FEC_NOTIFICACION,@COD_FOR_NOT,@COD_TIPO_NOTIFICACION)



 END
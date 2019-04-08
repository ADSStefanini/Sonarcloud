IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_GuardaDatosValores]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_GuardaDatosValores]  
END


GO




-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 30/10/2018
-- Description: OBTENER LA CONSULTA DE LA PARAMETRIZACIÓN DE LOS VALORES
-- =============================================

CREATE PROCEDURE [dbo].[SP_GuardaDatosValores] 
      
	  @ID_TIPO_OBLIGACION_VALORES VARCHAR(50),
	  @VALOR_OBLIGACION BIT,
      @PARTIDA_GLOBAL BIT,
      @SANCION_OMISION BIT,
      @SANCION_INEXACTITUD BIT,
      @SANCION_MORA BIT = 0 
AS                        
      
BEGIN                     
      
SET NOCOUNT ON       


UPDATE  [dbo].[TIPO_OBLIGACION_VALORES] SET
            [VALOR_OBLIGACION] = @VALOR_OBLIGACION
           ,[PARTIDA_GLOBAL] = @PARTIDA_GLOBAL
           ,[SANCION_OMISION] = @SANCION_OMISION
           ,[SANCION_INEXACTITUD] = @SANCION_INEXACTITUD
           ,[SANCION_MORA] =  @SANCION_MORA
  
  WHERE [ID_TIPO_OBLIGACION_VALORES] = @ID_TIPO_OBLIGACION_VALORES
	 
	
END 


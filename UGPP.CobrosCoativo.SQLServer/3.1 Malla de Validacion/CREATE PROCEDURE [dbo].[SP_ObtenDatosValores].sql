IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_ObtenDatosValores]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_ObtenDatosValores] 
END


GO

-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 30/10/2018
-- Description: OBTENER LA CONSULTA DE LA PARAMETRIZACIÓN DE LOS VALORES
-- =============================================

CREATE PROCEDURE [dbo].[SP_ObtenDatosValores] 
      
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
	WHERE B.codigo IN ('17','01','08','05','04','07','16','03','15','06','10','18')      ORDER BY B.codigo 
END 



GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_DOCUMENTOS_TITULO_TIPO_TITULO]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_DOCUMENTOS_TITULO_TIPO_TITULO] 
END


GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

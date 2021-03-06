IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_InsertaJustificacionCierreTit]')
                    AND type IN ( N'P', N'PC' ) ) 

BEGIN 
	DROP PROCEDURE [dbo].[SP_InsertaJustificacionCierreTit] 
END


GO


-- =============================================
-- Author: CARLOS FELIPE MOTTA MONJE	
-- Create date: 15/11/2018
-- Description: INSERTA LA JUSTIFICACIÓN DEL CIERRE DE TÍTULO
-- =============================================


CREATE PROCEDURE [dbo].[SP_InsertaJustificacionCierreTit] 

   @ID_UNICO_MT INT,
   @DESC_JUSTIFICACION_CIERRE VARCHAR(300)

      AS
   
BEGIN                     
      
SET NOCOUNT ON   


DECLARE @IDTAREAASIGNADATITULO INT
DECLARE @IDTAREAASIGNADAEXPEDIENTE INT

 SELECT @IDTAREAASIGNADATITULO = ID_TAREA_ASIGNADA  FROM  TAREA_ASIGNADA WHERE COD_TIPO_OBJ = 4 AND ID_UNICO_TITULO = @ID_UNICO_MT
 
 SELECT @IDTAREAASIGNADAEXPEDIENTE = ID_TAREA_ASIGNADA  FROM  TAREA_ASIGNADA WHERE COD_TIPO_OBJ = 5 AND ID_UNICO_TITULO = @ID_UNICO_MT

		INSERT INTO [dbo].[JUSTITICACION_CIERRE_TITULO]
				   ([ID_UNICO_MT]
				   ,[DESC_JUSTIFICACION_CIERRE]
				   ,[FEC_JUSTIFICACION_CIERRE])
			 VALUES
			 (@ID_UNICO_MT,@DESC_JUSTIFICACION_CIERRE,GETDATE())

	
	IF @IDTAREAASIGNADATITULO <> NULL
	BEGIN
	UPDATE TAREA_ASIGNADA SET COD_ESTADO_OPERATIVO = 20 WHERE ID_TAREA_ASIGNADA = @IDTAREAASIGNADATITULO
	END

	IF @IDTAREAASIGNADAEXPEDIENTE <> NULL
	BEGIN
	UPDATE TAREA_ASIGNADA SET COD_ESTADO_OPERATIVO = 18 WHERE ID_TAREA_ASIGNADA = @IDTAREAASIGNADAEXPEDIENTE
	END

END


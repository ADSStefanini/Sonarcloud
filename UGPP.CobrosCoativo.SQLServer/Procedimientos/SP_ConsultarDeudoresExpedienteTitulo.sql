IF EXISTS ( SELECT * 
            FROM   sysobjects 
            WHERE  id = object_id(N'[dbo].[SP_ConsultarDeudoresExpedienteTitulo]') 
                   and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
DROP PROCEDURE [dbo].[SP_ConsultarDeudoresExpedienteTitulo]
END
GO

/*
Nombre		:	[SP_ConsultarDeudoresExpedienteTitulo]
Descripcion	:	Se consulta
Parametros	:	@NROTITULO Numero de titulo

Historia	:
VERSION 	FECHA 			AUTOR 					DESCRIPCION
1.00.000	2018/12/13	OSCAR GONZALEZ 			CREACION
*/
CREATE PROCEDURE [dbo].[SP_ConsultarDeudoresExpedienteTitulo]
	@NROTITULO AS BIGINT = null	
AS	
BEGIN
	SELECT  [deudor],
			[NroExp],
			[tipo],	
			[participacion],
			[ID_MAESTRO_TITULOS] 
	FROM [dbo].[DEUDORES_EXPEDIENTES]
	WHERE [ID_MAESTRO_TITULOS] = ISNULL(@NROTITULO,[ID_MAESTRO_TITULOS])
END
GO



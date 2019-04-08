/*
Nombre		:	SP_TIPOS_AREA_ORIGEN
Descripcion	:	Consulta todos los tipos de area origen
Parametros	:	

Historia	:
VERSION 	FECHA 			AUTOR 					DESCRIPCION
1.00.000	2018/11/18		OSCAR GONZALEZ 			CREACION
*/
CREATE PROCEDURE [dbo].[SP_TIPOS_AREA_ORIGEN]
AS
	SELECT P.codigo,P.nombre FROM PROCEDENCIA_TITULOS P	
GO
-- =============================================
-- Author:		Stefanini - Edward Hernandez
-- Create date: 2018-10-23
-- Description:	Obtener las lista de páginas activas asociadas a un perfil
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerPaginasPorPerfil]
	@ID_PERFIL INT,
	@ID_PARENTPAGE INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #ids ( id INT NOT NULL);

	IF @ID_PARENTPAGE = 0
			INSERT INTO #ids (id)
			SELECT pk_codigo FROM pagina
			WHERE fk_padre IS NULL AND ind_pagina_interna = 0
	ELSE
			INSERT INTO #ids (id)
			SELECT pk_codigo FROM pagina
			WHERE fk_padre = @ID_PARENTPAGE AND ind_pagina_interna = 0

    SELECT 
		p.pk_codigo
		,p.val_nombre
		,p.val_url 
		,p.fk_padre
		,CAST(CASE p.ind_estado WHEN 1 THEN 1 ELSE 0 END AS INT) AS ind_estado
	FROM 
		PAGINA p
	JOIN 
		PERFIL_PAGINA pp ON p.pk_codigo = pp.fk_pagina_id
	WHERE 
		p.ind_estado = 1
		AND pp.ind_estado = 1
		AND pp.fk_perfil_id = @ID_PERFIL
		AND p.pk_codigo IN(
			SELECT id FROM #ids
		)
		AND p.ind_pagina_interna = 0

	DROP TABLE #ids
END
GO

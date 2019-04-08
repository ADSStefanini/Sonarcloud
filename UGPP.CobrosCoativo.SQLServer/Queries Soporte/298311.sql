SELECT 
	DISTINCT TDJ.NroTituloJ AS 'No. de Título Judicial' 
	,TDJ.NroDeposito AS 'No. de Depósito'
	,TDJ.ValorTDJ AS 'Valor del Título'
	,ETDJ.nombre AS 'Estado del Título'
	,TDJ.FecRecibido AS 'Fecha Recibido'
	,TDJ.FecEmision AS 'Fecha Emisión del título'
	,ED.ED_Nombre AS 'Consignante'
	,MB.BAN_NOMBRE AS 'Banco'
	,TDJ.Observac AS 'Observaciones'
	,TDJ.NroResolGes AS 'No. de Resolución'
	,TDJ.FecResolGes AS 'Fecha de Resolución'
	,TDJ.NroMemoGes AS 'No. Memorando'
	,TDJ.FecEnvioMemo AS 'Fecha de envio de Memorando'
	,TRTDJ.nombre AS 'Tipo de Resolución'
	,TDJ.FecDevol AS 'Fecha devolución'
FROM TDJ
LEFT JOIN ESTADOS_TDJ AS ETDJ ON TDJ.EstadoTDJ = ETDJ.codigo
LEFT JOIN MAESTRO_BANCOS AS MB ON TDJ.Banco = MB.BAN_CODIGO
LEFT JOIN TIPOS_RESOLTDJ AS TRTDJ ON TDJ.TipoResolGes = TRTDJ.codigo
LEFT JOIN ENTES_DEUDORES AS ED ON TDJ.Consignante = ED.ED_Codigo_Nit
LEFT JOIN DETALLE_EMBARGO AS DE ON TDJ.NroResolGes = de.NroResolEm
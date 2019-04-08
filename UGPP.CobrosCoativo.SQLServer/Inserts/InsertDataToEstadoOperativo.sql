-- =============================================
-- Author:		Stefanini - yeferson alba
-- Create date: 2018-11-30
-- Description:	Insertar Data tabla [ESTADO_OPERATIVO]
-- =============================================
INSERT INTO [dbo].[ESTADO_OPERATIVO]
           ([COD_TIPO_OBJ]
           ,[VAL_NOMBRE]
           ,[DEC_ESTADO_OPERATIVO]
           ,[IND_ESTADO])
     VALUES
           
(1,4,'EnCreacion','Puede estar todo titulo que se guardo parcialmente en el momento de cargue, Aun no se registra',1),
(2,4,'PorRepartir','El título se encuentra pendiente de reparto',1),
(3,4,'EnSolicitud','Se solicita un reasignación o priorización',1),
(4,4,'Asignado','En bandeja sin abrir / posible reparto',1),
(5,4,'Engestión','Abre el titulo o fue priorizado, o un retorno(Puede editar)',1),
(6,4,'Aceptado','Se termina la gestión del titulo y se convierte en un expediente',1),
(7,4,'Stand By','Puede estar todo expediente que se pauso por medio de una suspensión',1),
(8,4,'Subsanar','En este estado están los títulos devueltos por estudio de títulos',1),
(9,4,'Retorno',	'Titulo que tiene la respuesta de una devolución',1),
(10,5,'PorRepartir','El Expediente se encuentra expediente de reparto',1),
(11,5,'Asignado','En bandeja sin abrir / posible reparto si no se abre',1),
(12,5,'Retorno','Expediente que tiene la respuesta de una devolución',1),
(13,5,'EnSolicitud','Se solicita un cambio de estado, reasignación o priorización',1),
(14,5,'Stand By','En este estado puede estar el expediente que se pauso por medio de una solicitud',1),
(15,5,'EnGestión','Abre el expediente',1),
(16,5,'Terminado','Se termina la gestión del expediente',1),
(17,5,'Retorno','Expediente que tiene la respuesta de una devolución',1),
(18,5,'CierreAdministrativo','Cuando un expediente se retorna hasta estudio de títulos y no se debe continuar con su gestión',1),
(19,5,'Subsanar','En este estado están los expediente devueltos por estudio de títulos',1),
(20,4,'Terminado','Se termina la gestion del titulo',1);
GO

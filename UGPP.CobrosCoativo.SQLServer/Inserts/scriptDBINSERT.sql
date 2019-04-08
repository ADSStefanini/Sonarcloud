--DROP TABLE [dbo].[DOMINIO]
--DROP TABLE [dbo].[DOMINIO_DETALLE] 
--DBCC CHECKIDENT ('DOMINIO_DETALLE', RESEED, 11) 
INSERT INTO [dbo].[DOMINIO] ([DESCRIPCION])  VALUES 
('TIPOS DE NOTIFICACION'),
('TIPOS DE OBJETO'),
('TIPOS DE SOLICITUD'),
('Tiempo Correción área origen'),
('TIPOS DE RUTA')

INSERT INTO [dbo].[DOMINIO_DETALLE] ([ID_DOMINIO],[VAL_NOMBRE],[DESC_DESCRIPCION],[VAL_VALOR])  VALUES  
(1,'N_Titulo','Notificacion del titulo al usuario','1'),
(1,'N_RecursoReposicion_SegundaInstancia','Notificacion de un recurso de reposicion o segunda instacia','2'),
(1,'N_Apelacion_Reconsideracion_Casacion','Notificacion de apelacion o recurso de casacion','3'),
(2,'Titulo','Objeto de tipo titulo',NULL),
(2,'Expediente','Objeto de tipo expediente',NULL),
(3,'SolicitudSuspension','Solicitud Suspension',NULL),
(3,'SolicitudCambioEstado','Solicitud Cambio Estado',NULL),
(3,'SolicitudPriorizacion','Solicitud Priorizacion',NULL),
(3,'SolicitudResignacion','Solicitud Resignacion',NULL),
(4,'AlertaAmarillaBandejaAreaOrigen','Tiempo para mostrar una alerta amarilla en la bandeja de área origen de un título devuelto sin corregir',3),
(4,'AlertaRojaBandejaAreaOrigen','Tiempo para mostrar una alerta roja en la bandeja de área origen de un título devuelto sin corregir',5),
(5,'Local','Ruta alamacenamiento local',null),
(5,'Documentic','Link de documentic',null)
INSERT INTO FUENTE_DIRECCION (DES_NOMBRE_FUENTE_DIRECCION) VALUES
('Procesal'),
('RUT'),
('RUES'),
('Otra')

INSERT INTO [dbo].[TIPO_CARTERA] (DEC_TIPO_CARTERA) VALUES 
('Parafiscales'),
('Pensional'),
('Disciplinarios'),
('Administrativa')
--DELETE FROM DOCUMENTO_TITULO  

--DBCC CHECKIDENT ('DOCUMENTO_TITULO', RESEED, 0) 
INSERT INTO DOCUMENTO_TITULO (NOMBRE_DOCUMENTO) VALUES
('Acto administrativo de la entidad concurrente donde acepte la obligación impuesta o la constancia de su notificación y del silencio administrativo positivo.'),
('Actos Administrativos de reconocimiento de las prestaciones donde se haya aplicado la figura de la cuota paMayorte pensional (pensión de jubilación, reliquidaciones, sustituciones, etc.)'),
('Archivo con la información desagregada de la deuda que hace parte integral de la liquidación'),
('Archivo con la información desagregada de la deuda que hace parte integral de la liquidación oficial y/o de la resolución que resuelve el recurso de reconsideración (SQL)'),
('Auto liquida las costas'),
('Auto que aprueba las costas'),
('Certificación Descuentos de Nómina '),
('Certificado de pago de las mesadas pensionales por los periodos cobrados.'),
('Constancia de Ejecutoria '),
('Constancia de envío (guía) de la comunicación anterior'),
('Constancia de envío (Guía) de la cuenta de cobro.'),
('Constancia de la comunicación por medio de la cual se remite al deudor la resolución en la que se determina el valor de la cuota parte (separada o con las cuentas de cobro)'),
('Constancia de Notificación (guía correo y/o aviso con constancia de fijación y desfijación y/o certificado de notificación electrónico)'),
('Constancia de Notificación (Oficio Citatorio, aviso con sus respectivas guías de entrega y/o, constancia de publicación página Web, notificación electrónica con su respectivo certificado de certimail)'),
('Constancia de Notificación del recurso (guía correo y/o acta de notificación personal o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico)'),
('Constancia de Notificación del recurso de reconsideración (guía correo y/o acta de notificación personal o aviso con constancia de fijación y des fijación y/o certificado de notificación electrónico) '),
('Consulta del proyecto de Resolución en donde se determina el valor proyectado de la cuota.'),
('Copia de la C.C. del deudor o Beneficiario'),
('Copia de la sentencia judicial de primera instancia'),
('Copia de la sentencia judicial de segunda instancia y casación '),
('Copia del documento de identificación del Pensionado'),
('Cuenta de cobro a la entidad deudora.'),
('Informe de fiscalización o Requerimiento para declarar y/o corregir'),
('Liquidación de la cuota parte.'),
('Liquidación detallada de los mayores valores pagados en archivo pdf y excel'),
('Oficial y/o de la resolución que resuelve el recurso de reconsideración (SQL).'),
('Oficios de notificación'),
('Pliego de cargos '),
('Resolución de Liquidación Oficial o de Liquidación/Sanción'),
('Resolución por medio de la cual se determinan unos mayores valores recibidos '),
('Resolución que modifique el acto administrativo que determina el mayor valor pagado, junto con sus respectivas notificaciones '),
('Resolución que resuelve el recurso de reconsideración y sus respectivos '),
('Resolución que resuelve el recurso de reposición junto con su notificación. '),
('Resolución que resuelve el recurso de reposición. '),
('Resolución que resuelve recurso de reconsideración o reposición'),
('Resolución Sanción y sus respectivos.'),
('Resolución Sancionatoria'),
('Sentencia')


INSERT INTO [dbo].DOCUMENTO_TITULO_TIPO_TITULO
(ID_DOCUMENTO_TITULO,
COD_TIPO_TITULO,
VAL_ESTADO,
VAL_OBLIGATORIO)
VALUES
(1,'03',1,1),
(2,'03',1,1),
(3,'02',1,1),
(3,'04',1,1),
(4,'01',1,1),
(5,'16',1,1),
(6,'16',1,1),
(7,'12',1,0),
(8,'03',1,1),
(9,'06',1,1),
(9,'16',1,1),
(9,'01',1,1),
(9,'07',1,1),
(9,'12',1,1),
(9,'10',1,1),
(10,'03',1,1),
(11,'03',1,1),
(12,'03',1,1),
(13,'01',1,1),
(13,'02',1,1),
(13,'04',1,1),
(13,'07',1,1),
(13,'17',1,0),
(13,'10',1,1),
(14,'12',1,1),
(15,'07',1,1),
(16,'01',1,0),
(17,'03',1,1),
(18,'12',1,1),
(19,'06',1,1),
(20,'06',1,0),
(21,'03',1,1),
(22,'03',1,1),
(23,'02',1,1),
(23,'04',1,1),
(24,'03',1,1),
(25,'12',1,1),
(26,'02',1,0),
(26,'04',1,0),
(26,'04',1,0),
(26,'17',1,0),
(27,'02',1,0),
(27,'01',1,1),
(27,'07',1,1),
(27,'07',1,1),
(27,'01',1,0),
(27,'03',1,0),
(27,'12',1,0),
(28,'17',1,1),
(29,'01',1,1),
(30,'12',1,1),
(31,'12',1,0),
(32,'01',1,0),
(33,'12',1,0),
(33,'10',1,0),
(34,'03',1,0),
(35,'07',1,1),
(36,'07',1,1),
(37,'10',1,1),
(38,'16',1,1)

INSERT INTO TIPOS_TITULO (codigo,nombre,ID_TIPO_CARTERA) VALUES ('18','POLIZAS DE CUMPLIMIENTO',4)

UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='08'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='07'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='05'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='04'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='01'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=1 WHERE codigo='17'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='06'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='03'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='16'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=2 WHERE codigo='12'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=3 WHERE codigo='10'
UPDATE TIPOS_TITULO SET ID_TIPO_CARTERA=4 WHERE codigo='18'



--BACKUP DATABASE UGPPCobrosCoactivo TO  DISK = N'C:\Cod\SQL\20181101_UGPP_DEV.bak' WITH  INIT , NOUNLOAD ,  NAME = N'UGPPCobrosCoactivo',  STATS = 10,  FORMAT

GO

INSERT INTO [ESTADO_OPERATIVO] ([COD_TIPO_OBJ],[VAL_NOMBRE],[DEC_ESTADO_OPERATIVO]) VALUES
(4,N'EnCreacion','Puede estar todo titulo que se guardo parcialmente en el momento de cargue, Aun no se registra '),
(4,N'PorRepartir','El título se encuentra pendiente de reparto'),
(4,N'EnSolicitud','Se solicita un reasignación o priorización'),
(4,N'Asignado','En bandeja sin abrir / posible reparto '),
(4,N'Engestión','Abre el titulo o fue priorizado, o un retorno(Puede editar)'),
(4,N'Aceptado','Se termina la gestión del titulo y se convierte en un expediente'),
(4,N'Stand By','Puede estar todo expediente que se pauso por medio de una suspensión '),
(4,N'Subsanar','En este estado están los títulos devueltos por estudio de títulos'),
(4,N'Retorno','Titulo que tiene la respuesta de una devolución'),
(5,N'PorRepartir','El Expediente se encuentra expediente de reparto'),
(5,N'Asignado','En bandeja sin abrir / posible reparto si no se abre'),
(5,N'Retorno ','Expediente que tiene la respuesta de una devolución'),
(5,N'EnSolicitud','Se solicita un cambio de estado, reasignación o priorización'),
(5,N'Stand By','En este estado puede estar el expediente que se pauso por medio de una solicitud '),
(5,N'EnGestión','Abre el expediente '),
(5,N'Terminado','Se termina la gestión del expediente'),
(5,N'Retorno ','Expediente que tiene la respuesta de una devolución'),
(5,N'Cierre Administrativo','Cuando un expediente se retorna hasta estudio de títulos y no se debe continuar con su gestión'),
(5,N'Subsanar','En este estado están los expediente devueltos por estudio de títulos')



--DROP TABLE [dbo].[DOMINIO]
--DROP TABLE [dbo].[DOMINIO_DETALLE] 
--DBCC CHECKIDENT ('DOMINIO_DETALLE', RESEED, 11) 
INSERT INTO [dbo].[DOMINIO] ([DESCRIPCION])  VALUES 
('TIPOS DE NOTIFICACION'),
('TIPOS DE OBJETO'),
('TIPOS DE SOLICITUD'),
('Tiempo Correci�n �rea origen'),
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
(4,'AlertaAmarillaBandejaAreaOrigen','Tiempo para mostrar una alerta amarilla en la bandeja de �rea origen de un t�tulo devuelto sin corregir',3),
(4,'AlertaRojaBandejaAreaOrigen','Tiempo para mostrar una alerta roja en la bandeja de �rea origen de un t�tulo devuelto sin corregir',5),
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
('Acto administrativo de la entidad concurrente donde acepte la obligaci�n impuesta o la constancia de su notificaci�n y del silencio administrativo positivo.'),
('Actos Administrativos de reconocimiento de las prestaciones donde se haya aplicado la figura de la cuota paMayorte pensional (pensi�n de jubilaci�n, reliquidaciones, sustituciones, etc.)'),
('Archivo con la informaci�n desagregada de la deuda que hace parte integral de la liquidaci�n'),
('Archivo con la informaci�n desagregada de la deuda que hace parte integral de la liquidaci�n oficial y/o de la resoluci�n que resuelve el recurso de reconsideraci�n (SQL)'),
('Auto liquida las costas'),
('Auto que aprueba las costas'),
('Certificaci�n Descuentos de N�mina '),
('Certificado de pago de las mesadas pensionales por los periodos cobrados.'),
('Constancia de Ejecutoria '),
('Constancia de env�o (gu�a) de la comunicaci�n anterior'),
('Constancia de env�o (Gu�a) de la cuenta de cobro.'),
('Constancia de la comunicaci�n por medio de la cual se remite al deudor la resoluci�n en la que se determina el valor de la cuota parte (separada o con las cuentas de cobro)'),
('Constancia de Notificaci�n (gu�a correo y/o aviso con constancia de fijaci�n y desfijaci�n y/o certificado de notificaci�n electr�nico)'),
('Constancia de Notificaci�n (Oficio Citatorio, aviso con sus respectivas gu�as de entrega y/o, constancia de publicaci�n p�gina Web, notificaci�n electr�nica con su respectivo certificado de certimail)'),
('Constancia de Notificaci�n del recurso (gu�a correo y/o acta de notificaci�n personal o aviso con constancia de fijaci�n y des fijaci�n y/o certificado de notificaci�n electr�nico)'),
('Constancia de Notificaci�n del recurso de reconsideraci�n (gu�a correo y/o acta de notificaci�n personal o aviso con constancia de fijaci�n y des fijaci�n y/o certificado de notificaci�n electr�nico) '),
('Consulta del proyecto de Resoluci�n en donde se determina el valor proyectado de la cuota.'),
('Copia de la C.C. del deudor o Beneficiario'),
('Copia de la sentencia judicial de primera instancia'),
('Copia de la sentencia judicial de segunda instancia y casaci�n '),
('Copia del documento de identificaci�n del Pensionado'),
('Cuenta de cobro a la entidad deudora.'),
('Informe de fiscalizaci�n o Requerimiento para declarar y/o corregir'),
('Liquidaci�n de la cuota parte.'),
('Liquidaci�n detallada de los mayores valores pagados en archivo pdf y excel'),
('Oficial y/o de la resoluci�n que resuelve el recurso de reconsideraci�n (SQL).'),
('Oficios de notificaci�n'),
('Pliego de cargos '),
('Resoluci�n de Liquidaci�n Oficial o de Liquidaci�n/Sanci�n'),
('Resoluci�n por medio de la cual se determinan unos mayores valores recibidos '),
('Resoluci�n que modifique el acto administrativo que determina el mayor valor pagado, junto con sus respectivas notificaciones '),
('Resoluci�n que resuelve el recurso de reconsideraci�n y sus respectivos '),
('Resoluci�n que resuelve el recurso de reposici�n junto con su notificaci�n. '),
('Resoluci�n que resuelve el recurso de reposici�n. '),
('Resoluci�n que resuelve recurso de reconsideraci�n o reposici�n'),
('Resoluci�n Sanci�n y sus respectivos.'),
('Resoluci�n Sancionatoria'),
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
(4,N'PorRepartir','El t�tulo se encuentra pendiente de reparto'),
(4,N'EnSolicitud','Se solicita un reasignaci�n o priorizaci�n'),
(4,N'Asignado','En bandeja sin abrir / posible reparto '),
(4,N'Engesti�n','Abre el titulo o fue priorizado, o un retorno(Puede editar)'),
(4,N'Aceptado','Se termina la gesti�n del titulo y se convierte en un expediente'),
(4,N'Stand By','Puede estar todo expediente que se pauso por medio de una suspensi�n '),
(4,N'Subsanar','En este estado est�n los t�tulos devueltos por estudio de t�tulos'),
(4,N'Retorno','Titulo que tiene la respuesta de una devoluci�n'),
(5,N'PorRepartir','El Expediente se encuentra expediente de reparto'),
(5,N'Asignado','En bandeja sin abrir / posible reparto si no se abre'),
(5,N'Retorno ','Expediente que tiene la respuesta de una devoluci�n'),
(5,N'EnSolicitud','Se solicita un cambio de estado, reasignaci�n o priorizaci�n'),
(5,N'Stand By','En este estado puede estar el expediente que se pauso por medio de una solicitud '),
(5,N'EnGesti�n','Abre el expediente '),
(5,N'Terminado','Se termina la gesti�n del expediente'),
(5,N'Retorno ','Expediente que tiene la respuesta de una devoluci�n'),
(5,N'Cierre Administrativo','Cuando un expediente se retorna hasta estudio de t�tulos y no se debe continuar con su gesti�n'),
(5,N'Subsanar','En este estado est�n los expediente devueltos por estudio de t�tulos')



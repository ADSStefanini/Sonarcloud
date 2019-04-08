ALTER TABLE [dbo].[CAMBIOS_ESTADO] ADD estadooperativo int;
ALTER TABLE [dbo].[CAMBIOS_ESTADO] ADD etapaprocesal int;

ALTER TABLE [dbo].[CAMBIOS_ESTADO]  WITH CHECK ADD  CONSTRAINT [FK_CAMBIOS_ESTADO_ETAPA_PROCESAL] FOREIGN KEY([etapaprocesal])
REFERENCES [dbo].[ETAPA_PROCESAL] ([ID_ETAPA_PROCESAL]);

ALTER TABLE [dbo].[CAMBIOS_ESTADO]  WITH CHECK ADD  CONSTRAINT [FK_CAMBIOS_ESTADO_ESTADO_OPERATIVO] FOREIGN KEY([estadooperativo])
REFERENCES [dbo].[ESTADO_OPERATIVO] ([ID_ESTADO_OPERATIVOS]);

GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 20, 10)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 11)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 12)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 13)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 14)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 15)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 16)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 7, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (2, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 11, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (13, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 14, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 15, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 16, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 17, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 18, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 19, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (20, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (11, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (12, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (12, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 13, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (14, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (14, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (5, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (12, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (2, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (3, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (4, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (4, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (4, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (4, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (5, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (5, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (6, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (6, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 6, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 7, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 7, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 7, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (7, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 7, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (7, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (8, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (9, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 1)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 2)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 6)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 9, 7)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 8)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 9)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 8, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 11, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 11, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 11, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 11, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 12, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES (2, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 3)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 1, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 5, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 2, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 4, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 5)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
GO
INSERT [dbo].[CAMBIOS_ESTADO] ( [estadooperativo], [etapaprocesal]) VALUES ( 3, 4)
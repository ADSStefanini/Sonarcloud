ALTER TABLE [dbo].[TIPO_DOCUMENTAL] DROP CONSTRAINT [FK_TIPO_DOCUMENTAL_SUBSERIE]
GO
ALTER TABLE [dbo].[TIPO_DOCUMENTAL] DROP CONSTRAINT [FK_TIPO_DOCUMENTAL_SERIE]
GO
ALTER TABLE [dbo].[SUBSERIE] DROP CONSTRAINT [FK_SUBSERIE_SERIE]
GO
ALTER TABLE [dbo].[SERIE] DROP CONSTRAINT [FK_SERIE_DIRECCION_OFICINA]
GO
/****** Object:  Table [dbo].[TIPO_DOCUMENTAL]    Script Date: 20/11/2018 7:01:28 p. m. prueba para git ******/
DROP TABLE [dbo].[TIPO_DOCUMENTAL]
GO
/****** Object:  Table [dbo].[SUBSERIE]    Script Date: 20/11/2018 7:01:28 p. m. ******/
DROP TABLE [dbo].[SUBSERIE]
GO
/****** Object:  Table [dbo].[SERIE]    Script Date: 20/11/2018 7:01:28 p. m. ******/
DROP TABLE [dbo].[SERIE]
GO
/****** Object:  Table [dbo].[DIRECCION_OFICINA]    Script Date: 20/11/2018 7:01:28 p. m. ******/
DROP TABLE [dbo].[DIRECCION_OFICINA]
GO
/****** Object:  Table [dbo].[DIRECCION_OFICINA]    Script Date: 20/11/2018 7:01:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DIRECCION_OFICINA](
	[Id_Direccion_Oficina] [numeric](18, 0) NOT NULL,
	[Descripcion_Oficina] [varchar](62) NULL,
	[Activo] [varchar](10) NULL,
	[Id_Nivel] [numeric](18, 0) NULL,
	[Cod_Oficina] [varchar](7) NULL,
 CONSTRAINT [PK_DIRECCION_OFICINA] PRIMARY KEY CLUSTERED 
(
	[Id_Direccion_Oficina] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SERIE]    Script Date: 20/11/2018 7:01:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SERIE](
	[Id_Serie] [numeric](18, 0) NOT NULL,
	[Id_Direccion_Oficina] [numeric](18, 0) NULL,
	[Codigo_Serie] [varchar](50) NOT NULL,
	[Nombre_Serie] [varchar](100) NOT NULL,
	[Activo] [varchar](10) NOT NULL,
 CONSTRAINT [PK_SERIE] PRIMARY KEY CLUSTERED 
(
	[Id_Serie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SUBSERIE]    Script Date: 20/11/2018 7:01:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SUBSERIE](
	[Id_Subserie] [numeric](18, 0) NOT NULL,
	[Id_Serie] [numeric](18, 0) NOT NULL,
	[Codigo_Subserie] [varchar](50) NOT NULL,
	[Nombre_Subserie] [varchar](64) NOT NULL,
	[Activo] [varchar](10) NOT NULL,
 CONSTRAINT [PK_SUBSERIE] PRIMARY KEY CLUSTERED 
(
	[Id_Subserie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TIPO_DOCUMENTAL]    Script Date: 20/11/2018 7:01:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TIPO_DOCUMENTAL](
	[Id_Tipo_Documental] [numeric](18, 0) NOT NULL,
	[Codigo] [varchar](50) NULL,
	[Nombre] [varchar](250) NOT NULL,
	[Id_Serie] [numeric](18, 0) NOT NULL,
	[Id_Subserie] [numeric](18, 0) NULL,
 CONSTRAINT [PK_TIPO_DOCUMENTAL] PRIMARY KEY CLUSTERED 
(
	[Id_Tipo_Documental] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[DIRECCION_OFICINA] ([Id_Direccion_Oficina], [Descripcion_Oficina], [Activo], [Id_Nivel], [Cod_Oficina]) VALUES (CAST(81 AS Numeric(18, 0)), N'SUBDIRECCION DE COBRANZAS', N'1', NULL, N'1530')
INSERT [dbo].[SERIE] ([Id_Serie], [Id_Direccion_Oficina], [Codigo_Serie], [Nombre_Serie], [Activo]) VALUES (CAST(687 AS Numeric(18, 0)), CAST(81 AS Numeric(18, 0)), N'1530.04', N'ACTOS ADMINISTRATIVOS', N'1')
INSERT [dbo].[SERIE] ([Id_Serie], [Id_Direccion_Oficina], [Codigo_Serie], [Nombre_Serie], [Activo]) VALUES (CAST(688 AS Numeric(18, 0)), CAST(81 AS Numeric(18, 0)), N'1530.36', N'INFORMES', N'1')
INSERT [dbo].[SERIE] ([Id_Serie], [Id_Direccion_Oficina], [Codigo_Serie], [Nombre_Serie], [Activo]) VALUES (CAST(689 AS Numeric(18, 0)), CAST(81 AS Numeric(18, 0)), N'1530.44', N'ESPEDIENTES DE COBRO', N'1')
INSERT [dbo].[SERIE] ([Id_Serie], [Id_Direccion_Oficina], [Codigo_Serie], [Nombre_Serie], [Activo]) VALUES (CAST(690 AS Numeric(18, 0)), CAST(81 AS Numeric(18, 0)), N'1530.03', N'ACTAS', N'1')
INSERT [dbo].[SERIE] ([Id_Serie], [Id_Direccion_Oficina], [Codigo_Serie], [Nombre_Serie], [Activo]) VALUES (CAST(755 AS Numeric(18, 0)), CAST(81 AS Numeric(18, 0)), N'1530.23', N'CONSECUTIVO ACTAS DE REPARTO', N'1')
INSERT [dbo].[SUBSERIE] ([Id_Subserie], [Id_Serie], [Codigo_Subserie], [Nombre_Subserie], [Activo]) VALUES (CAST(350 AS Numeric(18, 0)), CAST(687 AS Numeric(18, 0)), N'1530.04.3', N'RESOLUCIONES', N'1')
INSERT [dbo].[SUBSERIE] ([Id_Subserie], [Id_Serie], [Codigo_Subserie], [Nombre_Subserie], [Activo]) VALUES (CAST(351 AS Numeric(18, 0)), CAST(690 AS Numeric(18, 0)), N'1530.03.11', N'ACTAS DE REUNION', N'1')
INSERT [dbo].[SUBSERIE] ([Id_Subserie], [Id_Serie], [Codigo_Subserie], [Nombre_Subserie], [Activo]) VALUES (CAST(390 AS Numeric(18, 0)), CAST(688 AS Numeric(18, 0)), N'1530.36.1', N'INFORMES ENTIDADES DE CONTROL Y VIGILANCIA', N'1')
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10625 AS Numeric(18, 0)), N'618', N'RESOLUCION', CAST(687 AS Numeric(18, 0)), CAST(350 AS Numeric(18, 0)))
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10629 AS Numeric(18, 0)), N'694', N'TITULO EJECUTIVO', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10630 AS Numeric(18, 0)), N'498', N'OFICIOS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10631 AS Numeric(18, 0)), N'029', N'ACTAS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10632 AS Numeric(18, 0)), N'188', N'CONTROL DE LLAMADAS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10633 AS Numeric(18, 0)), N'483', N'OFICIO DE COBRO PERSUASIVO', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10634 AS Numeric(18, 0)), N'4301', N'GUIA COMPRABANTE DE ENTREGA CORRESPONDENCIA', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10635 AS Numeric(18, 0)), N'387', N'INFORMACION DE BIENES', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10636 AS Numeric(18, 0)), N'465', N'MEDIDAS CAUTELARES', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10637 AS Numeric(18, 0)), N'2601', N'AUTOS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10638 AS Numeric(18, 0)), N'031', N'ACTO ADMINISTRATIVO', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10639 AS Numeric(18, 0)), N'479', N'NOTIFICACIONES', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10640 AS Numeric(18, 0)), N'560', N'RECURSO (CUANDO APLIQUE)', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10641 AS Numeric(18, 0)), N'157', N'COMUNICACION OFICIAL (CUANDO APLIQUE)', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10642 AS Numeric(18, 0)), N'143', N'COMUNICACION DEL DEUDOR INFORMANDO INTERPOSICION DE LA DEMANDA ANTE LA JURISDICCION CONTENCIOSA', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10643 AS Numeric(18, 0)), N'3115', N'COMUNICACION INTERNA', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10644 AS Numeric(18, 0)), N'357', N'FALLOS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10645 AS Numeric(18, 0)), N'427', N'INFORMES', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10646 AS Numeric(18, 0)), N'652', N'SOLICITUD DE FACILIDAD DE PAGO, ANEXOS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10647 AS Numeric(18, 0)), N'277', N'DERECHO DE PETICION Y RESPUESTA A DERECHOS DE PETICION', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10648 AS Numeric(18, 0)), N'703', N'ACTO ADMINISTRATIVO DE ENTIDADES DE CONTROL', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10649 AS Numeric(18, 0)), N'482', N'OFICIO DE OBJECIONES Y RESPUESTA', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10650 AS Numeric(18, 0)), N'704', N'NOTIFICACIONES EXTERNAS', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10651 AS Numeric(18, 0)), N'419', N'INFORME O RENDICION FINAL DE CUENTAS DEL LIQUIDADOR', CAST(689 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(10652 AS Numeric(18, 0)), N'029', N'ACTAS', CAST(690 AS Numeric(18, 0)), CAST(351 AS Numeric(18, 0)))
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(11134 AS Numeric(18, 0)), N'029', N'ACTAS', CAST(755 AS Numeric(18, 0)), NULL)
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(11211 AS Numeric(18, 0)), N'413', N'INFORME ENTE DE CONTROL Y VIGILANCIA', CAST(688 AS Numeric(18, 0)), CAST(390 AS Numeric(18, 0)))
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(11212 AS Numeric(18, 0)), N'617', N'REQUERIMIENTO Y SOLICITUD ENTES DE CONTROL Y VIGILANCIA', CAST(688 AS Numeric(18, 0)), CAST(390 AS Numeric(18, 0)))
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(11213 AS Numeric(18, 0)), N'632', N'RESPUESTA AL REQUERIMIENTO Y SOLICITUD ENTES DE CONTROL Y VIGILANCIA', CAST(688 AS Numeric(18, 0)), CAST(390 AS Numeric(18, 0)))
INSERT [dbo].[TIPO_DOCUMENTAL] ([Id_Tipo_Documental], [Codigo], [Nombre], [Id_Serie], [Id_Subserie]) VALUES (CAST(11413 AS Numeric(18, 0)), N'4603', N'BENEFICIOS TRIBUTARIOS', CAST(689 AS Numeric(18, 0)), NULL)
ALTER TABLE [dbo].[SERIE]  WITH CHECK ADD  CONSTRAINT [FK_SERIE_DIRECCION_OFICINA] FOREIGN KEY([Id_Direccion_Oficina])
REFERENCES [dbo].[DIRECCION_OFICINA] ([Id_Direccion_Oficina])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SERIE] CHECK CONSTRAINT [FK_SERIE_DIRECCION_OFICINA]
GO
ALTER TABLE [dbo].[SUBSERIE]  WITH CHECK ADD  CONSTRAINT [FK_SUBSERIE_SERIE] FOREIGN KEY([Id_Serie])
REFERENCES [dbo].[SERIE] ([Id_Serie])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SUBSERIE] CHECK CONSTRAINT [FK_SUBSERIE_SERIE]
GO
ALTER TABLE [dbo].[TIPO_DOCUMENTAL]  WITH CHECK ADD  CONSTRAINT [FK_TIPO_DOCUMENTAL_SERIE] FOREIGN KEY([Id_Serie])
REFERENCES [dbo].[SERIE] ([Id_Serie])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TIPO_DOCUMENTAL] CHECK CONSTRAINT [FK_TIPO_DOCUMENTAL_SERIE]
GO
ALTER TABLE [dbo].[TIPO_DOCUMENTAL]  WITH CHECK ADD  CONSTRAINT [FK_TIPO_DOCUMENTAL_SUBSERIE] FOREIGN KEY([Id_Subserie])
REFERENCES [dbo].[SUBSERIE] ([Id_Subserie])
GO
ALTER TABLE [dbo].[TIPO_DOCUMENTAL] CHECK CONSTRAINT [FK_TIPO_DOCUMENTAL_SUBSERIE]
GO


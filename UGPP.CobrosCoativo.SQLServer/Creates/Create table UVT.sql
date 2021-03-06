/****** Object:  Table [dbo].[UVT]    Script Date: 6/12/2018 3:08:51 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UVT](
	[IdUVT] [int] IDENTITY(1,1) NOT NULL,
	[Anio] [int] NULL,
	[Valor] [decimal](18, 0) NULL,
	[Activo] [bit] NULL,
 CONSTRAINT [PK_UVT] PRIMARY KEY CLUSTERED 
(
	[IdUVT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[UVT] ON 

GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (1, 2006, CAST(20000 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (2, 2007, CAST(20974 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (3, 2008, CAST(22054 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (4, 2009, CAST(23763 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (5, 2010, CAST(24555 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (6, 2011, CAST(25132 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (7, 2012, CAST(26049 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (8, 2013, CAST(26841 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (9, 2014, CAST(27485 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (10, 2015, CAST(28279 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (11, 2016, CAST(29753 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (12, 2017, CAST(31859 AS Decimal(18, 0)), 0)
GO
INSERT [dbo].[UVT] ([IdUVT], [Anio], [Valor], [Activo]) VALUES (13, 2018, CAST(33156 AS Decimal(18, 0)), 1)
GO
SET IDENTITY_INSERT [dbo].[UVT] OFF
GO

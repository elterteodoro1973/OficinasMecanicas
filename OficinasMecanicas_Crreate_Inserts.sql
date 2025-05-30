USE [OficinasMecanicas]
GO
/****** Object:  Table [dbo].[AgendamentoVisita]    Script Date: 30/05/2025 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgendamentoVisita](
	[Id] [uniqueidentifier] NOT NULL,
	[IdUsuario] [uniqueidentifier] NULL,
	[DataHora] [datetime] NULL,
	[Descricao] [varchar](256) NULL,
	[IdOficina] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AgendamentosVisita] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OficinaMecanica]    Script Date: 30/05/2025 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OficinaMecanica](
	[Id] [uniqueidentifier] NOT NULL,
	[Nome] [varchar](256) NULL,
	[Endereco] [varchar](256) NULL,
	[Servicos] [varchar](max) NULL,
 CONSTRAINT [PK_OficinaMecanica] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResetarSenha]    Script Date: 30/05/2025 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResetarSenha](
	[Id] [uniqueidentifier] NOT NULL,
	[Token] [varchar](150) NULL,
	[UsuarioId] [uniqueidentifier] NOT NULL,
	[DataSolicitacao] [datetime] NOT NULL,
	[DataExpiracao] [datetime] NOT NULL,
	[Efetivado] [bit] NULL,
	[Excluido] [bit] NULL,
 CONSTRAINT [PK_ResetarSenha] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServicosPrestados]    Script Date: 30/05/2025 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicosPrestados](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](256) NULL,
 CONSTRAINT [PK_Servico] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 30/05/2025 10:02:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [varchar](100) NULL,
	[PasswordHash] [varchar](512) NULL,
	[Email] [varchar](256) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AgendamentoVisita] ([Id], [IdUsuario], [DataHora], [Descricao], [IdOficina]) VALUES (N'47e33c60-f5a9-474b-a9b4-009c838414f5', N'de944d54-a53d-4038-b93c-3fba7b48b965', CAST(N'2025-06-12T08:10:40.720' AS DateTime), N'Instalação de acessórios', N'7abc9d16-f969-41e9-9e33-a7f588cae46a')
GO
INSERT [dbo].[AgendamentoVisita] ([Id], [IdUsuario], [DataHora], [Descricao], [IdOficina]) VALUES (N'716bcb5c-1043-476b-9380-c3c1938d3545', N'00e3bef2-7f49-4f55-b254-64ff70662fe0', CAST(N'2025-06-19T07:10:40.720' AS DateTime), N'Instalação de acessórios', N'0a581d3f-77a4-46b3-87b1-2fcc500c973d')
GO
INSERT [dbo].[AgendamentoVisita] ([Id], [IdUsuario], [DataHora], [Descricao], [IdOficina]) VALUES (N'31687b0b-2f1a-4306-900e-d26f61d0c8fc', N'70489661-c262-4958-aa07-90a923ec63c3', CAST(N'2025-06-10T08:10:40.720' AS DateTime), N'Instalação de acessórios', N'7abc9d16-f969-41e9-9e33-a7f588cae46a')
GO
INSERT [dbo].[AgendamentoVisita] ([Id], [IdUsuario], [DataHora], [Descricao], [IdOficina]) VALUES (N'024af15c-ade0-436e-a158-d80a22e0131b', N'de944d54-a53d-4038-b93c-3fba7b48b965', CAST(N'2025-07-04T18:43:24.230' AS DateTime), N'Alinhamento das valvulas', N'7abc9d16-f969-41e9-9e33-a7f588cae46a')
GO
INSERT [dbo].[AgendamentoVisita] ([Id], [IdUsuario], [DataHora], [Descricao], [IdOficina]) VALUES (N'a9e3256b-1f91-486a-8b7c-e0e1351cdae8', N'7060aa77-99a8-4979-a4ec-7db063ecfcb5', CAST(N'2025-06-11T07:10:40.720' AS DateTime), N'Instalação de acessórios', N'0a581d3f-77a4-46b3-87b1-2fcc500c973d')
GO
INSERT [dbo].[OficinaMecanica] ([Id], [Nome], [Endereco], [Servicos]) VALUES (N'0a581d3f-77a4-46b3-87b1-2fcc500c973d', N'Hanbai Motos Honda', N'Avenida Abilio Machado, 2635, 30830433, Belo Horizonte', N'Substituição de alternador,Substituição de pastilhas,Funilaria e pintura')
GO
INSERT [dbo].[OficinaMecanica] ([Id], [Nome], [Endereco], [Servicos]) VALUES (N'7abc9d16-f969-41e9-9e33-a7f588cae46a', N'Omegha Hit Velox', N'Rua Belem, 904, 30285010, Belo Horizonte', N'Substituição de alternador,Instalação de acessórios,Funilaria e pintura')
GO
INSERT [dbo].[OficinaMecanica] ([Id], [Nome], [Endereco], [Servicos]) VALUES (N'f555a5d3-8c86-435d-be64-b78bdfe83086', N'Oficina Brava', N'Rua São Paulo 1400', N',Alinhamento de rodas,One,Higienização e conserto de ar-condicionado,Inspeção de correias e tensionadores')
GO
INSERT [dbo].[OficinaMecanica] ([Id], [Nome], [Endereco], [Servicos]) VALUES (N'43a37a67-bac2-4e0e-ad91-d89b042c8319', N'Wfe Auto Center', N'Rua Senador Lima Guimaraes, 231, 30455600, Belo Horizonte', N'Alinhamento de rodas,Substituição de pastilhas,Funilaria e pintura')
GO
INSERT [dbo].[OficinaMecanica] ([Id], [Nome], [Endereco], [Servicos]) VALUES (N'6e57344c-a9bc-4b5f-8c10-f3dd590bfaef', N'Quick Reparacao Automotiva', N'Avenida Cristiano Machado, 10045, 31741465, Belo Horizonte', N'Reparação de amassados e arranhões,Substituição de pastilhas,Funilaria e pintura')
GO
INSERT [dbo].[ResetarSenha] ([Id], [Token], [UsuarioId], [DataSolicitacao], [DataExpiracao], [Efetivado], [Excluido]) VALUES (N'a6a21ec2-7777-4c2e-b74c-2ae859cfa47f', N'FZrsWhDC4CuQ4CbIi0uro2T5jG4B7awL2Qz2CltyCnfWAFItLYCzIzi1W0Lp5grAEkGxTcmbd6op0aLmCerXp8WKdNca', N'70489661-c262-4958-aa07-90a923ec63c3', CAST(N'2025-05-27T17:11:05.227' AS DateTime), CAST(N'2025-05-28T17:11:05.227' AS DateTime), 0, 0)
GO
INSERT [dbo].[ResetarSenha] ([Id], [Token], [UsuarioId], [DataSolicitacao], [DataExpiracao], [Efetivado], [Excluido]) VALUES (N'da6e0831-f160-4062-bee4-36c835a9c9aa', N'PHrWddtCsCRPBpZImp5gRlRVzhJAnfxzIbPesFczpG5Q3OU5chbKntzvnnr1ffFH7kObBUw7D9vCbHGTlV9IWkbC7mJv', N'de944d54-a53d-4038-b93c-3fba7b48b965', CAST(N'2025-05-29T07:07:53.423' AS DateTime), CAST(N'2025-05-29T11:07:53.423' AS DateTime), 0, 0)
GO
INSERT [dbo].[ResetarSenha] ([Id], [Token], [UsuarioId], [DataSolicitacao], [DataExpiracao], [Efetivado], [Excluido]) VALUES (N'c9cc0765-b623-429a-9507-4baed5376690', N'lAy9M9oC1l3KM61cCfIfJZdHUhw1jAEx6GLoZVMyMCjNK9KVhQwJehkRIGnFV9HU2A6tHDjURAQH6eDR4KgXK90zpLUa', N'4b2872ee-3217-4a76-85c4-c5864bdd0016', CAST(N'2025-05-26T21:39:01.247' AS DateTime), CAST(N'2025-05-27T21:39:01.250' AS DateTime), 0, 0)
GO
INSERT [dbo].[ResetarSenha] ([Id], [Token], [UsuarioId], [DataSolicitacao], [DataExpiracao], [Efetivado], [Excluido]) VALUES (N'708c092f-beba-4c71-a50e-5d361ff18e0b', N'x3RhQF3C4NPcsP3F2a5O2L613CgfryCPpHH45YuQfp0geJ2g2sqavcwQhlvACAr42CYWHu6BK0Kywj27DyRokQw1Aooa', N'105a3cab-1f57-49e8-841f-f94a4d2c21a6', CAST(N'2025-05-27T17:27:37.700' AS DateTime), CAST(N'2025-05-28T17:27:37.700' AS DateTime), 0, 0)
GO
INSERT [dbo].[ResetarSenha] ([Id], [Token], [UsuarioId], [DataSolicitacao], [DataExpiracao], [Efetivado], [Excluido]) VALUES (N'c7c2cb20-ae1b-4512-b0f7-f3f6eb401b66', N'LpU0VUfRpcLGE6laqVJzq2QghoobeNjjNE9VVmCt99zZH5n5PrVOhlJod2GTNTAN7Qofl2SCS6cRqjFZ4wTDritQvDTEwgaa', N'00e3bef2-7f49-4f55-b254-64ff70662fe0', CAST(N'2025-05-28T03:08:50.553' AS DateTime), CAST(N'2025-05-29T03:08:50.553' AS DateTime), 0, 0)
GO
SET IDENTITY_INSERT [dbo].[ServicosPrestados] ON 
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (1, N'Alinhamento de rodas')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (2, N'Balanceamento de pneus e rodas')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (3, N'Diagnóstico com scanners automotivos')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (4, N'Diagnóstico e reparação de problemas elétricos')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (5, N'Diagnóstico por Computador')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (6, N'Emissão de certificados de inspeção')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (7, N'Funilaria e pintura')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (8, N'Higienização e conserto de ar-condicionado')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (9, N'Inspeção de correias e tensionadores')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (10, N'Inspeção veicular')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (11, N'Instalação de acessórios')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (12, N'Instalação de kits de elevação/rebaixamento1')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (13, N'Instalação de sistemas de som e multimídia')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (14, N'Instalação e reparação de sistemas de iluminação')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (15, N'Leitura e redefinição de códigos de erro (check engine)')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (16, N'Manutenção e recarga de sistemas de ar-condicionado')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (17, N'Manutenção Pneus, rodas e afins')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (18, N'Personalização de rodas e pneus')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (19, N'Reparação de amassados e arranhões')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (20, N'Reparação de motor e transmissão')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (21, N'Reparação de sistemas de arrefecimento')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (22, N'Reparação de sistemas de direção')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (23, N'Reparação de sistemas de freio')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (24, N'Reparação de sistemas de suspensão e direção')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (25, N'Revisão de sistemas elétricos')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (26, N'Revisão de sistemas eletrônicos')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (27, N'Revisão Geral')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (28, N'Sangria do sistema de freio')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (29, N'Soldagem de partes estruturais')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (30, N'Soldagem e Reparação de Carroceria')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (31, N'Substituição de alternador')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (32, N'Substituição de amortecedores e molas')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (33, N'Substituição de bomba d’água')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (34, N'Substituição de embreagem')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (35, N'Substituição de pastilhas')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (36, N'Substituição de velas de ignição e cabos')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (37, N'Troca de discos de freio')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (38, N'Troca de fluidos da transmissão')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (39, N'Troca de fluidos de arrefecimento')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (40, N'Troca de fluidos de freio')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (41, N'Troca de óleo e filtro')
GO
INSERT [dbo].[ServicosPrestados] ([Id], [Nome]) VALUES (42, N'Troca de pastilhas e discos de freio')
GO
SET IDENTITY_INSERT [dbo].[ServicosPrestados] OFF
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'de944d54-a53d-4038-b93c-3fba7b48b965', N'Weber Bstista', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'elter.teodoro@bol.com.br')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'00e3bef2-7f49-4f55-b254-64ff70662fe0', N'Fernando Otavio', N'pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=', N'elterx.teodoro@bol.com.br')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'd4fcff7c-380e-4990-a2b5-784f70015172', N'Admin', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'admin@hinova.com.br')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'7060aa77-99a8-4979-a4ec-7db063ecfcb5', N'Sergio Garcia', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'stnfdls@yahoo.com.br')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'70489661-c262-4958-aa07-90a923ec63c3', N'Zeli aCosta', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'fredx@bol.com.br')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'4b2872ee-3217-4a76-85c4-c5864bdd0016', N'Elter Teodoro', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'elters.teodoro@gmail.com')
GO
INSERT [dbo].[Usuarios] ([Id], [Username], [PasswordHash], [Email]) VALUES (N'105a3cab-1f57-49e8-841f-f94a4d2c21a6', N'Paulo Cesar Moura', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'puliox@bol.com.br')
GO
ALTER TABLE [dbo].[OficinaMecanica] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ResetarSenha] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ResetarSenha] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Efetivado]
GO
ALTER TABLE [dbo].[ResetarSenha] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Excluido]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[AgendamentoVisita]  WITH CHECK ADD  CONSTRAINT [FK_AgendamentoVisita_OficinaMecanica] FOREIGN KEY([IdOficina])
REFERENCES [dbo].[OficinaMecanica] ([Id])
GO
ALTER TABLE [dbo].[AgendamentoVisita] CHECK CONSTRAINT [FK_AgendamentoVisita_OficinaMecanica]
GO
ALTER TABLE [dbo].[AgendamentoVisita]  WITH CHECK ADD  CONSTRAINT [FK_AgendamentoVisita_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[AgendamentoVisita] CHECK CONSTRAINT [FK_AgendamentoVisita_Usuarios]
GO
ALTER TABLE [dbo].[ResetarSenha]  WITH CHECK ADD  CONSTRAINT [FK_ResetarSenha_Usuarios] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[ResetarSenha] CHECK CONSTRAINT [FK_ResetarSenha_Usuarios]
GO

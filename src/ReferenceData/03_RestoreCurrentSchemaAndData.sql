/****** Object:  Table [dbo].[AmendCodes]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmendCodes](
	[AmendCode] [varchar](2) NOT NULL,
	[AmendCodeDescription] [varchar](50) NULL,
 CONSTRAINT [PK_AmendCodes] PRIMARY KEY CLUSTERED 
(
	[AmendCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AwardingBodies]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AwardingBodies](
	[AwardingBodyID] [int] NOT NULL,
	[AwardingBodyNumber] [varchar](10) NOT NULL,
	[AwardingBodyCode] [varchar](30) NOT NULL,
	[AwardingBodyName] [varchar](200) NULL,
	[Was_Other] [varchar](30) NULL,
	[DoesGradedExams] [bit] NOT NULL,
 CONSTRAINT [PK_AwardingBodies] PRIMARY KEY CLUSTERED 
(
	[AwardingBodyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ethnicities]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ethnicities](
	[EthnicityCode] [varchar](4) NOT NULL,
	[EthnicityDescription] [varchar](100) NOT NULL,
	[ParentEthnicityCode] [varchar](4) NULL,
 CONSTRAINT [PK_Ethnicities] PRIMARY KEY CLUSTERED 
(
	[EthnicityCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InclusionAdjustmentReasons]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InclusionAdjustmentReasons](
	[IncAdjReasonID] [smallint] NOT NULL,
	[IncAdjReasonDescription] [varchar](150) NOT NULL,
	[InJuneChecking] [bit] NOT NULL,
	[CanCancel] [bit] NOT NULL,
	[IsInclusion] [bit] NOT NULL,
	[IsNewStudentReason] [bit] NOT NULL,
	[ListOrder] [int] NULL,
 CONSTRAINT [PK_InclusionAdjustmentReasons] PRIMARY KEY CLUSTERED 
(
	[IncAdjReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[LanguageCode] [varchar](5) NOT NULL,
	[LanguageDescription] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[LanguageCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PINCLInclusionAdjData]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PINCLInclusionAdjData](
	[PINCLInclusionAdjustments_P_INCL] [char](3) NOT NULL,
	[PINCLInclusionAdjustments_IncAdjReasonID] [smallint] NOT NULL,
	[Prompts_PromptID] [smallint] NOT NULL,
 CONSTRAINT [PK_PINCLInclusionAdjData] PRIMARY KEY CLUSTERED 
(
	[PINCLInclusionAdjustments_P_INCL] ASC,
	[PINCLInclusionAdjustments_IncAdjReasonID] ASC,
	[Prompts_PromptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PINCLInclusionAdjustments]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PINCLInclusionAdjustments](
	[P_INCL] [char](3) NOT NULL,
	[IncAdjReasonID] [smallint] NOT NULL,
 CONSTRAINT [PK_PINCLInclusionAdjustments] PRIMARY KEY CLUSTERED 
(
	[P_INCL] ASC,
	[IncAdjReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PINCLs]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PINCLs](
	[P_INCL] [char](3) NOT NULL,
	[P_INCLDescription] [nvarchar](1000) NOT NULL,
	[DisplayFlag] [nchar](1) NULL,
 CONSTRAINT [PK_PINCLs] PRIMARY KEY CLUSTERED 
(
	[P_INCL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PromptResponses]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromptResponses](
	[PromptID] [smallint] NOT NULL,
	[ListOrder] [smallint] NOT NULL,
	[ListValue] [varchar](70) NOT NULL,
	[Rejected] [bit] NOT NULL,
 CONSTRAINT [PK_PromptResponses] PRIMARY KEY CLUSTERED 
(
	[PromptID] ASC,
	[ListOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prompts]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prompts](
	[PromptID] [smallint] NOT NULL,
	[PromptText] [varchar](1000) NOT NULL,
	[IsConditional] [bit] NOT NULL,
	[AllowNulls] [bit] NOT NULL,
	[PromptTypes_PromptTypeID] [smallint] NOT NULL,
	[ColumnName] [varchar](20) NULL,
	[PromptShortText] [varchar](1000) NULL,
 CONSTRAINT [PK_Prompts] PRIMARY KEY CLUSTERED 
(
	[PromptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PromptTypes]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromptTypes](
	[PromptTypeID] [smallint] NOT NULL,
	[PromptTypeName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PromptTypes] PRIMARY KEY CLUSTERED 
(
	[PromptTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SENStatus]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SENStatus](
	[SENStatusCode] [char](1) NOT NULL,
	[SENStatusDescription] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SENStatus] PRIMARY KEY CLUSTERED 
(
	[SENStatusCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YearGroups]    Script Date: 27/01/2021 12:29:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YearGroups](
	[YearGroupCode] [char](2) NOT NULL,
	[YearGroupDescription] [varchar](50) NOT NULL,
 CONSTRAINT [PK_YearGroups] PRIMARY KEY CLUSTERED 
(
	[YearGroupCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'-', N'')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'A', N'Amend Details')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'CL', N'Move results to merged pupil')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'F', N'')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'IN', N'')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'J', N'Contingency')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'N', N'New pupil')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'NR', N'Not on role')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'TI', N'Transfer In')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'TO', N'Transfer Out')
GO
INSERT [dbo].[AmendCodes] ([AmendCode], [AmendCodeDescription]) VALUES (N'Z', N'Not eligible')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'?', N'Not Supplied', N'100')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AAFR', N'African Asian', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'ABAN', N'Bangladeshi', N'2')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AIND', N'Indian', N'3')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AKAO', N'Kashmiri Other', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AKPA', N'Kashmiri Pakistani', N'11')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AMPK', N'Mirpuri Pakistani', N'11')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'ANEP', N'Nepali', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AOPK', N'Other Pakistani', N'11')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AOTA', N'Other Asian', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'AOTH', N'Any Other Asian Background', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'APKN', N'Pakistani', N'11')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'ASLT', N'Sri Lankan Tamil', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'ASNL', N'Sinhalese', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'ASRO', N'Sri Lankan Other', N'10')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BAFR', N'African', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BANN', N'Angolan', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BAOF', N'Other Black African', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BCON', N'Congolese', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BCRB', N'Caribbean', N'18')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BEUR', N'Black European', N'24')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BGHA', N'Ghanaian', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BNAM', N'Black North American', N'24')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BNGN', N'Nigerian', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BOTB', N'Other Black', N'24')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BOTH', N'Any Other Black Background', N'24')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BSLN', N'Sierra Leonean', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BSOM', N'Somali', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'BSUD', N'Sudanese', N'14')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'CHKC', N'Hong Kong Chinese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'CHNE', N'Chinese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'CMAL', N'Malaysian Chinese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'COCH', N'Other Chinese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'CSNG', N'Singaporean Chinese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'CTWN', N'Taiwanese', N'29')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MABL', N'Asian And Black', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MACH', N'Asian And Chinese', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MAOE', N'Asian And Any Other Ethnic Group', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MBCH', N'Black And Chinese', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MBOE', N'Black And Any Other Ethnic Group', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MCOE', N'Chinese And Any Other Ethnic Group', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MOTH', N'Any Other Mixed Background', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MOTM', N'Other Mixed Background', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWAI', N'White And Indian', N'45')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWAO', N'White And Any Other Asian Background', N'45')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWAP', N'White And Pakistani', N'45')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWAS', N'White And Asian', N'45')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWBA', N'White And Black African', N'46')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWBC', N'White And Black Caribbean', N'47')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWCH', N'White And Chinese', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'MWOE', N'White And Any Other Ethnic Group', N'40')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'NOBT', N'Information Not Obtained (Default)', N'100')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'NONE', N'Supplied as NONE', N'100')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OAFG', N'Afghanistani', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OARA', N'Arab', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OEGY', N'Egyptian', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OFIL', N'Filipino', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OIRN', N'Iranian', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OIRQ', N'Iraqi', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OJPN', N'Japanese', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OKOR', N'Korean', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OKRD', N'Kurdish', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OLAM', N'Latin American', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OLEB', N'Lebanese', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OLIB', N'Libyan', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OMAL', N'Malay', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OMRC', N'Moroccan', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OOEG', N'Other Ethnic Group', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OOTH', N'Any Other Ethnic Group', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OPOL', N'Polynesian', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OTHA', N'Thai', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OVIE', N'Vietnamese', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'OYEM', N'Yemeni', N'66')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'REFU', N'Refused', N'100')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'UNCL', N'Unclassified', N'100')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WALB', N'Albanian', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WBOS', N'Bosnian- Herzegovinian', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WBRI', N'British', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WCOR', N'White Cornish', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WCRO', N'Croatian', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WEEU', N'White Eastern European', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WENG', N'English', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WEUR', N'White European', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WGRC', N'Greek Cypriot', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WGRE', N'Greek/Greek Cypriot', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WGRK', N'Greek', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WIRI', N'Irish', N'83')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WIRT', N'Traveller Of Irish Heritage', N'84')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WITA', N'Italian', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WKOS', N'Kosovan', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WOTH', N'Any Other White Background', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WOTW', N'Other White', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WOWB', N'Other White British', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WPOR', N'Portuguese', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WROM', N'Gypsy/Roma', N'91')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WSCO', N'Scottish', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WSER', N'Serbian', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WTUC', N'Turkish Cypriot', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WTUK', N'Turkish', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WTUR', N'Turkish/Turkish Cypriot', N'87')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WWEL', N'Welsh', N'74')
GO
INSERT [dbo].[Ethnicities] ([EthnicityCode], [EthnicityDescription], [ParentEthnicityCode]) VALUES (N'WWEU', N'White Western European', N'87')
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (1, N'Was pupil on your roll on 15/01/2009', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (2, N'Pupil was never on roll', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (3, N'Cancel add-back', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (4, N'Cancel adjustment for admission following permanent exclusion', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (5, N'Remove pupil completely from all data', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (6, N'Publish pupil at this school', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (7, N'Reinstate the pupil', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (8, N'Admitted from abroad with English not first language', 1, 1, 0, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (9, N'Contingency', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (10, N'Admitted following permanent exclusion from a maintained school', 1, 1, 0, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (11, N'Permanently left England', 1, 1, 0, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (12, N'Deceased', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (13, N'Pupil not at end of Key Stage 4', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (17, N'Pupil at end of Key Stage 4', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (18, N'Left school roll before exams', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (19, N'Other', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (21, N'Add this pupil to the achievement and attainment tables', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (23, N'These results belong to another pupil on the list', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (30, N'Adjustment created implicitly by pupil edit', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (51, N'Pupil was not on roll at census date', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (54, N'Not at end of advanced study', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (55, N'Left before exams', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (56, N'Other', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (57, N'Add this student to the achievement and attainment tables', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (58, N'These results belong to another student on the list', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (59, N'Reinstate the student', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (92, N'Add this unlisted pupil to the achievement and attainment tables', 0, 1, 1, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (94, N'Add this unlisted pupil to the achievement and attainment tables', 0, 1, 1, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (95, N'Add this unlisted student to the achievement and attainment tables', 0, 1, 1, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (212, N'Contingency', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (213, N'Pupil not at end of Key Stage 2 in All Subjects', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (218, N'Left school roll before tests', 1, 1, 0, 1, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (219, N'Other', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (221, N'Add this pupil to the achievement and attainment tables', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (223, N'These results belong to another pupil on the list', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (313, N'Pupil not at end of Key Stage 3 in All Subjects', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (318, N'Left school roll before tests', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (319, N'Other', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (321, N'Add this pupil to the achievement and attainment tables', 1, 1, 1, 0, 0)
GO
INSERT [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID], [IncAdjReasonDescription], [InJuneChecking], [CanCancel], [IsInclusion], [IsNewStudentReason], [ListOrder]) VALUES (323, N'These results belong to another pupil on the list', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'?', N'Not supplied')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'ENB', N'Believed to be English')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'ENG', N'English')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'NOT', N'Not obtained')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'OTB', N'Believed to be other than English')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'OTH', N'Other than English')
GO
INSERT [dbo].[Languages] ([LanguageCode], [LanguageDescription]) VALUES (N'REF', N'Refused')
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 22200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 22210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 22220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 8, 22230)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 212, 21200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 213, 21310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 213, 21320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 213, 21330)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 218, 21801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 218, 21810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 218, 21820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 218, 21830)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 218, 21840)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'201', 219, 21900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'202', 213, 21310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'202', 213, 21320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'202', 213, 21330)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 21600)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 21610)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 21620)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 8, 21630)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 218, 21801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 218, 21810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 218, 21820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 218, 21830)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'299', 218, 21840)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 32200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 32210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 32220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 8, 32230)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 313, 31310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 313, 31320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 313, 31330)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 318, 31801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 318, 31810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 318, 31820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 318, 31830)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 318, 31840)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 319, 31914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'301', 323, 32300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 313, 31310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 313, 31320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 313, 31330)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31400)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31410)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31420)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31430)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31440)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31450)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 31460)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 321, 32110)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'302', 323, 32300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 9, 900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 10, 1001)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 10, 1002)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 13, 1310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 13, 1320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 18, 1801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 18, 1810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 18, 1820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1904)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'401', 19, 1914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'403', 3, 300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1400)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1410)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1420)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1430)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1440)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1450)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1460)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1470)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1480)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'404', 17, 1700)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 10, 1001)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 10, 1002)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 13, 1310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 13, 1320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 18, 1801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 18, 1810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 18, 1820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1904)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'405', 19, 1914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'407', 4, 400)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'407', 5, 500)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'408', 4, 400)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'408', 5, 500)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'409', 4, 400)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'409', 5, 500)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'410', 6, 600)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'411', 7, 700)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'412', 7, 700)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 13, 1310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 13, 1320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 18, 1801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 18, 1810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 18, 1820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1904)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 19, 1914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'421', 23, 2300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 13, 1310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 13, 1320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 18, 1801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 18, 1810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 18, 1820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1904)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 19, 1914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'422', 23, 2300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 804)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 11, 1103)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 12, 1201)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 13, 1310)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 13, 1320)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 18, 1801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 18, 1810)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 18, 1820)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1901)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1902)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1903)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1904)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1906)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1907)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1908)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1909)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1910)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1911)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1912)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1913)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 19, 1914)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'423', 23, 2300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'424', 23, 2300)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'425', 7, 700)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'427', 7, 700)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 10, 1001)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'498', 10, 1002)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 801)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 802)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 803)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 2200)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 2210)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 8, 2220)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 11, 1101)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'499', 11, 1102)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 54, 5410)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 54, 5420)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 55, 5500)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5600)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5610)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5620)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5630)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5640)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'501', 56, 5660)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'502', 57, 5710)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'502', 57, 5720)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'502', 58, 5800)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'503', 57, 5710)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'503', 57, 5720)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'503', 58, 5800)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 54, 5410)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 54, 5420)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 55, 5500)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5600)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5610)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5620)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5630)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5640)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 56, 5660)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'521', 58, 5800)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'522', 57, 5710)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'522', 57, 5720)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'522', 58, 5800)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'523', 59, 5900)
GO
INSERT [dbo].[PINCLInclusionAdjData] ([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID], [Prompts_PromptID]) VALUES (N'524', 59, 5900)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 212)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 213)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 218)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 219)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'201', 223)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'202', 213)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'202', 221)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'202', 223)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'299', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'299', 92)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'299', 218)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 313)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 318)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 319)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'301', 323)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'302', 313)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'302', 321)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'302', 323)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 9)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 10)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 13)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 18)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'401', 19)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'403', 3)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'404', 17)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 10)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 13)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 18)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'405', 19)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'407', 4)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'407', 5)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'408', 4)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'408', 5)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'409', 4)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'409', 5)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'410', 6)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'411', 7)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'412', 7)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 13)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 18)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 19)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'421', 23)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 13)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 18)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 19)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'422', 23)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 12)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 13)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 18)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 19)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'423', 23)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'424', 23)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'425', 7)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'427', 7)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'498', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'498', 10)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'498', 94)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'499', 8)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'499', 11)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'499', 94)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'501', 54)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'501', 55)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'501', 56)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'502', 57)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'502', 58)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'503', 57)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'503', 58)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'521', 54)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'521', 55)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'521', 56)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'521', 58)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'522', 57)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'522', 58)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'523', 59)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'524', 59)
GO
INSERT [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID]) VALUES (N'599', 95)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'201', N'Pupils with current year KS2 attainment for all subjects and at end of Key Stage : Included in KS2 calculations', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'202', N'Pupils with current year KS2 attainment for subset of subjects and NOT at end of Key Stage : Presentation and checking only – excluded from KS2 calculations', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'298', N'Unlisted pupils for a non-Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'299', N'Unlisted pupils for a Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'301', N'Pupil on roll during KS3 test week in 2008 and is at End of KS3 in all subjects Included in Key Stage 3', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'302', N'Pupil on roll during KS3 test week in 2008 but is NOT at End of KS3 in all subjects Excluded from Key Stage 3', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'401', N'Pupil on roll at 16/01/2020 Included in Key Stage 4 (both NOR and results)', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'402', N'Pupil not on roll at 16/01/2020 and omitted from all figures to be published', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'403', N'Pupil not on roll at 16/01/2020 Reached the end of compulsory schooling at your school without being published at end of KS4. Left your roll before 16/01/2020 and now added back. Included in Key Stage 4 (both NOR and results)', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'404', N'Pupil on roll at 16/01/2020 Pupil is not at the end of Key Stage 4 Omitted from Key Stage 4', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'405', N'Pupil on roll at 16/01/2020 Included in Key Stage 4 (both NOR and results)', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'407', N'Pupil on roll at 16/01/2020 Pupil admitted following permanent exclusion from a maintained school. Omitted from NOR and results data.', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'408', N'Pupil on roll at 16/01/2020 Pupil aged 15 admitted following permanent exclusion from a maintained school Omitted from Key Stage 4', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'409', N'Pupil on roll at 16/01/2020 Pupil at end of KS4 admitted following permanent exclusion from a maintained school Included in Key Stage 4 (results only - Omitted from NOR)', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'410', N'Pupil on roll at 16/01/2020 but dual-registered with another school and is published elsewhere', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'411', N'Pupil on roll at 16/01/2020 but admitted from overseas with English not first language. Omitted from all data', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'412', N'Pupil on roll at 16/01/2020 but removed on request from school. Omitted from all data', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'413', N'Year group adjusted to 12. Pupil previously reported as end of Key Stage 4. Omitted from all data.', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'414', N'Year group adjusted to 11. Pupil reported as Year 10 or below last year. Included in key stage 4 NoR and results.', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'421', N'Pupil assumed to be on your roll on 16/01/2020 (please remove if not on roll at that date) Pupil also assumed to be in year 11 (please correct year group if necessary) Included in Key Stage 4 data', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'422', N'Pupil assumed to be on your roll on 16/01/2020 Pupil is estimated not to be in year 11 (please correct year group if necessary) Omitted from Key Stage 4', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'423', N'Pupil assumed to be on your roll on 16/01/2020 (please remove if not on roll then) Estimated to be in year 11 (please correct year group if necessary) Included in Key Stage 4 data', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'424', N'TBC', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'425', N'Pupil not on roll on 16/01/2020 and removed from all performance measures', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'427', N'Pupil on roll at 16/01/2020 but admitted from overseas with poor English. Omitted from all data', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'430', N'Year group adjusted to 12. Pupil previously reported as end of Key Stage 4. Omitted from all data.', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'431', N'Year group adjusted to 11. Pupil reported as Year 10 or below last year. Included in key stage 4 NoR and results.', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'497', N'Unlisted pupils for a FE College with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'498', N'Unlisted pupils for a non-Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'499', N'Unlisted pupils for a Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'501', N'Student included in performance measures for level 3, technical certificates and/or English and maths', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'502', N'Student not included in performance measures for level 3, technical certificates and/or English and maths', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'503', N'Pupil excluded from Performance indicators. On roll at 16/01/2020 but excluded from Performance Tables as not in year 13', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'504', N'Pupil on roll at 16/01/2020. Deferred to year 12 last year and now added back. Included in 16-18 tables (both NOR and results)', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'505', N'Student previously published in performance tables', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'506', N'KS5 Specific removal reasons', N' ')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'521', N'Pupil assumed to be on your roll on 16/01/2020 (please remove if not on roll then) Estimated to be in year 13 (please correct year group if necessary)', N'√')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'522', N'Pupil is excluded because s/he does not appear to be in year 13. If this pupil was on your roll and at the end of 2 years of advanced study, please change the year group to 13 and add the pupil', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'523', N'Pupil removed from Performance Tables - not at end of advanced study', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'524', N'Pupil removed from Performance Tables - other', N'X')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'525', N'Pupil deferred to year 12 last year and now added back. Included in 16-18 tables (both NOR and results) regardless of whether they were on roll at 16/01/2020', N'P')
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'598', N'Unlisted pupils for a non-Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PINCLs] ([P_INCL], [P_INCLDescription], [DisplayFlag]) VALUES (N'599', N'Unlisted pupils for a Plasc school with an active Add Pupil request.', NULL)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 1, N'Afghan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 2, N'Afrikaans', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 3, N'Akan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 4, N'Albanian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 5, N'Amharic', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 6, N'Arabic', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 7, N'Armenian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 8, N'Assyrian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 9, N'Azerbaijani', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 10, N'Azeri', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 11, N'Balochi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 12, N'Bemba', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 13, N'Bengali', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 14, N'Berber', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 15, N'Bicol', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 16, N'Bosnian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 17, N'Bulgarian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 18, N'Burmese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 19, N'Cantonese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 20, N'Cebuano', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 21, N'Chichewa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 22, N'Chinese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 23, N'Creole', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 24, N'Croatian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 25, N'Czech', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 26, N'Danish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 27, N'Dari', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 28, N'Dutch', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 29, N'Edo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 30, N'Efik', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 31, N'English', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 32, N'Eritrean', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 33, N'Ethiopian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 34, N'Ewe', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 35, N'Fante', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 36, N'Fanti', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 37, N'Farsi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 38, N'Fijian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 39, N'Filipino', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 40, N'Finnish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 41, N'Frafra', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 42, N'French', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 43, N'Fulani', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 44, N'Ga', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 45, N'Gambian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 46, N'German', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 47, N'Greek', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 48, N'Gujarati', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 49, N'Hausa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 50, N'Hebrew', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 51, N'Hindi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 52, N'Hindustani', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 53, N'Hilgaynon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 54, N'Hungarian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 55, N'Ibo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 56, N'Igbo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 57, N'Ilocan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 58, N'Ilongo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 59, N'Indonesian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 60, N'Italian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 61, N'Japanese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 62, N'Jegawa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 63, N'Kaonda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 64, N'Kikuyu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 65, N'Kinyarwanda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 66, N'Kisi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 67, N'Kiswahili', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 68, N'Konkani', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 69, N'Korean', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 71, N'Krio', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 72, N'Kurdi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 73, N'Kurdish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 74, N'Latvian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 75, N'Lingala', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 76, N'Lithuanian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 77, N'Lozi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 78, N'Luganda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 79, N'Lunda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 80, N'Luo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 81, N'Lusaga', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 82, N'Luvale', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 83, N'Malay', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 84, N'Malayalam', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 85, N'Mandarin', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 86, N'Mandinka', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 87, N'Mbochi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 88, N'Memon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 89, N'Mende', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 90, N'Mirpuri', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 91, N'Mongolian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 92, N'Ndebele', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 93, N'Nepalese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 94, N'Nigerian', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 95, N'Norwegian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 96, N'Nyanja', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 97, N'Pampango', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 98, N'Pangasinense', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 99, N'Panjabi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 100, N'Pashai', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 101, N'Pashto', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 102, N'Pashtu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 103, N'Patois', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 104, N'Pedi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 105, N'Persian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 106, N'Polish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 107, N'Portugese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 108, N'Portuguese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 109, N'Punjabi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 110, N'Pushto', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 111, N'Pushtu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 112, N'Roma(ny)', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 113, N'Romanian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 114, N'Russian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 115, N'Rwanda', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 116, N'Serbian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 118, N'Sesotho', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 119, N'Setswana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 120, N'Shona', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 121, N'Sinhala', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 122, N'Slovak', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 123, N'Somali', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 124, N'Sotho', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 125, N'Spanish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 126, N'Sri Lankan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 127, N'Sunda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 128, N'Surigaonan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 129, N'Swahili', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 130, N'Swazi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 131, N'Swedish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 132, N'Sylheti', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 133, N'Tagalog', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 134, N'Tamil', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 135, N'Temne', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 136, N'Thai', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 137, N'Tigranya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 138, N'Tigre', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 139, N'Tigrinya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 140, N'Tonga', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 141, N'Tsonga', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 142, N'Tswana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 143, N'Turkish', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 144, N'Turkmen', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 145, N'Twi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 146, N'Ukrainian', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 147, N'Urdu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 148, N'Urhobo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 149, N'Uzbek', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 150, N'Venda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 151, N'Vietnamese', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 152, N'Waray', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 153, N'Welsh', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 154, N'Wolof', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 155, N'Xhosa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 156, N'Yoruba', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 157, N'Zulu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (801, 158, N'Other', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 1, N'Afghanistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 2, N'Albania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 3, N'Algeria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 4, N'Angola', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 5, N'Argentina', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 6, N'Armenia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 7, N'Australia', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 8, N'Austria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 9, N'Azerbaijan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 10, N'Bahamas', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 11, N'Bangladesh', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 12, N'Barbados', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 13, N'Belgium', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 14, N'Bolivia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 15, N'Bosnia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 16, N'Botswana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 17, N'Brazil', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 18, N'Brunei', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 19, N'Bulgaria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 20, N'Burundi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 21, N'Burma', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 22, N'Canada', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 23, N'Cameroon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 24, N'Channel Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 25, N'Chile', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 26, N'China', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 27, N'Colombia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 28, N'Congo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 29, N'Croatia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 30, N'Cuba', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 31, N'Cyprus', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 32, N'Czech Republic', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 33, N'Denmark', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 34, N'Djibouti', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 35, N'Dominican Republic', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 36, N'Egypt', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 37, N'Eire', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 38, N'Ecuador', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 39, N'Eritrea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 40, N'Estonia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 41, N'Ethiopia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 42, N'Finland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 43, N'Fiji', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 44, N'France', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 45, N'Gabon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 46, N'Gambia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 47, N'Georgia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 48, N'Germany', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 49, N'Ghana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 50, N'Greece', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 51, N'Grenada', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 52, N'Guinea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 53, N'Guyana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 54, N'Holland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 55, N'Hong Kong', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 56, N'Hungary', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 57, N'India', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 58, N'Indonesia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 59, N'Iran', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 60, N'Iraq', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 61, N'Ireland', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 62, N'Israel', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 63, N'Italy', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 64, N'Ivory Coast', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 65, N'Jamaica', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 66, N'Japan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 67, N'Jordan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 68, N'Kazakhstan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 69, N'Kenya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 70, N'Korea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 71, N'Kosovo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 72, N'Kurdistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 73, N'Kuwait', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 74, N'Latvia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 75, N'Lebanon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 76, N'Lesotho', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 77, N'Leeward Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 78, N'Liberia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 79, N'Libya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 80, N'Lithuania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 81, N'Macau', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 82, N'Macedonia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 83, N'Malawi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 84, N'Malaysia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 85, N'Martinique', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 86, N'Mauritius', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 87, N'Mexico', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 88, N'Moldova', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 89, N'Mongolia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 90, N'Montenegro', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 91, N'Montserrat', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 92, N'Morocco', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 93, N'Mozambique', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 94, N'Nepal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 95, N'Netherlands', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 96, N'New Zealand', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 97, N'Nigeria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 98, N'Norway', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 99, N'Oman', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 100, N'Pakistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 101, N'Peru', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 102, N'Philippines', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 103, N'Poland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 104, N'Portugal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 105, N'Romania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 106, N'Russia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 107, N'Rwanda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 108, N'Saudi Arabia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 109, N'Scotland', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 110, N'Senegal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 111, N'Serbia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 112, N'Seychelles', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 113, N'Sierra Leone', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 114, N'Singapore', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 115, N'Slovakia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 116, N'Somalia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 117, N'South Africa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 118, N'South Korea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 119, N'Spain', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 120, N'Sri Lanka', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 121, N'St Lucia', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 122, N'Sudan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 123, N'Sweden', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 124, N'Switzerland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 125, N'Syria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 126, N'Taiwan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 127, N'Tanzania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 128, N'Thailand', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 129, N'Tobago', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 130, N'Togo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 131, N'Trinidad', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 132, N'Turkey', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 133, N'United Arab Emirates', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 134, N'Uganda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 135, N'Ukraine', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 136, N'USA', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 137, N'Uzbekistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 138, N'Venezuela', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 139, N'Vietnam', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 140, N'Wales', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 141, N'Windward Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 142, N'Yemen', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 143, N'Yugoslavia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 144, N'Zaire', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 145, N'Zambia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 146, N'Zimbabwe', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (802, 147, N'Other', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 1, N'Afghanistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 2, N'Albania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 3, N'Algeria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 4, N'Angola', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 5, N'Argentina', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 6, N'Armenia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 7, N'Australia', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 8, N'Austria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 9, N'Azerbaijan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 10, N'Bahamas', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 11, N'Bangladesh', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 12, N'Barbados', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 13, N'Belgium', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 14, N'Bolivia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 15, N'Bosnia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 16, N'Botswana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 17, N'Brazil', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 18, N'Brunei', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 19, N'Bulgaria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 20, N'Burundi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 21, N'Burma', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 22, N'Canada', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 23, N'Cameroon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 24, N'Channel Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 25, N'Chile', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 26, N'China', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 27, N'Colombia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 28, N'Congo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 29, N'Croatia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 30, N'Cuba', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 31, N'Cyprus', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 32, N'Czech Republic', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 33, N'Denmark', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 34, N'Djibouti', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 35, N'Dominican Republic', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 36, N'Egypt', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 37, N'Eire', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 38, N'Ecuador', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 39, N'Eritrea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 40, N'Estonia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 41, N'Ethiopia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 42, N'Finland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 43, N'Fiji', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 44, N'France', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 45, N'Gabon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 46, N'Gambia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 47, N'Georgia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 48, N'Germany', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 49, N'Ghana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 50, N'Greece', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 51, N'Grenada', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 52, N'Guinea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 53, N'Guyana', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 54, N'Holland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 55, N'Hong Kong', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 56, N'Hungary', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 57, N'India', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 58, N'Indonesia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 59, N'Iran', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 60, N'Iraq', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 61, N'Ireland', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 62, N'Israel', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 63, N'Italy', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 64, N'Ivory Coast', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 65, N'Jamaica', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 66, N'Japan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 67, N'Jordan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 68, N'Kazakhstan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 69, N'Kenya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 70, N'Korea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 71, N'Kosovo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 72, N'Kurdistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 73, N'Kuwait', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 74, N'Latvia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 75, N'Lebanon', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 76, N'Lesotho', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 77, N'Leeward Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 78, N'Liberia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 79, N'Libya', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 80, N'Lithuania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 81, N'Macau', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 82, N'Macedonia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 83, N'Malawi', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 84, N'Malaysia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 85, N'Martinique', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 86, N'Mauritius', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 87, N'Mexico', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 88, N'Moldova', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 89, N'Mongolia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 90, N'Montenegro', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 91, N'Montserrat', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 92, N'Morocco', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 93, N'Mozambique', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 94, N'Nepal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 95, N'Netherlands', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 96, N'New Zealand', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 97, N'Nigeria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 98, N'Norway', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 99, N'Oman', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 100, N'Pakistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 101, N'Peru', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 102, N'Philippines', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 103, N'Poland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 104, N'Portugal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 105, N'Romania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 106, N'Russia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 107, N'Rwanda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 108, N'Saudi Arabia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 109, N'Scotland', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 110, N'Senegal', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 111, N'Serbia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 112, N'Seychelles', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 113, N'Sierra Leone', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 114, N'Singapore', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 115, N'Slovakia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 116, N'Somalia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 117, N'South Africa', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 118, N'South Korea', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 119, N'Spain', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 120, N'Sri Lanka', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 121, N'St Lucia', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 122, N'Sudan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 123, N'Sweden', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 124, N'Switzerland', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 125, N'Syria', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 126, N'Taiwan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 127, N'Tanzania', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 128, N'Thailand', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 129, N'Tobago', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 130, N'Togo', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 131, N'Trinidad', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 132, N'Turkey', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 133, N'United Arab Emirates', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 134, N'Uganda', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 135, N'Ukraine', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 136, N'USA', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 137, N'Uzbekistan', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 138, N'Venezuela', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 139, N'Vietnam', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 140, N'Wales', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 141, N'Windward Islands', 1)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 142, N'Yemen', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 143, N'Yugoslavia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 144, N'Zaire', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 145, N'Zambia', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 146, N'Zimbabwe', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1101, 147, N'Other', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1901, N'Ilness/pregnancy', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1902, N'Home tuition', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1903, N'EOTAS - Education other than at school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1904, N'Work or work experience', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1905, N'Permanently excluded from this school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1906, N'Persistent non attender, school phobic, untraceable', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1907, N'Repeating year 11', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1908, N'SEN - Special Educational Needs', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1909, N'Dual registered and should be published elsewhere', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1910, N'In young offenders institution, prison, or on remand', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1911, N'Not known at this school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1912, N'Travellers', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1913, N'Contingency for Bird flu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (1900, 1914, N'Other', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5100, 1, N'Yes', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5100, 2, N'No', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5610, N'Deceased', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5620, N'One year course of study', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5630, N'Work based learner', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5640, N'Part-time learner', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5650, N'Contingency for Bird flu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (5600, 5660, N'Other', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 1, N'Ilness/pregnancy', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 2, N'Home tuition', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 3, N'EOTAS - Education other than at school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 5, N'Permanently excluded from this school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 6, N'Persistent non attender, school phobic, untraceable', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 7, N'Repeating year 9', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 8, N'SEN - Special Educational Needs', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 9, N'Dual registered and should be published elsewhere', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 10, N'In young offenders institution, prison, or on remand', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 11, N'Not known at this school', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 12, N'Travellers', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 13, N'Contingency for Bird flu', 0)
GO
INSERT [dbo].[PromptResponses] ([PromptID], [ListOrder], [ListValue], [Rejected]) VALUES (31900, 14, N'Other', 0)
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (100, N'Was pupil on your roll on 15th January 2009??', 0, 0, 5, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (110, N'Please make sure the year group we have estimated is correct: use yeargroup 11 for pupils completing Key Stage 4 in 2009', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (120, N'Enter the other Forvus Index for this pupil. We will merge the results under that Index number', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (130, N'Pupil will be removed from all published data. The NOR figures will not change. If January census NOR totals are incorrect please amend them on your School Summary page.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (200, N'Was pupil ever enrolled at your school?', 0, 0, 5, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (210, N'Age 15 historical data is no longer calculated. No amendment is needed unless DOB is incorrect.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (220, N'Pupil will be removed from all published data', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (300, N'Pupil not on roll at 15/01/2009. Reached the end of compulsory schooling at your school without being published at end of KS4. Left your roll before 15/01/2009 and now added back. This pupil has been added back because they were not published in the KS4 data last year when they completed compulsory schooling. If you wish to appeal this add-back please give your reasons here.', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (400, N'This pupil has been removed from the published Number on Roll but their results are included in the published percentages and CVA. Please explain why you think they should be included in the NOR', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (500, N'This pupil has been removed from the published Number on Roll but their results are included in the published percentages and CVA. Please explain why their results should be removed also', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (600, N'Please explain why this pupil should be published at your school rather than the school where they are dually-registered. Please also ensure that all the details above are correct for YOUR school as they may currently reflect details provided by the other school', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (700, N'This pupil was removed from published data at the school''s request. Please explain why you wish to cancel that removal and reinstate them', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (801, N'Language', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (802, N'Country', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (803, N'Arrival Date', 0, 1, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (804, N'Please provide the country name', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (805, N'Please provide the language name', 1, 0, 4, N'Explanation', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (810, N'Please pick the date where the student joined the roll', 1, 0, 2, NULL, NULL)
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (820, N'Pupil will be removed from Age15 historical data', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (830, N'Please choose the revised admission date if available', 1, 1, 2, NULL, NULL)
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (900, N'Please give full details', 0, 0, 4, N'Explanation', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1001, N'Please specify the excluding school using its 7 digit DCSF number. If you do not know this you can search the <a href="http://www.edubase.gov.uk" target="_blank" Title="Link opens in a new window">www.edubase.gov.uk</a> website – the DCSF number consists of the 3 digit LA number followed by the 4 digit Establishment Number.', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1002, N'Exclusion date', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1010, N'The excluding school must be a recognized maintained school to meet DCSF criteria for ''pupils admitted following permanent exclusion from a maintained school''. Please give further details in support of this request.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1020, N'The exclusion date is after the date of admission to your school. Please explain why you are making this request.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1030, N'Exclusion Date before 1st September 2007 - does not meet DCSF criteria for ''pupils admitted following permanent exclusion from a maintained school''. Please explain why you are making this request', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1040, N'Request to add an unlisted pupil who was admitted following permanent exclusion from a maintained school.  Addition will be reviewed. Please add any missing attainment and send the evidence requested.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1101, N'Country', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1102, N'Off roll date', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1103, N'Please provide the country name', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1110, N'The date off roll is after the start of the tests. Please explain why you are making this request', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1120, N'The date off roll is before the January census but this pupil was recorded on your January census. Please explain your request', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1130, N'Request to add an unlisted pupil who has permanently left England.  Addition will be reviewed.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1201, N'Date off roll', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1210, N'The date off roll is before the January census but this pupil was recorded on your January census. Please explain your request', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1310, N'We are aware this pupil is not at the end of Key Stage 4 and they are NOT included in the KS4 indicators. But they must stay in the Age15 historical figures unless you can give valid grounds for removal', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1320, N'Please revise year group to make this request. Note 11 is correct for pupils at end of KS4 even if your school ends KS4 in year 10 or year 12. If pupil will repeat year 11 in 2010 please enter this as year 10', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1400, N'National Curriculum Year Group', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1410, N'This pupil will be added to the pupils at the end of Key Stage 4 published this year', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1420, N'If accepted, this change will remove the pupil from the published end of Key Stage 4 data', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1430, N'Pupil will be removed from the end of Key Stage 4 published data.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1440, N'Pupil will be removed from the end of Key Stage 4 published data BUT this pupil will be published in the end of Key Stage 4 data for your school in 2010 whether or not they continue their education at your school or elsewhere.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1445, N'Please explain why this pupil is more than two years out of the normal year group for their age.', 1, 0, 4, N'Explanation', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1450, N'DCSF will consider your request', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1455, N'Please explain why this pupil is more than two years out of the normal year group for their age. If accepted, this changes the year we estimated and removes the pupil from the KS4 measures. It does not alter the School Number On Roll total as reported in your January School Census return.', 1, 0, 4, N'Explanation', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1460, N'This changes the year we estimated and removes the pupil from the KS4 measures. It does not alter the School Number On Roll total as reported in your January School Census return.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1470, N'This changes the year we estimated and adds the pupil to the KS4 measures.  It does not alter the School Number On Roll total as reported in your January School Census return.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1480, N'This changes the year group which we estimated.  It does not alter the School Number On Roll total as reported in your January School Census return. Results are only included for year 11 pupils.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1500, N'Date of Birth', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1510, N'DCSF will consider your request', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1520, N'Pupil will no longer be counted as an ''early-taker''', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1530, N'This change may alter the results and number on roll in the Age 15 historical data', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1540, N'This change may alter the results included in the Age 15 historical data. It will not alter the published Number on Roll', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1600, N'Date of admission to school', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1610, N'Please explain why this pupil was recorded on your January school census if they joined your roll after it', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1620, N'Pupil joined after ASC so explain why should be published at your school', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1700, N'Please revise year group to make this request. Use 11 for pupils at end of KS4 even if your school ends KS4 in year 10 or year 12. Note that if pupil will repeat year 11 in 2010 then year group 10 is correct.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1801, N'Please enter the effective date this pupil was removed from your roll (use earlier date if backdated)', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1810, N'Please explain why this pupil was recorded on your January school census if they were removed from your roll before it.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1820, N'All pupils must be published at the school where they were on roll in January. You are entitled to any results obtained after this pupil left your roll.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1900, N'Other', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1901, N'If pupil will complete Key Stage 4 in 2010 you can ask for deferral by amending the NC Year Group to 10. Otherwise please give full details of illness, date last attended and plans for continuing the pupil''s education.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1902, N'Pupils kept on roll by LA for funding or other purposes cannot be removed', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1903, N'Pupils kept on roll by LA for funding or other purposes cannot be removed', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1904, N'Pupils on roll for the January census who leave school to start work or attend a mix of work/study cannot be removed', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1906, N'Please give details of steps taken to ensure attendance', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1907, N'If pupil will complete Key Stage 4 in 2010 you can change the taught year in 2009 to 10 BUT this pupil will be published in the end of Key Stage 4 data for your school in 2010', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1908, N'Special Needs do not offer grounds for removal from published Achievement and Attainment Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1909, N'Please give details of the school where you believe this pupil should be published. DCSF are unlikely to agree this request unless the other school requests the addition of this pupil', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1910, N'Please give outline details and dates of incarceration', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1911, N'The pupil was listed on your January 2009 school census return but may have been shown under a different name. Please check with the registry.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1912, N'DCFS does not generally accept removal', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1913, N'Please give details of problem', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (1914, N'Please give full details', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2000, N'If you feel there are exceptional circumstances regarding this pupil, please enter them here.', 0, 1, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2010, N'We regret that we are unable to remove this pupil without details of the circumstances.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2020, N'Thank you. Your request will be passed to DCSF to decide.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2100, N'Please check that this pupil is not already included under another name and is at end of KS4 in 2009. If you are confident they are not already listed please check/supply all details on this screen', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2200, N'Please enter the date the pupil joined your roll.', 1, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2210, N'Please explain, if possible, how the pupil was omitted from your school census return', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2220, N'Please explain why the pupil should be published at your school if they joined the roll after the school census', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (2300, N'Enter the Forvus Index Number of the pupil to whom these results belong. The results will be merged under that Index.', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5100, N'Was student on your roll on 15/01/2009? (Must answer this question)', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5110, N'Please make sure the year group we have estimated is correct: use year group 13 for students completing Level 3 study in 2009', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5120, N'Enter the other Forvus Index for this pupil. We will merge the results under that Index number', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5130, N'Student will be removed from all published data.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5200, N'National Curriculum Year Group', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5210, N'This student will be added to the published tables. Please add any missing attainment and send the evidence requested.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5220, N'This student will not be included in the post 16 publication.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5300, N'Date of Birth', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5310, N'Student aged 19+ is too old to be published in post 16 tables and will be removed', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5320, N'Student aged under 16 is too young to be published in post 16 tables and will be removed', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5410, N'We are aware this student is not at the end of advanced study and they are NOT included in the post 16 indicators.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5420, N'Please revise year group. Use 12 for students who will complete next year, and 14 for those who completed last year.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5500, N'Please amend each result to ''Withdrawn'' and provide evidence as requested.', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5600, N'Other', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5610, N'Student will be removed from published tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5620, N'Student will be removed from published tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5630, N'Student will be removed from published tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5640, N'Student will be removed from published tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5650, N'Please give full details', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5660, N'Please give full details', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5710, N'This student will be added to the published tables. Please add any missing attainment and send the evidence requested.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5720, N'Student cannot be included in published tables unless you change the taught year group to 13', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5800, N'Enter the Forvus Index Number of the student to whom these results belong. The results will be merged under that Index.', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (5900, N'This student was removed from published data at the school''s request. Please explain why you wish to cancel that removal and reinstate them', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21200, N'Please give full details', 0, 0, 4, N'Explanation', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21310, N'We are aware this pupil is not at the end of Key Stage 2 and they are NOT included in the KS2 Tables. They are included in the checking exercise so that you can check the KS2 results', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21320, N'Please explain why a year [Year Group] pupil should not be treated as being at the end of KS2.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21330, N'Although this pupil is not in Year group 6, they seem to have valid KS2 results for all subjects.  Please explain why this pupil is not at end of KS2', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21400, N'National Curriculum Year Group', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21410, N'Year Group change accepted. This pupil will be added to the pupils at the end of Key Stage 2 published this year. Warning: all KS2 ''Future'' results will be changed to ''Missing'' for publication - please amend these results, and provide evidence.  If this pupil is genuinely in Year Group 6 but is not at the end of KS2, please select option '' Pupil not at end of KS2 in all subjects'' to request removal from the Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21420, N'Year Group change accepted. This pupil will continue to be reported in Key Stage 2 2009 tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21430, N'Year Group change accepted. This pupil will be removed from the pupils at the end of Key Stage 2 published this year.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21440, N'Year Group change accepted. Warning: This pupil  has valid KS2 results for 2009 for one or more subjects. The pupil will NOT be removed from the pupils at the end of Key Stage 2 published this year.  If this year group change indicates that the pupil is not at end of KS2 in 2009, please select removal option ''Pupil not at end of KS2 in all subjects''', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21450, N'Year Group change accepted. This pupil has valid KS2 results for 2009.  They will NOT be removed from the pupils at the end of Key Stage 2 published this year.  If this year group change indicates that the pupil is not at end of KS2 in 2009, please select removal option ''Pupil not at end of KS2 in all subjects''', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21460, N'Year Group change accepted. The pupil will continue to be reported in the KS2 2009 Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21500, N'Date of Birth', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21510, N'Data of birth change accepted. The pupil will continue to be reported in the KS2 2009 Tables', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21520, N'Data of birth change accepted.  The pupil will continue NOT to be reported in the KS2 2009 Tables, as age is not a criteria for inclusion in the Tables', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21600, N'Date of admission to school', 1, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21610, N'Date of admission change accepted. The pupil will continue to be reported in the KS2 2009 Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21620, N'Pupil joined during test week. Request will be reviwed by the DfES', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21630, N'Please explain why this pupil was recorded with KS2 results at your school if they joined your roll after the KS2 testing week', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21801, N'Please enter the effective date this pupil was removed from your roll (use earlier date if backdated)', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21810, N'This pupil will be removed from your KS2 Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21820, N'This pupil will be removed from your KS2 Tables', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21830, N'All pupils must be published at the school where they took the KS2 tests. You are entitled to any results obtained by this pupil', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21840, N'Pupil has not done all 3 tests. Please provide more details for DCSF.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (21900, N'Please give full details', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (22200, N'Please enter the date the pupil joined your roll.', 1, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (22210, N'Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS2 test results.   Please explain, if possible, how the pupil was omitted from your school census return.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (22220, N'Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS2 test results', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (22230, N'Please explain why the pupil should be published at your school if they joined the roll after the KS2 test week. Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS2 test results', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (22300, N'Enter the Forvus Index Number of the pupil to whom these results belong. The results will be merged under that Index.', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31310, N'We are aware this pupil is not at the end of Key Stage 3 and they are NOT included in the KS3 Tables. They are included in the checking exercise so that you can check and amend any tests taken in earlier years.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31320, N'Please explain why a year <Year Group> pupil is not at end of KS3.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31330, N'Although this pupil is not in Year group 9, they seem to have valid KS3 results for all subjects.  Please explain why this pupil is not at end of KS3', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31400, N'National Curriculum Year Group', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31410, N'Year Group change accepted. This pupil will be added to the pupils at the end of Key Stage 3 published this year. Warning: all KS3 ''Future'' results will be changed to ''Missing'' for publication - please amend these results, and provide evidence.  If this pupil is genuinely in Year Group 9 but is not at the end of KS3, please select option '' Pupil not at end of KS3 in all subjects'' to request removal from the Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31420, N'Year Group change accepted. This pupil will continue to be reported in Key Stage 3 2007 tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31430, N'Year Group change accepted. This pupil will be removed from the pupils at the end of Key Stage 3 published this year.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31440, N'Year Group change accepted. This pupil has valid KS3 results for 2007 for all subjects. The pupil will NOT be removed from the pupils at the end of Key Stage 3 published this year.  If this year group change indicates that the pupil is not at end of KS3 in 2007, please select removal option ''Pupil not at end of KS3 in all subjects''', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31450, N'Year Group change accepted. This pupil has valid KS3 results for 2007. They will NOT be removed from the pupils at the end of Key Stage 3 published this year.  If this year group change indicates that the pupil is not at end of KS3 in 2007, please select removal option ''Pupil not at end of KS3 in all subjects''', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31460, N'Year Group change accepted. The pupil will continue to be reported in the KS3 2007 Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31500, N'Date of Birth', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31510, N'Data of birth change accepted. The pupil will continue to be reported in the KS3 2007 Tables', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31520, N'Data of birth change accepted. The pupil will continue NOT to be reported in the KS3 2007 Tables, as age is not a criteria for inclusion in the Tables', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31600, N'Date of admission to school', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31610, N'Date of admission change accepted. The pupil will continue to be reported in the KS3 2007 Tables', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31620, N'Pupil joined during test week. Request will be reviewed by the DfES', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31630, N'Please explain why this pupil was recorded with KS3 results at your school if they joined your roll after the KS3 testing week', 0, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31801, N'Please enter the effective date this pupil was removed from your roll (use earlier date if backdated)', 0, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31810, N'This pupil will be removed from your KS3 Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31820, N'Please explain why this pupil was recorded on your January school census if they were removed from your roll before it.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31830, N'All pupils must be published at the school where they took the KS3 tests. You are entitled to any results obtained by this pupil', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31840, N'Pupil has not done all 3 tests. Please provide more details for DfES.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31900, N'Other', 0, 0, 1, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31901, N'If pupil will complete Key Stage 3 in 2008 you can ask for deferral by picking the option for ''Pupils not at end of Key Stage''. Otherwise please give full details of illness, date last attended and plans for continuing the pupil''s education.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31902, N'Pupils kept on roll by LA for funding or other purposes cannot be removed', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31903, N'Pupils kept on roll by LA for funding or other purposes cannot be removed', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31906, N'Please give details of steps taken to ensure attendance', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31907, N'If pupil will complete Key Stage 3 in 2008 and has ''Future'' results flagged for some tests, you can change the taught year in 2007 to 8.  If the pupil has completed all KS3 tests, then these results will be reported in the 2007 tables.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31908, N'Special Needs do not offer grounds for removal from published Achievement and Attainment Tables', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31909, N'Please give details of the school where you believe this pupil should be published. DfES are unlikly to agree this request unless the other school requests the addition of this pupil', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31910, N'Please give outline details and dates of incarceration', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31911, N'The pupil was listed on your May KS3 Tests return but may have been shown under a different name. Please check with the registry.', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31912, N'DfES does not generally accept removal', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31913, N'Please give details of problem', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (31914, N'Please give full details', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32110, N'This pupil is not in Year Group 9, and is listed as having ''Future'' results, and so is regarded as ''Not at end of KS3''. If the Year Group is in error, please amend to its correct value.  Please also amend any erroneous KS3 results, and send in appropriate evidence.  Otherwise these ''Future'' results will be reported as ''Missing''', 0, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32200, N'Please enter the date the pupil joined your roll.', 1, 0, 2, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32210, N'Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS3 test results.   Please explain, if possible, how the pupil was omitted from your school census return.', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32220, N'Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS3 test results', 1, 0, 6, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32230, N'Please explain why the pupil should be published at your school if they joined the roll after the KS3 test week. Addition will be reviewed.  Please make sure that you provide details and evidence for Pupils KS3 test results', 1, 0, 4, N'', N'')
GO
INSERT [dbo].[Prompts] ([PromptID], [PromptText], [IsConditional], [AllowNulls], [PromptTypes_PromptTypeID], [ColumnName], [PromptShortText]) VALUES (32300, N'Enter the Forvus Index Number of the pupil to whom these results belong. The results will be merged under that Index.', 0, 0, 3, N'', N'')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (1, N'ListBox')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (2, N'Date')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (3, N'Integer')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (4, N'Text')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (5, N'YesNo')
GO
INSERT [dbo].[PromptTypes] ([PromptTypeID], [PromptTypeName]) VALUES (6, N'Info')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'?', N'Not Supplied')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'A', N'School action or early years action')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'E', N'Education, health and care plan')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'K', N'SEN support')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'N', N'No special educational need')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'P', N'School action plus or early years action plus')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'Q', N'School action plus statutory assessment')
GO
INSERT [dbo].[SENStatus] ([SENStatusCode], [SENStatusDescription]) VALUES (N'S', N'Statement')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'? ', N'Not Supplied')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'0 ', N'N/A')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'1 ', N'Year 1')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'10', N'Year 10')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'11', N'Year 11')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'12', N'Year 12')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'13', N'Year 13')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'14', N'Year 14')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'15', N'Year 15')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'16', N'Year 16')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'17', N'Year 17')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'2 ', N'Year 2')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'3 ', N'Year 3')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'4 ', N'Year 4')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'5 ', N'Year 5')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'6 ', N'Year 6')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'7 ', N'Year 7')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'8 ', N'Year 8')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'9 ', N'Year 9')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'99', N'N/A')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'N1', N'Nursery first year')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'N2', N'Nursery second year')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'R ', N'Reception')
GO
INSERT [dbo].[YearGroups] ([YearGroupCode], [YearGroupDescription]) VALUES (N'X ', N'National Curriculum not followed')
GO
ALTER TABLE [dbo].[PINCLInclusionAdjData]  WITH CHECK ADD  CONSTRAINT [FK_PINCLInclusionAdjData_PINCLInclusionAdjustments] FOREIGN KEY([PINCLInclusionAdjustments_P_INCL], [PINCLInclusionAdjustments_IncAdjReasonID])
REFERENCES [dbo].[PINCLInclusionAdjustments] ([P_INCL], [IncAdjReasonID])
GO
ALTER TABLE [dbo].[PINCLInclusionAdjData] CHECK CONSTRAINT [FK_PINCLInclusionAdjData_PINCLInclusionAdjustments]
GO
ALTER TABLE [dbo].[PINCLInclusionAdjData]  WITH CHECK ADD  CONSTRAINT [FK_PINCLInclusionAdjData_Prompts] FOREIGN KEY([Prompts_PromptID])
REFERENCES [dbo].[Prompts] ([PromptID])
GO
ALTER TABLE [dbo].[PINCLInclusionAdjData] CHECK CONSTRAINT [FK_PINCLInclusionAdjData_Prompts]
GO
ALTER TABLE [dbo].[PINCLInclusionAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_PINCLNORAdjustments_NORAdjustmentReasons] FOREIGN KEY([IncAdjReasonID])
REFERENCES [dbo].[InclusionAdjustmentReasons] ([IncAdjReasonID])
GO
ALTER TABLE [dbo].[PINCLInclusionAdjustments] CHECK CONSTRAINT [FK_PINCLNORAdjustments_NORAdjustmentReasons]
GO
ALTER TABLE [dbo].[PINCLInclusionAdjustments]  WITH CHECK ADD  CONSTRAINT [FK_PINCLNORAdjustments_PINCLs] FOREIGN KEY([P_INCL])
REFERENCES [dbo].[PINCLs] ([P_INCL])
GO
ALTER TABLE [dbo].[PINCLInclusionAdjustments] CHECK CONSTRAINT [FK_PINCLNORAdjustments_PINCLs]
GO
ALTER TABLE [dbo].[PromptResponses]  WITH CHECK ADD  CONSTRAINT [FK_PromptResponses_Prompts] FOREIGN KEY([PromptID])
REFERENCES [dbo].[Prompts] ([PromptID])
GO
ALTER TABLE [dbo].[PromptResponses] CHECK CONSTRAINT [FK_PromptResponses_Prompts]
GO
ALTER TABLE [dbo].[Prompts]  WITH CHECK ADD  CONSTRAINT [FK_Prompts_PromptTypes] FOREIGN KEY([PromptTypes_PromptTypeID])
REFERENCES [dbo].[PromptTypes] ([PromptTypeID])
GO
ALTER TABLE [dbo].[Prompts] CHECK CONSTRAINT [FK_Prompts_PromptTypes]
GO

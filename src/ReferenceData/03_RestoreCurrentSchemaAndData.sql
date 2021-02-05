USE [rscd]
GO
/****** Object:  Table [dbo].[AmendCodes]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[AwardingBodies]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[Ethnicities]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[InclusionAdjustmentReasons]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[Languages]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[PINCLInclusionAdjustments]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[PINCLs]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[PotentialAnswers]    Script Date: 05/02/2021 13:50:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PotentialAnswers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [nvarchar](150) NOT NULL,
	[AnswerValue] [nvarchar](150) NOT NULL,
	[Rejected] [bit] NULL,
 CONSTRAINT [PK_PotentialAnswers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SENStatus]    Script Date: 05/02/2021 13:50:33 ******/
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
/****** Object:  Table [dbo].[YearGroups]    Script Date: 05/02/2021 13:50:33 ******/
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
SET IDENTITY_INSERT [dbo].[PotentialAnswers] ON 
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (1, N'PupilNativeLanguageQuestion', N'Acholi', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (2, N'PupilNativeLanguageQuestion', N'Adangme', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (3, N'PupilNativeLanguageQuestion', N'Afar-Saho', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (4, N'PupilNativeLanguageQuestion', N'Afrikaans', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (5, N'PupilNativeLanguageQuestion', N'Akan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (6, N'PupilNativeLanguageQuestion', N'Albanian/Shqip', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (7, N'PupilNativeLanguageQuestion', N'Alur', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (8, N'PupilNativeLanguageQuestion', N'Amharic', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (9, N'PupilNativeLanguageQuestion', N'Anyi-Baule', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (10, N'PupilNativeLanguageQuestion', N'Arabic', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (11, N'PupilNativeLanguageQuestion', N'Armenian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (12, N'PupilNativeLanguageQuestion', N'Assamese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (13, N'PupilNativeLanguageQuestion', N'Assyrian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (14, N'PupilNativeLanguageQuestion', N'Aymara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (15, N'PupilNativeLanguageQuestion', N'Azerbaijani', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (16, N'PupilNativeLanguageQuestion', N'Azeri', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (17, N'PupilNativeLanguageQuestion', N'Balochi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (18, N'PupilNativeLanguageQuestion', N'Balti', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (19, N'PupilNativeLanguageQuestion', N'Bambara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (20, N'PupilNativeLanguageQuestion', N'Bamileke', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (21, N'PupilNativeLanguageQuestion', N'Basque/Euskara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (22, N'PupilNativeLanguageQuestion', N'Beja/Bedawi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (23, N'PupilNativeLanguageQuestion', N'Belarusian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (24, N'PupilNativeLanguageQuestion', N'Bemba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (25, N'PupilNativeLanguageQuestion', N'Bengali/Bangla', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (26, N'PupilNativeLanguageQuestion', N'Berber', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (27, N'PupilNativeLanguageQuestion', N'Bhojpuri', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (28, N'PupilNativeLanguageQuestion', N'Bikol', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (29, N'PupilNativeLanguageQuestion', N'Bilen', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (30, N'PupilNativeLanguageQuestion', N'Bosnian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (31, N'PupilNativeLanguageQuestion', N'British Sign Language', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (32, N'PupilNativeLanguageQuestion', N'Bulgarian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (33, N'PupilNativeLanguageQuestion', N'Burmese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (34, N'PupilNativeLanguageQuestion', N'Cambodian/Khmer', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (35, N'PupilNativeLanguageQuestion', N'Cantonese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (36, N'PupilNativeLanguageQuestion', N'Caribbean Creole English', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (37, N'PupilNativeLanguageQuestion', N'Caribbean Creole French', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (38, N'PupilNativeLanguageQuestion', N'Catalan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (39, N'PupilNativeLanguageQuestion', N'Cebuano', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (40, N'PupilNativeLanguageQuestion', N'Chaga', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (41, N'PupilNativeLanguageQuestion', N'Chattisgarhi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (42, N'PupilNativeLanguageQuestion', N'Chechen', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (43, N'PupilNativeLanguageQuestion', N'Chichewa/Nyanja', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (44, N'PupilNativeLanguageQuestion', N'Chinese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (45, N'PupilNativeLanguageQuestion', N'Chitrali/Khowar', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (46, N'PupilNativeLanguageQuestion', N'Chokwe', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (47, N'PupilNativeLanguageQuestion', N'Cornish', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (48, N'PupilNativeLanguageQuestion', N'Creole', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (49, N'PupilNativeLanguageQuestion', N'Croatian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (50, N'PupilNativeLanguageQuestion', N'Czech', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (51, N'PupilNativeLanguageQuestion', N'Dagaare', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (52, N'PupilNativeLanguageQuestion', N'Dagbani', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (53, N'PupilNativeLanguageQuestion', N'Danish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (54, N'PupilNativeLanguageQuestion', N'Dari', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (55, N'PupilNativeLanguageQuestion', N'Dinka', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (56, N'PupilNativeLanguageQuestion', N'Dutch/Flemish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (57, N'PupilNativeLanguageQuestion', N'Dzongkha/Bhutanese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (58, N'PupilNativeLanguageQuestion', N'Ebira', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (59, N'PupilNativeLanguageQuestion', N'Edo/Bini', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (60, N'PupilNativeLanguageQuestion', N'Efik-Ibibio', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (61, N'PupilNativeLanguageQuestion', N'English', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (62, N'PupilNativeLanguageQuestion', N'Esan /Ishan', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (63, N'PupilNativeLanguageQuestion', N'Estonian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (64, N'PupilNativeLanguageQuestion', N'Ewe', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (65, N'PupilNativeLanguageQuestion', N'Ewondo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (66, N'PupilNativeLanguageQuestion', N'Fang', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (67, N'PupilNativeLanguageQuestion', N'Farsi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (68, N'PupilNativeLanguageQuestion', N'Fijian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (69, N'PupilNativeLanguageQuestion', N'Filipino', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (70, N'PupilNativeLanguageQuestion', N'Finnish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (71, N'PupilNativeLanguageQuestion', N'Fon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (72, N'PupilNativeLanguageQuestion', N'French', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (73, N'PupilNativeLanguageQuestion', N'Fula/Fulfulde-Pulaar', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (74, N'PupilNativeLanguageQuestion', N'Ga', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (75, N'PupilNativeLanguageQuestion', N'Gaelic, Irish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (76, N'PupilNativeLanguageQuestion', N'Gaelic, Scottish', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (77, N'PupilNativeLanguageQuestion', N'Galician/Galego', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (78, N'PupilNativeLanguageQuestion', N'Georgian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (79, N'PupilNativeLanguageQuestion', N'German', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (80, N'PupilNativeLanguageQuestion', N'Gogo/Chigogo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (81, N'PupilNativeLanguageQuestion', N'Greek', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (82, N'PupilNativeLanguageQuestion', N'Guarani', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (83, N'PupilNativeLanguageQuestion', N'Gujarati', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (84, N'PupilNativeLanguageQuestion', N'Gurenne/Frafra', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (85, N'PupilNativeLanguageQuestion', N'Gurma', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (86, N'PupilNativeLanguageQuestion', N'Hausa', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (87, N'PupilNativeLanguageQuestion', N'Hebrew', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (88, N'PupilNativeLanguageQuestion', N'Herero', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (89, N'PupilNativeLanguageQuestion', N'Hiligaynon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (90, N'PupilNativeLanguageQuestion', N'Hindi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (91, N'PupilNativeLanguageQuestion', N'Hindko', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (92, N'PupilNativeLanguageQuestion', N'Hindustani', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (93, N'PupilNativeLanguageQuestion', N'Hungarian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (94, N'PupilNativeLanguageQuestion', N'Iban', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (95, N'PupilNativeLanguageQuestion', N'Icelandic', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (96, N'PupilNativeLanguageQuestion', N'Idoma', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (97, N'PupilNativeLanguageQuestion', N'Igala', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (98, N'PupilNativeLanguageQuestion', N'Igbo (Ibo)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (99, N'PupilNativeLanguageQuestion', N'Ijo (Ijaw)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (100, N'PupilNativeLanguageQuestion', N'Ilokano', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (101, N'PupilNativeLanguageQuestion', N'Ilonggo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (102, N'PupilNativeLanguageQuestion', N'Indonesian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (103, N'PupilNativeLanguageQuestion', N'Italian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (104, N'PupilNativeLanguageQuestion', N'Itsekiri', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (105, N'PupilNativeLanguageQuestion', N'Japanese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (106, N'PupilNativeLanguageQuestion', N'Javanese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (107, N'PupilNativeLanguageQuestion', N'Jinghpaw/Kachin', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (108, N'PupilNativeLanguageQuestion', N'Kalenjin', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (109, N'PupilNativeLanguageQuestion', N'Kannada', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (110, N'PupilNativeLanguageQuestion', N'Kanuri', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (111, N'PupilNativeLanguageQuestion', N'Kaonde', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (112, N'PupilNativeLanguageQuestion', N'Karen (language group)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (113, N'PupilNativeLanguageQuestion', N'Kashmiri', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (114, N'PupilNativeLanguageQuestion', N'Kazakh', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (115, N'PupilNativeLanguageQuestion', N'Khmer', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (116, N'PupilNativeLanguageQuestion', N'Kikamba', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (117, N'PupilNativeLanguageQuestion', N'Kikongo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (118, N'PupilNativeLanguageQuestion', N'Kikuyu/Gikuyu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (119, N'PupilNativeLanguageQuestion', N'Kimbundu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (120, N'PupilNativeLanguageQuestion', N'Kimeru', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (121, N'PupilNativeLanguageQuestion', N'Kinyarwanda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (122, N'PupilNativeLanguageQuestion', N'Kirghiz/Kyrgyz', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (123, N'PupilNativeLanguageQuestion', N'Kirundi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (124, N'PupilNativeLanguageQuestion', N'Kisi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (125, N'PupilNativeLanguageQuestion', N'Kisii/Ekegusii', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (126, N'PupilNativeLanguageQuestion', N'Kisukuma', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (127, N'PupilNativeLanguageQuestion', N'Kiswahili', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (128, N'PupilNativeLanguageQuestion', N'Konkani', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (129, N'PupilNativeLanguageQuestion', N'Korean', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (130, N'PupilNativeLanguageQuestion', N'Kpelle', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (131, N'PupilNativeLanguageQuestion', N'Krio', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (132, N'PupilNativeLanguageQuestion', N'Kru', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (133, N'PupilNativeLanguageQuestion', N'Kurdish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (134, N'PupilNativeLanguageQuestion', N'Kutchi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (135, N'PupilNativeLanguageQuestion', N'Lango', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (136, N'PupilNativeLanguageQuestion', N'Lao', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (137, N'PupilNativeLanguageQuestion', N'Latvian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (138, N'PupilNativeLanguageQuestion', N'Lingala', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (139, N'PupilNativeLanguageQuestion', N'Lithuanian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (140, N'PupilNativeLanguageQuestion', N'Lozi/Silozi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (141, N'PupilNativeLanguageQuestion', N'Luba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (142, N'PupilNativeLanguageQuestion', N'Luganda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (143, N'PupilNativeLanguageQuestion', N'Lugbara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (144, N'PupilNativeLanguageQuestion', N'Lugisu/Lumasaba', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (145, N'PupilNativeLanguageQuestion', N'Luhya', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (146, N'PupilNativeLanguageQuestion', N'Lunda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (147, N'PupilNativeLanguageQuestion', N'Luo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (148, N'PupilNativeLanguageQuestion', N'Lusoga', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (149, N'PupilNativeLanguageQuestion', N'Luvale', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (150, N'PupilNativeLanguageQuestion', N'Luxembourgish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (151, N'PupilNativeLanguageQuestion', N'Maasai', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (152, N'PupilNativeLanguageQuestion', N'Macedonian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (153, N'PupilNativeLanguageQuestion', N'Magahi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (154, N'PupilNativeLanguageQuestion', N'Magindanao', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (155, N'PupilNativeLanguageQuestion', N'Maithili', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (156, N'PupilNativeLanguageQuestion', N'Makua', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (157, N'PupilNativeLanguageQuestion', N'Malagasy', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (158, N'PupilNativeLanguageQuestion', N'Malay', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (159, N'PupilNativeLanguageQuestion', N'Malayalam', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (160, N'PupilNativeLanguageQuestion', N'Maldivian/Dhivehi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (161, N'PupilNativeLanguageQuestion', N'Maltese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (162, N'PupilNativeLanguageQuestion', N'Mandarin', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (163, N'PupilNativeLanguageQuestion', N'Mandinka', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (164, N'PupilNativeLanguageQuestion', N'Maninka/ Malinke', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (165, N'PupilNativeLanguageQuestion', N'Maori', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (166, N'PupilNativeLanguageQuestion', N'Marathi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (167, N'PupilNativeLanguageQuestion', N'Mauritian Creole', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (168, N'PupilNativeLanguageQuestion', N'Mayan (language group)', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (169, N'PupilNativeLanguageQuestion', N'Mbosi/Mbochi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (170, N'PupilNativeLanguageQuestion', N'Memon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (171, N'PupilNativeLanguageQuestion', N'Mende', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (172, N'PupilNativeLanguageQuestion', N'Mirpuri', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (173, N'PupilNativeLanguageQuestion', N'Moldovan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (174, N'PupilNativeLanguageQuestion', N'Mongolian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (175, N'PupilNativeLanguageQuestion', N'Moore/Mossi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (176, N'PupilNativeLanguageQuestion', N'Munda (language group)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (177, N'PupilNativeLanguageQuestion', N'Nahuatl', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (178, N'PupilNativeLanguageQuestion', N'Nama/Damara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (179, N'PupilNativeLanguageQuestion', N'Ndebele', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (180, N'PupilNativeLanguageQuestion', N'Nepali', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (181, N'PupilNativeLanguageQuestion', N'Newari', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (182, N'PupilNativeLanguageQuestion', N'Norwegian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (183, N'PupilNativeLanguageQuestion', N'Nubian (language group)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (184, N'PupilNativeLanguageQuestion', N'Nuer', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (185, N'PupilNativeLanguageQuestion', N'Nupe', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (186, N'PupilNativeLanguageQuestion', N'Nyakyusa-Ngonde', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (187, N'PupilNativeLanguageQuestion', N'Nyanja', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (188, N'PupilNativeLanguageQuestion', N'Nzema', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (189, N'PupilNativeLanguageQuestion', N'Ogoni (language group)', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (190, N'PupilNativeLanguageQuestion', N'Oriya', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (191, N'PupilNativeLanguageQuestion', N'Oromo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (192, N'PupilNativeLanguageQuestion', N'Oshiwambo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (193, N'PupilNativeLanguageQuestion', N'Pahari (Pakistan)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (194, N'PupilNativeLanguageQuestion', N'Pahari/Himachali', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (195, N'PupilNativeLanguageQuestion', N'Pampangan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (196, N'PupilNativeLanguageQuestion', N'Pangasinan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (197, N'PupilNativeLanguageQuestion', N'Panjabi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (198, N'PupilNativeLanguageQuestion', N'Pashai', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (199, N'PupilNativeLanguageQuestion', N'Pashto/Pashtu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (200, N'PupilNativeLanguageQuestion', N'Patois', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (201, N'PupilNativeLanguageQuestion', N'Pedi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (202, N'PupilNativeLanguageQuestion', N'Persian/Farsi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (203, N'PupilNativeLanguageQuestion', N'Polish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (204, N'PupilNativeLanguageQuestion', N'Portuguese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (205, N'PupilNativeLanguageQuestion', N'Punjabi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (206, N'PupilNativeLanguageQuestion', N'Pushto/Pushtu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (207, N'PupilNativeLanguageQuestion', N'Quechua', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (208, N'PupilNativeLanguageQuestion', N'Rajasthani/Marwari', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (209, N'PupilNativeLanguageQuestion', N'Romani', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (210, N'PupilNativeLanguageQuestion', N'Romanian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (211, N'PupilNativeLanguageQuestion', N'Romansch', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (212, N'PupilNativeLanguageQuestion', N'Runyakitara', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (213, N'PupilNativeLanguageQuestion', N'Russian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (214, N'PupilNativeLanguageQuestion', N'Rwanda', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (215, N'PupilNativeLanguageQuestion', N'Samoan', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (216, N'PupilNativeLanguageQuestion', N'Sango', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (217, N'PupilNativeLanguageQuestion', N'Sardinian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (218, N'PupilNativeLanguageQuestion', N'Scots', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (219, N'PupilNativeLanguageQuestion', N'Serbian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (220, N'PupilNativeLanguageQuestion', N'Sesotho', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (221, N'PupilNativeLanguageQuestion', N'Setswana', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (222, N'PupilNativeLanguageQuestion', N'Shelta/Traveller Irish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (223, N'PupilNativeLanguageQuestion', N'Shilluk/Chollo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (224, N'PupilNativeLanguageQuestion', N'Shona', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (225, N'PupilNativeLanguageQuestion', N'Sidamo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (226, N'PupilNativeLanguageQuestion', N'Sindhi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (227, N'PupilNativeLanguageQuestion', N'Sinhala', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (228, N'PupilNativeLanguageQuestion', N'Siraiki', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (229, N'PupilNativeLanguageQuestion', N'Slovak', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (230, N'PupilNativeLanguageQuestion', N'Slovenian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (231, N'PupilNativeLanguageQuestion', N'Somali', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (232, N'PupilNativeLanguageQuestion', N'Sotho', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (233, N'PupilNativeLanguageQuestion', N'Spanish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (234, N'PupilNativeLanguageQuestion', N'Sunda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (235, N'PupilNativeLanguageQuestion', N'Surigaonon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (236, N'PupilNativeLanguageQuestion', N'Swahili', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (237, N'PupilNativeLanguageQuestion', N'Swazi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (238, N'PupilNativeLanguageQuestion', N'Swedish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (239, N'PupilNativeLanguageQuestion', N'Sylheti', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (240, N'PupilNativeLanguageQuestion', N'Tagalog', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (241, N'PupilNativeLanguageQuestion', N'Tajiki', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (242, N'PupilNativeLanguageQuestion', N'Tamazight', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (243, N'PupilNativeLanguageQuestion', N'Tamil', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (244, N'PupilNativeLanguageQuestion', N'Telugu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (245, N'PupilNativeLanguageQuestion', N'Temne', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (246, N'PupilNativeLanguageQuestion', N'Teso/Ateso', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (247, N'PupilNativeLanguageQuestion', N'Tetum/Tetun', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (248, N'PupilNativeLanguageQuestion', N'Thai', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (249, N'PupilNativeLanguageQuestion', N'Tibetan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (250, N'PupilNativeLanguageQuestion', N'Tigre', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (251, N'PupilNativeLanguageQuestion', N'Tigrinya', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (252, N'PupilNativeLanguageQuestion', N'Tiv', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (253, N'PupilNativeLanguageQuestion', N'Tok Pisin', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (254, N'PupilNativeLanguageQuestion', N'Tonga/Chitonga', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (255, N'PupilNativeLanguageQuestion', N'Tongan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (256, N'PupilNativeLanguageQuestion', N'Tsonga', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (257, N'PupilNativeLanguageQuestion', N'Tswana', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (258, N'PupilNativeLanguageQuestion', N'Tulu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (259, N'PupilNativeLanguageQuestion', N'Tumbuka', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (260, N'PupilNativeLanguageQuestion', N'Turkish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (261, N'PupilNativeLanguageQuestion', N'Turkmen', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (262, N'PupilNativeLanguageQuestion', N'Twi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (263, N'PupilNativeLanguageQuestion', N'Ukrainian', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (264, N'PupilNativeLanguageQuestion', N'Umbundu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (265, N'PupilNativeLanguageQuestion', N'Urdu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (266, N'PupilNativeLanguageQuestion', N'Urhobo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (267, N'PupilNativeLanguageQuestion', N'Uyghur', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (268, N'PupilNativeLanguageQuestion', N'Uzbek', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (269, N'PupilNativeLanguageQuestion', N'Venda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (270, N'PupilNativeLanguageQuestion', N'Vietnamese', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (271, N'PupilNativeLanguageQuestion', N'Visayan/Bisaya', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (272, N'PupilNativeLanguageQuestion', N'Wali', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (273, N'PupilNativeLanguageQuestion', N'Wa-Parauk', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (274, N'PupilNativeLanguageQuestion', N'Waray', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (275, N'PupilNativeLanguageQuestion', N'Welsh', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (276, N'PupilNativeLanguageQuestion', N'Wolof', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (277, N'PupilNativeLanguageQuestion', N'Xhosa', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (278, N'PupilNativeLanguageQuestion', N'Yao/Chiyao', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (279, N'PupilNativeLanguageQuestion', N'Yiddish', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (280, N'PupilNativeLanguageQuestion', N'Yoruba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (281, N'PupilNativeLanguageQuestion', N'Zande', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (282, N'PupilNativeLanguageQuestion', N'Zulu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (283, N'PupilNativeLanguageQuestion', N'Other', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (284, N'PupilCountryQuestion', N'Afghanistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (285, N'PupilCountryQuestion', N'Aland Islands  ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (286, N'PupilCountryQuestion', N'Albania', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (287, N'PupilCountryQuestion', N'Algeria', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (288, N'PupilCountryQuestion', N'American Samoa', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (289, N'PupilCountryQuestion', N'Andorra', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (290, N'PupilCountryQuestion', N'Angola', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (291, N'PupilCountryQuestion', N'Argentina', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (292, N'PupilCountryQuestion', N'Armenia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (293, N'PupilCountryQuestion', N'Aruba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (294, N'PupilCountryQuestion', N'Australia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (295, N'PupilCountryQuestion', N'Austria', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (296, N'PupilCountryQuestion', N'Azerbaijan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (297, N'PupilCountryQuestion', N'Bahamas', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (298, N'PupilCountryQuestion', N'Bahrain', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (299, N'PupilCountryQuestion', N'Bangladesh', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (300, N'PupilCountryQuestion', N'Barbados', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (301, N'PupilCountryQuestion', N'Belarus', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (302, N'PupilCountryQuestion', N'Belgium', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (303, N'PupilCountryQuestion', N'Belize', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (304, N'PupilCountryQuestion', N'Benin', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (305, N'PupilCountryQuestion', N'Bermuda ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (306, N'PupilCountryQuestion', N'Bhutan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (307, N'PupilCountryQuestion', N'Bolivia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (308, N'PupilCountryQuestion', N'Bosnia and Herzegovina', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (309, N'PupilCountryQuestion', N'Botswana', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (310, N'PupilCountryQuestion', N'Brazil', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (311, N'PupilCountryQuestion', N'Brunei', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (312, N'PupilCountryQuestion', N'Bulgaria', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (313, N'PupilCountryQuestion', N'Burkina Faso', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (314, N'PupilCountryQuestion', N'Burundi', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (315, N'PupilCountryQuestion', N'Cambodia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (316, N'PupilCountryQuestion', N'Cameroon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (317, N'PupilCountryQuestion', N'Canada', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (318, N'PupilCountryQuestion', N'Cape Verde Islands', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (319, N'PupilCountryQuestion', N'Cayman Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (320, N'PupilCountryQuestion', N'Central African Republic', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (321, N'PupilCountryQuestion', N'Chad', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (322, N'PupilCountryQuestion', N'Channel Islands Alderney', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (323, N'PupilCountryQuestion', N'Channel Islands Guernsey', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (324, N'PupilCountryQuestion', N'Channel Islands Jersey', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (325, N'PupilCountryQuestion', N'Channel Islands Sark', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (326, N'PupilCountryQuestion', N'Chechnya  ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (327, N'PupilCountryQuestion', N'Guyana', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (328, N'PupilCountryQuestion', N'Haiti', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (329, N'PupilCountryQuestion', N'Holy See  (Vatican City)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (330, N'PupilCountryQuestion', N'Honduras', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (331, N'PupilCountryQuestion', N'Hong Kong', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (332, N'PupilCountryQuestion', N'Hungary', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (333, N'PupilCountryQuestion', N'Iceland', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (334, N'PupilCountryQuestion', N'India', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (335, N'PupilCountryQuestion', N'Indonesia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (336, N'PupilCountryQuestion', N'Iran', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (337, N'PupilCountryQuestion', N'Iraq', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (338, N'PupilCountryQuestion', N'Ireland (Eire)', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (339, N'PupilCountryQuestion', N'Isle of Man', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (340, N'PupilCountryQuestion', N'Israel', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (341, N'PupilCountryQuestion', N'Italy', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (342, N'PupilCountryQuestion', N'Ivory Coast', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (343, N'PupilCountryQuestion', N'Jamaica', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (344, N'PupilCountryQuestion', N'Japan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (345, N'PupilCountryQuestion', N'Jordan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (346, N'PupilCountryQuestion', N'Kazakhstan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (347, N'PupilCountryQuestion', N'Kenya', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (348, N'PupilCountryQuestion', N'Kiribati', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (349, N'PupilCountryQuestion', N'Kosovo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (350, N'PupilCountryQuestion', N'Kurdistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (351, N'PupilCountryQuestion', N'Kuwait', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (352, N'PupilCountryQuestion', N'Kyrgyzstan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (353, N'PupilCountryQuestion', N'Laos', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (354, N'PupilCountryQuestion', N'Latvia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (355, N'PupilCountryQuestion', N'Lebanon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (356, N'PupilCountryQuestion', N'Leeward Islands Anguilla', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (357, N'PupilCountryQuestion', N'Leeward Islands Antigua', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (358, N'PupilCountryQuestion', N'Leeward Islands Barbuda', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (359, N'PupilCountryQuestion', N'Leeward Islands British Virgin Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (360, N'PupilCountryQuestion', N'Leeward Islands Guadeloupe', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (361, N'PupilCountryQuestion', N'Leeward Islands Iles des Saintes', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (362, N'PupilCountryQuestion', N'Leeward Islands La Desirade', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (363, N'PupilCountryQuestion', N'Leeward Islands Marie-Galante', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (364, N'PupilCountryQuestion', N'Leeward Islands Montserrat', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (365, N'PupilCountryQuestion', N'Leeward Islands Nevis', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (366, N'PupilCountryQuestion', N'Leeward Islands Puerto Rican Virgin Islands/Spanish Virgin Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (367, N'PupilCountryQuestion', N'Leeward Islands Redonda', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (368, N'PupilCountryQuestion', N'Leeward Islands Saba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (369, N'PupilCountryQuestion', N'Leeward Islands Saint-Barthelemy', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (370, N'PupilCountryQuestion', N'Leeward Islands Saint Kitts', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (371, N'PupilCountryQuestion', N'Leeward Islands Saint Martin/Sint Maarten', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (372, N'PupilCountryQuestion', N'Leeward Islands Sint Eustatius', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (373, N'PupilCountryQuestion', N'Leeward Islands U.S. Virgin Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (374, N'PupilCountryQuestion', N'Peru', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (375, N'PupilCountryQuestion', N'Philippines', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (376, N'PupilCountryQuestion', N'Poland', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (377, N'PupilCountryQuestion', N'Portugal', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (378, N'PupilCountryQuestion', N'Puerto Rico', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (379, N'PupilCountryQuestion', N'Qatar', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (380, N'PupilCountryQuestion', N'Republic of the Congo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (381, N'PupilCountryQuestion', N'Reunion ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (382, N'PupilCountryQuestion', N'Romania', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (383, N'PupilCountryQuestion', N'Russia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (384, N'PupilCountryQuestion', N'Rwanda', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (385, N'PupilCountryQuestion', N'Saint Helena', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (386, N'PupilCountryQuestion', N'Saint Pierre and Miquelon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (387, N'PupilCountryQuestion', N'Samoa', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (388, N'PupilCountryQuestion', N'San Marino', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (389, N'PupilCountryQuestion', N'Sao Tome and Principe', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (390, N'PupilCountryQuestion', N'Saudi Arabia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (391, N'PupilCountryQuestion', N'Scotland', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (392, N'PupilCountryQuestion', N'Senegal', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (393, N'PupilCountryQuestion', N'Serbia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (394, N'PupilCountryQuestion', N'Seychelles', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (395, N'PupilCountryQuestion', N'Sierra Leone', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (396, N'PupilCountryQuestion', N'Singapore', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (397, N'PupilCountryQuestion', N'Slovakia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (398, N'PupilCountryQuestion', N'Slovenia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (399, N'PupilCountryQuestion', N'Solomon Islands', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (400, N'PupilCountryQuestion', N'Somalia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (401, N'PupilCountryQuestion', N'South Africa', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (402, N'PupilCountryQuestion', N'South Korea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (403, N'PupilCountryQuestion', N'South Sudan', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (404, N'PupilCountryQuestion', N'Spain', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (405, N'PupilCountryQuestion', N'Sri Lanka', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (406, N'PupilCountryQuestion', N'Sudan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (407, N'PupilCountryQuestion', N'Suriname', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (408, N'PupilCountryQuestion', N'Swaziland', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (409, N'PupilCountryQuestion', N'Sweden', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (410, N'PupilCountryQuestion', N'Chile', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (411, N'PupilCountryQuestion', N'China', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (412, N'PupilCountryQuestion', N'Christmas Island  ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (413, N'PupilCountryQuestion', N'Cocos (Keeling) Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (414, N'PupilCountryQuestion', N'Colombia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (415, N'PupilCountryQuestion', N'Comoros', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (416, N'PupilCountryQuestion', N'Cook Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (417, N'PupilCountryQuestion', N'Costa Rica', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (418, N'PupilCountryQuestion', N'Croatia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (419, N'PupilCountryQuestion', N'Cuba', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (420, N'PupilCountryQuestion', N'Curaçao', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (421, N'PupilCountryQuestion', N'Cyprus', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (422, N'PupilCountryQuestion', N'Czech Republic (Czechia)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (423, N'PupilCountryQuestion', N'Democratic Republic of the Congo (formerly known as Zaire)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (424, N'PupilCountryQuestion', N'Democratic Republic of Timor-Leste (East Timor)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (425, N'PupilCountryQuestion', N'Denmark', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (426, N'PupilCountryQuestion', N'Djibouti', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (427, N'PupilCountryQuestion', N'Dominican Republic', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (428, N'PupilCountryQuestion', N'Ecuador', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (429, N'PupilCountryQuestion', N'Egypt', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (430, N'PupilCountryQuestion', N'El Salvador', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (431, N'PupilCountryQuestion', N'Equatorial Guinea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (432, N'PupilCountryQuestion', N'Eritrea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (433, N'PupilCountryQuestion', N'Estonia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (434, N'PupilCountryQuestion', N'Ethiopia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (435, N'PupilCountryQuestion', N'Falkland Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (436, N'PupilCountryQuestion', N'Faroe Islands', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (437, N'PupilCountryQuestion', N'Fiji', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (438, N'PupilCountryQuestion', N'Finland', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (439, N'PupilCountryQuestion', N'France', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (440, N'PupilCountryQuestion', N'French Guiana', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (441, N'PupilCountryQuestion', N'French Polynesia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (442, N'PupilCountryQuestion', N'Gabon', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (443, N'PupilCountryQuestion', N'Gambia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (444, N'PupilCountryQuestion', N'Georgia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (445, N'PupilCountryQuestion', N'Germany', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (446, N'PupilCountryQuestion', N'Ghana', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (447, N'PupilCountryQuestion', N'Gibraltar', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (448, N'PupilCountryQuestion', N'Greece', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (449, N'PupilCountryQuestion', N'Greenland', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (450, N'PupilCountryQuestion', N'Guam ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (451, N'PupilCountryQuestion', N'Guatemala', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (452, N'PupilCountryQuestion', N'Guinea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (453, N'PupilCountryQuestion', N'Guinea-Bissau', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (454, N'PupilCountryQuestion', N'Lesotho', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (455, N'PupilCountryQuestion', N'Liberia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (456, N'PupilCountryQuestion', N'Libya', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (457, N'PupilCountryQuestion', N'Liechtenstein', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (458, N'PupilCountryQuestion', N'Lithuania', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (459, N'PupilCountryQuestion', N'Luxembourg', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (460, N'PupilCountryQuestion', N'Macau ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (461, N'PupilCountryQuestion', N'Madagascar', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (462, N'PupilCountryQuestion', N'Malawi', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (463, N'PupilCountryQuestion', N'Malaysia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (464, N'PupilCountryQuestion', N'Maldives', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (465, N'PupilCountryQuestion', N'Mali', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (466, N'PupilCountryQuestion', N'Malta', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (467, N'PupilCountryQuestion', N'Marshall Islands ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (468, N'PupilCountryQuestion', N'Mauritania', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (469, N'PupilCountryQuestion', N'Mauritius', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (470, N'PupilCountryQuestion', N'Mayotte ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (471, N'PupilCountryQuestion', N'Mexico', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (472, N'PupilCountryQuestion', N'Micronesia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (473, N'PupilCountryQuestion', N'Moldova', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (474, N'PupilCountryQuestion', N'Monaco', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (475, N'PupilCountryQuestion', N'Mongolia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (476, N'PupilCountryQuestion', N'Montenegro', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (477, N'PupilCountryQuestion', N'Montserrat ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (478, N'PupilCountryQuestion', N'Morocco', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (479, N'PupilCountryQuestion', N'Mozambique', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (480, N'PupilCountryQuestion', N'Myanmar (formally known as Burma)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (481, N'PupilCountryQuestion', N'Namibia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (482, N'PupilCountryQuestion', N'Nauru ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (483, N'PupilCountryQuestion', N'Nepal', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (484, N'PupilCountryQuestion', N'Netherlands (Holland)', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (485, N'PupilCountryQuestion', N'New Caledonia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (486, N'PupilCountryQuestion', N'New Zealand', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (487, N'PupilCountryQuestion', N'Nicaragua', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (488, N'PupilCountryQuestion', N'Niger', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (489, N'PupilCountryQuestion', N'Nigeria', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (490, N'PupilCountryQuestion', N'Niue', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (491, N'PupilCountryQuestion', N'Norfolk Island ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (492, N'PupilCountryQuestion', N'North Korea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (493, N'PupilCountryQuestion', N'Northern Mariana Islands ', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (494, N'PupilCountryQuestion', N'Norway', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (495, N'PupilCountryQuestion', N'Oman', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (496, N'PupilCountryQuestion', N'Pakistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (497, N'PupilCountryQuestion', N'Palau', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (498, N'PupilCountryQuestion', N'Palestine ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (499, N'PupilCountryQuestion', N'Panama', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (500, N'PupilCountryQuestion', N'Papua New Guinea', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (501, N'PupilCountryQuestion', N'Paraguay', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (502, N'PupilCountryQuestion', N'Switzerland', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (503, N'PupilCountryQuestion', N'Syria', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (504, N'PupilCountryQuestion', N'Taiwan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (505, N'PupilCountryQuestion', N'Tajikistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (506, N'PupilCountryQuestion', N'Tanzania', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (507, N'PupilCountryQuestion', N'Thailand', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (508, N'PupilCountryQuestion', N'Togo', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (509, N'PupilCountryQuestion', N'Tokelau', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (510, N'PupilCountryQuestion', N'Tonga', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (511, N'PupilCountryQuestion', N'Tunisia', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (512, N'PupilCountryQuestion', N'Turkey', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (513, N'PupilCountryQuestion', N'Turkmenistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (514, N'PupilCountryQuestion', N'Turks and Caicos Islands', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (515, N'PupilCountryQuestion', N'Tuvalu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (516, N'PupilCountryQuestion', N'Uganda', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (517, N'PupilCountryQuestion', N'Ukraine', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (518, N'PupilCountryQuestion', N'United Arab Emirates', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (519, N'PupilCountryQuestion', N'Uruguay', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (520, N'PupilCountryQuestion', N'USA', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (521, N'PupilCountryQuestion', N'Uzbekistan', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (522, N'PupilCountryQuestion', N'Vanuatu', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (523, N'PupilCountryQuestion', N'Venezuela', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (524, N'PupilCountryQuestion', N'Vietnam', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (525, N'PupilCountryQuestion', N'Wales', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (526, N'PupilCountryQuestion', N'Wallis and Futuna', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (527, N'PupilCountryQuestion', N'Western Sahara', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (528, N'PupilCountryQuestion', N'Windward Islands ', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (529, N'PupilCountryQuestion', N'Windward Islands  Dominica', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (530, N'PupilCountryQuestion', N'Windward Islands  Martinique', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (531, N'PupilCountryQuestion', N'Windward Islands  Saint Lucia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (532, N'PupilCountryQuestion', N'Windward Islands  Saint Vincent and the Grenadines', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (533, N'PupilCountryQuestion', N'Windward Islands  Grenada', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (534, N'PupilCountryQuestion', N'Windward Islands  Trinidad and Tobago', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (535, N'PupilCountryQuestion', N'Yemen', NULL)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (536, N'PupilCountryQuestion', N'Zambia', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (537, N'PupilCountryQuestion', N'Zimbabwe', 1)
GO
INSERT [dbo].[PotentialAnswers] ([Id], [QuestionId], [AnswerValue], [Rejected]) VALUES (538, N'PupilCountryQuestion', N'Other', 1)
GO
SET IDENTITY_INSERT [dbo].[PotentialAnswers] OFF
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

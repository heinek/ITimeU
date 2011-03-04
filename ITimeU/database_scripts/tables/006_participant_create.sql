USE [ITimeU]
GO

/****** Object:  Table [dbo].[Participant]    Script Date: 02/09/2011 10:40:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Participant](
	[ParticipantID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[SurName] [nvarchar](50) NULL,
	[PostalAddress] [nvarchar](100) NULL,
	[PostalCode] [nvarchar](4) NULL,
	[PostalPlace] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Club] [int] NULL,
	[Birthday] [datetime] NULL,
	[Gender] [char](1) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED 
(
	[ParticipantID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


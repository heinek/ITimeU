USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 02/09/2011 10:41:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceIntermediate](
	[CheckpointID] [int] NOT NULL,
	[ParticipantID] [int] NOT NULL,
	[TimeStamp] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_RaceIntermediate] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC,
	[ParticipantID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


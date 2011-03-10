USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 03/09/2011 10:35:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckpointOrder]') AND type in (N'U'))
DROP TABLE [dbo].[CheckpointOrder]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 03/09/2011 10:35:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CheckpointOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CheckpointID] [int] NULL,
	[StartingNumber] [int] NULL,
	[OrderNumber] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CheckpointOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CheckpointOrder_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CheckpointOrder] DROP CONSTRAINT [DF_CheckpointOrder_IsDeleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CheckpointOrder_IsMerged]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CheckpointOrder] DROP CONSTRAINT [DF_CheckpointOrder_IsMerged]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 04/17/2011 21:55:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckpointOrder]') AND type in (N'U'))
DROP TABLE [dbo].[CheckpointOrder]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 04/17/2011 21:55:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CheckpointOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CheckpointID] [int] NULL,
	[StartingNumber] [int] NULL,
	[OrderNumber] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsMerged] [bit] NOT NULL,
 CONSTRAINT [PK_CheckpointOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CheckpointOrder] ADD  CONSTRAINT [DF_CheckpointOrder_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[CheckpointOrder] ADD  CONSTRAINT [DF_CheckpointOrder_IsMerged]  DEFAULT ((0)) FOR [IsMerged]
GO


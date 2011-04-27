USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Runtime_Checkpoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Runtime]'))
ALTER TABLE [dbo].[Runtime] DROP CONSTRAINT [FK_Runtime_Checkpoint]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Runtime_IsMerged]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Runtime] DROP CONSTRAINT [DF_Runtime_IsMerged]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Runtime_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Runtime] DROP CONSTRAINT [DF_Runtime_IsDeleted]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 04/27/2011 12:47:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Runtime]') AND type in (N'U'))
DROP TABLE [dbo].[Runtime]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 04/27/2011 12:47:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Runtime](
	[RuntimeID] [int] IDENTITY(1,1) NOT NULL,
	[Runtime] [int] NOT NULL,
	[CheckpointID] [int] NOT NULL,
	[IsMerged] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Runtime] PRIMARY KEY CLUSTERED 
(
	[RuntimeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Runtime]  WITH CHECK ADD  CONSTRAINT [FK_Runtime_Checkpoint] FOREIGN KEY([CheckpointID])
REFERENCES [dbo].[Checkpoint] ([CheckpointID])
GO

ALTER TABLE [dbo].[Runtime] CHECK CONSTRAINT [FK_Runtime_Checkpoint]
GO

ALTER TABLE [dbo].[Runtime] ADD  CONSTRAINT [DF_Runtime_IsMerged]  DEFAULT ((0)) FOR [IsMerged]
GO

ALTER TABLE [dbo].[Runtime] ADD  CONSTRAINT [DF_Runtime_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


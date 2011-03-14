USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_Checkpoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_Checkpoint]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_CheckpointOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_CheckpointOrder]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_Runtime]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_Runtime]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RaceIntermediate_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [DF_RaceIntermediate_IsDeleted]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 03/10/2011 15:01:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]') AND type in (N'U'))
DROP TABLE [dbo].[RaceIntermediate]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 03/10/2011 15:01:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceIntermediate](
	[CheckpointID] [int] NOT NULL,
	[CheckpointOrderID] [int] NOT NULL,
	[RuntimeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RaceIntermediate] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC,
	[CheckpointOrderID] ASC,
	[RuntimeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_Checkpoint] FOREIGN KEY([CheckpointID])
REFERENCES [dbo].[Checkpoint] ([CheckpointID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_Checkpoint]
GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_CheckpointOrder] FOREIGN KEY([CheckpointOrderID])
REFERENCES [dbo].[CheckpointOrder] ([ID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_CheckpointOrder]
GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_Runtime] FOREIGN KEY([RuntimeId])
REFERENCES [dbo].[Runtime] ([RuntimeID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_Runtime]
GO

ALTER TABLE [dbo].[RaceIntermediate] ADD  CONSTRAINT [DF_RaceIntermediate_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


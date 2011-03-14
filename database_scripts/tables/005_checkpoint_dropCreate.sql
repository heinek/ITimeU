USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Checkpoint_Timer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Checkpoint]'))
ALTER TABLE [dbo].[Checkpoint] DROP CONSTRAINT [FK_Checkpoint_Timer]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Checkpoint_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Checkpoint] DROP CONSTRAINT [DF_Checkpoint_IsDeleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Checkpoint_SortOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Checkpoint] DROP CONSTRAINT [DF_Checkpoint_SortOrder]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 03/09/2011 10:35:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Checkpoint]') AND type in (N'U'))
DROP TABLE [dbo].[Checkpoint]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 03/09/2011 10:35:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Checkpoint](
	[CheckpointID] [int] IDENTITY(1,1) NOT NULL,
	[RaceID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimerID] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_Checkpoint] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Checkpoint]  WITH CHECK ADD  CONSTRAINT [FK_Checkpoint_Timer] FOREIGN KEY([TimerID])
REFERENCES [dbo].[Timer] ([TimerID])
GO

ALTER TABLE [dbo].[Checkpoint] CHECK CONSTRAINT [FK_Checkpoint_Timer]
GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_SortOrder]  DEFAULT ((1)) FOR [SortOrder]
GO


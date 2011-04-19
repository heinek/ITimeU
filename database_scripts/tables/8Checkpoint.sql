USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 04/15/2011 14:02:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Checkpoint](
	[CheckpointID] [int] IDENTITY(1,1) NOT NULL,
	[RaceID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimerID] [int] NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_Checkpoint] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Checkpoint]  WITH CHECK ADD  CONSTRAINT [FK_Checkpoint_Race] FOREIGN KEY([RaceID])
REFERENCES [dbo].[Race] ([RaceID])
GO

ALTER TABLE [dbo].[Checkpoint] CHECK CONSTRAINT [FK_Checkpoint_Race]
GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_SortOrder]  DEFAULT ((1)) FOR [SortOrder]
GO


USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 04/14/2011 10:30:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceIntermediate](
	[CheckpointID] [int] NOT NULL,
	[CheckpointOrderID] [int] NOT NULL,
	[RuntimeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AthleteId] [int] NULL,
 CONSTRAINT [PK_RaceIntermediate] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC,
	[CheckpointOrderID] ASC,
	[RuntimeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_Athlete] FOREIGN KEY([AthleteId])
REFERENCES [dbo].[Athlete] ([ID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_Athlete]
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


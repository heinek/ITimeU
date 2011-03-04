USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 02/28/2011 10:30:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Checkpoint](
	[CheckpointID] [int] IDENTITY(1,1) NOT NULL,
	[RaceID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[TimerID] [int] NULL,
 CONSTRAINT [PK_Checkpoint] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Checkpoint]  WITH CHECK ADD  CONSTRAINT [FK_Checkpoint_Timer] FOREIGN KEY([TimerID])
REFERENCES [dbo].[Timer] ([TimerID])
ON UPDATE CASCADE
ON DELETE SET DEFAULT
GO

ALTER TABLE [dbo].[Checkpoint] CHECK CONSTRAINT [FK_Checkpoint_Timer]
GO



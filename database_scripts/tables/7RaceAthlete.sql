USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceAthlete]    Script Date: 04/14/2011 10:29:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceAthlete](
	[RaceId] [int] NOT NULL,
	[AthleteId] [int] NOT NULL,
	[Startnumber] [int] NULL,
 CONSTRAINT [PK_RaceAthlete] PRIMARY KEY CLUSTERED 
(
	[RaceId] ASC,
	[AthleteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RaceAthlete]  WITH CHECK ADD  CONSTRAINT [FK_RaceAthlete_Athlete] FOREIGN KEY([AthleteId])
REFERENCES [dbo].[Athlete] ([ID])
GO

ALTER TABLE [dbo].[RaceAthlete] CHECK CONSTRAINT [FK_RaceAthlete_Athlete]
GO

ALTER TABLE [dbo].[RaceAthlete]  WITH CHECK ADD  CONSTRAINT [FK_RaceAthlete_Race] FOREIGN KEY([RaceId])
REFERENCES [dbo].[Race] ([RaceID])
GO

ALTER TABLE [dbo].[RaceAthlete] CHECK CONSTRAINT [FK_RaceAthlete_Race]
GO


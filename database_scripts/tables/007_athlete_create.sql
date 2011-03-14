USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Athlete_AthleteClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[Athlete]'))
ALTER TABLE [dbo].[Athlete] DROP CONSTRAINT [FK_Athlete_AthleteClass]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Athlete_Club]') AND parent_object_id = OBJECT_ID(N'[dbo].[Athlete]'))
ALTER TABLE [dbo].[Athlete] DROP CONSTRAINT [FK_Athlete_Club]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Athlete]    Script Date: 03/09/2011 10:34:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Athlete]') AND type in (N'U'))
DROP TABLE [dbo].[Athlete]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Athlete]    Script Date: 03/09/2011 10:34:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Athlete](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[PostalAddress] [nvarchar](100) NULL,
	[PostalCode] [nvarchar](4) NULL,
	[PostalPlace] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[ClubID] [int] NULL,
	[Birthday] [int] NULL,
	[ClassID] [int] NULL,
	[Startnumber] [int] NULL,
	[Gender] [char](1) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Athlete]  WITH CHECK ADD  CONSTRAINT [FK_Athlete_AthleteClass] FOREIGN KEY([ClassID])
REFERENCES [dbo].[AthleteClass] ([ID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Athlete] CHECK CONSTRAINT [FK_Athlete_AthleteClass]
GO

ALTER TABLE [dbo].[Athlete]  WITH CHECK ADD  CONSTRAINT [FK_Athlete_Club] FOREIGN KEY([ClubID])
REFERENCES [dbo].[Club] ([ClubID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Athlete] CHECK CONSTRAINT [FK_Athlete_Club]
GO


USE [ITimeU]
GO

/****** Object:  Table [dbo].[Athlete]    Script Date: 03/08/2011 15:24:42 ******/
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


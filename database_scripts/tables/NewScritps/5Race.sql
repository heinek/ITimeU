USE [ITimeU]
GO

/****** Object:  Table [dbo].[Race]    Script Date: 04/14/2011 10:29:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Race](
	[RaceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Distance] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[EventId] [int] NULL,
 CONSTRAINT [PK_Race] PRIMARY KEY CLUSTERED 
(
	[RaceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Race]  WITH CHECK ADD  CONSTRAINT [FK_Race_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
GO

ALTER TABLE [dbo].[Race] CHECK CONSTRAINT [FK_Race_Event]
GO

ALTER TABLE [dbo].[Race] ADD  CONSTRAINT [DF_Race_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


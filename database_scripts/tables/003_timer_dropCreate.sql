USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Timer_Race]') AND parent_object_id = OBJECT_ID(N'[dbo].[Timer]'))
ALTER TABLE [dbo].[Timer] DROP CONSTRAINT [FK_Timer_Race]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Timer_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Timer] DROP CONSTRAINT [DF_Timer_IsDeleted]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Timer]    Script Date: 03/11/2011 10:21:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Timer]') AND type in (N'U'))
DROP TABLE [dbo].[Timer]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Timer]    Script Date: 03/11/2011 10:21:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Timer](
	[TimerID] [int] IDENTITY(1,1) NOT NULL,
	[RaceID] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Timer] PRIMARY KEY CLUSTERED 
(
	[TimerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Timer]  WITH CHECK ADD  CONSTRAINT [FK_Timer_Race] FOREIGN KEY([RaceID])
REFERENCES [dbo].[Race] ([RaceID])
GO

ALTER TABLE [dbo].[Timer] CHECK CONSTRAINT [FK_Timer_Race]
GO

ALTER TABLE [dbo].[Timer] ADD  CONSTRAINT [DF_Timer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


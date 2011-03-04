USE [ITimeU]
GO

/****** Object:  Table [dbo].[Timer]    Script Date: 02/04/2011 13:39:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Timer](
	[TimerID] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Timer] PRIMARY KEY CLUSTERED 
(
	[TimerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Timer] ADD  CONSTRAINT [DF_Timer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


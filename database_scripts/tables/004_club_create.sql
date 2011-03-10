USE [ITimeU]
GO

/****** Object:  Table [dbo].[Club]    Script Date: 03/09/2011 10:29:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Club]') AND type in (N'U'))
DROP TABLE [dbo].[Club]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Club]    Script Date: 03/09/2011 10:29:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Club](
	[ClubID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ContactName] [nvarchar](100) NULL,
	[ContactPhone] [nvarchar](100) NULL,
	[ContactEmail] [nvarchar](100) NULL,
 CONSTRAINT [PK_Club] PRIMARY KEY CLUSTERED 
(
	[ClubID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Club] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


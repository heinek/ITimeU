USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Runtime_IsMerged]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Runtime] DROP CONSTRAINT [DF_Runtime_IsMerged]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 03/10/2011 15:01:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Runtime]') AND type in (N'U'))
DROP TABLE [dbo].[Runtime]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 03/10/2011 15:01:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Runtime](
	[RuntimeID] [int] IDENTITY(1,1) NOT NULL,
	[Runtime] [int] NOT NULL,
	[CheckpointID] [int] NOT NULL,
	[IsMerged] [bit] NOT NULL,
 CONSTRAINT [PK_Runtime] PRIMARY KEY CLUSTERED 
(
	[RuntimeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Runtime] ADD  CONSTRAINT [DF_Runtime_IsMerged]  DEFAULT ((0)) FOR [IsMerged]
GO


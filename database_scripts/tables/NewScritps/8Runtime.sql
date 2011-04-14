USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 04/14/2011 10:29:38 ******/
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


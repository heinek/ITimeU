USE [ITimeU]
GO

/****** Object:  Table [dbo].[Runtime]    Script Date: 03/07/2011 12:31:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Runtime](
	[RuntimeID] [int] IDENTITY(1,1) NOT NULL,
	[Runtime] [int] NOT NULL,
	[CheckpointID] [int] NOT NULL,
 CONSTRAINT [PK_Runtime] PRIMARY KEY CLUSTERED 
(
	[RuntimeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


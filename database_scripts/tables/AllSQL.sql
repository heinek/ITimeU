USE [master]
GO

/****** Object:  Database [ITimeU]    Script Date: 03/10/2011 10:14:52 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'ITimeU')
DROP DATABASE [ITimeU]
GO

USE [master]
GO

/****** Object:  Database [ITimeU]    Script Date: 03/10/2011 10:14:53 ******/
CREATE DATABASE [ITimeU] ON  PRIMARY 
( NAME = N'ITimeU', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\ITimeU.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'ITimeU_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\ITimeU_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [ITimeU] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ITimeU].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [ITimeU] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [ITimeU] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [ITimeU] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [ITimeU] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [ITimeU] SET ARITHABORT OFF 
GO

ALTER DATABASE [ITimeU] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [ITimeU] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [ITimeU] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [ITimeU] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [ITimeU] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [ITimeU] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [ITimeU] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [ITimeU] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [ITimeU] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [ITimeU] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [ITimeU] SET  DISABLE_BROKER 
GO

ALTER DATABASE [ITimeU] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [ITimeU] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [ITimeU] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [ITimeU] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [ITimeU] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [ITimeU] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [ITimeU] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [ITimeU] SET  READ_WRITE 
GO

ALTER DATABASE [ITimeU] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [ITimeU] SET  MULTI_USER 
GO

ALTER DATABASE [ITimeU] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [ITimeU] SET DB_CHAINING OFF 
GO

USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Race_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Race] DROP CONSTRAINT [DF_Race_IsDeleted]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Race]    Script Date: 03/09/2011 10:29:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Race]') AND type in (N'U'))
DROP TABLE [dbo].[Race]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Race]    Script Date: 03/09/2011 10:29:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Race](
	[RaceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Race] PRIMARY KEY CLUSTERED 
(
	[RaceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Race] ADD  CONSTRAINT [DF_Race_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

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

USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Checkpoint_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Checkpoint] DROP CONSTRAINT [DF_Checkpoint_IsDeleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Checkpoint_SortOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Checkpoint] DROP CONSTRAINT [DF_Checkpoint_SortOrder]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 03/24/2011 14:11:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Checkpoint]') AND type in (N'U'))
DROP TABLE [dbo].[Checkpoint]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[Checkpoint]    Script Date: 03/24/2011 14:11:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Checkpoint](
	[CheckpointID] [int] IDENTITY(1,1) NOT NULL,
	[RaceID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TimerID] [int] NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_Checkpoint] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Checkpoint] ADD  CONSTRAINT [DF_Checkpoint_SortOrder]  DEFAULT ((1)) FOR [SortOrder]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[AthleteClass]    Script Date: 03/09/2011 10:30:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AthleteClass]') AND type in (N'U'))
DROP TABLE [dbo].[AthleteClass]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[AthleteClass]    Script Date: 03/09/2011 10:30:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AthleteClass](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [PK_AthleteClass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_AthleteClass] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

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

EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CheckpointOrder_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CheckpointOrder] DROP CONSTRAINT [DF_CheckpointOrder_IsDeleted]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CheckpointOrder_IsMerged]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CheckpointOrder] DROP CONSTRAINT [DF_CheckpointOrder_IsMerged]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 03/10/2011 15:01:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckpointOrder]') AND type in (N'U'))
DROP TABLE [dbo].[CheckpointOrder]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[CheckpointOrder]    Script Date: 03/10/2011 15:01:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CheckpointOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CheckpointID] [int] NULL,
	[StartingNumber] [int] NULL,
	[OrderNumber] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsMerged] [bit] NOT NULL,
 CONSTRAINT [PK_CheckpointOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CheckpointOrder] ADD  CONSTRAINT [DF_CheckpointOrder_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[CheckpointOrder] ADD  CONSTRAINT [DF_CheckpointOrder_IsMerged]  DEFAULT ((0)) FOR [IsMerged]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[AthleteClass]    Script Date: 03/08/2011 15:24:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AthleteClass](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [PK_AthleteClass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_AthleteClass] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ITimeU]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_Checkpoint]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_Checkpoint]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_CheckpointOrder]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_CheckpointOrder]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RaceIntermediate_Runtime]') AND parent_object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]'))
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [FK_RaceIntermediate_Runtime]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_RaceIntermediate_IsDeleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[RaceIntermediate] DROP CONSTRAINT [DF_RaceIntermediate_IsDeleted]
END

GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 03/10/2011 15:01:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RaceIntermediate]') AND type in (N'U'))
DROP TABLE [dbo].[RaceIntermediate]
GO

USE [ITimeU]
GO

/****** Object:  Table [dbo].[RaceIntermediate]    Script Date: 03/10/2011 15:01:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RaceIntermediate](
	[CheckpointID] [int] NOT NULL,
	[CheckpointOrderID] [int] NOT NULL,
	[RuntimeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RaceIntermediate] PRIMARY KEY CLUSTERED 
(
	[CheckpointID] ASC,
	[CheckpointOrderID] ASC,
	[RuntimeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_Checkpoint] FOREIGN KEY([CheckpointID])
REFERENCES [dbo].[Checkpoint] ([CheckpointID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_Checkpoint]
GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_CheckpointOrder] FOREIGN KEY([CheckpointOrderID])
REFERENCES [dbo].[CheckpointOrder] ([ID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_CheckpointOrder]
GO

ALTER TABLE [dbo].[RaceIntermediate]  WITH CHECK ADD  CONSTRAINT [FK_RaceIntermediate_Runtime] FOREIGN KEY([RuntimeId])
REFERENCES [dbo].[Runtime] ([RuntimeID])
GO

ALTER TABLE [dbo].[RaceIntermediate] CHECK CONSTRAINT [FK_RaceIntermediate_Runtime]
GO

ALTER TABLE [dbo].[RaceIntermediate] ADD  CONSTRAINT [DF_RaceIntermediate_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


USE [master]
GO

/****** Object:  Database [ITimeU]    Script Date: 04/14/2011 10:26:18 ******/
CREATE DATABASE [ITimeU] ON  PRIMARY 
( NAME = N'ITimeU', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLEXPRESS\MSSQL\DATA\ITimeU.mdf' , SIZE = 3072KB , MAXSIZE = 102400KB , FILEGROWTH = 10%)
 LOG ON 
( NAME = N'ITimeU_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLEXPRESS\MSSQL\DATA\ITimeU_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
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

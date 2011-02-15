/*
   10. februar 201115:49:24
   User: 
   Server: Z9402328\SQLEXPRESS
   Database: ITimeU
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_RaceIntermediate
	(
	CheckpointID int NOT NULL,
	ParticipantID int NULL,
	TimeStamp datetime NULL,
	IsDeleted bit NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_RaceIntermediate SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.RaceIntermediate)
	 EXEC('INSERT INTO dbo.Tmp_RaceIntermediate (CheckpointID, ParticipantID, TimeStamp, IsDeleted)
		SELECT CheckpointID, ParticipantID, TimeStamp, IsDeleted FROM dbo.RaceIntermediate WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.RaceIntermediate
GO
EXECUTE sp_rename N'dbo.Tmp_RaceIntermediate', N'RaceIntermediate', 'OBJECT' 
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RaceIntermediate', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RaceIntermediate', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RaceIntermediate', 'Object', 'CONTROL') as Contr_Per 
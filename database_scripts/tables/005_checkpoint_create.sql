/*
   17. februar 201113:54:16
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
ALTER TABLE dbo.Timer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Timer', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Timer', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Timer', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.[Checkpoint] ADD
	TimerID int NULL
GO
ALTER TABLE dbo.[Checkpoint] ADD CONSTRAINT
	FK_Checkpoint_Timer FOREIGN KEY
	(
	TimerID
	) REFERENCES dbo.Timer
	(
	TimerID
	) ON UPDATE  CASCADE 
	 ON DELETE  SET DEFAULT 
	
GO
ALTER TABLE dbo.[Checkpoint] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.[Checkpoint]', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.[Checkpoint]', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.[Checkpoint]', 'Object', 'CONTROL') as Contr_Per 
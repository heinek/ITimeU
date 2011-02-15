/*
   14. februar 201116:01:44
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
CREATE TABLE dbo.Tmp_Runtime
	(
	RuntimeID int NOT NULL IDENTITY (1, 1),
	Runtime int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Runtime SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Runtime ON
GO
IF EXISTS(SELECT * FROM dbo.Runtime)
	 EXEC('INSERT INTO dbo.Tmp_Runtime (RuntimeID, Runtime)
		SELECT RuntimeID, Runtime FROM dbo.Runtime WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Runtime OFF
GO
DROP TABLE dbo.Runtime
GO
EXECUTE sp_rename N'dbo.Tmp_Runtime', N'Runtime', 'OBJECT' 
GO
ALTER TABLE dbo.Runtime ADD CONSTRAINT
	PK_Runtime PRIMARY KEY CLUSTERED 
	(
	RuntimeID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.Runtime', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Runtime', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Runtime', 'Object', 'CONTROL') as Contr_Per 
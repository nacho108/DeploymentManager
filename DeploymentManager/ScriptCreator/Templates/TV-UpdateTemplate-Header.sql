USE [TrueViewEx];
GO

/*
 * Apply Script Settings
 */
SET NOCOUNT    ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
GO

/*
 * Script Header.
 * Need to edit for every new script
 */
DECLARE @requiredVersion  nvarchar(40)  = '{REQUIRED VERSION}';
DECLARE @newVersion  nvarchar(40);

DECLARE @newRelease INT = {NEW MAJOR VERSION};
DECLARE @newSprint INT = {NEW MINOR VERSION};
DECLARE @newBuild  INT = {NEW BUILD VERSION};
DECLARE @newRevision INT = {NEW REVISION VERSION};

SET @newVersion = CONVERT(nvarchar(10), @newRelease) + '.' + CONVERT(nvarchar(10), @newSprint) + '.' + CONVERT(nvarchar(10), @newBuild) + '.' + CONVERT(nvarchar(10), @newRevision);
DECLARE @scriptName    nvarchar(255) = 'TV-' + @newVersion + '.sql';

DECLARE @LogMsg nvarchar(MAX);
/*
 * Version Checking 
 */
DECLARE @maxVersion nvarchar(20)	
EXEC [security].[pub_SystemSchemaGetMaxVersion] @maxVersion OUT;
SET @LogMsg = 'Current DB VERSION: ' + @maxVersion;

EXEC [security].[pub_SystemSchemaAddLogInfo] @scriptName, @LogMsg;

/* 
 * Script Body/Content 
 */ 
IF ([security].[pub_SystemSchemaIsVersionApplied](@requiredVersion) = 1 AND [security].[pub_SystemSchemaIsVersionApplied](@newVersion) = 0)
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION	
		

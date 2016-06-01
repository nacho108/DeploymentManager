
			EXEC [security].[pub_SystemSchemaAddLogInfo] @scriptName, 'Adding change script record to SystemSchemaChange Table...'
			EXEC [security].[pub_SystemSchemaSetVersion] @newRelease, @newSprint, @newBuild, @newRevision, @scriptName;
		COMMIT TRANSACTION

		SET @LogMsg = 'Script ' + @scriptName + ' completed successfully.';
		EXEC [security].[pub_SystemSchemaAddLogInfo] @scriptName, @LogMsg;
	END TRY
	BEGIN CATCH
		/* Rollback the transactions */

		SET @LogMsg = ERROR_MESSAGE() + CHAR(13)+'Line:' + CONVERT(nvarchar, ERROR_LINE())+' ERROR_STATE:' + CONVERT(nvarchar, ERROR_STATE())+' ERROR_SEVERITY:' + CONVERT(nvarchar, ERROR_SEVERITY())+' ERROR_NUMBER:' + CONVERT(nvarchar, ERROR_NUMBER());

		IF (@@TRANCOUNT > 0)
			ROLLBACK TRANSACTION

		EXEC [security].[pub_SystemSchemaAddLogError] @scriptName, @LogMsg;
		EXEC [security].[pub_SystemSchemaAddLogError] @scriptName, 'ERROR OCCURED! All changes will be rolled back';
	END CATCH
END
ELSE BEGIN
	IF ([security].[pub_SystemSchemaIsVersionApplied](@newVersion) = 1)
	BEGIN
		SET @LogMsg = 'Script ' + CONVERT(nvarchar(10), @newRelease) + '.' + CONVERT(nvarchar(10), @newSprint) + '.' + CONVERT(nvarchar(10), @newBuild) + '.' + CONVERT(nvarchar(10), @newRevision) + ' already applied!';
		EXEC [security].[pub_SystemSchemaAddLogInfo] @scriptName, @LogMsg;
	END
	ELSE IF ([security].[pub_SystemSchemaIsVersionApplied](@requiredVersion) = 0)
	BEGIN
		SET @LogMsg = 'ERROR: The script requires version ' + @requiredVersion + ' to be appplied before. Please run/install the script with version ' + @requiredVersion + ' before this script';
		EXEC [security].[pub_SystemSchemaAddLogInfo] @scriptName, @LogMsg;
	END
END

/* Make sure there is no open transactions  */

IF @@TRANCOUNT <> 0
BEGIN
	PRINT '***** CAUTION: '+ CAST(@@TRANCOUNT as nvarchar(10))+ ' TRANSACTION(s) REMAIN OPEN! *****' 
	PRINT '***** SCRIPT MUST BE ROLLED BACK! *****'
END

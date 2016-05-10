declare @n char(1)
set @n = char(10)

declare @stmt nvarchar(max)

-- procedures
select @stmt = isnull( @stmt + @n, '' ) +
    'drop procedure [' + schema_name(schema_id) + '].[' + name + ']'
from sys.procedures
where name not in ('pri_SystemSchemaWriteLog','pub_SystemSchemaAddLogError','pub_SystemSchemaAddLogInfo',
	'pub_SystemSchemaAddLogWarn','pub_SystemSchemaGetMaxVersion','pub_SystemSchemaGetVersion','pub_SystemSchemaSetVersion')

-- functions
select @stmt = isnull( @stmt + @n, '' ) +    'drop function [' + schema_name(schema_id) + '].[' + name + ']'
from sys.objects
where type in ( 'FN', 'IF', 'TF' ) and name not in('pub_SystemSchemaIsVersionApplied')

-- user defined types
select @stmt = isnull( @stmt + @n, '' ) +
    'drop type [' + schema_name(schema_id) + '].[' + name + ']'
from sys.types
where is_user_defined = 1

--exec dbo.LongPrint @stmt
 
exec sp_executesql @stmt
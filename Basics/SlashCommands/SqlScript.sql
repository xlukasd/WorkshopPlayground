DECLARE @SearchStrColumnValue nvarchar(255), @IncludeFullRowInResults bit, @IncludeFullRowInResultsMaxRows int
SET @SearchStrColumnValue = '%your_search_text%'
SET @IncludeFullRowInResultsMaxRows = 10

IF OBJECT_ID('tempdb..#Results') IS NOT NULL DROP TABLE #Results
CREATE TABLE #Results (TableName nvarchar(128), ColumnName nvarchar(128), ColumnValue nvarchar(max),ColumnType nvarchar(20))

SET NOCOUNT ON

DECLARE @TableName nvarchar(256) = '',@ColumnName nvarchar(128),@ColumnType nvarchar(20), @QuotedSearchStrColumnValue nvarchar(110), @QuotedSearchStrColumnName nvarchar(110)
SET @QuotedSearchStrColumnValue = QUOTENAME(@SearchStrColumnValue,'''')
DECLARE @ColumnNameTable TABLE (COLUMN_NAME nvarchar(128),DATA_TYPE nvarchar(20))

WHILE @TableName IS NOT NULL
BEGIN
    SET @TableName = 
    (
        SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
        FROM    INFORMATION_SCHEMA.TABLES
        WHERE       TABLE_TYPE = 'BASE TABLE'
            AND QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName
            AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), 'IsMSShipped') = 0
    )
    IF @TableName IS NOT NULL
    BEGIN
        DECLARE @sql VARCHAR(MAX)
        SET @sql = 'SELECT QUOTENAME(COLUMN_NAME),DATA_TYPE
                FROM    INFORMATION_SCHEMA.COLUMNS
                WHERE       TABLE_SCHEMA    = PARSENAME(''' + @TableName + ''', 2)
                AND TABLE_NAME  = PARSENAME(''' + @TableName + ''', 1)
                AND DATA_TYPE IN (' + CASE WHEN ISNUMERIC(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@SearchStrColumnValue,'%',''),'_',''),'[',''),']',''),'-','')) = 1 THEN '''tinyint'',''int'',''smallint'',''bigint'',''numeric'',''decimal'',''smallmoney'',''money'',' ELSE '' END + '''char'',''varchar'',''nchar'',''nvarchar'',''timestamp'',''uniqueidentifier''' + ',''xml''' + ')'
        INSERT INTO @ColumnNameTable
        EXEC (@sql)
        WHILE EXISTS (SELECT TOP 1 COLUMN_NAME FROM @ColumnNameTable)
        BEGIN
            PRINT @ColumnName
            SELECT TOP 1 @ColumnName = COLUMN_NAME,@ColumnType = DATA_TYPE FROM @ColumnNameTable
            SET @sql = 'SELECT ''' + @TableName + ''',''' + @ColumnName + ''',' + CASE @ColumnType WHEN 'xml' THEN 'LEFT(CAST(' + @ColumnName + ' AS nvarchar(MAX)), 4096),''' 
            WHEN 'timestamp' THEN 'master.dbo.fn_varbintohexstr('+ @ColumnName + '),'''
            ELSE 'LEFT(' + @ColumnName + ', 4096),''' END + @ColumnType + ''' 
                    FROM ' + @TableName + ' (NOLOCK) ' +
                    ' WHERE ' + CASE @ColumnType WHEN 'xml' THEN 'CAST(' + @ColumnName + ' AS nvarchar(MAX))' 
                    WHEN 'timestamp' THEN 'master.dbo.fn_varbintohexstr('+ @ColumnName + ')'
                    ELSE @ColumnName END + ' LIKE ' + @QuotedSearchStrColumnValue
            INSERT INTO #Results
            EXEC(@sql)            
            DELETE FROM @ColumnNameTable WHERE COLUMN_NAME = @ColumnName
        END 
    END
END
SET NOCOUNT OFF

SELECT TableName, ColumnName, ColumnValue, ColumnType, COUNT(*) AS Count FROM #Results
GROUP BY TableName, ColumnName, ColumnValue, ColumnType
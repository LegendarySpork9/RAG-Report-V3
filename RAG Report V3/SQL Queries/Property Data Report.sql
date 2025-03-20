declare @InstanceHealth table 
(
Data_Endpoint varchar(255),
Created_24Hours int,
Updated_24Hours int
)

declare @ProductReceivedDate table 
(
LastReceivedDate varchar(255)
)

declare @PropertySQL VARCHAR(MAX) = REPLACE(REPLACE('SELECT (SELECT ''PRODUCT''), 
(SELECT COUNT(*) as Created FROM [{Server}].[{Database}].dbo.Table with (nolock) WHERE DATECREATED >= DATEADD(HH, -24, GETDATE()))
, (SELECT COUNT(*) as Modified FROM [{Server}].[{Database}].dbo.Table with (nolock) WHERE LASTMODIFIED >= DATEADD(HH, -24, GETDATE()))', '{Server}', @dbs), '{Database}', @db)

Insert Into @InstanceHealth
EXEC (@PropertySQL)

declare @DateSQL VARCHAR(MAX) = REPLACE(REPLACE('select max(cast(LastAppearedInFeed as date)) as ReceivedDate from [{Server}].[{Database}].dbo.Table with (nolock)', '{Server}', @dbs)
, '{Database}', @db)

INSERT INTO @ProductReceivedDate
EXEC (@DateSQL)

SELECT LastReceivedDate, null, null FROM @ProductReceivedDate
UNION ALL
SELECT Data_Endpoint, Created_24Hours, Updated_24Hours FROM @InstanceHealth
order by 1 asc
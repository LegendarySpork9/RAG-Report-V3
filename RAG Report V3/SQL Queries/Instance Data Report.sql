declare @InstanceHealth table 
(
Data_Endpoint varchar(255),
Created_24Hours int,
Updated_24Hours int
)

declare @IntegrationDate table 
(
LastIntDate varchar(255)
)

declare @ContactSQL VARCHAR(MAX) = REPLACE(REPLACE('SELECT (SELECT ''CONTACT''), 
(SELECT COUNT(*) as Created FROM [{Server}].[{Database}].dbo.Table with (nolock) WHERE DATECREATED >= DATEADD(HH, -24, GETDATE()) ^)
, (SELECT COUNT(*) as Modified FROM [{Server}].[{Database}].dbo.Table with (nolock) WHERE LASTMODIFIED >= DATEADD(HH, -24, GETDATE()) ^)', '{Server}', @dbs), '{Database}', @db)

INSERT INTO @InstanceHealth
EXEC (@ContactSQL)

declare @ROSQL VARCHAR(MAX) = REPLACE(REPLACE('SELECT UPPER(Name), Isnull(Amount, 0) Amount_Created, ISNULL(Amount_M, 0) Amount_Modified FROM [{Server}].[{Database}].dbo.TableTwo with (nolock)
LEFT JOIN
(
SELECT DISTINCT RelatedObjectClass, Count(*) as Amount from [{Server}].[{Database}].dbo.TableThree with (nolock) WHERE DATECREATED >= DATEADD(HH, -24, Getdate()) and ContactID in (select ContactID from [{Server}].[{Database}].dbo.Table with (nolock) where ContactId is not null ^) Group By RelatedObjectClass
) RelatedObject On InstanceTypeName = RelatedObjectClass

LEFT JOIN
(
SELECT DISTINCT RelatedObjectClass, Count(*) as Amount_M from [{Server}].[{Database}].dbo.TableThree with (nolock) WHERE LASTMODIFIED >= DATEADD(HH, -24, Getdate()) and ContactID in (select ContactID from [{Server}].[{Database}].dbo.Table with (nolock) where ContactId is not null ^) Group By RelatedObjectClass
) Modified On InstanceTypeName = Modified.RelatedObjectClass

WHERE PropertyType = 7
AND Name not in (
''Referral'',
''OnlineValuations'',
''RTProperties'',
''RTViewings'',
''RTAppraisals'',
''RTTenancies'',
''RTOffers'',
''RTSales'',
''RoiScores'',
''ExternalLead'',
''ExternalAccount''
)', '{Server}', @dbs), '{Database}', @db)

Insert Into @InstanceHealth
EXEC (@ROSQL)

declare @DateSQL VARCHAR(MAX) = REPLACE(REPLACE('select max(cast(LastModified as date)) as IntegrationDate from [{Server}].[{Database}].dbo.Table with (nolock) where Email not like ''%@briefyourmarket.%'' and Email not like ''%@nurtur.%'' ^', '{Server}', @dbs)
, '{Database}', @db)

INSERT INTO @IntegrationDate
EXEC (@DateSQL)

SELECT LastIntDate, null, null FROM @IntegrationDate
UNION ALL
SELECT Data_Endpoint, Created_24Hours, Updated_24Hours FROM @InstanceHealth
where Data_Endpoint in (
'CONTACT',
|
)
order by 1 asc
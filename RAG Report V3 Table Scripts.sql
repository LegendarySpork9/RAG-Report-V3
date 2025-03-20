use BYMReporting

--Error Table Creation
CREATE TABLE [dbo].[Error](
	[ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[Application] [varchar](100) NOT NULL,
	[Summary] [varchar](255) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Errors] PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] COMMIT

--Exclusion Table Creation
CREATE TABLE [dbo].[Exclusion](
	[ExclusionID] [int] IDENTITY(1,1) NOT NULL,
	[InstanceID] [int] NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[ExcludeTillDate] [date] NOT NULL,
	[ExclusionReason] [varchar](40) NOT NULL,
	[Comment] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Exclusion] PRIMARY KEY CLUSTERED 
(
	[ExclusionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Exclusion]  WITH CHECK ADD  CONSTRAINT [FK_Exclusion_Instance] FOREIGN KEY([InstanceID])
REFERENCES [dbo].[Instance] ([InstanceID])
GO

ALTER TABLE [dbo].[Exclusion] CHECK CONSTRAINT [FK_Exclusion_Instance]
GO
COMMIT

--Instance Table Creation
CREATE TABLE [dbo].[Instance](
	[InstanceID] [int] NOT NULL,
	[URL] [varchar](255) NOT NULL,
	[Status] [varchar](30) NOT NULL,
	[ActiveTriggers] [int] NOT NULL,
 CONSTRAINT [PK_Instance] PRIMARY KEY CLUSTERED 
(
	[InstanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] COMMIT

--InstanceData Table Creation
CREATE TABLE [dbo].[InstanceData](
	[InstanceDataID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[InstanceInfoID] [int] NOT NULL,
	[Endpoint] [varchar](100) NOT NULL,
	[Created] [int] NOT NULL,
	[Modified] [int] NOT NULL,
 CONSTRAINT [PK_InstanceData] PRIMARY KEY CLUSTERED 
(
	[InstanceDataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InstanceData]  WITH CHECK ADD  CONSTRAINT [FK_InstanceData_InstanceInformation] FOREIGN KEY([InstanceInfoID])
REFERENCES [dbo].[InstanceInformation] ([InstanceInfoID])
GO

ALTER TABLE [dbo].[InstanceData] CHECK CONSTRAINT [FK_InstanceData_InstanceInformation]
GO
COMMIT

--InstanceInformation Table Creation
CREATE TABLE [dbo].[InstanceInformation](
	[InstanceInfoID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[InstanceID] [int] NOT NULL,
	[IntegrationID] [int] NULL,
	[PropertyFeedID] [int] NULL,
	[LastIntegrationDate] [date] NULL,
	[LastFeedReceived] [date] NULL,
	[RAGStatusID] [int] NOT NULL,
	[SetUpDate] [date] NOT NULL,
 CONSTRAINT [PK_InstanceInformation] PRIMARY KEY CLUSTERED 
(
	[InstanceInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InstanceInformation]  WITH CHECK ADD  CONSTRAINT [FK_InstanceInformation_Instance] FOREIGN KEY([InstanceID])
REFERENCES [dbo].[Instance] ([InstanceID])
GO

ALTER TABLE [dbo].[InstanceInformation] CHECK CONSTRAINT [FK_InstanceInformation_Instance]
GO

ALTER TABLE [dbo].[InstanceInformation]  WITH CHECK ADD  CONSTRAINT [FK_InstanceInformation_Integration] FOREIGN KEY([IntegrationID])
REFERENCES [dbo].[Integration] ([IntegrationID])
GO

ALTER TABLE [dbo].[InstanceInformation] CHECK CONSTRAINT [FK_InstanceInformation_Integration]
GO

ALTER TABLE [dbo].[InstanceInformation]  WITH CHECK ADD  CONSTRAINT [FK_InstanceInformation_PropertyFeed] FOREIGN KEY([PropertyFeedID])
REFERENCES [dbo].[PropertyFeed] ([PropertyFeedID])
GO

ALTER TABLE [dbo].[InstanceInformation] CHECK CONSTRAINT [FK_InstanceInformation_PropertyFeed]
GO

ALTER TABLE [dbo].[InstanceInformation]  WITH CHECK ADD  CONSTRAINT [FK_InstanceInformation_Status] FOREIGN KEY([RAGStatusID])
REFERENCES [dbo].[RAGStatus] ([StatusID])
GO

ALTER TABLE [dbo].[InstanceInformation] CHECK CONSTRAINT [FK_InstanceInformation_Status]
GO
COMMIT

--Integration Table Creation
CREATE TABLE [dbo].[Integration](
	[IntegrationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Type] [varchar](100) NOT NULL,
	[UniqueFields] [varchar](max) NULL,
	[Source] [varchar](50) NULL,
 CONSTRAINT [PK_Integration] PRIMARY KEY CLUSTERED 
(
	[IntegrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] COMMIT

--Integration Table Population
SET IDENTITY_INSERT [dbo].[Integration] ON 
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (126, N'Aspasia', N'CRM Gateway', N'AspasiaId^Contracts', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (127, N'SolexIntegration', N'-=Unknown=-', N'SolexUpdateKey^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (128, N'Undefined', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (129, N'Estates IT', N'External Integration Manager', N'CLCLASS^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (130, N'DezRez (V3)', N'CRM Gateway', N'CustomerId^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (131, N'ReapIT', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (132, N'OLD_Vebra Live (Related Object Model)', N'-=Unknown=-', N'LiveClientID^Properties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (133, N'SSP Electra M3 and Select', N'Internal Integration Manager', N'Contracts', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (134, N'CFP Winman', N'External Integration Manager', N'CFPClientType^CFPTenancyProperties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (135, N'LSLi', N'-=Unknown=-', N'LSLBranchCompanyName^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (136, N'Expert Agent', N'WebService - They Call Us', N'ExpertAgentStatus^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (137, N'Alcium (Agency Pilot)', N'Internal Integration Manager', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (138, N'Gladstone API 2016 (2)-old', N'CRM Gateway', N'GSMemberID^Attendances,Subscriptions,Bookings,Sales,RelatedMember,SalesAggregate,BookingsAggregateAttended,BookingsAggregateUnattended,AttendanceAggregate', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (139, N'DezRezDynamics', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (140, N'Your Move Franchise', N'-=Unknown=-', N'LSLBranchCompanyName^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (141, N'OLD_Expert_Agent', N'WebService - They Call Us', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (142, N'Jupix V2', N'CRM Gateway', N'JupixV2ContactID^SalesAppraisals,LettingsAppraisals,SalesViewings,LettingsViewings,Tenancies,Sales,Offers,LettingsProperties,SalesProperties,MaintenanceJobs,Inspections,ContactPeople,PotentialOwner', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (143, N'Aspasia With Viewings', N'CRM Gateway', N'AspasiaId^Contracts,Viewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (144, N'LIVE_SSP_Pure', N'-=Unknown=-', N'PureCode^Policies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (145, N'Guestline', N'-=Unknown=-', N'GuestlineReservations,GuestlineReservationProducts,GuestlineReservationProductTotals,GuestlineFinancialTransactions', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (146, N'Vebra Alto', N'WebService - They Call Us', N'VebraAltoAppraisals,VebraAltoOffers,VebraAltoProperties,VebraAltoSales,VebraAltoTenancies,VebraAltoViewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (147, N'LIVE_VebraPremise', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (148, N'Auction House', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (149, N'RPS12 (With Related Objects)', N'WebService - They Call Us', N'ReapitStatus^ReapitLandlordProperties,ReapitSecondVendors,ReapitSecondApplicants,ReapitSecondLandlords,ReapitCertificates,ReapitDiaries,Properties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (150, N'Live_RentMan', N'-=Unknown=-', N'Properties,Certs,ApplicantDetails,RentmanOffers,RentmanTenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (151, N'LIVE_Expert_Agent_oldnew_Combined', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (152, N'IT-Proz - Related Objects', N'-=Unknown=-', N'landlordvaluations,landlordtenancies,landlordproperties,landlordagentinspections,landlordgasinspections,tenanttenancies,tenantviewings,applicantoffers,vendoroffers,vendorproperties,vendorvaluations', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (153, N'Let MC - AgentOS', N'Internal Integration Manager', N'LetMCBranchName^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (154, N'Vebra Live (Related Objects)', N'External Integration Manager', N'LiveClientID^Properties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (155, N'LIVE_Veco_2015', N'-=Unknown=-', N'Viewings,Inspections,Offers,Properties,Tenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (156, N'Dynamics 2013 with tasks', N'-=Unknown=-', N'Task,AccountTasks,LeadTasks,Appointments', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (157, N'Douglas&Gordon_Live_2015', N'-=Unknown=-', N'landlordproperties,tenantproperties,lettingsrequirements,salesrequirements,vendorproperties,buyerproperties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (158, N'Legend_CSV', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (159, N'Vebra Alto (with RelatedObjects)', N'WebService - They Call Us', N'VebraAltoContactType^SalesProperties,LettingsProperties,Tenancies,Valuations,Offers,Sales,Inspections,LettingsViewings,SalesViewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (160, N'ESP', N'-=Unknown=-', N'ESPAction^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (161, N'Import Only', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (162, N'Aspasia With Property Feed and Viewings', N'CRM Gateway', N'AspasiaId^Contracts,Viewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (163, N'Sportsoft', N'-=Unknown=-', N'SportsoftCompanyName^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (164, N'Gladstone API 2016', N'-=Unknown=-', N'GladstoneStatusID^Attendances,Subscriptions,Bookings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (165, N'Brightside2016', N'-=Unknown=-', N'BrightsidePolicy', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (166, N'RPS12 (With Related Objects)2016', N'WebService - They Call Us', N'Branch^Properties,ReapitLandlordProperties,ReapitSecondVendors,ReapitCertificates,ReapitDiaries,ReapitSecondApplicants,ReapitSecondLandlords', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (167, N'Sharpsmart_Salesforce', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (168, N'Veco Fetherstone Leigh 2016', N'-=Unknown=-', N'Viewings,Inspections,Offers,Properties,Tenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (169, N'DezRezRezi', N'CRM Gateway', N'ReziCompanyName^ReziProperty,ReziAppraisal,ReziSale,ReziViewing,ReziOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (170, N'Bright Logic IM - Acquaint', N'WebService - They Call Us', N'BrightPersonalType^SalesProperties,LettingsProperties,Tenancies,Offers,Sales,Appointments', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (171, N'Gladstone API 2017', N'-=Unknown=-', N'GladstoneSiteID^Attendances,Subscriptions,Bookings,Sales,RelatedMember,SalesAggregate,AttendanceAggregate,BookingsAggregateAttended,BookingsAggregateUnattended', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (172, N'Freedom Leisure', N'-=Unknown=-', N'FreedomMembershipsHomesite^ActivitiesProperties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (173, N'Veco_2016 v2', N'-=Unknown=-', N'Viewings,Inspections,Offers,VecoProperties,Tenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (174, N'XN Leisure Lite - 2017', N'-=Unknown=-', N'XNName^XNRelatedMembers,XNMemberships,XNMemberDebts', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (175, N'ReziBasic', N'CRM Gateway', N'ReziBranch^REZIPropertyForSale,REZIPropertyForLet,REZIEvent', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (176, N'SSP Electra (Version 7) For CIA', N'Internal Integration Manager', N'SSPSectionName^Contracts', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (177, N'DezRez (V3) With Related Object', N'CRM Gateway', N'PropertyData', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (178, N'MishonMackay', N'-=Unknown=-', N'MmViewings,MmValuations', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (179, N'InternalCRM News', N'-=Unknown=-', N'CompanyName^Appointment', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (180, N'RentMan Trigger Integration', N'-=Unknown=-', N'company^ImportProperties,ImportOffers', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (181, N'ProductMatching-Reapit', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (182, N'LIVE_SSP_Pure_Endsleigh', N'-=Unknown=-', N'PureCode^Policies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (183, N'Veco_2016 v2 Century21 Only', N'External Integration Manager', N'VecoViewings,VecoInspections,VecoOffers,VecoProperties,VecoTenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (184, N'OpenGI InfoCentrePlus 2', N'External Integration Manager', N'OGIClientRefNo^OGIGeneralPolicy,OGIQuotes', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (185, N'MWU-UniversalSchema', N'-=Unknown=-', N'MWUAgentID^MWUValuations,MWUSaleViewings,MWUPurchaseViewings,MWUSales,MWUPurchases,MWUSaleOffer,MWUPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (186, N'*** Reapit Outbound 2018 ***', N'WebService - They Call Us', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (187, N'Acturis SW4', N'-=Unknown=-', N'ActurisClientKey^ActurisGenericPolicy,ActurisQuote', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (188, N'TCP-UniversalSchema', N'-=Unknown=-', N'TCPAgentID^TCPValuations,TCPSaleViewings,TCPPurchaseViewings,TCPSales,TCPPurchases,TCPSaleOffer,TCPPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (189, N'DO NOT USE. PARTNER ''CLIENTS OWN'' MEANS NO ROI DASHBOARD. PLEASE USE PROPERTY SCHEMA', N'Internal Integration Manager', N'BYMAgentID^BYMValuations,BYMSaleViewings,BYMPurchaseViewings,BYMSales,BYMPurchases,BYMSaleOffer,BYMPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (190, N'Brightside Data Merge 2018', N'-=Unknown=-', N'BrightPolicies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (191, N'Acturis Custom Integration Fields: Allianz-Policy', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (192, N'EPROP-UniversalSchema', N'-=Unknown=-', N'BYMAgentID^BYMValuations,BYMSaleViewings,BYMPurchaseViewings,BYMSales,BYMPurchases,BYMSaleOffer,BYMPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (193, N'Rps12/Jet MWU 2019', N'-=Unknown=-', N'Rps12JetOfficeId^Rps12JetViewing,Rps12JetDiaryEntry,Rps12JetApplicantOffer,Rps12JetGeneralProperty,Rps12JetOffice', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (194, N'Norton-ib Custom Fields', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (195, N'Auction House - CRM Gateway', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (196, N'PropCo Integration', N'Internal Integration Manager', N'PropCoContractType^PropCoProperty', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (197, N'Gnomen', N'WebService - They Call Us', N'GnomenLabel^SalesProperties,LettingsProperties,Tenancies,Valuations,Offers,Sales,Inspections,LettingsViewings,SalesViewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (198, N'Reapit API', N'WebService - They Call Us', N'ReapitAPIMarketingOptIn^ReapitAPISearchCriteria,ReapitAPIProperties,ReapitAPIOffers,ReapitAPIEvents,ReapitAPITenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (199, N'Guestline_2019_LANsync', N'-=Unknown=-', N'GuestlineReservations,GuestlineReservationProducts,GuestlineReservationProductTotals,GuestlineFinancialTransactions', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (200, N'Endsleigh Legacy CSV', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (201, N'Rezi', N'CRM Gateway', N'ReziBranch^REZIPropertyForSale,REZIPropertyForLet,REZIEvent', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (202, N'Andrews Online', N'-=Unknown=-', N'Offers,SearchCriteria,Viewings,Valuations,Tenancies,Properties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (203, N'Acturis Custom Integration Fields: Griffiths and Armour', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (204, N'Insurance Calendar', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (205, N'BrightLogic - Acquaint', N'WebService - They Call Us', N'BrightPersonalType^SalesProperties,LettingsProperties,Tenancies,Offers,Sales,Appointments', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (206, N'BYM Test Integration', N'-=Unknown=-', N'TestRelatedObjects', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (207, N'Acturis Custom Integration Fields: Allianz-Prospect FULL', N'CRM Gateway', N'AllianzID^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (208, N'Acturis Custom Integration Fields: Aston Lark', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (209, N'SDL-UniversalSchema', N'-=Unknown=-', N'SDLValuations,SDLSaleViewings,SDLPurchaseViewings,SDLSales,SDLPurchases,SDLSaleOffer,SDLPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (210, N'All Med Custom Insurance Calendar', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (211, N'Veco IM - Linley and Simpson Only', N'External Integration Manager', N'VecoBranchName^VecoValuations,VecoProperties,VecoOffers,VecoViewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (212, N'BriefYourMarket Schema - Estate Agent', N'-=Unknown=-', N'BYMEAContactBranchID^BYMEAProperties,BYMEAViewings,BYMEAOffers,BYMEAValuations,BYMEASearchCriterias,BYMEATenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (213, N'Bateman Custom Insurance Calendar', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (214, N'Veco_2020 v1 Standard', N'External Integration Manager', N'VecoContactType^VecoViewings,VecoInspections,VecoOffers,VecoProperties,VecoTenancies,VecoValuations', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (215, N'Fairer Business Energy - Import Only Integrations', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (216, N'Acturis Custom Integration Fields: Chas Insurance Prospects', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (217, N'Custom Integration: Orlando Reid - ASPX Buttons', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (218, N'TransformerTest', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (219, N'Jupix V3', N'CRM Gateway', N'JupixV2ContactID^SalesAppraisals,LettingsAppraisals,SalesViewings,LettingsViewings,Tenancies,Sales,Offers,LettingsProperties,SalesProperties,MaintenanceJobs,Inspections,ContactPeople,PotentialOwner', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (220, N'Custom Integration: Belvoir Lettings Hitchin - Rent Guarantee Renewal', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (221, N'Yomdel Integration Fields', N'-=Unknown=-', N'YomCompany^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (222, N'Custom Integration: Redkey Property Services - IFrame Fields', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (223, N'Acturis V12 Toolkit142 DataDictionary512', N'CRM Gateway', N'ActurisGenericPolicy,ActurisQuote', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (224, N'AgentPro Integration Fields', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (225, N'BriefYourMarket Schema - Insurance', N'-=Unknown=-', N'ClientKey^GenericPolicy,Quote', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (226, N'Acturis Custom Integration Fields: PIB Fish', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (227, N'RealCube Media - Unique Fields', N'WebService - They Call Us', N'RealCubeClientType^RCProperties', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (228, N'Estate Craft - 44', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (229, N'Estate Craft (EstateCraft-safe names) 62', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (230, N'Estate Craft (Related Object Model) 75', N'-=Unknown=-', N'EstateCraftApplicants,EstateCraftVendors,EstateCraftOccupiers', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (231, N'Reapit API FullDW', N'CRM Gateway', N'ReapitAPIOfficeId^ReapitAPISearchCriteria,ReapitAPIProperties,ReapitAPIOffers,ReapitAPIEvents,ReapitAPITenancies', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (232, N'Bluecoat Custom Integration Fields', N'-=Unknown=-', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (233, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Custom')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (234, N'Acturis V12 Toolkit142 DataDictionary512 OPP RO', N'CRM Gateway', N'ActurisClientKey^ActurisGenericPolicy,ActurisQuote,ActurisOpportunity', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (235, N'Acturis Custom Integration Fields: Swinton', N'CRM Gateway', NULL, NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (236, N'PFG-UniversalSchema', N'-=Unknown=-', N'PFGJxContactName^PFGValuations,PFGSaleViewings,PFGPurchaseViewings,PFGSales,PFGPurchases,PFGSaleOffer,PFGPurchaseOffer,PFGTenancies,PFGLettingValuations,PFGLettingApplicantViewing,PFGLettingsProperty,PFGLettingsViewing,PFGSalesProperty', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (237, N'MoveButler-UniversalSchema', N'-=Unknown=-', N'MoveButlerAgentID^MoveButlerValuations,MoveButlerSaleViewings,MoveButlerPurchaseViewings,MoveButlerSales,MoveButlerPurchases,MoveButlerSaleOffer,MoveButlerPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (238, N'Property Franchise Group Custom Integration Fields', N'CRM Gateway', N'PFGCustomIntContType^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (239, N'FruitfulMortgages-UniversalSchema', N'-=Unknown=-', N'FMValuations,FMSaleViewings,FMPurchaseViewings,FMSales,FMPurchases,FMSaleOffer,FMPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (240, N'MoveWithUs Schema', N'CRM Gateway', N'MWUPartnerSource^MWUValuations,MWUSaleViewings,MWUPurchaseViewings,MWUSales,MWUPurchases,MWUSaleOffer,MWUPurchaseOffer', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (241, N'Old Property Schema (Temporary)', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (242, N'Maple Brook Wills Integration', N'-=Unknown=-', N'MapleBrookWillsPersonID^MapleBrookWillsProduct', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (243, N'Property Schema (Temporary Fix)', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (244, N'Acturis V13 Toolkit142 DataDictionaryv7.6.3 OPP RO', N'CRM Gateway', N'ActurisClientKey^ActurisGenericPolicy,ActurisQuote,ActurisOpportunity', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (245, N'Property Schema - Gazeal', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (246, N'Acturis V13 Toolkit142 Extra Fields', N'CRM Gateway', N'ActurisGenericPolicy,ActurisQuote,ActurisOpportunity', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (247, N'Vebra Alto (with RelatedObjects) GDPR', N'WebService - They Call Us', N'SalesProperties,LettingsProperties,Tenancies,Valuations,Offers,Sales,Inspections,LettingsViewings,SalesViewings', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (248, N'Acturis V13 Toolkit142 DDv7.6.3 OPP RO PIB', N'CRM Gateway', N'ActurisClientKey^ActurisGenericPolicy,ActurisQuote,ActurisOpportunity', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (249, N'Mortgage Schema', N'Internal Integration Manager', N'MortgageSchemaContactBranches^MortgageSchemaProperty', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (250, N'Preview Mortgage Schema Addon', N'Internal Integration Manager', N'MortgageSchemaBrand^', NULL)
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1002, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Reapit Foundations')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1003, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Sme Professional')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1004, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Jupix')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1005, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Vtuk Openview')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1006, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'DezRez Rezi')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1007, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Property File Drop')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1008, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Mri Software')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1009, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Apex27')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1010, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'PropertyBase')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1011, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Mapped File Drop')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1012, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Rex Software')
GO
INSERT [dbo].[Integration] ([IntegrationID], [Name], [Type], [UniqueFields], [Source]) VALUES (1013, N'Property Schema', N'CRM Gateway', N'PropertySchemaContactType^PropertySchemaProperties,PropertySchemaAppointments,PropertySchemaValuations,PropertySchemaViewings,PropertySchemaOffers,PropertySchemaTenancies,PropertySchemaSearchCriteria,PropertySchemaReferrals', N'Street')
GO
SET IDENTITY_INSERT [dbo].[Integration] OFF
GO
COMMIT

--IntegrationIssue Table Creation
CREATE TABLE [dbo].[IntegrationIssue](
	[IntegrationIssuesID] [int] IDENTITY(1,1) NOT NULL,
	[InstanceID] [int] NOT NULL,
	[IntegrationID] [int] NOT NULL,
	[InstanceInfoID] [int] NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_IntegrationIssues] PRIMARY KEY CLUSTERED 
(
	[IntegrationIssuesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[IntegrationIssue]  WITH CHECK ADD  CONSTRAINT [FK_IntegrationIssue_Instance] FOREIGN KEY([InstanceID])
REFERENCES [dbo].[Instance] ([InstanceID])
GO

ALTER TABLE [dbo].[IntegrationIssue] CHECK CONSTRAINT [FK_IntegrationIssue_Instance]
GO

ALTER TABLE [dbo].[IntegrationIssue]  WITH CHECK ADD  CONSTRAINT [FK_IntegrationIssue_InstanceInformation] FOREIGN KEY([InstanceInfoID])
REFERENCES [dbo].[InstanceInformation] ([InstanceInfoID])
GO

ALTER TABLE [dbo].[IntegrationIssue] CHECK CONSTRAINT [FK_IntegrationIssue_InstanceInformation]
GO

ALTER TABLE [dbo].[IntegrationIssue]  WITH CHECK ADD  CONSTRAINT [FK_IntegrationIssue_Integration] FOREIGN KEY([IntegrationID])
REFERENCES [dbo].[Integration] ([IntegrationID])
GO

ALTER TABLE [dbo].[IntegrationIssue] CHECK CONSTRAINT [FK_IntegrationIssue_Integration]
GO
COMMIT

--PropertyFeed Table Creation
CREATE TABLE [dbo].[PropertyFeed](
	[PropertyFeedID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Type] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PropertyFeeds] PRIMARY KEY CLUSTERED 
(
	[PropertyFeedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] COMMIT

--PropertyFeed Table Population
SET IDENTITY_INSERT [dbo].[PropertyFeed] ON 
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (1, N'LSL', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (2, N'Property Product Schema', N'CRM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (3, N'Zipped BLM', N'CRM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (4, N'Aspasia', N'CRM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (5, N'ResiAnalytics', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (6, N'Rentman', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (7, N'ReapIT', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (8, N'Zipped BLM', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (9, N'Property Software Group Zipped BLM', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (10, N'Property Product Schema', N'BLM')
GO
INSERT [dbo].[PropertyFeed] ([PropertyFeedID], [Name], [Type]) VALUES (11, N'BLM', N'BLM')
GO
SET IDENTITY_INSERT [dbo].[PropertyFeed] OFF
GO
COMMIT

--PropertyIssue Table Creation
CREATE TABLE [dbo].[PropertyIssue](
	[PropertyIssuesID] [int] IDENTITY(1,1) NOT NULL,
	[InstanceID] [int] NOT NULL,
	[PropertyFeedID] [int] NOT NULL,
	[InstanceInfoID] [int] NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK_PropertyIssues] PRIMARY KEY CLUSTERED 
(
	[PropertyIssuesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PropertyIssue]  WITH CHECK ADD  CONSTRAINT [FK_PropertyIssue_Instance] FOREIGN KEY([InstanceID])
REFERENCES [dbo].[Instance] ([InstanceID])
GO

ALTER TABLE [dbo].[PropertyIssue] CHECK CONSTRAINT [FK_PropertyIssue_Instance]
GO

ALTER TABLE [dbo].[PropertyIssue]  WITH CHECK ADD  CONSTRAINT [FK_PropertyIssue_InstanceInformation] FOREIGN KEY([InstanceInfoID])
REFERENCES [dbo].[InstanceInformation] ([InstanceInfoID])
GO

ALTER TABLE [dbo].[PropertyIssue] CHECK CONSTRAINT [FK_PropertyIssue_InstanceInformation]
GO

ALTER TABLE [dbo].[PropertyIssue]  WITH CHECK ADD  CONSTRAINT [FK_PropertyIssue_PropertyFeed] FOREIGN KEY([PropertyFeedID])
REFERENCES [dbo].[PropertyFeed] ([PropertyFeedID])
GO

ALTER TABLE [dbo].[PropertyIssue] CHECK CONSTRAINT [FK_PropertyIssue_PropertyFeed]
GO
COMMIT

--RAGStatus Table Creation
CREATE TABLE [dbo].[RAGStatus](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Description] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] COMMIT

--RAGStatus Table Population
SET IDENTITY_INSERT [dbo].[RAGStatus] ON 
GO
INSERT [dbo].[RAGStatus] ([StatusID], [Name], [Description]) VALUES (1, N'DarkRed', N'No Data Created or Modified with Triggers')
GO
INSERT [dbo].[RAGStatus] ([StatusID], [Name], [Description]) VALUES (2, N'Red', N'No Data Created or Modified')
GO
INSERT [dbo].[RAGStatus] ([StatusID], [Name], [Description]) VALUES (3, N'Yellow', N'No Data Created Today')
GO
INSERT [dbo].[RAGStatus] ([StatusID], [Name], [Description]) VALUES (4, N'Green', N'Data Created or Modified Today')
GO
INSERT [dbo].[RAGStatus] ([StatusID], [Name], [Description]) VALUES (5, N'DarkViolet', N'Excluded')
GO
SET IDENTITY_INSERT [dbo].[RAGStatus] OFF
GO
COMMIT
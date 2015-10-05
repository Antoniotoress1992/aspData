
alter table AdjusterMaster add DeploymentAddress varchar(100)
alter table AdjusterMaster add DeploymentStateID int
alter table AdjusterMaster add DeploymentCity varchar(50)
alter table AdjusterMaster add DeploymentZipCode varchar(10)
alter table dbo.AdjusterMaster add isW9 bit
alter table dbo.AdjusterMaster add GeographicalSeriveArea varchar(100)
alter table dbo.AdjusterMaster add FirstName varchar(50)
alter table dbo.AdjusterMaster add LastName varchar(50)
alter table dbo.AdjusterMaster add YearsExperiece int
alter table dbo.AdjusterMaster add Certifications varchar(100)

alter table AdjusterMaster add MaxClaimNumber int
alter table AdjusterMaster add PhotoFileName varchar(50)
alter table AdjusterMaster add CityName varchar(50)
alter table AdjusterMaster add UseDeploymentAddress bit


ALTER TABLE dbo.AdjusterMaster
ADD CONSTRAINT fk_AdjusterMaster_DeploymentState
FOREIGN KEY (DeploymentStateID)
REFERENCES dbo.StateMaster(StateID) 

update am
set cityname = c.cityname
from AdjusterMaster am
inner join dbo.CityMaster c on c.CityId = am.Cityname
where am.cityname is not null



update am
set am.ZipCode = z.ZipCode
from AdjusterMaster am
inner join dbo.ZipCodeMaster z on z.ZipCodeID = am.ZipCode
where am.ZipCode is not null

alter table Contact add DepartmentName varchar(50)
alter table Contact add ContactTitle varchar(50)
alter table Contact add CarrierID int

alter table LeadContactType add ClientID int


alter table LeadInvoice add PolicyID int
alter table LeadInvoice add AdjusterID int

ALTER TABLE dbo.LeadInvoiceDetail
ADD CONSTRAINT fk_LeadInvoiceDetail_ServiceType
FOREIGN KEY (ServiceTypeID)
REFERENCES dbo.InvoiceServiceType(ServiceTypeID)

alter table LeadPolicy add CarrierID int
alter table LeadPolicy add SublimitLiabilityID int


ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_Mortgagee
FOREIGN KEY (MortgageeID)
REFERENCES dbo.Mortgagee(MortgageeID)


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Carrier](
	[CarrierID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierName] [varchar](100) NOT NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](50) NULL,
	[ClientID] [int] NULL,
	[CityID] [int] NULL,
	[StateID] [int] NULL,
	[ZipCodeID] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[InsertDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Carrier] PRIMARY KEY CLUSTERED 
(
	[CarrierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_Carrier
FOREIGN KEY (CarrierID)
REFERENCES dbo.Carrier(CarrierID)


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SubLimitOfLiabilityMaster](
	[SublimitLiabilityID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[ClientId] [int] NULL,
 CONSTRAINT [PK_SubLimitOfLiability] PRIMARY KEY CLUSTERED 
(
	[SublimitLiabilityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_SubLimitOfLiabilityMaster
FOREIGN KEY (SublimitLiabilityID)
REFERENCES dbo.SubLimitOfLiabilityMaster(SublimitLiabilityID)

alter table [LeadPolicyCoverage] add
[DwellingLimit] [money] NULL,
	[DwellingACV] [money] NULL,
	[DwellingRCV] [money] NULL,
	[DwellingOverage] [money] NULL,
	[BuildingLimit] [money] NULL,
	[BuildingACV] [money] NULL,
	[BuildingRCV] [money] NULL,
	[BuildingOverage] [money] NULL,
	[ContentsLimit] [money] NULL,
	[ContentsACV] [money] NULL,
	[ContentsRCV] [money] NULL,
	[ContentsOverage] [money] NULL
	
alter table LeadsDocument add IsPrint bit

alter table Mortgagee add
  [AddressLine1] VARCHAR (100) NULL,
    [AddressLine2] VARCHAR (50)  NULL,
    [StateID]      INT           NULL,
    [CityID]       INT           NULL,
    [ZipCodeID]    INT           NULL,
    [Phone]        VARCHAR (20)  NULL,
    [Fax]          VARCHAR (20)  NULL
    
    
ALTER TABLE dbo.Mortgagee
ADD CONSTRAINT fk_Mortgagee_StateMaster
FOREIGN KEY (StateID)
REFERENCES dbo.StateMaster(StateID)

ALTER TABLE dbo.Mortgagee
ADD CONSTRAINT fk_Mortgagee_CityMaster
FOREIGN KEY (CityID)
REFERENCES dbo.CityMaster(CityID)


ALTER TABLE dbo.Mortgagee
ADD CONSTRAINT fk_Mortgagee_ZipCodeMaster
FOREIGN KEY (ZipCodeID)
REFERENCES dbo.ZipCodeMaster(ZipCodeID)

alter table Task add Location varchar(1)

alter table [TypeOfDamageMaster] add ClientId int

CREATE TABLE [dbo].[AdjusterHandleClaimType] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [AdjusterID]   INT NULL,
    [PolicyTypeID] INT NULL,
    CONSTRAINT [PK_AdjusterHandleClaimType] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AdjusterHandleClaimType_AdjusterMaster] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[AdjusterMaster] ([AdjusterId]),
    CONSTRAINT [FK_AdjusterHandleClaimType_LeadPolicyType] FOREIGN KEY ([PolicyTypeID]) REFERENCES [dbo].[LeadPolicyType] ([LeadPolicyTypeID])
);
GO

CREATE TABLE [dbo].[AdjusterNote] (
    [NoteID]     INT           IDENTITY (1, 1) NOT NULL,
    [AdjusterID] INT           NULL,
    [NoteDate]   DATETIME      NULL,
    [UserID]     INT           NULL,
    [Notes]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AdjusterNote] PRIMARY KEY CLUSTERED ([NoteID] ASC),
    CONSTRAINT [FK_AdjusterNote_AdjusterMaster] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[AdjusterMaster] ([AdjusterId]),
    CONSTRAINT [FK_AdjusterNote_SecUser] FOREIGN KEY ([UserID]) REFERENCES [dbo].[SecUser] ([UserId])
);
GO

CREATE TABLE [dbo].[AdjusterReference] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [AdjusterID]      INT           NULL,
    [RereferenceName] VARCHAR (100) NULL,
    [Phone]           VARCHAR (20)  NULL,
    [Email]           VARCHAR (100) NULL,
    [CompanyName]     VARCHAR (100) NULL,
    [Position]        VARCHAR (50)  NULL,
    CONSTRAINT [PK_AdjusterReference] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AdjusterReference_AdjusterMaster] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[AdjusterMaster] ([AdjusterId])
);
GO

CREATE TABLE [dbo].[AdjusterServiceArea] (
    [ID]                    INT          IDENTITY (1, 1) NOT NULL,
    [AdjusterID]            INT          NULL,
    [StateID]               INT          NULL,
    [LicenseEffectiveDate]  DATETIME     NULL,
    [LicenseExpirationDate] DATETIME     NULL,
    [LicenseNumber]         VARCHAR (50) NULL,
    CONSTRAINT [PK_AdjusterServiceArea] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_AdjusterServiceArea_AdjusterMaster] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[AdjusterMaster] ([AdjusterId]),
    CONSTRAINT [FK_AdjusterServiceArea_StateMaster] FOREIGN KEY ([StateID]) REFERENCES [dbo].[StateMaster] ([StateId])
);
GO

CREATE TABLE [dbo].[CarrierInvoiceProfile] (
    [CarrierInvoiceProfileID]     INT           IDENTITY (1, 1) NOT NULL,
    [CarrierID]                   INT           NOT NULL,
    [CarrierInvoiceProfileTypeID] INT           NULL,
    [ProfileName]                 VARCHAR (100) NOT NULL,
    [EffiectiveDate]              DATETIME      NULL,
    [ExpirationDate]              DATETIME      NULL,
    [CoverageArea]                VARCHAR (100) NULL,
    [IsActive]                    BIT           NOT NULL,
    CONSTRAINT [PK_CarrierInvoiceProfile] PRIMARY KEY CLUSTERED ([CarrierInvoiceProfileID] ASC),
    CONSTRAINT [FK_CarrierInvoiceProfile_Carrier] FOREIGN KEY ([CarrierID]) REFERENCES [dbo].[Carrier] ([CarrierID])
);
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeItemized] (
    [ID]                      INT           IDENTITY (1, 1) NOT NULL,
    [CarrierInvoiceProfileID] INT           NOT NULL,
    [ItemDescription]         VARCHAR (100) NOT NULL,
    [ItemRate]                MONEY         NOT NULL,
    [ItemPercentage]          MONEY         NOT NULL,
    [IsActive]                BIT           NOT NULL,
    CONSTRAINT [PK_CarrierInvoiceProfileFeeItemized] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CarrierInvoiceProfileFeeItemized_CarrierInvoiceProfile] FOREIGN KEY ([CarrierInvoiceProfileID]) REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
);
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeProvision] (
    [ID]                      INT           IDENTITY (1, 1) NOT NULL,
    [CarrierInvoiceProfileID] INT           NOT NULL,
    [ProvisionText]           VARCHAR (MAX) NOT NULL,
    [ProvisionAmount]         MONEY         NOT NULL,
    CONSTRAINT [PK_CarrierInvoiceProfileFeeProvision] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CarrierInvoiceProfileFeeProvision_CarrierInvoiceProfile] FOREIGN KEY ([CarrierInvoiceProfileID]) REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
);
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeSchedule] (
    [ID]                      INT   IDENTITY (1, 1) NOT NULL,
    [CarrierInvoiceProfileID] INT   NOT NULL,
    [RangeAmountFrom]         MONEY NOT NULL,
    [RangeAmountTo]           MONEY NOT NULL,
    [FlatFee]                 MONEY NOT NULL,
    [PercentFee]              MONEY NOT NULL,
    [MinimumFee]              MONEY NOT NULL,
    CONSTRAINT [PK_CarrierInvoiceProfileFeeSchedule] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CarrierInvoiceProfileFeeSchedule_CarrierInvoiceProfile] FOREIGN KEY ([CarrierInvoiceProfileID]) REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
);
GO

CREATE TABLE [dbo].[CarrierLocation] (
    [CarrierLocationID] INT           IDENTITY (1, 1) NOT NULL,
    [CarrierID]         INT           NOT NULL,
    [LocationName]      VARCHAR (100) NULL,
    [DepartmentName]    VARCHAR (100) NULL,
    [AddressLine1]      VARCHAR (50)  NULL,
    [AddressLine2]      VARCHAR (50)  NULL,
    [StateName]         VARCHAR (50)  NULL,
    [CityName]          VARCHAR (50)  NULL,
    [ZipCode]           VARCHAR (20)  NULL,
    [CountryID]         INT           NULL,
    [IsActive]          BIT           NULL,
    [StateId]           INT           NULL,
    [CityId]            INT           NULL,
    [ZipCodeId]         INT           NULL,
    CONSTRAINT [PK_CarrierLocation] PRIMARY KEY CLUSTERED ([CarrierLocationID] ASC),
    CONSTRAINT [FK_CarrierLocation_Carrier] FOREIGN KEY ([CarrierID]) REFERENCES [dbo].[Carrier] ([CarrierID]),
    CONSTRAINT [FK_CarrierLocation_CityMaster] FOREIGN KEY ([CityId]) REFERENCES [dbo].[CityMaster] ([CityId]),
    CONSTRAINT [FK_CarrierLocation_CountryMaster] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[CountryMaster] ([CountryID]),
    CONSTRAINT [FK_CarrierLocation_StateMaster] FOREIGN KEY ([StateId]) REFERENCES [dbo].[StateMaster] ([StateId])
);
GO


CREATE TABLE [dbo].[InvoiceFeeProfile] (
    [InvoiceFeeProfileID]   INT           IDENTITY (1, 1) NOT NULL,
    [InvoiceFeeProfileName] VARCHAR (100) NULL,
    [IsActive]              BIT           NULL,
    [ClientID]              INT           NULL,
    CONSTRAINT [PK_InvoiceFeeProfile] PRIMARY KEY CLUSTERED ([InvoiceFeeProfileID] ASC)
);
GO


CREATE TABLE [dbo].[LeadPolicyDamageType] (
    [ID]             INT IDENTITY (1, 1) NOT NULL,
    [PolicyID]       INT NOT NULL,
    [TypeOfDamageId] INT NOT NULL,
    CONSTRAINT [PK_LeadPolicyDamageType_1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LeadPolicyDamageType_LeadPolicy] FOREIGN KEY ([PolicyID]) REFERENCES [dbo].[LeadPolicy] ([Id]),
    CONSTRAINT [FK_LeadPolicyDamageType_TypeOfDamageMaster] FOREIGN KEY ([TypeOfDamageId]) REFERENCES [dbo].[TypeOfDamageMaster] ([TypeOfDamageId])
);
GO

CREATE TABLE [dbo].[Ledger] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [LeadID]           INT           NULL,
    [InvoiceID]        INT           NULL,
    [ClientName]       VARCHAR (100) NULL,
    [ClientPaid]       DATETIME      NULL,
    [AdjusterID]       INT           NULL,
    [AdjusterPaidDate] DATE          NULL,
    [CommissionTotal]  MONEY         NULL,
    [TotalExpenses]    MONEY         NULL,
    [AdjusterNet]      MONEY         NULL,
    [InvoiceTotal]     MONEY         NULL,
    CONSTRAINT [PK_Ledger] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Ledger_AdjusterMaster] FOREIGN KEY ([AdjusterID]) REFERENCES [dbo].[AdjusterMaster] ([AdjusterId]),
    CONSTRAINT [FK_Ledger_LeadInvoice] FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[LeadInvoice] ([InvoiceID]),
    CONSTRAINT [FK_Ledger_Leads] FOREIGN KEY ([LeadID]) REFERENCES [dbo].[Leads] ([LeadId])
);
GO

Alter view [dbo].[vw_Reminder]
as
	select 
		DueIn = datediff(minute, getdate(), [start_date]),
		t.id, t.creator_id, t.[text], t.details, t.[start_date], t.end_date, t.status_id, t.owner_id,
		t.lead_id, t.master_status_id, t.lead_policy_id, t.policy_type, t.PriorityID, t.isAllDay,
		t.ReminderInterval, t.IsReminder,
		u.Email
	from Task t
	left join SecUser u on u.UserId = t.owner_id
	--where (datediff(minute, getdate(), dateadd(Minute, -(ReminderInterval), [start_date])) < 1)				
	where 
	--	((datediff(minute, dateadd(Minute, -(ReminderInterval), [start_date]), getdate()) = 0) -- coming up		
	--	or 
	--	(datediff(minute, getdate(), [start_date]) < 0)) -- overdue
		(datediff(minute, getdate(), [start_date]) <= isnull(t.ReminderInterval,0))				
		and (IsReminder = 1)
		and (status_id = 1)	-- active/pending
GO


alter table Carrier add
  [CountryID]    INT           NULL,
    [CityName]     VARCHAR (50)  NULL,
    [StateName]    VARCHAR (50)  NULL,
    [ZipCode]      VARCHAR (20)  NULL
    

ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_CityMaster
FOREIGN KEY (cityID)
REFERENCES dbo.CityMaster(cityID)


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_ZipCodeMaster
FOREIGN KEY (ZipCodeID)
REFERENCES dbo.ZipCodeMaster(ZipCodeID)


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_Country
FOREIGN KEY (CountryID)
REFERENCES dbo.CountryMaster(CountryID) 


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_stateMaster
FOREIGN KEY (stateID)
REFERENCES dbo.StateMaster(StateId)

    
-- 2014-05-12 tortega
alter table Claim add XactNetTransactionID varchar(50)
alter table SecUser add XactNetUserID varchar(50)
alter table ClaimExpense add XactNetExpenseCode varchar(20)
alter table Claim add XactNetTransactionID varchar(50)
alter table SecUser add XactNetUserID varchar(50)
alter table ClaimExpense add XactNetExpenseCode varchar(20)


/****** Object:  Trigger [dbo].[Claim_ProgressStatusID_Updated]    Script Date: 05/12/2014 18:16:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[Claim_ProgressStatusID_Updated]
ON [dbo].[Claim]
AFTER UPDATE /* Fire this trigger when a row is UPDATEd */
AS BEGIN
	DECLARE @New_ProgressStatusID INT
	DECLARE @ClaimID INT
	
	DECLARE @Old_ProgressStatusID INT
	
	-- new progress status
	SELECT	@New_ProgressStatusID = ProgressStatusID,
			@ClaimID = ClaimID
	FROM INSERTED
  
	-- old progress status
	SELECT	@Old_ProgressStatusID = ProgressStatusID,
			@ClaimID = ClaimID
	FROM DELETED
	
  UPDATE dbo.Claim
		SET LastProgressChanged = GETDATE()
  WHERE ClaimID = @ClaimID AND @New_ProgressStatusID <> @Old_ProgressStatusID
END

GO

/****** Object:  View [dbo].[[vw_AdjusterPayout]]    Script Date: 05/12/2014 11:30:30 ******/
ALTER VIEW [dbo].[vw_AdjusterPayout]
AS
SELECT     c.AdjusterClaimNumber, c.ClaimID, am.AdjusterId, am.AdjusterName, ce.ExpenseDate, SUM(ce.ExpenseAmount) AS ExpenseAmount, sm.StatusName, l.ClientID
FROM         dbo.Claim AS c WITH (NOLOCK) INNER JOIN
                      dbo.ClaimExpense AS ce WITH (NOLOCK) ON c.ClaimID = ce.ClaimID INNER JOIN
                      dbo.ClaimService AS cs WITH (NOLOCK) ON c.ClaimID = cs.ClaimID INNER JOIN
                      dbo.AdjusterMaster AS am WITH (NOLOCK) ON ce.AdjusterID = am.AdjusterId AND cs.AdjusterID = am.AdjusterId INNER JOIN
                      dbo.StatusMaster AS sm WITH (NOLOCK) ON c.StatusID = sm.StatusId INNER JOIN
                      dbo.LeadPolicy AS lp WITH (NOLOCK) ON c.PolicyID = lp.Id INNER JOIN
                      dbo.Leads AS l WITH (NOLOCK) ON lp.LeadId = l.LeadId INNER JOIN
                      dbo.Client AS cl WITH (NOLOCK) ON l.ClientID = cl.ClientId
WHERE     (c.IsInvoiced = 1) And (c.IsActive = 1)
GROUP BY c.AdjusterClaimNumber, c.ClaimID, am.AdjusterId, am.AdjusterName, ce.ExpenseDate, cs.ServiceAmount, sm.StatusName, c.IsInvoiced, l.ClientID



/****** Object:  View [dbo].[vw_Lead_Search]    Script Date: 05/12/2014 11:30:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER view [dbo].[vw_Lead_Search]
as
select	l.ClientId,
		l.LeadId,
		l.OriginalLeadDate,
		l.InsuredName, 
		l.ClaimantFirstName, 
		l.ClaimantLastName, 
		l.BusinessName, 
		l.MailingAddress, 
		l.[Status],
		l.CityName,
		l.StateName,
		l.Zip,l.LossAddress,
		l.LossAddress2,
		l.SiteSurveyDate,
		l.LastActivityDate,
		l.MailingCity,
		l.MailingState,
		l.MailingZip,		
		ls.LeadSourceName,
		c.LossDate, 
		c.AdjusterClaimNumber,
		c.SeverityNumber,
		c.EventType,
		c.EventName,
		c.ClaimWorkflowType,
		c.CauseOfLoss,
		ClaimStatusID = isnull(c.StatusID,0),
		ClaimSubStatusID = isnull(c.SubStatusID,0),
		policyID = p.ID,
		p.PolicyNumber,
		policyTypeID = p.PolicyType,
		Coverage = policyType.[Description],
		InsuranceCompanyName = car.CarrierName,	
		car.CarrierID,
		statusMaster.StatusName, 
		substatusMaster.SubStatusName,
		adj.AdjusterName,
		AdjusterID = adj.AdjusterId,
		contractor.ContractorId,
		contractor.ContractorName,
		appraiser.AppraiserId,
		appraiser.AppraiserName,
		umpire.UmpireName,
		producer.ProducerId,
		producer.ProducerName,		
		usr.UserName,
		usr.UserID
from Leads l
inner join SecUser usr with(nolock) on usr.UserID = l.UserID
left join LeadPolicy p  with(nolock) on l.LeadID = p.LeadId and  p.isActive = 1
left join LeadPolicyType policyType  with(nolock) on policyType.LeadPolicyTypeID = p.PolicyType
left join Carrier car  with(nolock) on car.carrierID = p.CarrierID
left join Claim c  with(nolock) on c.policyID = p.ID and c.IsActive = 1
left join ProducerMaster producer with(nolock)  on producer.ProducerId = l.PrimaryProducerId
left join dbo.UmpireMaster umpire with(nolock)  on umpire.UmpireId = l.UmpireId
left join AppraiserMaster appraiser  with(nolock) on appraiser.AppraiserId = l.AppraiserId
left join dbo.ContractorMaster contractor with(nolock)  on contractor.ContractorId = l.ContractorId
left join LeadSourceMaster ls  with(nolock) on ls.LeadSourceID = l.LeadSource
left join AdjusterMaster adj  with(nolock) on  adj.AdjusterID = c.AdjusterId
left join StatusMaster statusMaster  with(nolock) on statusMaster.StatusID = c.StatusID
left join SubStatusMaster substatusMaster  with(nolock) on substatusMaster.SubStatusID = c.SubStatusID
--where  and c.IsActive = 1

GO




-- 2014-05-08
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_AdjusterPayoutSubReport]
AS
SELECT     c.AdjusterClaimNumber, c.ClaimID, c.PolicyID, am.AdjusterName, ce.ExpenseAmount, cs.ServiceDate, cs.ServiceDescription, c.StatusID, l.ClientID
FROM         dbo.Claim AS c WITH (NOLOCK) INNER JOIN
                      dbo.ClaimExpense AS ce WITH (NOLOCK) ON c.ClaimID = ce.ClaimID INNER JOIN
                      dbo.ClaimService AS cs WITH (NOLOCK) ON c.ClaimID = cs.ClaimID INNER JOIN
                      dbo.AdjusterMaster AS am WITH (NOLOCK) ON ce.AdjusterID = am.AdjusterId AND cs.AdjusterID = am.AdjusterId INNER JOIN
                      dbo.StatusMaster AS sm WITH (NOLOCK) ON c.StatusID = sm.StatusId INNER JOIN
                      dbo.LeadPolicy AS lp WITH (NOLOCK) ON c.PolicyID = lp.Id INNER JOIN
                      dbo.Leads AS l WITH (NOLOCK) ON lp.LeadId = l.LeadId INNER JOIN
                      dbo.Client AS cl WITH (NOLOCK) ON l.ClientID = cl.ClientId
WHERE     (c.IsInvoiced = 1)
GROUP BY c.AdjusterClaimNumber, c.ClaimID, c.PolicyID, am.AdjusterName, ce.ExpenseAmount, cs.ServiceDate, cs.ServiceDescription, c.StatusID, l.ClientID
GO
SET ANSI_NULLS ON
GO

--2014-05-06
alter table dbo.Claim add LastProgressChanged datetime


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[Claim_ProgressStatusID_Updated]
ON [dbo].[Claim]
AFTER UPDATE /* Fire this trigger when a row is UPDATEd */
AS BEGIN
	DECLARE @New_ProgressStatusID INT
	DECLARE @ClaimID INT
	
	DECLARE @Old_ProgressStatusID INT
	
	-- new progress status
	SELECT	@New_ProgressStatusID = ProgressStatusID,
			@ClaimID = ClaimID
	FROM INSERTED
  
	-- old progress status
	SELECT	@Old_ProgressStatusID = ProgressStatusID,
			@ClaimID = ClaimID
	FROM DELETED
	
  UPDATE dbo.Claim
		SET LastProgressChanged = GETDATE()
  WHERE ClaimID = @ClaimID AND @New_ProgressStatusID <> @Old_ProgressStatusID
END

insert into dbo.StatusMaster(StatusName,[status])values('Reviewed',1)
insert into dbo.StatusMaster(StatusName,[status])values('Contacted',1)
insert into dbo.StatusMaster(StatusName,[status])values('Inspected',1)
insert into dbo.StatusMaster(StatusName,[status])values('Not sold',1)
insert into dbo.StatusMaster(StatusName,[status])values('QA rejected',1)
insert into dbo.StatusMaster(StatusName,[status])values('QA approved',1)
insert into dbo.StatusMaster(StatusName,[status])values('Client approved',1)
insert into dbo.StatusMaster(StatusName,[status])values('Client rejected',1)
insert into dbo.StatusMaster(StatusName,[status])values('Job started',1)
insert into dbo.StatusMaster(StatusName,[status])values('Job completed',1)
insert into dbo.StatusMaster(StatusName,[status])values('File closed',1)
insert into dbo.StatusMaster(StatusName,[status])values('File reopened',1)
insert into dbo.StatusMaster(StatusName,[status])values('Attention Needed',1)
insert into dbo.StatusMaster(StatusName,[status])values('Rejected” (sender use only)',1)

--2014-05-05
alter table dbo.ClaimDocument add DocumentCategoryID int

ALTER TABLE [dbo].[ClaimDocument]  WITH CHECK ADD  CONSTRAINT [FK_ClaimDocument_DocumentCategory] FOREIGN KEY([DocumentCategoryID])
REFERENCES [dbo].[DocumentCategory] ([DocumentCategoryID])

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DocumentCategory](
	[DocumentCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_DocumentCategory] PRIMARY KEY CLUSTERED 
(
	[DocumetCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


insert into dbo.DocumentCategory(CategoryName,IsActive) values('Appraisals', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Assignment', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Correspondence', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Expense Receipts', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Medical', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Other', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Photos', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Report', 1)
insert into dbo.DocumentCategory(CategoryName,IsActive) values('Service Bills', 1)

--2014-05-04

ALTER TABLE [dbo].[RuleException]  WITH CHECK ADD  CONSTRAINT [FK_RuleException_RuleObject] FOREIGN KEY([ObjectTypeID])
REFERENCES [dbo].[RuleObject] ([ObjectTypeID])


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RuleObject](
	[ObjectTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectName] [varchar](50) NULL,
 CONSTRAINT [PK_RuleObject] PRIMARY KEY CLUSTERED 
(
	[ObjectTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--2014-05-02
alter table AdjusterMaster add HourlyRate numeric(8,2)
alter table AdjusterMaster add CommissionRate numeric(8,2)
alter table AdjusterMaster add XactNetAddress varchar(50)
alter table AdjusterMaster Add SupervisorID int

-- 2014-04-30
alter table dbo.InvoiceDetail add ExpenseTypeID int
alter table ProgressStatus add SortOrder int

insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Adjuster Claim Review','Flag and Review "x" Invoices for training and review for "x" Adjuster',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Claim Assingment Review','Flag and Review all claims assigned without a status change within "x"  hours/days.',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('High Claim Amount Review','Flag and Review all claims equal to or greater than Net/Gross Payable of _______ for (All Adjusters OR a Specific Adjuster).',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Carrier Claim Review','Flag and Review all claims/invoices for review for ____ Carrier.',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Claim Expense Review','Flag and Review all invoices/claims assigned where Total Expenses is equal to or greater than ____% of the Total Services on the claim/invoice. ',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('CAT/Flat Rate Claim Borderline Review','Flag and Review all CAT/Flat Rate Invoice Type claims/invoices IF Net/Gross Claim Amount is >, =, < _____% of the Borderline Range.',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Approval Master Control - Review All Claims/Invoices by Exception','Review ALL claims/invoices for ____ days in the Claim/Invoice Approval Queue.',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Flag Specific Expense Type per Carrier','Flag and Review all claims/invoices IF ______________ Expense is on the claim/file for ________ CARRIER.',1)
insert into dbo.[Rule](ruleName,RuleDescription,isactive) values('Event Name Automated Program Name Billing Selection','Flag and Review all claims/invoices IF ______________ Expense is on the claim/file for ________ CARRIER.',1)


select * 
from claim c 
inner join leadpolicy p on p.id = c.policyid
inner join leads l on l.leadid = p.leadid
where clientid=7 and progressStatusID = 1
order by claimid

update claim set progressStatusID=3 where claimid between 301 and 800
update claim set progressStatusID=4 where claimid between 801 and 850
update claim set progressStatusID=5 where claimid between 851and 900
update claim set progressStatusID=6 where claimid between 901 and 950
update claim set progressStatusID=7 where claimid between 951 and 1000
update claim set progressStatusID=8 where claimid between 1001 and 1020
update claim set progressStatusID=9 where claimid between 1021 and 1030
update claim set progressStatusID=10 where claimid between 1031 and 1090
update claim set progressStatusID=11 where claimid between 1091 and 1190
update claim set progressStatusID=11 where claimid between 1191 and 1390
update claim set progressStatusID=12 where claimid between 1391 and 1400
update claim set progressStatusID=13 where claimid between 1401 and 1500
update claim set progressStatusID=14 where claimid between 1500 and 1510
--2014-04-26
alter table Claim add ProgressStatusID int

insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Claim Assigned, Not Accepted Yet',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Claim in Process, Accepted',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Contacted Insured',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Site Inspection Scheduled',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Site Inspection Completed',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('First Report Completed',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Interim Report Completed',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Final Report Completed',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Ready for Invoice',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Claim/Invoice Approved/Sent',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Claim/Invoice Rejected/Redo',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Invoice Partial Payment',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Invoice Paid in Full/Closed',1)
insert into dbo.ProgressStatus(ProgressDescription, isActive) values('Claim/Invoice Reopened',1)


--2014-04-23
alter table dbo.ClaimExpense add ExpenseQty numeric(10,2)
alter table dbo.ExpenseType add ExpenseRate numeric(10,2)

-- 2014-04-22
alter table dbo.CarrierInvoiceProfileFeeItemized add LogicalOperator int
alter table dbo.CarrierInvoiceProfileFeeItemized add LogicalOperatorOperand numeric(10,2)
alter table dbo.CarrierInvoiceProfile add FirmDiscountPercentage numeric(10,2)

alter table Claim add CarrierInvoiceProfileID int

insert into [StatusMaster](StatusName,[status]) values('Ready for Interim Invoice',1)
insert into [StatusMaster](StatusName,[status]) values('Invoiced',1)
  
  insert into dbo.Action(actionname,IsActive) values('Generate Invoice', 1)
alter table [CarrierInvoiceProfileFeeItemized] add [ServiceTypeID] [int]
alter table [CarrierInvoiceProfileFeeItemized] add [ExpenseTypeID] [int]


ALTER TABLE [dbo].[CarrierInvoiceProfileFeeItemized]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfileFeeItemized_InvoiceServiceType] FOREIGN KEY([ServiceTypeID])
REFERENCES [dbo].[InvoiceServiceType] ([ServiceTypeID])

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeItemized]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfileFeeItemized_ExpenseType] FOREIGN KEY([ExpenseTypeID])
REFERENCES [dbo].[ExpenseType] ([ExpenseTypeID])


ALTER TABLE [dbo].[CarrierInvoiceProfile]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfile_CarrierInvoiceType] FOREIGN KEY([InvoiceType])
REFERENCES [dbo].[CarrierInvoiceType] ([InvoiceTypeID])

-- 0214-04-18
alter table Client add InvoicePaymentTerms int
alter table Invoice add IsApprove bit

alter view vw_InvoiceLedger
as
select	l.ClientID, 
		inv.*,
		car.CarrierID,
		p.PolicyNumber
from dbo.Invoice inv
inner join	dbo.Claim c on c.ClaimID = inv.ClaimID
inner join	dbo.LeadPolicy p on p.ID = c.policyID
left join	dbo.Carrier car on car.CarrierID = p.CarrierID
inner join	dbo.Leads l on l.LeadID = p.LeadID
where (isnull(inv.isvoid,0) = 0)
	and (isnull(inv.IsApprove,0) = 0)


alter table leadpolicy add InitialCoverageDate datetime
alter table leadpolicy add AgentID int

alter table dbo.Mortgagee add ContactName varchar(50)
alter table dbo.Mortgagee add PrimaryContact varchar(50)
alter table dbo.Mortgagee add Email varchar(100)
alter table dbo.Mortgagee add CountryID int

alter table dbo.PolicyLienholder add LoanNumber varchar(50)

alter table dbo.Contact add AgentCode varchar(50)
alter table dbo.Contact add AgenctSubcode varchar(50)
alter table dbo.Contact add AgentCustomerID varchar(50)


SET IDENTITY_INSERT LeadContactType ON

INSERT LeadContactType(ID, [Description])
VALUES (57, 'Agent')

SET IDENTITY_INSERT LeadContactType OFF

/****** Object:  Table [dbo].[PolicyNote]    Script Date: 04/14/2014 17:09:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PolicyNote](
	[PolicyNoteID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyID] [int] NOT NULL,
	[Notes] [varchar](max) NULL,
	[NoteDate] [datetime] NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_PolicyNote] PRIMARY KEY CLUSTERED 
(
	[PolicyNoteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PolicyNote]  WITH CHECK ADD  CONSTRAINT [FK_PolicyNote_LeadPolicy] FOREIGN KEY([PolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])
GO

ALTER TABLE [dbo].[PolicyNote] CHECK CONSTRAINT [FK_PolicyNote_LeadPolicy]
GO

ALTER TABLE [dbo].[PolicyNote]  WITH CHECK ADD  CONSTRAINT [FK_PolicyNote_SecUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[SecUser] ([UserId])
GO

ALTER TABLE [dbo].[PolicyNote] CHECK CONSTRAINT [FK_PolicyNote_SecUser]
GO
alter table leadpolicy add InitialCoverageDate datetime

alter table dbo.Mortgagee add ContactName varchar(50)
alter table dbo.Mortgagee add PrimaryContact varchar(50)
alter table dbo.Mortgagee add Email varchar(100)
alter table dbo.Mortgagee add CountryID int

alter table dbo.PolicyLienholder add LoanNumber varchar(50)

alter table task add RecurringID int

/****** Object:  Table [dbo].[Recurrence]    Script Date: 04/11/2014 17:02:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Recurrence](
	[RecurringID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[EventID] [int] NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [date] NULL,
	[RepeatFrequencyID] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsRepeatDailyEveryDay] [bit] NULL,
	[IsRepeatDailyForEveryNDays] [bit] NULL,
	[RepeatDailyForEveryNDays] [int] NULL,
	[RepeatWeeklyEveryNWeeks] [int] NULL,
	[IsRepeatWeeklyEveryNWeeksMon] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksTue] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksWed] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksThur] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksFri] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksSat] [bit] NULL,
	[IsRepeatWeeklyEveryNWeeksSun] [bit] NULL,
	[IsRepeatMonthlyOnDay] [bit] NULL,
	[RepeatMonthlyOnDay] [int] NULL,
	[RepeatMonthlyOnDayEvery] [int] NULL,
	[IsRepeatMonthlyOn] [bit] NULL,
	[RepeatMonthlyOn] [int] NULL,
	[RepeatMonthlyOnWeekDay] [int] NULL,
	[RepeatMonthlyOnEvery] [int] NULL,
	[IsRepeatYearlyOnEvery] [bit] NULL,
	[RepeatYearlyMonth] [int] NULL,
	[RepeatYearlyMonthDay] [int] NULL,
	[IsRepeatYearlyOn] [bit] NULL,
	[RepeatYearlyOn] [int] NULL,
	[RepeatYearlyOnWeekDay] [int] NULL,
	[RepeatYearlyOnMonth] [int] NULL,
 CONSTRAINT [PK_Recurring] PRIMARY KEY CLUSTERED 
(
	[RecurringID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Recurrence]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO

ALTER TABLE [dbo].[Recurrence] CHECK CONSTRAINT [FK_Recurring_Task]
GO



/****** Object:  View [dbo].[vw_Reminder]    Script Date: 04/15/2014 07:28:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER view [dbo].[vw_Reminder]
as
	select 
		r.ReminderID,
		DueIn = datediff(minute, getdate(), r.ReminderDate),
		t.id, t.creator_id, t.[text], t.details, t.[start_date], t.end_date, t.status_id, t.owner_id,
		t.lead_id, t.master_status_id, t.lead_policy_id, t.policy_type, t.PriorityID, t.isAllDay,
		t.ReminderInterval, t.IsReminder,
		u.Email
	from Reminder r
	left join Task t on t.ID = r.TaskID
	left join SecUser u on u.UserId = t.owner_id
	
	where 	
		(datediff(minute, getdate(), r.ReminderDate) <= 0)				
		and (IsActive = 1)
	-- as of 4/10/2014
	--select 
	--	DueIn = datediff(minute, getdate(), [start_date]),
	--	t.id, t.creator_id, t.[text], t.details, t.[start_date], t.end_date, t.status_id, t.owner_id,
	--	t.lead_id, t.master_status_id, t.lead_policy_id, t.policy_type, t.PriorityID, t.isAllDay,
	--	t.ReminderInterval, t.IsReminder,
	--	u.Email
	--from Task t
	--left join SecUser u on u.UserId = t.owner_id
	
	--where 
	
	--	(datediff(minute, getdate(), [start_date]) <= isnull(t.ReminderInterval,0))				
	--	and (IsReminder = 1)
	--	and (status_id = 1)	-- active/pending



GO



alter table Task add TaskType int
alter table Task add ContactID int

/****** Object:  Table [dbo].[Action]    Script Date: 04/07/2014 22:53:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Action](
	[ActionID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [varchar](100) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RoleAction]    Script Date: 04/07/2014 22:52:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoleAction](
	[RoleActionID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[ActionID] [int] NOT NULL,
 CONSTRAINT [PK_RoleAction] PRIMARY KEY CLUSTERED 
(
	[RoleActionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Script for SelectTopNRows command from SSMS  ******/
CREATE view [dbo].[vw_Task]
as
SELECT t.[id]				as TaskID
	  ,[status_id]			as TaskStatusID
      ,ts.[title]			as TaskStatusName
      ,[creator_id]			as ClientID		
      ,[text]				as [Subject]
      ,[details]			as Detail
      ,[start_date]			as DateDue
      ,[end_date]
      
      ,[owner_id]			as UserID
      ,usr.UserName
      ,[lead_id]			as LeadID
      ,l.[InsuredName]
      ,[master_status_id]	as MasterStatusID
      ,[lead_policy_id]		as PolicyID
      ,[policy_type]		as PolicyTypeID
      ,t.[PriorityID]
      ,tp.PriorityName
      ,[isAllDay]
      ,[ReminderInterval]
      ,[IsReminder]
      ,[Location]
      ,t.[CarrierID]
      ,isnull(t.TaskType,1) as Activity
      ,t.details			as Description
  FROM [ClaimRuler].[dbo].[Task]  t with(nolock)
  left join dbo.TaskPriority tp with(nolock) on tp.PriorityID = t.PriorityID
  left join dbo.TaskStatus ts with(nolock) on ts.id = t.[status_id]
  left join Leads l with(nolock) on l.LeadId = t.lead_id
  left join SecUser usr with(nolock) on usr.UserId = t.owner_id

/****** Object:  Table [dbo].[Invitee]    Script Date: 04/05/2014 19:11:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Invitee](
	[InviteeID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[UserID] [int] NULL,
	[ContactID] [int] NULL,
	[LeadID] [int] NULL,
 CONSTRAINT [PK_Invitee] PRIMARY KEY CLUSTERED 
(
	[InviteeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Invitee]  WITH CHECK ADD  CONSTRAINT [FK_Invitee_Contact] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contact] ([ContactID])
GO

ALTER TABLE [dbo].[Invitee] CHECK CONSTRAINT [FK_Invitee_Contact]
GO

ALTER TABLE [dbo].[Invitee]  WITH CHECK ADD  CONSTRAINT [FK_Invitee_Leads] FOREIGN KEY([LeadID])
REFERENCES [dbo].[Leads] ([LeadId])
GO

ALTER TABLE [dbo].[Invitee] CHECK CONSTRAINT [FK_Invitee_Leads]
GO

ALTER TABLE [dbo].[Invitee]  WITH CHECK ADD  CONSTRAINT [FK_Invitee_SecUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[SecUser] ([UserId])
GO

ALTER TABLE [dbo].[Invitee] CHECK CONSTRAINT [FK_Invitee_SecUser]
GO

ALTER TABLE [dbo].[Invitee]  WITH CHECK ADD  CONSTRAINT [FK_Invitee_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO

ALTER TABLE [dbo].[Invitee] CHECK CONSTRAINT [FK_Invitee_Task]
GO



/****** Object:  Table [dbo].[Reminder]    Script Date: 04/03/2014 17:15:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Reminder](
	[ReminderID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[EventID] [int] NULL,
	[ReminderDate] [datetime] NULL,
	[RepeatFrequencyID] [int] NULL,
	[AlertTypeID] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[ReminderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Reminder]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO

ALTER TABLE [dbo].[Reminder] CHECK CONSTRAINT [FK_Reminder_Task]
GO



/****** Object:  Table [dbo].[Recurrence]    Script Date: 04/10/2014 14:15:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Recurrence](
	[RecurringID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[EventID] [int] NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [date] NULL,
	[RepeatFrequencyID] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Recurring] PRIMARY KEY CLUSTERED 
(
	[RecurringID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Recurrence]  WITH CHECK ADD  CONSTRAINT [FK_Recurring_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO

ALTER TABLE [dbo].[Recurrence] CHECK CONSTRAINT [FK_Recurring_Task]
GO



-- priority
insert into dbo.TaskPriority(PriorityName,IsActive) values('High', 1)
insert into dbo.TaskPriority(PriorityName,IsActive) values('Highest', 1)
insert into dbo.TaskPriority(PriorityName,IsActive) values('Low', 1)
insert into dbo.TaskPriority(PriorityName,IsActive) values('Lowwest', 1)
insert into dbo.TaskPriority(PriorityName,IsActive) values('Normal', 1)


alter table claim add OutsideAdjusterID int
alter table claim add ContentAdjusterID int
alter table claim add ExaminerID int
alter table claim add CompanyBuilderID int
alter table claim add CompanyInventoryID int
alter table claim add OurBuilderID int
alter table claim add InventoryCompanyID int

/****** Object:  View [dbo].[vw_Contact]    Script Date: 03/31/2014 07:54:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER View [dbo].[vw_Contact]
as
select co.ContactID, co.ClientId, co.FirstName, co.LastName, co.CompanyName, co.Email, lct.Description as ContactType, co.ContactName
from Contact co
left join LeadContactType lct on lct.ID = co.CategoryID
--where (co.Email is not null) and (rtrim(ltrim(co.Email)) <> '') and (co.ClientId is not null)
where (co.ClientId is not null)


/****** Object:  View [dbo].[vw_FormFields]    Script Date: 03/31/2014 07:54:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE view [dbo].[vw_FormFields]
as
SELECT f.FormID, f.FormName, ddf.FieldID, ddf.FieldPrompt, TemplateID = isnull(t.TemplateID,0), t.ClientID, IsSelected =isnull(t.IsSelected,1)
FROM dbo.DataFormField ddf
LEFT JOIN dbo.DataForm f ON f.FormID = ddf.FormID
LEFT JOIN dbo.DataFormFieldTemplate t on t.FieldID = ddf.FieldID and t.FormID = ddf.FormID



/****** Object:  Table [dbo].[DataEntryScreen]    Script Date: 03/26/2014 17:26:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DataEntryScreen](
	[ScreenID] [int] IDENTITY(1,1) NOT NULL,
	[ScreenName] [varchar](100) NULL,
 CONSTRAINT [PK_DataEntryScreen] PRIMARY KEY CLUSTERED 
(
	[ScreenID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



/****** Object:  Table [dbo].[DataEntryScreenFields]    Script Date: 03/26/2014 17:27:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DataEntryScreenFields](
	[FieldID] [int] IDENTITY(1,1) NOT NULL,
	[ScreenID] [int] NOT NULL,
	[FieldPrompt] [varchar](100) NULL,
	[FieldNameID] [nchar](10) NULL,
 CONSTRAINT [PK_DataEntryScreenFields] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[DataEntryScreenFields]  WITH CHECK ADD  CONSTRAINT [FK_DataEntryScreenFields_DataEntryScreen] FOREIGN KEY([ScreenID])
REFERENCES [dbo].[DataEntryScreen] ([ScreenID])
GO

ALTER TABLE [dbo].[DataEntryScreenFields] CHECK CONSTRAINT [FK_DataEntryScreenFields_DataEntryScreen]
GO




/****** Object:  View [dbo].[vw_Task]    Script Date: 03/24/2014 08:10:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Script for SelectTopNRows command from SSMS  ******/
CREATE view [dbo].[vw_Task]
as
SELECT t.[id]				as TaskID
	  ,[status_id]			as TaskStatusID
      ,ts.[title]			as TaskStatusName
      ,[creator_id]			as ClientID		
      ,[text]				as [Subject]
      ,[details]			as Detail
      ,[start_date]			as DateDue
      ,[end_date]
      
      ,[owner_id]			as UserID
      ,usr.UserName
      ,[lead_id]			as LeadID
      ,l.[InsuredName]
      ,[master_status_id]	as MasterStatusID
      ,[lead_policy_id]		as PolicyID
      ,[policy_type]		as PolicyTypeID
      ,t.[PriorityID]
      ,tp.PriorityName
      ,[isAllDay]
      ,[ReminderInterval]
      ,[IsReminder]
      ,[Location]
      ,t.[CarrierID]
  FROM [ClaimRuler].[dbo].[Task]  t with(nolock)
  left join dbo.TaskPriority tp with(nolock) on tp.PriorityID = t.PriorityID
  left join dbo.TaskStatus ts with(nolock) on ts.id = t.[status_id]
  left join Leads l with(nolock) on l.LeadId = t.lead_id
  left join SecUser usr with(nolock) on usr.UserId = t.owner_id


select	l.InsuredName, l.ClaimantFirstName, l.ClaimantLastNAme, l.BusinessName, l.MailingAddress, l.[Status],
		l.CityName,l.StateName,l.Zip,l.LossAddress,l.LossAddress2,
		c.LossDate, c.AdjusterClaimNumber,
		p.PolicyNumber
from Leads L
left join LeadPolicy p on l.LeadID = p.LeadId
left join Claim c on c.policyID = p.ID


alter table Client add ClientTypeID int

alter table InvoiceServiceType add MinimumFee decimal(18,2)
alter table InvoiceServiceType add EarningCodeID int
alter table InvoiceServiceType add IsTaxable bit
alter table InvoiceServiceType add DefaultQty decimal(18,2)

ALTER TABLE [dbo].[InvoiceServiceType]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceServiceType_EarningCode] FOREIGN KEY([EarningCodeID])
REFERENCES [dbo].[EarningCode] ([EarningCodeID])



/****** Object:  Table [dbo].[EarningCode]    Script Date: 03/14/2014 15:28:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EarningCode](
	[EarningCodeID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[Code] [varchar](50) NULL,
	[CodeDescription] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_EarningCode] PRIMARY KEY CLUSTERED 
(
	[EarningCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ClaimService]    Script Date: 03/14/2014 08:45:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClaimService](
	[ClaimServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimID] [int] NULL,
	[ServiceTypeID] [int] NOT NULL,
	[ServiceCodeID] [int] NOT NULL,
	[AdjusterID] [int] NULL,
	[ServiceDate] [datetime] NULL,
	[ServiceDescription] [varchar](500) NULL,
	[ServiceAmount] [decimal](18, 2) NULL,
	[ServiceQty] [decimal](18, 2) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_ClaimService] PRIMARY KEY CLUSTERED 
(
	[ClaimServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExpenseType]    Script Date: 03/13/2014 08:37:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExpenseType](
	[ExpenseTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseName] [varchar](100) NOT NULL,
	[ExpenseDescription] [varchar](100) NULL,
	[ClientID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ExpenseType] PRIMARY KEY CLUSTERED 
(
	[ExpenseTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[ClaimExpense]    Script Date: 03/13/2014 23:13:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClaimExpense](
	[ClaimExpenseID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimID] [int] NOT NULL,
	[ExpenseTypeID] [int] NOT NULL,
	[ExpenseDate] [datetime] NULL,
	[ExpenseDescription] [varchar](500) NOT NULL,
	[ExpenseAmount] [decimal](18, 2) NOT NULL,
	[IsReimbursable] [bit] NOT NULL,
	[UserID] [int] NULL,
	[AdjusterID] [int] NULL,
 CONSTRAINT [PK_ClaimExpense] PRIMARY KEY CLUSTERED 
(
	[ClaimExpenseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ClaimExpense]  WITH CHECK ADD  CONSTRAINT [FK_ClaimExpense_AdjusterMaster] FOREIGN KEY([AdjusterID])
REFERENCES [dbo].[AdjusterMaster] ([AdjusterId])
GO

ALTER TABLE [dbo].[ClaimExpense] CHECK CONSTRAINT [FK_ClaimExpense_AdjusterMaster]
GO

ALTER TABLE [dbo].[ClaimExpense]  WITH CHECK ADD  CONSTRAINT [FK_ClaimExpense_ClaimExpense] FOREIGN KEY([ExpenseTypeID])
REFERENCES [dbo].[ExpenseType] ([ExpenseTypeID])
GO

ALTER TABLE [dbo].[ClaimExpense] CHECK CONSTRAINT [FK_ClaimExpense_ClaimExpense]
GO





alter table contact add UserID int

ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_SecUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[SecUser] ([UserId])


alter table claim drop column CycleTime 
alter table claim drop column ReopenCycleTime
 
alter table claim add CycleTime decimal(10,2)
alter table claim add ReopenCycleTime decimal(10,2)
alter table Leads add InsuredName varchar(100)
alter table Leads add MailingAddress2 varchar(50)
alter table Leads add MailingCounty varchar(50)
alter table Leads add LossCounty varchar(50)

alter table Claim add DateEstimateUploaded datetime

alter table Contact add ContactName varchar(100)
alter table Contact add IsActive bit
update Contact set isActive = 1


/****** Object:  Table [dbo].[ImportField]    Script Date: 03/08/2014 10:38:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ImportField](
	[FieldID] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ImportFields] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



alter table dbo.SecModule add IsSystem bit
update SecModule set issystem = 0 

alter table ClaimImage add ImageDate datetime
alter table AdjusterMaster add isNotifyUserUploadDocument bit

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Alter view [dbo].[vw_CommentLog]
as
select lc.LeadID,lc.PolicyType,lc.UserID,lusr.UserName,lc.InsertDate,lc.CommentText,ClaimID=0
	from leadcomment lc
	left join SecUser lusr on lusr.UserID = lc.UserId
	where lc.[status] = 1
union all
select p.LeadID,p.PolicyType,cc.UserID,usr.UserName,cc.CommentDate,cc.CommentText,cc.ClaimID
	from claimcomment cc
	inner join claim c on c.claimid = cc.claimid
	inner join LeadPolicy p on c.policyid = p.id
	left join SecUser usr on usr.UserID = cc.UserId
	where cc.isActive = 1




 alter table claim add LastStatusUpdate datetime

create function f_getDamageID(@policyID int)
returns varchar(50)
as
begin
	declare @DamageIDs varchar(50)
	select @DamageIDs = COALESCE(@DamageIDs + ',', '') + 
						CAST(TypeOfDamageId AS varchar(5)) from dbo.LeadPolicyDamageType lpdt where lpdt.PolicyID = @policyID

	return @DamageIDs						
end

/****** Object:  View [dbo].[vw_Adjuster]    Script Date: 02/25/2014 11:51:37 ******/
ALTER View [dbo].[vw_Adjuster]
as
select ClientId=0,AdjusterId=0,AdjusterName='--- Select ---' 
union all
select ClientId, AdjusterId, 
	AdjusterName =	case
						when (FirstName is null ) and (LastName is null) then AdjusterName
						else  isnull(FirstName,'') + ' ' + isnull(MiddleInitial,'') + ' ' + isnull(LastName,'')
					end
from AdjusterMaster
where ([Status]=1) 





select m.AdjusterID, AdjusterName = m.FirstName + ' ' + m.LastName,
NumberClaimsAssigned = (
	select count(*) from Claim c
	where c.AdjuterID = m.AdjusterId
	--group by c.AdjuterID
)
from AdjusterMaster m
where m.[status] = 1



alter table Leads alter column PhoneNumber varchar(35)
alter table Leads alter column  SecondaryPhone varchar(35)
alter table Leads alter column  OwnerPhone varchar(35)

/****** Object:  Table [dbo].[PolicyLienholder]    Script Date: 02/25/2014 07:57:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PolicyLienholder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyID] [int] NOT NULL,
	[MortgageeID] [int] NOT NULL,
 CONSTRAINT [PK_PolicyLienholder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PolicyLienholder]  WITH CHECK ADD  CONSTRAINT [FK_PolicyLienholder_LeadPolicy] FOREIGN KEY([PolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])
GO

ALTER TABLE [dbo].[PolicyLienholder] CHECK CONSTRAINT [FK_PolicyLienholder_LeadPolicy]
GO

ALTER TABLE [dbo].[PolicyLienholder]  WITH CHECK ADD  CONSTRAINT [FK_PolicyLienholder_Mortgagee] FOREIGN KEY([MortgageeID])
REFERENCES [dbo].[Mortgagee] ([MortgageeID])
GO

ALTER TABLE [dbo].[PolicyLienholder] CHECK CONSTRAINT [FK_PolicyLienholder_Mortgagee]
GO



/****** Object:  View [dbo].[vw_Contact]    Script Date: 02/23/2014 21:25:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER View [dbo].[vw_Contact]
as
select co.ContactID, co.ClientId, co.FirstName, co.LastName, co.CompanyName, co.Email, lct.Description as ContactType
from Contact co
left join LeadContactType lct on lct.ID = co.CategoryID
where (co.Email is not null) and (rtrim(ltrim(co.Email)) <> '') and (co.ClientId is not null)

GO




ALTER View [dbo].[vw_ClaimLimit]
as
select ClaimLimitID = isnull(cl.ClaimLimitID,0), cl.ClaimID, 
	l.*,
	pl.LimitAmount, pl.LimitDeductible, pl.CATDeductible, pl.SettlementType, pl.ConInsuranceLimit,
	cl.LossAmountACV, cl.LossAmountRCV, cl.Depreciation, cl.OverageAmount
from dbo.Limit l
inner join dbo.PolicyLimit pl on l.LimitID = pl.LimitID
inner join claim c on c.policyID = pl.policyID
inner join dbo.ClaimLimit cl on cl.LimitID = l.LimitID and cl.claimid = c.claimid

/* original
select ClaimLimitID = isnull(cl.ClaimLimitID,0), cl.ClaimID, 
	l.*,
	pl.LimitAmount, pl.LimitDeductible, pl.CATDeductible, pl.SettlementType, pl.ConInsuranceLimit,
	cl.LossAmountACV, cl.LossAmountRCV, cl.Depreciation, cl.OverageAmount
from dbo.PolicyLimit pl
inner join dbo.Limit l on l.LimitID = pl.LimitID
left join dbo.ClaimLimit cl on cl.LimitID = l.LimitID 
*/
GO


/****** Object:  Table [dbo].[ClaimImage]    Script Date: 02/19/2014 23:25:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClaimImage](
	[ClaimImageID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimID] [int] NOT NULL,
	[ImageName] [varchar](100) NULL,
	[Location] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[UserID] [int] NULL,
	[IsPrint] [bit] NOT NULL,
 CONSTRAINT [PK_ClaimImage] PRIMARY KEY CLUSTERED 
(
	[ClaimImageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ClaimImage]  WITH CHECK ADD  CONSTRAINT [FK_ClaimImage_Claim] FOREIGN KEY([ClaimID])
REFERENCES [dbo].[Claim] ([ClaimID])
GO

ALTER TABLE [dbo].[ClaimImage] CHECK CONSTRAINT [FK_ClaimImage_Claim]
GO

ALTER TABLE [dbo].[ClaimImage]  WITH CHECK ADD  CONSTRAINT [FK_ClaimImage_SecUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[SecUser] ([UserId])
GO

ALTER TABLE [dbo].[ClaimImage] CHECK CONSTRAINT [FK_ClaimImage_SecUser]
GO



/****** Object:  Table [dbo].[AdjusterLicenseAppointmentType]    Script Date: 02/18/2014 18:59:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AdjusterLicenseAppointmentType](
	[LicenseAppointmentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[LicenseAppointmentType] [varchar](50) NOT NULL,
	[ClientID] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_AdjusterLicenseAppointmentType] PRIMARY KEY CLUSTERED 
(
	[LicenseAppointmentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


alter table dbo.AdjusterMaster add MiddleInitial varchar(1)
alter table dbo.AdjusterMaster add Suffix varchar(10)
alter table dbo.AdjusterServiceArea add LicenseType varchar(20)
alter table dbo.AdjusterServiceArea add AppointmentTypeID int

--Alter View vw_Adjuster
--as
select ClientId, AdjusterId, AdjusterName = FirstName + isnull(MiddleInitial,'') + ' ' + LastName
from AdjusterMaster
where firstname is not null and not lastname is null and [Status]=1

alter table dbo.TypeOfDamageMaster add IsHidden bit

alter table claim add InsurerClaimNumber varchar(50)

--alter View vw_ClaimLimit
--as
select cl.ClaimID, l.*,
	pl.LimitAmount, pl.LimitDeductible, pl.CATDeductible, pl.SettlementType, pl.ConInsuranceLimit,
	cl.LossAmountACV, cl.LossAmountRCV, cl.Depreciation, cl.OverageAmount
from dbo.PolicyLimit pl
inner join dbo.Limit l on l.LimitID = pl.LimitID
left join dbo.ClaimLimit cl on cl.LimitID = l.LimitID


/****** Object:  Table [dbo].[ClaimContact]    Script Date: 02/12/2014 15:15:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClaimContact](
	[ClaimContactID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimID] [int] NOT NULL,
	[ContactID] [int] NOT NULL,
 CONSTRAINT [PK_ClaimContact] PRIMARY KEY CLUSTERED 
(
	[ClaimContactID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClaimContact]  WITH CHECK ADD  CONSTRAINT [FK_ClaimContact_Contact] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contact] ([ContactID])
GO

ALTER TABLE [dbo].[ClaimContact] CHECK CONSTRAINT [FK_ClaimContact_Contact]
GO



alter table claim add CycleTime as datediff(d, DateClosed, DateAssigned) persisted
alter table claim add ReopenCycleTime as datediff(d, DateReopenCompleted, DateFirstReopened) persisted

alter table Claim add NetClaimPayable money
alter table Claim add DateCompleted datetime
alter table Claim add DateFirstClosed datetime
alter table Claim add IsInvoiceReady bit
alter table Claim add DamageType varchar(50)

alter table Claim add CasualtyFeeInvoiceDesignation int
alter table Claim add CasualtyGrossClaimPayable money


alter table Client add NextClaimNumber int

alter table Claim add NetClaimPayable money
alter table Claim add DateCompleted datetime
alter table Claim add DateFirstClosed datetime
alter table Claim add IsInvoiceReady bit

alter table LeadPolicy add EffectiveDate datetime
alter table LeadPolicy add ExpirationDate datetime
alter table Contact add IsPrimary bit
alter table Contact add Fax varchar(20)

alter table Task add CarrierID int

/****** Object:  Table [dbo].[ClaimLimit]    Script Date: 02/14/2014 07:39:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClaimLimit](
	[ClaimLimitID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimID] [int] NOT NULL,
	[LimitID] [int] NOT NULL,
	[LossAmountACV] [money] NULL,
	[LossAmountRCV] [money] NULL,
	[Depreciation] [money] NULL,
	[OverageAmount] [money] NULL,
 CONSTRAINT [PK_ClaimLimit] PRIMARY KEY CLUSTERED 
(
	[ClaimLimitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClaimLimit]  WITH CHECK ADD  CONSTRAINT [FK_ClaimLimit_Claim] FOREIGN KEY([ClaimID])
REFERENCES [dbo].[Claim] ([ClaimID])
GO

ALTER TABLE [dbo].[ClaimLimit] CHECK CONSTRAINT [FK_ClaimLimit_Claim]
GO

ALTER TABLE [dbo].[ClaimLimit]  WITH CHECK ADD  CONSTRAINT [FK_ClaimLimit_Limit] FOREIGN KEY([LimitID])
REFERENCES [dbo].[Limit] ([LimitID])
GO

ALTER TABLE [dbo].[ClaimLimit] CHECK CONSTRAINT [FK_ClaimLimit_Limit]
GO

/****** Object:  Table [dbo].[PolicyLimit]    Script Date: 02/14/2014 07:38:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PolicyLimit](
	[PolicyLimitID] [int] IDENTITY(1,1) NOT NULL,
	[PolicyID] [int] NOT NULL,
	[LimitID] [int] NOT NULL,
	[LimitAmount] [money] NULL,
	[LimitDeductible] [money] NULL,
	[CATDeductible] [varchar](20) NULL,
	[SettlementType] [varchar](20) NULL,
	[ConInsuranceLimit] [money] NULL,
 CONSTRAINT [PK_LimitID] PRIMARY KEY CLUSTERED 
(
	[PolicyLimitID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PolicyLimit]  WITH CHECK ADD  CONSTRAINT [FK_PolicyLimit_Limit] FOREIGN KEY([LimitID])
REFERENCES [dbo].[Limit] ([LimitID])
GO

ALTER TABLE [dbo].[PolicyLimit] CHECK CONSTRAINT [FK_PolicyLimit_Limit]
GO

ALTER TABLE [dbo].[PolicyLimit]  WITH CHECK ADD  CONSTRAINT [FK_PolicyPropertyLimit_LeadPolicy] FOREIGN KEY([PolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])
GO

ALTER TABLE [dbo].[PolicyLimit] CHECK CONSTRAINT [FK_PolicyPropertyLimit_LeadPolicy]
GO






/****** Object:  Table [dbo].[CarrierDocument]    Script Date: 02/05/2014 21:42:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierDocument](
	[CarrierDocumentID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierID] [int] NOT NULL,
	[DocumentDate] [datetime] NOT NULL,
	[DocumentName] [varchar](100) NOT NULL,
	[DocumentDescription] [varchar](500) NOT NULL,
 CONSTRAINT [PK_CarrierDocument] PRIMARY KEY CLUSTERED 
(
	[CarrierDocumentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CarrierDocument]  WITH CHECK ADD  CONSTRAINT [FK_CarrierDocument_Carrier] FOREIGN KEY([CarrierID])
REFERENCES [dbo].[Carrier] ([CarrierID])
GO

ALTER TABLE [dbo].[CarrierDocument] CHECK CONSTRAINT [FK_CarrierDocument_Carrier]
GO



alter table LeadPolicy add PolicyLimit money




/****** Object:  Table [dbo].[CarrierComment]    Script Date: 02/04/2014 17:01:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierComment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[CarrierID] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
	[CommentText] [varchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_CarrierComment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CarrierComment]  WITH CHECK ADD  CONSTRAINT [FK_CarrierComment_Carrier] FOREIGN KEY([CarrierID])
REFERENCES [dbo].[Carrier] ([CarrierID])
GO

ALTER TABLE [dbo].[CarrierComment] CHECK CONSTRAINT [FK_CarrierComment_Carrier]
GO

ALTER TABLE [dbo].[CarrierComment]  WITH CHECK ADD  CONSTRAINT [FK_CarrierComment_SecUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[SecUser] ([UserId])
GO

ALTER TABLE [dbo].[CarrierComment] CHECK CONSTRAINT [FK_CarrierComment_SecUser]
GO

alter table dbo.LeadPolicyCoverage add LossUseLimit money
alter table dbo.LeadPolicyCoverage add LossUseACV money
alter table dbo.LeadPolicyCoverage add LossUseRCV money
alter table dbo.LeadPolicyCoverage add LossUseOverage money

alter table dbo.LeadPolicyCoverage add PersonalLiabilityLimit money
alter table dbo.LeadPolicyCoverage add PersonalLiabilityACV money
alter table dbo.LeadPolicyCoverage add PersonalLiabilityRCV money
alter table dbo.LeadPolicyCoverage add PersonalLiabilityOverage money

alter table dbo.LeadPolicyCoverage add MedicalPaymentLimit money
alter table dbo.LeadPolicyCoverage add MedicalPaymentACV money
alter table dbo.LeadPolicyCoverage add MedicalPaymentRCV money
alter table dbo.LeadPolicyCoverage add MedicalPaymentOverage money

ALTER TABLE [dbo].[LeadPolicy] ADD [DateReported] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateFirstClosed] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateFirstReOpened] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateAssigned] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateAcknowledged] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateFirstContactAttempt] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [SiteSurveyDateCompleted] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateSubmitted] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateInitialReserveChange] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateIndemnityPaymentRequested] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateIndemnityPaymentApproved] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateIndemnityPaymentIssued] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateCompleted] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateClosed] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [Depreciation] [money] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [Deductible] [money] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [PolicyFormType] [varchar](50) NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateContacted] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [DateOpen] [datetime] NULL

alter table [LeadInvoice] add [CarrierInvoiceProfileID] int

ALTER TABLE dbo.[LeadInvoice]
ADD CONSTRAINT [FK_LeadInvoice_CarrierInvoiceProfile]
FOREIGN KEY ([CarrierInvoiceProfileID])
REFERENCES dbo.[CarrierInvoiceProfile]([CarrierInvoiceProfileID]) 




ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_LeadContactType] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[LeadContactType] ([ID])
/****** Object:  Table [dbo].[SecRoleInvoiceApprovalPermission]    Script Date: 01/30/2014 22:37:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SecRoleInvoiceApprovalPermission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[AmountFrom] [money] NOT NULL,
	[AmountTo] [money] NOT NULL,
 CONSTRAINT [PK_SecRoleInvoiceApprovalPermission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


alter table dbo.CarrierInvoiceProfile add AccountingContact varchar(50)
alter table dbo.CarrierInvoiceProfile add AccountingContactEmail varchar(100)
alter table SecRole add ClientID int
insert into SecRole(RoleName, RoleDescription, status, isClient,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy) values('Manager','Manager',1,1,getdate(),1,getdate(),1 )
insert into SecRole(RoleName, RoleDescription, status, isClient,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy) values('Field Inspector','Field Inspector',1,1,getdate(),1,getdate(),1)
insert into SecRole(RoleName, RoleDescription, status, isClient,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy) values('File Review Adjuster','File Review Adjuster',1,1,getdate(),1,getdate(),1)
insert into SecRole(RoleName, RoleDescription, status, isClient,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy) values('CPA','CPA',1,1,getdate(),1,getdate(),1)


ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [FK_LeadInvoice_LeadPolicy] FOREIGN KEY([PolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])

ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [FK_LeadInvoice_Client] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientId])

ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [FK_LeadInvoice_CarrierInvoiceType] FOREIGN KEY([InvoiceTypeID])
REFERENCES [dbo].[CarrierInvoiceType] ([InvoiceTypeID])

ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [FK_LeadInvoice_Carrier] FOREIGN KEY([CarrierID])
REFERENCES [dbo].[Carrier] ([CarrierID])

ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [FK_LeadInvoice_AdjusterMaster] FOREIGN KEY([AdjusterID])
REFERENCES [dbo].[AdjusterMaster] ([AdjusterId])

alter table LeadPolicy add IsInvoiced bit

/****** Object:  Table [dbo].[CarrierInvoiceType]    Script Date: 01/27/2014 15:30:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierInvoiceType](
	[InvoiceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceType] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CarrierInvoiceType] PRIMARY KEY CLUSTERED 
(
	[InvoiceTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


alter table dbo.AdjusterMaster add ZipCodeID int
alter table dbo.SecModule add VersionNumber int

alter table Client add InvoiceSettingID int
alter table Client add InvoiceContingencyFee money


alter table LeadPolicy add ClaimDesignationID int
alter table LeadPolicy add NetClaimPayable money
alter table LeadPolicy Add GrossLossPayable money
alter table LeadPolicy add IsInvoiceReady bit

alter table dbo.CarrierInvoiceProfile add InvoiceType int

alter table LeadInvoice add CarrierID int
alter table LeadInvoice add InvoiceTypeID int






alter table dbo.SecModule add VersionNumber int

alter table dbo.LeadPolicyCoverage add [DwellingLimit] [money] NULL,
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

alter table AdjusterMaster add MaxClaimNumber int
alter table AdjusterMaster add PhotoFileName varchar(50)
/****** Object:  Table [dbo].[LeadPolicyCoverage]    Script Date: 01/20/2014 19:07:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadPolicyCoverage](
	[CoverageID] [int] IDENTITY(1,1) NOT NULL,
	[LeadPolicyID] [int] NOT NULL,
	[Description] [varchar](100) NULL,
	[Limit] [varchar](50) NULL,
	[Deductible] [money] NULL,
	[CoInsuranceForm] [varchar](50) NULL,
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
	[ContentsOverage] [money] NULL,
 CONSTRAINT [PK_LeadPolicyCoverage] PRIMARY KEY CLUSTERED 
(
	[CoverageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LeadPolicyCoverage]  WITH CHECK ADD  CONSTRAINT [FK_LeadPolicyCoverage_LeadPolicy] FOREIGN KEY([LeadPolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])
GO

ALTER TABLE [dbo].[LeadPolicyCoverage] CHECK CONSTRAINT [FK_LeadPolicyCoverage_LeadPolicy]
GO
/****** Object:  Table [dbo].[CarrierInvoiceProfileFeeItemized]    Script Date: 01/17/2014 15:59:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeItemized](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierInvoiceProfileID] [int] NOT NULL,
	[ItemDescription] [varchar](100) NOT NULL,
	[ItemRate] [money] NOT NULL,
	[ItemPercentage] [money] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CarrierInvoiceProfileFeeItemized] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeItemized]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfileFeeItemized_CarrierInvoiceProfile] FOREIGN KEY([CarrierInvoiceProfileID])
REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeItemized] CHECK CONSTRAINT [FK_CarrierInvoiceProfileFeeItemized_CarrierInvoiceProfile]
GO

/****** Object:  Table [dbo].[CarrierInvoiceProfileFeeProvision]    Script Date: 01/16/2014 16:13:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeProvision](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierInvoiceProfileID] [int] NOT NULL,
	[ProvisionText] [varchar](max) NOT NULL,
	[ProvisionAmount] [money] NOT NULL,
 CONSTRAINT [PK_CarrierInvoiceProfileFeeProvision] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeProvision]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfileFeeProvision_CarrierInvoiceProfile] FOREIGN KEY([CarrierInvoiceProfileID])
REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeProvision] CHECK CONSTRAINT [FK_CarrierInvoiceProfileFeeProvision_CarrierInvoiceProfile]
GO


/****** Object:  Table [dbo].[CarrierInvoiceProfileFeeSchedule]    Script Date: 01/16/2014 15:30:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CarrierInvoiceProfileFeeSchedule](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierInvoiceProfileID] [int] NOT NULL,
	[RangeAmountFrom] [money] NOT NULL,
	[RangeAmountTo] [money] NOT NULL,
	[FlatFee] [money] NOT NULL,
	[PercentFee] [money] NOT NULL,
	[MinimumFee] [money] NOT NULL,
 CONSTRAINT [PK_CarrierInvoiceProfileFeeSchedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeSchedule]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfileFeeSchedule_CarrierInvoiceProfile] FOREIGN KEY([CarrierInvoiceProfileID])
REFERENCES [dbo].[CarrierInvoiceProfile] ([CarrierInvoiceProfileID])
GO

ALTER TABLE [dbo].[CarrierInvoiceProfileFeeSchedule] CHECK CONSTRAINT [FK_CarrierInvoiceProfileFeeSchedule_CarrierInvoiceProfile]
GO

/****** Object:  Table [dbo].[InvoiceFeeProfile]    Script Date: 01/13/2014 22:38:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InvoiceFeeProfile](
	[InvoiceFeeProfileID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceFeeProfileName] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[ClientID] [int] NULL,
 CONSTRAINT [PK_InvoiceFeeProfile] PRIMARY KEY CLUSTERED 
(
	[InvoiceFeeProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


/****** Object:  Table [dbo].[CarrierInvoiceProfile]    Script Date: 01/13/2014 15:27:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CarrierInvoiceProfile](
	[CarrierInvoiceProfileID] [int] IDENTITY(1,1) NOT NULL,
	[CarrierID] [int] NOT NULL,
	[InvoiceProfileID] [int] NULL,
	[ProfileName] [varchar](100) NOT NULL,
	[EffiectiveDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[CoverageArea] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CarrierInvoiceProfile] PRIMARY KEY CLUSTERED 
(
	[CarrierInvoiceProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CarrierInvoiceProfile]  WITH CHECK ADD  CONSTRAINT [FK_CarrierInvoiceProfile_Carrier] FOREIGN KEY([CarrierID])
REFERENCES [dbo].[Carrier] ([CarrierID])
GO

ALTER TABLE [dbo].[CarrierInvoiceProfile] CHECK CONSTRAINT [FK_CarrierInvoiceProfile_Carrier]
GO




alter table CarrierLocation add StateId int
alter table CarrierLocation add CityId int
alter table CarrierLocation add ZipCodeId int

ALTER TABLE [dbo].[CarrierLocation]  WITH CHECK ADD  CONSTRAINT [FK_CarrierLocation_CityMaster] FOREIGN KEY([CityId])
REFERENCES [dbo].[CityMaster] ([CityId])

ALTER TABLE [dbo].[CarrierLocation]  WITH CHECK ADD  CONSTRAINT [FK_CarrierLocation_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([StateId])


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_Country
FOREIGN KEY (CountryID)
REFERENCES dbo.CountryMaster(CountryID) 

ALTER TABLE dbo.AdjusterMaster
ADD CONSTRAINT fk_AdjusterMaster_DeploymentState
FOREIGN KEY (DeploymentStateID)
REFERENCES dbo.StateMaster(StateID) 

alter table AdjusterMaster add DeploymentAddress varchar(100)
alter table AdjusterMaster add DeploymentStateID int
alter table AdjusterMaster add DeploymentCity varchar(50)
alter table AdjusterMaster add DeploymentZipCode varchar(10)



/****** Object:  Table [dbo].[SubLimitOfLiabilityMaster]    Script Date: 01/11/2014 08:36:00 ******/
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
ADD CONSTRAINT fk_LeadPolicy_Mortgagee
FOREIGN KEY (MortgageeID)
REFERENCES dbo.Mortgagee(MortgageeID)


ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_SubLimitOfLiabilityMaster
FOREIGN KEY (SublimitLiabilityID)
REFERENCES dbo.SubLimitOfLiabilityMaster(SublimitLiabilityID)




/****** Object:  Table [dbo].[SubLimitOfLiabilityMaster]    Script Date: 01/11/2014 08:36:00 ******/
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


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_stateMaster
FOREIGN KEY (stateID)
REFERENCES dbo.TypeOfDamageMaster(TypeOfDamageId)


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_CityMaster
FOREIGN KEY (cityID)
REFERENCES dbo.CityMaster(cityID)


ALTER TABLE dbo.Carrier
ADD CONSTRAINT fk_Carrier_ZipCodeMaster
FOREIGN KEY (ZipCodeID)
REFERENCES dbo.ZipCodeMaster(ZipCodeID)


alter table dbo.AdjusterServiceArea add LicenseNumber varchar(50)
alter table dbo.TypeOfDamageMaster add ClientId int




-- update city/zip for adjustermaster
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


alter table dbo.LeadInvoice add AdjusterID int
alter table dbo.AdjusterMaster add isW9 bit
alter table dbo.AdjusterMaster add GeographicalSeriveArea varchar(100)
alter table dbo.AdjusterMaster add FirstName varchar(50)
alter table dbo.AdjusterMaster add LastName varchar(50)
alter table dbo.AdjusterMaster add YearsExperiece int
alter table dbo.AdjusterMaster add Certifications varchar(100)

/****** Object:  Table [dbo].[LeadPolicyCoverage]    Script Date: 12/28/2013 10:50:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadPolicyCoverage](
	[CoverageID] [int] IDENTITY(1,1) NOT NULL,
	[LeadPolicyID] [int] NOT NULL,
	[Description] [varchar](100) NULL,
	[Limit] [varchar](50) NULL,
	[Deductible] [money] NULL,
	[CoInsuranceForm] [varchar](50) NULL,
 CONSTRAINT [PK_LeadPolicyCoverage] PRIMARY KEY CLUSTERED 
(
	[CoverageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LeadPolicyCoverage]  WITH CHECK ADD  CONSTRAINT [FK_LeadPolicyCoverage_LeadPolicy] FOREIGN KEY([LeadPolicyID])
REFERENCES [dbo].[LeadPolicy] ([Id])
GO

ALTER TABLE [dbo].[LeadPolicyCoverage] CHECK CONSTRAINT [FK_LeadPolicyCoverage_LeadPolicy]
GO



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

/****** Object:  Table [dbo].[LeadPolicyLienholder]    Script Date: 12/18/2013 23:42:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeadPolicyLienholder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LeadID] [int] NULL,
	[MortgageeID] [int] NULL,
 CONSTRAINT [PK_LeadPolicyLienholder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LeadPolicyLienholder]  WITH CHECK ADD  CONSTRAINT [fk_LeadPolicyLienholder_Lead] FOREIGN KEY([LeadID])
REFERENCES [dbo].[Leads] ([LeadId])
GO

ALTER TABLE [dbo].[LeadPolicyLienholder] CHECK CONSTRAINT [fk_LeadPolicyLienholder_Lead]
GO

ALTER TABLE [dbo].[LeadPolicyLienholder]  WITH CHECK ADD  CONSTRAINT [fk_LeadPolicyLienholder_Mortgagee] FOREIGN KEY([MortgageeID])
REFERENCES [dbo].[Mortgagee] ([MortgageeID])
GO

ALTER TABLE [dbo].[LeadPolicyLienholder] CHECK CONSTRAINT [fk_LeadPolicyLienholder_Mortgagee]
GO


ALTER TABLE dbo.LeadPolicyLienholder
ADD CONSTRAINT fk_LeadPolicyLienholder_Mortgagee
FOREIGN KEY (MortgageeID)
REFERENCES dbo.Mortgagee(MortgageeID)


ALTER TABLE dbo.LeadPolicyLienholder
ADD CONSTRAINT fk_LeadPolicyLienholder_Lead
FOREIGN KEY (LeadId)
REFERENCES dbo.Leads(LeadID)


ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_LeadPolicyType
FOREIGN KEY (PolicyType)
REFERENCES dbo.LeadPolicyType(LeadPolicyTypeID)



alter table client add imapHost varchar(100)
alter table client add imapHostPort int
alter table client add imapHostUseSSL bit

alter table dbo.SecUser add isAddToEmailList bit
alter table Contact add DepartmentName varchar(50)
alter table Contact add ContactTitle varchar(50)



/****** Object:  Table [dbo].[Carrier]    Script Date: 12/12/2013 08:44:29 ******/
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

alter table LeadPolicy add CarrierID int
alter table Contact add CarrierID int
alter table LEadInvoice add PolicyID int

ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_Carrier
FOREIGN KEY (CarrierID)
REFERENCES dbo.Carrier(CarrierID)

/****** Object:  Table [dbo].[Ledger]    Script Date: 12/11/2013 16:26:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Ledger](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LeadID] [int] NULL,
	[InvoiceID] [int] NULL,
	[ClientName] [varchar](100) NULL,
	[ClientPaid] [datetime] NULL,
	[AdjusterID] [int] NULL,
	[AdjusterPaidDate] [date] NULL,
	[CommissionTotal] [money] NULL,
	[TotalExpenses] [money] NULL,
	[AdjusterNet] [money] NULL,
	[InvoiceTotal] [money] NULL,
 CONSTRAINT [PK_Ledger] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Ledger]  WITH CHECK ADD  CONSTRAINT [FK_Ledger_AdjusterMaster] FOREIGN KEY([AdjusterID])
REFERENCES [dbo].[AdjusterMaster] ([AdjusterId])
GO

ALTER TABLE [dbo].[Ledger] CHECK CONSTRAINT [FK_Ledger_AdjusterMaster]
GO

ALTER TABLE [dbo].[Ledger]  WITH CHECK ADD  CONSTRAINT [FK_Ledger_LeadInvoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[LeadInvoice] ([InvoiceID])
GO

ALTER TABLE [dbo].[Ledger] CHECK CONSTRAINT [FK_Ledger_LeadInvoice]
GO

ALTER TABLE [dbo].[Ledger]  WITH CHECK ADD  CONSTRAINT [FK_Ledger_Leads] FOREIGN KEY([LeadID])
REFERENCES [dbo].[Leads] ([LeadId])
GO

ALTER TABLE [dbo].[Ledger] CHECK CONSTRAINT [FK_Ledger_Leads]
GO



/****** Object:  Table [dbo].[TaskReminderMaster]    Script Date: 12/05/2013 11:00:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TaskReminderMaster](
	[TaskReminderID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Interval] [int] NOT NULL,
	[ClientID] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TaskReminderMaster] PRIMARY KEY CLUSTERED 
(
	[TaskReminderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

alter table task add ReminderInterval int default(0)
alter table task add IsReminder bit default(0)

/****** Object:  View [dbo].[vw_Reminder]    Script Date: 12/02/2013 08:13:49 ******/
USE [ClaimRuler_stage]
GO

/****** Object:  View [dbo].[vw_Reminder]    Script Date: 12/06/2013 15:48:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




ALTER view [dbo].[vw_Reminder]
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




		






alter table Task add PriorityID int
alter table TaskPriority add ClientID int

ALTER TABLE dbo.Task
ADD CONSTRAINT fk_Task_TaskPriority
FOREIGN KEY (PriorityID)
REFERENCES dbo.TaskPriority(PriorityID)



/****** Object:  Table [dbo].[TaskPriority]    Script Date: 11/20/2013 14:10:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TaskPriority](
	[PriorityID] [int] IDENTITY(1,1) NOT NULL,
	[PriorityName] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_TaskPriority] PRIMARY KEY CLUSTERED 
(
	[PriorityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO










alter table dbo.SecModule add SortOrder smallint default(0)
alter table InspectorMaster add ClientID int

alter table AdjusterMaster add CompanyName varchar(100)
alter table AdjusterMaster add FaxNumber varchar(20)
alter table AdjusterMaster add PhoneNumber varchar(20)
alter table Leads add Salutation varchar(50)
alter table Leads add ClaimantMiddleName varchar(50)
alter table LeadPolicy add PolicyPeriod varchar(50)


alter table Leads add LossLocation varchar(50)
alter table LeadInvoice add ClientID int
alter table LeadInvoice add InvoiceNumber int
alter table LeadInvoice add TaxRate smallmoney





alter table task add policy_type int
alter table dbo.LeadPolicy add PhoneNumber varchar(20)
alter table dbo.LeadPolicy add FaxNumber varchar(20)

alter table LeadComment add ReferenceID int

alter table leadinvoicedetail add Rate money
alter table leadinvoicedetail add UnitDescription varchar(50)
alter table leadinvoice add isVoid bit default(0)





alter table dbo.LeadInvoice add DueDate datetime
alter table dbo.LeadInvoice add Comments varchar(255)

alter table dbo.LeadInvoiceDetail add UnitID int

alter table dbo.LeadInvoiceDetail add isBillable bit default(1)



SET ANSI_PADDING OFF
GO
alter table LeadInvoice add DueDate datetime
alter table dbo.LeadInvoiceDetail add LineDate datetime
alter table dbo.LeadInvoiceDetail add [ServiceTypeID] int
alter table dbo.LeadInvoiceDetail add Qty numeric(8, 2)
alter table dbo.LeadInvoiceDetail add Total money default(0)
alter table dbo.LeadInvoiceDetail add Comments varchar(255)


ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [fk_Invoice_Lead] FOREIGN KEY([LeadId])
REFERENCES [dbo].[Leads] ([LeadId])
GO

ALTER TABLE [dbo].[LeadInvoice] CHECK CONSTRAINT [fk_Invoice_Lead]
GO









Create TRIGGER [dbo].[tgr_LeadPolicy_LeadStatus_Modified]
   ON [dbo].LeadPolicy
   AFTER UPDATE
AS 
BEGIN
SET NOCOUNT ON;
    IF UPDATE (LeadStatus) 
    begin

        UPDATE dbo.LeadPolicy
        SET LastStatusUpdate = GETDATE()
		FROM dbo.LeadPolicy AS l
		INNER JOIN	inserted AS i ON i.LeadId = l.LeadId
    end 

END


alter table leads add LastActivityDate datetime
ALTER TABLE dbo.LeadInvoiceDetail
ADD CONSTRAINT fk_LeadInvoiceDetail_ServiceType
FOREIGN KEY (ServiceTypeID)
REFERENCES dbo.InvoiceServiceType(ServiceTypeID)

ALTER TABLE dbo.InvoiceServiceType
ADD CONSTRAINT fk_InvoiceServiceType_InvoiceServiceUnit
FOREIGN KEY (UnitId)
REFERENCES dbo.InvoiceServiceUnit(UnitId)



alter table LeadPolicy add LastStatusUpdate datetime

alter table dbo.Client add InactivityDays int default(0)




alter table SecRole add isClient bit default(0)
alter table dbo.AdjusterMaster add userID int


ALTER TABLE dbo.AdjusterMaster
ADD CONSTRAINT fk_AdjusterMaster_SecUser
FOREIGN KEY (userID)
REFERENCES secUSer(userID)

alter table LeadPolicy add isAllDocumentUploaded bit default(0)
alter table dbo.AdjusterMaster add email varchar(100)
alter table dbo.AdjusterMaster add isEmailNotification bit default(1)


ALTER TABLE dbo.AdjusterMaster
ADD CONSTRAINT fk_AdjusterMaster_Client
FOREIGN KEY (clientid)
REFERENCES client(clientid)



alter table dbo.SecUser add isSSL bit default(1)
update SecUser set isSSL=1


alter table dbo.LeadsImage add isPrint bit default(1)
update LeadsImage set isPrint=1



alter table StatusMaster add isCountable bit default(1)
alter table StatusMaster add isCountAsOpen bit default(1)

/****** Object:  View [dbo].[vw_openLeadClaim]    Script Date: 09/21/2013 20:39:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[vw_openLeadClaim] 
as
select a.clientid, a.statusname, isnull(g.itemcount,0) as itemcount from statusMaster a
inner join (
	select sm.clientid, sm.statusid, itemCount = count(*) 
	from leadpolicy p 
	left join statusmaster sm on sm.statusid = p.leadstatus
	where p.leadstatus is not null
		and sm.[status] = 1
		and sm.isCountable = 1	
		and sm.isCountAsOpen = 1
	group by sm.clientid,sm.statusid
) g on a.statusid = g.statusid and a.clientid = a.clientid
--where a.clientid = 6
GO

/****** Object:  View [dbo].[vw_closeLeadClaim]    Script Date: 09/21/2013 20:38:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[vw_closeLeadClaim] 
as
select a.clientid, a.statusname, isnull(g.itemcount,0) as itemcount from statusMaster a
inner join (
	select sm.clientid, sm.statusid, itemCount = count(*) 
	from leadpolicy p 
	left join statusmaster sm on sm.statusid = p.leadstatus
	where p.leadstatus is not null
		and sm.[status] = 1
		and sm.isCountable = 1	
		and sm.isCountAsOpen = 0
	group by sm.clientid,sm.statusid
) g on a.statusid = g.statusid and a.clientid = a.clientid
--where a.clientid = 6
GO


insert into SubStatusMaster(SubStatusName,[Status],InsertBy,InsertDate,clientid)
select SubStatusName,[Status],InsertBy,InsertDate,6 from SubStatusMaster where clientID is null

alter table client add isShowTasks bit default(1)
alter table leads add SecondaryLeadSource varchar(50)

alter table dbo.SubStatusMaster add clientID int 

insert into dbo.FieldColumn(ColumnName) values('Last Name')
insert into dbo.FieldColumn(ColumnName) values('First Name')
insert into dbo.FieldColumn(ColumnName) values('Date Record Created')
insert into dbo.FieldColumn(ColumnName) values('Claim Number')
insert into dbo.FieldColumn(ColumnName) values('Status')
insert into dbo.FieldColumn(ColumnName) values('Sub Status')
insert into dbo.FieldColumn(ColumnName) values('Loss City')
insert into dbo.FieldColumn(ColumnName) values('Loss State')
insert into dbo.FieldColumn(ColumnName) values('Loss Zip')
insert into dbo.FieldColumn(ColumnName) values('Mailing City')
insert into dbo.FieldColumn(ColumnName) values('Mailing State')
insert into dbo.FieldColumn(ColumnName) values('Mailing Zip')
insert into dbo.FieldColumn(ColumnName) values('Lead Source')
insert into dbo.FieldColumn(ColumnName) values('Type of Damage')
insert into dbo.FieldColumn(ColumnName) values('Type Of Property')
insert into dbo.FieldColumn(ColumnName) values('Contractor')
insert into dbo.FieldColumn(ColumnName) values('Appraiser')
insert into dbo.FieldColumn(ColumnName) values('Umpire')
insert into dbo.FieldColumn(ColumnName) values('Primary Producer')
insert into dbo.FieldColumn(ColumnName) values('User Name')

/****** Object:  View [dbo].[vw_FieldColumn]    Script Date: 09/17/2013 17:25:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vw_FieldColumn] as
select fc.*, isnull(cfc.isVisible, cast(1 as bit)) as isVisible,cfc.clientid
from dbo.FieldColumn fc
left join dbo.ClientFieldColumn cfc on cfc.FieldColumnId = fc.ColumnID
GO



/****** Object:  Table [dbo].[FieldColumn]    Script Date: 09/17/2013 17:24:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FieldColumn](
	[ColumnID] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [varchar](50) NULL,
 CONSTRAINT [PK_FieldColumn] PRIMARY KEY CLUSTERED 
(
	[ColumnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO




/****** Object:  Table [dbo].[ClientFieldColumn]    Script Date: 09/17/2013 17:24:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientFieldColumn](
	[ClientID] [int] NOT NULL,
	[FieldColumnID] [int] NOT NULL,
	[isVisible] [bit] NULL,
 CONSTRAINT [PK_ClientFieldColumn] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC,
	[FieldColumnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



USE [CRM]
GO

/****** Object:  Table [dbo].[ClientLetterTemplate]    Script Date: 09/16/2013 17:42:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClientLetterTemplate](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NULL,
	[Description] [varchar](100) NULL,
	[Path] [varchar](100) NULL,
 CONSTRAINT [PK_ClientLetterTemplate] PRIMARY KEY CLUSTERED 
(
	[TemplateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



alter table leads add MailingAddress varchar(50)
alter table leads add MailingCity varchar(50)
alter table leads add MailingState varchar(20)
alter table leads add MailingZip varchar(10)

ALTER TABLE dbo.Leads
ADD CONSTRAINT fk_Lead_Contractor
FOREIGN KEY (ContractorID)
REFERENCES dbo.ContractorMaster(ContractorID)

alter table leads add CityName varchar(50)
alter table leads add StateName varchar(30)

ALTER TABLE dbo.Contact
ADD CONSTRAINT fk_Contact_StateMaster
FOREIGN KEY (StateID)
REFERENCES dbo.StateMaster(StateID)

ALTER TABLE dbo.Contact
ADD CONSTRAINT fk_Contact_CityMaster
FOREIGN KEY (CityId)
REFERENCES dbo.CityMaster(CityId)


ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicy_Adjuster
FOREIGN KEY (AdjusterID)
REFERENCES AdjusterMaster(AdjusterID)

ALTER TABLE dbo.Leads
ADD CONSTRAINT fk_Lead_Client
FOREIGN KEY (clientID)
REFERENCES client(ClientID)
/****** Object:  Table [dbo].[Contact]    Script Date: 09/10/2013 22:26:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Contact](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[CompanyName] [varchar](100) NULL,
	[ClaimName] [varchar](100) NULL,
	[CategoryID] [int] NULL,
	[Phone] [varchar](20) NULL,
	[Mobile] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[CityID] [int] NULL,
	[StateID] [int] NULL,
	[ZipCodeID] [int] NULL,
	[County] [varchar](50) NULL,
	[Balance] [money] NULL,
	[ClientID] [int] NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](50) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



alter table task add master_status_id int
alter table task add lead_policy_id int

USE [CRM]
GO

/****** Object:  Table [dbo].[ReminderMaster]    Script Date: 09/04/2013 18:04:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ReminderMaster](
	[ReminderID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](50) NULL,
	[Duration] [int] NULL,
 CONSTRAINT [PK_ReminderMaster] PRIMARY KEY CLUSTERED 
(
	[ReminderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



alter table dbo.StatusMaster add ReminderID int

ALTER TABLE dbo.StatusMaster
ADD CONSTRAINT fk_ReminderMaster_StatusMaster
FOREIGN KEY (ReminderID)
REFERENCES dbo.ReminderMaster(ReminderID)




USE [CRM]
GO

/****** Object:  Table [dbo].[UmpireMaster]    Script Date: 09/03/2013 17:48:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UmpireMaster](
	[UmpireId] [int] IDENTITY(1,1) NOT NULL,
	[UmpireName] [varchar](100) NULL,
	[Status] [bit] NULL,
	[InsertBy] [int] NULL,
	[InsertDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[ClientId] [int] NULL,
	[FederalTaxID] [varchar](20) NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](50) NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[ZipCode] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
 CONSTRAINT [PK_UmpireMaster] PRIMARY KEY CLUSTERED 
(
	[UmpireId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UmpireMaster] ADD  CONSTRAINT [DF_UmpireMaster_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].[UmpireMaster] ADD  CONSTRAINT [DF_UmpireMaster_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO




/****** Object:  Table [dbo].[ContractorMaster]    Script Date: 09/03/2013 17:48:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ContractorMaster](
	[ContractorId] [int] IDENTITY(1,1) NOT NULL,
	[ContractorName] [varchar](100) NULL,
	[Status] [bit] NULL,
	[InsertBy] [int] NULL,
	[InsertDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[ClientId] [int] NULL,
	[FederalTaxID] [varchar](20) NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](50) NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[ZipCode] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
 CONSTRAINT [PK_ContractorMaster] PRIMARY KEY CLUSTERED 
(
	[ContractorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ContractorMaster] ADD  CONSTRAINT [DF_ContractorMaster_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].[ContractorMaster] ADD  CONSTRAINT [DF_ContractorMaster_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO





/****** Object:  Table [dbo].[AppraiserMaster]    Script Date: 09/03/2013 17:47:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AppraiserMaster](
	[AppraiserId] [int] IDENTITY(1,1) NOT NULL,
	[AppraiserName] [varchar](100) NULL,
	[Status] [bit] NULL,
	[InsertBy] [int] NULL,
	[InsertDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[ClientId] [int] NULL,
	[FederalTaxID] [varchar](20) NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](50) NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[ZipCode] [varchar](10) NULL,
	[BusinessName] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
 CONSTRAINT [PK_AppraiserMaster] PRIMARY KEY CLUSTERED 
(
	[AppraiserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[AppraiserMaster] ADD  CONSTRAINT [DF_AppraiserMaster_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].[AppraiserMaster] ADD  CONSTRAINT [DF_AppraiserMaster_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO







ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicyStatus
FOREIGN KEY (LeadStatus)
REFERENCES dbo.StatusMaster(statusId)

ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicySubStatus
FOREIGN KEY (SubStatus)
REFERENCES dbo.SubStatusMaster(SubStatusId)



ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolictyState
FOREIGN KEY (InsuranceState)
REFERENCES dbo.StateMaster(StateId)

ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolictyCityMaster
FOREIGN KEY (InsuranceCity)
REFERENCES dbo.CityMaster(CityId)

ALTER TABLE dbo.LeadPolicy
ADD CONSTRAINT fk_LeadPolicty
FOREIGN KEY (LeadId)
REFERENCES dbo.Leads(LeadId)

alter table leadinvoice add PolicyTypeID int
alter table leadinvoice add AdjusterInvoiceNumber varchar(20)

alter table dbo.AdjusterMaster add FederalTaxID varchar(20)
alter table dbo.AdjusterMaster add Address1 varchar(100)
alter table dbo.AdjusterMaster add Address2 varchar(50)
alter table dbo.AdjusterMaster add StateID int
alter table dbo.AdjusterMaster add CityID int
alter table dbo.AdjusterMaster add ZipCode varchar(10)
alter table dbo.AdjusterMaster add FeePerContract money


alter table LeadInvoice add BillToName varchar(100)
alter table LeadInvoice add BillToAddress1 varchar(100)
alter table LeadInvoice add BillToAddress2 varchar(50)
alter table LeadInvoice add BillToAddress3 varchar(100)


alter table dbo.Client add FeePerContract money
alter table dbo.Client add FederalIDNo varchar(20)

alter table dbo.LeadPolicy add InsurerFileNo varchar(20)


ALTER TABLE dbo.LeadContact
ADD CONSTRAINT fk_LeadContactType
FOREIGN KEY (ContactTypeID)
REFERENCES dbo.LeadContactType(ID)



/****** Object:  Table [dbo].[LeadInvoice]    Script Date: 08/18/2013 20:57:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeadInvoice](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceDate] [datetime] NULL,
	[LeadId] [int] NULL,
	[TotalAmount] [money] NULL,
 CONSTRAINT [PK_LeadInvoice] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LeadInvoice]  WITH CHECK ADD  CONSTRAINT [fk_Invoice_Lead] FOREIGN KEY([LeadId])
REFERENCES [dbo].[Leads] ([LeadId])
GO

ALTER TABLE [dbo].[LeadInvoice] CHECK CONSTRAINT [fk_Invoice_Lead]
GO


/****** Object:  Table [dbo].[LeadInvoiceDetail]    Script Date: 08/18/2013 20:57:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadInvoiceDetail](
	[InvoiceLineID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceID] [int] NULL,
	[LineDescription] [varchar](100) NULL,
	[LineAmount] [money] NULL,
	[LineItemNo] [int] NULL,
 CONSTRAINT [PK_InvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[InvoiceLineID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LeadInvoiceDetail]  WITH CHECK ADD  CONSTRAINT [fk_LeadInvoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[LeadInvoice] ([InvoiceID])
GO

ALTER TABLE [dbo].[LeadInvoiceDetail] CHECK CONSTRAINT [fk_LeadInvoice]
GO


alter table dbo.LeadsImage add policyTypeID int

/****** Object:  Table [dbo].[LeadDocumentList]    Script Date: 08/16/2013 14:33:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LeadDocumentList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [int] NULL,
	[DocumentListId] [int] NULL,
 CONSTRAINT [PK_DocumentList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LeadDocumentList]  WITH CHECK ADD  CONSTRAINT [fk_LeadDocumentList] FOREIGN KEY([DocumentListId])
REFERENCES [dbo].[DocumentListMaster] ([DocumentListId])
GO

ALTER TABLE [dbo].[LeadDocumentList] CHECK CONSTRAINT [fk_LeadDocumentList]
GO



insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Copy of Insurance Policy',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Signed Retainer or Public Adjusting Contract',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('All Property Damage Photos',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Certified Copy of Insurance Policy from Insurance Company',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Property Owner Contracts for Repair, Estimates for Repair, Repair Costs Incurred Docs',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Contents List, Additional Living Expenses(ALE) List',1, 1)
insert into dbo.DocumentListMaster(DocumentName,PolicyTypeId,IsActive) values('Completed Property Damage Estimate',1, 1)


alter table dbo.LeadComment add PolicyId int
alter table LeadContact add PolicyTypeID int





/****** Object:  Table [dbo].[LeadPolicy]    Script Date: 08/14/2013 08:20:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadPolicy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [int] NULL,
	[InsuranceCompanyName] [varchar](50) NULL,
	[InsuranceAddress] [varchar](50) NULL,
	[InsuranceState] [int] NULL,
	[InsuranceCity] [int] NULL,
	[InsuranceZipCode] [varchar](10) NULL,
	[PolicyNumber] [varchar](100) NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[LeadPolicy] ADD [ClaimNumber] [varchar](50) NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [PolicyType] [int] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [LeadStatus] [int] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [SubStatus] [int] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [SiteSurveyDate] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [SiteInspectionCompleted] [datetime] NULL
ALTER TABLE [dbo].[LeadPolicy] ADD [AdjusterID] [int] NULL
/****** Object:  Index [PK_LeadPolicy]    Script Date: 08/14/2013 08:20:50 ******/
ALTER TABLE [dbo].[LeadPolicy] ADD  CONSTRAINT [PK_LeadPolicy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO





update SecRoleModule set createdon='2013-08-09'
update SecRoleModule set updatedon ='2013-08-09' where updatedon is null

update SecRoleModule set createdby = 1 where createdby is null
update SecRoleModule set updatedby = 1 where updatedby is null


alter table leads add CommericalInsuranceCompanyName varchar(50)
alter table leads add CommercialInsuranceContactName varchar(50)
alter table leads add CommercialInsuranceContactPhone varchar(20)
alter table leads add CommercialInsuranceContactEmail varchar(100)
alter table leads add CommercialInsuranceAddress varchar(50)
alter table leads add CommercialInsuranceState int
alter table leads add CommercialInsuranceCity int
alter table leads add CommercialInsuranceZipCode varchar(10)
alter table leads add CommercialInsurancePolicyNumber varchar(100)


alter table leads add EarthquakeInsuranceCompanyName varchar(50)
alter table leads add EarthquakeInsuranceContactName varchar(50)
alter table leads add EarthquakeInsuranceContactPhone varchar(20)
alter table leads add EarthquakeInsuranceContactEmail varchar(100)
alter table leads add EarthquakeInsuranceAddress varchar(50)
alter table leads add EarthquakeInsuranceState int
alter table leads add EarthquakeInsuranceCity int
alter table leads add EarthquakeInsuranceZipCode varchar(10)
alter table leads add EarthquakeInsurancePolicyNumber varchar(100)

alter table dbo.ProducerMaster add ClientId int
alter table  dbo.AdjusterMaster add ClientId int
alter table dbo.LeadSourceMaster  add ClientId int
alter table dbo.OtherSourceMaster  add ClientId int
alter table dbo.SecondaryProducerMaster add ClientID int



ALTER TABLE SecUser
ADD CONSTRAINT fk_ClientSecUSer
FOREIGN KEY (ClientID)
REFERENCES dbo.Client(ClientId)


alter table dbo.SecUser add emailPassword varchar(100)
alter table dbo.SecUser add emailSignature text
alter table secuser add emailHost varchar(100)
alter table secuser add emailHostPort varchar(10)

alter table dbo.SecUser add ClientID int

alter table leads add ClientID int

/****** Object:  Table [dbo].[Clients]    Script Date: 07/29/2013 21:05:15 ******/

/****** Object:  Table [dbo].[Client]    Script Date: 07/30/2013 13:47:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Client](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[StateId] [int] NULL,
	[CityId] [int] NULL,
	[ZipCode] [nvarchar](20) NULL,
	[StreetAddress1] [nvarchar](250) NULL,
	[StreetAddress2] [nvarchar](250) NULL,
	[PrimaryPhoneNo] [nvarchar](20) NULL,
	[PrimaryEmailId] [nvarchar](200) NULL,
	[SecondaryPhoneNo] [nvarchar](20) NULL,
	[SecondaryEmailId] [nvarchar](200) NULL,
	[EmailHost] [varchar](100) NULL,
	[EmailHostPort] [varchar](10) NULL,
	[Active] [int] NULL,
	[Status] [int] NULL,
	[InsertDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[BusinessName] [varchar](100) NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_CityMaster] FOREIGN KEY([CityId])
REFERENCES [dbo].[CityMaster] ([CityId])
GO

ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_CityMaster]
GO

ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_SecUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[SecUser] ([UserId])
GO

ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_SecUser]
GO

ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_StateMaster] FOREIGN KEY([StateId])
REFERENCES [dbo].[StateMaster] ([StateId])
GO

ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_StateMaster]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
GO

ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO






ALTER TABLE Task
ADD CONSTRAINT fk_Lead
FOREIGN KEY (lead_id)
REFERENCES dbo.Leads(LeadId)



ALTER TABLE Task
ADD CONSTRAINT fk_SecUSer
FOREIGN KEY (owner_id)
REFERENCES dbo.SecUser(UserID)







SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[InsuranceType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_InsuranceType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

alter table LeadContact add InsuranceTypeID int

insert into dbo.InsuranceType(Description) values('Homeowner')
insert into dbo.InsuranceType(Description) values('Wind')
insert into dbo.InsuranceType(Description) values('Flood')
insert into dbo.InsuranceType(Description) values('Earthquake')
insert into dbo.InsuranceType(Description) values('Fire')
insert into dbo.InsuranceType(Description) values('Commercial')

ALTER TABLE LeadContact
ADD CONSTRAINT fk_LeadContactInsuranceType
FOREIGN KEY (InsuranceTypeID)
REFERENCES dbo.InsuranceType(ID)







insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype)
values('Letter Printing', 'Letter Printing','LetterPrinting.aspx',1,'2013-07-15', 1, '2013-07-15',1,0)

insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Contract for Adjusting Services', 'Contract for Adjusting Services','ContractAdjustingServices.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, null)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, paretid)
values('Damage Details Survey', 'Damage Details Survey','DamageDetailsSurvey.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, null)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Interim Proof of Loss', 'Interim Proof of Loss','InterimProofLoss.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)

insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Letter to Insured on Notice of Claim', 'Letter to Insured on Notice of Claim','InsuredNoticeofClaim.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Notice to Insurer', 'Notice to Insurer','NoticetoInsurer.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Proof of Loss', 'Proof of Loss','ProofofLoss.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Schedule of Contents', 'Schedule of Contents','ScheduleofContents.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Sample Report', 'Sample Report','SampleReport.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


ALTER TABLE LeadContact
ADD CONSTRAINT fk_LeadContactType
FOREIGN KEY (ContactTypeID)
REFERENCES dbo.LeadContactType(ID)

insert into dbo.LeadContactType(Description,isActive) values('Public Adjuster', 1)
insert into dbo.LeadContactType(Description,isActive) values('Contractor', 1)
insert into dbo.LeadContactType(Description,isActive) values('Insurance Adjuster Wind', 1)
insert into dbo.LeadContactType(Description,isActive) values('Independent Adjuster Flood', 1)
insert into dbo.LeadContactType(Description,isActive) values('Insurance Claim Processor', 1)

USE [CRM]
GO

/****** Object:  Table [dbo].[LeadContactType]    Script Date: 07/19/2013 17:42:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadContactType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LeadContactType] ADD  CONSTRAINT [DF_ContactType_isActive]  DEFAULT ((1)) FOR [isActive]
GO




USE [CRM]
GO

/****** Object:  Table [dbo].[LeadContact]    Script Date: 07/19/2013 17:43:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LeadContact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactName] [varchar](100) NULL,
	[ContactTypeID] [int] NULL,
	[Phone] [varchar](10) NULL,
	[Mobile] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
	[DateCreated] [datetime] NULL,
	[isActive] [bit] NULL,
 CONSTRAINT [PK_LeadContact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LeadContact] ADD  CONSTRAINT [DF_LeadContact_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[LeadContact] ADD  CONSTRAINT [DF_LeadContact_isActive]  DEFAULT ((1)) FOR [isActive]
GO





select * from dbo.SecModule
select * from dbo.SecRoleModule

-- report printing
insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype)
values('Letter Printing', 'Letter Printing','LetterPrinting.aspx',1,'2013-07-15', 1, '2013-07-15',1,0)

insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Contract for Adjusting Services', 'Contract for Adjusting Services','ContractAdjustingServices.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, null)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, paretid)
values('Damage Details Survey', 'Damage Details Survey','DamageDetailsSurvey.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, null)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Interim Proof of Loss', 'Interim Proof of Loss','InterimProofLoss.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)

insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Letter to Insured on Notice of Claim', 'Letter to Insured on Notice of Claim','InsuredNoticeofClaim.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Notice to Insurer', 'Notice to Insurer','NoticetoInsurer.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Proof of Loss', 'Proof of Loss','ProofofLoss.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecModule(ModuleName,ModuleDesc,url,[status], createdon, createdby, updatedon, updatedby, moduletype, parentid)
values('Schedule of Contents', 'Schedule of Contents','ScheduleofContents.aspx',1,'2013-07-15', 1, '2013-07-15',1,0, 74)


insert into dbo.SecRoleModule(RoleID,ModuleID,ViewPermission,Status) values(13,73,1, 1)
insert into dbo.SecRoleModule(RoleID,ModuleID,ViewPermission,Status) values(1,81,1, 1)
alter view vw_Lead_Search
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
left join LeadPolicy p  with(nolock) on l.LeadID = p.LeadId 
inner join LeadPolicyType policyType  with(nolock) on policyType.LeadPolicyTypeID = p.PolicyType
left join Carrier car  with(nolock) on car.carrierID = p.CarrierID
left join Claim c  with(nolock) on c.policyID = p.ID
left join ProducerMaster producer with(nolock)  on producer.ProducerId = l.PrimaryProducerId
left join dbo.UmpireMaster umpire with(nolock)  on umpire.UmpireId = l.UmpireId
left join AppraiserMaster appraiser  with(nolock) on appraiser.AppraiserId = l.AppraiserId
left join dbo.ContractorMaster contractor with(nolock)  on contractor.ContractorId = l.ContractorId
left join LeadSourceMaster ls  with(nolock) on ls.LeadSourceID = l.LeadSource
left join AdjusterMaster adj  with(nolock) on  adj.AdjusterID = c.AdjusterId
left join StatusMaster statusMaster  with(nolock) on statusMaster.StatusID = c.StatusID
left join SubStatusMaster substatusMaster  with(nolock) on substatusMaster.SubStatusID = c.SubStatusID
where  p.isActive = 1 and c.IsActive = 1
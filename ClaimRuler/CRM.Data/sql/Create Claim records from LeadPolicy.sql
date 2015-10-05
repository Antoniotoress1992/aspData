INSERT INTO [Claim]
           ([PolicyID],[StatusID],[SubStatusID],[AdjusterID],[AdjusterClaimNumber],[SupervisorID],[TeamLeadID],[ManagerID]
           ,[ManagerEntityID],[SeverityNumber],[EventType],[EventName],[ClaimWorkflowType],[CauseOfLoss],[LossDate],[DateOpenedReported]
           ,[DateInitialReserveChange],[DateAssigned],[DateAcknowledged],[DateFirstContactAttempt],[DateContacted],[DateInspectionScheduled]
           
           ,[DateInspectionCompleted],[DateSubmitted],[DateIndemnityPaymentRequested],[DateIndemnityPaymentApproved],[DateIndemnityPaymentIssued]
           ,[DateClosed],[DateFirstReOpened],[DateReopenCompleted],[DateFinalClosed],[PolicyFormType],[IsActive],[IsInvoiced],[FeeInvoiceDesignation]
           ,[GrossLossPayable],[Depreciation],[Deductible],[OutstandingIndemnityReserve],[OutstandingLAEReserves],[TotalIndemnityPaid]
           
           ,[CoverageAPaid],[CoverageBPaid],[CoverageCPaid],[CoverageDPaid],[TotalExpensesPaid],[NetClaimPayable],[DateCompleted]
           ,[DateFirstClosed],[IsInvoiceReady],[DamageType],[CasualtyFeeInvoiceDesignation],[CasualtyGrossClaimPayable],[InsurerClaimNumber]
           ,LastStatusUpdate
           )
SELECT p.[Id],[LeadStatus],[SubStatus],[AdjusterID],[ClaimNumber],[SupervisorID],[TeamLeadID],[ManagerID]
		,[ManagerEntityID]=null,[SeverityNumber]=null,[EventType]=null,[EventName]=null,[ClaimWorkflowType]=null,[CauseOfLoss]=dbo.f_getDamageID(p.id),[LossDate],[DateOpen]
      ,[DateInitialReserveChange],[DateAssigned],[DateAcknowledged],[DateFirstContactAttempt],[DateContacted],[SiteSurveyDate]
      
      ,[SiteSurveyDateCompleted],[DateSubmitted],[DateIndemnityPaymentRequested],[DateIndemnityPaymentApproved],[DateIndemnityPaymentIssued]
      ,[DateClosed],[DateFirstReOpened],[DateReopenCompleted]=null,[DateFinalClosed]=null,[PolicyFormType],[IsActive]=1,[IsInvoiced],[ClaimDesignationID]
      ,[GrossLossPayable],[Depreciation],[Deductible],null,null,null
      
      ,[CoverageAPaid]=null,[CoverageBPaid]=null,[CoverageCPaid]=null,[CoverageDPaid]=null,[TotalExpensesPaid]=null,[NetClaimPayable],[DateCompleted]      
      ,[DateFirstClosed]=null,[IsInvoiceReady],[DamageType]=null,[CasualtyFeeInvoiceDesignation]=null,[CasualtyGrossClaimPayable]=0,[InsurerFileNo]
      ,[LastStatusUpdate] 
      --,[isAllDocumentUploaded]
      --,                                
  FROM [dbo].[LeadPolicy] p  
  where isActive=1
  

  
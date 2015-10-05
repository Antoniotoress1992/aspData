/*
rem 2014-01-20 tortega
rem step 1. run this batch @ 12:01am mondays via Windows TaskManager
rem step 2. run SQL Job at 1:00 am to email salesreport.csv located in c:\sites\reports
rem
cd\sites\reports
sqlcmd -S (local) -d occdb -E   ^
    -Q "exec spr_salesReport" 
	
sqlcmd -S (local) -d occdb -E -o "SalesReport.csv"  ^
    -Q "select replace(PortalName,',','') as PortalName,DatabaseName,BusinessCount,IndividualCount,StartupCount,FailCausationCount,CMTCount,MTCount,OPACount,RecentClaim from occdb.dbo.SalesReport" ^
    -W -w 999 -s","
*/
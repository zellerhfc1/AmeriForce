select rating_sort, * from contacts where id='0034W00002OQ55yQAD'

select rating_sort, * from contacts where update_needed <> ''

select * from companies where id=''


select count(id) from clients

select * from clients where id='0064W00001ADoMhQAL'

SELECT COUNT(ID) FROM CRMTASKS

select count(crmtasks.WhoId) as theCount, contacts.Id from contacts left join crmtasks on crmtasks.WhoId = contacts.Id group by contacts.Id order by theCount desc

select * from crmtasks where subject like 'Email:%'

select rating_sort, * from contacts where Rating_Sort='A'

SELECT * FROM CRMTASKS WHERE wHOiD='0034W00002OPdhIQAT'

select count(crmtasks.WhoId) as theCount, contacts.Id, contacts.OwnerId  from contacts left join crmtasks on crmtasks.WhoId = contacts.Id WHERE contacts.Rating_Sort='A' group by contacts.Id, contacts.OwnerId order by theCount desc

select * from aspnetusers where Id='0054W00000C3cWUQAZ'

SELECT * FROM CRMTASKS WHERE subject like '%brownies%'

select * from crmtasks where whoid='0034W00002OPG9GQAX'

select count(crmtasks.WhatId) as theCount, Clients.Id, Clients.OwnerId, clients.StageName, clients.CloseDate  from clients left join crmtasks on crmtasks.WhatId = Clients.Id  
	group by Clients.Id, Clients.OwnerId, clients.StageName, clients.CloseDate order by clients.CloseDate desc, theCount desc

select * from clients where id='0064W00001ACptxQAD'

select count(id) from emailmessages

select * from contacts where email='jean@usfoodgroups.com' or alt_email='jean@usfoodgroups.com'

select * from crmtasks where id='00T4W00005jlQQMUA2'

select * from clientcontactroles

select clientId, count(contactId) as contactCountPerClient from clientcontactroles group by clientId order by contactCountPerClient desc

select top 100 * from contacts where lastname = 'Bobby'

------------ BILL'S DASHBOARD ---------------------------

--Top 20 Referral Sources (People)
select top 20 Referring_contact, count(REFERRING_CONTACT) as theCount from clients where Referring_Contact <> '' AND StageName='50-Funded' and Initial_Funding > '01/01/2019'
	group by referring_contact
	order by thecount desc

	
--Top 20 Referral Sources (Companies)
select top 20 Referring_Company, count(Referring_Company) as theCount, com.Name from clients cl
		inner join Companies com on cl.Referring_Company = com.Id
	where Referring_Company <> '' AND StageName='50-Funded' and DATEDIFF(DAY, Initial_Funding, CURRENT_TIMESTAMP) <= 730
		group by Referring_Company, com.Name
		order by thecount desc, com.Name

select * from companies where id='0014W00002G4T00QAF'
	
	
--Pipeline - Leads by BDO
Select (FirstName + ' ' + LastName) as OwnerName, OwnerId, count(OwnerId) as theCount from clients 
	inner join aspnetusers on clients.OwnerId = aspnetusers.Id
	where stageName='00-Initial Review' 
	group by OwnerId, FirstName, LastName
	order by thecount desc

	select count(LeadSource) from clients where LeadSource <> 'Client Referral - Not a New Deal'


-- New Deals Funded Recently - Count
select YEAR(CloseDate) as ClosedYear, MONTH(CloseDate) as ClosedMonth, COUNT(Id) AS TotalClosed 
from clients 
	where 
	stageName='50-FUNDED' 
		and LeadSource <> 'Client Referral - Not a New Deal' 
		and DATEDIFF(month, Initial_Funding, CURRENT_TIMESTAMP) <= 25
		GROUP BY YEAR(CloseDate), MONTH(CloseDate)
		ORDER BY YEAR(CloseDate), MONTH(CloseDate)

		
-- New Deals Funded Recently - Amount
select YEAR(CloseDate) as ClosedYear, MONTH(CloseDate) as ClosedMonth, SUM(Amount) AS TotalAmountClosed 
from clients 
	where 
	stageName='50-FUNDED' 
		and LeadSource <> 'Client Referral - Not a New Deal' 
		and DATEDIFF(month, Initial_Funding, CURRENT_TIMESTAMP) <= 25
		GROUP BY YEAR(CloseDate), MONTH(CloseDate)
		ORDER BY YEAR(CloseDate), MONTH(CloseDate)

--New Deals, by Lead Source
select LeadSource, SUM(Amount) AS TotalAmountClosed 
from clients 
	where 
	stageName='50-FUNDED' 
		and LeadSource <> 'Client Referral - Not a New Deal' 
		and DATEDIFF(DAY, Initial_Funding, CURRENT_TIMESTAMP) <= 730
		GROUP BY LeadSource
		ORDER BY LeadSource


-- MY CONTACTS
select count(relationship_status), Relationship_Status from Contacts con
	inner join Companies com on con.AccountId = com.Id
	where ownerid='0054W00000C3cWFQAZ'
		and com.CompanyType = 'Referral Partner'
			and Update_Needed = ''
			group by Relationship_Status


-- Contacts - Email or Address Update Needed
select count(c.ownerId), (a.FirstName + ' ' + a.LastName) as ownername from contacts c
	inner join aspnetusers a on c.OwnerId = a.Id
	where update_needed <> ''
		and mailing_lists not like '%competitor%'
		and mailing_lists like '%client%'
		and mailing_lists not like '%prospect%'
			group by ownerid, Update_Needed, a.FirstName, a.LastName
			order by count(c.OwnerId) desc


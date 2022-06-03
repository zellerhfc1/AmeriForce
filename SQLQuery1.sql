select count(ID) from companies

select * from companies where id='0014W00002G4YUrQAN'

select count(ID) from contacts



select * from contacts where id='0034W00002ay1meQAA'

update contacts set NextActivityID='00Te0-e1b967517cbc' where id='0034W00002ay1meQAA'

select * from contacts where id='0034W00002OPLmiQAH'

select distinct(mailing_lists) from contacts

select * from aspnetusers where id='0054W00000C3cWDQAZ'

select distinct(rating_sort) from contacts

select relationship_status, * from contacts where rating_sort='Christmas Card 2015'
select relationship_status, * from contacts where rating_sort='N/A'
select relationship_status, * from contacts where rating_sort='Do not call list'
select relationship_status, * from contacts where rating_sort='CC'
select relationship_status, * from contacts where rating_sort=''
select relationship_status, * from contacts where rating_sort='1'

select update_needed, rating_sort, Relationship_Status, * from contacts where update_needed <> ''
select update_needed, rating_sort, Relationship_Status, * from contacts where referring_contact <> ''

select update_needed, rating_sort, Relationship_Status, * from contacts where id='0034W00002OPci5QAD' 

select distinct(companytype) from companies
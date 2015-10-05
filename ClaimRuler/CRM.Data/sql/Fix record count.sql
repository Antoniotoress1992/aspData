alter table LeadPolicy add IsActive bit default(1) not null

update LeadPolicy set IsActive = 1


update LeadPolicy set Isactive=0 where id between 1297 and 1312
update LeadPolicy set Isactive=0 where id between 2421 and 2428
update LeadPolicy set Isactive=0 where id between 1593 and 1596
update LeadPolicy set Isactive=0 where id between 1605 and 1608
update LeadPolicy set Isactive=0 where id between 1629 and 1632
update LeadPolicy set Isactive=0 where id between 1861 and 1880
update LeadPolicy set Isactive=0 where id between 797 and 800
update LeadPolicy set Isactive=0 where id between 1617 and 1620
update LeadPolicy set Isactive=0 where id between 497 and 508
update LeadPolicy set Isactive=0 where id between 1953 and 1960
update LeadPolicy set Isactive=0 where id between 609 and 612
update LeadPolicy set Isactive=0 where id between 2049 and 2064
update LeadPolicy set Isactive=0 where id between 1317 and 1320
update LeadPolicy set Isactive=0 where id between 1325 and 1332
update LeadPolicy set Isactive=0 where id between 1725 and 1740
update LeadPolicy set Isactive=0 where id between 129 and 132
update LeadPolicy set Isactive=0 where id between 149 and 152
update LeadPolicy set Isactive=0 where id between 157 and 160
update LeadPolicy set Isactive=0 where id between 165 and 168
update LeadPolicy set Isactive=0 where id between 173 and 176
update LeadPolicy set Isactive=0 where id between 181 and 184
update LeadPolicy set Isactive=0 where id between 189 and 196
update LeadPolicy set Isactive=0 where id between 201 and 208
update LeadPolicy set Isactive=0 where id between 213 and 216
update LeadPolicy set Isactive=0 where id between 221 and 228
update LeadPolicy set Isactive=0 where id between 233 and 236
update LeadPolicy set Isactive=0 where id between 277 and 280
update LeadPolicy set Isactive=0 where id between 285 and 288
update LeadPolicy set Isactive=0 where id between 297 and 304
update LeadPolicy set Isactive=0 where id between 309 and 312
update LeadPolicy set Isactive=0 where id between 2445 and 2448
update LeadPolicy set Isactive=0 where id between 321 and 324
update LeadPolicy set Isactive=0 where id between 333 and 336
update LeadPolicy set Isactive=0 where id between 341 and 344
update LeadPolicy set Isactive=0 where id between 349 and 352
update LeadPolicy set Isactive=0 where id between 357 and 360
update LeadPolicy set Isactive=0 where id between 373 and 376
update LeadPolicy set Isactive=0 where id between 661 and 668
update LeadPolicy set Isactive=0 where id between 873 and 876
update LeadPolicy set Isactive=0 where id between 1505 and 1508
update LeadPolicy set Isactive=0 where id between 1137 and 1140
update LeadPolicy set Isactive=0 where id between 1153 and 1156
update LeadPolicy set Isactive=0 where id between 1173 and 1176
update LeadPolicy set Isactive=0 where id between 1189 and 1192
update LeadPolicy set Isactive=0 where id between 1209 and 1212
update LeadPolicy set Isactive=0 where id between 1229 and 1232
update LeadPolicy set Isactive=0 where id between 1241 and 1244
update LeadPolicy set Isactive=0 where id between 1257 and 1260
update LeadPolicy set Isactive=0 where id between 1289 and 1292
update LeadPolicy set Isactive=0 where id between 865 and 868
update LeadPolicy set Isactive=0 where id between 628 and 632


select * from leads where claimantLastName like 'Howland%'

select * from leads where leadid=999

select * from leadpolicy where leadid=1018


select g.* from leads l
inner join (
	select leadid,count(*) as count_1
	from leadpolicy
	group by leadid having count(*) > 4
) g on g.leadid = l.leadid
where l.clientid=7


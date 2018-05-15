alter table filming_location add column key uuid;
update filming_location set key = gen_random_uuid();
alter table filming_location alter column key set not null;

alter table filming_location drop constraint pk_filming_location;
alter table filming_location add primary key (key);
alter table filming_location rename constraint filming_location_pkey to pk_filming_location;

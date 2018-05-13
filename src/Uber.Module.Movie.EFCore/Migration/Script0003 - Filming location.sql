alter table movie drop column fun_facts;

create table filming_location
(
	movie_key uuid not null,
	address_key uuid not null,
	fun_fact text,
	constraint pk_filming_location primary key (movie_key, address_key)
);

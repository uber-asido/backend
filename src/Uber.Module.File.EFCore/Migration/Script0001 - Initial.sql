create table upload_history
(
	key uuid not null,
	filename text not null,
	status integer not null,
	error text,
	timestamp timestamp(6) with time zone not null,
	constraint pk_search_item primary key (key)
)

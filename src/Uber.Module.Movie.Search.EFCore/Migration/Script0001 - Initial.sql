create table search_item
(
	key uuid not null,
	text text not null,
	type integer not null,
	constraint pk_search_item primary key (key)
)

create table location
(
	key uuid not null,
	address_key uuid not null,
	unformatted_address text not null,
	constraint pk_location primary key (key)
);

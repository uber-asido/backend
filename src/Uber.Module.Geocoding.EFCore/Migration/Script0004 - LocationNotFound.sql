create table location_not_found
(
	key uuid not null,
	unformatted_address text not null,
	constraint pk_location_not_found primary key (key)
);

create index idx_location_not_found_unformatted_address on location_not_found (unformatted_address);

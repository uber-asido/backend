create table address
(
	key uuid not null,
	formatted_address text not null,
	longitude double precision not null,
	latitude double precision not null,
	constraint pk_address primary key (key)
);

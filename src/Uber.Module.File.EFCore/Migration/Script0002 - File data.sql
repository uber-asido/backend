create table file_data
(
	key uuid not null,
	data bytea not null,
	constraint pk_file_data primary key (key)
)

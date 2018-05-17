create table search_item_target
(
	search_item_key uuid not null,
	target_key uuid not null,
	constraint pk_search_item_target primary key (search_item_key, target_key)
);

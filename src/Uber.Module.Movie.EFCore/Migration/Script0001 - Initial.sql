create table movie
(
	key uuid not null,
	title text not null,
	release_year integer not null,
	fun_facts text[] not null,
	constraint pk_search_item primary key (key)
);

create table actor
(
	key uuid not null,
	full_name text not null,
	constraint pk_actor primary key (key)
);

create table distributor
(
	key uuid not null,
	name text not null,
	constraint pk_distributor primary key (key)
);

create table production_company
(
	key uuid not null,
	name text not null,
	constraint pk_production_company primary key (key)
);

create table writer
(
	key uuid not null,
	full_name text not null,
	constraint pk_writer primary key (key)
);

create table movie_actor
(
	movie_key uuid not null,
	actor_key uuid not null,
	constraint pk_movie_actor primary key (movie_key, actor_key),
	constraint fk_movie_actor_movie_key foreign key (movie_key) references movie (key),
	constraint fk_movie_actor_actor_key foreign key (actor_key) references actor (key)
);

create table movie_distributor
(
	movie_key uuid not null,
	distributor_key uuid not null,
	constraint pk_movie_distributor primary key (movie_key, distributor_key),
	constraint fk_movie_distributor_movie_key foreign key (movie_key) references movie (key),
	constraint fk_movie_distributor_distributor_key foreign key (distributor_key) references distributor (key)
);

create table movie_filming_address
(
	movie_key uuid not null,
	address_key uuid not null,
	constraint pk_movie_filming_address primary key (movie_key, address_key),
	constraint fk_movie_filming_address_movie_key foreign key (movie_key) references movie (key)
);

create table movie_production_company
(
	movie_key uuid not null,
	production_company_key uuid not null,
	constraint pk_movie_production_company primary key (movie_key, production_company_key),
	constraint fk_movie_production_company_movie_key foreign key (movie_key) references movie (key),
	constraint fk_movie_production_company_production_company_key foreign key (production_company_key) references production_company (key)
);

create table movie_writer
(
	movie_key uuid not null,
	writer_key uuid not null,
	constraint pk_movie_writer primary key (movie_key, writer_key),
	constraint fk_movie_writer_movie_key foreign key (movie_key) references movie (key),
	constraint fk_movie_writer_writer_key foreign key (writer_key) references writer (key)
);

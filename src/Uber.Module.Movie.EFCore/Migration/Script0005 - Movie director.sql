create table director
(
	key uuid not null,
	full_name text not null,
	constraint pk_director primary key (key)
);

create table movie_director
(
	movie_key uuid not null,
	director_key uuid not null,
	constraint pk_movie_director primary key (movie_key, director_key)
);

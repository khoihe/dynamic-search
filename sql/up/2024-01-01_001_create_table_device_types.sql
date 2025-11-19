create table if not exists device_types
(
	id varchar(255) not null,
	name varchar(255) not null,
	created_utc timestamp without time zone not null default now(),
    updated_utc timestamp without time zone not null default now(),
	deleted boolean not null default false,
    constraint pk_device_types primary key(id)
);
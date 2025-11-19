create table if not exists devices
(
    id varchar(255) not null,
    name varchar(255) not null,
    type_id varchar(255) NOT NULL,
	created_utc timestamp without time zone not null default now(),
    updated_utc timestamp without time zone not null default now(),
	deleted boolean not null default false,
    constraint pk_devices primary key(id),
    constraint fk_devices_type_id foreign key(type_id) references device_types(id)
);
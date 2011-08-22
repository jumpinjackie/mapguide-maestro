/*----------------------------------------------------------------------*/
/* Name                                                                 */
/*  fdo_sys.sql - create standard system tables schema                    */
/*                                                                      */
/* Description                                                          */
/*      Command file to create standard schema and system tables for    */
/*      FDOSYS database.												*/
/*                                                                      */
/*----------------------------------------------------------------------*/


create table f_lt(
#ifdef Oracle
	ltid				number not null,
	ltname				varchar(30) not null,
	description			varchar(255),
	createdate			date,
	username			varchar(30) not null,
	lockid				number,
	locktype			varchar(1),
	constraint f_lt_pk PRIMARY KEY (ltid, ltname))
	/* tablespace info */
#else
#ifdef MySQL
	ltid				bigint not null auto_increment,
#else
	ltid				bigint not null identity(0,1),
#endif
	ltname				varchar(30) not null,
	description			varchar(255) null,
	createdate			datetime null,
#ifdef MySQL			
	username			varchar(16) not null,
#else
	username			varchar(128) not null,
#endif
	lockid				int	null,
	locktype			varchar(1) null,
	constraint f_lt_pk PRIMARY KEY (ltid, ltname))
#ifdef MySQL
	AUTO_INCREMENT = 0
	engine=InnoDb
#endif
#endif
;
/* index on lock columns */
create index f_lt_lkindx on f_lt(lockid, locktype);
create unique index f_lt_lkname on f_lt(ltname);

/* insert version 0 record */

/* !----------------------------------------------------------------------------------! */
/* ! Note that in a MySQL environment the insert statement to enter the default ver-  ! */
/* ! sion assigns the id 1 rather than 0 to the long transaction. This is caused by   ! */
/* ! the fact that the column LTID is set to be auto-incremented. As a result of this ! */
/* ! it is necessary to execute an update-statement right after the insert statement  ! */
/* ! to correct this.                                                                 ! */
/* ! A side-effect of this is that the next version being created is assigned the id  ! */
/* ! 2 rather than 1. However, missing version 1 is not a problem.                    ! */
/* !----------------------------------------------------------------------------------! */

#ifdef Oracle
insert into f_lt(ltid, ltname, description, createdate, username) values (0, 'Default_0', 'Default permanent version', sysdate, user);
#else
#ifdef MySQL
insert into f_lt(ltname, description, createdate, username) values ('Default_0', 'Default permanent version', current_date(), substring_index(current_user(), _utf8'@', 1));
update f_lt set ltid = 0 where ltid = 1;
#else
insert into f_lt (ltname, description, createdate, username) values ('Default_0', 'Default permanent version', GETDATE(), SYSTEM_USER);
#endif
#endif
 
/* f_lock table */
create table f_lock (
#ifdef Oracle
	lockid				number not null,
	username			varchar(30) not null,
	description			varchar(255),
	createdate			date,
	constraint f_lock_pk primary key (lockid, username)
	/* tablespace info */
#else
#ifdef MySQL
	lockid				bigint not null auto_increment,
	username			varchar(16) not null,
#else
	lockid				bigint not null identity(2,1),
	username			varchar(128) not null,
#endif
	description			varchar(255) null,
	createdate			datetime	null,
	constraint f_lock_pk primary key (lockid, username))
#ifdef MySQL
	auto_increment = 2
	engine=InnoDb
#endif
#endif
;

/* index on username */
create index f_lock_unameindx on f_lock(username);

/* f_lockid_in_table */
create table f_lockid_in_table (
#ifdef Oracle
	lockid				number not null,
	tablename			varchar(30) not null,
	datastorename		varchar(30) not null,
	constraint f_lockid_in_table_pk primary key (lockid, tablename, datastorename)
	/* tablespace info */
#else
#ifdef MySQL
	lockid				bigint not null,
	tablename			varchar(64) not null,
	datastorename		varchar(64) not null,
	constraint f_lockid_in_table_pk primary key (lockid, tablename, datastorename))
	engine=InnoDb
#else
	lockid				bigint not null,
	tablename			varchar(128) not null,
	datastorename		varchar(128) not null,
	constraint f_lockid_in_table_pk primary key (lockid, tablename, datastorename))
#endif
#endif
;

/* f_ltid_in_table table */
create table f_ltid_in_table (
#ifdef Oracle
	ltid				number not null,
	tablename			varchar(30) not null,
	datastorename		varchar(30) not null,
	constraint f_ltid_in_table_pk primary key (ltid, tablename, datastorename))
	/* tablespace info */
#else
#ifdef MySQL
	ltid				bigint not null,
	tablename			varchar(64) not null,
	datastorename		varchar(64) not null,
	constraint f_ltid_in_table_pk primary key (ltid, tablename, datastorename))
	engine=InnoDb
#else
	ltid				bigint not null,
	tablename			varchar(128) not null,
	datastorename		varchar(128) not null,
	constraint f_ltid_in_table_pk primary key (ltid, tablename, datastorename))
#endif
#endif
;

/* f_tldependency table */
create table f_ltdependency	(
#ifdef Oracle
	parentltid			number not null,
	childltid			number not null,
	constraint f_ltdependency_pk	primary key (parentltid, childltid))
	/* tablespace info */
#else
	parentltid			bigint not null,
	childltid			bigint not null,
	constraint f_ltdependency_pk	primary key (parentltid, childltid))
#ifdef MySQL
	engine=InnoDb
#endif
#endif
;

/* f_activelt table */
create table f_activelt	(
#ifdef Oracle
	username			varchar(30) not null,
	activeltid			number not null,
	sessionid			number	not null,
	activatedate		date,
	constraint	f_activelt_pk primary key (username, sessionid))
	/* tablespace info */
#else
#ifdef MySQL
	username			varchar(64) not null,
	activeltid			bigint not null,
	sessionid			bigint	not null,
	activatedate		date,
	constraint	f_activelt_pk primary key (username, sessionid))
	engine=InnoDb
#else
	username			varchar(128) not null,
	activeltid			bigint not null,
	sessionid			bigint	not null,
	activatedate		datetime,
	constraint	f_activelt_pk primary key (username, sessionid))
#endif
#endif
;
	



    
    





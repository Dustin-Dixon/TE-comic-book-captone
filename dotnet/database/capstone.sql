USE master
GO

--drop database if it exists
IF DB_ID('final_capstone') IS NOT NULL
BEGIN
	ALTER DATABASE final_capstone SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE final_capstone;
END

CREATE DATABASE final_capstone
GO

USE final_capstone
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL
	CONSTRAINT PK_user PRIMARY KEY (user_id)
);

CREATE TABLE collections (
	collection_id int IDENTITY(1,1) NOT NULL,
	user_id int NOT NULL,
	name varchar(100) NOT NULL
	CONSTRAINT PK_collections PRIMARY KEY (collection_id)
	CONSTRAINT FK_collections_user_id FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE comics (
	comic_id int IDENTITY(1,1) NOT NULL,
	name varchar(100) NOT NULL,
	author varchar (100) NOT NULL,
	release_date date NOT NULL
	CONSTRAINT PK_comics PRIMARY KEY (comic_id)
);

CREATE TABLE collections_comics (
	collection_id int NOT NULL,
	comic_id int NOT NULL,
	quantity int NOT NULL DEFAULT 1
	CONSTRAINT PK_collections_comics PRIMARY KEY (collection_id, comic_id)
	CONSTRAINT FK_collections_id FOREIGN KEY (collection_id) REFERENCES collections (collection_id),
	CONSTRAINT FK_comics_id FOREIGN KEY (comic_id) REFERENCES comics (comic_id)
);
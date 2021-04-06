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

--------Users Row--------------------------------------

SET IDENTITY_INSERT users ON;

INSERT INTO users (user_id, username, password_hash, salt, user_role) VALUES (1, 'testuser', 'zZwpBoyj0/pvZkhSD35Gwm5e6yE=', 'tXjvMz9Q5eA=', 'standard');

SET IDENTITY_INSERT users OFF;

--------Collections Rows-------------------------------

SET IDENTITY_INSERT collections ON;

INSERT INTO collections (collection_id, user_id, name) VALUES (1, 1, 'TestCollection');
INSERT INTO collections (collection_id, user_id, name) VALUES (2, 1, 'TestMarvel');

SET IDENTITY_INSERT collections OFF;

--------Comics Rows------------------------------------

SET IDENTITY_INSERT comics ON;

INSERT INTO comics (comic_id, name, author, release_date) VALUES (1, 'Spiderman', 'Stan Lee', '1962-06-05');
INSERT INTO comics (comic_id, name, author, release_date) VALUES (2, 'TestHero', 'Some Body', '2021-04-06');
SET IDENTITY_INSERT comics OFF;

--------Collections_comics Rows------------------------

INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (1, 1, 2);
INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (1, 2, 1);
INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (2, 2, 1);



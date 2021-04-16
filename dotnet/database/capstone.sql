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
	name varchar(100) NOT NULL,
	is_public bit NOT NULL
	CONSTRAINT PK_collections PRIMARY KEY (collection_id)
	CONSTRAINT FK_collections_user_id FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE volumes (
	volume_id int NOT NULL,
	name varchar(100) NULL,
	publisher varchar(100) NULL,
	api_detail_url varchar(100) NOT NULL,
	site_detail_url varchar(100) NOT NULL
	CONSTRAINT PK_volumes PRIMARY KEY (volume_id)
);

CREATE TABLE volume_images (
	volume_id int NOT NULL,
	icon_url varchar (255) NOT NULL,
	small_url varchar (255) NOT NULL,
	medium_url varchar (255) NOT NULL,
	thumb_url varchar (255) NOT NULL
	CONSTRAINT PK_volume_images PRIMARY KEY (volume_id)
	CONSTRAINT FK_volume_images FOREIGN KEY (volume_id) REFERENCES volumes (volume_id)
);

CREATE TABLE comics (
	comic_id int NOT NULL,
	name varchar(max) NOT NULL,
	issue_number varchar (20) NOT NULL,
	cover_date date NULL,
	site_detail_url varchar (255) NOT NULL,
	api_detail_url varchar (255) NOT NULL,
	volume_id int NULL,
	CONSTRAINT PK_comics PRIMARY KEY (comic_id),
	CONSTRAINT FK_comics_volumes_volume_id FOREIGN KEY (volume_id) REFERENCES volumes(volume_id)
);

CREATE TABLE comic_images (
	comic_id int NOT NULL,
	icon_url varchar (255) NOT NULL,
	small_url varchar (255) NOT NULL,
	medium_url varchar (255) NOT NULL,
	thumb_url varchar (255) NOT NULL
	CONSTRAINT PK_comic_images PRIMARY KEY (comic_id)
	CONSTRAINT FK_comic_images FOREIGN KEY (comic_id) REFERENCES comics (comic_id)
);

CREATE TABLE collections_comics (
	collection_id int NOT NULL,
	comic_id int NOT NULL,
	quantity int NOT NULL DEFAULT 1
	CONSTRAINT PK_collections_comics PRIMARY KEY (collection_id, comic_id)
	CONSTRAINT FK_collections_comics_collections_id FOREIGN KEY (collection_id) REFERENCES collections (collection_id),
	CONSTRAINT FK_collections_comics_comics_id FOREIGN KEY (comic_id) REFERENCES comics (comic_id)
);

CREATE TABLE comic_creators (
	creator_id int NOT NULL,
	name varchar(100) NOT NULL,
	CONSTRAINT PK_comic_creators PRIMARY KEY (creator_id)
);
	
CREATE TABLE comic_creators_contributions (
	comic_id int NOT NULL,
	creator_id int NOT NULL,
	CONSTRAINT PK_comic_creators_contributions PRIMARY KEY (comic_id,creator_id),
	CONSTRAINT FK_comic_creators_contributions_creators_id FOREIGN KEY (creator_id) REFERENCES comic_creators (creator_id),
	CONSTRAINT FK_comic_creators_contributions_comics_id FOREIGN KEY (comic_id) REFERENCES comics (comic_id)
);

CREATE TABLE characters (
	character_id int NOT NULL,
	name varchar (100) NOT NULL,
	CONSTRAINT PK_characters PRIMARY KEY (character_id)
);

CREATE TABLE comic_characters (
	comic_id int NOT NULL,
	character_id int NOT NULL,
	CONSTRAINT PK_comic_characters PRIMARY KEY (comic_id,character_id),
	CONSTRAINT FK_comic_characters_comic_id FOREIGN KEY (comic_id) REFERENCES comics (comic_id),
	CONSTRAINT FK_comic_characters_character_id FOREIGN KEY (character_id) REFERENCES characters (character_id)
);

CREATE TABLE tags (
	tag_id int IDENTITY(1,1) NOT NULL,
	tag_description varchar (100) NOT NULL,
	CONSTRAINT PK_tags PRIMARY KEY (tag_id)
);

CREATE TABLE comic_tags (
	tag_id int NOT NULL,
	comic_id int NOT NULL,
	CONSTRAINT PK_comic_tags PRIMARY KEY (tag_id,comic_id),
	CONSTRAINT FK_comic_tags_tag_id FOREIGN KEY (tag_id) REFERENCES tags (tag_id),
	CONSTRAINT FK_comic_tags_comic_id FOREIGN KEY (comic_id) REFERENCES comics (comic_id)
);

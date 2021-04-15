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
	name varchar(100) NOT NULL,
	publisher varchar(100) NOT NULL,
	api_detail_url varchar(100) NOT NULL,
	site_detail_url varchar(100) 
	CONSTRAINT PK_volumes PRIMARY KEY (volume_id)
);

CREATE TABLE comics (
	comic_id int NOT NULL,
	name varchar(max) NOT NULL,
	issue_number varchar (5) NOT NULL,
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

--------Users Row--------------------------------------

SET IDENTITY_INSERT users ON;

INSERT INTO users (user_id, username, password_hash, salt, user_role) VALUES (1, 'testuser', 'zZwpBoyj0/pvZkhSD35Gwm5e6yE=', 'tXjvMz9Q5eA=', 'standard');

SET IDENTITY_INSERT users OFF;

--------Collections Rows-------------------------------

SET IDENTITY_INSERT collections ON;

INSERT INTO collections (collection_id, user_id, name, is_public) VALUES (1, 1, 'BatmanCollection', 0);
INSERT INTO collections (collection_id, user_id, name, is_public) VALUES (2, 1, '"Marvel-ous" Collection', 1);

SET IDENTITY_INSERT collections OFF;

--------Comics Rows------------------------------------

INSERT INTO comics (comic_id, name, issue_number, cover_date, site_detail_url, api_detail_url) VALUES (20596, 'Spiderman & Howard the Duck', '96', '1980-08-31', 'https://comicvine.gamespot.com/marvel-team-up-96-spiderman-howard-the-duck/4000-20596/', 'https://comicvine.gamespot.com/api/issue/4000-20596/');
INSERT INTO comics (comic_id, name, issue_number, cover_date, site_detail_url, api_detail_url) VALUES (20736, 'Spiderman & Black Widow', '98', '1980-10-31', 'https://comicvine.gamespot.com/black-diamond-western-39/4000-11/', 'https://comicvine.gamespot.com/api/issue/4000-20736/');
INSERT INTO comics (comic_id, name, issue_number, cover_date, site_detail_url, api_detail_url) VALUES (544648, 'Universo Spiderman', '1', '2015-07-31', 'https://comicvine.gamespot.com/100-marvel-spiderwoman-1-universo-spiderman/4000-544648/', 'https://comicvine.gamespot.com/api/issue/4000-544648/');
INSERT INTO comics (comic_id, name, issue_number, cover_date, site_detail_url, api_detail_url) VALUES (4767, 'The Second Batman and Robin Team', '131', '1960-04-01', 'https://comicvine.gamespot.com/batman-131-the-second-batman-and-robin-team/4000-4767/', 'https://comicvine.gamespot.com/api/issue/4000-4767/');

--------Comic_Images Rows------------------------------

INSERT INTO comic_images (comic_id, icon_url, small_url, medium_url, thumb_url) VALUES (20596, 'https://comicvine1.cbsistatic.com/uploads/square_avatar/11/117763/2432782-marvelteam_up096.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_small/11/117763/2432782-marvelteam_up096.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_medium/11/117763/2432782-marvelteam_up096.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_avatar/11/117763/2432782-marvelteam_up096.jpg'); 
INSERT INTO comic_images (comic_id, icon_url, small_url, medium_url, thumb_url) VALUES (20736, 'https://comicvine1.cbsistatic.com/uploads/square_avatar/11/117763/2432785-marvelteam_up098.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_small/11/117763/2432785-marvelteam_up098.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_medium/11/117763/2432785-marvelteam_up098.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_avatar/11/117763/2432785-marvelteam_up098.jpg');
INSERT INTO comic_images (comic_id, icon_url, small_url, medium_url, thumb_url) VALUES (544648, 'https://comicvine1.cbsistatic.com/uploads/square_avatar/11112/111121983/5382348-8316246341-image_gallery', 'https://comicvine1.cbsistatic.com/uploads/scale_small/11112/111121983/5382348-8316246341-image_gallery', 'https://comicvine1.cbsistatic.com/uploads/scale_medium/11112/111121983/5382348-8316246341-image_gallery', 'https://comicvine1.cbsistatic.com/uploads/scale_avatar/11112/111121983/5382348-8316246341-image_gallery');
INSERT INTO comic_images (comic_id, icon_url, small_url, medium_url, thumb_url) VALUES (4767, 'https://comicvine1.cbsistatic.com/uploads/square_avatar/0/4/4357-796-4767-1-batman.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_small/0/4/4357-796-4767-1-batman.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_medium/0/4/4357-796-4767-1-batman.jpg', 'https://comicvine1.cbsistatic.com/uploads/scale_avatar/0/4/4357-796-4767-1-batman.jpg');

--------Collections_comics Rows------------------------


INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (2, 20596, 2);
INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (2, 20736, 1);
INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (2, 544648, 1);
INSERT INTO collections_comics (collection_id, comic_id, quantity) VALUES (1, 4767, 1);


--------Comic_creator Rows-----------------------------

INSERT INTO comic_creators (creator_id, name) VALUES (1, 'Stan Lee');
INSERT INTO comic_creators (creator_id, name) VALUES (2, 'LeBron James');


--------Comic_creator_contributions Rows---------------

INSERT INTO comic_creators_contributions (comic_id,creator_id) VALUES(20596,1);
INSERT INTO comic_creators_contributions (comic_id,creator_id) VALUES(20736,1);
INSERT INTO comic_creators_contributions (comic_id,creator_id) VALUES(544648,1);
INSERT INTO comic_creators_contributions (comic_id,creator_id) VALUES(4767,2);
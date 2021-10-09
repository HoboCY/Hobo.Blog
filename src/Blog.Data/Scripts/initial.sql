CREATE TABLE app_user(
id BINARY(16) NOT NULL,
username VARCHAR(50) NOT NULL,
email VARCHAR(100) NOT NULL,
email_confirmed TINYINT(1) NOT NULL DEFAULT(0),
password VARCHAR(500) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
last_modify_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
uuid CHAR(36) AS (BIN_TO_UUID(id)),
PRIMARY KEY(id)
);

CREATE TABLE category(
id INT NOT NULL AUTO_INCREMENT,
category_name VARCHAR(100) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
PRIMARY KEY(id)
);

CREATE TABLE post(
id BINARY(16) NOT NULL,
title VARCHAR(100) NOT NULL,
content TEXT(20000) NOT NULL,
content_abstract VARCHAR(300) NOT NULL,
category_ids JSON NOT NULL,
creator_id BINARY(16) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
last_modify_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
isdeleted TINYINT(1) NOT NULL DEFAULT(0),
PRIMARY KEY(id)
);

ALTER TABLE post
ADD INDEX idx_post_categoryIds ((cast((category_ids->"$") as unsigned array)));

CREATE TABLE role(
id INT NOT NULL AUTO_INCREMENT,
role_name VARCHAR(100) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
PRIMARY KEY(id)
)

CREATE TABLE user_role(
id BIGINT NOT NULL AUTO_INCREMENT,
user_id BINARY(16) NOT NULL,
role_id INT(4) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
PRIMARY KEY(id),
CONSTRAINT fk_user_id
FOREIGN KEY(user_id) REFERENCES app_user(id) ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT fk_role_id
FOREIGN KEY(role_id) REFERENCES role(id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE role_permission(
id BIGINT NOT NULL AUTO_INCREMENT,
role_id INT(4) NOT NULL,
permission_name VARCHAR(100) NOT NULL,
creation_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
PRIMARY KEY(id),
CONSTRAINT fk_role_permission_id
FOREIGN KEY(role_id) REFERENCES role(id) ON DELETE CASCADE ON UPDATE CASCADE
);
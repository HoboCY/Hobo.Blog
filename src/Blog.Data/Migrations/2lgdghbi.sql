CREATE TABLE `application_user` (
    `Id` varchar(50) NOT NULL,
    `UserName` varchar(50) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(50) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(50) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(50) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` varchar(100) CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    `NickName` varchar(50) CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `LastModificationTime` datetime(6) NULL,
    `LastLoginTime` datetime(6) NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_application_user` PRIMARY KEY (`Id`)
);

CREATE TABLE `application_role` (
    `Id` varchar(50) NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `CreatorId` varchar(50) NULL,
    `LastModificationTime` datetime(6) NULL,
    `LastModifierId` varchar(50) NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_application_role` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_application_role_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `application_user_claim` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(50) NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_application_user_claim` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_application_user_claim_application_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `application_user_login` (
    `UserId` varchar(50) NOT NULL,
    `LoginProvider` longtext CHARACTER SET utf8mb4 NULL,
    `ProviderKey` longtext CHARACTER SET utf8mb4 NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_application_user_login` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_application_user_login_application_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `application_user_token` (
    `UserId` varchar(50) NOT NULL,
    `LoginProvider` longtext CHARACTER SET utf8mb4 NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_application_user_token` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_application_user_token_application_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `category` (
    `Id` varchar(50) NOT NULL,
    `CategoryName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `NormalizedCategoryName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `CreatorId` varchar(50) NULL,
    `LastModificationTime` datetime(6) NULL,
    `LastModifierId` varchar(50) NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_category` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_category_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `comment` (
    `Id` varchar(50) NOT NULL,
    `PostId` varchar(50) NOT NULL,
    `CommentContent` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` varchar(50) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_comment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_comment_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `post` (
    `Id` varchar(50) NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 NULL,
    `Content` longtext NOT NULL,
    `ContentAbstract` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` varchar(50) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `LastModificationTime` datetime(6) NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_post` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_post_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `application_role_claim` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(50) NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_application_role_claim` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_application_role_claim_application_role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `application_role` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `application_user_role` (
    `UserId` varchar(50) NOT NULL,
    `RoleId` varchar(50) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `CreatorId` varchar(50) NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_application_user_role` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_application_user_role_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_application_user_role_application_role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `application_role` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_application_user_role_application_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `comment_reply` (
    `Id` varchar(50) NOT NULL,
    `CommentId` varchar(50) NOT NULL,
    `ReplyContent` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `CreatorId` varchar(50) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_comment_reply` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_comment_reply_comment_CommentId` FOREIGN KEY (`CommentId`) REFERENCES `comment` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_comment_reply_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `post_category` (
    `CategoryId` varchar(50) NOT NULL,
    `PostId` varchar(50) NOT NULL,
    `CreationTime` datetime(6) NOT NULL,
    `CreatorId` varchar(50) NOT NULL,
    `IsDeleted` tinyint(1) NOT NULL,
    `DeleterId` varchar(50) NULL,
    `DeletionTime` datetime(6) NULL,
    CONSTRAINT `PK_post_category` PRIMARY KEY (`PostId`, `CategoryId`),
    CONSTRAINT `FK_post_category_category_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `category` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_post_category_application_user_CreatorId` FOREIGN KEY (`CreatorId`) REFERENCES `application_user` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_post_category_post_PostId` FOREIGN KEY (`PostId`) REFERENCES `post` (`Id`) ON DELETE CASCADE
);

INSERT INTO `application_role` (`Id`, `ConcurrencyStamp`, `CreationTime`, `CreatorId`, `DeleterId`, `DeletionTime`, `IsDeleted`, `LastModificationTime`, `LastModifierId`, `Name`, `NormalizedName`)
VALUES ('ab7bdc62-1b73-408c-b1a2-428508e1bdd3', '5eb526bc-8b9b-4641-b344-8494f06f35ad', '2020-11-10 13:10:27.95105', NULL, NULL, NULL, FALSE, NULL, NULL, 'administrator', 'ADMINISTRATOR');

CREATE INDEX `IX_application_role_CreatorId` ON `application_role` (`CreatorId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `application_role` (`NormalizedName`);

CREATE INDEX `IX_application_role_claim_RoleId` ON `application_role_claim` (`RoleId`);

CREATE INDEX `EmailIndex` ON `application_user` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `application_user` (`NormalizedUserName`);

CREATE INDEX `IX_application_user_claim_UserId` ON `application_user_claim` (`UserId`);

CREATE INDEX `IX_application_user_role_CreatorId` ON `application_user_role` (`CreatorId`);

CREATE INDEX `IX_application_user_role_RoleId` ON `application_user_role` (`RoleId`);

CREATE INDEX `IX_category_CreatorId` ON `category` (`CreatorId`);

CREATE INDEX `IX_comment_CreatorId` ON `comment` (`CreatorId`);

CREATE INDEX `IX_comment_reply_CommentId` ON `comment_reply` (`CommentId`);

CREATE INDEX `IX_comment_reply_CreatorId` ON `comment_reply` (`CreatorId`);

CREATE INDEX `IX_post_CreatorId` ON `post` (`CreatorId`);

CREATE INDEX `IX_post_category_CategoryId` ON `post_category` (`CategoryId`);

CREATE INDEX `IX_post_category_CreatorId` ON `post_category` (`CreatorId`);

ALTER TABLE `post` MODIFY COLUMN `ContentAbstract` text NOT NULL;

ALTER TABLE `post_category` DROP FOREIGN KEY `FK_post_category_application_user_CreatorId`;

ALTER TABLE `post_category` DROP INDEX `IX_post_category_CreatorId`;

ALTER TABLE `post_category` DROP COLUMN `CreationTime`;

ALTER TABLE `post_category` DROP COLUMN `CreatorId`;

ALTER TABLE `post_category` DROP COLUMN `DeleterId`;

ALTER TABLE `post_category` DROP COLUMN `DeletionTime`;

ALTER TABLE `post_category` DROP COLUMN `IsDeleted`;


USE [master]

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'UserManagement')
BEGIN
	CREATE DATABASE [UserManagement]
END
GO


USE [UserManagement]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User')
BEGIN
	CREATE TABLE [User]
	(
		[UserId] [int] IDENTITY(1,1) NOT NULL,
		[Username] [varchar](50) NOT NULL,
		[PasswordHash] [varchar](100) NOT NULL,
		[CreatedTimeStamp] [datetime] NOT NULL,
		[UpdatedTimeStamp] [datetime] NOT NULL,

		CONSTRAINT [PK_User] PRIMARY KEY ([UserId]),
		CONSTRAINT [UQ_User_Username] UNIQUE ([Username])
	)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserProfile')
BEGIN
	CREATE TABLE [UserProfile]
	(
		[UserProfileId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[FullName] [varchar](200) NOT NULL,
		[ContactNo] [varchar](20) NOT NULL,
		[Email] [varchar](100) NOT NULL,
		[CreatedTimeStamp] [datetime] NOT NULL,
		[UpdatedTimeStamp] [datetime] NOT NULL,

		CONSTRAINT [PK_UserProfile] PRIMARY KEY ([UserProfileId]),
		CONSTRAINT [FK_UserProfile_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]) ON DELETE CASCADE
	)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Post')
BEGIN
	CREATE TABLE [Post]
	(
		[PostId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[PostAbbr] [varchar](20) NOT NULL,
		[PostTitle] [varchar](100) NOT NULL,
		[CreatedTimeStamp] [datetime] NOT NULL,
		[UpdatedTimeStamp] [datetime] NOT NULL,

		CONSTRAINT [PK_Post] PRIMARY KEY ([PostId]),
		CONSTRAINT [FK_Post_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]) ON DELETE CASCADE
	)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tag')
BEGIN
	CREATE TABLE [Tag]
	(
		[TagId] [int] IDENTITY(1,1) NOT NULL,
		[TagName] [varchar](20) NOT NULL,
		[TagDescription] [varchar](100) NOT NULL,
		[CreatedUserId] [int] NOT NULL,
		[CreatedTimeStamp] [datetime] NOT NULL,
		[UpdatedUserId] [int] NOT NULL,
		[UpdatedTimeStamp] [datetime] NOT NULL,

		CONSTRAINT [PK_Tag] PRIMARY KEY ([TagId]),
		CONSTRAINT [UQ_Tag_TagName] UNIQUE ([TagName])
	)
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PostTag')
BEGIN
	CREATE TABLE [PostTag]
	(
		[PostTagId] [int] IDENTITY(1,1) NOT NULL,
		[PostId] [int] NOT NULL,
		[TagId] [int] NOT NULL,
		[CreatedTimeStamp] [datetime] NOT NULL,
		[UpdatedTimeStamp] [datetime] NOT NULL,

		CONSTRAINT [PK_PostTag] PRIMARY KEY ([PostTagId]),
		CONSTRAINT [FK_Post_PostId] FOREIGN KEY ([PostId]) REFERENCES [Post]([PostId]) ON DELETE CASCADE,
		CONSTRAINT [FK_Post_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tag]([TagId]) ON DELETE CASCADE
	)
END
GO

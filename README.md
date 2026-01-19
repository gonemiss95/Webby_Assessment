# User Management API with ASP.NET 8 Web API

This is the code repository for Webby assessment, published by Kar Lun.



## Project overview & architecture notes

Framework: ASP.NET 8
API Layer: REST
Database: MSSQL
Cache: Redis
ORM: Entity Framework Core
Pattern: CQRS
Validation: FluentValidation
Authentication: JWT



### Prerequisites (SDK version, tooling)

Before running the project, need to install following:
.NET 8 SDK from https://dotnet.microsoft.com/en-us/download/dotnet/8.0
IDE such as Visual Studio 2022 or Visual Studio Code
WSL to install Ubuntu in Windows to start Redis server



## Setup & run instructions

Clone repo with https://github.com/gonemiss95/Webby_Assessment.git
Change the ConnectionStrings -> DefaultConnection setting in appsettings.json to connect to local DB
Change the ConnectionStrings -> Redis setting in appsettings.json to connect to Redis (default port is 6379)



## How to run migrations

Run the Execute SQL.bat in DB folder to create database and all tables needed



## Example REST endpoints with request/response samples

Request URL

GET api/post/getpostlist?pageNo=1&pageSize=5 HTTP/1.1


Response body

{
	"totalRecord": 2,
	"result":
	[
		{
			"postAbbr": "sdfvfvf",
			"postTitle": "asdasdass",
			"tagList":
			[
				{
					"tagName": "tag_001",
					"tagDesctiption": "Tag 001"
				}
			]
		},
		{
			"postAbbr": "fdeeeeeer",
			"postTitle": "ssdd fff  ff",
			"tagList":
			[
				{
					"tagName": "tag_002",
					"tagDesctiption": "Tag 002"
				},
				{
					"tagName": "tag_003",
					"tagDesctiption": "Tag 003"
				}
			]
		}
	]
}



## Error Handling Strategy

A GlobalExceptionHandler is created to capture any unhandled exceptions
The exception reponse follow ProblemDetails class

{
	"title": "Internal Server Error",
	"status": "500",
	"details": "An internal server error occured."
}



## Logging & Monitoring

Serilog is used for logging in this Project
Information/Warning/Error is log to file with Serilog default format, stack trace is also logged to file if exception happened for easy debugging



## API Rate Limiting

Using ASP.NET rate limiting middleware
Rate limiter is applied globally to all controller endpoints with partition by IP address


## Database Indexing


User

Unique key added for Username as username must be Unique


UserProfile

Foreign key added for UserId reference to User.UserId to speed up join with parent table
On cascade delete as UserProfile data can only exist if User data exists


Post

Foreign key added for UserId reference to User.UserId to speed up join with parent table
On cascade delete as UserProfile data can only exist if User data exists


PostTag

Foreign key added for PostId reference to Post.PostId to speed up join with parent table
On cascade delete as PostTag data can only exist if Post data exists

Foreign key added for TagId reference to Tag.TagId to speed up join with parent table
On cascade delete as PostTag data can only exist if Tag data exists



## Caching (Redis)

Set cache for tag and post list (with pagination) which is frequently view by user
Set cache for all tag list in create & update post APIs as it return all tag rows for comparison

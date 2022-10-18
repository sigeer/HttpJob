if not EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[db_replacementrule]') AND type IN ('U'))
CREATE TABLE db_replacementrule (
  Id int NOT NULL IDENTITY(1,1),
  ReplacementOldStr nvarchar(100) DEFAULT NULL,
  ReplacementNewlyStr nvarchar(100) DEFAULT NULL,
  TemplateId int NOT NULL,
  PRIMARY KEY (Id)
);

if not EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[db_spider]') AND type IN ('U'))
CREATE TABLE  db_spider (
  Id int NOT NULL IDENTITY(1,1),
  Name nvarchar(100) DEFAULT NULL,
  Description nvarchar(500) DEFAULT NULL,
  Method nvarchar(10) DEFAULT NULL,
  PostObjStr text ,
  Headers text ,
  NextPageTemplateId int DEFAULT NULL,
  CreateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  LastUpdatedTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (Id)
);

if not EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[db_spidertemplate]') AND type IN ('U'))
CREATE TABLE db_spidertemplate (
  Id int NOT NULL IDENTITY(1,1),
  SpiderId int NOT NULL,
  TemplateId int NOT NULL,
  PRIMARY KEY (Id)
);

if not EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[db_template]') AND type IN ('U'))
CREATE TABLE db_template (
  Id int NOT NULL IDENTITY(1,1),
  Name nvarchar(100) DEFAULT NULL,
  Type int NOT NULL,
  TemplateStr nvarchar(100) DEFAULT NULL,
  CreateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  LastUpdatedTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  LinkedSpiderId int DEFAULT NULL,
  PRIMARY KEY (Id)
);

if not EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[db_task]') AND type IN ('U'))
CREATE TABLE db_task( 
  Id INT NOT NULL IDENTITY(1,1), 
  Description nvarchar(100) DEFAULT NULL, 
  RootUrl nvarchar(500) NOT NULL, 
  SpiderId INT NOT NULL, 
  Status INT NOT NULL, 
  CreateTime DATETIME NOT NULL, 
  CompleteTime DATETIME, 
  CronExpression CHAR(50), 
  PRIMARY KEY (Id) 
); 
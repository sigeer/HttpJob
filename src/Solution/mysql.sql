CREATE TABLE If Not Exists `db_replacementrule` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReplacementOldStr` varchar(100) DEFAULT NULL,
  `ReplacementNewlyStr` varchar(100) DEFAULT NULL,
  `TemplateId` int NOT NULL,
  `IgnoreCase` bool not NULL default false
  PRIMARY KEY (`Id`)
);

CREATE TABLE If Not Exists  `db_spider` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Method` varchar(10) DEFAULT NULL,
  `PostObjStr` longtext ,
  `Headers` longtext ,
  `NextPageTemplateId` int DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LastUpdatedTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id`)
);

CREATE TABLE If Not Exists `db_spidertemplate` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SpiderId` int NOT NULL,
  `TemplateId` int NOT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE If Not Exists `db_template` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Type` int NOT NULL,
  `TemplateStr` varchar(100) DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LastUpdatedTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LinkedSpiderId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

CREATE TABLE If Not Exists `db_task`( 
  `Id` INT NOT NULL AUTO_INCREMENT, 
  `Description` VARCHAR(100) DEFAULT NULL, 
  `RootUrl` VARCHAR(500) NOT NULL, 
  `SpiderId` INT NOT NULL, 
  `Status` INT NOT NULL, 
  `CreateTime` DATETIME NOT NULL, 
  `CompleteTime` DATETIME, 
  `CronExpression` CHAR(50), 
  PRIMARY KEY (`Id`) 
); 
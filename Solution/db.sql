DROP TABLE IF EXISTS `db_replacementrule`;

CREATE TABLE `db_replacementrule` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ReplacementOldStr` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ReplacementNewlyStr` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TemplateId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

/*Table structure for table `db_resourcehistory` */

DROP TABLE IF EXISTS `db_resourcehistory`;

CREATE TABLE `db_resourcehistory` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL,
  `LastUpdatedTime` datetime(6) NOT NULL,
  `SpiderId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

/*Table structure for table `db_spider` */

DROP TABLE IF EXISTS `db_spider`;

CREATE TABLE `db_spider` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Method` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PostObjStr` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Headers` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `NextPageTemplateId` int DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LastUpdatedTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id`)
);

/*Table structure for table `db_spidertemplate` */

DROP TABLE IF EXISTS `db_spidertemplate`;

CREATE TABLE `db_spidertemplate` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SpiderId` int NOT NULL,
  `TemplateId` int NOT NULL,
  PRIMARY KEY (`Id`)
);

/*Table structure for table `db_template` */

DROP TABLE IF EXISTS `db_template`;

CREATE TABLE `db_template` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Type` int NOT NULL,
  `TemplateStr` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LastUpdatedTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `LinkedSpiderId` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
);
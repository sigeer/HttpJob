-- ----------------------------
-- Table structure for DB_ReplacementRule
-- ----------------------------
CREATE TABLE If Not Exists "DB_ReplacementRule" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "TemplateId" integer NOT NULL,
  "ReplacementOldStr" varchar(100),
  "ReplacementNewlyStr" varchar(100)
);

-- ----------------------------
-- Table structure for DB_Spider
-- ----------------------------
CREATE TABLE If Not Exists "DB_Spider" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "Name" varchar(100),
  "Description" varchar(500),
  "Method" varchar(10),
  "PostObjStr" text,
  "Headers" text,
  "NextPageTemplateId" integer,
  "CreateTime" datetime NOT NULL,
  "LastUpdatedTime" datetime NOT NULL
);

-- ----------------------------
-- Table structure for DB_SpiderTemplate
-- ----------------------------
CREATE TABLE If Not Exists "DB_SpiderTemplate" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "SpiderId" integer NOT NULL,
  "TemplateId" integer NOT NULL
);

-- ----------------------------
-- Table structure for DB_Task
-- ----------------------------
CREATE TABLE If Not Exists "DB_Task" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "Description" varchar(100),
  "RootUrl" varchar(500) NOT NULL,
  "SpiderId" integer NOT NULL,
  "Status" integer NOT NULL,
  "CreateTime" datetime NOT NULL,
  "CompleteTime" datetime,
  "CronExpression" varchar(50)
);

-- ----------------------------
-- Table structure for DB_Template
-- ----------------------------
CREATE TABLE If Not Exists "DB_Template" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "Name" varchar(255),
  "Type" integer NOT NULL,
  "TemplateStr" varchar(255),
  "CreateTime" datetime NOT NULL,
  "LastUpdatedTime" datetime NOT NULL,
  "LinkedSpiderId" integer
);

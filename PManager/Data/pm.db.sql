BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "APP" (
	"idApp"	INTEGER,
	"name"	TEXT UNIQUE,
	PRIMARY KEY("idApp" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "UserApp" (
	"idUser"	INTEGER NOT NULL,
	"idApp"	INTEGER NOT NULL,
	"userAppName"	TEXT NOT NULL,
	"userAppPassword"	TEXT NOT NULL,
	FOREIGN KEY("idUser") REFERENCES "User"("id"),
	FOREIGN KEY("idApp") REFERENCES "App"("id")
);
CREATE TABLE IF NOT EXISTS "TYPE" (
	"idType"	INTEGER,
	"nombre"	TEXT,
	PRIMARY KEY("idType")
);
CREATE TABLE IF NOT EXISTS "UserType" (
	"idUser"	INTEGER,
	"idType"	INTEGER,
	PRIMARY KEY("idUser","idType")
);
CREATE TABLE IF NOT EXISTS "User" (
	"idUser"	INTEGER,
	"name"	VARCHAR(255),
	"email"	VARCHAR(255),
	"password"	VARCHAR(255),
	"profile_picture"	BLOB,
	"PrivateKey"	TEXT,
	PRIMARY KEY("idUser" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "PublicKey" (
	"PublicKey"	TEXT,
	PRIMARY KEY("PublicKey")
);
COMMIT;

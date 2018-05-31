create table "Users"(
	"Id" SERIAL PRIMARY KEY,
	"Username" VARCHAR(20) NOT NULL,
	"Email" VARCHAR(30) NOT NULL,
	"Password" VARCHAR(50) NOT NULL,
	"LastLevel" INTEGER,
	"AuthKey" VARCHAR(50)
);

create table "Leaderboard"(
	"UserId" integer not null,
	"LevelNumber" integer not null,
	"Score" VARCHAR(15) not null,
	
	PRIMARY KEY( "UserId", "LevelNumber" ),
	FOREIGN KEY ("UserId") REFERENCES "Users"("Id")
);

SELECT "Username", "Score" FROM "Users" 
INNER JOIN "Leaderboard" ON "Users"."Id" = "Leaderboard"."UserId" AND "Leaderboard"."LevelNumber" = 2
ORDER BY "Score" DESC;
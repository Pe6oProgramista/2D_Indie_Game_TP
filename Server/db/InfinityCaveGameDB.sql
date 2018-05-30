create table "Users"(
	"Id" SERIAL PRIMARY KEY,
	"Username" VARCHAR(20) NOT NULL,
	"Password" VARCHAR(40) NOT NULL,
	"LastLevel" INTEGER,
	"AuthKey" VARCHAR(40)
);

create table "Leaderboard"(
	"UserId" integer not null,
	"LevelNumber" integer not null,
	"Score" VARCHAR(15) not null,
	
	PRIMARY KEY( "UserId", "LevelNumber" ),
	FOREIGN KEY ("UserId") REFERENCES "Users"("Id")
);
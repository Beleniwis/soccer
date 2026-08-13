CREATE TABLE TeamLeagues
(
    TeamId INT NOT NULL,

    LeagueId INT NOT NULL,

    CONSTRAINT PK_TeamLeagues
        PRIMARY KEY (TeamId, LeagueId),

    CONSTRAINT FK_TeamLeagues_Teams
        FOREIGN KEY (TeamId)
        REFERENCES Teams(Id),

    CONSTRAINT FK_TeamLeagues_Leagues
        FOREIGN KEY (LeagueId)
        REFERENCES Leagues(Id)
);
GO
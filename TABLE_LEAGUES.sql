CREATE TABLE Leagues
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(100) NOT NULL,

    CountryId INT NOT NULL,

    StartDate DATETIME2 NOT NULL
        CONSTRAINT DF_Leagues_StartDate DEFAULT GETDATE(),

    EndDate DATETIME2 NOT NULL
        CONSTRAINT DF_Leagues_EndDate DEFAULT GETDATE(),

    Enabled BIT NOT NULL
        CONSTRAINT DF_Leagues_Enabled DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Leagues_CreatedAt DEFAULT GETDATE(),

    CONSTRAINT FK_Leagues_Countries
        FOREIGN KEY (CountryId)
        REFERENCES Countries(Id),

    CONSTRAINT CK_Leagues_Name
        CHECK
        (
            LEN(LTRIM(RTRIM(Name))) >= 2
            AND Name NOT LIKE '%[^A-Za-zÁÉÍÓÚáéíóúÑñ ]%'
        ),

    CONSTRAINT CK_Leagues_Dates
        CHECK (StartDate <= EndDate)
);
GO
CREATE TABLE Teams
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(100) NOT NULL,

    CountryId INT NOT NULL,

    Enabled BIT NOT NULL
        CONSTRAINT DF_Teams_Enabled DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Teams_CreatedAt DEFAULT GETDATE(),

    CONSTRAINT FK_Teams_Countries
        FOREIGN KEY (CountryId)
        REFERENCES Countries(Id),

    CONSTRAINT CK_Teams_Name
        CHECK
        (
            LEN(LTRIM(RTRIM(Name))) >= 2
            AND Name NOT LIKE '%[^A-Za-zÁÉÍÓÚáéíóúÑñ ]%'
        )
);
GO
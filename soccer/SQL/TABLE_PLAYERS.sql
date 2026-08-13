CREATE TABLE Players
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(150) NOT NULL,

    TeamId INT NOT NULL,

    Enabled BIT NOT NULL
        CONSTRAINT DF_Players_Enabled DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Players_CreatedAt DEFAULT GETDATE(),

    CONSTRAINT FK_Players_Teams
        FOREIGN KEY (TeamId)
        REFERENCES Teams(Id),

    CONSTRAINT CK_Players_Name
        CHECK
        (
            LEN(LTRIM(RTRIM(Name))) >= 2
            AND Name NOT LIKE '%[^A-Za-zÁÉÍÓÚáéíóúÑñ ]%'
        )
);
GO
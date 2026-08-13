CREATE TABLE Countries
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(100) NOT NULL,

    Enabled BIT NOT NULL
        CONSTRAINT DF_Countries_Enabled DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Countries_CreatedAt DEFAULT GETDATE(),

    CONSTRAINT CK_Countries_Name
        CHECK
        (
            LEN(LTRIM(RTRIM(Name))) >= 2
            AND Name NOT LIKE '%[^A-Za-zÁÉÍÓÚáéíóúÑñ ]%'
        )
);
GO
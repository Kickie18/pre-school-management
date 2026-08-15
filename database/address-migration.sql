-- Migrates existing PreschoolManagementDb data to the normalized address model.
-- Run against the application database before starting the API with the new model.
-- Existing parent addresses remain in Parents.Address because this change covers
-- schools, teachers, and students.

IF OBJECT_ID(N'dbo.Addresses', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Addresses
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Addresses PRIMARY KEY,
        AddressLine1 NVARCHAR(200) NOT NULL,
        AddressLine2 NVARCHAR(200) NULL,
        City NVARCHAR(100) NOT NULL,
        State NVARCHAR(100) NOT NULL,
        PostalCode NVARCHAR(20) NOT NULL,
        Country NVARCHAR(100) NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Addresses_IsDeleted DEFAULT 0,
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL CONSTRAINT DF_Addresses_CreatedDate DEFAULT SYSUTCDATETIME(),
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL
    );
END;

IF COL_LENGTH(N'dbo.Schools', N'AddressId') IS NULL
    ALTER TABLE dbo.Schools ADD AddressId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH(N'dbo.Teachers', N'AddressId') IS NULL
    ALTER TABLE dbo.Teachers ADD AddressId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH(N'dbo.Students', N'AddressId') IS NULL
    ALTER TABLE dbo.Students ADD AddressId UNIQUEIDENTIFIER NULL;

-- Convert legacy school address text into address rows.
INSERT INTO dbo.Addresses (Id, AddressLine1, City, State, PostalCode, Country, CreatedBy, CreatedDate)
SELECT NEWID(), NULLIF(LTRIM(RTRIM(s.Address)), N''), N'Unknown', N'Unknown', N'Unknown', N'Unknown', N'database-migration', SYSUTCDATETIME()
FROM dbo.Schools s
WHERE s.AddressId IS NULL
  AND COL_LENGTH(N'dbo.Schools', N'Address') IS NOT NULL
  AND NULLIF(LTRIM(RTRIM(s.Address)), N'') IS NOT NULL;

UPDATE s
SET AddressId = a.Id
FROM dbo.Schools s
INNER JOIN dbo.Addresses a ON a.AddressLine1 = NULLIF(LTRIM(RTRIM(s.Address)), N'')
WHERE s.AddressId IS NULL;

-- Give legacy schools with no usable address text a valid placeholder before
-- the new foreign key is made mandatory.
INSERT INTO dbo.Addresses (Id, AddressLine1, City, State, PostalCode, Country, CreatedBy, CreatedDate)
SELECT NEWID(), N'Address pending update', N'Unknown', N'Unknown', N'Unknown', N'Unknown', N'database-migration', SYSUTCDATETIME()
FROM dbo.Schools s
WHERE s.AddressId IS NULL;

UPDATE s
SET AddressId = a.Id
FROM dbo.Schools s
CROSS APPLY (SELECT TOP (1) Id FROM dbo.Addresses WHERE AddressLine1 = N'Address pending update' ORDER BY CreatedDate) a
WHERE s.AddressId IS NULL;

-- Convert legacy student address text into address rows.
INSERT INTO dbo.Addresses (Id, AddressLine1, City, State, PostalCode, Country, CreatedBy, CreatedDate)
SELECT NEWID(), NULLIF(LTRIM(RTRIM(s.Address)), N''), N'Unknown', N'Unknown', N'Unknown', N'Unknown', N'database-migration', SYSUTCDATETIME()
FROM dbo.Students s
WHERE s.AddressId IS NULL
  AND COL_LENGTH(N'dbo.Students', N'Address') IS NOT NULL
  AND NULLIF(LTRIM(RTRIM(s.Address)), N'') IS NOT NULL;

UPDATE s
SET AddressId = a.Id
FROM dbo.Students s
INNER JOIN dbo.Addresses a ON a.AddressLine1 = NULLIF(LTRIM(RTRIM(s.Address)), N'')
WHERE s.AddressId IS NULL;

-- Give legacy students with no usable address text a valid placeholder.
INSERT INTO dbo.Addresses (Id, AddressLine1, City, State, PostalCode, Country, CreatedBy, CreatedDate)
SELECT NEWID(), N'Address pending update', N'Unknown', N'Unknown', N'Unknown', N'Unknown', N'database-migration', SYSUTCDATETIME()
FROM dbo.Students s
WHERE s.AddressId IS NULL;

UPDATE s
SET AddressId = a.Id
FROM dbo.Students s
CROSS APPLY (SELECT TOP (1) Id FROM dbo.Addresses WHERE AddressLine1 = N'Address pending update' ORDER BY CreatedDate) a
WHERE s.AddressId IS NULL;

-- Existing teachers had no address field. Give them an explicit required value.
INSERT INTO dbo.Addresses (Id, AddressLine1, City, State, PostalCode, Country, CreatedBy, CreatedDate)
SELECT NEWID(), N'Address pending update', N'Unknown', N'Unknown', N'Unknown', N'Unknown', N'database-migration', SYSUTCDATETIME()
FROM dbo.Teachers t
WHERE t.AddressId IS NULL;

UPDATE t
SET AddressId = a.Id
FROM dbo.Teachers t
INNER JOIN dbo.Addresses a ON a.AddressLine1 = N'Address pending update'
WHERE t.AddressId IS NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Schools_Addresses')
    ALTER TABLE dbo.Schools ADD CONSTRAINT FK_Schools_Addresses FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Teachers_Addresses')
    ALTER TABLE dbo.Teachers ADD CONSTRAINT FK_Teachers_Addresses FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_Addresses')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_Addresses FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id);

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Schools_AddressId' AND object_id = OBJECT_ID(N'dbo.Schools'))
    DROP INDEX IX_Schools_AddressId ON dbo.Schools;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_AddressId' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    DROP INDEX IX_Teachers_AddressId ON dbo.Teachers;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_AddressId' AND object_id = OBJECT_ID(N'dbo.Students'))
    DROP INDEX IX_Students_AddressId ON dbo.Students;

-- Remove the old text columns after validating the backfill.
IF COL_LENGTH(N'dbo.Schools', N'Address') IS NOT NULL
    ALTER TABLE dbo.Schools DROP COLUMN Address;

IF COL_LENGTH(N'dbo.Students', N'Address') IS NOT NULL
    ALTER TABLE dbo.Students DROP COLUMN Address;

ALTER TABLE dbo.Schools ALTER COLUMN AddressId UNIQUEIDENTIFIER NOT NULL;
ALTER TABLE dbo.Teachers ALTER COLUMN AddressId UNIQUEIDENTIFIER NOT NULL;
ALTER TABLE dbo.Students ALTER COLUMN AddressId UNIQUEIDENTIFIER NOT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Schools_AddressId' AND object_id = OBJECT_ID(N'dbo.Schools'))
    CREATE INDEX IX_Schools_AddressId ON dbo.Schools(AddressId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_AddressId' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE INDEX IX_Teachers_AddressId ON dbo.Teachers(AddressId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_AddressId' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE INDEX IX_Students_AddressId ON dbo.Students(AddressId);

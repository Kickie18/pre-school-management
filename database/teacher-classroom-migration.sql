-- Adds teacher login ownership and classroom school ownership.
-- Run after address-migration.sql and before starting the updated API.
-- Existing teachers must have matching Teacher-role users by email. Create any
-- missing users through POST /api/Users first, then rerun this script.

IF COL_LENGTH(N'dbo.Teachers', N'UserId') IS NULL
    ALTER TABLE dbo.Teachers ADD UserId UNIQUEIDENTIFIER NULL;
GO

IF COL_LENGTH(N'dbo.ClassRooms', N'SchoolId') IS NULL
    ALTER TABLE dbo.ClassRooms ADD SchoolId UNIQUEIDENTIFIER NULL;
GO

-- Automatically link existing teachers when their email matches a user.
UPDATE t
SET UserId = u.Id
FROM dbo.Teachers t
INNER JOIN dbo.Users u ON u.Email = t.Email
INNER JOIN dbo.Roles r ON r.Id = u.RoleId AND r.RoleName = N'Teacher'
WHERE t.UserId IS NULL;

-- Classrooms can be linked from their assigned teacher when available.
UPDATE c
SET SchoolId = t.SchoolId
FROM dbo.ClassRooms c
INNER JOIN dbo.Teachers t ON t.Id = c.TeacherId
WHERE c.SchoolId IS NULL;

IF EXISTS (SELECT 1 FROM dbo.ClassRooms WHERE SchoolId IS NULL)
    THROW 51002, 'Every existing classroom must be linked to a school before SchoolId can be made mandatory.', 1;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Teachers_Users')
    ALTER TABLE dbo.Teachers ADD CONSTRAINT FK_Teachers_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ClassRooms_Schools')
    ALTER TABLE dbo.ClassRooms ADD CONSTRAINT FK_ClassRooms_Schools FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_UserId' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE UNIQUE INDEX IX_Teachers_UserId ON dbo.Teachers(UserId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ClassRooms_SchoolId' AND object_id = OBJECT_ID(N'dbo.ClassRooms'))
    CREATE INDEX IX_ClassRooms_SchoolId ON dbo.ClassRooms(SchoolId);

ALTER TABLE dbo.Teachers ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
ALTER TABLE dbo.ClassRooms ALTER COLUMN SchoolId UNIQUEIDENTIFIER NOT NULL;

-- UserId remains nullable only for legacy teachers that predate teacher logins.
-- New teacher API requests require UserId and the foreign key still enforces
-- that any supplied value points to an existing user.

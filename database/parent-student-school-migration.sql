-- Adds mandatory school ownership to Parents and Students.
-- Run after teacher-classroom-migration.sql.

IF COL_LENGTH(N'dbo.Parents', N'SchoolId') IS NULL
    ALTER TABLE dbo.Parents ADD SchoolId UNIQUEIDENTIFIER NULL;
GO

IF COL_LENGTH(N'dbo.Students', N'SchoolId') IS NULL
    ALTER TABLE dbo.Students ADD SchoolId UNIQUEIDENTIFIER NULL;
GO

-- Students inherit school ownership from their classroom.
UPDATE s
SET SchoolId = c.SchoolId
FROM dbo.Students s
INNER JOIN dbo.ClassRooms c ON c.Id = s.ClassId
WHERE s.SchoolId IS NULL;

-- Parents inherit school ownership from one of their students.
UPDATE p
SET SchoolId = source.SchoolId
FROM dbo.Parents p
CROSS APPLY
(
    SELECT TOP (1) s.SchoolId
    FROM dbo.Students s
    WHERE s.ParentId = p.Id
      AND s.SchoolId IS NOT NULL
    ORDER BY s.CreatedDate
) source
WHERE p.SchoolId IS NULL;

IF EXISTS (SELECT 1 FROM dbo.Students WHERE SchoolId IS NULL)
    THROW 51020, 'Every existing student must be linked to a classroom with a school.', 1;

IF EXISTS (SELECT 1 FROM dbo.Parents WHERE SchoolId IS NULL)
    THROW 51021, 'Every existing parent must be linked to a school through a student.', 1;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Parents_Schools')
    ALTER TABLE dbo.Parents ADD CONSTRAINT FK_Parents_Schools FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_Schools')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_Schools FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id);

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Parents_SchoolId' AND object_id = OBJECT_ID(N'dbo.Parents'))
    DROP INDEX IX_Parents_SchoolId ON dbo.Parents;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_SchoolId' AND object_id = OBJECT_ID(N'dbo.Students'))
    DROP INDEX IX_Students_SchoolId ON dbo.Students;

ALTER TABLE dbo.Parents ALTER COLUMN SchoolId UNIQUEIDENTIFIER NOT NULL;
ALTER TABLE dbo.Students ALTER COLUMN SchoolId UNIQUEIDENTIFIER NOT NULL;

CREATE INDEX IX_Parents_SchoolId ON dbo.Parents(SchoolId);
CREATE INDEX IX_Students_SchoolId ON dbo.Students(SchoolId);

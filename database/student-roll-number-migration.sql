-- Adds the optional student roll number field.

IF COL_LENGTH(N'dbo.Students', N'RollNumber') IS NULL
    ALTER TABLE dbo.Students ADD RollNumber NVARCHAR(30) NULL;
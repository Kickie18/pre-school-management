-- Preschool Management starter schema outline (SQL Server)
-- This is a baseline script for review/documentation. Prefer EF Core migrations for production.

CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedBy NVARCHAR(100) NULL,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedBy NVARCHAR(100) NULL,
    UpdatedDate DATETIME2 NULL
);

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    ProfilePicture NVARCHAR(500) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    LastLoginDate DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedBy NVARCHAR(100) NULL,
    CreatedDate DATETIME2 NOT NULL,
    UpdatedBy NVARCHAR(100) NULL,
    UpdatedDate DATETIME2 NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Additional tables are modeled in EF Core domain + DbContext:
-- Schools, Teachers, Parents, Students, ClassRooms, Attendances,
-- StudentCheckInOuts, Timetables, FeeStructures, Payments,
-- InventoryItems, Notices, Notifications, RefreshTokens.

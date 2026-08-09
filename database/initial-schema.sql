-- Preschool Management full schema (SQL Server)
-- Creates all entity tables, relations, and key indexes from the Domain + DbContext model.
-- Script is idempotent for table creation.

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        RoleName NVARCHAR(50) NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Roles_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Roles PRIMARY KEY (Id),
        CONSTRAINT UQ_Roles_RoleName UNIQUE (RoleName)
    );
END;


IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        PhoneNumber NVARCHAR(20) NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        RoleId UNIQUEIDENTIFIER NOT NULL,
        ProfilePicture NVARCHAR(500) NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
        LastLoginDate DATETIME2 NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Users_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Users PRIMARY KEY (Id)
    );
END;


IF OBJECT_ID(N'dbo.Schools', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Schools
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        SchoolName NVARCHAR(200) NOT NULL,
        AddressId UNIQUEIDENTIFIER NULL,
        ContactNumber NVARCHAR(20) NULL,
        Email NVARCHAR(256) NULL,
        Logo NVARCHAR(MAX) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Schools_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Schools PRIMARY KEY (Id)
    );
END;


IF OBJECT_ID(N'dbo.Addresses', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Addresses
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        AddressLine1 NVARCHAR(300) NOT NULL,
        AddressLine2 NVARCHAR(300) NULL,
        City NVARCHAR(100) NOT NULL,
        [State] NVARCHAR(100) NOT NULL,
        PostalCode NVARCHAR(20) NOT NULL,
        Country NVARCHAR(100) NOT NULL,
        Latitude DECIMAL(9,6) NULL,
        Longitude DECIMAL(9,6) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Addresses_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Addresses PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.SchoolBranches', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolBranches
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        SchoolId UNIQUEIDENTIFIER NOT NULL,
        AddressId UNIQUEIDENTIFIER NULL,
        BranchName NVARCHAR(200) NOT NULL,
        BranchCode NVARCHAR(50) NOT NULL,
        ContactNumber NVARCHAR(20) NULL,
        Email NVARCHAR(256) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_SchoolBranches_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_SchoolBranches PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.UserSchools', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserSchools
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        SchoolId UNIQUEIDENTIFIER NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_UserSchools_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_UserSchools PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teachers
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        EmployeeCode NVARCHAR(30) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Gender INT NOT NULL,
        DOB DATETIME2 NOT NULL,
        Qualification NVARCHAR(MAX) NOT NULL,
        Experience INT NOT NULL,
        PhoneNumber NVARCHAR(MAX) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        JoiningDate DATETIME2 NOT NULL,
        ProfileImage NVARCHAR(MAX) NULL,
        SchoolId UNIQUEIDENTIFIER NOT NULL,
        BranchId UNIQUEIDENTIFIER NULL,
        AddressId UNIQUEIDENTIFIER NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Teachers_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Teachers PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Parents', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Parents
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        FatherName NVARCHAR(100) NOT NULL,
        MotherName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        PhoneNumber NVARCHAR(20) NOT NULL,
        Address NVARCHAR(300) NOT NULL,
        Occupation NVARCHAR(MAX) NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Parents_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Parents PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.ClassRooms', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ClassRooms
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        ClassName NVARCHAR(100) NOT NULL,
        AgeGroup NVARCHAR(50) NOT NULL,
        Capacity INT NOT NULL,
        TeacherId UNIQUEIDENTIFIER NULL,
        BranchId UNIQUEIDENTIFIER NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_ClassRooms_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_ClassRooms PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        AdmissionNumber NVARCHAR(50) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Gender INT NOT NULL,
        DOB DATETIME2 NOT NULL,
        BloodGroup NVARCHAR(10) NULL,
        AddressId UNIQUEIDENTIFIER NULL,
        JoiningDate DATETIME2 NOT NULL,
        ClassId UNIQUEIDENTIFIER NOT NULL,
        ParentId UNIQUEIDENTIFIER NOT NULL,
        BranchId UNIQUEIDENTIFIER NULL,
        ProfilePicture NVARCHAR(MAX) NULL,
        Status INT NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Students_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Students PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Attendances', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Attendances
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        StudentId UNIQUEIDENTIFIER NOT NULL,
        ClassId UNIQUEIDENTIFIER NOT NULL,
        [Date] DATE NOT NULL,
        Status INT NOT NULL,
        Remarks NVARCHAR(MAX) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Attendances_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Attendances PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.StudentCheckInOuts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentCheckInOuts
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        StudentId UNIQUEIDENTIFIER NOT NULL,
        CheckInTime DATETIME2 NOT NULL,
        CheckOutTime DATETIME2 NULL,
        PickupPerson NVARCHAR(100) NOT NULL,
        PickupRelationship NVARCHAR(100) NOT NULL,
        Notes NVARCHAR(MAX) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_StudentCheckInOuts_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_StudentCheckInOuts PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Timetables', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Timetables
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        ClassId UNIQUEIDENTIFIER NOT NULL,
        DayOfWeek INT NOT NULL,
        StartTime TIME NOT NULL,
        EndTime TIME NOT NULL,
        ActivityName NVARCHAR(200) NOT NULL,
        TeacherId UNIQUEIDENTIFIER NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Timetables_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Timetables PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.FeeStructures', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.FeeStructures
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        ClassId UNIQUEIDENTIFIER NOT NULL,
        FeeType NVARCHAR(100) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        DueDate DATE NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_FeeStructures_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_FeeStructures PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Payments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Payments
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        StudentId UNIQUEIDENTIFIER NOT NULL,
        FeeStructureId UNIQUEIDENTIFIER NOT NULL,
        AmountPaid DECIMAL(18,2) NOT NULL,
        PaymentDate DATETIME2 NOT NULL,
        PaymentMethod INT NOT NULL,
        TransactionReference NVARCHAR(100) NOT NULL,
        Status INT NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Payments_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Payments PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.InventoryItems', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.InventoryItems
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        ItemName NVARCHAR(200) NOT NULL,
        Category NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        AvailableQuantity INT NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL,
        PurchaseDate DATE NOT NULL,
        SupplierName NVARCHAR(200) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_InventoryItems_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_InventoryItems PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Notices', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notices
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(2000) NOT NULL,
        PublishDate DATETIME2 NOT NULL,
        ExpiryDate DATETIME2 NOT NULL,
        Author NVARCHAR(100) NOT NULL,
        TargetAudience INT NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Notices_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Notices PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Notifications', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notifications
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Message NVARCHAR(2000) NOT NULL,
        NotificationType INT NOT NULL,
        IsRead BIT NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_Notifications_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_Notifications PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RefreshTokens
    (
        Id UNIQUEIDENTIFIER NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        Token NVARCHAR(256) NOT NULL,
        ExpiresAt DATETIME2 NOT NULL,
        IsRevoked BIT NOT NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_RefreshTokens_IsDeleted DEFAULT (0),
        CreatedBy NVARCHAR(100) NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedBy NVARCHAR(100) NULL,
        UpdatedDate DATETIME2 NULL,
        CONSTRAINT PK_RefreshTokens PRIMARY KEY (Id)
    );
END;

IF COL_LENGTH('dbo.Schools', 'AddressId') IS NULL
    ALTER TABLE dbo.Schools ADD AddressId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH('dbo.Teachers', 'BranchId') IS NULL
    ALTER TABLE dbo.Teachers ADD BranchId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH('dbo.Teachers', 'AddressId') IS NULL
    ALTER TABLE dbo.Teachers ADD AddressId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH('dbo.Students', 'BranchId') IS NULL
    ALTER TABLE dbo.Students ADD BranchId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH('dbo.Students', 'AddressId') IS NULL
    ALTER TABLE dbo.Students ADD AddressId UNIQUEIDENTIFIER NULL;

IF COL_LENGTH('dbo.ClassRooms', 'BranchId') IS NULL
    ALTER TABLE dbo.ClassRooms ADD BranchId UNIQUEIDENTIFIER NULL;

GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Roles_RoleName' AND object_id = OBJECT_ID(N'dbo.Roles'))
    CREATE UNIQUE INDEX IX_Roles_RoleName ON dbo.Roles(RoleName);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Users_Email' AND object_id = OBJECT_ID(N'dbo.Users'))
    CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users(Email);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UserSchools_UserId_SchoolId' AND object_id = OBJECT_ID(N'dbo.UserSchools'))
    CREATE UNIQUE INDEX IX_UserSchools_UserId_SchoolId ON dbo.UserSchools(UserId, SchoolId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UserSchools_UserId' AND object_id = OBJECT_ID(N'dbo.UserSchools'))
    CREATE INDEX IX_UserSchools_UserId ON dbo.UserSchools(UserId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_UserSchools_SchoolId' AND object_id = OBJECT_ID(N'dbo.UserSchools'))
    CREATE INDEX IX_UserSchools_SchoolId ON dbo.UserSchools(SchoolId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Schools_AddressId' AND object_id = OBJECT_ID(N'dbo.Schools'))
    CREATE INDEX IX_Schools_AddressId ON dbo.Schools(AddressId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SchoolBranches_BranchCode' AND object_id = OBJECT_ID(N'dbo.SchoolBranches'))
    CREATE UNIQUE INDEX IX_SchoolBranches_BranchCode ON dbo.SchoolBranches(BranchCode);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SchoolBranches_SchoolId_BranchName' AND object_id = OBJECT_ID(N'dbo.SchoolBranches'))
    CREATE UNIQUE INDEX IX_SchoolBranches_SchoolId_BranchName ON dbo.SchoolBranches(SchoolId, BranchName);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_BranchId' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE INDEX IX_Teachers_BranchId ON dbo.Teachers(BranchId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_AddressId' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE INDEX IX_Teachers_AddressId ON dbo.Teachers(AddressId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ClassRooms_BranchId' AND object_id = OBJECT_ID(N'dbo.ClassRooms'))
    CREATE INDEX IX_ClassRooms_BranchId ON dbo.ClassRooms(BranchId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_BranchId' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE INDEX IX_Students_BranchId ON dbo.Students(BranchId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_AddressId' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE INDEX IX_Students_AddressId ON dbo.Students(AddressId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_EmployeeCode' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE UNIQUE INDEX IX_Teachers_EmployeeCode ON dbo.Teachers(EmployeeCode);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Parents_Email' AND object_id = OBJECT_ID(N'dbo.Parents'))
    CREATE UNIQUE INDEX IX_Parents_Email ON dbo.Parents(Email);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Parents_UserId' AND object_id = OBJECT_ID(N'dbo.Parents'))
    CREATE UNIQUE INDEX IX_Parents_UserId ON dbo.Parents(UserId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ClassRooms_ClassName' AND object_id = OBJECT_ID(N'dbo.ClassRooms'))
    CREATE UNIQUE INDEX IX_ClassRooms_ClassName ON dbo.ClassRooms(ClassName);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_AdmissionNumber' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE UNIQUE INDEX IX_Students_AdmissionNumber ON dbo.Students(AdmissionNumber);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Attendances_StudentId_Date' AND object_id = OBJECT_ID(N'dbo.Attendances'))
    CREATE UNIQUE INDEX IX_Attendances_StudentId_Date ON dbo.Attendances(StudentId, [Date]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Payments_TransactionReference' AND object_id = OBJECT_ID(N'dbo.Payments'))
    CREATE UNIQUE INDEX IX_Payments_TransactionReference ON dbo.Payments(TransactionReference);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_InventoryItems_ItemName' AND object_id = OBJECT_ID(N'dbo.InventoryItems'))
    CREATE INDEX IX_InventoryItems_ItemName ON dbo.InventoryItems(ItemName);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RefreshTokens_Token' AND object_id = OBJECT_ID(N'dbo.RefreshTokens'))
    CREATE UNIQUE INDEX IX_RefreshTokens_Token ON dbo.RefreshTokens(Token);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Users_Roles_RoleId')
    ALTER TABLE dbo.Users ADD CONSTRAINT FK_Users_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_UserSchools_Users_UserId')
    ALTER TABLE dbo.UserSchools ADD CONSTRAINT FK_UserSchools_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_UserSchools_Schools_SchoolId')
    ALTER TABLE dbo.UserSchools ADD CONSTRAINT FK_UserSchools_Schools_SchoolId FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Schools_Addresses_AddressId')
    ALTER TABLE dbo.Schools ADD CONSTRAINT FK_Schools_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_SchoolBranches_Schools_SchoolId')
    ALTER TABLE dbo.SchoolBranches ADD CONSTRAINT FK_SchoolBranches_Schools_SchoolId FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_SchoolBranches_Addresses_AddressId')
    ALTER TABLE dbo.SchoolBranches ADD CONSTRAINT FK_SchoolBranches_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Teachers_Schools_SchoolId')
    ALTER TABLE dbo.Teachers ADD CONSTRAINT FK_Teachers_Schools_SchoolId FOREIGN KEY (SchoolId) REFERENCES dbo.Schools(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Teachers_SchoolBranches_BranchId')
    ALTER TABLE dbo.Teachers ADD CONSTRAINT FK_Teachers_SchoolBranches_BranchId FOREIGN KEY (BranchId) REFERENCES dbo.SchoolBranches(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Teachers_Addresses_AddressId')
    ALTER TABLE dbo.Teachers ADD CONSTRAINT FK_Teachers_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Parents_Users_UserId')
    ALTER TABLE dbo.Parents ADD CONSTRAINT FK_Parents_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ClassRooms_Teachers_TeacherId')
    ALTER TABLE dbo.ClassRooms ADD CONSTRAINT FK_ClassRooms_Teachers_TeacherId FOREIGN KEY (TeacherId) REFERENCES dbo.Teachers(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ClassRooms_SchoolBranches_BranchId')
    ALTER TABLE dbo.ClassRooms ADD CONSTRAINT FK_ClassRooms_SchoolBranches_BranchId FOREIGN KEY (BranchId) REFERENCES dbo.SchoolBranches(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_Parents_ParentId')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_Parents_ParentId FOREIGN KEY (ParentId) REFERENCES dbo.Parents(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_ClassRooms_ClassId')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_ClassRooms_ClassId FOREIGN KEY (ClassId) REFERENCES dbo.ClassRooms(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_SchoolBranches_BranchId')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_SchoolBranches_BranchId FOREIGN KEY (BranchId) REFERENCES dbo.SchoolBranches(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Students_Addresses_AddressId')
    ALTER TABLE dbo.Students ADD CONSTRAINT FK_Students_Addresses_AddressId FOREIGN KEY (AddressId) REFERENCES dbo.Addresses(Id) ON DELETE SET NULL;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Attendances_Students_StudentId')
    ALTER TABLE dbo.Attendances ADD CONSTRAINT FK_Attendances_Students_StudentId FOREIGN KEY (StudentId) REFERENCES dbo.Students(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Attendances_ClassRooms_ClassId')
    ALTER TABLE dbo.Attendances ADD CONSTRAINT FK_Attendances_ClassRooms_ClassId FOREIGN KEY (ClassId) REFERENCES dbo.ClassRooms(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_StudentCheckInOuts_Students_StudentId')
    ALTER TABLE dbo.StudentCheckInOuts ADD CONSTRAINT FK_StudentCheckInOuts_Students_StudentId FOREIGN KEY (StudentId) REFERENCES dbo.Students(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Timetables_ClassRooms_ClassId')
    ALTER TABLE dbo.Timetables ADD CONSTRAINT FK_Timetables_ClassRooms_ClassId FOREIGN KEY (ClassId) REFERENCES dbo.ClassRooms(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Timetables_Teachers_TeacherId')
    ALTER TABLE dbo.Timetables ADD CONSTRAINT FK_Timetables_Teachers_TeacherId FOREIGN KEY (TeacherId) REFERENCES dbo.Teachers(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_FeeStructures_ClassRooms_ClassId')
    ALTER TABLE dbo.FeeStructures ADD CONSTRAINT FK_FeeStructures_ClassRooms_ClassId FOREIGN KEY (ClassId) REFERENCES dbo.ClassRooms(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Payments_Students_StudentId')
    ALTER TABLE dbo.Payments ADD CONSTRAINT FK_Payments_Students_StudentId FOREIGN KEY (StudentId) REFERENCES dbo.Students(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Payments_FeeStructures_FeeStructureId')
    ALTER TABLE dbo.Payments ADD CONSTRAINT FK_Payments_FeeStructures_FeeStructureId FOREIGN KEY (FeeStructureId) REFERENCES dbo.FeeStructures(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Notifications_Users_UserId')
    ALTER TABLE dbo.Notifications ADD CONSTRAINT FK_Notifications_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_RefreshTokens_Users_UserId')
    ALTER TABLE dbo.RefreshTokens ADD CONSTRAINT FK_RefreshTokens_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE;

-- Sample seed data for all tables (idempotent)
DECLARE @UtcNow DATETIME2 = SYSUTCDATETIME();

DECLARE @SuperAdminRoleId UNIQUEIDENTIFIER;
DECLARE @PreschoolAdminRoleId UNIQUEIDENTIFIER;
DECLARE @TeacherRoleId UNIQUEIDENTIFIER;
DECLARE @ParentRoleId UNIQUEIDENTIFIER;

SET @SuperAdminRoleId = (SELECT TOP 1 Id FROM dbo.Roles WHERE RoleName = N'SuperAdmin');
IF @SuperAdminRoleId IS NULL
BEGIN
    SET @SuperAdminRoleId = '10000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Roles (Id, RoleName, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@SuperAdminRoleId, N'SuperAdmin', 0, N'seed-script', @UtcNow, NULL, NULL);
END;

SET @PreschoolAdminRoleId = (SELECT TOP 1 Id FROM dbo.Roles WHERE RoleName = N'PreschoolAdmin');
IF @PreschoolAdminRoleId IS NULL
BEGIN
    SET @PreschoolAdminRoleId = '10000000-0000-0000-0000-000000000002';
    INSERT INTO dbo.Roles (Id, RoleName, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@PreschoolAdminRoleId, N'PreschoolAdmin', 0, N'seed-script', @UtcNow, NULL, NULL);
END;

SET @TeacherRoleId = (SELECT TOP 1 Id FROM dbo.Roles WHERE RoleName = N'Teacher');
IF @TeacherRoleId IS NULL
BEGIN
    SET @TeacherRoleId = '10000000-0000-0000-0000-000000000003';
    INSERT INTO dbo.Roles (Id, RoleName, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@TeacherRoleId, N'Teacher', 0, N'seed-script', @UtcNow, NULL, NULL);
END;

SET @ParentRoleId = (SELECT TOP 1 Id FROM dbo.Roles WHERE RoleName = N'Parent');
IF @ParentRoleId IS NULL
BEGIN
    SET @ParentRoleId = '10000000-0000-0000-0000-000000000004';
    INSERT INTO dbo.Roles (Id, RoleName, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
    VALUES (@ParentRoleId, N'Parent', 0, N'seed-script', @UtcNow, NULL, NULL);
END;

DECLARE @SchoolAddressId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Addresses WHERE AddressLine1 = N'12 Oak Street' AND City = N'Springfield');
IF @SchoolAddressId IS NULL
BEGIN
    SET @SchoolAddressId = '13000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Addresses
    (
        Id, AddressLine1, AddressLine2, City, [State], PostalCode, Country, Latitude, Longitude,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @SchoolAddressId, N'12 Oak Street', NULL, N'Springfield', N'CA', N'94105', N'USA', NULL, NULL,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @TeacherAddressId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Addresses WHERE AddressLine1 = N'89 Maple Drive' AND City = N'Springfield');
IF @TeacherAddressId IS NULL
BEGIN
    SET @TeacherAddressId = '13000000-0000-0000-0000-000000000002';
    INSERT INTO dbo.Addresses
    (
        Id, AddressLine1, AddressLine2, City, [State], PostalCode, Country, Latitude, Longitude,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @TeacherAddressId, N'89 Maple Drive', NULL, N'Springfield', N'CA', N'94107', N'USA', NULL, NULL,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @StudentAddressId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Addresses WHERE AddressLine1 = N'45 Pine Avenue' AND City = N'Springfield');
IF @StudentAddressId IS NULL
BEGIN
    SET @StudentAddressId = '13000000-0000-0000-0000-000000000003';
    INSERT INTO dbo.Addresses
    (
        Id, AddressLine1, AddressLine2, City, [State], PostalCode, Country, Latitude, Longitude,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @StudentAddressId, N'45 Pine Avenue', NULL, N'Springfield', N'CA', N'94108', N'USA', NULL, NULL,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @SuperAdminUserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Users WHERE Email = N'superadmin@preschool.local');
IF @SuperAdminUserId IS NULL
BEGIN
    SET @SuperAdminUserId = '20000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Users
    (
        Id, FirstName, LastName, Email, PhoneNumber, PasswordHash, RoleId,
        ProfilePicture, IsActive, LastLoginDate, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @SuperAdminUserId, N'Super', N'Admin', N'superadmin@preschool.local', N'0000000000',
        N'aG+C8jsE32jrcCqFp+T98w==:E/C69DuC/iN5RFTt/q8Oj3iZ4oiRAnwByhp/6ZssP6k=', @SuperAdminRoleId,
        NULL, 1, @UtcNow, 0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @ParentUserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Users WHERE Email = N'parent.johnson@preschool.local');
IF @ParentUserId IS NULL
BEGIN
    SET @ParentUserId = '20000000-0000-0000-0000-000000000002';
    INSERT INTO dbo.Users
    (
        Id, FirstName, LastName, Email, PhoneNumber, PasswordHash, RoleId,
        ProfilePicture, IsActive, LastLoginDate, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @ParentUserId, N'John', N'Johnson', N'parent.johnson@preschool.local', N'9000000001',
        N'Zv6XNPfS1Flh4+9V7Nr1qg==:+DLaBR5jjT4IaG+5K5EeGZ7+lsXgWEp9YSug4VaeiPM=', @ParentRoleId,
        NULL, 1, NULL, 0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @PreschoolAdminUserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Users WHERE Email = N'admin.sunrise@preschool.local');
IF @PreschoolAdminUserId IS NULL
BEGIN
    SET @PreschoolAdminUserId = '20000000-0000-0000-0000-000000000003';
    INSERT INTO dbo.Users
    (
        Id, FirstName, LastName, Email, PhoneNumber, PasswordHash, RoleId,
        ProfilePicture, IsActive, LastLoginDate, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @PreschoolAdminUserId, N'Sunrise', N'Admin', N'admin.sunrise@preschool.local', N'9000000002',
        N'aG+C8jsE32jrcCqFp+T98w==:E/C69DuC/iN5RFTt/q8Oj3iZ4oiRAnwByhp/6ZssP6k=', @PreschoolAdminRoleId,
        NULL, 1, NULL, 0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @SchoolId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Schools WHERE SchoolName = N'Sunrise Preschool');
IF @SchoolId IS NULL
BEGIN
    SET @SchoolId = '30000000-0000-0000-0000-000000000001';
    IF COL_LENGTH('dbo.Schools', 'Address') IS NOT NULL
    BEGIN
        INSERT INTO dbo.Schools
        (
            Id, SchoolName, Address, AddressId, ContactNumber, Email, Logo,
            IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
        )
        VALUES
        (
            @SchoolId, N'Sunrise Preschool', N'12 Oak Street, Springfield', @SchoolAddressId, N'9000001000', N'contact@sunrise-preschool.local', NULL,
            0, N'seed-script', @UtcNow, NULL, NULL
        );
    END;
    ELSE
    BEGIN
        INSERT INTO dbo.Schools
        (
            Id, SchoolName, AddressId, ContactNumber, Email, Logo,
            IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
        )
        VALUES
        (
            @SchoolId, N'Sunrise Preschool', @SchoolAddressId, N'9000001000', N'contact@sunrise-preschool.local', NULL,
            0, N'seed-script', @UtcNow, NULL, NULL
        );
    END;
END;

IF EXISTS (SELECT 1 FROM dbo.Schools WHERE Id = @SchoolId AND AddressId IS NULL)
BEGIN
    UPDATE dbo.Schools SET AddressId = @SchoolAddressId WHERE Id = @SchoolId;
END;

DECLARE @BranchId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.SchoolBranches WHERE BranchCode = N'SUN-MAIN');
IF @BranchId IS NULL
BEGIN
    SET @BranchId = '14000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.SchoolBranches
    (
        Id, SchoolId, AddressId, BranchName, BranchCode, ContactNumber, Email,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @BranchId, @SchoolId, @SchoolAddressId, N'Sunrise Main Branch', N'SUN-MAIN', N'9000001000', N'main@sunrise-preschool.local',
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.UserSchools WHERE UserId = @SuperAdminUserId AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO dbo.UserSchools
    (
        Id, UserId, SchoolId, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        '12000000-0000-0000-0000-000000000001', @SuperAdminUserId, @SchoolId, 0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.UserSchools WHERE UserId = @PreschoolAdminUserId AND SchoolId = @SchoolId)
BEGIN
    INSERT INTO dbo.UserSchools
    (
        Id, UserId, SchoolId, IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        '12000000-0000-0000-0000-000000000002', @PreschoolAdminUserId, @SchoolId, 0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @TeacherId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Teachers WHERE EmployeeCode = N'TCH-1001');
IF @TeacherId IS NULL
BEGIN
    SET @TeacherId = '40000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Teachers
    (
        Id, EmployeeCode, FirstName, LastName, Gender, DOB, Qualification, Experience,
        PhoneNumber, Email, JoiningDate, ProfileImage, SchoolId, BranchId, AddressId,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @TeacherId, N'TCH-1001', N'Emma', N'Clark', 2, '1990-05-10', N'B.Ed Early Childhood', 6,
        N'9000002000', N'emma.clark@sunrise-preschool.local', '2023-06-01', NULL, @SchoolId, @BranchId, @TeacherAddressId,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

UPDATE dbo.Teachers
SET BranchId = COALESCE(BranchId, @BranchId),
    AddressId = COALESCE(AddressId, @TeacherAddressId)
WHERE Id = @TeacherId;

DECLARE @ClassRoomId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.ClassRooms WHERE ClassName = N'Starters-A');
IF @ClassRoomId IS NULL
BEGIN
    SET @ClassRoomId = '50000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.ClassRooms
    (
        Id, ClassName, AgeGroup, Capacity, TeacherId, BranchId,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @ClassRoomId, N'Starters-A', N'3-4 Years', 25, @TeacherId, @BranchId,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

UPDATE dbo.ClassRooms
SET BranchId = COALESCE(BranchId, @BranchId)
WHERE Id = @ClassRoomId;

DECLARE @ParentId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Parents WHERE Email = N'parent.johnson@preschool.local');
IF @ParentId IS NULL
BEGIN
    SET @ParentId = '60000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Parents
    (
        Id, FatherName, MotherName, Email, PhoneNumber, Address, Occupation, UserId,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @ParentId, N'Mark Johnson', N'Sarah Johnson', N'parent.johnson@preschool.local', N'9000003000',
        N'45 Pine Avenue, Springfield', N'Engineer', @ParentUserId,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @StudentId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Students WHERE AdmissionNumber = N'ADM-2026-001');
IF @StudentId IS NULL
BEGIN
    SET @StudentId = '70000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Students
    (
        Id, AdmissionNumber, FirstName, LastName, Gender, DOB, BloodGroup, AddressId,
        JoiningDate, ClassId, ParentId, BranchId, ProfilePicture, Status,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @StudentId, N'ADM-2026-001', N'Olivia', N'Johnson', 2, '2021-03-21', N'O+', @StudentAddressId,
        '2026-04-01', @ClassRoomId, @ParentId, @BranchId, NULL, 1,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

UPDATE dbo.Students
SET BranchId = COALESCE(BranchId, @BranchId),
    AddressId = COALESCE(AddressId, @StudentAddressId)
WHERE Id = @StudentId;

DECLARE @AttendanceId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Attendances WHERE StudentId = @StudentId AND [Date] = CAST('2026-08-01' AS DATE));
IF @AttendanceId IS NULL
BEGIN
    SET @AttendanceId = '80000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Attendances
    (
        Id, StudentId, ClassId, [Date], Status, Remarks,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @AttendanceId, @StudentId, @ClassRoomId, '2026-08-01', 1, N'On time',
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @CheckInOutId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.StudentCheckInOuts WHERE StudentId = @StudentId AND CheckInTime = '2026-08-01T08:32:00');
IF @CheckInOutId IS NULL
BEGIN
    SET @CheckInOutId = '90000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.StudentCheckInOuts
    (
        Id, StudentId, CheckInTime, CheckOutTime, PickupPerson, PickupRelationship, Notes,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @CheckInOutId, @StudentId, '2026-08-01T08:32:00', '2026-08-01T15:01:00', N'Mark Johnson', N'Father', N'Normal day',
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @TimetableId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Timetables WHERE ClassId = @ClassRoomId AND DayOfWeek = 1 AND StartTime = CAST('09:00:00' AS TIME));
IF @TimetableId IS NULL
BEGIN
    SET @TimetableId = 'A0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Timetables
    (
        Id, ClassId, DayOfWeek, StartTime, EndTime, ActivityName, TeacherId,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @TimetableId, @ClassRoomId, 1, '09:00:00', '10:00:00', N'Phonics Circle', @TeacherId,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @FeeStructureId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.FeeStructures WHERE ClassId = @ClassRoomId AND FeeType = N'Monthly Tuition' AND DueDate = CAST('2026-08-05' AS DATE));
IF @FeeStructureId IS NULL
BEGIN
    SET @FeeStructureId = 'B0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.FeeStructures
    (
        Id, ClassId, FeeType, Amount, DueDate,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @FeeStructureId, @ClassRoomId, N'Monthly Tuition', 7500.00, '2026-08-05',
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @PaymentId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Payments WHERE TransactionReference = N'TXN-2026-0001');
IF @PaymentId IS NULL
BEGIN
    SET @PaymentId = 'C0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Payments
    (
        Id, StudentId, FeeStructureId, AmountPaid, PaymentDate, PaymentMethod, TransactionReference, Status,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @PaymentId, @StudentId, @FeeStructureId, 7500.00, '2026-08-03T11:20:00', 3, N'TXN-2026-0001', 2,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @InventoryItemId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.InventoryItems WHERE ItemName = N'Color Pencils Pack');
IF @InventoryItemId IS NULL
BEGIN
    SET @InventoryItemId = 'D0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.InventoryItems
    (
        Id, ItemName, Category, Quantity, AvailableQuantity, UnitPrice, PurchaseDate, SupplierName,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @InventoryItemId, N'Color Pencils Pack', N'Stationery', 100, 80, 120.00, '2026-07-25', N'Learning Supplies Ltd',
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @NoticeId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Notices WHERE Title = N'Parent Orientation Week');
IF @NoticeId IS NULL
BEGIN
    SET @NoticeId = 'E0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Notices
    (
        Id, Title, Description, PublishDate, ExpiryDate, Author, TargetAudience,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @NoticeId, N'Parent Orientation Week', N'Orientation sessions will run from Monday to Friday at 4 PM.',
        '2026-08-01T09:00:00', '2026-08-10T18:00:00', N'Admin Office', 2,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @NotificationId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Notifications WHERE UserId = @ParentUserId AND Title = N'Fee Received');
IF @NotificationId IS NULL
BEGIN
    SET @NotificationId = 'F0000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.Notifications
    (
        Id, UserId, Title, Message, NotificationType, IsRead,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @NotificationId, @ParentUserId, N'Fee Received', N'Your payment for Monthly Tuition has been received.', 2, 0,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

DECLARE @RefreshTokenId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.RefreshTokens WHERE Token = N'seed-refresh-token-superadmin-2026');
IF @RefreshTokenId IS NULL
BEGIN
    SET @RefreshTokenId = '11000000-0000-0000-0000-000000000001';
    INSERT INTO dbo.RefreshTokens
    (
        Id, UserId, Token, ExpiresAt, IsRevoked,
        IsDeleted, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate
    )
    VALUES
    (
        @RefreshTokenId, @SuperAdminUserId, N'seed-refresh-token-superadmin-2026', DATEADD(DAY, 30, @UtcNow), 0,
        0, N'seed-script', @UtcNow, NULL, NULL
    );
END;

IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AppUser] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [DateReg] datetime2 NOT NULL,
    [Email] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [Firstname] nvarchar(max) NULL,
    [Lastname] nvarchar(max) NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(max) NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRole] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRole] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaim] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaim] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUserClaim] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaim] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUserLogin] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserLogin] PRIMARY KEY ([UserId])
);

GO

CREATE TABLE [AspNetUserRole] (
    [RoleId] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRole] PRIMARY KEY ([RoleId], [UserId])
);

GO

CREATE TABLE [AspNetUserToken] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserToken] PRIMARY KEY ([UserId])
);

GO

CREATE TABLE [Files] (
    [Id] int NOT NULL IDENTITY,
    [AppUserId] nvarchar(450) NULL,
    [Date] datetime2 NOT NULL,
    [FileUrl] nvarchar(max) NULL,
    [IsLocked] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    [Tagname] nvarchar(max) NULL,
    CONSTRAINT [PK_Files] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Files_AppUser_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUser] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Downloads] (
    [Id] int NOT NULL IDENTITY,
    [AppUserId] nvarchar(450) NULL,
    [Date] datetime2 NOT NULL,
    [FileId] int NOT NULL,
    CONSTRAINT [PK_Downloads] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Downloads_AppUser_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUser] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Downloads_Files_FileId] FOREIGN KEY ([FileId]) REFERENCES [Files] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Histories] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NULL,
    [AppUserId] nvarchar(450) NULL,
    [Date] datetime2 NOT NULL,
    [FileId] int NOT NULL,
    [IsApproved] bit NOT NULL,
    CONSTRAINT [PK_Histories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Histories_AppUser_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUser] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Histories_Files_FileId] FOREIGN KEY ([FileId]) REFERENCES [Files] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Downloads_AppUserId] ON [Downloads] ([AppUserId]);

GO

CREATE INDEX [IX_Downloads_FileId] ON [Downloads] ([FileId]);

GO

CREATE INDEX [IX_Files_AppUserId] ON [Files] ([AppUserId]);

GO

CREATE INDEX [IX_Histories_AppUserId] ON [Histories] ([AppUserId]);

GO

CREATE INDEX [IX_Histories_FileId] ON [Histories] ([FileId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20171026140545_InitialCreate', N'2.0.0-rtm-26452');

GO


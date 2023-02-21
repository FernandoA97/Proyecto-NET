IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207022436_InitialMigration')
BEGIN
    CREATE TABLE [Genres] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207022436_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230207022436_InitialMigration', N'7.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207025909_FixGenres')
BEGIN
    ALTER TABLE [Genres] DROP CONSTRAINT [PK_Genres];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207025909_FixGenres')
BEGIN
    EXEC sp_rename N'[Genres]', N'Genre';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207025909_FixGenres')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Genre]') AND [c].[name] = N'Name');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Genre] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Genre] ALTER COLUMN [Name] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207025909_FixGenres')
BEGIN
    ALTER TABLE [Genre] ADD CONSTRAINT [PK_Genre] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230207025909_FixGenres')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230207025909_FixGenres', N'7.0.2');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE TABLE [Concert] (
        [Id] int NOT NULL IDENTITY,
        [GenreId] int NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [DateEvent] datetime2 NOT NULL,
        [ImageUrl] varchar(1000) NULL,
        [Place] nvarchar(100) NOT NULL,
        [TicketsQuantity] int NOT NULL,
        [UnitPrice] decimal(11,2) NOT NULL,
        [Finalized] bit NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Concert] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Concert_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE TABLE [Customer] (
        [Id] int NOT NULL IDENTITY,
        [Email] varchar(200) NOT NULL,
        [FullName] nvarchar(200) NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE TABLE [Sale] (
        [Id] int NOT NULL IDENTITY,
        [CustomerFk] int NOT NULL,
        [ConcertId] int NOT NULL,
        [SaleDate] datetime2 NOT NULL,
        [OperationNumber] varchar(20) NOT NULL,
        [Total] decimal(11,2) NOT NULL,
        [Quantity] smallint NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Sale] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Sale_Concert_ConcertId] FOREIGN KEY ([ConcertId]) REFERENCES [Concert] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Sale_Customer_CustomerFk] FOREIGN KEY ([CustomerFk]) REFERENCES [Customer] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] ON;
    EXEC(N'INSERT INTO [Genre] ([Id], [Name], [Status])
    VALUES (1, N''Rock'', CAST(1 AS bit)),
    (2, N''Pop'', CAST(1 AS bit)),
    (3, N''Jazz'', CAST(1 AS bit)),
    (4, N''Metal'', CAST(1 AS bit)),
    (5, N''Blues'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE INDEX [IX_Concert_GenreId] ON [Concert] ([GenreId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE INDEX [IX_Sale_ConcertId] ON [Sale] ([ConcertId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    CREATE INDEX [IX_Sale_CustomerFk] ON [Sale] ([CustomerFk]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230209010306_MoreTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230209010306_MoreTables', N'7.0.2');
END;
GO

COMMIT;
GO


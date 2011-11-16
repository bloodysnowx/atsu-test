
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/14/2011 16:24:35
-- Generated from EDMX file: D:\src\atsu-test\poker\PTRtoNote2\PTRtoNote2Model\PTRDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PlayerStakesData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StakesDatas] DROP CONSTRAINT [FK_PlayerStakesData];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyStake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stakes] DROP CONSTRAINT [FK_CurrencyStake];
GO
IF OBJECT_ID(N'[dbo].[FK_GameStake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stakes] DROP CONSTRAINT [FK_GameStake];
GO
IF OBJECT_ID(N'[dbo].[FK_BetTypeStake]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stakes] DROP CONSTRAINT [FK_BetTypeStake];
GO
IF OBJECT_ID(N'[dbo].[FK_StakeStakesData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StakesDatas] DROP CONSTRAINT [FK_StakeStakesData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[StakesDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StakesDatas];
GO
IF OBJECT_ID(N'[dbo].[Stakes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stakes];
GO
IF OBJECT_ID(N'[dbo].[Currencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currencies];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[BetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BetTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PlayerName] nvarchar(128)  NOT NULL,
    [BB_100] decimal(18,2)  NOT NULL,
    [Earnings] int  NOT NULL,
    [Rating] int  NOT NULL,
    [Hands] int  NOT NULL,
    [DateOfData] datetime  NOT NULL,
    [DateOfLastPlay] datetime  NOT NULL,
    [HU_BB_100] decimal(18,2)  NOT NULL,
    [HU_Hands] int  NOT NULL,
    [HU_Earnings] int  NOT NULL,
    [O_Hands] int  NOT NULL,
    [O_BB_100] decimal(18,2)  NOT NULL,
    [O_Earnings] int  NOT NULL,
    [FL_Hands] int  NOT NULL,
    [FL_BB_100] decimal(18,2)  NOT NULL,
    [FL_Earnings] int  NOT NULL,
    [NL_Hands] int  NOT NULL,
    [NL_BB_100] decimal(18,2)  NOT NULL,
    [NL_Earnings] int  NOT NULL,
    [EX_BB_100] decimal(18,2)  NOT NULL,
    [EX_Hands] int  NOT NULL,
    [EX_Earnings] int  NOT NULL
);
GO

-- Creating table 'StakesDatas'
CREATE TABLE [dbo].[StakesDatas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Hands] int  NOT NULL,
    [BB_100] decimal(18,2)  NOT NULL,
    [Earnings] int  NOT NULL,
    [TotalBB] int  NOT NULL,
    [DateOfLastPlay] datetime  NOT NULL,
    [Player_ID] int  NOT NULL,
    [Stake_ID] int  NOT NULL
);
GO

-- Creating table 'Stakes'
CREATE TABLE [dbo].[Stakes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Rate] int  NOT NULL,
    [PlayerNum] int  NOT NULL,
    [StackSize] int  NOT NULL,
    [Currency_ID] int  NOT NULL,
    [Game_ID] int  NOT NULL,
    [BetType_ID] int  NOT NULL
);
GO

-- Creating table 'Currencies'
CREATE TABLE [dbo].[Currencies] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'BetTypes'
CREATE TABLE [dbo].[BetTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(16)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StakesDatas'
ALTER TABLE [dbo].[StakesDatas]
ADD CONSTRAINT [PK_StakesDatas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Stakes'
ALTER TABLE [dbo].[Stakes]
ADD CONSTRAINT [PK_Stakes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Currencies'
ALTER TABLE [dbo].[Currencies]
ADD CONSTRAINT [PK_Currencies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BetTypes'
ALTER TABLE [dbo].[BetTypes]
ADD CONSTRAINT [PK_BetTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Player_ID] in table 'StakesDatas'
ALTER TABLE [dbo].[StakesDatas]
ADD CONSTRAINT [FK_PlayerStakesData]
    FOREIGN KEY ([Player_ID])
    REFERENCES [dbo].[Players]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerStakesData'
CREATE INDEX [IX_FK_PlayerStakesData]
ON [dbo].[StakesDatas]
    ([Player_ID]);
GO

-- Creating foreign key on [Currency_ID] in table 'Stakes'
ALTER TABLE [dbo].[Stakes]
ADD CONSTRAINT [FK_CurrencyStake]
    FOREIGN KEY ([Currency_ID])
    REFERENCES [dbo].[Currencies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyStake'
CREATE INDEX [IX_FK_CurrencyStake]
ON [dbo].[Stakes]
    ([Currency_ID]);
GO

-- Creating foreign key on [Game_ID] in table 'Stakes'
ALTER TABLE [dbo].[Stakes]
ADD CONSTRAINT [FK_GameStake]
    FOREIGN KEY ([Game_ID])
    REFERENCES [dbo].[Games]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GameStake'
CREATE INDEX [IX_FK_GameStake]
ON [dbo].[Stakes]
    ([Game_ID]);
GO

-- Creating foreign key on [BetType_ID] in table 'Stakes'
ALTER TABLE [dbo].[Stakes]
ADD CONSTRAINT [FK_BetTypeStake]
    FOREIGN KEY ([BetType_ID])
    REFERENCES [dbo].[BetTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BetTypeStake'
CREATE INDEX [IX_FK_BetTypeStake]
ON [dbo].[Stakes]
    ([BetType_ID]);
GO

-- Creating foreign key on [Stake_ID] in table 'StakesDatas'
ALTER TABLE [dbo].[StakesDatas]
ADD CONSTRAINT [FK_StakeStakesData]
    FOREIGN KEY ([Stake_ID])
    REFERENCES [dbo].[Stakes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StakeStakesData'
CREATE INDEX [IX_FK_StakeStakesData]
ON [dbo].[StakesDatas]
    ([Stake_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
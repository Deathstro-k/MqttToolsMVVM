
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/18/2022 16:05:11
-- Generated from EDMX file: C:\VSProjects\MqttToolsMVVM\BaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MqttToolsDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MqttServerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MqttServerSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MqttServerSet'
CREATE TABLE [dbo].[MqttServerSet] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Name] int IDENTITY(1,1) NOT NULL,
    [MqttServer_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'MqttServerSet'
ALTER TABLE [dbo].[MqttServerSet]
ADD CONSTRAINT [PK_MqttServerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Name] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([Name] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MqttServer_Id] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [FK_MqttServerUsers]
    FOREIGN KEY ([MqttServer_Id])
    REFERENCES [dbo].[MqttServerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MqttServerUsers'
CREATE INDEX [IX_FK_MqttServerUsers]
ON [dbo].[UsersSet]
    ([MqttServer_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
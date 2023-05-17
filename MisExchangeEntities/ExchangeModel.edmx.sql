
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/07/2015 15:08:31
-- Generated from EDMX file: D:\PROJECTS\NLSdotNetProjects\dev_MisExchange\MisExchangeService\MisExchangeEntities\ExchangeModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Mis_Exchange_Objects];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_RequestFilterStatusRequestFilter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequestFilterStatusSet] DROP CONSTRAINT [FK_RequestFilterStatusRequestFilter];
GO
IF OBJECT_ID(N'[dbo].[FK_SampleTarget]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TargetSet] DROP CONSTRAINT [FK_SampleTarget];
GO
IF OBJECT_ID(N'[dbo].[FK_RequestPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequestSet] DROP CONSTRAINT [FK_RequestPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientPatientCard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PatientSet] DROP CONSTRAINT [FK_PatientPatientCard];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientCardUserField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFieldSet] DROP CONSTRAINT [FK_PatientCardUserField];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientUserField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFieldSet] DROP CONSTRAINT [FK_PatientUserField];
GO
IF OBJECT_ID(N'[dbo].[FK_RequestUserField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFieldSet] DROP CONSTRAINT [FK_RequestUserField];
GO
IF OBJECT_ID(N'[dbo].[FK_RequestSample]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SampleSet] DROP CONSTRAINT [FK_RequestSample];
GO
IF OBJECT_ID(N'[dbo].[FK_RequestStatusRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RequestStatusSet] DROP CONSTRAINT [FK_RequestStatusRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultSampleResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SampleResultSet] DROP CONSTRAINT [FK_ResultSampleResult];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultStatusResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ResultStatusSet] DROP CONSTRAINT [FK_ResultStatusResult];
GO
IF OBJECT_ID(N'[dbo].[FK_SampleResultTargetResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TargetResultSet] DROP CONSTRAINT [FK_SampleResultTargetResult];
GO
IF OBJECT_ID(N'[dbo].[FK_TargetResultWork]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkSet] DROP CONSTRAINT [FK_TargetResultWork];
GO
IF OBJECT_ID(N'[dbo].[FK_SampleResultMicroResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MicroResultSet] DROP CONSTRAINT [FK_SampleResultMicroResult];
GO
IF OBJECT_ID(N'[dbo].[FK_MicroResultWork]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkSet] DROP CONSTRAINT [FK_MicroResultWork];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkNorm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkSet] DROP CONSTRAINT [FK_WorkNorm];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ResultSet] DROP CONSTRAINT [FK_PatientResult];
GO
IF OBJECT_ID(N'[dbo].[FK_ResultUserField]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFieldSet] DROP CONSTRAINT [FK_ResultUserField];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkDefect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DefectSet] DROP CONSTRAINT [FK_WorkDefect];
GO
IF OBJECT_ID(N'[dbo].[FK_SampleResultDefect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DefectSet] DROP CONSTRAINT [FK_SampleResultDefect];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[RequestFilterSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequestFilterSet];
GO
IF OBJECT_ID(N'[dbo].[RequestFilterStatusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequestFilterStatusSet];
GO
IF OBJECT_ID(N'[dbo].[SampleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SampleSet];
GO
IF OBJECT_ID(N'[dbo].[TargetSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TargetSet];
GO
IF OBJECT_ID(N'[dbo].[RequestSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequestSet];
GO
IF OBJECT_ID(N'[dbo].[PatientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientSet];
GO
IF OBJECT_ID(N'[dbo].[PatientCardSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientCardSet];
GO
IF OBJECT_ID(N'[dbo].[UserFieldSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFieldSet];
GO
IF OBJECT_ID(N'[dbo].[RequestStatusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequestStatusSet];
GO
IF OBJECT_ID(N'[dbo].[ResultSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResultSet];
GO
IF OBJECT_ID(N'[dbo].[SampleResultSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SampleResultSet];
GO
IF OBJECT_ID(N'[dbo].[DefectSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DefectSet];
GO
IF OBJECT_ID(N'[dbo].[TargetResultSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TargetResultSet];
GO
IF OBJECT_ID(N'[dbo].[MicroResultSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MicroResultSet];
GO
IF OBJECT_ID(N'[dbo].[WorkSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkSet];
GO
IF OBJECT_ID(N'[dbo].[NormSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NormSet];
GO
IF OBJECT_ID(N'[dbo].[ResultStatusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResultStatusSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'RequestFilterSet'
CREATE TABLE [dbo].[RequestFilterSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [PatientCodes] nvarchar(max)  NULL,
    [BirthDate] datetime  NULL,
    [Sex] int  NULL,
    [RequestCodes] nvarchar(max)  NULL,
    [DateFrom] datetime  NULL,
    [DateTill] datetime  NULL,
    [States] nvarchar(max)  NULL,
    [Priority] int  NULL,
    [DefectState] int  NULL,
    [CustomStates] nvarchar(max)  NULL,
    [Targets] nvarchar(max)  NULL,
    [Departments] nvarchar(max)  NULL,
    [Hospitals] nvarchar(max)  NULL,
    [CustDepartments] nvarchar(max)  NULL,
    [Doctors] nvarchar(max)  NULL,
    [EndDateFrom] datetime  NULL,
    [EndDateTill] datetime  NULL
);
GO

-- Creating table 'RequestFilterStatusSet'
CREATE TABLE [dbo].[RequestFilterStatusSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Status] tinyint  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [RequestFilter_Id] int  NOT NULL
);
GO

-- Creating table 'SampleSet'
CREATE TABLE [dbo].[SampleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Barcode] nvarchar(10)  NULL,
    [BiomaterialCode] nvarchar(16)  NOT NULL,
    [Priority] int  NOT NULL,
    [RequestId] int  NOT NULL
);
GO

-- Creating table 'TargetSet'
CREATE TABLE [dbo].[TargetSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Priority] int  NOT NULL,
    [ReadOnly] bit  NULL,
    [SampleId] int  NOT NULL
);
GO

-- Creating table 'RequestSet'
CREATE TABLE [dbo].[RequestSet] (
    [RequestCode] nvarchar(10)  NOT NULL,
    [HospitalCode] nvarchar(16)  NOT NULL,
    [HospitalName] nvarchar(64)  NULL,
    [DepartmentCode] nvarchar(16)  NULL,
    [DepartmentName] nvarchar(128)  NULL,
    [DoctorCode] nvarchar(16)  NULL,
    [DoctorName] nvarchar(128)  NULL,
    [SamplingDate] datetime  NULL,
    [SampleDeliveryDate] datetime  NULL,
    [PregnancyDuration] int  NULL,
    [CyclePeriod] int  NULL,
    [ReadOnly] bit  NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [PatientId] int  NOT NULL,
    [PayCategoryCode] nvarchar(16)  NULL,
    [Email] nvarchar(128)  NULL
);
GO

-- Creating table 'PatientSet'
CREATE TABLE [dbo].[PatientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [FirstName] nvarchar(64)  NOT NULL,
    [LastName] nvarchar(64)  NOT NULL,
    [MiddleName] nvarchar(64)  NOT NULL,
    [BirthDay] int  NOT NULL,
    [BirthMonth] int  NOT NULL,
    [BirthYear] int  NOT NULL,
    [Sex] int  NOT NULL,
    [Country] nvarchar(128)  NULL,
    [City] nvarchar(128)  NULL,
    [Street] nvarchar(128)  NULL,
    [Building] nvarchar(128)  NULL,
    [Flat] nvarchar(128)  NULL,
    [InsuranceCompany] nvarchar(1024)  NULL,
    [PolicySeries] nvarchar(128)  NULL,
    [PolicyNumber] nvarchar(128)  NULL,
    [PatientCard_Id] int  NOT NULL
);
GO

-- Creating table 'PatientCardSet'
CREATE TABLE [dbo].[PatientCardSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CardNr] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserFieldSet'
CREATE TABLE [dbo].[UserFieldSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Name] nvarchar(64)  NULL,
    [Value] nvarchar(1024)  NOT NULL,
    [PatientCardId] int  NULL,
    [PatientId] int  NULL,
    [RequestId] int  NULL,
    [ResultId] int  NULL
);
GO

-- Creating table 'RequestStatusSet'
CREATE TABLE [dbo].[RequestStatusSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Status] tinyint  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [Request_Id] int  NOT NULL
);
GO

-- Creating table 'ResultSet'
CREATE TABLE [dbo].[ResultSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RequestCode] nvarchar(16)  NOT NULL,
    [ResultStatusId] int  NOT NULL,
    [PatientId] int  NOT NULL
);
GO

-- Creating table 'SampleResultSet'
CREATE TABLE [dbo].[SampleResultSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BiomaterialCode] nvarchar(16)  NOT NULL,
    [Barcode] nvarchar(16)  NOT NULL,
    [Comments] nvarchar(1024)  NULL,
    [ResultId] int  NOT NULL
);
GO

-- Creating table 'DefectSet'
CREATE TABLE [dbo].[DefectSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Name] nvarchar(64)  NOT NULL,
    [WorkId] int  NULL,
    [SampleResultId] int  NULL
);
GO

-- Creating table 'TargetResultSet'
CREATE TABLE [dbo].[TargetResultSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Name] nvarchar(1024)  NOT NULL,
    [Comments] nvarchar(1024)  NULL,
    [SampleResultId] int  NOT NULL
);
GO

-- Creating table 'MicroResultSet'
CREATE TABLE [dbo].[MicroResultSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Name] nvarchar(64)  NOT NULL,
    [Value] nvarchar(1024)  NOT NULL,
    [Comments] nvarchar(1024)  NULL,
    [SampleResultId] int  NOT NULL
);
GO

-- Creating table 'WorkSet'
CREATE TABLE [dbo].[WorkSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(16)  NOT NULL,
    [Name] nvarchar(1024)  NOT NULL,
    [Value] nvarchar(1024)  NOT NULL,
    [UnitName] nvarchar(64)  NULL,
    [State] int  NOT NULL,
    [ApprovingDoctor] nvarchar(1024)  NULL,
    [CreateDate] datetime  NOT NULL,
    [ApproveDate] datetime  NULL,
    [ModifyDate] datetime  NULL,
    [GroupRank] int  NULL,
    [RankInGroup] int  NULL,
    [TestRank] int  NULL,
    [Precision] int  NULL,
    [Comments] nvarchar(1024)  NULL,
    [Normality] int  NOT NULL,
    [TargetResultId] int  NULL,
    [MicroResultId] int  NULL,
    [Norm_Id] int  NOT NULL
);
GO

-- Creating table 'NormSet'
CREATE TABLE [dbo].[NormSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CriticalLowLimit] float  NULL,
    [LowLimit] float  NULL,
    [CriticalHighLimit] float  NULL,
    [HighLimit] float  NULL,
    [Norms] nvarchar(1024)  NULL,
    [NormComment] nvarchar(128)  NULL,
    [UnitName] nvarchar(128)  NULL
);
GO

-- Creating table 'ResultStatusSet'
CREATE TABLE [dbo].[ResultStatusSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Status] tinyint  NOT NULL,
    [Comments] nvarchar(max)  NULL,
    [Result_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'RequestFilterSet'
ALTER TABLE [dbo].[RequestFilterSet]
ADD CONSTRAINT [PK_RequestFilterSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RequestFilterStatusSet'
ALTER TABLE [dbo].[RequestFilterStatusSet]
ADD CONSTRAINT [PK_RequestFilterStatusSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SampleSet'
ALTER TABLE [dbo].[SampleSet]
ADD CONSTRAINT [PK_SampleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TargetSet'
ALTER TABLE [dbo].[TargetSet]
ADD CONSTRAINT [PK_TargetSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RequestSet'
ALTER TABLE [dbo].[RequestSet]
ADD CONSTRAINT [PK_RequestSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PatientSet'
ALTER TABLE [dbo].[PatientSet]
ADD CONSTRAINT [PK_PatientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PatientCardSet'
ALTER TABLE [dbo].[PatientCardSet]
ADD CONSTRAINT [PK_PatientCardSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserFieldSet'
ALTER TABLE [dbo].[UserFieldSet]
ADD CONSTRAINT [PK_UserFieldSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RequestStatusSet'
ALTER TABLE [dbo].[RequestStatusSet]
ADD CONSTRAINT [PK_RequestStatusSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResultSet'
ALTER TABLE [dbo].[ResultSet]
ADD CONSTRAINT [PK_ResultSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SampleResultSet'
ALTER TABLE [dbo].[SampleResultSet]
ADD CONSTRAINT [PK_SampleResultSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DefectSet'
ALTER TABLE [dbo].[DefectSet]
ADD CONSTRAINT [PK_DefectSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TargetResultSet'
ALTER TABLE [dbo].[TargetResultSet]
ADD CONSTRAINT [PK_TargetResultSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MicroResultSet'
ALTER TABLE [dbo].[MicroResultSet]
ADD CONSTRAINT [PK_MicroResultSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WorkSet'
ALTER TABLE [dbo].[WorkSet]
ADD CONSTRAINT [PK_WorkSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NormSet'
ALTER TABLE [dbo].[NormSet]
ADD CONSTRAINT [PK_NormSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResultStatusSet'
ALTER TABLE [dbo].[ResultStatusSet]
ADD CONSTRAINT [PK_ResultStatusSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RequestFilter_Id] in table 'RequestFilterStatusSet'
ALTER TABLE [dbo].[RequestFilterStatusSet]
ADD CONSTRAINT [FK_RequestFilterStatusRequestFilter]
    FOREIGN KEY ([RequestFilter_Id])
    REFERENCES [dbo].[RequestFilterSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequestFilterStatusRequestFilter'
CREATE INDEX [IX_FK_RequestFilterStatusRequestFilter]
ON [dbo].[RequestFilterStatusSet]
    ([RequestFilter_Id]);
GO

-- Creating foreign key on [SampleId] in table 'TargetSet'
ALTER TABLE [dbo].[TargetSet]
ADD CONSTRAINT [FK_SampleTarget]
    FOREIGN KEY ([SampleId])
    REFERENCES [dbo].[SampleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SampleTarget'
CREATE INDEX [IX_FK_SampleTarget]
ON [dbo].[TargetSet]
    ([SampleId]);
GO

-- Creating foreign key on [PatientId] in table 'RequestSet'
ALTER TABLE [dbo].[RequestSet]
ADD CONSTRAINT [FK_RequestPatient]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[PatientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequestPatient'
CREATE INDEX [IX_FK_RequestPatient]
ON [dbo].[RequestSet]
    ([PatientId]);
GO

-- Creating foreign key on [PatientCard_Id] in table 'PatientSet'
ALTER TABLE [dbo].[PatientSet]
ADD CONSTRAINT [FK_PatientPatientCard]
    FOREIGN KEY ([PatientCard_Id])
    REFERENCES [dbo].[PatientCardSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientPatientCard'
CREATE INDEX [IX_FK_PatientPatientCard]
ON [dbo].[PatientSet]
    ([PatientCard_Id]);
GO

-- Creating foreign key on [PatientCardId] in table 'UserFieldSet'
ALTER TABLE [dbo].[UserFieldSet]
ADD CONSTRAINT [FK_PatientCardUserField]
    FOREIGN KEY ([PatientCardId])
    REFERENCES [dbo].[PatientCardSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientCardUserField'
CREATE INDEX [IX_FK_PatientCardUserField]
ON [dbo].[UserFieldSet]
    ([PatientCardId]);
GO

-- Creating foreign key on [PatientId] in table 'UserFieldSet'
ALTER TABLE [dbo].[UserFieldSet]
ADD CONSTRAINT [FK_PatientUserField]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[PatientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientUserField'
CREATE INDEX [IX_FK_PatientUserField]
ON [dbo].[UserFieldSet]
    ([PatientId]);
GO

-- Creating foreign key on [RequestId] in table 'UserFieldSet'
ALTER TABLE [dbo].[UserFieldSet]
ADD CONSTRAINT [FK_RequestUserField]
    FOREIGN KEY ([RequestId])
    REFERENCES [dbo].[RequestSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequestUserField'
CREATE INDEX [IX_FK_RequestUserField]
ON [dbo].[UserFieldSet]
    ([RequestId]);
GO

-- Creating foreign key on [RequestId] in table 'SampleSet'
ALTER TABLE [dbo].[SampleSet]
ADD CONSTRAINT [FK_RequestSample]
    FOREIGN KEY ([RequestId])
    REFERENCES [dbo].[RequestSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequestSample'
CREATE INDEX [IX_FK_RequestSample]
ON [dbo].[SampleSet]
    ([RequestId]);
GO

-- Creating foreign key on [Request_Id] in table 'RequestStatusSet'
ALTER TABLE [dbo].[RequestStatusSet]
ADD CONSTRAINT [FK_RequestStatusRequest]
    FOREIGN KEY ([Request_Id])
    REFERENCES [dbo].[RequestSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RequestStatusRequest'
CREATE INDEX [IX_FK_RequestStatusRequest]
ON [dbo].[RequestStatusSet]
    ([Request_Id]);
GO

-- Creating foreign key on [ResultId] in table 'SampleResultSet'
ALTER TABLE [dbo].[SampleResultSet]
ADD CONSTRAINT [FK_ResultSampleResult]
    FOREIGN KEY ([ResultId])
    REFERENCES [dbo].[ResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultSampleResult'
CREATE INDEX [IX_FK_ResultSampleResult]
ON [dbo].[SampleResultSet]
    ([ResultId]);
GO

-- Creating foreign key on [Result_Id] in table 'ResultStatusSet'
ALTER TABLE [dbo].[ResultStatusSet]
ADD CONSTRAINT [FK_ResultStatusResult]
    FOREIGN KEY ([Result_Id])
    REFERENCES [dbo].[ResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultStatusResult'
CREATE INDEX [IX_FK_ResultStatusResult]
ON [dbo].[ResultStatusSet]
    ([Result_Id]);
GO

-- Creating foreign key on [SampleResultId] in table 'TargetResultSet'
ALTER TABLE [dbo].[TargetResultSet]
ADD CONSTRAINT [FK_SampleResultTargetResult]
    FOREIGN KEY ([SampleResultId])
    REFERENCES [dbo].[SampleResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SampleResultTargetResult'
CREATE INDEX [IX_FK_SampleResultTargetResult]
ON [dbo].[TargetResultSet]
    ([SampleResultId]);
GO

-- Creating foreign key on [TargetResultId] in table 'WorkSet'
ALTER TABLE [dbo].[WorkSet]
ADD CONSTRAINT [FK_TargetResultWork]
    FOREIGN KEY ([TargetResultId])
    REFERENCES [dbo].[TargetResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TargetResultWork'
CREATE INDEX [IX_FK_TargetResultWork]
ON [dbo].[WorkSet]
    ([TargetResultId]);
GO

-- Creating foreign key on [SampleResultId] in table 'MicroResultSet'
ALTER TABLE [dbo].[MicroResultSet]
ADD CONSTRAINT [FK_SampleResultMicroResult]
    FOREIGN KEY ([SampleResultId])
    REFERENCES [dbo].[SampleResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SampleResultMicroResult'
CREATE INDEX [IX_FK_SampleResultMicroResult]
ON [dbo].[MicroResultSet]
    ([SampleResultId]);
GO

-- Creating foreign key on [MicroResultId] in table 'WorkSet'
ALTER TABLE [dbo].[WorkSet]
ADD CONSTRAINT [FK_MicroResultWork]
    FOREIGN KEY ([MicroResultId])
    REFERENCES [dbo].[MicroResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MicroResultWork'
CREATE INDEX [IX_FK_MicroResultWork]
ON [dbo].[WorkSet]
    ([MicroResultId]);
GO

-- Creating foreign key on [Norm_Id] in table 'WorkSet'
ALTER TABLE [dbo].[WorkSet]
ADD CONSTRAINT [FK_WorkNorm]
    FOREIGN KEY ([Norm_Id])
    REFERENCES [dbo].[NormSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkNorm'
CREATE INDEX [IX_FK_WorkNorm]
ON [dbo].[WorkSet]
    ([Norm_Id]);
GO

-- Creating foreign key on [PatientId] in table 'ResultSet'
ALTER TABLE [dbo].[ResultSet]
ADD CONSTRAINT [FK_PatientResult]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[PatientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientResult'
CREATE INDEX [IX_FK_PatientResult]
ON [dbo].[ResultSet]
    ([PatientId]);
GO

-- Creating foreign key on [ResultId] in table 'UserFieldSet'
ALTER TABLE [dbo].[UserFieldSet]
ADD CONSTRAINT [FK_ResultUserField]
    FOREIGN KEY ([ResultId])
    REFERENCES [dbo].[ResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResultUserField'
CREATE INDEX [IX_FK_ResultUserField]
ON [dbo].[UserFieldSet]
    ([ResultId]);
GO

-- Creating foreign key on [WorkId] in table 'DefectSet'
ALTER TABLE [dbo].[DefectSet]
ADD CONSTRAINT [FK_WorkDefect]
    FOREIGN KEY ([WorkId])
    REFERENCES [dbo].[WorkSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkDefect'
CREATE INDEX [IX_FK_WorkDefect]
ON [dbo].[DefectSet]
    ([WorkId]);
GO

-- Creating foreign key on [SampleResultId] in table 'DefectSet'
ALTER TABLE [dbo].[DefectSet]
ADD CONSTRAINT [FK_SampleResultDefect]
    FOREIGN KEY ([SampleResultId])
    REFERENCES [dbo].[SampleResultSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SampleResultDefect'
CREATE INDEX [IX_FK_SampleResultDefect]
ON [dbo].[DefectSet]
    ([SampleResultId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
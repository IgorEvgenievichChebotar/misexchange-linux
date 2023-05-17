USE [MisExchangeCache]
GO

IF DB_NAME() <> N'MisExchangeCache' SET NOEXEC ON
GO


--
-- Изменить уровень локализации транзакции
--
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

--
-- Начать транзакцию
--
BEGIN TRANSACTION
GO

--
-- Создать таблицу [dbo].[PatientCards]
--
CREATE TABLE [dbo].[PatientCards] (
  [Id] [bigint] IDENTITY,
  [CardNr] [nvarchar](255) NULL,
  CONSTRAINT [PK_dbo.PatientCards] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Patients]
--
CREATE TABLE [dbo].[Patients] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [FirstName] [nvarchar](255) NULL,
  [LastName] [nvarchar](255) NULL,
  [MiddleName] [nvarchar](255) NULL,
  [BirthDay] [int] NULL,
  [BirthMonth] [int] NULL,
  [BirthYear] [int] NULL,
  [Sex] [int] NOT NULL,
  [Country] [nvarchar](255) NULL,
  [City] [nvarchar](255) NULL,
  [Street] [nvarchar](1024) NULL,
  [Building] [nvarchar](255) NULL,
  [Flat] [nvarchar](255) NULL,
  [InsuranceCompany] [nvarchar](1024) NULL,
  [PolicySeries] [nvarchar](255) NULL,
  [PolicyNumber] [nvarchar](255) NULL,
  [PatientCard_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Patients] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_PatientCard_Id] для объекта типа таблица [dbo].[Patients]
--
CREATE INDEX [IX_PatientCard_Id]
  ON [dbo].[Patients] ([PatientCard_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Patients_dbo.PatientCards_PatientCard_Id] для объекта типа таблица [dbo].[Patients]
--
ALTER TABLE [dbo].[Patients]
  ADD CONSTRAINT [FK_dbo.Patients_dbo.PatientCards_PatientCard_Id] FOREIGN KEY ([PatientCard_Id]) REFERENCES [dbo].[PatientCards] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Results]
--
CREATE TABLE [dbo].[Results] (
  [Id] [bigint] IDENTITY,
  [RequestCode] [nvarchar](255) NULL,
  [SampleDeliveryDate] [datetime] NULL,
  [HospitalCode] [nvarchar](255) NULL,
  [HospitalName] [nvarchar](1024) NULL,
  [State] [int] NOT NULL,
  [ExtraData] [nvarchar](max) NULL,
  [OrganizationCode] [nvarchar](255) NULL,
  [OrganizationName] [nvarchar](1024) NULL,
  [Email] [nvarchar](1024) NULL,
  [Telephone] [nvarchar](20) NULL,
  [Patient_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Results] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Patient_Id] для объекта типа таблица [dbo].[Results]
--
CREATE INDEX [IX_Patient_Id]
  ON [dbo].[Results] ([Patient_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Results_dbo.Patients_Patient_Id] для объекта типа таблица [dbo].[Results]
--
ALTER TABLE [dbo].[Results]
  ADD CONSTRAINT [FK_dbo.Results_dbo.Patients_Patient_Id] FOREIGN KEY ([Patient_Id]) REFERENCES [dbo].[Patients] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[SampleResults]
--
CREATE TABLE [dbo].[SampleResults] (
  [Id] [bigint] IDENTITY,
  [Barcode] [nvarchar](255) NULL,
  [BiomaterialCode] [nvarchar](255) NULL,
  [Comments] [nvarchar](max) NULL,
  [Result_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.SampleResults] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Result_Id] для объекта типа таблица [dbo].[SampleResults]
--
CREATE INDEX [IX_Result_Id]
  ON [dbo].[SampleResults] ([Result_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.SampleResults_dbo.Results_Result_Id] для объекта типа таблица [dbo].[SampleResults]
--
ALTER TABLE [dbo].[SampleResults]
  ADD CONSTRAINT [FK_dbo.SampleResults_dbo.Results_Result_Id] FOREIGN KEY ([Result_Id]) REFERENCES [dbo].[Results] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[TargetResults]
--
CREATE TABLE [dbo].[TargetResults] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Name] [nvarchar](1024) NULL,
  [Comments] [nvarchar](max) NULL,
  [SampleResult_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.TargetResults] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_SampleResult_Id] для объекта типа таблица [dbo].[TargetResults]
--
CREATE INDEX [IX_SampleResult_Id]
  ON [dbo].[TargetResults] ([SampleResult_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.TargetResults_dbo.SampleResults_SampleResult_Id] для объекта типа таблица [dbo].[TargetResults]
--
ALTER TABLE [dbo].[TargetResults]
  ADD CONSTRAINT [FK_dbo.TargetResults_dbo.SampleResults_SampleResult_Id] FOREIGN KEY ([SampleResult_Id]) REFERENCES [dbo].[SampleResults] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[MicroResults]
--
CREATE TABLE [dbo].[MicroResults] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Name] [nvarchar](1024) NULL,
  [Value] [nvarchar](1024) NULL,
  [Comments] [nvarchar](max) NULL,
  [Found] [bit] NULL,
  [ParentWork_Id] [bigint] NULL,
  [SampleResult_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.MicroResults] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_ParentWork_Id] для объекта типа таблица [dbo].[MicroResults]
--
CREATE INDEX [IX_ParentWork_Id]
  ON [dbo].[MicroResults] ([ParentWork_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_SampleResult_Id] для объекта типа таблица [dbo].[MicroResults]
--
CREATE INDEX [IX_SampleResult_Id]
  ON [dbo].[MicroResults] ([SampleResult_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.MicroResults_dbo.SampleResults_SampleResult_Id] для объекта типа таблица [dbo].[MicroResults]
--
ALTER TABLE [dbo].[MicroResults]
  ADD CONSTRAINT [FK_dbo.MicroResults_dbo.SampleResults_SampleResult_Id] FOREIGN KEY ([SampleResult_Id]) REFERENCES [dbo].[SampleResults] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[ResultObjectStatus]
--
CREATE TABLE [dbo].[ResultObjectStatus] (
  [Id] [bigint] IDENTITY,
  [StatusId] [int] NOT NULL,
  [CreateDate] [datetime] NOT NULL,
  [ModifyingDate] [datetime] NOT NULL,
  [Comment] [nvarchar](max) NULL,
  [Result_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.ResultObjectStatus] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Result_Id] для объекта типа таблица [dbo].[ResultObjectStatus]
--
CREATE INDEX [IX_Result_Id]
  ON [dbo].[ResultObjectStatus] ([Result_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.ResultObjectStatus_dbo.Results_Result_Id] для объекта типа таблица [dbo].[ResultObjectStatus]
--
ALTER TABLE [dbo].[ResultObjectStatus]
  ADD CONSTRAINT [FK_dbo.ResultObjectStatus_dbo.Results_Result_Id] FOREIGN KEY ([Result_Id]) REFERENCES [dbo].[Results] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Requests]
--
CREATE TABLE [dbo].[Requests] (
  [Id] [bigint] IDENTITY,
  [RequestCode] [nvarchar](255) NULL,
  [HospitalCode] [nvarchar](255) NULL,
  [HospitalName] [nvarchar](1024) NULL,
  [DepartmentCode] [nvarchar](255) NULL,
  [DepartmentName] [nvarchar](1024) NULL,
  [DoctorCode] [nvarchar](255) NULL,
  [DoctorName] [nvarchar](1024) NULL,
  [SamplingDate] [datetime] NULL,
  [SampleDeliveryDate] [datetime] NULL,
  [PregnancyDuration] [int] NULL,
  [CyclePeriod] [int] NULL,
  [ReadOnly] [bit] NULL,
  [Priority] [int] NULL,
  [PayCategoryCode] [nvarchar](255) NULL,
  [Email] [nvarchar](1024) NULL,
  [OrganizationCode] [nvarchar](255) NULL,
  [ExtraData] [nvarchar](max) NULL,
  [Telephone] [nvarchar](20) NULL,
  [CardAmbulatory] [nvarchar](128) NULL,
  [CardStationary] [nvarchar](128) NULL,
  [CardExtraType1] [nvarchar](128) NULL,
  [Patient_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Requests] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Patient_Id] для объекта типа таблица [dbo].[Requests]
--
CREATE INDEX [IX_Patient_Id]
  ON [dbo].[Requests] ([Patient_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Requests_dbo.Patients_Patient_Id] для объекта типа таблица [dbo].[Requests]
--
ALTER TABLE [dbo].[Requests]
  ADD CONSTRAINT [FK_dbo.Requests_dbo.Patients_Patient_Id] FOREIGN KEY ([Patient_Id]) REFERENCES [dbo].[Patients] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[UserFields]
--
CREATE TABLE [dbo].[UserFields] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Name] [nvarchar](1024) NULL,
  [Value] [nvarchar](2048) NULL,
  [PatientCard_Id] [bigint] NULL,
  [Patient_Id] [bigint] NULL,
  [Request_Id] [bigint] NULL,
  [Result_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.UserFields] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Patient_Id] для объекта типа таблица [dbo].[UserFields]
--
CREATE INDEX [IX_Patient_Id]
  ON [dbo].[UserFields] ([Patient_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_PatientCard_Id] для объекта типа таблица [dbo].[UserFields]
--
CREATE INDEX [IX_PatientCard_Id]
  ON [dbo].[UserFields] ([PatientCard_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Request_Id] для объекта типа таблица [dbo].[UserFields]
--
CREATE INDEX [IX_Request_Id]
  ON [dbo].[UserFields] ([Request_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Result_Id] для объекта типа таблица [dbo].[UserFields]
--
CREATE INDEX [IX_Result_Id]
  ON [dbo].[UserFields] ([Result_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.UserFields_dbo.PatientCards_PatientCard_Id] для объекта типа таблица [dbo].[UserFields]
--
ALTER TABLE [dbo].[UserFields]
  ADD CONSTRAINT [FK_dbo.UserFields_dbo.PatientCards_PatientCard_Id] FOREIGN KEY ([PatientCard_Id]) REFERENCES [dbo].[PatientCards] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.UserFields_dbo.Patients_Patient_Id] для объекта типа таблица [dbo].[UserFields]
--
ALTER TABLE [dbo].[UserFields]
  ADD CONSTRAINT [FK_dbo.UserFields_dbo.Patients_Patient_Id] FOREIGN KEY ([Patient_Id]) REFERENCES [dbo].[Patients] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.UserFields_dbo.Requests_Request_Id] для объекта типа таблица [dbo].[UserFields]
--
ALTER TABLE [dbo].[UserFields]
  ADD CONSTRAINT [FK_dbo.UserFields_dbo.Requests_Request_Id] FOREIGN KEY ([Request_Id]) REFERENCES [dbo].[Requests] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.UserFields_dbo.Results_Result_Id] для объекта типа таблица [dbo].[UserFields]
--
ALTER TABLE [dbo].[UserFields]
  ADD CONSTRAINT [FK_dbo.UserFields_dbo.Results_Result_Id] FOREIGN KEY ([Result_Id]) REFERENCES [dbo].[Results] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Samples]
--
CREATE TABLE [dbo].[Samples] (
  [Id] [bigint] IDENTITY,
  [Barcode] [nvarchar](255) NULL,
  [BiomaterialCode] [nvarchar](255) NULL,
  [Priority] [int] NULL,
  [Request_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Samples] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Request_Id] для объекта типа таблица [dbo].[Samples]
--
CREATE INDEX [IX_Request_Id]
  ON [dbo].[Samples] ([Request_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Samples_dbo.Requests_Request_Id] для объекта типа таблица [dbo].[Samples]
--
ALTER TABLE [dbo].[Samples]
  ADD CONSTRAINT [FK_dbo.Samples_dbo.Requests_Request_Id] FOREIGN KEY ([Request_Id]) REFERENCES [dbo].[Requests] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Targets]
--
CREATE TABLE [dbo].[Targets] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Priority] [int] NULL,
  [ReadOnly] [bit] NULL,
  [Sample_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Targets] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Sample_Id] для объекта типа таблица [dbo].[Targets]
--
CREATE INDEX [IX_Sample_Id]
  ON [dbo].[Targets] ([Sample_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Targets_dbo.Samples_Sample_Id] для объекта типа таблица [dbo].[Targets]
--
ALTER TABLE [dbo].[Targets]
  ADD CONSTRAINT [FK_dbo.Targets_dbo.Samples_Sample_Id] FOREIGN KEY ([Sample_Id]) REFERENCES [dbo].[Samples] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Tests]
--
CREATE TABLE [dbo].[Tests] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Target_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Tests] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Target_Id] для объекта типа таблица [dbo].[Tests]
--
CREATE INDEX [IX_Target_Id]
  ON [dbo].[Tests] ([Target_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Tests_dbo.Targets_Target_Id] для объекта типа таблица [dbo].[Tests]
--
ALTER TABLE [dbo].[Tests]
  ADD CONSTRAINT [FK_dbo.Tests_dbo.Targets_Target_Id] FOREIGN KEY ([Target_Id]) REFERENCES [dbo].[Targets] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[RequestObjectStatus]
--
CREATE TABLE [dbo].[RequestObjectStatus] (
  [Id] [bigint] IDENTITY,
  [StatusId] [int] NOT NULL,
  [CreateDate] [datetime] NOT NULL,
  [ModifyingDate] [datetime] NOT NULL,
  [Comment] [nvarchar](max) NULL,
  [Request_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.RequestObjectStatus] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Request_Id] для объекта типа таблица [dbo].[RequestObjectStatus]
--
CREATE INDEX [IX_Request_Id]
  ON [dbo].[RequestObjectStatus] ([Request_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.RequestObjectStatus_dbo.Requests_Request_Id] для объекта типа таблица [dbo].[RequestObjectStatus]
--
ALTER TABLE [dbo].[RequestObjectStatus]
  ADD CONSTRAINT [FK_dbo.RequestObjectStatus_dbo.Requests_Request_Id] FOREIGN KEY ([Request_Id]) REFERENCES [dbo].[Requests] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Norms]
--
CREATE TABLE [dbo].[Norms] (
  [Id] [bigint] IDENTITY,
  [LowLimit] [float] NULL,
  [HighLimit] [float] NULL,
  [CriticalLowLimit] [float] NULL,
  [CriticalHighLimit] [float] NULL,
  [Norms] [nvarchar](2048) NULL,
  [NormComment] [nvarchar](max) NULL,
  [UnitName] [nvarchar](255) NULL,
  CONSTRAINT [PK_dbo.Norms] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Works]
--
CREATE TABLE [dbo].[Works] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Name] [nvarchar](1024) NULL,
  [Value] [nvarchar](1024) NULL,
  [UnitName] [nvarchar](1024) NULL,
  [State] [int] NOT NULL,
  [Normality] [int] NOT NULL,
  [ApprovingDoctor] [nvarchar](2048) NULL,
  [ApprovingDoctorCode] [nvarchar](255) NULL,
  [CreateDate] [datetime] NOT NULL,
  [ApproveDate] [datetime] NULL,
  [ModifyDate] [datetime] NULL,
  [Comments] [nvarchar](max) NULL,
  [GroupRank] [int] NULL,
  [RankInGroup] [int] NULL,
  [TestRank] [int] NULL,
  [Precision] [int] NULL,
  [PatientGroupCode] [nvarchar](255) NULL,
  [Norm_Id] [bigint] NULL,
  [MicroResult_Id] [bigint] NULL,
  [TargetResult_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Works] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_MicroResult_Id] для объекта типа таблица [dbo].[Works]
--
CREATE INDEX [IX_MicroResult_Id]
  ON [dbo].[Works] ([MicroResult_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Norm_Id] для объекта типа таблица [dbo].[Works]
--
CREATE INDEX [IX_Norm_Id]
  ON [dbo].[Works] ([Norm_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_TargetResult_Id] для объекта типа таблица [dbo].[Works]
--
CREATE INDEX [IX_TargetResult_Id]
  ON [dbo].[Works] ([TargetResult_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Works_dbo.MicroResults_MicroResult_Id] для объекта типа таблица [dbo].[Works]
--
ALTER TABLE [dbo].[Works]
  ADD CONSTRAINT [FK_dbo.Works_dbo.MicroResults_MicroResult_Id] FOREIGN KEY ([MicroResult_Id]) REFERENCES [dbo].[MicroResults] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Works_dbo.Norms_Norm_Id] для объекта типа таблица [dbo].[Works]
--
ALTER TABLE [dbo].[Works]
  ADD CONSTRAINT [FK_dbo.Works_dbo.Norms_Norm_Id] FOREIGN KEY ([Norm_Id]) REFERENCES [dbo].[Norms] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Works_dbo.TargetResults_TargetResult_Id] для объекта типа таблица [dbo].[Works]
--
ALTER TABLE [dbo].[Works]
  ADD CONSTRAINT [FK_dbo.Works_dbo.TargetResults_TargetResult_Id] FOREIGN KEY ([TargetResult_Id]) REFERENCES [dbo].[TargetResults] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.MicroResults_dbo.Works_ParentWork_Id] для объекта типа таблица [dbo].[MicroResults]
--
ALTER TABLE [dbo].[MicroResults]
  ADD CONSTRAINT [FK_dbo.MicroResults_dbo.Works_ParentWork_Id] FOREIGN KEY ([ParentWork_Id]) REFERENCES [dbo].[Works] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Defects]
--
CREATE TABLE [dbo].[Defects] (
  [Id] [bigint] IDENTITY,
  [Code] [nvarchar](255) NULL,
  [Name] [nvarchar](1024) NULL,
  [Work_Id] [bigint] NULL,
  [SampleResult_Id] [bigint] NULL,
  CONSTRAINT [PK_dbo.Defects] PRIMARY KEY CLUSTERED ([Id]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_SampleResult_Id] для объекта типа таблица [dbo].[Defects]
--
CREATE INDEX [IX_SampleResult_Id]
  ON [dbo].[Defects] ([SampleResult_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать индекс [IX_Work_Id] для объекта типа таблица [dbo].[Defects]
--
CREATE INDEX [IX_Work_Id]
  ON [dbo].[Defects] ([Work_Id])
  WITH (FILLFACTOR = 90)
  ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Defects_dbo.SampleResults_SampleResult_Id] для объекта типа таблица [dbo].[Defects]
--
ALTER TABLE [dbo].[Defects]
  ADD CONSTRAINT [FK_dbo.Defects_dbo.SampleResults_SampleResult_Id] FOREIGN KEY ([SampleResult_Id]) REFERENCES [dbo].[SampleResults] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [FK_dbo.Defects_dbo.Works_Work_Id] для объекта типа таблица [dbo].[Defects]
--
ALTER TABLE [dbo].[Defects]
  ADD CONSTRAINT [FK_dbo.Defects_dbo.Works_Work_Id] FOREIGN KEY ([Work_Id]) REFERENCES [dbo].[Works] ([Id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[__MigrationHistory]
--
CREATE TABLE [dbo].[__MigrationHistory] (
  [MigrationId] [nvarchar](150) NOT NULL,
  [ContextKey] [nvarchar](300) NOT NULL,
  [Model] [varbinary](max) NOT NULL,
  [ProductVersion] [nvarchar](32) NOT NULL,
  CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey]) WITH (FILLFACTOR = 90)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Фиксировать транзакцию
--
IF @@TRANCOUNT>0 COMMIT TRANSACTION
GO

--
-- Установить NOEXEC в состояние off
--
SET NOEXEC OFF
GO
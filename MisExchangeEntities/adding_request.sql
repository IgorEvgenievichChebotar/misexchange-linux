BEGIN TRANSACTION

USE [MIS_Exchange_objects]
GO

DECLARE @PatientCardId int;
DECLARE @PatientId int;
DECLARE @RequestId int;
DECLARE @SampleId int;

-- Добавление карты пациента

INSERT INTO [dbo].[PatientCardSet]
           ([CardNr])
     VALUES
           ('1234')

SET @PatientCardId = (SELECT MAX(id) FROM PatientCardSet);

-- Добавление значения пользовательского поля карты пациента

INSERT INTO [dbo].[UserFieldSet]
           ([Code]
           ,[Value]
           ,[PatientCardId])
     VALUES
           ('House',
		   'Значение 1',
		   @PatientCardId)

-- Добавление пациента

INSERT INTO [dbo].[PatientSet]
           ([Code]
           ,[FirstName]
           ,[LastName]
           ,[MiddleName]
           ,[BirthDay]
           ,[BirthMonth]
           ,[BirthYear]
           ,[Sex]
           ,[Country]
           ,[City]
           ,[Street]
           ,[Building]
           ,[Flat]
           ,[InsuranceCompany]
           ,[PolicySeries]
           ,[PolicyNumber]
           ,[PatientCard_Id])
     VALUES
           ('7654321',
		   'Петр',
		   'Петров',
		   'Петрович',
		   1,
		   1,
		   1954,
		   1,
		   'Россия',
		   'Москва',
		   'Ленина',
		   '1/3',
		   '10',
		   'РГС',
		   '333',
		   '100 200 300',
		   @PatientCardId)
		   
SET @PatientId = (SELECT MAX(id) FROM PatientSet);
		   
-- Добавление заявки

INSERT INTO [dbo].[RequestSet]
           ([RequestCode]
           ,[HospitalCode]
           ,[HospitalName]
           ,[DepartmentCode]
           ,[DepartmentName]
           ,[DoctorCode]
           ,[DoctorName]
           ,[SamplingDate]
           ,[SampleDeliveryDate]
           ,[PregnancyDuration]
           ,[CyclePeriod]
           ,[PatientId])
     VALUES
           ('4021400502',
           '001',
		   'ЛПУ № 1',
		   '002',
		   'Травма',
		   '003',
		   'Иванов И. И.',
		   GETDATE(),
		   null,
		   0,
		   0,
		   @PatientId)
		   
SET @RequestId = (SELECT MAX(id) FROM RequestSet);

-- Добавление значения пользовательского поля заявки

INSERT INTO [dbo].[UserFieldSet]
           ([Code]
           ,[Value]
           ,[RequestId])
     VALUES
           ('House',
		   'Значение 1',
		   @RequestId)

-- Добавление пробы

INSERT INTO [dbo].[SampleSet]
           ([Barcode]
           ,[BiomaterialCode]
           ,[Priority]
           ,[RequestId])
     VALUES
           ('001',
           '57',
		   0,
		   @RequestId)
		   
SET @SampleId = (SELECT MAX(id) FROM SampleSet);
		   
-- Добавление информации об исследовании к пробе

INSERT INTO [dbo].[TargetSet]
           ([Code]
           ,[Priority]
           ,[SampleId])
     VALUES
           ('Leuconostoc',
		   0,
		   @SampleId)

-- Добавление записи статуса для новой заявки
-- Значения поля Status записи: 1 - новая, 2 - обработана, 3 - ошибка

INSERT INTO [dbo].[RequestStatusSet]
           ([CreateDate]
           ,[Status]
           ,[Request_Id])
     VALUES
           (GETDATE(),
           1,
           @RequestId)
GO

ROLLBACK TRANSACTION
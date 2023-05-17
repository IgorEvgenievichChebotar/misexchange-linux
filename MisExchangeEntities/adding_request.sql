BEGIN TRANSACTION

USE [MIS_Exchange_objects]
GO

DECLARE @PatientCardId int;
DECLARE @PatientId int;
DECLARE @RequestId int;
DECLARE @SampleId int;

-- ���������� ����� ��������

INSERT INTO [dbo].[PatientCardSet]
           ([CardNr])
     VALUES
           ('1234')

SET @PatientCardId = (SELECT MAX(id) FROM PatientCardSet);

-- ���������� �������� ����������������� ���� ����� ��������

INSERT INTO [dbo].[UserFieldSet]
           ([Code]
           ,[Value]
           ,[PatientCardId])
     VALUES
           ('House',
		   '�������� 1',
		   @PatientCardId)

-- ���������� ��������

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
		   '����',
		   '������',
		   '��������',
		   1,
		   1,
		   1954,
		   1,
		   '������',
		   '������',
		   '������',
		   '1/3',
		   '10',
		   '���',
		   '333',
		   '100 200 300',
		   @PatientCardId)
		   
SET @PatientId = (SELECT MAX(id) FROM PatientSet);
		   
-- ���������� ������

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
		   '��� � 1',
		   '002',
		   '������',
		   '003',
		   '������ �. �.',
		   GETDATE(),
		   null,
		   0,
		   0,
		   @PatientId)
		   
SET @RequestId = (SELECT MAX(id) FROM RequestSet);

-- ���������� �������� ����������������� ���� ������

INSERT INTO [dbo].[UserFieldSet]
           ([Code]
           ,[Value]
           ,[RequestId])
     VALUES
           ('House',
		   '�������� 1',
		   @RequestId)

-- ���������� �����

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
		   
-- ���������� ���������� �� ������������ � �����

INSERT INTO [dbo].[TargetSet]
           ([Code]
           ,[Priority]
           ,[SampleId])
     VALUES
           ('Leuconostoc',
		   0,
		   @SampleId)

-- ���������� ������ ������� ��� ����� ������
-- �������� ���� Status ������: 1 - �����, 2 - ����������, 3 - ������

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
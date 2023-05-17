--BEGIN TRANSACTION

DECLARE @RequestFilterId int;

INSERT INTO
  dbo.RequestFilterSet
(
  FirstName,
  LastName,
  MiddleName,
  PatientCodes,
  BirthDate,
  Sex,
  RequestCodes,
  DateFrom,
  DateTill,
  States,
  Priority,
  DefectState,
  CustomStates,
  Targets,
  Departments,
  Hospitals,
  CustDepartments,
  Doctors,
  EndDateFrom,
  EndDateTill
) 
VALUES (  
  'Кузя',
  'Кузьмин',
  'Кузьмич',
  '007,008',
  GETDATE(),
  1,
  '123,456',
  GETDATE(),
  GETDATE(),
  '1,2,3',
  1,
  1,
  '4,5,6',
  '7,8',
  '9,10',
  '11,12',
  '13,14',
  '15,16',
  GETDATE(),
  GETDATE()
);

SET @RequestFilterId = (SELECT MAX(id) FROM RequestFilterSet);

INSERT INTO 
  dbo.RequestFilterStatusSet
(
  CreateDate,
  Status,
  RequestFilter_Id
) 
VALUES (
  GETDATE(),
  1,
  @RequestFilterId
);

--ROLLBACK TRANSACTION
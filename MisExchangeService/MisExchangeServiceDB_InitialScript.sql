--
-- Начать транзакцию
--
BEGIN TRANSACTION
GO

--
-- Создать таблицу [dbo].[Request]
--
CREATE TABLE [dbo].[Request] (
  [id] [int] IDENTITY,
  [request_code] [varchar](64) NOT NULL,
  [lis_id] [int] NOT NULL,
  [create_date] [datetime] NOT NULL CONSTRAINT [DF__Request__create___00551192] DEFAULT (getdate()),
  [modify_date] [datetime] NOT NULL CONSTRAINT [DF__Request__modify___014935CB] DEFAULT (getdate()),
  CONSTRAINT [Request_pk] PRIMARY KEY CLUSTERED ([id]),
  CONSTRAINT [Request_uq] UNIQUE ([request_code])
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать триггер [Request_tru] на таблицу [dbo].[Request]
--
GO
CREATE TRIGGER [Request_tru] ON [dbo].[Request]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
  -- изменяем дату
  IF NOT update(modify_date)-- отсекаем явное(ручное) изменение
    UPDATE Request
	   SET modify_date = GetDate()
      FROM inserted i
      JOIN Request r ON (i.id = r.id)
END
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Request].[id] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Первичный ключ таблицы', 'SCHEMA', N'dbo', 'TABLE', N'Request', 'COLUMN', N'id'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Request].[request_code] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Код заявки', 'SCHEMA', N'dbo', 'TABLE', N'Request', 'COLUMN', N'request_code'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Request].[lis_id] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Идентификатор заявки в ЛИС', 'SCHEMA', N'dbo', 'TABLE', N'Request', 'COLUMN', N'lis_id'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Request].[create_date] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Дата создания записи', 'SCHEMA', N'dbo', 'TABLE', N'Request', 'COLUMN', N'create_date'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Request].[modify_date] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Дата последнего изменения записи', 'SCHEMA', N'dbo', 'TABLE', N'Request', 'COLUMN', N'modify_date'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать таблицу [dbo].[Sample]
--
CREATE TABLE [dbo].[Sample] (
  [id] [int] IDENTITY,
  [request_id] [int] NOT NULL,
  [barcode] [varchar](64) NOT NULL,
  [lis_id] [int] NOT NULL,
  [create_date] [datetime] NOT NULL DEFAULT (getdate()),
  [modify_date] [datetime] NOT NULL DEFAULT (getdate()),
  PRIMARY KEY CLUSTERED ([id]),
  CONSTRAINT [Sample_uq] UNIQUE ([barcode])
)
ON [PRIMARY]
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Создать внешний ключ [Sample_fk] для объекта типа таблица [dbo].[Sample]
--
ALTER TABLE [dbo].[Sample]
  ADD CONSTRAINT [Sample_fk] FOREIGN KEY ([request_id]) REFERENCES [dbo].[Request] ([id])
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[id] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Первичный ключ таблицы', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'id'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[request_id] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Внешний ключ на Request.id', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'request_id'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[barcode] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Штрих код(номер) пробы', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'barcode'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[lis_id] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Идентификатор пробы в ЛИС', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'lis_id'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[create_date] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Дата создания записи', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'create_date'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Добавить расширенное свойство [MS_Description] для [dbo].[Sample].[modify_date] (столбец)
--
EXEC sys.sp_addextendedproperty N'MS_Description', N'Дата последнего изменения записи', 'SCHEMA', N'dbo', 'TABLE', N'Sample', 'COLUMN', N'modify_date'
GO
IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
GO

--
-- Фиксировать транзакцию
--
IF @@TRANCOUNT>0 COMMIT TRANSACTION
GO
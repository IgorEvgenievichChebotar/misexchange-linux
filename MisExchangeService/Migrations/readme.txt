1) Добавить классу контекста конструктор по-умолчанию:

public EmiasMapperContext() : base(ConfigurationManager.ConnectionStrings["EmiasMapperConnectionStr"].ConnectionString) { }

Строка подключения должна быть определена в файле app.config

2) EntityFramework\Enable-Migrations -MigrationsDirectory "Migrations\FederalServiceDLI" -ContextTypeName ru.novolabs.MisExchange.ExchangeHelpers.REGIZ_Spb.REGIZDependencies.MapperItemClasses.RegizMapperContext -EnableAutomaticMigrations

3) Переименовать сгенерированный класс Configuration в ConfigurationFederalServiceDLI (в папке "Migrations\FederalServiceDLI")

4) Добавление новой миграции:
	
	EntityFramework\Add-Migration AddMicroorganismMapping -ConfigurationTypeName ConfigurationFederalServiceDLI   (EntityFramework\Add-Migration migration_name)

5) При наличии в пункте 2 опции -EnableAutomaticMigrations в классе "ConfigurationXXX" устанавливается AutomaticMigrationsEnabled в "true"

ConfigurationFederalServiceDLI()
        {
            AutomaticMigrationsEnabled = true;
			.....

6) В конструкторе класса "ExchangeHelperXXX" необходимо в коде настроить инициализатор, "дотягивающий" базу данных до последней версии:

Database.SetInitializer(new MigrateDatabaseToLatestVersion<RegizMapperContext, ConfigurationFederalServiceDLI>());

===== Необязательно ===============================================

7) Ручное удаление миграций:

EntityFramework\Remove-Migration

8) Ручное применение миграций:

EntityFramework\Update-Database -ConfigurationTypeName ConfigurationFederalServiceDLI
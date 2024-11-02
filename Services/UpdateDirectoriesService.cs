using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Services
{
    // Данная служба предназначена для обновления всех справочников каждые три часа
    public class UpdateDirectoriesService(IServiceScopeFactory serviceScopeFactory)
        : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(45), stoppingToken); // Задержка при запуске сервиса

            while (!stoppingToken.IsCancellationRequested)
            {
                WriteLog.Debug("Инициализировано обновление справочников...");

                await UpdateEmployeeRoles();
                await UpdateEmployees();                
                await UpdateEmployeeGroups();
                await UpdateEmployeeGroupsConnect();
                await UpdateKinds();
                await UpdateKindParameters();
                await UpdateManufacturerers();
                await UpdateModels();
                await UpdateCategories();
                await UpdateCompanies();
                await UpdateMaintenanceEntities();
                await UpdateIssueDictionaries();
                await UpdateIssues();
                await UpdateTimeEntries();
                await UpdateEquipments();

                WriteLog.Debug("Обновление справочников успешно завершено!");

                TimeSpan remaining = DateTime.Now.AddHours(3) - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }

        async Task UpdateEmployeeRoles()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IRoleRepository employeeRoleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
            // Обновление через обычное API т.к. в SQL API нет метода на получение ролей
            await employeeRoleRepository.GetRolesFromOkdesk();
        }

        async Task UpdateEmployees()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IEmployeeRepository employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            // Обновление через обычное API т.к. в SQL API нет связей с ролями и таблица many to many не заполняется
            await employeeRepository.UpdateEmployeesFromAPIOkdesk();
        }

        async Task UpdateEmployeeGroups()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IGroupRepository employeeGroupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();
            await employeeGroupRepository.GetGroupsFromDBOkdesk();
        }

        async Task UpdateEmployeeGroupsConnect()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IEmployeeGroupsRepository employeeGroupsRepository = scope.ServiceProvider.GetRequiredService<IEmployeeGroupsRepository>();
            await employeeGroupsRepository.UpdateEmployeeGroupsFromDBOkdesk();
        }

        async Task UpdateKindParameters()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IKindParameterRepository kindParameterRepository = scope.ServiceProvider.GetRequiredService<IKindParameterRepository>();
            // Обновление через обычное API, а не через SQL API т.к. в во втором пока что нет возможности получить связи между kind и kind_parameters
            // Сделал через SQL API, добавили нужную таблицу! 02.11.2024
            await kindParameterRepository.UpdateKindParametersFromDBOkdesk();
        }

        async Task UpdateKinds()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IKindRepository kindRepository = scope.ServiceProvider.GetRequiredService<IKindRepository>();
            await kindRepository.UpdateKindsFromAPIOkdesk();
        }

        async Task UpdateManufacturerers()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IManufacturerRepository manufacturerRepository = scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
            await manufacturerRepository.UpdateManufacturersFromDBOkdesk();
        }

        async Task UpdateModels()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IModelRepository modelRepository = scope.ServiceProvider.GetRequiredService<IModelRepository>();
            await modelRepository.UpdatetModelsFromDBOkdesk();
        }

        async Task UpdateCategories()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ICategoryRepository categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
            await categoryRepository.UpdateCategoriesFromDBOkdesk();
        }

        async Task UpdateCompanies()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ICompanyRepository companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            // Обновление через обычное API т.к. в SQL API нельзя получить цвет категорий
            await companyRepository.UpdateCompaniesFromAPIOkdesk();
        }

        async Task UpdateMaintenanceEntities()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IMaintenanceEntityRepository maintenanceEntityRepository = scope.ServiceProvider.GetRequiredService<IMaintenanceEntityRepository>();
            await maintenanceEntityRepository.UpdateMaintenanceEntitiesFromDBOkdesk();
        }

        async Task UpdateIssueDictionaries()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IIssueRepository issueRepository = scope.ServiceProvider.GetRequiredService<IIssueRepository>();
            await issueRepository.UpdateIssueDictionaryFromDB();
        }

        async Task UpdateIssues()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IIssueRepository issueRepository = scope.ServiceProvider.GetRequiredService<IIssueRepository>();

            DateTime now = DateTime.Now;
            DateTime dateFrom = new(now.Year, now.Month, now.Day, hour: 0, minute: 0, second: 0);
            DateTime dateTo = new(now.Year, now.Month, now.Day, hour: 23, minute: 59, second: 59);
            await issueRepository.UpdateIssuesFromDBOkdesk(dateFrom.AddDays(-1), dateTo);
        }

        async Task UpdateTimeEntries()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ITimeEntryRepository timeEntryRepository = scope.ServiceProvider.GetRequiredService<ITimeEntryRepository>();

            DateTime now = DateTime.Now;
            DateTime dateFrom = new(now.Year, now.Month, now.Day, hour: 0, minute: 0, second: 0);
            DateTime dateTo = new(now.Year, now.Month, now.Day, hour: 23, minute: 59, second: 59);
            await timeEntryRepository.UpdateTimeEntryFromDBOkdesk(dateFrom.AddDays(-1), dateTo);
        }

        async Task UpdateEquipments()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IEquipmentRepository equipmentRepository = scope.ServiceProvider.GetRequiredService<IEquipmentRepository>();
            await equipmentRepository.UpdateEquipmentsFromDBOkdesk();
        }
    }
}
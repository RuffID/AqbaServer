using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Services
{
    // Данная служба предназначена для получения данных за предыдущий в день на случай если вчерашние данные были изменены
    public class UpdateDirectoriesService(IServiceScopeFactory serviceScopeFactory)
        : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1200000, stoppingToken); // Задержка при запуске сервиса

            while (!stoppingToken.IsCancellationRequested)
            {

                await UpdateKind();
                await UpdateManufacturer();
                await UpdateModel();
                await UpdateEmployeeRole();
                await UpdateEmployee();
                await UpdateEmployeeGroup();
                await UpdateCompany();
                await UpdateMaintenanceEntity();
                await UpdateEquipment();

                TimeSpan remaining = DateTime.Now.AddHours(3) - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }
        

        async Task UpdateKind()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IKindRepository kindRepository = scope.ServiceProvider.GetRequiredService<IKindRepository>();
            await kindRepository.GetKindsFromOkdesk();
        }

        async Task UpdateManufacturer()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IManufacturerRepository manufacturerRepository = scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
            await manufacturerRepository.GetManufacturersFromOkdesk();
        }

        async Task UpdateModel()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IModelRepository modelRepository = scope.ServiceProvider.GetRequiredService<IModelRepository>();
            await modelRepository.GetModelsFromOkdesk();
        }

        async Task UpdateEmployee()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IEmployeeRepository employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            await employeeRepository.GetEmployeesFromOkdesk();
        }

        async Task UpdateEmployeeGroup()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IGroupRepository employeeGroupRepository = scope.ServiceProvider.GetRequiredService<IGroupRepository>();
            await employeeGroupRepository.GetGroupsFromOkdesk();
        }

        async Task UpdateEmployeeRole()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IRoleRepository employeeRoleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
            await employeeRoleRepository.GetRolesFromOkdesk();
        }

        async Task UpdateCompany()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            ICompanyRepository companyRepository = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            int? lastCompanyId = await companyRepository.GetLastCompanyId();
            if (lastCompanyId != null)
                await companyRepository.GetCompaniesFromOkdesk((int)lastCompanyId);
        }

        async Task UpdateMaintenanceEntity()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IMaintenanceEntityRepository maintenanceEntityRepository = scope.ServiceProvider.GetRequiredService<IMaintenanceEntityRepository>();
            int? lastMaintenanceEntityId = await maintenanceEntityRepository.GetLastMaintenanceEntitiyId();
            if (lastMaintenanceEntityId != null)
                await maintenanceEntityRepository.GetMaintenanceEntitiesFromOkdesk((int)lastMaintenanceEntityId);
        }

        async Task UpdateEquipment()
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IEquipmentRepository equipmentRepository = scope.ServiceProvider.GetRequiredService<IEquipmentRepository>();
            int? lastEquipmentId = await equipmentRepository.GetLastEquipmentId();
            if (lastEquipmentId != null)
                await equipmentRepository.GetEquipmentsFromOkdesk((int)lastEquipmentId);
        }
    }
}
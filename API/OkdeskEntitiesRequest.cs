using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.API
{
    public static class OkdeskEntitiesRequest
    {
        public static async Task<List<Company>> GetCompanies(ICollection<Category> categories, int pageSize, int lastCompanyId = 0)
        {
            List<Company> companies = [];

            Company[]? response;

            foreach (var category in categories)
            {
                while (true)
                {
                    // Получение списка компаний через API
                    response = await Request.GetСompanies(lastCompanyId, Convert.ToInt32(category.Id), pageSize);

                    // Если список компаний не пуст, то находит id последнего клиента чтобы в следующей итерации получить следующие 100 компаний
                    if (response?.Length > 0)
                        lastCompanyId = response[response.Length - 1].Id + 1;

                    // Если получен список с 0 значениями, то прекращает API запросы
                    if (response == null || response.Length <= 0)
                        break;
                    else
                        // Если получен не пустой список, то добавляет поочерёдно каждую компанию в список
                        foreach (var company in response)
                        {
                            company.Category = category;
                            companies.Add(company);
                        }
                }
                lastCompanyId = 0;
            }
            return companies;
        }

        public static async Task<ICollection<MaintenanceEntity>> GetMaintenanceEntities(int pageSize, int lastMaintenanceEntitiesId = 0)
        {
            List<MaintenanceEntity> maintenanceEntities = [];
            MaintenanceEntity[]? response;

            while (true)
            {
                if (maintenanceEntities.Count > 0)
                    lastMaintenanceEntitiesId = maintenanceEntities[^1].Id + 1;

                response = await Request.GetMaintenanceEntities(lastMaintenanceEntitiesId, pageSize);

                if (response == null || response.Length <= 0)
                    break;
                else
                    foreach (var obj in response)
                        maintenanceEntities.Add(obj);

                if (pageSize == 1) break;
            }
            return maintenanceEntities;
        }

        public static async Task<ICollection<Equipment>> GetEquipments(int equipmentId = 0, int pageSize = 100)
        {

            List<Equipment> equipments = [];
            Equipment[]? responseEquipment;
            int counter = 0;

            while (true)
            {
                if (equipments.Count > 0)
                    equipmentId = equipments[^1].Id + 1;

                responseEquipment = await Request.GetEquipments(equipmentId, pageSize);

                if (responseEquipment == null || responseEquipment.Length <= 0)
                    break;
                else
                    foreach (var equip in responseEquipment)
                        equipments.Add(equip);

                if (pageSize >= ++counter)
                    break;
            }
            return equipments;
        }

        public static async Task<ICollection<Manufacturer>> GetManufacturers()
        {
            List<Manufacturer> manufacturers = [];

            long lastManufacturerId = 0;
            Manufacturer[]? responseManufacturers;

            while (true)
            {
                if (manufacturers.Count > 0)
                    lastManufacturerId = manufacturers[^1].Id + 1;

                responseManufacturers = await Request.GetManufacturers(lastManufacturerId);

                if (responseManufacturers == null || responseManufacturers.Length <= 0)
                    break;
                else
                    foreach (var manufacturer in responseManufacturers)
                        manufacturers.Add(manufacturer);
            }
            return manufacturers;
        }

        public static async Task<ICollection<Model>> GetModels()
        {
            List<Model> models = [];

            long lastEquipmentParameterId = 0;
            Model[]? responseEquipmentParameter;

            while (true)
            {
                if (models.Count > 0)
                    lastEquipmentParameterId = models[models.Count - 1].Id + 1;

                responseEquipmentParameter = await Request.GetModels(lastEquipmentParameterId);

                if (responseEquipmentParameter == null || responseEquipmentParameter.Length <= 0)
                    break;
                else
                    foreach (var equip in responseEquipmentParameter)
                        models.Add(equip);
            }
            return models;
        }

        public static async Task<ICollection<Kind>> GetKinds()
        {
            List<Kind> kinds = [];

            long lastKindId = 0;
            Kind[]? responseKinds;

            while (true)
            {
                if (kinds.Count > 0)
                    lastKindId = kinds[kinds.Count - 1].Id + 1;

                responseKinds = await Request.GetKinds(lastKindId);

                if (responseKinds == null || responseKinds.Length <= 0)
                    break;
                else
                    foreach (var equip in responseKinds)
                        kinds.Add(equip);
            }
            return kinds;
        }

        public static ICollection<Category> GetCategories()
        {
            ICollection<Category> categories =
            [
                new(0, "#FFFFFF"),
                new(1, "#FFEE00"),
                new(2, "#00FF00"),
                new(3, "#FF0000"),
                new(4, "#FF00D5"),
                new(5, "#000000"),
                new(6, "#5CB85C"),
                new(7, "#D3F720"),
                new(8, "#6C89EB"),
                new(9, "#08C7F7"),
                new(10, "#0011FF"),
                new(11, "#9E9E9E"),
                new(12, "#FF7300")
            ];
            return categories;
        }

        public static async Task<ICollection<Employee>> GetEmployees()
        {
            List<Employee> employees = [];

            long lastEmployeeId = 0;
            Employee[]? responseEmployees;

            while (true)
            {
                if (employees.Count > 0)
                    lastEmployeeId = employees[employees.Count - 1].Id + 1;

                responseEmployees = await Request.GetEmployees(lastEmployeeId);

                if (responseEmployees == null || responseEmployees.Length <= 0)
                    break;
                else
                    foreach (var equip in responseEmployees)
                        employees.Add(equip);
            }
            return employees;
        }
    }
}
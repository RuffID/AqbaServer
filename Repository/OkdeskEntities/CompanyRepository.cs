using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMaintenanceEntityRepository _maintenanceEntityRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        public CompanyRepository(ICategoryRepository categoryRepository, IMaintenanceEntityRepository maintenanceEntityRepository, IEquipmentRepository equipmentRepository)
        {
            _categoryRepository = categoryRepository;
            _maintenanceEntityRepository = maintenanceEntityRepository;
            _equipmentRepository = equipmentRepository;
        }

        public async Task<bool> CreateCompany(string? categoryCode, Company? companyMap)
        {
            if (string.IsNullOrEmpty(categoryCode) || companyMap == null)
            {
                Console.WriteLine($"Method: {nameof(CreateCompany)}] Category code or company is null");
                return false;
            }
            var category = await _categoryRepository.GetCategory(categoryCode);

            if (category == null)
            {
                Console.WriteLine($"Method: {nameof(CreateCompany)}] Unable to find category");
                return false;
            }

            return await DBInsert.InsertCompany(category.Id, companyMap);
        }

        public async Task<bool> DeleteCompany(int companyId)
        {
            return await DBDelete.DeleteCompany(companyId);
        }

        public async Task<ICollection<Company>?> GetCompanies(int companyId)
        {
            return await DBSelect.SelectCompanies(companyId);
        }

        public async Task<int?> GetLastCompanyId()
        {
            return await DBSelect.SelectLastCompany();
        }

        public async Task<ICollection<Company>?> GetCompaniesByCategory(string categoryCode, int companyId)
        {
            return await DBSelect.SelectCompaniesByCategory(categoryCode, companyId);
        }
        
        public async Task<bool> UpdateCompanyFromOkdesk(int companyId)
        {
            var company = await Request.GetСompany(companyId);

            if (company == null) return false;

            return await UpdateCompany(companyId, company);
        }

        public async Task<(Company? company, ICollection<MaintenanceEntity>? objects, ICollection<Equipment>? equipments)> GetUpdatingCompany(int companyId)
        {
            if (!await UpdateCompanyFromOkdesk(companyId)) return (null, null, null);

            var company = await GetCompany(companyId);

            if (!await _maintenanceEntityRepository.UpdateMaintenanceEntitiesFromOkdesk(companyId: companyId)) return (null, null, null);

            var objects = await _maintenanceEntityRepository.GetMaintenanceEntitiesByCompany(companyId);

            if (!await _equipmentRepository.UpdateEquipmentsFromAPIOkdesk(companyId: companyId)) return (null, null, null);

            var equipments = await _equipmentRepository.GetEquipmentsByCompany(companyId);

            return (company, objects, equipments);
        }

        public async Task<bool> UpdateCompaniesFromAPIOkdesk(int lastCompanyId = 0, int pageSize = 100)
        {
            var categories = await _categoryRepository.GetCategories();
            if (categories == null)
            {
                WriteLog.Debug($"[Method: {nameof(UpdateCompaniesFromAPIOkdesk)}] Cannot get categories from DB");
                return false;
            }
            var companies = await OkdeskEntitiesRequest.GetCompanies(categories, pageSize, lastCompanyId);

            return await SaveOrUpdateInDB(companies);
        }

        public async Task<Company?> GetCompany(int companyId)
        {
            return await DBSelect.SelectCompany(companyId);
        }

        public async Task<bool> UpdateCompany(int companyId, Company? company)
        {
            if (company == null) return false;
            return await DBUpdate.UpdateCompany(companyId, company);
        }

        public async Task<bool> UpdateCompaniesFromDBOkdesk()
        {
            var companies = await PGSelect.SelectCompanies();

            return await SaveOrUpdateInDB(companies);
        }
        
        async Task<bool> SaveOrUpdateInDB(ICollection<Company>? companies)
        {
            if (companies == null || companies.Count <= 0)
            {
                Console.WriteLine($"Method: {nameof(SaveOrUpdateInDB)}] Не удалось получить компании из API");
                return false;
            }

            foreach (var company in companies)
            {
                var tempComp = await GetCompany(company.Id);

                if (tempComp == null)
                {
                    if (company.Category == null || string.IsNullOrEmpty(company.Category?.Code))
                    {
                        company.Category = new Category(0, "Без категории", color: "#FFFFFF", code: "no_category");
                    }

                    if (!await CreateCompany(company.Category.Code, company))
                    {
                        Console.WriteLine($"Method: {nameof(SaveOrUpdateInDB)}] Не удалось создать компанию {company?.Id}");
                        return false;
                    }
                }

                if (tempComp != null)
                {
                    if (!await UpdateCompany(tempComp.Id, company))
                    {
                        Console.WriteLine($"Method: {nameof(SaveOrUpdateInDB)}] Не удалось обновить компанию {company?.Id}");
                        return false;                
                    }
                }
            }
            return true;
        }
    }
}

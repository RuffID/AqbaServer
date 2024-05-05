using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface ICompanyRepository
    {
        Task<ICollection<Company>?> GetCompaniesByCategory(string categoryCode, int companyId);
        Task<Company?> GetCompany(int companyId);
        Task<int?> GetLastCompanyId();
        Task<ICollection<Company>?> GetCompanies(int companyId);
        Task<bool> UpdateCompanyFromOkdesk(int companyId);
        Task<bool> UpdateCompaniesFromAPIOkdesk(int lastCompanyId = 0, int pageSize = 100);
        Task<bool> CreateCompany(string? categoryCode, Company? companyMap);
        Task<bool> UpdateCompany(int categoryId, Company? company);
        Task<bool> DeleteCompany(int companyId);
        Task<bool> UpdateCompaniesFromDBOkdesk();
        Task<(Company? company, ICollection<MaintenanceEntity>? objects, ICollection<Equipment>? equipments)> GetUpdatingCompany(int companyId);
    }
}
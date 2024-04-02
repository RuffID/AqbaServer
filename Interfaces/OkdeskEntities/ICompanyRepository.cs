using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface ICompanyRepository
    {
        Task<ICollection<Company>?> GetCompaniesByCategory(int categoryId, int companyId);
        Task<Company?> GetCompany(int companyId);
        Task<int?> GetLastCompanyId();
        Task<ICollection<Company>?> GetCompanies(int companyId);
        Task<bool> GetCompanyFromOkdesk(int companyId);
        Task<bool> GetCompaniesFromOkdesk(int lastCompanyId = 0, int pageSize = 100);
        Task<bool> CreateCompany(int categoryId, Company companyMap);
        Task<bool> UpdateCompany(int categoryId, Company company);
        Task<bool> DeleteCompany(int companyId);
    }
}
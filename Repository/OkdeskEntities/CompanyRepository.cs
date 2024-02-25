using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        public CompanyRepository(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> CreateCompany(int categoryId, Company companyMap)
        {
            return await DBInsert.InsertCompany(categoryId, companyMap);
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

        public async Task<ICollection<Company>?> GetCompaniesByCategory(int categoryId, int companyId)
        {
            return await DBSelect.SelectCompaniesByCategory(categoryId, companyId);
        }

        public async Task<bool> GetCompaniesFromOkdesk(int lastCompanyId = 0)
        {
            var categories = await _categoryRepository.GetCategories();
            if (categories == null) return false;
            var companies = await OkdeskEntitiesRequest.GetCompanies(categories, lastCompanyId);
            if (companies == null) return false;

            foreach (var company in companies)
            {
                var tempComp = await GetCompany(company.Id);
                if (tempComp == null)
                    if (!await CreateCompany(company.Category.Id, company))
                        return false;

                if (tempComp != null)
                    if (!await UpdateCompany(tempComp.Id, company))
                        return false;
            }
            return true;
        }

        public async Task<Company?> GetCompany(int companyId)
        {
            return await DBSelect.SelectCompany(companyId);
        }

        public async Task<bool> UpdateCompany(int categoryId, Company company)
        {
            return await DBUpdate.UpdateCompany(categoryId, company);
        }
    }
}

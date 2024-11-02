using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.WebHook;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.WebHook
{
    public class CompanyWebHookRepository : ICompanyWebHookRepository
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyWebHookRepository(ICompanyRepository companyRepository) 
        {
            _companyRepository = companyRepository;
        }
        
        public async Task ChangeCompany(Company? company)
        {
            if (company?.Id == null || company.Id == 0) return;

            await _companyRepository.UpdateCompany(company.Id, company);
            WriteLog.Debug($"[Method: {nameof(ChangeCompany)}] Компания {company.Id} была обновлена");
        }

        public async Task NewCompany(Company? company)
        {
            if (company == null || company?.Id == null || company.Id == 0) return;

            if (company.Category == null || string.IsNullOrEmpty( company.Category?.Code))
            {
                company.Category = new Category(0, "Без категории", color: "#FFFFFF", code: "no_category");
            }

            await _companyRepository.CreateCompany(company.Category.Code, company);
            WriteLog.Debug($"[Method: {nameof(ChangeCompany)}] Компания {company.Id} была создана");
        }
    }
}
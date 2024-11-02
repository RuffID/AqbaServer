using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.WebHook
{
    public interface ICompanyWebHookRepository
    {
        Task ChangeCompany(Company? company);
        Task NewCompany(Company? company);
    }
}
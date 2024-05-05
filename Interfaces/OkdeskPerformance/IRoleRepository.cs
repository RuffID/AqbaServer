using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IRoleRepository
    {
        Task<Role?> GetRole(Role? role);
        Task<bool> CreateRole(Role? role);
        Task<bool> UpdateRole(string? roleName, Role? role);
        Task<bool> GetRolesFromOkdesk();
    }
}
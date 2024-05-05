using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class RoleRepository : IRoleRepository
    {
        public async Task<Role?> GetRole(Role? role)
        {
            if (role == null) return null;

            return await DBSelect.SelectRole(role?.Name);
        }

        public async Task<bool> CreateRole(Role? role)
        {
            if (role == null) return false;

            return await DBInsert.InsertRole(role);
        }

        public async Task<bool> UpdateRole(string? roleName, Role? role)
        {
            if (string.IsNullOrEmpty(roleName) || role == null) return false;

            return await DBUpdate.UpdateRole(roleName, role);
        }

        public async Task<bool> GetRolesFromOkdesk()
        {
            var roles = await Request.GetRoles();
            if (roles == null || roles.Length <= 0)
            {
                WriteLog.Warn("null при получении roles с окдеска");
                return false;
            }

            foreach (var role in roles)
            {
                var tempRole = await GetRole(role);

                if (tempRole == null)
                {
                    if (!await CreateRole(role))
                        return false;
                }
                else if (tempRole != null)
                {
                    if (!await UpdateRole(tempRole?.Name, role))
                        return false;
                }
            }
            return true;
        }
    }
}

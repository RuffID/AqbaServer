using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class RoleRepository : IRoleRepository
    {
        public async Task<Role?> GetRole(Role role)
        {
            return await DBSelect.SelectRole(role?.Name);
        }

        public async Task<bool> CreateRole(Role role)
        {
            return await DBInsert.InsertRole(role);
        }

        public async Task<bool> UpdateRole(string roleName, Role role)
        {
            return await DBUpdate.UpdateRole(roleName, role);
        }

        public async Task<bool> GetRolesFromOkdesk()
        {
            var roles = await Request.GetRoles();
            if (roles == null || roles.Length <= 0)
            {
                WriteLog.Warn("null при получении groups с окдеска");
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
                    if (!await UpdateRole(tempRole.Name, role))
                        return false;
                }
            }
            return true;
        }
    }
}

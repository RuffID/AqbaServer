using AqbaServer.Data.MySql;
using AqbaServer.Interfaces.Authorization;

namespace AqbaServer.Repository.Authorization
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public async Task<int?> GetUserRole(string roleName)
        {
            return await DBSelect.SelectUserRole(roleName);
        }       
    }
}

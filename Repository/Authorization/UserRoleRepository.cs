using AqbaServer.Data;
using AqbaServer.Interfaces.Authorization;
using AqbaServer.Models.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

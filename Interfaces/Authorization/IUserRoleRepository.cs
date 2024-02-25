namespace AqbaServer.Interfaces.Authorization
{
    public interface IUserRoleRepository
    {
        Task<int?> GetUserRole(string roleName);
    }
}

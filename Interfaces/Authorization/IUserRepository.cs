using AqbaServer.Models.Authorization;

namespace AqbaServer.Interfaces.Authorization
{
    public interface IUserRepository
    {
        Task<User?> GetUser(string userEmail);
        Task<ICollection<User>?> GetUsers();
        Task<string?> GetUserRole(string apiKey);
        Task<bool> CreateUser(User user);
        string? GenerateToken(User user);
        string GenerateRefreshToken();
        Task<bool> UpdateRefreshToken(int userId, string refreshToken, DateTime expirationNewRefreshToken);
        Task<bool> UpdateUser(User userUpdate);
        Task<bool> DeleteUser(int id);
        Task<User?> GetUserByRefreshToken(string refreshToken);
    }
}

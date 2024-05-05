using AqbaServer.Authorization;
using AqbaServer.Data.MySql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.Authorization;
using AqbaServer.Models.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AqbaServer.Repository.Authorization
{
    public class UserRepository : IUserRepository
    {
        public async Task<User?> GetUser(string userEmail)
        {
            return await DBSelect.SelectUser(userEmail);
        }

        public async Task<ICollection<User>?> GetUsers()
        {
            return await DBSelect.SelectUsers();
        }

        public async Task<string?> GetUserRole(string apiKey)
        {
            return await DBSelect.SelectUserRoles(apiKey);
        }

        public async Task<bool> CreateUser(User user)
        {
            return await DBInsert.InsertUser(user);
        }

        public async Task<bool> UpdateRefreshToken(int userId, string refreshToken, DateTime expirationNewRefreshToken)
        {
            return await DBUpdate.UpdateRefreshToken(userId, refreshToken, expirationNewRefreshToken);
        }

        public async Task<bool> UpdateUser(User userUpdate)
        {
            return await DBUpdate.UpdateUser(userUpdate);
        }

        public string? GenerateToken(User user)
        {
            if (user.Email == null || user.Role == null) { return null; }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AuthOptions.ISSUER,
                Audience = AuthOptions.AUDIENCE,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Config.TokenLifeTimeFromMinutes),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string userToken = tokenHandler.WriteToken(token);
            return userToken;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));            
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await DBDelete.DeleteUser(id);
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            return await DBSelect.SelectUserByRefreshToken(refreshToken);
        }
    }
}

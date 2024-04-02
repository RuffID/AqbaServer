using AqbaServer.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AqbaServer.Authorization
{
    public class AuthOptions
    {
        public const string ISSUER = "AqbaServer"; // издатель токена
        public const string AUDIENCE = "AqbaClient"; // потребитель токена
        public static SymmetricSecurityKey? GetSymmetricSecurityKey()
        {
            if (string.IsNullOrEmpty(Config.JwtKey))
            {
                WriteLog.Error("Please fill out the config to get started. JWT key is missing.");
                return null;
            }
            return new(Encoding.UTF8.GetBytes(Config.JwtKey));
        }
    }
}

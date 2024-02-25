using AqbaServer.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AqbaServer.Authorization
{
    public class AuthOptions
    {
        public const string ISSUER = "AqbaServer"; // издатель токена
        public const string AUDIENCE = "AqbaClient"; // потребитель токена
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new(Encoding.UTF8.GetBytes(Config.JwtKey));
    }
}

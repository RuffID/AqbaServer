using System.Text.Json.Serialization;

namespace AqbaServer.Models.Authorization
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime TokenExpires { get; set; }
    }
}

using Newtonsoft.Json;

namespace AqbaServer.Dto
{
    public class ConfigDto
    {
        public string? OkdeskApiToken { get; set; }
        public string? OkdeskDomainLink { get; set; }
        public string? OkdeskLogin { get; set; }
        public string? OkdeskPassword { get; set; }
        public string? PartnersLogin { get; set; }
        public string? PartnersPassword { get; set; }
        public string? ConnectionString { get; set; }
        public string? JwtKey { get; set; }
        public int TokenLifeTimeFromMinutes { get; set; }
        public int RefreshTokenLifeTimeFromDays { get; set; }
        public string? SMTPEmail { get; set; }
        public string? SMTPPAssword { get; set; }
    }
}

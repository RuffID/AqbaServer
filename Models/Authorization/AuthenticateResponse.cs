namespace AqbaServer.Models.Authorization
{
    public class AuthenticateResponse(string apiKey)
    {
        public string ApiKey { get; set; } = apiKey;
    }
}

namespace AqbaServer.Helper
{
    public static class Immutable
    {
        public static readonly string OkdeskApiLink  = Config.OkdeskDomainLink + "/api/v1";
        public static readonly string OkdeskLoginLink = Config.OkdeskDomainLink + "/users/sign_in";
        public static readonly string OkdeskContentForLoginOnSite = $"utf8=✓&user[login]={Config.OkdeskLogin}&user[password]={Config.OkdeskPassword}&user[remember_me]=1";
        public static readonly string PartnersContentForLoginOnSite = $"username={Config.PartnersLogin}&password={Config.PartnersPassword}&rememberme=on&service=login&Login=Sign+in";
        public const string PARTNERS_LINK = "https://partners.iiko.ru/";
        public const string JSON_GET_ACCEPT_HEADER = "application/json, text/javascript";
        public const string TEXT_GET_ACCEPT_HEADER = "text/html";
        public const string TEXT_JS_GET_ACCEPT_HEADER = "*/*;q=0.5, text/javascript, application/javascript, application/ecmascript, application/x-ecmascript";
        public const string PATH_TO_VERSION_DIR = "Update";
        public const string PATH_TO_VERSION_JSON = $"{PATH_TO_VERSION_DIR}\\version.json";
        public static DateTime ServerStartingTime { get; set; } = DateTime.Now;
        public static int Errors { get; set; }
    }
}

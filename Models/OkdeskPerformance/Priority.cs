using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskReport
{
    public class Priority
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int Position { get; set; }
        public bool Default { get; set; }
        public string? Color { get; set; }
    }
}

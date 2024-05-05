using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskReport
{
    public class IssueType
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool? Default { get; set; }
        public bool? Inner { get; set; }
        public bool? Available_for_client { get; set; }
        public string? Type { get; set; }
        public IssueType[]? Children { get; set; }
        

        public IssueType() { }

        public IssueType(IssueType taskType)
        {
            Id = taskType.Id;
            Name = taskType.Name;
            Code = taskType.Code;
            Default = taskType.Default;
            Inner = taskType.Inner;
            Available_for_client = taskType.Available_for_client;
            Type = taskType.Type;
            Children = taskType.Children;
        }
    }
}

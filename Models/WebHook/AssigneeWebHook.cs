using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Models.WebHook
{
    public class AssigneeWebHook
    {
        public Group? Group { get; set; }
        public Employee? Employee { get; set; }
    }
}

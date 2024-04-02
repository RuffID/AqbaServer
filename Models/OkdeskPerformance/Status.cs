using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskReport
{
    public class Status
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        private bool isChecked = false;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
            }
        }
    }
}

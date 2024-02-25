using AqbaServer.Models;

namespace AqbaServer.Dto
{
    public class KindDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? Visible { get; set; }
    }
}

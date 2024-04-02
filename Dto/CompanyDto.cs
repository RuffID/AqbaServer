using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Dto
{
    public class CompanyDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? AdditionalName { get; set; }

        public bool? Active { get; set; }

        public Category? Category { get; set; }
    }
}
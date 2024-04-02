namespace AqbaServer.Models.OkdeskPerformance
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Color { get; set; }

        public Category() { }

        public Category(int id, string color)
        {
            Id = id;
            Color = color;
        }
    }
}

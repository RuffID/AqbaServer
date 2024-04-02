using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategories();
        Task<Category?> GetCategory(int categoryId);
        Task<bool> GetCategoriesFromOkdesk();
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(int categoryId, Category category);
        Task<bool> DeleteCategory(int categoryId);
    }
}
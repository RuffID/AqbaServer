using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>?> GetCategories();
        Task<Category?> GetCategory(string? categoryCode);
        Task<Category?> GetCategory(int categoryId);
        Task<bool> UpdateCategoriesFromDBOkdesk();
        Task<bool> CreateCategory(Category? category);
        Task<bool> UpdateCategory(string? categoryCode, Category? category);
        Task<bool> DeleteCategory(string? categoryCode);
    }
}
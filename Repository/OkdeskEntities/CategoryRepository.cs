using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<ICollection<Category>?> GetCategories()
        {
            return await DBSelect.SelectCategories();
        }

        public async Task<Category?> GetCategory(string? categoryCode)
        {
            if (string.IsNullOrEmpty(categoryCode)) return null;

            return await DBSelect.SelectCategory(categoryCode);
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            return await DBSelect.SelectCategory(categoryId);
        }
        
        public async Task<bool> CreateCategory(Category? category)
        {
            if (category == null) return false;
            return await DBInsert.InsertCategory(category);
        }

        public async Task<bool> UpdateCategory(string? categoryCode, Category? category)
        {
            if (string.IsNullOrEmpty(categoryCode) || category == null) return false;
            return await DBUpdate.UpdateCategory(categoryCode, category);
        }

        public async Task<bool> UpdateCategoryWithoutColor(string? categoryCode, Category? category)
        {
            // Это обновление необходимо т.к. из API окдеска категории приходят с полем color, а из SQL API без него
            if (string.IsNullOrEmpty(categoryCode) || category == null) return false;
            return await DBUpdate.UpdateCategoryWithoutColor(categoryCode, category);
        }

        public async Task<bool> DeleteCategory(string? categoryCode)
        {
            if (categoryCode == null) return false;

            return await DBDelete.DeleteCategory(categoryCode);
        }

        public async Task<bool> UpdateCategoriesFromDBOkdesk()
        {
            var categories = await PGSelect.SelectCompanyCategories();

            if (categories == null) return false;

            // Создание категории с нулевым id которой нет в базе окдеска, но по которой ищутся клиенты без категории
            // Это нужно для первого запуска сервера
            Category no_category = new() { Id = 0, Name = "Без категории", Code = "no_category", Color = "#FFFFFF" };
            if (await GetCategory(no_category.Code) == null)
                if (!await CreateCategory(no_category))
                    return false;

            foreach (var category in categories)
            {
                var tempCat = await GetCategory(category.Code);
                category.Id = categories.ToList().IndexOf(category) + 1;

                if (tempCat == null)
                {
                    if (!await CreateCategory(category))
                        return false;
                }
                else if (tempCat != null)
                {
                    if (!await UpdateCategoryWithoutColor(tempCat.Code, category))
                        return false;
                }
            }
            return true;
        }
    }
}

using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<ICollection<Category>> GetCategories()
        {
            return await DBSelect.SelectCategories();
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
            return await DBSelect.SelectCategory(categoryId);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            return await DBInsert.InsertCategory(category);
        }

        public async Task<bool> UpdateCategory(int caregoryId, Category category)
        {
            return await DBUpdate.UpdateCategory(caregoryId, category);
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            return await DBDelete.DeleteCategory(categoryId);
        }

        public async Task<bool> GetCategoriesFromOkdesk()
        {
            //TODO сделать чтоб бралось из конфига а не из кода
            var categories = OkdeskEntitiesRequest.GetCategories();

            foreach (var category in categories)
            {
                var tempCat = await GetCategory(category.Id);

                if (tempCat == null)
                    if (!await CreateCategory(category))
                        return false;

                if (tempCat != null)
                    if (!await UpdateCategory(tempCat.Id, category))
                        return false;
            }
            return true;
        }
    }
}

using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IModelRepository
    {
        Task<bool> CreateModel(string kindCode, string manufacturerCode, Model model);
        Task<bool> DeleteModel(int id);
        Task<Model> GetModel(string modelCode);
        Task<ICollection<Model>> GetModels();
        Task<bool> GetModelsFromOkdesk();
        Task<bool> UpdateModel(string modelCode, Model model);
    }
}

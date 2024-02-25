using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class ModelRepository : IModelRepository
    {
        private readonly IKindRepository _kindRepository;
        private readonly IManufacturerRepository _manufacturerRepository;

        public ModelRepository(IKindRepository kindRepository, IManufacturerRepository manufacturerRepository)
        {
            _kindRepository = kindRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> CreateModel(string kindCode, string manufacturerCode, Model model)
        {
            var kind = await _kindRepository.GetKind(kindCode);
            var manufacturer = await _manufacturerRepository.GetManufacturer(manufacturerCode);

            model.EquipmentKind = kind;
            model.EquipmentManufacturer = manufacturer;

            return await DBInsert.InsertModel(model);
        }

        public async Task<bool> DeleteModel(int modelId)
        {
            return await DBDelete.DeleteModel(modelId);
        }

        public async Task<Model> GetModel(string modelCode)
        {
            return await DBSelect.SelectModel(modelCode);
        }

        public async Task<ICollection<Model>> GetModels()
        {
            return await DBSelect.SelectModels();
        }

        public async Task<bool> GetModelsFromOkdesk()
        {
            var models = await OkdeskEntitiesRequest.GetModels();

            if (models == null || models.Count <= 0) return false;

            foreach (var model in models)
            {
                var tempModel = await GetModel(model.Code);
                var tempManuf = await _manufacturerRepository.GetManufacturer(model?.EquipmentManufacturer?.Code);
                var tempKind = await _kindRepository.GetKind(model?.EquipmentKind?.Code);
                model.EquipmentManufacturer = tempManuf;
                model.EquipmentKind = tempKind;

                if (tempModel == null)
                {
                    if (!await CreateModel(tempKind.Code, tempManuf.Code, model))
                        return false;
                }
                else if (tempModel != null)
                {
                    if (!await UpdateModel(tempModel.Code, model))
                        return false;
                }
            }

            return true;
        }

        public async Task<bool> UpdateModel(string modelCode, Model model)
        {
            return await DBUpdate.UpdateModel(modelCode, model);
        }
    }
}

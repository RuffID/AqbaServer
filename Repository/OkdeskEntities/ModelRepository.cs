using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

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

        public async Task<bool> CreateModel(string? kindCode, string? manufacturerCode, Model? model)
        {
            if (model == null) return false;

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

        public async Task<Model?> GetModel(string? modelCode)
        {
            if (string.IsNullOrEmpty(modelCode)) return null;

            return await DBSelect.SelectModel(modelCode);
        }

        public async Task<ICollection<Model>?> GetModels()
        {
            return await DBSelect.SelectModels();
        }

        public async Task<bool> UpdatetModelsFromDBOkdesk()
        {
            var models = await PGSelect.SelectModels();

            return await SaveOrUpdateInDB(models);
        }

        public async Task<bool> GetModelsFromOkdesk()
        {
            var models = await OkdeskEntitiesRequest.GetModels();

            return await SaveOrUpdateInDB(models);
        }

        public async Task<bool> UpdateModel(string? modelCode, Model? model)
        {
            if (string.IsNullOrEmpty(modelCode) || model == null) return false;

            return await DBUpdate.UpdateModel(modelCode, model);
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<Model>? models)
        {
            if (models == null || models.Count <= 0) return false;

            foreach (var model in models)
            {
                var tempModel = await GetModel(model.Code);
                model.EquipmentManufacturer = await _manufacturerRepository.GetManufacturer(model.EquipmentManufacturer?.Code);
                model.EquipmentKind = await _kindRepository.GetKind(model.EquipmentKind?.Code);

                if (tempModel == null)
                {
                    if (!await CreateModel(model.Code, model.EquipmentManufacturer?.Code, model))
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
    }
}

using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly IKindRepository _kindRepository;
        private readonly IKindParameterRepository _kindParameterRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IEquipmentParameterRepository _equipmentParameterRepository;

        public EquipmentRepository(IKindRepository kindRepository, IManufacturerRepository manufacturerRepository, IModelRepository modelRepository, IEquipmentParameterRepository equipmentParameterRepository, IKindParameterRepository kindParameterRepository)
        {
            _kindRepository = kindRepository;
            _manufacturerRepository = manufacturerRepository;
            _modelRepository = modelRepository;
            _equipmentParameterRepository = equipmentParameterRepository;
            _kindParameterRepository = kindParameterRepository;
        }

        public async Task<bool> CreateEquipment(Equipment? equipment)
        {
            if (equipment == null) return false;

            var kind = await _kindRepository.GetKind(equipment.Equipment_kind?.Code);
            equipment.Equipment_kind = kind;
            var manufacturer = await _manufacturerRepository.GetManufacturer(equipment.Equipment_manufacturer?.Code);
            equipment.Equipment_manufacturer = manufacturer;
            var model = await _modelRepository.GetModel(equipment.Equipment_model?.Code);
            equipment.Equipment_model = model;

            return await DBInsert.InsertEquipment(equipment);
        }

        public async Task<bool> DeleteEquipment(Equipment? equipment)
        {
            if (equipment == null) return false;
            if (await DBDelete.DeleteEquipmentParameterByEquipment(equipment.Id))
                return await DBDelete.DeleteEquipment(equipment.Id);
            else return false;
        }

        public async Task<Equipment?> GetEquipment(int equipmentId)
        {
            return await DBSelect.SelectEquipment(equipmentId);
        }

        public async Task<int?> GetLastEquipmentId()
        {
            return await DBSelect.SelectLastEquipment();
        }

        public async Task<bool> UpdateEquipmentsFromAPIOkdesk(int equipmentId = 0, int pageSize = 100, int maintenanceEntityId = 0, int companyId = 0)
        {
            var equipments = await OkdeskEntitiesRequest.GetEquipments(equipmentId, pageSize, maintenanceEntityId, companyId);

            return await SaveOrUpdateInDB(equipments);
        }

        public async Task<bool> UpdateEquipmentsFromDBOkdesk(long lastEquipmentId = 0)
        {
            ICollection<Equipment>? equipments = [];
            
            while (true)
            {
                if (equipments.Count > 0)
                    lastEquipmentId = equipments.Last().Id;

                #if DEBUG
                await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateEquipmentsFromDBOkdesk)}] Last equipment ID: " + lastEquipmentId);
                #endif

                equipments = await PGSelect.SelectEquipments(lastEquipmentId);

                if (equipments == null || equipments.Count <= 0)
                    break;
                else await SaveOrUpdateInDB(equipments);

                if (equipments.Count < PGSelect.limitForEquipment)
                {
                    #if DEBUG
                    await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateEquipmentsFromDBOkdesk)} has been completed]");
                    #endif
                    break;
                }
            }
            return true;
        }        

        public async Task<ICollection<Equipment>?> GetEquipments(int equipmentId)
        {
            return await DBSelect.SelectEquipments(equipmentId);
        }

        public async Task<ICollection<Equipment>?> GetEquipments()
        {
            return await DBSelect.SelectEquipments();
        }

        public async Task<ICollection<Equipment>?> GetEquipmentsByMaintenanceEntity(int maintenanceEntityId)
        {
            return await DBSelect.SelectEquipmentsByMaintenanceEntity(maintenanceEntityId);
        }

        public async Task<ICollection<Equipment>?> GetEquipmentsByCompany(int companyId)
        {
            return await DBSelect.SelectEquipmentsByCompany(companyId);
        }

        public async Task<bool> UpdateEquipment(int equipmentId, Equipment? equipment)
        {
            if (equipment == null) return false;

            await CheckEquipment(equipment);
            
            return await DBUpdate.UpdateEquipment(equipmentId, equipment);            
        }
        
        async Task CheckEquipment(Equipment? equipment)
        {
            if (equipment == null) return;

            var kind = await _kindRepository.GetKind(equipment.Equipment_kind?.Code);
            equipment.Equipment_kind = kind;
            var manufacturer = await _manufacturerRepository.GetManufacturer(equipment.Equipment_manufacturer?.Code);
            equipment.Equipment_manufacturer = manufacturer;
            var model = await _modelRepository.GetModel(equipment.Equipment_model?.Code);
            equipment.Equipment_model = model;
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<Equipment>? equipments)
        {
            if (equipments != null && equipments.Count > 0)
            {
                foreach (var equipment in equipments)
                    await CheckEquipment(equipment);

                foreach (var equipment in equipments)
                {
                    var tempEquip = await GetEquipment(equipment.Id);
                    if (tempEquip == null)
                    {
                        if (!await CreateEquipment(equipment))
                            return false;
                    }
                    else if (tempEquip != null)
                    {
                        if (!await UpdateEquipment(tempEquip.Id, equipment))
                            return false;
                    }

                    if (equipment.Parameters != null && equipment.Parameters.Count > 0)
                    {
                        foreach (var param in equipment.Parameters)
                        {
                            // Если параметр пустой, то его нет смысла сохранять, пропуск итерации
                            if (string.IsNullOrEmpty(Convert.ToString(param.Value))) continue;

                            var tempKindParam = await _kindParameterRepository.GetKindParameter(param.Code);
                            if (tempKindParam == null) continue;

                            // Проверка есть ли уже в БД запись в таблице "parameter"
                            var tempEquipParam = await _equipmentParameterRepository.GetEquipmentParameter(equipment.Id, tempKindParam.Id);

                            param.Equipment = equipment;
                            param.KindParam = tempKindParam;

                            // Если его нет, то создание, иначе обновление
                            if (tempEquipParam == null)
                            {
                                if (!await _equipmentParameterRepository.CreateEquipmentParameter(param))
                                    continue;
                            }
                            else if (tempEquipParam != null)
                            {
                                if (!await _equipmentParameterRepository.UpdateEquipmentParameter(tempEquipParam.Id, param))
                                    continue;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
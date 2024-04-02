using AqbaServer.Data;
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

        public async Task<bool> CreateEquipment(Equipment equipment)
        {
            var kind = await _kindRepository.GetKind(equipment?.Equipment_kind?.Code);
            equipment.Equipment_kind = kind;
            var manufacturer = await _manufacturerRepository.GetManufacturer(equipment?.Equipment_manufacturer?.Code);
            equipment.Equipment_manufacturer = manufacturer;
            var model = await _modelRepository.GetModel(equipment?.Equipment_model?.Code);
            equipment.Equipment_model = model;

            return await DBInsert.InsertEquipment(equipment);
        }

        public async Task<bool> DeleteEquipment(Equipment equipment)
        {
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

        public async Task<bool> GetEquipmentsFromOkdesk(int equipmentId = 0, int pageSize = 100)
        {
            var equipments = await API.OkdeskEntitiesRequest.GetEquipments(equipmentId, pageSize);
            if (equipments == null || equipments.Count <= 0) return false;

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

                // После обновления основной информации об оборудовании идёт создание либо обновление параметров
                // Т.к. в API окдеска нет получения kind параметров, то приходится вытягивать его из оборудования
                // То есть при каждом создании либо обновлении оборудования проверяется есть ли параметр и в случае надобности добавляется

                if (equipment?.Parameters?.Count > 0)
                {
                    foreach (var param in equipment.Parameters)
                    {
                        // Сначала создание справочников, kind, kind param (связующая таблица) и непосредственно kind parameter
                        var kind = await _kindRepository.GetKind(equipment?.Equipment_kind?.Code);
                        if (kind == null) return false;
                        var tempKindParam = await _kindParameterRepository.GetKindParameter(param?.Code);

                        if (tempKindParam == null)
                        {
                            if (!await _kindParameterRepository.CreateKindParameter(kind.Id, param))
                                return false;
                        }
                        else if (tempKindParam != null)
                        {
                            if (!await _kindParameterRepository.UpdateKindParameter(kind.Id, tempKindParam?.Code, param))
                                return false;
                        }

                        // Повторная загрузка параметра для получения id в БД
                        tempKindParam = await _kindParameterRepository.GetKindParameter(param.Code);
                        // Проверка есть ли уже в БД equipment параметр
                        var tempEquipParam = await _equipmentParameterRepository.GetEquipmentParameter(equipment.Id, tempKindParam.Id);
                        param.Equipment = equipment;
                        param.KindParam = tempKindParam;

                        // Если его нет, то создание, иначе обновление
                        if (tempEquipParam == null)
                        {
                            if (!await _equipmentParameterRepository.CreateEquipmentParameter(param, equipment))
                                return false;
                        }
                        else if (tempEquipParam != null)
                        {
                            if (!await _equipmentParameterRepository.UpdateEquipmentParameter(tempEquipParam.Id, param))
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public async Task<ICollection<Equipment>> GetEquipments(int equipmentId)
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

        public async Task<bool> UpdateEquipment(int equipmentId, Equipment equipment)
        {
            var kind = await _kindRepository.GetKind(equipment?.Equipment_kind?.Code);
            equipment.Equipment_kind = kind;
            var manufacturer = await _manufacturerRepository.GetManufacturer(equipment?.Equipment_manufacturer?.Code);
            equipment.Equipment_manufacturer = manufacturer;
            var model = await _modelRepository.GetModel(equipment?.Equipment_model?.Code);
            equipment.Equipment_model = model;


            return await DBUpdate.UpdateEquipment(equipmentId, equipment);
        }
    }
}

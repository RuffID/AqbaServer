using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindRepository : IKindRepository
    {
        /*private readonly IKindParameterRepository _kindParameterRepository;
        private readonly IKindParamRepository _kindParamRepository;
        public KindRepository(IKindParameterRepository kindParameterRepository, IKindParamRepository kindParamRepository) 
        {
            _kindParameterRepository = kindParameterRepository;
            _kindParamRepository = kindParamRepository;
        }*/

        public async Task<bool> CreateKind(Kind? kind)
        {
            if (kind == null) return false;
            return await DBInsert.InsertKind(kind);
        }

        public async Task<bool> UpdateKindsFromAPIOkdesk()
        {
            var kinds = await OkdeskEntitiesRequest.GetKinds();
            if (kinds == null || kinds.Count <= 0) return false;

            return await SaveOrUpdateInDB(kinds);
        }

        public async Task<bool> UpdateKindsFromDBOkdesk()
        {
            var kinds = await PGSelect.SelectKinds();

            return await SaveOrUpdateInDB(kinds);
        }

        public async Task<bool> DeleteKind(int kindId)
        {
            // Сначала удаление связи в таблице много ко многим и если там связь удалилась, то после удаляется сам kind
            if (await DBDelete.DeleteKindParamByKind(kindId))
                return await DBDelete.DeleteKind(kindId);
            else return false;
        }

        public async Task<Kind?> GetKind(string? kindCode)
        {
            if (kindCode == null) return null;
            return await DBSelect.SelectKind(kindCode);
        }

        public async Task<ICollection<Kind>?> GetKinds()
        {
            return await DBSelect.SelectKinds();
        }

        public async Task<bool> UpdateKind(string? kindCode, Kind? kindMap)
        {
            if (string.IsNullOrEmpty(kindCode) || kindMap == null) return false;
            return await DBUpdate.UpdateKind(kindCode, kindMap);
        }        

        async Task<bool> SaveOrUpdateInDB(ICollection<Kind>? kinds)
        {
            if (kinds == null || kinds.Count <= 0)
            {
                WriteLog.Warn("null при получении issue types с окдеска");
                return false;
            }

            foreach (var kind in kinds)
            {
                var tempKind = await GetKind(kind.Code);

                if (tempKind == null)
                {
                    if (!await CreateKind(kind))
                        return false;
                }
                else if (tempKind != null)
                {
                    if (!await UpdateKind(kind.Name, kind))
                        return false;
                }

                /*// В данном цикле обновляется таблица kind param для связей между kind и kind_parameters
                if (kind.Parameters != null && kind.Parameters.Count > 0)
                {
                    foreach (var param in kind.Parameters)
                    {
                        // Получение id kind parameter, но есть ньюанс, если в базе два kind_parameters несколько параметров с одинаковым именем (name), то возникнут ошибки...
                        // Пока это не буду фиксить, надеюсь что такого не произойдёт,
                        // но вообще написал в окдеск чтобы они дали нормально парсить эти связи без подобных танцов с бубнами
                        var kindParameter = await _kindParameterRepository.GetKindParameterByName(param.Name);
                        if (kindParameter != null)
                        {
                            // Т.к. неизвестно обновился kind или был создан нельзя узнать точный id и поэтому запрашивает его ещё раз
                            var tempTempKind = await GetKind(kind.Code);
                            // Проверка на всякий случай
                            if (tempTempKind != null)
                            {
                                // Уточнение есть ли уже связь между kind и kind parameter в таблице kind_param
                                var kindParamConnect = await _kindParamRepository.GetKindParam(tempTempKind.Id, kindParameter.Id);

                                // Если связи нет, то создаёт
                                if (kindParamConnect == false)
                                    await _kindParamRepository.CreateKindParam(tempTempKind.Id, kindParameter.Id);
                            }
                        }

                    }
                }*/
            }
            return true;
        }
    }
}

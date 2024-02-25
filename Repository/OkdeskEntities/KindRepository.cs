using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindRepository : IKindRepository
    {
        public async Task<bool> CreateKind(Kind kind)
        {
            return await DBInsert.InsertKind(kind);
        }

        public async Task<bool> GetKindsFromOkdesk()
        {
            var kinds = await OkdeskEntitiesRequest.GetKinds();
            if (kinds == null || kinds.Count <= 0) return false;

            foreach (var kind in kinds)
            {
                var tempKind = await GetKind(kind?.Code);
                if (tempKind == null)
                {
                    if (!await CreateKind(kind))
                        return false;
                }
                else if (tempKind != null)
                {
                    if (!await UpdateKind(tempKind?.Code, kind))
                        return false;
                }
            }
            return true;
        }

        public async Task<bool> DeleteKind(int kindId)
        {
            // Сначала удаление связи в таблице много ко многим и если там связь удалилась, то после удаляется сам kind
            if (await DBDelete.DeleteKindParamByKind(kindId))
                return await DBDelete.DeleteKind(kindId);
            else return false;
        }

        public async Task<Kind> GetKind(string kindCode)
        {
            return await DBSelect.SelectKind(kindCode);
        }

        public async Task<ICollection<Kind>> GetKinds()
        {
            return await DBSelect.SelectKinds();
        }

        public async Task<bool> UpdateKind(string kindName, Kind kindMap)
        {
            return await DBUpdate.UpdateKind(kindName, kindMap);
        }
    }
}

using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IKindParameterRepository
    {
        Task<bool> CreateKindParameter(int kindId, EquipmentParameter param);
        //Task<bool> CreateKindParameters(Kind kind, int kindId);
        Task<bool> DeleteKindParameter(int id);
        Task<KindParameter> GetKindParameter(string kindParameterCode);
        Task<ICollection<KindParameter>> GetKindParameters();
        Task<ICollection<KindParameter>> GetKindParameters(int kindId);
        Task<bool> UpdateKindParameter(int kindId, string kindParameterCode, EquipmentParameter param);
    }
}
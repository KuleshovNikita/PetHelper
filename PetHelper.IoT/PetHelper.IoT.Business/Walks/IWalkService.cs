using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Walks
{
    public interface IWalkService
    {
        Task<ServiceResult<Empty>> SetWalkSettings(Guid petId);
    }
}

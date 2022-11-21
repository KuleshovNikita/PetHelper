using PetHelper.IoT.Domain.WalksModels;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Walks
{
    public interface IWalkService
    {
        Task<ServiceResult<Empty>> FinishWalk(Guid walkId);

        Task<ServiceResult<Guid>> StartWalk(WalkStartInfo walkStartInfo);
    }
}

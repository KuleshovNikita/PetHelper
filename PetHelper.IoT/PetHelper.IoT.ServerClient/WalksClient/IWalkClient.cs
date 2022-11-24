using PetHelper.IoT.Domain.WalksModels;
using PetHelper.IoT.ServerClient.Models;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.WalksClient
{
    public interface IWalkClient
    {
        Task<ServiceResult<Empty>> FinishWalk(Guid walkId);

        Task<ServiceResult<WalkModel>> StartWalk(WalkRequestModel walkRequestModel);
    }
}

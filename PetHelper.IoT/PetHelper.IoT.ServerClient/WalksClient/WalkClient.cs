using PetHelper.IoT.Domain.WalksModels;
using PetHelper.IoT.ServerClient.Models;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.WalksClient
{
    public class WalkClient : IWalkClient
    {
        private readonly ServerClient _client;

        public WalkClient(ServerClient client)
        {
            _client = client;
        }

        public async Task<ServiceResult<Empty>> FinishWalk(Guid walkId)
            => await _client.Patch($"/api/walk/{walkId}");

        public async Task<ServiceResult<WalkModel>> StartWalk(WalkRequestModel walkRequestModel)
            => await _client.Post<WalkRequestModel, WalkModel>("/api/walk", walkRequestModel);
    }
}

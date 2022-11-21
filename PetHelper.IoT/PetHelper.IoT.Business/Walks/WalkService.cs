using AutoMapper;
using PetHelper.IoT.Business.Streaming;
using PetHelper.IoT.DeviceController.Controllers.Storage;
using PetHelper.IoT.Domain.WalksModels;
using PetHelper.IoT.ServerClient.Models;
using PetHelper.IoT.ServerClient.PetsClient;
using PetHelper.IoT.ServerClient.WalksClient;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Walks
{
    public class WalkService : IWalkService
    {
        private readonly IPetClient _petClient;
        private readonly IWalkClient _walkClient;
        private readonly IMapper _mapper;
        private readonly IStorageController _storageController;
        private readonly IPositionStream _positionStream;

        public WalkService(IPetClient petClient, IMapper mapper, 
            IStorageController storageController, IPositionStream positionStream, IWalkClient walkClient)
        {
            _petClient = petClient;
            _mapper = mapper;
            _storageController = storageController;
            _positionStream = positionStream;
            _walkClient = walkClient;
        }

        public async Task<ServiceResult<Empty>> FinishWalk(Guid walkId)
        {
            var res = await _walkClient.FinishWalk(walkId);
            return res.CatchAny();
        }

        public async Task<ServiceResult<Guid>> StartWalk(WalkStartInfo walkStartInfo)
        {
            var serviceResult = new ServiceResult<Guid>();

            var petModel = await _petClient.GetPet(walkStartInfo.PetId);
            petModel.CatchAny(petModel.ClientErrorMessage);

            var walkResult = await _walkClient.StartWalk(new WalkRequestModel
            {
                PetId = walkStartInfo.PetId,
                ScheduleId = walkStartInfo.ScheduleId
            });
            walkResult.CatchAny();

            var walkOptions = _mapper.Map<WalkOptions>(petModel.Value);
            walkStartInfo.WalkOptions = walkOptions;

            _storageController.SaveWalkSettings(walkStartInfo);
            _positionStream.BeginPositionStreaming();

            serviceResult.Value = walkResult.Value.Id;
            return serviceResult.Success();
        }
    }
}

using AutoMapper;
using PetHelper.IoT.Business.Streaming;
using PetHelper.IoT.DeviceController.Controllers.Storage;
using PetHelper.IoT.Domain.WalksModels;
using PetHelper.IoT.ServerClient.PetsClient;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Walks
{
    public class WalkService : IWalkService
    {
        private readonly IPetClient _petClient;
        private readonly IMapper _mapper;
        private readonly IStorageController _storageController;
        private readonly IPositionStream _positionStream;

        public WalkService(IPetClient petClient, IMapper mapper, 
            IStorageController storageController, IPositionStream positionStream)
        {
            _petClient = petClient;
            _mapper = mapper;
            _storageController = storageController;
            _positionStream = positionStream;
        }

        public async Task<ServiceResult<Empty>> SetWalkSettings(WalkStartInfo walkStartInfo)
        {
            var serviceResult = new ServiceResult<Empty>();

            var petModel = await _petClient.GetPet(walkStartInfo.PetId);
            var walkOptions = _mapper.Map<WalkOptions>(petModel);
            walkStartInfo.WalkOptions = walkOptions;

            _storageController.SaveWalkSettings(walkStartInfo);
            _positionStream.BeginPositionStreaming();

            return serviceResult.Success();
        }
    }
}

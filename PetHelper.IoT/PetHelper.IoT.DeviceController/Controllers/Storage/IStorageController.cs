using PetHelper.IoT.Domain.WalksModels;

namespace PetHelper.IoT.DeviceController.Controllers.Storage
{
    public interface IStorageController
    {
        bool SaveWalkSettings(WalkStartInfo options);

        WalkStartInfo GetWalkOptions();
    }
}

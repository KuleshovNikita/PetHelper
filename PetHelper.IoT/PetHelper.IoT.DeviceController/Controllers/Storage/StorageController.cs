using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.WalksModels;
using System.Text.Json;

namespace PetHelper.IoT.DeviceController.Controllers.Storage
{
    public class StorageController : IStorageController
    {
        private readonly string _storagePath;

        public StorageController(IConfiguration config)
        {
            _storagePath = config.GetSection("StoragePath").Value!;
            _storagePath = $"{_storagePath}\\WalkConfiguration.txt";
        }

        public bool SaveWalkSettings(WalkStartInfo options)
        {
            try
            {
                File.WriteAllText(_storagePath, JsonSerializer.Serialize(options));
            }
            catch
            {
                return false;
            }

            return true;
        }

        public WalkStartInfo GetWalkOptions()
        {
            try
            {
                var data = File.ReadAllText(_storagePath);
                return JsonSerializer.Deserialize<WalkStartInfo>(data)!;
            }
            catch
            {
                return null!;
            }
        }
    }
}

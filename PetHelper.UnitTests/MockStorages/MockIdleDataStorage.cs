using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic;

namespace PetHelper.UnitTests.MockStorages
{
    internal class MockIdleDataStorage
    {
        public static IEnumerable<IdlePetStatisticModel> Data { get; set; } = new List<IdlePetStatisticModel>
        {
            new IdlePetStatisticModel
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000001"),
                AnimalType = AnimalType.Dog,
                Breed = "Taxa",
                IdleWalkDuringTime = 15,
                IdleWalksCountPerDay = 3,
                IsUnifiedAnimalData = false
            },
            new IdlePetStatisticModel
            {
                Id = Guid.Parse("50000000-0000-0000-0000-000000000002"),
                AnimalType = AnimalType.Dog,
                Breed = null!,
                IdleWalkDuringTime = 15,
                IdleWalksCountPerDay = 4,
                IsUnifiedAnimalData = true
            }
        };
    }
}

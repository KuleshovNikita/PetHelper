using PetHelper.Domain.Pets;

namespace PetHelper.UnitTests.MockStorages
{
    public static class PetMockStorage
    {
        public static IEnumerable<PetModel> Data { get; set; } = new List<PetModel>
        {
            new PetModel
            {
                Id = _petIds![1],
                Name = "Rex",
                AnimalType = null!,
                Breed = null!,
                Owner = null!,
                OwnerId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                WalkingSchedule = null!,
            },
            new PetModel
            {
                Id = _petIds![2],
                Name = "Bim",
                AnimalType = AnimalType.Dog,
                Breed = "Taxa",
                Owner = null!,
                OwnerId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                WalkingSchedule = null!,
            },
            new PetModel
            {
                Id = _petIds![3],
                Name = "Jojo",
                AnimalType = AnimalType.Dog,
                Breed = null,
                Owner = null!,
                OwnerId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                WalkingSchedule = null!,
            }
        };

        private static IDictionary<int, Guid> _petIds => new Dictionary<int, Guid>
        {
            [1] = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            [2] = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            [3] = Guid.Parse("20000000-0000-0000-0000-000000000003"),
        };
    }
}

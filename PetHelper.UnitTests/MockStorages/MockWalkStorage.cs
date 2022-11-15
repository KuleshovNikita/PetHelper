using PetHelper.Domain.Pets;

namespace PetHelper.UnitTests.MockStorages
{
    public static class MockWalkStorage
    {
        public static IEnumerable<WalkModel> Data { get; set; } = new List<WalkModel>
        {
            new WalkModel 
            { 
                Id = Guid.Parse("40000000-0000-0000-0000-000000000001"),
                StartTime = new DateTime(2022, 11, 1, 10, 10, 0), 
                EndTime = new DateTime(2022, 11, 1, 10, 25, 0),
                PetId = _petIds![1],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![1]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                Schedule = null!
            },
            new WalkModel
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000002"),
                StartTime = new DateTime(2022, 11, 1, 12, 15, 0),
                EndTime = new DateTime(2022, 11, 1, 12, 25, 0),
                PetId = _petIds![1],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![1]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                Schedule = null!
            },
            new WalkModel
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000003"),
                StartTime = new DateTime(2022, 11, 1, 17, 0, 0),
                EndTime = new DateTime(2022, 11, 1, 17, 7, 0),
                PetId = _petIds![2],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![2]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                Schedule = null!
            },
            new WalkModel
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000004"),
                StartTime = new DateTime(2022, 11, 1, 10, 17, 0),
                EndTime = new DateTime(2022, 11, 1, 10, 43, 0),
                PetId = _petIds![2],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![2]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
                Schedule = null!
            },
            new WalkModel
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000005"),
                StartTime = new DateTime(2022, 11, 1, 10, 0, 0),
                EndTime = new DateTime(2022, 11, 1, 10, 7, 0),
                PetId = _petIds![3],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![3]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000005"),
                Schedule = null!
            },
            new WalkModel
            {
                Id = Guid.Parse("40000000-0000-0000-0000-000000000006"),
                StartTime = new DateTime(2022, 11, 1, 10, 17, 0),
                EndTime = new DateTime(2022, 11, 1, 10, 43, 0),
                PetId = _petIds![3],
                Pet = PetMockStorage.Data.First(x => x.Id == _petIds![3]),
                ScheduleId = Guid.Parse("30000000-0000-0000-0000-000000000006"),
                Schedule = null!
            }
        };

        private static IDictionary<int, Guid> _petIds => new Dictionary<int, Guid>()
        {
            [1] = Guid.Parse("20000000-0000-0000-0000-000000000001"),
            [2] = Guid.Parse("20000000-0000-0000-0000-000000000002"),
            [3] = Guid.Parse("20000000-0000-0000-0000-000000000003") 
        };
    }
}

namespace PetHelper.Domain.Pets
{
    public record PetModel : BaseModel
    {
        public string Name { get; set; }

        public AnimalType? AnimalType { get; set; }

        public string? Breed { get; set; }

        public IEnumerable<ScheduleModel> WalkingSchedule { get; set; }

        public Guid OwnerId { get; set; }

        public UserModel Owner { get; set; }
    }
}

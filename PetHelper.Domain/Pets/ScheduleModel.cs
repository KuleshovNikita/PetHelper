namespace PetHelper.Domain.Pets
{
    public record ScheduleModel : BaseModel
    {
        public TimeSpan ScheduledStart { get; set; }

        public TimeSpan ScheduledEnd { get; set; }

        public Guid PetId { get; set; }

        public PetModel Pet { get; set; }
    }
}

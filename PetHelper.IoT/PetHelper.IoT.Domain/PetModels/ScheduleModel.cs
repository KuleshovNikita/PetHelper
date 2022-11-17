namespace PetHelper.IoT.Domain.PetModels
{
    public record ScheduleModel : BaseModel
    {
        public DateTime ScheduledStart { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public Guid PetId { get; set; }

        public PetModel Pet { get; set; } = null!;
    }
}

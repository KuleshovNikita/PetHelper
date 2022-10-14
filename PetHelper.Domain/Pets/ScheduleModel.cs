namespace PetHelper.Domain.Pets
{
    public record ScheduleModel : BaseModel
    {
        public DateTime ScheduledTime { get; set; }

        public Guid PetId { get; set; }

        public PetModel Pet { get; set; }
    }
}

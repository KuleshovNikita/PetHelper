namespace PetHelper.Domain.Pets
{
    public record WalkModel : BaseModel
    {
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public Guid ScheduleId { get; set; }

        public ScheduleModel Schedule { get; set; } = null!;

        public Guid PetId { get; set; }

        public PetModel Pet { get; set; } = null!;
    }
}

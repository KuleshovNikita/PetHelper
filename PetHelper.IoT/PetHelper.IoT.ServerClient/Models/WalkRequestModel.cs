namespace PetHelper.IoT.ServerClient.Models
{
    public record WalkRequestModel
    {
        public Guid ScheduleId { get; set; }

        public Guid PetId { get; set; }
    }
}

namespace PetHelper.Api.Models.RequestModels.Walks
{
    public record WalkRequestModel
    {
        public Guid ScheduleId { get; set; }

        public Guid PetId { get; set; }
    }
}

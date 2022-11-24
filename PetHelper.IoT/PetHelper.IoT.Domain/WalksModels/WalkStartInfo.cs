namespace PetHelper.IoT.Domain.WalksModels
{
    public record WalkStartInfo
    {
        public Guid PetId { get; set; }

        public Guid ScheduleId { get; set; }

        public Position OwnerPosition { get; set; } = null!;

        public WalkOptions WalkOptions { get; set; } = null!;
    }
}

namespace PetHelper.Api.Models.RequestModels.Statistic
{
    public record StatisticRequestModel
    {
        public Guid PetId { get; set; }

        public DateTime SampleStartDate { get; set; }

        public DateTime SampleEndDate { get; set; }
    }
}

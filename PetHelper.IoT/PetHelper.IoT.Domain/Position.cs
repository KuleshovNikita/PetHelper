namespace PetHelper.IoT.Domain
{
    public record Position
    {
        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}

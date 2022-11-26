using PetHelper.Domain.Pets;

namespace PetHelper.Api.Models.RequestModels.Pets
{
    public record PetUpdateRequestModel
    {
        public string? Name { get; set; }

        public AnimalType? AnimalType { get; set; }

        public string? Breed { get; set; }

        public double AllowedDistance { get; set; }
    }
}

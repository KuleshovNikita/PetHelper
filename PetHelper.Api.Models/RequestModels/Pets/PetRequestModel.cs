using PetHelper.Domain.Pets;

namespace PetHelper.Api.Models.RequestModels.Pets
{
    public record PetRequestModel
    {
        public string Name { get; set; }

        public AnimalType? AnimalType { get; set; }

        public string? Breed { get; set; }
    }
}

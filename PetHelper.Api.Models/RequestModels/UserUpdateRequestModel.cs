using PetHelper.Domain;

namespace PetHelper.Api.Models.RequestModels
{
    public record UserUpdateRequestModel
    {
        public Guid? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public string? Password { get; set; }

        public string? Login { get; set; }
    }
}
